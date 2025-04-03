using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using OMS.Models;
using OMS.Utilities;
using OMS.DAC;
using OMS.Controls;
using MailKit;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.IO;
using MailKit.Net.Imap;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;


namespace OMS.Mail
{
    public partial class FrmMailEntry : FrmBaseDetail
    {
        #region Constants
        private const string CONST_FILE_PATH_TEMP = "~/TempAttachFile";
        private const string URL_LIST = "~/Mail/FrmMailList.aspx";
        private const string ADMINCD = "0000";
        private const string CONST_FILE_MAX_SIZE_STRING = "5 MB";
        private const string FOLDER_SEND_STRING = "sent";
        private const long CONST_FILE_MAX_SIZE = 5 * 1024 * 1024;
        private const string CONST_NM_REPLACE = "【社員名】";
        private const string FULL_DATE_TIME_FM = "{0:" + Constants.FMT_DATETIME_SHOW + "}";
        private const int HID_DEFAULT_CONTENT = 1;
        #endregion

        #region Property
        /// <summary>
        /// Get or set HID
        /// </summary>
        public int HID
        {
            get { return ViewState["ID"] == null ? 0 : (int)ViewState["ID"]; }
            set { ViewState["ID"] = value; }
        }

        /// <summary>
        /// show dialog
        /// </summary>
        public bool IsShowDialog
        {
            get { return (bool)ViewState["IsShowDialog"]; }
            set { ViewState["IsShowDialog"] = value; }
        }

        /// <summary>
        /// Flg select all
        /// </summary>
        public bool IsSelectAll
        {
            get { return (bool)ViewState["IsSelectAll"]; }
            set { ViewState["IsSelectAll"] = value; }
        }

        /// <summary>
        /// flg draft
        /// </summary>
        public int DraftMail
        {
            get { return ViewState["DraftMail"] == null ? 0 : (int)ViewState["DraftMail"]; }
            set { ViewState["DraftMail"] = value; }
        }

        /// <summary>
        /// PATH_SAVE
        /// </summary>
        private string DIR_PATH
        {
            get { return GetConfig.getStringValue("SAVE_PATH"); }
        }

        /// <summary>
        /// EXTMAIL
        /// </summary>
        private string EXT_MAIL
        {
            get { return GetConfig.getStringValue("EXTENSION_EMAIL"); }
        }

        /// <summary>
        /// MAIL_ID
        /// </summary>
        private string MAIL_ID
        {
            get { return GetConfig.getStringValue("MAIL_ID"); }
        }

        /// <summary>
        /// MAIL_NAME
        /// </summary>
        private string MAIL_NAME
        {
            get { return this.GetNameConfig(); }
        }

        /// <summary>
        /// MAIL_PASSWORD
        /// </summary>
        private string MAIL_PASSWORD
        {
            get { return GetConfig.getStringValue("MAIL_PASSWORD"); }
        }

        /// <summary>
        /// HOST_SMTP
        /// </summary>
        private string HOST_SMTP
        {
            get { return GetConfig.getStringValue("HOST_SMTP"); }
        }

        /// <summary>
        /// HOST_IMAP
        /// </summary>
        private string HOST_IMAP
        {
            get { return GetConfig.getStringValue("HOST_IMAP"); }
        }

        /// <summary>
        /// PORT_SMTP
        /// </summary>
        private int PORT_SMTP
        {
            get { return GetConfig.getIntValue("PORT_SMTP"); }
        }

        /// <summary>
        /// PORT_IMAP
        /// </summary>
        private int PORT_IMAP
        {
            get { return GetConfig.getIntValue("PORT_IMAP"); }
        }

        /// <summary>
        /// SSL
        /// </summary>
        private bool PROTOCOL_SSL
        {
            get { return GetConfig.getBoolValue("SSL"); }
        }

        #endregion

        #region Event

        /// <summary>
        /// Event Init
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            //Set Title
            base.FormTitle = "送信メール作成";
            base.FormSubTitle = "Detail";
            this.txtSubject.MaxLength = T_Mail_H.SUBJECT_MAX_LENGTH;
            this.txtFile1.MaxLength = T_Mail_H.FILE_PATH_1_MAX_LENGTH;
            this.txtFile2.MaxLength = T_Mail_H.FILE_PATH_2_MAX_LENGTH;
            this.txtFile3.MaxLength = T_Mail_H.FILE_PATH_3_MAX_LENGTH;
            this.txtStartDate.MaxLength = T_Mail_H.START_DATE_MAX_LENGTH;
            this.txtEndDate.MaxLength = T_Mail_H.END_DATE_MAX_LENGTH;
            //Init Event
            this.btnDownload.ServerClick += new EventHandler(btnDownload_Click);
            this.btnDownloadAll.ServerClick += new EventHandler(btnDownLoadAll_Click);
            this.btnShowUserSelect.ServerClick += new EventHandler(btnShowUserSelect_Click);
            this.btnConfirmEmail.ServerClick += new EventHandler(btnProcessData);
            this.treeViewLeftData.Value = string.Empty;
            this.treeViewRightData.Value = string.Empty;
            //Init Combobox
            InitCombobox(this.cmbBaseDivision, M_Config_H.CONFIG_BASE_DIVISION);
            InitCombobox(this.cmbStartDivision, M_Config_H.CONFIG_START_DIVISION);
            InitCombobox(this.cmbEndDivison, M_Config_H.CONFIG_START_DIVISION);
            //show confirm delete
            LinkButton btnYes = (LinkButton)this.Master.FindControl("btnYes");
            btnYes.Click += new EventHandler(btnProcessData);
            LinkButton btnNo = (LinkButton)this.Master.FindControl("btnNo");
            btnNo.Click += new EventHandler(btnShowData);

            string exts = GetExtension();
            if (!string.IsNullOrEmpty(exts))
            {
                this.fileUpload1.Attributes.Add("accept", GetExtension());
                this.fileUpload2.Attributes.Add("accept", GetExtension());
                this.fileUpload3.Attributes.Add("accept", GetExtension());
            }
        }

