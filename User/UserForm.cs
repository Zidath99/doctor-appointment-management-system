using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Doctor_Appointment_Management_System.User
{
    public partial class UserForm : Form
    {
        public UserForm()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            String username = txtUsername.Text;
            String name = txtName.Text;
            String email = txtEmail.Text;
            String password = txtPassword.Text;
            Console.WriteLine(username);
            Console.WriteLine(name);
            Console.WriteLine(email);
            Console.WriteLine(password);
        }

       
    }
}
