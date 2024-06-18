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

            builder.Property(c => c.Id)
                .HasColumnName("Id")
                .UseIdentityColumn();

            builder.Property(c => c.Email)
                .IsRequired();

            builder.Property(c => c.Telefone)
                .IsRequired();

            builder.Property(c => c.PessoaFisicaId);

            builder.Property(c => c.PessoaJuridicaId);
        }
    }
}
