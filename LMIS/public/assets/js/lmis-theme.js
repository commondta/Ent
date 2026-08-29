/* ============================================================
   LMIS theme behaviours (2026-08-22) – presentation layer only.
   1. DataTables empty-state vector (set before any table inits)
   2. Breadcrumb + page-title icon chip from the active menu item
   3. Ctrl+K command search over every module / form in the menu
   4. Required-field asterisks on labels
   5. Recent & Favourites: tracker, header popovers, page-bar star, shared API
   6. Navigation (PMS parity): chevron chips, active trail, icons-only < 1200px, ⋮ More
   No route, controller, permission or form contract is touched.
   ============================================================ */
(function () {
    'use strict';

    var EMPTY_SVG = '<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"><polyline points="22 12 16 12 14 15 10 15 8 12 2 12"/><path d="M5.45 5.11 2 12v6a2 2 0 0 0 2 2h16a2 2 0 0 0 2-2v-6l-3.45-6.89A2 2 0 0 0 16.76 4H7.24a2 2 0 0 0-1.79 1.11z"/></svg>';

    /* 1. DataTables defaults – runs immediately, before the layout's ready() auto-init */
    if (window.jQuery && jQuery.fn && jQuery.fn.dataTable) {
        jQuery.extend(true, jQuery.fn.dataTable.defaults, {
            language: {
                emptyTable: '<div class="lm-empty">' + EMPTY_SVG + '<span>No data available</span></div>',
                zeroRecords: '<div class="lm-empty">' + EMPTY_SVG + '<span>No matching records</span></div>'
            }
        });
    }

    /* 5. Recent & Favourites — one browser-side store shared by the header
          popovers, the page-bar star and the My Home cards (as in PMS).
          Exposed immediately as window.limsRecentFav so page scripts can use it. */
    var LRF = (function () {
        var RECENT_KEY = 'lims_recent_forms', FAV_KEY = 'lims_fav_forms', MAX_RECENT = 10;
        var listeners = [];
        function load(k) { try { return JSON.parse(localStorage.getItem(k)) || []; } catch (e) { return []; } }
        function save(k, v) { try { localStorage.setItem(k, JSON.stringify(v)); } catch (e) { } }
        function esc(s) { return String(s == null ? '' : s).replace(/[&<>"']/g, function (c) { return { '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c]; }); }
        function emit() { listeners.forEach(function (fn) { try { fn(); } catch (e) { } }); }
        var api = {
            esc: esc,
            recents: function () { return load(RECENT_KEY); },
            favs: function () { return load(FAV_KEY); },
            record: function (u, t, m) {
                if (!u || !t) return;
                var list = load(RECENT_KEY).filter(function (it) { return it.u !== u; });
                list.unshift({ u: u, t: t, m: m || '', ts: Date.now() });
                save(RECENT_KEY, list.slice(0, MAX_RECENT)); emit();
            },
            favHas: function (u) { return load(FAV_KEY).some(function (f) { return f.u === u; }); },
            favToggle: function (u, t, m) {
                if (!u) return false;
                var favs = load(FAV_KEY), on;
                if (favs.some(function (f) { return f.u === u; })) { favs = favs.filter(function (f) { return f.u !== u; }); on = false; }
                else { favs.unshift({ u: u, t: t || u, m: m || '' }); on = true; }
                save(FAV_KEY, favs); emit(); return on;
            },
            onChange: function (fn) { listeners.push(fn); },
            /* one list row: link + star; `cls` prefixes the element classes (hq- for popovers, mh- for Home) */
            itemHtml: function (e, fav, cls) {
                cls = cls || 'hq';
                var label = (fav ? 'Remove from favourites' : 'Add to favourites') + ': ' + e.t;
                return '<li><a class="' + cls + '-link" href="' + esc(e.u) + '">' + esc(e.t) + (e.m ? '<small>' + esc(e.m) + '</small>' : '') + '</a>' +
                    '<button type="button" class="' + cls + '-star lm-star' + (fav ? ' on' : '') + '" title="' + esc(label) + '" aria-label="' + esc(label) + '" aria-pressed="' + (fav ? 'true' : 'false') + '" data-u="' + esc(e.u) + '" data-t="' + esc(e.t) + '" data-m="' + esc(e.m || '') + '">&#9733;</button></li>';
            }
        };
        /* any star rendered by itemHtml toggles the favourite */
        document.addEventListener('click', function (ev) {
            var b = ev.target.closest('.lm-star'); if (!b) return;
            ev.preventDefault(); ev.stopPropagation();
            api.favToggle(b.getAttribute('data-u'), b.getAttribute('data-t'), b.getAttribute('data-m'));
        });
        return api;
    })();
    window.limsRecentFav = LRF;

    function text(el) { return (el && (el.textContent || '')).replace(/\s+/g, ' ').trim(); }

    function featherName(link) {
        var f = link && link.querySelector('[data-feather]');
        if (f) return f.getAttribute('data-feather');
        var svg = link && link.querySelector('svg.feather');
        if (svg) {
            var cls = (svg.getAttribute('class') || '').split(/\s+/).filter(function (c) { return c.indexOf('feather-') === 0; })[0];
            return cls ? cls.replace('feather-', '') : null;
        }
        return null;
    }

    function featherSvg(name, cls) {
        if (window.feather && feather.icons && feather.icons[name]) {
            return feather.icons[name].toSvg({ 'class': cls || '' });
        }
        return '';
    }

    function ready(fn) {
        // Run after the layout's jQuery ready handlers (sidebar-active.js marks the
        // active menu item there); jQuery 3 fires those asynchronously, so a plain
        // DOMContentLoaded listener would see the menu before it is marked.
        if (window.jQuery) { jQuery(function () { setTimeout(fn, 0); }); return; }
        if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', fn); else fn();
    }

    ready(function () {
        var nav = document.getElementById('navbarVerticalNav');
        if (!nav) return;

        /* ---- menu index (module → pages) ---- */
        var index = [];
        Array.prototype.forEach.call(nav.querySelectorAll('.nav-item-wrapper'), function (wrap) {
            var head = wrap.querySelector(':scope > a.nav-link');
            var moduleName = head ? text(head) : 'Modules';
            Array.prototype.forEach.call(wrap.querySelectorAll('.parent-wrapper a.nav-link'), function (a) {
                var href = a.getAttribute('href');
                if (!href || href === '#' || href.charAt(0) === '#' || /javascript:/i.test(href)) return;
                index.push({ module: moduleName, title: text(a), href: href, icon: featherName(a), el: a, head: head });
            });
        });
        // Settings / Administration style folders that are not wrapped
        Array.prototype.forEach.call(nav.querySelectorAll('a.nav-link[href]'), function (a) {
            var href = a.getAttribute('href');
            if (!href || href.charAt(0) === '#' || /javascript:/i.test(href)) return;
            if (index.some(function (i) { return i.el === a; })) return;
            var head = a.closest('.parent-wrapper') ? a.closest('.nav-item-wrapper') : null;
            var headLink = head ? head.querySelector(':scope > a.nav-link') : null;
            index.push({ module: headLink ? text(headLink) : 'General', title: text(a), href: href, icon: featherName(a), el: a, head: headLink });
        });

        /* ---- 1b. active form from the URL (PMS rule: exactly one leaf, marked from the URL).
           sidebar-active.js only knows some routes; for the rest the menu link whose path
           is the longest prefix of the current path becomes the active leaf. ---- */
        if (!nav.querySelector('a.nav-link.active:not(.dropdown-indicator)')) {
            var lmPath = location.pathname.replace(/\/+$/, ''), lmBest = null;
            nav.querySelectorAll('a.nav-link:not(.dropdown-indicator)[href]').forEach(function (a) {
                try {
                    var p = new URL(a.getAttribute('href'), location.href).pathname.replace(/\/+$/, '');
                    if (p && (lmPath === p || lmPath.indexOf(p + '/') === 0) && (!lmBest || p.length > lmBest.p.length)) lmBest = { a: a, p: p };
                } catch (e) { }
            });
            if (lmBest) lmBest.a.classList.add('active');
        }

        /* ---- 2. breadcrumb + title chip ---- */
        (function () {
            var active = nav.querySelector('a.nav-link.active:not(.dropdown-indicator)');
            var content = document.querySelector('main .content') || document.querySelector('.content');
            if (!content) return;
            var entry = null;
            if (active) entry = index.filter(function (i) { return i.el === active; })[0] || { module: null, title: text(active), icon: featherName(active) };
            /* Recent forms: remember this page when it is a form from the menu */
            var isForm = !!(entry && entry.title && active && !active.classList.contains('home-nav'));
            var pagePath = location.pathname + location.search;
            if (isForm) LRF.record(pagePath, entry.title, entry.module || '');
            if (content.classList.contains('lm-no-crumbs')) return;
            var crumbs = document.createElement('nav');
            crumbs.className = 'lm-pagebar';
            crumbs.setAttribute('aria-label', 'Breadcrumb');
            var homeMeta = document.querySelector('meta[name="lm-home-url"]'); var homeHref = homeMeta ? homeMeta.getAttribute('content') : ((document.querySelector('.navbar-top .navbar-brand') || {}).getAttribute ? document.querySelector('.navbar-top .navbar-brand').getAttribute('href') : '#');
            var html = '<a href="' + homeHref + '">Home</a>';
            if (entry && entry.module) html += '<span class="lm-crumb-sep">/</span><span>' + entry.module + '</span>';
            var titleEl = content.querySelector('.card-header h4, .card-header h3, .card-header .h4, h4.text-900');
            var pageTitle = entry ? entry.title : (titleEl ? text(titleEl) : '');
            if (pageTitle) html += '<span class="lm-crumb-sep">/</span><span class="lm-crumb-current">' + pageTitle + '</span>';
            crumbs.innerHTML = html;
            if (isForm) {
                /* star for the open form — same favourite as the header / My Home */
                var star = document.createElement('button');
                star.type = 'button'; star.className = 'lm-pagebar-star';
                star.setAttribute('data-u', pagePath);
                var paintStar = function () {
                    var on = LRF.favHas(pagePath);
                    star.classList.toggle('on', on);
                    star.setAttribute('aria-pressed', on ? 'true' : 'false');
                    star.title = on ? 'Remove from favourites' : 'Add to favourites';
                    star.innerHTML = '<span class="lm-star-glyph" aria-hidden="true">&#9733;</span><span class="lm-star-text">' + (on ? 'Favourite' : 'Add to favourites') + '</span>';
                };
                paintStar();
                star.addEventListener('click', function () { LRF.favToggle(pagePath, entry.title, entry.module || ''); });
                LRF.onChange(paintStar);
                crumbs.appendChild(star);
            }
            content.insertBefore(crumbs, content.firstChild);
            if (titleEl && !titleEl.querySelector('.lm-title-icon')) {
                var iconName = (entry && entry.icon) || 'file-text';
                var chip = document.createElement('span');
                chip.className = 'lm-title-icon';
                chip.setAttribute('aria-hidden', 'true');
                chip.innerHTML = featherSvg(iconName) || '';
                titleEl.insertBefore(chip, titleEl.firstChild);
            }
        })();

        /* ---- 3. command search ---- */
        (function () {
            var box = document.getElementById('lmCmd');
            var input = document.getElementById('lmCmdInput');
            var results = document.getElementById('lmCmdResults');
            if (!box || !input || !results) return;
            var activeIdx = -1, shown = [];

            function highlight(str, q) {
                if (!q) return str;
                var i = str.toLowerCase().indexOf(q.toLowerCase());
                if (i < 0) return str;
                return str.slice(0, i) + '<mark>' + str.slice(i, i + q.length) + '</mark>' + str.slice(i + q.length);
            }
            function render(q) {
                q = (q || '').trim();
                var list = index.filter(function (i) {
                    if (!q) return true;
                    var hay = (i.title + ' ' + i.module).toLowerCase();
                    return q.toLowerCase().split(/\s+/).every(function (w) { return hay.indexOf(w) >= 0; });
                }).slice(0, 40);
                shown = list; activeIdx = list.length ? 0 : -1;
                if (!list.length) { results.innerHTML = '<div class="lm-cmd-empty">Nothing matches “' + q.replace(/</g, '&lt;') + '”</div>'; return; }
                var html = '', lastGroup = null;
                list.forEach(function (i, n) {
                    if (i.module !== lastGroup) { html += '<div class="lm-cmd-group">' + i.module + '</div>'; lastGroup = i.module; }
                    html += '<a class="lm-cmd-item' + (n === 0 ? ' is-active' : '') + '" role="option" data-idx="' + n + '" href="' + i.href + '">' +
                        featherSvg(i.icon || 'file-text') + '<span>' + highlight(i.title, q) + '</span><span class="lm-cmd-path">Open</span></a>';
                });
                results.innerHTML = html;
            }
            function open() { render(input.value); results.classList.add('show'); box.setAttribute('aria-expanded', 'true'); }
            function close() { results.classList.remove('show'); box.setAttribute('aria-expanded', 'false'); }
            function setActive(n) {
                var items = results.querySelectorAll('.lm-cmd-item');
                if (!items.length) return;
                activeIdx = (n + items.length) % items.length;
                Array.prototype.forEach.call(items, function (el, k) { el.classList.toggle('is-active', k === activeIdx); });
                var cur = items[activeIdx]; if (cur && cur.scrollIntoView) cur.scrollIntoView({ block: 'nearest' });
            }
            input.addEventListener('focus', open);
            input.addEventListener('input', function () { render(input.value); results.classList.add('show'); });
            input.addEventListener('keydown', function (e) {
                if (e.key === 'ArrowDown') { e.preventDefault(); setActive(activeIdx + 1); }
                else if (e.key === 'ArrowUp') { e.preventDefault(); setActive(activeIdx - 1); }
                else if (e.key === 'Enter') { e.preventDefault(); var cur = results.querySelector('.lm-cmd-item.is-active'); if (cur) window.location.href = cur.getAttribute('href'); }
                else if (e.key === 'Escape') { input.blur(); close(); }
            });
            results.addEventListener('mousedown', function (e) { e.preventDefault(); }); // keep focus until click completes
            results.addEventListener('click', function (e) { var a = e.target.closest('.lm-cmd-item'); if (a) window.location.href = a.getAttribute('href'); });
            document.addEventListener('click', function (e) { if (!box.contains(e.target)) close(); });
            document.addEventListener('keydown', function (e) {
                if ((e.ctrlKey || e.metaKey) && (e.key === 'k' || e.key === 'K')) { e.preventDefault(); input.focus(); input.select(); }
                if (e.key === '/' && !/input|textarea|select/i.test((e.target && e.target.tagName) || '') && !e.ctrlKey && !e.metaKey && !e.altKey) { e.preventDefault(); input.focus(); }
            });
        })();

        /* ---- 3b. form action bar: the main form's submit (+ sibling cancel/back) buttons
                move into one right-aligned bar at the bottom of the form (still inside
                the <form>, so submit/validation wiring is untouched) ---- */
        (function () {
            var forms = document.querySelectorAll('main .content form, main form');
            Array.prototype.forEach.call(forms, function (form) {
                if (form.closest('.modal, table, .dataTables_wrapper, .navbar, .lm-cmd')) return;
                var submits = Array.prototype.filter.call(form.querySelectorAll('button[type="submit"], input[type="submit"]'), function (b) {
                    return !b.closest('table, .modal, .input-group, .lo-table') && !b.classList.contains('btn-sm') && b.offsetParent !== null;
                });
                if (submits.length !== 1) return;                       // ambiguous forms are left alone
                var btn = submits[0];
                if (btn.closest('.lm-form-actions')) return;
                var holder = btn.parentElement;
                var group = [btn];
                if (holder && holder !== form) {
                    Array.prototype.forEach.call(holder.querySelectorAll(':scope > .btn, :scope > a.btn, :scope > button'), function (b) {
                        if (b !== btn && group.indexOf(b) < 0 && !b.closest('table')) group.push(b);
                    });
                }
                var bar = document.createElement('div');
                bar.className = 'lm-form-actions';
                // one style everywhere: a bordered strip at the end of the form (CSS handles .row gutters)
                // secondary buttons first, primary last (rightmost)
                group.sort(function (a, b) { return (a === btn) - (b === btn); });
                group.forEach(function (b) { bar.appendChild(b); });
                form.appendChild(bar);
                if (holder && holder !== form && holder.children.length === 0 && !holder.textContent.trim()) holder.style.display = 'none';
            });
        })();

        /* ---- 3c. long menu names: full name on hover (native tooltip) when the text is clipped ---- */
        (function () {
            var links = nav.querySelectorAll('a.nav-link');
            function refresh() {
                Array.prototype.forEach.call(links, function (a) {
                    var t = a.querySelector('.nav-link-text');
                    if (!t) return;
                    var clipped = t.scrollWidth > t.clientWidth + 1;
                    if (clipped) a.setAttribute('title', text(t)); else if (a.getAttribute('data-lm-title') !== 'keep') a.removeAttribute('title');
                });
            }
            refresh();
            var timer; window.addEventListener('resize', function () { clearTimeout(timer); timer = setTimeout(refresh, 150); });
            // sidebar expand/collapse (Phoenix toggles a class on <html>) changes widths too
            var mo = new MutationObserver(function () { clearTimeout(timer); timer = setTimeout(refresh, 250); });
            mo.observe(document.documentElement, { attributes: true, attributeFilter: ['class'] });
            nav.addEventListener('shown.bs.collapse', refresh); nav.addEventListener('hidden.bs.collapse', refresh);
            if (window.jQuery) jQuery(nav).on('shown.bs.collapse hidden.bs.collapse', refresh);
        })();

        /* ---- 3d. broken upload thumbnails: hide instead of showing the browser's broken-image glyph ---- */
        (function () {
            Array.prototype.forEach.call(document.querySelectorAll('main .content img'), function (img) {
                if (img.closest('.navbar, .lm-brand-mark, .lm-footer-text')) return;
                var hide = function () { img.style.display = 'none'; img.setAttribute('data-lm-broken', '1'); };
                img.addEventListener('error', hide);
                if (img.complete && img.naturalWidth === 0 && img.getAttribute('src')) hide();
            });
        })();

        /* ---- 4. required markers ---- */
        (function () {
            var fields = document.querySelectorAll('form [required]:not([type="hidden"])');
            Array.prototype.forEach.call(fields, function (f) {
                var label = null;
                if (f.id) label = document.querySelector('label[for="' + CSS.escape(f.id) + '"]');
                if (!label) {
                    var p = f.parentElement, hops = 0;
                    while (p && hops < 3 && !label) { label = p.querySelector(':scope > label, :scope > .form-label'); p = p.parentElement; hops++; }
                }
                if (!label || label.querySelector('.lm-req') || /\*\s*$/.test(text(label))) return;
                var s = document.createElement('span'); s.className = 'lm-req'; s.setAttribute('aria-hidden', 'true'); s.textContent = '*';
                label.appendChild(s);
            });
        })();

        /* ---- 5. header popovers: Recent / Favourites quick buttons ---- */
        (function () {
            var btnR = document.getElementById('lmRecentBtn'), btnF = document.getElementById('lmFavBtn');
            if (!btnR && !btnF) return;
            function makePop(id, title, empty) {
                var d = document.createElement('div');
                d.className = 'lm-pop'; d.id = id; d.setAttribute('role', 'menu');
                d.innerHTML = '<div class="lm-pop-title">' + title + '</div><ul></ul><div class="lm-pop-empty">' + empty + '</div>';
                document.body.appendChild(d); return d;
            }
            var popR = makePop('lmRecentPop', 'Recent forms', 'Nothing yet — forms you open will appear here.');
            var popF = makePop('lmFavPop', 'Favourite forms', 'No favourites yet — star a form in the Recent list or on its page bar.');
            function render() {
                var favs = LRF.favs(), map = {}; favs.forEach(function (f) { map[f.u] = true; });
                var rec = LRF.recents().slice(0, 10);
                popR.querySelector('ul').innerHTML = rec.map(function (e) { return LRF.itemHtml(e, !!map[e.u], 'hq'); }).join('');
                popR.querySelector('.lm-pop-empty').style.display = rec.length ? 'none' : '';
                popF.querySelector('ul').innerHTML = favs.map(function (e) { return LRF.itemHtml(e, true, 'hq'); }).join('');
                popF.querySelector('.lm-pop-empty').style.display = favs.length ? 'none' : '';
            }
            function closeAll() {
                [popR, popF].forEach(function (p) { p.classList.remove('show'); });
                [btnR, btnF].forEach(function (b) { if (b) { b.classList.remove('active'); b.setAttribute('aria-expanded', 'false'); } });
            }
            function toggle(btn, pop) {
                var wasOpen = pop.classList.contains('show');
                closeAll(); if (wasOpen) return;
                render();
                var r = btn.getBoundingClientRect(), w = 300;
                var left = Math.max(6, Math.min(r.left - 10, window.innerWidth - 6 - w));
                pop.style.top = (r.bottom + 6) + 'px'; pop.style.left = left + 'px';
                pop.classList.add('show'); btn.classList.add('active'); btn.setAttribute('aria-expanded', 'true');
            }
            if (btnR) btnR.addEventListener('click', function (e) { e.stopPropagation(); toggle(btnR, popR); });
            if (btnF) btnF.addEventListener('click', function (e) { e.stopPropagation(); toggle(btnF, popF); });
            LRF.onChange(function () { if (popR.classList.contains('show') || popF.classList.contains('show')) render(); });
            document.addEventListener('click', function (e) { if (!e.target.closest('.lm-pop, #lmRecentBtn, #lmFavBtn')) closeAll(); });
            document.addEventListener('keydown', function (e) { if (e.key === 'Escape') closeAll(); });
            window.addEventListener('resize', closeAll);
        })();

        /* ---- 6. navigation (PMS parity): chevron chips, active trail, icons-only below 1200px ---- */
        (function () {
            var CHEV_DOUBLE = '<svg class="lm-chev-double" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 16 16" fill="currentColor" aria-hidden="true"><path d="M2 2.5 7.5 8 2 13.5h3.2L10.7 8 5.2 2.5Z"/><path d="M7 2.5 12.5 8 7 13.5h3.2L15.7 8 10.2 2.5Z"/></svg>';
            var CHEV_SINGLE = '<svg class="lm-chev-single" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 16 16" fill="currentColor" aria-hidden="true"><path d="M4 2.5 9.5 8 4 13.5h3.2L12.7 8 7.2 2.5Z"/></svg>';
            /* a) the hand-drawn solid chevrons: double on main modules, single on sub-folders */
            nav.querySelectorAll('a.nav-link.dropdown-indicator').forEach(function (a) {
                var holder = a.querySelector('.dropdown-indicator-icon');
                if (!holder) {
                    holder = document.createElement('div'); holder.className = 'dropdown-indicator-icon';
                    a.appendChild(holder);
                }
                if (holder.querySelector('svg.lm-chev-double, svg.lm-chev-single')) return;
                /* drop the theme's caret (Font Awesome may already have turned the span into an inline svg) */
                holder.querySelectorAll('.fas, .fa-caret-right, .svg-inline--fa').forEach(function (n) { n.remove(); });
                var topLevel = !a.closest('.parent-wrapper');
                holder.insertAdjacentHTML('beforeend', topLevel ? CHEV_DOUBLE : CHEV_SINGLE);
            });
            /* b) active trail: the open form's ancestors render expanded (one leaf active at a time) */
            var activeLeaf = nav.querySelector('a.nav-link.active:not(.dropdown-indicator)');
            if (activeLeaf) {
                var col = activeLeaf.closest('.collapse');
                while (col && col.id !== 'navbarVerticalCollapse') {
                    col.classList.add('show');
                    var trig = document.querySelector('[aria-controls="' + col.id + '"], a[href="#' + col.id + '"]');
                    if (trig) { trig.setAttribute('aria-expanded', 'true'); }
                    col = col.parentElement ? col.parentElement.closest('.collapse') : null;
                }
            }
            /* c) icons-only mode — PMS short bar: clicking a module icon opens its list in a fly-out
                  panel to the right (NetSuite-style); one open at a time; outside click / Esc / mode
                  toggle close it. Bootstrap's collapse toggle is suppressed for the module row there. */
            function closeFlyouts() { nav.querySelectorAll('.nav-item-wrapper.lm-flyout-open').forEach(function (w) { w.classList.remove('lm-flyout-open'); }); }
            nav.addEventListener('click', function (e) {
                var a = e.target.closest('a.nav-link.dropdown-indicator.label-1'); if (!a) return;
                if (!document.documentElement.classList.contains('navbar-vertical-collapsed')) return;
                e.preventDefault(); e.stopPropagation(); e.stopImmediatePropagation();
                /* Bootstrap's delegated collapse handler still saw this click (the list sat in .collapsing for
                   ~350 ms and the forms appeared late): take the toggle attribute away for this event */
                var tog = a.getAttribute('data-bs-toggle');
                if (tog) { a.removeAttribute('data-bs-toggle'); setTimeout(function () { a.setAttribute('data-bs-toggle', tog); }, 0); }
                var w = a.closest('.nav-item-wrapper'); if (!w) return;
                var open = w.classList.contains('lm-flyout-open');
                closeFlyouts();
                if (!open) w.classList.add('lm-flyout-open');
            }, true);
            document.addEventListener('click', function (e) { if (!e.target.closest('.nav-item-wrapper.lm-flyout-open')) closeFlyouts(); });
            document.addEventListener('keydown', function (e) { if (e.key === 'Escape') closeFlyouts(); });
            var vt = document.querySelector('.navbar-vertical-toggle'); if (vt) vt.addEventListener('click', closeFlyouts);
            /* d) the navigation runs icons-only below 1200px (user's Collapsed View choice still wins above it) */
            function autoCollapse() {
                var pref = false;
                try { pref = JSON.parse(localStorage.getItem('phoenixIsNavbarVerticalCollapsed')) === true; } catch (e) { }
                var html = document.documentElement;
                if (window.innerWidth < 1200) { html.classList.add('navbar-vertical-collapsed'); html.setAttribute('data-lm-auto-collapsed', '1'); }
                else if (html.getAttribute('data-lm-auto-collapsed') === '1') { html.removeAttribute('data-lm-auto-collapsed'); if (!pref) html.classList.remove('navbar-vertical-collapsed'); }
            }
            autoCollapse();
            var rt; window.addEventListener('resize', function () { clearTimeout(rt); rt = setTimeout(function () { autoCollapse(); closeFlyouts(); }, 120); });
            /* f) search on phones: the header search icon drops the command pill under the bar */
            var sb = document.getElementById('lmSearchBtn'), cmdIn = document.getElementById('lmCmdInput');
            if (sb && cmdIn) {
                sb.addEventListener('click', function (e) {
                    e.stopPropagation();
                    var open = document.body.classList.toggle('lm-cmd-mobile-open');
                    sb.setAttribute('aria-expanded', open ? 'true' : 'false');
                    if (open) setTimeout(function () { cmdIn.focus(); }, 30);
                });
                document.addEventListener('click', function (e) { if (!e.target.closest('#lmCmd, #lmSearchBtn')) { document.body.classList.remove('lm-cmd-mobile-open'); sb.setAttribute('aria-expanded', 'false'); } });
                document.addEventListener('keydown', function (e) { if (e.key === 'Escape') { document.body.classList.remove('lm-cmd-mobile-open'); sb.setAttribute('aria-expanded', 'false'); } });
            }
            /* e) header ⋮ More: Recent / Favourites items open the same popovers */
            document.querySelectorAll('.lm-hdr-more [data-act]').forEach(function (it) {
                it.addEventListener('click', function (e) {
                    e.preventDefault();
                    var act = it.getAttribute('data-act');
                    var btn = document.getElementById(act === 'recent' ? 'lmRecentBtn' : 'lmFavBtn');
                    var pop = document.getElementById(act === 'recent' ? 'lmRecentPop' : 'lmFavPop');
                    if (!btn || !pop) return;
                    /* anchor the popover to the ⋮ button since the original control is folded away */
                    var more = it.closest('.lm-hdr-more').querySelector('.nav-link');
                    btn.click();
                    if (more && pop.classList.contains('show')) {
                        var r = more.getBoundingClientRect(), w = 300;
                        pop.style.top = (r.bottom + 6) + 'px';
                        pop.style.left = Math.max(6, Math.min(r.right - w, window.innerWidth - 6 - w)) + 'px';
                    }
                });
            });
        })();

        if (window.feather) { try { feather.replace(); } catch (e) { /* icons are optional */ } }
    });
})();
