using Microsoft.EntityFrameworkCore;
using projeto_clientes.Data.Mapeamento;
using projeto_clientes.Models;

namespace projeto_clientes.Data
{
    public class dbContext : DbContext
    {
        public DbSet<PessoaFisica> PessoasFisicas { get; set; }
        public DbSet<PessoaJuridica> PessoasJuridicas { get; set; }
        public DbSet<Contato> Contatos { get; set; }

        public dbContext(DbContextOptions<dbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PessoaFisicaMap());
            modelBuilder.ApplyConfiguration(new PessoaJuridicaMap());
            modelBuilder.ApplyConfiguration(new ContatoMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}
