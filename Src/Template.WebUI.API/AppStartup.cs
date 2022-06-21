using Bit.Core.Contracts;
using Bit.OData.Contracts;
using Bit.Owin;
using Template.WebUI.API;

[assembly: ODataModule("TemplateRoute")]
[assembly: AppModule(typeof(AppModules))]

namespace Template.WebUI.API;

public class AppStartup : AspNetCoreAppStartup
{
}