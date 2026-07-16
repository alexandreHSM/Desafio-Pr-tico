using Microsoft.AspNetCore.Mvc;
using Backend.DTOs;
using Backend.Services;
using Backend.Models;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]// definindo a rota para api/pessoas ( o [controller], serve para que a rota tenha o nome do arquivo)
public class PessoasController : ControllerBase
{
    private readonly PessoaService _pessoaService; 

    //inserindo o services 
    public PessoasController(PessoaService pessoaService)
    {
        _pessoaService = pessoaService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Pessoa>>> GetPessoas()
    {
        var pessoas = await _pessoaService.GetPessoasAsync();
        return Ok(pessoas);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Pessoa>> GetPessoaById(int id)
    {
        var pessoa = await _pessoaService.GetPessoaPorIdAsync(id);

        if (pessoa == null)
        {
            return NotFound(new { mensagem = "Pessoa não encontrada." });
        }

        return Ok(pessoa);
    }

    [HttpPost]
    public async Task<ActionResult<Pessoa>> PostPessoa([FromBody] CreatePessoaDto dto)
    {
        var pessoa = await _pessoaService.CriarPessoaAsync(dto);
        return CreatedAtAction(nameof(GetPessoaById), new { id = pessoa.PessoaId }, pessoa);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePessoa(int id)
    {
        var deletou = await _pessoaService.DeletarPessoaAsync(id);

        if (!deletou)
        {
            return NotFound(new { mensagem = "Pessoa não encontrada." });
        }

        return NoContent();
    }

    [HttpGet("totais")]
    public async Task<ActionResult<RelatorioTotaisDto>> GetTotais()
    {
        var relatorio = await _pessoaService.GetTotaisAsync();
        return Ok(relatorio);
    }
}