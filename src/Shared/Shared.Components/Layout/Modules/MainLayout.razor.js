class NavMenu {
	navMenuToggle;
	navMenuModeToggle;
	navMenuOverlay;
	minimumWidth = 600;

	constructor() {
	}

	getNavMenuState = () => {
		let sideMenu = localStorage.getItem("sideMenu");
		if (sideMenu) {
			return sideMenu;
		}

		return window.innerWidth > this.minimumWidth ? 'open' : 'close';
	}

	bindNavMenuState = () => {
		let state = this.getNavMenuState();

		let navMenuContainerId = document.getElementById('navMenuContainerId');
		let sideMenuToggleLabelId = document.getElementById('sideMenuToggleLabelId');

		if (state === 'open') {
			this.navMenuToggle.checked = true;
			navMenuContainerId.style.marginLeft = '0px';
			sideMenuToggleLabelId.classList.add('swap-active');
		} else {
			this.navMenuToggle.checked = false;
			sideMenuToggleLabelId.classList.remove('swap-active');
			navMenuContainerId.style.marginLeft = '';
		}
	}

	toggleNavMenu = () => {
		let newMenuState = this.getNavMenuState() === 'open' ? 'close' : 'open';
		localStorage.setItem('sideMenu', newMenuState);

		this.render();
	}

	getNavMenuModeState = () => {
		let menuSideMode = localStorage.getItem("menuSideMode");
		if (menuSideMode) {
			return menuSideMode;
		}

		return window.innerWidth > this.minimumWidth ? 'push' : 'overlay';
	}

	bindNavMenuModeState = () => {
		let state = this.getNavMenuModeState();
		let navMenuContainerId = document.getElementById('navMenuContainerId');
		if (state === 'push') {
			this.navMenuModeToggle.checked = true;
			navMenuContainerId.style.position = 'relative';
		} else {
			this.navMenuModeToggle.checked = false;
			navMenuContainerId.style.position = '';
		}
	}

	toggleNavMenuMode = () => {
		let newMenuSideMode = this.getNavMenuModeState() === 'push' ? 'overlay' : 'push';
		localStorage.setItem("menuSideMode", newMenuSideMode);

		this.render();
	}

	bindNavMenuOverlay = () => {
		let navMenuState = this.getNavMenuState();
		let navMenuModeState = this.getNavMenuModeState();

		if (navMenuState === 'open') {
			this.navMenuOverlay.classList.add('animate-fade');
			this.navMenuOverlay.classList.add('animate-duration-150');
			if (navMenuModeState === 'push') {
				this.navMenuOverlay.classList.add('hidden');
			}
			else {
				this.navMenuOverlay.classList.remove('hidden');
			}
		} else {
			this.navMenuOverlay.classList.add('hidden');
			this.navMenuOverlay.classList.remove('animate-fade');
			this.navMenuOverlay.classList.remove('animate-duration-150');
		}
	}

	handleWindowResize = () => {
		let menuSideMode = this.getNavMenuModeState();
		let sideMenu = this.getNavMenuState();

		if (window.innerWidth < this.minimumWidth && menuSideMode === 'push') {
			this.toggleNavMenuMode();

			if (sideMenu === 'open') {
				this.toggleNavMenu();
			}
		}
		else if (window.innerWidth >= this.minimumWidth && menuSideMode === 'overlay') {
			this.toggleNavMenuMode();

			if (sideMenu === 'close') {
				this.toggleNavMenu();
			}
		}
	}

	autoCloseOnOverlayMode = () => {
		let navMenuState = this.getNavMenuState();
		let navMenuModeState = this.getNavMenuModeState();

		if (navMenuState === 'open' && navMenuModeState === 'overlay') {
			this.toggleNavMenu();
		}
	}

	render = () => {
		this.bindNavMenuState();
		this.bindNavMenuModeState();
		this.bindNavMenuOverlay();
	}

	initListeners = () => {
		this.navMenuToggle.addEventListener('change', this.toggleNavMenu);
		this.navMenuModeToggle.addEventListener('change', this.toggleNavMenuMode);
		window.addEventListener('resize', this.handleWindowResize);
	}

	removeListeners = () => {
		this.navMenuToggle.removeEventListener('change', this.toggleNavMenu);
		this.navMenuModeToggle.removeEventListener('change', this.toggleNavMenuMode);
		window.removeEventListener('resize', this.handleWindowResize);
	}
}

class ThemeController {
	themeToggle;

	getThemeState = () => {
		let theme = localStorage.getItem("theme");
		if (theme) {
			return theme == 'light' ? 'light' : 'dark';
		}

		return window.matchMedia("(prefers-color-scheme: dark)").matches ? 'dark' : 'light';
	}

	toggleThemeState = () => {
		let newTheme = this.getThemeState() == 'dark' ? 'light' : 'dark';
		localStorage.setItem("theme", newTheme);
		this.render();
	}

	render = () => {
		let theme = this.getThemeState();
		this.themeToggle.checked = theme == 'light' ? true : false;
		this.themeToggle.value = theme;
	}

	initListeners = () => {
		this.themeToggle.addEventListener('change', this.toggleThemeState);
	}
	removeListeners = () => {
		this.themeToggle.removeEventListener('change', this.toggleThemeState);
	}
}

let navMenu;
let themeController;
export function onLoad() {
	navMenu = new NavMenu();
	
	navMenu.navMenuToggle = document.getElementById('sideMenuToggleId');
	navMenu.navMenuModeToggle = document.getElementById('expandNavSideMenuId');
	navMenu.navMenuOverlay = document.getElementById('navSideMenuOverlayId');
	navMenu.initListeners();

	themeController = new ThemeController();
	themeController.themeToggle = document.getElementById('themeCheckBoxId');
	themeController.initListeners();

	navMenu.render();
	themeController.render();
}

export function onUpdate() {
	navMenu.autoCloseOnOverlayMode();
	navMenu.render();

	themeController.render();
}

export function onDispose() {
	navMenu.removeListeners();
	themeController.removeListeners();
}
