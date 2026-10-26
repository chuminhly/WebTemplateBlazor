using Microsoft.AspNetCore.Components;

namespace WebTemplate.Core.Services;

public enum DialogKind { Confirm, Danger, Info }

public sealed class DialogRequest
{
    public required string Title { get; init; }
    public string? Message { get; init; }
    public RenderFragment? Body { get; init; }
    public string? OkText { get; init; }
    public string? CancelText { get; init; }
    public DialogKind Kind { get; init; } = DialogKind.Confirm;
    public bool ShowCancel { get; init; } = true;
    internal TaskCompletionSource<bool> Result { get; } = new();
}

/// <summary>
/// Hop thoai xac nhan dung chung. Can <c>&lt;WtDialogHost /&gt;</c> trong layout (WtAppShell da co san).
/// </summary>
/// <example><code>if (await Dialogs.ConfirmDeleteAsync("khach hang ABC")) { ... }</code></example>
public sealed class DialogService
{
    private readonly Queue<DialogRequest> _queue = new();

    public DialogRequest? Current { get; private set; }

    public event Action? Changed;

    public Task<bool> ShowAsync(DialogRequest request)
    {
        if (Current is null)
        {
            Current = request;
            Changed?.Invoke();
        }
        else
        {
            _queue.Enqueue(request);
        }
        return request.Result.Task;
    }

    public Task<bool> ConfirmAsync(string title, string? message = null, string? okText = null) =>
        ShowAsync(new DialogRequest { Title = title, Message = message, OkText = okText });

    public Task<bool> ConfirmDeleteAsync(string itemName) =>
        ShowAsync(new DialogRequest
        {
            Title = "Xác nhận xóa",
            Message = $"Bạn có chắc muốn xóa {itemName}? Thao tác này không thể hoàn tác.",
            OkText = "Xóa",
            Kind = DialogKind.Danger,
        });

    public Task AlertAsync(string title, string? message = null) =>
        ShowAsync(new DialogRequest { Title = title, Message = message, Kind = DialogKind.Info, ShowCancel = false, OkText = "Đóng" });

    internal void Close(bool result)
    {
        var closing = Current;
        Current = _queue.Count > 0 ? _queue.Dequeue() : null;
        closing?.Result.TrySetResult(result);
        Changed?.Invoke();
    }
}
