using System;
using System.Data;
using System.Windows.Forms;
using System.Data.OleDb;

namespace EmployeeProfile
{
    public partial class ViewDocument : Form
    {
        private OleDbConnection conn = new OleDbConnection();
        OleDbCommand command = new OleDbCommand();

        public ViewDocument()
        {
            InitializeComponent();
            conn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Rhea Carmela\Documents\EmployeeProfile.accdb;
                                Persist Security Info=False;";
            command.Connection = conn;
            cboEmployee.Text = "Employee";
            cboType.Text = "Document";
            dateTimeExpiryDate.Format = DateTimePickerFormat.Custom;
            dateTimeExpiryDate.CustomFormat = "dd-MMM-yyyy";
            dateTimeExpiryDate.MinDate = DateTime.Today;
        }

        private void ViewDocument_Load(object sender, EventArgs e)
        {
            GetEmployee();
            GetDocumentType();
            DocumentGridView();
        }

        private void DocumentGridView()
        {
            try
            {
                conn.Open();
                command.Connection = conn;
                string query = "Select D.EmpID As EmployeeID, (E.FName & ' ' & E.LName) As Employee, D.Type As DocType, D.IssueDate, D.ExpiryDate, D.DocNo  from Document D Left Join Employee E on E.ID = D.EmpID";

                if (cboEmployee.SelectedItem != null && cboType.SelectedItem == null && dateTimeExpiryDate.Value.ToShortDateString() == DateTime.Today.ToShortDateString())
                {
                    query = query + " Where (E.FName & ' ' & E.LName) = '" + cboEmployee.SelectedItem.ToString() + "'";
                }

                if (cboEmployee.SelectedItem == null && cboType.SelectedItem != null && dateTimeExpiryDate.Value.ToShortDateString() == DateTime.Today.ToShortDateString())
                {
                    query = query + " Where D.Type = '" + cboType.SelectedItem.ToString() + "'";
                }

                if (cboEmployee.SelectedItem == null && cboType.SelectedItem == null && dateTimeExpiryDate.Value.ToShortDateString() != DateTime.Today.ToShortDateString())
                {
                    query = query + " Where D.ExpiryDate Like '" + dateTimeExpiryDate.Value.ToShortDateString() + "%'";
                }

                if (cboEmployee.SelectedItem != null && cboType.SelectedItem != null && dateTimeExpiryDate.Value.ToShortDateString() != DateTime.Today.ToShortDateString())
                {
                    query = query + " Where (E.FName & ' ' & E.LName) = '" + cboEmployee.SelectedItem.ToString() + "'" +
                        " And D.Type = '" + cboType.SelectedItem.ToString() + "'" +
                        " And D.ExpiryDate Like '" + dateTimeExpiryDate.Value.ToShortDateString() + "%'";
                }

                if (cboEmployee.SelectedItem != null && cboType.SelectedItem != null && dateTimeExpiryDate.Value.ToShortDateString() == DateTime.Today.ToShortDateString())
                {
                    query = query + " Where (E.FName & ' ' & E.LName) = '" + cboEmployee.SelectedItem.ToString() + "'" +
                       " And D.Type = '" + cboType.SelectedItem.ToString() + "'";
                }

                if (cboEmployee.SelectedItem != null && cboType.SelectedItem == null && dateTimeExpiryDate.Value.ToShortDateString() != DateTime.Today.ToShortDateString())
                {
                    query = query + " Where (E.FName & ' ' & E.LName) = '" + cboEmployee.SelectedItem.ToString() + "'" +
                       " And D.ExpiryDate Like '" + dateTimeExpiryDate.Value.ToShortDateString() + "%'";
                }

                if (cboEmployee.SelectedItem == null && cboType.SelectedItem != null && dateTimeExpiryDate.Value.ToShortDateString() != DateTime.Today.ToShortDateString())
                {
                    query = query + " Where D.Type = '" + cboType.SelectedItem.ToString() + "'" +
                        " And D.ExpiryDate Like '" + dateTimeExpiryDate.Value.ToShortDateString() + "%'";
                }

                command.CommandText = query;

                OleDbDataAdapter da = new OleDbDataAdapter(command);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridViewDocument.DataSource = dt;

                conn.Close();
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
            }
        }

        private void GetEmployee()
        {
            try
            {
                conn.Open();
                string query = "Select Distinct D.EmpID As EmployeeID, (E.FName & ' ' & E.LName) As Employee from Document D Left Join Employee E on E.ID = D.EmpID";
                command.CommandText = query;

                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    cboEmployee.Items.Add(reader["Employee"].ToString());
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
            }
        }

        private void GetDocumentType()
        {
            try
            {
                conn.Open();
                string query = "Select Distinct Type from Document";
                command.CommandText = query;

                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    cboType.Items.Add(reader["Type"].ToString());
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
            }
        }

        private void ErrorMessage(Exception ex)
        {
            using (new CenterMessageBox(this))
            {
                MessageBox.Show("Error " + ex);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            var backHome = new Home();
            backHome.FormClosed += (s, args) => this.Close();
            backHome.Show();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            DocumentGridView();
        }
    }
}
