namespace AutoProcessMail
{
    partial class frmSetting
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
            this.gbDatabase = new System.Windows.Forms.GroupBox();
            this.btnTestConnectionDatabase = new System.Windows.Forms.Button();
            this.txtDatabase = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtServerName = new System.Windows.Forms.TextBox();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.gbMail = new System.Windows.Forms.GroupBox();
            this.btnBrowseFile = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.btnTestConnectionIMap4 = new System.Windows.Forms.Button();
            this.btnTestConnectionSMTP = new System.Windows.Forms.Button();
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.gbDatabase.SuspendLayout();
            this.gbMail.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbDatabase
            // 
            this.gbDatabase.Controls.Add(this.btnTestConnectionDatabase);
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
            this.gbDatabase.Size = new System.Drawing.Size(663, 153);
            this.gbDatabase.TabIndex = 0;
            this.gbDatabase.TabStop = false;
            this.gbDatabase.Text = "Database Setting";
            // 
            // btnTestConnectionDatabase
            // 
            this.btnTestConnectionDatabase.Location = new System.Drawing.Point(449, 62);
            this.btnTestConnectionDatabase.Name = "btnTestConnectionDatabase";
            this.btnTestConnectionDatabase.Size = new System.Drawing.Size(102, 25);
            this.btnTestConnectionDatabase.TabIndex = 4;
            this.btnTestConnectionDatabase.Text = "&Test Connection";
            this.btnTestConnectionDatabase.UseVisualStyleBackColor = true;
            this.btnTestConnectionDatabase.Click += new System.EventHandler(this.btnTestConnectionDatabase_Click);
            // 
            // txtDatabase
            // 
            this.txtDatabase.Location = new System.Drawing.Point(138, 117);
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.Size = new System.Drawing.Size(200, 19);
            this.txtDatabase.TabIndex = 3;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(138, 86);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(200, 19);
            this.txtPassword.TabIndex = 2;
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(138, 56);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(200, 19);
            this.txtUserName.TabIndex = 1;
            // 
            // txtServerName
            // 
            this.txtServerName.Location = new System.Drawing.Point(138, 25);
            this.txtServerName.Name = "txtServerName";
            this.txtServerName.Size = new System.Drawing.Size(200, 19);
            this.txtServerName.TabIndex = 0;
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
            this.gbMail.Controls.Add(this.btnBrowseFile);
            this.gbMail.Controls.Add(this.label15);
            this.gbMail.Controls.Add(this.label14);
            this.gbMail.Controls.Add(this.btnTestConnectionIMap4);
            this.gbMail.Controls.Add(this.btnTestConnectionSMTP);
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
            this.gbMail.Size = new System.Drawing.Size(660, 250);
            this.gbMail.TabIndex = 1;
            this.gbMail.TabStop = false;
            this.gbMail.Text = "Mail Setting";
            // 
            // btnBrowseFile
            // 
            this.btnBrowseFile.Location = new System.Drawing.Point(547, 113);
            this.btnBrowseFile.Name = "btnBrowseFile";
            this.btnBrowseFile.Size = new System.Drawing.Size(81, 23);
            this.btnBrowseFile.TabIndex = 4;
            this.btnBrowseFile.Text = "&Browse File";
            this.btnBrowseFile.UseVisualStyleBackColor = true;
            this.btnBrowseFile.Click += new System.EventHandler(this.btnBrowseFile_Click);
            // 
            // label15
            // 
            this.label15.Font = new System.Drawing.Font("Tahoma", 9F);
            this.label15.Location = new System.Drawing.Point(341, 85);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(114, 17);
            this.label15.TabIndex = 70;
            this.label15.Text = "Minute";
            // 
            // label14
            // 
            this.label14.Font = new System.Drawing.Font("Tahoma", 9F);
            this.label14.Location = new System.Drawing.Point(544, 142);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(105, 17);
            this.label14.TabIndex = 69;
            this.label14.Text = "Test Connection";
            // 
            // btnTestConnectionIMap4
            // 
            this.btnTestConnectionIMap4.Location = new System.Drawing.Point(547, 162);
            this.btnTestConnectionIMap4.Name = "btnTestConnectionIMap4";
            this.btnTestConnectionIMap4.Size = new System.Drawing.Size(81, 25);
            this.btnTestConnectionIMap4.TabIndex = 8;
            this.btnTestConnectionIMap4.Text = "Test &IMAP4";
            this.btnTestConnectionIMap4.UseVisualStyleBackColor = true;
            this.btnTestConnectionIMap4.Click += new System.EventHandler(this.btnTestConnectionIMap4_Click);
            // 
            // btnTestConnectionSMTP
            // 
            this.btnTestConnectionSMTP.Location = new System.Drawing.Point(547, 199);
            this.btnTestConnectionSMTP.Name = "btnTestConnectionSMTP";
            this.btnTestConnectionSMTP.Size = new System.Drawing.Size(81, 25);
            this.btnTestConnectionSMTP.TabIndex = 12;
            this.btnTestConnectionSMTP.Text = "Test &SMTP";
            this.btnTestConnectionSMTP.UseVisualStyleBackColor = true;
            this.btnTestConnectionSMTP.Click += new System.EventHandler(this.btnTestConnectionSMTP_Click);
            // 
            // txtSavePath
            // 
            this.txtSavePath.Location = new System.Drawing.Point(135, 114);
            this.txtSavePath.Name = "txtSavePath";
            this.txtSavePath.Size = new System.Drawing.Size(406, 19);
            this.txtSavePath.TabIndex = 3;
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label13.Location = new System.Drawing.Point(11, 116);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(114, 17);
            this.label13.TabIndex = 65;
            this.label13.Text = "Save Path";
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(132, 142);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(137, 17);
            this.label12.TabIndex = 64;
            this.label12.Text = "Server Host Name";
            // 
            // ckBSSLSMTP
            // 
            this.ckBSSLSMTP.AutoSize = true;
            this.ckBSSLSMTP.Location = new System.Drawing.Point(483, 205);
            this.ckBSSLSMTP.Name = "ckBSSLSMTP";
            this.ckBSSLSMTP.Size = new System.Drawing.Size(15, 14);
            this.ckBSSLSMTP.TabIndex = 11;
            this.ckBSSLSMTP.UseVisualStyleBackColor = true;
            this.ckBSSLSMTP.CheckedChanged += new System.EventHandler(this.changeSSLSMTP);
            // 
            // cBPortSMTP
            // 
            this.cBPortSMTP.FormattingEnabled = true;
            this.cBPortSMTP.Items.AddRange(new object[] {
            "587",
            "25",
            "465"});
            this.cBPortSMTP.Location = new System.Drawing.Point(368, 199);
            this.cBPortSMTP.MaxLength = 5;
            this.cBPortSMTP.Name = "cBPortSMTP";
            this.cBPortSMTP.Size = new System.Drawing.Size(87, 20);
            this.cBPortSMTP.TabIndex = 10;
            this.cBPortSMTP.SelectedValueChanged += new System.EventHandler(this.cBPortSMTP_SelectedValueChanged);
            this.cBPortSMTP.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cBPortIMap4_KeyPress);
            // 
            // cBPortIMap4
            // 
            this.cBPortIMap4.FormattingEnabled = true;
            this.cBPortIMap4.Items.AddRange(new object[] {
            "143",
            "993"});
            this.cBPortIMap4.Location = new System.Drawing.Point(368, 161);
            this.cBPortIMap4.MaxLength = 5;
            this.cBPortIMap4.Name = "cBPortIMap4";
            this.cBPortIMap4.Size = new System.Drawing.Size(87, 20);
            this.cBPortIMap4.TabIndex = 6;
            this.cBPortIMap4.SelectedValueChanged += new System.EventHandler(this.cBPortIMap4_SelectedValueChanged);
            this.cBPortIMap4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cBPortIMap4_KeyPress);
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Tahoma", 9F);
            this.label11.Location = new System.Drawing.Point(480, 143);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(33, 17);
            this.label11.TabIndex = 59;
            this.label11.Text = "SSL";
            // 
            // ckBSSLIMap4
            // 
            this.ckBSSLIMap4.AutoSize = true;
            this.ckBSSLIMap4.Location = new System.Drawing.Point(483, 169);
            this.ckBSSLIMap4.Name = "ckBSSLIMap4";
            this.ckBSSLIMap4.Size = new System.Drawing.Size(15, 14);
            this.ckBSSLIMap4.TabIndex = 7;
            this.ckBSSLIMap4.UseVisualStyleBackColor = true;
            this.ckBSSLIMap4.CheckedChanged += new System.EventHandler(this.changeSSLIMap4);
            // 
            // txtHostSMTP
            // 
            this.txtHostSMTP.Location = new System.Drawing.Point(135, 200);
            this.txtHostSMTP.Name = "txtHostSMTP";
            this.txtHostSMTP.Size = new System.Drawing.Size(200, 19);
            this.txtHostSMTP.TabIndex = 9;
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Tahoma", 9F);
            this.label10.Location = new System.Drawing.Point(365, 143);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(114, 17);
            this.label10.TabIndex = 56;
            this.label10.Text = "Port";
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label9.Location = new System.Drawing.Point(11, 199);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(114, 17);
            this.label9.TabIndex = 55;
            this.label9.Text = "SMTP";
            // 
            // txtHostIMap4
            // 
            this.txtHostIMap4.Location = new System.Drawing.Point(135, 162);
            this.txtHostIMap4.Name = "txtHostIMap4";
            this.txtHostIMap4.Size = new System.Drawing.Size(200, 19);
            this.txtHostIMap4.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label8.Location = new System.Drawing.Point(11, 166);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(114, 17);
            this.label8.TabIndex = 53;
            this.label8.Text = "IMAP4";
            // 
            // txtReceiveInterval
            // 
            this.txtReceiveInterval.Location = new System.Drawing.Point(135, 83);
            this.txtReceiveInterval.MaxLength = 4;
            this.txtReceiveInterval.Name = "txtReceiveInterval";
            this.txtReceiveInterval.Size = new System.Drawing.Size(200, 19);
            this.txtReceiveInterval.TabIndex = 2;
            this.txtReceiveInterval.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtRecieveInterval_KeyPress);
            // 
            // txtEmailPassword
            // 
            this.txtEmailPassword.Location = new System.Drawing.Point(135, 54);
            this.txtEmailPassword.Name = "txtEmailPassword";
            this.txtEmailPassword.PasswordChar = '*';
            this.txtEmailPassword.Size = new System.Drawing.Size(200, 19);
            this.txtEmailPassword.TabIndex = 1;
            // 
            // txtEmailAddress
            // 
            this.txtEmailAddress.Location = new System.Drawing.Point(135, 24);
            this.txtEmailAddress.Name = "txtEmailAddress";
            this.txtEmailAddress.Size = new System.Drawing.Size(406, 19);
            this.txtEmailAddress.TabIndex = 0;
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
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(496, 435);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 27);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(598, 435);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 27);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(727, 474);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.gbMail);
            this.Controls.Add(this.gbDatabase);
            this.Name = "frmSetting";
            this.Text = "自動処理メール";
            this.Load += new System.EventHandler(this.frmSetting_Load);
            this.gbDatabase.ResumeLayout(false);
            this.gbDatabase.PerformLayout();
            this.gbMail.ResumeLayout(false);
            this.gbMail.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
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
        internal System.Windows.Forms.Button btnOK;
        internal System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox ckBSSLIMap4;
        internal System.Windows.Forms.TextBox txtHostSMTP;
        internal System.Windows.Forms.Label label10;
        internal System.Windows.Forms.Label label9;
        internal System.Windows.Forms.TextBox txtHostIMap4;
        internal System.Windows.Forms.Label label8;
        internal System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cBPortIMap4;
        internal System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox ckBSSLSMTP;
        private System.Windows.Forms.ComboBox cBPortSMTP;
        internal System.Windows.Forms.TextBox txtSavePath;
        internal System.Windows.Forms.Label label13;
        internal System.Windows.Forms.Button btnTestConnectionSMTP;
        internal System.Windows.Forms.Button btnTestConnectionDatabase;
        internal System.Windows.Forms.Button btnTestConnectionIMap4;
        internal System.Windows.Forms.Label label14;
        internal System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btnBrowseFile;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}

