using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Data.SqlClient;
using MailKit.Net.Pop3;
using MailKit.Net.Smtp;
using MailKit.Net.Imap;

namespace AutoProcessMail
{
    public partial class frmSetting : Form
    {

        #region Const

        private const string FILE_NAME = "Info.ini";
        private const string SERVER_NAME = "ServerName";
        private const string USER_NAME = "UserName";
        private const string PASSWORD = "Password";
        private const string DATABASE = "DataBase";

        private const string EMAIL_ADDRESS = "EmailAddress";
        private const string EMAIL_PASSWORD = "EmailPassword";
        private const string RECEIVE_INTERVAL = "ReceiveInterval";
        private const string HOSTIMAP4 = "HostIMap4";
        private const string HOSTSMTP = "HostSMTP";
        private const string PORTIMAP4 = "PortIMap4";
        private const string PORTSMTP = "PortSMTP";
        private const string SSLIMAP4 = "SSLIMap4";
        private const string SSLSMTP = "SSLSMTP";
        private const string SAVEPATH = "SavePath";

        #endregion

        #region Property

        public string ServerName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DataBase { get; set; }
        public string EmailAddress { get; set; }
        public string EmailPassword { get; set; }
        public string ReceiveInterval { get; set; }
        public string HostIMap4 { get; set; }
        public string PortIMap4 { get; set; }
        public string HostSMTP { get; set; }
        public string PortSMTP { get; set; }
        public string SavePath { get; set; }
        public Boolean SslIMap4 { get; set; }
        public Boolean SslSMTP { get; set; }
        public List<MailStruct> ResendMailList { get; set; }
        public Boolean changeCBSMTP = false;

        public struct MailStruct
        {
            public int ID;
            public int ResendTime;
        }

        #endregion

        #region Contructor

        public frmSetting()
        {
            InitializeComponent();
        }

        #endregion

        #region "Event"

        private void frmSetting_Load(object sender, EventArgs e)
        {
            //Clear
            this.Clear();

            //Read File
            this.ReadFile();

            ////Set focus
            this.txtServerName.Focus();
        }

        /// <summary>
        /// Click Setting Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(Path.Combine(Application.StartupPath, FILE_NAME)))
                {
                    //DB
                    sw.WriteLine(string.Format("{0}={1}", SERVER_NAME, this.txtServerName.Text.Trim()));
                    sw.WriteLine(string.Format("{0}={1}", USER_NAME, this.txtUserName.Text.Trim()));
                    sw.WriteLine(string.Format("{0}={1}", PASSWORD, this.txtPassword.Text.Trim()));
                    sw.WriteLine(string.Format("{0}={1}", DATABASE, this.txtDatabase.Text.Trim()));

                    //File
                    sw.WriteLine(string.Format("{0}={1}", EMAIL_ADDRESS, this.txtEmailAddress.Text.Trim()));
                    sw.WriteLine(string.Format("{0}={1}", EMAIL_PASSWORD, this.txtEmailPassword.Text.Trim()));
                    sw.WriteLine(string.Format("{0}={1}", RECEIVE_INTERVAL, this.txtReceiveInterval.Text.Trim()));
                    sw.WriteLine(string.Format("{0}={1}", HOSTIMAP4, this.txtHostIMap4.Text.Trim()));
                    sw.WriteLine(string.Format("{0}={1}", PORTIMAP4, this.cBPortIMap4.Text.Trim()));
                    sw.WriteLine(string.Format("{0}={1}", HOSTSMTP, this.txtHostSMTP.Text.Trim()));
                    sw.WriteLine(string.Format("{0}={1}", PORTSMTP, this.cBPortSMTP.Text.Trim()));
                    sw.WriteLine(string.Format("{0}={1}", SAVEPATH, this.txtSavePath.Text.Trim()));
                    if (this.ckBSSLIMap4.Checked)
                    {
                        sw.WriteLine(string.Format("{0}={1}", SSLIMAP4, "1"));
                    }
                    else
                    {
                        sw.WriteLine(string.Format("{0}={1}", SSLIMAP4, "0"));
                    }
                    if (this.ckBSSLSMTP.Checked)
                    {
                        sw.WriteLine(string.Format("{0}={1}", SSLSMTP, "1"));
                    }
                    else
                    {
                        sw.WriteLine(string.Format("{0}={1}", SSLSMTP, "0"));
                    }
                    MessageBox.Show("Successful!", this.Text);
                    this.SetFormProperty();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }

                
            }
            catch (Exception)
            {
                MessageBox.Show("Failure!", this.Text);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.ShowPropertyToForm();
            this.Close();
        }

