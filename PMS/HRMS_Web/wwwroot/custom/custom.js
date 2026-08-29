function validateNumberInput(event) {
    // Allow: backspace, delete, tab, escape, and enter
    if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 ||
        // Allow: Ctrl+A
        (event.keyCode == 65 && event.ctrlKey === true) ||
        // Allow: home, end, left, right, down, up
        (event.keyCode >= 35 && event.keyCode <= 40)) {
        // let it happen, don't do anything
        return;
    }
    else {
        // Ensure that it is a number and stop the keypress
        if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
            event.preventDefault();
        }
    }
}

function formatPKR(value) {
    var roundedValue = Math.round(value);
    var formattedValue = roundedValue.toLocaleString('en-PK').replace(/Rs/g, '');
    return formattedValue;
}

/* Sidebar open/active state is rendered SERVER-SIDE since 2026-08-17:
   _NavigationMenu.cshtml matches the current URL against the registry and
   emits nav-active on the open form plus open/nav-trail on its ancestor
   chain (theme.js keeps the trail expanded). The sessionStorage replay and
   li.active URL-matching that lived here are retired — they double-marked
   folders and left stale highlights after search navigation. Only the
   content-height recalc remains. */
jQuery(function ($) {
    if (typeof calculateContentHeight === 'function') {
        calculateContentHeight();
    }
});




