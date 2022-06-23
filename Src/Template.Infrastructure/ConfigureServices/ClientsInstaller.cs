using Bit.Core.Contracts;
using Template.Domain.Options;
using Template.Infrastructure.Common.Contracts;

// ReSharper disable once ObjectCreationAsStatement

namespace Template.Infrastructure.ConfigureServices;

public class ClientsInstaller : IBitInstaller
{
    public void InstallServices(IServiceCollection services, IDependencyManager dependencyManager, AppSettings appSettings)
    {
        //// SSO Client
        //services.AddHttpClient<SSOClient>(client =>
        //{
        //    client.BaseAddress = new Uri(serverAppSettings.URLOptions!.SSOClient!);
        //});

        //// HR Client
        //services.AddHttpClient<HRClient>(client =>
        //{
        //    string hrClientBaseUrl = AspNetCoreAppEnvironmentsProvider.Current.HostingEnvironment.IsDevelopment()
        //        ? "https://dummy.com/" //Doesn't matter
        //        : serverAppSettings.URLOptions!.HRClient!;

        //    client.BaseAddress = new Uri(hrClientBaseUrl);
        //    client.DefaultRequestHeaders.Add("api_key", serverAppSettings.HRClientOptions?.APIKey);
        //});
    }
}