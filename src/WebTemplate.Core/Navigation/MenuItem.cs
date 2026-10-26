using Microsoft.AspNetCore.Components.Routing;

namespace WebTemplate.Core.Navigation;

/// <summary>Mot muc trong menu ben trai. Co the long nhieu cap qua <see cref="Children"/>.</summary>
public sealed class MenuItem
{
    public MenuItem() { }

    public MenuItem(string title, string? href = null, string? icon = null)
    {
        Title = title;
        Href = href;
        Icon = icon;
    }

    public string Title { get; set; } = "";

    /// <summary>Duong dan trang. Bo trong neu chi la nhom chua menu con.</summary>
    public string? Href { get; set; }

    /// <summary>Ten icon cua <c>WtIcon</c>, vd "home", "users", "chart".</summary>
    public string? Icon { get; set; }

    /// <summary>Nhan nho ben phai (vd so don moi).</summary>
    public string? Badge { get; set; }

    /// <summary>Chi hien voi nguoi dung thuoc mot trong cac role nay. Bo trong = ai cung thay.</summary>
    public string[]? Roles { get; set; }

    /// <summary>Tieu de nhom: hien thanh dong chu nho phan cach, khong bam duoc.</summary>
    public bool IsSection { get; set; }

    public NavLinkMatch Match { get; set; } = NavLinkMatch.Prefix;

    public List<MenuItem> Children { get; set; } = [];

    public MenuItem Add(MenuItem child)
    {
        Children.Add(child);
        return this;
    }

    public static MenuItem Section(string title) => new(title) { IsSection = true };

    /// <summary>Duyet cay menu theo chieu sau, kem duong dan tu goc (dung cho breadcrumb).</summary>
    public static IEnumerable<IReadOnlyList<MenuItem>> Flatten(IEnumerable<MenuItem> items, IReadOnlyList<MenuItem>? parents = null)
    {
        parents ??= [];
        foreach (var item in items)
        {
            var path = parents.Append(item).ToList();
            yield return path;
            foreach (var child in Flatten(item.Children, path))
            {
                yield return child;
            }
        }
    }
}
