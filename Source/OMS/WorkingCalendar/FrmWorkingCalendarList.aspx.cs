using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using OMS.Models;
using OMS.DAC;
using OMS.Models.Type;
using OMS.Utilities;
using System.Data;

namespace OMS.Master
{
    public partial class FrmWorkingCalendarList : FrmBaseList
    {
        private const string CONST_DANGER_TEXT = "Invalid";
        #region Property

        /// <summary>
        /// Get or set Collapse
        /// </summary>
        public string Collapse
        {
            get { return (string)ViewState["Collapse"]; }
            set { ViewState["Collapse"] = value; }
        }

        public bool isOnlyPaidLeave;
        #endregion

        #region Event

        /// <summary>
        /// Event Init
        /// </summary>
        /// <param name="e"></param>
        /// 

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            //Set Title
            base.FormTitle = "勤務体系カレンダー一覧";
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
            this.PagingHeader.IsShowColor = false;
            this.PagingHeader.DangerText = CONST_DANGER_TEXT;

            //Search button
            this.btnSearch.ServerClick += new EventHandler(btnSearch_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            base.SetAuthority(FormId.WorkCalendar);
            this.isOnlyPaidLeave = false;
            if (!this._authority.IsWorkCalendarView)
            {
                if (IsExistsCalendar())
                {
                    isOnlyPaidLeave = true;
                }else
                {
                    Response.Redirect("~/Menu/FrmMainMenu.aspx");
                }
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
                this.HeaderGrid.SortField = "3"; //Calendar code
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
            this.HeaderGrid.SortField = "1";

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

        private bool IsExistsCalendar()
        {
            using (DB db = new DB())
            {
                T_WorkingCalendar_HService calendarHSer = new T_WorkingCalendar_HService(db);
                return calendarHSer.IsExistsByUID(this.LoginInfo.User.ID, -1);
            }
        }

        /// <summary>
        /// Save condition search
        /// </summary>
        private void SaveCondition()
        {
            Hashtable hash = new Hashtable();
            hash.Add(this.txtCalendarCD.ID, this.txtCalendarCD.Value);
            hash.Add(this.txtCalendarName.ID, this.txtCalendarName.Value);
            
            //hash.Add(this.cmbInvalidData.ID, this.cmbInvalidData.SelectedValue);
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
            this.txtCalendarCD.Value = data[this.txtCalendarCD.ID].ToString();
            this.txtCalendarName.Value = data[this.txtCalendarName.ID].ToString();

            int curPage = int.Parse(data["CurrentPage"].ToString());
            this.PagingHeader.CurrentPage = curPage;
            this.PagingFooter.CurrentPage = curPage;

            int rowOfPage = int.Parse(data["NumRowOnPage"].ToString());
            this.PagingHeader.NumRowOnPage = rowOfPage;

            this.HeaderGrid.SortField = data["SortField"].ToString();
            this.HeaderGrid.SortDirec = data["SortDirec"].ToString();
        }

        /// <summary>
        /// Init Data
        /// </summary>
        private void InitData()
        {
            // Default data valide
            this.hdInValideDefault.Value = this.GetDefaultValueForDropdownList(M_Config_H.CONFIG_CD_INVALID_TYPE);
            
            // header grid
            this.HeaderGrid.SortDirec = "1";
            this.HeaderGrid.SortField = "1";

            base.DisabledLink(this.btnNew, !base._authority.IsMasterNew);
        }

        /// <summary>
        /// Init Combobox
        /// </summary>
        private void InitCombobox(DropDownList ddl)
        {
            // init combox 
            ddl.DataSource = this.GetDataForDropdownList(M_Config_H.CONFIG_CD_INVALID_TYPE);
            ddl.DataValueField = "Value";
            ddl.DataTextField = "DisplayName";
            ddl.DataBind();
            ddl.SelectedValue = this.hdInValideDefault.Value;
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
        /// load data grid
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="numOnPage"></param>
        private void LoadDataGrid(int pageIndex, int numOnPage)
        {
            int totalRow = 0;
            int onlyUserID = 0;
            DataTable dataTableWorkingDate;
            IList<WorkingCalendarResult> listWorkingCalendarResult;
            WorkingCalendarHeaderSearch model = new WorkingCalendarHeaderSearch();
            model.CalendarCD = txtCalendarCD.Value;
            model.CalendarName = txtCalendarName.Value;
            if (this.isOnlyPaidLeave)
            {
                onlyUserID = this.LoginInfo.User.ID;
            }
            //Get data
            using (DB db = new DB())
            {
                T_WorkingCalendar_HService workingCalendarService = new T_WorkingCalendar_HService(db);

                totalRow = workingCalendarService.GetTotalRow(model, onlyUserID);

                listWorkingCalendarResult = workingCalendarService.GetListForSearch(model, onlyUserID, pageIndex, numOnPage, int.Parse(this.HeaderGrid.SortField), int.Parse(this.HeaderGrid.SortDirec));
                
                // Change format InitialDate
                dataTableWorkingDate = InitDataTableWorkingDate();
                int index = 0;
                foreach (var item in listWorkingCalendarResult)
                {
                    DateTime fromDate = (DateTime)item.InitialDate;
                    DateTime toDate = fromDate.AddYears(1).AddDays(-1);
                    string strInitialDate = String.Format(Constants.FMR_DATE_YMD, fromDate) + "~" + string.Format(Constants.FMR_DATE_YMD, toDate);
                    DataRow drow = dataTableWorkingDate.NewRow();
                    dataTableWorkingDate.Rows.Add(drow);
                    dataTableWorkingDate.Rows[index].SetField("RowNumber", item.RowNumber);
                    dataTableWorkingDate.Rows[index].SetField("ID", item.ID);
                    dataTableWorkingDate.Rows[index].SetField("CalendarCD", item.CalendarCD);
                    dataTableWorkingDate.Rows[index].SetField("CalendarName", item.CalendarName);
                    dataTableWorkingDate.Rows[index].SetField("InitialDate", strInitialDate);
                    index++;
                }
            }

            //Show data
            if (listWorkingCalendarResult.Count == 0)
            {
                this.rptWorkingCalendarList.DataSource = null;
            }
            else
            {
                // paging header
                this.PagingHeader.RowNumFrom = int.Parse(listWorkingCalendarResult[0].RowNumber.ToString());
                this.PagingHeader.RowNumTo = int.Parse(listWorkingCalendarResult[listWorkingCalendarResult.Count - 1].RowNumber.ToString());
                this.PagingHeader.TotalRow = totalRow;
                this.PagingHeader.CurrentPage = pageIndex;

                // paging footer
                this.PagingFooter.CurrentPage = pageIndex;
                this.PagingFooter.NumberOnPage = numOnPage;
                this.PagingFooter.TotalRow = totalRow;

                // header
                this.HeaderGrid.TotalRow = totalRow;
                this.HeaderGrid.AddColumms(new string[] { "#", "", "コード", "名称", "対象期間" });

                // detail
                this.rptWorkingCalendarList.DataSource = dataTableWorkingDate;
            }

            this.rptWorkingCalendarList.DataBind();
        }
        #endregion

        #region Web Methods

        /// <summary>
        /// InitDataTableWorkingDate
        /// </summary>
        /// <param name="groupCd">ID</param>
        /// <returns>Data Table</returns>
        public DataTable InitDataTableWorkingDate()
        {
            DataTable dataTableWorkingDate = new DataTable();

            dataTableWorkingDate.Columns.Add("RowNumber", typeof(int));
            dataTableWorkingDate.Columns.Add("ID", typeof(string));
            dataTableWorkingDate.Columns.Add("CalendarCD", typeof(string));
            dataTableWorkingDate.Columns.Add("CalendarName", typeof(string));
            dataTableWorkingDate.Columns.Add("InitialDate", typeof(string));

            return dataTableWorkingDate;

        }
        #endregion
        
    }
}