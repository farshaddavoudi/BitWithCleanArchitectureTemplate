using NetCore.AutoRegisterDi;
using System.Reflection;
using Template.Application.Common.Contracts;
using Template.Domain.Options;

namespace Template.WebUI.Client.ConfigureServices
{
    public class AutoRegisterDiInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, AppSettings appSettings)
        {
            var webAssembly = Assembly.GetExecutingAssembly();
            var clientWebServiceAssembly = typeof(ClientAppSettings).Assembly;

            var assembliesToScan = new[] { webAssembly, clientWebServiceAssembly };

            #region Generic Type Dependencies
            //services.AddScoped(typeof(IRepository<>), typeof(EfCoreRepositoryBase<,>));
            #endregion


            #region Register DIs By Name
            services.RegisterAssemblyPublicNonGenericClasses(assembliesToScan)
                .Where(c => c.Name.EndsWith("Service"))
                .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);
            #endregion 

        }
    }
}