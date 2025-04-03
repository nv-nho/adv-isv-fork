using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using OMS.Models;
using System.Collections;
using OMS.DAC;

namespace OMS.Master
{
    /// <summary>
    /// Form Config Master
    /// nv-Nho
    /// </summary>
    public partial class FrmConfigList : FrmBaseList
    {
        #region Property

        /// <summary>
        /// Get or set collapse property
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
            base.FormTitle = "Config Master";
            base.FormSubTitle = "List";

            // header grid sort
            this.HeaderGrid.OnSortClick += Sort_Click;

            // paging footer
            this.PagingFooter.OnClick += PagingFooter_Click;

            // paging header
            this.PagingHeader.OnClick += PagingHeader_Click;
            this.PagingHeader.OnPagingClick += PagingFooter_Click;
            this.PagingHeader.NumRowOnPage = base.NumRowOnPageDefault;
            this.PagingHeader.CurrentPage = base.CurrentPageDefault;

            //Search button
            this.btnSearch.ServerClick += new EventHandler(btnSearch_Click);

            //Init Max Length
            this.txtConfigCD.MaxLength = M_Config_H.CONFIG_CODE_MAX_LENGTH;
            this.txtConfigName.MaxLength = M_Config_H.CONFIG_NAME_MAX_LENGTH;
        }

        /// <summary>
        /// Page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsAdmin())
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
        }

        /// <summary>
        /// Search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Refresh sort header
            this.HeaderGrid.SortDirec = "1";
            this.HeaderGrid.SortField = "3";

            // Refresh load grid
            this.LoadDataGrid(1, this.PagingHeader.NumRowOnPage);

            this.Collapse = "in";
        }

        /// <summary>
        /// Event changed page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNew_Click(object sender, CommandEventArgs e)
        {
            //Save condition
            this.SaveCondition();
        }

        /// <summary>
        /// Event changed page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDetail_Click(object sender, CommandEventArgs e)
        {
            //UserID
            this.ViewState["ID"] = e.CommandArgument;

            //Save condition
            this.SaveCondition();
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

        /// <summary>
        /// Click Sort
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
        /// Save condition search
        /// </summary>
        private void SaveCondition()
        {
            Hashtable hash = new Hashtable();
            hash.Add(this.txtConfigCD.ID, this.txtConfigCD.Value);
            hash.Add(this.txtConfigName.ID, this.txtConfigName.Value);
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
            this.txtConfigCD.Value = data[this.txtConfigCD.ID].ToString();
            this.txtConfigName.Value = data[this.txtConfigName.ID].ToString();

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
        /// Init Data
        /// </summary>
        private void InitData()
        {
            // header grid
            this.HeaderGrid.SortDirec = "1";
            this.HeaderGrid.SortField = "3";
        }

        /// <summary>
        /// load data grid
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="numOnPage"></param>
        private void LoadDataGrid(int pageIndex, int numOnPage)
        {
            int totalRow = 0;

            IList<ConfigInfo> listInfo;

            //Get data
            using (DB db = new DB())
            {
                Config_HService dbSer = new Config_HService(db);
                totalRow = dbSer.getTotalRow(this.txtConfigCD.Value, this.txtConfigName.Value);

                listInfo = dbSer.GetListByCond(this.txtConfigCD.Value, this.txtConfigName.Value,
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
                this.HeaderGrid.AddColumms(new string[] { "#", "", "CD", "Config Name" });

                // detail
                this.rptList.DataSource = listInfo;
            }

            this.rptList.DataBind();
        }

        #endregion
    }
}