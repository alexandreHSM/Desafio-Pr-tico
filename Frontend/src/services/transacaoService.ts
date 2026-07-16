import { API_BASE_URL } from './Api';
import type { Transacao, CreateTransacaoDto } from '../types/Transacao';

// metodo GET para buscar as transacoes cadastradas
export async function getTransacoes(): Promise<Transacao[]> {
  const response = await fetch(`${API_BASE_URL}/transacoes`);

  if (!response.ok) {
    throw new Error('Erro ao buscar transações.');
  }

  return response.json();
}

// metodo POST para criar as transacoes 
export async function criarTransacao(dto: CreateTransacaoDto): Promise<Transacao> {
  const response = await fetch(`${API_BASE_URL}/transacoes`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(dto),
  });

  if (!response.ok) {

    const erroBody = await response.json().catch(() => null);
    const mensagem = erroBody?.mensagem ?? 'Erro ao criar transação.'; // ler o body para que a mensagem de erro seja quue a pessoa e menor de 18 anor
    throw new Error(mensagem); 
  }

  return response.json();
}