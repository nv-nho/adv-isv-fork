using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using OMS.Utilities;
using OMS.DAC;
using OMS.Models;
using OMS.Reports.EXCEL;
using NPOI.SS.UserModel;

namespace OMS.Project
{
    public partial class FrmProjectList : FrmBaseList
    {
        #region Constant
        private const string CONST_DANGER_TEXT = "無効";

        private const string ATTENDANCELIST_DOWNLOAD = "99.オーダーマスタ.xls";

        #endregion

        #region Property
        /// <summary>
        /// Get or set Collapse
        /// </summary>
        public string Collapse
        {
            get { return (string)ViewState["Collapse"]; }
            set { ViewState["Collapse"] = value; }
        }

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
            base.FormTitle = "プロジェクト一覧";
            base.FormSubTitle = "List";

            //header grid sort
            this.HeaderGrid.OnSortClick += Sort_Click;

            //paging footer
            this.PagingFooter.OnClick += Paging_Footer_click;

            //paging header
            this.PagingHeader.OnClick += Paging_Header_click;
            this.PagingHeader.OnPagingClick += Paging_Footer_click;
            this.PagingHeader.NumRowOnPage = base.NumRowOnPageDefault;
            this.PagingHeader.CurrentPage = base.CurrentPageDefault;
            this.PagingHeader.IsShowColor = true;
            this.PagingHeader.DangerText = CONST_DANGER_TEXT;

            //define event button search
            this.btnSearch.ServerClick += new EventHandler(btnSearch_Click);

            //Click the Download Excel button
            this.btnDownload.ServerClick += new EventHandler(btnDownloadExcel_Click);

            //Init Max Length
            this.txtProjectCD.MaxLength = M_Project.PROJECT_CODE_SHOW_MAX_LENGTH;
            this.txtProjectName.MaxLength = M_Project.PROJECT_NAME_MAX_LENGTH;
            this.txtWorkPlace.MaxLength = M_Project.PROJECT_WORKPLACE_MAX_LENGTH;
        }


        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Check authority
            base.SetAuthority(FormId.Project);
            if (!this._authority.IsMasterView)
            {
                Response.Redirect("~/Menu/FrmMasterMenu.aspx");
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

                this.Collapse = string.Empty;
            }

