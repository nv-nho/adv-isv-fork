using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using OMS.Models;
using OMS.DAC;
using System.Collections;
using OMS.Utilities;
using System.Data;
using OMS.Reports.EXCEL;
using NPOI.SS.UserModel;
using System.Web.UI.HtmlControls;
using System.Linq;
using NPOI.HSSF.UserModel;
using System.IO;
using System.Xml;
using OMS.Properties;
using MailKit;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Net.Imap;
using MailKit.Security;
using System.Threading.Tasks;

namespace OMS.Expence
{
    public partial class FrmExpenceList : FrmBaseList
    {
        #region Constants
        private const string CONST_DANGER_TEXT = "無効";
        private const string ATTENDANCELIST_DOWNLOAD = "経費一覧_{0}.xlsx";
        private const string ATTENDANCELIST_DOWNLOAD2 = "交通・宿泊費清算書_{0}.xlsx";
        private const string FMT_YMDHMM = "yyMMddHHmm";
        private const string FOLDER_SEND_STRING = "sent";
        #endregion

        #region Property

        public bool Success { get; set; }
        public static bool excel2 = false;

        /// <summary>
        /// Get or set ListExpenceTotalRow
        /// </summary>
        public int ListExpenceTotalRow
        {
            get { return (int)ViewState["ListExpenceTotalRow"]; }
            set { ViewState["ListExpenceTotalRow"] = value; }
        }

        /// <summary>
        /// Get or set ListExpenceInfoViewState
        /// </summary>
        public IList<ExpenceInfo> ListExpenceInfoViewState
        {
            get { return (IList<ExpenceInfo>)ViewState["ListExpenceInfoViewState"]; }
            set { ViewState["ListExpenceInfoViewState"] = value; }
        }

        /// <summary>
        /// Get or set tabIndex
        /// </summary>
        public short tabIndex
        {
            get { return (short)ViewState["tabIndex"]; }
            set { ViewState["tabIndex"] = value; }
        }

        /// <summary>
        /// Sort Field
        /// </summary>
        public string SortField
        {
            get { return string.Format("{0}", ViewState["hdSortField"]); }
            set { ViewState["hdSortField"] = value; }
        }

        /// <summary>
        /// Sort Direc
        /// </summary>
        public string SortDirec
        {
            get { return string.Format("{0}", ViewState["hdSortDirec"]); }
            set { ViewState["hdSortDirec"] = value; }
        }

        /// <summary>
        /// Get or set flagNew
        /// </summary>
        public bool flagNew
        {
            get { return (bool)ViewState["flagNew"]; }
            set { ViewState["flagNew"] = value; }
        }

        /// <summary>
        /// Get or set flagSubmit
        /// </summary>
        public bool flagSubmit
        {
            get { return (bool)ViewState["flagSubmit"]; }
            set { ViewState["flagSubmit"] = value; }
        }

        /// <summary>
        /// Get or set flagSubmitCancel
        /// </summary>
        public bool flagSubmitCancel
        {
            get { return (bool)ViewState["flagSubmitCancel"]; }
            set { ViewState["flagSubmitCancel"] = value; }
        }

        /// <summary>
        /// Get or ser IsShowQuestion
        /// </summary>
        public bool IsShowQuestion { get; set; }

        /// <summary>
        /// Get or ser DefaultButton
        /// </summary>
        public string DefaultButton { get; set; }

        /// <summary>
        /// Get or set ExpenceID
        /// </summary>
        public int ExpenceID
        {
            get { return (int)ViewState["ID"]; }
            set { ViewState["ID"] = value; }
        }

        /// <summary>
        /// Get or set UserList
        /// </summary>
        public IList<DropDownModel> UserList1
        {
            get
            {
                return (IList<DropDownModel>)ViewState["UserList1"];
            }
            set
            {
                ViewState["UserList1"] = value;
            }
        }

        /// <summary>
        /// Get or set UserList
        /// </summary>
        public IList<DropDownModel> UserList2
        {
            get
            {
                return (IList<DropDownModel>)ViewState["UserList2"];
            }
            set
            {
                ViewState["UserList2"] = value;
            }
        }

