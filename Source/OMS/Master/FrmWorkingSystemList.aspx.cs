using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Collections;
using OMS.Models;
using OMS.DAC;
using OMS.Utilities;

namespace OMS.WorkingSystem
{
    /// <summary>
    /// FrmWorkingSystemList
    /// Create  : isv.than
    /// Date    : 19/09/2017
    /// </summary>
    public partial class FrmWorkingSystemList : FrmBaseList
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
        /// On Init
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //Set Title
            base.FormTitle = "勤務体系一覧";
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

            //define event button search
            this.btnSearch.ServerClick += new EventHandler(btnSearch_Click);

            //Init Max Length
            this.txtWorkingSystemCD.MaxLength = M_WorkingSystem.WORKING_SYSTEM_CODE_SHOW_MAX_LENGTH;
            this.txtWorkingSystemName.MaxLength = M_WorkingSystem.WORKING_SYSTEM_NAME_MAX_LENGTH;
        }


        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Check authority
            base.SetAuthority(FormId.WorkingSystem);
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
        /// Get Format WorkingSystemCD
        /// </summary>
        /// <param name="in1"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string GetWorkingSystemCD(string in1)
        {
            return EditDataUtil.JsonSerializer(new
            {
                txtWorkingSystemCD = in1
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
            this.HeaderGrid.SortDirec = "1";
            this.HeaderGrid.SortField = "3";

            // Refresh load grid
            this.LoadDataGrid(1, this.PagingHeader.NumRowOnPage);

            this.Collapse = "in";
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
            this.HeaderGrid.SortField = "3";

            //this.btnNew.Attributes.Add("class", this.CheckAuthorityMaster(FormId.GroupUser, AuthorTypeMaster.New) ? Constants.CSS_BTN_NEW : Constants.CSS_BTN_NEW_DISABLED);
            //this.btnNew.Attributes.Add("class", this._authority.IsMasterNew ? Constants.CSS_BTN_NEW : Constants.CSS_BTN_NEW_DISABLED);

            base.DisabledLink(this.btnNew, !base._authority.IsMasterNew);
        }

        /// <summary>
        /// Save condition search
        /// </summary>
        private void SaveCondSearch()
        {
            Hashtable hash = new Hashtable();
            hash.Add(this.txtWorkingSystemCD.ID, this.txtWorkingSystemCD.Value);
            hash.Add(this.txtWorkingSystemName.ID, this.txtWorkingSystemName.Value);

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
            this.txtWorkingSystemCD.Value = data[this.txtWorkingSystemCD.ID].ToString();
            this.txtWorkingSystemName.Value = data[this.txtWorkingSystemName.ID].ToString();

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

            IList<WorkingSystemInfo> listWorkingSystem;
            try
            {
                using (DB db = new DB())
                {
                    WorkingSystemService wrkService = new WorkingSystemService(db);

                    totalRow = wrkService.getTotalRowForList(this.txtWorkingSystemCD.Value, this.txtWorkingSystemName.Value);

                    listWorkingSystem = wrkService.GetListByCond(this.txtWorkingSystemCD.Value, this.txtWorkingSystemName.Value,
                                                        pageIndex, numOnPage, int.Parse(this.HeaderGrid.SortField), int.Parse(this.HeaderGrid.SortDirec));
                }

                if (listWorkingSystem.Count == 0)
                {
                    this.rptWorkingSystemList.DataSource = null;
                }
                else
                {
                    // paging header
                    this.PagingHeader.RowNumFrom = int.Parse(listWorkingSystem[0].RowNumber.ToString());
                    this.PagingHeader.RowNumTo = int.Parse(listWorkingSystem[listWorkingSystem.Count - 1].RowNumber.ToString());
                    this.PagingHeader.TotalRow = totalRow;
                    this.PagingHeader.CurrentPage = pageIndex;

                    // paging footer
                    this.PagingFooter.CurrentPage = pageIndex;
                    this.PagingFooter.NumberOnPage = numOnPage;
                    this.PagingFooter.TotalRow = totalRow;

                    // header
                    this.HeaderGrid.TotalRow = totalRow;
                    this.HeaderGrid.AddColumms(new string[] { "#", "", "コード", "勤務体系名称", "略称" });

                    // detail
                    this.rptWorkingSystemList.DataSource = listWorkingSystem;
                }

                this.rptWorkingSystemList.DataBind();
            }
            catch (Exception e)
            {
                Log.Instance.WriteLog(e);
            }
        }
        #endregion
    }
}