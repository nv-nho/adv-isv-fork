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
using System.Reflection;

namespace AutoProcessMail
{
    public partial class frmMain : Form
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
        private const string ISRUN = "IsRun";
        private const string APP_NAME = "AutoProcessMail";
        private const string RESENT_TIME = "ResentTime";

        #endregion

        #region Variable

        private bool isClose = false;
        RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

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
        public DateTime? ResentTime { get; set; }
        public Boolean SslIMap4 { get; set; }
        public Boolean SslSMTP { get; set; }
        public List<MailStruct> ResendMailList { get; set; }
        public Boolean IsRun = false;

        public struct MailStruct
        {
            public int ID;
            public int ResendTime;
        }

        #endregion

        #region Contructor

        public frmMain()
        {
            InitializeComponent();
        }

        #endregion

        #region "Event"
        private void frmMain_Load(object sender, EventArgs e)
        {
            if (rkApp.GetValue(APP_NAME) == null)
            {
                rkApp.SetValue(APP_NAME, Assembly.GetExecutingAssembly().Location);
                rkApp.Close();
            }
            //Clear
            this.Clear();

            //Read File
            this.ReadFile();
        }

        private void tmGetResendData_Tick(object sender, EventArgs e)
        {
            this.GetReSendData();
            this.ProcessResend();
        }

        private void tmReSendMail_Tick(object sender, EventArgs e)
        {
            this.ProcessResend();
        }

        private void tmRecieveMail_Tick(object sender, EventArgs e)
        {
            Process compiler = new Process();
            compiler.StartInfo.FileName = Path.Combine(Application.StartupPath, "MailProcess.exe");
            compiler.StartInfo.Arguments = "0";
            compiler.StartInfo.UseShellExecute = false;
            compiler.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isClose)
            {
                Hide();
                notifyIcon.Visible = true;
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Click Setting Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetting_Click(object sender, EventArgs e)
        {
            frmSetting setting = new frmSetting();
            var result = setting.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.ReadFile();
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.ShowPropertyToForm();
            this.Close();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            IsRun = !IsRun;
            SetRunStop(IsRun);
        }

        private void SetRunStop(Boolean IsRun)
        {
            if (IsRun == true)
            {
                this.GetReSendData();
                this.ProcessResend();
                int interval = int.TryParse(this.ReceiveInterval, out interval) ? interval : 1;
                this.tmRecieveMail.Interval = interval * 60000;
                this.tmRecieveMail.Enabled = true;
                this.btnRun.Text = "Stop";
                Hide();
                notifyIcon.Visible = true;
            }
            else
            {
                tmGetResendData.Enabled = false;
                tmRecieveMail.Enabled = false;
                tmReSendMail.Enabled = false;
                this.btnRun.Text = "Run";
            }

            List<string> line = File.ReadAllLines(Path.Combine(Application.StartupPath,FILE_NAME)).ToList();
            int isRunLine = line.FindIndex(l => l.Contains(ISRUN));
            if(isRunLine >= 0)
            {
                line[isRunLine] = string.Format("{0}={1}", ISRUN, IsRun ? 1 : 0);
            }
            else
            {
                line.Add((string.Format("{0}={1}", ISRUN, IsRun ? 1 : 0)));
            }
            File.WriteAllLines(Path.Combine(Application.StartupPath, FILE_NAME), line);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void miClose_Click(object sender, EventArgs e)
        {
            List<string> line = File.ReadAllLines(Path.Combine(Application.StartupPath, FILE_NAME)).ToList();
            int isRunLine = line.FindIndex(l => l.Contains(ISRUN));
            line[isRunLine] = string.Format("{0}={1}", ISRUN, 0);
            File.WriteAllLines(Path.Combine(Application.StartupPath, FILE_NAME), line);
            isClose = true;
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon_Click(object sender, EventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void miShow_Click(object sender, EventArgs e)
        {
            this.notifyIcon_Click(sender, e);
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
                                if (val[1] == "1")
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

                            case ISRUN:
                                if (val[1] == "1")
                                {
                                    this.IsRun = true;
                                }
                                else
                                {
                                    this.IsRun = false;
                                }
                                break;

                            case RESENT_TIME:
                                this.ResentTime = DateTime.Parse(val[1]);

                                break;

                            default: break;
                        }
                    }
                }
                this.SetFormProperty();
                SetRunStop(this.IsRun);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetFormProperty()
        {
            this.tmRecieveMail.Enabled = false;
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

        private void GetReSendData()
        {
            tmGetResendData.Enabled = false;

            string connstr;
            if (!string.IsNullOrEmpty(this.UserName))
            {
                connstr = "data source=" + this.ServerName + ";initial catalog=" + this.DataBase + ";user id=" + this.UserName + ";password=" + this.Password;
            }
            else
            {
                connstr = "data source=" + this.ServerName + ";initial catalog=" + this.DataBase + ";Integrated Security=true";
            }
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                ResendMailList = new List<MailStruct>();
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT A.ID, A.ResendTime");
                sql.AppendLine("FROM");
                sql.AppendLine("(");
                sql.AppendLine("SELECT H.ID");
                sql.AppendLine(", CASE BaseDivision WHEN 0 THEN cast(CreateDate AS date) ELSE ReplyDueDate END AS BaseDate");
                sql.AppendLine(", H.ResendInterval");
                sql.AppendLine(", H.StartDate");
                sql.AppendLine(", H.StartDivision");
                sql.AppendLine(", H.EndDate");
                sql.AppendLine(", H.BaseDivision");
                sql.AppendLine(", H.EndDivision");
                sql.AppendLine(", H.ResendTime");
                sql.AppendLine("FROM [dbo].[T_Mail_H] H");
                sql.AppendLine("WHERE EXISTS (SELECT 1 ");
                sql.AppendLine("FROM [dbo].T_Mail_D D");
                sql.AppendLine("WHERE D.HID = H.ID");
                sql.AppendLine("AND D.ReceiveFlag = 0)");
                sql.AppendLine("AND H.ResendFlag = 1");
                sql.AppendLine("AND H.DraftFlag = 0");
                sql.AppendLine(") A");

                sql.AppendLine("WHERE DATEDIFF ( DAY , DATEADD(DAY,  CASE StartDivision WHEN 0 THEN - 1 ELSE 1 END * StartDate , BaseDate) , GETDATE())  % (A.ResendInterval + 1) = 0");
                sql.AppendLine("AND cast(GETDATE() AS date) BETWEEN DATEADD(DAY,  CASE StartDivision WHEN 0 THEN - 1 ELSE 1 END * StartDate , BaseDate) AND DATEADD(DAY, CASE EndDivision WHEN 0 THEN - 1 ELSE 1 END * EndDate , BaseDate)");
                sql.AppendLine("ORDER BY A.ResendTime");

                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql.ToString();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            MailStruct newItem = new MailStruct();
                            newItem.ID = (int)dr["ID"];
                            newItem.ResendTime = (int)dr["ResendTime"];
                            ResendMailList.Add(newItem);
                        }
                    }
                }
            }

