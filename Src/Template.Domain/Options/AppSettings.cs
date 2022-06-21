namespace Template.Domain.Options;

public class AppSettings
{
    public ServerAppSettings? Server { get; set; }

    public ClientAppSettings? Client { get; set; }
}