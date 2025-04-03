using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using OMS.Utilities;
using OMS.DAC;
using OMS.Models;
using System.Globalization;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using OMS.Reports.EXCEL;
using OMS.Models.Type;
using NPOI.SS.UserModel;
using System.Collections;
using System.Web.Script.Services;
using System.Text;

namespace OMS.WorkingCalendar
{
    /// <summary>
    /// FrmWorkingCalendarEntry
    /// Create  : ISV.Hien
    /// Date    : 1/10/2017
    /// </summary>
    public partial class FrmWorkingCalendarEntry : FrmBaseDetail
    {

        #region Constants
        private const string URL_LIST = "~/WorkingCalendar/FrmWorkingCalendarList.aspx";
        private const string FULL_TIME_EMPLOYMENT = "1";

        //労働日数時間集計表出力（Excel）
        private const string WORKING_CALENDA_ENTRY_DOWNLOAD_FORMAT_A = "労働日数時間集計表_{0}.xls";

        // 年間勤務カレンダー出力（Excel）
        private const string WORKING_CALENDA_ENTRY_DOWNLOAD_FORMAT_B = "年間勤務カレンダー_{0}.xls";

        private const string FMT_YMDHMM = "yyMMddHHmm";
        private const string FMT_GGYYMMDD = "gg yy'年'MM'月'dd'日'";
        private const string ADMINCD = "0000";
        #endregion

        #region Property

        /// <summary>
        /// Get or set WorkingCalendarID
        /// </summary>
        public int WorkingCalendarID
        {
            get {
                if (ViewState["WorkingCalendarID"] == null)
                {
                    return -1;
                }
                return (int)ViewState["WorkingCalendarID"]; }
            set { ViewState["WorkingCalendarID"] = value; }
        }

        /// <summary>
        /// Get or set OldUpdateDate
        /// </summary>
        public DateTime OldUpdateDate
        {
            get { return (DateTime)ViewState["OldUpdateDate"]; }
            set { ViewState["OldUpdateDate"] = value; }
        }

        /// <summary>
        /// Get or set ModeClearAfterClear
        /// </summary>
        public string ModeClearAfterClear
        {
            get { return (string)ViewState["ModeClearAfterClear"]; }
            set { ViewState["ModeClearAfterClear"] = value; }
        }

        // <summary>
        /// Get or set WorkingCalendarEntryExcelFlag
        /// </summary>
        public string WorkingCalendarEntryExcelFlag
        {
            get { return (string)ViewState["WorkingCalendarEntryExcelFlag"]; }
            set { ViewState["WorkingCalendarEntryExcelFlag"] = value; }
        }

        #endregion

        #region Variable

        /// <summary>
        /// flagSetFocus
        /// </summary>
        public int flagSetFocus;

        public bool isApproval;

        public bool isOnlyPaidLeave;

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
            base.FormTitle = "勤務体系カレンダー登録";
            base.FormSubTitle = "Detail";

            //Init Max Length
            this.txtCalendarCD.MaxLength = T_WorkingCalendar_H.CALENDAR_CD_MAX_LENGTH;
            this.txtCalendarName.MaxLength = T_WorkingCalendar_H.CALENDAR_NAME_MAX_LENGTH;

            //Init Event
            LinkButton btnYes = (LinkButton)this.Master.FindControl("btnYes");
            btnYes.Click += new EventHandler(btnProcessData);

            LinkButton btnNo = (LinkButton)this.Master.FindControl("btnNo");
            btnNo.Click += new EventHandler(btnShowData);

            //Click the Download Excel button
            this.btnDownload.ServerClick += new EventHandler(btnDownloadExcel_Click);
            this.hidUserID.Value = this.LoginInfo.User.ID.ToString(); ;

        }

        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

            base.SetAuthority(FormId.WorkCalendar);
            this.isOnlyPaidLeave = false;
            if (!base._authority.IsWorkCalendarView)
            {
                if (!IsExistsCalendar(-1))
                {
                    Response.Redirect("~/Menu/FrmMainMenu.aspx");
                }
                else
                {
                    this.isOnlyPaidLeave = true;
                }

                
            }

            GetDefaultValue();

            if (!this.IsPostBack)
            {

                if (this.PreviousPage != null)
                {
                    //Save condition of previous page
                    this.ViewState["Condition"] = this.PreviousPageViewState["Condition"];

                    //Check mode
                    if (this.PreviousPageViewState["ID"] == null)
                    {
                        this.WorkingCalendarID = -1;
                        //Set mode
                        this.ProcessMode(Mode.Insert);

                    }
                    else
                    {
                        //Get WorkingCalendar ID
                        this.WorkingCalendarID = int.Parse(PreviousPageViewState["ID"].ToString());
                        T_WorkingCalendar_H workingCalendar = this.GetWorkingCalendarById(this.WorkingCalendarID, new DB());

                        //Check WorkingCalendar
                        if (workingCalendar != null)
                        {
                            //Show data
                            this.ShowData(workingCalendar);

                            //Set Mode
                            this.ProcessMode(Mode.View);
                        }
                        else
                        {
                            Server.Transfer(URL_LIST);
                        }
                    }
                }
                else
                {
                    //Set mode
                    this.ProcessMode(Mode.Insert);
                }
            }

