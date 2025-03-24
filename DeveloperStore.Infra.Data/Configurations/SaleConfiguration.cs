using DeveloperStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Infra.Data.Configurations
{
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable("Sale");

            builder.HasKey(s => s.Id);

            builder.Property(p => p.Id)
                .HasColumnType("char(36)")
                .IsRequired()
                .IsFixedLength();

            builder.Property(s => s.SaleDate)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(s => s.TotalSaleAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(s => s.IsCancelled)
                .HasDefaultValue(false)
                .IsRequired();

            builder.HasOne(s => s.User)
                .WithMany(c => c.Sales)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
