using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace Database
{
    public class Base : IBase
    {
        private string ConnectionString = ConfigurationManager.AppSettings["SqlConnection"];

        public string Key
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
                        return pi.GetValue(this).ToString();
                    }
                }

                return "normal";
            }
        }

        /// <summary>
        /// Faz uma espécie de conversão de string pegando os tipos do C# e transformando e tipos SQL
        /// </summary>
        public string TipoPropriedade(PropertyInfo pi)
        {
            switch (pi.PropertyType.Name)
            {
                case "Int32":
                    return "INT";
                case "Int64":
                    return "BIGINT";
                case "Double":
                    return "DECIMAL(9,2)";
                case "Single":
                    return "FLOAT";
                case "DateTime":
                    return "DATETIME";
                case "Boolean":
                    return "TINYINT";
                default:
                    return "VARCHAR(255)";
            }
        }

        public virtual void Salvar()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                List<string> valores = new List<string>();

                foreach (PropertyInfo pi in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    OpcoesBase pOpcoesBase = (OpcoesBase)pi.GetCustomAttribute(typeof(OpcoesBase));

                    if (pOpcoesBase != null && pOpcoesBase.UsarNoBancoDeDados)
                    {
                        if (pi.PropertyType.Name == "Double")
                        {
                            valores.Add("'" + pi.GetValue(this).ToString().Replace(".", "").Replace(",", ".") + "'");
                        }
                        else
                        {
                            valores.Add("'" + pi.GetValue(this) + "'");
                        }
                    }
                }

                string queryString = string.Empty;

                if (this.Key == "normal")
                {
                    queryString = "EXEC uspGerir" + this.GetType().Name + " ( 1, " + string.Join(", ", valores.ToArray()) + ");";
                }
                else
                {
                    queryString = "EXEC uspGerir" + this.GetType().Name + " ( 2, " + string.Join(", ", valores.ToArray()) + ");";
                }

                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
