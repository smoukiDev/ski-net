using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.ProductId)
                .IsRequired();
            builder.Property(p => p.ProductName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(500);
            builder.Property(p => p.PictureUrl)
                .IsRequired();
            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)");
            builder.HasOne(b => b.ProductBrand)
                .WithMany()
                .HasForeignKey(p => p.ProductBrandId);
            builder.HasOne(b => b.ProductType)
                .WithMany()
                .HasForeignKey(p => p.ProductTypeId);
        }
    }
}