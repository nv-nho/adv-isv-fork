using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OMS.Utilities;
using System.Collections;
using OMS.DAC;
using OMS.Models;
using System.Data;
using OMS.Models.Type;
using System.Web.UI.HtmlControls;
using NPOI.SS.UserModel;
using OMS.Reports.EXCEL;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;

namespace OMS.Attendance
{
    /// <summary>
    /// FrmAttendanceList
    /// Create  : isv.than
    /// Date    : 19/10/2017
    /// </summary>
    public partial class FrmAttendanceList : FrmBaseList
    {
        #region Cosntant
        private const string ATTENDANCELIST_DOWNLOAD = "勤務表_{0}.xlsx";
        private const string FMT_YMDHMM = "yyMMddHHmm";
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
        /// Get or set flagNew
        /// </summary>
        public bool flagNew
        {
            get { return (bool)ViewState["flagNew"]; }
            set { ViewState["flagNew"] = value; }
        }

        /// <summary>
        /// Get or set flagSubmit
        /// </summary>
        public bool flagSubmit
        {
            get { return (bool)ViewState["flagSubmit"]; }
            set { ViewState["flagSubmit"] = value; }
        }

        /// <summary>
        /// Get or set flagSubmitCancel
        /// </summary>
        public bool flagSubmitCancel
        {
            get { return (bool)ViewState["flagSubmitCancel"]; }
            set { ViewState["flagSubmitCancel"] = value; }
        }

        public string formNameDir
        {
            get { return (string)ViewState["formNameDir"]; }
            set { ViewState["formNameDir"] = value; }
        }

        /// <summary>
        /// Get or set isShowbtnSubmitORbtnSubmitCancel
        /// </summary>
        public bool isShowbtnSubmitORbtnSubmitCancel
        {
            get { return (bool)ViewState["isShowbtnSubmitORbtnSubmitCancel"]; }
            set { ViewState["isShowbtnSubmitORbtnSubmitCancel"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool hasData
        {
            get { return (bool)ViewState["hasData"]; }
            set { ViewState["hasData"] = value; }
        }

        /// <summary>
        /// Get or set ShowSubmitCance
        /// </summary>
        public bool ShowSubmitCance { get; set; }

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
        /// Get or ser IsShowInfo
        /// </summary>
        public bool IsShowInfo { get; set; }

        /// <summary>
        /// Get or ser DefaultButton
        /// </summary>
        public string DefaultButton { get; set; }

        /// <summary>
        /// Get or set Success
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Get or set isApprovalForm
        /// </summary>
        public bool isApprovalForm { get; set; }

        public string nameButtonApproval { get; set; }

        /// <summary>
        /// Get or set UserList
        /// </summary>
        public IList<DropDownModel> UserList
        {
            get
            {
                return (IList<DropDownModel>)ViewState["UserList"];
            }
            set
            {
                ViewState["UserList"] = value;
            }
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
            base.FormTitle = "勤務表一覧";
            base.FormSubTitle = "List";

            //Get Message
            M_Message mess = (M_Message)this.Messages[M_Message.MSG_REQUEST_INFO];
            this.infoMessage.InnerHtml = mess.Message3;

            //Init Event
            LinkButton btnYes = (LinkButton)this.Master.FindControl("btnYes");
            btnYes.Click += new EventHandler(btnProcess);

            LinkButton btnNo = (LinkButton)this.Master.FindControl("btnNo");
            btnNo.Click += new EventHandler(btCancelSubmit);

            //Click the Download Excel button
            this.btnDownload.ServerClick += new EventHandler(btnDownloadExcel_Click);

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
        /// btnProcessRegisterDefault
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnProcess(object sender, EventArgs e)
        {
            IList<T_Attendance> attendance;
            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MinValue;
            int userID = int.Parse(ddlUser.SelectedValue != string.Empty ? ddlUser.SelectedValue : Constants.DEFAULT_VALUE_STRING);
            if (ddlDateOfService.SelectedValue != string.Empty && ddlDateOfService.SelectedValue != Constants.DEFAULT_VALUE_STRING)
            {
                startDate = DateTime.Parse(ddlDateOfService.SelectedValue);
                endDate = startDate.AddMonths(1).AddDays(-1);
            }

            if (flagNew)
            {
                try
                {
                    int AttendanceId;
                    int WCID;
                    int workSystemId = Constants.DEFAULT_VALUE_INT;

                    DateTime fromDate = DateTime.Parse(this.ddlDateOfService.SelectedValue);
                    DateTime toDate = fromDate.AddMonths(1).AddDays(-1);

                    int totalDate = (int)(toDate - fromDate).TotalDays + 1;
                    IList<T_WorkingCalendar_D> lstTWorkingCalendarD;

                    using (DB db = new DB())
                    {
                        T_WorkingCalendar_H tWorkingCalendarH = new T_WorkingCalendar_H();
                        T_WorkingCalendar_HService tWorkingCalendarHSer = new T_WorkingCalendar_HService(db);
                        T_WorkingCalendar_DService tWorkingCalendarDService = new T_WorkingCalendar_DService(db);
                        WCID = tWorkingCalendarHSer.GetIDByUserAndDate(userID, fromDate);
                        tWorkingCalendarH = tWorkingCalendarHSer.GetByID(WCID);

                        //get list T_WorkingCalendar_D
                        lstTWorkingCalendarD = tWorkingCalendarDService.GetListWorkingCalendarByHId(WCID);
                    }

                    using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                    {
                        AttendanceService AttendanceSer = new AttendanceService(db);
                        T_PaidLeaveService paidLeaveSer = new T_PaidLeaveService(db);
                        var dicPaidLeave = paidLeaveSer.GetDicInMonths(WCID, userID, fromDate, toDate);
                        decimal useVacationDays = 0;
                        for (int i = 0; i < totalDate; i++)
                        {
                            T_Attendance tAttendance = new T_Attendance();
                            T_Work_H t_Work_H = new T_Work_H();
                            DateTime currentDate = fromDate.AddDays(i);
                            string strCurrentDate = currentDate.ToString(Constants.FMT_DATE_EN);

                            // get T_Attendance in database
                            tAttendance = AttendanceSer.GetAttendanceByUidAndDate(userID, strCurrentDate);

                            if (tAttendance == null)
                            {
                                workSystemId = lstTWorkingCalendarD.First(item => item.WorkingDate == currentDate).WorkingSystemID;

                                //Create Data
                                tAttendance = CreateDataAttendanceHeader(currentDate, userID, workSystemId, dicPaidLeave);

                                if (tAttendance != null)
                                {
                                    // Create T_Attendance
                                    tAttendance.WSID = workSystemId;
                                    AttendanceSer.Insert(tAttendance);
                                    AttendanceId = db.GetIdentityId<T_Attendance>();

                                    //Create T_Work_H
                                    t_Work_H = CreateDataAttendanceDetail(currentDate, userID, AttendanceId);
                                    Work_HService work_HService = new Work_HService(db);
                                    work_HService.Insert(t_Work_H);

                                    if (tAttendance.VacationFlag.HasValue)
                                    {
                                        useVacationDays += 1;
                                    }
                                }
                            }
                        }

                        //********* update vacation days of user ***********
                        if (useVacationDays != 0)
                        {

                            if (!this.PaidVacationProcess(db, userID, useVacationDays))
                            {
                                this.SetMessage(string.Empty, M_Message.MSG_DATA_CHANGED);
                                this.Success = false;
                            }
                            else
                            {
                                db.Commit();
                                this.Success = true;
                            }
                        }
                        else
                        {
                            db.Commit();
                            this.Success = true;
                        }
                    }

                    // load grid
                    LoadDataGrid();
                }
                catch (SqlException ex)
                {
                    if (ex.Message.Contains(Models.Constant.T_ATTENDANCE_UN))
                    {
                        //data had changed
                        this.SetMessage(string.Empty, M_Message.MSG_DATA_CHANGED);
                    }
                    Log.Instance.WriteLog(ex);

                }
                catch (Exception ex)
                {
                    this.SetMessage(string.Empty, M_Message.MSG_UPDATE_FAILE, "新規");

                    Log.Instance.WriteLog(ex);
                }
            }

            if (flagSubmit)
            {
                bool result = false;
                using (DB db = new DB())
                {
                    AttendanceService atdService = new AttendanceService(db);

                    attendance = atdService.GetListAttendanceByUidAndDate(userID, startDate, endDate);

                }

                try
                {
                    if (!checkSubmitssion(userID, startDate, endDate))
                    {
                        ShowMessageUpdateSubmitssionErrors(M_Message.MSG_SUBMIT_ERROR, Models.DefaultButton.No);
                    }
                    else if (HaveUnAprrovaledData(userID, startDate, endDate))
                    {
                        ShowMessageUpdateSubmitssionErrors(M_Message.MSG_SUBMIT_APPROVAL_ERROR, Models.DefaultButton.No);
                    }
                    else
                    {
                        using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                        {
                            foreach (var item in attendance)
                            {
                                result = updateStatusFlag(db, item, (int)AttendanceStatusFlag.Submitted);

                                if (!result)
                                {
                                    break;
                                }
                            }

                            if (result)
                            {
                                //Set Success
                                this.Success = true;
                                db.Commit();
                            }
                            else
                            {
                                //Set Success
                                this.Success = false;
                                this.SetMessage(string.Empty, M_Message.MSG_DATA_CHANGED);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.SetMessage(string.Empty, M_Message.MSG_UPDATE_FAILE, "更新");
                    Log.Instance.WriteLog(ex);
                }
            }

            if (flagSubmitCancel)
            {

                int countApproval = 0;
                bool result = false;
                using (DB db = new DB())
                {

                    AttendanceService atdService = new AttendanceService(db);

                    attendance = atdService.GetListAttendanceByUidAndDate(userID, startDate, endDate);
                    countApproval = atdService.checkAttendanceApproval(userID, startDate, endDate);


                }

                if (countApproval > 0)
                {
                    this.Success = false;
                    this.SetMessage(string.Empty, M_Message.MSG_DATA_CHANGED);
                }
                else
                {
                    using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                    {
                        foreach (var item in attendance)
                        {
                            result = updateStatusFlag(db, item, (int)AttendanceStatusFlag.NotSubmitted);

                            if (!result)
                            {
                                break;
                            }
                        }

                        if (result)
                        {
                            //Set Success
                            this.Success = true;
                            db.Commit();
                        }
                        else
                        {
                            //Set Success
                            this.Success = false;
                            this.SetMessage(string.Empty, M_Message.MSG_DATA_CHANGED);
                        }
                    }
                }
            }

            LoadDataGrid();
            CheckAuthority();
        }

        /// <summary>
        /// Process Paid Vacation day of user
        /// </summary>
        /// <param name="db"></param>
        /// <param name="useVacationDays"></param>
        /// <returns></returns>
        private bool PaidVacationProcess(DB db, int userID ,decimal useVacationDays)
        {
            T_PaidVacationService vacationDaySer = new T_PaidVacationService(db);
            IList<T_PaidVacation> lstPaidVacation = vacationDaySer.GetListValid(userID);
            int count = 0;
            foreach (var vacationModel in lstPaidVacation)
            {
                if (useVacationDays == 0)
                {
                    break;
                }
                count += 1;
                if (vacationModel.VacationDay == 0 && count < lstPaidVacation.Count)
                {
                    continue;
                }
                if (vacationModel.VacationDay >= useVacationDays)
                {
                    vacationModel.VacationDay = useVacationDays;
                    useVacationDays = 0;
                }
                else
                {
                    useVacationDays = useVacationDays - (decimal)vacationModel.VacationDay;
                }

                vacationModel.UpdateUID = this.LoginInfo.User.ID;
                int vacationUpdateCount = vacationDaySer.UpdateVacationDays(vacationModel);

                if (vacationUpdateCount == 0)
                {
                    return false;
                }
            }

            if (useVacationDays != 0)
            {
                return false;
            }
            return true;
        }

        protected void btCancelSubmit(object sender, EventArgs e)
        {
            LoadDataGrid();
            CheckAuthority();
        }

        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Check authority of login user
            base.SetAuthority(FormId.Attendance);
            this.hidDetailClientID.Value = string.Empty;
            if (!this._authority.IsAttendanceView)
            {
                if (this.PreviousPage != null)
                {
                    if (this.PreviousPageViewState["ConditionApproval"] != null)
                    {
                        this.ViewState["ConditionApproval"] = this.PreviousPageViewState["ConditionApproval"];
                        Server.Transfer("~/AttendanceApproval/FrmAttendanceApproval.aspx");
                    }
                    else
                    {
                        Response.Redirect("~/Menu/FrmMainMenu.aspx");
                    }
                }
                else
                {
                    Response.Redirect("~/Menu/FrmMainMenu.aspx");
                }

            }
            this.IsShowInfo = false;
            if (!this.IsPostBack)
            {
                //Show condition
                if (this.PreviousPage != null)
                {

                    if (this.PreviousPageViewState["Condition"] != null)
                    {
                        Hashtable data = (Hashtable)PreviousPageViewState["Condition"];
                        this.ViewState["ConditionApproval"] = this.PreviousPageViewState["ConditionApproval"];
                        this.ShowCondition(data);
                    }
                    else
                    {
                        if (this.PreviousPageViewState["ConditionApproval"] != null)
                        {
                            this.ViewState["ConditionApproval"] = this.PreviousPageViewState["ConditionApproval"];
                            formNameDir = "FormApproval";
                            isApprovalForm = true;

                            string[] PreviousPageViewState = base.PreviousPageViewState["ID"].ToString().Split(',');

                            DateTime fromDate = new DateTime();
                            DateTime toDate = new DateTime();
                            fromDate = DateTime.Parse(PreviousPageViewState[4]);
                            toDate = DateTime.Parse(PreviousPageViewState[5]);

                            string stInitialDate = string.Empty;
                            if (fromDate.Year != toDate.Year)
                            {
                                stInitialDate = string.Format(Constants.FMR_DATE_YM, fromDate) + "〜" + string.Format(Constants.FMR_DATE_YM, toDate);
                            }
                            else
                            {

                                if (fromDate.Month != toDate.Month)
                                {
                                    stInitialDate = string.Format(Constants.FMR_DATE_YM, fromDate) + "〜" + string.Format(Constants.FMR_DATE_M, toDate);
                                }
                                else
                                {
                                    stInitialDate = string.Format(Constants.FMR_DATE_YM, fromDate);
                                }
                            }

                            this.LoadWorkingCalendarComboboxData();
                            this.ddlWorkingCalendar.SelectedValue = PreviousPageViewState[6];

                            this.LoadDateOfServiceCombobox();
                            this.ddlDateOfService.SelectedValue = string.Format(Constants.FMR_DATE_YMD, DateTime.Parse(PreviousPageViewState[4]));

                            this.LoadDepartmentComboboxData();
                            this.ddlDepartment.SelectedValue = PreviousPageViewState[2];

                            this.LoadUserComboboxData();
                            this.ddlUser.SelectedValue = PreviousPageViewState[0];

                            ddlWorkingCalendar.Enabled = false;
                            ddlUser.Enabled = false;
                            ddlDateOfService.Enabled = false;
                            ddlDepartment.Enabled = false;

                            InitVacation();
                            InitOverTime();
                        }
                        else
                        {
                            //Init data
                            this.InitData();
                        }

                    }
                }
                else
                {
                    isApprovalForm = false;
                    //Init data
                    this.InitData();
                }

                showSubmitCancel();

                //Show data on grid
                this.LoadDataGrid();

                // check Authority
                CheckAuthority();
                this.Collapse = string.Empty;
            }

            // check Authority
            CheckAuthority();
            LoadUserComboboxAttribute();
        }

        /// <summary>
        /// CheckAuthority
        /// </summary>
        private void CheckAuthority()
        {
            // Check Other Departments
            if (!base._authority.IsAttendanceOtherDepartments)
            {
                this.ddlDepartment.Enabled = false;
            }

            // Check Shelf Registration
            if (!base._authority.IsAttendanceOtherEmployees)
            {
                this.ddlUser.Enabled = false;
                //this.ddlWorkingCalendar.Enabled = false;
            }

            //check Excel
            base.DisabledLink(this.btnExcel, !base._authority.IsAttendanceExportExcel || !hasData);
            base.DisabledLink(this.btnExcelButtom, !base._authority.IsAttendanceExportExcel || !hasData);

            if (ddlUser.SelectedValue != this.LoginInfo.User.ID.ToString())
            {
                base.DisabledLink(this.btnRegisterDefault, !base._authority.IsAttendanceOtherUpdates || !base._authority.IsAttendanceNew || !base._authority.IsAttendanceShelfRegistration || showSubmitCancel());
                base.DisabledLink(this.btnRegisterDefaultBottom, !base._authority.IsAttendanceOtherUpdates || !base._authority.IsAttendanceNew || !base._authority.IsAttendanceShelfRegistration || showSubmitCancel());
            }
            else
            {
                base.DisabledLink(this.btnRegisterDefault, !base._authority.IsAttendanceShelfRegistration || !base._authority.IsAttendanceNew || showSubmitCancel());
                base.DisabledLink(this.btnRegisterDefaultBottom, !base._authority.IsAttendanceShelfRegistration || !base._authority.IsAttendanceNew || showSubmitCancel());
            }
        }

        /// <summary>
        /// Event btnDetail_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDetail_Click(object sender, CommandEventArgs e)
        {
            this.ViewState["ID"] = e.CommandArgument.ToString();

            var btnClientID = ((LinkButton)sender).ClientID;

            //Save condition
            this.SaveCondSearch(btnClientID);
        }

        /// <summary>
        /// Event btnRegisterDefault_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRegisterDefault_Click(object sender, EventArgs e)
        {
            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MinValue;
            int userID = ddlUser.SelectedValue != string.Empty ? int.Parse(ddlUser.SelectedValue) : Constants.DEFAULT_VALUE_INT;
            int calendarID = ddlWorkingCalendar.SelectedValue != string.Empty ? int.Parse(ddlWorkingCalendar.SelectedValue) : Constants.DEFAULT_VALUE_INT;
            if (ddlDateOfService.SelectedValue != string.Empty)
            {
                startDate = DateTime.Parse(ddlDateOfService.SelectedValue);
                endDate = startDate.AddMonths(1).AddDays(-1);
            }

            if (!CheckRegistDefault(calendarID, userID, startDate, endDate))
            {
                ShowMessageUpdateSubmitssionErrors(M_Message.MSG_NOT_ENOUGH_VACATION_DAYS, Models.DefaultButton.No);
                return;

            }

            ShowQuestionMessage(M_Message.MSG_REGISTER_DEFAULT, Models.DefaultButton.No, true);
        }

        /// <summary>
        /// rptAttendanceList_ItemDataBound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptAttendanceList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView dr = e.Item.DataItem as DataRowView;

                string ClassApprovalDetail = dr["ClassApprovalDetail"].ToString();

                LinkButton btnDetail = (LinkButton)e.Item.FindControl("btnDetail");
                if (btnDetail != null) btnDetail.CssClass = "btn btn-info btn-sm loading " + ClassApprovalDetail;

                int ID = int.Parse(dr["ID"].ToString());
                IList<ContentInfo> listContentRested;
                IList<ContentInfo> listContentInfo;

                using (DB db = new DB())
                {
                    AttendanceService atdService = new AttendanceService(db);

                    listContentInfo = atdService.GetContentByID(ID);
                    listContentRested = atdService.GetContentRestedByID(ID, M_Config_H.CONFIG_CD_VACATION_TYPE);
                }

                Repeater rpChildContentRested = e.Item.FindControl("rptRested") as Repeater;
                if (listContentRested.Count == 0)
                {
                    listContentRested.Add(new ContentInfo());
                    rpChildContentRested.DataSource = listContentRested;
                }
                else if (listContentRested.Count == 1 && listContentRested[0].VacationFlag == ((int)VacationFlag.MorningAndAfternoon).ToString())
                {
                    ContentInfo contentInfoNew1 = new ContentInfo();
                    contentInfoNew1 = listContentRested[0];
                    ContentInfo contentInfoNew2 = new ContentInfo();
                    contentInfoNew2.RowNumber = 2;
                    contentInfoNew2.ContentVacation = "&nbsp;&nbsp;&nbsp;後半休 : " + "<span style='color:red'>" + contentInfoNew1.Value3 + "</span>";
                    listContentRested.Add(contentInfoNew2);
                    rpChildContentRested.DataSource = listContentRested;
                }
                else
                {
                    rpChildContentRested.DataSource = listContentRested;

                }
                rpChildContentRested.DataBind();

                Repeater rpChildContent = e.Item.FindControl("rptContent") as Repeater;
                if (listContentInfo.Count == 0)
                {
                    listContentInfo.Add(new ContentInfo());
                    rpChildContent.DataSource = listContentInfo;
                }
                else
                {
                    rpChildContent.DataSource = listContentInfo;
                }
                rpChildContent.DataBind();

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
                            overTimeList[0].Value4 = dr["PrematureOvertimeWork"].ToString();
                            break;
                        case 2:
                            overTimeList[0].Value4 = dr["PrematureOvertimeWork"].ToString();
                            overTimeList[1].Value4 = dr["RegularOverTime"].ToString();
                            break;
                        case 3:
                            overTimeList[0].Value4 = dr["PrematureOvertimeWork"].ToString();
                            overTimeList[1].Value4 = dr["RegularOverTime"].ToString();
                            overTimeList[2].Value4 = dr["LateNightOverTime"].ToString();
                            break;
                        case 4:
                            overTimeList[0].Value4 = dr["PrematureOvertimeWork"].ToString();
                            overTimeList[1].Value4 = dr["RegularOverTime"].ToString();
                            overTimeList[2].Value4 = dr["LateNightOverTime"].ToString();
                            overTimeList[3].Value4 = dr["PredeterminedHolidayLateNight"].ToString();
                            break;
                        default:
                            overTimeList[0].Value4 = dr["PrematureOvertimeWork"].ToString();
                            overTimeList[1].Value4 = dr["RegularOverTime"].ToString();
                            overTimeList[2].Value4 = dr["LateNightOverTime"].ToString();
                            overTimeList[3].Value4 = dr["PredeterminedHolidayLateNight"].ToString();
                            overTimeList[4].Value4 = dr["LegalHolidayLateNight"].ToString();
                            break;
                    }

                    Repeater rpChilOverTimeD2 = e.Item.FindControl("rptOverTimeD2") as Repeater;
                    if (overTimeList.Count == 0)
                    {
                        rpChilOverTimeD2.DataSource = null;
                    }
                    else
                    {
                        rpChilOverTimeD2.DataSource = overTimeList;
                    }
                    rpChilOverTimeD2.DataBind();

                    IList<M_Config_D> overTimeListH2Hiden;
                    overTimeListH2Hiden = config_DService.GetListByConfigCd(M_Config_H.CONFIG_CD_OVER_TIME_TYPE);

                    Repeater rpChildOverTimeH2Hiden = e.Item.FindControl("rptOverTimeH2Hiden") as Repeater;
                    if (overTimeListH2Hiden.Count == 0)
                    {
                        rpChildOverTimeH2Hiden.DataSource = null;
                    }
                    else
                    {
                        rpChildOverTimeH2Hiden.DataSource = overTimeList;
                    }
                    rpChildOverTimeH2Hiden.DataBind();


                    if (e.Item.ItemIndex != 15)
                    {
                        HtmlControl divHeader = e.Item.FindControl("divHeader") as HtmlControl;
                        divHeader.Visible = false;
                    }
                    else
                    {
                        HtmlControl divHeader = e.Item.FindControl("divHeader") as HtmlControl;
                        divHeader.Visible = true;
                    }
                }
            }
        }

        /// <summary>
        /// ddlWorkingCalendar_SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlWorkingCalendar_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadDateOfServiceCombobox();
            this.LoadDepartmentComboboxData();
            this.LoadUserComboboxData();
            this.LoadDataGrid();
            this.Collapse = "in";
            CheckAuthority();
        }

        /// <summary>
        /// ddlDateOfService_SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlDateOfService_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadDataGrid();
            CheckAuthority();
        }

