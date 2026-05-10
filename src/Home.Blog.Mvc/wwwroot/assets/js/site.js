/* Site shell — navigation toggle, scroll state, and gallery bootstrap.
 * No dependencies. Safe to load with `defer`.
 */
(function () {
    'use strict';

    // ── Sticky nav: toggle .is-scrolled on scroll (rAF-throttled) ──
    var nav = document.getElementById('site-nav');
    if (nav) {
        var scrolled = false;
        var ticking  = false;
        var threshold = 24;

        var update = function () {
            var next = window.scrollY > threshold;
            if (next !== scrolled) {
                scrolled = next;
                nav.classList.toggle('is-scrolled', scrolled);
            }
            ticking = false;
        };

        window.addEventListener('scroll', function () {
            if (!ticking) {
                window.requestAnimationFrame(update);
                ticking = true;
            }
        }, { passive: true });

        update();
    }

    // ── Mobile nav toggle ──────────────────────────────────────────
    var toggle = document.querySelector('[data-nav-toggle]');
    if (toggle && nav) {
        var menu = document.getElementById(toggle.getAttribute('aria-controls'));

        var setOpen = function (open) {
            nav.setAttribute('data-open', open ? 'true' : 'false');
            toggle.setAttribute('aria-expanded', open ? 'true' : 'false');
        };

        toggle.addEventListener('click', function () {
            setOpen(toggle.getAttribute('aria-expanded') !== 'true');
        });

        // Close on Escape
        document.addEventListener('keydown', function (e) {
            if (e.key === 'Escape' && toggle.getAttribute('aria-expanded') === 'true') {
                setOpen(false);
                toggle.focus();
            }
        });

        // Close when a menu link is clicked (mobile UX)
        if (menu) {
            menu.addEventListener('click', function (e) {
                if (e.target.closest('a') && window.matchMedia('(max-width: 768px)').matches) {
                    setOpen(false);
                }
            });
        }
    }

    // ── Galleries ──────────────────────────────────────────────────
    var galleries = document.querySelectorAll('[data-gallery]');
    galleries.forEach(initGallery);

    function initGallery(root) {
        var slides     = Array.prototype.slice.call(root.querySelectorAll('.gallery__slide'));
        var indicators = Array.prototype.slice.call(root.querySelectorAll('[data-gallery-goto]'));
        var prev       = root.querySelector('[data-gallery-prev]');
        var next       = root.querySelector('[data-gallery-next]');

        if (slides.length <= 1) { return; }

        var index = 0;

        var go = function (n) {
            index = (n + slides.length) % slides.length;
            slides.forEach(function (s, i) {
                var active = i === index;
                s.setAttribute('data-active', active ? 'true' : 'false');
                if (active) { s.removeAttribute('aria-hidden'); }
                else        { s.setAttribute('aria-hidden', 'true'); }
            });
            indicators.forEach(function (b, i) {
                b.setAttribute('aria-current', i === index ? 'true' : 'false');
            });
        };

        if (prev) { prev.addEventListener('click', function () { go(index - 1); }); }
        if (next) { next.addEventListener('click', function () { go(index + 1); }); }

        indicators.forEach(function (b) {
            b.addEventListener('click', function () {
                go(parseInt(b.getAttribute('data-gallery-goto'), 10) || 0);
            });
        });

        // Keyboard arrows when gallery has focus
        root.setAttribute('tabindex', '0');
        root.addEventListener('keydown', function (e) {
            if (e.key === 'ArrowLeft')  { e.preventDefault(); go(index - 1); }
            if (e.key === 'ArrowRight') { e.preventDefault(); go(index + 1); }
        });
    }
})();
