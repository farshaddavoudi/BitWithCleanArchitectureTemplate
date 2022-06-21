using Bit.Core;
using Bit.Owin;
using Bit.Owin.Implementations;

namespace Template.WebUI.API;

public class Program
{
    public static Task Main(string[] args)
    {
        AssemblyContainer.Current.Init();

        AssemblyContainer.Current.AddAppAssemblies(typeof(Program).Assembly);

        AspNetCoreAppEnvironmentsProvider.Current.Use();

        return CreateHostBuilder(args)
            .Build()
            .RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        BitWebHost.CreateWebHost<AppStartup>(args)
            .ConfigureWebHostDefaults(webHostBuilder =>
            {
#if Development
                    webHostBuilder.UseUrls("http://*:5000/", "https://*:5001/");
#endif
            });
}