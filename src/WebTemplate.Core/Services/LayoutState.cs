namespace WebTemplate.Core.Services;

/// <summary>Trang thai khung giao dien (menu thu gon, menu tren dien thoai) cua nguoi dung hien tai.</summary>
public sealed class LayoutState(WebTemplateJsInterop js)
{
    private const string CollapsedKey = "wt-sidebar-collapsed";
    private bool _loaded;

    public bool SidebarCollapsed { get; private set; }

    public bool MobileMenuOpen { get; private set; }

    public event Action? Changed;

    public async Task LoadAsync()
    {
        if (_loaded)
        {
            return;
        }
        _loaded = true;
        SidebarCollapsed = await js.GetStoredAsync<bool>(CollapsedKey);
        Changed?.Invoke();
    }

    public async Task ToggleSidebarAsync()
    {
        SidebarCollapsed = !SidebarCollapsed;
        Changed?.Invoke();
        await js.SetStoredAsync(CollapsedKey, SidebarCollapsed);
    }

    public void SetMobileMenu(bool open)
    {
        if (MobileMenuOpen != open)
        {
            MobileMenuOpen = open;
            Changed?.Invoke();
        }
    }
}
