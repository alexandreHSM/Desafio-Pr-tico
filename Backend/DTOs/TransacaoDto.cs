using System.ComponentModel.DataAnnotations;
using Backend.Models;

namespace Backend.DTOs;

public class CreateTransacaoDto
{

    [Required(ErrorMessage = "A descrição é obrigatória.")] //impedi que o campo seja vazio
    [StringLength(200, MinimumLength = 2, ErrorMessage = "A descrição deve ter entre 2 e 200 caracteres.")] //evita criar transação sem informar a sua descrição
    public string Descricao { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")] // evita transação com valores negativos, o range evita que o valor seja menos que 0.01
    public decimal Valor { get; set; }

    public Transacao.TipoDeTransacao Tipo { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "É necessário informar uma pessoa válida.")] //evita transação sem uma pessoa vinculada o range impede que o id seja menor que 1
    public int PessoaId { get; set; }

    //todos retornando erro se tiver faltando ou algo errado
}