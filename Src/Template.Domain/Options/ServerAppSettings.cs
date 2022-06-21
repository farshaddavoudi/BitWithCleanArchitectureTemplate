namespace Template.Domain.Options;

public class ServerAppSettings
{
    public ConnectionStringOptions? ConnectionStringOptions { get; set; }

    public URLOptions? URLOptions { get; set; }

    public AuthOptions? AuthOptions { get; set; }
}