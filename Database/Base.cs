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
        public virtual void Salvar(int acao)
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

                /// <summary> Inserir </summary>
                if (acao == 1)
                {
                    ///<summary> Concatena a instrução de execução da procedure com os valores da lista </summary>
                    queryString = "EXEC uspGerir" + this.GetType().Name + "  1, " + string.Join(", ", valores.ToArray()) + ";";
                }
                /// <summary> Editar </summary>
                else if (acao == 2)
                {
                    queryString = "EXEC uspGerir" + this.GetType().Name + "  2, " + string.Join(", ", valores.ToArray()) + ";";
                }
                /// <summary> Excluir </summary>
                else if (acao == 3)
                {
                    queryString = "EXEC uspGerir" + this.GetType().Name + "  3, " + string.Join(", ", valores.ToArray()) + ";";
                }

                ///<summary> Abre a conexão com banco e executa a query </summary>
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public virtual List<IBase> Todos()
        {
            var list = new List<IBase>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string queryString = "SELECT * FROM " + this.GetType().Name;
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var obj = (IBase)Activator.CreateInstance(this.GetType());
                    setProperty(ref obj, reader);
                    list.Add(obj);
                }
            }

            return list;
        }

        private void setProperty(ref IBase obj, SqlDataReader reader)
        {
            foreach (PropertyInfo pi in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                OpcoesBase pOpcoesBase = (OpcoesBase)pi.GetCustomAttribute(typeof(OpcoesBase));
                if (pOpcoesBase != null && pOpcoesBase.UsarNoBancoDeDados)
                {
                    pi.SetValue(obj, reader[pi.Name].ToString());
                }
            }
        }
    }
}
