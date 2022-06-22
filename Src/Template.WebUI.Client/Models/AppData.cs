namespace Template.WebUI.Client.Models
{
    public class AppData
    {
        //private List<LocationReadDto>? _locations = new();
        //public List<LocationReadDto> Locations
        //{
        //    get => _locations ?? new List<LocationReadDto>();
        //    set
        //    {
        //        _locations = value;
        //        NotifyDataChanged();
        //    }
        //}

        public string? UserProfileImageUrl { get; set; }


        public event Action? OnChange;
        private void NotifyDataChanged() => OnChange?.Invoke();
    }
}