using Microsoft.AspNetCore.Components;

namespace Template.WebUI.Client.Components
{
    public partial class HeaderAddOrEdit
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }
    }
}