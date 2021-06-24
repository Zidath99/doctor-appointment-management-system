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
        private bool isUpdate = false;

        public PatientForm()
        {
            InitializeComponent();

            this.databaseConnection = Databse.DatabaseConnection.getConnection();

        }

       

        private bool isValidPatientForm()
        {
            // show error message if patientID is not entered
            if (txtPatientId.Text.Length == 0)
            {
                MessageBox.Show("Patient ID field is required");
                return false;
            }
            // show error message if firstname is not entered
            if (txtFirstName.Text.Length == 0)
            {
                MessageBox.Show("First Name field is required");
                return false;
            }

            // show error message if lastname is not entered
            if (txtLastName.Text.Length == 0)
            {
                MessageBox.Show("Last Name field is required");
                return false;
            }
            // show error message if dob is not entered
            if (txtDOB.Text.Length == 0)
            {
                MessageBox.Show("DOB field is required");
                return false;
            }
            // show error message if address is not entered
            if (txtAddress.Text.Length == 0)
            {
                MessageBox.Show("Address field is required");
                return false;
            }
            // show error message if lastname is not entered
            if (txtPhone.Text.Length == 0)
            {
                MessageBox.Show("Phone number field is required");
                return false;
            }

             

            return true;
        }

      

        private void button1_Click(object sender, EventArgs e)
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

        private void PatientForm_Load(object sender, EventArgs e)
        {

        }

        private void loadPatient(String id)
        {

            SqlCommand selectUserCommand;

            selectUserCommand = new SqlCommand("SELECT patient_id,first_name,last_name,dob,address,phone_number FROM Patient WHERE patient_id=@patient_id", this.databaseConnection);
            Databse.DatabaseConnection.open(); // open databse

            // bind values to select query
            selectUserCommand.Parameters.AddWithValue("@patient_id", id);

            using (SqlDataReader reader = selectUserCommand.ExecuteReader())
            {
                if (reader.Read())
                {
                    txtFirstName.Text = reader["first_name"].ToString();
                    txtLastName.Text = reader["last_name"].ToString();
                    txtDOB.Text = reader["dob"].ToString();
                    txtAddress.Text = reader["address"].ToString();
                    txtPhone.Text = reader["phone_number"].ToString();
                }
            }

            // we do not need the connection any more, close the database connection
            Databse.DatabaseConnection.close();
        }

        private void updatePatient()
        {
            if (isValidPatientForm())
            {
                try
                {
                    SqlCommand patientInsertCommand;

                    patientInsertCommand = new SqlCommand("UPDATE Patient SET first_name=@firstname,last_name=@lastname,dob=@dob,address=@address,phone_number=@phone where patient_id=@id", this.databaseConnection);
                    Databse.DatabaseConnection.open(); // open databse

                    // bind values to insert query
                    patientInsertCommand.Parameters.AddWithValue("@id", txtPatientId.Text);
                    patientInsertCommand.Parameters.AddWithValue("@firstname", txtFirstName.Text);
                    patientInsertCommand.Parameters.AddWithValue("@lastname", txtLastName.Text);
                    patientInsertCommand.Parameters.AddWithValue("@dob", txtDOB.Text);
                    patientInsertCommand.Parameters.AddWithValue("@address", txtAddress.Text);
                    patientInsertCommand.Parameters.AddWithValue("@phone", txtPhone.Text);

                    // execute the insert command, data will be insert into database
                    patientInsertCommand.ExecuteNonQuery();

                    // we do not need the connection any more close the database connection
                    Databse.DatabaseConnection.close(); ;

                    // show success message to user
                    MessageBox.Show("Patient updated successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Patient update failed: " + ex.Message);
                }
            }
        }

        private void savePatient()
        {
            if (isValidPatientForm())
            {
                try
                {
                    SqlCommand patientInsertCommand;

                    patientInsertCommand = new SqlCommand("INSERT INTO Patient (patient_id,first_name,last_name,dob,address,phone_number) VALUES (@patientid,@firstname,@lastname,@dob,@address,@phone)", this.databaseConnection);
                    Databse.DatabaseConnection.open(); // open databse

                    // bind values to insert query
                    patientInsertCommand.Parameters.AddWithValue("@patientid", txtPatientId.Text);
                    patientInsertCommand.Parameters.AddWithValue("@firstname", txtFirstName.Text);
                    patientInsertCommand.Parameters.AddWithValue("@lastname", txtLastName.Text);
                    patientInsertCommand.Parameters.AddWithValue("@dob", txtDOB.Text);
                    patientInsertCommand.Parameters.AddWithValue("@address", txtAddress.Text);
                    patientInsertCommand.Parameters.AddWithValue("@phone", txtPhone.Text);

                    // execute the insert command, data will be insert into database
                    patientInsertCommand.ExecuteNonQuery();

                    // we do not need the connection any more close the database connection
                    Databse.DatabaseConnection.close(); ;

                    // show success message to user
                    MessageBox.Show("Patient saved successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Patient save failed: " + ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            btnSubmit.Text = "Update";
            isUpdate = true;
            loadPatient(txtload.Text);
        }
    }
}
