using System;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data;

namespace EmployeeProfile
{
    public partial class ViewEmployee : Form
    {
        private OleDbConnection conn = new OleDbConnection();
        OleDbCommand command = new OleDbCommand();

        public ViewEmployee()
        {
            InitializeComponent();
            conn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Rhea Carmela\Documents\EmployeeProfile.accdb;
                                Persist Security Info=False;";
            command.Connection = conn;
            cboGender.Text = "Gender";
            cboNationality.Text = "Nationality";
        }

        private void ViewEmployee_Load(object sender, EventArgs e)
        {
            GetGendder();
            GetNationality();
            EmployeeGridView();
        }

        private void EmployeeGridView()
        {
            try
            {
                conn.Open();
                command.Connection = conn;
                string query = "Select * from Employee";

                if (cboGender.SelectedItem != null && cboNationality.SelectedItem == null && txtName.TextLength == 0)
                {
                    query = query + " Where Sex = '" + cboGender.SelectedItem.ToString() + "'";
                }

                if (cboGender.SelectedItem == null && cboNationality.SelectedItem != null && txtName.TextLength == 0)
                {
                    query = query + " Where Nationality = '" + cboNationality.SelectedItem.ToString() + "'";
                }

                if (cboGender.SelectedItem == null && cboNationality.SelectedItem == null && txtName.TextLength > 0)
                {
                    query = query + " Where FName Like '" + txtName.Text + "%'";
                }

                if (cboGender.SelectedItem != null && cboNationality.SelectedItem != null && txtName.TextLength > 0)
                {
                    query = query + " Where Sex = '" + cboGender.SelectedItem.ToString() + "'" +
                        " And Nationality = '" + cboNationality.SelectedItem.ToString() + "'" +
                        " And FName Like '" + txtName.Text + "%'";
                }

                if (cboGender.SelectedItem != null && cboNationality.SelectedItem != null && txtName.TextLength == 0)
                {
                    query = query + " Where Sex = '" + cboGender.SelectedItem.ToString() + "'" +
                       " And Nationality = '" + cboNationality.SelectedItem.ToString() + "'";
                }

                if (cboGender.SelectedItem != null && cboNationality.SelectedItem == null && txtName.TextLength > 0)
                {
                    query = query + " Where Sex = '" + cboGender.SelectedItem.ToString() + "'" +
                       " And FName Like '" + txtName.Text + "%'";
                }

                if (cboGender.SelectedItem == null && cboNationality.SelectedItem != null && txtName.TextLength > 0)
                {
                    query = query + " Where Nationality = '" + cboNationality.SelectedItem.ToString() + "'" +
                        " And FName Like '" + txtName.Text + "%'";
                }

                command.CommandText = query;

                OleDbDataAdapter da = new OleDbDataAdapter(command);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridViewEmployee.DataSource = dt;

                conn.Close();
            }
            catch(Exception ex)
            {
                ErrorMessage(ex);
            }
        }

        private void GetGendder()
        {
            try
            {
                conn.Open();
                string query = "Select Distinct Sex From Employee";
                command.CommandText = query;

                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    cboGender.Items.Add(reader["Sex"].ToString());
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
            }
        }

        private void GetNationality()
        {
            try
            {
                conn.Open();
                string query = "Select Distinct Nationality From Employee";
                command.CommandText = query;

                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    cboNationality.Items.Add(reader["Nationality"].ToString());
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
            }
        }

        private void linkLabelHome_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            var backHome = new Home();
            backHome.FormClosed += (s, args) => this.Close();
            backHome.Show();
        }

        private void ErrorMessage(Exception ex)
        {
            using (new CenterMessageBox(this))
            {
                MessageBox.Show("Error " + ex);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            EmployeeGridView();
        }
    }
}
