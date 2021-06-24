using System.Data;
using System.Data.SqlClient;

namespace Doctor_Appointment_Management_System.Databse
{
    class DatabaseConnection
    {
        private static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Nipun\doctor-appointment-management-system\database.mdf;Integrated Security=True";
        private static SqlConnection connection;
        public static SqlConnection getConnection() {
            connection = new SqlConnection(connectionString);
            return connection;
        }
         
        public static void open() {
            if (connection != null && connection.State == ConnectionState.Closed) {
                connection.Open();
            }
        }

        public static void close()
        {
            if (connection != null && connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }
}
