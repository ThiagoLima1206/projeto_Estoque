using System;
using System.Collections.Generic;
using System.Text;

namespace Database
{
    public interface IBase
    {
        string Key { get; }
        void CriarTabela();
        void Salvar();
        void Excluir();
        List<IBase> Todos();
        List<IBase> Busca();
    }
}