        /// <summary>
        /// Primary Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Check authority of login user
            base.SetAuthority(FormId.SendMail);
            this.IsShowDialog = false;
            //if (!base._authority.IsMailView)
            //{
            //    Response.Redirect("~/Menu/FrmMainMenu.aspx");
            //}
            if (!this.IsPostBack)
            {
                if (this.PreviousPage != null)
                {
                    //Save condition of previous page
                    this.ViewState["Condition"] = this.PreviousPageViewState["Condition"];
                    //Check mode
                    if (this.PreviousPageViewState["ID"] == null)
                    {
                        //Set mode
                        this.ProcessMode(Mode.Insert);
                    }
                    else
                    {
                        this.HID = int.Parse(PreviousPageViewState["ID"].ToString());
                        T_Mail_H mail_H = this.Get_Mail_H_ByHID(this.HID);
                        if (mail_H != null)
                        {
                            //Set Mode
                            if (mail_H.DraftFlag == 1)
                            {
                                this.ProcessMode(Mode.Insert);
                            }
                            else
                            {
                                this.ProcessMode(Mode.View);
                            }
                            this.ShowData(mail_H);
                        }
                        else
                        {
                            Server.Transfer(URL_LIST);
                        }
                    }
                }
                else
                {
                    //Set mode
                    this.ProcessMode(Mode.Insert);
                }
            }
            else
            {
                this.GetFileSrc(this.fileUpload1, txtFile1);
                this.GetFileSrc(this.fileUpload2, txtFile2);
                this.GetFileSrc(this.fileUpload3, txtFile3);
            }
        }

        /// <summary>
        /// Process Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnProcessData(object sender, EventArgs e)
        {
            switch (this.Mode)
            {
                case Utilities.Mode.Insert:
                    //Sent message 
                    if (this.DraftMail == 0)
                    {
                        if (this.SendEmailProcess())
                        {
                            //Set Mode
                            this.ProcessMode(Mode.View);

                            T_Mail_H mail_H = this.Get_Mail_H_ByHID(this.HID);
                            if (mail_H != null)
                            {
                                //Show data
                                this.ShowData(mail_H);
                            }
                            ClearFileTemp();
                            this.Success = true;
                        }
                    }
                    else
                    {
                        if (CreateDraftEmail())
                        {
                            //Set Mode
                            this.ProcessMode(Mode.Insert);

                            T_Mail_H mail_H = this.Get_Mail_H_ByHID(this.HID);
                            if (mail_H != null)
                            {
                                //Show data
                                this.ShowData(mail_H);
                            }
                            ClearFileTemp();
                            this.Success = true;
                        }
                    }
                    break;
                case Utilities.Mode.Delete:

                    //Delete Information
                    if (!this.DeteleEmail())
                    {
                        //Set Mode
                        this.ProcessMode(Mode.View);
                    }
                    else
                    {
                        Server.Transfer(URL_LIST);
                    }
                    break;
                case Utilities.Mode.Revise:

                    var t_mail_H = this.Get_Mail_H_ByHID(HID);
                    var lst_mail_D = this.GetListT_Mail_D(HID);
                    if (!this.IsSelectAll)
                    {
                        lst_mail_D = this.GetListNotReply(HID);
                    }
                    //Resend Email
                    if (this.ReSendEmail(t_mail_H, lst_mail_D))
                    {
                        //Set Mode
                        this.ProcessMode(Mode.View);
                        this.Success = true;
                    }
                    else
                    {
                        Server.Transfer(URL_LIST);
                    }
                    break;
                case Utilities.Mode.Update:
                    if (this.UpdateEmail())
                    {
                        this.ProcessMode(Mode.View);
                        T_Mail_H mail_H = this.Get_Mail_H_ByHID(this.HID);
                        if (mail_H != null)
                        {
                            //Show data
                            this.ShowData(mail_H);
                        }
                        ClearFileTemp();
                        this.Success = true;
                    }
                    break;
            }
        }

        #region showdata
        /// <summary>
        /// Show Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShowData(object sender, EventArgs e)
        {
            T_Mail_H mail_H = this.Get_Mail_H_ByHID(this.HID);
            if (mail_H != null)
            {
                this.ProcessMode(Mode.View);
                //Show data
                this.ShowData(mail_H);
            }
            else
            {
                Server.Transfer(URL_LIST);
            }
        }

        /// <summary>
        /// Show User Select Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShowUserSelect_Click(object sender, EventArgs e)
        {
            string[] arrayUid = treeViewSave.Value.Split('|');
            if (arrayUid[0] != "")
            {
                var lstUser = new List<M_User>();
                using (DB db = new DB())
                {
                    UserService userSer = new UserService(db);
                    for (var i = 0; i < arrayUid.Length; i++)
                    {
                        //Get User Info
                        var userid = int.Parse(arrayUid[i]);
                        var mUser = userSer.GetByID(userid);
                        if (mUser != null)
                        {
                            var usercd = mUser.UserCD;
                            mUser.UserCD = EditDataUtil.ToFixCodeShow(usercd, M_User.MAX_USER_CODE_SHOW);
                            lstUser.Add(mUser);
                        }
                    }
                    this.rptUser.DataSource = lstUser;
                }
            }
            else
            {
                this.rptUser.DataSource = null;
            }

            this.rptUser.DataBind();
        }
        #endregion

        #region send email
        /// <summary>
        /// Event Send Mail
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSend_Click(object sender, EventArgs e)
        {
            this.txtFile1.Text = this.hdnFileUpload1.Value;
            this.txtFile2.Text = this.hdnFileUpload2.Value;
            this.txtFile3.Text = this.hdnFileUpload3.Value;
            if (!this.CheckInput())
            {
                return;
            }
            this.Mode = Mode.Insert;
            this.DraftMail = 0;
            //Show question 
            base.IsShowQuestion = true;
            this.IsShowDialog = false;
        }
        #endregion

        #region event update email
        /// <summary>
        /// edit email
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            this.Mode = Mode.Update;
            this.ProcessMode(Mode.Update);
            T_Mail_H mail_H = this.Get_Mail_H_ByHID(this.HID);
            if (mail_H != null)
            {
                //Show data
                this.ShowData(mail_H);
            }
            else
            {
                Server.Transfer(URL_LIST);
            }
        }

        /// <summary>
        /// update email
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            this.txtFile1.Text = this.hdnFileUpload1.Value;
            this.txtFile2.Text = this.hdnFileUpload2.Value;
            this.txtFile3.Text = this.hdnFileUpload3.Value;
            if (!this.CheckInput())
            {
                return;
            }
            this.IsShowDialog = true;
            //Show question update
            base.ShowQuestionMessage(M_Message.MSG_QUESTION_UPDATE, Models.DefaultButton.Yes);
        }
        #endregion

        #region event detele email
        /// <summary>
        /// event delete email
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDeteleEmail_Click(object sender, EventArgs e)
        {
            //Set Model
            this.Mode = Mode.Delete;
            this.IsShowDialog = true;

            base.ShowQuestionMessage(M_Message.MSG_QUESTION_DELETE, Models.DefaultButton.No, true);
        }
        #endregion

        #region event resend email
        /// <summary>
        /// event resend all
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ReSendingAll_Click(object sender, EventArgs e)
        {
            //Set Model
            this.Mode = Mode.Revise;
            this.IsShowDialog = true;
            this.IsSelectAll = true;

            base.ShowQuestionMessage(M_Message.MSG_CONFIRM_RESEND_EMAIL, Models.DefaultButton.No, true);
        }

        /// <summary>
        /// event resend list not reply email
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ReSendingNotReply_Click(object sender, EventArgs e)
        {
            //Set Model
            this.Mode = Mode.Revise;
            this.IsShowDialog = true;
            this.IsSelectAll = false;

            base.ShowQuestionMessage(M_Message.MSG_CONFIRM_RESEND_EMAIL, Models.DefaultButton.No, true);
        }
        #endregion

        #region event draft email
        /// <summary>
        /// draft email
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCreateDraft(object sender, EventArgs e)
        {
            this.txtFile1.Text = this.hdnFileUpload1.Value;
            this.txtFile2.Text = this.hdnFileUpload2.Value;
            this.txtFile3.Text = this.hdnFileUpload3.Value;
            this.DraftMail = 1;
            this.Mode = Mode.Insert;
            this.IsShowDialog = true;
            base.ShowQuestionMessage(M_Message.MSG_MAKE_DRAFT, Models.DefaultButton.Yes);
        }
        #endregion

        #region event download attachments
        /// <summary>
        /// Event changed page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDownload_Click(object sender, EventArgs e)
        {
            var fileInfo = this.Get_Mail_H_ByHID(HID);
            var filename = string.Empty;
            var filePath = string.Empty;

            //attachments on mail send
            if (this.hdnFileName.Value == "1")
            {
                if (!this.txtFile1.IsEmpty)
                {
                    filePath = fileInfo.FilePath1;
                    filename = Path.GetFileName(fileInfo.FilePath1);
                }
            }
            else if (this.hdnFileName.Value == "2")
            {
                if (!this.txtFile2.IsEmpty)
                {
                    filePath = fileInfo.FilePath2;
                    filename = Path.GetFileName(fileInfo.FilePath2);
                }
            }
            else if (this.hdnFileName.Value == "3")
            {
                if (!this.txtFile3.IsEmpty)
                {
                    filePath = fileInfo.FilePath3;
                    filename = Path.GetFileName(fileInfo.FilePath3);
                }
            }
            else
            {   //attachments on mail receive
                if (!string.IsNullOrWhiteSpace(this.hdnUID_Receive.Value))
                {
                    var uid = int.Parse(this.hdnUID_Receive.Value);
                    var hid = string.IsNullOrWhiteSpace(this.hdnHID.Value) ? 0 : int.Parse(this.hdnHID.Value);
                    var mailPath = this.GetMailPath(hid, uid);
                    if (!string.IsNullOrEmpty(mailPath))
                    {
                        if (File.Exists(mailPath))
                        {
                            var message = MimeMessage.Load(mailPath);
                            var attach = message.Attachments.OfType<MimePart>().ToList();
                            var tempPath = Path.GetDirectoryName(mailPath) + Path.DirectorySeparatorChar + message.MessageId;
                            if (this.hdnFileName.Value == "4")
                            {
                                filePath = Path.Combine(tempPath, attach[0].FileName);
                                filename = attach[0].FileName;
                            }
                            if (this.hdnFileName.Value == "5")
                            {
                                filePath = Path.Combine(tempPath, attach[1].FileName);
                                filename = attach[1].FileName;
                            }
                            if (this.hdnFileName.Value == "6")
                            {
                                filePath = Path.Combine(tempPath, attach[2].FileName);
                                filename = attach[2].FileName;
                            }
                        }
                    }
                }
            }

            if (File.Exists(filePath))
            {
                Response.ClearContent();
                Response.Clear();
                Response.ContentType = "text/plain";
                Response.AddHeader("Content-Disposition", string.Format("attachment; filename = \"{0}\"", filename));
                Response.TransmitFile(filePath);
                Response.Flush();
                Response.End();
            }
        }

        protected void btnDownLoadAll_Click(object sender, EventArgs e)
        {
            IList<MailDetailUserInfo> listDetail;
            using (DB db = new DB())
            {
                var mail_DSer = new Mail_DService(db);
                listDetail = mail_DSer.GetListReplyedUserInfo(HID);
            }

            var fileFolderPath = Server.MapPath("~") + "/TempDownload/" + string.Format("{0}_{1}", HID, DateTime.Now.ToString(Constants.FMT_DATETIME));
            var zipFolderPath = Server.MapPath("~") + "/TempDownload/";

            List<string> lstFile = new List<string>();
            foreach (var item in listDetail)
            {
                lstFile.AddRange(LoadEmailAttachFile(fileFolderPath , item.UserCD, item.MailPath));
            }

            if (lstFile.Count == 0)
            {
                return;
            }
            string zipPath = Path.Combine(zipFolderPath, string.Format("{0}_{1}.zip", HID, DateTime.Now.ToString(Constants.FMT_DATETIME)));
            FastZip fastZip = new FastZip();
            string fileFilter = null;

            // Will always overwrite if target filenames already exist
            fastZip.CreateZip(zipPath, fileFolderPath, true,fileFilter);

            Directory.Delete(fileFolderPath, true);

            if (File.Exists(zipPath))
            {
                Response.ClearContent();
                Response.Clear();
                Response.ContentType = "application/zip";
                Response.AddHeader("Content-Disposition", string.Format("attachment; filename = \"{0}\"", Path.GetFileName(zipPath)));
                Response.TransmitFile(zipPath);
                Response.Flush();
                File.Delete(zipPath);
                Response.End();
            }
        }
        #endregion

        /// <summary>
        /// Event Back Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void backToForm(object sender, EventArgs e)
        {
            this.ClearFileTemp();
            if (this.Mode == Mode.Insert)
            {
                Server.Transfer(URL_LIST);
            }
            else
            {
                if (ViewState["ID"] != null)
                {
                    this.Mode = Mode.View;
                    ProcessMode(Mode.View);

                    T_Mail_H mail_H = this.Get_Mail_H_ByHID(this.HID);
                    if (mail_H != null)
                    {
                        //Show data
                        this.ShowData(mail_H);
                    }
                }
                else
                {
                    Response.Redirect("~/Menu/FrmMainMenu.aspx");
                }
            }
        }
        #endregion

        #region Method

        #region Combobox
        /// <summary>
        /// GetDataForDropdownList
        /// </summary>
        /// <param name="configCD"></param>
        /// <returns></returns>
        private IList<DropDownModel> GetDataForDropdownList(string configCD)
        {
            using (DB db = new DB())
            {
                Config_HService configSer = new Config_HService(db);
                return configSer.GetDataForDropDownList(configCD, sortIndex: ConfigSort.value4);
            }
        }

        /// <summary>
        /// Init Combobox
        /// </summary>
        private void InitCombobox(DropDownList ddl, string config)
        {
            // init combox 
            ddl.DataSource = this.GetDataForDropdownList(config);
            ddl.DataValueField = "Value";
            ddl.DataTextField = "DisplayName";
            ddl.DataBind();
            ddl.SelectedValue = this.hdInValideDefault.Value;
        }
        #endregion

        #region Message

        /// <summary>
        /// Create New Message
        /// </summary>
        /// <returns>new MimeMessage</returns>
        private MimeMessage CreateMessage()
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress(this.MAIL_NAME, this.MAIL_ID));
            return message;
        }

        /// <summary>
        /// Send Messagae
        /// </summary>
        /// <param name="client">SmtpClient</param>
        /// <param name="message">MimeMessage</param>
        /// <returns></returns>
        private bool SendMessage(SmtpClient client, MimeMessage message)
        {
            try
            {
                //send message
                client.Send(message);
            }
            //catch status email
            catch (SmtpCommandException exSmtpCommand)
            {
                switch (exSmtpCommand.ErrorCode)
                {
                    case SmtpErrorCode.RecipientNotAccepted:
                        Log.Instance.WriteLog(string.Format("Recipient not accepted: {0}", exSmtpCommand.Mailbox));
                        return false;
                    case SmtpErrorCode.SenderNotAccepted:
                        Log.Instance.WriteLog(string.Format("Sender not accepted: {0}", exSmtpCommand.Mailbox));
                        return false;
                    case SmtpErrorCode.MessageNotAccepted:
                        Log.Instance.WriteLog(string.Format("Message not accepted", exSmtpCommand.Mailbox));
                        return false;
                    case SmtpErrorCode.UnexpectedStatusCode:
                        Log.Instance.WriteLog(string.Format("An unexpected status code {0}", exSmtpCommand.Mailbox));
                        return false;
                }
            }
            catch (SmtpProtocolException exProtocol)
            {
                Log.Instance.WriteLog(string.Format("Protocol error while sending message {0}", exProtocol.Message));
                return false;
            }
            catch (ServiceNotConnectedException exService)
            {
                Log.Instance.WriteLog(string.Format("MailTransport is not connected {0}", exService.Message));
                return false;
            }
            //mail sent success
            return true;
        }

        /// <summary>
        /// Create new and send mail
        /// </summary>
        private bool SendEmailProcess()
        {
            using (DB db = new DB(System.Data.IsolationLevel.Serializable))
            {
                try
                {
                    //init
                    UserService userSer = new UserService(db);
                    Mail_DService mail_DSer = new Mail_DService(db);
                    M_User mUser = new M_User();
                    BodyBuilder body = new BodyBuilder();
                    string[] arrayUid = treeViewSave.Value.Split('|');
                    bool result = true;
                    int ret = 0;

                    using (SmtpClient client = new SmtpClient())
                    {
                        var message = this.CreateMessage();
                        var messageId = string.Empty;
                        if (ViewState["ID"] == null)
                        {
                            messageId = message.MessageId;
                        }
                        else
                        {
                            var mail_H = this.Get_Mail_H_ByHID(HID);
                            messageId = GetOldMessageId(mail_H);
                            this.UpdateAttachments(mail_H);
                        }
                        var pathList = GetFileAttachments(messageId);
                        foreach (var path in pathList.Where(x => !string.IsNullOrEmpty(x))) { body.Attachments.Add(path); } //add attachments
                        var hid = this.Execute_T_Mail_H(db, pathList);  //insert Mail_H
                        message.Subject = string.Format("【{0}】{1}", hid, this.txtSubject.Text);
                        Clear_T_Mail_D(db, hid);
                        if (ConnectedServerSmtp(client))
                        {
                            using (ImapClient imap = new ImapClient())
                            {
                                IMailFolder sendFolder = this.GetFolderSent(imap);
                                if (sendFolder == null)
                                {
                                    if (imap.IsConnected)
                                    {
                                        CloseConnectImap(imap);
                                    }
                                    return false;
                                }
                                for (int i = 0; i < arrayUid.Length; i++) //list user 
                                {
                                    var userid = int.Parse(arrayUid[i]);
                                    mUser = userSer.GetByID(userid);
                                    message.To.Clear();  //avoid send cc
                                    message.To.Add(new MailboxAddress(mUser.UserName1, mUser.MailAddress));
                                    body.TextBody = this.txtContent.Value.Replace(CONST_NM_REPLACE, mUser.UserName1);
                                    message.Body = body.ToMessageBody();
                                    if (SendMessage(client, message))
                                    {
                                        this.Execute_T_Mail_D(mail_DSer, mUser, hid);
                                        sendFolder.Append(message); // sync email to server
                                        ret += 1;
                                    }
                                    else
                                    {
                                        base.SetMessage(string.Empty, M_Message.MSG_USER_SEND_ERMAIL_ERROR, mUser.UserName1);
                                        result = false;
                                    }
                                }
                                CloseConnectSmtp(client);
                                CloseConnectImap(imap);
                                if (ret != 0)
                                {
                                    HID = hid;
                                    db.Commit();
                                }
                                return result;
                            }
                        }
                        else
                        {
                            this.SetMessage(string.Empty, M_Message.MSG_CONNECT_ERMAIL_ERROR, "");
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.SetMessage(string.Empty, M_Message.MSG_UPDATE_FAILE, "メール");
                    Log.Instance.WriteLog(ex);
                    return false;
                }
            }
        }

        /// <summary>
        /// Create and Update Draft Email
        /// </summary>
        /// <returns></returns>
        private bool CreateDraftEmail()
        {
            using (DB db = new DB(System.Data.IsolationLevel.Serializable))
            {
                try
                {
                    var mail_DService = new Mail_DService(db);
                    var mUserService = new UserService(db);
                    var mUser = new M_User();
                    var messageId = string.Empty;
                    if (ViewState["ID"] == null)
                    {
                        var message = CreateMessage();
                        messageId = message.MessageId;
                    }
                    else
                    {
                        var mail_H = this.Get_Mail_H_ByHID(HID);
                        messageId = GetOldMessageId(mail_H);
                        this.UpdateAttachments(mail_H);
                    }
                    string[] arrayUid = treeViewSave.Value.Split('|');
                    var pathList = GetFileAttachments(messageId);
                    var hid = this.Execute_T_Mail_H(db, pathList);
                    this.Clear_T_Mail_D(db, hid);
                    if (arrayUid[0] != "")
                    {
                        for (int i = 0; i < arrayUid.Length; i++) //list user 
                        {
                            var userid = int.Parse(arrayUid[i]);
                            mUser = mUserService.GetByID(userid);
                            this.Execute_T_Mail_D(mail_DService, mUser, hid);
                        }
                    }
                    HID = hid; // save hid
                    db.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    Log.Instance.WriteLog(ex);
                    this.SetMessage(string.Empty, M_Message.MSG_UPDATE_FAILE, "メール");
                    return false;
                }
            }
        }

        /// <summary>
        /// Update Mail
        /// </summary>
        /// <returns></returns>
        private bool UpdateEmail()
        {
            using (DB db = new DB(System.Data.IsolationLevel.Serializable))
            {
                try
                {
                    if (ViewState["ID"] == null) { return false; }
                    string[] arrayUid = treeViewSave.Value.Split('|');
                    var mail_D_Service = new Mail_DService(db);
                    var mUserService = new UserService(db);
                    var mUser = new M_User();
                    var mail_H = this.Get_Mail_H_ByHID(HID);
                    var messageId = GetOldMessageId(mail_H); // take old message id if null create new
                    var lstMailD = this.GetListT_Mail_D(HID);
                    var lstUserRemove = new List<int>();
                    this.UpdateAttachments(mail_H); // remove old file
                    var pathList = GetFileAttachments(messageId);
                    HID = this.Execute_T_Mail_H(db, pathList);
                    if (arrayUid[0] != "")
                    {
                        foreach (var item in lstMailD) //filter user not exist on new list
                        {
                            var flgExist = false;

                            for (int i = 0; i < arrayUid.Length; i++)
                            {
                                var userid = int.Parse(arrayUid[i]);
                                if (item.UID == userid)
                                {
                                    flgExist = true;
                                }
                            }
                            if (!flgExist)
                            {
                                lstUserRemove.Add(item.UID);
                            }
                        }
                        for (int i = 0; i < arrayUid.Length; i++) // insert new user
                        {
                            var userid = int.Parse(arrayUid[i]);
                            if (!this.IsExistPK_Mail_D(HID, userid))
                            {
                                mUser = mUserService.GetByID(userid);
                                this.Execute_T_Mail_D(mail_D_Service, mUser, HID);
                            }
                        }
                        if (lstUserRemove.Count > 0) // remove old user
                        {
                            this.RemoveMailDByListUser(db, lstUserRemove, HID);
                        }
                    }
                    db.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    Log.Instance.WriteLog(ex);
                    this.SetMessage(string.Empty, M_Message.MSG_UPDATE_FAILE, "編集");
                    return false;
                }
            }
        }

        /// <summary>
        /// Delete Email
        /// </summary>
        /// <returns></returns>
        private bool DeteleEmail()
        {
            using (DB db = new DB(System.Data.IsolationLevel.Serializable))
            {
                try
                {
                    var mail_HSer = new Mail_HService(db);
                    var mail_DSer = new Mail_DService(db);
                    this.ClearAttachments(); // clear attachment before delete
                    var t_mail_H = mail_HSer.Delete(HID);
                    var t_mail_D = mail_DSer.DeleteAllByHId(HID);
                    if (t_mail_H > 0 || t_mail_D > 0)
                    {
                        db.Commit();
                        return true;
                    }
                    this.SetMessage(string.Empty, M_Message.MSG_DATA_CHANGED);
                    return false;
                }
                catch (Exception ex)
                {
                    Log.Instance.WriteLog(ex);
                    this.SetMessage(string.Empty, M_Message.MSG_UPDATE_FAILE, "削除");
                    return false;
                }
            }
        }

        /// <summary>
        /// Save Email
        /// </summary>
        /// <param name="message">MimeMessage</param>
        /// <returns></returns>
        private string SaveToMessage(MimeMessage message)
        {
            try
            {
                var fileName = Path.Combine(DIR_PATH, string.Format("{0}_{1}", message.MessageId, message.Subject) + EXT_MAIL);
                using (var stream = File.Create(fileName))
                    message.WriteTo(stream);
                return fileName;
            }
            catch (Exception ex) { Log.Instance.WriteLog(ex); return string.Empty; }
        }

        /// <summary>
        /// Load file eml
        /// </summary>
        /// <param name="filePath"></param>
        private bool HaveAttachFileInMail()
        {

            IList<MailDetailUserInfo> listDetail;
            using (DB db = new DB())
            {
                var mail_DSer = new Mail_DService(db);
                listDetail = mail_DSer.GetListReplyedUserInfo(HID);
            }

            foreach (var item in listDetail)
            {
                if (File.Exists(item.MailPath))
                {
                    try
                    {
                        var message = MimeMessage.Load(item.MailPath);
                        var lstFile = new List<string>();
                        if (message != null)
                        {
                            if (message.Attachments.Count() != 0)
                            {
                                return true;
                            }
                        }
                    }
                    catch (Exception) {  }
                }
            }
            return false;
        }

        /// <summary>
        /// Load file eml
        /// </summary>
        /// <param name="filePath"></param>
        public List<string> LoadEmailAttachFile(string destFolder,string userCd,string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    var message = MimeMessage.Load(filePath);
                    var lstFile = new List<string>();
                    if (message != null)
                    {
                        if (message.Attachments.Count() != 0)
                        {
                            if (!Directory.Exists(destFolder))
                            {
                                Directory.CreateDirectory(destFolder);
                            }
                            foreach (MimePart part in message.Attachments.OfType<MimePart>()) //save attachments
                            {
                                var fileNm = Path.Combine(destFolder, userCd + "_" + part.FileName);
                                using (var stream = File.Create(fileNm))
                                    part.ContentObject.DecodeTo(stream);
                                lstFile.Add(fileNm);
                            }
                        }
                        return lstFile;
                    }
                    return new List<string>();
                }
                catch (Exception ex) { Log.Instance.WriteLog(ex); return null; }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Load file eml
        /// </summary>
        /// <param name="filePath"></param>
        public static MailDetailInfo LoadToEmail(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    var message = MimeMessage.Load(filePath);
                    var mail = new MailDetailInfo();
                    var lstFile = new List<string>();
                    if (message != null)
                    {
                        mail.BodyMail = message.TextBody;
                        mail.Subject = message.Subject;
                        mail.ReceiveDate = string.Format(FULL_DATE_TIME_FM, message.Date);
                        if (message.Attachments.Count() != 0)
                        {
                            var tempPath = Path.GetDirectoryName(filePath);
                            var path = tempPath + Path.DirectorySeparatorChar + message.MessageId;
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            foreach (MimePart part in message.Attachments.OfType<MimePart>()) //save attachments
                            {
                                var fileNm = Path.Combine(path, part.FileName);
                                using (var stream = File.Create(fileNm))
                                    part.ContentObject.DecodeTo(stream);
                                lstFile.Add(part.FileName);
                            }
                        }
                        if (lstFile.Count != 0) //maximum 3 attach
                        {
                            if (lstFile.Count == 1)
                            {
                                mail.FilePath1 = lstFile[0];
                            }
                            else if (lstFile.Count == 2)
                            {
                                mail.FilePath1 = lstFile[0];
                                mail.FilePath2 = lstFile[1];
                            }
                            else
                            {
                                mail.FilePath1 = lstFile[0];
                                mail.FilePath2 = lstFile[1];
                                mail.FilePath3 = lstFile[2];
                            }
                        }
                        return mail;
                    }
                    return null;
                }
                catch (Exception ex) { Log.Instance.WriteLog(ex); return null; }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// ReSending Email
        /// </summary>
        /// <returns></returns>
        private bool ReSendEmail(T_Mail_H t_mail_H, List<T_Mail_D> lstMail)
        {
            try
            {
                using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                {
                    if (t_mail_H != null)
                    {
                        var mail_HSer = new Mail_HService(db);
                        var mUserService = new UserService(db);
                        var client = new SmtpClient();
                        var body = new BodyBuilder();
                        var message = this.CreateMessage();
                        var mUser = new M_User();
                        var result = true;
                        //Update database when send all
                        if (this.IsSelectAll)
                        {
                            var ret = mail_HSer.UpdateDate(t_mail_H.ID, LoginInfo.User.ID);
                            if (ret == 0)
                            {
                                this.SetMessage(string.Empty, M_Message.MSG_CONNECT_ERMAIL_ERROR, "");
                                return false;
                            }
                        }
                        message.Subject = string.Format("【{0}】{1}", t_mail_H.ID, t_mail_H.Subject);
                        if (!string.IsNullOrWhiteSpace(t_mail_H.FilePath1) && File.Exists(t_mail_H.FilePath1))
                        {
                            body.Attachments.Add(t_mail_H.FilePath1);
                        }
                        if (!string.IsNullOrWhiteSpace(t_mail_H.FilePath2) && File.Exists(t_mail_H.FilePath2))
                        {
                            body.Attachments.Add(t_mail_H.FilePath2);
                        }
                        if (!string.IsNullOrWhiteSpace(t_mail_H.FilePath3) && File.Exists(t_mail_H.FilePath3))
                        {
                            body.Attachments.Add(t_mail_H.FilePath3);
                        }
                        if (ConnectedServerSmtp(client))
                        {
                            using (ImapClient imap = new ImapClient())
                            {
                                var sendFolder = this.GetFolderSent(imap);
                                if (sendFolder == null)
                                {
                                    if (imap.IsConnected) { CloseConnectImap(imap); }
                                    return false;
                                }
                                foreach (var item in lstMail)
                                {
                                    mUser = mUserService.GetByID(item.UID);
                                    message.To.Clear();
                                    message.To.Add(new MailboxAddress(mUser.UserName1, mUser.MailAddress));
                                    body.TextBody = t_mail_H.BodyMail.Replace(CONST_NM_REPLACE, mUser.UserName1);
                                    message.Body = body.ToMessageBody();
                                    sendFolder.Append(message); //sync email to server
                                    if (!SendMessage(client, message))
                                    {
                                        base.SetMessage(string.Empty, M_Message.MSG_USER_SEND_ERMAIL_ERROR, mUser.UserName1);
                                        result = false;
                                    }
                                }
                                this.CloseConnectSmtp(client);
                                this.CloseConnectImap(imap);
                                db.Commit();
                                return result;
                            }
                        }
                        else
                        {
                            this.SetMessage(string.Empty, M_Message.MSG_CONNECT_ERMAIL_ERROR, "");
                            return false;
                        }
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Instance.WriteLog(ex);
                this.SetMessage(string.Empty, M_Message.MSG_UPDATE_FAILE, "メール");
                return false;
            }
        }

        #region Connection Email
        /// <summary>
        /// Connected server Smtp
        /// </summary>
        /// <param name="client">SmtpClient</param>
        /// <returns></returns>
        private bool ConnectedServerSmtp(SmtpClient client)
        {
            // connect mail server
            try
            {
                var options = SecureSocketOptions.Auto;
                if (this.PROTOCOL_SSL)
                {
                    options = SecureSocketOptions.SslOnConnect;
                }
                client.Connect(this.HOST_SMTP, this.PORT_SMTP, options);
            }
            catch (Exception ex)
            {
                Log.Instance.WriteLog(ex);
                return false;
            }
            try
            {
                // client eamil and password
                client.Authenticate(this.MAIL_ID, this.MAIL_PASSWORD);
            }
            //Some SMTP servers support authentication.
            catch (AuthenticationException exAuthen)
            {
                Log.Instance.WriteLog(string.Format("Invalid: {0}", exAuthen.Message));
                return false;
            }
            catch (SmtpCommandException exSmtpCommand)
            {
                Log.Instance.WriteLog(string.Format("Error trying to authenticate: {0}", exSmtpCommand.Message));
                return false;
            }
            catch (SmtpProtocolException exProtocol)
            {
                Log.Instance.WriteLog(string.Format("Protocol error while trying to authenticate: {0}", exProtocol.Message));
                return false;
            }
            //connecion susscess
            return client.IsConnected;
        }

        /// <summary>
        /// Close Connection Smtp
        /// </summary>
        /// <param name="client">SmtpClient</param>
        private void CloseConnectSmtp(SmtpClient client)
        {
            client.Disconnect(true);
        }

        /// <summary>
        /// Close Connection Smtp
        /// </summary>
        /// <param name="client">ImapClient</param>
        private void CloseConnectImap(ImapClient client)
        {
            client.Disconnect(true);
        }

        /// <summary>
        /// Get folder send on server
        /// </summary>
        /// <param name="imap"></param>
        /// <returns></returns>
        private IMailFolder GetFolderSent(ImapClient imap)
        {
            try
            {
                var options = SecureSocketOptions.Auto;
                if (this.PROTOCOL_SSL)
                {
                    options = SecureSocketOptions.SslOnConnect;
                }
                imap.Connect(this.HOST_IMAP, this.PORT_IMAP, options);
                imap.Authenticate(this.MAIL_ID, this.MAIL_PASSWORD);

                if (imap.IsConnected)
                {
                    IMailFolder sendFolder = imap.GetFolders(imap.PersonalNamespaces[0]).FirstOrDefault(x => FOLDER_SEND_STRING.Contains(x.Name)); //get folder
                    if (sendFolder == null)
                    {
                        var personal = imap.GetFolder(imap.PersonalNamespaces[0]);
                        sendFolder = personal.Create(FOLDER_SEND_STRING, true);
                    }
                    return sendFolder;
                }
                return null;
            }
            catch (Exception ex)
            {
                Log.Instance.WriteLog(ex);
                base.SetMessage(string.Empty, M_Message.MSG_CONNECT_ERMAIL_ERROR, "");
                return null;
            }
        }
        #endregion

        #region Attachments Email

        /// <summary>
        /// create new folder temp
        /// </summary>
        /// <returns></returns>
        private string GetTemporaryDirectory()
        {
            string tempDirectory = CONST_FILE_PATH_TEMP + Path.DirectorySeparatorChar + Path.GetRandomFileName();
            Directory.CreateDirectory(MapPath(tempDirectory));
            this.hdnTempPath.Value = tempDirectory;
            return tempDirectory;
        }

        /// <summary>
        /// get attachments
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public List<String> GetFileAttachments(string messageId)
        {
            var lstFile = new List<string>();
            //get file client upload
            lstFile.Add(SaveFileUpload(this.fileUpload1, this.txtFile1, messageId, this.hdnFileUpload1));
            lstFile.Add(SaveFileUpload(this.fileUpload2, this.txtFile2, messageId, this.hdnFileUpload2));
            lstFile.Add(SaveFileUpload(this.fileUpload3, this.txtFile3, messageId, this.hdnFileUpload3));
            if (ViewState["ID"] != null)
            {
                var fileInfo = this.Get_Mail_H_ByHID(HID);
                if (fileInfo != null) //check file exist and get file have save in primary folder
                {
                    if (!string.IsNullOrEmpty(fileInfo.FilePath1) && File.Exists(fileInfo.FilePath1))
                    {
                        lstFile[0] = fileInfo.FilePath1;
                    }
                    if (!string.IsNullOrEmpty(fileInfo.FilePath2) && File.Exists(fileInfo.FilePath2))
                    {
                        lstFile[1] = fileInfo.FilePath2;
                    }
                    if (!string.IsNullOrEmpty(fileInfo.FilePath3) && File.Exists(fileInfo.FilePath3))
                    {
                        lstFile[2] = fileInfo.FilePath3;
                    }
                }
            }
            if (string.IsNullOrEmpty(lstFile[0]) && string.IsNullOrEmpty(lstFile[1]) && string.IsNullOrEmpty(lstFile[2]))
            {   //delete folder when folder empty
                var filePath = Path.Combine(DIR_PATH, messageId);
                if (Directory.Exists(filePath))
                {
                    var countFile = Directory.GetFiles(filePath);
                    if (countFile.Length == 0)
                    {
                        Directory.Delete(filePath);
                    }
                }
            }
            return lstFile;
        }

        /// <summary>
        /// save file upload
        /// </summary>
        /// <param name="fileUpload">FileUpload control</param>
        /// <param name="txtFile">txtFile</param>
        /// <param name="messageId">messageId</param>
        /// <returns></returns>
        private string SaveFileUpload(FileUpload fileUpload, ITextBox txtFile, string messageId, HiddenField hdnTxtFile)
        {
            //temp folder
            string tmpDirectory = this.hdnTempPath.Value;
            string fileName;

            string tmpPathFile;
            string pathFile = DIR_PATH + Path.DirectorySeparatorChar + messageId; //primary folder
            if (!Directory.Exists(pathFile))
            {
                Directory.CreateDirectory(pathFile);
            }
            if (!string.IsNullOrWhiteSpace(hdnTxtFile.Value))
            {
                fileName = hdnTxtFile.Value;
                var pathDestFile = Path.Combine(pathFile, fileName);
                tmpPathFile = MapPath(tmpDirectory + Path.DirectorySeparatorChar + hdnTxtFile.Value);
                if (File.Exists(tmpPathFile) && !File.Exists(pathDestFile))
                {
                    File.Copy(tmpPathFile, pathDestFile);
                    return pathDestFile;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Save File Temp
        /// </summary>
        /// <param name="fileUpload"></param>
        /// <param name="txtFile"></param>
        private void GetFileSrc(FileUpload fileUpload, ITextBox txtFile)
        {
            if (fileUpload.PostedFile != null && fileUpload.PostedFile.ContentLength > 0)
            {
                string tmpDirectory = this.hdnTempPath.Value;
                if (string.IsNullOrWhiteSpace(this.hdnTempPath.Value))
                {
                    tmpDirectory = this.GetTemporaryDirectory();
                }
                string fileName;
                if (!Directory.Exists(MapPath(tmpDirectory)))
                {
                    Directory.CreateDirectory(MapPath(tmpDirectory));
                }
                fileName = txtFile.Value = Path.GetFileName(fileUpload.PostedFile.FileName);
                fileUpload.SaveAs(MapPath(tmpDirectory + Path.DirectorySeparatorChar + fileName));
            }
        }

        /// <summary>
        /// Clear File Temp
        /// </summary>
        private void ClearFileTemp()
        {
            try
            {
                string tmpDirectory = this.hdnTempPath.Value;
                if (string.IsNullOrEmpty(tmpDirectory)) { return; }
                var file = Directory.GetFiles(MapPath(tmpDirectory));
                if (file.Length > 0)
                {
                    for (var i = 0; i < file.Length; i++)
                    {
                        File.Delete(file[i]);
                    }
                }
                Directory.Delete(MapPath(tmpDirectory));
            }
            catch (Exception ex)
            {
                Log.Instance.WriteLog(ex);
                return;
            }
        }

        /// <summary>
        /// Delete attachments
        /// </summary>
        /// <param name="fileInfo">filePath</param>
        private void ClearAttachments()
        {
            try
            {
                //clear attachment send
                var fileInfo = this.Get_Mail_H_ByHID(HID);
                if (fileInfo != null)
                {
                    var path = string.Empty;
                    if (!string.IsNullOrEmpty(fileInfo.FilePath1))
                    {
                        path = Path.GetDirectoryName(fileInfo.FilePath1);
                    }
                    else if (!string.IsNullOrEmpty(fileInfo.FilePath2))
                    {
                        path = Path.GetDirectoryName(fileInfo.FilePath2);
                    }
                    else if (!string.IsNullOrEmpty(fileInfo.FilePath3))
                    {
                        path = Path.GetDirectoryName(fileInfo.FilePath3);
                    }
                    if (!string.IsNullOrEmpty(path))
                    {
                        if (Directory.Exists(path))
                        {
                            DeleteFileInPath(path);
                            Directory.Delete(path, true);
                        }
                    }
                }
                //clear eml receive
                var lstSend = this.GetListT_Mail_D(HID);
                if (lstSend.Count != 0)
                {
                    foreach (var item in lstSend)
                    {
                        if (!string.IsNullOrEmpty(item.MailPath))
                        {
                            DeleteByMailPath(item.MailPath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Instance.WriteLog(ex);
                return;
            }
        }

        /// <summary>
        /// Clear file and folder by mailpath
        /// </summary>
        /// <param name="mailPath"></param>
        private void DeleteByMailPath(string mailPath)
        {
            var path = Path.GetDirectoryName(mailPath);
            if (Directory.Exists(path))
            {
                var lstSubFolder = Directory.GetDirectories(path);
                if (lstSubFolder.Length > 0)
                {
                    for (var i = 0; i < lstSubFolder.Length; i++)
                    {
                        DeleteFileInPath(lstSubFolder[i]);
                        Directory.Delete(lstSubFolder[i], true);
                    }
                }
                else
                {
                    DeleteFileInPath(path);
                }
                Directory.Delete(path, true);
            }
        }

        /// <summary>
        /// Delete all file in path folder
        /// </summary>
        /// <param name="path"></param>
        private void DeleteFileInPath(string path)
        {
            var lstFile = Directory.GetFiles(path);
            if (lstFile.Length > 0)
            {
                for (var i = 0; i < lstFile.Length; i++)
                {
                    File.Delete(lstFile[i]);
                }
            }
        }
        #endregion

        #endregion

        /// <summary>
        /// Process Mode
        /// </summary>
        /// <param name="mode">Mode</param>
        private void ProcessMode(Mode mode)
        {
            //Set Model
            this.Mode = mode;
            //Check model
            switch (mode)
            {
                case Mode.Insert:
                    this.txtContent.Value = string.Empty;
                    this.txtSubject.ReadOnly = false;
                    this.txtReplyDueDate.Value = DateTime.Now.AddDays(7);
                    this.btnDownload1.Disabled = true;
                    this.btnDownload2.Disabled = true;
                    this.btnDownload3.Disabled = true;
                    base.DisabledLink(this.btnSend, !base._authority.IsMailSend);
                    base.DisabledLink(this.btnDraft, !base._authority.IsMailSend);
                    base.DisabledLink(this.btnSendTop, !base._authority.IsMailSend);
                    base.DisabledLink(this.btnDraftTop, !base._authority.IsMailSend);
                    if (this.txtContent.IsEmpty)
                    {
                        this.txtContent.Value = this.getBodyMailConst();
                    }
                    break;

                case Mode.Delete:
                case Mode.Revise:
                case Mode.View:
                    this.txtReplyDueDate.ReadOnly = true;
                    this.txtContent.ReadOnly = true;
                    this.txtSubject.ReadOnly = true;
                    this.btnFile1.Disabled = true;
                    this.btnFile2.Disabled = true;
                    this.btnFile3.Disabled = true;
                    base.DisabledLink(this.btnSend, true);
                    base.DisabledLink(this.btnSendTop, true);
                    this.btnModelUser.Disabled = true;
                    this.btnDownload1.Disabled = true;
                    this.btnDownload2.Disabled = true;
                    this.btnDownload3.Disabled = true;
                    this.btnRemove1.Disabled = true;
                    this.btnRemove2.Disabled = true;
                    this.btnRemove3.Disabled = true;
                    this.chkResendFlag.Disabled = true;
                    this.cmbBaseDivision.Enabled = false;
                    this.cmbStartDivision.Enabled = false;
                    this.cmbEndDivison.Enabled = false;
                    this.txtStartDate.ReadOnly = true;
                    this.txtEndDate.ReadOnly = true;
                    this.txtResendInterval.ReadOnly = true;
                    this.txtResendTime.ReadOnly = true;

                    T_Mail_H mail_H = this.Get_Mail_H_ByHID(HID);
                    if (mail_H != null && mail_H.CreateUID == base.LoginInfo.User.ID)
                    {
                        base.DisabledLink(this.btnEdit, false);
                        base.DisabledLink(this.btnReSendAll, false);
                        base.DisabledLink(this.btnReSendNoReply, false);
                        base.DisabledLink(this.btnDeteleEmail, false);

                        base.DisabledLink(this.btnEditTop, false);
                        base.DisabledLink(this.btnReSendAllTop, false);
                        base.DisabledLink(this.btnReSendNoReplyTop, false);
                        base.DisabledLink(this.btnDeteleEmailTop, false);
                    }
                    else
                    {
                        base.DisabledLink(this.btnEdit, !base._authority.IsMailEdit);
                        base.DisabledLink(this.btnReSendAll, !base._authority.IsMailReSend);
                        base.DisabledLink(this.btnReSendNoReply, !base._authority.IsMailReSend);
                        base.DisabledLink(this.btnDeteleEmail, !base._authority.IsMailDelete);

                        base.DisabledLink(this.btnEditTop, !base._authority.IsMailEdit);
                        base.DisabledLink(this.btnReSendAllTop, !base._authority.IsMailReSend);
                        base.DisabledLink(this.btnReSendNoReplyTop, !base._authority.IsMailReSend);
                        base.DisabledLink(this.btnDeteleEmailTop, !base._authority.IsMailDelete);
                    }

                    break;
                case Mode.Update:
                    this.txtReplyDueDate.ReadOnly = false;
                    this.txtContent.ReadOnly = false;
                    this.txtSubject.ReadOnly = false;
                    this.btnFile1.Disabled = false;
                    this.btnFile2.Disabled = false;
                    this.btnFile3.Disabled = false;
                    this.btnModelUser.Disabled = false;
                    this.chkResendFlag.Disabled = false;
                    this.cmbBaseDivision.Enabled = true;
                    this.cmbStartDivision.Enabled = true;
                    this.cmbEndDivison.Enabled = true;
                    this.txtStartDate.ReadOnly = false;
                    this.txtEndDate.ReadOnly = false;
                    this.txtResendInterval.ReadOnly = false;
                    this.txtResendTime.ReadOnly = false;
                    break;
            }
        }

        /// <summary>
        /// Validate Input
        /// </summary>
        /// <returns></returns>
        private bool CheckInput()
        {
            using (DB db = new DB())
            {
                if (!Directory.Exists((DIR_PATH)))
                {   //create primary folder
                    Directory.CreateDirectory((DIR_PATH));
                }
                var m_User = new M_User();
                var mUserService = new UserService(db);
                string[] arrayUid = treeViewSave.Value.Split('|');
                //Check require Subject
                if (this.txtSubject.IsEmpty)
                {
                    this.SetMessage(this.txtSubject.ID, M_Message.MSG_REQUIRE, "件名");
                }
                //Check require DueDate
                if (this.txtReplyDueDate.IsEmpty)
                {
                    this.SetMessage(this.txtReplyDueDate.ID, M_Message.MSG_REQUIRE, "返信期限");
                }
                else
                {   //check DueDate more than today
                    if (this.Mode == Mode.Insert)
                    {
                        if (DateTime.Parse(this.txtReplyDueDate.Text) < DateTime.Today)
                        {
                            this.SetMessage(this.txtReplyDueDate.ID, M_Message.MSG_DATE_GREATER_THAN_EQUAL, "返信期限", "当日");
                        }
                    }
                }
                //Check user receive message
                if (arrayUid[0] == string.Empty)
                {
                    this.SetMessage(this.btnModelUser.ID, M_Message.MSG_REQUIRE, "送信先選択");
                }
                else
                {
                    for (int i = 0; i < arrayUid.Length; i++)
                    {
                        var userid = int.Parse(arrayUid[i]);
                        m_User = mUserService.GetByID(userid);
                        //Check email address exist
                        if (m_User.MailAddress == string.Empty)
                        {
                            this.SetMessage(string.Empty, M_Message.MSG_NOT_EXIST_CODE, string.Format("User {0} EmailAddress", m_User.UserName1));
                        }
                        //Check email format
                        else if (!CheckDataUtil.IsEmail(m_User.MailAddress))
                        {
                            this.SetMessage(string.Empty, M_Message.MSG_INCORRECT_FORMAT, string.Format("User {0} EmailAddress {1}", m_User.UserName1, m_User.mailAddress));
                        }
                    }
                }
                List<string> lstExtensions = this.GetAllowExtensions();
                //check size file upload1
                if (!this.txtFile1.IsEmpty)
                {

                    if (!IsValidExtension(Path.GetExtension(this.txtFile1.Text), lstExtensions))
                    {
                        this.SetMessage(this.txtFile1.ID, M_Message.MSG_EXTENSION);
                    }
                    else
                    {
                        float ms = new MemoryStream(this.fileUpload1.FileBytes).Length;
                        if (ms > CONST_FILE_MAX_SIZE)
                        {
                            this.SetMessage(this.txtFile1.ID, M_Message.MSG_SIZE_FILE_UPLOAD_LESS_THAN_EQUAL, "添付ファイル1", CONST_FILE_MAX_SIZE_STRING);
                        }
                    }
                }
                //check size file upload2
                if (!this.txtFile2.IsEmpty)
                {
                    if (!IsValidExtension(Path.GetExtension(this.txtFile2.Text), lstExtensions))
                    {
                        this.SetMessage(this.txtFile2.ID, M_Message.MSG_EXTENSION);
                    }
                    else
                    {
                        float ms = new MemoryStream(this.fileUpload2.FileBytes).Length;
                        if (ms > CONST_FILE_MAX_SIZE)
                        {
                            this.SetMessage(this.txtFile2.ID, M_Message.MSG_SIZE_FILE_UPLOAD_LESS_THAN_EQUAL, "添付ファイル2", CONST_FILE_MAX_SIZE_STRING);
                        }
                    }
                }
                //check size file upload3
                if (!this.txtFile3.IsEmpty)
                {
                    if (!IsValidExtension(Path.GetExtension(this.txtFile3.Text), lstExtensions))
                    {
                        this.SetMessage(this.txtFile3.ID, M_Message.MSG_EXTENSION);
                    }
                    else
                    {
                        float ms = new MemoryStream(this.fileUpload3.FileBytes).Length;
                        if (ms > CONST_FILE_MAX_SIZE)
                        {
                            this.SetMessage(this.txtFile3.ID, M_Message.MSG_SIZE_FILE_UPLOAD_LESS_THAN_EQUAL, "添付ファイル3", CONST_FILE_MAX_SIZE_STRING);
                        }
                    }
                }
                //check reply flag
                if (this.chkResendFlag.Checked)
                {   //check required
                    if (this.txtStartDate.IsEmpty)
                    {
                        this.SetMessage(this.txtStartDate.ID, M_Message.MSG_REQUIRE, "再送開始日");
                    }
                    if (this.txtEndDate.IsEmpty)
                    {
                        this.SetMessage(this.txtEndDate.ID, M_Message.MSG_REQUIRE, "再送終了日");
                    }
                    if (this.txtResendInterval.IsEmpty)
                    {
                        this.SetMessage(this.txtResendInterval.ID, M_Message.MSG_REQUIRE, "再送間隔");
                    }
                    if (this.txtResendTime.IsEmpty)
                    {
                        this.SetMessage(this.txtResendTime.ID, M_Message.MSG_REQUIRE, "再送信開始時刻");
                    }
                    if (!this.txtStartDate.IsEmpty && !this.txtEndDate.IsEmpty)
                    {
                        if (this.cmbBaseDivision.SelectedIndex == 0)
                        {
                            if (int.Parse(this.txtStartDate.Text) > int.Parse(this.txtEndDate.Text))
                            {
                                this.SetMessage(this.txtStartDate.ID, M_Message.MSG_INCORRECT_FORMAT, "数値");
                            }
                        }
                        else if (this.cmbBaseDivision.SelectedIndex == 1)
                        {
                            if (this.cmbStartDivision.SelectedIndex == 0 && this.cmbEndDivison.SelectedIndex == 0)
                            {
                                if (int.Parse(this.txtStartDate.Text) < int.Parse(this.txtEndDate.Text))
                                {
                                    this.SetMessage(this.txtEndDate.ID, M_Message.MSG_INCORRECT_FORMAT, "数値");
                                }
                            }
                            else if (this.cmbStartDivision.SelectedIndex == 1)
                            {
                                if (int.Parse(this.txtStartDate.Text) > int.Parse(this.txtEndDate.Text))
                                {
                                    this.SetMessage(this.txtStartDate.ID, M_Message.MSG_INCORRECT_FORMAT, "数値");
                                }
                            }
                            //Check StartDate 
                            if (this.cmbStartDivision.SelectedIndex == 0 && !this.txtReplyDueDate.IsEmpty & this.Mode == Mode.Insert)
                            {
                                var chkDate = DateTime.Parse(this.txtReplyDueDate.Value.ToString()).AddDays(-int.Parse(this.txtStartDate.Value));
                                if (chkDate < DateTime.Today)
                                {
                                    this.SetMessage(this.txtStartDate.ID, M_Message.MSG_DATE_GREATER_THAN_EQUAL, "再送開始日", "当日");
                                }
                            }
                        }
                    }
                }
            }
            return !base.HaveError;
        }

        /// <summary>
        /// Get setting
        /// </summary>
        /// <returns>M_Setting model</returns>
        private List<string> GetAllowExtensions()
        {
            using (DB db = new DB())
            {
                SettingService settingSer = new SettingService(db);

                M_Setting setting = settingSer.GetData();
                if (setting == null)
                {
                    return new List<string>();
                }
                else
                {
                    var lstExtension = setting.Extension.Split(',').ToList();
                    for (int i = 0; i < lstExtension.Count; i++)
                    {
                        lstExtension[i] = lstExtension[i].Trim();
                    }
                    return lstExtension;
                }
            }
        }

        /// <summary>
        /// Check Extension is allowed
        /// </summary>
        /// <param name="ext"></param>
        /// <param name="lstExt"></param>
        /// <returns></returns>
        private bool IsValidExtension(string ext, List<string> lstExt)
        {
            if (lstExt.Count == 0)
            {
                return true;
            }
            foreach (var allowExt in lstExt)
            {
                if (string.IsNullOrEmpty(allowExt))
                {
                    continue;
                }
                if (allowExt.StartsWith("."))
                {
                    if (ext.ToLower() == allowExt.ToLower())
                    {
                        return true;
                    }
                }
                else
                { 
                    if (ext.ToLower() == "." + allowExt.ToLower())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Get display message
        /// </summary>
        /// <returns>HTML of error message</returns>
        private string GetExtension()
        {
            List<string> lstExtension = GetAllowExtensions();

            if (lstExtension.Count == 0)
            {
                return string.Empty;
            }

            for (int i = 0; i < lstExtension.Count; i++)
            {
                if (!lstExtension[i].StartsWith("."))
                {
                    lstExtension[i] = "." + lstExtension[i];
                }
            }

            return string.Join(",", lstExtension);
        }

        /// <summary>
        /// Show data on form
        /// </summary>
        /// <param name="user">User</param>
        private void ShowData(T_Mail_H mail_H)
        {
            this.hdnIsMailDisplay.Value = (this._authority.IsMailView || mail_H.CreateUID == base.LoginInfo.User.ID).ToString();

            IList<T_Mail_D> lstmail_D = null;
            M_User mUser = null;

            List<MailDetailInfo> lstDataMail = new List<MailDetailInfo>();
            using (DB db = new DB())
            {
                UserService userSer = new UserService(db);
                Mail_DService mail_DSer = new Mail_DService(db);
                lstmail_D = mail_DSer.GetListByHID(HID);

                this.txtSubject.Text = mail_H.Subject;
                this.txtReplyDueDate.Text = mail_H.ReplyDueDate.ToString("yyyy/MM/dd");
                this.txtContent.Value = mail_H.BodyMail;
                if (!string.IsNullOrWhiteSpace(mail_H.FilePath1))
                {
                    this.txtFile1.Text = Path.GetFileName(mail_H.FilePath1);
                    this.hdnFileUpload1.Value = this.txtFile1.Text;
                    this.hdnFileUpload1Old.Value = this.txtFile1.Text;
                    this.btnDownload1.Disabled = false;
                }
                if (!string.IsNullOrWhiteSpace(mail_H.FilePath2))
                {
                    this.txtFile2.Text = Path.GetFileName(mail_H.FilePath2);
                    this.hdnFileUpload2.Value = this.txtFile2.Text;
                    this.hdnFileUpload2Old.Value = this.txtFile2.Text;
                    this.btnDownload2.Disabled = false;
                }
                if (!string.IsNullOrWhiteSpace(mail_H.FilePath3))
                {
                    this.txtFile3.Text = Path.GetFileName(mail_H.FilePath3);
                    this.hdnFileUpload3.Value = this.txtFile3.Text;
                    this.hdnFileUpload3Old.Value = this.txtFile3.Text;
                    this.btnDownload3.Disabled = false;
                }
                this.hdnHID.Value = mail_H.ID.ToString();
                if (mail_H.ResendFlag == 1)
                {
                    this.chkResendFlag.Checked = true;
                }
                this.DraftMail = mail_H.DraftFlag;
                if ((this.DraftMail == 1) || (this.Mode == Mode.Update))
                {
                    this.ShowUserDaftMail();
                }
                this.cmbBaseDivision.SelectedIndex = mail_H.BaseDivision;
                this.cmbStartDivision.SelectedIndex = mail_H.StartDivision;
                this.cmbEndDivison.SelectedIndex = mail_H.EndDivison;
                this.txtStartDate.Text = mail_H.StartDate.ToString();
                this.txtEndDate.Text = mail_H.EndDate.ToString();
                this.txtResendInterval.Text = mail_H.ResendInterval.ToString();
                this.txtResendTime.Text = CommonUtil.IntToTime(mail_H.ResendTime, true);
                if (lstmail_D.Count != 0)
                {
                    foreach (var item in lstmail_D)
                    {
                        mUser = userSer.GetByID(item.UID);
                        if (mUser != null)
                        {
                            var strDateFM = "{0:" + Constants.FMT_DATETIME_SHOW + "}";
                            MailDetailInfo info = new MailDetailInfo
                            {
                                ID = mUser.ID,
                                HID = item.HID,
                                UserCD = EditDataUtil.ToFixCodeShow(mUser.UserCD, M_User.MAX_USER_CODE_SHOW),
                                UserName1 = mUser.UserName1,
                                UserName2 = mUser.UserName2,
                                ReceiveDate = string.Format(strDateFM, item.ReceiveDate)
                            };
                            lstDataMail.Add(info);
                        }
                    }
                    this.rptDetail.DataSource = lstDataMail;
                }
                else
                {
                    this.rptDetail.DataSource = null;
                }
                this.rptDetail.DataBind();
            }

            btnDownloadAllDisp.Attributes.Remove("class");
            if (HaveAttachFileInMail())
            {
                this.btnDownloadAllDisp.Disabled = false;
                btnDownloadAllDisp.Attributes.Add("class", "btn btn-info btn-sm");
            }
            else
            {
                this.btnDownloadAllDisp.Disabled = true;
                btnDownloadAllDisp.Attributes.Add("class", "btn btn-default btn-sm");
            }
        }

        /// <summary>
        /// show user draft email
        /// </summary>
        private void ShowUserDaftMail()
        {
            var lstMailD = this.GetListT_Mail_D(HID);
            if (lstMailD.Count != 0)
            {
                StringBuilder strUser = new StringBuilder();
                var i = 1;
                var lstUser = new List<M_User>();
                using (DB db = new DB())
                {
                    UserService userSer = new UserService(db);
                    foreach (var item in lstMailD)
                    {
                        //Get User Info
                        var mUser = userSer.GetByID(item.UID);
                        if (mUser != null)
                        {
                            strUser.Append(mUser.ID);
                            if (i != lstMailD.Count) { strUser.Append("|"); }
                            mUser.UserCD = EditDataUtil.ToFixCodeShow(mUser.UserCD, M_User.MAX_USER_CODE_SHOW);
                            lstUser.Add(mUser);
                        }
                        i += 1;
                    }
                    this.treeViewSave.Value = strUser.ToString();
                    this.rptUser.DataSource = lstUser;
                }
            }
            else
            {
                this.rptUser.DataSource = null;
            }
            this.rptUser.DataBind();
        }

        /// <summary>
        /// update attachment
        /// </summary>
        /// <param name="mail_H"></param>
        private void UpdateAttachments(T_Mail_H mail_H)
        {
            this.ClearAttachmentOld(this.hdnFileUpload1, this.hdnFileUpload1Old, mail_H.FilePath1);
            this.ClearAttachmentOld(this.hdnFileUpload2, this.hdnFileUpload2Old, mail_H.FilePath2);
            this.ClearAttachmentOld(this.hdnFileUpload3, this.hdnFileUpload3Old, mail_H.FilePath3);
        }

        /// <summary>
        /// update attachment
        /// </summary>
        private void ClearAttachmentOld(HiddenField hdnUpload, HiddenField hdnUploadOld, string filePath)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(hdnUpload.Value) || !string.IsNullOrWhiteSpace(hdnUploadOld.Value))
                {
                    if (!string.Equals(hdnUpload.Value, hdnUploadOld.Value))
                    {
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                        else { return; }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Instance.WriteLog(ex);
                return;
            }
        }

        /// <summary>
        /// take old message ID
        /// </summary>
        /// <param name="mail_H"></param>
        private string GetOldMessageId(T_Mail_H mail_H)
        {
            try
            {
                if (mail_H != null)
                {
                    var messageId = string.Empty;
                    if (!string.IsNullOrEmpty(mail_H.FilePath1))
                    {
                        var path = Path.GetDirectoryName(mail_H.FilePath1).Split(Path.DirectorySeparatorChar);
                        messageId = path[path.Length - 1];
                        return messageId;
                    }
                    if (!string.IsNullOrEmpty(mail_H.FilePath2))
                    {
                        var path = Path.GetDirectoryName(mail_H.FilePath2).Split(Path.DirectorySeparatorChar);
                        messageId = path[path.Length - 1];
                        return messageId;
                    }
                    if (!string.IsNullOrEmpty(mail_H.FilePath3))
                    {
                        var path = Path.GetDirectoryName(mail_H.FilePath3).Split(Path.DirectorySeparatorChar);
                        messageId = path[path.Length - 1];
                        return messageId;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Instance.WriteLog(ex);
            }
            var message = CreateMessage();
            return message.MessageId;
        }

        #region DB

        /// <summary>
        /// Set Model T_Mail_H
        /// </summary>
        /// <param name="hid">ID</param>
        /// <param name="lstPath">pathFile</param>
        /// <returns>HID</returns>
        private int Execute_T_Mail_H(DB db, List<String> lstPath)
        {
            Mail_HService mail_HSer = new Mail_HService(db);
            T_Mail_H model = new T_Mail_H();


            if (ViewState["ID"] != null)
            {
                model = this.Get_Mail_H_ByHID(HID);
            }
            if (!string.IsNullOrEmpty(model.Subject) && model.ReplyDueDate != null)
            {
                model = SetModel_Mail_H(model, lstPath);
                mail_HSer.Update(model);

                if (this.DraftMail != 1)
                {
                    mail_HSer.UpdateFlgDraft(HID, LoginInfo.User.ID, 0);
                    this.DraftMail = 0;
                }
                return HID;
            }
            else
            {
                model = SetModel_Mail_H(model, lstPath);
                model.CreateUID = this.LoginInfo.User.ID;
                model.UpdateUID = this.LoginInfo.User.ID;
                mail_HSer.Insert(model);
            }
            return db.GetIdentityId<T_Mail_H>();
        }

        /// <summary>
        /// set model t_mail_h
        /// </summary>
        /// <param name="model"></param>
        /// <param name="lstPath"></param>
        /// <returns></returns>
        private T_Mail_H SetModel_Mail_H(T_Mail_H model, List<String> lstPath)
        {

            int flgReSend = 0;
            if (this.chkResendFlag.Checked)
            {
                flgReSend = 1;
            }

            model.Subject = this.txtSubject.Text;
            model.ReplyDueDate = DateTime.Parse(this.txtReplyDueDate.Text.ToString());
            model.BodyMail = this.txtContent.Value;
            model.FilePath1 = lstPath[0];
            model.FilePath2 = lstPath[1];
            model.FilePath3 = lstPath[2];
            model.DraftFlag = DraftMail;
            model.ResendFlag = flgReSend;
            if (flgReSend == 1)
            {
                model.BaseDivision = this.cmbBaseDivision.SelectedIndex;
                model.StartDate = this.txtStartDate.IsEmpty ? 0 : int.Parse(this.txtStartDate.Text);
                model.StartDivision = this.cmbStartDivision.SelectedIndex;
                model.EndDate = this.txtEndDate.IsEmpty ? 0 : int.Parse(this.txtEndDate.Text);
                model.EndDivison = this.cmbEndDivison.SelectedIndex;
                model.ResendTime = this.txtResendTime.IsEmpty ? 0 : CommonUtil.TimeToInt(txtResendTime.Value);
                model.ResendInterval = this.txtResendInterval.IsEmpty ? 0 : int.Parse(this.txtResendInterval.Text);
            }
            else
            {
                model.BaseDivision = 0;
                model.StartDate = 0;
                model.StartDivision = 0;
                model.EndDate = 0;
                model.EndDivison = 0;
                model.ResendTime = 0;
                model.ResendInterval = 0;
            }
            return model;
        }

        /// <summary>
        /// Insert T_Mail_D
        /// </summary>
        /// <param name="mail_DSer">service</param>
        /// <param name="user">M_user</param>
        /// <param name="hid">ID Mail D</param>
        /// <returns></returns>
        private void Execute_T_Mail_D(Mail_DService mail_DSer, M_User user, int hid)
        {
            T_Mail_D model = new T_Mail_D
            {
                HID = hid,
                UID = user.ID,
                MailAddress = user.MailAddress,
                ReceiveFlag = 0,
                MailPath = string.Empty
            };
            mail_DSer.Insert(model);
        }

        /// <summary>
        /// Get Mail_D By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private T_Mail_H Get_Mail_H_ByHID(int id)
        {
            try
            {
                using (DB db = new DB())
                {
                    Mail_HService mail_HSer = new Mail_HService(db);
                    return mail_HSer.GetByID(id);
                }
            }
            catch (Exception ex)
            {
                Log.Instance.WriteLog(ex);
                return null;
            }
        }

        /// <summary>
        /// get list t_mail_d
        /// </summary>
        /// <returns></returns>
        private List<T_Mail_D> GetListT_Mail_D(int hid)
        {
            using (DB db = new DB())
            {
                Mail_DService mail_DSer = new Mail_DService(db);
                return mail_DSer.GetListByHID(hid).ToList();
            }
        }

        /// <summary>
        /// get list t_mail_d not reply
        /// </summary>
        /// <returns></returns>
        private List<T_Mail_D> GetListNotReply(int hid)
        {
            using (DB db = new DB())
            {
                Mail_DService mail_DSer = new Mail_DService(db);
                return mail_DSer.GetByListNotReply(hid).ToList();
            }
        }

        /// <summary>
        /// Clear old mail D
        /// </summary>
        /// <param name="hid"></param>
        private void Clear_T_Mail_D(DB db, int hid)
        {
            try
            {
                Mail_DService mail_DSer = new Mail_DService(db);
                var lstMail = this.GetListT_Mail_D(hid);
                if (lstMail.Count != 0)
                {
                    var ret = mail_DSer.DeleteAllByHId(hid);
                    if (ret == 0)
                    {
                        this.SetMessage(string.Empty, M_Message.MSG_UPDATE_FAILE, "メール");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Instance.WriteLog(ex);
                this.SetMessage(string.Empty, M_Message.MSG_UPDATE_FAILE, "メール");
                return;
            }
        }

        /// <summary>
        /// delete user in mail_D
        /// </summary>
        /// <param name="db"></param>
        /// <param name="lstUser"></param>
        /// <param name="hid"></param>
        private void RemoveMailDByListUser(DB db, List<int> lstUser, int hid)
        {
            var mail_DSer = new Mail_DService(db);
            foreach (var uid in lstUser)
            {
                var mail_D = mail_DSer.GetByPK(hid, uid);
                if (!string.IsNullOrEmpty(mail_D.MailPath))
                {
                    DeleteByMailPath(mail_D.MailPath);
                }
                mail_DSer.Delete(hid, uid);
            }
        }

        /// <summary>
        /// Check exist user not reply
        /// </summary>
        /// <param name="hid"></param>
        /// <returns></returns>
        private bool IsListUserReply(int hid)
        {
            using (DB db = new DB())
            {
                var mail_DSer = new Mail_DService(db);
                if (mail_DSer.GetCountUserNotReply(hid) > 0)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// get const string body mail
        /// </summary>
        /// <returns></returns>
        private string getBodyMailConst()
        {
            try
            {
                using (DB db = new DB())
                {
                    var mail_HSer = new Mail_HService(db);
                    return mail_HSer.GetByID(HID_DEFAULT_CONTENT).BodyMail;
                }
            }
            catch (Exception ex)
            {
                Log.Instance.WriteLog(ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// get mail path
        /// </summary>
        /// <param name="hid"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        private string GetMailPath(int hid, int uid)
        {
            using (DB db = new DB())
            {
                var mail_DSer = new Mail_DService(db);
                var mail_D = mail_DSer.GetByPK(hid, uid);
                if (mail_D != null)
                {
                    return mail_D.MailPath;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// check exist user in mail_D
        /// </summary>
        /// <param name="hid"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        private bool IsExistPK_Mail_D(int hid, int uid)
        {
            using (DB db = new DB())
            {
                var mail_DSer = new Mail_DService(db);
                var mail_D = mail_DSer.GetByPK(hid, uid);
                if (mail_D != null)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// get config name
        /// </summary>
        /// <returns></returns>
        private string GetNameConfig()
        {
            using (DB db = new DB())
            {
                try
                {
                    var config_D = new Config_DService(db);
                    var config_H = new Config_HService(db);
                    var Isdefalt = config_H.GetDefaultValueDrop(M_Config_H.CONFIG_MAIL_SEND_NAME);
                    if (Isdefalt != null && Isdefalt == "1")
                    {
                        return config_D.GetValue2(M_Config_H.CONFIG_MAIL_SEND_NAME, 1);
                    }
                    return string.Empty;
                }
                catch (Exception ex)
                {
                    Log.Instance.WriteLog(ex);
                    return string.Empty;
                }
            }
        }

        #endregion

        #endregion

        #region WebMethod
        /// <summary>
        /// GetDataTreeView
        /// </summary>
        /// <param name="existWorkingCalendar"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string GetDataTreeView(int HID)
        {
            try
            {
                IList<M_Department> lstDepartment;
                List<DepartmentTreeView> lstDepTreeView = new List<DepartmentTreeView>();
                using (DB db = new DB())
                {
                    DepartmentService DepartmentSer = new DepartmentService(db);
                    lstDepartment = DepartmentSer.GetAll();
                }

                lstDepTreeView = lstDepartment.OrderBy(l => l.ID)
                        .Select(l => new DepartmentTreeView
                        {
                            id = l.ID,
                            text = l.DepartmentName,
                            children = GetChildren(HID, l.ID)
                        }).ToList();

                return EditDataUtil.JsonSerializer<object>(lstDepTreeView);
            }
            catch (Exception ex)
            {
                Log.Instance.WriteLog(ex);
                return null;
            }
        }

        /// <summary>
        /// init data tree view right
        /// </summary>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string GetDataTreeRight(int HID)
        {
            try
            {
                IList<M_Department> lstDepartment;
                List<DepartmentTreeView> lstDepTreeView = new List<DepartmentTreeView>();
                using (DB db = new DB())
                {
                    DepartmentService DepartmentSer = new DepartmentService(db);
                    lstDepartment = DepartmentSer.GetAll();
                }
                lstDepTreeView = lstDepartment.OrderBy(l => l.ID)
                        .Select(l => new DepartmentTreeView
                        {
                            id = l.ID,
                            text = l.DepartmentName,
                            children = GetDefaultUser(HID, l.ID)
                        }).ToList();

                return EditDataUtil.JsonSerializer<object>(lstDepTreeView);
            }
            catch (Exception ex)
            {
                Log.Instance.WriteLog(ex);
                return null;
            }
        }

        /// <summary>
        /// get defaut user in draft mode
        /// </summary>
        /// <param name="HID"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public static List<UserTreeView> GetDefaultUser(int HID, int parentId)
        {
            List<UserTreeView> lstUserTreeView = new List<UserTreeView>();
            if (HID == 0)
            {
                return new List<UserTreeView>();
            }
            else
            {
                using (DB db = new DB())
                {
                    UserService UserSer = new UserService(db);
                    lstUserTreeView = UserSer.GetListUserByHID(HID, parentId).ToList();
                }
                return lstUserTreeView.OrderBy(l => l.id)
                    .Select(l => new UserTreeView
                    {
                        id = parentId.ToString() + '_' + l.id,
                        text = l.text,
                        departmentid = l.departmentid
                    }).ToList();
            }
        }
        /// <summary>
        /// GetChildren
        /// </summary>
        /// <param name="lstDepTreeView"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public static List<UserTreeView> GetChildren(int HID, int parentId)
        {
            M_User mUser = new M_User();
            List<int> lstID = new List<int>();
            List<UserTreeView> lstUserTreeView = new List<UserTreeView>();
            using (DB db = new DB())
            {
                UserService UserSer = new UserService(db);
                lstUserTreeView = UserSer.GetByUserExcept(HID, parentId).ToList();
            }
            if (lstUserTreeView == null)
            {
                return new List<UserTreeView>();
            }
            return lstUserTreeView.OrderBy(l => l.id)
                    .Select(l => new UserTreeView
                    {
                        id = parentId.ToString() + '_' + l.id,
                        text = l.text,
                        departmentid = l.departmentid
                    }).ToList();
        }

        /// <summary>
        /// GetCheckedTreeViewLeft
        /// </summary>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string GetCheckedTreeViewLeft(List<string> checkedIds)
        {
            try
            {
                string[] strUserId;
                List<int> lstUserIdLeft = new List<int>();
                foreach (var item in checkedIds)
                {
                    if (item.Contains('_'))
                    {
                        strUserId = item.Split('_');
                        lstUserIdLeft.Add(int.Parse(strUserId[1]));
                    }
                }

                return OMS.Utilities.EditDataUtil.JsonSerializer<object>(checkedIds);
            }
            catch (Exception ex)
            {
                Log.Instance.WriteLog(ex);
                return null;
            }
        }

        /// <summary>
        /// GetCheckedTreeViewRight
        /// </summary>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string GetCheckedTreeViewRight(List<string> checkedIds)
        {
            try
            {
                string[] strUserId;
                List<int> lstUserIdRight = new List<int>();
                foreach (var item in checkedIds)
                {
                    if (item.Contains('_'))
                    {
                        strUserId = item.Split('_');
                        lstUserIdRight.Add(int.Parse(strUserId[1]));
                    }
                }

                return OMS.Utilities.EditDataUtil.JsonSerializer<object>(checkedIds);
            }
            catch (Exception ex)
            {
                Log.Instance.WriteLog(ex);
                return null;
            }
        }

        /// <summary>
        /// Get data for TreeView form List User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string GetDataFromListUser(string lstParentIdUserId)
        {
            IList<M_Department> lstDepartment;
            List<DepartmentTreeView> lstDepTreeView = new List<DepartmentTreeView>();
            List<UserTreeView> lstUserTreeView = new List<UserTreeView>();

            string parentName = string.Empty;
            List<int> lstParentId = new List<int>();

            try
            {
                string[] strUserParentId = lstParentIdUserId.Split('|');
                using (DB db = new DB())
                {
                    DepartmentService DepartmentSer = new DepartmentService(db);
                    UserService userSer = new UserService(db);
                    lstDepartment = DepartmentSer.GetAll();
                    for (int i = 0; i < strUserParentId.Length; i++)
                    {

                        if (lstParentIdUserId != "")
                        {
                            string[] userParent = strUserParentId[i].Split('_');
                            M_User mUser = new M_User();
                            mUser = userSer.GetByID(int.Parse(userParent[1]));

                            UserTreeView userTreeView = new UserTreeView();
                            userTreeView.id = mUser.ID.ToString();
                            userTreeView.text = mUser.UserName1;
                            userTreeView.departmentid = int.Parse(userParent[0]);
                            lstUserTreeView.Add(userTreeView);
                        }
                    }
                }

                lstDepTreeView = lstDepartment.OrderBy(l => l.ID)
                    .Select(l => new DepartmentTreeView
                    {
                        id = l.ID,
                        text = l.DepartmentName,
                        children = (lstUserTreeView.Where(u => u.departmentid == l.ID).OrderBy(u => u.id)
                                    .Select(u => new UserTreeView
                                    {
                                        id = l.ID.ToString() + '_' + u.id,
                                        text = u.text,
                                        departmentid = u.departmentid
                                    }).ToList()
                                    )
                    }).ToList();

                return OMS.Utilities.EditDataUtil.JsonSerializer<object>(lstDepTreeView);
            }
            catch (Exception ex)
            {
                Log.Instance.WriteLog(ex);
                return null;
            }
        }

        /// <summary>
        /// Get Parent Node
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string GetParentNode(string userId)
        {
            int parentId = 0;
            string parentName = string.Empty;
            try
            {
                string[] strUserId = userId.Split('_');
                System.Text.StringBuilder result = new System.Text.StringBuilder();
                M_User mUser;
                M_Department mDepartment;
                using (DB db = new DB())
                {
                    UserService UserSer = new UserService(db);
                    DepartmentService departmentSer = new DepartmentService(db);
                    mUser = UserSer.GetByID(int.Parse(strUserId[1]));
                    mDepartment = departmentSer.GetDataDepartmentById(mUser.DepartmentID);
                    if (mUser != null)
                    {
                        parentId = mUser.DepartmentID;
                        parentName = mDepartment.DepartmentName;
                    }
                    result.Append("{");
                    result.Append(string.Format(" \"ParentId\":\"{0}\"", parentId));
                    result.Append(string.Format(", \"ParentName\":\"{0}\"", mDepartment.DepartmentName));
                    result.Append("}");
                }

                return result.ToString();
            }
            catch (Exception ex)
            {
                Log.Instance.WriteLog(ex);
                return null;
            }
        }
        
        /// <summary>
        /// GetReplyStatus
        /// </summary>
        /// <param name="UID"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static List<string> GetReplyStatus(int HID)
        {
            using (DB db = new DB())
            {
                try
                {
                    var mailHService = new Mail_HService(db);
                    var mailDService = new Mail_DService(db);
                    var userService = new UserService(db);
                    var mail_h = mailHService.GetByID(HID);
                    if (mail_h != null)
                    {
                        var listDetail = mailDService.GetListByHID(HID);
                        var listResult = new List<string>();
                        foreach (var item in listDetail)
                        {
                            if (item.ReceiveFlag != 0)
                            {
                                if (File.Exists(item.MailPath))
                                {
                                    try
                                    {
                                        var message = MimeMessage.Load(item.MailPath);
                                        if (message != null)
                                        {
                                            var body = message.TextBody;
                                            string status = string.Empty;
                                            List<int> lstAcceptIndex = new List<int>();
                                            List<int> lstCancelIndex = new List<int>();

                                           
                                            int currentIndex = body.Length;
                                            
                                            int acceptIndex1 = body.IndexOf("出席");
                                            int acceptIndex2 = body.IndexOf("参加");
                                            int cancelIndex1 = body.IndexOf("欠席");
                                            int cancelIndex2 = body.IndexOf("不参加");

                                            if (acceptIndex1 >= 0 && acceptIndex1 < currentIndex)
                                            {
                                                currentIndex = acceptIndex1;
                                                status = "出席";
                                            }

                                            if (acceptIndex2 >= 0 && acceptIndex2 < currentIndex)
                                            {
                                                currentIndex = acceptIndex2;
                                                status = "出席";
                                            }

                                            if (cancelIndex1 >= 0 && cancelIndex1 < currentIndex)
                                            {
                                                currentIndex = cancelIndex1;
                                                status = "欠席";
                                            }

                                            if (cancelIndex2 >= 0 && cancelIndex2 < currentIndex)
                                            {
                                                currentIndex = cancelIndex2;
                                                status = "欠席";
                                            }

                                            listResult.Add(string.Format("{0}:{1}", item.UID, status));
                                        }
                                    }
                                    catch (Exception) { }
                                }
                            }
                        }
                        return listResult;
                    }
                    return new List<string>();
                }
                catch (Exception ex)
                {
                    Log.Instance.WriteLog(ex);
                    return new List<string>();
                }

            }
        }
        /// <summary>
        /// Get Email User
        /// </summary>
        /// <param name="UID"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string GetDataEmailUser(int HID, int UID)
        {
            using (DB db = new DB())
            {
                try
                {
                    var mailHService = new Mail_HService(db);
                    var mailDService = new Mail_DService(db);
                    var userService = new UserService(db);
                    var mail_h = mailHService.GetByID(HID);
                    if (mail_h != null)
                    {
                        var mUser = userService.GetByID(UID);
                        var mail_D = mailDService.GetByPK(HID, UID);
                        if (mail_D != null)
                        {
                            var message = LoadToEmail(mail_D.MailPath);
                            message.MailAddress = mUser.UserName1;
                            message.ID = mail_D.UID;
                            message.HID = mail_D.HID;
                            message.ReplyDueDate = string.Format(Constants.FMR_DATE_YMD, mail_h.ReplyDueDate);
                            return EditDataUtil.JsonSerializer<object>(message);
                        }
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    Log.Instance.WriteLog(ex);
                    return null;
                }

            }
        }
        #endregion
    }
}