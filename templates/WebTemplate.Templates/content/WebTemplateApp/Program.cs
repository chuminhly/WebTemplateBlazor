using System.Globalization;
using Microsoft.AspNetCore.Components.Routing;
using WebTemplate.Core;
using WebTemplate.Core.Navigation;
using WebTemplate.DevExpress;
using WebTemplateApp.Components;
using WebTemplateApp.Reports;

var builder = WebApplication.CreateBuilder(args);

var vi = CultureInfo.GetCultureInfo("vi-VN");
CultureInfo.DefaultThreadCurrentCulture = vi;
CultureInfo.DefaultThreadCurrentUICulture = vi;

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddWebTemplate(o =>
{
    o.AppName = "__APP_TITLE__";
    o.DefaultTheme = "__THEME__";
    o.Company = new CompanyInfo { Name = "__COMPANY__" };

    // Khai bao menu ben trai. Icon: xem trang /giao-dien cua app mau hoac WtIcons.Names.
    o.AddMenu(new MenuItem("Tổng quan", "", "home") { Match = NavLinkMatch.All })
     .AddMenu(MenuItem.Section("Nghiệp vụ"))
     .AddMenu(new MenuItem("Danh mục mẫu", "danh-muc", "grid"))
     .AddMenu(new MenuItem("Báo cáo", "bao-cao", "pie"));
});

builder.Services.AddWebTemplateDevExpress(o =>
{
    o.Reports.Add<SampleListReport>("danh-sach-mau", "Danh sách mẫu", group: "Báo cáo chung");
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
