using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Text;

namespace Database
{
    public class Base : IBase
    {
        private string ConnectionString = ConfigurationManager.AppSettings["SqlConnection"];

        public int Key
        {
            get
            {
                /// <summary>
                /// Retorna todas as propriedades públicas de instância do objeto atual 
                /// </summary>
                foreach (PropertyInfo pi in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    /// <summary>
                    /// Recupera um atributo personalizado de um tipo especificado
                    /// </summary>
                    OpcoesBase pOpcoesBase = (OpcoesBase)pi.GetCustomAttribute(typeof(OpcoesBase));

                    /// <summary>
                    /// Verifica se o objeto possui atributos e retorna sua Chave Primária
                    /// </summary>
                    if (pOpcoesBase != null && pOpcoesBase.ChavePrimaria)
                    {
                        return Convert.ToInt32(pi.GetValue(this));
                    }
                }

                return 0;
            }
        }
    }
}
