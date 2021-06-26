using Doctor_Appointment_Management_System.Appointment;
using Doctor_Appointment_Management_System.Patient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Doctor_Appointment_Management_System.Main
{
    public partial class ReceptionistView : Form
    {
        private int childFormNumber = 0;

        public ReceptionistView()
        {
            InitializeComponent();
        }

      
        private void ShowNewForm(object sender, EventArgs e) { }
            private void createNewToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            PatientForm patientForm = new PatientForm();
            patientForm.MdiParent = this;
            patientForm.Show();
        }

        private void viewAllToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            PatientList patientList = new PatientList();
            patientList.MdiParent = this;
            patientList.Show();
        }

        private void createNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AppointmentForm appointmentForm = new AppointmentForm();
            appointmentForm.MdiParent = this;
            appointmentForm.Show();
        }

        private void viewAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AppointmentList appoinmentList = new AppointmentList();
            appoinmentList.MdiParent = this;
            appoinmentList.Show();
        }
    }
}
