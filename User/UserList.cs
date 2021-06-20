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
    public partial class UserList : Form
    {
        private SqlConnection databaseConnection;
        public UserList()
        {
            InitializeComponent();
            this.databaseConnection = Databse.DatabaseConnection.getConnection();
        }

        private void loadUsers()
        {

            // open database connection
            Databse.DatabaseConnection.open();

            // creare Datatable object
            DataTable userListDataTable = new DataTable();
            SqlDataAdapter auserListSqlAdapter = new SqlDataAdapter("select id as ID, username as Username, name as Name, email as Email, user_type as [User Type] from [user]", this.databaseConnection);

            auserListSqlAdapter.Fill(userListDataTable);
            tblUserList.DataSource = userListDataTable;

            //close database connection
            Databse.DatabaseConnection.close();

        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            UserForm userForm = new UserForm();
            userForm.Show();
        }

        private void UserList_Load(object sender, EventArgs e)
        {
            loadUsers();
        }

        private void tblUserList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            loadUsers();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
          DialogResult confirmResut =  MessageBox.Show("Are you sure you want to delete this user?","Delete User", MessageBoxButtons.YesNo);

            int userIdToDelete = 8;
            if (confirmResut == DialogResult.Yes) {
                try
                {
                    SqlCommand userInsertCommand;

                    userInsertCommand = new SqlCommand("DELETE [user] WHERE id=@id", this.databaseConnection);
                    Databse.DatabaseConnection.open(); // open databse

                    // bind values to delete query
                    userInsertCommand.Parameters.AddWithValue("@id", userIdToDelete);

                    // execute the delete command, data will be delete from database
                    userInsertCommand.ExecuteNonQuery();

                    // we do not need the connection any more close the database connection
                    Databse.DatabaseConnection.close(); ;

                    // refresh data table
                    loadUsers();

                    // show success message to user
                    MessageBox.Show("User deleted successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("User delete failed: " + ex.Message);
                }
            }
        }
    }
}
