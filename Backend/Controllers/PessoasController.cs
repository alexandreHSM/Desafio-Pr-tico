using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Backend.DTOs;

namespace Backend.Controllers;

[ApiController] //indicar que a classe responde a requisições de API
[Route("api/[controller]")]// definir a rota  padrão
public class PessoasController : ControllerBase
{
    private readonly AppDbContext _context;

    public PessoasController(AppDbContext context)
    {
        _context = context;
    }

   [HttpGet] // Rota: GET /api/pessoas/5
    public async Task<ActionResult<Pessoa>> GetPessoa(int id)//indica que o metodo é assinc.
    {
        var pessoas = await _context.Pessoas.ToListAsync();//busca todas as pessoas de forma assincrona
        

        return Ok(pessoas); 
    }

    [HttpPost]
    public async Task<ActionResult<Pessoa>> PostPessoa([FromBody] CreatePessoaDto dto)
    {
        // Aqui é onde a "tradução" acontece: pegamos só os dados confiáveis do DTO
        // e montamos a entidade real que vai pro banco.
        var pessoa = new Pessoa
        {
            Nome = dto.Nome,
            Idade = dto.Idade
        };

        _context.Pessoas.Add(pessoa);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPessoa), new { id = pessoa.PessoaId }, pessoa);
    }
    [HttpDelete("{id}")] // definindo o metodo DELETE /api/pessoas/id
    public async Task<IActionResult> DeletePessoa(int id)
    { 
        var pessoa = await _context.Pessoas.FindAsync(id); // Procura a pessoa pelo ID informado
        

        if (pessoa == null)// Se não encontrar, retorna Status 404 Not Found
        {
            return NotFound(new { mensagem = "Pessoa não encontrada." });
        }

        _context.Pessoas.Remove(pessoa);// Remove a pessoa do contexto
        

        await _context.SaveChangesAsync();// Salva as alterações 

        return NoContent();
    }
    

    [HttpGet("totais")] // Define a rota: GET /api/pessoas/totais
    public async Task<ActionResult<RelatorioTotaisDto>> GetTotais()
    {
        // 1. Buscamos as pessoas incluindo suas transações do banco de dados
        var pessoasComTransacoes = await _context.Pessoas
            .Include(p => p.Transacoes)
            .ToListAsync();

        var relatorio = new RelatorioTotaisDto();

        // 2. Calculamos os totais de cada pessoa individualmente
        foreach (var pessoa in pessoasComTransacoes)
        {
            // Soma os valores onde o Tipo é Receita (0)
            var receitas = pessoa.Transacoes
                .Where(t => t.Tipo == Transacao.TipoDeTransacao.Receita)
                .Sum(t => t.Valor);

            // Soma os valores onde o Tipo é Despesa (1)
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

        // 3. Calculamos os totais gerais consolidando a lista acima
        relatorio.TotalGeralReceitas = relatorio.Pessoas.Sum(p => p.TotalReceitas);
        relatorio.TotalGeralDespesas = relatorio.Pessoas.Sum(p => p.TotalDespesas);

        return Ok(relatorio);
    }

}