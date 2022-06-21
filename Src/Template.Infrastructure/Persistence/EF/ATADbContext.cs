using ATABit.Data.Extensions;
using Bit.Data.EntityFrameworkCore.Implementations;
using Microsoft.EntityFrameworkCore;
using Template.Domain.Entities;

namespace Template.Infrastructure.Persistence.EF;

public class ATADbContext : EfCoreDbContextBase
{
    public ATADbContext(DbContextOptions<ATADbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Auto Register all Entities
        modelBuilder.RegisterDbSets(typeof(ATAEntity).Assembly);

        modelBuilder.UseJsonDbFunctions();

        // Configure Views
        //modelBuilder.ConfigureDbView<FoodMenuViewEntity>(DbViews.FoodMenu, DbSchemas.ATA, hasKey: true);

        base.OnModelCreating(modelBuilder);

        // Seed Base Data to Database
        //modelBuilder.SeedDefaultRoles();

        // Ef Global Query Filters
        modelBuilder.RegisterIsArchivedGlobalQueryFilter();
        //modelBuilder.Entity<UserRolePairViewEntity>().HasQueryFilter(ur => ur.ApplicationName == AppMetadata.SSOAppName);

        modelBuilder.ConfigureDecimalPrecision();

        // Restrict Delete (in Hard delete scenarios)
        // Ef default is Cascade
        modelBuilder.SetRestrictAsDefaultDeleteBehavior();

        // Auto Register all Entity Configurations (Fluent-API)
        modelBuilder.ApplyConfigurations(typeof(ATADbContext).Assembly);
    }
}