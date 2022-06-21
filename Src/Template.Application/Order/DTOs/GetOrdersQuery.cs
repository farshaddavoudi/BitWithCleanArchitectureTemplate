using DNTPersianUtils.Core;
using System.ComponentModel.DataAnnotations.Schema;

namespace Template.Application.Order.DTOs;

[ComplexType]
public class GetOrdersQuery
{
    public int? CityId { get; set; } //Null means user my-foods report

    public bool IsUserMyFoodsReport => CityId is null;

    public string? SearchTerm { get; set; }

    [ValidPersianDateTime(ErrorMessage = "فرمت تاریخ شمسی معتبر نیست")]
    public string? JalaliDate { get; set; }

    public IEnumerable<string>? Locations { get; set; }

    public string? Unit { get; set; }
}