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
        private String selectedRowId;
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
            SqlDataAdapter auserListSqlAdapter = new SqlDataAdapter("select id as ID ,first_name as [First Name],last_name as [Last Name],address as [address],phone as Phone,email as Email,dob as [Date of Birth] from [patient]", this.databaseConnection);

            auserListSqlAdapter.Fill(patientListDataTable);
            tblPatientList.DataSource = patientListDataTable;

            //close database connection
            Databse.DatabaseConnection.close();

        }

        private void tblPatientList_SelectionChanged(object sender, EventArgs e)
        {
            if (tblPatientList.SelectedCells.Count > 0)
            {
                int selectedrowindex = tblPatientList.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = tblPatientList.Rows[selectedrowindex];
                this.selectedRowId = Convert.ToString(selectedRow.Cells["id"].Value);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            PatientForm patientForm = new PatientForm();
            patientForm.ShowDialog(this);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            PatientForm patientForm = new PatientForm();
            patientForm.loadPatientToUpdate(this.selectedRowId);
            patientForm.ShowDialog(this);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult confirmResut = MessageBox.Show(this, "Are you sure you want to delete this patient?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmResut == DialogResult.Yes)
            {
                try
                {
                    SqlCommand deleteCommand;

                    Databse.DatabaseConnection.open(); // open databse
                    deleteCommand = new SqlCommand("DELETE [patient] WHERE id=@id", this.databaseConnection);

                    // bind values to delete query
                    deleteCommand.Parameters.AddWithValue("@id", this.selectedRowId);

                    // execute the delete command, data will be delete from database
                    deleteCommand.ExecuteNonQuery();

                    // we do not need the connection any more close the database connection
                    Databse.DatabaseConnection.close(); ;

                    // refresh data table
                    loadPatients();
}
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Patient delete failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            loadPatients();
        }

        private void PatientList_Load(object sender, EventArgs e)
        {
            loadPatients();
        }
    }
}
