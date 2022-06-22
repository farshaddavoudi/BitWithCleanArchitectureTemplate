using System.ComponentModel.DataAnnotations;

namespace Template.WebUI.Client.Enums;

public enum OperationType
{
    Nothing,

    [Display(Name = "افزودن")]
    Add,

    [Display(Name = "فیلتر")]
    Edit,

    [Display(Name = "فیلتر")]
    Filter,

    Custom1,

    Custom2
}