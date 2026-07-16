import { useState, useEffect } from 'react';
import type { Pessoa } from '../types/Pessoa';
import type { Transacao, CreateTransacaoDto } from '../types/Transacao';
import { TipoDeTransacaoEnum } from '../types/Transacao';
import { getPessoas } from '../services/pessoaService';
import { getTransacoes, criarTransacao } from '../services/transacaoService';
import './PessoaManager.css'; 
import './TransacaoManager.css';

export function TransacaoManager() {
  const [transacoes, setTransacoes] = useState<Transacao[]>([]);
  const [pessoas, setPessoas] = useState<Pessoa[]>([]);
  const [descricao, setDescricao] = useState('');
  const [valor, setValor] = useState('');
  const [tipo, setTipo] = useState<number>(TipoDeTransacaoEnum.Receita);
  const [pessoaId, setPessoaId] = useState('');
  const [erro, setErro] = useState<string | null>(null);
  const [carregando, setCarregando] = useState(true);

  useEffect(() => {
    carregarDados();
  }, []);

  async function carregarDados() {
    try {
      setCarregando(true);
      const [dadosTransacoes, dadosPessoas] = await Promise.all([
        getTransacoes(),
        getPessoas(),
      ]);
      setTransacoes(dadosTransacoes);
      setPessoas(dadosPessoas);
    } catch {
      setErro('Não foi possível carregar os dados.');
    } finally {
      setCarregando(false);
    }
  }

  async function handleCadastrar(e: React.FormEvent) {
    e.preventDefault();
    setErro(null);

    const dto: CreateTransacaoDto = {
      descricao,
      valor: Number(valor),
      tipo: tipo as 0 | 1,
      pessoaId: Number(pessoaId),
    };

    try {
      await criarTransacao(dto);
      setDescricao('');
      setValor('');
      await carregarDados();
    } catch (err) {
      setErro(err instanceof Error ? err.message : 'Erro ao cadastrar transação.');
    }
  }

  return (
    <div className="tela-container">
      <h1>Transações</h1>

      <form className="form-row" onSubmit={handleCadastrar}>
        <input
          type="text"
          placeholder="Descrição"
          value={descricao}
          onChange={(e) => setDescricao(e.target.value)}
          required
        />
        <input
          type="number"
          placeholder="Valor"
          value={valor}
          onChange={(e) => setValor(e.target.value)}
          required
        />
        <select value={tipo} onChange={(e) => setTipo(Number(e.target.value))}>
          <option value={TipoDeTransacaoEnum.Receita}>Receita</option>
          <option value={TipoDeTransacaoEnum.Despesa}>Despesa</option>
        </select>
        <select value={pessoaId} onChange={(e) => setPessoaId(e.target.value)} required>
          <option value="">Selecione a pessoa</option>
          {pessoas.map((pessoa) => (
            <option key={pessoa.pessoaId} value={pessoa.pessoaId}>
              {pessoa.nome}
            </option>
          ))}
        </select>
        <button type="submit" className="btn-cadastrar">Cadastrar</button>
      </form>

      {erro && <p className="erro-msg">{erro}</p>}

      <div className="tabela-container">
        <table>
          <thead>
            <tr>
              <th>Descrição</th>
              <th>Valor</th>
              <th>Tipo</th>
              <th>Pessoa</th>
            </tr>
          </thead>
          <tbody>
            {carregando ? (
              <tr><td colSpan={4}>Carregando...</td></tr>
            ) : (
              transacoes.map((t) => (
                <tr key={t.transacaoId}>
                  <td>{t.descricao}</td>
                  <td>R$ {t.valor.toFixed(2)}</td>
                  <td>
                    <span className={t.tipo === TipoDeTransacaoEnum.Receita ? 'badge badge-receita' : 'badge badge-despesa'}>
                      {t.tipo === TipoDeTransacaoEnum.Receita ? 'Receita' : 'Despesa'}
                    </span>
                  </td>
                  <td>{t.pessoa.nome}</td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}