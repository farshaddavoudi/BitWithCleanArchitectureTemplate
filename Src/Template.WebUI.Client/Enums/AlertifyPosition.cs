using System.ComponentModel.DataAnnotations;

namespace Template.WebUI.Client.Enums
{
    public enum AlertifyPosition
    {
        [Display(Name = "top-right")]
        TopRight,

        [Display(Name = "top-center")]
        TopCenter,

        [Display(Name = "top-left")]
        TopLeft,

        [Display(Name = "bottom-right")]
        BottomRight,

        [Display(Name = "bottom-center")]
        BottomCenter,

        [Display(Name = "bottom-left")]
        BottomLeft
    }
}
