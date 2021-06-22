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
        public PatientForm()
        {
            InitializeComponent();
            this.databaseConnection = Databse.DatabaseConnection.getConnection();
        }

        private bool validateform()
        {
            if (txtpatientid.Text.Length == 0)
            {
                MessageBox.Show("Patient ID field is required");
                return false;
            }
            if (txtfirstname.Text.Length == 0)
            {
                MessageBox.Show("Firstname field is required");
                return false;
            }
            if (txtlastname.Text.Length == 0)
            {
                MessageBox.Show("Lastname field is required");
                return false;
            }
            if (txtdob.Text.Length == 0)
            {
                MessageBox.Show("DOB field is required");
                return false;
            }
            if (txtaddress.Text.Length == 0)
            {
                MessageBox.Show("Address field is required");
                return false;
            }
            if (txtphone.Text.Length == 0)
            {
                MessageBox.Show("Phone Number field is required");
                return false;
            }
            return true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            saveUser();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void PatientForm_Load(object sender, EventArgs e)
        {

        }
        private void saveUser()
        {
            if (validateform()) {
                try
                {
                    int id = int.Parse(txtpatientid.Text);
                    string firstname = txtfirstname.Text;
                    string lastname = txtlastname.Text;
                    string dob = txtdob.Text;
                    string address = txtaddress.Text;
                    int phone = int.Parse(txtphone.Text);

                    SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\User\source\repos\doctor-appointment-management-system\db.mdf;Integrated Security=True");


                    string query = "INSERT INTO Patient VALUES(" + id + ",'" + firstname + "','" + lastname + "','" + dob + "','" + address + "'," + phone + ")";
                    SqlCommand cmd = new SqlCommand(query, con);


                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record added successfully");
                }
                catch (Exception er)
                {
                    MessageBox.Show("" + er);
                }
            }
        
        }

        private void loadPatient(String patientid)
        {
           
            SqlCommand selectPatientCommand;

            selectPatientCommand = new SqlCommand("SELECT patient_id, first_name, last_name, dob, address, phone_number FROM Patient WHERE patient_id=@patient_id", this.databaseConnection);
            Databse.DatabaseConnection.open(); // open databse

            // bind values to select query
            selectPatientCommand.Parameters.AddWithValue("@patient_id", patientid);

            using (SqlDataReader reader = selectPatientCommand.ExecuteReader())
            {
                if (reader.Read())
                {
                    txtpatientid.Text = reader["patient_id"].ToString();
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

        private void button2_Click(object sender, EventArgs e)
        {
            loadPatient(txtpatientid.Text);
        }
    }
}
