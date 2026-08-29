/* Payroll ERP shell behaviour: navigation state, global search, recent activity, toasts, confirm dialogs. */
(function ($) {
    'use strict';

    var ERP = window.ERP = window.ERP || {};
    ERP.base = (ERP.base || '/').replace(/\/$/, '');
    ERP.url = function (p) { return ERP.base + p; };

    // ---------- helpers ----------
    ERP.token = function () { return $('#af input[name=__RequestVerificationToken]').val(); };

    /** POST with the anti-forgery token attached; `data` may be an object or a FormData/string. */
    ERP.post = function (url, data, done, fail) {
        var payload = $.extend({ __RequestVerificationToken: ERP.token() }, data || {});
        return $.ajax({ type: 'POST', url: url, data: payload, traditional: false })
            .done(function (r) { if (done) done(r); })
            .fail(function (xhr) {
                var msg = xhr.status === 403 ? 'You do not have permission for this action.' :
                          xhr.status === 401 ? 'Your session has expired. Please sign in again.' :
                          'The request failed (' + xhr.status + ').';
                if (fail) fail(msg, xhr); else ERP.toast(msg, 'err');
                if (xhr.status === 401) setTimeout(function () { location.href = ERP.url('/Account/Login') + '?returnUrl=' + encodeURIComponent(location.pathname + location.search); }, 1200);
            });
    };

    ERP.esc = function (s) { return $('<div>').text(s == null ? '' : String(s)).html(); };
    ERP.icon = function (name, size) {
        var d = (ERP.icons && ERP.icons[name]) || ERP.icons['circle'];
        return '<svg class="ico" width="' + (size || 16) + '" height="' + (size || 16) + '" viewBox="0 0 16 16" fill="currentColor" aria-hidden="true"><path d="' + d + '"/></svg>';
    };

    // Small inline icon set for client-rendered content (mirrors Infrastructure/Icons.cs for the few we need).
    ERP.icons = {
        'circle': 'M8 3a5 5 0 1 0 0 10A5 5 0 0 0 8 3',
        'check': 'M13.854 3.646a.5.5 0 0 1 0 .708l-7 7a.5.5 0 0 1-.708 0l-3.5-3.5a.5.5 0 1 1 .708-.708L6.5 10.293l6.646-6.647a.5.5 0 0 1 .708 0',
        'x': 'M4.646 4.646a.5.5 0 0 1 .708 0L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 0 1 0-.708',
        'info': 'M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14m0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16 m8.93 6.588-2.29.287-.082.38.45.083c.294.07.352.176.288.469l-.738 3.468c-.194.897.105 1.319.808 1.319.545 0 1.178-.252 1.465-.598l.088-.416c-.2.176-.492.246-.686.246-.275 0-.375-.193-.304-.533zM9 4.5a1 1 0 1 1-2 0 1 1 0 0 1 2 0',
        'warn': 'M7.938 2.016A.13.13 0 0 1 8.002 2a.13.13 0 0 1 .063.016.15.15 0 0 1 .054.057l6.857 11.667c.036.06.035.124.002.183a.2.2 0 0 1-.054.06.1.1 0 0 1-.066.017H1.146a.1.1 0 0 1-.066-.017.2.2 0 0 1-.054-.06.18.18 0 0 1 .002-.183L7.884 2.073a.15.15 0 0 1 .054-.057m1.044-.45a1.13 1.13 0 0 0-1.96 0L.165 13.233c-.457.778.091 1.767.98 1.767h13.713c.889 0 1.438-.99.98-1.767zM7 6a1 1 0 1 1 2 0v3a1 1 0 0 1-2 0zm1 6a1 1 0 1 1 0-2 1 1 0 0 1 0 2',
        'person': 'M8 8a3 3 0 1 0 0-6 3 3 0 0 0 0 6m2-3a2 2 0 1 1-4 0 2 2 0 0 1 4 0m4 8c0 1-1 1-1 1H3s-1 0-1-1 1-4 6-4 6 3 6 4m-1-.004c-.001-.246-.154-.986-.832-1.664C11.516 10.68 10.289 10 8 10s-3.516.68-4.168 1.332c-.678.678-.83 1.418-.832 1.664z',
        'file': 'M5 4a.5.5 0 0 0 0 1h6a.5.5 0 0 0 0-1zm-.5 2.5A.5.5 0 0 1 5 6h6a.5.5 0 0 1 0 1H5a.5.5 0 0 1-.5-.5M5 8a.5.5 0 0 0 0 1h6a.5.5 0 0 0 0-1zm0 2a.5.5 0 0 0 0 1h3a.5.5 0 0 0 0-1z M2 2a2 2 0 0 1 2-2h8a2 2 0 0 1 2 2v12a2 2 0 0 1-2 2H4a2 2 0 0 1-2-2zm10-1H4a1 1 0 0 0-1 1v12a1 1 0 0 0 1 1h8a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1',
        'activity': 'M6 2a.5.5 0 0 1 .47.33L10 12.036l1.53-4.208A.5.5 0 0 1 12 7.5h3.5a.5.5 0 0 1 0 1h-3.15l-1.88 5.17a.5.5 0 0 1-.94 0L6 3.964 4.47 8.171A.5.5 0 0 1 4 8.5H.5a.5.5 0 0 1 0-1h3.15l1.88-5.17A.5.5 0 0 1 6 2',
        'person-badge': 'M6.5 2a.5.5 0 0 0 0 1h3a.5.5 0 0 0 0-1zM11 8a3 3 0 1 1-6 0 3 3 0 0 1 6 0 M4.5 0A2.5 2.5 0 0 0 2 2.5V14a2 2 0 0 0 2 2h8a2 2 0 0 0 2-2V2.5A2.5 2.5 0 0 0 11.5 0zM3 2.5A1.5 1.5 0 0 1 4.5 1h7A1.5 1.5 0 0 1 13 2.5v10.795a4.2 4.2 0 0 0-.776-.492C11.392 12.387 10.063 12 8 12s-3.392.387-4.224.803a4.2 4.2 0 0 0-.776.492z'
    };

    // ---------- toast ----------
    ERP.toast = function (msg, kind, ms) {
        var wrap = $('.toast-wrap');
        if (!wrap.length) wrap = $('<div class="toast-wrap">').appendTo('body');
        var t = $('<div class="toast ' + (kind === 'err' ? 'err' : 'ok') + '" role="status">' + ERP.icon(kind === 'err' ? 'warn' : 'check') + '<div>' + ERP.esc(msg) + '</div></div>').appendTo(wrap);
        setTimeout(function () { t.fadeOut(200, function () { t.remove(); }); }, ms || (kind === 'err' ? 5000 : 3000));
    };

    // ---------- confirm ----------
    /** ERP.confirm({title, text, ok:'Delete', danger:true}, function(){...}) */
    ERP.confirm = function (opt, onOk) {
        var id = 'erpConfirm';
        $('#' + id).remove();
        var html =
            '<div class="modal fade confirm-box" id="' + id + '" tabindex="-1" role="dialog"><div class="modal-dialog"><div class="modal-content">' +
            '<div class="modal-body"><div class="confirm-ico ' + (opt.danger ? '' : 'neutral') + '">' + ERP.icon(opt.danger ? 'warn' : 'info', 18) + '</div>' +
            '<div><h4>' + ERP.esc(opt.title || 'Are you sure?') + '</h4><p>' + ERP.esc(opt.text || '') + '</p></div></div>' +
            '<div class="modal-footer"><button type="button" class="btn btn-default" data-dismiss="modal">' + ERP.esc(opt.cancel || 'Cancel') + '</button>' +
            '<button type="button" class="btn ' + (opt.danger ? 'btn-danger' : 'btn-primary') + ' js-ok">' + ERP.esc(opt.ok || 'Continue') + '</button></div>' +
            '</div></div></div>';
        var m = $(html).appendTo('body');
        m.find('.js-ok').on('click', function () { m.modal('hide'); if (onOk) onOk(); });
        m.on('hidden.bs.modal', function () { m.remove(); });
        m.modal('show');
    };

    // ---------- navigation ----------
    function initNav() {
        var path = location.pathname.toLowerCase().replace(/\/$/, '');
        var $active = $('.sidebar-menu a[href]').filter(function () {
            var h = ($(this).attr('href') || '').toLowerCase().replace(/\/$/, '');
            return h && h === path;
        }).first();
        if ($active.length) {
            $active.closest('li').addClass('active');
            $active.closest('.treeview').addClass('active menu-open');
        }

        // Treeview toggle (one open at a time in expanded mode).
        $('.sidebar-menu > li.treeview > a').on('click', function (e) {
            e.preventDefault();
            var li = $(this).parent();
            if ($('body').hasClass('sidebar-collapse') && $(window).width() > 767) {
                // Collapsed (icons only): expand the same bar and open this module - no separate flyout panel.
                $('body').removeClass('sidebar-collapse');
                try { localStorage.setItem('erp_nav', 'full'); } catch (e2) { }
                $('.sidebar-menu > li.treeview.menu-open').not(li).removeClass('menu-open').find('.treeview-menu').hide();
                li.addClass('menu-open'); li.children('.treeview-menu').show();
                return;
            }
            var open = li.hasClass('menu-open');
            $('.sidebar-menu > li.treeview.menu-open').not(li).removeClass('menu-open').find('.treeview-menu').slideUp(120);
            if (open) { li.removeClass('menu-open'); li.children('.treeview-menu').slideUp(120); }
            else { li.addClass('menu-open'); li.children('.treeview-menu').slideDown(120); }
        });
        $('.sidebar-menu > li.treeview.menu-open > .treeview-menu').show();

        // Collapse state persisted per browser.
        var body = $('body');
        try { if (localStorage.getItem('erp_nav') === 'mini' && $(window).width() > 767) body.addClass('sidebar-collapse'); } catch (e) { }
        $('[data-toggle="nav-collapse"]').on('click', function (e) {
            e.preventDefault();
            if ($(window).width() <= 767) { body.toggleClass('sidebar-open'); return; }
            body.toggleClass('sidebar-collapse');
            try { localStorage.setItem('erp_nav', body.hasClass('sidebar-collapse') ? 'mini' : 'full'); } catch (e2) { }
        });
        $(document).on('click', function (e) {
            if ($(window).width() <= 767 && body.hasClass('sidebar-open') && !$(e.target).closest('.main-sidebar, [data-toggle="nav-collapse"]').length) body.removeClass('sidebar-open');
        });
    }

    // ---------- global search ----------
    function initSearch() {
        var $wrap = $('.gs-wrap'), $input = $('#gsInput'), $res = $('#gsResults');
        if (!$input.length) return;
        var timer, seq = 0, idx = -1;

        function render(items, q) {
            idx = -1;
            if (!items.length) { $res.html('<div class="gs-empty">No results for "' + ERP.esc(q) + '"</div>').addClass('open'); return; }
            var groups = {}, order = [];
            items.forEach(function (it) { if (!groups[it.type]) { groups[it.type] = []; order.push(it.type); } groups[it.type].push(it); });
            var html = '';
            order.forEach(function (g) {
                html += '<div class="gs-group">' + ERP.esc(g === 'Form' ? 'Forms & modules' : g + 's') + '</div>';
                groups[g].forEach(function (it) {
                    html += '<a href="' + ERP.esc(it.url) + '">' + ERP.icon(it.icon === 'person-badge' ? 'person-badge' : 'file') + '<div><span>' + ERP.esc(it.title) + '</span><small>' + ERP.esc(it.sub) + '</small></div></a>';
                });
            });
            $res.html(html).addClass('open');
        }
        function search() {
            var q = $.trim($input.val());
            if (!q) { $res.removeClass('open').empty(); return; }
            var my = ++seq;
            $.getJSON(ERP.url('/Home/Search'), { q: q }).done(function (items) { if (my === seq) render(items, q); });
        }
        $input.on('input', function () { clearTimeout(timer); timer = setTimeout(search, 180); })
            .on('focus', function () { $wrap.addClass('has-focus'); if ($.trim($input.val())) search(); })
            .on('blur', function () { $wrap.removeClass('has-focus'); })
            .on('keydown', function (e) {
                var $a = $res.find('a');
                if (e.key === 'ArrowDown' || e.key === 'ArrowUp') {
                    e.preventDefault();
                    if (!$a.length) return;
                    idx = (idx + (e.key === 'ArrowDown' ? 1 : -1) + $a.length) % $a.length;
                    $a.removeClass('active').eq(idx).addClass('active');
                } else if (e.key === 'Enter') {
                    var $t = idx >= 0 ? $a.eq(idx) : $a.first();
                    if ($t.length) { e.preventDefault(); location.href = $t.attr('href'); }
                } else if (e.key === 'Escape') { $res.removeClass('open'); $input.blur(); }
            });
        $(document).on('click', function (e) { if (!$(e.target).closest('.gs-wrap').length) $res.removeClass('open'); });
        $(document).on('keydown', function (e) { if (e.key === '/' && !$(e.target).is('input,textarea,select,[contenteditable]')) { e.preventDefault(); $input.focus(); } });
    }

    // ---------- recent activity / approvals ----------
    function initActivity() {
        var $menu = $('#hdrActivity');
        if (!$menu.length) return;
        function load() {
            $.getJSON(ERP.url('/Security/RecentActivity')).done(function (r) {
                var $b = $('#approvalBadge');
                $b.text(r.pending || '').attr('data-zero', r.pending ? '0' : '1');
                var html = '';
                (r.items || []).forEach(function (a) {
                    var inner = '<span class="act-ico">' + ERP.icon(a.Action === 'Login' || a.Action === 'Logout' ? 'person' : 'activity', 13) + '</span>' +
                        '<div class="act-body"><b>' + ERP.esc(a.Username) + '</b> <span>' + ERP.esc(a.Detail || a.Action) + '</span><small>' + ERP.esc(a.When) + (a.Module ? ' · ' + ERP.esc(a.Module) : '') + '</small></div>';
                    html += '<li><a href="' + (a.Url ? ERP.esc(a.Url) : 'javascript:void(0)') + '">' + inner + '</a></li>';
                });
                $menu.find('.hdr-pop-list').html(html || '<li><div class="hdr-pop-empty">No recent activity</div></li>');
            });
        }
        $menu.closest('li.dropdown').on('show.bs.dropdown', load);
        load();
    }

    // ---------- legacy view polish ----------
    function polishLegacy() {
        // Old views hard-code a blue modal header; the class override handles the colour, this removes stray inline styles.
        $('.modal-header[style]').removeAttr('style');
        // Old "Attach file" buttons rendered as an unlabeled plus - give them a tooltip-visible label.
        $('.box-header .btn[data-toggle="tooltip"]').each(function () { var t = $(this).attr('title'); if (t && !$(this).text().trim()) $(this).attr('aria-label', t); });
        // Wrap DataTables search input consistently.
        $('.dataTables_filter input').addClass('form-control');
    }

    /* Approval gate: any AJAX answer that says the write was held for approval shows it, whatever the legacy view does with the response */
    $(document).ajaxComplete(function (ev, xhr) {
        try { var r = JSON.parse(xhr.responseText); if (r && r.approvalPending) ERP.toast(r.message, 'ok', 6000); } catch (e) { }
    });

    $(function () {
        initNav();
        initSearch();
        initActivity();
        polishLegacy();
        // Legacy code uses alert() for confirmations; keep those, but show TempData messages as toasts.
        $('[data-toast]').each(function () { ERP.toast($(this).data('toast'), $(this).data('toast-kind') || 'ok'); });
    });
})(jQuery);
