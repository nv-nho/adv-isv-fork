using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using OMS.Utilities;
using System.Collections;
using OMS.DAC;
using OMS.Models;
using System.IO;

namespace OMS.Payslip
{
    public partial class FrmPayslipList : FrmBaseList
    {
        #region Constant
        private const string CONST_KAKUNIN_TEXT = "確認済";
        #endregion

        #region Property

        /// <summary>
        /// Get or set Success
        /// </summary>
        public bool Downloading { get; set; }

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
            base.FormTitle = "給与・賞与明細一覧";
            base.FormSubTitle = "List";

            //Init Event

            // header grid sort
            this.HeaderGrid.OnSortClick += Sort_Click;

            // paging footer
            this.PagingFooter.OnClick += PagingFooter_Click;

            // paging header
            this.PagingHeader.OnClick += PagingHeader_Click;
            this.PagingHeader.OnPagingClick += PagingFooter_Click;
            this.PagingHeader.NumRowOnPage = base.NumRowOnPageDefault;
            this.PagingHeader.CurrentPage = base.CurrentPageDefault;
            this.PagingHeader.IsShowColor = true;
            this.PagingHeader.LightGrayText = CONST_KAKUNIN_TEXT;

            //Download File Click
            this.btnDownload.ServerClick += new EventHandler(btnDownload_Click);

            //Download File Click
            this.btnDownloading.ServerClick += new EventHandler(btnDownloading_Click);
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
            base.SetAuthority(FormId.AttendancePayslip);
            if (!this._authority.IsAttendancePayslipView)
            {
                if (this.PreviousPage != null)
                {
                    if (this.PreviousPageViewState["ConditionApproval"] != null)
                    {
                        this.ViewState["ConditionApproval"] = this.PreviousPageViewState["ConditionApproval"];
                        Server.Transfer("~/Payslip/FrmPayslipList.aspx");
                    }
                    else
                    {
                        Response.Redirect("~/Menu/FrmMainMenu.aspx");
                    }
                }
                else
                {
                    Response.Redirect("~/Menu/FrmMainMenu.aspx");
                }

            }
            if (!this.IsPostBack)
            {
                InitData();

                //Show condition
                if (this.PreviousPage != null)
                {
                    if (this.PreviousPageViewState["Condition"] != null)
                    {
                        Hashtable data = (Hashtable)PreviousPageViewState["Condition"];

                        this.ShowCondition(data);
                    }
                }

                //Upload
                if (this._authority.IsAttendancePayslipUpload == false)
                {
                    base.DisabledLink(this.btnUpload, !this._authority.IsAttendancePayslipUpload);
                }

                //Departments
                if (this._authority.IsAttendancePayslipDepartments == true)
                {
                    this.ddlDepartment.Enabled = true;
                }
                else
                {
                    this.ddlDepartment.Enabled = false;
                }

                //Employees
                if (this._authority.IsAttendancePayslipEmployees == true)
                {
                    this.ddlUser.Enabled = true;
                }
                else
                {
                    this.ddlUser.Enabled = false;
                }

                //Show data on grid
                this.LoadDataGrid(this.PagingHeader.CurrentPage, this.PagingHeader.NumRowOnPage);
            }
            this.Downloading = false;
        }

        /// <summary>
        /// Init Data
        /// </summary>
        private void InitData()
        {
            InitCombobox();

            // header grid
            this.HeaderGrid.SortDirec = "1";
            this.HeaderGrid.SortField = "3";

            this.ddlDepartment.Enabled = false;
            this.ddlUser.Enabled = false;
        }

        /// <summary>
        /// CheckAuthority
        /// </summary>
        private void CheckAuthority()
        {
            // Check Other Departments
            if (!base._authority.IsAttendancePayslipDepartments)
            {
                this.ddlDepartment.Enabled = false;
            }

            // Check Shelf Registration
            if (!base._authority.IsAttendancePayslipEmployees)
            {
                this.ddlUser.Enabled = false;
            }
        }

