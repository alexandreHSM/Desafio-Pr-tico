namespace Backend.Models;

public class Pessoa
{
    public int PessoaId {get; set;}
    public string Nome {get; set;} = String.Empty; //evitar começar como Null
    public int Idade {get; set;}

    public bool MenorDeIdade => Idade < 18;

    public List<Transacao> Transacoes { get; set; } = new(); // Propriedade de navegação: representa a lista de transações desta pessoa.
}