        /// <summary>
        /// ddlUser_SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            string userSelect = ddlUser.SelectedValue.ToString();
            using (DB db = new DB())
            {
                UserService userService = new UserService(db);
                M_User user = userService.GetByID(int.Parse(userSelect));
                if (user != null)
                {
                    this.ddlDepartment.SelectedValue = user.DepartmentID.ToString();
                }
            }

            this.LoadUserComboboxData();
            this.ddlUser.SelectedValue = userSelect;
            this.LoadDataGrid();
            CheckAuthority();
        }

        /// <summary>
        /// ddlDepartment_SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadUserComboboxData();
            this.LoadDataGrid();
            CheckAuthority();
        }

        #endregion

        #region Method
        /// <summary>
        /// Init Data
        /// </summary>
        private void InitData()
        {
            InitCombobox();

            InitVacation();

            InitOverTime();
        }

        /// <summary>
        /// Save condition search
        /// </summary>
        private void SaveCondSearch(string btnDetailID)
        {
            Hashtable hash = new Hashtable();
            hash.Add(this.ddlDateOfService.ID, this.ddlDateOfService.SelectedValue);
            hash.Add(this.ddlDepartment.ID, this.ddlDepartment.SelectedValue);
            hash.Add(this.ddlUser.ID, this.ddlUser.SelectedValue);
            hash.Add(this.ddlWorkingCalendar.ID, this.ddlWorkingCalendar.SelectedValue);
            hash.Add(this.hidDetailClientID.ID, btnDetailID);

            if (formNameDir == string.Empty || formNameDir == null)
            {
                hash.Add("FormDirId", "Menu");
            }
            else
            {
                hash.Add("ddlDateOfServiceName", this.ddlDateOfService.SelectedItem.Text);
                hash.Add("ddlDepartmentName", this.ddlDepartment.SelectedItem.Text);
                hash.Add("ddlUserName", this.ddlUser.SelectedItem.Text);
                hash.Add("FormDirId", "AttendanceApproval");
            }

            this.ViewState["Condition"] = hash;
        }

        /// <summary>
        /// Show Condition
        /// </summary>
        private void ShowCondition(Hashtable data)
        {
            string formId = data["FormDirId"].ToString();

            if (formId == "Menu")
            {
                isApprovalForm = false;
            }
            else
            {
                isApprovalForm = true;
                formNameDir = "Approval";

                ddlWorkingCalendar.Enabled = false;
                ddlUser.Enabled = false;
                ddlDateOfService.Enabled = false;
                ddlDepartment.Enabled = false;
            }

            this.LoadWorkingCalendarComboboxData();
            this.ddlWorkingCalendar.SelectedValue = data[this.ddlWorkingCalendar.ID].ToString();

            this.LoadDateOfServiceCombobox();
            this.ddlDateOfService.SelectedValue = data[this.ddlDateOfService.ID].ToString();

            this.LoadDepartmentComboboxData();
            this.ddlDepartment.SelectedValue = data[this.ddlDepartment.ID].ToString();

            this.LoadUserComboboxData();
            this.ddlUser.SelectedValue = data[this.ddlUser.ID].ToString();

            this.hidDetailClientID.Value = data[this.hidDetailClientID.ID].ToString();

            InitVacation();
            InitOverTime();
        }

        /// <summary>
        /// load data gird
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="numOnPage"></param>
        private void LoadDataGrid()
        {
            LoadPaidVacation();
            if (this.ddlUser.SelectedValue == Constants.DEFAULT_VALUE_STRING
                || this.ddlDepartment.SelectedValue == Constants.DEFAULT_VALUE_STRING
                || this.ddlDateOfService.SelectedValue == Constants.DEFAULT_VALUE_STRING)
            {
                isShowbtnSubmitORbtnSubmitCancel = false;
                hasData = false;
                return;
            }

            if (!base._authority.IsAttendanceOtherEmployees && this.ddlUser.SelectedValue != LoginInfo.User.ID.ToString())
            {
                // detail
                this.rptAttendanceList.DataSource = null;
                this.rptAttendanceList.DataBind();
                isShowbtnSubmitORbtnSubmitCancel = false;
                hasData = false;
            }

            //int totalRow = 0; 
            IList<AttendanceDetailInfo> listAttendanceDetailInfo;
            DataTable dataAttendanceDetailInfo;
            try
            {

                using (DB db = new DB())
                {

                    DateTime startDate = DateTime.MinValue;
                    DateTime endDate = DateTime.MinValue;
                    if (ddlDateOfService.SelectedValue != string.Empty)
                    {
                        startDate = DateTime.Parse(ddlDateOfService.SelectedValue);
                        endDate = startDate.AddMonths(1).AddDays(-1);
                    }
                    int userID = ddlUser.SelectedValue != string.Empty ? int.Parse(ddlUser.SelectedValue) : Constants.DEFAULT_VALUE_INT;

                    AttendanceService atdService = new AttendanceService(db);
                    SetDataHeader(db, atdService, startDate, endDate, userID);
                    int count = atdService.checkAttendanceSubmit(userID, startDate, endDate);
                    listAttendanceDetailInfo = atdService.GetListByCond(startDate, endDate, userID);

                    /* Isv-Tinh 2020/04/13 ADD Start -- config interval time */
                    IList<AttendanceDetailInfo> listPreviousStartDate = atdService.GetPreviousRecordByCond(startDate, userID);
                    SetDataInterValTime(listAttendanceDetailInfo, listPreviousStartDate);
                    /* Isv-Tinh 2020/04/13 ADD End */

                    //CreateDataDetail(startDate, endDate, ref listAttendanceDetailInfo);
                    dataAttendanceDetailInfo = ConvertListToDataTable(listAttendanceDetailInfo, startDate, endDate, count, db);
                    SetDataVacation(listAttendanceDetailInfo);


                }

                if (dataAttendanceDetailInfo.Rows.Count == 0)
                {
                    this.rptAttendanceList.DataSource = null;
                    this.rptAttendanceList.DataBind();
                    isShowbtnSubmitORbtnSubmitCancel = false;
                    hasData = false;

                    return;
                }
                else
                {
                    // detail
                    this.rptAttendanceList.DataSource = dataAttendanceDetailInfo;
                    this.rptAttendanceList.DataBind();

                    hasData = true;
                    isShowbtnSubmitORbtnSubmitCancel = true;

                    SetHeaderOverTime();
                    showSubmitCancel();
                    CheckAuthority();
                }
            }
            catch (Exception e)
            {
                Log.Instance.WriteLog(e);
            }
        }

        private void LoadPaidVacation()
        {
            int userID = ddlUser.SelectedValue != string.Empty ? int.Parse(ddlUser.SelectedValue) : Constants.DEFAULT_VALUE_INT;
            using (DB db = new DB())
            {
                //set VacationDay
                T_PaidVacationService paidVacationSer = new T_PaidVacationService(db);
                txtVacationDay.Text = paidVacationSer.GetTotalVacationDays(userID).ToString("#,##0.0");

                //set Verification
                AttendanceService atdSer = new AttendanceService(db);
                var dataVacation = atdSer.GetListVacation(userID);

                if (dataVacation.Count == 0)
                {
                    this.rptVacationList.DataSource = null;
                    this.rptVacationList.DataBind();

                    this.rptVacationHeader.DataSource = null;
                    this.rptVacationHeader.DataBind();
                }
                else
                {
                    var header = new List<AttendanceVacationInfo>() {
                            dataVacation[0]
                        };
                    this.rptVacationHeader.DataSource = header;
                    this.rptVacationHeader.DataBind();

                    dataVacation.RemoveAt(0);

                    this.rptVacationList.DataSource = dataVacation;
                    this.rptVacationList.DataBind();
                }
            }
        }

        /// <summary>
        /// InitCombobox
        /// </summary>
        private void InitCombobox()
        {
            this.LoadWorkingCalendarComboboxData();
            this.LoadDateOfServiceCombobox();
            this.LoadDepartmentComboboxData();
            this.LoadUserComboboxData();

            isShowbtnSubmitORbtnSubmitCancel = false;
            hasData = false;
        }

        /// <summary>
        /// Load LoadWorkingCalendarComboboxData
        /// </summary>
        private void LoadWorkingCalendarComboboxData()
        {
            using (DB db = new DB())
            {
                IList<DropDownModel> workingCalendarList;
                string defaultVal = string.Empty;
                T_WorkingCalendar_HService t_workingcalendar_hservice = new T_WorkingCalendar_HService(db);
                T_WorkingCalendar_UService t_workingcalendar_uservice = new T_WorkingCalendar_UService(db);

                workingCalendarList = t_workingcalendar_hservice.GetWorkingCalendarCbbData(ref defaultVal, this.LoginInfo.User.ID);

                if (!base._authority.IsAttendanceOtherDepartments)
                {
                    workingCalendarList = t_workingcalendar_hservice.GetWorkingCalendarCbbData(ref defaultVal, this.LoginInfo.User.ID, false, this.LoginInfo.User.DepartmentID);
                }

                if (!base._authority.IsAttendanceOtherEmployees)
                {
                    workingCalendarList = t_workingcalendar_hservice.GetWorkingCalendarCbbData(ref defaultVal, this.LoginInfo.User.ID, true);
                }

                IList<T_WorkingCalendar_U> item = t_workingcalendar_uservice.GetByUId(LoginInfo.User.ID);

                ddlWorkingCalendar.Items.Clear();
                if (workingCalendarList.Count > 0)
                {
                    ddlWorkingCalendar.DataSource = workingCalendarList;
                    ddlWorkingCalendar.DataValueField = "Value";
                    ddlWorkingCalendar.DataTextField = "DisplayName";
                    ddlWorkingCalendar.SelectedValue = defaultVal;
                    ddlWorkingCalendar.DataBind();

                    ddlDateOfService.Enabled = true;
                }
                else
                {
                    ddlWorkingCalendar.DataSource = null;
                    ddlWorkingCalendar.DataBind();
                    ddlDateOfService.Enabled = false;
                    ShowQuestionMessage(M_Message.MSG_NOT_EXIST_CALENDAR, Models.DefaultButton.No, false);
                }
            }
        }

        /// <summary>
        /// Load data DateOfService Combobox
        /// </summary>
        private void LoadDateOfServiceCombobox()
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
                ddlDateOfService.Items.Clear();

                ddlDateOfService.DataSource = setDataTableCombobox(DateOfServiceList);
                ddlDateOfService.SelectedValue = defaultVal;
                ddlDateOfService.DataValueField = "Value";
                ddlDateOfService.DataTextField = "DisplayName";
                ddlDateOfService.DataBind();
            }
        }

        /// <summary>
        /// Load Department Combobox data
        /// </summary>
        private void LoadDepartmentComboboxData()
        {
            int _calID = Constants.DEFAULT_VALUE_INT;
            if (!string.IsNullOrEmpty(ddlWorkingCalendar.SelectedValue))
            {
                _calID = int.Parse(this.ddlWorkingCalendar.SelectedValue);
            }

            using (DB db = new DB())
            {
                IList<DropDownModel> departmentList;
                DepartmentService departmentService = new DepartmentService(db);
                if (!base._authority.IsAttendanceOtherDepartments)
                {
                    departmentList = departmentService.GetDepartmentCbbData(_calID, LoginInfo.User.DepartmentID);
                }
                else
                {
                    departmentList = departmentService.GetDepartmentCbbData(_calID);
                }

                ddlDepartment.Items.Clear();

                ddlDepartment.DataSource = setDataTableCombobox(departmentList);
                ddlDepartment.DataValueField = "Value";
                ddlDepartment.DataTextField = "DisplayName";
                if (this.LoginInfo.User.DepartmentID >= 10)
                {
                    foreach (var item in departmentList)
                    {
                        if (item.Value == this.LoginInfo.User.DepartmentID.ToString())
                        {
                            ddlDepartment.SelectedValue = this.LoginInfo.User.DepartmentID.ToString();
                        }
                    }
                }
                ddlDepartment.DataBind();
            }
        }

        /// <summary>
        /// Load user combobox data
        /// </summary>
        private void LoadUserComboboxData()
        {
            int _calID = Constants.DEFAULT_VALUE_INT;
            if (!string.IsNullOrEmpty(ddlWorkingCalendar.SelectedValue))
            {
                _calID = int.Parse(this.ddlWorkingCalendar.SelectedValue);
            }

            using (DB db = new DB())
            {
                IList<DropDownModel> userList;
                UserService userService = new UserService(db);

                // Check Shelf Registration
                if (!base._authority.IsAttendanceOtherEmployees)
                {
                    userList = userService.GetCbbUserDataByDepartmentID(int.Parse(ddlDepartment.SelectedValue), _calID, userId: LoginInfo.User.ID);
                }
                else
                {
                    userList = userService.GetCbbUserDataByDepartmentID(int.Parse(ddlDepartment.SelectedValue), _calID);
                }

                this.UserList = userList;
                ddlUser.Items.Clear();

                ddlUser.DataSource = setDataTableCombobox(userList);
                ddlUser.DataValueField = "Value";
                ddlUser.DataTextField = "DisplayName";
                foreach (var item in userList)
                {
                    if (item.Value == this.LoginInfo.User.ID.ToString())
                    {
                        ddlUser.SelectedValue = this.LoginInfo.User.ID.ToString();
                    }
                }
                ddlUser.DataBind();
            }

            LoadUserComboboxAttribute();
        }

        /// <summary>
        /// Load user combobox attribute
        /// </summary>
        private void LoadUserComboboxAttribute()
        {
            if (this.UserList != null)
            {
                int index = 0;
                foreach (ListItem item in ddlUser.Items)
                {
                    if (item.Value != "-1")
                    {
                        if (this.UserList[index].Status != "0")
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
                if (this.ddlUser.SelectedIndex > 0)
                {
                    if (this.UserList[this.ddlUser.SelectedIndex - 1].Value != "-1" && this.UserList[this.ddlUser.SelectedIndex - 1].Status != "0")
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
        /// setDataTableCombobox
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
            dt.Rows[0].SetField("Value", Constants.DEFAULT_VALUE_STRING);
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
        /// InitVacation
        /// </summary>
        private void InitVacation()
        {
            using (DB db = new DB())
            {
                Config_DService config_DService = new Config_DService(db);

                IList<M_Config_D> vacationList;

                vacationList = config_DService.GetListByConfigCd(M_Config_H.CONFIG_CD_VACATION_TYPE);

                if (vacationList.Count > 0)
                {
                    rptVacationH1.DataSource = vacationList;
                    rptVacationD1.DataSource = vacationList;
                }
                else
                {
                    rptVacationH1.DataSource = null;
                    rptVacationD1.DataSource = null;
                }

                rptVacationH1.DataBind();
                rptVacationD1.DataBind();

            }
        }

        /// <summary>
        /// InitOverTime
        /// </summary>
        private void InitOverTime()
        {
            using (DB db = new DB())
            {
                Config_DService config_DService = new Config_DService(db);

                IList<M_Config_D> overTimeList;

                overTimeList = config_DService.GetListByConfigCd(M_Config_H.CONFIG_CD_OVER_TIME_TYPE);

                if (overTimeList.Count > 0)
                {
                    rptOverTimeH1.DataSource = overTimeList;

                }
                else
                {
                    rptOverTimeH1.DataSource = null;
                }
                rptOverTimeH1.DataBind();
            }
        }

        /// <summary>
        /// ConvertToDataCombobox
        /// </summary>
        /// <param name="initialDate"></param>
        /// <returns></returns>
        private DataTable ConvertToDataCombobox(DateTime initialDate, ref DateTime? flagDate)
        {
            DateTime dateCurent = DateTime.Parse(DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString());
            DataTable dataCombobox = new DataTable();
            dataCombobox.Columns.Add("Value", typeof(DateTime));
            dataCombobox.Columns.Add("Display", typeof(string));
            dataCombobox.Columns.Add("FlagCurrent", typeof(bool));

            DateTime fromDate = initialDate;
            DateTime toDate = new DateTime();
            for (int i = 0; i < 12; i++)
            {
                fromDate = initialDate.AddMonths(i);
                toDate = initialDate.AddMonths(i + 1).AddDays(-1);

                string stInitialDate = string.Empty;
                if (fromDate.Year != toDate.Year)
                {
                    stInitialDate = string.Format(Constants.FMR_DATE_YM, fromDate) + "〜" + string.Format(Constants.FMR_DATE_YM, toDate);
                }
                else
                {
                    if (fromDate.Month != toDate.Month)
                    {
                        stInitialDate = string.Format(Constants.FMR_DATE_YM, fromDate) + "〜" + string.Format(Constants.FMR_DATE_M, toDate);
                    }
                    else
                    {
                        stInitialDate = string.Format(Constants.FMR_DATE_YM, fromDate);
                    }
                }
                DataRow dr = dataCombobox.NewRow();
                if (dateCurent >= fromDate && dateCurent <= toDate)
                {
                    flagDate = fromDate;
                }

                dr["Value"] = fromDate;
                dr["Display"] = stInitialDate;
                dataCombobox.Rows.Add(dr);

                // set from day
                fromDate = fromDate.AddMonths(1);
            }

            dataCombobox.AcceptChanges();

            return dataCombobox;
        }

        /// <summary>
        /// ConvertListToDataTable
        /// </summary>
        /// <param name="listAttendanceDetailInfo"></param>
        /// <returns></returns>
        private DataTable ConvertListToDataTable(IList<AttendanceDetailInfo> listAttendanceDetailInfo, DateTime startDate, DateTime endStart, int count, DB db)
        {
            try
            {

                DataTable dataAttendanceDetailInfo = new DataTable();
                dataAttendanceDetailInfo.Columns.Add("ID", typeof(int));
                dataAttendanceDetailInfo.Columns.Add("UID", typeof(int));
                dataAttendanceDetailInfo.Columns.Add("StartDate", typeof(DateTime));
                dataAttendanceDetailInfo.Columns.Add("EndDate", typeof(DateTime));
                dataAttendanceDetailInfo.Columns.Add("Date", typeof(DateTime));
                dataAttendanceDetailInfo.Columns.Add("StringDate", typeof(string));
                dataAttendanceDetailInfo.Columns.Add("EntryTime", typeof(string));
                dataAttendanceDetailInfo.Columns.Add("ExitTime", typeof(string));
                dataAttendanceDetailInfo.Columns.Add("WorkingHours", typeof(string));
                dataAttendanceDetailInfo.Columns.Add("LateHours", typeof(string));
                dataAttendanceDetailInfo.Columns.Add("EarlyHours", typeof(string));
                dataAttendanceDetailInfo.Columns.Add("SH_Hours", typeof(string));
                dataAttendanceDetailInfo.Columns.Add("LH_Hours", typeof(string));

                dataAttendanceDetailInfo.Columns.Add("PrematureOvertimeWork", typeof(string));
                dataAttendanceDetailInfo.Columns.Add("RegularOvertime", typeof(string));
                dataAttendanceDetailInfo.Columns.Add("LateNightOvertime", typeof(string));
                dataAttendanceDetailInfo.Columns.Add("PredeterminedHolidayLateNight", typeof(string));
                dataAttendanceDetailInfo.Columns.Add("LegalHolidayLateNight", typeof(string));

                dataAttendanceDetailInfo.Columns.Add("WorkingSystemName", typeof(string));
                dataAttendanceDetailInfo.Columns.Add("TextColorClass", typeof(string));
                dataAttendanceDetailInfo.Columns.Add("TextColorCurrentDate", typeof(string));
                dataAttendanceDetailInfo.Columns.Add("ClassApprovalDetail", typeof(string));

                dataAttendanceDetailInfo.Columns.Add("Memo", typeof(string));
                dataAttendanceDetailInfo.Columns.Add("ExchangeStatus", typeof(string));

                /*Isv-Tinh 2020/04/13 ADD start Intervaltime config*/
                dataAttendanceDetailInfo.Columns.Add("BackColorInterValTime", typeof(string));
                /*Isv-Tinh 2020/04/13 ADD End */

                dataAttendanceDetailInfo.Columns.Add("ClassApprovalStatus", typeof(string));
                dataAttendanceDetailInfo.Columns.Add("ApprovalStatus", typeof(string));

                Config_DService config_service = new Config_DService(db);
                Config_HService dbCFSer = new Config_HService(db);
                var isUseTimeApproval = dbCFSer.GetDefaultValueDrop(M_Config_H.CONFIG_USE_TIME_APPROVAL) != "0";
                foreach (AttendanceDetailInfo item in listAttendanceDetailInfo)
                {
                    DataRow dr = dataAttendanceDetailInfo.NewRow();
                    dr["ID"] = item.ID;
                    dr["UID"] = item.UID;
                    dr["StartDate"] = startDate;
                    dr["EndDate"] = endStart;
                    dr["Date"] = item.Date;
                    dr["StringDate"] = item.StringDate;
                    if (item.WorkingSystemName != "&nbsp;")
                    {
                        dr["EntryTime"] = item.EntryTime != string.Empty ? item.EntryTime : "-";
                        dr["ExitTime"] = item.ExitTime != string.Empty ? item.ExitTime : "-";
                    }
                    else
                    {
                        dr["EntryTime"] = string.Empty;
                        dr["ExitTime"] = string.Empty;
                    }

                    dr["WorkingHours"] = item.WorkingHours;
                    dr["LateHours"] = item.LateHours;
                    dr["EarlyHours"] = item.EarlyHours;
                    dr["SH_Hours"] = item.SH_Hours;
                    dr["LH_Hours"] = item.LH_Hours;

                    dr["PrematureOvertimeWork"] = item.PrematureOvertimeWork;
                    dr["RegularOvertime"] = item.RegularOvertime;
                    dr["LateNightOvertime"] = item.LateNightOvertime;
                    dr["PredeterminedHolidayLateNight"] = item.PredeterminedHolidayLateNight;
                    dr["LegalHolidayLateNight"] = item.LegalHolidayLateNight;
                    dr["WorkingSystemName"] = item.WorkingSystemName;
                    dr["TextColorClass"] = item.TextColorClass;
                    dr["TextColorCurrentDate"] = item.TextColorCurrentDate;


                    if (item.ID == Constants.DEFAULT_VALUE_INT && count > 0)
                    {
                        dr["ClassApprovalDetail"] = "disabled";
                    }
                    else
                    {
                        dr["ClassApprovalDetail"] = string.Empty;
                    }

                    // 一覧への表示文字数は20～30文字程度で結構です。 

                    dr["Memo"] = item.Memo;
                    if (item.Memo.Trim().Length > 100)
                    {
                        dr["Memo"] = item.Memo.Substring(0, 100);
                    }
                    dr["ExchangeStatus"] = item.ExchangeStatus;

                    dr["ClassApprovalStatus"] = string.Empty;
                    dr["ApprovalStatus"] = string.Empty;

                    StringBuilder tag_status = new StringBuilder();
                    string value2 = string.Empty;
                    string value3 = string.Empty;

                    if (item.ApprovalStatus != AttendanceApprovalStatus.NeedApproval.GetHashCode())
                    {
                        dr["ClassApprovalStatus"] = "display : none;";
                    }
                    else
                    {
                        this.IsShowInfo = true;
                    }

                    if (item.ApprovalStatus == AttendanceApprovalStatus.Request.GetHashCode())
                    {
                        if (isUseTimeApproval)
                        {
                            if (item.Late_Early_Hours_Flg == 1)
                            {
                                //遅刻/早退
                                value2 = config_service.GetValue2(M_Config_H.APPROVAL_STATUS.ToString(), AttendanceApprovalShinseiKubun.Late_Early.GetHashCode());
                                value3 = config_service.GetValue3(M_Config_H.APPROVAL_STATUS.ToString(), AttendanceApprovalShinseiKubun.Late_Early.GetHashCode());
                                tag_status.Append(String.Format("<div style=\"height: 24px;\"><span class=\"label\" style=\"border: {0} solid 1px; color: {0};\">{1}</span></div>", value3, value2));
                            }
                            if (item.SH_Hours_Flg == 1)
                            {
                                //所定休日
                                value2 = config_service.GetValue2(M_Config_H.APPROVAL_STATUS.ToString(), AttendanceApprovalShinseiKubun.SH_Hours.GetHashCode());
                                value3 = config_service.GetValue3(M_Config_H.APPROVAL_STATUS.ToString(), AttendanceApprovalShinseiKubun.SH_Hours.GetHashCode());
                                tag_status.Append(String.Format("<div style=\"height: 24px;\"><span class=\"label\" style=\"border: {0} solid 1px; color: {0};\">{1}</span></div>", value3, value2));
                            }
                            if (item.LH_Hours_Flg == 1)
                            {
                                //法定休日
                                value2 = config_service.GetValue2(M_Config_H.APPROVAL_STATUS.ToString(), AttendanceApprovalShinseiKubun.LH_Hours.GetHashCode());
                                value3 = config_service.GetValue3(M_Config_H.APPROVAL_STATUS.ToString(), AttendanceApprovalShinseiKubun.LH_Hours.GetHashCode());
                                tag_status.Append(String.Format("<div style=\"height: 24px;\"><span class=\"label\" style=\"border: {0} solid 1px; color: {0};\">{1}</span></div>", value3, value2));
                            }
                            if (item.SH_OverTimeHours_Flg == 1)
                            {
                                //残業
                                value2 = config_service.GetValue2(M_Config_H.APPROVAL_STATUS.ToString(), AttendanceApprovalShinseiKubun.SH_OverTimeHours.GetHashCode());
                                value3 = config_service.GetValue3(M_Config_H.APPROVAL_STATUS.ToString(), AttendanceApprovalShinseiKubun.SH_OverTimeHours.GetHashCode());
                                tag_status.Append(String.Format("<div style=\"height: 24px;\"><span class=\"label\" style=\"border: {0} solid 1px; color: {0};\">{1}</span></div>", value3, value2));
                            }
                        }
                        if (item.VacationFlag >= 0)
                        {
                            //休暇
                            value2 = config_service.GetValue2(M_Config_H.APPROVAL_STATUS.ToString(), AttendanceApprovalShinseiKubun.Vacation.GetHashCode());
                            value3 = config_service.GetValue3(M_Config_H.APPROVAL_STATUS.ToString(), AttendanceApprovalShinseiKubun.Vacation.GetHashCode());
                            tag_status.Append(String.Format("<div style=\"height: 24px;\"><span class=\"label\" style=\"border: {0} solid 1px; color: {0};\">{1}</span></div>", value3, value2));
                        }

                    }
                    else if (item.ApprovalStatus == AttendanceApprovalStatus.Approved.GetHashCode())
                    {
                        if (isUseTimeApproval)
                        {
                            if (item.Late_Early_Hours_Flg == 1)
                            {
                                //遅刻/早退
                                value2 = config_service.GetValue2(M_Config_H.APPROVAL_STATUS.ToString(), AttendanceApprovalShinseiKubun.Late_Early.GetHashCode());
                                value3 = config_service.GetValue3(M_Config_H.APPROVAL_STATUS.ToString(), AttendanceApprovalShinseiKubun.Late_Early.GetHashCode());
                                tag_status.Append(String.Format("<div style=\"height: 23px;\"><span class=\"label\" style=\"background-color: {0};\">{1}</span></div>", value3, value2));
                            }
                            if (item.SH_Hours_Flg == 1)
                            {
                                //所定休日
                                value2 = config_service.GetValue2(M_Config_H.APPROVAL_STATUS.ToString(), AttendanceApprovalShinseiKubun.SH_Hours.GetHashCode());
                                value3 = config_service.GetValue3(M_Config_H.APPROVAL_STATUS.ToString(), AttendanceApprovalShinseiKubun.SH_Hours.GetHashCode());
                                tag_status.Append(String.Format("<div style=\"height: 23px;\"><span class=\"label\" style=\"background-color: {0};\">{1}</span></div>", value3, value2));
                            }
                            if (item.LH_Hours_Flg == 1)
                            {
                                //法定休日
                                value2 = config_service.GetValue2(M_Config_H.APPROVAL_STATUS.ToString(), AttendanceApprovalShinseiKubun.LH_Hours.GetHashCode());
                                value3 = config_service.GetValue3(M_Config_H.APPROVAL_STATUS.ToString(), AttendanceApprovalShinseiKubun.LH_Hours.GetHashCode());
                                tag_status.Append(String.Format("<div style=\"height: 23px;\"><span class=\"label\" style=\"background-color: {0};\">{1}</span></div>", value3, value2));
                            }
                            if (item.SH_OverTimeHours_Flg == 1)
                            {
                                //残業
                                value2 = config_service.GetValue2(M_Config_H.APPROVAL_STATUS.ToString(), AttendanceApprovalShinseiKubun.SH_OverTimeHours.GetHashCode());
                                value3 = config_service.GetValue3(M_Config_H.APPROVAL_STATUS.ToString(), AttendanceApprovalShinseiKubun.SH_OverTimeHours.GetHashCode());
                                tag_status.Append(String.Format("<div style=\"height: 23px;\"><span class=\"label\" style=\"background-color: {0};\">{1}</span></div>", value3, value2));
                            }
                        }
                        if (item.VacationFlag >= 0)
                        {
                            //休暇
                            value2 = config_service.GetValue2(M_Config_H.APPROVAL_STATUS.ToString(), AttendanceApprovalShinseiKubun.Vacation.GetHashCode());
                            value3 = config_service.GetValue3(M_Config_H.APPROVAL_STATUS.ToString(), AttendanceApprovalShinseiKubun.Vacation.GetHashCode());
                            tag_status.Append(String.Format("<div style=\"height: 23px;\"><span class=\"label\" style=\"background-color: {0};\">{1}</span></div>", value3, value2));
                        }
                    }
                    else if (item.ApprovalStatus == AttendanceApprovalStatus.Cancel.GetHashCode())
                    {
                        if (isUseTimeApproval)
                        {
                            if (item.Late_Early_Hours_Flg == 1)
                            {
                                //遅刻/早退
                                value2 = config_service.GetValue2(M_Config_H.APPROVAL_STATUS.ToString(), AttendanceApprovalShinseiKubun.Late_Early.GetHashCode());
                                value3 = config_service.GetValue4(M_Config_H.APPROVAL_STATUS.ToString(), AttendanceApprovalShinseiKubun.Late_Early.GetHashCode());
                                tag_status.Append(String.Format("<div style=\"height: 23px;\"><span class=\"label\" style=\"background-color: {0};\">{1}</span></div>", value3, value2));
                            }
                            if (item.SH_Hours_Flg == 1)
                            {
                                //所定休日
                                value2 = config_service.GetValue2(M_Config_H.APPROVAL_STATUS.ToString(), AttendanceApprovalShinseiKubun.SH_Hours.GetHashCode());
                                value3 = config_service.GetValue4(M_Config_H.APPROVAL_STATUS.ToString(), AttendanceApprovalShinseiKubun.SH_Hours.GetHashCode());
                                tag_status.Append(String.Format("<div style=\"height: 23px;\"><span class=\"label\" style=\"background-color: {0};\">{1}</span></div>", value3, value2));
                            }
                            if (item.LH_Hours_Flg == 1)
                            {
                                //法定休日
                                value2 = config_service.GetValue2(M_Config_H.APPROVAL_STATUS.ToString(), AttendanceApprovalShinseiKubun.LH_Hours.GetHashCode());
                                value3 = config_service.GetValue4(M_Config_H.APPROVAL_STATUS.ToString(), AttendanceApprovalShinseiKubun.LH_Hours.GetHashCode());
                                tag_status.Append(String.Format("<div style=\"height: 23px;\"><span class=\"label\" style=\"background-color: {0};\">{1}</span></div>", value3, value2));
                            }
                            if (item.SH_OverTimeHours_Flg == 1)
                            {
                                //残業
                                value2 = config_service.GetValue2(M_Config_H.APPROVAL_STATUS.ToString(), AttendanceApprovalShinseiKubun.SH_OverTimeHours.GetHashCode());
                                value3 = config_service.GetValue4(M_Config_H.APPROVAL_STATUS.ToString(), AttendanceApprovalShinseiKubun.SH_OverTimeHours.GetHashCode());
                                tag_status.Append(String.Format("<div style=\"height: 23px;\"><span class=\"label\" style=\"background-color: {0};\">{1}</span></div>", value3, value2));
                            }
                        }
                        if (item.VacationFlag >= 0)
                        {
                            //休暇
                            value2 = config_service.GetValue2(M_Config_H.APPROVAL_STATUS.ToString(), AttendanceApprovalShinseiKubun.Vacation.GetHashCode());
                            value3 = config_service.GetValue4(M_Config_H.APPROVAL_STATUS.ToString(), AttendanceApprovalShinseiKubun.Vacation.GetHashCode());
                            tag_status.Append(String.Format("<div style=\"height: 23px;\"><span class=\"label\" style=\"background-color: {0};\">{1}</span></div>", value3, value2));
                        }
                    }
                    dr["ApprovalStatus"] = tag_status;

                    dr["BackColorInterValTime"] = item.BackColorInterValTime;//Add by Isv-Tinh 2020/04/13 Add IntervalTime Config

                    dataAttendanceDetailInfo.Rows.Add(dr);
                }
                dataAttendanceDetailInfo.AcceptChanges();

                DataView dataAttendanceDetailInfoView = new DataView(dataAttendanceDetailInfo);
                dataAttendanceDetailInfoView.Sort = "Date";

                dataAttendanceDetailInfo = dataAttendanceDetailInfoView.ToTable();

                return dataAttendanceDetailInfo;
            }
            catch (Exception ex)
            {
                Log.Instance.WriteLog(ex);
                return null;
            }
        }

        /// <summary>
        /// SetDataHeader
        /// </summary>
        /// <param name="db"></param>
        /// <param name="atdService"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="userID"></param>
        private void SetDataHeader(DB db, AttendanceService atdService, DateTime startDate, DateTime endDate, int userID)
        {
            AttendanceHeaderInfo attendanceHeaderInfo = new AttendanceHeaderInfo();
            attendanceHeaderInfo = atdService.GetAttendanceHeaderInfo(startDate, endDate, userID);

            numWorkingDays.Text = attendanceHeaderInfo.NumWorkingDays.ToString() != "0" ? attendanceHeaderInfo.NumWorkingDays.ToString("#.0") : "&nbsp;";
            numLateDays.Text = attendanceHeaderInfo.NumLateDays.ToString() != "0" ? attendanceHeaderInfo.NumLateDays.ToString("#.0") : "&nbsp;";
            numEarlyDays.Text = attendanceHeaderInfo.NumEarlyDays.ToString() != "0" ? attendanceHeaderInfo.NumEarlyDays.ToString("#.0") : "&nbsp;";
            numLH_Days.Text = attendanceHeaderInfo.NumLH_Days.ToString() != "0" ? attendanceHeaderInfo.NumLH_Days.ToString("#.0") : "&nbsp;";
            numSH_Days.Text = attendanceHeaderInfo.NumSH_Days.ToString() != "0" ? attendanceHeaderInfo.NumSH_Days.ToString("#.0") : "&nbsp;";

            timeWorkingHours.Text = attendanceHeaderInfo.WorkingHours.ToString() != string.Empty ? attendanceHeaderInfo.WorkingHours.ToString() : "&nbsp;";
            timeLateHours.Text = attendanceHeaderInfo.LateHours.ToString() != string.Empty ? attendanceHeaderInfo.LateHours.ToString() : "&nbsp;";
            timeEarlyHours.Text = attendanceHeaderInfo.EarlyHours.ToString() != string.Empty ? attendanceHeaderInfo.EarlyHours.ToString() : "&nbsp;";
            timeLH_Hours.Text = attendanceHeaderInfo.LH_Hours.ToString() != string.Empty ? attendanceHeaderInfo.LH_Hours.ToString() : "&nbsp;";
            timeSH_Hours.Text = attendanceHeaderInfo.SH_Hours.ToString() != string.Empty ? attendanceHeaderInfo.SH_Hours.ToString() : "&nbsp;";

            Config_DService config_DService = new Config_DService(db);
            IList<M_Config_D> overTimeList;
            overTimeList = config_DService.GetListByConfigCd(M_Config_H.CONFIG_CD_OVER_TIME_TYPE);
            switch (overTimeList.Count)
            {
                case 0:
                    break;
                case 1:
                    overTimeList[0].Value4 = attendanceHeaderInfo.OverTimeHours1.ToString() != string.Empty ? attendanceHeaderInfo.OverTimeHours1.ToString() : "&nbsp;";
                    break;
                case 2:
                    overTimeList[0].Value4 = attendanceHeaderInfo.OverTimeHours1.ToString() != string.Empty ? attendanceHeaderInfo.OverTimeHours1.ToString() : "&nbsp;";
                    overTimeList[1].Value4 = attendanceHeaderInfo.OverTimeHours2.ToString() != string.Empty ? attendanceHeaderInfo.OverTimeHours2.ToString() : "&nbsp;";
                    break;
                case 3:
                    overTimeList[0].Value4 = attendanceHeaderInfo.OverTimeHours1.ToString() != string.Empty ? attendanceHeaderInfo.OverTimeHours1.ToString() : "&nbsp;";
                    overTimeList[1].Value4 = attendanceHeaderInfo.OverTimeHours2.ToString() != string.Empty ? attendanceHeaderInfo.OverTimeHours2.ToString() : "&nbsp;";
                    overTimeList[2].Value4 = attendanceHeaderInfo.OverTimeHours3.ToString() != string.Empty ? attendanceHeaderInfo.OverTimeHours3.ToString() : "&nbsp;";
                    break;
                case 4:
                    overTimeList[0].Value4 = attendanceHeaderInfo.OverTimeHours1.ToString() != string.Empty ? attendanceHeaderInfo.OverTimeHours1.ToString() : "&nbsp;";
                    overTimeList[1].Value4 = attendanceHeaderInfo.OverTimeHours2.ToString() != string.Empty ? attendanceHeaderInfo.OverTimeHours2.ToString() : "&nbsp;";
                    overTimeList[2].Value4 = attendanceHeaderInfo.OverTimeHours3.ToString() != string.Empty ? attendanceHeaderInfo.OverTimeHours3.ToString() : "&nbsp;";
                    overTimeList[3].Value4 = attendanceHeaderInfo.OverTimeHours4.ToString() != string.Empty ? attendanceHeaderInfo.OverTimeHours4.ToString() : "&nbsp;";
                    break;
                default:
                    overTimeList[0].Value4 = attendanceHeaderInfo.OverTimeHours1.ToString() != string.Empty ? attendanceHeaderInfo.OverTimeHours1.ToString() : "&nbsp;";
                    overTimeList[1].Value4 = attendanceHeaderInfo.OverTimeHours2.ToString() != string.Empty ? attendanceHeaderInfo.OverTimeHours2.ToString() : "&nbsp;";
                    overTimeList[2].Value4 = attendanceHeaderInfo.OverTimeHours3.ToString() != string.Empty ? attendanceHeaderInfo.OverTimeHours3.ToString() : "&nbsp;";
                    overTimeList[3].Value4 = attendanceHeaderInfo.OverTimeHours4.ToString() != string.Empty ? attendanceHeaderInfo.OverTimeHours4.ToString() : "&nbsp;";
                    overTimeList[4].Value4 = attendanceHeaderInfo.OverTimeHours5.ToString() != string.Empty ? attendanceHeaderInfo.OverTimeHours5.ToString() : "&nbsp;";
                    break;
            }

            if (overTimeList.Count == 0)
            {
                rptOverTimeD1.DataSource = null;
            }
            else
            {
                NumOvertime = overTimeList.Count;
                rptOverTimeD1.DataSource = overTimeList;
            }
            rptOverTimeD1.DataBind();

            totalOverTimeHours.Text = attendanceHeaderInfo.TotalOverTimeHours.ToString();
            totalWorkingHours.Text = attendanceHeaderInfo.TotalWorkingHours.ToString();
        }

        /// <summary>
        /// SetDataVacation
        /// </summary>
        /// <param name="listAttendanceDetailInfo"></param>
        private void SetDataVacation(IList<AttendanceDetailInfo> listAttendanceDetailInfo)
        {
            using (DB db = new DB())
            {
                Config_DService config_DService = new Config_DService(db);
                IList<M_Config_D> vacationList;
                vacationList = config_DService.GetListByConfigCd(M_Config_H.CONFIG_CD_VACATION_TYPE);
                foreach (M_Config_D item in vacationList)
                {
                    item.Value4 = "0";
                }

                foreach (AttendanceDetailInfo item in listAttendanceDetailInfo)
                {
                    foreach (M_Config_D item1 in vacationList)
                    {

                        if (item.VacationFlag == (int)VacationFlag.AllHolidays)
                        {
                            if (item.VacationFullCD == item1.Value1)
                            {
                                item1.Value4 = (double.Parse(item1.Value4) + 1).ToString();
                            }
                        }
                        else if (item.VacationFlag == (int)VacationFlag.Morning)
                        {
                            if (item.VacationMorningCD == item1.Value1)
                            {
                                item1.Value4 = (double.Parse(item1.Value4) + 0.4).ToString();
                            }
                        }
                        else if (item.VacationFlag == (int)VacationFlag.Afternoon)
                        {
                            if (item.VacationAfternoonCD == item1.Value1)
                            {
                                item1.Value4 = (double.Parse(item1.Value4) + 0.6).ToString();
                            }
                        }
                        else if (item.VacationFlag == (int)VacationFlag.MorningAndAfternoon)
                        {
                            if (item.VacationMorningCD == item1.Value1)
                            {
                                item1.Value4 = (double.Parse(item1.Value4) + 0.4).ToString();
                            }

                            if (item.VacationAfternoonCD == item1.Value1)
                            {
                                item1.Value4 = (double.Parse(item1.Value4) + 0.6).ToString();
                            }
                        }
                    }
                }

                foreach (M_Config_D item in vacationList)
                {
                    double value = double.Parse(item.Value4);
                    if (item.Value4 == "0")
                    {
                        item.Value4 = "&nbsp;";
                    }
                    else
                    {
                        item.Value4 = value.ToString("F1");
                    }
                }

                if (vacationList.Count == 0)
                {
                    rptVacationD1.DataSource = null;
                }
                else
                {
                    NumVacation = vacationList.Count;
                    rptVacationD1.DataSource = vacationList;
                }
                rptVacationD1.DataBind();
            }
        }

        /// <summary>
        /// SetDataInterValTime
        /// </summary>
        /// <param name="listAttendanceDetailInfo"></param>
        /// <param name="listAttendancePreviousDate"></param>
        /// <!--ISV-Tinh 2020/04/13 ADD Interval Time Config -->
        private void SetDataInterValTime(IList<AttendanceDetailInfo> listAttendanceDetailInfo, IList<AttendanceDetailInfo> listPrevious)
        {
            using (DB db = new DB())
            {
                Config_DService config_DService = new Config_DService(db);
                IList<M_Config_D> intervalTimeList;
                intervalTimeList = config_DService.GetListByConfigCd(M_Config_H.CONFIG_CD_INTERVAL_TIME);
                long intervalTick;
                string styleBgColog = string.Empty;
                if (!intervalTimeList.Any())
                {
                    return;
                }
                if (intervalTimeList[0].Value1 != 1)
                {
                    return;
                }
                styleBgColog = intervalTimeList[0].Value4;
                CultureInfo info = new CultureInfo("ja-JP");
                DateTime temp;
                if (DateTime.TryParseExact(intervalTimeList[0].Value2, "H:mm", info, DateTimeStyles.None, out temp))
                {
                    intervalTick = temp.TimeOfDay.Ticks;
                }
                else
                {
                    return;
                }
                bool firstDateCheck = false;
                long exitTimeTick;
                long entryTimeTick;
                for (var i = 0; i < listAttendanceDetailInfo.Count; i++)
                {
                    DateTime dtExitTimePrevious = DateTime.MinValue;
                    DateTime dtEntryTime = DateTime.MinValue;
                    if (!firstDateCheck)
                    {
                        if (listPrevious.Count > 0)
                        {
                            dtExitTimePrevious = listPrevious[0].Date.Date;
                            if (!string.IsNullOrEmpty(listAttendanceDetailInfo[i].EntryTime))
                            {
                                dtEntryTime = listAttendanceDetailInfo[i].Date.Date;

                                exitTimeTick = TimeSpan.FromMinutes(Utilities.CommonUtil.TimeToMinute(listPrevious[0].ExitTime)).Ticks;
                                entryTimeTick = TimeSpan.FromMinutes(Utilities.CommonUtil.TimeToMinute(listAttendanceDetailInfo[i].EntryTime)).Ticks;
                                if (dtEntryTime.AddTicks(entryTimeTick) < dtExitTimePrevious.AddTicks(exitTimeTick + intervalTick))
                                {
                                    listAttendanceDetailInfo[i].OutIntervalTime = true;
                                    listAttendanceDetailInfo[i].BackColorInterValTime = styleBgColog;
                                }
                                firstDateCheck = true;
                            }
                        }
                        else
                        {
                            firstDateCheck = true;
                        }


                        continue;
                    }
                    string strExitTime = string.Empty;
                    for (int j = i - 1; j >= 0; j--)
                    {
                        if (!string.IsNullOrEmpty(listAttendanceDetailInfo[j].ExitTime))
                        {
                            strExitTime = listAttendanceDetailInfo[j].ExitTime;
                            dtExitTimePrevious = listAttendanceDetailInfo[j].Date.Date;
                            break;
                        }
                    }
                    if (!string.IsNullOrEmpty(listAttendanceDetailInfo[i].EntryTime))
                    {
                        dtEntryTime = listAttendanceDetailInfo[i].Date.Date;
                        exitTimeTick = TimeSpan.FromMinutes(Utilities.CommonUtil.TimeToMinute(strExitTime)).Ticks;
                        entryTimeTick = TimeSpan.FromMinutes(Utilities.CommonUtil.TimeToMinute(listAttendanceDetailInfo[i].EntryTime)).Ticks;
                        if (dtEntryTime.AddTicks(entryTimeTick) < dtExitTimePrevious.AddTicks(exitTimeTick + intervalTick))
                        {
                            listAttendanceDetailInfo[i].OutIntervalTime = true;
                            listAttendanceDetailInfo[i].BackColorInterValTime = styleBgColog;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// SetHeaderOverTime
        /// </summary>
        private void SetHeaderOverTime()
        {
            using (DB db = new DB())
            {
                Config_DService config_DService = new Config_DService(db);
                IList<M_Config_D> overTimeList;
                overTimeList = config_DService.GetListByConfigCd(M_Config_H.CONFIG_CD_OVER_TIME_TYPE);

                Control HeaderTemplate = rptAttendanceList.Controls[0];

                Repeater rpChildOverTimeH2 = rptAttendanceList.Controls[0].FindControl("rptOverTimeH2") as Repeater;

                if (overTimeList.Count == 0)
                {
                    rpChildOverTimeH2.DataSource = null;
                }
                else
                {
                    rpChildOverTimeH2.DataSource = overTimeList;
                }
                rpChildOverTimeH2.DataBind();
            }
        }

        /// <summary>
        /// CheckHoliday
        /// </summary>
        /// <param name="date"></param>
        private bool CheckHoliday(DateTime date)
        {
            try
            {
                using (DB db = new DB())
                {
                    Config_DService config_DService = new Config_DService(db);
                    IList<M_Config_D> holidayList;
                    holidayList = config_DService.GetListByConfigCd(M_Config_H.CONFIG_CD_HOLIDAY_TYPE);

                    if (holidayList.Count > 0)
                    {
                        foreach (M_Config_D item in holidayList)
                        {
                            DateTime dt = DateTime.Parse(item.Value2);
                            if (date == dt)
                            {
                                return true;
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Log.Instance.WriteLog(ex);
                return false;
            }
        }

        /// <summary>
        /// CheckExistInUserListAdmin
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="userListAdmin"></param>
        /// <returns></returns>
        public bool CheckExistInUserListAdmin(int userID, IList<UserInfo> userListAdmin)
        {
            foreach (UserInfo item in userListAdmin)
            {
                if (item.ID == userID)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// CreateDataAttendance
        /// </summary>
        /// <param name="currentDate"></param>
        /// <param name="userId"></param>
        public T_Attendance CreateDataAttendanceHeader(DateTime currentDate, int userId, int workSystemId, Dictionary<DateTime, T_PaidLeave> dicPaidLeave)
        {
            using (DB db = new DB())
            {
                T_Attendance attendanceInfo = new T_Attendance();
                attendanceInfo.UID = userId;
                attendanceInfo.Date = currentDate;

                DateTime dt = currentDate;
                // check holiday 
                string nomalWorkingId = string.Empty;
                string holidayId = string.Empty;
                string legalHoliday = string.Empty;

                IList<M_Config_D> lstMConfigD;
                M_WorkingSystem mWorkingSystem = new M_WorkingSystem();
                Config_DService configDService = new Config_DService(db);
                T_WorkingCalendar_DService T_WorkingCalendar_DService = new T_WorkingCalendar_DService(db);
                T_WorkingCalendar_HService T_WorkingCalendar_HService = new T_WorkingCalendar_HService(db);

                lstMConfigD = configDService.GetListByConfigCd(M_Config_H.CONFIG_CD_HOLIDAY_TYPE);

                // get ID with workingtype;
                WorkingSystemService WorSystemService = new WorkingSystemService(db);
                mWorkingSystem = WorSystemService.GetDataWorkingSystemById(workSystemId);

                if (mWorkingSystem.WorkingType != (int)WorkingType.WorkFullTime)
                {
                    return null;
                }

                //set WorkingHours
                TimeSpan workingHours = new TimeSpan(0, 0, 0);

                DateTime[] arrBreakTime = new DateTime[8];
                DateTime[] arrOverTime = new DateTime[10];

                if (mWorkingSystem.WorkingType == 0)
                {
                    if (mWorkingSystem.WorkingSystemCD != "4" && !dicPaidLeave.ContainsKey(currentDate.Date))
                    {

                        string[] systemWorkingStart;
                        if (mWorkingSystem.Working_Start != null)
                        {
                            systemWorkingStart = (Utilities.CommonUtil.IntToTime((int)mWorkingSystem.Working_Start, true)).Split(':');
                        }
                        else
                        {
                            systemWorkingStart = null;
                        }
                        string[] systemWorkingEnd;
                        if (mWorkingSystem.Working_End != null)
                        {
                            systemWorkingEnd = (Utilities.CommonUtil.IntToTime((int)mWorkingSystem.Working_End, true)).Split(':');
                        }
                        else
                        {
                            systemWorkingEnd = null;
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
                        DateTime workST = new DateTime();
                        DateTime workET = new DateTime();
                        if (systemWorkingStart != null)
                        {
                            workST = new DateTime(1, 1, 1).AddHours(int.Parse(systemWorkingStart[0])).AddMinutes(int.Parse(systemWorkingStart[1]));
                            workET = new DateTime(1, 1, 1).AddHours(int.Parse(systemWorkingEnd[0])).AddMinutes(int.Parse(systemWorkingEnd[1]));
                        }
                        if (mWorkingSystem.BreakType == 0)
                        {
                            // BreakTime Hours
                            TimeSpan BreakTimeHour = new TimeSpan(arrBreakTime[0].Hour, arrBreakTime[0].Minute, 0);

                            //Working time
                            workingHours = CalDurationWorkTime(workST, workET, arrBreakTime);
                        }
                        else if (mWorkingSystem.BreakType == 1)
                        {
                            // BreakTime Hours
                            TimeSpan BreakTimeHour = new TimeSpan(arrBreakTime[0].Hour, arrBreakTime[0].Minute, 0);

                            //Working time
                            workingHours = CalDurationWorkTime(workST, workET, arrBreakTime).Subtract(BreakTimeHour);
                        }
                        else if (mWorkingSystem.BreakType == 2)
                        {
                            // BreakTime Hours
                            TimeSpan TimeHourToBreak = new TimeSpan(arrBreakTime[0].Hour, arrBreakTime[0].Minute, 0);
                            TimeSpan TimeBreak = new TimeSpan(arrBreakTime[1].Hour, arrBreakTime[1].Minute, 0);
                            //Working time
                            TimeSpan TotalWorkInDay = workET.Subtract(workST);
                            workingHours = new TimeSpan(0, 0, 0);
                            TimeSpan timecheck = new TimeSpan(0, 0, 0);
                            if (TotalWorkInDay < TimeHourToBreak || TotalWorkInDay < TimeHourToBreak.Add(TimeBreak))
                            {
                                workingHours = TotalWorkInDay;
                            }
                            else
                            {
                                while (timecheck < TotalWorkInDay)
                                {
                                    timecheck = timecheck.Add(TimeHourToBreak);
                                    if (timecheck < TotalWorkInDay)
                                    {
                                        timecheck = timecheck.Add(TimeBreak);
                                        workingHours = workingHours.Add(TimeHourToBreak);
                                    }
                                    else
                                    {
                                        workingHours = workingHours.Add(TotalWorkInDay.Subtract(timecheck.Subtract(TimeHourToBreak)));
                                        break;
                                    }
                                }
                            }
                        }

                        attendanceInfo.EntryTime = mWorkingSystem.Working_Start;
                        attendanceInfo.ExitTime = mWorkingSystem.Working_End;
                        if (Utilities.CommonUtil.TimeToInt(FormatTimeResult(workingHours)) != 0)
                        {
                            attendanceInfo.WorkingHours = Utilities.CommonUtil.TimeToInt(FormatTimeResult(workingHours));
                            attendanceInfo.TotalWorkingHours = Utilities.CommonUtil.TimeToInt(FormatTimeResult(workingHours));
                        }

                    }
                    else
                    {
                        attendanceInfo.VacationFlag = (int)VacationFlag.AllHolidays;
                        attendanceInfo.VacationFullCD = (int)Vacation.AnnualPaid;
                    }
                }

                if (attendanceInfo.VacationFlag.HasValue)
                {
                    attendanceInfo.ApprovalStatus = (int)AttendanceApprovalStatus.NeedApproval;
                }
                else
                {
                    attendanceInfo.ApprovalStatus = (int)AttendanceApprovalStatus.None;
                }
                attendanceInfo.ApprovalUID = 0;
                attendanceInfo.ApprovalDate = null;
                attendanceInfo.ApprovalNote = string.Empty;
                attendanceInfo.RequestNote = string.Empty;

                attendanceInfo.StatusFlag = 0;
                attendanceInfo.UpdateUID = this.LoginInfo.User.ID;
                attendanceInfo.CreateUID = this.LoginInfo.User.ID;

                return attendanceInfo;
            }
        }

        /// <summary>
        /// Format time result
        /// </summary>
        /// <param name="inVal"></param>
        /// <returns></returns>
        private static string FormatTimeResult(TimeSpan inVal)
        {
            if (inVal.TotalMinutes > 0)
            {
                return string.Format("{0:00}:{1:00}", inVal.Hours, inVal.Minutes);
            }
            else
            {
                return string.Empty;
            }
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
        /// <param name="lstRestTime">list rest time</param>
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
        public T_Work_H CreateDataAttendanceDetail(DateTime currentDate, int userId, int HID)
        {
            T_Work_H t_Work_H = new T_Work_H();
            t_Work_H.HID = HID;
            t_Work_H.UID = userId;
            t_Work_H.Date = currentDate;
            t_Work_H.CreateUID = this.LoginInfo.User.ID;
            t_Work_H.UpdateUID = this.LoginInfo.User.ID;

            return t_Work_H;
        }

        /// <summary>
        /// Show message question
        /// </summary>
        /// <param name="messageID">MessageID</param>
        /// <param name="defaultButton">Default Button</param>
        protected void ShowQuestionMessage(string messageID, DefaultButton defaultButton, bool isAddNew = false, params string[] args)
        {
            if (isAddNew)
            {
                flagNew = true;
            }
            else
            {
                flagNew = false;
            }
            flagSubmitCancel = false;
            flagSubmit = false;

            //Get Message
            M_Message mess = (M_Message)this.Messages[messageID];
            HtmlGenericControl questionMessage = (HtmlGenericControl)this.Master.FindControl("questionMessage");
            questionMessage.InnerHtml = "<p>" + " " + string.Format(mess.Message3, args) + "</p>";

            this.IsShowQuestion = true;

            LinkButton btnNo = (LinkButton)this.Master.FindControl("btnNo");
            btnNo.Visible = isAddNew;
            if (defaultButton == Models.DefaultButton.Yes)
            {
                this.DefaultButton = "#btnYes";
            }
            else
            {
                if (isAddNew)
                {
                    this.DefaultButton = "#btnNo";
                }
            }
        }

        #endregion

        #region Event Excel

        /// <summary>
        /// btnExcel Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcel_Click(object sender, CommandEventArgs e)
        {

            AttendanceListExcelModal attendanceListExcelModal = new AttendanceListExcelModal();
            attendanceListExcelModal.VacationType = new ArrayList();
            attendanceListExcelModal.OverTimeList = new ArrayList();
            attendanceListExcelModal.DataDetailList = new ArrayList();
            attendanceListExcelModal.OverTimeForDayList = new ArrayList();
            IList<AttendanceDetailInfo> listAttendanceDetailInfo;
            AttendanceHeaderInfo attendanceHeaderInfo = new AttendanceHeaderInfo();
            // DataTable dataAttendanceDetailInfo;
            IList<M_Config_D> overTimeList;
            IList<M_Config_D> vacationList;
            ArrayList OverTimeGird = new ArrayList();

            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MinValue;

            if (ddlDateOfService.SelectedValue != string.Empty)
            {
                startDate = DateTime.Parse(ddlDateOfService.SelectedValue);
                endDate = startDate.AddMonths(1).AddDays(-1);
            }
            int userID = ddlUser.SelectedValue != string.Empty ? int.Parse(ddlUser.SelectedValue) : Constants.DEFAULT_VALUE_INT;

            M_User user = getUserById(userID);

            attendanceListExcelModal.DateOfService = startDate.ToString("yyyy'年'MM'月'") + "~" + endDate.ToString("yyyy'年'MM'月'");
            attendanceListExcelModal.Department = getDepartmentById(int.Parse(this.ddlDepartment.SelectedValue)).DepartmentName.ToString();
            attendanceListExcelModal.UserCD = EditDataUtil.ToFixCodeShow(user.UserCD, M_User.MAX_USER_CODE_SHOW);
            attendanceListExcelModal.UserNm = user.UserName1;

            using (DB db = new DB())
            {

                AttendanceService atdService = new AttendanceService(db);

                listAttendanceDetailInfo = atdService.GetListByCond(startDate, endDate, userID);
                attendanceHeaderInfo = atdService.GetAttendanceHeaderInfo(startDate, endDate, userID);

                /* Isv-Tinh 2020/04/13 ADD Start -- config interval time */
                IList<AttendanceDetailInfo> listPreviousStartDate = atdService.GetPreviousRecordByCond(startDate, userID);
                SetDataInterValTime(listAttendanceDetailInfo, listPreviousStartDate);
                /* Isv-Tinh 2020/04/13 ADD End */

                //CreateDataDetail(startDate, endDate, ref listAttendanceDetailInfo);

                Config_DService config_DService = new Config_DService(db);

                vacationList = config_DService.GetListByConfigCd(M_Config_H.CONFIG_CD_VACATION_TYPE);
                overTimeList = config_DService.GetListByConfigCd(M_Config_H.CONFIG_CD_OVER_TIME_TYPE);

            }

            foreach (M_Config_D item in vacationList)
            {
                item.Value4 = "0";
            }

            listAttendanceDetailInfo = listAttendanceDetailInfo.OrderBy(x => x.Date).ToList();
            foreach (var item in listAttendanceDetailInfo)
            {

                int workDId = item.ID;

                List<string> restedContentList = getRestedContent(item.VacationFullCD, item.VacationMorningCD, item.VacationAfternoonCD);

                item.ProjectInfo = getProjectInfo(workDId);
                if (restedContentList[0] != "")
                {
                    if (item.ProjectInfo != "")
                    {
                        item.ProjectInfo = restedContentList[0] + ", " + item.ProjectInfo;
                    }
                    else
                    {
                        item.ProjectInfo = restedContentList[0];
                    }

                    item.VacaitionColor = restedContentList;
                    item.VacaitionColor.RemoveAt(0);
                }
                else
                {
                    item.VacaitionColor = new List<string>();
                }


                item.TotalOverTimeForDay = getTotalOverTimeForday(item.PrematureOvertimeWork, item.RegularOvertime, item.LateNightOvertime, item.PredeterminedHolidayLateNight, item.LegalHolidayLateNight);
                item.TotalWorkingHoursForDay = getTotalWorkingHoursForDay(item.TotalOverTimeForDay, item.WorkingHours, item.SH_Hours, item.LH_Hours);

                string textColorCurrentDate = string.Empty;

                item.HolidayName = getHolidayName(item.Date);
                attendanceListExcelModal.DataDetailList.Add(item);
                foreach (M_Config_D item1 in vacationList)
                {
                    if (item.VacationFlag == (int)VacationFlag.AllHolidays)
                    {
                        if (item.VacationFullCD == item1.Value1)
                        {
                            item1.Value4 = (double.Parse(item1.Value4) + 1).ToString();
                        }
                    }
                    else if (item.VacationFlag == (int)VacationFlag.Morning)
                    {
                        if (item.VacationMorningCD == item1.Value1)
                        {
                            item1.Value4 = (double.Parse(item1.Value4) + 0.4).ToString();
                        }
                    }
                    else if (item.VacationFlag == (int)VacationFlag.Afternoon)
                    {
                        if (item.VacationAfternoonCD == item1.Value1)
                        {
                            item1.Value4 = (double.Parse(item1.Value4) + 0.6).ToString();
                        }
                    }
                    else if (item.VacationFlag == (int)VacationFlag.MorningAndAfternoon)
                    {
                        if (item.VacationMorningCD == item1.Value1)
                        {
                            item1.Value4 = (double.Parse(item1.Value4) + 0.4).ToString();
                        }

                        if (item.VacationAfternoonCD == item1.Value1)
                        {
                            item1.Value4 = (double.Parse(item1.Value4) + 0.6).ToString();
                        }
                    }
                }
            }

            switch (overTimeList.Count)
            {
                case 0:
                    break;
                case 1:
                    overTimeList[0].Value4 = attendanceHeaderInfo.OverTimeHours1.ToString();
                    break;
                case 2:
                    overTimeList[0].Value4 = attendanceHeaderInfo.OverTimeHours1.ToString();
                    overTimeList[1].Value4 = attendanceHeaderInfo.OverTimeHours2.ToString();
                    break;
                case 3:
                    overTimeList[0].Value4 = attendanceHeaderInfo.OverTimeHours1.ToString();
                    overTimeList[1].Value4 = attendanceHeaderInfo.OverTimeHours2.ToString();
                    overTimeList[2].Value4 = attendanceHeaderInfo.OverTimeHours3.ToString();
                    break;
                case 4:
                    overTimeList[0].Value4 = attendanceHeaderInfo.OverTimeHours1.ToString();
                    overTimeList[1].Value4 = attendanceHeaderInfo.OverTimeHours2.ToString();
                    overTimeList[2].Value4 = attendanceHeaderInfo.OverTimeHours3.ToString();
                    overTimeList[3].Value4 = attendanceHeaderInfo.OverTimeHours4.ToString();
                    break;
                default:
                    overTimeList[0].Value4 = attendanceHeaderInfo.OverTimeHours1.ToString();
                    overTimeList[1].Value4 = attendanceHeaderInfo.OverTimeHours2.ToString();
                    overTimeList[2].Value4 = attendanceHeaderInfo.OverTimeHours3.ToString();
                    overTimeList[3].Value4 = attendanceHeaderInfo.OverTimeHours4.ToString();
                    overTimeList[4].Value4 = attendanceHeaderInfo.OverTimeHours5.ToString();
                    break;
            }

            foreach (M_Config_D item in vacationList)
            {
                double value = double.Parse(item.Value4);
                if (item.Value4 == "0")
                {
                    item.Value4 = "";
                }
                else
                {
                    item.Value4 = value.ToString("F1");
                }

                attendanceListExcelModal.VacationType.Add(item);
            }


            foreach (var item in overTimeList)
            {
                attendanceListExcelModal.OverTimeList.Add(item);
            }

            attendanceListExcelModal.NumWorkingDays = attendanceHeaderInfo.NumWorkingDays.ToString();
            attendanceListExcelModal.NumSH_Days = attendanceHeaderInfo.NumSH_Days.ToString();
            attendanceListExcelModal.NumLH_Days = attendanceHeaderInfo.NumLH_Days.ToString();
            attendanceListExcelModal.NumLateDays = attendanceHeaderInfo.NumLateDays.ToString();
            attendanceListExcelModal.NumEarlyDays = attendanceHeaderInfo.NumEarlyDays.ToString();

            attendanceListExcelModal.TimeEarlyHours = attendanceHeaderInfo.EarlyHours.ToString();
            attendanceListExcelModal.TimeLateHours = attendanceHeaderInfo.LateHours.ToString();
            attendanceListExcelModal.TimeLH_Hours = attendanceHeaderInfo.LH_Hours.ToString();
            attendanceListExcelModal.TimeSH_Hours = attendanceHeaderInfo.SH_Hours.ToString();
            attendanceListExcelModal.TimeWorkingHours = attendanceHeaderInfo.WorkingHours.ToString();


            attendanceListExcelModal.TotalOverTimeHours = attendanceHeaderInfo.TotalOverTimeHours.ToString();
            attendanceListExcelModal.TotalWorkingHours = attendanceHeaderInfo.TotalWorkingHours.ToString();

            AttendanceListExcel excel = new AttendanceListExcel();
            excel.modelInput = attendanceListExcelModal;
            IWorkbook wb = excel.OutputExcel();

            if (wb != null)
            {
                this.SaveFile(wb, ".xlsx");
            }

            if (formNameDir == string.Empty || formNameDir == null)
            {
                isApprovalForm = false;
                showSubmitCancel();

            }
            else
            {
                isApprovalForm = true;
            }

        }
        #endregion

        #region btnBack Button
        /// <summary>
        /// Event btnBackNew Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (formNameDir == string.Empty || formNameDir == null)
            {
                Server.Transfer("~/Menu/FrmMainMenu.aspx");

            }
            else
            {
                Server.Transfer("~/AttendanceApproval/FrmAttendanceApproval.aspx");

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

                filename = string.Format(ATTENDANCELIST_DOWNLOAD, DateTime.Now.ToString(FMT_YMDHMM));
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
        }

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

        private string getProjectInfo(int HID)
        {
            string info = "";
            IList<WorkDInfoEXcel> workDInfoEXcel;
            using (DB db = new DB())
            {
                Work_DService workDService = new Work_DService(db);
                workDInfoEXcel = workDService.GetListWorkDInfoEXcelByHID(HID);
            }

            for (int i = 0; i < workDInfoEXcel.Count; i++)
            {
                WorkDInfoEXcel item = workDInfoEXcel[i];

                if (i == workDInfoEXcel.Count - 1)
                {
                    info = info + "[" + item.workTime + "]" + " " + item.projectNm;
                }
                else
                {
                    info = info + "[" + item.workTime + "]" + " " + item.projectNm + ", ";
                }
            }
            return info;
        }

        private string getTotalOverTimeForday(string prematureOvertimeWork, string regularOvertime, string lateNightOvertime, string predeterminedHolidayLateNight, string legalHolidayLateNight)
        {
            string total = "";
            int sum = 0;
            sum = Utilities.CommonUtil.TimeToInt(prematureOvertimeWork == null ? "" : prematureOvertimeWork)
                + Utilities.CommonUtil.TimeToInt(regularOvertime == null ? "" : regularOvertime)
                + Utilities.CommonUtil.TimeToInt(lateNightOvertime == null ? "" : lateNightOvertime)
                 + Utilities.CommonUtil.TimeToInt(predeterminedHolidayLateNight == null ? "" : predeterminedHolidayLateNight)
                + Utilities.CommonUtil.TimeToInt(legalHolidayLateNight == null ? "" : legalHolidayLateNight);

            if (sum != 0)
            {
                total = Utilities.CommonUtil.IntToTime(sum, false);
            }
            return total;
        }

        private string getTotalWorkingHoursForDay(string TotalOverTimeForday, string workingtime, string sH_Hours, string lH_Hours)
        {
            string total = "";
            int sum = 0;
            sum = Utilities.CommonUtil.TimeToInt(TotalOverTimeForday == null ? "" : TotalOverTimeForday)
                + Utilities.CommonUtil.TimeToInt(workingtime == null ? "" : workingtime)
                + Utilities.CommonUtil.TimeToInt(sH_Hours == null ? "" : sH_Hours)
                + Utilities.CommonUtil.TimeToInt(lH_Hours == null ? "" : lH_Hours);

            if (sum != 0)
            {
                total = Utilities.CommonUtil.IntToTime(sum, false);
            }
            return total;
        }

        private string getHolidayName(DateTime date)
        {
            try
            {
                using (DB db = new DB())
                {
                    Config_DService config_DService = new Config_DService(db);
                    IList<M_Config_D> holidayList;
                    holidayList = config_DService.GetListByConfigCd(M_Config_H.CONFIG_CD_HOLIDAY_TYPE);

                    if (holidayList.Count > 0)
                    {
                        foreach (M_Config_D item in holidayList)
                        {
                            DateTime dt = DateTime.Parse(item.Value2);
                            if (date == dt)
                            {
                                return item.Value3;
                            }
                        }
                    }
                    else
                    {
                        return "";
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                Log.Instance.WriteLog(ex);
                return "";
            }

        }

        private List<string> getRestedContent(int vacationFullCD, int vacationMorningCD, int vacationAfternoonCD)
        {

            List<string> result = new List<string>();
            result.Add(string.Empty);

            try
            {
                using (DB db = new DB())
                {
                    Config_DService config_DService = new Config_DService(db);

                    if (vacationFullCD != Constants.DEFAULT_VALUE_INT)
                    {
                        result[0] += "全休: " + config_DService.GetValue2(M_Config_H.CONFIG_CD_VACATION_TYPE, vacationFullCD);
                        result.Add(string.Format("{0}_{1}", 4, result[0].Length));
                    }
                    else
                    {
                        if (vacationMorningCD != Constants.DEFAULT_VALUE_INT)
                        {
                            result[0] += "前半休: " + config_DService.GetValue2(M_Config_H.CONFIG_CD_VACATION_TYPE, vacationMorningCD);
                            result.Add(string.Format("{0}_{1}", 5, result[0].Length));
                        }

                        if (vacationAfternoonCD != Constants.DEFAULT_VALUE_INT)
                        {
                            if (result[0] != "")
                            {
                                result[0] = result[0] + ", " + "後半休: " + config_DService.GetValue2(M_Config_H.CONFIG_CD_VACATION_TYPE, vacationAfternoonCD);
                                result.Add(string.Format("{0}_{1}", result[0].Length - config_DService.GetValue2(M_Config_H.CONFIG_CD_VACATION_TYPE, vacationAfternoonCD).Length - 1, result[0].Length));
                            }
                            else
                            {
                                result[0] = "後半休: " + config_DService.GetValue2(M_Config_H.CONFIG_CD_VACATION_TYPE, vacationAfternoonCD);
                                result.Add(string.Format("{0}_{1}", 5, result[0].Length));
                            }

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Log.Instance.WriteLog(ex);
                return result;
            }

            return result;

        }
        #endregion

        #region Event Submission

        /// <summary>
        /// btnSubmission_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmission_Click(object sender, CommandEventArgs e)
        {

            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MinValue;
            int userID = ddlUser.SelectedValue != string.Empty ? int.Parse(ddlUser.SelectedValue) : Constants.DEFAULT_VALUE_INT;

            if (ddlDateOfService.SelectedValue != string.Empty)
            {
                startDate = DateTime.Parse(ddlDateOfService.SelectedValue);
                endDate = startDate.AddMonths(1).AddDays(-1);
            }

            if (!checkSubmitssion(userID, startDate, endDate))
            {
                ShowMessageUpdateSubmitssionErrors(M_Message.MSG_SUBMIT_ERROR, Models.DefaultButton.No);

            }
            else if (HaveUnAprrovaledData(userID, startDate, endDate))
            {
                ShowMessageUpdateSubmitssionErrors(M_Message.MSG_SUBMIT_APPROVAL_ERROR, Models.DefaultButton.No);
            }
            else
            {

                ShowMessageUpdateSubmitssion(M_Message.MSG_SUBMIT, Models.DefaultButton.No, true);

                showSubmitCancel();
                CheckAuthority();

            }
        }

        /// <summary>
        /// btnSubmissionCancel_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmissionCancel_Click(object sender, CommandEventArgs e)
        {

            ShowMessageUpdateSubmitssionCancel(M_Message.MSG_SUBMIT_CANCEL, Models.DefaultButton.No, true);

            showSubmitCancel();
            CheckAuthority();

        }
        #endregion

        #region Method Submitssion

        /// <summary>
        /// check data when btnSubmission_Click
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private bool HaveUnAprrovaledData(int userID, DateTime startDate, DateTime endDate)
        {
            using (DB db = new DB())
            {
                AttendanceService atdService = new AttendanceService(db);

                return atdService.HaveUnAprrovaledData(userID, startDate, endDate);
            }
        }


        /// <summary>
        /// check data when btnSubmission_Click
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private bool checkSubmitssion(int userID, DateTime startDate, DateTime endDate)
        {
            bool bsubmit = false;

            int count = 0;

            using (DB db = new DB())
            {
                AttendanceService atdService = new AttendanceService(db);

                count = atdService.checkSubmission(userID, startDate, endDate);
            }

            if (count > 0)
            {
                bsubmit = false;
            }
            else
            {
                bsubmit = true;
            }

            return bsubmit;
        }

        /// <summary>
        /// check data when btnSubmission_Click
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private bool CheckRegistDefault(int calendarID,int userID, DateTime startDate, DateTime endDate)
        {

            using (DB db = new DB())
            {
                AttendanceService atdService = new AttendanceService(db);
                return atdService.IsEnoughPaidVacation(calendarID, userID, startDate, endDate);
            }

        }

        /// <summary>
        /// updateStatusFlag
        /// </summary>
        /// <param name="attendance"></param>
        /// <param name="value"></param>
        private bool updateStatusFlag(DB db, T_Attendance attendance, int value)
        {
            try
            {
                int ret = 0;

                if (attendance != null)
                {
                    AttendanceService atdService = new AttendanceService(db);

                    attendance.StatusFlag = value;
                    attendance.UpdateUID = this.LoginInfo.User.ID;
                    ret = atdService.Update(attendance);

                    if (ret == 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                this.SetMessage(string.Empty, M_Message.MSG_UPDATE_FAILE, "更新");

                Log.Instance.WriteLog(ex);
                return false;
            }

        }

        /// <summary>
        /// Show message question when btnSubmission_Click
        /// </summary>
        /// <param name="messageID">MessageID</param>
        /// <param name="defaultButton">Default Button</param>
        protected void ShowMessageUpdateSubmitssion(string messageID, DefaultButton defaultButton, bool issubmitbool = false, params string[] args)
        {
            if (issubmitbool)
            {
                flagSubmit = true;
            }
            else
            {
                flagSubmit = false;
            }
            flagNew = false;
            flagSubmitCancel = false;

            //Get Message
            M_Message mess = (M_Message)this.Messages[messageID];
            HtmlGenericControl questionMessage = (HtmlGenericControl)this.Master.FindControl("questionMessage");
            questionMessage.InnerHtml = "<p>" + " " + string.Format(mess.Message3, args) + "</p>";

            this.IsShowQuestion = true;

            LinkButton btnNo = (LinkButton)this.Master.FindControl("btnNo");
            btnNo.Visible = issubmitbool;
            if (defaultButton == Models.DefaultButton.Yes)
            {
                this.DefaultButton = "#btnYes";
            }
            else
            {
                if (issubmitbool)
                {
                    this.DefaultButton = "#btnNo";
                }
            }
        }

        /// <summary>
        /// Show message question when btnSubmissionCancel_Click
        /// </summary>
        /// <param name="messageID">MessageID</param>
        /// <param name="defaultButton">Default Button</param>
        protected void ShowMessageUpdateSubmitssionCancel(string messageID, DefaultButton defaultButton, bool issubmitbool = false, params string[] args)
        {
            flagSubmitCancel = issubmitbool;
            flagNew = false;
            flagSubmit = false;

            //Get Message
            M_Message mess = (M_Message)this.Messages[messageID];
            HtmlGenericControl questionMessage = (HtmlGenericControl)this.Master.FindControl("questionMessage");
            questionMessage.InnerHtml = "<p>" + " " + string.Format(mess.Message3, args) + "</p>";

            this.IsShowQuestion = true;

            LinkButton btnNo = (LinkButton)this.Master.FindControl("btnNo");
            btnNo.Visible = issubmitbool;
            if (defaultButton == Models.DefaultButton.Yes)
            {
                this.DefaultButton = "#btnYes";
            }
            else
            {
                if (issubmitbool)
                {
                    this.DefaultButton = "#btnNo";
                }
            }
        }

        /// <summary>
        /// Show message question SubmitssionErrors
        /// </summary>
        /// <param name="messageID">MessageID</param>
        /// <param name="defaultButton">Default Button</param>
        protected void ShowMessageUpdateSubmitssionErrors(string messageID, DefaultButton defaultButton, bool issubmitbool = false, params string[] args)
        {

            flagSubmit = false;
            flagNew = false;
            flagSubmitCancel = false;

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

        /// <summary>
        /// btnSubmitCancel is AttendanceStatusFlag = 1
        /// </summary>
        private bool showSubmitCancel()
        {
            bool result = true;
            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MinValue;
            int userID = ddlUser.SelectedValue != string.Empty ? int.Parse(ddlUser.SelectedValue) : Constants.DEFAULT_VALUE_INT;
            int count = 0;
            int countApproval = 0;

            if (ddlDateOfService.SelectedValue != string.Empty && ddlDateOfService.SelectedValue != Constants.DEFAULT_VALUE_STRING)
            {
                startDate = DateTime.Parse(ddlDateOfService.SelectedValue);
                endDate = startDate.AddMonths(1).AddDays(-1);
            }

            using (DB db = new DB())
            {
                AttendanceService atdService = new AttendanceService(db);

                count = atdService.checkAttendanceSubmit(userID, startDate, endDate);
                countApproval = atdService.checkAttendanceApproval(userID, startDate, endDate);

            }

            if (count > 0)
            {

                this.ShowSubmitCance = true;

                if (countApproval > 0)
                {
                    this.btnSubmissionCancel.Enabled = false;
                    this.btnSubmitCancelButtom.Enabled = false;
                    this.btnSubmissionCancel.CssClass += " disabled";
                    this.btnSubmitCancelButtom.CssClass += " disabled";

                    this.nameButtonApproval = "承認済";
                }
                else
                {
                    this.btnSubmission.Enabled = true;
                    this.btnSubmissionCancel.Enabled = true;
                    this.btnSubmission.CssClass = this.btnSubmission.CssClass.Replace("disabled", "");

                    this.btnSubmissionCancel.Enabled = true;
                    this.btnSubmitCancelButtom.Enabled = true;
                    this.btnSubmissionCancel.CssClass = this.btnSubmissionCancel.CssClass.Replace("disabled", "");
                    this.btnSubmitCancelButtom.CssClass = this.btnSubmitCancelButtom.CssClass.Replace("disabled", "");
                    this.nameButtonApproval = "提出済";
                }

                result = true;
            }
            else
            {
                this.ShowSubmitCance = false;
                result = false;
            }

            return result;
        }

        #endregion
    }
}