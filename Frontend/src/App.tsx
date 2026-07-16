import { useState } from 'react';
import { Sidebar, type Tela } from './components/sideBar';
import { PessoaManager } from './components/PessoaManager';
import { TransacaoManager } from './components/TransacaoManager';
import { TotaisManager } from './components/TotaisManager';
import './App.css';

function App() {
  const [telaAtiva, setTelaAtiva] = useState<Tela>('pessoas');

  return (
    <div className="app-container">
      <Sidebar telaAtiva={telaAtiva} onMudarTela={setTelaAtiva} />
      <main className="main-content">
        {telaAtiva === 'pessoas' && <PessoaManager />}
        {telaAtiva === 'transacoes' && <TransacaoManager />}
        {telaAtiva === 'totais' && <TotaisManager />}
      </main>
    </div>
  );
}

export default App;