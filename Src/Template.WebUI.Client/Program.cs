using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Template.Application.Common.Contracts;
using Template.Domain.Options;
using Template.WebUI.Client.Implementations;
using Template.WebUI.Client.Models;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace Template.WebUI.Client;

public class Program
{
    public static Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<HeadOutlet>("head::after");

        var clientAppSettings = builder.Configuration.GetSection(nameof(ClientAppSettings)).Get<ClientAppSettings>();
        clientAppSettings.URLOptions!.AppURL = builder.HostEnvironment.BaseAddress;

        // Host client
        builder.Services.AddHttpClient<HostClient>(httpClient => httpClient.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
            .AddHttpMessageHandler<AppHttpExceptionHandler>();

        // Configure Dependencies with Service Installers
        var installers = new[] { Assembly.GetExecutingAssembly() }.SelectMany(a => a.GetExportedTypes())
            .Where(c => c.IsClass && !c.IsAbstract && c.IsPublic && typeof(IInstaller).IsAssignableFrom(c))
            .Select(Activator.CreateInstance).Cast<IInstaller>().ToList();
        installers.ForEach(i => i.InstallServices(builder.Services, new AppSettings { Client = clientAppSettings }));

        builder.UseLoadingBar();

        return builder
            .Build()
            .RunAsync();
    }
}