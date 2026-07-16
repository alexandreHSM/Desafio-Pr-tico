import { useState, useEffect } from 'react';
import type { Pessoa, CreatePessoaDto } from '../types/Pessoa';
import type { PessoaTotalDto } from '../types/Totais';
import { getPessoas, criarPessoa, deletarPessoa } from '../services/pessoaService';
import { getTotais } from '../services/totaisServices';
import { Modal } from '../components/Modal';
import './PessoaManager.css';

export function PessoaManager() {
  const [pessoas, setPessoas] = useState<Pessoa[]>([]);
  const [nome, setNome] = useState('');
  const [idade, setIdade] = useState('');
  const [erro, setErro] = useState<string | null>(null);
  const [carregando, setCarregando] = useState(true);

  // Guarda o total da pessoa que está sendo exibida no modal.
  // null = modal fechado.
  const [totalSelecionado, setTotalSelecionado] = useState<PessoaTotalDto | null>(null);

  useEffect(() => {
    carregarPessoas();
  }, []);

  async function carregarPessoas() {
    try {
      setCarregando(true);
      const dados = await getPessoas();
      setPessoas(dados);
    } catch {
      setErro('Não foi possível carregar as pessoas.');
    } finally {
      setCarregando(false);
    }
  }

  async function handleCadastrar(e: React.FormEvent) {
    e.preventDefault();
    setErro(null);

    const dto: CreatePessoaDto = { nome, idade: Number(idade) };

    try {
      await criarPessoa(dto);
      setNome('');
      setIdade('');
      await carregarPessoas();
    } catch {
      setErro('Erro ao cadastrar pessoa.');
    }
  }

  async function handleDeletar(id: number) {
    try {
      await deletarPessoa(id);
      await carregarPessoas();
    } catch {
      setErro('Erro ao deletar pessoa.');
    }
  }

  // Busca o relatório completo de totais e filtra só a pessoa clicada.
  async function handleVerTotal(pessoaId: number) {
    try {
      const relatorio = await getTotais();
      const totalDaPessoa = relatorio.pessoas.find((p) => p.pessoaId === pessoaId);

      if (totalDaPessoa) {
        setTotalSelecionado(totalDaPessoa);
      } else {
        setErro('Não foi possível encontrar o total dessa pessoa.');
      }
    } catch {
      setErro('Erro ao buscar total da pessoa.');
    }
  }

  return (
    <div className="tela-container">
      <h1>Pessoas</h1>

      <form className="form-row" onSubmit={handleCadastrar}>
        <input
          type="text"
          placeholder="Nome"
          value={nome}
          onChange={(e) => setNome(e.target.value)}
          required
        />
        <input
          type="number"
          placeholder="Idade"
          value={idade}
          onChange={(e) => setIdade(e.target.value)}
          required
        />
        <button type="submit" className="btn-cadastrar">Cadastrar</button>
      </form>

      {erro && <p className="erro-msg">{erro}</p>}

      <div className="tabela-container">
        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Nome</th>
              <th>Idade</th>
              <th style={{ textAlign: 'right' }}>Ações</th>
            </tr>
          </thead>
          <tbody>
            {carregando ? (
              <tr><td colSpan={3}>Carregando...</td></tr>
            ) : (
              pessoas.map((pessoa) => (
                <tr key={pessoa.pessoaId}>
                  <td>{pessoa.pessoaId}</td>
                  <td>{pessoa.nome}</td>
                  <td>{pessoa.idade} anos</td>
                  <td className="acoes-cell">
                    <button
                      className="btn-icon btn-ver-total"
                      onClick={() => handleVerTotal(pessoa.pessoaId)}
                      title="Ver total"
                    >
                      $
                    </button>
                    <button
                      className="btn-icon btn-excluir"
                      onClick={() => handleDeletar(pessoa.pessoaId)}
                      title="Excluir"
                    >
                      ×
                    </button>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      {totalSelecionado && (
        <Modal titulo={totalSelecionado.nome} onFechar={() => setTotalSelecionado(null)}>
          <p>Receitas: R$ {totalSelecionado.totalReceitas.toFixed(2)}</p>
          <p>Despesas: R$ {totalSelecionado.totalDespesas.toFixed(2)}</p>
          <p><strong>Saldo: R$ {totalSelecionado.saldo.toFixed(2)}</strong></p>
        </Modal>
      )}
    </div>
  );
}