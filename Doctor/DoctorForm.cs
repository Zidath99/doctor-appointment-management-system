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

namespace Doctor_Appointment_Management_System.Doctor
{
    public partial class DoctorForm : Form
    {
        private SqlConnection databaseConnection;
        private bool isUpdate = false; // this flag is used to know whether this is add doctor form or update doctor form
        private string doctorIdForUpdate; // we use this variable to store doctor Id and use for update

        public DoctorForm()
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
                updateDoctor();
            }
            else
            {
                saveDoctor();
            }
        }


        private void updateDoctor()
        {
            if (isValidUserForm())
            {
                try
                {
                    SqlCommand doctorUpdateCommand;

                    doctorUpdateCommand = new SqlCommand("UPDATE [doctor] SET first_name=@first_name,last_name=@last_name,address=@address,email=@email,phone=@phone,dob=@dob,gender=@gender,specialization=@specialization,consultation_fee=@consultation_fee where id=@id", this.databaseConnection);
                    Databse.DatabaseConnection.open(); // open databse


                    // bind values to update query
                    doctorUpdateCommand.Parameters.AddWithValue("@id", this.doctorIdForUpdate);
                    doctorUpdateCommand.Parameters.AddWithValue("@first_name", txtFirstName.Text);
                    doctorUpdateCommand.Parameters.AddWithValue("@last_name", txtLastName.Text);
                    doctorUpdateCommand.Parameters.AddWithValue("@address", txtAddress.Text);
                    doctorUpdateCommand.Parameters.AddWithValue("@phone", txtPhone.Text);
                    doctorUpdateCommand.Parameters.AddWithValue("@email", txtEmail.Text);
                    doctorUpdateCommand.Parameters.AddWithValue("@dob", dateDOB.Value.ToString("yyyy-MM-dd"));
                    doctorUpdateCommand.Parameters.AddWithValue("@gender", radioMale.Checked ? "Male" : "Female");
                    doctorUpdateCommand.Parameters.AddWithValue("@specialization", txtSpecialization.Text);
                    doctorUpdateCommand.Parameters.AddWithValue("@consultation_fee", txtConsultationFee.Text);

                    // execute the insert command, data will be insert into database
                    doctorUpdateCommand.ExecuteNonQuery();

                    // we do not need the connection any more close the database connection
                    Databse.DatabaseConnection.close(); ;

                    // show success message to user
                    MessageBox.Show(this, "Doctor updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception ex)
                {
                    // show error message
                    MessageBox.Show(this, "Doctor save failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }

        private void saveDoctor()
        {
            if (isValidUserForm())
            {
                try
                {
                    SqlCommand doctorInsertCommand;

                    doctorInsertCommand = new SqlCommand("INSERT INTO [doctor] (first_name,last_name,address,phone,email,dob,gender,specialization,consultation_fee) VALUES (@first_name,@last_name,@address,@phone,@email,@dob,@gender,@specialization,@consultation_fee)", this.databaseConnection);
                    Databse.DatabaseConnection.open(); // open databse

                    // bind values to insert query
                    doctorInsertCommand.Parameters.AddWithValue("@first_name", txtFirstName.Text);
                    doctorInsertCommand.Parameters.AddWithValue("@last_name", txtLastName.Text);
                    doctorInsertCommand.Parameters.AddWithValue("@address", txtAddress.Text);
                    doctorInsertCommand.Parameters.AddWithValue("@phone", txtPhone.Text);
                    doctorInsertCommand.Parameters.AddWithValue("@email", txtEmail.Text);
                    doctorInsertCommand.Parameters.AddWithValue("@dob", dateDOB.Value.ToString("yyyy-MM-dd"));
                    doctorInsertCommand.Parameters.AddWithValue("@gender", radioMale.Checked ? "Male" : "Female");
                    doctorInsertCommand.Parameters.AddWithValue("@specialization", txtSpecialization.Text);
                    doctorInsertCommand.Parameters.AddWithValue("@consultation_fee", txtConsultationFee.Text);

                    // execute the insert command, data will be insert into database
                    doctorInsertCommand.ExecuteNonQuery();

                    // we do not need the connection any more close the database connection
                    Databse.DatabaseConnection.close(); ;

                    // show success message to user
                    MessageBox.Show(this, "Doctor created successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception ex)
                {
                    // show error message
                    MessageBox.Show(this, "Doctor save failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }
        /**
     * This is public method that will called from the user list form when showing this form
     * This will load user for the given user id and mark this form as update form*/
        public void loadDoctorToUpdate(string doctorId)
        {
            btnSubmit.Text = "Update"; // change lable of the submit button to update
            this.Text = "Update Doctor - #" + doctorId;
            isUpdate = true; // change isUpdate flag to true. so we know that this is an update form
            this.doctorIdForUpdate = doctorId; // save doctor id in class variable so that update function can access this variable to do the update
            loadDoctor(doctorId); // load doctor form 
        }

        private void loadDoctor(String id)
        {

            SqlCommand selectUserCommand;

            selectUserCommand = new SqlCommand("SELECT first_name,last_name,address,phone,email,dob,gender,specialization,consultation_fee FROM [doctor] WHERE id=@id", this.databaseConnection);
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
                    radioMale.Checked = reader["gender"].ToString() == "Male";
                    radioFemale.Checked = reader["gender"].ToString() == "Female";
                    txtSpecialization.Text = reader["specialization"].ToString();
                    txtConsultationFee.Text = reader["consultation_fee"].ToString();
                }
            }

            // we do not need the connection any more, close the database connection
            Databse.DatabaseConnection.close();
        }
    }
}
