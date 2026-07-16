using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Backend.DTOs;

namespace Backend.Services;


// O service fala com o _context e AppDbContext, e se comunica com o controller
public class PessoaService
{
    private readonly AppDbContext _context;

    public PessoaService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Pessoa>> GetPessoasAsync()
    {
        return await _context.Pessoas.ToListAsync();
    }

    public async Task<Pessoa?> GetPessoaPorIdAsync(int id)
    {
        return await _context.Pessoas.FindAsync(id);
    }

    public async Task<Pessoa> CriarPessoaAsync(CreatePessoaDto dto)
    {
        var pessoa = new Pessoa
        {
            Nome = dto.Nome,
            Idade = dto.Idade
        };

        _context.Pessoas.Add(pessoa);
        await _context.SaveChangesAsync();

        return pessoa;
    }

    public async Task<bool> DeletarPessoaAsync(int id)
    {
        var pessoa = await _context.Pessoas.FindAsync(id);

        if (pessoa == null)
        {
            return false;
        }

        _context.Pessoas.Remove(pessoa);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<RelatorioTotaisDto> GetTotaisAsync()
    {
        var pessoasComTransacoes = await _context.Pessoas
            .Include(p => p.Transacoes)
            .ToListAsync();

        var relatorio = new RelatorioTotaisDto();

        foreach (var pessoa in pessoasComTransacoes)
        {
            var receitas = pessoa.Transacoes
                .Where(t => t.Tipo == Transacao.TipoDeTransacao.Receita)
                .Sum(t => t.Valor);

            var despesas = pessoa.Transacoes
                .Where(t => t.Tipo == Transacao.TipoDeTransacao.Despesa)
                .Sum(t => t.Valor);

            relatorio.Pessoas.Add(new PessoaTotalDto
            {
                PessoaId = pessoa.PessoaId,
                Nome = pessoa.Nome,
                TotalReceitas = receitas,
                TotalDespesas = despesas
            });
        }

        relatorio.TotalGeralReceitas = relatorio.Pessoas.Sum(p => p.TotalReceitas);
        relatorio.TotalGeralDespesas = relatorio.Pessoas.Sum(p => p.TotalDespesas);

        return relatorio;
    }
}