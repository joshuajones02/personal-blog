// ============================================================
// FABLE — site.js
// Dependency-free progressive enhancement:
//   1. Mobile navigation toggle (ARIA-driven)
// Galleries use pure CSS scroll-snap; no JS needed.
// ============================================================
(function () {
    'use strict';

    var toggle = document.querySelector('[data-nav-toggle]');
    var nav = document.querySelector('[data-site-nav]');

    if (toggle && nav) {
        toggle.addEventListener('click', function () {
            var open = nav.classList.toggle('is-open');
            toggle.setAttribute('aria-expanded', open ? 'true' : 'false');
        });

        // Close menu with Escape and return focus to the toggle
        document.addEventListener('keydown', function (e) {
            if (e.key === 'Escape' && nav.classList.contains('is-open')) {
                nav.classList.remove('is-open');
                toggle.setAttribute('aria-expanded', 'false');
                toggle.focus();
            }
        });
    }
})();
