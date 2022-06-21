using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Domain.Entities.Identity;

namespace Template.Infrastructure.Persistence.EF.Configuration;

public class IdentityEFConfiguration : IEntityTypeConfiguration<IdentityTokenEntity>
{
    public void Configure(EntityTypeBuilder<IdentityTokenEntity> builder)
    {
        // Table
        builder.ToTable("IdentityTokens");

        builder.HasKey(t => t.Id);

        // Props
        builder.Property(b => b.ClientName).IsRequired().HasMaxLength(100);
    }
}