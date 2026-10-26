namespace WebTemplate.Core.Services;

public enum ToastLevel { Success, Info, Warning, Error }

public sealed record ToastMessage(Guid Id, ToastLevel Level, string Title, string? Message, TimeSpan Duration);

/// <summary>
/// Hien thong bao nho goc phai man hinh. Can dat <c>&lt;WtToastHost /&gt;</c> trong layout (WtAppShell da co san).
/// </summary>
public sealed class ToastService
{
    private readonly List<ToastMessage> _items = [];

    public IReadOnlyList<ToastMessage> Items => _items;

    public event Action? Changed;

    public void Show(ToastLevel level, string title, string? message = null, TimeSpan? duration = null)
    {
        var toast = new ToastMessage(Guid.NewGuid(), level, title, message, duration ?? TimeSpan.FromSeconds(level == ToastLevel.Error ? 8 : 4));
        _items.Add(toast);
        Changed?.Invoke();
        _ = RemoveLaterAsync(toast);
    }

    public void Success(string title, string? message = null) => Show(ToastLevel.Success, title, message);
    public void Info(string title, string? message = null) => Show(ToastLevel.Info, title, message);
    public void Warning(string title, string? message = null) => Show(ToastLevel.Warning, title, message);
    public void Error(string title, string? message = null) => Show(ToastLevel.Error, title, message);

    public void Dismiss(Guid id)
    {
        if (_items.RemoveAll(t => t.Id == id) > 0)
        {
            Changed?.Invoke();
        }
    }

    private async Task RemoveLaterAsync(ToastMessage toast)
    {
        await Task.Delay(toast.Duration);
        Dismiss(toast.Id);
    }
}
