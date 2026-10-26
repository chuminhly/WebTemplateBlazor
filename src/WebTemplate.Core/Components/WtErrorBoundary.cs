using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;

namespace WebTemplate.Core.Components;

/// <summary>Bat loi trong noi dung trang va hien thong bao than thien thay vi lam sap ca app.</summary>
public sealed class WtErrorBoundary : ErrorBoundary
{
    [Inject] private ILogger<WtErrorBoundary> Logger { get; set; } = default!;
    [Inject] private NavigationManager Nav { get; set; } = default!;

    protected override Task OnErrorAsync(Exception exception)
    {
        Logger.LogError(exception, "Loi khi hien thi trang {Uri}", Nav.Uri);
        return Task.CompletedTask;
    }

    protected override void OnParametersSet()
    {
        // Chuyen trang thi xoa trang thai loi de trang moi hien binh thuong.
        Recover();
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (CurrentException is null)
        {
            builder.AddContent(0, ChildContent);
            return;
        }

        builder.OpenComponent<WtErrorPage>(1);
        builder.AddComponentParameter(2, nameof(WtErrorPage.Code), 500);
        builder.AddComponentParameter(3, nameof(WtErrorPage.Detail), CurrentException.Message);
        builder.AddComponentParameter(4, nameof(WtErrorPage.OnRetry), EventCallback.Factory.Create(this, Recover));
        builder.CloseComponent();
    }
}
