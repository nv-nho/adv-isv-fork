using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using OMS.Utilities;
using OMS.Models;
using OMS.DAC;
using OMS.Controls;
using System.Web.UI.HtmlControls;
using System.Data;
using MailKit;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MailKit.Net.Imap;
using OMS.Properties;
using System.Xml;
using System.Threading;
using System.Threading.Tasks;

namespace OMS.Expence
{
    public partial class FrmExpenceEntry : FrmBaseDetail
    {
        #region Cosntant
        private const string URL_LIST = "~/Expence/FrmExpenceList.aspx";
        private const string FOLDER_SEND_STRING = "sent";

        private enum ModeSendMail
        {
            Apply,
            Approve,
            Reject
        }

        #endregion

        #region static
        public static bool Back_List = false;
        public static int User_Project = 0;
        #endregion

        #region Property

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
        /// <summary>
        /// Get or set ExpenceID
        /// </summary>
        public int ExpenceID
        {
            get { return (int)ViewState["ID"]; }
            set { ViewState["ID"] = value; }
        }

        /// <summary>
        /// Get or set OldUpdateDate
        /// </summary>
        public DateTime OldUpdateDate
        {
            get { return (DateTime)ViewState["OldUpdateDate"]; }
            set { ViewState["OldUpdateDate"] = value; }
        }

        /// <summary>
        /// Get or set ApprovedFlag
        /// </summary>
        public int ApprovedFlag
        {
            get { return (int)ViewState["ApprovedFlag"]; }
            set { ViewState["ApprovedFlag"] = value; }
        }

        /// <summary>
        /// Get or set listValue4ExpenceType
        /// </summary>

        public IList<M_Config_D> listValue4ExpenceType = new List<M_Config_D>();
        #endregion

        #region Variable

        /// <summary>
        /// Index Sell
        /// </summary>
        private int _indexSell;
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
            base.FormTitle = "経費登録";
            base.FormSubTitle = "Detail";

            this.txtProjectCode.MaxLength = M_Project.PROJECT_CODE_SHOW_MAX_LENGTH;
            this.txtMemo.MaxLength = M_Project.PORJECT_MEMO_LENGTH;

            //Init Event
            LinkButton btnYes = (LinkButton)this.Master.FindControl("btnYes");
            btnYes.Click += new EventHandler(btnProcessData);

            LinkButton btnNo = (LinkButton)this.Master.FindControl("btnNo");
            btnNo.Click += new EventHandler(btnShowData);
        }

        /// <summary>
        /// Page Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Check authority of login user
            base.SetAuthority(FormId.ExpenceGroup);
            if (!this._authority.IsExpenceView)
            {
                Response.Redirect("~/Expence/FrmExpenceList.aspx");
            }

            using (DB db = new DB())
            {
                Config_DService dbSer = new Config_DService(db);
                //Get list detail
                IList<M_Config_D> listM_Config_D = dbSer.GetListByConfigCd(M_Config_H.CONFIG_CD_EXPENCE_TYPE);
                listValue4ExpenceType = listM_Config_D.OrderBy(f => f.Value1).ToList();
            }

            if (!this.IsPostBack)
            {
                if (this.PreviousPage != null)
                {
                    // set Default Value
                    this.GetDefaultValue();
                    //Save condition of previous page
                    this.ViewState["Condition"] = this.PreviousPageViewState["Condition"];

                    //Check mode
                    if (this.PreviousPageViewState["ID"] == null)
                    {
                        //Set mode
                        this.ProcessMode(Mode.Insert);

                        this.LoadCmbUserData(-1, false);
                        if (LoginInfo.User.ID != 1)
                        {
                            this.cmbUser.SelectedValue = LoginInfo.User.ID.ToString();
                            this.ddlDepartment.SelectedValue = LoginInfo.User.DepartmentID.ToString();
                        }

                        //Init detail list
                        this.InitDetailList();

                        this.txtExpenceAmount.Value = 0;
                        this.ApprovedFlag = 0;
                    }
                    else
                    {
                        //Get Expence ID
                        this.ExpenceID = int.Parse(PreviousPageViewState["ID"].ToString());
                        T_Expence_H t_expence_d = this.GetExpence(ExpenceID);

                        //Check t_expence_d
                        if (t_expence_d != null)
                        {
                            //Show data
                            this.ShowData(t_expence_d, false);

                            //Set Mode
                            this.ProcessMode(Mode.View);
                        }
                        else
                        {
                            Server.Transfer(URL_LIST);
                        }
                    }

                }
                else
                {
                    // set Default Value
                    this.GetDefaultValue();

                    //Set mode
                    this.ProcessMode(Mode.Insert);
                    this.LoadCmbUserData(-1, false);
                    if (LoginInfo.User.ID != 1)
                    {
                        this.cmbUser.SelectedValue = LoginInfo.User.ID.ToString();
                        this.ddlDepartment.SelectedValue = LoginInfo.User.DepartmentID.ToString();
                    }

                    //Init detail list
                    this.InitDetailList();

                    this.txtExpenceAmount.Value = 0;
                }
            }

