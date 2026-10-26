using DevExpress.Blazor.Localization;
using Microsoft.Extensions.Options;
using WebTemplate.Core;

namespace WebTemplate.DevExpress.Localization;

/// <summary>
/// Dich cac chuoi hay gap cua luoi, phan trang, bo loc DevExpress sang tieng Viet.
/// Chuoi chua dich se dung ban tieng Anh mac dinh. Them chuoi: ke thua va ghi de <see cref="Translations"/>.
/// </summary>
public class WtDxLocalizationService(IOptions<WebTemplateOptions> options) : DxLocalizationService, IDxLocalizationService
{
    protected virtual IReadOnlyDictionary<string, string> Translations => Vietnamese;

    string? IDxLocalizationService.GetString(string key)
    {
        var vietnamese = !string.Equals(options.Value.Language, "en", StringComparison.OrdinalIgnoreCase);
        if (vietnamese && key.StartsWith(Prefix, StringComparison.Ordinal) && Translations.TryGetValue(key[Prefix.Length..], out var text))
        {
            return text;
        }
        return base.GetString(key);
    }

    private const string Prefix = "DxBlazorStringId.";

    public static readonly IReadOnlyDictionary<string, string> Vietnamese = new Dictionary<string, string>
    {
        ["Grid_ClearFilterButton"] = "Xóa lọc",
        ["Grid_ColumnChooser_CommandColumn"] = "Cột thao tác",
        ["Grid_ColumnChooser_HeaderText"] = "Chọn cột hiển thị",
        ["Grid_ColumnChooser_SelectionColumn"] = "Cột chọn",
        ["Grid_CommandItem_DeselectAll"] = "Bỏ chọn tất cả",
        ["Grid_CommandItem_SelectAll"] = "Chọn tất cả",
        ["Grid_ContextMenu_AutoFitAll"] = "Tự căn độ rộng các cột",
        ["Grid_ContextMenu_ClearColumnSorting"] = "Bỏ sắp xếp",
        ["Grid_ContextMenu_ClearGrouping"] = "Bỏ nhóm",
        ["Grid_ContextMenu_CollapseAll"] = "Thu gọn tất cả",
        ["Grid_ContextMenu_ExpandAll"] = "Mở rộng tất cả",
        ["Grid_ContextMenu_GroupByColumn"] = "Nhóm theo cột này",
        ["Grid_ContextMenu_HideColumn"] = "Ẩn cột",
        ["Grid_ContextMenu_HideGroupPanel"] = "Ẩn vùng nhóm",
        ["Grid_ContextMenu_ShowColumnChooser"] = "Chọn cột hiển thị",
        ["Grid_ContextMenu_ShowFilterBuilder"] = "Lọc nâng cao",
        ["Grid_ContextMenu_ShowGroupPanel"] = "Hiện vùng nhóm",
        ["Grid_ContextMenu_SortColumnAscending"] = "Sắp xếp tăng dần",
        ["Grid_ContextMenu_SortColumnDescending"] = "Sắp xếp giảm dần",
        ["Grid_ContextMenu_UngroupColumn"] = "Bỏ nhóm cột này",
        ["Grid_DragHint_RowsCount"] = "{0} dòng",
        ["Grid_Editing_CancelButton"] = "Hủy",
        ["Grid_Editing_DeleteButton"] = "Xóa",
        ["Grid_Editing_DeleteConfirmationText"] = "Bạn có chắc muốn xóa dòng này?",
        ["Grid_Editing_EditButton"] = "Sửa",
        ["Grid_Editing_NewButton"] = "Thêm mới",
        ["Grid_Editing_NewItemRowText"] = "Bấm vào đây để thêm dòng mới",
        ["Grid_Editing_PopupEditFormHeaderText"] = "Cập nhật thông tin",
        ["Grid_Editing_SaveButton"] = "Lưu",
        ["Grid_Editing_ValuesWereDiscardedNotification"] = "Các thay đổi chưa lưu đã bị hủy.",
        ["Grid_EmptyDataRow"] = "Không có dữ liệu",
        ["Grid_EmptyHeaders"] = "Kéo cột vào đây...",
        ["Grid_Export_LoadingPanelText"] = "Đang xuất file...",
        ["Grid_FilterBuilder_ApplyButton"] = "Áp dụng",
        ["Grid_FilterBuilder_CancelButton"] = "Hủy",
        ["Grid_FilterBuilder_HeaderText"] = "Lọc nâng cao",
        ["Grid_FilterMenu_ApplyButton"] = "Áp dụng",
        ["Grid_FilterMenu_BlanksItem"] = "(Trống)",
        ["Grid_FilterMenu_CancelButton"] = "Hủy",
        ["Grid_FilterMenu_ClearButton"] = "Xóa lọc",
        ["Grid_FilterMenu_SearchBoxNullText"] = "Tìm giá trị...",
        ["Grid_FilterMenu_SelectAll"] = "Chọn tất cả",
        ["Grid_FilterMenu_UnsupportedColumnTypeNotification"] = "Cột này không hỗ trợ lọc",
        ["Grid_FilterMenu_ValuesHeaderText"] = "Giá trị lọc",
        ["Grid_FilterPanel_CreateFilter"] = "Tạo bộ lọc",
        ["Grid_FilterPanel_Operators_Not"] = "Không",
        ["Grid_GroupInterval_DateRange_BeyondNextMonth"] = "Sau tháng tới",
        ["Grid_GroupInterval_DateRange_EarlierThisMonth"] = "Đầu tháng này",
        ["Grid_GroupInterval_DateRange_LastMonth"] = "Tháng trước",
        ["Grid_GroupInterval_DateRange_LastWeek"] = "Tuần trước",
        ["Grid_GroupInterval_DateRange_LaterThisMonth"] = "Cuối tháng này",
        ["Grid_GroupInterval_DateRange_NextMonth"] = "Tháng tới",
        ["Grid_GroupInterval_DateRange_NextWeek"] = "Tuần tới",
        ["Grid_GroupInterval_DateRange_Older"] = "Cũ hơn",
        ["Grid_GroupInterval_DateRange_ThreeWeeksAgo"] = "Ba tuần trước",
        ["Grid_GroupInterval_DateRange_ThreeWeeksAway"] = "Ba tuần tới",
        ["Grid_GroupInterval_DateRange_Today"] = "Hôm nay",
        ["Grid_GroupInterval_DateRange_Tomorrow"] = "Ngày mai",
        ["Grid_GroupInterval_DateRange_TwoWeeksAgo"] = "Hai tuần trước",
        ["Grid_GroupInterval_DateRange_TwoWeeksAway"] = "Hai tuần tới",
        ["Grid_GroupInterval_DateRange_Yesterday"] = "Hôm qua",
        ["Grid_GroupPanel"] = "Kéo tiêu đề cột vào đây để nhóm theo cột đó",
        ["Grid_Loading"] = "Đang tải...",
        ["Grid_Pager_Summary_ItemFormat"] = "({0} dòng)",
        ["Grid_Pager_Summary_ItemsFormat"] = "({0} dòng)",
        ["Grid_Pager_Summary_PageLabel"] = "Trang",
        ["Grid_PageSizeSelector_AllRowsItem"] = "Tất cả",
        ["Grid_PageSizeSelector_Caption"] = "Số dòng/trang",
        ["Grid_SearchBoxNullText"] = "Tìm kiếm...",
        ["Grid_Summary_Average"] = "TB",
        ["Grid_Summary_AverageOfAnotherColumn"] = "TB của {0}",
        ["Grid_Summary_Count"] = "Số dòng",
        ["Grid_Summary_Max"] = "Lớn nhất",
        ["Grid_Summary_MaxOfAnotherColumn"] = "Lớn nhất của {0}",
        ["Grid_Summary_Min"] = "Nhỏ nhất",
        ["Grid_Summary_MinOfAnotherColumn"] = "Nhỏ nhất của {0}",
        ["Grid_Summary_Sum"] = "Tổng",
        ["Grid_Summary_SumOfAnotherColumn"] = "Tổng {0}",
        ["Pager_PageOfLabel"] = "/",
        ["SearchBox_NullText"] = "Tìm kiếm...",
    };
}