        private void btnTestConnectionIMap4_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtHostIMap4.Text) && !string.IsNullOrEmpty(this.cBPortIMap4.Text))
            {
                try
                {
                    using (var client = new ImapClient())
                    {
                        client.Connect(this.txtHostIMap4.Text, int.Parse(this.cBPortIMap4.Text), this.ckBSSLIMap4.Checked);
                        client.Authenticate(this.txtEmailAddress.Text, this.txtEmailPassword.Text);
                        MessageBox.Show("Server Connect Successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
            }
            else
            {
                MessageBox.Show("Please input HostIMap4 and PortIMap4", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            }
        }

        private void btnTestConnectionSMTP_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtHostSMTP.Text) && !string.IsNullOrEmpty(this.cBPortSMTP.Text))
            {
                try
                {
                    using (var client = new SmtpClient())
                    {
                        client.Connect(this.txtHostSMTP.Text, int.Parse(this.cBPortSMTP.Text), this.ckBSSLSMTP.Checked);
                        client.Authenticate(this.txtEmailAddress.Text, this.txtEmailPassword.Text);
                        MessageBox.Show("Server Connect Successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
            }
            else
            {
                MessageBox.Show("Please input HostSMTP and PortSMTP", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            }
        }

        private void btnTestConnectionDatabase_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(this.txtServerName.Text) && !string.IsNullOrEmpty(this.txtDatabase.Text))
            {
                string connstr;
                if (!string.IsNullOrEmpty(this.txtUserName.Text))
                {
                    connstr = "data source=" + this.txtServerName.Text + ";initial catalog=" + this.txtDatabase.Text + ";user id=" + this.txtUserName.Text + ";password=" + this.txtPassword.Text;
                }
                else
                {
                    connstr = "data source=" + this.txtServerName.Text + ";initial catalog=" + this.txtDatabase.Text + ";Integrated Security=true";
                }

                try
                {
                    using (SqlConnection conn = new SqlConnection(connstr))
                    {
                        conn.Open();
                        MessageBox.Show("Server Connect Successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
            }
            else
            {
                MessageBox.Show("Please input ServerName and Database", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            }
        }

        private void cBPortIMap4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtRecieveInterval_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnBrowseFile_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;
            // Show the FolderBrowserDialog.  
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtSavePath.Text = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
            }
        }

        #endregion

        #region Method

        /// <summary>
        /// Clear
        /// </summary>
        private void Clear()
        {
            this.txtServerName.Text = string.Empty;
            this.txtUserName.Text = string.Empty;
            this.txtPassword.Text = string.Empty;
            this.txtDatabase.Text = string.Empty;

            this.txtEmailAddress.Text = string.Empty;
            this.txtEmailPassword.Text = string.Empty;
            this.txtReceiveInterval.Text = string.Empty;
            this.txtHostSMTP.Text = string.Empty;
            this.txtHostIMap4.Text = string.Empty;
            this.cBPortIMap4.Text = string.Empty;
            this.cBPortSMTP.Text = string.Empty;
            this.txtSavePath.Text = string.Empty;
            this.ckBSSLIMap4.Checked = false;
            this.ckBSSLSMTP.Checked = false;
        }

        /// <summary>
        /// Read File
        /// </summary>
        private void ReadFile()
        {
            //Check exists file
            if (File.Exists(Path.Combine(Application.StartupPath, FILE_NAME)))
            {
                //Read file
                using (StreamReader sr = new StreamReader(Path.Combine(Application.StartupPath, FILE_NAME)))
                {
                    while (sr.Peek() >= 0)
                    {
                        string[] val = sr.ReadLine().Split('=');
                        switch (val[0])
                        {
                            case SERVER_NAME:
                                this.txtServerName.Text = val[1];

                                break;

                            case USER_NAME:
                                this.txtUserName.Text = val[1];

                                break;

                            case PASSWORD:
                                this.txtPassword.Text = val[1];

                                break;

                            case DATABASE:
                                this.txtDatabase.Text = val[1];

                                break;

                            case EMAIL_ADDRESS:
                                this.txtEmailAddress.Text = val[1];

                                break;

                            case EMAIL_PASSWORD:
                                this.txtEmailPassword.Text = val[1];

                                break;

                            case RECEIVE_INTERVAL:
                                this.txtReceiveInterval.Text = val[1];

                                break;

                            case HOSTIMAP4:
                                this.txtHostIMap4.Text = val[1];

                                break;

                            case PORTIMAP4:
                                this.cBPortIMap4.Text = val[1];

                                break;

                            case HOSTSMTP:
                                this.txtHostSMTP.Text = val[1];

                                break;

                            case PORTSMTP:
                                this.cBPortSMTP.Text = val[1];

                                break;

                            case SAVEPATH:
                                this.txtSavePath.Text = val[1];

                                break;

                            case SSLIMAP4:
                                if(val[1] == "1")
                                {
                                    this.ckBSSLIMap4.Checked = true;
                                }
                                else
                                {
                                    this.ckBSSLIMap4.Checked = false;
                                }

                                break;

                            case SSLSMTP:
                                if (val[1] == "1")
                                {
                                    this.ckBSSLSMTP.Checked = true;
                                }
                                else
                                {
                                    this.ckBSSLSMTP.Checked = false;
                                }

                                break;

                            default: break;
                        }
                    }
                }

                this.SetFormProperty();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetFormProperty()
        {
            this.ServerName = this.txtServerName.Text.Trim();
            this.UserName = this.txtUserName.Text.Trim();
            this.Password = this.txtPassword.Text.Trim();
            this.DataBase = this.txtDatabase.Text.Trim();

            this.EmailAddress = this.txtEmailAddress.Text.Trim();
            this.EmailPassword = this.txtEmailPassword.Text.Trim();
            this.ReceiveInterval = this.txtReceiveInterval.Text.Trim();
            this.HostIMap4 = this.txtHostIMap4.Text.Trim();
            this.PortIMap4 = this.cBPortIMap4.Text.Trim();
            this.HostSMTP = this.txtHostSMTP.Text.Trim();
            this.PortSMTP = this.cBPortSMTP.Text.Trim();
            this.SavePath = this.txtSavePath.Text.Trim();
            this.SslIMap4 = this.ckBSSLIMap4.Checked;
            this.SslSMTP = this.ckBSSLSMTP.Checked;
        }

        private void ShowPropertyToForm()
        {
            this.txtServerName.Text = this.ServerName;
            this.txtUserName.Text = this.UserName;
            this.txtPassword.Text = this.Password;
            this.txtDatabase.Text = this.DataBase;

            this.txtEmailAddress.Text = this.EmailAddress;
            this.txtEmailPassword.Text = this.EmailPassword;
            this.txtReceiveInterval.Text = this.ReceiveInterval;
            this.txtHostIMap4.Text = this.HostIMap4;
            this.cBPortIMap4.Text = this.PortIMap4;
            this.txtHostSMTP.Text = this.HostSMTP;
            this.cBPortSMTP.Text = this.PortSMTP;
            this.txtSavePath.Text = this.SavePath;
            this.ckBSSLIMap4.Checked = this.SslIMap4;
            this.ckBSSLSMTP.Checked = this.SslSMTP;

        }

        private void changeSSLIMap4(object sender, EventArgs e)
        {
            if(this.ckBSSLIMap4.Checked == true)
            {
                this.cBPortIMap4.SelectedItem = "993";
            }
            else
            {
                this.cBPortIMap4.SelectedItem = "143";
            }
        }

        private void changeSSLSMTP(object sender, EventArgs e)
        {
            if (this.ckBSSLSMTP.Checked == true)
            {
                this.cBPortSMTP.SelectedItem = "465";
            }
            else if(changeCBSMTP == false)
            {
                this.cBPortSMTP.SelectedItem = "25";
            }
            changeCBSMTP = false;
        }

        private void cBPortIMap4_SelectedValueChanged(object sender, EventArgs e)
        {
            if(this.cBPortIMap4.SelectedItem.ToString() == "993")
            {
                this.ckBSSLIMap4.Checked = true;
            }
            else
            {
                this.ckBSSLIMap4.Checked = false;
            }
        }

        private void cBPortSMTP_SelectedValueChanged(object sender, EventArgs e)
        {
            this.changeCBSMTP = true;
            if(this.cBPortSMTP.SelectedItem.ToString() == "465")
            {
                this.ckBSSLSMTP.Checked = true;
            }
            else
            {
                this.ckBSSLSMTP.Checked = false;
            }
        }
        #endregion
    }
}
