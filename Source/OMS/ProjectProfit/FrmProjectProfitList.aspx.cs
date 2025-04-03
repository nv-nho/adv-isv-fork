using OMS.DAC;
using OMS.Models;
using OMS.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OMS.Reports.EXCEL;
using NPOI.SS.UserModel;

namespace OMS.ProjectProfit
{
    public partial class FrmProjectProfitList : FrmBaseList
    {
        #region Constant
        private const string CONST_DANGER_TEXT = "無効";
        private const int OPT_WORKING_TYPE_0 = 0;
        private const int OPT_WORKING_TYPE_1 = 1;
        private const int OPT_WORKING_TYPE_2 = 2;
        private const int OPT_BREAK_TYPE_0 = 0;
        private const int OPT_BREAK_TYPE_1 = 1;
        private const int OPT_BREAK_TYPE_2 = 2;

        public const string OT_CONFIG_CD = "C004";

        private const string ATTENDANCELIST_DOWNLOAD = "採算管理一覧表_{0}.xlsx";
        private const string FMT_YMDHMM = "yyMMddHHmm";
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
            base.FormTitle = "採算管理一覧";
            base.FormSubTitle = "List";

            //Click the Download Excel button
            this.btnDownload.ServerClick += new EventHandler(btnDownloadExcel_Click);
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

            //define event button search
            this.btnSearch.ServerClick += new EventHandler(btnSearch_Click);

            //Init Max Length
            this.txtProjectCode.MaxLength = M_Project.PROJECT_CODE_SHOW_MAX_LENGTH;
            this.txtProjectName.MaxLength = M_Project.PROJECT_NAME_MAX_LENGTH;
        }

        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            base.SetAuthority(FormId.ProjectProfit);
            if (!this._authority.IsMasterView)
            {
                Response.Redirect("~/Menu/FrmMainMenu.aspx");
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
                this.HeaderGrid.SortDirec = "2";
                this.HeaderGrid.SortField = "2";
                this.LoadDataGrid(this.PagingHeader.CurrentPage, this.PagingHeader.NumRowOnPage);

                this.Collapse = string.Empty;
            }
            else
            {
                this.LoadDataGrid(this.PagingHeader.CurrentPage, this.PagingHeader.NumRowOnPage);
                //set paging
                this.Collapse = "in";

            }
            LoadUserComboboxAttribute();
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
        /// Search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Refresh sort header
            this.HeaderGrid.SortDirec = "2";
            this.HeaderGrid.SortField = "2";

            // Refresh load grid
            this.LoadDataGrid(1, this.PagingHeader.NumRowOnPage);

