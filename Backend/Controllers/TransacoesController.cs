using Microsoft.AspNetCore.Mvc;
using Backend.DTOs;
using Backend.Services;
using Backend.Models;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransacoesController : ControllerBase
{
    private readonly TransacaoService _transacaoService;

    public TransacoesController(TransacaoService transacaoService)
    {
        _transacaoService = transacaoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Transacao>>> GetTransacoes()
    {
        var transacoes = await _transacaoService.GetTransacoesAsync();
        return Ok(transacoes);
    }

    [HttpPost]
    public async Task<ActionResult<Transacao>> PostTransacao([FromBody] CreateTransacaoDto dto)
    {
        var resultado = await _transacaoService.CriarTransacaoAsync(dto);

        if (!resultado.Sucesso)
        {
            return BadRequest(new { mensagem = resultado.MensagemErro });
        }

        return CreatedAtAction(nameof(GetTransacoes), new { id = resultado.Dado!.TransacaoId }, resultado.Dado);
    }
}