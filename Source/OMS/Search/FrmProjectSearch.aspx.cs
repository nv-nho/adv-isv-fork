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
    /// FrmProjectSearch
    /// Create  : isv.Hien
    /// Date    : 2017/10/18
    /// </summary>
    public partial class FrmProjectSearch : FrmBaseList
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
            this.txtProjectCD.MaxLength = M_Project.PROJECT_CODE_SHOW_MAX_LENGTH;
            this.txtProjectName.MaxLength = M_Project.PROJECT_NAME_MAX_LENGTH;

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
            if (Request.QueryString["ProjectCD"] != null)
            {
                this.txtProjectCD.Value = Request.QueryString["ProjectCD"];
            }

            //Set GroupName
            if (Request.QueryString["ProjectNm"] != null)
            {
                this.txtProjectName.Value = Request.QueryString["ProjectNm"];
            }

            //Set GroupName
            if (Request.QueryString["initDate"] != null)
            {
                this.initDateHiddenSearch.Value = Request.QueryString["initDate"];
            }
            
            ////Set GroupName
            if (Request.QueryString["ProjectCDCtrl"] != null)
            {
                this.Out1.Value = Request.QueryString["ProjectCDCtrl"];
            }

            //Set GroupName
            if (Request.QueryString["ProjectNmCtrl"] != null)
            {
                this.Out2.Value = Request.QueryString["ProjectNmCtrl"];
            }
        }

        /// <summary>
        /// Load data into grid
        /// </summary>
        /// <param name="pageIndex"></param>
        private void LoadDataGrid(int pageIndex, int numOnPage)
        {
            string projectCd = null;
            string projectNm = null;
            string initDate = null;
            //Project Code
            if (!this.txtProjectCD.IsEmpty)
            {
                projectCd = this.txtProjectCD.Value;
            }

            //Project Name
            if (!this.txtProjectName.IsEmpty)
            {
                projectNm = this.txtProjectName.Value;
            }

            // set initDate
            initDate = this.initDateHiddenSearch.Value;
            

            //Get data
            IList<ProjectSearchInfo> listProjectInfo = null;
            int totalRow = 0;
            using (DB db = new DB())
            {
                ProjectService proSer = new ProjectService(db);
                totalRow = proSer.GetCountByConditionForSearch(projectCd, projectNm, initDate);

                listProjectInfo = proSer.GetListByConditionForSearch(projectCd, projectNm, initDate, pageIndex, numOnPage, int.Parse(this.HeaderGrid.SortField), int.Parse(this.HeaderGrid.SortDirec));
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
                this.HeaderGrid.AddColumms(new string[] { "#", "", "コード", "プロジェクト名" });
            }

            this.Collapse = listProjectInfo.Count > 0 ? string.Empty : "in";
            this.rptProjectUserList.DataSource = listProjectInfo;
            this.rptProjectUserList.DataBind();
        }

        #endregion

        #region Web Methods

        /// <summary>
        /// Format Project Code
        /// </summary>
        /// <param name="in1">ProjectCD</param>
        /// <returns>ProjectCD</returns>
        [System.Web.Services.WebMethod]
        public static string FormatProjectCD(string in1)
        {
            try
            {
                var projectCd = in1;
                var projectCdShow = in1;
               // projectCd = OMS.Utilities.EditDataUtil.ToFixCodeDB(projectCd, M_Project.PROJECT_CODE_DB_MAX_LENGTH);
              //  projectCdShow = OMS.Utilities.EditDataUtil.ToFixCodeShow(projectCd, M_Project.PROJECT_CODE_SHOW_MAX_LENGTH);
                var onlyCd = new
                {
                    txtProjectCD = projectCdShow
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