using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data.OleDb;

namespace EmployeeProfile
{
    public partial class AddEmployee : Form
    {
        private OleDbConnection conn = new OleDbConnection();
        public AddEmployee()
        {
            InitializeComponent();
            conn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Rhea Carmela\Documents\EmployeeProfile.accdb;
                                Persist Security Info=False;";
        }

        private void AddEmployee_Load(object sender, EventArgs e)
        {

            dateTimeDOB.Format = DateTimePickerFormat.Custom;
            dateTimeDOB.CustomFormat = "dd-MMM-yyyy";

            Dictionary<string, string> comboSource = new Dictionary<string, string>();
            comboSource.Add("Male", "Male");
            comboSource.Add("Female", "Female");

            cboSex.DataSource = new BindingSource(comboSource, null);
            cboSex.DisplayMember = "Value";
            cboSex.ValueMember = "Key";

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            
            try
            {
                conn.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = conn;
                command.CommandText = "Insert Into Employee (FName, LName, DOB, Nationality, Sex) Values ('" +
                    txtFName.Text + "','" + txtLName.Text + "','"  + dateTimeDOB.Value.ToShortDateString() + "','" + txtNationality.Text + "','" + cboSex.SelectedValue.ToString() + "')";
                command.ExecuteNonQuery();
                conn.Close();

                using (new CenterMessageBox(this))
                {
                    MessageBox.Show("Data Saved");
                }
                
            }
            catch(Exception ex)
            {
                using (new CenterMessageBox(this))
                {
                    MessageBox.Show("Error " + ex);
                }
            }

            this.Hide();
            var backHome = new Home();
            backHome.FormClosed += (s, args) => this.Close();
            backHome.Show();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            var backHome = new Home();
            backHome.FormClosed += (s, args) => this.Close();
            backHome.Show();
        }
    }
}
