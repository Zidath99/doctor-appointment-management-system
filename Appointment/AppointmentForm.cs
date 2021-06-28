using Doctor_Appointment_Management_System.Doctor;
using Doctor_Appointment_Management_System.Patient;
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

namespace Doctor_Appointment_Management_System.Appointment
{
    public partial class AppointmentForm : Form
    {
        private SqlConnection databaseConnection;
        private bool isUpdate = false; // this flag is used to know whether this is create appointment  form or update appointment form
        private string appointmentIdForUpdate; // we use this variable to store appointment Id and use for update


        public AppointmentForm()
        {
            InitializeComponent();
            this.databaseConnection = Databse.DatabaseConnection.getConnection();
        }

        private bool isValidUserForm()
        {
            // show error message if patient name is not displayed
            if (txtPatientName.Text.Length == 0)
            {
                MessageBox.Show("Patient is required");
                return false;
            }

            // show error message if doctor name is not displayed
            if (txtDoctorName.Text.Length == 0)
            {
                MessageBox.Show("Doctor is required");
                return false;
            }
            decimal result = 0;
            // show error message if doctor fee is not displayed
            if (txtxDoctorFee.Text.Length == 0 || !Decimal.TryParse(txtxDoctorFee.Text, out result))
            {
                MessageBox.Show("Doctor Fee is required and should be an valid valid price");
                return false;
            }

            // show error message if doctor fee is not displayed
            if (txtHospitalFee.Text.Length == 0 || !Decimal.TryParse(txtHospitalFee.Text, out result))
            {
                MessageBox.Show("Hospital Fee is required and should be an valid price");
                return false;
            }

            // show error message if total fee is not displayed
            if (txtxTotalFee.Text.Length == 0 || !Decimal.TryParse(txtxTotalFee.Text, out result))
            {
                MessageBox.Show("Total Fee is required and should be an valid valid price");
                return false;
            }

            return true;
        }

