import '../App.css';

export type Tela = 'pessoas' | 'transacoes' | 'totais';

interface SidebarProps {
  telaAtiva: Tela;
  onMudarTela: (tela: Tela) => void;
}

export function Sidebar({ telaAtiva, onMudarTela }: SidebarProps) {
  const itens: { id: Tela; label: string }[] = [
    { id: 'pessoas', label: 'Pessoas' },
    { id: 'transacoes', label: 'Transações' },
    { id: 'totais', label: 'Totais' },
  ];

  return (
    <aside className="sidebar">
      {itens.map((item) => (
        <button
          key={item.id}
          className={`sidebar-item ${telaAtiva === item.id ? 'active' : ''}`}
          onClick={() => onMudarTela(item.id)}
        >
          {item.label}
        </button>
      ))}
    </aside>
  );
}