using BussinessCore.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenericUnitOfWork
{
    internal class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");
            builder.Property(x => x.Id).HasColumnName("ProductId");
            builder.Property(x => x.Timestamp).IsRowVersion();
        }
    }
}