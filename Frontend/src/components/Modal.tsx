import type { ReactNode } from 'react';
import './Modal.css';

interface ModalProps {
  titulo: string;
  onFechar: () => void;
  children: ReactNode; // permite colocar qualquer conteúdo dentro do modal
}

export function Modal({ titulo, onFechar, children }: ModalProps) {
  return (
    // O "overlay" é o fundo escurecido. Clicar nele fecha o modal (clique fora = fechar).
    <div className="modal-overlay" onClick={onFechar}>
      {/* stopPropagation impede que o clique DENTRO da caixa "vaze" pro overlay e feche o modal */}
      <div className="modal-box" onClick={(e) => e.stopPropagation()}>
        <div className="modal-header">
          <h2>{titulo}</h2>
          <button className="modal-close" onClick={onFechar}>×</button>
        </div>
        <div className="modal-body">{children}</div>
      </div>
    </div>
  );
}