let io;
function initObserverElements() {
	let rootObserverView = document.getElementById('main-body');
	io = new IntersectionObserver(entries => {
		entries.forEach(elementInView => {
			if (elementInView.isIntersecting) {
				elementInView.target.classList.add('animate');
				/*io.unobserve(elementInView.target);*/
			}
		});
	}, {
		threshold: [0.1],
		root: rootObserverView
	});

	var elements = document.querySelectorAll('[data-observer-animate]');
	if (elements) {
		elements.forEach(el => {
			io.observe(el);
		});
	}
}

export function onLoad() {
	initObserverElements();
}

export function onUpdate() {
	
}

export function onDispose() {
	io.disconnect();
}