using Microsoft.EntityFrameworkCore;
using ProjetoBancoCP2.Models;

namespace ProjetoBancoCP2.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Agencia> Agencias { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<PessoaFisica> PessoasFisicas { get; set; }
        public DbSet<PessoaJuridica> PessoasJuridicas { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Emprestimo> Emprestimos { get; set; }
        public DbSet<Contratacao> Contratacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Herança Cliente → discriminator
            modelBuilder.Entity<Cliente>()
                .HasDiscriminator<string>("TIPO_CLIENTE")
                .HasValue<PessoaFisica>("PF")
                .HasValue<PessoaJuridica>("PJ");

            // Herança Produto → discriminator
            modelBuilder.Entity<Produto>()
                .HasDiscriminator<string>("TIPO_PRODUTO")
                .HasValue<Emprestimo>("EMPRESTIMO");

            // Agencia → Cliente (1 para N)
            modelBuilder.Entity<Cliente>()
                .HasOne(c => c.Agencia)
                .WithMany()
                .HasForeignKey(c => c.IdAgencia)
                .OnDelete(DeleteBehavior.Restrict);

            // Cliente → Contratacao (1 para N)
            modelBuilder.Entity<Contratacao>()
                .HasOne(c => c.Cliente)
                .WithMany()
                .HasForeignKey(c => c.IdCliente)
                .OnDelete(DeleteBehavior.Restrict);

            // Produto → Contratacao (1 para N)
            modelBuilder.Entity<Contratacao>()
                .HasOne(c => c.Produto)
                .WithMany()
                .HasForeignKey(c => c.IdProduto)
                .OnDelete(DeleteBehavior.Restrict);

            // Precisão dos campos decimais do Emprestimo
            modelBuilder.Entity<Emprestimo>()
                .Property(e => e.ValorSolicitado)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Emprestimo>()
                .Property(e => e.TaxaJuros)
                .HasPrecision(5, 2); // ex: 99.99%

            modelBuilder.Entity<Emprestimo>()
                .Property(e => e.ValorParcela)
                .HasPrecision(18, 2);
        }
    }
}