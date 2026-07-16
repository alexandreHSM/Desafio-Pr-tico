using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {        
    }

    public DbSet<Pessoa> Pessoas => Set<Pessoa>();//representando a tabela Pessoas no banco
    public DbSet<Transacao> Transacoes => Set<Transacao>();//representando a tabela Transacoes no banco


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pessoa>()
            .HasMany(p => p.Transacoes) //uma pessoa pode ter mais de uma transações
            .WithOne(t => t.Pessoa) //cada transação pertence a uma pessoa
            .HasForeignKey(t => t.PessoaId)
            .OnDelete(DeleteBehavior.Cascade); 
            //delet usando o "cascade", para excluir as transações 
            // que são dependentes do PessoaId que foi excluido

        base.OnModelCreating(modelBuilder);
    }

}