// Chay dong bo trong <head> truoc khi trang hien thi, de ap theme da luu ngay lap tuc (khong nhay mau).
(function () {
    try {
        var raw = localStorage.getItem('wt-theme-preferences');
        if (!raw) return;
        var p = JSON.parse(raw);
        var themes = ['light', 'dark', 'system'];
        var mode = themes[p.Mode] || 'light';
        var dark = mode === 'dark' || (mode === 'system' && window.matchMedia('(prefers-color-scheme: dark)').matches);
        var r = document.documentElement;
        if (p.ThemeKey) r.setAttribute('data-theme', p.ThemeKey);
        r.setAttribute('data-mode', dark ? 'dark' : 'light');
        r.setAttribute('data-density', p.Density === 1 ? 'compact' : 'comfortable');
        if (p.FontScale) r.style.setProperty('--wt-font-scale', (p.FontScale / 100).toString());
        r.style.colorScheme = dark ? 'dark' : 'light';
    } catch (e) { }
})();
