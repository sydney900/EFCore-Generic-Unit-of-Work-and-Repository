using BussinessCore.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenericUnitOfWork
{
    internal class ClientConfig : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.Property(x => x.Id).HasColumnName("ClientId");
            builder.Property(x => x.Timestamp).IsRowVersion();
        }
    }
}