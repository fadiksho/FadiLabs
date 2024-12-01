const rootObserverView = document.getElementById('main-body');
const io = new IntersectionObserver(entries => {
	entries.forEach(elementInView => {
		if (elementInView.isIntersecting) {
			elementInView.target.classList.add('animate');
			io.unobserve(elementInView.target);
		}
	});
}, {
	threshold: [0.1],
	root: rootObserverView
});

function initObserverElements() {
	var elements = document.querySelectorAll('[data-observer-animate]');
	if (elements) {
		elements.forEach(el => {
			io.observe(el);
		});
	}
}

function desposeObserverElements() {
	io.disconnect();
}

export function onLoad() {
	initObserverElements();
}

export function onUpdate() {
	
}

export function onDispose() {
	desposeObserverElements();
}