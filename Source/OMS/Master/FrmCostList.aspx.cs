using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using OMS.DAC;
using OMS.Models;
using OMS.Utilities;
using System.Xml;
using System.Data;

namespace OMS.Master
{
    /// <summary>
    /// Cost List
    /// </summary>
    public partial class FrmCostList : FrmBaseList
    {
        #region Constants
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
            base.FormTitle = "原価一覧";
            base.FormSubTitle = "リスト";

            // Header grid sort
            this.HeaderGrid.OnSortClick += this.Sort_Click;

            // Paging footer
            this.PagingFooter.OnClick += this.PagingFooter_Click;

            // Paging header
            this.PagingHeader.OnClick += this.PagingHeader_Click;
            this.PagingHeader.OnPagingClick += this.PagingFooter_Click;
            this.PagingHeader.NumRowOnPage = base.NumRowOnPageDefault;
            this.PagingHeader.CurrentPage = base.CurrentPageDefault;
        }

        /// <summary>
        /// Page Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Check authority of login user
            base.SetAuthority(FormId.Cost);
            if (!this._authority.IsMasterView)
            {
                Response.Redirect("~/Menu/FrmMasterMenu.aspx");
            }

            //if (!base.CheckAuthorityMaster(FormId.ExchangeRate, AuthorTypeMaster.View))
            //{
            //    Response.Redirect("~/Menu/FrmMasterMenu.aspx");
            //}

            if (!base.IsPostBack)
            {
                //Init data
                this.InitData();

                //Show condition
                if (base.PreviousPage != null)
                {
                    if (base.PreviousPageViewState["Condition"] != null)
                    {
                        Hashtable data = (Hashtable)base.PreviousPageViewState["Condition"];

                        this.ShowCondition(data);
                    }
                }

                //Show data on grid
                this.LoadDataGrid(this.PagingHeader.CurrentPage, this.PagingHeader.NumRowOnPage);
            }
        }

        /// <summary>
        /// Event changed page
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">CommandEventArgs</param>
        protected void btnNew_Click(object sender, CommandEventArgs e)
        {
            //Save condition
            this.SaveCondition();
        }

        /// <summary>
        /// Event changed page
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">CommandEventArgs</param>
        protected void btnDetail_Click(object sender, CommandEventArgs e)
        {
            //ID
            base.ViewState["ID"] = e.CommandArgument;

            //Save condition
            this.SaveCondition();
        }

        /// <summary>
        /// Paging change list
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
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
        /// Paging change list
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void PagingHeader_Click(object sender, EventArgs e)
        {
            if (sender != null)
            {
                this.LoadDataGrid(1, this.PagingHeader.NumRowOnPage);
            }
        }

        /// <summary>
        /// Sort change list
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Sort_Click(object sender, EventArgs e)
        {
            this.LoadDataGrid(1, this.PagingHeader.NumRowOnPage);
        }
        #endregion

        #region Method
        /// <summary>
        /// Save condition search
        /// </summary>
        private void SaveCondition()
        {
            Hashtable hash = new Hashtable();
            hash.Add("NumRowOnPage", this.PagingHeader.NumRowOnPage);
            hash.Add("CurrentPage", this.PagingHeader.CurrentPage);

            hash.Add("SortField", this.HeaderGrid.SortField);
            hash.Add("SortDirec", this.HeaderGrid.SortDirec);

            ViewState["Condition"] = hash;
        }

        /// <summary>
        /// Get condition Save
        /// </summary>
        /// <param name="data">Data</param>
        private void ShowCondition(Hashtable data)
        {
            int curPage = int.Parse(data["CurrentPage"].ToString());
            this.PagingHeader.CurrentPage = curPage;
            this.PagingFooter.CurrentPage = curPage;

            this.HeaderGrid.SortField = data["SortField"].ToString();
            this.HeaderGrid.SortDirec = data["SortDirec"].ToString();

            int rowOfPage = int.Parse(data["NumRowOnPage"].ToString());
            this.PagingHeader.NumRowOnPage = rowOfPage;
        }

        /// <summary>
        /// Init control
        /// </summary>
        private void InitData()
        {
            // Header grid
            this.HeaderGrid.SortDirec = "1";
            this.HeaderGrid.SortField = "3";

            //this.btnNew.Attributes.Add("class", this.CheckAuthorityMaster(FormId.ExchangeRate, AuthorTypeMaster.New) ? Constants.CSS_BTN_NEW : Constants.CSS_BTN_NEW_DISABLED);
            //this.btnNew.Attributes.Add("class", this._authority.IsMasterNew ? Constants.CSS_BTN_NEW : Constants.CSS_BTN_NEW_DISABLED);
            //base.DisabledLink(this.btnNew, !base._authority.IsMasterNew);
        }

        /// <summary>
        /// Load data gird
        /// </summary>
        /// <param name="pageIndex">Page Index</param>
        /// <param name="numOnPage">Number On Page</param>
        private void LoadDataGrid(int pageIndex, int numOnPage)
        {
            int totalRow = 0;
            IList<CostInfo> listCostInfo;

            //Get data
            using (DB db = new DB())
            {
                Cost_HService Cost_HService = new Cost_HService(db);

                //Get total row
                totalRow = Cost_HService.GetTotalRow(string.Empty, string.Empty);

                //Get list
                listCostInfo = Cost_HService.GetList(string.Empty, string.Empty, pageIndex, numOnPage, int.Parse(this.HeaderGrid.SortField), int.Parse(this.HeaderGrid.SortDirec));
            }

            //Show data
            if (listCostInfo.Count == 0)
            {
                this.rptCostList.DataSource = null;
            }
            else
            {
                // Paging header
                this.PagingHeader.RowNumFrom = int.Parse(listCostInfo[0].RowNumber.ToString());
                this.PagingHeader.RowNumTo = int.Parse(listCostInfo[listCostInfo.Count - 1].RowNumber.ToString());
                this.PagingHeader.TotalRow = totalRow;
                this.PagingHeader.CurrentPage = pageIndex;

                // Paging footer
                this.PagingFooter.CurrentPage = pageIndex;
                this.PagingFooter.NumberOnPage = numOnPage;
                this.PagingFooter.TotalRow = totalRow;

                // Header
                this.HeaderGrid.TotalRow = totalRow;
                this.HeaderGrid.AddColumms(new string[] { "#", "", "原価名称", "開始日", "終了日", "原価（時給単価)" });

                // Detail
                this.rptCostList.DataSource = listCostInfo;
            }

            this.rptCostList.DataBind();
        }
        #endregion
    }
}