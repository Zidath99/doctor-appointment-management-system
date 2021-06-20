using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Doctor_Appointment_Management_System.User
{
    public partial class UserForm : Form
    {
        public UserForm()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // create databse connection
                SqlConnection databaseConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Nipun\source\repos\doctor-appointment-management-system\db.mdf;Integrated Security=True");
                SqlCommand userInsertCommand;

                userInsertCommand = new SqlCommand("INSERT INTO [user] (username,name,email,password) VALUES (@username,@name,@email,@password)", databaseConnection);
                databaseConnection.Open(); // open databse

                // bind values to insert query
                userInsertCommand.Parameters.AddWithValue("@username", txtUsername.Text);
                userInsertCommand.Parameters.AddWithValue("@name", txtName.Text);
                userInsertCommand.Parameters.AddWithValue("@email", txtEmail.Text);
                userInsertCommand.Parameters.AddWithValue("@password", txtPassword.Text);

                // execute the insert command, data will be insert into database
                userInsertCommand.ExecuteNonQuery();

                // we do not need the connection any more, close the database connection
                databaseConnection.Close();

                // show success message to user
                MessageBox.Show("User saved successfully!");
            }
            catch (Exception ex) {
                MessageBox.Show("User save failed: "+ex.Message);
            }
        }

       
    }
}
