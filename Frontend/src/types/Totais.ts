
export interface PessoaTotalDto {
  pessoaId: number;
  nome: string;
  totalReceitas: number;
  totalDespesas: number;
  saldo: number; 
}

export interface RelatorioTotaisDto {
  pessoas: PessoaTotalDto[];
  totalGeralReceitas: number;
  totalGeralDespesas: number;
  saldoLiquidoGeral: number;
}