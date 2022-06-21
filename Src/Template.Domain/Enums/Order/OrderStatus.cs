using System.ComponentModel.DataAnnotations;

namespace Template.Domain.Enums.Order;

public enum OrderStatus
{
    [Display(Name = "ثبت شده")]
    Confirmed = 0,

    [Display(Name = "حذف مستقیم توسط کاربر")]
    DeletedDirectlyByUser = 1,

    [Display(Name = "حذف بدلیل ویرایش غذا توسط کاربر")]
    DeletedBecauseOfFoodUpdateByUser = 2,

    [Display(Name = "حذف بدلیل تغییر برنامه غذایی")]
    DeletedBecauseOfGroupDeleteByCatering = 3,

    [Display(Name = "حذف بدلیل تغییر برنامه غذایی")]
    DeletedBecauseOfCalendarFoodChangeByCatering = 4
}

public static class OrderStatusExtensions
{
    public static bool IsDelete(this OrderStatus orderStatus)
    {
        return orderStatus != OrderStatus.Confirmed;
    }
}