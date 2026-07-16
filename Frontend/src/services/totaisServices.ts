import { API_BASE_URL } from './Api';
import type { RelatorioTotaisDto } from '../types/Totais';

// metodo GET para buscar o relatorio / gastos totais
export async function getTotais(): Promise<RelatorioTotaisDto> {
  const response = await fetch(`${API_BASE_URL}/pessoas/totais`);

  if (!response.ok) {
    throw new Error('Erro ao buscar totais.');
  }

  return response.json();
}