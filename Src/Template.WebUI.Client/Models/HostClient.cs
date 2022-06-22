using System.Net.Http;

namespace Template.WebUI.Client.Models
{
    public class HostClient
    {
        public HttpClient Client { get; }

        public HostClient(HttpClient client)
        {
            Client = client;
        }
    }
}
