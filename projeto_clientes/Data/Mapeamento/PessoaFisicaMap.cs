using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using projeto_clientes.Models;

namespace projeto_clientes.Data.Mapeamento
{
    public class PessoaFisicaMap : IEntityTypeConfiguration<PessoaFisica>
    {
        public void Configure(EntityTypeBuilder<PessoaFisica> builder)
        {
            builder.ToTable("PessoasFisicas");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasColumnName("Id")
                .UseIdentityColumn();

            builder.Property(p => p.NomeCompleto)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.CPF)
                .IsRequired()
                .HasMaxLength(11);

            builder.Property(p => p.DataDeNascimento);

            builder.Property(p => p.Endereco);
        }
    }
}
