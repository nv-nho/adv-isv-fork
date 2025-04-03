using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using OMS.DAC;
using OMS.Models;
using OMS.Utilities;
using System.Data;
using System.Web.UI.HtmlControls;
using OMS.Reports.EXCEL;
using NPOI.SS.UserModel;
using System.Text;
using System.IO;

namespace OMS.Attendance
{
    public partial class FrmAttendanceSummary : FrmBaseList
    {

        #region Constant

        private const string CONST_DANGER_TEXT = "Invalid";
        private const string SUMMARY_EXCEL_DOWNLOAD_NAME = "勤務集計表_{0}.xlsx";
        private const string CSV_DOWNLOAD_NAME = "勤務集計表_{0}.csv";
        private const string EXCHANGE_DATE_EXCEL_DOWNLOAD_NAME = "振替休日取得一覧表_{0}.xlsx";
        private const string FMT_YMDHMM = "yyMMddHHmm";

        public enum ATSummaryExcelType
        {
            CSV = 0,
            SummaryList,
            ExchangeDate
        }

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
        /// Get or set isShowButtonMenu
        /// </summary>
        public bool isShowButtonMenu
        {
            get { return (bool)ViewState["isShowButtonMenu"]; }
            set { ViewState["isShowButtonMenu"] = value; }
        }

        /// <summary>
        /// Get or set FlagExcel
        /// </summary>
        public ATSummaryExcelType FlagExcel
        {
            get { return (ATSummaryExcelType)ViewState["FlagExcel"]; }
            set { ViewState["FlagExcel"] = value; }
        }

        /// <summary>
        /// Get or set Collapse
        /// </summary>
        public Hashtable pageMenu
        {
            get { return (Hashtable)ViewState["pageMenu"]; }
            set { ViewState["pageMenu"] = value; }
        }

        /// <summary>
        /// Get or set UserList
        /// </summary>
        public IList<DropDownModel> UserList1
        {
            get
            {
                return (IList<DropDownModel>)ViewState["UserList1"];
            }
            set
            {
                ViewState["UserList1"] = value;
            }
        }

        /// <summary>
        /// Get or set UserList
        /// </summary>
        public IList<DropDownModel> UserList2
        {
            get
            {
                return (IList<DropDownModel>)ViewState["UserList2"];
            }
            set
            {
                ViewState["UserList2"] = value;
            }
        }

        /// <summary>
        /// Get or ser DefaultButton
        /// </summary>
        public bool ErrorCSV { get; set; }

        /// <summary>
        /// Get or set NumVacation
        /// </summary>
        public int NumVacation { get; set; }

        /// <summary>
        /// Get or set NumVacation
        /// </summary>
        public int NumOvertime { get; set; }

        /// <summary>
        /// Get or ser IsShowQuestion
        /// </summary>
        public bool IsShowQuestion { get; set; }

        /// <summary>
        /// Get or ser DefaultButton
        /// </summary>
        public string DefaultButton { get; set; }

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
            base.FormTitle = "勤務表集計";
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

            //define event button search
            this.btnSearch.ServerClick += new EventHandler(btnSearch_Click);

            //Click the Download Excel button
            this.btnDownload.ServerClick += new EventHandler(btnDownloadExcel_Click);
            this.isShowButtonMenu = false;

            LinkButton btnYes = (LinkButton)this.Master.FindControl("btnYes");
            btnYes.Click += new EventHandler(btCSVError);