        // <summary>
        /// Get or set UserList
        /// </summary>
        public IList<DropDownModel> UserList3
        {
            get
            {
                return (IList<DropDownModel>)ViewState["UserList3"];
            }
            set
            {
                ViewState["UserList3"] = value;
            }
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
        /// On Init
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //Set Title
            base.FormTitle = "経費一覧";
            base.FormSubTitle = "List";

            //Click the Download Excel button
            this.btnDownload.ServerClick += new EventHandler(btnDownloadExcel_Click);

            this.BtnApply.ServerClick += new EventHandler(tabApply_click);
            this.BtnApproved.ServerClick += new EventHandler(tabApproved_Click);
            //paging footer
            this.PagingFooter.OnClick += Paging_Footer_click;

            //paging header
            this.PagingHeader.OnClick += Paging_Header_click;
            this.PagingHeader.OnPagingClick += Paging_Footer_click;
            this.PagingHeader.NumRowOnPage = base.NumRowOnPageDefault;
            this.PagingHeader.CurrentPage = base.CurrentPageDefault;
            this.PagingHeader.IsShowColor = true;

            //Lock control
            this.txtProjectName.ReadOnly = true;

            //define event button search
            this.btnSearch.ServerClick += new EventHandler(btnSearch_Click);

            ////Init Max Length
            this.txtProjectCode.MaxLength = M_Project.PROJECT_CODE_SHOW_MAX_LENGTH;

            this.tabIndex = 0;

            this.SortField = "ExpenceNo";
            this.SortDirec = "2";

            //Init Event
            LinkButton btnYes = (LinkButton)this.Master.FindControl("btnYes");
            btnYes.Click += new EventHandler(btnProcessData);

            LinkButton btnNo = (LinkButton)this.Master.FindControl("btnNo");
            btnNo.Click += new EventHandler(btCancelSubmit);
        }

        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Check authority
            base.SetAuthority(FormId.ExpenceGroup);
            if (!this._authority.IsExpenceView)
            {
                Response.Redirect("~/Menu/FrmMainMenu.aspx");
            }
            if (!this._authority.IsExpenceExportExcel)
            {
                base.DisabledLink(this.btnExcel, true);
            }

            if (!this.IsPostBack)
            {
                //Init data
                this.InitData();

                //Show condition
                if (this.PreviousPage != null)
                {
                    if (this.PreviousPageViewState["Condition"] != null)
                    {
                        Hashtable data = (Hashtable)PreviousPageViewState["Condition"];
                        this.ShowCondition(data);
                    }
                }

                //Show data on grid
                this.LoadDataGrid(this.PagingHeader.CurrentPage, this.PagingHeader.NumRowOnPage);
            }

            LoadUserComboboxAttribute(cmbUser, UserList1);
            LoadUserComboboxAttribute(cmbUser2, UserList2);
            LoadUserComboboxAttribute(cmbUser3, UserList3);
        }

        /// <summary>
        /// Event changed page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNew_Click(object sender, CommandEventArgs e)
        {
            //Save condition
            this.SaveCondSearch();
        }

        /// <summary>
        /// Event changed page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDetail_Click(object sender, CommandEventArgs e)
        {
            ViewState["ID"] = e.CommandArgument;
            this.SaveCondSearch();
        }

        /// <summary>
        /// Paging Footer Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Paging_Footer_click(object sender, EventArgs e)
        {
            if (sender != null)
            {
                int curPage = int.Parse((sender as LinkButton).CommandArgument);
                this.PagingFooter.CurrentPage = curPage;
                this.PagingHeader.CurrentPage = curPage;
                LoadDataGrid(curPage, this.PagingHeader.NumRowOnPage);
            }
        }

        /// <summary>
        /// Paging Header Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Paging_Header_click(object sender, EventArgs e)
        {
            if (sender != null)
            {
                LoadDataGrid(1, this.PagingHeader.NumRowOnPage);
            }
        }

