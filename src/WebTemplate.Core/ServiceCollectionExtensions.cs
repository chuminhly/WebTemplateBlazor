using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebTemplate.Core.Localization;
using WebTemplate.Core.Services;
using WebTemplate.Core.Theming;

namespace WebTemplate.Core;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Dang ky layout, theme, menu, thong bao, hop thoai cua WebTemplate.
    /// </summary>
    /// <example>
    /// <code>
    /// builder.Services.AddWebTemplate(o =&gt;
    /// {
    ///     o.AppName = "Quan ly ban hang";
    ///     o.AddMenu(new MenuItem("Trang chu", "/", "home") { Match = NavLinkMatch.All });
    /// });
    /// </code>
    /// </example>
    public static IServiceCollection AddWebTemplate(this IServiceCollection services, Action<WebTemplateOptions>? configure = null)
    {
        services.AddOptions<WebTemplateOptions>();
        if (configure is not null)
        {
            services.Configure(configure);
        }

        services.AddHttpContextAccessor();
        services.TryAddScoped<WebTemplateJsInterop>();
        services.TryAddScoped<IThemePreferenceStore, BrowserThemePreferenceStore>();
        services.TryAddScoped<ThemeService>();
        services.TryAddScoped<ToastService>();
        services.TryAddScoped<DialogService>();
        services.TryAddScoped<LayoutState>();
        services.TryAddSingleton<WtText>();
        return services;
    }
}
