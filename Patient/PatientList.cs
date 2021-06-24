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
        private SqlConnection databaseConnection;
        public PatientList()
        {
            InitializeComponent();
            this.databaseConnection = Databse.DatabaseConnection.getConnection();
        }

        private void loadPatients()
        {

            // open database connection
            Databse.DatabaseConnection.open();

            // creare Datatable object
            DataTable patientListDataTable = new DataTable();
            SqlDataAdapter auserListSqlAdapter = new SqlDataAdapter("select patient_id as ID, first_name as [First Name], last_name as [Last Name], dob as DOB, address as Address, phone_number as [Phone Number] from Patient;", this.databaseConnection);

            auserListSqlAdapter.Fill(patientListDataTable);
            tblpatientgrid.DataSource = patientListDataTable;

            //close database connection
            Databse.DatabaseConnection.close();

        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            PatientForm patientForm = new PatientForm();
            patientForm.Show();
        }

        private void tblPatientList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            loadPatients();
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            DialogResult confirmResut = MessageBox.Show("Are you sure you want to delete this user?", "Delete User", MessageBoxButtons.YesNo);

            int patientIdToDelete = 8;
            if (confirmResut == DialogResult.Yes)
            {
                try
                {
                    SqlCommand patientInsertCommand;

                    patientInsertCommand = new SqlCommand("DELETE Patient WHERE id=@id", this.databaseConnection);
                    Databse.DatabaseConnection.open(); // open databse

                    // bind values to delete query
                    patientInsertCommand.Parameters.AddWithValue("@id", patientIdToDelete);

                    // execute the delete command, data will be delete from database
                    patientInsertCommand.ExecuteNonQuery();

                    // we do not need the connection any more close the database connection
                    Databse.DatabaseConnection.close(); ;

                    // refresh data table
                    loadPatients();

                    // show success message to user
                    MessageBox.Show("User deleted successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("User delete failed: " + ex.Message);
                }
            }
        }

        private void PatientList_Load_1(object sender, EventArgs e)
        {
            loadPatients();
        }
    }
}