        /// <summary>
        /// Sort Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Sort_Click(object sender, EventArgs e)
        {
            string newSortField = (sender as LinkButton).CommandArgument;
            if (!this.SortField.Equals(newSortField))
            {
                this.SortDirec = "2";
                this.SortField = newSortField;
            }
            else
            {
                if (this.SortDirec.Equals("2"))
                {
                    this.SortDirec = "1";
                }
                else
                {
                    this.SortDirec = "2";
                }
            }
            this.LoadDataGrid(1, this.PagingHeader.NumRowOnPage);
        }

        /// <summary>
        /// Search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.SortDirec = "2";
            this.SortField = "ExpenceNo";

            this.ResetProjectName();

            // Refresh load grid
            this.LoadDataGrid(1, this.PagingHeader.NumRowOnPage);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAccept_Click(object sender, EventArgs e)
        {
            if (selectRow())
            {
                ShowQuestionMessage(M_Message.MSG_CONFIRM_SEND_MAIL_APPROVE, Models.DefaultButton.No, true);
                this.LoadDataForPagingHeader(this.PagingHeader.CurrentPage, this.PagingHeader.NumRowOnPage);
            }
            else
            {
                this.Success = false;
                this.SetMessage(string.Empty, M_Message.MSG_SELECT_ROW_DATA);
                this.LoadDataGrid(this.PagingHeader.CurrentPage, this.PagingHeader.NumRowOnPage);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tabApproved_Click(object sender, EventArgs e)
        {
            this.tabIndex = 1;
            this.btnAccept.Visible = false;
            this.LoadDataGrid(1, this.PagingHeader.NumRowOnPage);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tabApply_click(object sender, EventArgs e)
        {
            this.tabIndex = 0;
            btnAccept.Visible = true;
            this.LoadDataGrid(1, this.PagingHeader.NumRowOnPage);
        }

        /// <summary>
        /// Show message question
        /// </summary>
        /// <param name="messageID">MessageID</param>
        /// <param name="defaultButton">Default Button</param>
        protected void ShowQuestionMessage(string messageID, DefaultButton defaultButton, bool isAddNew = false, params string[] args)
        {
            if (isAddNew)
            {
                flagNew = true;
            }
            else
            {
                flagNew = false;
            }
            flagSubmitCancel = false;
            flagSubmit = false;

            //Get Message
            M_Message mess = (M_Message)this.Messages[messageID];
            HtmlGenericControl questionMessage = (HtmlGenericControl)this.Master.FindControl("questionMessage");
            questionMessage.InnerHtml = "<p>" + " " + string.Format(mess.Message3, args) + "</p>";

            this.IsShowQuestion = true;

            LinkButton btnNo = (LinkButton)this.Master.FindControl("btnNo");
            btnNo.Visible = isAddNew;
            if (defaultButton == Models.DefaultButton.Yes)
            {
                this.DefaultButton = "#btnYes";
            }
            else
            {
                if (isAddNew)
                {
                    this.DefaultButton = "#btnNo";
                }
            }
        }
        #endregion

        #region Method
        /// <summary>
        /// Init Data
        /// </summary>
        private void InitData()
        {
            //Invalid
            this.InitCombobox(this.cmbExpenseType);
            using (DB db = new DB())
            {
                var depaermentSV = new DepartmentService(db);
                var listDepartment = depaermentSV.GetDepartmentCbbData(-1);
                listDepartment.Insert(0, new DropDownModel("-1", "---"));
                this.cmbDepartment.DataSource = listDepartment;
                this.cmbDepartment.DataValueField = "Value";
                this.cmbDepartment.DataTextField = "DisplayName";
                this.cmbDepartment.DataBind();
                this.cmbDepartment.SelectedValue = "-1";

                var userSV = new UserService(db);

                // 支払先（社員）
                IList<DropDownModel> listUser = new List<DropDownModel>();
                if (this._authority.IsExpenceAllApproved)
                {
                    listUser = userSV.GetCbbUserDataByDepartmentID(-1);
                }
                else
                {
                    listUser = userSV.GetCbbUserExistExpense(this.LoginInfo.User.ID.ToString());
                    IEnumerable<DropDownModel> results = listUser.Where(s => s.Value == this.LoginInfo.User.ID.ToString());
                    if (results.Count() == 0)
                    {
                        listUser.Add(new DropDownModel(this.LoginInfo.User.ID.ToString(), this.LoginInfo.User.UserName1, this.LoginInfo.User.StatusFlag.ToString()));
                    }
                }
                listUser.Insert(0, new DropDownModel("-1", "---"));

                cmbUser.DataSource = listUser;
                cmbUser.DataValueField = "Value";
                cmbUser.DataTextField = "DisplayName";
                cmbUser.DataBind();
                cmbUser.SelectedValue = "-1";
                UserList1 = listUser;
                LoadUserComboboxAttribute(cmbUser, UserList1);


                // 承認者
                var listUser2 = userSV.GetCbbUserDataByDepartmentID(-1);
                listUser2.Insert(0, new DropDownModel("-1", "---"));
                cmbUser2.DataSource = listUser2;
                cmbUser2.DataValueField = "Value";
                cmbUser2.DataTextField = "DisplayName";
                cmbUser2.DataBind();
                cmbUser2.SelectedValue = "-1";
                cmbUser2.Enabled = this._authority.IsExpenceAllApproved;
                UserList2 = listUser2;
                LoadUserComboboxAttribute(cmbUser2, UserList2);

                // cmbUser3
                var listUser3 = userSV.GetCbbUserDataByDepartmentID(-1);
                listUser3.Insert(0, new DropDownModel("-1", "全社員"));
                cmbUser3.DataSource = listUser3;
                cmbUser3.DataValueField = "Value";
                cmbUser3.DataTextField = "DisplayName";
                cmbUser3.DataBind();
                if (this._authority.IsExpenceAllApproved)
                {
                    cmbUser3.SelectedValue = "-1";
                }
                else
                {
                    cmbUser3.SelectedValue = this.LoginInfo.User.ID.ToString();
                    cmbUser3.Enabled = false;
                }
                UserList3 = listUser3;
                LoadUserComboboxAttribute(cmbUser3, UserList3);
            }

            base.DisabledLink(this.btnNew, !base._authority.IsExpenceNew);
            base.DisabledLink(this.btnAccept, !base._authority.IsExpencemutilAccept);
        }

        /// <summary>
        /// Load user combobox attribute
        /// </summary>
        private void LoadUserComboboxAttribute(DropDownList ddlUser, IList<DropDownModel> userList)
        {
            if (userList != null)
            {
                int index = 0;
                foreach (ListItem item in ddlUser.Items)
                {
                    if (item.Value != "-1")
                    {
                        if (userList[index].Status != "0")
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
                if (userList[ddlUser.SelectedIndex].Value != "-1" && userList[ddlUser.SelectedIndex].Status != "0")
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
        ///
        /// </summary>
        /// <returns></returns>
        public bool selectRow()
        {
            bool chkFlag = false;
            foreach (RepeaterItem item in this.rptProjectList.Items)
            {
                HtmlInputCheckBox chkExpence = (HtmlInputCheckBox)item.FindControl("chkSelectlg");
                HiddenField hidUid = (HiddenField)item.FindControl("hiddenID");

                ExpenceInfo temp = ListExpenceInfoViewState.Where(expenceInfo => expenceInfo.ID == int.Parse(hidUid.Value)).SingleOrDefault();

                if (temp != null)
                {
                    temp.CheckFlag = chkExpence.Checked;
                    if (temp.CheckFlag)
                    {
                        chkFlag = true;
                    }
                }
            }

            return chkFlag;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btCancelSubmit(object sender, EventArgs e)
        {
            this.LoadDataForPagingHeader(this.PagingHeader.CurrentPage, this.PagingHeader.NumRowOnPage);
        }

        /// <summary>
        /// Process Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnProcessData(object sender, EventArgs e)
        {
            List<T_Expence_H> items = new List<T_Expence_H>();
            using (DB db = new DB(System.Data.IsolationLevel.Serializable))
            {
                foreach (RepeaterItem item in this.rptProjectList.Items)
                {
                    HtmlInputCheckBox chkAttendanceApprovaDel = (HtmlInputCheckBox)item.FindControl("chkSelectlg");
                    HiddenField hidUid = (HiddenField)item.FindControl("hiddenID");

                    ExpenceInfo temp = ListExpenceInfoViewState.Where(expenceInfo => expenceInfo.ID == int.Parse(hidUid.Value)).SingleOrDefault();

                    temp.CheckFlag = chkAttendanceApprovaDel.Checked;
                    int ret = 0;
                    if (temp.CheckFlag)
                    {
                        T_Expence_H header = this.GetExpence(temp.ID);
                        if (header != null)
                        {
                            items.Add(header);

                            header.ApprovedFlag = 1;
                            header.UpdateDate = temp.UpdateDate;
                            header.UpdateUID = this.LoginInfo.User.ID;
                            Expence_HService headerService = new Expence_HService(db);
                            ret = headerService.Update(header);
                            this.Success = true;
                        }

                        //Check result update
                        if (ret == 0)
                        {
                            items.Clear();
                            //data had changed
                            this.Success = false;
                            this.SetMessage(string.Empty, M_Message.MSG_DATA_CHANGED);
                            this.LoadDataForPagingHeader(this.PagingHeader.CurrentPage, this.PagingHeader.NumRowOnPage);
                            break;
                        }
                    }
                }

                if (Success)
                {
                    db.Commit();
                    this.LoadDataGrid(1, this.PagingHeader.NumRowOnPage);
                }
            }

            Task.Factory.StartNew(() => SendMail(items));
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
        /// Save condition search
        /// </summary>
        private void SaveCondSearch()
        {
            Hashtable hash = new Hashtable();
            hash.Add(this.cmbDepartment.ID, this.cmbDepartment.SelectedValue);
            hash.Add(this.txtProjectCode.ID, this.txtProjectCode.Value);
            hash.Add(this.txtProjectName.ID, this.txtProjectName.Value);
            hash.Add(this.cmbUser.ID, this.cmbUser.SelectedValue);
            hash.Add(this.cmbUser2.ID, this.cmbUser2.SelectedValue);
            hash.Add(this.cmbExpenseType.ID, this.cmbExpenseType.SelectedValue);
            hash.Add(this.dtAccountingDate1.ID, this.dtAccountingDate1.Value);
            hash.Add(this.dtAccountingDate2.ID, this.dtAccountingDate2.Value);
            hash.Add(this.dtTheDateOfUse1.ID, this.dtTheDateOfUse1.Value);
            hash.Add(this.dtTheDateOfUse2.ID, this.dtTheDateOfUse2.Value);
            hash.Add("tabIndex", this.tabIndex);

            hash.Add("NumRowOnPage", this.PagingHeader.NumRowOnPage);
            hash.Add("CurrentPage", this.PagingHeader.CurrentPage);

            this.ViewState["Condition"] = hash;
        }

        /// <summary>
        /// Show Condition
        /// </summary>
        private void ShowCondition(Hashtable data)
        {
            this.cmbDepartment.SelectedValue = data[this.cmbDepartment.ID].ToString();
            this.txtProjectCode.Value = data[this.txtProjectCode.ID].ToString();
            this.txtProjectName.Value = data[this.txtProjectName.ID].ToString();
            this.cmbUser.SelectedValue = data[this.cmbUser.ID].ToString();
            this.cmbUser2.SelectedValue = data[this.cmbUser2.ID].ToString();
            this.cmbExpenseType.SelectedValue = data[this.cmbExpenseType.ID].ToString();
            this.dtAccountingDate1.Value = (DateTime?)data[this.dtAccountingDate1.ID];
            this.dtAccountingDate2.Value = (DateTime?)data[this.dtAccountingDate2.ID];
            this.dtTheDateOfUse1.Value = (DateTime?)data[this.dtTheDateOfUse1.ID];
            this.dtTheDateOfUse2.Value = (DateTime?)data[this.dtTheDateOfUse2.ID];

            int curPage = int.Parse(data["CurrentPage"].ToString());
            this.PagingHeader.CurrentPage = curPage;
            this.PagingFooter.CurrentPage = curPage;

            int rowOfPage = int.Parse(data["NumRowOnPage"].ToString());
            this.PagingHeader.NumRowOnPage = rowOfPage;

            this.tabIndex = (short)data["tabIndex"];
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="RowNumberTo"></param>
        /// <param name="RowNumberFrom"></param>
        /// <param name="getForm"></param>
        private void LoadDataForPagingHeader(int pageIndex, int numOnPage)
        {
            if (this.ListExpenceInfoViewState != null)
            {
                // paging header
                this.PagingHeader.RowNumFrom = int.Parse(this.ListExpenceInfoViewState[0].RowNumber.ToString());
                this.PagingHeader.RowNumTo = int.Parse(this.ListExpenceInfoViewState[this.ListExpenceInfoViewState.Count - 1].RowNumber.ToString());
                this.PagingHeader.TotalRow = this.ListExpenceTotalRow;
                this.PagingHeader.CurrentPage = pageIndex;

                // paging footer
                this.PagingFooter.CurrentPage = pageIndex;
                this.PagingFooter.NumberOnPage = numOnPage;
                this.PagingFooter.TotalRow = this.ListExpenceTotalRow;
            }
            else
            {
                this.PagingHeader.TotalRow = 0;
                this.PagingHeader.CurrentPage = 0;
            }
        }

        /// <summary>
        /// load data gird
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="numOnPage"></param>
        private void LoadDataGrid(int pageIndex, int numOnPage)
        {
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }
            this.ListExpenceTotalRow = 0;
            try
            {
                using (DB db = new DB())
                {
                    Expence_HService prService = new Expence_HService(db);
                    this.ListExpenceTotalRow = prService.getTotalRow(this.cmbUser.SelectedValue, this.cmbUser2.SelectedValue,
                                                                 this.cmbDepartment.SelectedValue,
                                                                 this.txtProjectCode.Value, this.cmbExpenseType.SelectedValue, this.dtAccountingDate1.Value, this.dtAccountingDate2.Value,
                                                                 this.dtTheDateOfUse1.Value, this.dtTheDateOfUse2.Value, this.tabIndex, LoginInfo.User.ID, this._authority.IsExpenceAllApproved);

                    this.ListExpenceInfoViewState = prService.GetListByCond(this.cmbUser.SelectedValue, this.cmbUser2.SelectedValue,
                                                          this.cmbDepartment.SelectedValue,
                                                          this.txtProjectCode.Value, this.cmbExpenseType.SelectedValue,
                                                          this.dtAccountingDate1.Value, this.dtAccountingDate2.Value,
                                                          this.dtTheDateOfUse1.Value, this.dtTheDateOfUse2.Value,
                                                          pageIndex, numOnPage, this.tabIndex, this.LoginInfo.User.ID, this._authority.IsExpenceAllApproved, this.SortDirec, this.SortField);

                    if (this.ListExpenceInfoViewState.Count == 0)
                    {
                        this.ListExpenceInfoViewState = null;
                    }

                    if (this.tabIndex == 0)
                    {
                        this.rptProjectList.DataSource = this.ListExpenceInfoViewState;
                        this.rptProject_ApprovedList.DataSource = null;
                        this.btnAccept.Visible = this.ListExpenceInfoViewState != null;
                    }
                    else
                    {
                        this.rptProjectList.DataSource = null;
                        this.rptProject_ApprovedList.DataSource = this.ListExpenceInfoViewState;
                        this.btnAccept.Visible = false;
                    }

                    this.rptProjectList.DataBind();
                    this.rptProject_ApprovedList.DataBind();

                    this.LoadDataForPagingHeader(pageIndex, numOnPage);
                }
            }
            catch (Exception e)
            {
                Log.Instance.WriteLog(e);
            }
        }

        /// <summary>
        /// Init Combobox
        /// </summary>
        private void InitCombobox(DropDownList ddl)
        {
            // init combox
            using (DB db = new DB())
            {
                Config_HService dbSer = new Config_HService(db);

                //Get list detail
                IList<DropDownModel> listM_Config_D = dbSer.GetDataForDropDownList(M_Config_H.CONFIG_CD_EXPENCE_TYPE, true);

                ddl.DataSource = listM_Config_D;
                ddl.DataValueField = "Value";
                ddl.DataTextField = "DisplayName";
                ddl.DataBind();
            }

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

        #endregion
        #region Web Methods

        /// <summary>
        /// Get Project Name By Project Code
        /// </summary>
        /// <param name="groupCd">Project Code</param>
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
                        projecNm = string.Empty
                    };
                    return OMS.Utilities.EditDataUtil.JsonSerializer<object>(onlyCD);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        protected void btnExcel_Command(object sender, CommandEventArgs e)
        {
            excel2 = false;
            IList<ExpenceInfo> IlistExpence;

            using (DB db = new DB())
            {
                Expence_HService prService = new Expence_HService(db);
                this.ListExpenceTotalRow = prService.getTotalRow(this.cmbUser.SelectedValue, this.cmbUser2.SelectedValue,
                                                                 this.cmbDepartment.SelectedValue,
                                                                 this.txtProjectCode.Value, this.cmbExpenseType.SelectedValue, this.dtAccountingDate1.Value, this.dtAccountingDate2.Value,
                                                                 this.dtTheDateOfUse1.Value, this.dtTheDateOfUse2.Value, this.tabIndex, LoginInfo.User.ID, this._authority.IsExpenceAllApproved);
                IlistExpence = prService.GetListByCond(this.cmbUser.SelectedValue, this.cmbUser2.SelectedValue,
                                                       this.cmbDepartment.SelectedValue,
                                                       this.txtProjectCode.Value, cmbExpenseType.SelectedValue,
                                                       dtAccountingDate1.Value, dtAccountingDate2.Value,
                                                       dtTheDateOfUse1.Value, dtTheDateOfUse2.Value,
                                                       1, this.ListExpenceTotalRow, this.tabIndex, this.LoginInfo.User.ID, this._authority.IsExpenceAllApproved);
            }

            ExpenceListExcel excel = new ExpenceListExcel();
            excel.modelInput = IlistExpence;
            IWorkbook wb = excel.OutputExcel();
            if (wb != null)
            {
                this.SaveFile(wb);
            }

            this.LoadDataGrid(this.PagingHeader.CurrentPage, this.PagingHeader.NumRowOnPage);
        }

        //excel2
        protected void btnExcel2_Click(object sender, CommandEventArgs e)
        {
            excel2 = true;
            DateTime? _dateFrom = this.dtUseDateFrom.Value;
            DateTime? _dateTo = this.dtUseDateTo.Value;
            List<ExpenceSeisanExcel> _lstExpence = new List<ExpenceSeisanExcel>();
            using (DB db = new DB())
            {
                if (this.radAccountingDate.Checked)
                {
                    _dateFrom = dtAccountingDateFrom.Value;
                    _dateTo = dtAccountingDateTo.Value;
                }

                Expence_HService prService = new Expence_HService(db);
                _lstExpence = prService.GetListByCond_SeisanExcel(this.cmbUser3.SelectedValue, this.radAccountingDate.Checked, _dateFrom, _dateTo, this.radStatusSinsei.Checked).ToList();
            }

            ExpenceSeisanListExcel excel = new ExpenceSeisanListExcel();
            //if (_lstExpence.Count == 0)
            //{
            //    if (this.cmbUser3.SelectedValue.ToString() == "-1")
            //    {
            //        using (DB db = new DB())
            //        {
            //            UserService user = new UserService(db);
            //            M_User muser = user.GetByID(int.Parse(this.cmbUser3.SelectedValue.ToString()));

            //            ExpenceSeisanExcel item = new ExpenceSeisanExcel();
            //            item.UserCD = EditDataUtil.ToFixCodeShow(muser.UserCD, M_User.MAX_USER_CODE_SHOW);
            //            item.UserName = muser.UserName1;
            //            _lstExpence.Add(item);
            //        }
            //    }
            //}

            excel.modelInput = _lstExpence;
            excel.isAccountingDate = this.radAccountingDate.Checked;
            excel.dateForm = string.Format(Constants.FMR_DATE_YMD, _dateFrom);
            excel.dateTo = string.Format(Constants.FMR_DATE_YMD, _dateTo);

            IWorkbook wb = excel.OutputExcel();
            if (wb != null)
            {
                this.SaveFile(wb);
            }
        }

        /// <summary>
        /// btnDownloadExcel_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDownloadExcel_Click(object sender, EventArgs e)
        {
            if (this.IsOutFile)
            {
                var filename = string.Empty;
                if (excel2)
                {
                    filename = string.Format(ATTENDANCELIST_DOWNLOAD2, DateTime.Now.ToString(FMT_YMDHMM));
                }
                else
                {
                    filename = string.Format(ATTENDANCELIST_DOWNLOAD, DateTime.Now.ToString(FMT_YMDHMM));
                }

                var filePath = this.ViewState["OUTFILE"].ToString();
                using (var exportData = base.GetFileStream("OUTFILE"))
                {
                    Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("Content-Disposition", string.Format("attachment; filename = \"{0}\"", filename));
                    Response.Clear();
                    Response.BinaryWrite(exportData.GetBuffer());
                    Response.End();
                }


            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        private XmlNode getNodeXml()
        {
            XmlDocument xd = new XmlDocument();
            xd.LoadXml(Resources.Email);

            XmlNodeList nodelist = xd.SelectNodes("/sendmail/email");

            foreach (XmlNode node in nodelist)
            {
                string type = node.SelectSingleNode("type").InnerText;

                if (type == "expence_approve")
                {
                    return node;
                }
            }

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="items"></param>
        private void SendMail(List<T_Expence_H> items)
        {
            try
            {
                if (items == null)
                {
                    return;
                }

                if (!this._authority.IsExpenceMail)
                {
                    return;
                }

                string _subject = string.Empty;
                string _content = string.Empty;
                XmlNode node_email = this.getNodeXml();
                _subject = node_email.SelectSingleNode("subject").InnerText.Trim();
                _content = node_email.SelectSingleNode("content").InnerText.Trim();

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

                            for (int j = 0; j < items.Count; j++)
                            {
                                using (DB db = new DB())
                                {
                                    UserService userSer = new UserService(db);
                                    M_User mUserSyonin = userSer.GetByID_Project(items[j].ProjectID);
                                    M_User mUserSinsei = userSer.GetByID(items[j].UserID);

                                    Config_DService config_D = new Config_DService(db);
                                    string expence_type = config_D.GetValue2(M_Config_H.CONFIG_CD_EXPENCE_TYPE, int.Parse(items[j].AccountCD));

                                    List<M_User> lstUserTo = new List<M_User>();
                                    lstUserTo = userSer.GetListUserByDepartmentCode(M_Department.DB_DEPARTMENT_CODE_2).ToList();

                                    // 承認者　＝　総務者　➔　✖
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

                                    for (int i = 0; i < lstUserTo.Count; i++)
                                    {
                                        BodyBuilder body = new BodyBuilder();
                                        body.TextBody = string.Format(_content, lstUserTo[i].UserName1, mUserSinsei.UserName1, this.LoginInfo.User.UserName1,
                                            string.Format(Constants.FMR_DATE_YMD, items[j].Date),
                                            expence_type, items[j].ExpenceAmount);

                                        var message = this.CreateMessage();
                                        message.To.Clear();
                                        message.To.Add(new MailboxAddress(lstUserTo[i].MailAddress));
                                        message.Subject = _subject;
                                        message.Body = body.ToMessageBody();

                                        sendFolder.Append(message); //sync email to server
                                        if (!SendMessage(client, message))
                                        {
                                            base.SetMessage(string.Empty, M_Message.MSG_USER_SEND_ERMAIL_ERROR, mUserSyonin.UserName1);
                                        }

                                        System.Diagnostics.Debug.WriteLine(items[j].ExpenceNo + "/" + lstUserTo[i].LoginID);
                                    }
                                }
                            }

                            this.CloseConnectSmtp(client);
                            this.CloseConnectImap(imap);
                        }
                    }
                    else
                    {
                        this.SetMessage(string.Empty, M_Message.MSG_CONNECT_ERMAIL_ERROR, "");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Instance.WriteLog(ex);
                base.SetMessage(string.Empty, M_Message.MSG_CONNECT_ERMAIL_ERROR, "");
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
    }
}