        private void updateDoctor()
        {
            if (isValidUserForm())
            {
                try
                {
                    SqlCommand appointmentUpdateCommand;

                    appointmentUpdateCommand = new SqlCommand("UPDATE [appointment] SET appointment_date=@appointment_date, patient_id=@patient_id, doctor_id=@doctor_id, doctor_fee=@doctor_fee, hospital_fee=@hospital_fee, total_fee=@total_fee where id=@id", this.databaseConnection);
                    Databse.DatabaseConnection.open(); // open databse


                    // bind values to update query
                    appointmentUpdateCommand.Parameters.AddWithValue("@appointment_date", dateTime.Value.ToString("yyyy-MM-dd"));
                    appointmentUpdateCommand.Parameters.AddWithValue("@patient_id", txtPatientId.Text);
                    appointmentUpdateCommand.Parameters.AddWithValue("@doctor_id", txtDoctorId.Text);
                    appointmentUpdateCommand.Parameters.AddWithValue("@doctor_fee", txtxDoctorFee.Text);
                    appointmentUpdateCommand.Parameters.AddWithValue("@hospital_fee", txtHospitalFee.Text);
                    appointmentUpdateCommand.Parameters.AddWithValue("@total_fee", txtxTotalFee.Text);

                    // execute the insert command, data will be insert into database
                    appointmentUpdateCommand.ExecuteNonQuery();

                    // we do not need the connection any more close the database connection
                    Databse.DatabaseConnection.close(); ;

                    // show success message to user
                    MessageBox.Show(this, "Appointment updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception ex)
                {
                    // show error message
                    MessageBox.Show(this, "Appointment save failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }

        private void saveDoctor()
        {
            if (isValidUserForm())
            {
                try
                {
                    SqlCommand appointmentInsertCommand;

                    appointmentInsertCommand = new SqlCommand("INSERT INTO [appointment] (appointment_date,patient_id,doctor_id,doctor_fee,hospital_fee,total_fee) VALUES (@appointment_date,@patient_id,@doctor_id,@doctor_fee,@hospital_fee,@total_fee)", this.databaseConnection);
                    Databse.DatabaseConnection.open(); // open databse

                    // bind values to insert query
                    appointmentInsertCommand.Parameters.AddWithValue("@appointment_date", dateTime.Value.ToString("yyyy-MM-dd"));
                    appointmentInsertCommand.Parameters.AddWithValue("@patient_id", txtPatientId.Text);
                    appointmentInsertCommand.Parameters.AddWithValue("@doctor_id", txtDoctorId.Text);
                    appointmentInsertCommand.Parameters.AddWithValue("@doctor_fee", txtxDoctorFee.Text);
                    appointmentInsertCommand.Parameters.AddWithValue("@hospital_fee", txtHospitalFee.Text);
                    appointmentInsertCommand.Parameters.AddWithValue("@total_fee",txtxTotalFee.Text);

                    // execute the insert command, data will be insert into database
                    appointmentInsertCommand.ExecuteNonQuery();

                    // we do not need the connection any more close the database connection
                    Databse.DatabaseConnection.close(); ;

                    // show success message to user
                    MessageBox.Show(this, "Appointment created successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception ex)
                {
                    // show error message
                    MessageBox.Show(this, "Appointment save failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }

        /**
          * This is public method that will called from the user list form when showing this form
          * This will load user for the given user id and mark this form as update form
        */
        public void loadAppointmentToUpdate(string appointmentId)
        {
            btnSubmit.Text = "Update"; // change lable of the submit button to update
            this.Text = "Update Appointment - #" + appointmentId;
            isUpdate = true; // change isUpdate flag to true. so we know that this is an update form
            this.appointmentIdForUpdate = appointmentId; // save appointment id in class variable so that update function can access this variable to do the update
            loadAppointment(appointmentId); // load appointment form 
        }


        private void loadAppointment(String id)
        {

            SqlCommand selectUserCommand;

            selectUserCommand = new SqlCommand("SELECT appointment_date,  patient_id, CONCAT(p.first_name,' ',p.last_name) as Patient, doctor_id, CONCAT(d.first_name,' ',d.last_name) as doctor, doctor_fee, hospital_fee, total_fee FROM appointment a JOIN doctor d ON a.doctor_id=d.id JOIN patient p ON a.patient_id = p.id WHERE a.id=@id", this.databaseConnection);
            Databse.DatabaseConnection.open(); // open databse

            // bind values to select query
            selectUserCommand.Parameters.AddWithValue("@id", id);

            using (SqlDataReader reader = selectUserCommand.ExecuteReader())
            {
                if (reader.Read())
                {
                    dateTime.Text = reader["appointment_date"].ToString();
                    txtPatientId.Text = reader["patient_id"].ToString();
                    txtDoctorId.Text = reader["doctor_id"].ToString();
                    txtPatientName.Text = reader["Patient"].ToString();
                    txtDoctorName.Text = reader["Doctor"].ToString();
                    txtxDoctorFee.Text = reader["doctor_fee"].ToString();
                    txtHospitalFee.Text = reader["hospital_fee"].ToString();
                    txtxTotalFee.Text = reader["total_fee"].ToString();
                }
            }

            // we do not need the connection any more, close the database connection
            Databse.DatabaseConnection.close();
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

        private void calculateTotal() {

            decimal hospitalFee = 0;
            decimal doctorFee = 0;

            Decimal.TryParse(txtHospitalFee.Text, out hospitalFee);
            Decimal.TryParse(txtxDoctorFee.Text, out doctorFee);

            decimal totalFee = hospitalFee + doctorFee;
            txtxTotalFee.Text = totalFee.ToString();
        }

        private void txtxDoctorFee_TextChanged(object sender, EventArgs e)
        {
            calculateTotal();
        }

        private void txtHospitalFee_TextChanged(object sender, EventArgs e)
        {
            calculateTotal();
        }

        private void btnNewDoctor_Click(object sender, EventArgs e)
        {
            DoctorForm doctorForm = new DoctorForm();
            doctorForm.ShowDialog(this);
        }

        private void btnNewPatient_Click(object sender, EventArgs e)
        {
            PatientForm patientForm = new PatientForm();
            patientForm.ShowDialog(this);
        }

        private void btnFindPatient_Click(object sender, EventArgs e)
        {
            this.databaseConnection = Databse.DatabaseConnection.getConnection();
            SqlCommand selectUserCommand;

            selectUserCommand = new SqlCommand("SELECT first_name,last_name FROM [patient] WHERE id=@id", this.databaseConnection);
            Databse.DatabaseConnection.open(); // open databse

            // bind values to select query
            selectUserCommand.Parameters.AddWithValue("@id", txtPatientId.Text);

            using (SqlDataReader reader = selectUserCommand.ExecuteReader())
            {
                if (reader.Read())
                {
                    txtPatientName.Text = reader["first_name"].ToString() + " " + reader["last_name"].ToString();
                }
                else {
                    MessageBox.Show(this, "Patient not found. Please check patient ID is correct.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // we do not need the connection any more, close the database connection
            Databse.DatabaseConnection.close();

        }

        private void txtPatientId_TextChanged(object sender, EventArgs e)
        {
            txtPatientName.Text = "";
        }

        private void btnFindDoctor_Click(object sender, EventArgs e)
        {
            this.databaseConnection = Databse.DatabaseConnection.getConnection();
            SqlCommand selectDoctorCommand;

            selectDoctorCommand = new SqlCommand("SELECT first_name,last_name,consultation_fee FROM [doctor] WHERE id=@id", this.databaseConnection);
            Databse.DatabaseConnection.open(); // open databse

            // bind values to select query
            selectDoctorCommand.Parameters.AddWithValue("@id", txtDoctorId.Text);

            using (SqlDataReader reader = selectDoctorCommand.ExecuteReader())
            {
                if (reader.Read())
                {
                    txtDoctorName.Text = reader["first_name"].ToString() + " " + reader["last_name"].ToString();
                    txtxDoctorFee.Text = reader["consultation_fee"].ToString();
                }
                else
                {
                    MessageBox.Show(this, "Doctor not found. Please check doctor ID is correct.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // we do not need the connection any more, close the database connection
            Databse.DatabaseConnection.close();
        }

        private void txtDoctorId_TextChanged(object sender, EventArgs e)
        {
            txtDoctorName.Text = "";
        }
    }

}
