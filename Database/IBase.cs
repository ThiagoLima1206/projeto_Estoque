using System;
using System.Collections.Generic;
using System.Text;

namespace Database
{
    public interface IBase
    {
        int Key { get; }
        void CriarTabela();
        void Salvar();
        void Excluir();
        void Editar();
        List<IBase> Todos();
        List<IBase> Busca();
    }
}
