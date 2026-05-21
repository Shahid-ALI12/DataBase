using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Database_Project
{
    internal static class DbHelper
    {
        private const string ConnectionString = "Data Source=SURAIMAHMAD\\SQLEXPRESS01;Initial Catalog=AIRLINE;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";

        public static int ExecuteNonQuery(string commandText, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(commandText, connection))
            {
                if (parameters != null && parameters.Length > 0)
                {
                    command.Parameters.AddRange(parameters);
                }

                connection.Open();
                return command.ExecuteNonQuery();
            }
        }
    }
}
