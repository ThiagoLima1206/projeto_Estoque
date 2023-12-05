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

        /// <summary>
        /// Método genérico para salvar no Banco de Dados 
        /// </summary>
        public virtual void Salvar()
        {
            ///<summary> Utiliza uma string contendo o endereço do servidor, o nome do banco e o acesso do SQL Server </summary>
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                List<string> valores = new List<string>();

                ///<summary> Retorna o atributo da classe </summary>
                foreach (PropertyInfo pi in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    ///<summary> Retorna as propriedades do atributo </summary>
                    OpcoesBase pOpcoesBase = (OpcoesBase)pi.GetCustomAttribute(typeof(OpcoesBase));

                    if (pOpcoesBase != null && pOpcoesBase.UsarNoBancoDeDados)
                    {
                        ///<summary> Se for Double troca-se as vírgulas por pontos e retira-se os pontos marcadores de casas </summary>
                        if (pi.PropertyType.Name == "Double")
                        {
                            valores.Add("'" + pi.GetValue(this).ToString().Replace(".", "").Replace(",", ".") + "'");
                        }
                        else
                        {
                            ///<summary> Pega o valor que foi inserido no atributo do objeto e o armazena em uma lista </summary>
                            valores.Add("'" + pi.GetValue(this) + "'");
                        }
                    }
                }

                string queryString = string.Empty;

                ///<summary> Concatena a instrução de execução da procedure com os valores da lista </summary>
                queryString = "EXEC uspGerir" + this.GetType().Name + "  1, " + string.Join(", ", valores.ToArray()) + ";";

                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
