import type { Pessoa } from "./Pessoa.ts";

export type TipoDeTransacao = 0 | 1;

export const TipoDeTransacaoEnum = {
  Receita: 0,
  Despesa: 1,
} as const;

export interface Transacao {
  transacaoId: number;
  descricao: string;
  valor: number;
  tipo: TipoDeTransacao;
  pessoaId: number;
  pessoa: Pessoa; // vem preenchida por causa do .Include(t => t.Pessoa) no backend
}

export interface CreateTransacaoDto {
  descricao: string;
  valor: number;
  tipo: TipoDeTransacao;
  pessoaId: number;
}