            DateTime nowDate = DateTime.Now;
            DateTime nextDate = new DateTime(nowDate.Year, nowDate.Month, nowDate.Day);
            nextDate = nextDate.AddDays(1);
            TimeSpan timeSpan = nextDate - nowDate;
            tmGetResendData.Interval = (int)(timeSpan.TotalSeconds * 1000);
            tmGetResendData.Enabled = true;
        }

        private void ProcessResend()
        {
            tmReSendMail.Enabled = false;
            int curTime = this.TimeToInt(DateTime.Now.ToString("HH:mm"));
            int processCount = 0;
            string mailIds = "";
            foreach (var item in this.ResendMailList)
            {
                if (!this.ResentTime.HasValue || this.ResentTime.Value.Date < DateTime.Now.Date)
                {
                    if (curTime >= item.ResendTime)
                    {
                        mailIds = mailIds + item.ID.ToString() + ",";
                        processCount += 1;
                    }
                }
                else
                {
                    if (curTime >= item.ResendTime)
                    {
                        if (this.TimeToInt(this.ResentTime.Value.ToString("HH:mm")) < item.ResendTime)
                        {
                            mailIds = mailIds + item.ID.ToString() + ",";
                        }
                        processCount += 1;
                    }
                }
            }
            if (!string.IsNullOrEmpty(mailIds))
            {
                this.ResentTime = DateTime.Now;
                List<string> line = File.ReadAllLines(Path.Combine(Application.StartupPath, FILE_NAME)).ToList();
                int isResentTimeLine = line.FindIndex(l => l.Contains(RESENT_TIME));
                if (isResentTimeLine >= 0)
                {
                    line[isResentTimeLine] = string.Format("{0}={1}", RESENT_TIME, this.ResentTime);
                }
                else
                {
                    line.Add((string.Format("{0}={1}", RESENT_TIME, this.ResentTime)));
                }
                File.WriteAllLines(Path.Combine(Application.StartupPath, FILE_NAME), line);

                Process compiler = new Process();
                compiler.StartInfo.FileName = Path.Combine(Application.StartupPath, "MailProcess.exe");
                compiler.StartInfo.Arguments = "1 " + mailIds.Trim(',');
                compiler.StartInfo.UseShellExecute = false;
                compiler.Start();
            }

            for (int i = 0; i < processCount; i++)
            {
                this.ResendMailList.RemoveAt(0);
            }

            if (this.ResendMailList.Count > 0)
            {
                tmReSendMail.Interval = (this.ResendMailList[0].ResendTime - curTime) * 6000;
                tmReSendMail.Enabled = true;
            }
        }

        /// <summary>
        /// TimeToInt
        /// </summary>
        /// <param name="pValue"></param>
        /// <returns></returns>
        private int TimeToInt(string pValue)
        {
            if (pValue == "")
                return 0;
            string[] ary = pValue.Split(':');
            int nHours = 0;
            int nMinutes = 0;

            if (ary.Length != 2)
            {
                return 0;
            }

            nHours = int.Parse(ary[0]);
            nMinutes = int.Parse(ary[1]);
            if (nMinutes < 0)
            {

                return 0;
            }

            return (nHours * 600 + nMinutes * 10);
        }

        #endregion

    }
}
