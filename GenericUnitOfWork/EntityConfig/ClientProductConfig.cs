using BussinessCore.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace GenericUnitOfWork
{
    public class ClientProductConfig : IEntityTypeConfiguration<ClientProduct>
    {
        public void Configure(EntityTypeBuilder<ClientProduct> builder)
        {
            builder.HasKey(x => new { x.ClientId, x.ProductId });

            //builder.HasOne(x => x.Client).WithMany(c => c.ProductsClientProducts).HasForeignKey(cp => cp.ClientId);

            //builder.HasOne(x => x.Product).WithMany(x => x.ClientsClientProducts).HasForeignKey(pc => pc.ProductId);
        }
    }
}
