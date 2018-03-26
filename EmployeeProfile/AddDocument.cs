using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data.OleDb;

namespace EmployeeProfile
{
    public partial class AddDocument : Form
    {
        private OleDbConnection conn = new OleDbConnection();
        OleDbCommand command = new OleDbCommand();
        List<EmpRow> empDataRow = new List<EmpRow>();
        int empID = 0;

        public AddDocument()
        {
            InitializeComponent();
            conn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Rhea Carmela\Documents\EmployeeProfile.accdb;
                                Persist Security Info=False;";
            cboEmployee.Text = "---Select Employee--";
            cboType.Text = "--Select Document Type--";

            command.Connection = conn;
        }

        private void AddDocument_Load(object sender, EventArgs e)
        {
            dateTimeIssueDate.Format = DateTimePickerFormat.Custom;
            dateTimeIssueDate.CustomFormat = "dd-MMM-yyyy";

            dateTimeExpiryDate.Format = DateTimePickerFormat.Custom;
            dateTimeExpiryDate.CustomFormat = "dd-MMM-yyyy";

            GetDocumentType();
            GetEmployee();
        }

        private void GetDocumentType()
        {
            try
            {
                conn.Open();
                string query = "Select Distinct Type From Type";
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

        private void GetEmployee()
        {
            try
            {
                conn.Open();

                string query = "Select ID, FName, LName From Employee";
                command.CommandText = query;

                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string temp = null;
                    temp = reader["FName"].ToString() + " " + reader["LName"].ToString();
                    cboEmployee.Items.Add(temp);

                    EmpRow empRow = new EmpRow
                    {
                        empID = Convert.ToInt32(reader["ID"]),
                        empName = temp
                    };

                    empDataRow.Add(empRow);

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            try
            {

                if (empID == 0)
                {
                    MessageBoxPopup("Employee Not Selected. Select An Employee.");
                }
                else if (cboType.SelectedItem == null)
                {
                    MessageBoxPopup("Document Type Not Specified. Select Document Type.");
                }
                else
                {
                    try
                    {
                        int duplicateDocNo = 0;

                        conn.Open();
                        OleDbCommand command = new OleDbCommand();
                        command.Connection = conn;

                        string query = "Select Count(DocNo) From Document Where EmpID = " + empID +
                            " And Type = " + "'" + cboType.SelectedItem.ToString() + "' And DocNo = " + "'" + txtDocNo.Text + "'";

                        command.CommandText = query;


                        if (command.ExecuteScalar() != null)
                        {
                            duplicateDocNo = Convert.ToInt32(command.ExecuteScalar());
                        }

                        if (duplicateDocNo == 0)
                        {
                            command.CommandText = "Insert Into Document (EmpID, Type, IssueDate, ExpiryDate, DocNo) Values ('" +
                            empID + "','" + cboType.SelectedItem.ToString() + "','" + dateTimeIssueDate.Value.ToShortDateString() + "','" + dateTimeExpiryDate.Value.ToShortDateString() + "','" + txtDocNo.Text + "')";
                            command.ExecuteNonQuery();
                            
                            MessageBoxPopup("Data Saved");

                            this.Hide();
                            var backHome = new Home();
                            backHome.FormClosed += (s, args) => this.Close();
                            backHome.Show();
                        }
                        else
                        {
                            MessageBoxPopup("Duplicate in Document No. " + txtDocNo.Text + " of Document Type " + 
                                cboType.SelectedItem.ToString() + " For Employee " + cboEmployee.SelectedItem.ToString() + ".");
                        }

                        conn.Close();
                    }
                    catch(Exception ex)
                    {
                        ErrorMessage(ex);
                    }

                }

            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
            }
        }

        private void MessageBoxPopup(string message)
        {
            using (new CenterMessageBox(this))
            {
                MessageBox.Show(message);
            }
        }

        private void ErrorMessage(Exception ex)
        {
            using (new CenterMessageBox(this))
            {
                MessageBox.Show("Error " + ex);
            }
        }

        private void cboEmployee_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            try
            {
                foreach (var item in empDataRow)
                {
                    if (cboEmployee.SelectedItem != null)
                    {
                        if (item.empName == cboEmployee.SelectedItem.ToString())
                        {
                            empID = item.empID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
            }
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
