class ThemeController {
	themeToggle;

	constructor(themeToggle) {
		this.themeToggle = themeToggle;
	}

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

	startListener() {
		this.themeToggle.addEventListener('change', this.toggleThemeState);
	}
	stopListener() {
		this.themeToggle.removeEventListener('change', this.toggleThemeState);
	}
}
let themeToggle =
	new ThemeController(document.getElementById('themeCheckBoxId'));
export function onLoad() {
	themeToggle.startListener();
	themeToggle.render();
}

export function onUpdate() {
	themeToggle.render();
}

export function onDispose() {
	themeToggle.stopListener();
}

onLoad();