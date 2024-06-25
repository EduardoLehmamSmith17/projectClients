using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using projeto_clientes.Models;

namespace projeto_clientes.Data.Mapeamento
{
    public class ContatoMap : IEntityTypeConfiguration<Contato>
    {
        public void Configure(EntityTypeBuilder<Contato> builder)
        {
            builder.ToTable("Contatos");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("IdContact").UseIdentityColumn();

            builder.Property(c => c.Email).IsRequired().HasColumnName("Email");

            builder.Property(c => c.Telefone).IsRequired().HasColumnName("Telephone");
        }
    }
}
