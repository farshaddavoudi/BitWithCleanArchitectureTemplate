using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;

namespace ATA.Food.Client.Web.Components.TelerikComponents;

public class CustomDropDownListPopupSettings : DropDownListPopupSettings
{
    [Parameter] [Required] public bool HasDefaultText { get; set; }
    [Parameter] [Required] public int ItemsCount { get; set; }

    protected override Task OnParametersSetAsync()
    {
        Height = CalculatePopUpHeight();
        Class = "form-dropdown-popup";
        return base.OnParametersSetAsync();
    }

    private string CalculatePopUpHeight()
    {
        int paddingHeight = HasDefaultText ? 47 : 20;
        int itemHeight = 40;
        int maxHeight = 210;

        int itemsHeight = (ItemsCount == 0 ? 1 : ItemsCount) * itemHeight;
        int popUpHeight = itemsHeight + paddingHeight;

        int height = popUpHeight > maxHeight ? maxHeight : popUpHeight;

        return $"{height}px";
    }
}