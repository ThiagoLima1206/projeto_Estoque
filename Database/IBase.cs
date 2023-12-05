using System;
using System.Collections.Generic;
using System.Text;

namespace Database
{
    public interface IBase
    {
        //void CriarTabela();
        void Salvar(int acao);
        List<IBase> Todos();
        //List<IBase> Busca();
    }
}
