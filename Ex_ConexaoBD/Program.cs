/*
 * Pacotes Necessários para conexão com BD
 * Na Solução, clicar com o botão direito do mouse, escolher Gerenciar NuGet Packages e instalar:
 * Microsoft.Extensions.Configuration;
 * Microsoft.Extensions.Configuration.FileExtensions;
 * Microsoft.Extensions.Configuration.Json;
 * System.Data.SqlClient.
*/

// 22/03/2022 - Adicionado exemplo de INSERT

using System;
using System.Data;
using System.Data.SqlClient;

namespace Ex_ConexaoBD
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //Montando a String de Conexao
                var datasource = @"DESKTOP-PJOKI67\SQLEXPRESS";//instancia do servidor
                var database = "Faculdade_XPTO"; //Base de Dados
                var username = "sa"; //usuario da conexão
                var password = "123@abc"; //senha

                //Concatenando as informações de sua string de conexão 
                string connString = @"Data Source=" + datasource + ";Initial Catalog="
                            + database + ";Persist Security Info=True;User ID=" + username + ";Password=" + password;

                //cria a instância de conexão com a base de dados
                SqlConnection connection = new SqlConnection(connString);

                using (connection)
                {
                    //Exemplo de Comando SQL - INSERT
                    //Comando_SQL_INSERCAO(connection);

                    // Exemplo de Comando SQL
                    Comando_SQL(connection);

                    // Exemplo de Chamada de Stored Procedure
                    //Console.WriteLine("\n\nChamando a Procedure NOTA2...");
                   // Call_SP(connection);
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.WriteLine("\nFIM\n\nPressone qualquer tecla para finalizar");
            Console.ReadLine();
        }

        public static void Comando_SQL_INSERCAO(SqlConnection connection)
        {
            Console.WriteLine("\nExemplo de Inserção, com outra forma de preparação dos comandos:");

            connection.Open();
            SqlCommand command = new SqlCommand(null, connection);

            // Cria e prepapara a estrutura do Comando SQL.
            command.CommandText =
                "INSERT INTO Aluno (ra, nome) " +
                "VALUES (@ra, @nome)";
            SqlParameter idParam = new SqlParameter("@ra", SqlDbType.Int, 0);
            SqlParameter descParam = new SqlParameter("@nome", SqlDbType.Text, 50);
            Console.WriteLine("Informe o RA do aluno (Registro Acadêmico): ");
            idParam.Value = Console.ReadLine();
            Console.WriteLine("Informe o nome do aluno: ");
            descParam.Value = Console.ReadLine();
            command.Parameters.Add(idParam);
            command.Parameters.Add(descParam);

            // Executa primeiro a preparação do comando após fazer as configurações do mesmo.
            command.Prepare();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public static void Call_SP(SqlConnection connection)
        {
            // Definição dos parâmetros de forma fixa (Estática) -
            // Poderia ter feito o usuário informar esses valores
            int ra = 33221100;
            string disciplina = "33002";
            int ano = 2022;
            int semestre = 1;
            double  nota1 = 5.5;

            using (connection)
            {
                connection.Open();
                SqlCommand sql_cmnd = new SqlCommand("Inserir_Nota", connection);
                sql_cmnd.CommandType = CommandType.StoredProcedure;
                sql_cmnd.Parameters.AddWithValue("@Aluno", SqlDbType.Int).Value = ra;
                sql_cmnd.Parameters.AddWithValue("@Disciplina", SqlDbType.NVarChar).Value = disciplina;
                sql_cmnd.Parameters.AddWithValue("@Ano", SqlDbType.Int).Value = ano;
                sql_cmnd.Parameters.AddWithValue("@Semestre", SqlDbType.Int).Value = semestre;
                sql_cmnd.Parameters.AddWithValue("@Nota2", SqlDbType.Float).Value = nota1;
                sql_cmnd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static void Comando_SQL(SqlConnection connection)
        {
            Console.WriteLine("\nExemplo de Resultados de SQL:");
            Console.WriteLine("=========================================\n");

            connection.Open();

            String sql = "SELECT ra, nome FROM dbo.Aluno";

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine("{0} {1}", reader.GetInt32(0), reader.GetString(1));
                    }
                }

            }
            connection.Close();
        }
    }
}
