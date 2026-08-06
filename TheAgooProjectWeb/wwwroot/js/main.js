$(function () {
    var sidebarEl = document.getElementById('sidebarOffcanvas');
    var $sidebar = $(sidebarEl);

    // Single toggle button: offcanvas below lg, icon-only collapse at lg+
    $('#sidebarToggle').on('click', function () {
        if (window.innerWidth < 992) {
            bootstrap.Offcanvas.getOrCreateInstance(sidebarEl).toggle();
        } else {
            $sidebar.toggleClass('sidebar-collapsed');
        }
    });

    // Keep state sane when crossing the breakpoint
    $(window).on('resize', function () {
        if (window.innerWidth >= 992) {
            var oc = bootstrap.Offcanvas.getInstance(sidebarEl);
            if (oc) oc.hide();
        } else {
            $sidebar.removeClass('sidebar-collapsed');
        }
    });

    // Re-expand a collapsed sidebar automatically when a submenu is opened
    $('.sidebar [data-bs-toggle="collapse"]').on('click', function () {
        if ($sidebar.hasClass('sidebar-collapsed')) $sidebar.removeClass('sidebar-collapsed');
    });

    // Manual active-state + auto-close on mobile for placeholder / non-scrollspy links
    $('.sidebar .nav-link:not([data-bs-toggle]):not(.scrollspy-link)').on('click', function () {
        $('.sidebar .nav-link').removeClass('active');
        $(this).addClass('active');
        if (window.innerWidth < 992) {
            var oc = bootstrap.Offcanvas.getInstance(sidebarEl);
            if (oc) oc.hide();
        }
    });

    // Theme toggle (Bootstrap's own data-bs-theme color mode) + persistence
    function applyThemeIcon() {
        var isDark = document.documentElement.getAttribute('data-bs-theme') === 'dark';
        $('#themeIcon').attr('class', isDark ? 'bi bi-moon-stars' : 'bi bi-sun-fill');
    }
    applyThemeIcon();
    $('#themeToggle').on('click', function () {
        var html = document.documentElement;
        var isDark = html.getAttribute('data-bs-theme') === 'dark';
        var next = isDark ? 'light' : 'dark';
        html.setAttribute('data-bs-theme', next);
        try { localStorage.setItem('agoo-theme', next); } catch (e) { }
        applyThemeIcon();
    });

    // Live date + clock
    function updateClock() {
        var now = new Date();
        $('#fullDate').text(now.toLocaleDateString('en-GB', { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' }));
        $('#clock').text(now.toLocaleTimeString('en-GB', { hour: '2-digit', minute: '2-digit', second: '2-digit' }));
    }
    updateClock();
    setInterval(updateClock, 1000);

    // Animate attendance bars in
    setTimeout(function () {
        $('.bar').each(function () { $(this).css('height', $(this).data('height') + '%'); });
    }, 200);

    // Count-up KPI numbers
    $('.kpi-value').each(function () {
        var $el = $(this);
        var target = parseFloat($el.data('count'));
        var prefix = $el.data('prefix') || '';
        var suffix = $el.data('suffix') || '';
        var isDecimal = target % 1 !== 0;
        var current = 0;
        var steps = 40;
        var increment = target / steps;
        var timer = setInterval(function () {
            current += increment;
            if (current >= target) { current = target; clearInterval(timer); }
            $el.text(prefix + (isDecimal ? current.toFixed(1) : Math.round(current).toLocaleString()) + suffix);
        }, 25);
    });
});