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

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void PatientForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
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

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Record added successfully");
            }
            catch(Exception er)
            {
                MessageBox.Show("" + er);
                con.Close();
            }
        }
    }
}
