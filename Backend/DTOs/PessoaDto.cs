using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs;

public class CreatePessoaDto
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 100 caracteres.")] 
    public string Nome { get; set; } = string.Empty;

    [Range(0, 120, ErrorMessage = "A idade deve estar entre 0 e 120 anos.")]
    public int Idade { get; set; }
    // evita criar pessoa sem nome e com idade negativa gerando uma mensahem de erro
}
// Representa o total individual de cada pessoa
public class PessoaTotalDto
{
    public int PessoaId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal TotalReceitas { get; set; }
    public decimal TotalDespesas { get; set; }
    public decimal Saldo => TotalReceitas - TotalDespesas;
}

// Representa o relatório completo (lista de pessoas + total geral)
public class RelatorioTotaisDto
{
    public List<PessoaTotalDto> Pessoas { get; set; } = new();
    public decimal TotalGeralReceitas { get; set; }
    public decimal TotalGeralDespesas { get; set; }
    public decimal SaldoLiquidoGeral => TotalGeralReceitas - TotalGeralDespesas;
}