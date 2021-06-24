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

namespace Doctor_Appointment_Management_System.Doctor
{
    public partial class DoctorList : Form
    {
        private SqlConnection databaseConnection;
        private String selectedRowId;
        public DoctorList()
        {
            InitializeComponent();
        }
        private void loadDoctors()
        {
            //create database connection
            this.databaseConnection = Databse.DatabaseConnection.getConnection();

            // open database connection
            Databse.DatabaseConnection.open();

            // creare Datatable object
            DataTable doctorListDataTable = new DataTable();
            SqlDataAdapter auserListSqlAdapter = new SqlDataAdapter("select id as ID ,first_name as [First Name],last_name as [Last Name],address as [address],phone as Phone,email as Email,dob as [Date of Birth], gender as Gender,specialization as Specialization, consultation_fee as [Consultation Fee]  from [doctor]", this.databaseConnection);

            auserListSqlAdapter.Fill(doctorListDataTable);
            tblDoctorList.DataSource = doctorListDataTable;

            //close database connection
            Databse.DatabaseConnection.close();

        }

        private void tblPatientList_SelectionChanged(object sender, EventArgs e)
        {
            if (tblDoctorList.SelectedCells.Count > 0)
            {
                int selectedrowindex = tblDoctorList.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = tblDoctorList.Rows[selectedrowindex];
                this.selectedRowId = Convert.ToString(selectedRow.Cells["id"].Value);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DoctorForm doctorForm = new DoctorForm();
            doctorForm.ShowDialog(this);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            DoctorForm doctorForm = new DoctorForm();
            doctorForm.loadDoctorToUpdate(this.selectedRowId);
            doctorForm.ShowDialog(this);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult confirmResut = MessageBox.Show(this, "Are you sure you want to delete this doctor?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmResut == DialogResult.Yes)
            {
                try
                {
                    SqlCommand deleteCommand;
                    //create database connection
                    this.databaseConnection = Databse.DatabaseConnection.getConnection();

                    Databse.DatabaseConnection.open(); // open databse
                    deleteCommand = new SqlCommand("DELETE [doctor] WHERE id=@id", this.databaseConnection);

                    // bind values to delete query
                    deleteCommand.Parameters.AddWithValue("@id", this.selectedRowId);

                    // execute the delete command, data will be delete from database
                    deleteCommand.ExecuteNonQuery();

                    // we do not need the connection any more close the database connection
                    Databse.DatabaseConnection.close(); ;

                    // refresh data table
                    loadDoctors();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Doctor delete failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            loadDoctors();
        }

        private void DoctorList_Load_1(object sender, EventArgs e)
        {
            loadDoctors();
        }
    }
}
