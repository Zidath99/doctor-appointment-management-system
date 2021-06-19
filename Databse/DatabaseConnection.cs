using System.Data.SqlClient;

namespace Doctor_Appointment_Management_System.Databse
{
    class DatabaseConnection
    {
        private static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='C:\Users\Nipun\source\repos\Doctor Appointment Management System\db.mdf';Integrated Security=True";
        private static SqlConnection connection;
        public static SqlConnection getConnection() {
            connection = new SqlConnection(connectionString);
            return connection;
        }
    }
}
