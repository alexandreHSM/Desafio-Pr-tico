using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Backend.DTOs;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransacoesController : ControllerBase
{
    private readonly AppDbContext _context;

    public TransacoesController(AppDbContext context)
    {
        _context = context;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<Transacao>>> GetTransacoes()
    {
        
        var transacoes = await _context.Transacoes
            .Include(t => t.Pessoa)// .Include(t => t.Pessoa) faz o "JOIN" para trazer os dados da pessoa associada à transação
            .ToListAsync();

        return Ok(transacoes);
    }

    
    [HttpPost]
    public async Task<ActionResult<Transacao>> PostTransacao([FromBody] CreateTransacaoDto dto)
    {
        // Verificar se a Pessoa existe no banco
        var pessoa = await _context.Pessoas.FindAsync(dto.PessoaId);
        if (pessoa == null)
        {
            return BadRequest(new { mensagem = "A pessoa informada não existe no cadastro." });
        }

        // Verificar se é menor de idadde, se for apenas despesa.
        if (pessoa.MenorDeIdade && dto.Tipo != Transacao.TipoDeTransacao.Despesa)
        {
            return BadRequest(new { mensagem = "Menores de 18 anos só podem registrar transações do tipo Despesa." });
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

        return CreatedAtAction(nameof(GetTransacoes), new { id = transacao.TransacaoId }, transacao);
    }
}