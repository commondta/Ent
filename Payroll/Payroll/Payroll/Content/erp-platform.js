/* ERP platform: header application switcher (click to open, Esc / outside click to close) */
(function () {
    var root = document.getElementById('erpAppSwitch');
    if (!root) return;
    var toggle = root.querySelector('.erp-appswitch-toggle');
    var menu = root.querySelector('.erp-appswitch-menu');
    if (!toggle || !menu) return;
    function open() { menu.hidden = false; toggle.setAttribute('aria-expanded', 'true'); }
    function close() { menu.hidden = true; toggle.setAttribute('aria-expanded', 'false'); }
    toggle.addEventListener('click', function (e) { e.preventDefault(); e.stopPropagation(); if (menu.hidden) open(); else close(); });
    document.addEventListener('click', function (e) { if (!root.contains(e.target)) close(); });
    document.addEventListener('keydown', function (e) { if (e.key === 'Escape') close(); });
})();
