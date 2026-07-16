
export interface Pessoa {
  pessoaId: number;
  nome: string;
  idade: number;
  menorDeIdade: boolean; // propriedade calculada que já vem pronta da API
}

export interface CreatePessoaDto {
  nome: string;
  idade: number;
}