            LoadUserComboboxAttribute();
        }

        /// <summary>
        /// Event changed page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNew_Click(object sender, CommandEventArgs e)
        {
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
        /// Get Format ProjectCD
        /// </summary>
        /// <param name="in1"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string GetProjectCD(string in1)
        {
            return EditDataUtil.JsonSerializer(new
            {
                txtProjectCD = in1
            });
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
            this.LoadDataGrid(1, this.PagingHeader.NumRowOnPage);
        }

        /// <summary>
        /// Search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Refresh sort header
            this.HeaderGrid.SortDirec = "2";
            this.HeaderGrid.SortField = "3";

            // Refresh load grid
            this.LoadDataGrid(1, this.PagingHeader.NumRowOnPage);

            this.Collapse = "in";
        }

        #region Event Excel

        /// <summary>
        /// btnExcel Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcel_Click(object sender, CommandEventArgs e)
        {
            SetPaging();

            ProjectExcel excel = new ProjectExcel();

            using (DB db = new DB())
            {

                ProjectService proSer = new ProjectService(db);
                UserService userSer = new UserService(db);

                excel.ListProject = proSer.GetListForExcel();
                excel.ListUser = userSer.GetValidUsers();
            }

            IWorkbook wb = excel.OutputExcel();

            if (wb != null)
            {
                this.SaveFile(wb, ".xls");
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

                filename = string.Format(ATTENDANCELIST_DOWNLOAD);
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

        #endregion

        #endregion

        #region Method
        /// <summary>
        /// Init Data
        /// </summary>
        private void InitData()
        {
            // header grid
            this.HeaderGrid.SortDirec = "2";
            this.HeaderGrid.SortField = "3";

            //Invalid
            this.InitCombobox(this.cmbInvalidData);
            //AcceptanceFlag
            this.InitCombobox(this.cmbAcceptanceFlag);
            //Department
            this.InitCombobox(this.cmbDepartment);
            //User
            this.InitCombobox(this.cmbUser);

            base.DisabledLink(this.btnNew, !base._authority.IsMasterNew);
        }

        /// <summary>
        /// Save condition search
        /// </summary>
        private void SaveCondSearch()
        {
            Hashtable hash = new Hashtable();
            hash.Add(this.txtProjectCD.ID, this.txtProjectCD.Value);
            hash.Add(this.txtProjectName.ID, this.txtProjectName.Value);
            hash.Add(this.txtWorkPlace.ID, this.txtWorkPlace.Value);
            hash.Add(this.cmbInvalidData.ID, this.cmbInvalidData.SelectedValue);
            hash.Add(this.dtStartDate1.ID, this.dtStartDate1.Value);
            hash.Add(this.dtStartDate2.ID, this.dtStartDate2.Value);
            hash.Add(this.dtEndDate1.ID, this.dtEndDate1.Value);
            hash.Add(this.dtEndDate2.ID, this.dtEndDate2.Value);
            hash.Add(this.cmbAcceptanceFlag.ID, this.cmbAcceptanceFlag.SelectedValue);
            hash.Add(this.cmbDepartment.ID, this.cmbDepartment.SelectedValue);
            hash.Add(this.cmbUser.ID, this.cmbUser.SelectedValue);

            hash.Add("NumRowOnPage", this.PagingHeader.NumRowOnPage);
            hash.Add("CurrentPage", this.PagingHeader.CurrentPage);

            //------------------- ISV-GIAM Edited 2014/12/29 -------------------
            hash.Add("SortField", this.HeaderGrid.SortField);
            hash.Add("SortDirec", this.HeaderGrid.SortDirec);
            //------------------------------------------------------------------

            this.ViewState["Condition"] = hash;
        }

        /// <summary>
        /// Show Condition
        /// </summary>
        private void ShowCondition(Hashtable data)
        {
            this.txtProjectCD.Value = data[this.txtProjectCD.ID].ToString();
            this.txtProjectName.Value = data[this.txtProjectName.ID].ToString();
            this.txtWorkPlace.Value = data[this.txtWorkPlace.ID].ToString();
            this.dtStartDate1.Value = (DateTime?)data[this.dtStartDate1.ID];
            this.dtStartDate2.Value = (DateTime?)data[this.dtStartDate2.ID];
            this.dtEndDate1.Value = (DateTime?)data[this.dtEndDate1.ID];
            this.dtEndDate2.Value = (DateTime?)data[this.dtEndDate2.ID];
            this.cmbInvalidData.SelectedValue = data[this.cmbInvalidData.ID].ToString();
            this.cmbAcceptanceFlag.SelectedValue = data[this.cmbAcceptanceFlag.ID].ToString();
            this.cmbDepartment.SelectedValue = data[this.cmbDepartment.ID].ToString();
            this.cmbUser.SelectedValue = data[this.cmbUser.ID].ToString();

            int curPage = int.Parse(data["CurrentPage"].ToString());
            this.PagingHeader.CurrentPage = curPage;
            this.PagingFooter.CurrentPage = curPage;

            int rowOfPage = int.Parse(data["NumRowOnPage"].ToString());
            this.PagingHeader.NumRowOnPage = rowOfPage;

            //------------------- ISV-GIAM Edited 2014/12/29 -------------------
            this.HeaderGrid.SortField = data["SortField"].ToString();
            this.HeaderGrid.SortDirec = data["SortDirec"].ToString();
            //------------------------------------------------------------------
        }

        /// <summary>
        /// load data gird
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="numOnPage"></param>
        private void LoadDataGrid(int pageIndex, int numOnPage)
        {
            int totalRow = 0;
            Hashtable pageMenu = new Hashtable();
            IList<ProjectInfo> listProject;
            try
            {
                using (DB db = new DB())
                {
                    ProjectService prService = new ProjectService(db);

                    totalRow = prService.getTotalRowForList(this.txtProjectCD.Value, this.txtProjectName.Value, this.txtWorkPlace.Value,
                                                            dtStartDate1.Value, dtStartDate2.Value,
                                                            dtEndDate1.Value, dtEndDate2.Value,
                                                            cmbInvalidData.SelectedValue, int.Parse(cmbAcceptanceFlag.SelectedValue),
                                                            int.Parse(cmbDepartment.SelectedValue), int.Parse(cmbUser.SelectedValue));

                    listProject = prService.GetListByCond(this.txtProjectCD.Value, this.txtProjectName.Value, this.txtWorkPlace.Value,
                                                            dtStartDate1.Value, dtStartDate2.Value,
                                                            dtEndDate1.Value, dtEndDate2.Value,
                                                            cmbInvalidData.SelectedValue, int.Parse(cmbAcceptanceFlag.SelectedValue),
                                                            int.Parse(cmbDepartment.SelectedValue), int.Parse(cmbUser.SelectedValue),
                                                            pageIndex, numOnPage, int.Parse(this.HeaderGrid.SortField), int.Parse(this.HeaderGrid.SortDirec));
                }

                if (listProject.Count == 0)
                {
                    this.rptProjectList.DataSource = null;
                    pageMenu.Add("CurrentPage", pageIndex);
                    pageMenu.Add("NumberOnPage", numOnPage);
                    pageMenu.Add("RowNumFrom", 0);
                    pageMenu.Add("RowNumTo", 0);
                }
                else
                {
                    // paging header
                    this.PagingHeader.RowNumFrom = int.Parse(listProject[0].RowNumber.ToString());
                    this.PagingHeader.RowNumTo = int.Parse(listProject[listProject.Count - 1].RowNumber.ToString());
                    this.PagingHeader.TotalRow = totalRow;
                    this.PagingHeader.CurrentPage = pageIndex;

                    // paging footer
                    this.PagingFooter.CurrentPage = pageIndex;
                    this.PagingFooter.NumberOnPage = numOnPage;
                    this.PagingFooter.TotalRow = totalRow;

                    // header
                    this.HeaderGrid.TotalRow = totalRow;
                    this.HeaderGrid.AddColumms(new string[] { "#", "", "コード", "プロジェクト名", "管理者", "N#受注金額", "期間", "状況", "備考" });

                    // detail
                    this.rptProjectList.DataSource = listProject;

                    pageMenu.Add("RowNumFrom", this.PagingHeader.RowNumFrom);
                    pageMenu.Add("RowNumTo", this.PagingHeader.RowNumTo);
                    pageMenu.Add("CurrentPage", this.PagingHeader.CurrentPage);
                    pageMenu.Add("NumberOnPage", this.PagingFooter.NumberOnPage);
                }

                pageMenu.Add("TotalRow", totalRow);
                this.pageMenu = pageMenu;

                this.rptProjectList.DataBind();
            }
            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// Init Combobox
        /// </summary>
        private void InitCombobox(DropDownList ddl)
        {
            if (ddl.Equals(this.cmbInvalidData))
            {
                // init combox
                ddl.DataSource = this.GetDataForDropdownList(M_Config_H.CONFIG_CD_INVALID_TYPE);
                ddl.DataValueField = "Value";
                ddl.DataTextField = "DisplayName";
                ddl.DataBind();
                ddl.SelectedValue = this.GetDefaultValueForDropdownList(M_Config_H.CONFIG_CD_INVALID_TYPE);
            }
            if (ddl.Equals(this.cmbDepartment))
            {
                var depaermentSV = new DepartmentService(new DB());
                var list = depaermentSV.GetDepartmentCbbData(-1);
                list.Insert(0, new DropDownModel("-1", "---"));
                ddl.DataSource = list;
                ddl.DataValueField = "Value";
                ddl.DataTextField = "DisplayName";
                ddl.DataBind();
            }
            if (ddl.Equals(this.cmbUser))
            {
                var depaermentSV = new DepartmentService(new DB());
                var userSV = new UserService(new DB());
                var list = userSV.GetCbbUserManagementProject();
                UserList = list;
                list.Insert(0, new DropDownModel("-1", "---"));
                ddl.DataSource = list;
                ddl.DataValueField = "Value";
                ddl.DataTextField = "DisplayName";
                ddl.DataBind();
                ddl.SelectedValue = "-1";
            }
            if (ddl.Equals(this.cmbAcceptanceFlag))
            {
                var lstStatus = new List<DropDownModel>();
                lstStatus.Add(new DropDownModel("-1", "---"));
                lstStatus.Add(new DropDownModel("0", "仕掛"));
                lstStatus.Add(new DropDownModel("1", "検収"));
                ddl.DataSource = lstStatus;
                ddl.DataValueField = "Value";
                ddl.DataTextField = "DisplayName";
                ddl.DataBind();
                ddl.SelectedValue = "0";
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
                foreach (ListItem item in cmbUser.Items)
                {
                    if (item.Value != "-1")
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

                if (this.UserList[this.cmbUser.SelectedIndex].Value != "-1" && this.UserList[this.cmbUser.SelectedIndex].Status != "0")
                {
                    cmbUser.CssClass = "form-control input-sm bg-danger";
                }
                else
                {
                    cmbUser.CssClass = "form-control input-sm";
                }

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
            this.HeaderGrid.TotalRow = (int)pageMenu["TotalRow"];
            this.HeaderGrid.AddColumms(new string[] { "#", "", "コード", "プロジェクト名", "管理者", "N#受注金額", "期間", "状況", "備考" });
        }
        #endregion
    }
}