            //Set init
            this.Success = false;
        }

        /// <summary>
        /// Show Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShowData(object sender, EventArgs e)
        {
            //Get T_Expence_H Header
            T_Expence_H header = this.GetExpence(ExpenceID);
            if (header != null)
            {
                //Show data
                this.ShowData(header, false);

                //Set Mode
                this.ProcessMode(Mode.View);
            }
            else
            {
                Server.Transfer(URL_LIST);
            }
        }

        /// <summary>
        /// Edit Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            //Get Expence ID
            T_Expence_H t_expence_d = this.GetExpence(ExpenceID);

            //Check t_expence_d
            if (t_expence_d != null)
            {
                //Show data
                this.ShowData(t_expence_d, false);

                //Set Mode
                this.ProcessMode(Mode.Update);
            }

            else
            {
                Server.Transfer(URL_LIST);
            }
        }

        /// <summary>
        /// Delete Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            //Set Model
            this.Mode = Mode.Delete;

            //Show question delete
            base.ShowQuestionMessage(M_Message.MSG_QUESTION_DELETE, Models.DefaultButton.No, true);
        }

        /// <summary>
        /// 承認
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnApprove_Click(object sender, EventArgs e)
        {
            //Set Model
            this.Mode = Mode.Approve;

            //Show question delete
            base.ShowQuestionMessage(M_Message.MSG_CONFIRM_SEND_MAIL_APPROVE, Models.DefaultButton.No, true);
        }

        /// <summary>
        /// 承認削除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btndenied_Click(object sender, EventArgs e)
        {
            //Set Model
            this.Mode = Mode.NotApprove;

            //Show question delete
            base.ShowQuestionMessage(M_Message.MSG_CONFIRM_SEND_MAIL_REJECT, Models.DefaultButton.No, true);
        }

        /// <summary>
        /// Event Button Update Submit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            this.ResetProjectName();
            //Check input
            if (!this.CheckInput())
            {
                return;
            }

            IList<T_Expence_D> details = this.GetDetailList(true);

            if (details.Count > 0)
            {
                this.rptDetail.DataSource = details;
                this.rptDetail.DataBind();
            }
            else
            {
                //Init detail list
                this.InitDetailList();
            }

            //Show question update
            base.ShowQuestionMessage(M_Message.MSG_QUESTION_UPDATE, Models.DefaultButton.Yes);
        }

        /// <summary>
        /// Event Button Back
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (this.Mode == Mode.View || this.Mode == Mode.Insert)
            {

                Server.Transfer(URL_LIST);
            }

            if (this.Mode == Mode.Update || this.Mode == Mode.Insert || this.Mode == Mode.Copy)
            {
                //Get hExpence by ID
                T_Expence_H hExpence = this.GetExpence(ExpenceID);

                //Check hExpence exists
                if (hExpence != null)
                {
                    this.Mode = Mode.View;

                    //Show data
                    this.ShowData(hExpence, false);

                    //Set Mode
                    this.ProcessMode(Mode.View);
                }
                else
                {
                    Server.Transfer(URL_LIST);
                }
            }
        }
        #endregion

        #region Method

        #region mail
        private void SendMail(int p_expenceID, ModeSendMail p_type = ModeSendMail.Apply)
        {
            try
            {
                if (!this._authority.IsExpenceMail)
                {
                    return;
                }

                string _subject = string.Empty;
                string _content = string.Empty;
                T_Expence_H header = this.GetExpence(p_expenceID);
                if (p_type == ModeSendMail.Apply)
                {
                    using (DB db = new DB())
                    {
                        ProjectService prService = new ProjectService(db);
                        M_Project m_Project = prService.GetDataProjectById(header.ProjectID);
                        if (m_Project.UserID == this.LoginInfo.User.ID)
                        {
                            return;
                        }
                    }
                }

                XmlNode node_email = this.getNodeXml(p_type);
                _subject = node_email.SelectSingleNode("subject").InnerText.Trim();
                _content = node_email.SelectSingleNode("content").InnerText.Trim();

                using (DB db = new DB())
                {
                    UserService userSer = new UserService(db);
                    M_User mUserSyonin = userSer.GetByID_Project(header.ProjectID);
                    M_User mUserSinsei = userSer.GetByID(header.UserID);

                    // 種別
                    Config_DService config_D = new Config_DService(db);
                    string expence_type = config_D.GetValue2(M_Config_H.CONFIG_CD_EXPENCE_TYPE, int.Parse(header.AccountCD));

                    List<M_User> lstUserTo = new List<M_User>();
                    if (p_type == ModeSendMail.Apply)
                    {
                        lstUserTo.Add(mUserSyonin);
                    }
                    else
                    {
                        lstUserTo = userSer.GetListUserByDepartmentCode(M_Department.DB_DEPARTMENT_CODE_2).ToList();

                        // 承認者　＝　総務者 ➔　✖
                        if (lstUserTo.Any(cus => cus.ID == this.LoginInfo.User.ID))
                        {
                            lstUserTo.RemoveAll(x => x.ID == this.LoginInfo.User.ID);
                        }

                        // 承認者　！＝　申請者　➔　〇
                        if (this.LoginInfo.User.ID != mUserSinsei.ID)
                        {
                            if (!lstUserTo.Any(cus => cus.ID == mUserSinsei.ID))
                            {
                                lstUserTo.Add(mUserSinsei);
                            }
                        }
                        else
                        {
                            if (lstUserTo.Any(cus => cus.ID == mUserSinsei.ID))
                            {
                                lstUserTo.RemoveAll(x => x.ID == mUserSinsei.ID);
                            }
                        }
                    }

                    using (SmtpClient client = new SmtpClient())
                    {
                        if (ConnectedServerSmtp(client))
                        {
                            using (ImapClient imap = new ImapClient())
                            {
                                var sendFolder = this.GetFolderSent(imap);
                                if (sendFolder == null)
                                {
                                    if (imap.IsConnected) { CloseConnectImap(imap); }
                                    return;
                                }

                                for (int i = 0; i < lstUserTo.Count; i++)
                                {
                                    BodyBuilder body = new BodyBuilder();
                                    if (p_type == ModeSendMail.Apply)
                                    {
                                        body.TextBody = string.Format(_content, lstUserTo[i].UserName1, mUserSinsei.UserName1,
                                            string.Format(Constants.FMR_DATE_YMD, header.Date),
                                            expence_type, header.ExpenceAmount);
                                    }

                                    if (p_type == ModeSendMail.Approve || p_type == ModeSendMail.Reject)
                                    {
                                        body.TextBody = string.Format(_content, lstUserTo[i].UserName1, mUserSinsei.UserName1, this.LoginInfo.User.UserName1,
                                            string.Format(Constants.FMR_DATE_YMD, header.Date),
                                            expence_type, header.ExpenceAmount);
                                    }

                                    var message = this.CreateMessage();
                                    message.To.Clear();
                                    message.To.Add(new MailboxAddress(lstUserTo[i].MailAddress));
                                    message.Subject = _subject;
                                    message.Body = body.ToMessageBody();

                                    sendFolder.Append(message); //sync email to server
                                    if (!SendMessage(client, message))
                                    {
                                        base.SetMessage(string.Empty, M_Message.MSG_USER_SEND_ERMAIL_ERROR, lstUserTo[i].UserName1);
                                    }
                                }

                                this.CloseConnectSmtp(client);
                                this.CloseConnectImap(imap);
                            }
                        }
                        else
                        {
                            this.SetMessage(string.Empty, M_Message.MSG_CONNECT_ERMAIL_ERROR, string.Empty);
                        }

                        System.Diagnostics.Debug.WriteLine(header.ExpenceNo);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Instance.WriteLog(ex);
                this.SetMessage(string.Empty, M_Message.MSG_CONNECT_ERMAIL_ERROR, string.Empty);
            }
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

        /// <summary>
        /// Get Default Value
        /// </summary>
        private void GetDefaultValue()
        {
            // Default data valide
            this.hdInValideDefault.Value = this.GetDefaultValueForDropdownList(M_Config_H.CONFIG_CD_EXPENCE_TYPE);
            //InitCombobox
            InitCombobox(this.cmbExpenseType, M_Config_H.CONFIG_CD_EXPENCE_TYPE);


            int _calID = Constants.DEFAULT_VALUE_INT;

            using (DB db = new DB())
            {
                IList<DropDownModel> departmentList;
                DepartmentService departmentService = new DepartmentService(db);
                departmentList = departmentService.GetDepartmentCbbData(_calID);
                ddlDepartment.Items.Clear();

                //ddlDepartment.DataSource = setDataTableCombobox(departmentList);
                ddlDepartment.DataSource = departmentList;
                ddlDepartment.DataValueField = "Value";
                ddlDepartment.DataTextField = "DisplayName";
                if (this.LoginInfo.User.DepartmentID >= 10)
                {
                    ddlDepartment.SelectedValue = this.LoginInfo.User.DepartmentID.ToString();
                }
                ddlDepartment.DataBind();
            }
        }

        private void LoadCmbUserData(int userID, bool isCopy)
        {
            using (DB db = new DB())
            {
                List<DropDownModel> listUser = new List<DropDownModel>();
                ProjectService prService = new ProjectService(db);

                if (this._authority.IsExpenceAllApproved)
                {
                    listUser = prService.GetCbbUserDataByDepartmentIDAndUserID(-1, -1).ToList();
                }
                else
                {
                    if (this._authority.IsExpenceOtherApply)
                    {
                        if (userID == this.LoginInfo.User.ID || this.Mode == Mode.Insert)
                        {
                            listUser = prService.GetCbbUserDataByDepartmentIDAndUserID(this.LoginInfo.User.DepartmentID, -1).ToList();
                        }
                        else
                        {
                            listUser = prService.GetCbbUserDataByDepartmentIDAndUserID(-1, -1).ToList();
                        }
                    }
                    else
                    {
                        listUser = prService.GetCbbUserDataByDepartmentIDAndUserID(-1, this.LoginInfo.User.ID).ToList();
                    }
                }

                listUser.Insert(0, new DropDownModel("-1", "---"));
                cmbUser.DataSource = listUser;
                cmbUser.DataValueField = "Value";
                cmbUser.DataTextField = "DisplayName";
                cmbUser.DataBind();

                cmbUser.SelectedValue = "-1";

                foreach (var item in listUser)
                {
                    if (item.Value == userID.ToString())
                    {
                        cmbUser.SelectedValue = userID.ToString();
                    }
                }
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
                return configSer.GetDataForDropDownList(configCD);

            }
        }

        /// <summary>
        /// GetDefaultValueForDropdownList
        /// </summary>
        /// <param name="configCD"></param>
        /// <returns></returns>
        private string GetDefaultValueForDropdownList(string configCD)
        {
            using (DB db = new DB())
            {
                Config_HService configSer = new Config_HService(db);
                return configSer.GetDefaultValueDrop(configCD);
            }
        }

        /// <summary>
        /// Process Mode
        /// </summary>
        /// <param name="mode">Mode</param>
        protected void ProcessMode(Mode mode)
        {
            bool enable;
            //Set Model
            this.Mode = mode;

            //Check model
            switch (mode)
            {
                case Mode.Copy:
                case Mode.Insert:
                    this.dtAccountingDate.Value = DateTime.Now;
                    if (base._authority.IsExpenceAllApproved)
                    {
                        this.cmbUser.Enabled = true;
                    }
                    else
                    {
                        if (base._authority.IsExpenceOtherApply)
                        {
                            this.cmbUser.Enabled = true;
                        }
                        else
                        {
                            this.cmbUser.Enabled = false;
                        }
                    }

                    enable = false;
                    break;

                case Mode.Update:
                    if (base._authority.IsExpenceAllApproved)
                    {
                        this.cmbUser.Enabled = true;
                    }
                    else
                    {
                        if (base._authority.IsExpenceOtherApply)
                        {
                            this.cmbUser.Enabled = true;
                        }
                        else
                        {
                            this.cmbUser.Enabled = false;
                        }
                    }
                    enable = false;
                    break;

                default:
                    enable = true;
                    var expence = GetExpence(ExpenceID);
                    ShowData(expence, false);

                    if (!base._authority.IsExpenceEdit)
                    {
                        base.DisabledLink(this.btnEdit, true);
                    }
                    else
                    {
                        if (this.cmbUser.SelectedValue == this.LoginInfo.User.ID.ToString())
                        {
                            base.DisabledLink(this.btnEdit, this.ApprovedFlag == 1);
                        }
                        else
                        {
                            base.DisabledLink(this.btnEdit, true);
                        }

                    }

                    if (!base._authority.IsExpenceDelete)
                    {
                        base.DisabledLink(this.btnDelete, true);
                    }
                    else
                    {
                        if (this.cmbUser.SelectedValue == this.LoginInfo.User.ID.ToString())
                        {
                            base.DisabledLink(this.btnDelete, this.ApprovedFlag == 1);
                        }
                        else
                        {
                            base.DisabledLink(this.btnDelete, true);
                        }
                    }

                    if (!base._authority.IsExpenceCopy)
                    {
                        base.DisabledLink(this.btnCopy, true);
                    }
                    else
                    {
                        if (this.cmbUser.SelectedValue == this.LoginInfo.User.ID.ToString())
                        {
                            base.DisabledLink(this.btnCopy, this.ApprovedFlag == 1);
                        }
                        else
                        {
                            base.DisabledLink(this.btnCopy, true);
                        }
                    }

                    if (!base._authority.IsExpenceNew)
                    {
                        base.DisabledLink(this.btnInsert, true);
                    }
                    else
                    {
                        base.DisabledLink(this.btnInsert, this.ApprovedFlag == 1);
                    }

                    if (base._authority.IsExpenceAllApproved)
                    {
                        base.DisabledLink(this.btnApprove, false);
                        base.DisabledLink(this.btnApproveDelete, false);
                    }
                    else
                    {
                        bool flgApp = true;
                        bool flgAppD = true;
                        if (expence != null)
                        {
                            flgApp = getUserProject(expence.ProjectID) != this.LoginInfo.User.ID;
                            flgAppD = expence.UpdateUID != this.LoginInfo.User.ID;
                        }
                        base.DisabledLink(this.btnApprove, !base._authority.IsExpenceAccept || flgApp);
                        base.DisabledLink(this.btnApproveDelete, !base._authority.IsExpenceAccept || flgAppD);
                    }

                    this.cmbUser.Enabled = !enable;
                    break;
            }

            txtExpenceAmount.Attributes.Add("readonly", "true");
            this.txtExpenceNo.ReadOnly = true;
            this.dtAccountingDate.ReadOnly = enable;
            this.txtProjectCode.ReadOnly = enable;
            this.txtProjectName.ReadOnly = true;
            this.txtMemo.ReadOnly = enable;
            this.ddlDepartment.Enabled = !enable;
            this.cmbExpenseType.Enabled = !enable;

            this.btnProjectSearch.Disabled = enable;
        }


        /// <summary>
        /// Get Data Expence By Id
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        private T_Expence_H GetExpence(int expenceID)
        {
            using (DB db = new DB())
            {
                Expence_HService expenceSer = new Expence_HService(db);
                return expenceSer.GetByID(expenceID);
            }
        }

        /// <summary>
        /// Show Data
        /// </summary>
        /// <param name="ExpenceH"></param>
        private void ShowData(T_Expence_H ExpenceH, bool isCopy)
        {

            M_Project projectp = null;

            if (ExpenceH != null)
            {
                //Get data
                using (DB db = new DB())
                {
                    ProjectService projectSer = new ProjectService(db);
                    projectp = projectSer.GetDataProjectById(ExpenceH.ProjectID);
                }

                this.txtExpenceNo.Value = EditDataUtil.ToFixCodeShow(ExpenceH.ExpenceNo, T_Expence_H.EXPENCE_CODE_SHOW_MAX_LENGTH);
                this.dtAccountingDate.Value = ExpenceH.Date;
                this.cmbExpenseType.SelectedValue = ExpenceH.AccountCD;
                this.ddlDepartment.SelectedValue = ExpenceH.DepartmentID.ToString();
                this.LoadCmbUserData(ExpenceH.UserID, isCopy);
                this.txtProjectCode.Value = projectp.ProjectCD;
                this.txtProjectName.Value = projectp.ProjectName;
                this.txtExpenceAmount.Value = ExpenceH.ExpenceAmount;
                this.txtMemo.Value = ExpenceH.Memo;
                this.OldUpdateDate = ExpenceH.UpdateDate;
                this.ApprovedFlag = ExpenceH.ApprovedFlag;
                // Show detail Data
                ShowDetailData(ExpenceH.ID);
            }
        }
        private int getUserProject(int ProjectID)
        {
            using (DB db = new DB())
            {
                UserService userSer = new UserService(db);
                M_User M_user_project = new M_User();
                M_user_project = userSer.GetByID_Project(ProjectID);
                User_Project = M_user_project.ID;
                return User_Project;
            }
        }
        /// <summary>
        /// Reset Project Name
        /// </summary>
        private void ResetProjectName()
        {
            this.txtProjectName.Value = string.Empty;
            if (!this.txtProjectCode.IsEmpty)
            {
                using (DB db = new DB())
                {
                    ProjectService projectSer = new ProjectService(db);
                    M_Project dp = projectSer.GetByProjectCd(this.txtProjectCode.Value);
                    if (dp != null)
                    {
                        this.txtProjectName.Value = dp.ProjectName;
                    }
                }
            }
        }

        /// <summary>
        /// Show detail data on form
        /// </summary>
        /// <param name="headerID">Header ID</param>
        private void ShowDetailData(int HID)
        {
            IList<T_Expence_D> ListTExpenceD;
            using (DB db = new DB())
            {

                ListTExpenceD = this.GetListExpenceDById(HID, db);

                foreach (var item in ListTExpenceD)
                {
                    item.TaxRate = item.TaxRate * 100;
                }

                if (ListTExpenceD.Count == 0)
                {
                    //Get new list from screen
                    List<T_Expence_D> listDetail = new List<T_Expence_D>();
                    listDetail.Add(new T_Expence_D());
                    rptDetail.DataSource = listDetail;
                    rptDetail.DataBind();
                }
                else
                {
                    rptDetail.DataSource = ListTExpenceD;
                    rptDetail.DataBind();
                }
            }
        }

        /// <summary>
        /// Get T_Expence_D BY ID
        /// </summary>
        /// <param name="expenceDId">T_Expence_D ID</param>
        /// <returns>T_Expence_D</returns>
        private IList<T_Expence_D> GetListExpenceDById(int expenceDId, DB db)
        {
            Expence_DService expence_DService = new Expence_DService(db);

            //Get WorkingCalendar
            return expence_DService.GetListByHID(expenceDId);
        }

        /// <summary>
        /// Init Detail List
        /// </summary>
        private void InitDetailList()
        {
            //Add data
            IList<T_Expence_D> listDetail = new List<T_Expence_D>();
            listDetail.Add(new T_Expence_D());


            if (listValue4ExpenceType[Int32.Parse(cmbExpenseType.SelectedValue) - 1].Value4.Equals("1"))
            {
                listDetail.First().RouteType = Int32.Parse(this.GetDefaultValueForDropdownList(M_Config_H.CONFIG_CD_ROUTE_TYPE));
            }
            listDetail.First().TaxType = Int32.Parse(this.GetDefaultValueForDropdownList(M_Config_H.CONFIG_CD_TAX_TYPE));
            if (listDetail.First().TaxType == 3 && !listValue4ExpenceType[Int32.Parse(cmbExpenseType.SelectedValue) - 1].Value4.Equals("1"))
            {
                listDetail.First().TaxRate = 0;
            }
            else
            {
                listDetail.First().TaxRate = Int32.Parse(this.GetDefaultValueForDropdownList(M_Config_H.CONFIG_CD_TAX_RATE));
            }
            //Process list view
            this.rptDetail.DataSource = listDetail;
            this.rptDetail.DataBind();
        }

        /// <summary>
        /// Event Button RemoveRow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRemoveRow_Click(object sender, EventArgs e)
        {
            this.ResetProjectName();
            //Get new list from screen
            var listDetail = this.GetDetailList(false);

            //Get list index remove item
            List<int> listDel = new List<int>();
            for (int i = listDetail.Count - 1; i >= 0; i--)
            {
                if (listDetail[i].DelFlag)
                {
                    listDel.Add(i);
                }

                listDetail[i].DelFlag = false;
            }

            //Remove row
            foreach (var item in listDel)
            {
                listDetail.RemoveAt(item);
            }

            if (listDetail.Count == 0)
            {
                listDetail.Add(new T_Expence_D());
            }

            //Process list view
            this.rptDetail.DataSource = listDetail;
            this.rptDetail.DataBind();
        }

        /// <summary>
        /// Event Button AddRow
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnAddRow_Click(object sender, EventArgs e)
        {
            this.ResetProjectName();
            //Get new list from screen
            var listDetail = this.GetDetailList(false);
            if (listDetail != null)
            {

                T_Expence_D newRow = new T_Expence_D();
                if (listValue4ExpenceType[Int32.Parse(cmbExpenseType.SelectedValue) - 1].Value4.Equals("1"))
                {
                    newRow.RouteType = Int32.Parse(this.GetDefaultValueForDropdownList(M_Config_H.CONFIG_CD_ROUTE_TYPE));
                }
                newRow.TaxType = Int32.Parse(this.GetDefaultValueForDropdownList(M_Config_H.CONFIG_CD_TAX_TYPE));

                if (newRow.TaxType == 3 && !listValue4ExpenceType[Int32.Parse(cmbExpenseType.SelectedValue) - 1].Value4.Equals("1"))
                {
                    newRow.TaxRate = 0;
                }
                else
                {
                    newRow.TaxRate = Int32.Parse(this.GetDefaultValueForDropdownList(M_Config_H.CONFIG_CD_TAX_RATE));
                }
                listDetail.Add(newRow);
            }

            //Process list view
            this.rptDetail.DataSource = listDetail;
            this.rptDetail.DataBind();

        }

        /// <summary>
        /// Up Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUp_Click(object sender, EventArgs e)
        {
            this.ResetProjectName();
            //Get Data
            this.SwapDetail(true);
        }

        /// <summary>
        /// Down Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDown_Click(object sender, EventArgs e)
        {
            this.ResetProjectName();
            //Get Data
            this.SwapDetail(false);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCopy_Click(object sender, EventArgs e)
        {
            T_Expence_H m_Expence_h = this.GetExpence(this.ExpenceID);

            if (m_Expence_h != null)
            {
                //Show data
                this.ShowData(m_Expence_h, true);

                //Set Mode
                this.ProcessMode(Mode.Copy);

                this.txtExpenceNo.Value = string.Empty;
            }
            else
            {
                Server.Transfer(URL_LIST);
            }
        }

        /// <summary>
        /// Event Button Insert Submit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnInsert_Click(object sender, EventArgs e)
        {
            this.ResetProjectName();

            //Check input
            if (!this.CheckInput())
            {
                return;
            }

            IList<T_Expence_D> details = this.GetDetailList(true);

            if (details.Count > 0)
            {
                this.rptDetail.DataSource = details;
                this.rptDetail.DataBind();
            }
            else
            {
                //Init detail list
                this.InitDetailList();
            }

            //Show question insert
            base.ShowQuestionMessage(M_Message.MSG_CONFIRM_SEND_MAIL_APPLY, Models.DefaultButton.Yes);
        }

        /// <summary>
        /// Process Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnProcessData(object sender, EventArgs e)
        {
            bool ret;

            //Check Mode
            switch (this.Mode)
            {
                case Utilities.Mode.Copy:
                case Utilities.Mode.Insert:

                    //Insert Data
                    int newID = 0;
                    ret = this.InsertData(ref newID);

                    if (ret)
                    {
                        Task.Factory.StartNew(() => SendMail(newID, ModeSendMail.Apply));

                        //Set Mode
                        GetExpenceNoMax();
                        this.ProcessMode(Mode.View);
                        //Set Success
                        this.Success = true;
                    }

                    break;
                case Utilities.Mode.Delete:

                    //Delete data
                    ret = this.DeleteData();
                    if (!ret)
                    {
                        //Set Mode
                        this.ProcessMode(Mode.View);
                    }
                    else
                    {
                        Server.Transfer(URL_LIST);
                    }
                    break;

                case Utilities.Mode.Approve:
                case Utilities.Mode.NotApprove:

                    ret = this.ApproveData();

                    if (!ret)
                    {
                        this.ProcessMode(Mode.View);
                    }
                    else
                    {
                        if (Mode == Mode.Approve)
                        {
                            Task.Factory.StartNew(() => SendMail(this.ExpenceID, ModeSendMail.Approve));
                        }

                        if (Mode == Mode.NotApprove)
                        {
                            Task.Factory.StartNew(() => SendMail(this.ExpenceID, ModeSendMail.Reject));
                        }

                        Server.Transfer(URL_LIST);
                    }
                    break;

                default:
                    //Update Data
                    int res = this.UpdateData();
                    if (res == 1)
                    {
                        Task.Factory.StartNew(() => SendMail(this.ExpenceID, ModeSendMail.Apply));
                    }

                    if (res != -1)
                    {
                        //Set Mode
                        this.ProcessMode(Mode.View);

                        //Set Success
                        this.Success = true;
                        if (Back_List)
                        {
                            Back_List = false;
                            Server.Transfer(URL_LIST);
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Check input
        /// </summary>
        /// <returns>Valid:true, Invalid:false</returns>
        private bool CheckInput()
        {
            //dtAccountingDate
            if (this.dtAccountingDate.IsEmpty)
            {
                this.SetMessage(this.dtAccountingDate.ID, M_Message.MSG_REQUIRE, "計上日");
            }

            //txtUserCode
            if (this.cmbUser.SelectedIndex == 0)
            {
                this.SetMessage(this.cmbUser.ID, M_Message.MSG_REQUIRE, "支払先（社員）");
            }

            //txtProjectCode
            if (this.txtProjectCode.IsEmpty)
            {
                this.txtProjectName.Value = string.Empty;
                this.SetMessage(this.txtProjectCode.ID, M_Message.MSG_REQUIRE, "負担プロジェクトコード");
            }
            else
            {
                using (DB db = new DB())
                {
                    ProjectService projectSer = new ProjectService(db);
                    M_Project dp = projectSer.GetByProjectCd(this.txtProjectCode.Value);
                    if (dp == null)
                    {
                        this.txtProjectName.Value = string.Empty;
                        this.SetMessage(this.txtProjectCode.ID, M_Message.MSG_NOT_EXIST_CODE, "負担プロジェクトコード");
                    }
                }
            }

            foreach (RepeaterItem item in this.rptDetail.Items)
            {

                if (this.IsEmptyRow(item))
                {
                    continue;
                }

                int rowIndex = item.ItemIndex + 1;

                //dtDate
                IDateTextBox dtDate = (IDateTextBox)item.FindControl("dtDate");
                HtmlGenericControl divDate = (HtmlGenericControl)item.FindControl("divdtDate");
                if (string.IsNullOrEmpty(dtDate.Text))
                {
                    base.SetMessage(dtDate.ID + "_" + item.ItemIndex.ToString(), M_Message.MSG_REQUIRE_GRID, "利用日", rowIndex);
                    this.AddErrorForListItem(divDate, dtDate.ID + "_" + item.ItemIndex.ToString());
                }
                else
                {
                    ClearErrorForListItem(divDate);
                }
                //txtPaidTo
                ITextBox txtPaidTo = (ITextBox)item.FindControl("txtPaidTo");
                HtmlGenericControl divPaidTo = (HtmlGenericControl)item.FindControl("divPaidTo");
                if (string.IsNullOrEmpty(txtPaidTo.Text))
                {
                    base.SetMessage(txtPaidTo.ID + "_" + item.ItemIndex.ToString(), M_Message.MSG_REQUIRE_GRID, "支払先", rowIndex);
                    this.AddErrorForListItem(divPaidTo, txtPaidTo.ID + "_" + item.ItemIndex.ToString());
                }
                else
                {
                    ClearErrorForListItem(divPaidTo);
                }

                //txtAmount
                INumberTextBox txtAmount = (INumberTextBox)item.FindControl("txtAmount");
                HtmlGenericControl divAmount = (HtmlGenericControl)item.FindControl("divAmount");
                if (string.IsNullOrEmpty(txtAmount.Text))
                {
                    base.SetMessage(txtAmount.ID + "_" + item.ItemIndex.ToString(), M_Message.MSG_REQUIRE_GRID, "金額", rowIndex);
                    this.AddErrorForListItem(divAmount, txtAmount.ID + "_" + item.ItemIndex.ToString());
                }
                else
                {
                    ClearErrorForListItem(divAmount);
                }

            }
            //Check error
            return !base.HaveError;
        }

        /// <summary>
        /// Add display error for control
        /// </summary>
        /// <param name="divCtrl">div error control</param>
        /// <param name="errorKey">Error Control ID</param>
        private void AddErrorForListItem(HtmlGenericControl divCtrl, string errorKey)
        {
            divCtrl.Attributes.Add("class", "form-group " + base.GetClassError(errorKey));
        }

        /// <summary>
        /// Add display error for control
        /// </summary>
        /// <param name="divCtrl">div error control</param>
        /// <param name="errorKey">Error Control ID</param>
        private void ClearErrorForListItem(HtmlGenericControl divCtrl)
        {
            divCtrl.Attributes.Add("class", "form-group");
        }

        /// <summary>
        /// Insert data
        /// </summary>
        private bool InsertData(ref int newId)
        {
            try
            {
                T_Expence_H header = this.GetHeader();

                using (DB db = new DB())
                {
                    Expence_HService headerService = new Expence_HService(db);
                    Expence_DService detailService = new Expence_DService(db);

                    // Insert Header (T_Attendance)
                    headerService.Insert(header);
                    ProjectService prService = new ProjectService(db);
                    M_Project m_Project = prService.GetDataProjectById(header.ProjectID);
                    newId = db.GetIdentityId<T_Expence_H>();

                    List<T_Expence_D> detailExpenceD = this.GetDetailList(true);

                    foreach (var item in detailExpenceD)
                    {
                        item.TaxRate = item.TaxRate / 100;
                        item.HID = newId;
                        detailService.Insert(item);
                    }

                    db.Commit();
                }
            }
            catch (Exception ex)
            {
                this.SetMessage(this.txtExpenceNo.ID, M_Message.MSG_UPDATE_FAILE, "新規");

                Log.Instance.WriteLog(ex);

                return false;
            }

            return true;
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <returns>Success:true, Faile:false</returns>
        private bool DeleteData()
        {
            try
            {
                int ret = 0;
                using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                {
                    //Delete detailExpenceD
                    Expence_DService detailExpence_DService = new Expence_DService(db);
                    detailExpence_DService.DeleteAllByHId(ExpenceID);

                    //Delete HeaderExpenceH
                    Expence_HService headerWork_HService = new Expence_HService(db);
                    ret = headerWork_HService.Delete(ExpenceID, this.OldUpdateDate);
                    if (ret == 1)
                    {
                        db.Commit();
                    }
                }

                //Check result update
                if (ret == 0)
                {
                    //data change (check two session)
                    this.SetMessage(string.Empty, M_Message.MSG_DATA_CHANGED);
                    return false;
                }
            }
            catch (Exception ex)
            {
                this.SetMessage(string.Empty, M_Message.MSG_UPDATE_FAILE, "削除");

                Log.Instance.WriteLog(ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 申請
        /// </summary>
        /// <returns>Success:true, Faile:false</returns>
        private bool ApproveData()
        {
            try
            {
                int ret = 0;
                T_Expence_H header = this.GetExpence(ExpenceID);
                if (header != null)
                {
                    header = this.GetHeader(header);
                    header.ID = this.ExpenceID;

                    //Update
                    using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                    {
                        if (header.Status == DataStatus.Changed || this.IsDetailChange(db))
                        {
                            //Update header
                            Expence_HService headerService = new Expence_HService(db);
                            ret = headerService.Update(header);
                            if (ret != 0)
                            {
                                db.Commit();
                            }
                        }
                    }
                }

                //Check result update
                if (ret == 0)
                {
                    //data had changed
                    this.SetMessage(string.Empty, M_Message.MSG_DATA_CHANGED);
                    return false;
                }
            }
            catch (Exception ex)
            {
                this.SetMessage(string.Empty, M_Message.MSG_UPDATE_FAILE, "承認");

                Log.Instance.WriteLog(ex);
                throw;
            }
            return true;
        }

        /// <summary>
        /// Update Data
        /// </summary>
        /// <returns>Success:true, Faile:false</returns>
        private int UpdateData()
        {
            try
            {
                int ret = 0;
                T_Expence_H header = this.GetExpence(ExpenceID);
                if (header != null)
                {
                    header = this.GetHeader(header);
                    header.ID = this.ExpenceID;

                    //Update
                    using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                    {
                        if (header.Status == DataStatus.Changed || this.IsDetailChange(db))
                        {
                            //Update header
                            Expence_HService headerService = new Expence_HService(db);
                            ret = headerService.Update(header);
                            if (ret != 0)
                            {
                                //Update detail
                                Expence_DService detailExpence_DService = new Expence_DService(db);
                                detailExpence_DService.DeleteAllByHId(ExpenceID);

                                List<T_Expence_D> detail = this.GetDetailList(true);
                                foreach (var item in detail)
                                {
                                    item.TaxRate = item.TaxRate / 100;
                                    item.HID = header.ID;
                                    detailExpence_DService.Insert(item);
                                }

                                db.Commit();
                            }
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }

                //Check result update
                if (ret == 0)
                {
                    //data had changed
                    this.SetMessage(string.Empty, M_Message.MSG_DATA_CHANGED);
                    return -1;
                }
            }
            catch (Exception ex)
            {
                this.SetMessage(string.Empty, M_Message.MSG_UPDATE_FAILE, "更新");

                Log.Instance.WriteLog(ex);

                return -1;
            }

            return 1;
        }

        /// <summary>
        /// Check detail change data
        /// </summary>
        /// <returns></returns>
        private bool IsDetailChange(DB db)
        {
            //Get list from data
            IList<T_Expence_D> listDetailData;

            Expence_DService expence_DService = new Expence_DService(db);
            listDetailData = expence_DService.GetListByHID(this.ExpenceID);

            //Get list from screen
            List<T_Expence_D> listDetailSrceen = this.GetDetailList(true);

            //Check count change
            if (listDetailSrceen.Count != listDetailData.Count)
            {
                return true;
            }
            else
            {
                for (int i = 0; i < listDetailSrceen.Count; i++)
                {
                    var oldData = listDetailData[i];
                    var newData = listDetailSrceen[i];


                    if (oldData.Date != newData.Date)
                    {
                        return true;
                    }

                    if (oldData.PaidTo.CompareTo(newData.PaidTo) != 0)
                    {
                        return true;
                    }

                    if (listValue4ExpenceType[Int32.Parse(cmbExpenseType.SelectedValue) - 1].Value4.Equals("1"))
                    {
                        if (oldData.RouteFrom.CompareTo(newData.RouteFrom) != 0)
                        {
                            return true;
                        }

                        if (oldData.RouteTo.CompareTo(newData.RouteTo) != 0)
                        {
                            return true;
                        }

                        if (oldData.RouteType.CompareTo(newData.RouteType) != 0)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (oldData.Note.CompareTo(newData.Note) != 0)
                        {
                            return true;
                        }
                    }

                    if (oldData.TaxType.CompareTo(newData.TaxType) != 0)
                    {
                        return true;
                    }

                    if (oldData.TaxRate.CompareTo(newData.TaxRate / 100) != 0)
                    {
                        return true;
                    }

                    if (oldData.Amount != newData.Amount)
                    {
                        return true;
                    }

                    if (oldData.TaxAmount != newData.TaxAmount)
                    {
                        return true;
                    }

                }
            }
            return false;
        }

        /// <summary>
        /// Up or Down Item
        /// </summary>
        /// <param name="up">Up Flg</param>
        private void SwapDetail(bool up)
        {
            IList<T_Expence_D> details = this.GetDetailList(true);

            if (details.Count == 0)
            {
                return;
            }

            IList<T_Expence_D> results = new List<T_Expence_D>();

            if (up)
            {
                results.Add(details.First());
                for (int i = 1; i < details.Count; i++)
                {
                    var item = details[i];
                    var itemPre = results[i - 1];

                    if (item.Checked)
                    {
                        if (itemPre.Checked)
                        {
                            results.Add(item);
                        }
                        else
                        {
                            results.Insert(i - 1, item);
                        }
                    }
                    else
                    {
                        results.Add(item);
                    }
                }
            }
            else
            {
                results.Add(details.Last());
                for (int i = details.Count - 2; i >= 0; i--)
                {
                    var item = details[i];
                    if (item.Checked)
                    {
                        if (results[0].Checked)
                        {
                            results.Insert(0, item);
                        }
                        else
                        {
                            results.Insert(1, item);
                        }
                    }
                    else
                    {
                        results.Insert(0, item);
                    }
                }
            }

            this._indexSell = 1;
            for (int i = 0; i < results.Count; i++)
            {
                var row = results[i];
                row.LineNo = this._indexSell++;
            }

            this.rptDetail.DataSource = results;
            this.rptDetail.DataBind();
        }

        /// <summary>
        /// Repeater Detail Item Data Bound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptDetail_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var temp = (T_Expence_D)e.Item.DataItem;
                //Find the DropDownList in the Repeater Item.
                DropDownList cmbTaxType = (e.Item.FindControl("cmbTaxType") as DropDownList);
                InitCombobox(cmbTaxType, M_Config_H.CONFIG_CD_TAX_TYPE);



                if (listValue4ExpenceType[Int32.Parse(cmbExpenseType.SelectedValue) - 1].Value4.Equals("1"))
                {
                    DropDownList cmbRouteType = (e.Item.FindControl("cmbRouteType") as DropDownList);
                    InitCombobox(cmbRouteType, M_Config_H.CONFIG_CD_ROUTE_TYPE);
                    cmbRouteType.SelectedValue = temp.RouteType.ToString();
                    cmbTaxType.SelectedValue = "2";
                    cmbTaxType.Enabled = false;
                }
                else
                {
                    cmbTaxType.SelectedValue = temp.TaxType.ToString();
                }

                DropDownList cmbTaxRate = (e.Item.FindControl("cmbTaxRate") as DropDownList);
                InitCombobox(cmbTaxRate, M_Config_H.CONFIG_CD_TAX_RATE);

                cmbTaxRate.SelectedValue = decimal.ToInt32(temp.TaxRate).ToString();

                INumberTextBox txtTaxAmount = (e.Item.FindControl("txtTaxAmount") as INumberTextBox);
                ITextBox txtTaxAmountDisp = (e.Item.FindControl("txtTaxAmountDisp") as ITextBox);

                if (cmbTaxType.SelectedValue.Equals("3"))
                {
                    cmbTaxRate.Enabled = false;
                    txtTaxAmount.Attributes.Add("readonly", "true");
                }

                if (cmbTaxRate.SelectedValue.Equals("0"))
                {
                    txtTaxAmount.Attributes.Add("readonly", "true");
                }

            }

        }

        /// <summary>
        /// Get Expence No Max
        /// </summary>
        /// <returns></returns>
        void GetExpenceNoMax()
        {
            using (DB db = new DB())
            {
                Expence_HService prService = new Expence_HService(db);
                this.txtExpenceNo.Value = prService.GetCurrentID().ToString("D" + T_Expence_H.EXPENCE_CODE_SHOW_MAX_LENGTH);
                ExpenceID = prService.GetByExpenceNo(txtExpenceNo.Value).ID;
            }
        }

        /// <summary>
        /// Get Expence No
        /// </summary>
        /// <returns></returns>
        private T_Expence_H GetExpenceNo(string expenceNo)
        {
            using (DB db = new DB())
            {
                Expence_HService expenceSer = new Expence_HService(db);
                return expenceSer.GetByExpenceNo(expenceNo);
            }
        }

        #endregion
        #region Web Methods

        /// <summary>
        /// Get Project Name By Project Code
        /// </summary>
        /// <param name="projectCD">Project Code</param>
        /// <returns>Project Name</returns>
        [System.Web.Services.WebMethod]
        public static string GetProjectName(string projectCD)
        {
            var projectCd = projectCD;
            var projectCdShow = projectCD;
            try
            {
                using (DB db = new DB())
                {
                    ProjectService grpSer = new ProjectService(db);
                    M_Project model = grpSer.GetByProjectCd(projectCd);
                    if (model != null)
                    {
                        var result = new
                        {
                            projectCD = projectCdShow,
                            projectNm = model.ProjectName
                        };
                        return OMS.Utilities.EditDataUtil.JsonSerializer<object>(result);
                    }
                    var onlyCD = new
                    {
                        projectCD = projectCdShow,
                        projectNm = string.Empty
                    };
                    return OMS.Utilities.EditDataUtil.JsonSerializer<object>(onlyCD);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Get detail list from screen
        /// </summary>
        /// <returns></returns>
        private List<T_Expence_D> GetDetailList(bool isProcess)
        {
            List<T_Expence_D> results = new List<T_Expence_D>();

            foreach (RepeaterItem item in this.rptDetail.Items)
            {
                if (isProcess && this.IsEmptyRow(item))
                {
                    continue;
                }
                T_Expence_D addItem = new T_Expence_D();
                HtmlInputCheckBox chkDelFlg = (HtmlInputCheckBox)item.FindControl("deleteFlag");
                IDateTextBox dtDate = (IDateTextBox)item.FindControl("dtDate");
                ITextBox txtPaidTo = (ITextBox)item.FindControl("txtPaidTo");
                if (listValue4ExpenceType[Int32.Parse(cmbExpenseType.SelectedValue) - 1].Value4.Equals("1"))
                {
                    ITextBox txtRouteFrom = (ITextBox)item.FindControl("txtRouteFrom");
                    ITextBox txtRouteTo = (ITextBox)item.FindControl("txtRouteTo");
                    string cmbRouteType = ((DropDownList)item.FindControl("cmbRouteType")).SelectedValue;

                    //RouteFrom
                    addItem.RouteFrom = txtRouteFrom.Text;

                    //RouteTo
                    addItem.RouteTo = txtRouteTo.Text;

                    //StartTime
                    addItem.RouteType = Int32.Parse(cmbRouteType);
                }
                else
                {
                    ITextBox txtNote = (ITextBox)item.FindControl("txtNote");

                    //Note
                    addItem.Note = txtNote.Text;
                }
                string cmbTaxType = ((DropDownList)item.FindControl("cmbTaxType")).SelectedValue;
                string cmbTaxRate = ((DropDownList)item.FindControl("cmbTaxRate")).SelectedValue;
                INumberTextBox txtAmount = (INumberTextBox)item.FindControl("txtAmount");
                INumberTextBox txtTaxAmount = (INumberTextBox)item.FindControl("txtTaxAmount");

                //Delete flag
                if (chkDelFlg != null)
                {
                    addItem.DelFlag = (chkDelFlg.Checked) ? true : false;
                }

                // for up/dow
                if (chkDelFlg != null)
                {
                    addItem.Checked = (chkDelFlg.Checked) ? true : false;
                }

                //Date
                addItem.Date = dtDate.Value;

                //PaidTo
                addItem.PaidTo = txtPaidTo.Text;

                //TaxType
                addItem.TaxType = Int32.Parse(cmbTaxType);

                //TaxRate
                addItem.TaxRate = decimal.Parse(cmbTaxRate);

                //Amount
                addItem.Amount = txtAmount.Value;

                //TaxAmount
                addItem.TaxAmount = txtTaxAmount.Value;

                results.Add(addItem);
            }

            //Reset index for row
            this._indexSell = 1;
            for (int i = 0; i < results.Count; i++)
            {
                var row = results[i];
                row.LineNo = this._indexSell++;
            }
            return results;
        }


        /// <summary>
        /// Check empty row
        /// </summary>
        /// <returns></returns>
        private bool IsEmptyRow(RepeaterItem item)
        {
            bool ret = true;

            //Date
            IDateTextBox dtDate = (IDateTextBox)item.FindControl("dtDate");
            if (dtDate != null)
            {
                if (dtDate.Value != null)
                {
                    ret = false;
                }
            }

            //PaidTo
            ITextBox txtPaidTo = (ITextBox)item.FindControl("txtPaidTo");
            if (txtPaidTo != null)
            {
                if (!string.IsNullOrEmpty(txtPaidTo.Value))
                {
                    ret = false;
                }
            }
            if (listValue4ExpenceType[Int32.Parse(cmbExpenseType.SelectedValue) - 1].Value4.Equals("1"))
            {
                //RouteFrom
                ITextBox txtRouteFrom = (ITextBox)item.FindControl("txtRouteFrom");
                if (txtRouteFrom != null)
                {
                    if (!string.IsNullOrEmpty(txtRouteFrom.Value))
                    {
                        ret = false;
                    }
                }

                //RouteTo
                ITextBox txtRouteTo = (ITextBox)item.FindControl("txtRouteTo");
                if (txtRouteTo != null)
                {
                    if (!string.IsNullOrEmpty(txtRouteTo.Value))
                    {
                        ret = false;
                    }
                }
            }
            else
            {
                //Note
                ITextBox txtNote = (ITextBox)item.FindControl("txtNote");
                if (txtNote != null)
                {
                    if (!string.IsNullOrEmpty(txtNote.Value))
                    {
                        ret = false;
                    }
                }
            }

            //Amount
            INumberTextBox txtAmount = (INumberTextBox)item.FindControl("txtAmount");
            if (txtAmount != null)
            {
                if (txtAmount.Value != null)
                {
                    ret = false;
                }
            }

            //TaxAmount
            INumberTextBox txtTaxAmount = (INumberTextBox)item.FindControl("txtTaxAmount");
            if (txtTaxAmount != null)
            {
                if (txtTaxAmount.Value != null)
                {
                    ret = false;
                }
            }

            return ret;
        }

        /// <summary>
        /// Get ID Project By CD
        /// </summary>
        private int GetIDProjectByCD(string ProjectCD)
        {
            using (DB db = new DB())
            {
                ProjectService prService = new ProjectService(db);
                return prService.GetByProjectCd(ProjectCD).ID;
            }
        }

        /// <summary>
        /// Get ID User By CD
        /// </summary>
        private int GetIDUserByCD(string UserCD)
        {
            using (DB db = new DB())
            {
                UserService prService = new UserService(db);
                return prService.GetByUserCD(UserCD).ID;
            }
        }

        /// <summary>
        /// Get header
        /// </summary>
        /// <returns></returns>
        private T_Expence_H GetHeader(T_Expence_H header = null)
        {
            if (header == null)
            {
                header = new T_Expence_H();
            }

            header.ExpenceNo = EditDataUtil.ToFixCodeDB(this.txtExpenceNo.Value, T_Expence_H.EXPENCE_CODE_DB_MAX_LENGTH);
            header.Date = DateTime.Parse(this.dtAccountingDate.Value.ToString());
            header.AccountCD = this.cmbExpenseType.SelectedValue;
            header.UserID = Int32.Parse(cmbUser.SelectedValue);
            header.DepartmentID = Int32.Parse(ddlDepartment.SelectedValue);
            header.ProjectID = GetIDProjectByCD(this.txtProjectCode.Value);
            header.ExpenceAmount = this.txtExpenceAmount.Value.GetValueOrDefault(0);
            header.Memo = this.txtMemo.Value;

            if (this.Mode == Mode.Approve)
            {
                header.ApprovedFlag = 1;
            }

            if (this.Mode == Mode.NotApprove)
            {
                header.ApprovedFlag = 0;
            }

            if (this.Mode == Mode.Insert)
            {
                header.CreateUID = this.LoginInfo.User.ID;
                header.UpdateUID = this.LoginInfo.User.ID;
            }

            if (this.Mode == Mode.Update || this.Mode == Mode.Approve || this.Mode == Mode.NotApprove)
            {
                header.UpdateDate = OldUpdateDate;
                header.UpdateUID = this.LoginInfo.User.ID;
            }

            return header;
        }

        #endregion

        /// <summary>
        /// GetWorkingTimeProjectValue
        /// </summary>
        /// <param name="workingSystemId"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string GetTaxAmountValue(string amountValue, int cmbTaxRateValue, int cmbTaxTypeValue)
        {
            System.Text.StringBuilder result = new System.Text.StringBuilder();
            try
            {
                string value = "";
                string[] arrAmount = amountValue.Split(',');
                double total = 0;
                foreach (var item in arrAmount)
                {
                    value = value + item;
                }

                using (DB db = new DB())
                {
                    Config_DService dbSer = new Config_DService(db);
                    //Get list detail
                    IList<M_Config_D> listM_Config_D = dbSer.GetListByConfigCd(M_Config_H.CONFIG_CD_TAX_TYPE);
                    var listValue4TaxType = listM_Config_D.OrderBy(f => f.Value1).ToList();


                    double taxRate = ((double)cmbTaxRateValue / 100);
                    double temp = 0;
                    if (cmbTaxTypeValue == 2)
                    {
                        temp = ((double)Int32.Parse(value) / (1 + taxRate) * taxRate);
                    }
                    else
                    {
                        temp = ((double)Int32.Parse(value) * taxRate);
                    }
                    if (listValue4TaxType[cmbTaxTypeValue - 1].Value4.Equals("1"))
                    {
                        total = Math.Round(temp);
                    }
                    else if (listValue4TaxType[cmbTaxTypeValue - 1].Value4.Equals("2"))
                    {
                        total = Math.Round((double)(temp));
                    }
                    else
                    {
                        total = temp;
                    }
                    //var a = string.Format("{0:F}", total);

                    result.Append("{");

                    result.Append(string.Format("\"taxAmountValue\":\"{0:#,##0}\"", total));

                    result.Append("}");



                    return result.ToString();
                }
            }

            catch (Exception ex)
            {
                Log.Instance.WriteLog(ex);
                return null;
            }
        }

        protected void cmbExpenseType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ResetProjectName();
            //Set mode
            this.ProcessMode(this.Mode);

            //Init detail list
            this.InitDetailList();

            this.txtExpenceAmount.Value = 0;

        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        private XmlNode getNodeXml(ModeSendMail mode)
        {
            XmlDocument xd = new XmlDocument();
            xd.LoadXml(Resources.Email);

            XmlNodeList nodelist = xd.SelectNodes("/sendmail/email");

            foreach (XmlNode node in nodelist)
            {
                string type = node.SelectSingleNode("type").InnerText;
                if (mode == ModeSendMail.Apply)
                {
                    if (type == "expence_apply")
                    {
                        return node;
                    }
                }

                if (mode == ModeSendMail.Approve)
                {
                    if (type == "expence_approve")
                    {
                        return node;
                    }
                }

                if (mode == ModeSendMail.Reject)
                {
                    if (type == "expence_reject")
                    {
                        return node;
                    }
                }

            }

            return null;
        }
    }
}