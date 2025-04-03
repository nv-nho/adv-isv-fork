using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using OMS.Models;
using OMS.DAC;
using OMS.Utilities;

namespace OMS.Master
{
    /// <summary>
    /// Page Information List
    /// -------------------------
    /// Author : ISV-TRAM
    /// Date   : 2014/09/30
    /// Ver    : 0.0.0.1
    /// -------------------------
    /// </summary>
    public partial class FrmInformationList : FrmBaseList
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
            //Set Title
            base.FormTitle = "Information Master";
            base.FormSubTitle = "List";

            // header grid sort
            this.HeaderGrid.OnSortClick += Sort_Click;

            // Paging footer
            this.PagingFooter.OnClick += PagingFooter_Click;

            // Paging header
            this.PagingHeader.OnClick += PagingHeader_Click;
            this.PagingHeader.OnPagingClick += PagingFooter_Click;
            this.PagingHeader.NumRowOnPage = base.NumRowOnPageDefault;
            this.PagingHeader.CurrentPage = base.CurrentPageDefault;

            //Search button
            this.btnSearch.ServerClick += new EventHandler(btnSearch_Click);

            //Init Max Length                        
            this.txtInformationName.MaxLength = M_Information.INFORMATION_NAME_MAX_LENGTH;
        }

        /// <summary>
        /// Page Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Check authority of login user
            base.SetAuthority(FormId.Information);
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
            this.ViewState["DataID"] = e.CommandArgument;

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

        ///// <summary>
        ///// Get Information Name by Information Code
        ///// </summary>
        ///// <param name="in1">Information Code</param>
        ///// <returns>Information Name</returns>
        //[System.Web.Services.WebMethod]
        //public static string GetInformationName(string in1)
        //{
        //    try
        //    {
        //        var informationCD = in1;
        //        informationCD = OMS.Utilities.EditDataUtil.ToFixCodeDB(informationCD, M_Information.INFORMATION_CODE_MAX_LENGTH);
        //        using (DB db = new DB())
        //        {
        //            InformationService service = new InformationService(db);

        //            var model = service.GetByCD(informationCD);
        //            if (model != null)
        //            {
        //                var result = new
        //                {
        //                    txtInformationCode = informationCD,
        //                    txtInformationName = model.InformationName
        //                };
        //                return OMS.Utilities.EditDataUtil.JsonSerializer<object>(result);
        //            }
        //            var onlyCd = new
        //            {
        //                txtInformationCode = informationCD
        //            };
        //            return OMS.Utilities.EditDataUtil.JsonSerializer<object>(onlyCd);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}
        #endregion

        #region Method

        /// <summary>
        /// Save condition search
        /// </summary>
        private void SaveCondition()
        {
            Hashtable hash = new Hashtable();
            hash.Add(this.txtInformationName.ID, this.txtInformationName.Value);

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
            this.txtInformationName.Value = data[this.txtInformationName.ID].ToString();

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

            base.DisabledLink(this.btnNew, !base._authority.IsMasterNew);
        }

        /// <summary>
        /// Load data grid
        /// </summary>
        /// <param name="pageIndex">Index of page</param>
        /// <param name="numOnPage">The number of lines on a page</param>
        private void LoadDataGrid(int pageIndex, int numOnPage)
        {
            int totalRow = 0;

            IList<InformationInfo> dataSource;
           
            //Get data
            using (DB db = new DB())
            {
                InformationService service = new InformationService(db);
                totalRow = service.GetTotalRow(this.txtInformationName.Value);

                dataSource = service.GetListByCond(this.txtInformationName.Value,
                                                   pageIndex, numOnPage, int.Parse(this.HeaderGrid.SortField), int.Parse(this.HeaderGrid.SortDirec));
            }

            //Show data
            if (dataSource.Count == 0)
            {
                this.rptEventList.DataSource = null;
            }
            else
            {
                // paging header
                this.PagingHeader.RowNumFrom = int.Parse(dataSource[0].RowNumber.ToString());
                this.PagingHeader.RowNumTo = int.Parse(dataSource[dataSource.Count - 1].RowNumber.ToString());
                this.PagingHeader.TotalRow = totalRow;
                this.PagingHeader.CurrentPage = pageIndex;

                // paging footer
                this.PagingFooter.CurrentPage = pageIndex;
                this.PagingFooter.NumberOnPage = numOnPage;
                this.PagingFooter.TotalRow = totalRow;

                // header
                this.HeaderGrid.TotalRow = totalRow;
                this.HeaderGrid.AddColumms(new string[] { "#", "", "お知らせ情報名", "開始日", "終了日" });

                // detail
                this.rptEventList.DataSource = dataSource;
            }

            this.rptEventList.DataBind();
        }

        #endregion
    }
}