// wwwroot/js/login.js

// Simulation du chargement
window.addEventListener('load', function () {
    setTimeout(function () {
        const loadingScreen = document.getElementById('loadingScreen');
        if (loadingScreen) {
            loadingScreen.classList.add('hidden');
        }
    }, 2000);
});

// Animation des formes de décoration
document.addEventListener('DOMContentLoaded', function () {
    const shapes = document.querySelectorAll('.shape');
    shapes.forEach((shape, index) => {
        shape.style.animationDelay = `${index * 0.5}s`;
    });
});

// Effet de parallaxe léger sur la souris
document.addEventListener('mousemove', function (e) {
    const shapes = document.querySelectorAll('.decoration');
    if (shapes.length === 0) return;

    const mouseX = e.clientX / window.innerWidth;
    const mouseY = e.clientY / window.innerHeight;

    shapes.forEach((shape, index) => {
        const speed = (index + 1) * 0.02;
        const x = mouseX * speed * 50;
        const y = mouseY * speed * 50;
        shape.style.transform = `translate(${x}px, ${y}px)`;
    });
});

// Amélioration de l'expérience utilisateur lors de la soumission
document.addEventListener('DOMContentLoaded', function () {
    const form = document.querySelector('form');
    if (form) {
        form.addEventListener('submit', function (e) {
            const submitBtn = document.querySelector('.btn-primary');
            if (submitBtn) {
                submitBtn.textContent = 'Connexion en cours...';
                submitBtn.disabled = true;
            }
        });
    }
});