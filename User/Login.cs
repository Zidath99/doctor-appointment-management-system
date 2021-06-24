using Doctor_Appointment_Management_System.Main;
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
    public partial class Login : Form
    {
        private SqlConnection databaseConnection;
        public Login()
        {
            InitializeComponent();
            this.databaseConnection = Databse.DatabaseConnection.getConnection();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
       
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            login();
        }

        private void login()
        {

            SqlCommand selectUserCommand;

            selectUserCommand = new SqlCommand("SELECT user_type FROM [user] WHERE username=@username AND password=@password", this.databaseConnection);
            Databse.DatabaseConnection.open(); // open databse

            // bind values to select query
            selectUserCommand.Parameters.AddWithValue("@username", txtUsername.Text);
            selectUserCommand.Parameters.AddWithValue("@password", txtPassword.Text);

            using (SqlDataReader reader = selectUserCommand.ExecuteReader())
            {
                if (reader.Read())
                {
                    Hide(); // hide login form

                    string userType = reader["user_type"].ToString();
                    if (userType == "admin")
                    {
                        // create new instance of Admin View 
                        AdminView adminView = new AdminView();
                        adminView.Show(); // show admin view
                    }
                    else
                    {
                        // create new instance of Receptionist View 
                        ReceptionistView receptionistView = new ReceptionistView();
                        receptionistView.Show(); // show Receptionist view
                    }

                }
                else {
                    MessageBox.Show(this, "Incorrect username or password. Please try again.", "Login Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // we do not need the connection any more, close the database connection
            Databse.DatabaseConnection.close();
        }
    }
}
