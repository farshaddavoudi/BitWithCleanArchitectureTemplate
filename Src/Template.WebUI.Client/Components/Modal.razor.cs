using Microsoft.AspNetCore.Components;

namespace Template.WebUI.Client.Components
{
    public partial class Modal
    {
        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public string Text { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        public bool Visibility { get; set; }

        public void ToggleVisibility()
        {
            Visibility = !Visibility;
        }

        public void Show()
        {
            Visibility = true;
        }

        public void Hide()
        {
            Visibility = false;
        }
    }
}