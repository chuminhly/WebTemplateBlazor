// Module JavaScript cua WebTemplate.Core.
const root = document.documentElement;
const darkQuery = window.matchMedia('(prefers-color-scheme: dark)');

function resolveDark(mode) {
    return mode === 'dark' || (mode === 'system' && darkQuery.matches);
}

export function applyTheme(p) {
    const dark = resolveDark(p.mode);
    root.setAttribute('data-theme', p.theme);
    root.setAttribute('data-mode', dark ? 'dark' : 'light');
    root.setAttribute('data-density', p.density);
    root.style.setProperty('--wt-font-scale', (p.fontScale / 100).toString());
    root.style.colorScheme = dark ? 'dark' : 'light';
    const value = [p.theme, p.mode, p.density, p.fontScale, dark ? 'dark' : 'light'].join('|');
    document.cookie = `wt-theme=${encodeURIComponent(value)}; path=/; max-age=31536000; samesite=lax`;
    return dark;
}

export function watchSystemMode(dotnetRef) {
    darkQuery.addEventListener('change', () => dotnetRef.invokeMethodAsync('OnSystemModeChanged'));
}

export function getStored(key) {
    try { return localStorage.getItem(key); } catch { return null; }
}

export function setStored(key, value) {
    try { localStorage.setItem(key, value); } catch { /* bo qua khi trinh duyet chan storage */ }
}

export function removeStored(key) {
    try { localStorage.removeItem(key); } catch { }
}

export function scrollToTop() {
    window.scrollTo({ top: 0, behavior: 'smooth' });
}
