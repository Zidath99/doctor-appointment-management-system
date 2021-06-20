using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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

        private SqlConnection databaseConnection;
        private bool isUpdate = false; // this flag is used to know whether this is add user form or update user form
        private string userIdForUpdate; // we use this variable to store user Id and use for update

        public UserForm()
        {
            InitializeComponent();

            this.databaseConnection = Databse.DatabaseConnection.getConnection();

        }

        private bool isValidUserForm()
        {
            // show error message if username is not entered
            if (txtUsername.Text.Length == 0)
            {
                MessageBox.Show("Username field is required");
                return false;
            }

            // show error message if username is not entered
            if (!isUpdate)
            {
                // this validation is checcked only when inserting
                if (isUsernameExists(txtUsername.Text))
                {
                    MessageBox.Show("Username is already in use");
                    return false;
                }
            }

            // show error message if name field is not entered
            if (txtName.Text.Length == 0)
            {
                MessageBox.Show("Name field is required");
                return false;
            }

            // show error message if email is not entered of invalid email is entered
            if (txtEmail.Text.Length == 0 || !new EmailAddressAttribute().IsValid(txtEmail.Text))
            {
                MessageBox.Show("Email format is invalid");
                return false;
            }

            // show error message if password is not entered or not matched with confirm password
            if (txtPassword.Text.Length == 0 || txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Password is empty or not matched");
                return false;
            }

            // show error message if user type is not selected
            if (cmbUserType.Text.Length == 0)
            {
                MessageBox.Show("User type is required");
                return false;
            }

            return true;
        }

        private bool isUsernameExists(String usernmae)
        {

            SqlCommand selectUserCommand;

            selectUserCommand = new SqlCommand("SELECT count(id) as userCount FROM [user] WHERE username=@username", this.databaseConnection);
            Databse.DatabaseConnection.open(); // open databse

            // bind values to select query
            selectUserCommand.Parameters.AddWithValue("@username", usernmae);

            using (SqlDataReader reader = selectUserCommand.ExecuteReader())
            {
                if (reader.Read())
                {
                    if (reader["userCount"].ToString() == "0")
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }

                }
            }

            // we do not need the connection any more, close the database connection
            Databse.DatabaseConnection.close();

            return false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.isUpdate)
            {
                updateUser();
            }
            else
            {
                saveUser();
            }

        }

        private void UserForm_Load(object sender, EventArgs e)
        {

        }

        private void loadUser(String id)
        {

            SqlCommand selectUserCommand;

            selectUserCommand = new SqlCommand("SELECT id,username,name,email,user_type FROM [user] WHERE id=@id", this.databaseConnection);
            Databse.DatabaseConnection.open(); // open databse

            // bind values to select query
            selectUserCommand.Parameters.AddWithValue("@id", id);

            using (SqlDataReader reader = selectUserCommand.ExecuteReader())
            {
                if (reader.Read())
                {
                    txtUsername.Text = reader["username"].ToString();
                    txtName.Text = reader["name"].ToString();
                    txtEmail.Text = reader["email"].ToString();
                    cmbUserType.Text = reader["user_type"].ToString();
                }
            }

            // we do not need the connection any more, close the database connection
            Databse.DatabaseConnection.close();
        }

        /**
         * This is public method that will called from the user list form when showing this form
         * This will load user for the given user id and mark this form as update form*/
        public void loadUserToUpdate(string userId)
        {
            btnSave.Text = "Update"; // change lable of the submit button to update
            isUpdate = true; // change isUpdate flag to tru. so we know that this is an update form
            this.userIdForUpdate = userId; // save user id in class variable so that update function can access this variable to do the update
            loadUser(userId); // load user form 
        }

        private void updateUser()
        {
            if (isValidUserForm())
            {
                try
                {
                    SqlCommand userInsertCommand;

                    userInsertCommand = new SqlCommand("UPDATE [user] SET username=@username,name=@name,email=@email,password=@password,user_type=@userType where id=@id", this.databaseConnection);
                    Databse.DatabaseConnection.open(); // open databse

                    // bind values to insert query
                    userInsertCommand.Parameters.AddWithValue("@id", this.userIdForUpdate);
                    userInsertCommand.Parameters.AddWithValue("@username", txtUsername.Text);
                    userInsertCommand.Parameters.AddWithValue("@name", txtName.Text);
                    userInsertCommand.Parameters.AddWithValue("@email", txtEmail.Text);
                    userInsertCommand.Parameters.AddWithValue("@password", txtPassword.Text);
                    userInsertCommand.Parameters.AddWithValue("@userType", cmbUserType.Text.ToLower());

                    // execute the insert command, data will be insert into database
                    userInsertCommand.ExecuteNonQuery();

                    // we do not need the connection any more close the database connection
                    Databse.DatabaseConnection.close(); ;

                    // show success message to user
                    MessageBox.Show("User updated successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("User update failed: " + ex.Message);
                }
            }
        }

        private void saveUser()
        {
            if (isValidUserForm())
            {
                try
                {
                    SqlCommand userInsertCommand;

                    userInsertCommand = new SqlCommand("INSERT INTO [user] (username,name,email,password,user_type) VALUES (@username,@name,@email,@password,@userType)", this.databaseConnection);
                    Databse.DatabaseConnection.open(); // open databse

                    // bind values to insert query
                    userInsertCommand.Parameters.AddWithValue("@username", txtUsername.Text);
                    userInsertCommand.Parameters.AddWithValue("@name", txtName.Text);
                    userInsertCommand.Parameters.AddWithValue("@email", txtEmail.Text);
                    userInsertCommand.Parameters.AddWithValue("@password", txtPassword.Text);
                    userInsertCommand.Parameters.AddWithValue("@userType", cmbUserType.Text.ToLower());

                    // execute the insert command, data will be insert into database
                    userInsertCommand.ExecuteNonQuery();

                    // we do not need the connection any more close the database connection
                    Databse.DatabaseConnection.close(); ;

                    // show success message to user
                    MessageBox.Show("User saved successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("User save failed: " + ex.Message);
                }
            }
        }
    }
}
