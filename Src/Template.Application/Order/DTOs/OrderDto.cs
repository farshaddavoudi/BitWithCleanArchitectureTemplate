using ATABit.Helper.Extensions;
using System.ComponentModel.DataAnnotations.Schema;
using Template.Domain.Enums.Order;

namespace Template.Application.Order.DTOs;

[ComplexType]
public record OrderDto
{
    public int Id { get; set; }

    public int OrderStatus { get; set; }
    public string? OrderStatusDisplay => ((OrderStatus)OrderStatus).ToDisplayName();

    public int CityId { get; set; }
    public string? CityDisplay => ((ReferenceCity)CityId).ToDisplayName();

    public int GroupCalendarId { get; set; }

    public DateTime GroupCalendarDate { get; set; } //Flattening
                                                    //public string GroupCalendarDateJalali => GroupCalendarDate.ToJalaliString();
                                                    // public string FoodDateDisplay => $"{GroupCalendarDateJalali} – {GroupCalendarDate.GetPersianWeekDayName()}";

    public DateTime? DateTime { get; set; }

    public int? FoodMenuId { get; set; }
    public string? FoodMenuName { get; set; } //Flattening

    public int? TbzFoodMenuId { get; set; }
    public string? TbzFoodMenuName { get; set; } //Flattening

    public int UserPersonnelCode { get; set; }

    public string? UserFullName { get; set; }

    public string? UserWorkLocation { get; set; }

    public string? UserUnit { get; set; }

    public string? Location { get; set; }

    public bool IsActive { get; set; } = false;

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    //public DateTime? CreatedDate { get; set; }
    //public string RegisteredAtJalali => CreatedDate.ToJalaliString(false);

    public DateTime? UpdatedDate { get; set; }

    public bool IsHalfServing { get; set; }

    public bool IsArchived { get; set; }
}