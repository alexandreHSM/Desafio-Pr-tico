import { useState, useEffect } from 'react';
import type { RelatorioTotaisDto } from '../types/Totais';
import { getTotais } from '../services/totaisServices';
import './PessoaManager.css'; 
import './TotaisManager.css';

export function TotaisManager() {
  const [relatorio, setRelatorio] = useState<RelatorioTotaisDto | null>(null);
  const [erro, setErro] = useState<string | null>(null);
  const [carregando, setCarregando] = useState(true);

  useEffect(() => {
    carregarTotais();
  }, []);

  async function carregarTotais() {
    try {
      setCarregando(true);
      const dados = await getTotais();
      setRelatorio(dados);
    } catch {
      setErro('Não foi possível carregar os totais.');
    } finally {
      setCarregando(false);
    }
  }

  return (
    <div className="tela-container">
      <h1>Totais</h1>

      {erro && <p className="erro-msg">{erro}</p>}

      <div className="tabela-container">
        <table>
          <thead>
            <tr>
              <th>Nome</th>
              <th>Receitas</th>
              <th>Despesas</th>
              <th>Saldo</th>
            </tr>
          </thead>
          <tbody>
            {carregando ? (
              <tr><td colSpan={4}>Carregando...</td></tr>
            ) : relatorio && relatorio.pessoas.length > 0 ? (
              relatorio.pessoas.map((p) => (
                <tr key={p.pessoaId}>
                  <td>{p.nome}</td>
                  <td>R$ {p.totalReceitas.toFixed(2)}</td>
                  <td>R$ {p.totalDespesas.toFixed(2)}</td>
                  <td className={p.saldo >= 0 ? 'valor-positivo' : 'valor-negativo'}>
                    R$ {p.saldo.toFixed(2)}
                  </td>
                </tr>
              ))
            ) : (
              <tr><td colSpan={4}>Nenhuma pessoa cadastrada.</td></tr>
            )}
          </tbody>
        </table>
      </div>

      {/* Resumo geral só aparece depois que os dados carregarem, evitando mostrar "R$ 0,00" piscando */}
      {relatorio && (
        <div className="resumo-geral">
          <div className="item">
            <span className="label">Total Receitas</span>
            <span>R$ {relatorio.totalGeralReceitas.toFixed(2)}</span>
          </div>
          <div className="item">
            <span className="label">Total Despesas</span>
            <span>R$ {relatorio.totalGeralDespesas.toFixed(2)}</span>
          </div>
          <div className="item">
            <span className="label">Saldo Líquido</span>
            <span className={relatorio.saldoLiquidoGeral >= 0 ? 'valor-positivo' : 'valor-negativo'}>
              R$ {relatorio.saldoLiquidoGeral.toFixed(2)}
            </span>
          </div>
        </div>
      )}
    </div>
  );
}