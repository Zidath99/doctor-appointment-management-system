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

namespace Doctor_Appointment_Management_System.Patient
{
    public partial class PatientForm : Form
    {

        private SqlConnection databaseConnection;
        private bool isUpdate = false; // this flag is used to know whether this is add patient form or update patient form
        private string patientIdForUpdate; // we use this variable to store patient Id and use for update

        public PatientForm()
        {
            InitializeComponent();
            this.databaseConnection = Databse.DatabaseConnection.getConnection();
        }

        private bool isValidUserForm()
        {
            // show error message if first name is not entered
            if (txtFirstName.Text.Length == 0)
            {
                MessageBox.Show("First name field is required");
                return false;
            }

            // show error message if address field is not entered
            if (txtAddress.Text.Length == 0)
            {
                MessageBox.Show("Address field is required");
                return false;
            }


            // show error message if phone has no 10 digits
            if (txtPhone.Text.Length != 10)
            {
                MessageBox.Show("Phone number is required and should be 10 digits");
                return false;
            }

            // show error message if email is entered and invalid
            if (txtEmail.Text.Length > 0 && !new EmailAddressAttribute().IsValid(txtEmail.Text))
            {
                MessageBox.Show("Email format is invalid");
                return false;
            }
            return true;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {

            if (this.isUpdate)
            {
                updatePatient();
            }
            else
            {
                savePatient();
            }
        }


        private void updatePatient()
        {
            if (isValidUserForm())
            {
                try
                {
                    SqlCommand patientUpdateCommand;

                    patientUpdateCommand = new SqlCommand("UPDATE [patient] SET first_name=@first_name,last_name=@last_name,address=@address,email=@email,phone=@phone,dob=@dob where id=@id", this.databaseConnection);
                    Databse.DatabaseConnection.open(); // open databse


                    // bind values to update query
                    patientUpdateCommand.Parameters.AddWithValue("@id", this.patientIdForUpdate);
                    patientUpdateCommand.Parameters.AddWithValue("@first_name", txtFirstName.Text);
                    patientUpdateCommand.Parameters.AddWithValue("@last_name", txtLastName.Text);
                    patientUpdateCommand.Parameters.AddWithValue("@address", txtAddress.Text);
                    patientUpdateCommand.Parameters.AddWithValue("@phone", txtPhone.Text);
                    patientUpdateCommand.Parameters.AddWithValue("@email", txtEmail.Text);
                    patientUpdateCommand.Parameters.AddWithValue("@dob", dateDOB.Value.ToString("yyyy-MM-dd"));

                    // execute the insert command, data will be insert into database
                    patientUpdateCommand.ExecuteNonQuery();

                    // we do not need the connection any more close the database connection
                    Databse.DatabaseConnection.close(); ;

                    // show success message to user
                    MessageBox.Show(this, "Patient updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception ex)
                {
                    // show error message
                    MessageBox.Show(this, "User save failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }

        private void savePatient()
        {
            if (isValidUserForm())
            {
                try
                {
                    SqlCommand patientInsertCommand;

                    patientInsertCommand = new SqlCommand("INSERT INTO [patient] (first_name,last_name,address,phone,email,dob) VALUES (@first_name,@last_name,@address,@phone,@email,@dob)", this.databaseConnection);
                    Databse.DatabaseConnection.open(); // open databse

                    // bind values to insert query
                    patientInsertCommand.Parameters.AddWithValue("@first_name", txtFirstName.Text);
                    patientInsertCommand.Parameters.AddWithValue("@last_name", txtLastName.Text);
                    patientInsertCommand.Parameters.AddWithValue("@address", txtAddress.Text);
                    patientInsertCommand.Parameters.AddWithValue("@phone", txtPhone.Text);
                    patientInsertCommand.Parameters.AddWithValue("@email", txtEmail.Text);
                    patientInsertCommand.Parameters.AddWithValue("@dob", dateDOB.Value.ToString("yyyy-MM-dd"));

                    // execute the insert command, data will be insert into database
                    patientInsertCommand.ExecuteNonQuery();

                    // we do not need the connection any more close the database connection
                    Databse.DatabaseConnection.close(); ;

                    // show success message to user
                    MessageBox.Show(this, "Patient created successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception ex)
                {
                    // show error message
                    MessageBox.Show(this, "User save failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }
        /**
     * This is public method that will called from the user list form when showing this form
     * This will load user for the given user id and mark this form as update form*/
        public void loadPatientToUpdate(string patientId)
        {
            btnSubmit.Text = "Update"; // change lable of the submit button to update
            this.Text = "Update Patient - #"+patientId;
            isUpdate = true; // change isUpdate flag to true. so we know that this is an update form
            this.patientIdForUpdate = patientId; // save patient id in class variable so that update function can access this variable to do the update
            loadPatient(patientId); // load patient form 
        }

        private void loadPatient(String id)
        {

            SqlCommand selectUserCommand;

            selectUserCommand = new SqlCommand("SELECT first_name,last_name,address,phone,email,dob FROM [patient] WHERE id=@id", this.databaseConnection);
            Databse.DatabaseConnection.open(); // open databse

            // bind values to select query
            selectUserCommand.Parameters.AddWithValue("@id", id);

            using (SqlDataReader reader = selectUserCommand.ExecuteReader())
            {
                if (reader.Read())
                {
                    txtFirstName.Text = reader["first_name"].ToString();
                    txtLastName.Text = reader["last_name"].ToString();
                    txtAddress.Text = reader["address"].ToString();
                    txtPhone.Text = reader["phone"].ToString();
                    txtEmail.Text = reader["email"].ToString();
                    dateDOB.Text = reader["dob"].ToString();
                }
            }

            // we do not need the connection any more, close the database connection
            Databse.DatabaseConnection.close();
        }
    }
}
