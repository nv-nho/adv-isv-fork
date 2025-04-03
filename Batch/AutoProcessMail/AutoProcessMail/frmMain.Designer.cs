namespace AutoProcessMail
{
    partial class frmMain
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miShow = new System.Windows.Forms.ToolStripMenuItem();
            this.miClose = new System.Windows.Forms.ToolStripMenuItem();
            this.gbDatabase = new System.Windows.Forms.GroupBox();
            this.txtDatabase = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtServerName = new System.Windows.Forms.TextBox();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.gbMail = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtSavePath = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.ckBSSLSMTP = new System.Windows.Forms.CheckBox();
            this.cBPortSMTP = new System.Windows.Forms.ComboBox();
            this.cBPortIMap4 = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.ckBSSLIMap4 = new System.Windows.Forms.CheckBox();
            this.txtHostSMTP = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtHostIMap4 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtReceiveInterval = new System.Windows.Forms.TextBox();
            this.txtEmailPassword = new System.Windows.Forms.TextBox();
            this.txtEmailAddress = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnSetting = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.tmRecieveMail = new System.Windows.Forms.Timer(this.components);
            this.tmReSendMail = new System.Windows.Forms.Timer(this.components);
            this.tmGetResendData = new System.Windows.Forms.Timer(this.components);
            this.btnRun = new System.Windows.Forms.Button();
            this.contextMenuStrip.SuspendLayout();
            this.gbDatabase.SuspendLayout();
            this.gbMail.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipText = "メール受信して,条件設定により再送信処理します。";
            this.notifyIcon.BalloonTipTitle = "自動処理メール";
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "自動処理メール";
            this.notifyIcon.Visible = true;
            this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_Click);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miShow,
            this.miClose});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(105, 48);
            // 
            // miShow
            // 
            this.miShow.Name = "miShow";
            this.miShow.Size = new System.Drawing.Size(152, 22);
            this.miShow.Text = "表示";
            this.miShow.Click += new System.EventHandler(this.miShow_Click);
            // 
            // miClose
            // 
            this.miClose.Name = "miClose";
            this.miClose.Size = new System.Drawing.Size(152, 22);
            this.miClose.Text = "閉じる";
            this.miClose.Click += new System.EventHandler(this.miClose_Click);
            // 
            // gbDatabase
            // 
            this.gbDatabase.Controls.Add(this.txtDatabase);
            this.gbDatabase.Controls.Add(this.txtPassword);
            this.gbDatabase.Controls.Add(this.txtUserName);
            this.gbDatabase.Controls.Add(this.txtServerName);
            this.gbDatabase.Controls.Add(this.Label4);
            this.gbDatabase.Controls.Add(this.Label3);
            this.gbDatabase.Controls.Add(this.Label2);
            this.gbDatabase.Controls.Add(this.Label1);
            this.gbDatabase.Location = new System.Drawing.Point(10, 10);
            this.gbDatabase.Name = "gbDatabase";
            this.gbDatabase.Size = new System.Drawing.Size(359, 153);
            this.gbDatabase.TabIndex = 1;
            this.gbDatabase.TabStop = false;
            this.gbDatabase.Text = "Database Setting";
            // 
            // txtDatabase
            // 
            this.txtDatabase.Enabled = false;
            this.txtDatabase.Location = new System.Drawing.Point(138, 118);
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.Size = new System.Drawing.Size(200, 19);
            this.txtDatabase.TabIndex = 37;
            // 
            // txtPassword
            // 
            this.txtPassword.Enabled = false;
            this.txtPassword.Location = new System.Drawing.Point(138, 86);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(200, 19);
            this.txtPassword.TabIndex = 36;
            // 
            // txtUserName
            // 
            this.txtUserName.Enabled = false;
            this.txtUserName.Location = new System.Drawing.Point(138, 51);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(200, 19);
            this.txtUserName.TabIndex = 35;
            // 
            // txtServerName
            // 
            this.txtServerName.Enabled = false;
            this.txtServerName.Location = new System.Drawing.Point(138, 20);
            this.txtServerName.Name = "txtServerName";
            this.txtServerName.Size = new System.Drawing.Size(200, 19);
            this.txtServerName.TabIndex = 34;
            // 
            // Label4
            // 
            this.Label4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.Label4.Location = new System.Drawing.Point(14, 118);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(92, 14);
            this.Label4.TabIndex = 40;
            this.Label4.Text = "Database";
            // 
            // Label3
            // 
            this.Label3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.Label3.Location = new System.Drawing.Point(14, 86);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(92, 14);
            this.Label3.TabIndex = 41;
            this.Label3.Text = "Password";
            // 
            // Label2
            // 
            this.Label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.Label2.Location = new System.Drawing.Point(14, 56);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(92, 14);
            this.Label2.TabIndex = 38;
            this.Label2.Text = "User Name";
            // 
            // Label1
            // 
            this.Label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.Label1.Location = new System.Drawing.Point(14, 25);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(92, 14);
            this.Label1.TabIndex = 39;
            this.Label1.Text = "Server Name";
            // 
            // gbMail
            // 
            this.gbMail.Controls.Add(this.label15);
            this.gbMail.Controls.Add(this.txtSavePath);
            this.gbMail.Controls.Add(this.label13);
            this.gbMail.Controls.Add(this.label12);
            this.gbMail.Controls.Add(this.ckBSSLSMTP);
            this.gbMail.Controls.Add(this.cBPortSMTP);
            this.gbMail.Controls.Add(this.cBPortIMap4);
            this.gbMail.Controls.Add(this.label11);
            this.gbMail.Controls.Add(this.ckBSSLIMap4);
            this.gbMail.Controls.Add(this.txtHostSMTP);
            this.gbMail.Controls.Add(this.label10);
            this.gbMail.Controls.Add(this.label9);
            this.gbMail.Controls.Add(this.txtHostIMap4);
            this.gbMail.Controls.Add(this.label8);
            this.gbMail.Controls.Add(this.txtReceiveInterval);
            this.gbMail.Controls.Add(this.txtEmailPassword);
            this.gbMail.Controls.Add(this.txtEmailAddress);
            this.gbMail.Controls.Add(this.label7);
            this.gbMail.Controls.Add(this.label6);
            this.gbMail.Controls.Add(this.label5);
            this.gbMail.Location = new System.Drawing.Point(13, 179);
            this.gbMail.Name = "gbMail";
            this.gbMail.Size = new System.Drawing.Size(558, 233);
            this.gbMail.TabIndex = 2;
            this.gbMail.TabStop = false;
            this.gbMail.Text = "Mail Setting";
            // 
            // label15
            // 
            this.label15.Font = new System.Drawing.Font("Tahoma", 9F);
            this.label15.Location = new System.Drawing.Point(341, 83);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(114, 17);
            this.label15.TabIndex = 78;
            this.label15.Text = "Minute";
            // 
            // txtSavePath
            // 
            this.txtSavePath.Enabled = false;
            this.txtSavePath.Location = new System.Drawing.Point(135, 115);
            this.txtSavePath.Name = "txtSavePath";
            this.txtSavePath.Size = new System.Drawing.Size(406, 19);
            this.txtSavePath.TabIndex = 77;
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label13.Location = new System.Drawing.Point(11, 116);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(114, 17);
            this.label13.TabIndex = 76;
            this.label13.Text = "Save Path";
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(132, 147);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(137, 17);
            this.label12.TabIndex = 75;
            this.label12.Text = "Server Host Name";
            // 
            // ckBSSLSMTP
            // 
            this.ckBSSLSMTP.AutoSize = true;
            this.ckBSSLSMTP.Enabled = false;
            this.ckBSSLSMTP.Location = new System.Drawing.Point(483, 200);
            this.ckBSSLSMTP.Name = "ckBSSLSMTP";
            this.ckBSSLSMTP.Size = new System.Drawing.Size(15, 14);
            this.ckBSSLSMTP.TabIndex = 74;
            this.ckBSSLSMTP.UseVisualStyleBackColor = true;
            // 
            // cBPortSMTP
            // 
            this.cBPortSMTP.Enabled = false;
            this.cBPortSMTP.FormattingEnabled = true;
            this.cBPortSMTP.Items.AddRange(new object[] {
            "587",
            "25",
            "465"});
            this.cBPortSMTP.Location = new System.Drawing.Point(374, 194);
            this.cBPortSMTP.Name = "cBPortSMTP";
            this.cBPortSMTP.Size = new System.Drawing.Size(81, 20);
            this.cBPortSMTP.TabIndex = 73;
            // 
            // cBPortIMap4
            // 
            this.cBPortIMap4.Enabled = false;
            this.cBPortIMap4.FormattingEnabled = true;
            this.cBPortIMap4.Items.AddRange(new object[] {
            "110",
            "995"});
            this.cBPortIMap4.Location = new System.Drawing.Point(374, 168);
            this.cBPortIMap4.Name = "cBPortIMap4";
            this.cBPortIMap4.Size = new System.Drawing.Size(81, 20);
            this.cBPortIMap4.TabIndex = 72;
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Tahoma", 9F);
            this.label11.Location = new System.Drawing.Point(480, 148);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(114, 17);
            this.label11.TabIndex = 71;
            this.label11.Text = "SSL";
            // 
            // ckBSSLIMap4
            // 
            this.ckBSSLIMap4.AutoSize = true;
            this.ckBSSLIMap4.Enabled = false;
            this.ckBSSLIMap4.Location = new System.Drawing.Point(483, 174);
            this.ckBSSLIMap4.Name = "ckBSSLIMap4";
            this.ckBSSLIMap4.Size = new System.Drawing.Size(15, 14);
            this.ckBSSLIMap4.TabIndex = 70;
            this.ckBSSLIMap4.UseVisualStyleBackColor = true;
            // 
            // txtHostSMTP
            // 
            this.txtHostSMTP.Enabled = false;
            this.txtHostSMTP.Location = new System.Drawing.Point(135, 195);
            this.txtHostSMTP.Name = "txtHostSMTP";
            this.txtHostSMTP.Size = new System.Drawing.Size(200, 19);
            this.txtHostSMTP.TabIndex = 69;
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Tahoma", 9F);
            this.label10.Location = new System.Drawing.Point(371, 148);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(114, 17);
            this.label10.TabIndex = 68;
            this.label10.Text = "Port";
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label9.Location = new System.Drawing.Point(11, 194);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(114, 17);
            this.label9.TabIndex = 67;
            this.label9.Text = "SMTP";
            // 
            // txtHostIMap4
            // 
            this.txtHostIMap4.Enabled = false;
            this.txtHostIMap4.Location = new System.Drawing.Point(135, 167);
            this.txtHostIMap4.Name = "txtHostIMap4";
            this.txtHostIMap4.Size = new System.Drawing.Size(200, 19);
            this.txtHostIMap4.TabIndex = 66;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label8.Location = new System.Drawing.Point(11, 171);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(114, 17);
            this.label8.TabIndex = 65;
            this.label8.Text = "IMAP4";
            // 
            // txtReceiveInterval
            // 
            this.txtReceiveInterval.Enabled = false;
            this.txtReceiveInterval.Location = new System.Drawing.Point(135, 83);
            this.txtReceiveInterval.Name = "txtReceiveInterval";
            this.txtReceiveInterval.Size = new System.Drawing.Size(200, 19);
            this.txtReceiveInterval.TabIndex = 46;
            // 
            // txtEmailPassword
            // 
            this.txtEmailPassword.Enabled = false;
            this.txtEmailPassword.Location = new System.Drawing.Point(135, 54);
            this.txtEmailPassword.Name = "txtEmailPassword";
            this.txtEmailPassword.PasswordChar = '*';
            this.txtEmailPassword.Size = new System.Drawing.Size(200, 19);
            this.txtEmailPassword.TabIndex = 45;
            // 
            // txtEmailAddress
            // 
            this.txtEmailAddress.Enabled = false;
            this.txtEmailAddress.Location = new System.Drawing.Point(135, 24);
            this.txtEmailAddress.Name = "txtEmailAddress";
            this.txtEmailAddress.Size = new System.Drawing.Size(406, 19);
            this.txtEmailAddress.TabIndex = 42;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(11, 83);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(114, 17);
            this.label7.TabIndex = 44;
            this.label7.Text = "Receive Interval";
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(11, 54);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(114, 19);
            this.label6.TabIndex = 43;
            this.label6.Text = "Email Password";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(11, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(114, 14);
            this.label5.TabIndex = 42;
            this.label5.Text = "Email Address";
            // 
            // btnSetting
            // 
            this.btnSetting.Location = new System.Drawing.Point(436, 68);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(75, 27);
            this.btnSetting.TabIndex = 1;
            this.btnSetting.Text = "&Setting";
            this.btnSetting.UseVisualStyleBackColor = true;
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(436, 101);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 27);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tmRecieveMail
            // 
            this.tmRecieveMail.Interval = 5000;
            this.tmRecieveMail.Tick += new System.EventHandler(this.tmRecieveMail_Tick);
            // 
            // tmReSendMail
            // 
            this.tmReSendMail.Interval = 5000;
            this.tmReSendMail.Tick += new System.EventHandler(this.tmReSendMail_Tick);
            // 
            // tmGetResendData
            // 
            this.tmGetResendData.Tick += new System.EventHandler(this.tmGetResendData_Tick);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(436, 35);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 27);
            this.btnRun.TabIndex = 0;
            this.btnRun.Text = "&Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 436);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSetting);
            this.Controls.Add(this.gbMail);
            this.Controls.Add(this.gbDatabase);
            this.Name = "frmMain";
            this.Text = "自動処理メール";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.contextMenuStrip.ResumeLayout(false);
            this.gbDatabase.ResumeLayout(false);
            this.gbDatabase.PerformLayout();
            this.gbMail.ResumeLayout(false);
            this.gbMail.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem miShow;
        private System.Windows.Forms.ToolStripMenuItem miClose;
        private System.Windows.Forms.GroupBox gbDatabase;
        internal System.Windows.Forms.TextBox txtDatabase;
        internal System.Windows.Forms.TextBox txtPassword;
        internal System.Windows.Forms.TextBox txtUserName;
        internal System.Windows.Forms.TextBox txtServerName;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label Label1;
        private System.Windows.Forms.GroupBox gbMail;
        internal System.Windows.Forms.TextBox txtReceiveInterval;
        internal System.Windows.Forms.TextBox txtEmailPassword;
        internal System.Windows.Forms.TextBox txtEmailAddress;
        internal System.Windows.Forms.Label label7;
        internal System.Windows.Forms.Label label6;
        internal System.Windows.Forms.Label label5;
        internal System.Windows.Forms.Button btnSetting;
        internal System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Timer tmRecieveMail;
        private System.Windows.Forms.Timer tmReSendMail;
        private System.Windows.Forms.Timer tmGetResendData;
        internal System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox ckBSSLSMTP;
        private System.Windows.Forms.ComboBox cBPortSMTP;
        private System.Windows.Forms.ComboBox cBPortIMap4;
        internal System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox ckBSSLIMap4;
        internal System.Windows.Forms.TextBox txtHostSMTP;
        internal System.Windows.Forms.Label label10;
        internal System.Windows.Forms.Label label9;
        internal System.Windows.Forms.TextBox txtHostIMap4;
        internal System.Windows.Forms.Label label8;
        internal System.Windows.Forms.TextBox txtSavePath;
        internal System.Windows.Forms.Label label13;
        internal System.Windows.Forms.Button btnRun;
        internal System.Windows.Forms.Label label15;
    }
}

