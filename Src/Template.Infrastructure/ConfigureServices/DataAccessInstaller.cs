using ATABit.Data;
using ATABit.Data.Contracts;
using ATABit.Data.Interceptors;
using ATABit.Model.Data.Contracts;
using Bit.Core.Contracts;
using Bit.Data;
using Bit.Data.Contracts;
using Bit.Owin.Implementations;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Data.Common;
using System.Reflection;
using Template.Domain.Options;
using Template.Infrastructure.Common.Contracts;
using Template.Infrastructure.Persistence.EF;
using Template.Infrastructure.Persistence.EF.Repository;

namespace Template.Infrastructure.ConfigureServices;

public class DataAccessInstaller : IBitInstaller
{
    public void InstallServices(IServiceCollection services, IDependencyManager dependencyManager, AppSettings appSettings)
    {
        services.AddATABitDataAccess();

        dependencyManager.RegisterRepository(typeof(ATARepository<>).GetTypeInfo());
        dependencyManager.RegisterRepository(typeof(ATARepositoryReadOnly<>).GetTypeInfo());

        dependencyManager.RegisterGeneric(typeof(IATARepository<>).GetTypeInfo(), typeof(ATARepository<>).GetTypeInfo());
        dependencyManager.RegisterGeneric(typeof(IReadOnlyATARepository<>).GetTypeInfo(), typeof(ATARepositoryReadOnly<>).GetTypeInfo());

        dependencyManager.Register<IDbConnectionProvider, DefaultDbConnectionProvider<SqlConnection>>();

        dependencyManager.RegisterEfCoreDbContext<ATADbContext>((serviceProvider, optionsBuilder) =>
        {
            var connectionString = appSettings.Server!.ConnectionStringOptions!.AppDbConnectionString!;

            DbConnection connection = serviceProvider.GetRequiredService<IDbConnectionProvider>().GetDbConnection(connectionString, rollbackOnScopeStatusFailure: true);

            optionsBuilder.UseSqlServer(connection, sqlServerOptions =>
            {
                sqlServerOptions.CommandTimeout((int)TimeSpan.FromMinutes(1)
                    .TotalSeconds); //Default is 30 seconds
            });

            // Interceptors
            var entityAuditProvider = serviceProvider.GetRequiredService<IEntityAuditProvider>();
            optionsBuilder.AddInterceptors(new AuditSaveChangesInterceptor(entityAuditProvider));
            optionsBuilder.AddInterceptors(new FixSchemaInterceptor());

            // Show Detailed Errors
            if (AspNetCoreAppEnvironmentsProvider.Current.HostingEnvironment.IsDevelopment())
                optionsBuilder.EnableSensitiveDataLogging().EnableDetailedErrors();
        });
    }

}