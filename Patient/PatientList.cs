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
    public partial class PatientList : Form
    {
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\User\source\repos\doctor-appointment-management-system\db.mdf;Integrated Security=True;";
        public PatientList()
        {
            InitializeComponent();
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            PatientForm Form = new PatientForm();
            Form.Show();
        }

        private void btnload_Click(object sender, EventArgs e)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlDataAdapter sqlda = new SqlDataAdapter("SELECT * FROM Patient", sqlCon);
                DataTable dtbl = new DataTable();
                sqlda.Fill(dtbl);

                tblpatientgrid.DataSource = tblpatientgrid;

            }
        }
    }
}