            //Set init
            this.Success = false;
        }

        /// <summary>
        /// Event Back
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            // set value treeview default
            this.treeViewLeftData.Value = string.Empty;
            this.treeViewRightData.Value = string.Empty;

            //Get WorkingCalendar
            T_WorkingCalendar_H workingCalendar = this.GetWorkingCalendarById(this.WorkingCalendarID, new DB());

            //Check workingCalendar
            if (workingCalendar != null)
            {
                //Show data
                this.ShowData(workingCalendar);

                //Set Mode
                this.ProcessMode(Mode.View);

                DateTime InitialDate = workingCalendar.InitialDate;
                DataTable dt1 = InitDataTableHeader();
                dt1 = SetDataTableHeader(dt1, InitialDate);
                this.rptWorkingCalendarHeader.DataSource = dt1;
                this.rptWorkingCalendarHeader.DataBind();
            }

        }

        /// <summary>
        /// Edit Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            //Get t_WorkingCalendar_H
            T_WorkingCalendar_H t_WorkingCalendar_H = this.GetWorkingCalendarById(this.WorkingCalendarID, new DB());
            InitCombobox(this.cmbWorkingSystemData);

            //Check t_WorkingCalendar_H
            if (t_WorkingCalendar_H != null)
            {
                InitDateSave.Value = txtInitialDate.Text;
                //Show data
                this.ShowData(t_WorkingCalendar_H);

                //Set Mode
                this.ProcessMode(Mode.Update);
            }

            else
            {
                Server.Transfer(URL_LIST);
            }
        }

        /// <summary>
        /// Event Delete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            //Set Model
            this.Mode = Mode.Delete;
            ModeClearAfterClear = "Delete";

            //Show question insert
            base.ShowQuestionMessage(M_Message.MSG_QUESTION_DELETE, Models.DefaultButton.No, true);
        }

        /// <summary>
        /// Event Insert Submit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnInsert_Click(object sender, EventArgs e)
        {
            //Check input
            if (!this.CheckInput())
            {
                return;
            }

            //Draw Calendar with new Hidden Value
            DataTable dt = InitDataTableHeader();
            DateTime fromDate = DateTime.Parse(txtInitialDate.Text);

            string strfromDate = string.Format(Constants.FMT_YMD, fromDate);
            dt = SetDataTableHeader(dt, fromDate);

            rptWorkingCalendarHeader.DataSource = dt;
            rptWorkingCalendarHeader.DataBind();
            ModeClearAfterClear = null;

            //Check input
            if (!this.CheckInput())
            {
                return;
            }

            //Show question insert
            base.ShowQuestionMessage(M_Message.MSG_QUESTION_INSERT, Models.DefaultButton.Yes);

        }

        /// <summary>
        /// Event Update Submit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            //Check input
            if (!this.CheckInput())
            {
                return;
            }
            //Draw Calendar with new Hidden Value
            DataTable dt = InitDataTableHeader();
            DateTime fromDate = DateTime.Parse(txtInitialDate.Text);

            string strfromDate = string.Format(Constants.FMT_YMD, fromDate);
            dt = SetDataTableHeader(dt, fromDate);

            rptWorkingCalendarHeader.DataSource = dt;
            rptWorkingCalendarHeader.DataBind();

            //Show question update
            base.ShowQuestionMessage(M_Message.MSG_QUESTION_UPDATE, Models.DefaultButton.Yes);
        }

        /// <summary>
        /// Event changed page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNew_Click(object sender, CommandEventArgs e)
        {
            this.txtCalendarCD.Value = string.Empty;
            this.txtCalendarName.Value = string.Empty;
            this.txtInitialDate.Text = string.Empty;

            DataTable dtCalendar = InitDataTableHeader();
            rptWorkingCalendarHeader.DataSource = dtCalendar;
            rptWorkingCalendarHeader.DataBind();
            //Set mode
            this.ProcessMode(Mode.Insert);
        }

        /// <summary>
        /// Clear page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClear_Click(object sender, EventArgs e)
        {
            ModeClearAfterClear = this.Mode.ToString();

            //Draw Calendar with new Hidden Value
            DataTable dt = InitDataTableHeader();
            DateTime fromDate = DateTime.Parse(txtInitialDate.Text);

            string strfromDate = string.Format(Constants.FMT_YMD, fromDate);
            dt = SetDataTableHeader(dt, fromDate);

            rptWorkingCalendarHeader.DataSource = dt;
            rptWorkingCalendarHeader.DataBind();

            base.ShowQuestionMessage(M_Message.MSG_CLEAR, Models.DefaultButton.No, true);
        }

        #endregion

        #region Method

        private bool IsExistsCalendar(int calendarID)
        {
            using (DB db = new DB())
            {
                T_WorkingCalendar_HService calendarHSer = new T_WorkingCalendar_HService(db);
                return calendarHSer.IsExistsByUID(this.LoginInfo.User.ID, calendarID);
            }
        }

        /// <summary>
        /// Process Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnProcessData(object sender, EventArgs e)
        {

            //Check Mode
            switch (this.Mode)
            {
                case Utilities.Mode.Insert:
                case Utilities.Mode.Copy:

                    //call Search_CLick
                    if (ModeClearAfterClear == "Insert")
                    {
                        ModeClearAfterClear = "Clear";
                        DrawCalendar(this.Mode);
                        ModeClearAfterClear = null;

                        //set default data 
                        this.treeViewLeftData.Value = string.Empty;
                        this.treeViewRightData.Value = string.Empty;
                    }

                    //Insert Data
                    else if (this.InsertData())
                    {

                        T_WorkingCalendar_H t_WorkingCalendar_H = this.GetWorkingCalendarById(WorkingCalendarID, new DB());

                        //Show data
                        this.ShowData(t_WorkingCalendar_H);

                        //Set Mode
                        this.ProcessMode(Mode.View);

                        //Set Success
                        this.Success = true;
                    }
                    break;
                case Utilities.Mode.Delete:

                    //Delete group
                    if (!this.DeleteData())
                    {
                        //Set Mode
                        this.ProcessMode(Mode.View);
                    }
                    else
                    {
                        Server.Transfer(URL_LIST);
                    }
                    break;

                default:

                    //call Search_CLick (for button clear in update)
                    if (ModeClearAfterClear == "Update")
                    {
                        ModeClearAfterClear = "Clear";
                        DrawCalendar(this.Mode);
                        ModeClearAfterClear = null;

                        //set default data 
                        this.treeViewLeftData.Value = string.Empty;
                        this.treeViewRightData.Value = string.Empty;
                    }
                    else
                    {
                        //Update Data
                        if (this.UpdateData())
                        {
                            T_WorkingCalendar_H t_WorkingCalendar_H = this.GetWorkingCalendarById(this.WorkingCalendarID, new DB());

                            //Show data
                            this.ShowData(t_WorkingCalendar_H);

                            //Set Mode
                            this.ProcessMode(Mode.View);

                            //Set Success
                            this.Success = true;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Check input
        /// </summary>
        /// <returns>Valid:true, Invalid:false</returns>
        private bool CheckInput()
        {
            this.flagSetFocus = 0;
            using (DB db = new DB())
            {
                // WorkingCalendarCD
                if (this.txtCalendarCD.IsEmpty)
                {
                    this.SetMessage(this.txtCalendarCD.ID, M_Message.MSG_REQUIRE, "勤務カレンダーコード");
                }
                else
                {
                    if (this.Mode == Mode.Insert)
                    {
                        // Check WorkingCalendar by CalendarCD 
                        if (this.GetWorkingCalendarByCd(this.txtCalendarCD.Text, db) != null)
                        {
                            this.SetMessage(this.txtCalendarCD.ID, M_Message.MSG_EXIST_CODE, "勤務カレンダーコード");
                        }
                    }
                }

                if (this.txtCalendarName.IsEmpty)
                {
                    this.SetMessage(this.txtCalendarName.ID, M_Message.MSG_REQUIRE, "勤務カレンダ名称");
                }

                // check User exist
                string[] arrayUid = treeViewSave.Value.Split('|');

                System.Text.StringBuilder lstUserMessage = new System.Text.StringBuilder();
                if (arrayUid[0] != string.Empty)
                {
                    List<int> lstUIdError = new List<int>();
                    IList<T_WorkingCalendar_U> lstTWorkingCalendarUAll;
                    IList<T_WorkingCalendar_U> lstTWorkingCalendarU;

                    T_WorkingCalendar_UService tWorkingCalendarUSer = new T_WorkingCalendar_UService(db);
                    UserService userSer = new UserService(db);
                    M_User mUser = new M_User();

                    lstTWorkingCalendarU = tWorkingCalendarUSer.GetAll();
                    for (int i = 0; i < arrayUid.Length; i++)
                    {
                        lstTWorkingCalendarUAll = tWorkingCalendarUSer.GetByUId(int.Parse(arrayUid[i]));

                        lstTWorkingCalendarU.Clear();
                        foreach (var item in lstTWorkingCalendarUAll)
                        {
                            if (item.HID != this.WorkingCalendarID)
                            {
                                lstTWorkingCalendarU.Add(item);
                            }
                        }

                        //check time
                        if (checkTimeWorkingCalendar(lstTWorkingCalendarU, this.WorkingCalendarID))
                        {
                            lstUIdError.Add(int.Parse(arrayUid[i]));
                        }
                    }

                    if (lstUIdError.Count > 0)
                    {
                        foreach (var item in lstUIdError)
                        {
                            mUser = userSer.GetByID(item);
                            lstUserMessage.Append("</br>");
                            lstUserMessage.Append("&nbsp;");
                            lstUserMessage.Append(OMS.Utilities.EditDataUtil.ToFixCodeShow(mUser.UserCD, M_User.MAX_USER_CODE_SHOW));
                            lstUserMessage.Append("&nbsp;");
                            lstUserMessage.Append(mUser.UserName1);
                        }

                        // set class 
                        HtmlGenericControl divModelUser = (HtmlGenericControl)this.Master.FindControl("MainContent").FindControl("divModelUser");

                        //string errorEndTimeId = txtEndtime.ID;

                        this.SetMessage(this.divModelUser.ID, M_Message.MSG_VALUE_EXIST_TREEVIEW, lstUserMessage);

                        this.flagSetFocus = 1;
                    }
                }

                if (!this.txtInitialDate.IsEmpty)
                {
                    //Draw Calendar with new Hidden Value
                    DataTable dt = InitDataTableHeader();
                    DateTime fromDate = DateTime.Parse(txtInitialDate.Text);

                    string strfromDate = string.Format(Constants.FMT_YMD, fromDate);
                    dt = SetDataTableHeader(dt, fromDate);

                    rptWorkingCalendarHeader.DataSource = dt;
                    rptWorkingCalendarHeader.DataBind();
                }

                if (this.txtInitialDate.IsEmpty)
                {
                    this.SetMessage(this.txtInitialDate.ID, M_Message.MSG_REQUIRE, "起算日");

                    DataTable dtEmty = InitDataTableHeader();
                    rptWorkingCalendarHeader.DataSource = dtEmty;
                    rptWorkingCalendarHeader.DataBind();
                    txtHolidayInMonth1.Text = string.Empty;
                }

            }
            //Check error
            return !base.HaveError;
        }

        /// <summary>
        /// Process Mode
        /// </summary>
        /// <param name="mode">Mode</param>
        private void ProcessMode(Mode mode)
        {
            bool isReadonly;

            //Set Model
            this.Mode = mode;

            bool canEditPaidLeave = IsExistsCalendar(this.WorkingCalendarID);

            //Check model
            switch (mode)
            {
                case Mode.Insert:
                case Mode.Update:


                    if (this.Mode == Mode.Update)
                    {
                        this.txtCalendarCD.ReadOnly = true;
                    }
                    else
                    {
                        this.txtCalendarCD.ReadOnly = false;
                        base.DisabledLink(this.btnClearInsert, true);
                        base.DisabledLink(this.btnClearInsertBottom, true);
                        base.DisabledLink(this.btnInsert, !base._authority.IsWorkCalendarNew);
                    }
                    if (this.Mode == Mode.Update && !base._authority.IsWorkCalendarEdit)
                    {
                        isReadonly = true;
                    }
                    else
                    {
                        isReadonly = false;
                    }

                    break;

                default:

                    this.txtCalendarCD.ReadOnly = true;
                    base.DisabledLink(this.btnInsert, !base._authority.IsWorkCalendarNew);
                    base.DisabledLink(this.btnInsertBottom, !base._authority.IsWorkCalendarNew);

                    base.DisabledLink(this.btnEdit, !base._authority.IsWorkCalendarEdit && !canEditPaidLeave);
                    base.DisabledLink(this.btnEditBottom, !base._authority.IsWorkCalendarEdit && !canEditPaidLeave);

                    base.DisabledLink(this.btnDelete, !base._authority.IsWorkCalendarDelete);
                    base.DisabledLink(this.btnDeleteBottom, !base._authority.IsWorkCalendarDelete);

                    isReadonly = true;
                    break;
            }

            //Lock control
            this.txtCalendarName.ReadOnly = isReadonly;

            if (checkApproval())
            {
                this.txtInitialDate.ReadOnly = true;
                this.isApproval = true;
            }
            else
            {
                this.txtInitialDate.ReadOnly = isReadonly;
                this.isApproval = false;
            }

        }

        private bool checkTimeWorkingCalendar(IList<T_WorkingCalendar_U> lstTWorkingCalendarU, int WorkingCalendarID)
        {
            using (DB db = new DB())
            {
                T_WorkingCalendar_H tWorkingCalendarH = new T_WorkingCalendar_H();
                T_WorkingCalendar_HService tWorkingCalendarHSer = new T_WorkingCalendar_HService(db);

                DateTime startDateUser;
                DateTime endDateUser;
                DateTime startDateWorkingCalendar;
                DateTime endDateWorkingCalendar;

                // -1 for mode new

                startDateUser = DateTime.Parse(txtInitialDate.Text);
                endDateUser = startDateUser.AddMonths(12).AddDays(-1);

                if (lstTWorkingCalendarU.Count == 0)
                {
                    return false;
                }
                else
                {
                    foreach (var item in lstTWorkingCalendarU)
                    {
                        tWorkingCalendarH = tWorkingCalendarHSer.GetByID(item.HID);
                        startDateWorkingCalendar = tWorkingCalendarH.InitialDate;
                        endDateWorkingCalendar = startDateWorkingCalendar.AddMonths(12).AddDays(-1);

                        //Check time
                        if (!(startDateWorkingCalendar > endDateUser || endDateWorkingCalendar < startDateUser))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Get Default Value
        /// </summary>
        private void GetDefaultValue()
        {
            //InitCombobox 
            InitCombobox(this.cmbWorkingSystemData);
            DataTable dtHiddenValue = new DataTable();
        }

        /// <summary>
        /// Init DataTableHeader
        /// </summary>
        /// <param></param>
        protected DataTable InitDataTableHeader()
        {
            DataTable dtHearder = new DataTable();

            dtHearder.Columns.Add("WorkingCalendarID", typeof(int));
            dtHearder.Columns.Add("Index", typeof(int));
            dtHearder.Columns.Add("IndexTable", typeof(string));
            dtHearder.Columns.Add("InitialDate", typeof(string));
            dtHearder.Columns.Add("FromDate", typeof(string));
            dtHearder.Columns.Add("ToDate", typeof(string));
            dtHearder.Columns.Add("Text", typeof(string));
            dtHearder.Columns.Add("HtmlHeader", typeof(string));
            dtHearder.Columns.Add("HtmlBottom", typeof(string));
            dtHearder.Columns.Add("StateContent", typeof(string));
            dtHearder.Columns.Add("StyleButton", typeof(string));
            dtHearder.Columns.Add("ButtonID", typeof(string));
            dtHearder.Columns.Add("ButtonValue", typeof(string));
            dtHearder.Columns.Add("HiddenButton", typeof(string));
            dtHearder.Columns.Add("HiddenApproval", typeof(string));

            return dtHearder;

        }

        /// <summary>
        /// Init DataTableContent
        /// </summary>
        /// <param></param>
        protected DataTable InitDataTableContent()
        {
            DataTable dtContent = new DataTable();

            dtContent.Columns.Add("Index", typeof(int));
            dtContent.Columns.Add("Date", typeof(DateTime));
            dtContent.Columns.Add("Color", typeof(string));
            dtContent.Columns.Add("DayOfWeek", typeof(string));
            dtContent.Columns.Add("HtmlText", typeof(string));

            return dtContent;

        }

        /// <summary>
        /// Set Data for DataTableHeader
        /// </summary>
        /// <param></param>
        protected DataTable SetDataTableHeader(DataTable dtHearder, DateTime initialDate)
        {

            DateTime fromDate = initialDate;

            DateTime dateIndex = initialDate.AddMonths(-1);
            string dateIndexValue = dateIndex.Month.ToString();

            // set value TotalDate
            this.txtCountDayInMonthTotal.Text = ((initialDate.AddYears(1) - initialDate).TotalDays).ToString();
            this.txtCountDayInMonthTotal_xs.Text = ((initialDate.AddYears(1) - initialDate).TotalDays).ToString();

            DateTime toDate = new DateTime();

            // keep value date in month 2
            int keepDate = fromDate.Day;
            int nextDate = fromDate.AddMonths(1).Day;
            for (int i = 0; i < 12; i++)
            {
                fromDate = initialDate.AddMonths(i);
                toDate = initialDate.AddMonths(i + 1).AddDays(-1);

                string stInitialDate = string.Format(Constants.FMR_DATE_YMD, fromDate) + "~" + string.Format(Constants.FMR_DATE_YMD, toDate);

                DataRow drow = dtHearder.NewRow();
                dtHearder.Rows.Add(drow);

                int id = i + 1;

                if (fromDate.Month != toDate.Month)
                {
                    dateIndexValue = fromDate.Month.ToString() + "~" + toDate.Month.ToString() + "月";
                }
                else
                {
                    dateIndexValue = fromDate.Month.ToString() + "月";
                }

                dtHearder.Rows[i].SetField("Index", id);
                dtHearder.Rows[i].SetField("IndexTable", dateIndexValue);
                dtHearder.Rows[i].SetField("InitialDate", stInitialDate);
                dtHearder.Rows[i].SetField("FromDate", string.Format(Constants.FMR_DATE_YMD, fromDate));
                dtHearder.Rows[i].SetField("ToDate", string.Format(Constants.FMR_DATE_YMD, toDate));
                dtHearder.Rows[i].SetField("ButtonID", "btnWorkingCalendarDetail_" + i.ToString());

                if (checkEdit(fromDate))
                {
                    dtHearder.Rows[i].SetField("HiddenApproval", "false");
                }
                else
                {
                    dtHearder.Rows[i].SetField("HiddenApproval", "true");
                }



                // set DateState
                if (this.Mode == Mode.Insert)
                {
                    this.WorkingCalendarID = -1;
                }
                string StateContent = this.WorkingCalendarID + ","
                                                             + string.Format(Constants.FMR_DATE_YMD, fromDate) + ","
                                                             + string.Format(Constants.FMR_DATE_YMD, toDate);

                dtHearder.Rows[i].SetField("StateContent", StateContent);

                if (i == 0 || i == 3 || i == 6 || i == 9)
                {
                    dtHearder.Rows[i].SetField("HtmlHeader", "<div class='row'>");
                    dtHearder.Rows[i].SetField("HtmlBottom", "");
                }
                else if (i == 2 || i == 5 || i == 8 || i == 11)
                {
                    dtHearder.Rows[i].SetField("HtmlHeader", "");
                    dtHearder.Rows[i].SetField("HtmlBottom", "</div>");
                }
                else
                {
                    dtHearder.Rows[i].SetField("HtmlHeader", "");
                    dtHearder.Rows[i].SetField("HtmlBottom", "");
                }

                //set value for table display.
                string strMonthIndex = "txtMonthIndex" + (i + 1).ToString();
                string strMonthIndex_xs = "txtMonthIndex" + (i + 1).ToString() + "_xs";
                Label controltxtMonthIndex = this.Master.FindControl("MainContent").FindControl(strMonthIndex) as Label;
                Label controltxtMonthIndex_xs = this.Master.FindControl("MainContent").FindControl(strMonthIndex_xs) as Label;
                controltxtMonthIndex.Text = dateIndexValue;
                controltxtMonthIndex_xs.Text = dateIndexValue;
                // set from day
                fromDate = fromDate.AddMonths(1);

            }

            // set default for keepDate
            keepDate = 0;

            string[] listHiddButtonValue = new string[12];
            string HiddenValue;
            for (int i = 0; i < 12; i++)
            {
                HiddenValue = "HiddButtonValue" + i.ToString();
                listHiddButtonValue[i] = Request.Form[HiddenValue];
                if (listHiddButtonValue[i] == null)
                {
                    Array.Clear(listHiddButtonValue, 0, listHiddButtonValue.Length);
                    break;
                }
            }

            // Set style button 
            if (this.Mode == Mode.View)
            {
                T_WorkingCalendar_H t_WorkingCalendar_H;
                using (DB db = new DB())
                {
                    T_WorkingCalendar_HService t_WorkingCalendar_HService = new T_WorkingCalendar_HService(db);
                    t_WorkingCalendar_H = t_WorkingCalendar_HService.GetByID(WorkingCalendarID);
                }

                if (t_WorkingCalendar_H.AgreementFlag1 == 1)
                {
                    SetButtonHeaderDefaut(ref dtHearder, 0, "btn-agreement", "協定済", 1);
                }
                else
                {
                    SetButtonHeaderDefaut(ref dtHearder, 0, "btn-info", "未確定", 0);
                }

                if (t_WorkingCalendar_H.AgreementFlag2 == 1)
                {
                    SetButtonHeaderDefaut(ref dtHearder, 1, "btn-agreement", "協定済", 1);
                }
                else
                {
                    SetButtonHeaderDefaut(ref dtHearder, 1, "btn-info", "未確定", 0);
                }

                if (t_WorkingCalendar_H.AgreementFlag3 == 1)
                {
                    SetButtonHeaderDefaut(ref dtHearder, 2, "btn-agreement", "協定済", 1);
                }
                else
                {
                    SetButtonHeaderDefaut(ref dtHearder, 2, "btn-info", "未確定", 0);
                }

                if (t_WorkingCalendar_H.AgreementFlag4 == 1)
                {
                    SetButtonHeaderDefaut(ref dtHearder, 3, "btn-agreement", "協定済", 1);
                }
                else
                {
                    SetButtonHeaderDefaut(ref dtHearder, 3, "btn-info", "未確定", 0);
                }

                if (t_WorkingCalendar_H.AgreementFlag5 == 1)
                {
                    SetButtonHeaderDefaut(ref dtHearder, 4, "btn-agreement", "協定済", 1);
                }
                else
                {
                    SetButtonHeaderDefaut(ref dtHearder, 4, "btn-info", "未確定", 0);
                }

                if (t_WorkingCalendar_H.AgreementFlag6 == 1)
                {
                    SetButtonHeaderDefaut(ref dtHearder, 5, "btn-agreement", "協定済", 1);
                }
                else
                {
                    SetButtonHeaderDefaut(ref dtHearder, 5, "btn-info", "未確定", 0);
                }

                if (t_WorkingCalendar_H.AgreementFlag7 == 1)
                {
                    SetButtonHeaderDefaut(ref dtHearder, 6, "btn-agreement", "協定済", 1);
                }
                else
                {
                    SetButtonHeaderDefaut(ref dtHearder, 6, "btn-info", "未確定", 0);
                }

                if (t_WorkingCalendar_H.AgreementFlag8 == 1)
                {
                    SetButtonHeaderDefaut(ref dtHearder, 7, "btn-agreement", "協定済", 1);
                }
                else
                {
                    SetButtonHeaderDefaut(ref dtHearder, 7, "btn-info", "未確定", 0);
                }

                if (t_WorkingCalendar_H.AgreementFlag9 == 1)
                {
                    SetButtonHeaderDefaut(ref dtHearder, 8, "btn-agreement", "協定済", 1);
                }
                else
                {
                    SetButtonHeaderDefaut(ref dtHearder, 8, "btn-info", "未確定", 0);
                }

                if (t_WorkingCalendar_H.AgreementFlag10 == 1)
                {
                    SetButtonHeaderDefaut(ref dtHearder, 9, "btn-agreement", "協定済", 1);
                }
                else
                {
                    SetButtonHeaderDefaut(ref dtHearder, 9, "btn-info", "未確定", 0);
                }

                if (t_WorkingCalendar_H.AgreementFlag11 == 1)
                {
                    SetButtonHeaderDefaut(ref dtHearder, 10, "btn-agreement", "協定済", 1);
                }
                else
                {
                    SetButtonHeaderDefaut(ref dtHearder, 10, "btn-info", "未確定", 0);
                }

                if (t_WorkingCalendar_H.AgreementFlag12 == 1)
                {
                    SetButtonHeaderDefaut(ref dtHearder, 11, "btn-agreement", "協定済", 1);
                }
                else
                {
                    SetButtonHeaderDefaut(ref dtHearder, 11, "btn-info", "未確定", 0);
                }

            }
            else
            {
                if (listHiddButtonValue[0] == null || ModeClearAfterClear == "Clear")
                {
                    for (int i = 0; i < 12; i++)
                    {
                        SetButtonHeaderDefaut(ref dtHearder, i, "btn-info", "未確定", 0);

                    }
                }
                else
                {
                    for (int i = 0; i < 12; i++)
                    {
                        if (listHiddButtonValue[i] == "0")
                        {
                            dtHearder.Rows[i].SetField("StyleButton", "btn-info");
                            dtHearder.Rows[i].SetField("Text", "未確定");
                        }
                        else
                        {
                            dtHearder.Rows[i].SetField("StyleButton", "btn-agreement");
                            dtHearder.Rows[i].SetField("Text", "協定済");
                        }

                        dtHearder.Rows[i].SetField("HiddenButton", "<input type='hidden' name ='HiddButtonValue" + i + "' id ='HiddButtonValue" + i + "'  value= '" + listHiddButtonValue[i] + "' runat='server'>");
                    }
                }

            }
            return dtHearder;
        }

        /// <summary>
        /// set value defaut for Button Hearder
        /// </summary>
        /// <param></param>
        protected void SetButtonHeaderDefaut(ref DataTable dt, int index, string style, string text, int value)
        {
            dt.Rows[index].SetField("StyleButton", style);
            dt.Rows[index].SetField("Text", text);
            dt.Rows[index].SetField("HiddenButton", "<input type='hidden' name ='HiddButtonValue" + index + "' id ='HiddButtonValue" + index + "'  value='" + value + "' runat='server'>");
        }

        /// <summary>
        /// rptWorkingCalendarHeader_ItemDataBound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptWorkingCalendarHeader_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            int HolidayInMonth = 0;
            int WorkingDayInMonth = 0;

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView dataItem = e.Item.DataItem as DataRowView;
                DateTime fromDate = DateTime.Parse(dataItem.Row["FromDate"].ToString());
                DateTime toDate = DateTime.Parse(dataItem.Row["ToDate"].ToString());
                DateTime dt;
                DateTime fromHiddenDate;

                //get dtHiddenValue
                DataTable dtHiddenValue = InitDataTableHiddenValue();
                {
                    if (InitDateSave.Value != "")
                    {
                        dt = DateTime.Parse(InitDateSave.Value);
                        fromHiddenDate = dt;
                    }
                    else
                    {
                        dt = DateTime.Parse(txtInitialDate.Text);
                        fromHiddenDate = dt;
                    }
                }


                DateTime toHiddenDate = dt.AddYears(1).AddDays(-1);
                int totaldate = int.Parse(((toHiddenDate - fromHiddenDate).TotalDays + 1).ToString());

                // get value hidden date
                for (int i = 0; i < totaldate; i++)
                {
                    string idHiddenDate = FormatDateForHidden(dt);
                    string idHiddenWorkingSystemValue = "workingSystemCD" + FormatDateForHidden(dt);
                    string idHiddenWorkingTextHoliday = "textHoliday" + FormatDateForHidden(dt);
                    string idHiddenCheckBoxIvent = "checkBoxIvent" + FormatDateForHidden(dt);
                    string idHiddenCheckBoxPaidLeave = "checkBoxPaidLeave" + FormatDateForHidden(dt);

                    string hiddenValue = Request.Form[idHiddenDate];
                    string workingSysteValue = Request.Form[idHiddenWorkingSystemValue];
                    string workingTextHolidayValue = Request.Form[idHiddenWorkingTextHoliday];
                    string checkBoxIventValue = Request.Form[idHiddenCheckBoxIvent];
                    string checkBoxPaidLeaveValue = Request.Form[idHiddenCheckBoxPaidLeave];
                    if (workingTextHolidayValue == null)
                    {
                        workingTextHolidayValue = string.Empty;
                    }
                    if (checkBoxIventValue == null)
                    {
                        checkBoxIventValue = "0";
                    }
                    if (checkBoxPaidLeaveValue == null)
                    {
                        checkBoxPaidLeaveValue = "0";
                    }
                    if (hiddenValue == null)
                    {
                        dtHiddenValue.Clear();
                        break;
                    }
                    else
                    {
                        DataRow drow = dtHiddenValue.NewRow();
                        dtHiddenValue.Rows.Add(drow);

                        dtHiddenValue.Rows[i].SetField("Date", idHiddenDate);
                        dtHiddenValue.Rows[i].SetField("Value", hiddenValue.Substring(0, 1));
                        dtHiddenValue.Rows[i].SetField("WorkingSystemValue", workingSysteValue.Substring(0, 1));
                        dtHiddenValue.Rows[i].SetField("TextHoliday", workingTextHolidayValue);
                        dtHiddenValue.Rows[i].SetField("CheckBoxIvent", checkBoxIventValue);
                        dtHiddenValue.Rows[i].SetField("CheckBoxPaidLeave", checkBoxPaidLeaveValue);
                        dt = dt.AddDays(1);
                    }
                }

                ///InitDataTableConfig
                DataTable dtConfig = new DataTable();

                IList<M_Config_D> listHoliDate_Type;
                IList<M_WorkingSystem> lstM_WorkingSystem;
                IList<T_WorkingCalendar_D> lstT_WorkingCalendar_D;
                IList<T_PaidLeave> lstPaidLeave;
                M_WorkingSystem mWorkingSystem = new M_WorkingSystem();

                //set workingTime
                TimeSpan workingTime = new TimeSpan(0, 0, 0);
                using (DB db = new DB())
                {

                    Config_DService config_DService = new Config_DService(db);
                    listHoliDate_Type = config_DService.GetListByConfigCd(M_Config_H.CONFIG_CD_HOLIDAY_TYPE);

                    WorkingSystemService workingSystemService = new WorkingSystemService(db);
                    lstM_WorkingSystem = workingSystemService.GetAll();

                    T_WorkingCalendar_DService t_WorkingCalendar_DService = new T_WorkingCalendar_DService(db);
                    //lstT_WorkingCalendar_D = t_WorkingCalendar_DService.GetListWorkingCalendarByHId(WorkingCalendarID);
                    lstT_WorkingCalendar_D = GetListWorkingCalendarInMonth(WorkingCalendarID, fromDate, toDate, db);

                    T_PaidLeaveService paidLeaveSer = new T_PaidLeaveService(db);
                    lstPaidLeave = paidLeaveSer.GetListInMonths(WorkingCalendarID, this.LoginInfo.User.ID, fromDate, toDate);

                    // get Working System with CD 1
                    mWorkingSystem = workingSystemService.GetByWorkingSystemCd(FULL_TIME_EMPLOYMENT);

                }

                // array Break Time.
                DateTime[] arrBreakTime = new DateTime[8];

                if (mWorkingSystem != null)
                {
                    if (mWorkingSystem.WorkingType == (int)WorkingType.WorkFullTime)
                    {
                        string[] systemWorkingStart = new string[2];
                        string[] systemWorkingEnd = new string[2];

                        if (mWorkingSystem.Working_Start != null)
                        {
                            systemWorkingStart = (Utilities.CommonUtil.IntToTime((int)mWorkingSystem.Working_Start, true)).Split(':');
                            systemWorkingEnd = (Utilities.CommonUtil.IntToTime((int)mWorkingSystem.Working_End, true)).Split(':');
                        }
                        else
                        {
                            systemWorkingStart[0] = "0";
                            systemWorkingStart[1] = "0";
                            systemWorkingEnd[0] = "0";
                            systemWorkingEnd[1] = "0";
                        }

                        //get breakTime
                        if (mWorkingSystem.Break1_Start != null)
                        {
                            arrBreakTime[0] = ConvertIntToDateTime((int)mWorkingSystem.Break1_Start);
                        }
                        if (mWorkingSystem.Break1_End != null)
                        {
                            arrBreakTime[1] = ConvertIntToDateTime((int)mWorkingSystem.Break1_End);
                        }
                        if (mWorkingSystem.Break2_Start != null)
                        {
                            arrBreakTime[2] = ConvertIntToDateTime((int)mWorkingSystem.Break2_Start);
                        }
                        if (mWorkingSystem.Break2_End != null)
                        {
                            arrBreakTime[3] = ConvertIntToDateTime((int)mWorkingSystem.Break2_End);
                        }
                        if (mWorkingSystem.Break3_Start != null)
                        {
                            arrBreakTime[4] = ConvertIntToDateTime((int)mWorkingSystem.Break3_Start);
                        }
                        if (mWorkingSystem.Break3_End != null)
                        {
                            arrBreakTime[5] = ConvertIntToDateTime((int)mWorkingSystem.Break3_End);
                        }
                        if (mWorkingSystem.Break4_Start != null)
                        {
                            arrBreakTime[6] = ConvertIntToDateTime((int)mWorkingSystem.Break4_Start);
                        }
                        if (mWorkingSystem.Break4_End != null)
                        {
                            arrBreakTime[7] = ConvertIntToDateTime((int)mWorkingSystem.Break4_End);
                        }

                        //Time Work reality
                        DateTime workST = new DateTime(1, 1, 1).AddHours(int.Parse(systemWorkingStart[0])).AddMinutes(int.Parse(systemWorkingStart[1]));
                        DateTime workET = new DateTime(1, 1, 1).AddHours(int.Parse(systemWorkingEnd[0])).AddMinutes(int.Parse(systemWorkingEnd[1]));

                        if (mWorkingSystem.BreakType == 0)
                        {
                            //Working time
                            workingTime = CalDurationWorkTime(workST, workET, arrBreakTime);
                        }
                        else if (mWorkingSystem.BreakType == 1)
                        {
                            TimeSpan BreakTimeHour = new TimeSpan();

                            // BreakTime Hours
                            if (arrBreakTime[0] != null)
                            {
                                BreakTimeHour = new TimeSpan(arrBreakTime[0].Hour, arrBreakTime[0].Minute, 0);
                            }
                            
                            //Working time
                            workingTime = CalDurationWorkTime(workST, workET, arrBreakTime).Subtract(BreakTimeHour);

                        }
                        else if (mWorkingSystem.BreakType == 2)
                        {
                            DateTime[] arrBreakTime_Type2 = new DateTime[40];
                            TimeSpan TimeHourToBreak = new TimeSpan();
                            TimeSpan TimeBreak = new TimeSpan();
                            DateTime sTime = new DateTime(1, 1, 1).AddHours(workST.Hour).AddMinutes(workST.Minute);
                            DateTime eTime = new DateTime(1, 1, 1).AddHours(24);
                            if (workST.Date == workET.Date)
                            {
                                eTime = new DateTime(1, 1, 1).AddHours(workET.Hour).AddMinutes(workET.Minute);
                            }
                            else
                            {
                                eTime = new DateTime(1, 1, 2).AddHours(workET.Hour).AddMinutes(workET.Minute);
                            }

                            // BreakTime Hours
                            if (mWorkingSystem.Break1_Start == null || mWorkingSystem.Break1_End == null)
                            {
                                workingTime = eTime.Subtract(sTime);
                            }
                            else
                            {
                                TimeHourToBreak = new TimeSpan(arrBreakTime[0].Hour, arrBreakTime[0].Minute, 0);
                                TimeBreak = new TimeSpan(arrBreakTime[1].Hour, arrBreakTime[1].Minute, 0);

                                if (eTime.Subtract(sTime) > TimeHourToBreak)
                                {
                                    arrBreakTime_Type2[0] = sTime.Add(TimeHourToBreak);
                                    arrBreakTime_Type2[1] = sTime.Add(TimeHourToBreak).Add(TimeBreak);
                                    sTime = sTime.Add(TimeHourToBreak).Add(TimeBreak);

                                    int i = 2;
                                    while (eTime > sTime)
                                    {
                                        arrBreakTime_Type2[i] = sTime.Add(TimeHourToBreak);
                                        arrBreakTime_Type2[i + 1] = sTime.Add(TimeHourToBreak).Add(TimeBreak);
                                        if (arrBreakTime_Type2[i + 1] > eTime)
                                        {
                                            arrBreakTime_Type2[i] = new DateTime();
                                            arrBreakTime_Type2[i + 1] = new DateTime();
                                            break;
                                        }
                                        else
                                        {
                                            sTime = sTime.Add(TimeHourToBreak).Add(TimeBreak);
                                            i = i + 2;
                                        }
                                    }
                                }
                            }

                            //Working time
                            workingTime = CalDurationWorkTime(workST, workET, arrBreakTime_Type2);
                        }
                    }
                }

                // set Working Time In Minute 
                int workingTimeMinute = (int)workingTime.TotalMinutes;

                // set value workingtimesystem for hidden in view
                this.WorkingTimeSystem.Value = workingTimeMinute.ToString();

                int workingTimeInMonthValue = 0;
                string workingTimeInMonthDisplay = string.Empty;

                double WorkingTimeWeeklyAverage = 0;
                int WorkingTimeWeeklyAverageRoundUpHour = 0;
                int WorkingTimeWeeklyAverageRoundUpMinute = 0;
                string WorkingTimeWeeklyAverageRoundUp = string.Empty;

                var currentMonth = fromDate.Month;
                int dayOfWeek = (int)fromDate.DayOfWeek;
                DataTable dtContent = InitDataTableContent();
                var totalday = (toDate - fromDate).TotalDays + 1;

                // set for txt CountDayInMonth.
                int CountDayInMonth = int.Parse(totalday.ToString());
                DateTime currentDate = fromDate;
                int currentRow = 0;
                string workingType = string.Empty;
                string holidateName = string.Empty;
                string workingSystemCD = string.Empty;
                string strPaintDay = string.Empty;
                string backgroundCss = string.Empty;
                string textCss = string.Empty;
                string valueCheckBoxIvent = string.Empty;
                string valuePaidLeave = string.Empty;
                List<string> lstH = new List<string>();
                for (var j = 0; j < totalday; j++)
                {

                    if (j == 0)
                    {
                        // for first row.
                        DataRow drowTr = dtContent.NewRow();
                        dtContent.Rows.Add(drowTr);
                        dtContent.Rows[currentRow].SetField("HtmlText", "<tr>");

                        for (var k = 0; k < 7 - dayOfWeek; k++)
                        {
                            DataRow drow = dtContent.NewRow();
                            dtContent.Rows.Add(drow);
                            lstH = CheckHoliday(currentDate, listHoliDate_Type, lstT_WorkingCalendar_D, lstPaidLeave, dtHiddenValue, this.Mode);
                            workingType = lstH[0];
                            workingSystemCD = lstH[1];
                            holidateName = lstH[2];
                            valueCheckBoxIvent = lstH[3];
                            valuePaidLeave = lstH[4];
                            checkHolidayOrWorkingDay(currentDate, ref backgroundCss, workingType, workingSystemCD, ref HolidayInMonth, ref WorkingDayInMonth, holidateName, ref textCss, valueCheckBoxIvent, valuePaidLeave);

                            strPaintDay = InsertOneDay(backgroundCss, currentDate, holidateName, workingType, workingSystemCD, textCss, valueCheckBoxIvent, valuePaidLeave);
                            if (k == 0)
                            {
                                for (int count = 0; count < dayOfWeek; count++)
                                {
                                    strPaintDay = "<td>&nbsp;</td>" + strPaintDay;
                                }
                            }

                            dtContent.Rows[currentRow].SetField("HtmlText", strPaintDay);

                            currentDate = currentDate.AddDays(1);
                            currentRow++;

                        }
                        totalday = totalday - (7 - dayOfWeek - 1);

                        // for first row.
                        DataRow drowTrLast = dtContent.NewRow();
                        dtContent.Rows.Add(drowTrLast);
                        dtContent.Rows[currentRow].SetField("HtmlText", "</tr>");
                    }
                    else
                    {
                        DataRow drow = dtContent.NewRow();
                        dtContent.Rows.Add(drow);

                        lstH = CheckHoliday(currentDate, listHoliDate_Type, lstT_WorkingCalendar_D, lstPaidLeave, dtHiddenValue, this.Mode);
                        workingType = lstH[0];
                        workingSystemCD = lstH[1];
                        holidateName = lstH[2];
                        valueCheckBoxIvent = lstH[3];
                        valuePaidLeave = lstH[4];

                        checkHolidayOrWorkingDay(currentDate, ref backgroundCss, workingType, workingSystemCD, ref HolidayInMonth, ref WorkingDayInMonth, holidateName, ref textCss, valueCheckBoxIvent, valuePaidLeave);

                        if (j == 1 || j == 8 || j == 15 || j == 22)
                        {
                            strPaintDay = InsertOneDay(backgroundCss, currentDate, holidateName, workingType, workingSystemCD, textCss, valueCheckBoxIvent, valuePaidLeave);
                            strPaintDay = "<tr>" + strPaintDay;
                            dtContent.Rows[currentRow].SetField("HtmlText", strPaintDay);

                        }
                        else if (j == 7 || j == 14 || j == 21 || j == 28)
                        {
                            strPaintDay = InsertOneDay(backgroundCss, currentDate, holidateName, workingType, workingSystemCD, textCss, valueCheckBoxIvent, valuePaidLeave);
                            strPaintDay = strPaintDay + "</tr>";
                            dtContent.Rows[currentRow].SetField("HtmlText", strPaintDay);

                        }
                        else
                        {
                            strPaintDay = InsertOneDay(backgroundCss, currentDate, holidateName, workingType, workingSystemCD, textCss, valueCheckBoxIvent, valuePaidLeave);
                            dtContent.Rows[currentRow].SetField("HtmlText", strPaintDay);
                        }
                        if (j == totalday - 1)
                        {
                            // check for last rows
                            strPaintDay = InsertOneDay(backgroundCss, currentDate, holidateName, workingType, workingSystemCD, textCss, valueCheckBoxIvent, valuePaidLeave);
                            int dayOfWeekLast = 7 - (int)currentDate.DayOfWeek - 1;
                            for (int count = 0; count < dayOfWeekLast; count++)
                            {
                                strPaintDay = strPaintDay + "<td>&nbsp;</td>";
                            }

                            dtContent.Rows[currentRow].SetField("HtmlText", strPaintDay);
                        }

                        currentDate = currentDate.AddDays(1);
                        currentRow++;
                    }
                }

                // set Working Time In Minute
                workingTimeInMonthValue = workingTimeMinute * WorkingDayInMonth;
                workingTimeInMonthDisplay = (workingTimeInMonthValue / 60).ToString().PadLeft(2, '0') + ":" + (workingTimeInMonthValue % 60).ToString().PadLeft(2, '0');
                WorkingTimeWeeklyAverage = double.Parse(workingTimeInMonthValue.ToString()) / (double.Parse(CountDayInMonth.ToString()) / 7);
                WorkingTimeWeeklyAverageRoundUpHour = int.Parse(Math.Round(WorkingTimeWeeklyAverage, 0, MidpointRounding.AwayFromZero).ToString());
                WorkingTimeWeeklyAverageRoundUpMinute = int.Parse(Math.Ceiling(WorkingTimeWeeklyAverage % 60).ToString());
                WorkingTimeWeeklyAverageRoundUp = (WorkingTimeWeeklyAverageRoundUpHour / 60).ToString().PadLeft(2, '0') + ":" + WorkingTimeWeeklyAverageRoundUpMinute.ToString().PadLeft(2, '0');

                string index = (e.Item.ItemIndex + 1).ToString();
                string strHolidayInMonth = "txtHolidayInMonth" + index;
                string strHolidayInMonth_xs = "txtHolidayInMonth" + index + "_xs";
                string strWorkingDate = "txtWorkingDate" + index;
                string strWorkingDate_xs = "txtWorkingDate" + index + "_xs";
                string strCountDayInMonth = "txtCountDayInMonth" + index;
                string strCountDayInMonth_xs = "txtCountDayInMonth" + index + "_xs";
                string strWorkingTimeInMonth = "txtWorkingTimeInMonth" + index;
                string strWorkingTimeInMonth_xs = "txtWorkingTimeInMonth" + index + "_xs";
                string strWorkingTimeWeeklyAverage = "txtWorkingTimeWeeklyAverage" + index;
                string strWorkingTimeWeeklyAverage_xs = "txtWorkingTimeWeeklyAverage" + index + "_xs";

                Label controltxtHolidayInMonth = this.Master.FindControl("MainContent").FindControl(strHolidayInMonth) as Label;
                Label controltxtHolidayInMonth_xs = this.Master.FindControl("MainContent").FindControl(strHolidayInMonth_xs) as Label;
                Label controltxtWorkingDate = this.Master.FindControl("MainContent").FindControl(strWorkingDate) as Label;
                Label controltxtWorkingDate_xs = this.Master.FindControl("MainContent").FindControl(strWorkingDate_xs) as Label;
                Label controltxtCountDayInMonth = this.Master.FindControl("MainContent").FindControl(strCountDayInMonth) as Label;
                Label controltxtCountDayInMonth_xs = this.Master.FindControl("MainContent").FindControl(strCountDayInMonth_xs) as Label;
                Label controlWorkingTimeInMonth = this.Master.FindControl("MainContent").FindControl(strWorkingTimeInMonth) as Label;
                Label controlWorkingTimeInMonth_xs = this.Master.FindControl("MainContent").FindControl(strWorkingTimeInMonth_xs) as Label;
                Label controlWorkingTimeWeeklyAverage = this.Master.FindControl("MainContent").FindControl(strWorkingTimeWeeklyAverage) as Label;
                Label controlWorkingTimeWeeklyAverage_xs = this.Master.FindControl("MainContent").FindControl(strWorkingTimeWeeklyAverage_xs) as Label;

                // set value for table total
                controltxtHolidayInMonth.Text = HolidayInMonth.ToString();
                controltxtHolidayInMonth_xs.Text = HolidayInMonth.ToString();

                controltxtCountDayInMonth.Text = CountDayInMonth.ToString();
                controltxtCountDayInMonth_xs.Text = CountDayInMonth.ToString();

                controltxtWorkingDate.Text = WorkingDayInMonth.ToString();
                controltxtWorkingDate_xs.Text = WorkingDayInMonth.ToString();

                controlWorkingTimeInMonth.Text = workingTimeInMonthDisplay;
                controlWorkingTimeInMonth_xs.Text = workingTimeInMonthDisplay;

                controlWorkingTimeWeeklyAverage.Text = WorkingTimeWeeklyAverageRoundUp;
                controlWorkingTimeWeeklyAverage_xs.Text = WorkingTimeWeeklyAverageRoundUp;

                Repeater rpChild = e.Item.FindControl("rptWorkingCalendarContent") as Repeater;
                rpChild.DataSource = dtContent;
                rpChild.DataBind();
            }
        }

        /// <summary>
        /// return a string to paint a day
        /// </summary>
        /// <param></param>
        protected string InsertOneDay(string classValue, DateTime currentDate, string textHolidate, string check, string workingSystemCD, string textStyle, string valueCheckBoxIvent, string valuePaidLeave)
        {
            StringBuilder strPaintOneDate = new StringBuilder();
            strPaintOneDate.AppendLine("<td class='" + classValue + "'>");
            strPaintOneDate.AppendLine("<div><div class = '" + textStyle + "'>" + currentDate.Day + "</div></div>");
            strPaintOneDate.AppendLine("</td>");
            strPaintOneDate.AppendLine("<input type='hidden' name ='" + FormatDateForHidden(currentDate) + "' id='" + FormatDateForHidden(currentDate) + "'value='" + check + "' runat='server'>");
            strPaintOneDate.AppendLine("<input type='hidden' name ='textHoliday" + FormatDateForHidden(currentDate) + "' id='textHoliday" + FormatDateForHidden(currentDate) + "'value='" + textHolidate + "'runat='server'>");
            strPaintOneDate.AppendLine("<input type='hidden' name ='workingSystemCD" + FormatDateForHidden(currentDate) + "' id='workingSystemCD" + FormatDateForHidden(currentDate) + "'value='" + workingSystemCD + "'runat='server'>");
            strPaintOneDate.AppendLine("<input type='hidden' name ='checkBoxIvent" + FormatDateForHidden(currentDate) + "' id='checkBoxIvent" + FormatDateForHidden(currentDate) + "'value='" + valueCheckBoxIvent + "'runat='server'>");
            strPaintOneDate.AppendLine("<input type='hidden' name ='checkBoxPaidLeave" + FormatDateForHidden(currentDate) + "' id='checkBoxPaidLeave" + FormatDateForHidden(currentDate) + "'value='" + valuePaidLeave + "'runat='server'>");
            return strPaintOneDate.ToString();
        }

        /// <summary>
        /// check current day (Holiday or Workingday)
        /// </summary>
        /// <param></param>
        protected void checkHolidayOrWorkingDay(DateTime currentDay, ref string backgroundCss, string workingType, string workingSystemCD,
                                                ref int HolidayInMonth, ref int WorkingDayInMonth, string holidayName, ref string textCss, 
                                                string valueCheckBoxIvent, string valuePaidLeave)
        {
            if (string.IsNullOrEmpty(workingType))
            {
                return;
            }

            switch ((WorkingType)int.Parse(workingType))
            {
                case WorkingType.LegalHoliDay:
                    HolidayInMonth = HolidayInMonth + 1;
                    if (!string.IsNullOrEmpty(holidayName))
                    {
                        backgroundCss = "legal-holiday";
                        textCss = "text-color-holiday";
                    }
                    else
                    {
                        backgroundCss = "legal";
                        textCss = "text-color-holiday";
                    }

                    break;
                case WorkingType.WorkHoliDay:
                    HolidayInMonth = HolidayInMonth + 1;

                    if (!string.IsNullOrEmpty(holidayName))
                    {
                        backgroundCss = "legal-holiday";
                        textCss = "text-color-holiday";
                    }
                    else
                    {
                        backgroundCss = "attendance";
                        textCss = "text-color-saturday";
                    }

                    break;
                case WorkingType.WorkFullTime:
                    WorkingDayInMonth = WorkingDayInMonth + 1;

                    if (workingSystemCD == "4")
                    {
                        backgroundCss = "attendance-full-time";
                    }
                    else if (!string.IsNullOrEmpty(holidayName))
                    {
                        backgroundCss = "work-holiday";
                    }
                    else if (currentDay.DayOfWeek == DayOfWeek.Sunday)
                    {
                        backgroundCss = "work-weekend";
                    }
                    else if (currentDay.DayOfWeek == DayOfWeek.Saturday)
                    {
                        backgroundCss = "work-weekend-saturday";
                    }
                    else
                    {
                        backgroundCss = string.Empty;
                    }
                    textCss = "text-color-normalday";

                    break;
            }

            if (valueCheckBoxIvent == "1")
            {
                textCss = textCss + " checkbox-ivent-circle";
            }

            if (valuePaidLeave == "1")
            {
                backgroundCss = backgroundCss + " paid-leave";
            }
        }

        /// <summary>
        /// Show data on form
        /// </summary>
        /// <param name="T_WorkingCalendar_H">T_WorkingCalendar_H</param>
        private void ShowData(T_WorkingCalendar_H workingCalendar)
        {

            if (workingCalendar != null)
            {
                this.txtCalendarCD.Value = workingCalendar.CalendarCD;
                this.txtCalendarName.Value = workingCalendar.CalendarName;
                this.txtInitialDate.Value = workingCalendar.InitialDate;

                //Save UpdateDate
                this.OldUpdateDate = workingCalendar.UpdateDate;

                // Draw Calendar Year
                DateTime InitialDate = workingCalendar.InitialDate;
                DateTime dt = InitialDate.AddMonths(1).AddDays(-1);
                DataTable dt1 = InitDataTableHeader();
                dt1 = SetDataTableHeader(dt1, InitialDate);
                this.rptWorkingCalendarHeader.DataSource = dt1;
                this.rptWorkingCalendarHeader.DataBind();

                this.WorkingCalendarID = workingCalendar.ID;
            }
        }

        /// <summary>
        /// check holiday
        /// </summary>
        /// <param name="List">List</param>
        public List<string> CheckHoliday(DateTime dt, IList<M_Config_D> lstM_Config_D, IList<T_WorkingCalendar_D> lstT_WorkingCalendar_D, IList<T_PaidLeave> lstPaidLeave, DataTable dtHiddenValue, Mode mode)
        {
            List<string> lst = new List<string>();
            lst.Add("0");
            lst.Add("1");
            lst.Add("");
            lst.Add("0");
            lst.Add("0");

            if (mode == Mode.View)
            {
                foreach (M_Config_D item in lstM_Config_D.ToList())
                {
                    if (DateTime.Parse(item.Value2.ToString()) == dt)
                    {
                        lst[2] = item.Value3;
                        lstM_Config_D.Remove(item);
                    }
                }

                if (lstPaidLeave.Any(m => m.Date == dt))
                {
                    lst[4] = "1";
                }

                foreach (T_WorkingCalendar_D item in lstT_WorkingCalendar_D.ToList())
                {
                    if (item.WorkingDate == dt)
                    {
                        lst[0] = GetWorkingTypeByID(item.WorkingSystemID);
                        lst[1] = GetWorkingSystemCDById(item.WorkingSystemID);
                        lst[3] = item.Ivent1.ToString();
                        return lst;
                    }
                }
            }
            else
            {
                if (ModeClearAfterClear != "Clear")
                {
                    if (dtHiddenValue.Rows.Count != 0)
                    {
                        foreach (DataRow row in dtHiddenValue.Rows)
                        {
                            if (row["Date"].ToString() == FormatDateForHidden(dt))
                            {
                                lst[0] = row["Value"].ToString();
                                lst[1] = row["WorkingSystemValue"].ToString();
                                lst[2] = row["TextHoliday"].ToString();
                                lst[3] = row["CheckBoxIvent"].ToString();
                                lst[4] = row["CheckBoxPaidLeave"].ToString();
                                return lst;

                            }
                        }
                    }
                }
                foreach (M_Config_D item in lstM_Config_D.ToList())
                {
                    if (lstPaidLeave.Any(m => m.Date == dt))
                    {
                        lst[4] = "1";
                    }
                    if (DateTime.Parse(item.Value2.ToString()) == dt)
                    {

                        if (dt.DayOfWeek == DayOfWeek.Sunday)
                        {
                            lst[0] = GetWorkingTypeByCD("3");
                            lst[1] = "3";
                        }
                        else
                        {
                            lst[0] = GetWorkingTypeByCD(item.Value4);
                            lst[1] = item.Value4;
                        }

                        if (lst[0] != "0")
                        {
                            lst[4] = "0";
                        }
                        
                        lst[2] = item.Value3;
                        lstM_Config_D.Remove(item);
                        return lst;
                    }

                    

                }
            }

            string strHoliday = dt.DayOfWeek.ToString();
            if (strHoliday == "Sunday" || strHoliday == "Saturday")
            {
                lst[0] = strHoliday == "Saturday" ? "1" : "2";
                lst[1] = strHoliday == "Saturday" ? "2" : "3";
                if (lst[0] != "0")
                {
                    lst[4] = "0";
                }
                return lst;
            }
            return lst;
        }

        /// <summary>
        /// Format date for Hidden
        /// </summary>
        protected string FormatDateForHidden(DateTime date)
        {
            string strDate = string.Format(Constants.FMT_YYYYMMDD, date);
            return strDate;
        }

        /// <summary>
        /// InitDataTableHiddenValue
        /// </summary>
        private DataTable InitDataTableHiddenValue()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Value", typeof(string));
            dt.Columns.Add("WorkingSystemValue", typeof(string));
            dt.Columns.Add("TextHoliday", typeof(string));
            dt.Columns.Add("CheckBoxIvent", typeof(string));
            dt.Columns.Add("CheckBoxPaidLeave", typeof(string));
            return dt;

        }

        /// <summary>
        /// InitDatTableTreeView
        /// </summary>
        /// <returns></returns>
        public static DataTable InitDatTableTreeView()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(string));
            dt.Columns.Add("departmentid", typeof(int));
            dt.Columns.Add("text", typeof(string));
            dt.Columns.Add("checked", typeof(bool));
            dt.Columns.Add("existFlag", typeof(int));
            return dt;
        }

        /// <summary>
        /// Insert Data
        /// </summary>
        /// <returns>Success:true, Faile:false</returns>
        private bool InsertData()
        {
            try
            {
                // list T_WorkingCalendar_U

                string[] ArrayUidHidden = treeViewSave.Value.Split('|');

                //Create model
                T_WorkingCalendar_H t_WorkingCalendar_H = new T_WorkingCalendar_H();
                t_WorkingCalendar_H.CalendarCD = this.txtCalendarCD.Value;
                t_WorkingCalendar_H.CalendarName = this.txtCalendarName.Value;

                t_WorkingCalendar_H.InitialDate = DateTime.Parse(txtInitialDate.Text);
                t_WorkingCalendar_H.CreateUID = this.LoginInfo.User.ID;
                t_WorkingCalendar_H.UpdateUID = this.LoginInfo.User.ID;

                // Get Hidden Value 
                string[] listHiddButtonValue = new string[12];
                string HiddenValue;
                for (int i = 0; i < 12; i++)
                {
                    HiddenValue = "HiddButtonValue" + i.ToString();
                    listHiddButtonValue[i] = Request.Form[HiddenValue];
                }

                t_WorkingCalendar_H.AgreementFlag1 = short.Parse(listHiddButtonValue[0]);
                t_WorkingCalendar_H.AgreementFlag2 = short.Parse(listHiddButtonValue[1]);
                t_WorkingCalendar_H.AgreementFlag3 = short.Parse(listHiddButtonValue[2]);
                t_WorkingCalendar_H.AgreementFlag4 = short.Parse(listHiddButtonValue[3]);
                t_WorkingCalendar_H.AgreementFlag5 = short.Parse(listHiddButtonValue[4]);
                t_WorkingCalendar_H.AgreementFlag6 = short.Parse(listHiddButtonValue[5]);
                t_WorkingCalendar_H.AgreementFlag7 = short.Parse(listHiddButtonValue[6]);
                t_WorkingCalendar_H.AgreementFlag8 = short.Parse(listHiddButtonValue[7]);
                t_WorkingCalendar_H.AgreementFlag9 = short.Parse(listHiddButtonValue[8]);
                t_WorkingCalendar_H.AgreementFlag10 = short.Parse(listHiddButtonValue[9]);
                t_WorkingCalendar_H.AgreementFlag11 = short.Parse(listHiddButtonValue[10]);
                t_WorkingCalendar_H.AgreementFlag12 = short.Parse(listHiddButtonValue[11]);

                DateTime fromDate = t_WorkingCalendar_H.InitialDate;
                DateTime toDate = fromDate.AddMonths(12).AddDays(-1);
                int totaldate = int.Parse(((toDate - fromDate).TotalDays + 1).ToString());

                string[] arrayUserId = treeViewSave.Value.Split('|');

                bool paidLeaveRegist = arrayUserId.Contains(LoginInfo.User.ID.ToString());

                using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                {
                    T_WorkingCalendar_DService t_WorkingCalendar_DService = new T_WorkingCalendar_DService(db);
                    T_WorkingCalendar_HService t_WorkingCalendar_HService = new T_WorkingCalendar_HService(db);
                    T_WorkingCalendar_UService t_WorkingCalendar_UService = new T_WorkingCalendar_UService(db);
                    T_PaidLeaveService tPaidLeaveSer = new T_PaidLeaveService(db);

                    //Insert WorkingCalendarH
                    t_WorkingCalendar_HService.Insert(t_WorkingCalendar_H);

                    this.WorkingCalendarID = db.GetIdentityId<T_WorkingCalendar_H>();
                    DateTime workingDate_D = fromDate;

                    for (int i = 0; i < totaldate; i++)
                    {
                        T_WorkingCalendar_D t_WorkingCalendar_D = new T_WorkingCalendar_D();
                        string idHidden = "workingSystemCD" + FormatDateForHidden(workingDate_D);
                        string idHiddenCheckBoxIvent = "checkBoxIvent" + FormatDateForHidden(workingDate_D);
                        string idHiddenCheckBoxPaidLeave = "checkBoxPaidLeave" + FormatDateForHidden(workingDate_D);
                        int workingSystemID = GetWorkingSystemID(Request.Form[idHidden]);
                        int valueCheckBox = int.Parse(Request.Form[idHiddenCheckBoxIvent]);
                        int paidLeaveVal = int.Parse(Request.Form[idHiddenCheckBoxPaidLeave]);


                        t_WorkingCalendar_D.HID = WorkingCalendarID;
                        t_WorkingCalendar_D.WorkingDate = workingDate_D.Date;
                        t_WorkingCalendar_D.WorkingSystemID = workingSystemID;
                        t_WorkingCalendar_D.Ivent1 = valueCheckBox;

                        //Insert WorkingCalendarD
                        t_WorkingCalendar_DService.Insert(t_WorkingCalendar_D);

                        if (paidLeaveVal == 1 && paidLeaveRegist)
                        {
                            T_PaidLeave paidLeaveModel = new T_PaidLeave();
                            paidLeaveModel.CalendarID = WorkingCalendarID;
                            paidLeaveModel.UserID = this.LoginInfo.User.ID;
                            paidLeaveModel.Date = workingDate_D.Date;
                            paidLeaveModel.CreateUID = this.LoginInfo.User.ID;
                            paidLeaveModel.UpdateUID = this.LoginInfo.User.ID;
                            tPaidLeaveSer.Insert(paidLeaveModel);
                        }

                        workingDate_D = workingDate_D.AddDays(1);
                    }

                    // Insert T_WorkingCalendar_U
                    if (ArrayUidHidden[0] != string.Empty)
                    {
                        for (int i = 0; i < ArrayUidHidden.Length; i++)
                        {
                            T_WorkingCalendar_U tWorkingCalendarU = new T_WorkingCalendar_U();
                            tWorkingCalendarU.HID = this.WorkingCalendarID;
                            tWorkingCalendarU.UID = int.Parse(ArrayUidHidden[i]);
                            t_WorkingCalendar_UService.Insert(tWorkingCalendarU);
                        }
                    }

                    db.Commit();
                }

            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains(Models.Constant.T_WORKINGCALENDAR_H_UN))
                {
                    this.SetMessage(this.txtCalendarCD.ID, M_Message.MSG_EXIST_CODE, "勤務カレンダーコード");
                }

                Log.Instance.WriteLog(ex);

                return false;
            }
            catch (Exception ex)
            {
                this.SetMessage(string.Empty, M_Message.MSG_UPDATE_FAILE, "新規");

                Log.Instance.WriteLog(ex);

                return false;
            }

            return true;
        }

        /// <summary>
        /// Get T_WorkingCalendar_H BY ID
        /// </summary>
        /// <param name="workingCalendarID">workingCalendar ID</param>
        /// <returns>T_WorkingCalendar_H</returns>
        private T_WorkingCalendar_H GetWorkingCalendarById(int workingCalendarID, DB db)
        {
            T_WorkingCalendar_HService workingCalendarSer = new T_WorkingCalendar_HService(db);

            //Get WorkingCalendar
            return workingCalendarSer.GetByID(workingCalendarID);
        }

        /// <summary>
        /// Get T_WorkingCalendar_H BY WorkingCalendar CD
        /// </summary>
        /// <param name="workingCalendarID">workingCalendar CD</param>
        /// <returns>T_WorkingCalendar_H</returns>
        private T_WorkingCalendar_H GetWorkingCalendarByCd(string workingCalendarCD, DB db)
        {
            T_WorkingCalendar_HService workingCalendarSer = new T_WorkingCalendar_HService(db);

            //Get WorkingCalendar
            return workingCalendarSer.GetByCD(workingCalendarCD);
        }

        /// <summary>
        /// Update Data
        /// </summary>
        /// <returns>Success:true, Faile:false</returns>
        private bool UpdateData()
        {
            try
            {
                bool errors = false;
                for (int i = 0; i < 12; i++)
                {
                    DateTime initialDate = DateTime.Parse(txtInitialDate.Text);
                    DateTime fromDate = initialDate.AddMonths(i);
                    DateTime toDate = initialDate.AddMonths(i + 1).AddDays(-1);
                    DateTime fromDateTemp = fromDate;

                    if (!checkEdit(fromDate))
                    {
                        continue;
                    }

                    while (DateTime.Compare(fromDateTemp, toDate) <= 0)
                    {
                        string idHidden = "workingSystemCD" + FormatDateForHidden(fromDateTemp);
                        string idHiddenCheckBoxIvent = "checkBoxIvent" + FormatDateForHidden(fromDateTemp);
                        int workingSystemID = GetWorkingSystemID(Request.Form[idHidden]);
                        int valueCheckBox = int.Parse(Request.Form[idHiddenCheckBoxIvent]);

                        T_WorkingCalendar_D t_WorkingCalendar_D = new T_WorkingCalendar_D();
                        using (DB db = new DB())
                        {
                            T_WorkingCalendar_DService tWorkingCalendarDService = new T_WorkingCalendar_DService(db);
                            t_WorkingCalendar_D = tWorkingCalendarDService.GetWorkingCalendarByKey(this.WorkingCalendarID, fromDateTemp);
                        }

                        if (t_WorkingCalendar_D != null)
                        {
                            if (t_WorkingCalendar_D.WorkingSystemID != workingSystemID)
                            {
                                errors = true;
                                break;
                            }

                            if (t_WorkingCalendar_D.Ivent1 != valueCheckBox)
                            {
                                errors = true;
                                break;
                            }

                        }

                        fromDateTemp = fromDateTemp.AddDays(1);
                    }

                    if (errors)
                    {
                        break;
                    }

                }

                if (errors)
                {
                    this.SetMessage(string.Empty, M_Message.MSG_DATA_CHANGED);
                    return false;
                }

                string[] ArrayUidHidden = treeViewSave.Value.Split('|');
                bool paidLeaveRegist = ArrayUidHidden.Contains(LoginInfo.User.ID.ToString());

                int ret = 0;

                using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                {
                    T_WorkingCalendar_HService tWorkingCalendarHService = new T_WorkingCalendar_HService(db);
                    T_WorkingCalendar_DService tWorkingCalendarDService = new T_WorkingCalendar_DService(db);
                    T_WorkingCalendar_UService tWorkingCalendarUService = new T_WorkingCalendar_UService(db);
                    T_PaidLeaveService tPaidLeaveSer = new T_PaidLeaveService(db);
                    T_WorkingCalendar_H t_WorkingCalendar_H = this.GetWorkingCalendarById(this.WorkingCalendarID, db);
                    if (t_WorkingCalendar_H != null)
                    {
                        //Create and set value for t_WorkingCalendar_H
                        t_WorkingCalendar_H.ID = WorkingCalendarID;
                        t_WorkingCalendar_H.CalendarCD = this.txtCalendarCD.Value;
                        t_WorkingCalendar_H.CalendarName = this.txtCalendarName.Value;
                        t_WorkingCalendar_H.InitialDate = DateTime.Parse(txtInitialDate.Text);
                        t_WorkingCalendar_H.AnnualWorkingDays = int.Parse(Request.Form["HiddenWorkingDateTotal"]);
                        t_WorkingCalendar_H.AnnualWorkingHours = Utilities.CommonUtil.TimeToInt(Request.Form["HiddenWorkingTimeInMonthTotal"]);

                        t_WorkingCalendar_H.UpdateUID = this.LoginInfo.User.ID;
                        t_WorkingCalendar_H.UpdateDate = this.OldUpdateDate;
                        // Get Hidden Value 
                        string[] listHiddButtonValue = new string[12];
                        string HiddenValue;
                        for (int i = 0; i < 12; i++)
                        {
                            HiddenValue = "HiddButtonValue" + i.ToString();
                            listHiddButtonValue[i] = Request.Form[HiddenValue];
                        }

                        t_WorkingCalendar_H.AgreementFlag1 = short.Parse(listHiddButtonValue[0]);
                        t_WorkingCalendar_H.AgreementFlag2 = short.Parse(listHiddButtonValue[1]);
                        t_WorkingCalendar_H.AgreementFlag3 = short.Parse(listHiddButtonValue[2]);
                        t_WorkingCalendar_H.AgreementFlag4 = short.Parse(listHiddButtonValue[3]);
                        t_WorkingCalendar_H.AgreementFlag5 = short.Parse(listHiddButtonValue[4]);
                        t_WorkingCalendar_H.AgreementFlag6 = short.Parse(listHiddButtonValue[5]);
                        t_WorkingCalendar_H.AgreementFlag7 = short.Parse(listHiddButtonValue[6]);
                        t_WorkingCalendar_H.AgreementFlag8 = short.Parse(listHiddButtonValue[7]);
                        t_WorkingCalendar_H.AgreementFlag9 = short.Parse(listHiddButtonValue[8]);
                        t_WorkingCalendar_H.AgreementFlag10 = short.Parse(listHiddButtonValue[9]);
                        t_WorkingCalendar_H.AgreementFlag11 = short.Parse(listHiddButtonValue[10]);
                        t_WorkingCalendar_H.AgreementFlag12 = short.Parse(listHiddButtonValue[11]);

                        if (t_WorkingCalendar_H.Status == DataStatus.Changed && this._authority.IsWorkCalendarEdit)
                        {
                            //Update t_WorkingCalendar_H
                            ret = tWorkingCalendarHService.Update(t_WorkingCalendar_H);

                            if (ret != 0)
                            {
                                tWorkingCalendarDService.DeleteAll(WorkingCalendarID);
                                var dicPaidLeave = tPaidLeaveSer.GetDicByKey(WorkingCalendarID, this.LoginInfo.User.ID);

                                // Insert data for T_WorkingCalendar_D
                                DateTime fromDate = t_WorkingCalendar_H.InitialDate;
                                DateTime toDate = fromDate.AddMonths(12).AddDays(-1);
                                DateTime workingDate_D = fromDate;
                                int totaldate = int.Parse(((toDate - fromDate).TotalDays + 1).ToString());
                                for (int i = 0; i < totaldate; i++)
                                {
                                    T_WorkingCalendar_D t_WorkingCalendar_D = new T_WorkingCalendar_D();

                                    string idHidden = "workingSystemCD" + FormatDateForHidden(workingDate_D);
                                    string idHiddenCheckBoxIvent = "checkBoxIvent" + FormatDateForHidden(workingDate_D);
                                    string idHiddenCheckBoxPaidLeave = "checkBoxPaidLeave" + FormatDateForHidden(workingDate_D);

                                    int workingSystemID = GetWorkingSystemID(Request.Form[idHidden]);
                                    int valueCheckBox = int.Parse(Request.Form[idHiddenCheckBoxIvent]);
                                    int paidLeaveVal = int.Parse(Request.Form[idHiddenCheckBoxPaidLeave]);

                                    t_WorkingCalendar_D.HID = WorkingCalendarID;
                                    t_WorkingCalendar_D.WorkingDate = workingDate_D.Date;
                                    t_WorkingCalendar_D.WorkingSystemID = workingSystemID;
                                    t_WorkingCalendar_D.Ivent1 = valueCheckBox;

                                    //Insert each T_WorkingCalendarD
                                    tWorkingCalendarDService.Insert(t_WorkingCalendar_D);

                                    if (paidLeaveVal == 1 && paidLeaveRegist)
                                    {
                                        if (dicPaidLeave.ContainsKey(workingDate_D.Date))
                                        {
                                            dicPaidLeave.Remove(workingDate_D.Date);
                                        }
                                        else
                                        {
                                            T_PaidLeave paidLeaveModel = new T_PaidLeave();
                                            paidLeaveModel.CalendarID = WorkingCalendarID;
                                            paidLeaveModel.UserID = this.LoginInfo.User.ID;
                                            paidLeaveModel.Date = workingDate_D.Date;
                                            paidLeaveModel.CreateUID = this.LoginInfo.User.ID;
                                            paidLeaveModel.UpdateUID = this.LoginInfo.User.ID;
                                            tPaidLeaveSer.Insert(paidLeaveModel);
                                        }
                                        
                                    }

                                    workingDate_D = workingDate_D.AddDays(1);
                                }

                                // Delete All T_WorkingCalendar_U
                                tWorkingCalendarUService.DeleteAll(this.WorkingCalendarID);

                                if (ArrayUidHidden[0] != string.Empty)
                                {
                                    for (int i = 0; i < ArrayUidHidden.Length; i++)
                                    {
                                        // Insert T_WorkingCalendar_U
                                        T_WorkingCalendar_U tWorkingCalendarU = new T_WorkingCalendar_U();
                                        tWorkingCalendarU.HID = this.WorkingCalendarID;
                                        tWorkingCalendarU.UID = int.Parse(ArrayUidHidden[i]);
                                        tWorkingCalendarUService.Insert(tWorkingCalendarU);
                                    }
                                }

                                foreach (var item in dicPaidLeave.Values)
                                {
                                    tPaidLeaveSer.DeleteByKey(item.CalendarID, item.UserID, item.Date); 
                                }

                                tPaidLeaveSer.DeleteInvalidData(this.WorkingCalendarID);
                            }
                            db.Commit();

                        }
                        else
                        {
                            if (IsExistsCalendar(this.WorkingCalendarID))
                            {
                                var dicPaidLeave = tPaidLeaveSer.GetDicByKey(WorkingCalendarID, this.LoginInfo.User.ID);

                                // Insert data for T_WorkingCalendar_D
                                DateTime fromDate = t_WorkingCalendar_H.InitialDate;
                                DateTime toDate = fromDate.AddMonths(12).AddDays(-1);
                                DateTime workingDate_D = fromDate;
                                int totaldate = int.Parse(((toDate - fromDate).TotalDays + 1).ToString());
                                for (int i = 0; i < totaldate; i++)
                                {
                                    string idHiddenCheckBoxPaidLeave = "checkBoxPaidLeave" + FormatDateForHidden(workingDate_D);
                                    int paidLeaveVal = int.Parse(Request.Form[idHiddenCheckBoxPaidLeave]);

                                    if (paidLeaveVal == 1 && paidLeaveRegist)
                                    {
                                        if (dicPaidLeave.ContainsKey(workingDate_D.Date))
                                        {
                                            dicPaidLeave.Remove(workingDate_D.Date);
                                        }
                                        else
                                        {
                                            T_PaidLeave paidLeaveModel = new T_PaidLeave();
                                            paidLeaveModel.CalendarID = WorkingCalendarID;
                                            paidLeaveModel.UserID = this.LoginInfo.User.ID;
                                            paidLeaveModel.Date = workingDate_D.Date;
                                            paidLeaveModel.CreateUID = this.LoginInfo.User.ID;
                                            paidLeaveModel.UpdateUID = this.LoginInfo.User.ID;
                                            tPaidLeaveSer.Insert(paidLeaveModel);
                                        }

                                    }

                                    workingDate_D = workingDate_D.AddDays(1);
                                }

                                foreach (var item in dicPaidLeave.Values)
                                {
                                    tPaidLeaveSer.DeleteByKey(item.CalendarID, item.UserID, item.Date);
                                }

                                db.Commit();
                            }
                            return true;
                        }
                    }
                }

                //Check result update
                if (ret == 0)
                {
                    //data had changed
                    this.SetMessage(string.Empty, M_Message.MSG_DATA_CHANGED);
                    return false;
                }
            }

            catch (SqlException ex)
            {
                if (ex.Message.Contains(Models.Constant.T_WORKINGCALENDAR_H_UN))
                {
                    this.SetMessage(this.txtCalendarCD.ID, M_Message.MSG_EXIST_CODE, "勤務カレンダーコード");
                }

                Log.Instance.WriteLog(ex);

                return false;
            }
            catch (Exception ex)
            {
                this.SetMessage(string.Empty, M_Message.MSG_UPDATE_FAILE, "更新");

                Log.Instance.WriteLog(ex);

                return false;
            }

            return true;
        }

        /// <summary>
        /// Delete Data
        /// </summary>
        /// <returns></returns>
        private bool DeleteData()
        {
            try
            {
                int ret1 = 0;
                int ret2 = 0;
                int ret3 = 0;
                using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                {
                    T_WorkingCalendar_HService tWorkingCalendarHService = new T_WorkingCalendar_HService(db);
                    T_WorkingCalendar_DService tWorkingCalendarDService = new T_WorkingCalendar_DService(db);
                    T_WorkingCalendar_UService tWorkingCalendarUService = new T_WorkingCalendar_UService(db);
                    T_PaidLeaveService tPaidLeaveSer = new T_PaidLeaveService(db);

                    ret1 = tWorkingCalendarDService.DeleteAll(this.WorkingCalendarID);
                    IList<T_WorkingCalendar_U> lstTWorkingCalendarU;
                    lstTWorkingCalendarU = tWorkingCalendarUService.GetByHId(this.WorkingCalendarID);
                    if (lstTWorkingCalendarU.Count > 0)
                    {
                        ret2 = tWorkingCalendarUService.DeleteAll(this.WorkingCalendarID);
                    }
                    else
                    {
                        ret2 = 1;
                    }

                    ret3 = tWorkingCalendarHService.Delete(this.WorkingCalendarID);

                    tPaidLeaveSer.DeleteByCalendar(this.WorkingCalendarID);

                    if (ret1 > 0 && ret2 > 0 && ret3 > 0)
                    {
                        db.Commit();
                    }
                }

                if (ret1 == 0 || ret2 == 0 || ret3 == 0)
                {
                    this.SetMessage(string.Empty, M_Message.MSG_DATA_CHANGED);
                    return false;
                }
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains(Models.Constant.T_ATTENDANCE_FK_WORKINGCALENDARHID))
                {
                    this.SetMessage(string.Empty, M_Message.MSG_EXIST_CANT_DELETE, "勤務カレンダーコード" + this.txtCalendarCD.Value);
                }

                if (ex.Message.Contains(Models.Constant.T_ATTENDANCERESULT_FK_CALLENDARID))
                {
                    this.SetMessage(string.Empty, M_Message.MSG_EXIST_CANT_DELETE, "勤務カレンダーコード" + this.txtCalendarCD.Value);
                }

                Log.Instance.WriteLog(ex);

                return false;
            }
            catch (Exception ex)
            {
                this.SetMessage(string.Empty, M_Message.MSG_UPDATE_FAILE, "削除");

                Log.Instance.WriteLog(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// btnSearch_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (InitDateSave.Value != "")
            {
                base.ShowQuestionMessage(M_Message.MSG_CLEAR, Models.DefaultButton.No, true);
            }

            DateTime fromDate = new DateTime();
            ModeClearAfterClear = this.Mode.ToString();

            //Draw Calendar with new Hidden Value
            DataTable dt = InitDataTableHeader();
            if (this.Mode == Mode.Update || (this.Mode == Mode.Insert && this.InitDateSave.Value != ""))
            {
                fromDate = DateTime.Parse(InitDateSave.Value);
            }
            else if (this.Mode == Mode.Insert && InitDateSave.Value == "")
            {
                fromDate = DateTime.Parse(txtInitialDate.Text);
                InitDateSave.Value = txtInitialDate.Text;
                base.DisabledLink(this.btnClearInsert, false);
                base.DisabledLink(this.btnClearInsertBottom, false);
            }
            string strfromDate = string.Format(Constants.FMT_YMD, fromDate);
            dt = SetDataTableHeader(dt, fromDate);
            rptWorkingCalendarHeader.DataSource = dt;
            rptWorkingCalendarHeader.DataBind();
            ModeClearAfterClear = this.Mode.ToString();

        }

        /// <summary>
        /// DrawCalendar
        /// </summary>
        /// <param name="mode"></param>
        protected void DrawCalendar(Mode mode)
        {
            if (mode == Mode.Insert || mode == Mode.Update)
            {

                DataTable dt = InitDataTableHeader();
                DateTime fromDate = DateTime.Parse(txtInitialDate.Text);

                string strfromDate = string.Format(Constants.FMT_YMD, fromDate);
                dt = SetDataTableHeader(dt, fromDate);

                rptWorkingCalendarHeader.DataSource = dt;
                rptWorkingCalendarHeader.DataBind();
                base.DisabledLink(this.btnClearInsert, false);
                base.DisabledLink(this.btnClearInsertBottom, false);

                InitDateSave.Value = txtInitialDate.Text;

                if (mode == Mode.Update)
                {
                    using (DB db = new DB())
                    {
                        T_WorkingCalendar_UService calendarUserSer = new T_WorkingCalendar_UService(db);
                        var lstUser = calendarUserSer.GetByHId(WorkingCalendarID).Select(m => m.UID.ToString());

                        this.treeViewSave.Value = string.Join("|", lstUser);
                    }

                }
                else
                {
                    this.treeViewSave.Value = ""; 
                }
                

            }
        }

        /// <summary>
        /// Get T_WorkingCalendar_H BY WorkingCalendar ID
        /// </summary>
        /// <param name="workingCalendarID">workingCalendar ID</param>
        /// <returns>T_WorkingCalendar_H</returns>
        private T_WorkingCalendar_H GetWorkingCalendar(int workingCalendarID, DB db)
        {
            T_WorkingCalendar_HService workingCalendarSer = new T_WorkingCalendar_HService(db);

            //Get WorkingCalendarH
            return workingCalendarSer.GetByID(workingCalendarID);
        }

        /// <summary>
        /// Get T_WorkingCalendar_D BY WorkingCalendar ID
        /// </summary>
        /// <param name="workingCalendarID">workingCalendar ID</param>
        /// <returns>T_WorkingCalendar_D</returns>
        private IList<T_WorkingCalendar_D> GetListWorkingCalendarInMonth(int workingCalendarID, DateTime fromDate, DateTime toDate, DB db)
        {
            T_WorkingCalendar_DService workingCalendar_DSer = new T_WorkingCalendar_DService(db);

            //Get WorkingCalendarD 
            return workingCalendar_DSer.GetListInMonth(workingCalendarID, fromDate, toDate);

        }

        /// <summary>
        /// Show Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShowData(object sender, EventArgs e)
        {
            if (ModeClearAfterClear == "Delete")
            {
                T_WorkingCalendar_H t_WorkingCalendar_H = this.GetWorkingCalendarById(this.WorkingCalendarID, new DB());


                if (t_WorkingCalendar_H != null)
                {

                    //Show data
                    this.ShowData(t_WorkingCalendar_H);

                    //Set Mode
                    this.ProcessMode(Mode.View);
                }
                else
                {
                    Server.Transfer(URL_LIST);
                }
            }
            else
            {
                if (InitDateSave.Value != txtInitialDate.Text)
                {
                    txtInitialDate.Text = InitDateSave.Value;
                }
            }

            ModeClearAfterClear = null;

        }

        /// <summary>
        /// Init Combobox
        /// </summary>
        private void InitCombobox(DropDownList ddl)
        {
            // init combox
            IList<M_WorkingSystem> lstM_WorkingSystem;

            using (DB db = new DB())
            {

                //Get data
                WorkingSystemService workingSystemService = new WorkingSystemService(db);
                lstM_WorkingSystem = workingSystemService.GetAll();
            }
            DataTable dt = new DataTable();
            dt.Columns.Add("Value", typeof(string));
            dt.Columns.Add("DisplayName", typeof(string));
            int index = 0;
            string valueCombobox = string.Empty;
            foreach (var item in lstM_WorkingSystem)
            {
                DataRow drow = dt.NewRow();
                dt.Rows.Add(drow);
                valueCombobox = item.WorkingSystemCD + "_" + item.WorkingType;
                dt.Rows[index].SetField("Value", valueCombobox);
                dt.Rows[index].SetField("DisplayName", item.WorkingSystemName2);
                index = index + 1;
            }
            ddl.DataSource = dt;
            ddl.DataValueField = "Value";
            ddl.DataTextField = "DisplayName";
            ddl.DataBind();

        }

        /// <summary>
        /// GetWorkingTypeByCD
        /// </summary>
        protected string GetWorkingTypeByCD(string workingSystemCD)
        {
            M_WorkingSystem m_WorkingSystem;
            using (DB db = new DB())
            {
                WorkingSystemService workingSystemService = new WorkingSystemService(db);
                m_WorkingSystem = workingSystemService.GetByWorkingSystemCd(workingSystemCD);
            }
            if (m_WorkingSystem != null)
            {
                return m_WorkingSystem.WorkingType.ToString();
            }
            else
            {
                return "0";
            }

        }

        /// <summary>
        /// GetWorkingTypeByID
        /// </summary>
        protected string GetWorkingTypeByID(int workingSystemID)
        {
            M_WorkingSystem m_WorkingSystem;
            using (DB db = new DB())
            {
                WorkingSystemService workingSystemService = new WorkingSystemService(db);
                m_WorkingSystem = workingSystemService.GetDataWorkingSystemById(workingSystemID);
            }
            return m_WorkingSystem.WorkingType.ToString();
        }

        /// <summary>
        /// GetWorkingSystemID
        /// </summary>
        protected int GetWorkingSystemID(string workingSystemCD)
        {
            M_WorkingSystem m_WorkingSystem;
            using (DB db = new DB())
            {
                WorkingSystemService workingSystemService = new WorkingSystemService(db);
                m_WorkingSystem = workingSystemService.GetByWorkingSystemCd(workingSystemCD);
            }
            return m_WorkingSystem.ID;
        }

        /// <summary>
        /// GetWorkingSystemID
        /// </summary>
        protected string GetWorkingSystemCDById(int workingSystemID)
        {
            M_WorkingSystem m_WorkingSystem;
            using (DB db = new DB())
            {
                WorkingSystemService workingSystemService = new WorkingSystemService(db);
                m_WorkingSystem = workingSystemService.GetDataWorkingSystemById(workingSystemID);
            }
            return m_WorkingSystem.WorkingSystemCD;
        }

        #endregion

        #region WebMethod

        /// <summary>
        /// GetDataTreeView
        /// </summary>
        /// <param name="existWorkingCalendar"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string GetDataTreeView(string workingCalendarCd, bool existWorkingCalendar)
        {
            try
            {
                IList<M_Department> lstDepartment;
                List<DepartmentTreeView> lstDepTreeView = new List<DepartmentTreeView>();
                using (DB db = new DB())
                {
                    DepartmentService DepartmentSer = new DepartmentService(db);
                    lstDepartment = DepartmentSer.GetAll();
                }

                lstDepTreeView = lstDepartment.OrderBy(l => l.ID)
                        .Select(l => new DepartmentTreeView
                        {
                            id = l.ID,
                            text = l.DepartmentName,
                            children = GetChildren(l.ID, existWorkingCalendar, workingCalendarCd)
                        }).ToList();

                return OMS.Utilities.EditDataUtil.JsonSerializer<object>(lstDepTreeView);
            }
            catch (Exception ex)
            {
                Log.Instance.WriteLog(ex);
                return null;
            }
        }

        /// <summary>
        /// GetChildren
        /// </summary>
        /// <param name="lstDepTreeView"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public static List<UserTreeView> GetChildren(int parentId, bool existWorkingCalendar, string workingCalendarCd)
        {

            M_User mUser = new M_User();
            IList<UserInfo> lstUser;
            List<UserTreeView> lstUserTreeView = new List<UserTreeView>();
            IList<T_WorkingCalendar_U> lstTWorkingCalendarU;
            T_WorkingCalendar_H tWorkingCalendarH = new T_WorkingCalendar_H();
            using (DB db = new DB())
            {
                UserService UserSer = new UserService(db);
                T_WorkingCalendar_UService tWorkingCalendarUService = new T_WorkingCalendar_UService(db);
                T_WorkingCalendar_HService tWorkingCalendarHService = new T_WorkingCalendar_HService(db);

                tWorkingCalendarH = tWorkingCalendarHService.GetByCD(workingCalendarCd);
                lstUser = UserSer.GetAllUser();

                //lstUser = lstUser.Where(m => m.StatusFlag == 0 && m.UserCD != ADMINCD).ToList();
                lstUser = lstUser.Where(m => m.UserCD != ADMINCD).ToList();
                //foreach (var item in lstUser)
                //{
                //    if (item.UserCD == ADMINCD || item.StatusFlag == 1)
                //    {
                //        lstUser.Remove(item);
                //        break;
                //    }
                //}

                if (tWorkingCalendarH != null)
                {
                    lstTWorkingCalendarU = tWorkingCalendarUService.GetByHId(tWorkingCalendarH.ID);

                    foreach (var itemUser in lstUser)
                    {
                        // Set data UserTreeView
                        UserTreeView userTreeView = new UserTreeView();
                        userTreeView.id = itemUser.ID.ToString() + "_" + itemUser.StatusFlag.ToString();
                        userTreeView.departmentid = itemUser.DepartmentID;
                        userTreeView.text = itemUser.UserName1;

                        //treeLeft
                        if (!existWorkingCalendar)
                        {
                            if (!checkConstantInList(itemUser.ID.ToString(), lstTWorkingCalendarU) && itemUser.StatusFlag == 0)
                            {
                                if (!lstUserTreeView.Contains(userTreeView))
                                {
                                    lstUserTreeView.Add(userTreeView);
                                }
                            }
                        }
                        //treeRight
                        else
                        {
                            if (checkConstantInList(itemUser.ID.ToString(), lstTWorkingCalendarU))
                            {
                                if (!lstUserTreeView.Contains(userTreeView))
                                {
                                    lstUserTreeView.Add(userTreeView);
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (var itemUser in lstUser)
                    {
                        // Set data UserTreeView
                        UserTreeView userTreeView = new UserTreeView();
                        userTreeView.id = itemUser.ID.ToString() + "_" + itemUser.StatusFlag.ToString();
                        userTreeView.departmentid = itemUser.DepartmentID;
                        userTreeView.text = itemUser.UserName1;

                        //treeLeft
                        if (!existWorkingCalendar && itemUser.StatusFlag == 0)
                        {
                            if (!lstUserTreeView.Contains(userTreeView))
                            {
                                lstUserTreeView.Add(userTreeView);
                            }
                        }
                    }
                }

            }

            return lstUserTreeView.Where(l => l.departmentid == parentId).OrderBy(l => l.id)
                .Select(l => new UserTreeView
                {
                    id = parentId.ToString() + '_' + l.id,
                    text = l.text,
                    departmentid = l.departmentid
                }).ToList();
        }

        /// <summary>
        /// check id contant in list
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool checkConstantInList(string id, IList<T_WorkingCalendar_U> lstTWorkingCalendarU)
        {
            foreach (var item in lstTWorkingCalendarU)
            {
                if (id == item.UID.ToString())
                {
                    return true;
                }
            }
            return false;

        }

        /// <summary>
        /// GetCheckedTreeViewLeft
        /// </summary>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string GetCheckedTreeViewLeft(List<string> checkedIds)
        {
            try
            {
                string[] strUserId;
                List<int> lstUserIdLeft = new List<int>();
                foreach (var item in checkedIds)
                {
                    if (item.Contains('_'))
                    {
                        strUserId = item.Split('_');
                        lstUserIdLeft.Add(int.Parse(strUserId[1]));
                    }
                }

                return OMS.Utilities.EditDataUtil.JsonSerializer<object>(checkedIds);
            }
            catch (Exception ex)
            {
                Log.Instance.WriteLog(ex);
                return null;
            }
        }

        /// <summary>
        /// GetCheckedTreeViewRight
        /// </summary>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string GetCheckedTreeViewRight(List<string> checkedIds)
        {
            try
            {
                string[] strUserId;
                List<int> lstUserIdRight = new List<int>();
                foreach (var item in checkedIds)
                {
                    if (item.Contains('_'))
                    {
                        strUserId = item.Split('_');
                        lstUserIdRight.Add(int.Parse(strUserId[1]));
                    }
                }

                return OMS.Utilities.EditDataUtil.JsonSerializer<object>(checkedIds);
            }
            catch (Exception ex)
            {
                Log.Instance.WriteLog(ex);
                return null;
            }
        }

        /// <summary>
        /// Get data for TreeView form List User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string GetDataFromListUser(string lstParentIdUserId)
        {
            IList<M_Department> lstDepartment;
            List<DepartmentTreeView> lstDepTreeView = new List<DepartmentTreeView>();
            List<UserTreeView> lstUserTreeView = new List<UserTreeView>();

            string parentName = string.Empty;
            List<int> lstParentId = new List<int>();

            try
            {
                string[] strUserParentId = lstParentIdUserId.Split('|');
                using (DB db = new DB())
                {
                    DepartmentService DepartmentSer = new DepartmentService(db);
                    UserService userSer = new UserService(db);
                    lstDepartment = DepartmentSer.GetAll();
                    for (int i = 0; i < strUserParentId.Length; i++)
                    {

                        if (lstParentIdUserId != "")
                        {
                            string[] userParent = strUserParentId[i].Split('_');
                            M_User mUser = new M_User();
                            mUser = userSer.GetByID(int.Parse(userParent[1]));

                            UserTreeView userTreeView = new UserTreeView();
                            userTreeView.id = mUser.ID.ToString() + "_" + mUser.StatusFlag.ToString(); ;
                            userTreeView.text = mUser.UserName1;
                            userTreeView.departmentid = int.Parse(userParent[0]);
                            lstUserTreeView.Add(userTreeView);
                        }
                    }
                }

                lstDepTreeView = lstDepartment.OrderBy(l => l.ID)
                    .Select(l => new DepartmentTreeView
                    {
                        id = l.ID,
                        text = l.DepartmentName,
                        children = (lstUserTreeView.Where(u => u.departmentid == l.ID).OrderBy(u => u.id)
                                    .Select(u => new UserTreeView
                                    {
                                        id = l.ID.ToString() + '_' + u.id,
                                        text = u.text,
                                        departmentid = u.departmentid
                                    }).ToList()
                                    )
                    }).ToList();

                return OMS.Utilities.EditDataUtil.JsonSerializer<object>(lstDepTreeView);
            }
            catch (Exception ex)
            {
                Log.Instance.WriteLog(ex);
                return null;
            }
        }

        /// <summary>
        /// Get Parent Node
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string GetParentNode(string userId)
        {
            int parentId = 0;
            string parentName = string.Empty;
            try
            {
                string[] strUserId = userId.Split('_');
                System.Text.StringBuilder result = new System.Text.StringBuilder();
                M_User mUser;
                M_Department mDepartment;
                using (DB db = new DB())
                {
                    UserService UserSer = new UserService(db);
                    DepartmentService departmentSer = new DepartmentService(db);
                    mUser = UserSer.GetByID(int.Parse(strUserId[1]));
                    mDepartment = departmentSer.GetDataDepartmentById(mUser.DepartmentID);
                    if (mUser != null)
                    {
                        parentId = mUser.DepartmentID;
                        parentName = mDepartment.DepartmentName;
                    }
                    result.Append("{");
                    result.Append(string.Format(" \"ParentId\":\"{0}\"", parentId));
                    result.Append(string.Format(", \"ParentName\":\"{0}\"", mDepartment.DepartmentName));
                    result.Append("}");
                }

                return result.ToString();
            }
            catch (Exception ex)
            {
                Log.Instance.WriteLog(ex);
                return null;
            }
        }

        #endregion

        /// <summary>
        /// get day month for arrayDay
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private ArrayList monnthday(DateTime startDate, int index)
        {
            ArrayList arrDay = new ArrayList();
            ExcelFormatBValue valueEcxeclB = new ExcelFormatBValue();
            DateTime fromDate = startDate.AddMonths(index);
            DateTime toDate = startDate.AddMonths(index + 1).AddDays(-1);
            int totaldate = int.Parse(((toDate - fromDate).TotalDays + 1).ToString());
            int dayOfWeek = (int)fromDate.DayOfWeek;
            DateTime currentDate = fromDate;
            var totalday = (toDate - fromDate).TotalDays + 1;

            for (var i = 0; i < 42; i++)
            {
                if (arrDay.Count >= 42)
                {
                    break;
                }
                if (i < totalday)
                {

                    if (i == 0)
                    {

                        for (var k = 0; k < 7 - dayOfWeek; k++)
                        {

                            if (k == 0)
                            {
                                for (int count = 0; count < dayOfWeek; count++)
                                {
                                    valueEcxeclB = setValueExcelvalueB("0", currentDate, "-1", "-1", "-1", "-1");
                                    arrDay.Add(valueEcxeclB);
                                }

                            }

                            valueEcxeclB = setValueExcelvalueB(currentDate.Day.ToString(), currentDate, Request.Form["checkBoxIvent" + FormatDateForHidden(currentDate)], Request.Form["textHoliday" + FormatDateForHidden(currentDate)], Request.Form["workingSystemCD" + FormatDateForHidden(currentDate)], Request.Form[FormatDateForHidden(currentDate)]);
                            arrDay.Add(valueEcxeclB);

                            currentDate = currentDate.AddDays(1);
                        }
                        totalday = totalday - (7 - dayOfWeek - 1);

                    }
                    else
                    {
                        if (i != totalday - 1)
                        {
                            valueEcxeclB = setValueExcelvalueB(currentDate.Day.ToString(), currentDate, Request.Form["checkBoxIvent" + FormatDateForHidden(currentDate)], Request.Form["textHoliday" + FormatDateForHidden(currentDate)], Request.Form["workingSystemCD" + FormatDateForHidden(currentDate)], Request.Form[FormatDateForHidden(currentDate)]);
                            arrDay.Add(valueEcxeclB);
                        }
                        else
                        {
                            // check for last rows
                            valueEcxeclB = setValueExcelvalueB(currentDate.Day.ToString(), currentDate, Request.Form["checkBoxIvent" + FormatDateForHidden(currentDate)], Request.Form["textHoliday" + FormatDateForHidden(currentDate)], Request.Form["workingSystemCD" + FormatDateForHidden(currentDate)], Request.Form[FormatDateForHidden(currentDate)]);
                            arrDay.Add(valueEcxeclB);
                            int dayOfWeekLast = 7 - (int)currentDate.DayOfWeek - 1;
                            for (int count = 0; count < dayOfWeekLast; count++)
                            {
                                valueEcxeclB = setValueExcelvalueB("0", currentDate, "-1", "-1", "-1", "-1");
                                arrDay.Add(valueEcxeclB);
                            }
                        }


                        currentDate = currentDate.AddDays(1);

                    }
                }
                else
                {
                    valueEcxeclB = setValueExcelvalueB("0", currentDate, "-1", "-1", "-1", "-1");
                    arrDay.Add(valueEcxeclB);
                }
            }

            return arrDay;
        }

        /// <summary>
        /// return a string to paint a day
        /// </summary>
        /// <param></param>
        protected ExcelFormatBValue setValueExcelvalueB(string day, DateTime currentDate, string checkBoxIvent, string holiday, string workingSystemCD, string workingSystemCDType)
        {
            ExcelFormatBValue valueEcxeclB = new ExcelFormatBValue();
            valueEcxeclB = new ExcelFormatBValue();

            valueEcxeclB.day = day;
            valueEcxeclB.currentDate = currentDate;
            valueEcxeclB.checkBoxIvent = checkBoxIvent;
            valueEcxeclB.holiday = holiday;
            valueEcxeclB.workingSystemCD = workingSystemCD;
            valueEcxeclB.workingSystemCDType = workingSystemCDType;
            return valueEcxeclB;

        }

        #region Event Excel

        /// <summary>
        /// btnExcel Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcel_Click(object sender, CommandEventArgs e)
        {

            DateTimeFormatInfo df = CultureInfo.CreateSpecificCulture("ja-JP").DateTimeFormat;
            df.Calendar = new System.Globalization.JapaneseCalendar();

            WorkingCalendarEntryExcelModal workingCalendarEntryExcelModal = new WorkingCalendarEntryExcelModal();
            workingCalendarEntryExcelModal.CalendarCD = txtCalendarCD.Text;

            workingCalendarEntryExcelModal.CalendarName = txtCalendarName.Text;
            //

            DateTime initDate = DateTime.Parse(txtInitialDate.Text);
            string strinitDate = initDate.ToString(FMT_GGYYMMDD, df);
            workingCalendarEntryExcelModal.InitialDate = strinitDate;
            workingCalendarEntryExcelModal.Tilte = this.txtCalendarName.Text;
            workingCalendarEntryExcelModal.InitialDateFomatB = initDate.ToString("yyyy'年'MM'月'dd'日'");
            workingCalendarEntryExcelModal.arrDay1 = new ArrayList();

            //set day
            workingCalendarEntryExcelModal.arrDay1 = monnthday(initDate, 0);
            workingCalendarEntryExcelModal.arrDay2 = monnthday(initDate, 1);
            workingCalendarEntryExcelModal.arrDay3 = monnthday(initDate, 2);
            workingCalendarEntryExcelModal.arrDay4 = monnthday(initDate, 3);
            workingCalendarEntryExcelModal.arrDay5 = monnthday(initDate, 4);
            workingCalendarEntryExcelModal.arrDay6 = monnthday(initDate, 5);
            workingCalendarEntryExcelModal.arrDay7 = monnthday(initDate, 6);
            workingCalendarEntryExcelModal.arrDay8 = monnthday(initDate, 7);
            workingCalendarEntryExcelModal.arrDay9 = monnthday(initDate, 8);
            workingCalendarEntryExcelModal.arrDay10 = monnthday(initDate, 9);
            workingCalendarEntryExcelModal.arrDay11 = monnthday(initDate, 10);
            workingCalendarEntryExcelModal.arrDay12 = monnthday(initDate, 11);

            DateTime fromInitDate = initDate;
            DateTime toInitDate = initDate.AddYears(1).AddDays(-1);
            string strFromInitDate = fromInitDate.ToString(FMT_GGYYMMDD, df);
            string strToInitDate = toInitDate.ToString(FMT_GGYYMMDD, df);
            workingCalendarEntryExcelModal.FromInitDate = strFromInitDate;
            workingCalendarEntryExcelModal.ToInitDate = strToInitDate;

            workingCalendarEntryExcelModal.CountDayInMonthTotal = txtCountDayInMonthTotal.Text + "日";
            HiddenField controlWorkingTimeSystem = this.Master.FindControl("MainContent").FindControl("WorkingTimeSystem") as HiddenField;
            string strdailyWorkingHours = MinuteToTimeJapan(int.Parse(controlWorkingTimeSystem.Value), true);
            workingCalendarEntryExcelModal.DailyWorkingHours = strdailyWorkingHours;

            workingCalendarEntryExcelModal.WorkingDateTotal = Request.Form["HiddenWorkingDateTotal"] + "日";
            workingCalendarEntryExcelModal.HolidayInMonthTotal = Request.Form["HiddenHolidayInMonthTotal"] + "日";
            workingCalendarEntryExcelModal.WorkingTimeInMonthTotal = MinuteToTimeJapan(Utilities.CommonUtil.TimeToMinute(Request.Form["HiddenWorkingTimeInMonthTotal"]), true);
            workingCalendarEntryExcelModal.WorkingTimeWeeklyAverageTotal = MinuteToTimeJapan(Utilities.CommonUtil.TimeToMinute(Request.Form["HiddenWorkingTimeWeeklyAverageTotal"]), true);

            workingCalendarEntryExcelModal.MonthIndex1 = txtMonthIndex1.Text;
            workingCalendarEntryExcelModal.CountDayInMonth1 = int.Parse(txtCountDayInMonth1.Text);
            workingCalendarEntryExcelModal.HolidayInMonth1 = int.Parse(txtHolidayInMonth1.Text);
            workingCalendarEntryExcelModal.WorkingDate1 = int.Parse(txtWorkingDate1.Text);
            workingCalendarEntryExcelModal.WorkingTimeInMonth1 = txtWorkingTimeInMonth1.Text;
            workingCalendarEntryExcelModal.WorkingTimeWeeklyAverage1 = txtWorkingTimeWeeklyAverage1.Text;

            workingCalendarEntryExcelModal.MonthIndex2 = txtMonthIndex2.Text;
            workingCalendarEntryExcelModal.CountDayInMonth2 = int.Parse(txtCountDayInMonth2.Text);
            workingCalendarEntryExcelModal.HolidayInMonth2 = int.Parse(txtHolidayInMonth2.Text);
            workingCalendarEntryExcelModal.WorkingDate2 = int.Parse(txtWorkingDate2.Text);
            workingCalendarEntryExcelModal.WorkingTimeInMonth2 = txtWorkingTimeInMonth2.Text;
            workingCalendarEntryExcelModal.WorkingTimeWeeklyAverage2 = txtWorkingTimeWeeklyAverage2.Text;

            workingCalendarEntryExcelModal.MonthIndex3 = txtMonthIndex3.Text;
            workingCalendarEntryExcelModal.CountDayInMonth3 = int.Parse(txtCountDayInMonth3.Text);
            workingCalendarEntryExcelModal.HolidayInMonth3 = int.Parse(txtHolidayInMonth3.Text);
            workingCalendarEntryExcelModal.WorkingDate3 = int.Parse(txtWorkingDate3.Text);
            workingCalendarEntryExcelModal.WorkingTimeInMonth3 = txtWorkingTimeInMonth3.Text;
            workingCalendarEntryExcelModal.WorkingTimeWeeklyAverage3 = txtWorkingTimeWeeklyAverage3.Text;

            workingCalendarEntryExcelModal.MonthIndex4 = txtMonthIndex4.Text;
            workingCalendarEntryExcelModal.CountDayInMonth4 = int.Parse(txtCountDayInMonth4.Text);
            workingCalendarEntryExcelModal.HolidayInMonth4 = int.Parse(txtHolidayInMonth4.Text);
            workingCalendarEntryExcelModal.WorkingDate4 = int.Parse(txtWorkingDate4.Text);
            workingCalendarEntryExcelModal.WorkingTimeInMonth4 = txtWorkingTimeInMonth4.Text;
            workingCalendarEntryExcelModal.WorkingTimeWeeklyAverage4 = txtWorkingTimeWeeklyAverage4.Text;

            workingCalendarEntryExcelModal.MonthIndex5 = txtMonthIndex5.Text;
            workingCalendarEntryExcelModal.CountDayInMonth5 = int.Parse(txtCountDayInMonth5.Text);
            workingCalendarEntryExcelModal.HolidayInMonth5 = int.Parse(txtHolidayInMonth5.Text);
            workingCalendarEntryExcelModal.WorkingDate5 = int.Parse(txtWorkingDate5.Text);
            workingCalendarEntryExcelModal.WorkingTimeInMonth5 = txtWorkingTimeInMonth5.Text;
            workingCalendarEntryExcelModal.WorkingTimeWeeklyAverage5 = txtWorkingTimeWeeklyAverage5.Text;

            workingCalendarEntryExcelModal.MonthIndex6 = txtMonthIndex6.Text;
            workingCalendarEntryExcelModal.CountDayInMonth6 = int.Parse(txtCountDayInMonth6.Text);
            workingCalendarEntryExcelModal.HolidayInMonth6 = int.Parse(txtHolidayInMonth6.Text);
            workingCalendarEntryExcelModal.WorkingDate6 = int.Parse(txtWorkingDate6.Text);
            workingCalendarEntryExcelModal.WorkingTimeInMonth6 = txtWorkingTimeInMonth6.Text;
            workingCalendarEntryExcelModal.WorkingTimeWeeklyAverage6 = txtWorkingTimeWeeklyAverage6.Text;

            workingCalendarEntryExcelModal.MonthIndex7 = txtMonthIndex7.Text;
            workingCalendarEntryExcelModal.CountDayInMonth7 = int.Parse(txtCountDayInMonth7.Text);
            workingCalendarEntryExcelModal.HolidayInMonth7 = int.Parse(txtHolidayInMonth7.Text);
            workingCalendarEntryExcelModal.WorkingDate7 = int.Parse(txtWorkingDate7.Text);
            workingCalendarEntryExcelModal.WorkingTimeInMonth7 = txtWorkingTimeInMonth7.Text;
            workingCalendarEntryExcelModal.WorkingTimeWeeklyAverage7 = txtWorkingTimeWeeklyAverage7.Text;

            workingCalendarEntryExcelModal.MonthIndex8 = txtMonthIndex8.Text;
            workingCalendarEntryExcelModal.CountDayInMonth8 = int.Parse(txtCountDayInMonth8.Text);
            workingCalendarEntryExcelModal.HolidayInMonth8 = int.Parse(txtHolidayInMonth8.Text);
            workingCalendarEntryExcelModal.WorkingDate8 = int.Parse(txtWorkingDate8.Text);
            workingCalendarEntryExcelModal.WorkingTimeInMonth8 = txtWorkingTimeInMonth8.Text;
            workingCalendarEntryExcelModal.WorkingTimeWeeklyAverage8 = txtWorkingTimeWeeklyAverage8.Text;

            workingCalendarEntryExcelModal.MonthIndex9 = txtMonthIndex9.Text;
            workingCalendarEntryExcelModal.CountDayInMonth9 = int.Parse(txtCountDayInMonth9.Text);
            workingCalendarEntryExcelModal.HolidayInMonth9 = int.Parse(txtHolidayInMonth9.Text);
            workingCalendarEntryExcelModal.WorkingDate9 = int.Parse(txtWorkingDate9.Text);
            workingCalendarEntryExcelModal.WorkingTimeInMonth9 = txtWorkingTimeInMonth9.Text;
            workingCalendarEntryExcelModal.WorkingTimeWeeklyAverage9 = txtWorkingTimeWeeklyAverage9.Text;

            workingCalendarEntryExcelModal.MonthIndex10 = txtMonthIndex10.Text;
            workingCalendarEntryExcelModal.CountDayInMonth10 = int.Parse(txtCountDayInMonth10.Text);
            workingCalendarEntryExcelModal.HolidayInMonth10 = int.Parse(txtHolidayInMonth10.Text);
            workingCalendarEntryExcelModal.WorkingDate10 = int.Parse(txtWorkingDate10.Text);
            workingCalendarEntryExcelModal.WorkingTimeInMonth10 = txtWorkingTimeInMonth10.Text;
            workingCalendarEntryExcelModal.WorkingTimeWeeklyAverage10 = txtWorkingTimeWeeklyAverage10.Text;

            workingCalendarEntryExcelModal.MonthIndex11 = txtMonthIndex11.Text;
            workingCalendarEntryExcelModal.CountDayInMonth11 = int.Parse(txtCountDayInMonth11.Text);
            workingCalendarEntryExcelModal.HolidayInMonth11 = int.Parse(txtHolidayInMonth11.Text);
            workingCalendarEntryExcelModal.WorkingDate11 = int.Parse(txtWorkingDate11.Text);
            workingCalendarEntryExcelModal.WorkingTimeInMonth11 = txtWorkingTimeInMonth11.Text;
            workingCalendarEntryExcelModal.WorkingTimeWeeklyAverage11 = txtWorkingTimeWeeklyAverage11.Text;

            workingCalendarEntryExcelModal.MonthIndex12 = txtMonthIndex12.Text;
            workingCalendarEntryExcelModal.CountDayInMonth12 = int.Parse(txtCountDayInMonth12.Text);
            workingCalendarEntryExcelModal.HolidayInMonth12 = int.Parse(txtHolidayInMonth12.Text);
            workingCalendarEntryExcelModal.WorkingDate12 = int.Parse(txtWorkingDate12.Text);
            workingCalendarEntryExcelModal.WorkingTimeInMonth12 = txtWorkingTimeInMonth12.Text;
            workingCalendarEntryExcelModal.WorkingTimeWeeklyAverage12 = txtWorkingTimeWeeklyAverage12.Text;

            //労働時間が最も長い日の労働時間数
            workingCalendarEntryExcelModal.LongestWorkingHoursDay = strdailyWorkingHours;

            // LongestWorkingHourseWeek
            ArrayList lstLongestWorkingHourseWeek = new ArrayList();
            ArrayList arrListWorkingDayHourseWeek = new ArrayList();
            ArrayList arrListWorkingDayWeeklarge48HourseContinuity = new ArrayList();
            ArrayList arrListWorkingDayLarge = new ArrayList();

            int workingDayEachWeek = 0;
            int WorkingDayWeekHourse = 0;
            int WorkingDayWeeklarge48Hourse = 0;
            int WorkingDayWeeklarge48HourseContinuity = 0;
            int WorkingDayLarge = 0;
            IList<T_WorkingCalendar_D> lst_T_WorkingCalendar_D;

            using (DB db = new DB())
            {
                T_WorkingCalendar_DService t_WorkingCalendar_DService = new T_WorkingCalendar_DService(db);
                lst_T_WorkingCalendar_D = t_WorkingCalendar_DService.GetListWorkingCalendarByHId(WorkingCalendarID);
                T_WorkingCalendar_D lastItem = lst_T_WorkingCalendar_D.Last();
                foreach (var item in lst_T_WorkingCalendar_D)
                {
                    if (GetWorkingTypeByID(item.WorkingSystemID) == "0")
                    {
                        workingDayEachWeek = workingDayEachWeek + 1;
                        WorkingDayLarge = WorkingDayLarge + 1;
                    }
                    else
                    {
                        arrListWorkingDayLarge.Add(WorkingDayLarge);
                        WorkingDayLarge = 0;

                    }
                    if (item.WorkingDate.DayOfWeek.ToString() == "Sunday" || item == lastItem)
                    {


                        WorkingDayWeekHourse = workingDayEachWeek * int.Parse(controlWorkingTimeSystem.Value);
                        arrListWorkingDayHourseWeek.Add(WorkingDayWeekHourse);
                        workingDayEachWeek = 0;
                    }
                }
            }

            object lastItemarrListWorkingDayHourseWeek = arrListWorkingDayHourseWeek[arrListWorkingDayHourseWeek.Count - 1];
            foreach (var item in arrListWorkingDayHourseWeek)
            {
                if (int.Parse(item.ToString()) > 2880)
                {
                    WorkingDayWeeklarge48Hourse = WorkingDayWeeklarge48Hourse + 1;
                    WorkingDayWeeklarge48HourseContinuity = WorkingDayWeeklarge48HourseContinuity + 1;
                }
                else
                {
                    arrListWorkingDayWeeklarge48HourseContinuity.Add(WorkingDayWeeklarge48HourseContinuity);
                    WorkingDayWeeklarge48HourseContinuity = 0;
                }
            }
            arrListWorkingDayWeeklarge48HourseContinuity.Sort();
            arrListWorkingDayHourseWeek.Sort();
            arrListWorkingDayLarge.Sort();

            //労働時時間が最も長い週の労働時間数
            workingCalendarEntryExcelModal.LongestWorkingHourseWeek = MinuteToTimeJapan(int.Parse(arrListWorkingDayHourseWeek[arrListWorkingDayHourseWeek.Count - 1].ToString()), true);

            //労働時間が48時間を超える週の最長連続週数
            workingCalendarEntryExcelModal.WorkingDayWeeklarge48HourseContinuity = arrListWorkingDayWeeklarge48HourseContinuity[arrListWorkingDayWeeklarge48HourseContinuity.Count - 1].ToString() + "週";

            //労働時時間が最も長い週の労働時間数
            workingCalendarEntryExcelModal.WorkingDayWeeklarge48Hourse = WorkingDayWeeklarge48Hourse.ToString() + "週";

            //対象期間中の最も長い連続労働日数
            workingCalendarEntryExcelModal.WorkingDayLarge = arrListWorkingDayLarge[arrListWorkingDayLarge.Count - 1].ToString() + "日";

            this.WorkingCalendarEntryExcelFlag = "Excel";

            if (this.rbExcel1.Checked == true)
            {
                WorkingCalendarEntryExcel excel = new WorkingCalendarEntryExcel();
                excel.modelInput = workingCalendarEntryExcelModal;
                IWorkbook wb = excel.OutputExcel();

                if (wb != null)
                {
                    this.SaveFile(wb);
                }
            }
            else
            {
                WorkingCalendarEntryExcelFormatB excel = new WorkingCalendarEntryExcelFormatB();
                excel.modelInput = workingCalendarEntryExcelModal;
                IWorkbook wb = excel.OutputExcel();

                if (wb != null)
                {
                    this.SaveFile(wb);
                }
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

                if (this.rbExcel1.Checked == true)
                {
                    filename = string.Format(WORKING_CALENDA_ENTRY_DOWNLOAD_FORMAT_A, DateTime.Now.ToString(FMT_YMDHMM));

                }
                else
                {
                    filename = string.Format(WORKING_CALENDA_ENTRY_DOWNLOAD_FORMAT_B, DateTime.Now.ToString(FMT_YMDHMM));
                }

                var filePath = this.ViewState["OUTFILE"].ToString();
                using (var exportData = base.GetFileStream("OUTFILE"))
                {
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("Content-Disposition", string.Format("attachment; filename = \"{0}\"", filename));
                    Response.Clear();
                    Response.BinaryWrite(exportData.GetBuffer());
                    Response.End();
                }
            }
        }

        /// <summary>
        /// MinuteToTimeJapan
        /// </summary>
        /// <param name="pValue"></param>
        /// <param name="pShowZero"></param>
        /// <returns></returns>
        protected static string MinuteToTimeJapan(int pValue, bool pShowZero)
        {

            string sResult = string.Empty;
            int nHours = 0;
            int nMinutes = 0;


            //ゼロ値表示無効
            if (pValue == 0 && !pShowZero)
            {
                return string.Empty;
            }

            //負数対応
            if (pValue < 0)
            {
                pValue *= -1;
                sResult = "-";
            }

            //単位変換(600 = 1時間)
            nHours = pValue / 60;
            nMinutes = (pValue % 60);

            sResult += nHours.ToString();
            sResult += "時間";
            sResult += nMinutes.ToString().PadLeft(2, '0');
            sResult += "分";
            return sResult;
        }

        /// <summary>
        /// ConvertIntToDateTime
        /// </summary>
        /// <param name="breakTime"> break time</param>
        /// <returns></returns>
        public static DateTime ConvertIntToDateTime(int breakTime)
        {
            var strbreakTime = Utilities.CommonUtil.IntToTime(breakTime, true);
            var arrbreakTime = strbreakTime.Split(':');
            return new DateTime(1, 1, 1).AddHours(int.Parse(arrbreakTime[0])).AddMinutes(int.Parse(arrbreakTime[1]));
        }

        /// <summary>
        /// Calculate work time
        /// </summary>
        /// <param name="startTime">start time</param>
        /// <param name="endTime">end time</param>
        /// <param name="arrBreakTime">array break time</param>
        /// <returns></returns>
        public static TimeSpan CalDurationWorkTime(DateTime startTime, DateTime endTime, DateTime[] arrBreakTime)
        {

            TimeSpan ret = new TimeSpan();
            if (endTime <= startTime)
            {
                return ret;
            }

            while (startTime <= endTime)
            {
                DateTime sTime = new DateTime(1, 1, 1).AddHours(startTime.Hour).AddMinutes(startTime.Minute);
                DateTime eTime = new DateTime(1, 1, 1).AddHours(24);
                if (startTime.Date == endTime.Date)
                {
                    eTime = new DateTime(1, 1, 1).AddHours(endTime.Hour).AddMinutes(endTime.Minute);
                }
                else
                {
                    eTime = new DateTime(1, 1, 2).AddHours(endTime.Hour).AddMinutes(endTime.Minute);
                }
                for (int i = 0; i < arrBreakTime.Length - 1; i = i + 2)
                {
                    if (sTime < arrBreakTime[i])
                    {
                        if (eTime <= arrBreakTime[i])
                        {
                            break;
                        }
                        else if (eTime > arrBreakTime[i] && eTime <= arrBreakTime[i + 1])
                        {
                            eTime = arrBreakTime[i];
                            break;
                        }
                        else
                        {
                            ret = ret.Add(arrBreakTime[i].Subtract(sTime));
                            sTime = arrBreakTime[i + 1];
                        }
                    }
                    else if (sTime >= arrBreakTime[i] && sTime < arrBreakTime[i + 1])
                    {
                        sTime = arrBreakTime[i + 1];
                    }

                    if (sTime >= eTime)
                    {
                        break;
                    }
                }

                if (eTime > sTime)
                {
                    ret = ret.Add(eTime.Subtract(sTime));
                    break;
                }
                startTime = startTime.AddDays(1);
            }

            return ret;
        }

        #endregion


        #region checkApproval

        /// <summary>
        /// check calendar id is approval
        /// </summary>
        /// <returns></returns>
        private bool checkApproval()
        {
            bool result = false;
            int count = 0;
            using (DB db = new DB())
            {
                AttendanceResultService attendanceResultService = new AttendanceResultService(db);

                count = attendanceResultService.checkApprovalBycalendar(this.txtCalendarCD.Value);

            }

            if (count > 0)
            {
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// check month calendar is approval
        /// </summary>
        /// <param name="startdate"></param>
        /// <returns> true: can't update- false: cant update</returns>
        public bool checkEdit(DateTime startdate)
        {
            bool result = false;
            int count = 0;
            using (DB db = new DB())
            {
                AttendanceResultService attendanceResultService = new AttendanceResultService(db);

                count = attendanceResultService.checkEditBycalendar(startdate, this.txtCalendarCD.Value);

            }

            if (count > 0)
            {
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }
        #endregion

    }
}