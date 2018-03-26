using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmployeeProfile
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnViewEmp_Click(object sender, EventArgs e)
        {
            this.Hide();
            var viewEmp = new ViewEmployee();
            viewEmp.FormClosed += (s, args) => this.Close();
            viewEmp.Show();
        }

        private void btnViewDoc_Click(object sender, EventArgs e)
        {
            this.Hide();
            var viewDoc = new ViewDocument();
            viewDoc.FormClosed += (s, args) => this.Close();
            viewDoc.Show();
        }

        private void btnAddEmp_Click(object sender, EventArgs e)
        {
            this.Hide();
            var newEmp = new AddEmployee();
            newEmp.FormClosed += (s, args) => this.Close();
            newEmp.Show();
        }

        private void btnAddDoc_Click(object sender, EventArgs e)
        {
            this.Hide();
            var newDocument = new AddDocument();
            newDocument.FormClosed += (s, args) => this.Close();
            newDocument.Show();
        }
    }
}