        /// <summary>
        /// ddlUser_SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            string yearId = this.ddlYear.SelectedValue;
            LoadYearComboboxData();
            if (ddlYear.Items.FindByValue(yearId) != null)
            {
                this.ddlYear.SelectedValue = yearId;
            }

            string userId = this.ddlUser.SelectedValue;
            this.LoadUserComboboxData(year: ddlYear.SelectedValue);

            if (ddlUser.Items.FindByValue(userId) != null)
            {
                this.ddlUser.SelectedValue = userId;
            }

            this.LoadDataGrid(1, this.PagingHeader.NumRowOnPage);
            
            //LoadUserComboboxAttribute();
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

            this.LoadYearComboboxData();

            //Load user combobox data
            this.LoadUserComboboxData(year: ddlYear.SelectedValue);

            this.LoadDataGrid(1, this.PagingHeader.NumRowOnPage);
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {

            string userId = this.ddlUser.SelectedValue;
            this.LoadUserComboboxData(year: ddlYear.SelectedValue);

            if (ddlUser.Items.FindByValue(userId) != null)
            {
                this.ddlUser.SelectedValue = userId;
            }

            this.LoadDataGrid(1, this.PagingHeader.NumRowOnPage);
            //LoadUserComboboxAttribute();
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

            //Load calendar combobox data
            this.LoadYearComboboxData();

            //Load user combobox data
            this.LoadUserComboboxData(year: ddlYear.SelectedValue);
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
        /// Load Department Combobox data
        /// </summary>
        private void LoadDepartmentComboboxData()
        {
            using (DB db = new DB())
            {
                IList<DropDownModel> departmentList;
                DepartmentService departmentService = new DepartmentService(db);
                if (!base._authority.IsAttendancePayslipDepartments)
                {
                    departmentList = departmentService.GetDepartmentCbbData(departmentId: LoginInfo.User.DepartmentID);
                }
                else
                {
                    departmentList = departmentService.GetDepartmentCbbData();
                }

                ddlDepartment.Items.Clear();

                if (departmentList.Count > 0)
                {
                    ddlDepartment.DataSource = departmentList;
                    ddlDepartment.DataValueField = "Value";
                    ddlDepartment.DataTextField = "DisplayName";
                    if (this.LoginInfo.User.DepartmentID >= 10)
                    {
                        ddlDepartment.SelectedValue = this.LoginInfo.User.DepartmentID.ToString();
                    }
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
            if (string.IsNullOrEmpty(ddlDepartment.SelectedValue))
            {
                return;
            }
            using (DB db = new DB())
            {
                IList<DropDownModel> userList = new List<DropDownModel>();
                UserService userService = new UserService(db);
                
                DropDownModel emptOption = new DropDownModel();
                emptOption.DisplayName = "----------";
                emptOption.Value = "";

                if (!base._authority.IsAttendancePayslipEmployees)
                {
                    userList = userService.GetCbbUserDataByDepartmentID(int.Parse(ddlDepartment.SelectedValue), year: year, userId: LoginInfo.User.ID);
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

                    foreach (var item in userList)
                    {
                        if (item.Value == this.LoginInfo.User.ID.ToString())
                        {
                            ddlUser.SelectedValue = this.LoginInfo.User.ID.ToString();
                        }
                    }

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
        /// Load year combobox data
        /// </summary>
        private void LoadYearComboboxData()
        {
            using (DB db = new DB())
            {
                IList<DropDownModel> yearList;
                PaysplipService paysplipService = new PaysplipService(db);
                DateTime todaysDate = DateTime.Now.Date;
                string dispYear = todaysDate.Year.ToString();
                if (todaysDate.Month > 6) {
                    dispYear = (todaysDate.Year + 1).ToString();
                }
                Boolean flag = false;
                UserService userService = new UserService(db);
                M_User user = new M_User();

                if (!string.IsNullOrEmpty(this.ddlUser.SelectedValue))
                {
                    user = userService.GetByUserCD(this.ddlUser.SelectedValue);
                    yearList = paysplipService.GetCbbTargetYear(this.ddlUser.SelectedValue);
                }
                else
                {
                    yearList = paysplipService.GetCbbTargetYearByDepartment();
                }

                ddlYear.Items.Clear();

                if (yearList.Count > 0)
                {
                    ddlYear.DataSource = yearList.Reverse();
                    ddlYear.DataValueField = "Value";
                    ddlYear.DataTextField = "DisplayName";

                    foreach (var item in yearList)
                    {
                        if (item.Value == dispYear)
                        {
                            ddlYear.SelectedValue = dispYear;
                            flag = true;
                        }
                            
                    }
                    if (!flag) {
                        if(this.ddlUser.SelectedIndex != 0 && UserList[this.ddlUser.SelectedIndex].Status == "0")
                        {
                            DropDownModel dropdown = new DropDownModel(dispYear, dispYear);
                            yearList.Add(dropdown);
                            ddlYear.DataSource = yearList.Reverse();
                        }
                    }
                }
                else
                {
                    DropDownModel dropdown = new DropDownModel(dispYear, dispYear);
                    yearList.Add(dropdown);
                    ddlYear.DataValueField = "Value";
                    ddlYear.DataTextField = "DisplayName";
                    ddlYear.DataSource = yearList;
                }
                ddlYear.DataBind();
            }
        }

        /// <summary>
        /// load data grid
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="numOnPage"></param>
        private void LoadDataGrid(int pageIndex, int numOnPage)
        {
            int totalRow = 0;

            if (string.IsNullOrEmpty(this.ddlYear.SelectedValue) || string.IsNullOrEmpty(this.ddlUser.SelectedValue))
            {
                // detail
                this.rptList.DataSource = null;
            }
            else if (!this._authority.IsAttendancePayslipEmployees && this.ddlUser.SelectedValue != LoginInfo.User.ID.ToString())
            {
                // detail
                this.rptList.DataSource = null;
            }
            else
            {
                IList<PayslipInfo> listInfo;

                //Get data
                using (DB db = new DB())
                {
                    PaysplipService dbSer = new PaysplipService(db);
                    totalRow = dbSer.getTotalRow(this.ddlYear.SelectedValue, this.ddlUser.SelectedValue);

                    listInfo = dbSer.GetListByCond(this.ddlYear.SelectedValue, this.ddlUser.SelectedValue,
                                                   pageIndex, numOnPage, int.Parse(this.HeaderGrid.SortField), int.Parse(this.HeaderGrid.SortDirec));
                }

                //Show data
                if (listInfo.Count == 0)
                {
                    this.rptList.DataSource = null;
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
                    this.HeaderGrid.AddColumms(new string[] { "#", "", "明細区分", "対象月", "登録日", "確認日" });

                    // detail
                    this.rptList.DataSource = listInfo;
                }
            }

            LoadUserComboboxAttribute();

            this.rptList.DataBind();
        }

        /// <summary>
        /// Event changed page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDownload_Click(object sender, EventArgs e)
        {
            using (DB db = new DB())
            {
                PaysplipUploadService payslipUploadSer = new PaysplipUploadService(db);
                PaysplipService payslipSer = new PaysplipService(db);

                int payslipID = Int32.Parse(this.hdnFileName.Value);
                PayslipDowloadInfo payslipUpload = payslipUploadSer.getFilePathbyPayslipID(payslipID);
                String filePath = payslipUpload.Filepath;

                if (!String.IsNullOrEmpty(filePath))
                {
                    string filename = Path.GetFileName(filePath);

                    if (File.Exists((filePath)))
                    {
                        //UpdateDownloadDate 
                        payslipSer.UpdateDownloadDate(payslipID, this.LoginInfo.User.ID);
                        this.LoadDataGrid(this.PagingHeader.CurrentPage, this.PagingHeader.NumRowOnPage);
                        this.Downloading = true;
                    }
                    else
                    {
                        this.SetMessage(string.Empty, M_Message.MSG_VALUE_NOT_EXIST, "ファイル");
                    }
                }
                
                this.LoadDataGrid(this.PagingHeader.CurrentPage, this.PagingHeader.NumRowOnPage);
            }
        }

        /// <summary>
        /// Event changed page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDownloading_Click(object sender, EventArgs e)
        {
            using (DB db = new DB())
            {
                PaysplipUploadService payslipUploadSer = new PaysplipUploadService(db);
                PaysplipService payslipSer = new PaysplipService(db);

                int payslipID = Int32.Parse(this.hdnFileName.Value);
                PayslipDowloadInfo payslipUpload = payslipUploadSer.getFilePathbyPayslipID(payslipID);
                String filePath = payslipUpload.Filepath;

                if (!String.IsNullOrEmpty(filePath))
                {
                    string filename = Path.GetFileName(filePath);

                    if (File.Exists((filePath)))
                    {
                        Response.ClearContent();
                        Response.Clear();
                        Response.ContentType = "text/plain";
                        Response.AddHeader("Content-Disposition", string.Format("attachment; filename = \"{0}\"", filename));
                        Response.TransmitFile(filePath);
                        Response.Flush();
                        Response.End();
                    }
                    else
                    {
                        this.SetMessage(string.Empty, M_Message.MSG_VALUE_NOT_EXIST, "ファイル");
                    }
                }

                this.LoadDataGrid(this.PagingHeader.CurrentPage, this.PagingHeader.NumRowOnPage);
            }
        }

        /// <summary>
        /// Save condition search
        /// </summary>
        private void SaveCondition()
        {
            Hashtable hash = new Hashtable();
            hash.Add(this.ddlYear.ID, this.ddlYear.SelectedValue);
            hash.Add(this.ddlDepartment.ID, this.ddlDepartment.SelectedValue);
            hash.Add(this.ddlUser.ID, this.ddlUser.SelectedValue);
            hash.Add("NumRowOnPage", this.PagingHeader.NumRowOnPage);
            hash.Add("CurrentPage", this.PagingHeader.CurrentPage);
            //----------------Add 2014/12/29 ISV-HUNG-----------------------//
            hash.Add("SortField", this.HeaderGrid.SortField);
            hash.Add("SortDirec", this.HeaderGrid.SortDirec);
            //----------------Add 2014/12/29 ISV-HUNG-----------------------//
            this.ViewState["Condition"] = hash;
        }

        /// <summary>
        /// Show Condition
        /// </summary>
        private void ShowCondition(Hashtable data)
        {
            this.ddlDepartment.SelectedValue = data[this.ddlDepartment.ID].ToString();
            this.LoadUserComboboxData(year: data[this.ddlYear.ID].ToString());
            this.ddlUser.SelectedValue = data[this.ddlUser.ID].ToString();
            if (this.UserList[this.ddlUser.SelectedIndex].Value != "" && this.UserList[this.ddlUser.SelectedIndex].Status != "0")
            {
                ddlUser.CssClass = "form-control input-sm bg-danger";
            }
            else
            {
                ddlUser.CssClass = "form-control input-sm";
            }
            this.LoadYearComboboxData();
            this.ddlYear.SelectedValue = data[this.ddlYear.ID].ToString();

            int curPage = int.Parse(data["CurrentPage"].ToString());
            this.PagingHeader.CurrentPage = curPage;
            this.PagingFooter.CurrentPage = curPage;

            //----------------Add 2014/12/29 ISV-HUNG-----------------------//
            this.HeaderGrid.SortField = data["SortField"].ToString();
            this.HeaderGrid.SortDirec = data["SortDirec"].ToString();
            //----------------Add 2014/12/29 ISV-HUNG-----------------------//
            int rowOfPage = int.Parse(data["NumRowOnPage"].ToString());
            this.PagingHeader.NumRowOnPage = rowOfPage;
        }

        /// <summary>
        /// Event changed page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUploadItem_Click(object sender, CommandEventArgs e)
        {
            //Save condition
            this.SaveCondition();
            this.ViewState["Year"] = this.ddlYear.SelectedValue;
        }

        /// <summary>
        /// Event changed page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDetail_Click(object sender, CommandEventArgs e)
        {
            this.LoadDataGrid(1, this.PagingHeader.NumRowOnPage);
        }

        #endregion
    }
}