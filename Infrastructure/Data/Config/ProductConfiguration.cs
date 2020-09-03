using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Description).IsRequired().HasMaxLength(180);
            builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
            builder.Property(p => p.PictureUrl).IsRequired();

            //Each product has a single brand that is related to. And we say .WithMany() because each brand can be associated with many products
            //EntityFramework already did this for us. We saw this in the original migrations but we can do this here as well and we can be specific about what our entity or our migration is gonna look like
            builder.HasOne(b => b.ProductBrand).WithMany().HasForeignKey(p => p.ProductBrandId); 
            builder.HasOne(t => t.ProductType).WithMany().HasForeignKey(p => p.ProductTypeId); 

            //After we finish here, we need to tell our StoreContext that there is configurations to look for
            //So what we do in the StoreContext class is that we override one of the methods inside DbContext
        }
    }
}