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
            if (txtpatientid.Text.Length == 0)
            {
                MessageBox.Show("Patient ID field is required");
                return false;
            }
            // show error message if firstname is not entered
            if (txtfirstname.Text.Length == 0)
            {
                MessageBox.Show("First Name field is required");
                return false;
            }

            // show error message if lastname is not entered
            if (txtlastname.Text.Length == 0)
            {
                MessageBox.Show("Last Name field is required");
                return false;
            }
            // show error message if dob is not entered
            if (txtdob.Text.Length == 0)
            {
                MessageBox.Show("DOB field is required");
                return false;
            }
            // show error message if address is not entered
            if (txtaddress.Text.Length == 0)
            {
                MessageBox.Show("Address field is required");
                return false;
            }
            // show error message if lastname is not entered
            if (txtphone.Text.Length == 0)
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
                    txtfirstname.Text = reader["first_name"].ToString();
                    txtlastname.Text = reader["last_name"].ToString();
                    txtdob.Text = reader["dob"].ToString();
                    txtaddress.Text = reader["address"].ToString();
                    txtphone.Text = reader["phone_number"].ToString();
                }
            }

            // we do not need the connection any more, close the database connection
            Databse.DatabaseConnection.close();
        }

       /* private void button1_Click(object sender, EventArgs e)
        {
            loadPatient(txtloadpatientId.Text);
        }*/


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
                    patientInsertCommand.Parameters.AddWithValue("@id", txtpatientid.Text);
                    patientInsertCommand.Parameters.AddWithValue("@firstname", txtfirstname.Text);
                    patientInsertCommand.Parameters.AddWithValue("@lastname", txtlastname.Text);
                    patientInsertCommand.Parameters.AddWithValue("@dob", txtdob.Text);
                    patientInsertCommand.Parameters.AddWithValue("@address", txtaddress.Text);
                    patientInsertCommand.Parameters.AddWithValue("@phone", txtphone.Text);

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
                    patientInsertCommand.Parameters.AddWithValue("@patientid", txtpatientid.Text);
                    patientInsertCommand.Parameters.AddWithValue("@firstname", txtfirstname.Text);
                    patientInsertCommand.Parameters.AddWithValue("@lastname", txtlastname.Text);
                    patientInsertCommand.Parameters.AddWithValue("@dob", txtdob.Text);
                    patientInsertCommand.Parameters.AddWithValue("@address", txtaddress.Text);
                    patientInsertCommand.Parameters.AddWithValue("@phone", txtphone.Text);

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
            button1.Text = "Update";
            isUpdate = true;
            loadPatient(txtload.Text);
        }
    }
}
