using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using OMS.Utilities;
using System.Collections;
using OMS.DAC;
using OMS.Models;
using System.IO;
using MimeKit;
using MailKit.Net.Smtp;
using System.Xml;
using MailKit;
using MailKit.Security;
using OMS.Properties;

namespace OMS.Approval
{
    public partial class FrmApprovalList : FrmBaseList
    {
        #region Property

        /// <summary>
        /// Get or set UserList
        /// </summary>
        public IList<DropDownModel> UserList
        {
            get
            {
                return (IList<DropDownModel>)ViewState["UserList"];
            }
            set
            {
                ViewState["UserList"] = value;
            }
        }

        /// <summary>
        /// Get or set Collapse
        /// </summary>
        public Hashtable pageMenu
        {
            get { return (Hashtable)ViewState["pageMenu"]; }
            set { ViewState["pageMenu"] = value; }
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
        /// PORT_SMTP
        /// </summary>
        private int PORT_SMTP
        {
            get { return GetConfig.getIntValue("PORT_SMTP"); }
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
        /// On Init
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //Set Title
            base.FormTitle = "申請承認一覧";
            base.FormSubTitle = "List";

            this.lblMessageSelect.Text = this.Messages[M_Message.MSG_SELECT_ROW_DATA].Message3;

            //Init Event

            // header grid sort
            this.HeaderGrid.OnSortClick += Sort_Click;

            // paging footer
            this.PagingFooter.OnClick += PagingFooter_Click;

            //define event button search
            this.btnSearch.ServerClick += new EventHandler(btnSearch_Click);
            this.btnClear.ServerClick += new EventHandler(btnClear_Click);
            this.btnAprrovalProcess.ServerClick += new EventHandler(btnAprrovalProcess_Click);
            
            // paging header
            this.PagingHeader.OnClick += PagingHeader_Click;
            this.PagingHeader.OnPagingClick += PagingFooter_Click;
            this.PagingHeader.NumRowOnPage = base.NumRowOnPageDefault;
            this.PagingHeader.CurrentPage = base.CurrentPageDefault;
            this.PagingHeader.IsShowColor = true;
        }

        /// <summary>
        /// Click Sort
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Sort_Click(object sender, EventArgs e)
        {
            this.LoadDataGrid(1, this.PagingHeader.NumRowOnPage);
        }

        /// <summary>
        /// Click PagingFooter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PagingFooter_Click(object sender, EventArgs e)
        {
            if (sender != null)
            {
                int curPage = int.Parse((sender as LinkButton).CommandArgument);
                this.PagingFooter.CurrentPage = curPage;
                this.PagingHeader.CurrentPage = curPage;
                this.LoadDataGrid(curPage, this.PagingHeader.NumRowOnPage);
            }
        }

        /// <summary>
        /// Click PagingHeader
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PagingHeader_Click(object sender, EventArgs e)
        {
            if (sender != null)
            {
                this.LoadDataGrid(1, this.PagingHeader.NumRowOnPage);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Check authority of login user
            base.SetAuthority(FormId.Approval);

            if (!this.IsPostBack)
            {
                InitData();

                if (!this._authority.IsApproval && !this._authority.IsApprovalALL )
                {
                    ProcessMode(Mode.View);
                }

                //Show condition
                if (this.PreviousPage != null)
                {
                    if (this.PreviousPageViewState["Condition"] != null)
                    {
                        Hashtable data = (Hashtable)PreviousPageViewState["Condition"];

                        this.ShowCondition(data);

                        //Save condition
                        this.SaveCondition();
                    }
                }

                //Show data on grid
                this.LoadDataGrid(this.PagingHeader.CurrentPage, this.PagingHeader.NumRowOnPage);
            }
        }

        /// <summary>
        /// Init Data
        /// </summary>
        private void InitData()
        {
            InitCombobox();
            InitDateTextBox();

            // header grid
            this.HeaderGrid.SortDirec = "1";
            this.HeaderGrid.SortField = "4";
        }

        /// <summary>
        /// ddlDepartment_SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Load user combobox data
            this.LoadUserComboboxData();
            this.SetPaging();
        }

        /// <summary>
        /// Event btnDetail_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDetail_Click(object sender, CommandEventArgs e)
        {
            int index = e.CommandArgument.ToString().IndexOf(" ");
            this.ViewState["ID"] = e.CommandArgument.ToString();
        }

        /// <summary>
        /// Event btnView_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnView_Click(object sender, CommandEventArgs e)
        {
            int index = e.CommandArgument.ToString().IndexOf(" ");
            this.ViewState["ID"] = e.CommandArgument.ToString();
        }
        
        /// <summary>
        /// Search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.LoadDataGrid(1, this.PagingHeader.NumRowOnPage);
            //Save condition
            this.SaveCondition();
        }

