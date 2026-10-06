/* HORIZON — site behaviour. Vanilla JS, no dependencies. */
(function () {
    "use strict";

    var motionOK = !window.matchMedia("(prefers-reduced-motion: reduce)").matches;

    /* ── Header: shadow once scrolled ─────────────────── */
    var header = document.getElementById("site-header");

    if (header) {
        var onScroll = function () {
            header.classList.toggle("scrolled", window.scrollY > 8);
        };
        window.addEventListener("scroll", onScroll, { passive: true });
        onScroll();
    }

    /* ── Mobile navigation ────────────────────────────── */
    var toggle = document.querySelector(".nav-toggle");
    var nav = document.getElementById("site-nav");

    if (header && toggle && nav) {
        var setOpen = function (open) {
            toggle.setAttribute("aria-expanded", open ? "true" : "false");
            header.classList.toggle("menu-open", open);
        };
        var isOpen = function () {
            return toggle.getAttribute("aria-expanded") === "true";
        };

        toggle.addEventListener("click", function () {
            setOpen(!isOpen());
        });

        document.addEventListener("keydown", function (e) {
            if (e.key === "Escape" && isOpen()) {
                setOpen(false);
                toggle.focus();
            }
        });

        document.addEventListener("click", function (e) {
            if (isOpen() && !header.contains(e.target)) {
                setOpen(false);
            }
        });

        nav.addEventListener("click", function (e) {
            if (e.target.closest("a")) {
                setOpen(false);
            }
        });

        var desktop = window.matchMedia("(min-width: 48em)");
        var onBreakpoint = function (e) {
            if (e.matches) {
                setOpen(false);
            }
        };
        if (desktop.addEventListener) {
            desktop.addEventListener("change", onBreakpoint);
        }
    }

    /* ── Image galleries: scroll-snap + buttons ───────── */
    document.querySelectorAll(".gallery").forEach(function (gallery) {
        var track = gallery.querySelector(".gallery__track");
        var prev = gallery.querySelector(".gallery__control--prev");
        var next = gallery.querySelector(".gallery__control--next");

        if (!track || !prev || !next) {
            return;
        }

        var update = function () {
            var overflow = track.scrollWidth - track.clientWidth;
            var hasOverflow = overflow > 4;

            prev.hidden = !hasOverflow;
            next.hidden = !hasOverflow;

            if (hasOverflow) {
                prev.disabled = track.scrollLeft <= 4;
                next.disabled = track.scrollLeft >= overflow - 4;
            }
        };

        var scheduled = false;
        var scheduleUpdate = function () {
            if (scheduled) {
                return;
            }
            scheduled = true;
            window.requestAnimationFrame(function () {
                scheduled = false;
                update();
            });
        };

        var scrollByAmount = function (direction) {
            track.scrollBy({
                left: direction * track.clientWidth * 0.85,
                behavior: motionOK ? "smooth" : "auto"
            });
        };

        prev.addEventListener("click", function () { scrollByAmount(-1); });
        next.addEventListener("click", function () { scrollByAmount(1); });
        track.addEventListener("scroll", scheduleUpdate, { passive: true });

        if (window.ResizeObserver) {
            new ResizeObserver(scheduleUpdate).observe(track);
        } else {
            window.addEventListener("resize", scheduleUpdate);
        }

        update();
    });
})();
