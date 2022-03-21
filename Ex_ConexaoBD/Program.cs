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
                var datasource = @"DESKTOP-PJOKI67\SQLEXPRESS";//instancia do servidor
                var database = "Faculdade_XPTO"; //Base de Dados
                var username = "sa"; //usuario da conexão
                var password = "123@abc"; //senha

                //sua string de conexão 
                string connString = @"Data Source=" + datasource + ";Initial Catalog="
                            + database + ";Persist Security Info=True;User ID=" + username + ";Password=" + password;

                //cria a instância de conexão com a base de dados
                SqlConnection connection = new SqlConnection(connString);

                using (connection)
                {
                    // Exemplo de Comando SQL
                    Comando_SQL(connection);

                    // Exemplo de Chamada de Stored Procedure
                    Console.WriteLine("\n\nChamando a Procedure...");
                    Call_SP(connection);
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.WriteLine("\nFIM\n\nPressone qualquer tecla para finalizar");
            Console.ReadLine();
        }

        public static void Call_SP(SqlConnection connection)
        {
            //Definição dos parâmetros
            int ra = 33221100;
            string disciplina = "33001";
            int ano = 2022;
            int semestre = 1;
            double  nota1 = 3.75;


            using (connection)
            {
                connection.Open();
                SqlCommand sql_cmnd = new SqlCommand("Inserir_Nota1", connection);
                sql_cmnd.CommandType = CommandType.StoredProcedure;
                sql_cmnd.Parameters.AddWithValue("@Aluno", SqlDbType.Int).Value = ra;
                sql_cmnd.Parameters.AddWithValue("@Disciplina", SqlDbType.NVarChar).Value = disciplina;
                sql_cmnd.Parameters.AddWithValue("@Ano", SqlDbType.Int).Value = ano;
                sql_cmnd.Parameters.AddWithValue("@Semestre", SqlDbType.Int).Value = semestre;
                sql_cmnd.Parameters.AddWithValue("@Nota1", SqlDbType.Float).Value = nota1;
                //sql_cmnd.Parameters.AddWithValue("@AGE", SqlDbType.Int).Value = age;
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