            using (DB db = new DB())
            {
                Config_DService config_DService = new Config_DService(db);
                IList<M_Config_D> vacationList;
                vacationList = config_DService.GetListByConfigCd(M_Config_H.CONFIG_CD_VACATION_TYPE);
                NumVacation = vacationList.Count;

                IList<M_Config_D> overTimeList;
                overTimeList = config_DService.GetListByConfigCd(M_Config_H.CONFIG_CD_OVER_TIME_TYPE);
                NumOvertime = overTimeList.Count;
            }
        }

        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Check authority
            base.SetAuthority(FormId.AttendanceSummary);
            if (!this._authority.IsAttendanceSummaryView)
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
                    }
                }

                //Show data on grid
                this.LoadDataGrid(this.PagingHeader.CurrentPage, this.PagingHeader.NumRowOnPage, this.hdSortField.Value, this.GetSortDirec());

                // check Authority
                CheckAuthority();
            }
            else
            {
                this.Collapse = "in";

                // check Authority
                CheckAuthority();

                SetPaging();
            }

            LoadUserComboboxAttribute(this.ddlUser11, UserList1);
            LoadUserComboboxAttribute(this.ddlUser12, UserList1);
            LoadUserComboboxAttribute(this.ddlUser13, UserList1);
            LoadUserComboboxAttribute(this.ddlUser14, UserList1);

            LoadUserComboboxAttribute(this.ddlUser21, UserList2);
            LoadUserComboboxAttribute(this.ddlUser22, UserList2);
            LoadUserComboboxAttribute(this.ddlUser23, UserList2);
            LoadUserComboboxAttribute(this.ddlUser24, UserList2);
        }

        /// <summary>
        /// CheckAuthority
        /// </summary>
        private void CheckAuthority()
        {
            //check Excel, CSV
            base.DisabledLink(this.btnExcelTop, !base._authority.IsAttendanceSummaryExportExcel);
            base.DisabledLink(this.btnExcelBottom, !base._authority.IsAttendanceSummaryExportExcel);
            base.DisabledLink(this.btnCSVTop, !base._authority.IsAttendanceSummaryExportExcel);
            base.DisabledLink(this.btnCSVBottom, !base._authority.IsAttendanceSummaryExportExcel);
        }

        /// <summary>
        /// Click Sort
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Sort_Click(object sender, EventArgs e)
        {
            string newSortField = (sender as LinkButton).CommandArgument;
            this.SetSort(newSortField);

            this.LoadDataGrid(1, this.PagingHeader.NumRowOnPage, this.hdSortField.Value, this.GetSortDirec());
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
                this.LoadDataGrid(curPage, this.PagingHeader.NumRowOnPage, this.hdSortField.Value, this.GetSortDirec());
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
                this.LoadDataGrid(1, this.PagingHeader.NumRowOnPage, this.hdSortField.Value, this.GetSortDirec());
            }
        }

        /// <summary>
        /// rptAttendanceList_ItemDataBound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptAttendanceSummaryList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DateTime startDate;
                DateTime endDate;
                T_WorkingCalendar_H tWorkingCalendarH = new T_WorkingCalendar_H();
                using (DB db = new DB())
                {
                    T_WorkingCalendar_HService tWorkingCalendarHSer = new T_WorkingCalendar_HService(db);
                    tWorkingCalendarH = tWorkingCalendarHSer.GetByID(int.Parse(ddlWorkingCalendar.SelectedValue));
                }

                DataRowView dr = e.Item.DataItem as DataRowView;

                startDate = tWorkingCalendarH.InitialDate;
                endDate = startDate.AddMonths(12).AddDays(-1);

                if (rbTotal.Checked)
                {
                    if (this.dtStartDate.Value != null)
                    {
                        startDate = this.dtStartDate.Value.Value;
                    }

                    if (this.dtEndDate.Value != null)
                    {
                        endDate = this.dtEndDate.Value.Value;
                    }
                }
                else
                {
 
                    if (ddlDateOfServiceFrom.SelectedValue != "-1")
                    {
                        startDate = DateTime.Parse(ddlDateOfServiceFrom.SelectedValue);
                    }
                    if (ddlDateOfServiceTo.SelectedValue != "-1")
                    {
                        endDate = (DateTime.Parse(ddlDateOfServiceTo.SelectedValue)).AddMonths(1).AddDays(-1);
                    }
                }


                Repeater rptOverTimeH = e.Item.FindControl("rptOverTimeH") as Repeater;
                setHeaderOverTime(rptOverTimeH);

                Repeater rptVacationH = e.Item.FindControl("rptVacationH") as Repeater;
                getVacationConfig(rptVacationH, M_Config_H.CONFIG_CD_VACATION_TYPE, startDate, endDate, int.Parse(dr["UID"].ToString()), int.Parse(dr["Flag"].ToString()));

                Repeater rptVacationD = e.Item.FindControl("rptVacationD") as Repeater;
                getVacationConfig(rptVacationD, M_Config_H.CONFIG_CD_VACATION_TYPE, startDate, endDate, int.Parse(dr["UID"].ToString()), int.Parse(dr["Flag"].ToString()));

                using (DB db = new DB())
                {
                    Config_DService config_DService = new Config_DService(db);
                    IList<M_Config_D> overTimeList;
                    overTimeList = config_DService.GetListByConfigCd(M_Config_H.CONFIG_CD_OVER_TIME_TYPE);

                    switch (overTimeList.Count)
                    {
                        case 0:
                            break;
                        case 1:
                            overTimeList[0].Value4 = dr["TotalOverTimeHours1"].ToString();
                            break;
                        case 2:
                            overTimeList[0].Value4 = dr["TotalOverTimeHours1"].ToString();
                            overTimeList[1].Value4 = dr["TotalOverTimeHours2"].ToString();
                            break;
                        case 3:
                            overTimeList[0].Value4 = dr["TotalOverTimeHours1"].ToString();
                            overTimeList[1].Value4 = dr["TotalOverTimeHours2"].ToString();
                            overTimeList[2].Value4 = dr["TotalOverTimeHours3"].ToString();
                            break;
                        case 4:
                            overTimeList[0].Value4 = dr["TotalOverTimeHours1"].ToString();
                            overTimeList[1].Value4 = dr["TotalOverTimeHours2"].ToString();
                            overTimeList[2].Value4 = dr["TotalOverTimeHours3"].ToString();
                            overTimeList[3].Value4 = dr["TotalOverTimeHours4"].ToString();
                            break;
                        default:
                            overTimeList[0].Value4 = dr["TotalOverTimeHours1"].ToString();
                            overTimeList[1].Value4 = dr["TotalOverTimeHours2"].ToString();
                            overTimeList[2].Value4 = dr["TotalOverTimeHours3"].ToString();
                            overTimeList[3].Value4 = dr["TotalOverTimeHours4"].ToString();
                            overTimeList[4].Value4 = dr["TotalOverTimeHours5"].ToString();
                            break;
                    }

                    Repeater rptOverTimeD = e.Item.FindControl("rptOverTimeD") as Repeater;
                    if (overTimeList.Count == 0)
                    {
                        rptOverTimeD.DataSource = null;
                    }
                    else
                    {
                        rptOverTimeD.DataSource = overTimeList;
                    }
                    rptOverTimeD.DataBind();
                }
            }
        }

        /// <summary>
        /// Search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //Check input
            if (!this.CheckInput(false))
            {
                SetPaging();
                return;
            }
            // Refresh sort header
            this.HeaderGrid.SortDirec = "1";
            this.HeaderGrid.SortField = "1";
            if (sender != null)
            {
                this.InitSort();
            }

            // Refresh load grid
            this.LoadDataGrid(1, this.PagingHeader.NumRowOnPage, this.hdSortField.Value, this.GetSortDirec());

            this.Collapse = "in";
        }
        #endregion

        #region Method
        /// <summary>
        /// Init Data
        /// </summary>
        private void InitData()
        {
            this.Collapse = "in";
            // Default data valide
            this.InitSort();
            InitCombobox();
        }

        private void InitSort()
        {
            this.hdSortField.Value = Models.Constant.SORT_FIELD_DEPT;
            this.hdSortDeptDirec.Value = ((int)SortDirec.ASC).ToString();
            this.hdSortUserDirec.Value = ((int)SortDirec.None).ToString();
        }

        private int GetSortDirec()
        {
            int ret = 1;
            try
            {
                if (this.hdSortField.Value.Equals(Constant.SORT_FIELD_DEPT))
                {
                    ret = int.Parse(this.hdSortDeptDirec.Value);
                }
                else
                {
                    ret = int.Parse(this.hdSortUserDirec.Value);
                }
            }
            catch (Exception)
            {
            }
            return ret;
        }

        private void SetSort(string sortField)
        {
            try 
	        {	        
		        if (sortField.Equals(Constant.SORT_FIELD_DEPT))
                {
                    this.hdSortField.Value = Models.Constant.SORT_FIELD_DEPT;

                    SortDirec bfSortDirec = (SortDirec)(int.Parse(this.hdSortDeptDirec.Value));
                    switch (bfSortDirec)
                    {
                        case SortDirec.None:
                        case SortDirec.DESC:
                            this.hdSortDeptDirec.Value = ((int)SortDirec.ASC).ToString();
                            break;
                        case SortDirec.ASC:
                            this.hdSortDeptDirec.Value = ((int)SortDirec.DESC).ToString();
                            break;
                    }

                    this.hdSortUserDirec.Value = ((int)SortDirec.None).ToString();

                }
                else
                {
                    this.hdSortField.Value = Models.Constant.SORT_FIELD_USER;
                    this.hdSortDeptDirec.Value = ((int)SortDirec.None).ToString();

                    SortDirec bfSortDirec = (SortDirec)(int.Parse(this.hdSortUserDirec.Value));
                    switch (bfSortDirec)
                    {
                        case SortDirec.None:
                        case SortDirec.DESC:
                            this.hdSortUserDirec.Value = ((int)SortDirec.ASC).ToString();
                            break;
                        case SortDirec.ASC:
                            this.hdSortUserDirec.Value = ((int)SortDirec.DESC).ToString();
                            break;
                    }
                }
	        }
	        catch (Exception)
	        {
		        this.InitSort();
	        }
           
        }

        /// <summary>
        /// Set error messsage
        /// </summary>
        /// <param name="ctrlID">Error ControlID</param>
        /// <param name="msgID">Message Id</param>
        /// <param name="args">List argument of messsage</param>
        protected virtual string GetHeadSortText(string sortField)
        {
            string ret = String.Empty;
            SortDirec sortDirec = SortDirec.None;
            if (sortField.Equals(Constant.SORT_FIELD_DEPT))
            {
                ret = "部署";
                sortDirec = (SortDirec)(int.Parse(this.hdSortDeptDirec.Value));
                
            }
            else
            {
                ret = "コード";
                sortDirec = (SortDirec)(int.Parse(this.hdSortUserDirec.Value));
            }
            switch (sortDirec)
            {
                case SortDirec.None:
                    break;
                case SortDirec.ASC:
                    ret += "&nbsp; <span class='glyphicon glyphicon-arrow-up'></span>";
                    break;
                case SortDirec.DESC:
                    ret += "&nbsp; <span class='glyphicon glyphicon-arrow-down'></span>";
                    break;
            }
            return ret;
        }

        /// <summary>
        /// Check input
        /// </summary>
        /// <returns>Valid:true, Invalid:false</returns>
        private bool CheckInput(bool fromCsv)
        {
            DateTime startDateFrom = DateTime.MinValue;
            DateTime startDateTo = DateTime.MinValue;
            T_WorkingCalendar_H cHModel = null;

            using (DB db = new DB())
            {
                T_WorkingCalendar_HService cHser = new T_WorkingCalendar_HService(db);
                cHModel = cHser.GetByID(int.Parse(this.ddlWorkingCalendar.SelectedValue));
            }
            if (fromCsv)
            {
                if (this.rbApprove.Checked)
                {
                    if (this.ddlDateOfServiceFrom.SelectedValue == "-1")
                    {
                        this.SetMessage(this.ddlDateOfServiceFrom.ID, M_Message.MSG_REQUIRE, "勤務年月From");
                    }
                    if (this.ddlDateOfServiceTo.SelectedValue == "-1")
                    {
                        this.SetMessage(this.ddlDateOfServiceTo.ID, M_Message.MSG_REQUIRE, "勤務年月To");
                    }
                    if (this.ddlDateOfServiceFrom.SelectedValue != "-1" && this.ddlDateOfServiceTo.SelectedValue != "-1")
                    {
                        if (!string.IsNullOrEmpty(this.ddlDateOfServiceFrom.SelectedValue))
                        {
                            startDateFrom = DateTime.Parse(this.ddlDateOfServiceFrom.SelectedValue);
                            startDateTo = DateTime.Parse(this.ddlDateOfServiceTo.SelectedValue);
                        }
                        if (startDateFrom != startDateTo)
                        {
                            this.SetMessage(this.ddlDateOfServiceFrom.ID, M_Message.MSG_VALUE_MUST_EQUAL, "勤務年月From", "勤務年月To");
                        }
                    }
                }
                else
                {
                    if (this.dtStartDate.Value == null)
                    {
                        this.SetMessage(this.dtStartDate.ID, M_Message.MSG_REQUIRE, "勤務年月日From");
                    } 
                    else if (this.dtStartDate.Value.Value < cHModel.InitialDate
                        || this.dtStartDate.Value.Value > cHModel.InitialDate.AddMonths(12).AddDays(-1))
                    {
                        this.SetMessage(this.dtStartDate.ID, M_Message.MSG_INCORRECT_FORMAT, "勤務年月日From");
                    }
                    if (this.dtEndDate.Value == null)
                    {
                        this.SetMessage(this.dtEndDate.ID, M_Message.MSG_REQUIRE, "勤務年月日To");
                    }
                    else if (this.dtEndDate.Value.Value < cHModel.InitialDate
                        || this.dtEndDate.Value.Value > cHModel.InitialDate.AddMonths(12).AddDays(-1))
                    {
                        this.SetMessage(this.dtEndDate.ID, M_Message.MSG_INCORRECT_FORMAT, "勤務年月日To");
                    }
                    if (this.dtStartDate.Value != null && this.dtEndDate.Value != null)
                    {
                        startDateFrom = this.dtStartDate.Value.Value;
                        startDateTo = this.dtEndDate.Value.Value;
                        
                            int subMonth = ((startDateFrom.Year - cHModel.InitialDate.Year) * 12) + startDateFrom.Month - cHModel.InitialDate.Month;

                            if (startDateFrom.AddMonths(1).AddDays(-1) != startDateTo
                                || cHModel.InitialDate.AddMonths(subMonth).Day != startDateFrom.Day)
                            {
                                this.SetMessage(this.dtStartDate.ID, M_Message.MSG_CSV_RANGE_DATE);
                            }
                        
                    }
                }
            }
            else
            {
                if (this.rbApprove.Checked)
                {
                    // When FromDay > ToDay
                    if (this.ddlDateOfServiceFrom.SelectedValue != "-1" && this.ddlDateOfServiceTo.SelectedValue != "-1")
                    {

                        if (!string.IsNullOrEmpty(this.ddlDateOfServiceFrom.SelectedValue))
                        {
                            startDateFrom = DateTime.Parse(this.ddlDateOfServiceFrom.SelectedValue);
                            startDateTo = DateTime.Parse(this.ddlDateOfServiceTo.SelectedValue);
                        }
                        if (startDateFrom > startDateTo)
                        {
                            this.SetMessage(this.ddlDateOfServiceFrom.ID, M_Message.MSG_LESS_THAN_EQUAL, "勤務年月From", "勤務年月To");
                        }
                    }
                }
                else
                {

                    if (this.dtStartDate.Value != null)
                    {
                        if (this.dtStartDate.Value.Value < cHModel.InitialDate
                            || this.dtStartDate.Value.Value > cHModel.InitialDate.AddMonths(12).AddDays(-1))
                        {
                            this.SetMessage(this.dtStartDate.ID, M_Message.MSG_INCORRECT_FORMAT, "勤務年月日From");
                        }
                    }
                    if (this.dtEndDate.Value != null)
                    {
                        if (this.dtEndDate.Value.Value < cHModel.InitialDate
                           || this.dtEndDate.Value.Value > cHModel.InitialDate.AddMonths(12).AddDays(-1))
                        {
                            this.SetMessage(this.dtEndDate.ID, M_Message.MSG_INCORRECT_FORMAT, "勤務年月日To");
                        }
                    }
                    // When FromDay > ToDay
                    if (this.dtStartDate.Value != null && this.dtEndDate.Value != null)
                    {
                        startDateFrom = this.dtStartDate.Value.Value;
                        startDateTo = this.dtEndDate.Value.Value;
                        if (startDateFrom > startDateTo)
                        {
                            this.SetMessage(this.dtStartDate.ID, M_Message.MSG_LESS_THAN_EQUAL, "勤務年月日From", "勤務年月日To");
                        }
                    }
                }
            }

            //Check error
            return !base.HaveError;
        }

        /// <summary>
        /// Load Data Gird
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="numOnPage"></param>
        private void LoadDataGrid(int pageIndex, int numOnPage, string sortField, int sortDirec)
        {
            int totalRow = 0;
            int flag = 0;

            // List AttendanceSummary 
            Hashtable pageMenu = new Hashtable();
            IList<AttendanceSummaryInfo> lstAttendanceSummary;
            DataTable AttendanceSummaryListInfo;
            try
            {
                // User Name
                decimal Hour_Date = decimal.MinValue;
                string strHours = string.Empty;
                List<string> lstUserID = this.GetListConditonUserID();
                List<string> lstDeptID = this.GetListConditonDeptID();
                using (DB db = new DB())
                {
                    AttendanceService attendanceService = new AttendanceService(db);

                    if (this.ddlOvertime_Vacation.SelectedValue != "-1")
                    {
                        if (!(this.ddlOvertime_Vacation.SelectedValue.Contains("Vacation") || this.ddlOvertime_Vacation.SelectedValue.Contains("Days")))
                        {
                            if (this.txtHour_Days.Text != string.Empty)
                            {
                                if (this.txtHour_Days.Text.Contains("."))
                                {
                                    string[] strHour_Days = this.txtHour_Days.Text.Split('.');
                                    Hour_Date = (Utilities.CommonUtil.TimeToInt(strHour_Days[0] + ":" + (int.Parse(strHour_Days[1]) * 6).ToString()));
                                }
                                else
                                {
                                    strHours = this.txtHour_Days.Text + ":" + "00";
                                    Hour_Date = (Utilities.CommonUtil.TimeToInt(strHours));
                                }
                            }
                            else
                            {
                                this.ddlOvertime_Vacation.SelectedValue = "-1";
                            }
                        }
                        else
                        {
                            if (this.txtHour_Days.Text != string.Empty)
                            {
                                Hour_Date = decimal.Parse(this.txtHour_Days.Text);
                            }
                            else
                            {
                                this.ddlOvertime_Vacation.SelectedValue = "-1";
                            }
                        }
                    }

                    if (this.rbTotal.Checked == true)
                    {
                        flag = 1;
                        // get total row all
                        totalRow = attendanceService.GetTotalRowAll(this.ddlWorkingCalendar.SelectedValue
                                                                      , lstUserID
                                                                      , this.dtStartDate.Value
                                                                      , this.dtEndDate.Value
                                                                      , lstDeptID
                                                                      , this.ddlOvertime_Vacation.SelectedValue
                                                                      , this.ddlCompare.SelectedValue
                                                                      , Hour_Date);
                        lstAttendanceSummary = attendanceService.GetListAttendanceSummaryAll(this.ddlWorkingCalendar.SelectedValue
                                                                                            , lstUserID
                                                                                            , this.dtStartDate.Value
                                                                                            , this.dtEndDate.Value
                                                                                            , lstDeptID
                                                                                            , this.ddlOvertime_Vacation.SelectedValue
                                                                                            , this.ddlCompare.SelectedValue
                                                                                            , Hour_Date
                                                                                            , pageIndex
                                                                                            , numOnPage
                                                                                            , sortField
                                                                                            , sortDirec);
                                                                                            
                    }
                    else
                    {
                        // get total row Approve
                        totalRow = attendanceService.GetTotalRowApprove(this.ddlWorkingCalendar.SelectedValue
                                                                      , lstUserID
                                                                      , this.ddlDateOfServiceFrom.SelectedValue
                                                                      , this.ddlDateOfServiceTo.SelectedValue
                                                                      , lstDeptID
                                                                      , this.ddlOvertime_Vacation.SelectedValue
                                                                      , this.ddlCompare.SelectedValue
                                                                      , Hour_Date);
                        lstAttendanceSummary = attendanceService.GetListAttendanceSummaryApprove(this.ddlWorkingCalendar.SelectedValue
                                                                                            , lstUserID
                                                                                            , this.ddlDateOfServiceFrom.SelectedValue
                                                                                            , this.ddlDateOfServiceTo.SelectedValue
                                                                                            , lstDeptID
                                                                                            , this.ddlOvertime_Vacation.SelectedValue
                                                                                            , this.ddlCompare.SelectedValue
                                                                                            , Hour_Date
                                                                                            , pageIndex
                                                                                            , numOnPage
                                                                                            , sortField
                                                                                            , sortDirec);
                    }
                }

                if (lstAttendanceSummary.Count == 0)
                {
                    this.rptAttendanceSummaryList.DataSource = null;

                    // Show menu bottom
                    this.isShowButtonMenu = false;

                    pageMenu.Add("CurrentPage", pageIndex);
                    pageMenu.Add("NumberOnPage", numOnPage);
                    pageMenu.Add("RowNumFrom", 0);
                    pageMenu.Add("RowNumTo", 0);
                }
                else
                {
                    // detail
                    AttendanceSummaryListInfo = ConvertListToDataTable(lstAttendanceSummary, flag);

                    // paging header
                    this.PagingHeader.RowNumFrom = int.Parse(lstAttendanceSummary[0].RowNumber.ToString());
                    this.PagingHeader.RowNumTo = int.Parse(lstAttendanceSummary[lstAttendanceSummary.Count - 1].RowNumber.ToString());
                    this.PagingHeader.CurrentPage = pageIndex;

                    // paging footer
                    this.PagingFooter.CurrentPage = pageIndex;
                    this.PagingFooter.NumberOnPage = numOnPage;
                    

                    this.rptAttendanceSummaryList.DataSource = AttendanceSummaryListInfo;

                    // Show menu bottom
                    this.isShowButtonMenu = true;


                    pageMenu.Add("RowNumFrom", this.PagingHeader.RowNumFrom);
                    pageMenu.Add("RowNumTo", this.PagingHeader.RowNumTo);
                    pageMenu.Add("CurrentPage", this.PagingHeader.CurrentPage);
                    pageMenu.Add("NumberOnPage", this.PagingFooter.NumberOnPage);
                    
                }

                pageMenu.Add("TotalRow", totalRow);
                this.pageMenu = pageMenu;

                this.PagingHeader.TotalRow = totalRow;
                this.PagingFooter.TotalRow = totalRow;

                this.rptAttendanceSummaryList.DataBind();
            }
            catch (Exception e)
            {
                Log.Instance.WriteLog(e);
            }
        }

        private List<string> GetListConditonUserID()
        {
            List<string> ret = new List<string>();
            string lstUser1 = string.Empty;
            string lstUser2 = string.Empty;

            if (!this.ddlUser11.SelectedValue.Equals("-1"))
            {
                lstUser1 = this.ddlUser11.SelectedValue + ",";
            }
            if (!this.ddlUser12.SelectedValue.Equals("-1"))
            {
                lstUser1 += this.ddlUser12.SelectedValue + ",";
            }
            if (!this.ddlUser13.SelectedValue.Equals("-1"))
            {
                lstUser1 += this.ddlUser13.SelectedValue + ",";
            }
            if (!this.ddlUser14.SelectedValue.Equals("-1"))
            {
                lstUser1 += this.ddlUser14.SelectedValue + ",";
            }
            ret.Add(lstUser1.Trim(','));

            if (!this.ddlUser21.SelectedValue.Equals("-1"))
            {
                lstUser2 = this.ddlUser21.SelectedValue + ",";
            }
            if (!this.ddlUser22.SelectedValue.Equals("-1"))
            {
                lstUser2 += this.ddlUser22.SelectedValue + ",";
            }
            if (!this.ddlUser23.SelectedValue.Equals("-1"))
            {
                lstUser2 += this.ddlUser23.SelectedValue + ",";
            }
            if (!this.ddlUser24.SelectedValue.Equals("-1"))
            {
                lstUser2 += this.ddlUser24.SelectedValue + ",";
            }

            ret.Add(lstUser2.Trim(','));
            return ret;
        }

        private List<string> GetListConditonDeptID()
        {
            List<string> ret = new List<string>();

            if (!this.ddlDepartment1.SelectedValue.Equals("-1"))
            {
                ret.Add(this.ddlDepartment1.SelectedValue);
            }
            else
            {
                ret.Add(string.Empty);
            }
            if (!this.ddlDepartment2.SelectedValue.Equals("-1"))
            {
                ret.Add(this.ddlDepartment2.SelectedValue);
            }
            else
            {
                ret.Add(string.Empty);
            }
            return ret;
        }

        /// <summary>
        /// ConvertListToDataTable
        /// </summary>
        /// <param name="listAttendanceDetailInfo"></param>
        /// <returns></returns>
        private DataTable ConvertListToDataTable(IList<AttendanceSummaryInfo> AttendanceSummaryInfolist, int flag)
        {
            // RowNumber 
            int RowNumber = 0;
            RowNumber = int.Parse(AttendanceSummaryInfolist[0].RowNumber.ToString());
            bool hasCreateColor = this.HasCreateColor();
            try
            {
                using (DB db = new DB())
                {
                    AttendanceService atdService = new AttendanceService(db);

                    DataTable dataAttendanceDetailInfo = new DataTable();
                    dataAttendanceDetailInfo.Columns.Add("RowNumber", typeof(int));
                    dataAttendanceDetailInfo.Columns.Add("UID", typeof(int));
                    dataAttendanceDetailInfo.Columns.Add("UserCD", typeof(string));
                    dataAttendanceDetailInfo.Columns.Add("UserNm", typeof(string));
                    dataAttendanceDetailInfo.Columns.Add("DepartmentName", typeof(string));
                    dataAttendanceDetailInfo.Columns.Add("WorkingHours", typeof(string));
                    dataAttendanceDetailInfo.Columns.Add("LateHours", typeof(string));
                    dataAttendanceDetailInfo.Columns.Add("EarlyHours", typeof(string));
                    dataAttendanceDetailInfo.Columns.Add("SH_Hours", typeof(string));
                    dataAttendanceDetailInfo.Columns.Add("LH_Hours", typeof(string));

                    dataAttendanceDetailInfo.Columns.Add("numWorkingDays", typeof(string));
                    dataAttendanceDetailInfo.Columns.Add("numLateDays", typeof(string));
                    dataAttendanceDetailInfo.Columns.Add("numEarlyDays", typeof(string));
                    dataAttendanceDetailInfo.Columns.Add("numLH_Days", typeof(string));
                    dataAttendanceDetailInfo.Columns.Add("numSH_Days", typeof(string));

                    dataAttendanceDetailInfo.Columns.Add("OverTimeHours1", typeof(string));
                    dataAttendanceDetailInfo.Columns.Add("OverTimeHours2", typeof(string));
                    dataAttendanceDetailInfo.Columns.Add("OverTimeHours3", typeof(string));
                    dataAttendanceDetailInfo.Columns.Add("OverTimeHours4", typeof(string));
                    dataAttendanceDetailInfo.Columns.Add("OverTimeHours5", typeof(string));

                    dataAttendanceDetailInfo.Columns.Add("SH_OverTimeHours1", typeof(string));
                    dataAttendanceDetailInfo.Columns.Add("SH_OverTimeHours2", typeof(string));
                    dataAttendanceDetailInfo.Columns.Add("SH_OverTimeHours3", typeof(string));
                    dataAttendanceDetailInfo.Columns.Add("SH_OverTimeHours4", typeof(string));
                    dataAttendanceDetailInfo.Columns.Add("SH_OverTimeHours5", typeof(string));

                    dataAttendanceDetailInfo.Columns.Add("LH_OverTimeHours1", typeof(string));
                    dataAttendanceDetailInfo.Columns.Add("LH_OverTimeHours2", typeof(string));
                    dataAttendanceDetailInfo.Columns.Add("LH_OverTimeHours3", typeof(string));
                    dataAttendanceDetailInfo.Columns.Add("LH_OverTimeHours4", typeof(string));
                    dataAttendanceDetailInfo.Columns.Add("LH_OverTimeHours5", typeof(string));

                    dataAttendanceDetailInfo.Columns.Add("TotalOverTimeHours1", typeof(string));
                    dataAttendanceDetailInfo.Columns.Add("TotalOverTimeHours2", typeof(string));
                    dataAttendanceDetailInfo.Columns.Add("TotalOverTimeHours3", typeof(string));
                    dataAttendanceDetailInfo.Columns.Add("TotalOverTimeHours4", typeof(string));
                    dataAttendanceDetailInfo.Columns.Add("TotalOverTimeHours5", typeof(string));

                    dataAttendanceDetailInfo.Columns.Add("TotalOverTimeHours", typeof(string));
                    dataAttendanceDetailInfo.Columns.Add("TotalWorkingHours", typeof(string));
                    dataAttendanceDetailInfo.Columns.Add("Flag", typeof(int));
                    dataAttendanceDetailInfo.Columns.Add("tr-class", typeof(string));

                    // RowIndex DataTable
                    int rowIndex = 0;
                    foreach (AttendanceSummaryInfo item in AttendanceSummaryInfolist)
                    {
                        DataRow dr = dataAttendanceDetailInfo.NewRow();
                        //dr["RowNumber"] = rowIndex + 1;
                        dr["RowNumber"] = RowNumber;
                        dr["UID"] = item.UID;

                        dr["UserCD"] = Utilities.EditDataUtil.ToFixCodeShow(item.UserCD, M_User.MAX_USER_CODE_SHOW); ;
                        dr["UserNm"] = item.UserName1;
                        dr["DepartmentName"] = item.DepartmentName;

                        dr["WorkingHours"] = item.WorkingHours;
                        dr["LateHours"] = item.LateHours;
                        dr["EarlyHours"] = item.EarlyHours;
                        dr["SH_Hours"] = item.SH_Hours;
                        dr["LH_Hours"] = item.LH_Hours;

                        dr["numWorkingDays"] = item.NumWorkingDays.ToString() != "0" ? item.NumWorkingDays.ToString("#.0") : "&nbsp;";
                        dr["numLateDays"] = item.NumLateDays.ToString() != "0" ? item.NumLateDays.ToString("#.0") : "&nbsp;";
                        dr["numEarlyDays"] = item.NumEarlyDays.ToString() != "0" ? item.NumEarlyDays.ToString("#.0") : "&nbsp;";
                        dr["numLH_Days"] = item.NumLH_Days.ToString() != "0" ? item.NumLH_Days.ToString("#.0") : "&nbsp;";
                        dr["numSH_Days"] = item.NumSH_Days.ToString() != "0" ? item.NumSH_Days.ToString("#.0") : "&nbsp;";

                        dr["TotalOverTimeHours1"] = item.TotalOverTimeHours1.ToString() != "0" ? Utilities.CommonUtil.IntToTime(item.TotalOverTimeHours1, false) : "&nbsp;";
                        dr["TotalOverTimeHours2"] = item.TotalOverTimeHours2.ToString() != "0" ? Utilities.CommonUtil.IntToTime(item.TotalOverTimeHours2, false) : "&nbsp;";
                        dr["TotalOverTimeHours3"] = item.TotalOverTimeHours3.ToString() != "0" ? Utilities.CommonUtil.IntToTime(item.TotalOverTimeHours3, false) : "&nbsp;";
                        dr["TotalOverTimeHours4"] = item.TotalOverTimeHours4.ToString() != "0" ? Utilities.CommonUtil.IntToTime(item.TotalOverTimeHours4, false) : "&nbsp;";
                        dr["TotalOverTimeHours5"] = item.TotalOverTimeHours5.ToString() != "0" ? Utilities.CommonUtil.IntToTime(item.TotalOverTimeHours5, false) : "&nbsp;";

                        dr["TotalOverTimeHours"] = item.TotalOverTimeHours;
                        dr["TotalWorkingHours"] = item.TotalWorkingHours;
                        dr["Flag"] = flag;
                        if (hasCreateColor)
                        {
                            int count = atdService.checkAttendanceSubmit(item.UID, this.dtStartDate.Value.Value.Date, this.dtEndDate.Value.Value.Date);
                            if (count > 0)
                            {
                                dr["tr-class"] = "tr-normal";
                            }
                            else
                            {
                                dr["tr-class"] = "danger";
                            }
                        }
                        else
                        {
                            dr["tr-class"] = "tr-normal"; 
                        }
                        dataAttendanceDetailInfo.Rows.Add(dr);
                        rowIndex++;
                        RowNumber++;
                    }
                    dataAttendanceDetailInfo.AcceptChanges();

                    DataView dataAttendanceDetailInfoView = new DataView(dataAttendanceDetailInfo);

                    dataAttendanceDetailInfo = dataAttendanceDetailInfoView.ToTable();
                    return dataAttendanceDetailInfo;
                }

            }
            catch (Exception ex)
            {
                Log.Instance.WriteLog(ex);
                return null;
            }
        }

        /// <summary>
        /// has create unsubmit data
        /// </summary>
        /// <returns></returns>
        private bool HasCreateColor()
        {
            if (this.rbTotal.Checked && this.dtStartDate.Value != null && this.dtEndDate.Value != null)
            {
                using (DB db = new DB())
                {
                    T_WorkingCalendar_HService calendarHSer = new T_WorkingCalendar_HService(db);
                    var calendarH = calendarHSer.GetByID(int.Parse(ddlWorkingCalendar.SelectedValue));
                    for (int i = 0; i < 12; i++)
                    {
                        var fromDate = calendarH.InitialDate.AddMonths(i);
                        var toDate = calendarH.InitialDate.AddMonths(i + 1).AddDays(-1);
                        if (this.dtStartDate.Value.Value.Date == fromDate.Date && this.dtEndDate.Value.Value.Date == toDate.Date) 
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Init Combobox
        /// </summary>
        private void InitCombobox()
        {
            //Load WorkingCalendar combobox data
            this.LoadWorkingCalendarComboboxData();

            //Load Date combobox datas
            this.LoadDdlDateOfServiceCombobox(true);

            //Load Department combobox datas
            this.LoadDepartmentComboboxData();

            //Load User combobox data
            this.LoadUserComboboxData(ddlDepartment1);
            this.LoadUserComboboxData(ddlDepartment2);

            //Load Overtime combobox data
            LoadOvertimeComboboxData();

            //Load Overtime combobox data
            LoadCompareComboboxData();

        }

        /// <summary>
        /// Load Department Combobox data
        /// </summary>
        private void LoadWorkingCalendarComboboxData()
        {
            using (DB db = new DB())
            {
                IList<DropDownModel> workingCalendarList;
                T_WorkingCalendar_HService workingCalendarService = new T_WorkingCalendar_HService(db);
                string defaultVal = string.Empty;
                workingCalendarList = workingCalendarService.GetWorkingCalendarCbbData(ref defaultVal);
                ddlWorkingCalendar.Items.Clear();

                if (workingCalendarList.Count > 0)
                {
                    hdCalendarDefault.Value = defaultVal;
                    ddlWorkingCalendar.DataSource = workingCalendarList;
                    ddlWorkingCalendar.DataValueField = "Value";
                    ddlWorkingCalendar.DataTextField = "DisplayName";
                    ddlWorkingCalendar.SelectedValue = hdCalendarDefault.Value;
                }
                else
                {
                    ddlWorkingCalendar.DataSource = null;
                    ddlWorkingCalendar.DataBind();
                }

                ddlWorkingCalendar.DataBind();
            }
        }

        /// <summary>
        /// Load Department Combobox data
        /// </summary>
        private void LoadDdlDateOfServiceCombobox(bool isInit)
        {
            if (string.IsNullOrEmpty(ddlWorkingCalendar.SelectedValue))
            {
                return;
            }
            using (DB db = new DB())
            {
                IList<DropDownModel> DateOfServiceList;
                T_WorkingCalendar_HService calendarHSer = new T_WorkingCalendar_HService(db);
                string defaultVal = string.Empty;
                DateOfServiceList = calendarHSer.GetDateOfServiceComboboxByWorkingCalendarHID(int.Parse(ddlWorkingCalendar.SelectedValue), ref defaultVal);
                ddlDateOfServiceTo.Items.Clear();
                ddlDateOfServiceFrom.Items.Clear();

                ddlDateOfServiceTo.DataSource = setDataTableCombobox(DateOfServiceList);
                ddlDateOfServiceTo.DataValueField = "Value";
                ddlDateOfServiceTo.DataTextField = "DisplayName";

                ddlDateOfServiceTo.DataBind();

                ddlDateOfServiceFrom.DataSource = setDataTableCombobox(DateOfServiceList);
                ddlDateOfServiceFrom.DataValueField = "Value";
                ddlDateOfServiceFrom.DataTextField = "DisplayName";

                ddlDateOfServiceFrom.DataBind();

                if (isInit)
                {
                    this.dtStartDate.Value = CommonUtil.ParseDate(defaultVal, Constants.FMT_DATE_EN);
                    this.dtEndDate.Value = dtStartDate.Value.Value.AddMonths(1).AddDays(-1);
                }
                else
                {
                    this.dtStartDate.Value = null;
                    this.dtEndDate.Value = null;
                }
            }
        }

        /// <summary>
        /// Load Department Combobox data
        /// </summary>
        private void LoadDepartmentComboboxData()
        {
            IList<DropDownModel> departmentList;
            using (DB db = new DB())
            {
                DepartmentService departmentService = new DepartmentService(db);
                departmentList = departmentService.GetDepartmentCbbData(int.Parse(this.ddlWorkingCalendar.SelectedValue));
            }

            DataTable dtDeptData = setDataTableCombobox(departmentList);

            SetDataToCombo(this.ddlDepartment1, dtDeptData);
            SetDataToCombo(this.ddlDepartment2, dtDeptData);
        }

        /// <summary>
        /// Load User combobox data
        /// </summary>
        private void LoadUserComboboxData(DropDownList ddlDept)
        {
            IList<DropDownModel> userList;
            using (DB db = new DB())
            {
                UserService userService = new UserService(db);
                userList = userService.GetCbbUserDataByDepartmentID(int.Parse(ddlDept.SelectedValue), int.Parse(ddlWorkingCalendar.SelectedValue));
            }

            DataTable dtUserData = setDataTableCombobox(userList);

            if (ddlDept.ID.Equals(this.ddlDepartment1.ID))
            {
                SetDataToCombo(this.ddlUser11, dtUserData);
                SetDataToCombo(this.ddlUser12, dtUserData);
                SetDataToCombo(this.ddlUser13, dtUserData);
                SetDataToCombo(this.ddlUser14, dtUserData);
                UserList1 = userList;
                LoadUserComboboxAttribute(this.ddlUser11, UserList1);
                LoadUserComboboxAttribute(this.ddlUser12, UserList1);
                LoadUserComboboxAttribute(this.ddlUser13, UserList1);
                LoadUserComboboxAttribute(this.ddlUser14, UserList1);

            }
            else
            {
                SetDataToCombo(this.ddlUser21, dtUserData);
                SetDataToCombo(this.ddlUser22, dtUserData);
                SetDataToCombo(this.ddlUser23, dtUserData);
                SetDataToCombo(this.ddlUser24, dtUserData);
                UserList2 = userList;
                LoadUserComboboxAttribute(this.ddlUser21, UserList2);
                LoadUserComboboxAttribute(this.ddlUser22, UserList2);
                LoadUserComboboxAttribute(this.ddlUser23, UserList2);
                LoadUserComboboxAttribute(this.ddlUser24, UserList2);

            }
        }

        /// <summary>
        /// Load user combobox attribute
        /// </summary>
        private void LoadUserComboboxAttribute(DropDownList ddlUser, IList<DropDownModel> userList)
        {
            if (userList != null)
            {
                int index = 0;
                foreach (ListItem item in ddlUser.Items)
                {
                    if (item.Value != "-1")
                    {
                        if (userList[index].Status != "0")
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
                        continue;
                    }
                    index++;
                }
                if (ddlUser.SelectedIndex > 0)
                {
                    if (userList[ddlUser.SelectedIndex - 1].Value != "-1" && userList[ddlUser.SelectedIndex - 1].Status != "0")
                    {
                        ddlUser.CssClass = "form-control input-sm bg-danger";
                    }
                    else
                    {
                        ddlUser.CssClass = "form-control input-sm";
                    }
                }

            }
        }

        /// <summary>
        /// Set datasource to combobox
        /// </summary>
        /// <param name="ddlControl"></param>
        /// <param name="dtData"></param>
        private void SetDataToCombo(DropDownList ddlControl, DataTable dtData)
        {
            ddlControl.Items.Clear();
            ddlControl.DataSource = dtData;
            ddlControl.DataValueField = "Value";
            ddlControl.DataTextField = "DisplayName";
            ddlControl.DataBind();
        }

        /// <summary>
        /// Load Overtime combobox data
        /// </summary>
        private void LoadOvertimeComboboxData()
        {
            using (DB db = new DB())
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Value", typeof(string));
                dt.Columns.Add("DisplayName", typeof(string));

                int index = 0;
                DataRow drow = dt.NewRow();
                dt.Rows.Add(drow);
                dt.Rows[index].SetField("Value", "-1");
                dt.Rows[index].SetField("DisplayName", "-----------");

                index = index + 1;
                DataRow drow1 = dt.NewRow();
                dt.Rows.Add(drow1);
                dt.Rows[index].SetField("Value", "TotalOverTimeHours");
                dt.Rows[index].SetField("DisplayName", "総残業時間");

                index = index + 1;
                DataRow drow2 = dt.NewRow();
                dt.Rows.Add(drow2);
                dt.Rows[index].SetField("Value", "TotalWorkingHours");
                dt.Rows[index].SetField("DisplayName", "総労働時間");

                IList<DropDownModel> overtimeList;
                Config_DService ConfigDSer = new Config_DService(db);
                overtimeList = ConfigDSer.GetListByConfigCdComboboxData(M_Config_H.CONFIG_CD_OVER_TIME_TYPE);

                index = index + 1;
                foreach (var item in overtimeList)
                {
                    DataRow drow3 = dt.NewRow();
                    dt.Rows.Add(drow3);
                    string valueCombobox ="OverTimeHours" + "_" + item.Value;
                    dt.Rows[index].SetField("Value", valueCombobox);
                    dt.Rows[index].SetField("DisplayName", item.DisplayName);
                    index = index + 1;
                }
                DataRow drow4 = dt.NewRow();
                dt.Rows.Add(drow4);
                dt.Rows[index].SetField("Value", "WorkingDays");
                dt.Rows[index].SetField("DisplayName", "出勤日数");

                index = index + 1;
                DataRow drow5 = dt.NewRow();
                dt.Rows.Add(drow5);
                dt.Rows[index].SetField("Value", "LateDays");
                dt.Rows[index].SetField("DisplayName", "遅刻日数");

                index = index + 1;
                DataRow drow6 = dt.NewRow();
                dt.Rows.Add(drow6);
                dt.Rows[index].SetField("Value", "EarlyDays");
                dt.Rows[index].SetField("DisplayName", "早退日数");

                index = index + 1;
                DataRow drow7 = dt.NewRow();
                dt.Rows.Add(drow7);
                dt.Rows[index].SetField("Value", "SH_Days");
                dt.Rows[index].SetField("DisplayName", "所定休日日数");

                index = index + 1;
                DataRow drow8 = dt.NewRow();
                dt.Rows.Add(drow8);
                dt.Rows[index].SetField("Value", "LH_Days");
                dt.Rows[index].SetField("DisplayName", "法定休日日数");


                IList<DropDownModel> lstVacation;
                lstVacation = ConfigDSer.GetListByConfigCdComboboxData(M_Config_H.CONFIG_CD_VACATION_TYPE);

                index = index + 1;
                foreach (var item in lstVacation)
                {
                    DataRow drow9 = dt.NewRow();
                    dt.Rows.Add(drow9);
                    string valueCombobox ="Vacation" + "_" + item.Value;
                    dt.Rows[index].SetField("Value", valueCombobox);
                    dt.Rows[index].SetField("DisplayName", item.DisplayName);
                    index = index + 1;
                }

                ddlOvertime_Vacation.Items.Clear();
                ddlOvertime_Vacation.DataSource = dt;
                ddlOvertime_Vacation.DataValueField = "Value";
                ddlOvertime_Vacation.DataTextField = "DisplayName";
                ddlOvertime_Vacation.DataBind();
            }
        }

        /// <summary>
        /// Load Compare combobox data
        /// </summary>
        private void LoadCompareComboboxData()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Value", typeof(string));
            dt.Columns.Add("DisplayName", typeof(string));
            int index = 0;
            
            DataRow drow = dt.NewRow();
            dt.Rows.Add(drow);
            dt.Rows[index].SetField("Value", 0);
            dt.Rows[index].SetField("DisplayName", "以上");

            index = index + 1;
            DataRow drow2 = dt.NewRow();
            dt.Rows.Add(drow2);
            dt.Rows[index].SetField("Value", 1);
            dt.Rows[index].SetField("DisplayName", "未満");

            ddlCompare.DataSource = dt;
            ddlCompare.DataValueField = "Value";
            ddlCompare.DataTextField = "DisplayName";
            ddlCompare.DataBind();
        }

        /// <summary>
        /// Set Data Table Combobox
        /// </summary>
        /// <param name="dataList"></param>
        /// <returns></returns>
        private DataTable setDataTableCombobox(IList<DropDownModel> dataList)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Value", typeof(string));
            dt.Columns.Add("DisplayName", typeof(string));
            DataRow drow = dt.NewRow();
            dt.Rows.Add(drow);
            dt.Rows[0].SetField("Value", -1);
            dt.Rows[0].SetField("DisplayName", "-----------");
            int index = 1;
            foreach (var item in dataList)
            {
                DataRow drowItem = dt.NewRow();
                dt.Rows.Add(drowItem);

                dt.Rows[index].SetField("Value", item.Value);
                dt.Rows[index].SetField("DisplayName", item.DisplayName);
                index = index + 1;
            }

            return dt;
        }

        /// <summary>
        /// ddlWorkingCalendar_SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlWorkingCalendar_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadDdlDateOfServiceCombobox(false);
            this.LoadDepartmentComboboxData();
            this.LoadUserComboboxData(this.ddlDepartment1);
            this.LoadUserComboboxData(this.ddlDepartment2);
            // paging header
            this.PagingHeader.RowNumFrom = (int)pageMenu["RowNumFrom"];
            this.PagingHeader.RowNumTo = (int)pageMenu["RowNumTo"];
            this.PagingHeader.CurrentPage = (int)pageMenu["CurrentPage"];
            this.PagingHeader.TotalRow = (int)pageMenu["TotalRow"];

            // paging footer
            this.PagingFooter.CurrentPage = (int)pageMenu["CurrentPage"];
            this.PagingFooter.NumberOnPage = (int)pageMenu["NumberOnPage"];
            this.PagingFooter.TotalRow = (int)pageMenu["TotalRow"];

            this.Collapse = "in";
        }

        /// <summary>
        /// ddlDepartment_SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadUserComboboxData((DropDownList)sender);
            
            //set paging
            SetPaging();
            this.Collapse = "in";
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
        /// InitOverTime
        /// </summary>
        private void getVacationConfig(Repeater rpChildContentRested, string constants, DateTime startDate, DateTime endStart, int uId, int flag)
        {
            DataTable VacationConfigInfo;
            using (DB db = new DB())
            {
                Config_DService config_DService = new Config_DService(db);
                AttendanceService AttendanceSer = new AttendanceService(db); 

                IList<VacationDateInFoByAttendanceSummary> configDList;

                if (flag == 1)
                {
                    // For All
                    configDList = config_DService.GetListVacationDateForAttendanceSummary(constants, startDate, endStart, uId);
                }
                else
                {
                    // For Approve
                    configDList = AttendanceSer.GetListVacationDateForAttendanceSummary(constants, startDate, endStart, uId);
                }

                if (configDList.Count > 0)
                {
                    VacationConfigInfo = ConvertConfigDListToDataTable(configDList);
                    rpChildContentRested.DataSource = VacationConfigInfo;
                }
                else
                {
                    rpChildContentRested.DataSource = null;
                }
                rpChildContentRested.DataBind();
            }
        }

        /// <summary>
        /// setHeaderOverTime
        /// </summary>
        /// <param name="rpChildContentRested"></param>
        private void setHeaderOverTime(Repeater rpChildContentRested)
        {
            using (DB db = new DB())
            {
                Config_DService config_DService = new Config_DService(db);

                IList<M_Config_D> overTimeList;
                overTimeList = config_DService.GetListByConfigCd(M_Config_H.CONFIG_CD_OVER_TIME_TYPE);

                if (overTimeList.Count > 0)
                {
                    rpChildContentRested.DataSource = overTimeList;

                }
                else
                {
                    rpChildContentRested.DataSource = null;
                }
                rpChildContentRested.DataBind();

            }
        }
        /// <summary>
        /// ConvertConfigDListToDataTable
        /// </summary>
        /// <param name="VacationDateInFoByAttendanceSummary"></param>
        /// <returns></returns>
        private DataTable ConvertConfigDListToDataTable(IList<VacationDateInFoByAttendanceSummary> vacationDateInFoByAttendanceSummary)
        {
            try
            {
                DataTable dataVacationDDetailInfo = new DataTable();
                dataVacationDDetailInfo.Columns.Add("Value1", typeof(int));
                dataVacationDDetailInfo.Columns.Add("Value3", typeof(string));
                dataVacationDDetailInfo.Columns.Add("VacationDate", typeof(string));

                foreach (VacationDateInFoByAttendanceSummary item in vacationDateInFoByAttendanceSummary)
                {
                    DataRow dr = dataVacationDDetailInfo.NewRow();

                    dr["Value1"] = item.Value1;
                    dr["Value3"] = item.Value3;
                    dr["VacationDate"] = item.VacationDate != "0.0" ? item.VacationDate : "&nbsp;";
                    
                    dataVacationDDetailInfo.Rows.Add(dr);
                }

                dataVacationDDetailInfo.AcceptChanges();
                DataView dataAttendanceDetailInfoView = new DataView(dataVacationDDetailInfo);
                dataVacationDDetailInfo = dataAttendanceDetailInfoView.ToTable();

                return dataVacationDDetailInfo;
            }
            catch (Exception ex)
            {
                Log.Instance.WriteLog(ex);
                return null;
            }
        }

        /// <summary>
        /// Show message question CreateCSV Errors
        /// </summary>
        /// <param name="messageID">MessageID</param>
        /// <param name="defaultButton">Default Button</param>
        protected void ShowMessageCreateCSVErrors(string messageID, DefaultButton defaultButton, params string[] args)
        {
            this.ErrorCSV = false;
            //Get Message
            M_Message mess = (M_Message)this.Messages[messageID];
            HtmlGenericControl questionMessage = (HtmlGenericControl)this.Master.FindControl("questionMessage");
            questionMessage.InnerHtml = "<p>" + " " + string.Format(mess.Message3, args) + "</p>";

            this.IsShowQuestion = true;

            LinkButton btnNo = (LinkButton)this.Master.FindControl("btnNo");
            btnNo.Visible = false;
            if (defaultButton == Models.DefaultButton.Yes)
            {
                this.DefaultButton = "#btnYes";
            }
            else
            {

                this.DefaultButton = "#btnNo";

            }
        }

        protected void btCSVError(object sender, EventArgs e)
        {
            SetPaging();
            CheckAuthority();
        }

        #endregion

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
        }

        #region Event Excel

        /// <summary>
        /// btnExcel Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcel_Click(object sender, CommandEventArgs e)
        {
            //Check input
            if (!this.CheckInput(false))
            {
                SetPaging();
                return;
            }

            //call Search_click event
            btnSearch_Click(null, null);

            int totalRow = 0;
            string strExtractionConditions = string.Empty;
            string strApprovalState = string.Empty;
            // flag check all or approve. 
            // 0: Approve, 1: All 
            int flag = 0;

            bool hasCreateColor = this.HasCreateColor();

            try
            {
                List<string> lstUserId = this.GetListConditonUserID();
                List<string> lstDeptId = this.GetListConditonDeptID();
                decimal Hour_Date = decimal.MinValue;
                string strHours = string.Empty;
                using (DB db = new DB())
                {
                    AttendanceService attendanceService = new AttendanceService(db);

                    if (this.ddlOvertime_Vacation.SelectedValue != "-1")
                    {
                        if (!(this.ddlOvertime_Vacation.SelectedValue.Contains("Vacation") || this.ddlOvertime_Vacation.SelectedValue.Contains("Days")))
                        {
                            if (this.txtHour_Days.Text != string.Empty)
                            {
                                if (this.txtHour_Days.Text.Contains("."))
                                {
                                    string[] strHour_Days = this.txtHour_Days.Text.Split('.');
                                    string strHor_Days = strHour_Days[0] + ":" + (int.Parse(strHour_Days[1]) * 6).ToString();
                                    Hour_Date = (Utilities.CommonUtil.TimeToInt(strHor_Days));
                                    strExtractionConditions = ddlOvertime_Vacation.SelectedItem.Text + "　が　" + strHor_Days + "　時間　" + ddlCompare.SelectedItem.Text;
                                }
                                else
                                {
                                    strHours = this.txtHour_Days.Text + ":" + "00";
                                    Hour_Date = (Utilities.CommonUtil.TimeToInt(strHours));
                                    strExtractionConditions = ddlOvertime_Vacation.SelectedItem.Text + "　が　" + strHours + "　時間　" + ddlCompare.SelectedItem.Text;

                                }
                            }
                            else
                            {
                                this.ddlOvertime_Vacation.SelectedValue = "-1";
                                strExtractionConditions = string.Empty;
                            }
                        }
                        else
                        {
                            if (this.txtHour_Days.Text != string.Empty)
                            {
                                Hour_Date = decimal.Parse(this.txtHour_Days.Text);
                                strExtractionConditions = ddlOvertime_Vacation.SelectedItem.Text + "　が　" + this.txtHour_Days.Text + "　日数　" + ddlCompare.SelectedItem.Text;
                            }
                            else
                            {
                                this.ddlOvertime_Vacation.SelectedValue = "-1";
                                strExtractionConditions = string.Empty;
                            }
                        }
                    }

                    if (this.rbTotal.Checked == true)
                    {
                        // get total row All
                        totalRow = attendanceService.GetTotalRowAll(this.ddlWorkingCalendar.SelectedValue
                                                                      , lstUserId
                                                                      , this.dtStartDate.Value
                                                                      , this.dtEndDate.Value
                                                                      , lstDeptId
                                                                      , this.ddlOvertime_Vacation.SelectedValue
                                                                      , this.ddlCompare.SelectedValue
                                                                      , Hour_Date);

                    }
                    else
                    {
                        // get total row Approve
                        totalRow = attendanceService.GetTotalRowApprove(this.ddlWorkingCalendar.SelectedValue
                                                                      , lstUserId
                                                                      , this.ddlDateOfServiceFrom.SelectedValue
                                                                      , this.ddlDateOfServiceTo.SelectedValue
                                                                      , lstDeptId
                                                                      , this.ddlOvertime_Vacation.SelectedValue
                                                                      , this.ddlCompare.SelectedValue
                                                                      , Hour_Date);
                    }

                    if (totalRow == 0)
                    {
                        return;
                    }
                    else
                    {
                        // AttendanceSummaryListExcelModal
                        AttendanceSummaryListExcelModal attendanceSummaryListExcelModal = new AttendanceSummaryListExcelModal();
                        IList<AttendanceSummaryInfo> lstAttendanceSummaryInfo;

                        //set date for attendanceSummaryListExcelModal
                        attendanceSummaryListExcelModal.CalendarNm = ddlWorkingCalendar.SelectedItem.Text;

                        attendanceSummaryListExcelModal.DepartmentNm = string.Format("{0}{1}",
                                                                        ddlDepartment1.SelectedValue == "-1" ? "" : ddlDepartment1.SelectedItem.Text + "、",
                                                                        ddlDepartment2.SelectedValue == "-1" ? "" : ddlDepartment2.SelectedItem.Text
                                                                        ).Trim('、');

                        attendanceSummaryListExcelModal.UserNm = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                                                                                ddlUser11.SelectedValue == "-1" ? "" : ddlUser11.SelectedItem.Text + "、",
                                                                                ddlUser12.SelectedValue == "-1" ? "" : ddlUser12.SelectedItem.Text + "、",
                                                                                ddlUser13.SelectedValue == "-1" ? "" : ddlUser13.SelectedItem.Text + "、",
                                                                                ddlUser14.SelectedValue == "-1" ? "" : ddlUser14.SelectedItem.Text + "、",
                                                                                ddlUser21.SelectedValue == "-1" ? "" : ddlUser21.SelectedItem.Text + "、",
                                                                                ddlUser22.SelectedValue == "-1" ? "" : ddlUser22.SelectedItem.Text + "、",
                                                                                ddlUser23.SelectedValue == "-1" ? "" : ddlUser23.SelectedItem.Text + "、",
                                                                                ddlUser24.SelectedValue == "-1" ? "" : ddlUser24.SelectedItem.Text
                                                                                ).Trim('、');

                        // get data detail
                        if (this.rbTotal.Checked == true)
                        {
                            flag = 1;
                            strApprovalState = "全データ（未承認含む）";
                            lstAttendanceSummaryInfo = attendanceService.GetListAttendanceSummaryAll(this.ddlWorkingCalendar.SelectedValue
                                                                                            , lstUserId
                                                                                            , this.dtStartDate.Value
                                                                                            , this.dtEndDate.Value
                                                                                            , lstDeptId
                                                                                            , this.ddlOvertime_Vacation.SelectedValue
                                                                                            , this.ddlCompare.SelectedValue
                                                                                            , Hour_Date
                                                                                            , -1
                                                                                            , -1
                                                                                            , this.hdSortField.Value
                                                                                            , this.GetSortDirec());

                        }
                        else
                        {
                            flag = 0;
                            strApprovalState = "承認済データのみ";
                            lstAttendanceSummaryInfo = attendanceService.GetListAttendanceSummaryApprove(this.ddlWorkingCalendar.SelectedValue
                                                                                                , lstUserId
                                                                                                , this.ddlDateOfServiceFrom.SelectedValue
                                                                                                , this.ddlDateOfServiceTo.SelectedValue
                                                                                                , lstDeptId
                                                                                                , this.ddlOvertime_Vacation.SelectedValue
                                                                                                , this.ddlCompare.SelectedValue
                                                                                                , Hour_Date
                                                                                                , -1
                                                                                                , -1
                                                                                                , this.hdSortField.Value
                                                                                                , this.GetSortDirec());
                        }

                        attendanceSummaryListExcelModal.ApprovalState = strApprovalState;
                        attendanceSummaryListExcelModal.ExtractionConditions = strExtractionConditions;

                        DateTime startDate;
                        DateTime endDate;
                        string strStartDate = string.Empty;
                        string strEndDate = string.Empty;

                        T_WorkingCalendar_H tWorkingCalendarH = new T_WorkingCalendar_H();
                        T_WorkingCalendar_HService tWorkingCalendarHSer = new T_WorkingCalendar_HService(db);
                        tWorkingCalendarH = tWorkingCalendarHSer.GetByID(int.Parse(ddlWorkingCalendar.SelectedValue));

                        startDate = tWorkingCalendarH.InitialDate;
                        endDate = startDate.AddMonths(12).AddDays(-1);

                        if (this.rbTotal.Checked == true)
                        {
                            if (this.dtStartDate.Value != null)
                            {
                                startDate = this.dtStartDate.Value.Value;
                            }

                            if (this.dtEndDate.Value != null)
                            {
                                endDate = this.dtEndDate.Value.Value;
                            }

                            strStartDate = startDate.ToString(Constants.FMT_DATE_EN);
                            strEndDate = endDate.ToString(Constants.FMT_DATE_EN);
                        }
                        else
                        {
                            strStartDate = ddlDateOfServiceFrom.Items[1].Text;
                            strEndDate = ddlDateOfServiceTo.Items[ddlDateOfServiceTo.Items.Count - 1].Text;

                            if (this.ddlDateOfServiceFrom.SelectedValue != "-1")
                            {
                                startDate = DateTime.Parse(ddlDateOfServiceFrom.SelectedValue);
                                strStartDate = ddlDateOfServiceFrom.SelectedItem.Text;
                            }

                            if (this.ddlDateOfServiceTo.SelectedValue != "-1")
                            {
                                endDate = (DateTime.Parse(ddlDateOfServiceTo.SelectedValue)).AddMonths(1).AddDays(-1);
                                strEndDate = ddlDateOfServiceTo.SelectedItem.Text;
                            }
                        }

                        attendanceSummaryListExcelModal.FromDate = strStartDate;
                        attendanceSummaryListExcelModal.ToDate = strEndDate;

                        // Service Config
                        Config_DService config_DService = new Config_DService(db);

                        //set Vacation Header
                        IList<M_Config_D> vacationHeadersList;
                        vacationHeadersList = config_DService.GetListByConfigCd(M_Config_H.CONFIG_CD_VACATION_TYPE);
                        if (vacationHeadersList.Count > 0)
                        {
                            attendanceSummaryListExcelModal.VacationHeaderList = vacationHeadersList;
                        }

                        //set OverTime Header
                        IList<M_Config_D> overTimeHeadersList;
                        overTimeHeadersList = config_DService.GetListByConfigCd(M_Config_H.CONFIG_CD_OVER_TIME_TYPE);

                        if (overTimeHeadersList.Count > 0)
                        {
                            attendanceSummaryListExcelModal.OverTimeHeaderList = overTimeHeadersList;
                        }

                        foreach (var item in lstAttendanceSummaryInfo)
                        {
                            // set overTime detail
                            IList<M_Config_D> overTimeDetailList;
                            overTimeDetailList = config_DService.GetListByConfigCd(M_Config_H.CONFIG_CD_OVER_TIME_TYPE);

                            switch (overTimeDetailList.Count)
                            {
                                case 0:
                                    break;
                                case 1:
                                    overTimeDetailList[0].Value4 = item.TotalOverTimeHours1.ToString();
                                    break;
                                case 2:
                                    overTimeDetailList[0].Value4 = item.TotalOverTimeHours1.ToString();
                                    overTimeDetailList[1].Value4 = item.TotalOverTimeHours2.ToString();
                                    break;
                                case 3:
                                    overTimeDetailList[0].Value4 = item.TotalOverTimeHours1.ToString();
                                    overTimeDetailList[1].Value4 = item.TotalOverTimeHours2.ToString();
                                    overTimeDetailList[2].Value4 = item.TotalOverTimeHours3.ToString();
                                    break;
                                case 4:
                                    overTimeDetailList[0].Value4 = item.TotalOverTimeHours1.ToString();
                                    overTimeDetailList[1].Value4 = item.TotalOverTimeHours2.ToString();
                                    overTimeDetailList[2].Value4 = item.TotalOverTimeHours3.ToString();
                                    overTimeDetailList[3].Value4 = item.TotalOverTimeHours4.ToString();
                                    break;
                                default:
                                    overTimeDetailList[0].Value4 = item.TotalOverTimeHours1.ToString();
                                    overTimeDetailList[1].Value4 = item.TotalOverTimeHours2.ToString();
                                    overTimeDetailList[2].Value4 = item.TotalOverTimeHours3.ToString();
                                    overTimeDetailList[3].Value4 = item.TotalOverTimeHours4.ToString();
                                    overTimeDetailList[4].Value4 = item.TotalOverTimeHours5.ToString();
                                    break;
                            }
                            if (overTimeDetailList.Count > 0)
                            {
                                foreach (var itemOverTimeDetai in overTimeDetailList)
                                {
                                    itemOverTimeDetai.Value4 = Utilities.CommonUtil.IntToTime(int.Parse(itemOverTimeDetai.Value4.ToString()), false);
                                }
                                attendanceSummaryListExcelModal.OverTimeDetailList.Add(overTimeDetailList);
                            }

                            AttendanceService AttendanceSer = new AttendanceService(db); 
                            IList<VacationDateInFoByAttendanceSummary> configDetailList;

                            if (flag == 1)
                            {
                                // For All
                                configDetailList = config_DService.GetListVacationDateForAttendanceSummary(M_Config_H.CONFIG_CD_VACATION_TYPE, startDate, endDate, item.UID);
                            }
                            else
                            {
                                // For Approve
                                configDetailList = AttendanceSer.GetListVacationDateForAttendanceSummary(M_Config_H.CONFIG_CD_VACATION_TYPE, startDate, endDate, item.UID);
                            }

                            if (configDetailList.Count > 0)
                            {
                                foreach (VacationDateInFoByAttendanceSummary itemConfigDetail in configDetailList)
                                {
                                    if (itemConfigDetail.VacationDate == "0.0")
                                    {
                                        itemConfigDetail.VacationDate = string.Empty;
                                    }
                                }
                                attendanceSummaryListExcelModal.VacationDetailList.Add(configDetailList);   
                            }

                            if (hasCreateColor)
                            {
                                int count = attendanceService.checkAttendanceSubmit(item.UID, this.dtStartDate.Value.Value.Date, this.dtEndDate.Value.Value.Date);
                                if (count > 0)
                                {
                                    item.IsUnSubmit = false;
                                }
                                else
                                {
                                    item.IsUnSubmit = true;
                                }
                            }
                            else
                            {
                                item.IsUnSubmit = false;
                            }

                            // add detail data
                            attendanceSummaryListExcelModal.DataDetailList.Add(item);
                        }

                        
                        AttendanceSummaryListExcel excel = new AttendanceSummaryListExcel();
                        excel.modelInput = attendanceSummaryListExcelModal;
                        IWorkbook wb = excel.OutputExcel();

                        if (wb != null)
                        {
                            this.SaveFile(wb,".xlsx");
                        }

                        //set flag 
                        this.FlagExcel = ATSummaryExcelType.SummaryList;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Instance.WriteLog(ex);
            }
        }

        /// <summary>
        /// btnExcel Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExchangeExcel_Click(object sender, CommandEventArgs e)
        {
            //call Search_click event
            btnSearch_Click(null, null);

            try
            {
                List<string> lstUserId = this.GetListConditonUserID();
                List<string> lstDeptId = this.GetListConditonDeptID();
                using (DB db = new DB())
                {
                    AttendanceService attendanceService = new AttendanceService(db);

                    IList<ExchangeDateExcelModel> excelDataList = attendanceService.GetListExchangeDateForExcel(this.ddlWorkingCalendar.SelectedValue
                                                                                                                               , lstUserId
                                                                                                                               , lstDeptId);
                    if (excelDataList.Count == 0)
                    {
                        this.SetMessage(string.Empty, M_Message.MSG_HAS_NOT_DATA);
                        return;
                    }
                    else
                    {
                        // AttendanceSummaryListExcelModal
                        AttendanceSummaryExchangeDateExcelModal excelModel = new AttendanceSummaryExchangeDateExcelModal();
                        excelModel.DataList = excelDataList;
                        excelModel.DepartmentNm = string.Format("{0}{1}",
                                                                ddlDepartment1.SelectedValue == "-1" ? "" : ddlDepartment1.SelectedItem.Text + "、",
                                                                ddlDepartment2.SelectedValue == "-1" ? "" : ddlDepartment2.SelectedItem.Text
                                                                ).Trim('、');

                        AttendanceSummaryExchangeDateExcel excel = new AttendanceSummaryExchangeDateExcel();
                        excel.modelInput = excelModel;
                        IWorkbook wb = excel.OutputExcel();

                        if (wb != null)
                        {
                            this.SaveFile(wb,".xlsx");
                        }

                        //set flag 
                        this.FlagExcel = ATSummaryExcelType.ExchangeDate;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Instance.WriteLog(ex);
            }
        }

        #endregion

        #region Method Excel

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
                if (this.FlagExcel != ATSummaryExcelType.CSV)
                {
                    if (this.FlagExcel == ATSummaryExcelType.SummaryList)
                    {
                        filename = string.Format(SUMMARY_EXCEL_DOWNLOAD_NAME, DateTime.Now.ToString(FMT_YMDHMM));
                    }
                    else
                    {
                        filename = string.Format(EXCHANGE_DATE_EXCEL_DOWNLOAD_NAME, DateTime.Now.ToString(FMT_YMDHMM));
                    }
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
                else
                {
                    filename = string.Format(CSV_DOWNLOAD_NAME, DateTime.Now.ToString(FMT_YMDHMM));
                    var filePath = this.ViewState["OUTFILE"].ToString();
                    using (var exportData = base.GetFileStream("OUTFILE"))
                    {
                        Response.ContentType = "application/CSV";
                        Response.AddHeader("Content-Disposition", string.Format("attachment; filename = \"{0}\"", filename));
                        Response.Clear();
                        Response.BinaryWrite(exportData.ToArray());
                        Response.End();
                    }
                }
            }
        }

        /// <summary>
        /// getCalendarById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private T_WorkingCalendar_H getCalendarById(int id)
        {
            T_WorkingCalendar_H tWorkingCalendarH = new T_WorkingCalendar_H();

            using (DB db = new DB())
            {
                T_WorkingCalendar_HService tWorkingCalendarHSer = new T_WorkingCalendar_HService(db);
                tWorkingCalendarH = tWorkingCalendarHSer.GetByID(id);
            }
            return tWorkingCalendarH;
        }

        /// <summary>
        /// getDepartmentById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private M_Department getDepartmentById(int id)
        {
            M_Department department = new M_Department();

            using (DB db = new DB())
            {
                DepartmentService departmentService = new DepartmentService(db);
                department = departmentService.GetDataDepartmentById(id);
            }
            return department;
        }

        /// <summary>
        /// getUserById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private M_User getUserById(int id)
        {
            M_User user = new M_User();

            using (DB db = new DB())
            {
                UserService userService = new UserService(db);
                user = userService.GetByID(id);
            }
            return user;
        }

        #endregion

        #region Event CSV

        /// <summary>
        /// btnCSV Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCSV_Click(object sender, CommandEventArgs e)
        {
            //Check input
            if (!this.CheckInput(true))
            {
                SetPaging();
                return;
            }

            //call Search_click event
            btnSearch_Click(null, null);
            this.ErrorCSV = true;
            int totalRow = 0;
            string strExtractionConditions = string.Empty;
            string strApprovalState = string.Empty;

            string DEFAULTMINHOUR = "00:00";
            string DEFAULJAPANMINHOUR = "0:00";

            // flag check all or approve 
            // 0: Approve, 1: All 
            int flag = 0;
            try
            {
                // User Name
                List<string> lstUserId = this.GetListConditonUserID();
                List<string> lstDeptId = this.GetListConditonDeptID();
                decimal Hour_Date = decimal.MinValue;
                string strHours = string.Empty;
                DateTime startDate;
                DateTime endDate;
                string strStartDate = string.Empty;
                string strEndDate = string.Empty;

                if (this.rbTotal.Checked == true)
                {
                    startDate = this.dtStartDate.Value.Value;
                    endDate = this.dtEndDate.Value.Value;

                    //set strStartDate and strEndDate
                    strStartDate = this.dtStartDate.Text;
                    strEndDate = this.dtEndDate.Text;
                }
                else
                {
                    startDate = DateTime.Parse(ddlDateOfServiceFrom.SelectedValue);
                    endDate = (DateTime.Parse(ddlDateOfServiceTo.SelectedValue)).AddMonths(1).AddDays(-1);

                    //set strStartDate and strEndDate
                    strStartDate = ddlDateOfServiceFrom.SelectedItem.Text;
                    strEndDate = ddlDateOfServiceTo.SelectedItem.Text;
                }

                using (DB db = new DB())
                {
                    AttendanceService attendanceService = new AttendanceService(db);

                    if (this.ddlOvertime_Vacation.SelectedValue != "-1")
                    {
                        if (!(this.ddlOvertime_Vacation.SelectedValue.Contains("Vacation") || this.ddlOvertime_Vacation.SelectedValue.Contains("Days")))
                        {
                            if (this.txtHour_Days.Text != string.Empty)
                            {
                                if (this.txtHour_Days.Text.Contains("."))
                                {
                                    string[] strHour_Days = this.txtHour_Days.Text.Split('.');
                                    string strHor_Days = strHour_Days[0] + ":" + (int.Parse(strHour_Days[1]) * 6).ToString();
                                    Hour_Date = (Utilities.CommonUtil.TimeToInt(strHor_Days));
                                    strExtractionConditions = ddlOvertime_Vacation.SelectedItem.Text + "　が　" + strHor_Days + "　時間　" + ddlCompare.SelectedItem.Text;
                                }
                                else
                                {
                                    strHours = this.txtHour_Days.Text + ":" + "00";
                                    Hour_Date = (Utilities.CommonUtil.TimeToInt(strHours));
                                    strExtractionConditions = ddlOvertime_Vacation.SelectedItem.Text + "　が　" + strHours + "　時間　" + ddlCompare.SelectedItem.Text;

                                }
                            }
                            else
                            {
                                this.ddlOvertime_Vacation.SelectedValue = "-1";
                                strExtractionConditions = string.Empty;
                            }
                        }
                        else
                        {
                            if (this.txtHour_Days.Text != string.Empty)
                            {
                                Hour_Date = decimal.Parse(this.txtHour_Days.Text);
                                strExtractionConditions = ddlOvertime_Vacation.SelectedItem.Text + "　が　" + this.txtHour_Days.Text + "　日数　" + ddlCompare.SelectedItem.Text;
                            }
                            else
                            {
                                this.ddlOvertime_Vacation.SelectedValue = "-1";
                                strExtractionConditions = string.Empty;
                            }
                        }
                    }

                    if (this.rbTotal.Checked == true)
                    {
                        // get total row All
                        totalRow = attendanceService.GetTotalRowAll(this.ddlWorkingCalendar.SelectedValue
                                                                      , lstUserId
                                                                      , this.dtStartDate.Value
                                                                      , this.dtEndDate.Value
                                                                      , lstDeptId
                                                                      , this.ddlOvertime_Vacation.SelectedValue
                                                                      , this.ddlCompare.SelectedValue
                                                                      , Hour_Date);

                    }
                    else
                    {
                        // get total row Approve
                        totalRow = attendanceService.GetTotalRowApprove(this.ddlWorkingCalendar.SelectedValue
                                                                      , lstUserId
                                                                      , this.ddlDateOfServiceFrom.SelectedValue
                                                                      , this.ddlDateOfServiceTo.SelectedValue
                                                                      , lstDeptId
                                                                      , this.ddlOvertime_Vacation.SelectedValue
                                                                      , this.ddlCompare.SelectedValue
                                                                      , Hour_Date);
                    }

                    if (totalRow == 0)
                    {
                        return;
                    }
                    else
                    {
                        // AttendanceSummaryListExcelModal
                        AttendanceSummaryListCSVModal attendanceSummaryListCSVModal = new AttendanceSummaryListCSVModal();
                        int totalCSVNomalDate = 0;
                        IList<AttendanceSummaryInfo> lstAttendanceSummaryInfo;
                        ArrayList lstAttendanceSummaryCSVInfo = new ArrayList();
                        var csv = new StringBuilder();

                        // get data detail
                        if (this.rbTotal.Checked == true)
                        {
                            flag = 1;
                            strApprovalState = "全データ（未承認含む）";
                            lstAttendanceSummaryInfo = attendanceService.GetListAttendanceSummaryAll(this.ddlWorkingCalendar.SelectedValue
                                                                                            , lstUserId
                                                                                            , this.dtStartDate.Value
                                                                                            , this.dtEndDate.Value
                                                                                            , lstDeptId
                                                                                            , this.ddlOvertime_Vacation.SelectedValue
                                                                                            , this.ddlCompare.SelectedValue
                                                                                            , Hour_Date
                                                                                            , -1
                                                                                            , -1
                                                                                            , this.hdSortField.Value
                                                                                            , this.GetSortDirec());
                        }
                        else
                        {
                            flag = 0;
                            strApprovalState = "承認済データのみ";
                            lstAttendanceSummaryInfo = attendanceService.GetListAttendanceSummaryApprove(this.ddlWorkingCalendar.SelectedValue
                                                                                                , lstUserId
                                                                                                , this.ddlDateOfServiceFrom.SelectedValue
                                                                                                , this.ddlDateOfServiceTo.SelectedValue
                                                                                                , lstDeptId
                                                                                                , this.ddlOvertime_Vacation.SelectedValue
                                                                                                , this.ddlCompare.SelectedValue
                                                                                                , Hour_Date
                                                                                                , -1
                                                                                                , -1
                                                                                                , this.hdSortField.Value
                                                                                                , this.GetSortDirec());
                        }

                        //get list nomal date for csv.
                        totalCSVNomalDate = attendanceService.GetListDateCSV(this.ddlWorkingCalendar.SelectedValue
                                                                            , startDate
                                                                            , endDate
                                                                            );
                        
                        //check TotalNomal Date
                        if (totalCSVNomalDate > 31)
                        {
                            ShowMessageCreateCSVErrors(M_Message.MSG_CREATE_CSV_ERROR, Models.DefaultButton.No);
                        }
                       
                        int MAXVALUEHOURS = CommonUtil.TimeToInt("743:59");
                        int MAXVALUEDAY = 31;
                        T_WorkingCalendar_H tWorkingCalendarH = new T_WorkingCalendar_H();
                        T_WorkingCalendar_HService tWorkingCalendarHSer = new T_WorkingCalendar_HService(db);
                        tWorkingCalendarH = tWorkingCalendarHSer.GetByID(int.Parse(ddlWorkingCalendar.SelectedValue));

                        // Service Config
                        Config_DService config_DService = new Config_DService(db);

                        //set Vacation Header
                        IList<M_Config_D> vacationHeadersList;
                        vacationHeadersList = config_DService.GetListByConfigCd(M_Config_H.CONFIG_CD_VACATION_TYPE);

                        //set OverTime Header
                        IList<M_Config_D> overTimeHeadersList;
                        overTimeHeadersList = config_DService.GetListByConfigCd(M_Config_H.CONFIG_CD_OVER_TIME_TYPE);

                        
                        foreach (var item in lstAttendanceSummaryInfo)
                        {
                            AttendanceSummaryCSVInfo attendanceSummaryCSVInfo = new AttendanceSummaryCSVInfo();
                            attendanceSummaryCSVInfo.UserCD = item.UserCD.TrimStart('0');                                                                              //1.社員コード
                            if (item.NumWorkingDays > MAXVALUEDAY)
                            {
                                
                                ShowMessageCreateCSVErrors(M_Message.MSG_CREATE_CSV_ERROR, Models.DefaultButton.No);
                            }
                            else
                            {
                                attendanceSummaryCSVInfo.WorkingDays = item.NumWorkingDays.ToString("0.00");                                            //2.就業日数
                            }

                            if ((item.NumWorkingDays + item.NumLH_Days + item.NumSH_Days) > MAXVALUEDAY)
                            {
                                ShowMessageCreateCSVErrors(M_Message.MSG_CREATE_CSV_ERROR, Models.DefaultButton.No);
                            }
                            else
                            {
                                attendanceSummaryCSVInfo.AttendanceDays = (item.NumWorkingDays + item.NumLH_Days + item.NumSH_Days).ToString("0.00");   //3.出勤日数
                            }

                            //get vacationDate
                            AttendanceService AttendanceSer = new AttendanceService(db);
                            IList<VacationDateInFoByAttendanceSummary> configDetailList;

                            if (flag == 1)
                            {
                                // For All
                                configDetailList = config_DService.GetListVacationDateForAttendanceSummary(M_Config_H.CONFIG_CD_VACATION_TYPE, startDate, endDate, item.UID);
                            }
                            else
                            {
                                // For Approve
                                configDetailList = AttendanceSer.GetListVacationDateForAttendanceSummary(M_Config_H.CONFIG_CD_VACATION_TYPE, startDate, endDate, item.UID);
                            }

                            switch (configDetailList.Count)
                            {
                                case 0:
                                    break;
                                case 1:
                                    if (float.Parse(configDetailList[0].VacationDate) > MAXVALUEDAY || (item.NumLH_Days + item.NumSH_Days) > MAXVALUEDAY)
                                    {
                                        ShowMessageCreateCSVErrors(M_Message.MSG_CREATE_CSV_ERROR, Models.DefaultButton.No);
                                    }
                                    else
                                    {
                                        attendanceSummaryCSVInfo.DaysLeft[0] = (float.Parse(configDetailList[0].VacationDate)).ToString("0.00");      //5.有休日数
                                        attendanceSummaryCSVInfo.DaysLeft[1] = "0";
                                        attendanceSummaryCSVInfo.SubstituteHoliday = (item.NumLH_Days + item.NumSH_Days).ToString("0.00");            //7.休出日数
                                    }
                                    break;
                                case 2:
                                    if (float.Parse(configDetailList[0].VacationDate) > MAXVALUEDAY || (item.NumLH_Days + item.NumSH_Days) > MAXVALUEDAY)
                                    {
                                        ShowMessageCreateCSVErrors(M_Message.MSG_CREATE_CSV_ERROR, Models.DefaultButton.No);
                                    }
                                    else
                                    {
                                        attendanceSummaryCSVInfo.DaysLeft[0] = configDetailList[0].VacationDate;                                    //5.有休日数
                                        attendanceSummaryCSVInfo.DaysLeft[1] = "0";
                                        attendanceSummaryCSVInfo.SubstituteHoliday = (item.NumLH_Days + item.NumSH_Days).ToString("0.00");          //7.休出日数
                                    }
                                    break;
                                case 3:
                                    if (float.Parse(configDetailList[0].VacationDate) > MAXVALUEDAY
                                        || (item.NumLH_Days + item.NumSH_Days) > MAXVALUEDAY
                                        || float.Parse(configDetailList[2].VacationDate) > MAXVALUEDAY)
                                    {
                                        ShowMessageCreateCSVErrors(M_Message.MSG_CREATE_CSV_ERROR, Models.DefaultButton.No);
                                    }
                                    else
                                    {
                                        attendanceSummaryCSVInfo.DaysLeft[0] = configDetailList[0].VacationDate;                                    //5.有休日数
                                        attendanceSummaryCSVInfo.DaysLeft[1] = "0";
                                        attendanceSummaryCSVInfo.SubstituteHoliday = (item.NumLH_Days + item.NumSH_Days).ToString("0.00");          //7.休出日数
                                        attendanceSummaryCSVInfo.DayOff[0] =  configDetailList[2].VacationDate;                                     //8.代休日数
                                        attendanceSummaryCSVInfo.DayOff[1] = "0";                                
                                    }
                                    break;
                                case 4:
                                    if (float.Parse(configDetailList[0].VacationDate) > MAXVALUEDAY
                                        || float.Parse(configDetailList[3].VacationDate) > MAXVALUEDAY
                                        || (item.NumLH_Days + item.NumSH_Days) > MAXVALUEDAY
                                        || float.Parse(configDetailList[2].VacationDate) > MAXVALUEDAY)
                                    {
                                        ShowMessageCreateCSVErrors(M_Message.MSG_CREATE_CSV_ERROR, Models.DefaultButton.No);
                                    }
                                    else
                                    {
                                        attendanceSummaryCSVInfo.DaysLeft[0] = configDetailList[0].VacationDate;                                    //5.有休日数
                                        attendanceSummaryCSVInfo.DaysLeft[1] = "0";
                                        attendanceSummaryCSVInfo.SpecialHolidayNumber[0] = configDetailList[3].VacationDate;                        //6.特休日数
                                        attendanceSummaryCSVInfo.SpecialHolidayNumber[1] = "0";
                                        attendanceSummaryCSVInfo.SubstituteHoliday = (item.NumLH_Days + item.NumSH_Days).ToString("0.00");          //7.休出日数                                        
                                        attendanceSummaryCSVInfo.DayOff[0] = configDetailList[2].VacationDate;                                      //8.代休日数
                                        attendanceSummaryCSVInfo.DayOff[1] = "0";
                                    }
                                    
                                    break;
                                default:
                                    if (float.Parse(configDetailList[4].VacationDate) > MAXVALUEDAY
                                        || float.Parse(configDetailList[0].VacationDate) > MAXVALUEDAY
                                        || float.Parse(configDetailList[3].VacationDate) > MAXVALUEDAY
                                        || (item.NumLH_Days + item.NumSH_Days) > MAXVALUEDAY
                                        || float.Parse(configDetailList[2].VacationDate) > MAXVALUEDAY)
                                    {
                                        ShowMessageCreateCSVErrors(M_Message.MSG_CREATE_CSV_ERROR, Models.DefaultButton.No);
                                    }
                                    else
                                    {
                                        attendanceSummaryCSVInfo.DaysWithoutWork[0] = (float.Parse(configDetailList[4].VacationDate)).ToString("0.00");         //4.欠勤日数
                                        attendanceSummaryCSVInfo.DaysWithoutWork[1] = "0";
                                        attendanceSummaryCSVInfo.DaysLeft[0] = (float.Parse(configDetailList[0].VacationDate)).ToString("0.00");                //5.有休日数
                                        attendanceSummaryCSVInfo.DaysLeft[1] = "0";
                                        attendanceSummaryCSVInfo.SpecialHolidayNumber[0] = (float.Parse(configDetailList[3].VacationDate)).ToString("0.00");    //6.特休日数
                                        attendanceSummaryCSVInfo.SpecialHolidayNumber[1] = "0";
                                        attendanceSummaryCSVInfo.SubstituteHoliday = (item.NumLH_Days + item.NumSH_Days).ToString("0.00");                      //7.休出日数
                                        attendanceSummaryCSVInfo.DayOff[0] = (float.Parse(configDetailList[2].VacationDate)).ToString("0.00");                  //8.代休日数
                                        attendanceSummaryCSVInfo.DayOff[1] = "0";
                                    }
                                    break;
                            }

                            if (item.NumEarlyDays + item.NumLateDays > 99)
                            {
                                ShowMessageCreateCSVErrors(M_Message.MSG_CREATE_CSV_ERROR, Models.DefaultButton.No);
                            }
                            else
                            {
                                attendanceSummaryCSVInfo.LateEarlyDays = (item.NumEarlyDays + item.NumLateDays).ToString("0.00");                               //9.遅早回数
                            }

                            if (CommonUtil.TimeToInt(item.TotalWorkingHours) > MAXVALUEHOURS)
                            {
                                ShowMessageCreateCSVErrors(M_Message.MSG_CREATE_CSV_ERROR, Models.DefaultButton.No);
                            }
                            else
                            {
                                attendanceSummaryCSVInfo.AttendanceTimeHours = item.TotalWorkingHours;                                                          //10.出勤時間
                            }

                            if((CommonUtil.TimeToInt(item.EarlyHours) + CommonUtil.TimeToInt(item.LateHours) > MAXVALUEHOURS))
                            {
                                ShowMessageCreateCSVErrors(M_Message.MSG_CREATE_CSV_ERROR, Models.DefaultButton.No);
                            }
                            else
                            {
                                attendanceSummaryCSVInfo.LateTimeHours = CommonUtil.IntToTime(CommonUtil.TimeToInt(item.EarlyHours) + CommonUtil.TimeToInt(item.LateHours),true);       //11.遅早時間
                            }
                            
                            if(CommonUtil.TimeToInt(item.SH_Hours) > MAXVALUEHOURS)
                            {
                                ShowMessageCreateCSVErrors(M_Message.MSG_CREATE_CSV_ERROR, Models.DefaultButton.No);
                            }
                            else
                            {
                                attendanceSummaryCSVInfo.OvertimeOnholiday = item.SH_Hours;                                                     //14.休日残業時間
                            }

                            if (item.NumLH_Days > MAXVALUEDAY)
                            {
                                ShowMessageCreateCSVErrors(M_Message.MSG_CREATE_CSV_ERROR, Models.DefaultButton.No);
                                
                            }
                            else
                            {
                                attendanceSummaryCSVInfo.StatutoryHoliday = item.NumLH_Days.ToString("0.00");                                   //16.法定内休出日数
                            }
                            if (CommonUtil.TimeToInt(item.LH_Hours) > MAXVALUEHOURS)
                            {
                                ShowMessageCreateCSVErrors(M_Message.MSG_CREATE_CSV_ERROR, Models.DefaultButton.No);
                            }
                            else
                            {
                                attendanceSummaryCSVInfo.LH_Hours = item.LH_Hours;                                                              //17.法定休日普通残業時間
                            }
                            
                            
                            // set overTime detail
                            IList<M_Config_D> overTimeDetailList;
                            overTimeDetailList = config_DService.GetListByConfigCd(M_Config_H.CONFIG_CD_OVER_TIME_TYPE);
                            
                            if (overTimeDetailList.Count > 0)
                            {
                                foreach (var itemOverTimeDetai in overTimeDetailList)
                                {
                                    itemOverTimeDetai.Value4 = string.Empty;
                                }
                            }

                            switch (overTimeDetailList.Count)
                            {
                                case 0:
                                    break;
                                case 1:
                                    if (item.TotalOverTimeHours1 > MAXVALUEHOURS)
                                    {
                                        ShowMessageCreateCSVErrors(M_Message.MSG_CREATE_CSV_ERROR, Models.DefaultButton.No);
                                        return;
                                    }
                                    else
                                    {
                                        attendanceSummaryCSVInfo.PrepaidOvertimeHours[0] = CommonUtil.IntToTime(item.TotalOverTimeHours1,true);                               //20.早出残業時間
                                        attendanceSummaryCSVInfo.PrepaidOvertimeHours[1] = "0";
                                    }
                                    
                                    break;
                                case 2:

                                    attendanceSummaryCSVInfo.WeekdayNormalOvertimeHours[0] = CommonUtil.IntToTime(item.TotalOverTimeHours2,true);                              //12.平日普通残業時間
                                    attendanceSummaryCSVInfo.WeekdayNormalOvertimeHours[1] = "0";
                                    attendanceSummaryCSVInfo.PrepaidOvertimeHours[0] = CommonUtil.IntToTime(item.TotalOverTimeHours1,true);                                    //20.早出残業時間
                                    attendanceSummaryCSVInfo.PrepaidOvertimeHours[1] = "0";
                                    break;
                                case 3:
                                    attendanceSummaryCSVInfo.WeekdayNormalOvertimeHours[0] = CommonUtil.IntToTime(item.TotalOverTimeHours2,true);                              //12.平日普通残業時間
                                    attendanceSummaryCSVInfo.WeekdayNormalOvertimeHours[1] = "0";
                                    attendanceSummaryCSVInfo.OvertimeLate[0] = CommonUtil.IntToTime(item.TotalOverTimeHours3,true);                                            //13.平日深夜残業時間
                                    attendanceSummaryCSVInfo.OvertimeLate[0] = "0";
                                    attendanceSummaryCSVInfo.PrepaidOvertimeHours[0] = CommonUtil.IntToTime(item.TotalOverTimeHours1,true);                                    //20.早出残業時間
                                    attendanceSummaryCSVInfo.PrepaidOvertimeHours[1] = "0";
                                    break;
                                case 4:
                                     
                                    attendanceSummaryCSVInfo.WeekdayNormalOvertimeHours[0] = CommonUtil.IntToTime(item.TotalOverTimeHours2,true);                              //12.平日普通残業時間
                                    attendanceSummaryCSVInfo.WeekdayNormalOvertimeHours[1] = "0";
                                    attendanceSummaryCSVInfo.OvertimeLate[0] = CommonUtil.IntToTime(item.TotalOverTimeHours3,true);                                            //13.平日深夜残業時間
                                    attendanceSummaryCSVInfo.OvertimeLate[0] = "0";
                                    attendanceSummaryCSVInfo.OvertimeMidNight[0] = CommonUtil.IntToTime(item.TotalOverTimeHours4,true);                                        //15.休日深夜残業時間
                                    attendanceSummaryCSVInfo.OvertimeMidNight[1] = "0";
                                    attendanceSummaryCSVInfo.PrepaidOvertimeHours[0] = CommonUtil.IntToTime(item.TotalOverTimeHours1,true);                                    //20.早出残業時間
                                    attendanceSummaryCSVInfo.PrepaidOvertimeHours[1] = "0";
                                    break;
                                default:
                                    
                                    attendanceSummaryCSVInfo.WeekdayNormalOvertimeHours[0] = CommonUtil.IntToTime(item.TotalOverTimeHours2,true);                              //12.平日普通残業時間
                                    attendanceSummaryCSVInfo.WeekdayNormalOvertimeHours[1] = "0";
                                    attendanceSummaryCSVInfo.OvertimeLate[0] = CommonUtil.IntToTime(item.TotalOverTimeHours3,true);                                            //13.平日深夜残業時間
                                    attendanceSummaryCSVInfo.OvertimeLate[1] = "0";
                                    attendanceSummaryCSVInfo.OvertimeMidNight[0] = CommonUtil.IntToTime(item.TotalOverTimeHours4,true);                                        //15.休日深夜残業時間
                                    attendanceSummaryCSVInfo.OvertimeMidNight[1] = "0";
                                    attendanceSummaryCSVInfo.LH_OverTimeHours[0] = CommonUtil.IntToTime(item.TotalOverTimeHours5,true);                                        //18.法定休日深夜残業時間
                                    attendanceSummaryCSVInfo.LH_OverTimeHours[1] = "0";
                                    attendanceSummaryCSVInfo.PrepaidOvertimeHours[0] = CommonUtil.IntToTime(item.TotalOverTimeHours1,true);                                    //20.早出残業時間
                                    attendanceSummaryCSVInfo.PrepaidOvertimeHours[1] = "0";
                                    break;
                            }
                            lstAttendanceSummaryCSVInfo.Add(attendanceSummaryCSVInfo);
                        }

                        // Create the CSV file to which grid data will be exported.
                        if (ErrorCSV)
                        {
                            this.CreateFileCSV();
                            var filePath = this.ViewState["OUTFILE"].ToString();
                            StreamWriter stWriter = new StreamWriter(filePath, false);

                            for (int i = 0; i < lstAttendanceSummaryCSVInfo.Count; i++)
                            {
                                AttendanceSummaryCSVInfo attendanceSummaryCSVStream = (AttendanceSummaryCSVInfo)lstAttendanceSummaryCSVInfo[i];
                                //1.社員コード
                                stWriter.Write(attendanceSummaryCSVStream.UserCD);
                                stWriter.Write(",");

                                //2.就業日数
                                stWriter.Write(totalCSVNomalDate.ToString("0.00"));
                                stWriter.Write(",");

                                //3.出勤日数
                                stWriter.Write(attendanceSummaryCSVStream.AttendanceDays);
                                stWriter.Write(",");

                                //4.欠勤日数
                                if (attendanceSummaryCSVStream.DaysWithoutWork[1] != "-1")
                                {
                                    stWriter.Write(attendanceSummaryCSVStream.DaysWithoutWork[0]);
                                    stWriter.Write(",");
                                }

                                //5.有休日数
                                if (attendanceSummaryCSVStream.DaysLeft[1] != "-1")
                                {
                                    stWriter.Write(attendanceSummaryCSVStream.DaysLeft[0]);
                                    stWriter.Write(",");
                                }

                                //6.特休日数
                                if (attendanceSummaryCSVStream.SpecialHolidayNumber[1] != "-1")
                                {
                                    stWriter.Write(attendanceSummaryCSVStream.SpecialHolidayNumber[0]);
                                    stWriter.Write(",");
                                }

                                //7.休出日数
                                stWriter.Write(attendanceSummaryCSVStream.SubstituteHoliday);
                                stWriter.Write(",");

                                //8.代休日数
                                if (attendanceSummaryCSVStream.DayOff[1] != "-1")
                                {
                                    stWriter.Write(attendanceSummaryCSVStream.DayOff[0]);
                                    stWriter.Write(",");
                                }

                                //9.遅早回数
                                stWriter.Write(attendanceSummaryCSVStream.LateEarlyDays);
                                stWriter.Write(",");

                                //10.出勤時間
                                if (attendanceSummaryCSVStream.AttendanceTimeHours == DEFAULTMINHOUR || attendanceSummaryCSVStream.AttendanceTimeHours == string.Empty)
                                {
                                    stWriter.Write(DEFAULJAPANMINHOUR);
                                }
                                else
                                {
                                    stWriter.Write(formatCsvHour(attendanceSummaryCSVStream.AttendanceTimeHours));
                                }
                                
                                stWriter.Write(",");

                                //11.遅早時間
                                if (attendanceSummaryCSVStream.LateTimeHours == DEFAULTMINHOUR || attendanceSummaryCSVStream.LateTimeHours == string.Empty)
                                {
                                    stWriter.Write(DEFAULJAPANMINHOUR);
                                }
                                else
                                {
                                    stWriter.Write(formatCsvHour(attendanceSummaryCSVStream.LateTimeHours));
                                }
                                stWriter.Write(",");

                                //12.平日普通残業時間
                                if (attendanceSummaryCSVStream.WeekdayNormalOvertimeHours[1] != "-1")
                                {
                                    if (attendanceSummaryCSVStream.WeekdayNormalOvertimeHours[0] == DEFAULTMINHOUR || attendanceSummaryCSVStream.WeekdayNormalOvertimeHours[0] == string.Empty)
                                    {
                                        stWriter.Write(DEFAULJAPANMINHOUR);
                                    }
                                    else
                                    {
                                        stWriter.Write(formatCsvHour(attendanceSummaryCSVStream.WeekdayNormalOvertimeHours[0]));
                                    }
                                    stWriter.Write(",");
                                }

                                //13.平日深夜残業時間
                                if (attendanceSummaryCSVStream.OvertimeLate[1] != "-1")
                                {
                                    if (attendanceSummaryCSVStream.OvertimeLate[0] == DEFAULTMINHOUR || attendanceSummaryCSVStream.OvertimeLate[0] == string.Empty)
                                    {
                                        stWriter.Write(DEFAULJAPANMINHOUR);
                                    }
                                    else
                                    {
                                        stWriter.Write(formatCsvHour(attendanceSummaryCSVStream.OvertimeLate[0]));
                                    }
                                    stWriter.Write(",");
                                }

                                //14.休日残業時間
                                if (attendanceSummaryCSVStream.OvertimeOnholiday == DEFAULTMINHOUR || attendanceSummaryCSVStream.OvertimeOnholiday == string.Empty)
                                {
                                    stWriter.Write(DEFAULJAPANMINHOUR);
                                }
                                else
                                {
                                    stWriter.Write(formatCsvHour(attendanceSummaryCSVStream.OvertimeOnholiday));
                                }
                                stWriter.Write(",");

                                //15.休日深夜残業時間
                                if (attendanceSummaryCSVStream.OvertimeMidNight[1] != "-1")
                                {
                                    if (attendanceSummaryCSVStream.OvertimeMidNight[0] == DEFAULTMINHOUR || attendanceSummaryCSVStream.OvertimeMidNight[0] == string.Empty)
                                    {
                                        stWriter.Write(DEFAULJAPANMINHOUR);
                                    }
                                    else
                                    {
                                        stWriter.Write(formatCsvHour(attendanceSummaryCSVStream.OvertimeMidNight[0]));
                                    }
                                    stWriter.Write(",");
                                }

                                //16.法定内休出日数
                                stWriter.Write(attendanceSummaryCSVStream.StatutoryHoliday);
                                stWriter.Write(",");

                                //17.法定休日普通残業時間
                                if (attendanceSummaryCSVStream.LH_Hours == DEFAULTMINHOUR || attendanceSummaryCSVStream.LH_Hours == string.Empty)
                                {
                                    stWriter.Write(DEFAULJAPANMINHOUR);
                                }
                                else
                                {
                                    stWriter.Write(formatCsvHour(attendanceSummaryCSVStream.LH_Hours));
                                }
                                stWriter.Write(",");

                                //18.法定休日深夜残業時間
                                if (attendanceSummaryCSVStream.LH_OverTimeHours[1] != "-1")
                                {
                                    if (attendanceSummaryCSVStream.LH_OverTimeHours[0] == DEFAULTMINHOUR || attendanceSummaryCSVStream.LH_OverTimeHours[0] == string.Empty)
                                    {
                                        stWriter.Write(DEFAULJAPANMINHOUR);
                                    }
                                    else
                                    {
                                        stWriter.Write(formatCsvHour(attendanceSummaryCSVStream.LH_OverTimeHours[0]));
                                    }
                                    stWriter.Write(",");
                                }

                                //19.法定内残業時間
                                //stWriter.Write(attendanceSummaryCSVStream.StatutoryOverTimeHours);
                                stWriter.Write("0:00");
                                stWriter.Write(",");

                                //20.早出残業時間
                                if (attendanceSummaryCSVStream.PrepaidOvertimeHours[1] != "-1")
                                {
                                    if (attendanceSummaryCSVStream.PrepaidOvertimeHours[0] == DEFAULTMINHOUR || attendanceSummaryCSVStream.PrepaidOvertimeHours[0] == string.Empty)
                                    {
                                        stWriter.Write(DEFAULJAPANMINHOUR);
                                    }
                                    else
                                    {
                                        stWriter.Write(formatCsvHour(attendanceSummaryCSVStream.PrepaidOvertimeHours[0]));
                                    }
                                    stWriter.Write(",");
                                }

                                //21->30.予備項目
                                stWriter.Write(",,,,,,,,,");

                                //if (i != lstAttendanceSummaryCSVInfo.Count -1)
                                //{
                                stWriter.Write(stWriter.NewLine);
                                //}
                            }

                            stWriter.Close();

                            //set flag
                            this.FlagExcel = ATSummaryExcelType.CSV;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Instance.WriteLog(ex);
            }
        }

        /// <summary>
        /// format for Hour Csv file
        /// </summary>
        /// <param name="timeHour"></param>
        /// <returns></returns>
        protected string formatCsvHour(string timeHour) 
        {
            string value = string.Empty;
            if (timeHour[0] == '0')
            {
                value = timeHour.Remove(0, 1);
            }
            else
            {
                value = timeHour;
            }
            
            return value;
        }

        #endregion
    }
}