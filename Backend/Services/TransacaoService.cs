using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Backend.DTOs;

namespace Backend.Services;

// O service fala com o _context e AppDbContext, e se comunica com o controller/ mesma logica de pessoas
public class ResultadoOperacao<T>
{
    public bool Sucesso { get; set; }
    public string? MensagemErro { get; set; }
    public T? Dado { get; set; }

    public static ResultadoOperacao<T> Ok(T dado) =>
        new() { Sucesso = true, Dado = dado };

    public static ResultadoOperacao<T> Falha(string mensagem) =>
        new() { Sucesso = false, MensagemErro = mensagem };
}

public class TransacaoService
{
    private readonly AppDbContext _context;

    public TransacaoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Transacao>> GetTransacoesAsync()
    {
        return await _context.Transacoes
            .Include(t => t.Pessoa)
            .ToListAsync();
    }

    public async Task<ResultadoOperacao<Transacao>> CriarTransacaoAsync(CreateTransacaoDto dto)
    {
        var pessoa = await _context.Pessoas.FindAsync(dto.PessoaId);
        if (pessoa == null)
        {
            return ResultadoOperacao<Transacao>.Falha("A pessoa informada não existe no cadastro.");
        }

        if (pessoa.MenorDeIdade && dto.Tipo != Transacao.TipoDeTransacao.Despesa)
        {
            return ResultadoOperacao<Transacao>.Falha(
                "Menores de 18 anos só podem registrar transações do tipo Despesa.");
        }

        var transacao = new Transacao
        {
            Descricao = dto.Descricao,
            Valor = dto.Valor,
            Tipo = dto.Tipo,
            PessoaId = dto.PessoaId
        };

        _context.Transacoes.Add(transacao);
        await _context.SaveChangesAsync();

        return ResultadoOperacao<Transacao>.Ok(transacao);
    }
}