        /// <summary>
        /// Clear
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClear_Click(object sender, EventArgs e)
        {
            InitCombobox();
            InitDateTextBox();

            this.SetPaging();
        }

        protected void btnAprrovalProcess_Click(object sender, EventArgs e)
        {
            var datas = this.hidApprovedIDs.Value.Split(',');
            List<T_Attendance> lstApproved = new List<T_Attendance>();
            try
            {
                using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                {
                    AttendanceService dbSer = new AttendanceService(db);
                    foreach (var data in datas)
                    {
                        var arrData = data.Split(':');
                        int id = int.Parse(arrData[0]);
                        DateTime updateDate = new DateTime(long.Parse(arrData[1]));
                        int ret = 0;
                        T_Attendance header = dbSer.GetDataAttendanceById(id);
                        if (header != null)
                        {
                            if (this.hidApprovedMode.Value == "Approval")
                            {
                                header.ApprovalStatus = (int)AttendanceApprovalStatus.Approved;
                            }
                            else
                            {
                                header.ApprovalStatus = (int)AttendanceApprovalStatus.Cancel;
                            }
                            header.ApprovalNote = this.txtApprovalNote.Value;
                            header.ApprovalUID = this.LoginInfo.User.ID;
                            header.UpdateDate = updateDate;

                            //Update header
                            AttendanceService headerService = new AttendanceService(db);
                            ret = headerService.UpdateApproval(header);
                        }

                        //Check result update
                        if (ret == 0)
                        {
                            //data had changed
                            this.SetMessage(string.Empty, M_Message.MSG_DATA_CHANGED);
                            this.SetPaging();
                            return;
                        }
                        else
                        {
                            lstApproved.Add(header);
                        }
                    }

                    db.Commit();
                }
            }   
            catch (Exception ex)
            {
                this.SetMessage(string.Empty, M_Message.MSG_UPDATE_FAILE, this.hidApprovedMode.Value == "Approval"　?　"承認"　:　"差戻");
                Log.Instance.WriteLog(ex);
                this.SetPaging();
                return;
            }

            foreach (var item in lstApproved)
            {
                this.SendEmail((AttendanceApprovalStatus)item.ApprovalStatus, item);
            }
            this.LoadDataGrid(1, this.PagingHeader.NumRowOnPage);
        }

