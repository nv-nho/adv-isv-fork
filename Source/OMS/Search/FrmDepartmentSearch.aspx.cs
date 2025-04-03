using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OMS.Models;
using OMS.DAC;
using OMS.Utilities;

namespace OMS.Search
{
    /// <summary>
    /// FrmDepartmentSearch
    /// Create  : isv.Hien
    /// Date    : 2017/10/18
    /// </summary>
    public partial class FrmDepartmentSearch : FrmBaseList
    {
        #region Property

        /// <summary>
        /// Get or set Collapse
        /// </summary>
        public string Collapse
        {
            get { return (string)ViewState["Collapse"]; }
            set { ViewState["Collapse"] = value; }
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

            // header grid sort
            this.HeaderGrid.OnSortClick += Sort_Click;

            // paging header
            this.PagingHeader.OnClick += PagingHeader_Click;
            this.PagingHeader.OnPagingClick += Paging_Click;
            this.PagingHeader.NumRowOnPage = base.NumRowOnPageDefault;
            this.PagingHeader.CurrentPage = base.CurrentPageDefault;

            // Paging footer
            this.PagingFooter.OnClick += Paging_Click;

            //Search button
            this.btnSearch.ServerClick += new EventHandler(btnSearch_Click);

            //Init Max Length
            this.txtDepartmentCD.MaxLength = M_Department.DEPARTMENT_CODE_SHOW_MAX_LENGTH;
            this.txtDepartmentName.MaxLength = M_Department.DEPARTMENT_NAME_MAX_LENGTH;

        }

        /// <summary>
        /// Page Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                // header grid
                this.HeaderGrid.SortDirec = "1";
                this.HeaderGrid.SortField = "1";
                this.PagingHeader.IsCloseForm = true;
                //this.PagingHeader.AddClass = "btn-success";

                //Set data into control
                this.InitData();

                //Load data into grid
                this.LoadDataGrid(this.PagingHeader.CurrentPage, this.PagingHeader.NumRowOnPage);

            }
        }

        /// <summary>
        /// Search Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Refresh sort header
            this.HeaderGrid.SortDirec = "1";
            this.HeaderGrid.SortField = "1";

            this.LoadDataGrid(1, this.PagingHeader.NumRowOnPage);
            this.Collapse = "in";

        }

        /// <summary>
        /// Paging Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Paging_Click(object sender, EventArgs e)
        {
            if (sender != null)
            {
                this.LoadDataGrid(int.Parse((sender as LinkButton).CommandArgument), this.PagingHeader.NumRowOnPage);
            }
        }

        /// <summary>
        /// Click on the paging header
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

        /// <summary>
        /// Sorting on the repeater header
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Sort_Click(object sender, EventArgs e)
        {
            this.LoadDataGrid(1, this.PagingHeader.NumRowOnPage);
        }

        #endregion

        #region Method

        /// <summary>
        /// Set search conditions
        /// </summary>
        private void InitData()
        {
            //Set GroupCD
            if (Request.QueryString["DepartmentCD"] != null)
            {
                this.txtDepartmentCD.Value = Request.QueryString["DepartmentCD"];
            }

            //Set GroupName
            if (Request.QueryString["DepartmentNm"] != null)
            {
                this.txtDepartmentName.Value = Request.QueryString["DepartmentNm"];
            }

            ////Set GroupName
            if (Request.QueryString["DepartmentCDCtrl"] != null)
            {
                this.Out1.Value = Request.QueryString["DepartmentCDCtrl"];
            }

            //Set GroupName
            if (Request.QueryString["DepartmentNmCtrl"] != null)
            {
                this.Out2.Value = Request.QueryString["DepartmentNmCtrl"];
            }
        }

        /// <summary>
        /// Load data into grid
        /// </summary>
        /// <param name="pageIndex"></param>
        private void LoadDataGrid(int pageIndex, int numOnPage)
        {
            string departmentCd = null;
            string departmentNm = null;

            //Project Code
            if (!this.txtDepartmentCD.IsEmpty)
            {
                departmentCd = EditDataUtil.ToFixCodeDB(this.txtDepartmentCD.Value, M_Department.DEPARTMENT_CODE_DB_MAX_LENGTH);
            }

            //Project Name
            if (!this.txtDepartmentName.IsEmpty)
            {
                departmentNm = this.txtDepartmentName.Value;
            }

            //Get data
            IList<DepartmentSearchInfo> listProjectInfo = null;
            int totalRow = 0;
            using (DB db = new DB())
            {
                DepartmentService proSer = new DepartmentService(db);
                totalRow = proSer.GetCountByConditionForSearch(departmentCd, departmentNm);

                listProjectInfo = proSer.GetListByConditionForSearch(departmentCd, departmentNm, pageIndex, numOnPage, int.Parse(this.HeaderGrid.SortField), int.Parse(this.HeaderGrid.SortDirec));
            }

            //Show Data
            if (totalRow != 0)
            {
                this.PagingHeader.RowNumFrom = int.Parse(listProjectInfo[0].RowNumber.ToString());
                this.PagingHeader.RowNumTo = int.Parse(listProjectInfo[listProjectInfo.Count - 1].RowNumber.ToString());
                this.PagingHeader.TotalRow = totalRow;
                this.PagingHeader.CurrentPage = pageIndex;

                // paging footer
                this.PagingFooter.CurrentPage = pageIndex;
                this.PagingFooter.NumberOnPage = numOnPage;
                this.PagingFooter.TotalRow = totalRow;

                // header
                this.HeaderGrid.TotalRow = totalRow;
                this.HeaderGrid.AddColumms(new string[] { "#", "", "コード", "部門名" });
            }

            this.Collapse = listProjectInfo.Count > 0 ? string.Empty : "in";
            this.rptDepartmentList.DataSource = listProjectInfo;
            this.rptDepartmentList.DataBind();
        }

        #endregion

        #region Web Methods

        /// <summary>
        /// Format Department Code
        /// </summary>
        /// <param name="in1">ProjectCD</param>
        /// <returns>ProjectCD</returns>
        [System.Web.Services.WebMethod]
        public static string FormatDepartmentCD(string in1)
        {
            try
            {
                var departmentCd = in1;
                var departmentCdShow = in1;
                departmentCd = OMS.Utilities.EditDataUtil.ToFixCodeDB(departmentCd, M_Department.DEPARTMENT_CODE_DB_MAX_LENGTH);
                departmentCdShow = OMS.Utilities.EditDataUtil.ToFixCodeShow(departmentCd, M_Department.DEPARTMENT_CODE_SHOW_MAX_LENGTH);
                var onlyCd = new
                {
                    txtDepartmentCD = departmentCdShow
                };
                return OMS.Utilities.EditDataUtil.JsonSerializer<object>(onlyCd);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion
    }
}