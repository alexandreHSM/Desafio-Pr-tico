namespace Backend.Models;

public class Transacao
{
    public enum TipoDeTransacao
    {
        Receita = 0,
        Despesa = 1 //O codigo manda apenas 0 ou 1 evitado conflito de nomes "diferentes": Despes, DESPESA, despesa.
    }

    public int TransacaoId {get; set;}
    public string Descricao {get; set;} = string.Empty; //evitar começar como Null
    public decimal Valor {get; set;}
    public TipoDeTransacao Tipo {get; set;}
    public int PessoaId {get; set;}// a "chave estrangeira" (guarda só o número)

    
    public Pessoa Pessoa { get; set; } = null!; // a "propriedade de navegação" (o objeto inteiro)

}