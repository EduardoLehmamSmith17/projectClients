using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using projeto_clientes.Models;

namespace projeto_clientes.Data.Mapeamento
{
    public class PessoaJuridicaMap : IEntityTypeConfiguration<PessoaJuridica>
    {
        public void Configure(EntityTypeBuilder<PessoaJuridica> builder)
        {
            builder.ToTable("PessoasJuridicas");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).HasColumnName("Id").UseIdentityColumn();

            builder.Property(p => p.RazaoSocial).IsRequired().HasColumnName("RazaoSocial").HasMaxLength(200);

            builder.Property(p => p.CNPJ).IsRequired().HasColumnName("CNPJ").HasMaxLength(14);

            builder.Property(p => p.NomeFantasia).HasColumnName("NomeFantasia").HasMaxLength(200);

            builder.Property(p => p.Endereco).HasColumnName("Endereço");
        }
    }
}
