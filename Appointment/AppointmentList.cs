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
    public partial class AppointmentList : Form
    {
        private SqlConnection databaseConnection;
        private String selectedRowId;

        public AppointmentList()
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
            SqlDataAdapter auserListSqlAdapter = new SqlDataAdapter("SELECT a.id as ID, appointment_date as Date,  CONCAT(p.first_name,' ',p.last_name) as Patient, CONCAT(d.first_name,' ',d.last_name) as doctor, doctor_fee as [Doctor Fee], hospital_fee as [Hospital Fee], total_fee as [Total Fee] FROM appointment a JOIN doctor d ON a.doctor_id=d.id JOIN patient p ON a.patient_id = p.id", this.databaseConnection);

            auserListSqlAdapter.Fill(doctorListDataTable);
            tbAppointmentList.DataSource = doctorListDataTable;

            //close database connection
            Databse.DatabaseConnection.close();

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AppointmentForm appointmentForm = new AppointmentForm();
            appointmentForm.ShowDialog(this);
        }

        private void tbAppointmentList_SelectionChanged(object sender, EventArgs e)
        {
            if (tbAppointmentList.SelectedCells.Count > 0)
            {
                int selectedrowindex = tbAppointmentList.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = tbAppointmentList.Rows[selectedrowindex];
                this.selectedRowId = Convert.ToString(selectedRow.Cells["id"].Value);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            loadDoctors();
        }

        private void AppointmentList_Load(object sender, EventArgs e)
        {
            loadDoctors();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            AppointmentForm appointmentForm = new AppointmentForm();
            appointmentForm.loadAppointmentToUpdate(this.selectedRowId);
            appointmentForm.ShowDialog(this);
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            DialogResult confirmResut = MessageBox.Show(this, "Are you sure you want to delete this appointment?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmResut == DialogResult.Yes)
            {
                try
                {
                    SqlCommand deleteCommand;
                    //create database connection
                    this.databaseConnection = Databse.DatabaseConnection.getConnection();

                    Databse.DatabaseConnection.open(); // open databse
                    deleteCommand = new SqlCommand("DELETE [appointment] WHERE id=@id", this.databaseConnection);

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
                    MessageBox.Show(this, "Appointment delete failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