        /// <summary>
        /// InitCombobox
        /// </summary>
        private void InitCombobox()
        {
            //Load Department combobox data
            this.LoadDepartmentComboboxData();

            //Load user combobox data
            this.LoadUserComboboxData();

            //Load Approval Type combobox data
            this.GetCombobox_ApprovalType();
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
        /// Init Combobox
        /// </summary>
        private void GetCombobox_ApprovalType()
        {
            // init combox 
            IList<DropDownModel> ApprovalTypeList = new List<DropDownModel>();
            DropDownModel emptOption = new DropDownModel();
            emptOption.DisplayName = "----------";
            emptOption.Value = "";
            
            ApprovalTypeList = this.GetDataForDropdownList(M_Config_H.CONFIG_APPROVAL_TYPE);
            ApprovalTypeList.Insert(0, emptOption);
            ApprovalTypeList.RemoveAt((int)AttendanceApprovalStatus.NeedApproval);
            ddlType.DataSource = ApprovalTypeList;
            ddlType.DataValueField = "Value";
            ddlType.DataTextField = "DisplayName";
            ddlType.SelectedValue = this.GetDefaultValueForDropdownList(M_Config_H.CONFIG_APPROVAL_TYPE);
            ddlType.DataBind();
        }

        /// <summary>
        /// Load Department Combobox data
        /// </summary>
        private void LoadDepartmentComboboxData()
        {
            using (DB db = new DB())
            {
                IList<DropDownModel> departmentList;
                DepartmentService departmentService = new DepartmentService(db);
                departmentList = departmentService.GetDepartmentCbbData();

                DropDownModel emptOption = new DropDownModel();
                emptOption.DisplayName = "----------";
                emptOption.Value = "";
                departmentList.Insert(0, emptOption);

                ddlDepartment.Items.Clear();

                if (departmentList.Count > 0)
                {
                    ddlDepartment.DataSource = departmentList;
                    ddlDepartment.DataValueField = "Value";
                    ddlDepartment.DataTextField = "DisplayName";
                    ddlDepartment.DataBind();
                }
                else
                {
                    //msg Please input Department
                    ddlDepartment.DataSource = null;
                    ddlDepartment.DataBind();
                }
            }
        }

        /// <summary>
        /// Load user combobox data
        /// </summary>
        private void LoadUserComboboxData(string year = "")
        {
            using (DB db = new DB())
            {
                IList<DropDownModel> userList = new List<DropDownModel>();
                UserService userService = new UserService(db);
                
                DropDownModel emptOption = new DropDownModel();
                emptOption.DisplayName = "----------";
                emptOption.Value = "";


                if (ddlDepartment.SelectedValue == "")
                {
                    userList = userService.GetCbbUserDataByDepartmentID(-1, year: year);
                }
                else
                {
                    userList = userService.GetCbbUserDataByDepartmentID(int.Parse(ddlDepartment.SelectedValue), year: year);
                }
                

                userList.Insert(0, emptOption);
                ddlUser.Items.Clear();

                if (userList.Count > 0)
                {
                    ddlUser.DataSource = userList;
                    ddlUser.DataValueField = "Value";
                    ddlUser.DataTextField = "DisplayName";
                    this.UserList = userList;
                }
                else
                {
                    ddlUser.DataSource = null;
                    this.UserList =null;
                }


                ddlUser.DataBind();
                LoadUserComboboxAttribute();
            }
        }

        /// <summary>
        /// Load user combobox attribute
        /// </summary>
        private void LoadUserComboboxAttribute()
        {
            if (this.UserList != null)
            {
                int index = 0;
                foreach (ListItem item in ddlUser.Items)
                {
                    if (item.Value != "")
                    {
                        if (this.UserList[index].Status != "0")
                        {
                            item.Attributes.Add("style", "background-color: #f2dede; !important");
                        }
                        else
                        {
                            item.Attributes.Add("style", "background-color: #ffffff; !important");
                        }
                    }
                    else
                    {
                        item.Attributes.Add("style", "background-color: #ffffff; !important");
                    }
                    index++;
                }

                if (this.UserList[this.ddlUser.SelectedIndex].Value != "" && this.UserList[this.ddlUser.SelectedIndex].Status != "0")
                {
                    ddlUser.CssClass = "form-control input-sm bg-danger";
                }
                else
                {
                    ddlUser.CssClass = "form-control input-sm";
                }
                
            }
        }

        /// <summary>
        /// Load Date Text data
        /// </summary>
        private void InitDateTextBox()
        {
            this.dtStartDate.Value = null;
            this.dtEndDate.Value = null;

        }

        /// <summary>
        /// load data grid
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="numOnPage"></param>
        private void LoadDataGrid(int pageIndex, int numOnPage)
        {
            this.hidApprovedIDs.Value = string.Empty;
            this.hidApprovedMode.Value = string.Empty;
            int totalRow = 0;
            Hashtable pageMenu = new Hashtable();
            IList<ApprovalInfo> listInfo;

            //Get data
            using (DB db = new DB())
            {
                ApprovalService dbSer = new ApprovalService(db);
                Config_HService dbCFSer = new Config_HService(db);

                if (!base._authority.IsApprovalALL)
                {
                    ddlDepartment.SelectedValue = this.LoginInfo.User.DepartmentID.ToString();
                }

                if (!base._authority.IsApproval && !base._authority.IsApprovalALL)
                {
                    ddlUser.SelectedValue = this.LoginInfo.User.ID.ToString();

                }


                //Get Config header
                var isUseTimeApproval = dbCFSer.GetDefaultValueDrop(M_Config_H.CONFIG_USE_TIME_APPROVAL) != "0";

                totalRow = dbSer.getTotalRow(this.ddlDepartment.SelectedValue, this.ddlUser.SelectedValue, this.ddlType.SelectedValue, this.dtStartDate.Value, this.dtEndDate.Value);

                listInfo = dbSer.GetListByCond(this.ddlDepartment.SelectedValue, this.ddlUser.SelectedValue, this.ddlType.SelectedValue, this.dtStartDate.Value, this.dtEndDate.Value,
                                                pageIndex, numOnPage, int.Parse(this.HeaderGrid.SortField), int.Parse(this.HeaderGrid.SortDirec), isUseTimeApproval);
            }

            //Show data
            if (listInfo.Count == 0)
            {
                this.rptList.DataSource = null;
                pageMenu.Add("CurrentPage", pageIndex);
                pageMenu.Add("NumberOnPage", numOnPage);
                pageMenu.Add("RowNumFrom", 0);
                pageMenu.Add("RowNumTo", 0);
            }
            else
            {
                // paging header
                this.PagingHeader.RowNumFrom = int.Parse(listInfo[0].RowNumber.ToString());
                this.PagingHeader.RowNumTo = int.Parse(listInfo[listInfo.Count - 1].RowNumber.ToString());
                this.PagingHeader.TotalRow = totalRow;
                this.PagingHeader.CurrentPage = pageIndex;

                // paging footer
                this.PagingFooter.CurrentPage = pageIndex;
                this.PagingFooter.NumberOnPage = numOnPage;
                this.PagingFooter.TotalRow = totalRow;

                // header
                this.HeaderGrid.TotalRow = totalRow;
                this.HeaderGrid.AddColumms(new string[] { "#", "", "", "状態", "社員名", "日付", "申請内容" });
                this.HeaderGrid.Columns[6].Sorting = false;

                // detail
                this.rptList.DataSource = listInfo;

                pageMenu.Add("RowNumFrom", this.PagingHeader.RowNumFrom);
                pageMenu.Add("RowNumTo", this.PagingHeader.RowNumTo);
                pageMenu.Add("CurrentPage", this.PagingHeader.CurrentPage);
                pageMenu.Add("NumberOnPage", this.PagingFooter.NumberOnPage);
            }
            pageMenu.Add("TotalRow", totalRow);
            this.pageMenu = pageMenu;

            LoadUserComboboxAttribute();

            this.rptList.DataBind();
        }

        /// <summary>
        /// Save condition search
        /// </summary>
        private void SaveCondition()
        {
            Hashtable hash = new Hashtable();
            hash.Add(this.ddlDepartment.ID, this.ddlDepartment.SelectedValue);
            hash.Add(this.ddlUser.ID, this.ddlUser.SelectedValue);
            hash.Add(this.ddlType.ID, this.ddlType.SelectedValue);
            hash.Add(this.dtEndDate.ID, this.dtEndDate.Value);
            hash.Add(this.dtStartDate.ID, this.dtStartDate.Value);
            hash.Add("NumRowOnPage", this.PagingHeader.NumRowOnPage);
            hash.Add("CurrentPage", this.PagingHeader.CurrentPage);
            hash.Add("SortField", this.HeaderGrid.SortField);
            hash.Add("SortDirec", this.HeaderGrid.SortDirec);
            this.ViewState["Condition"] = hash;
        }

        /// <summary>
        /// Show Condition
        /// </summary>
        private void ShowCondition(Hashtable data)
        {
            this.ddlDepartment.SelectedValue = data[this.ddlDepartment.ID].ToString();
            this.ddlType.SelectedValue = data[this.ddlType.ID].ToString();
            this.ddlUser.SelectedValue = data[this.ddlUser.ID].ToString();
            this.dtEndDate.Text = data[this.dtEndDate.ID] == null ? string.Empty : data[this.dtEndDate.ID].ToString();
            this.dtStartDate.Text = data[this.dtStartDate.ID] == null ? string.Empty : data[this.dtStartDate.ID].ToString();
            if (this.UserList[this.ddlUser.SelectedIndex].Value != "" && this.UserList[this.ddlUser.SelectedIndex].Status != "0")
            {
                ddlUser.CssClass = "form-control input-sm bg-danger";
            }
            else
            {
                ddlUser.CssClass = "form-control input-sm";
            }

            int curPage = int.Parse(data["CurrentPage"].ToString());
            this.PagingHeader.CurrentPage = curPage;
            this.PagingFooter.CurrentPage = curPage;

            this.HeaderGrid.SortField = data["SortField"].ToString();
            this.HeaderGrid.SortDirec = data["SortDirec"].ToString();
            int rowOfPage = int.Parse(data["NumRowOnPage"].ToString());
            this.PagingHeader.NumRowOnPage = rowOfPage;
        }

        protected void SetPaging()
        {
            // paging header
            this.PagingHeader.RowNumFrom = (int)pageMenu["RowNumFrom"];
            this.PagingHeader.RowNumTo = (int)pageMenu["RowNumTo"];
            this.PagingHeader.CurrentPage = (int)pageMenu["CurrentPage"];
            this.PagingHeader.TotalRow = (int)pageMenu["TotalRow"];

            // paging footer
            this.PagingFooter.CurrentPage = (int)pageMenu["CurrentPage"];
            this.PagingFooter.NumberOnPage = (int)pageMenu["NumberOnPage"];
            this.PagingFooter.TotalRow = (int)pageMenu["TotalRow"];

            // header
            this.HeaderGrid.TotalRow = this.PagingHeader.TotalRow;
            this.HeaderGrid.AddColumms(new string[] { "#", "", "", "状態", "社員名", "日付", "申請内容" });
            this.HeaderGrid.Columns[5].Sorting = false;
        }

        /// <summary>
        /// Process mode
        /// </summary>
        /// <param name="mode">Mode</param>
        private void ProcessMode(Mode mode)
        {
            //Check model
            switch (mode)
            {
                case Mode.View:
                    ddlDepartment.SelectedValue = this.LoginInfo.User.DepartmentID.ToString();
                    ddlUser.SelectedValue = this.LoginInfo.User.ID.ToString();
                    this.ddlDepartment.Enabled = base._authority.IsApprovalALL;
                    this.ddlUser.Enabled = base._authority.IsApproval || base._authority.IsApprovalALL;
                    break;
            }
        }
        #endregion

        #region mail 
        /// <summary>
        /// Send Email
        /// </summary>
        /// <returns></returns>
        private bool SendEmail(AttendanceApprovalStatus approvalStatus, T_Attendance tAttendance)
        {
            try
            {
                var client = new SmtpClient();
                var body = new BodyBuilder();
                var message = this.CreateMessage();
                var result = true;
                var isUseTimeApproval = this.GetIsUseApply(M_Config_H.CONFIG_USE_TIME_APPROVAL);
                string mailType = string.Empty;
                string subject;
                string content;

                switch (approvalStatus)
                {
                    case AttendanceApprovalStatus.Approved:
                        mailType = "attendance_approved";
                        break;
                    case AttendanceApprovalStatus.Cancel:
                        mailType = "attendance_cancel";
                        break;
                }

                var approvalMailInfo = this.GetApprovalMailInfo(tAttendance.ID, isUseTimeApproval);
                XmlNode node_email = this.getNodeXml(mailType);
                subject = node_email.SelectSingleNode("subject").InnerText.Trim();
                content = node_email.SelectSingleNode("content").InnerText.Trim();
                string mailBody = string.Empty;

                switch (approvalStatus)
                {
                    case AttendanceApprovalStatus.Approved:
                        mailBody = string.Format(content
                                                , this.GetMailApplyContent(approvalMailInfo)
                                                , this.GetMailApprovalContent("承認者", approvalMailInfo));
                        break;
                    case AttendanceApprovalStatus.Cancel:
                        mailBody = string.Format(content
                                                , this.GetMailApplyContent(approvalMailInfo)
                                                , this.GetMailApprovalContent("差戻者", approvalMailInfo));
                        break;
                }

                message.Subject = string.Format(subject, approvalMailInfo.MailSubject);
                body.TextBody = mailBody;
                message.Body = body.ToMessageBody();

                if (ConnectedServerSmtp(client))
                {
                    List<M_User> lstMail = new List<M_User>();
                    using (DB db = new DB())
                    {
                        UserService userService = new UserService(db);
                        
                        if (tAttendance.ApprovalUID != tAttendance.UID)
                        {
                            lstMail.Add(userService.GetByID(tAttendance.UID));
                        }
                        if (approvalStatus == AttendanceApprovalStatus.Approved)
                        {
                            lstMail.AddRange(userService.GetListConfirmMails(tAttendance.UID).ToList());
                        }
                    }

                    foreach (var item in lstMail)
                    {
                        if (item.MailAddress.Length <= 0)
                        {
                            continue;
                        }

                        message.To.Clear();
                        message.To.Add(new MailboxAddress(item.UserName1, item.MailAddress));
                        if (!SendMessage(client, message))
                        {
                            base.SetMessage(string.Empty, M_Message.MSG_USER_SEND_ERMAIL_ERROR, item.UserName1);
                            result = false;
                        }
                    }
                    this.CloseConnectSmtp(client);
                    return result;
                }
                else
                {
                    this.SetMessage(string.Empty, M_Message.MSG_CONNECT_ERMAIL_ERROR, "");
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
        /// Get Header Data
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        private bool GetIsUseApply(string configCD)
        {
            using (DB db = new DB())
            {
                Config_HService dbSer = new Config_HService(db);

                //Get Config header
                return dbSer.GetDefaultValueDrop(configCD) != "0";
            }
        }

        private ApprovalMailInfo GetApprovalMailInfo(int AttendanceId, bool isUseTimeApproval)
        {
            using (DB db = new DB())
            {
                ApprovalService userService = new ApprovalService(db);
                return userService.GetApprovalMailInfo(AttendanceId, isUseTimeApproval);
            }
        }

        /// <summary>
        /// メールの内容
        /// </summary>
        /// <returns>内容</returns>
        private string GetMailApplyContent(ApprovalMailInfo approvalMailInfo)
        {
            List<string> lstApplyContent = new List<string>();

            lstApplyContent.Add(string.Format("部門：{0}", approvalMailInfo.DepartmentName));
            lstApplyContent.Add(string.Format("社員名：{0}", approvalMailInfo.UserName));
            lstApplyContent.Add(string.Format("対象日：{0}", approvalMailInfo.Date + approvalMailInfo.ExchangeDate));
            lstApplyContent.Add(string.Format("申請内容：{0}", approvalMailInfo.ApplyContent));
            lstApplyContent.Add(string.Format("事由：{0}", approvalMailInfo.RequestNote));
            if (!string.IsNullOrEmpty(approvalMailInfo.EntryTime))
            {
                lstApplyContent.Add(string.Format("出勤：{0}", approvalMailInfo.EntryTime));
            }
            if (!string.IsNullOrEmpty(approvalMailInfo.ExitTime))
            {
                lstApplyContent.Add(string.Format("退勤：{0}", approvalMailInfo.ExitTime));
            }
            return string.Join("\n", lstApplyContent);
        }

        /// <summary>
        /// メールの内容
        /// </summary>
        /// <returns>内容</returns>
        private string GetMailApprovalContent(string approvalUserLable, ApprovalMailInfo approvalMailInfo)
        {
            List<string> lstApprovalContent = new List<string>();

            lstApprovalContent.Add(string.Format("{0}：{1}", approvalUserLable, approvalMailInfo.ApprovalUserName));
            lstApprovalContent.Add(string.Format("事由：{0}", approvalMailInfo.ApprovalNote));

            return string.Join("\n", lstApprovalContent);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        private XmlNode getNodeXml(string mailType)
        {
            XmlDocument xd = new XmlDocument();
            xd.LoadXml(Resources.Email);

            XmlNodeList nodelist = xd.SelectNodes("/sendmail/email");

            foreach (XmlNode node in nodelist)
            {
                string type = node.SelectSingleNode("type").InnerText;

                if (type == mailType)
                {
                    return node;
                }
            }

            return null;
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
        #endregion
    }
}