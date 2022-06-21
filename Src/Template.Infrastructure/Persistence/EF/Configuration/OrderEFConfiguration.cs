using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Domain.Entities.Order;
using Template.Domain.Enums.Order;

namespace Template.Infrastructure.Persistence.EF.Configuration;

public class OrderEFConfiguration : IEntityTypeConfiguration<OrderEntity>
{
    public void Configure(EntityTypeBuilder<OrderEntity> builder)
    {
        // Table
        builder.ToTable("FoodOrder", DbSchemas.ATA);

        builder.HasKey(b => b.Id);

        // Props
        builder.Property(b => b.Id).HasColumnType("bigint");

        builder.Property(b => b.OrderStatus).HasDefaultValue(OrderStatus.Confirmed);

        builder.Property(b => b.GroupCalendarId).HasColumnType("bigint");

        builder.Property(b => b.UserPersonnelCode).HasColumnName("EmployeeCode").HasColumnType("bigint");

        builder.Property(b => b.UserFullName).HasMaxLength(20);
    }
}