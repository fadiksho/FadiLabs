const pageScriptInfoBySrc = new Map();

function registerPageScriptElement(src) {
  if (!src) {
    throw new Error('Must provide a non-empty value for the "src" attribute.');
  }

  let pageScriptInfo = pageScriptInfoBySrc.get(src);

  if (pageScriptInfo) {
    pageScriptInfo.referenceCount++;
  } else {
    pageScriptInfo = { referenceCount: 1, module: null };
    pageScriptInfoBySrc.set(src, pageScriptInfo);
    initializePageScriptModule(src, pageScriptInfo);
  }
}

function unregisterPageScriptElement(src) {
  if (!src) {
    return;
  }

  const pageScriptInfo = pageScriptInfoBySrc.get(src);

  if (!pageScriptInfo) {
    return;
  }

  pageScriptInfo.referenceCount--;
}


async function initializePageScriptModule(src, pageScriptInfo) {
  if (src.startsWith("./")) {
    src = new URL(src.substring(2), document.baseURI).toString();
  }

  const module = await import(src);

  if (pageScriptInfo.referenceCount <= 0) {
    return;
  }

  pageScriptInfo.module = module;
  module.onLoad?.();
  module.onUpdate?.();
}

async function initialzeMauiPageScriptCustomElement(blazor) {
  customElements.define('maui-page-script', class extends HTMLElement {
    
    async connectedCallback() {
      let src = this.getAttribute('src');
      if (!src) {
        throw new Error('Must provide a non-empty value for the "src" attribute.');
      }

      if (src.startsWith("./")) {
        src = new URL(src.substring(2), document.baseURI).toString();
      }

      this.module = await import(src);
      this.module?.onLoad?.();
    }

    disconnectedCallback() {
      this.module?.onDispose?.();
    }
  });
}

function initialzePageScriptCustomElement(blazor) {
  customElements.define('page-script', class extends HTMLElement {
    static observedAttributes = ['src'];

    attributeChangedCallback(name, oldValue, newValue) {
      if (name !== 'src') {
        return;
      }

      this.src = newValue;
      unregisterPageScriptElement(oldValue);
      registerPageScriptElement(newValue);
    }

    disconnectedCallback() {
      unregisterPageScriptElement(this.src);
    }
  });
}

function onEnhancedLoad() {
  for (const [src, { module, referenceCount }] of pageScriptInfoBySrc) {
    if (referenceCount <= 0) {
      module?.onDispose?.();
      pageScriptInfoBySrc.delete(src);
    }
  }

  for (const { module } of pageScriptInfoBySrc.values()) {
    module?.onUpdate?.();
  }
}

export function afterWebStarted(blazor) {
  initialzePageScriptCustomElement(blazor);
    
  blazor.addEventListener('enhancedload', onEnhancedLoad);
}

export function afterStarted(blazor) {
  initialzeMauiPageScriptCustomElement(blazor);
}