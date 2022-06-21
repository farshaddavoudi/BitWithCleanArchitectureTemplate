using System.ComponentModel.DataAnnotations;

namespace Template.Domain.Enums.Order;

public enum OrderOperation
{
    [Display(Name = "ثبت")]
    Add = 0,

    [Display(Name = "ویرایش")]
    Update = 1,

    [Display(Name = "حذف")]
    Delete = 2
}
