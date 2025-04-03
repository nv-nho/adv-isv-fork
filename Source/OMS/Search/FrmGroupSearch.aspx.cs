using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using OMS.DAC;
using OMS.Models;
using OMS.Utilities;

namespace OMS.Search
{
    /// <summary>
    /// FrmGroupSearch
    /// Create  : isv.thuy
    /// Date    : 24/07/2014
    /// </summary>
    public partial class FrmGroupSearch : FrmBaseList
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
            this.txtGroupCD.MaxLength = M_GroupUser_H.GROUP_CODE_SHOW_MAX_LENGTH;
            this.txtGroupName.MaxLength = M_GroupUser_H.GROUP_NAME_MAX_LENGTH;
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
            if (Request.QueryString["GroupCD"] != null)
            {
                this.txtGroupCD.Value = Request.QueryString["GroupCD"];
            }

            //Set GroupName
            if (Request.QueryString["GroupNm"] != null)
            {
                this.txtGroupName.Value = Request.QueryString["GroupNm"];
            }

            ////Set GroupName
            if (Request.QueryString["GroupCDCtrl"] != null)
            {
                this.Out1.Value = Request.QueryString["GroupCDCtrl"];
            }

            //Set GroupName
            if (Request.QueryString["GroupNmCtrl"] != null)
            {
                this.Out2.Value = Request.QueryString["GroupNmCtrl"];
            }
        }

        /// <summary>
        /// Load data into grid
        /// </summary>
        /// <param name="pageIndex"></param>
        private void LoadDataGrid(int pageIndex, int numOnPage)
        {
            string groupCd = null;
            string groupNm = null;

            //Group Code
            if (!this.txtGroupCD.IsEmpty)
            {
                groupCd = EditDataUtil.ToFixCodeDB(this.txtGroupCD.Value, M_GroupUser_H.GROUP_CODE_DB_MAX_LENGTH);
            }

            //Group Name
            if (!this.txtGroupName.IsEmpty)
            {
                groupNm = this.txtGroupName.Value;
            }

            //Get data
            IList<GroupUserSearchInfo> listGroupUserInfo = null;
            int totalRow = 0;
            using (DB db = new DB())
            {
                GroupUserService groupUserSer = new GroupUserService(db);
                totalRow = groupUserSer.GetCountByConditionForSearch(groupCd, groupNm);

                listGroupUserInfo = groupUserSer.GetListByConditionForSearch(groupCd, groupNm, pageIndex, numOnPage, int.Parse(this.HeaderGrid.SortField), int.Parse(this.HeaderGrid.SortDirec));
            }

            //Show Data
            if (totalRow != 0)
            {
                this.PagingHeader.RowNumFrom = int.Parse(listGroupUserInfo[0].RowNumber.ToString());
                this.PagingHeader.RowNumTo = int.Parse(listGroupUserInfo[listGroupUserInfo.Count - 1].RowNumber.ToString());
                this.PagingHeader.TotalRow = totalRow;
                this.PagingHeader.CurrentPage = pageIndex;

                // paging footer
                this.PagingFooter.CurrentPage = pageIndex;
                this.PagingFooter.NumberOnPage = numOnPage;
                this.PagingFooter.TotalRow = totalRow;

                // header
                this.HeaderGrid.TotalRow = totalRow;
                this.HeaderGrid.AddColumms(new string[] { "#", "", "コード", "権限グループ名" });
            }

            this.Collapse = listGroupUserInfo.Count > 0 ? string.Empty : "in";
            this.rptGroupUserList.DataSource = listGroupUserInfo;
            this.rptGroupUserList.DataBind();
        }

        #endregion

        #region Web Methods

        /// <summary>
        /// Format Group Code
        /// </summary>
        /// <param name="in1">GroupCD</param>
        /// <returns>GroupCD</returns>
        [System.Web.Services.WebMethod]
        public static string FormatGroupCD(string in1)
        {
            try
            {
                var groupCd = in1;
                var groupCdShow = in1;
                groupCd = OMS.Utilities.EditDataUtil.ToFixCodeDB(groupCd, M_GroupUser_H.GROUP_CODE_DB_MAX_LENGTH);
                groupCdShow = OMS.Utilities.EditDataUtil.ToFixCodeShow(groupCd, M_GroupUser_H.GROUP_CODE_SHOW_MAX_LENGTH);
                var onlyCd = new
                {
                    txtGroupCD = groupCdShow
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