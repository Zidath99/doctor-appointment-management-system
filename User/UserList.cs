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
            SqlDataAdapter auserListSqlAdapter = new SqlDataAdapter("select username as Username, name as Name, email as Email, user_type as [User Type] from [user]", this.databaseConnection);

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
    }
}