            this.Collapse = "in";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDetail_Command(object sender, CommandEventArgs e)
        {
            ViewState["ID"] = e.CommandArgument;
            this.SaveCondSearch();
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcel_Command(object sender, CommandEventArgs e)
        {

            List<ProjectProfitInfo> listProjectProfit;
            IList<ProjectProfitFullInfo> listProjectProfitFull;
            using (DB db = new DB())
            {
                ProjectProfitService prfService = new ProjectProfitService(db);
                int totalRow = prfService.GetTotalRowForProjectList(this.txtProjectCode.Value, this.txtProjectName.Value, int.Parse(this.cmbDepartment.SelectedValue),
                                                            int.Parse(this.cmbUser.SelectedValue), int.Parse(cmbStatus.SelectedValue));

                IList<M_Project> listProject = prfService.GetListProjectByCondition(this.txtProjectCode.Value, this.txtProjectName.Value, int.Parse(this.cmbDepartment.SelectedValue),
                                                            int.Parse(this.cmbUser.SelectedValue),
                                                            int.Parse(cmbStatus.SelectedValue),
                                                            1, totalRow, int.Parse(this.HeaderGrid.SortField), int.Parse(this.HeaderGrid.SortDirec));

                listProjectProfitFull = prfService.GetFullListByCondition(this.dtStartDate.Value, this.dtEndDate.Value, listProject.Select(pr => pr.ID).ToList(),
                                                            int.Parse(this.HeaderGrid.SortField), int.Parse(this.HeaderGrid.SortDirec));

                listProjectProfit = prfService.CreateProjectProfitInfoGroupProject(listProjectProfitFull, this.dtStartDate.Value, this.dtEndDate.Value, OT_CONFIG_CD).ToList();
            }
            if (listProjectProfit.Count == 0)
            {
            }
            else
            {
                ProjectProfitListExcel excel = new ProjectProfitListExcel();
                excel.modelInput = listProjectProfit;
                IWorkbook wb = excel.OutputExcel();
                if (wb != null)
                {
                    this.SaveFile(wb, ".xlsx");
                }
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

                filename = string.Format(ATTENDANCELIST_DOWNLOAD, DateTime.Now.ToString(FMT_YMDHMM));
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
        /// Get Department Name By Group Code
        /// </summary>
        /// <param name="groupCd">Department Code</param>
        /// <returns>Department Name</returns>
        [System.Web.Services.WebMethod]
        public static string GetProjectName(string in1)
        {
            var projectCd = in1;
            var projectCdShow = in1;

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

        #endregion
        #region Method
        /// <summary>
        /// Init Data
        /// </summary>
        private void InitData()
        {
            // header grid
            this.HeaderGrid.SortDirec = "1";
            this.HeaderGrid.SortField = "2";

            // Default data valide
            this.hdDepartmentDefault.Value = "-1";

            //combobox
            this.InitCombobox(this.cmbDepartment);
            this.InitCombobox(this.cmbUser);
            this.InitCombobox(this.cmbStatus);
        }
        /// <summary>
        /// Init Combobox
        /// </summary>
        private void InitCombobox(DropDownList ddl)
        {
            using (DB db = new DB())
            {
                if (ddl.Equals(this.cmbDepartment))
                {
                    var depaermentSV = new DepartmentService(db);
                    var list = depaermentSV.GetDepartmentCbbData(-1);
                    list.Insert(0, new DropDownModel("-1", "---"));
                    ddl.DataSource = list;
                    ddl.DataValueField = "Value";
                    ddl.DataTextField = "DisplayName";
                    ddl.DataBind();
                    ddl.SelectedValue = this.hdDepartmentDefault.Value;
                }
                if (ddl.Equals(this.cmbUser))
                {
                    var userSV = new UserService(db);
                    var list = userSV.GetCbbUserManagementProject();
                    this.UserList = list;
                    list.Insert(0, new DropDownModel("-1", "---"));
                    ddl.DataSource = list;
                    ddl.DataValueField = "Value";
                    ddl.DataTextField = "DisplayName";
                    ddl.DataBind();
                    ddl.SelectedValue = "-1";
                    LoadUserComboboxAttribute();
                }
            }

            if (ddl.Equals(this.cmbStatus))
            {
                var lstStatus = new List<DropDownModel>();
                lstStatus.Add(new DropDownModel("-1", "---"));
                lstStatus.Add(new DropDownModel("0", "仕掛"));
                lstStatus.Add(new DropDownModel("1", "検収"));
                ddl.DataSource = lstStatus;
                ddl.DataValueField = "Value";
                ddl.DataTextField = "DisplayName";
                ddl.DataBind();
                ddl.SelectedValue = "-1";
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
        /// load data gird
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="numOnPage"></param>
        private void LoadDataGrid(int pageIndex, int numOnPage)
        {

            int totalRow = 0;
            IList<ProjectProfitInfo> listProjectProfit;
            IList<ProjectProfitFullInfo> listProjectProfitFull;

            IList<M_Project> listProject;

            using (DB db = new DB())
            {
                ProjectProfitService prfService = new ProjectProfitService(db);

                listProject = prfService.GetListProjectByCondition(this.txtProjectCode.Value, this.txtProjectName.Value, int.Parse(this.cmbDepartment.SelectedValue),
                                                            int.Parse(this.cmbUser.SelectedValue),
                                                            int.Parse(cmbStatus.SelectedValue),
                                                            pageIndex, numOnPage, int.Parse(this.HeaderGrid.SortField), int.Parse(this.HeaderGrid.SortDirec));

                totalRow = prfService.GetTotalRowForProjectList(this.txtProjectCode.Value, this.txtProjectName.Value, int.Parse(this.cmbDepartment.SelectedValue),
                                                            int.Parse(this.cmbUser.SelectedValue), int.Parse(cmbStatus.SelectedValue));

                listProjectProfitFull = prfService.GetFullListByCondition(this.dtStartDate.Value, this.dtEndDate.Value, listProject.Select(pr => pr.ID).ToList(),
                                                            int.Parse(this.HeaderGrid.SortField), int.Parse(this.HeaderGrid.SortDirec));

                listProjectProfit = prfService.CreateProjectProfitInfoGroupProject(listProjectProfitFull, this.dtStartDate.Value, this.dtEndDate.Value, OT_CONFIG_CD);
            }

            if (listProjectProfit.Count == 0)
            {
                this.rptProjectList.DataSource = null;
            }
            else
            {
                // paging header
                this.PagingHeader.RowNumFrom = int.Parse(listProjectProfit[0].RowNumber.ToString());
                this.PagingHeader.RowNumTo = int.Parse(listProjectProfit[listProjectProfit.Count - 1].RowNumber.ToString());
                this.PagingHeader.CurrentPage = pageIndex;
                this.PagingHeader.TotalRow = totalRow;

                // paging footer
                this.PagingFooter.CurrentPage = pageIndex;
                this.PagingFooter.NumberOnPage = numOnPage;
                this.PagingFooter.TotalRow = totalRow;

                // header
                this.HeaderGrid.TotalRow = totalRow;
                this.HeaderGrid.AddColumms(new string[] { "", "プロジェクト", "部門/管理者", "期間", "状況", "N#受注金額", "N#利益率(%)", "N#原価計", "N#直接費", "N#間接費", "N#経費" });
                foreach (var colInfo in this.HeaderGrid.Columns)
                {
                    if (colInfo.ColumnIndex >= 7)
                    {
                        colInfo.Sorting = false;
                        colInfo.CssClass = "text-dark";
                    }
                }
                // detail
                this.rptProjectList.DataSource = listProjectProfit;

                this.PagingHeader.RowNumFrom = ((pageIndex - 1) * numOnPage + 1);
                this.PagingHeader.RowNumTo = ((((pageIndex - 1) * numOnPage + 1) + numOnPage) - 1) > this.PagingHeader.TotalRow ?
                                                        this.PagingHeader.TotalRow
                                                        : ((((pageIndex - 1) * numOnPage + 1) + numOnPage) - 1);

            }

            this.PagingHeader.TotalRow = totalRow;
            this.PagingFooter.TotalRow = totalRow;

            this.rptProjectList.DataBind();

        }


        /// <summary>
        /// Save condition search
        /// </summary>
        private void SaveCondSearch()
        {
            Hashtable hash = new Hashtable();
            hash.Add(this.txtProjectCode.ID, this.txtProjectCode.Value);
            hash.Add(this.txtProjectName.ID, this.txtProjectName.Value);
            hash.Add(this.cmbDepartment.ID, this.cmbDepartment.SelectedValue);
            hash.Add(this.cmbUser.ID, this.cmbUser.SelectedValue);
            hash.Add(this.dtStartDate.ID, this.dtStartDate.Value);
            hash.Add(this.dtEndDate.ID, this.dtEndDate.Value);
            hash.Add(this.cmbStatus.ID, this.cmbStatus.SelectedValue);

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
            this.txtProjectCode.Value = data[this.txtProjectCode.ID].ToString();
            this.txtProjectName.Value = data[this.txtProjectName.ID].ToString();
            this.cmbDepartment.SelectedValue = data[this.cmbDepartment.ID].ToString();
            InitCombobox(this.cmbUser);
            this.cmbUser.SelectedValue = data[this.cmbUser.ID].ToString();
            this.dtStartDate.Value = (DateTime?)data[this.dtStartDate.ID];
            this.dtEndDate.Value = (DateTime?)data[this.dtEndDate.ID];
            this.cmbStatus.SelectedValue = data[this.cmbStatus.ID].ToString();

            int curPage = int.Parse(data["CurrentPage"].ToString());
            this.PagingHeader.CurrentPage = curPage;
            this.PagingFooter.CurrentPage = curPage;

            int rowOfPage = int.Parse(data["NumRowOnPage"].ToString());
            this.PagingHeader.NumRowOnPage = rowOfPage;

            this.HeaderGrid.SortField = data["SortField"].ToString();
            this.HeaderGrid.SortDirec = data["SortDirec"].ToString();
        }


        #endregion


    }
}