import { API_BASE_URL } from './Api';
import type { Pessoa, CreatePessoaDto } from '../types/Pessoa';

//metodo GET para buscar as pessoas cadastradas
export async function getPessoas(): Promise<Pessoa[]> {
    const response = await fetch(`${API_BASE_URL}/pessoas`); 
    
    if (!response.ok){ //caso tenha erro ele vai acionar 
        throw new Error('Erro ao buscar pessoas.');
    }

    return response.json();
}

//metodo post para cadastrar/criar nova pessoa
export async function criarPessoa(dto: CreatePessoaDto): Promise<Pessoa> {
  const response = await fetch(`${API_BASE_URL}/pessoas`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json', // avisa a API que o corpo é JSON
    },
    body: JSON.stringify(dto), // transforma o objeto JS em texto JSON
  });

  if (!response.ok) {
    throw new Error('Erro ao criar pessoa.');
  }

  return response.json();
}

// metodo delet para remover uma pessoa
export async function deletarPessoa(id: number): Promise<void> {
  const response = await fetch(`${API_BASE_URL}/pessoas/${id}`, {
    method: 'DELETE',
  });

  if (!response.ok) {
    throw new Error('Erro ao deletar pessoa.');
  }
  // Repare que não tem "return response.json()" aqui — seu DeletePessoa no C#
  // retorna NoContent() (204), que não tem corpo de resposta pra converter.
}

