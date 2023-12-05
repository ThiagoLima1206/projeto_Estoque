using System;
using System.Collections.Generic;
using System.Text;

namespace Database
{
    public class OpcoesBase : Attribute
    {
        public bool UsarNoBancoDeDados { get; set; }
        public bool UsarParaBuscar { get; set; }
        public bool ChavePrimaria { get; set; }
        public bool AutoIncrementar { get; set; }
    }
}
