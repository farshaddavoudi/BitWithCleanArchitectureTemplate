using System.Net.Http;
using Template.WebUI.Client.Contracts;
using Template.WebUI.Client.Models;

namespace Template.WebUI.Client.Implementations;

public class AppDataService : IAppDataService
{
    private AppData _appData;
    private HttpClient _httpClient;

    public AppDataService(AppData appData, HttpClient httpClient)
    {
        _appData = appData;
        _httpClient = httpClient;
    }

    //public async Task<List<LocationReadDto>> GetAllLocations()
    //{
    //    List<LocationReadDto> locations = new();

    //    if (_appData.Locations.Any() is false)
    //        locations = await _httpClient.Location().GetAllLocations();
    //    else
    //        locations = _appData.Locations;

    //    return locations;
    //}
}