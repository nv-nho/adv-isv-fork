using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OMS.DAC;
using OMS.Models;
using System.Data;
using OMS.Utilities;
using OMS.Controls;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Data.SqlClient;

using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Net.Imap;
using MailKit.Security;
using MailKit;
using System.Text;
using System.Xml;
using OMS.Properties;

namespace OMS.Attendance
{
    public partial class FrmAttendanceEntry : FrmBaseDetail
    {
        //Config Value
        private const string URL_LIST = "~/Attendance/FrmAttendanceList.aspx";
        private const string URL_APPROVAL_LIST = "~/Approval/FrmApprovalList.aspx";

        private const string FRM_APPROVAL = "FrmApprovalList";
        private const string FRM_APPROVAL_VIEW = "FrmApprovalList_View";
        private const string FRM_ATTENDENCE_REQUEST = "FrmAttendanceList_Request";

        private const int ENTRY_TIME_MAX_LENGTH = 5;
        private const int LEAVE_TIME_MAX_LENGTH = 5;
        private const string HOURS_EMPTY = "00:00"; 

        #region Property

        /// <summary>
        /// Get or set TAttendanceId
        /// </summary>
        public int TAttendanceId
        {
            get { return (int)ViewState["TAttendanceId"]; }
            set { ViewState["TAttendanceId"] = value; }
        }

        /// <summary>
        /// Get or set Date
        /// </summary>
        public string dateViewState
        {
            get { return (string)ViewState["dateViewState"]; }
            set { ViewState["dateViewState"] = value; }
        }

        /// <summary>
        /// Get or set Date
        /// </summary>
        public string UID
        {
            get { return (string)ViewState["UID"]; }
            set { ViewState["UID"] = value; }
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
        /// Get or set OldUpdateDate
        /// </summary>
        public T_Attendance OldT_Attendance
        {
            get { return (T_Attendance)ViewState["OldT_Attendance"]; }
            set { ViewState["OldT_Attendance"] = value; }
        }
        

        /// <summary>
        /// Get or set PreviousPage
        /// </summary>
        public string PreviousPage
        {
            get { return ViewState["PreviousPage"].ToString(); }
            set { ViewState["PreviousPage"] = value; }
        }

        /// <summary>
        /// Get or set AttendanceApprovalStatus
        /// </summary>
        public AttendanceApprovalStatus AttendanceApprovalStatus
        {
            get { return base.GetValueViewState<AttendanceApprovalStatus>("AttendanceApprovalStatus"); }
            set { ViewState["AttendanceApprovalStatus"] = value; }
        }

        /// <summary>
        /// MAIL_ID
        /// </summary>
        private string MAIL_ID
        {
            get { return GetConfig.getStringValue("MAIL_ID"); }
        }

        /// <summary>
        /// MAIL_NAME
        /// </summary>
        private string MAIL_NAME
        {
            get { return this.GetNameConfig(); }
        }

        /// <summary>
        /// MAIL_PASSWORD
        /// </summary>
        private string MAIL_PASSWORD
        {
            get { return GetConfig.getStringValue("MAIL_PASSWORD"); }
        }

        /// <summary>
        /// HOST_SMTP
        /// </summary>
        private string HOST_SMTP
        {
            get { return GetConfig.getStringValue("HOST_SMTP"); }
        }

        /// <summary>
        /// PORT_SMTP
        /// </summary>
        private int PORT_SMTP
        {
            get { return GetConfig.getIntValue("PORT_SMTP"); }
        }

        /// <summary>
        /// SSL
        /// </summary>
        private bool PROTOCOL_SSL
        {
            get { return GetConfig.getBoolValue("SSL"); }
        }

        #endregion

        #region Variable

        /// <summary>
        /// Index Sell
        /// </summary>
        private int _indexSell;

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
            base.FormTitle = "勤務表";
            base.FormSubTitle = "Detail";

            //Init Max Length
            this.txtEntryTime.MaxLength = ENTRY_TIME_MAX_LENGTH;
            this.txtExitTime.MaxLength = LEAVE_TIME_MAX_LENGTH;

            //Init Event
            LinkButton btnYes = (LinkButton)this.Master.FindControl("btnYes");
            btnYes.Click += new EventHandler(btnProcessData);

            LinkButton btnNo = (LinkButton)this.Master.FindControl("btnNo");
            btnNo.Click += new EventHandler(btnShowData);
        }

        /// <summary>
        /// Page Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Check authority of login user
            base.SetAuthority(FormId.Attendance);
            if (!this._authority.IsAttendanceView)
            {
                Response.Redirect("~/Menu/FrmMainMenu.aspx");
            }

            if (!this.IsPostBack)
            {
                if (base.PreviousPage != null)
                {
                    GetDefaultValue();

                    //Save condition of previous page
                    base.ViewState["Condition"] = base.PreviousPageViewState["Condition"];


                    if (this.PreviousPageViewState["ConditionApproval"] != null)
                    {
                        this.ViewState["ConditionApproval"] = this.PreviousPageViewState["ConditionApproval"];
                    }

                    string[] PreviousPageViewState = base.PreviousPageViewState["ID"].ToString().Split(',');
                    
                    this.PreviousPage = PreviousPageViewState[5].ToString();
                    this.InitComboboxExchangeDate(int.Parse(PreviousPageViewState[1]), DateTime.Parse(PreviousPageViewState[2]));
                    
                    //Check mode
                    if (PreviousPageViewState[0] == "-1")
                    {
                        this.UID = PreviousPageViewState[1];

                        //set viewstate 
                        this.dateViewState = PreviousPageViewState[2];

                        bool checkStatusFlagSubmit = checkAttendanceStatusFlag(int.Parse(PreviousPageViewState[1]), DateTime.Parse(PreviousPageViewState[3]), DateTime.Parse(PreviousPageViewState[4]));
                        //Set mode
                        this.ProcessMode(Mode.Insert, (int)AttendanceStatusFlag.NotSubmitted, checkStatusFlagSubmit);
                        
                        //Uid, Date 
                        GetDefaultInsert(PreviousPageViewState[1], PreviousPageViewState[2]);

                        //Init detail list
                        this.InitDetailList();

                        //set DateHidden
                        this.InitDateHidden.Value = PreviousPageViewState[2];
                    }
                    else
                    {
                        this.UID = PreviousPageViewState[1];

                        //set viewstate 
                        this.dateViewState = PreviousPageViewState[2];

                        //Get T_Work_H data by ID
                        this.TAttendanceId = int.Parse(PreviousPageViewState[0]);

                        T_Attendance t_Attendance = this.GetTAttendanceById(TAttendanceId, new DB());

                        if (t_Attendance != null)
                        {
                            if (this.PreviousPage == FRM_APPROVAL)
                            {
                                this.Mode = Mode.Approval;
                            }
                            else if (this.PreviousPage == FRM_ATTENDENCE_REQUEST)
                            {
                                this.Mode = Mode.Request;
                            }
                            else
                            {
                                this.Mode = Mode.View;
                            }

                            //Show data
                            this.ShowHeaderData(t_Attendance);

                            //Set Mode
                            //Set Mode
                            if (this.PreviousPage == FRM_APPROVAL)
                            {
                                this.ProcessMode(Mode.Approval, (int)t_Attendance.StatusFlag, false, (AttendanceApprovalStatus)t_Attendance.ApprovalStatus);
                            }
                            else if (this.PreviousPage == FRM_ATTENDENCE_REQUEST)
                            {
                                this.ProcessMode(Mode.Request, (int)t_Attendance.StatusFlag, false, (AttendanceApprovalStatus)t_Attendance.ApprovalStatus);
                            }
                            else
                            {
                                this.ProcessMode(Mode.View, (int)t_Attendance.StatusFlag, false, (AttendanceApprovalStatus)t_Attendance.ApprovalStatus);

                            }
                        }
                        else
                        {
                            if (this.PreviousPage == FRM_APPROVAL || this.PreviousPage == FRM_APPROVAL_VIEW)
                            {
                                Server.Transfer(URL_APPROVAL_LIST);
                            }
                            else
                            {
                                Server.Transfer(URL_LIST);
                            }
                        }


                        //set DateHidden
                        this.InitDateHidden.Value = PreviousPageViewState[2];
                    }
                }
                else
                {
                    Server.Transfer(URL_LIST);
                }
            }

            //Set init
            this.Success = false;
        }

        /// <summary>
        /// Edit Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            //Get Config data by ID
            T_Attendance tAttendance = this.GetHeaderData(this.TAttendanceId);

            //Check Config exists
            if (tAttendance != null)
            {
                this.Mode = Mode.Update;

                //Show data
                this.ShowHeaderData(tAttendance);

                //Set Mode
                this.ProcessMode(Mode.Update, (int)tAttendance.StatusFlag);
            }
            else
            {
                Server.Transfer(URL_LIST);
            }
        }

        /// <summary>
        /// Event Button AddRow
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnAddRow_Click(object sender, EventArgs e)
        {
            //Get new list from screen
            var listDetail = this.GetDetailList(false);
            if (listDetail != null)
            {
                listDetail.Add(new T_Work_D());
            }
            
            //Process list view
            this.rptDetail.DataSource = listDetail;
            this.rptDetail.DataBind();

            // Lock control overtime. 
            foreach (RepeaterItem item in this.rptDetail.Items)
            {

                ICodeTextBox txtProjectCD = (ICodeTextBox)item.FindControl("txtProjectCD");
                if (item.ItemIndex == rptDetail.Items.Count -1 )
                {
                    SetFocus(txtProjectCD);
                    txtProjectCD.Focus();
                }
            }
        }

        /// <summary>
        /// Event Button RemoveRow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRemoveRow_Click(object sender, EventArgs e)
        {
            //Get new list from screen
            var listDetail = this.GetDetailList(false);

            //Get list index remove item
            List<int> listDel = new List<int>();
            for (int i = listDetail.Count - 1; i >= 0; i--)
            {
                if (listDetail[i].DelFlag)
                {
                    listDel.Add(i);
                }

                listDetail[i].DelFlag = false;
            }

            //Remove row
            foreach (var item in listDel)
            {
                listDetail.RemoveAt(item);
            }

            if (listDetail.Count == 0)
            {
                listDetail.Add(new T_Work_D());
            }

            //Process list view
            this.rptDetail.DataSource = listDetail;
            this.rptDetail.DataBind();
        }

        /// <summary>
        ///btnApproval_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnApproval_Click(object sender, CommandEventArgs e)
        {
            var aprovalNotes = this.txtApprovalNote.Value;
            if (this.OldT_Attendance != null)
            {
                //Show data
                this.ShowHeaderData(this.OldT_Attendance, false);
            }
            this.txtApprovalNote.Value = aprovalNotes;

            //Show question update
            base.ShowQuestionMessage(M_Message.MSG_QUESTION_ACCEPT_APPROVAL, Models.DefaultButton.Yes);
        }

        /// <summary>
        /// Event btnRelease_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, CommandEventArgs e)
        {
            var aprovalNotes = this.txtApprovalNote.Value;
            if (this.OldT_Attendance != null)
            {
                //Show data
                this.ShowHeaderData(this.OldT_Attendance, false);
            }
            this.txtApprovalNote.Value = aprovalNotes;
            this.Mode = Mode.Cancel;

            //Show question update
            base.ShowQuestionMessage(M_Message.MSG_QUESTION_CANCEL_APPROVAL, Models.DefaultButton.No, true);
        }

        /// <summary>
        /// Request Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRequest_Click(object sender, EventArgs e)
        {
            var requestNotes = this.txtRequestNote.Value;
            if (this.OldT_Attendance != null)
            {
                //Show data
                this.ShowHeaderData(this.OldT_Attendance, false);
            }
            this.txtRequestNote.Value = requestNotes;

            //Check input
            if (!this.CheckInputRequest())
            {
                return;
            }

            //Show question Request
            base.ShowQuestionMessage(M_Message.MSG_QUESTION_ACCEPT_REQUEST, Models.DefaultButton.Yes);
        }

        /// <summary>
        /// Process Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnProcessData(object sender, EventArgs e)
        {
            bool ret;
            T_Attendance tAttendance = null;

            //Check Mode
            switch (this.Mode)
            {
                case Utilities.Mode.Insert:
                    
                    //Insert Data
                    int newID = 0;
                    ret = this.InsertData(ref newID);
                    if (ret)
                    {
                        this.TAttendanceId = newID;
                        using (DB db = new DB())
                        {
                            tAttendance = this.GetTAttendanceById(newID, db);
                        }
                        if ((AttendanceApprovalStatus)tAttendance.ApprovalStatus == AttendanceApprovalStatus.NeedApproval)
                        {
                            //Show data
                            this.ShowHeaderData(tAttendance);

                            //Set Mode
                            this.ProcessMode(Mode.Request, (int)tAttendance.StatusFlag);

                            //Set Success
                            this.Success = true;

                            return;
                        }
                        else
                        {
                            Server.Transfer(URL_LIST);
                        }
                    }
                    break;
                case Utilities.Mode.Delete:

                    //Delete data
                    ret = this.DeleteData();
                    if (!ret)
                    {
                        //Set Mode
                        this.ProcessMode(Mode.View, (int)AttendanceStatusFlag.NotSubmitted);
                    }
                    else
                    {
                        Server.Transfer(URL_LIST);
                    }
                    break;
                case Utilities.Mode.Approval:
                    //Approval Data
                    ret = this.ApprovalData();

                    if (ret)
                    {
                        Server.Transfer(URL_APPROVAL_LIST);

                    }
                    else
                    {
                        var aprovalNotes = this.txtApprovalNote.Value;
                        if (this.OldT_Attendance != null)
                        {
                            //Show data
                            this.ShowHeaderData(this.OldT_Attendance, false);
                        }
                        this.txtApprovalNote.Value = aprovalNotes;
                    }
                    break;
                case Utilities.Mode.Cancel:
                    //Approval Data
                    ret = this.CancelApprovalData();

                    if (ret)
                    {
                        Server.Transfer(URL_APPROVAL_LIST);

                    }
                    else
                    {
                        this.Mode = Mode.Approval;
                        var aprovalNotes = this.txtApprovalNote.Value;
                        if (this.OldT_Attendance != null)
                        {
                            //Show data
                            this.ShowHeaderData(this.OldT_Attendance, false);
                        }
                        this.txtApprovalNote.Value = aprovalNotes;
                    }
                    break;
                case Utilities.Mode.Request:
                    //Approval Data
                    ret = this.RequestApprovalData();

                    if (ret)
                    {
                        Server.Transfer(URL_LIST);
                    }
                    else
                    {
                        var requestNotes = this.txtRequestNote.Value;
                        if (this.OldT_Attendance != null)
                        {
                            //Show data
                            this.ShowHeaderData(this.OldT_Attendance, false);
                        }
                        this.txtRequestNote.Value = requestNotes;
                    }
                    break;
                default:

                    //Update Data
                    ret = this.UpdateData();
                    if (ret)
                    {
                        using (DB db = new DB())
                        {
                            tAttendance = this.GetTAttendanceById(TAttendanceId, db);
                        }
                        if ((AttendanceApprovalStatus)tAttendance.ApprovalStatus == AttendanceApprovalStatus.NeedApproval)
                        {
                            //Show data
                            this.ShowHeaderData(tAttendance);

                            //Set Mode
                            this.ProcessMode(Mode.Request, (int)tAttendance.StatusFlag);

                            //Set Success
                            this.Success = true;

                            return;
                        }
                        else
                        {
                            Server.Transfer(URL_LIST);
                        }

                    }

                    break;
            }

            if (ret)
            {
                //Show data
                this.ShowHeaderData(tAttendance);

                //Set Mode
                this.ProcessMode(Mode.View, (int)tAttendance.StatusFlag);

                //Set Success
                this.Success = true;
            }
        }

        /// <summary>
        /// Show Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShowData(object sender, EventArgs e)
        {
            if (this.Mode == Mode.Cancel)
            {
                this.Mode = Mode.Approval;
                return;
            }

            //Get T_Attendance Header
            T_Attendance header = this.GetHeaderData(this.TAttendanceId);
            if (header != null)
            {
                //Show data
                this.ShowHeaderData(header);

                //Set Mode
                this.ProcessMode(Mode.View, (int)header.StatusFlag);
            }
            else
            {
                Server.Transfer(URL_LIST);
            }
        }

        /// <summary>
        /// Up Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUp_Click(object sender, EventArgs e)
        {
            //Get Data
            this.SwapDetail(true);
        }

        /// <summary>
        /// Down Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDown_Click(object sender, EventArgs e)
        {
            //Get Data
            this.SwapDetail(false);
        }

        /// <summary>
        /// Event Button Insert Submit
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

            IList<T_Work_D> details = this.GetDetailList(true);

            if (details.Count > 0)
            {
                this.rptDetail.DataSource = SortDetail(details);
                this.rptDetail.DataBind();
            }
            else
            {
                //Init detail list
                this.InitDetailList();
            }

            //set value WorkingHours
            setValueWorkingHour();

            //Show question insert
            base.ShowQuestionMessage(M_Message.MSG_QUESTION_INSERT, Models.DefaultButton.Yes);
        }

        /// <summary>
        /// Event Button Update Submit
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

            IList<T_Work_D> details = this.GetDetailList(true);

            if (details.Count > 0)
            {
                this.rptDetail.DataSource = SortDetail(details);
                this.rptDetail.DataBind();
            }
            else
            {
                //Init detail list
                this.InitDetailList();
            }

            //set value WorkingHours
            setValueWorkingHour();

            if (AttendanceApprovalStatus.None == this.AttendanceApprovalStatus || AttendanceApprovalStatus.NeedApproval == this.AttendanceApprovalStatus)
            {
                //Show question update
                base.ShowQuestionMessage(M_Message.MSG_QUESTION_UPDATE, Models.DefaultButton.Yes);
            }
            else
            {
                //Show question update
                string[] param = { this.AttendanceApprovalStatus.GetDescription(), this.Mode.GetDescription() };

                base.ShowQuestionMessage(M_Message.MSG_CONFIRM_ATTENDENCE_UPDATE, Models.DefaultButton.No, false, param);
            }
        }

        /// <summary>
        /// SortDetail 
        /// </summary>
        /// <param name="details"></param>
        /// <returns></returns>
        protected IList<T_Work_D> SortDetail(IList<T_Work_D> details)
        {
            T_Work_D tWorkD = new T_Work_D();
            IList<T_Work_D> results = new List<T_Work_D>();
            if (details.Count <= 1)
            {
                results = details;
            }
            else
            {
                while (details.Count != 0)
                {
                    tWorkD = details[0];
                    for (int i = 1; i < details.Count; i++)
                    {
                        if ((tWorkD.StartTime > details[i].StartTime) || ((tWorkD.StartTime == details[i].StartTime) && tWorkD.EndTime > details[i].EndTime))
                        {
                            tWorkD = details[i];
                        }
                    }
                    results.Add(tWorkD);
                    details.Remove(tWorkD);
                }
            }

            return results;
        }

        /// <summary>
        /// Delete Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            //Set Model
            this.Mode = Mode.Delete;

            if (this.OldT_Attendance != null)
            {
                //Show data
                this.ShowHeaderData(this.OldT_Attendance, false);
            }
            if (AttendanceApprovalStatus.None == this.AttendanceApprovalStatus || AttendanceApprovalStatus.NeedApproval == this.AttendanceApprovalStatus)
            {
                //Show question insert
                base.ShowQuestionMessage(M_Message.MSG_QUESTION_DELETE, Models.DefaultButton.No, true);
            }
            else
            {
                //Show question update
                string[] param = { this.AttendanceApprovalStatus.GetDescription(), this.Mode.GetDescription() };

                base.ShowQuestionMessage(M_Message.MSG_CONFIRM_ATTENDENCE_UPDATE, Models.DefaultButton.No, true, param);
            }
        }

        /// <summary>
        /// Copy from prev date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCopyFromDate_Click(object sender, EventArgs e)
        {
            //Check input
            if (this.dtCopyDate.Value == null)
            {
                this.SetMessage(this.dtCopyDate.ID, M_Message.MSG_REQUIRE, "コピー日");
                return;
            }

            string strDate = dtCopyDate.Text;
            T_Attendance tAttendance = new T_Attendance();
            using(DB db = new DB())
            {
                AttendanceService AttendanceSer = new AttendanceService(db);
                tAttendance = AttendanceSer.GetAttendanceByUidAndDate(int.Parse(this.UID), strDate);
                if (tAttendance != null)
                {
                    //Show detail Data
                    this.ShowDetailData(tAttendance.ID);
                }
                else
                {
                    //Init detail list
                    this.InitDetailList();   
                }
            }
        }

        /// <summary>
        /// Event Button Back
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (this.Mode == Mode.View || this.Mode == Mode.Insert)
            {
  
                Server.Transfer(URL_LIST);
            }

            if (this.Mode == Mode.Update || this.Mode == Mode.Insert)
            {
                //Get tAttendance by ID
                T_Attendance tAttendance = this.GetHeaderData(this.TAttendanceId);

                //Check tAttendance exists
                if (tAttendance != null)
                {
                    this.Mode = Mode.View;

                    //Show data
                    this.ShowHeaderData(tAttendance);

                    //Set Mode
                    this.ProcessMode(Mode.View, (int)tAttendance.StatusFlag);
                }
                else
                {
                    Server.Transfer(URL_LIST);
                }
            }
        }

        /// <summary>
        /// Event btnBackNew Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBackNew_Click(object sender, EventArgs e)
        {
            T_Attendance tAttendance = new T_Attendance();
            using (DB db = new DB())
            {
                AttendanceService AttendanceSer = new AttendanceService(db);
                tAttendance = AttendanceSer.GetAttendanceByUidAndDate(int.Parse(this.UID), this.dateViewState);
            }

            //Check tAttendance exists
            if (tAttendance != null)
            {
                this.Mode = Mode.View;

                //Show data
                this.ShowHeaderData(tAttendance);

                //Set Mode
                this.ProcessMode(Mode.View, (int)tAttendance.StatusFlag);
            }
            else
            {
                Server.Transfer(URL_LIST);
            }
        }

        /// <summary>
        /// Event btnBackView Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBackApproval_Click(object sender, EventArgs e)
        {
            if (this.PreviousPage == FRM_APPROVAL || this.PreviousPage == FRM_APPROVAL_VIEW)
            {
                Server.Transfer(URL_APPROVAL_LIST);
            }
            else
            {
                Server.Transfer(URL_LIST);
            }
        }
        #endregion

        #region Method
        /// <summary>
        /// Process mode
        /// </summary>
        /// <param name="mode">Mode</param>
        private void ProcessMode(Mode mode, int statusFlg, bool checkStatusFlagSubmit = false, AttendanceApprovalStatus attendanceApprovalStatus = 0)
        {
            bool enable;

            //Set mode
            this.Mode = mode;

            //Set Attendance Approval Status
            this.AttendanceApprovalStatus = attendanceApprovalStatus;

            //Check model
            switch (mode)
            {
                case Mode.Insert:
                    enable = false;
                    this.txtDate.ReadOnly = true;
                    this.txtEmployeeName.ReadOnly = true;
                   
                    // check UID
                    if (this.UID != this.LoginInfo.User.ID.ToString())
                    {
                        base.DisabledLink(this.btnInsert, !base._authority.IsAttendanceOtherUpdates || !base._authority.IsAttendanceNew || checkStatusFlagSubmit);
                    }
                    else
                    {
                        base.DisabledLink(this.btnInsert, !base._authority.IsAttendanceNew || checkStatusFlagSubmit);
                    }
                    break;

                case Mode.Update:
                    enable = false;
                    break;
                default:
                    enable = true;
                    bool isExchanged = false;
                    using (DB db = new DB())
                    {
                        AttendanceService attSer = new AttendanceService(db);
                        isExchanged = attSer.ExchangeDateIsUsed(int.Parse(this.UID), DateTime.Parse(this.dateViewState), DateTime.Parse(this.dateViewState));
                    }
                    if (this.UID != this.LoginInfo.User.ID.ToString())
                    {
                        base.DisabledLink(this.btnInsert, !base._authority.IsAttendanceOtherUpdates || !base._authority.IsAttendanceNew || statusFlg != (int)AttendanceStatusFlag.NotSubmitted || attendanceApprovalStatus == Utilities.AttendanceApprovalStatus.Approved);
                        base.DisabledLink(this.btnEdit, !base._authority.IsAttendanceOtherUpdates || !base._authority.IsAttendanceEdit || statusFlg != (int)AttendanceStatusFlag.NotSubmitted || attendanceApprovalStatus == Utilities.AttendanceApprovalStatus.Approved);
                        base.DisabledLink(this.btnDelete, !base._authority.IsAttendanceOtherUpdates || !base._authority.IsAttendanceDelete || statusFlg != (int)AttendanceStatusFlag.NotSubmitted || attendanceApprovalStatus == Utilities.AttendanceApprovalStatus.Approved || isExchanged);
                    }
                    else
                    {
                        base.DisabledLink(this.btnEdit, !base._authority.IsAttendanceEdit || statusFlg != (int)AttendanceStatusFlag.NotSubmitted || attendanceApprovalStatus == Utilities.AttendanceApprovalStatus.Approved);
                        base.DisabledLink(this.btnDelete, !base._authority.IsAttendanceDelete || statusFlg != (int)AttendanceStatusFlag.NotSubmitted || attendanceApprovalStatus == Utilities.AttendanceApprovalStatus.Approved || isExchanged);
                    }
                    var user = this.GetUser(int.Parse(this.UID));
                    //申請承認
                    base.SetAuthority(FormId.Approval);
                    base.DisabledLink(this.btnApproval, !(base._authority.IsApproval && this.LoginInfo.User.DepartmentID == user.DepartmentID) && !base._authority.IsApprovalALL);
                    base.DisabledLink(this.btnCancel, !(base._authority.IsApproval && this.LoginInfo.User.DepartmentID == user.DepartmentID) && !base._authority.IsApprovalALL);
                    base.SetAuthority(FormId.Attendance);

                    if (attendanceApprovalStatus == AttendanceApprovalStatus.Approved)
                    {
                        base.DisabledLink(this.btnApproval, true);
                    }
                    else if (attendanceApprovalStatus == AttendanceApprovalStatus.Cancel)
                    {
                        base.DisabledLink(this.btnCancel, true);
                    }
                    break;
            }

            //Lock control
            this.txtWorkingHours.ReadOnly = enable;
            this.txtLate.ReadOnly = enable;
            this.txtEarlyHours.ReadOnly = enable;
            this.txtCertainHoliday.ReadOnly = enable;
            this.txtLegalHoliday.ReadOnly = enable;

            // Lock control overtime. 
            foreach (RepeaterItem item in this.rpttbConfig4_Data.Items)
            {

                ITimeTextBox txtOverTime = (ITimeTextBox)item.FindControl("txtOverTime");
                if (txtOverTime != null)
                {
                    txtOverTime.ReadOnly = enable;
                }
            }
        }

        /// <summary>
        /// Init Detail List
        /// </summary>
        private void InitDetailList()
        {
            //Add data
            IList<T_Work_D> listDetail = new List<T_Work_D>();
            listDetail.Add(new T_Work_D ());

            //Process list view
            this.rptDetail.DataSource = listDetail;
            this.rptDetail.DataBind();
        }

        /// <summary>
        /// Get Header Data
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        private bool GetIsUseApply(string configCD)
        {
            using (DB db = new DB())
            {
                Config_HService dbSer = new Config_HService(db);

                //Get Config header
                return dbSer.GetDefaultValueDrop(configCD) != "0";
            }
        }

        /// <summary>
        /// Get Header Data
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        private T_Attendance GetHeaderData(int id)
        {
            using (DB db = new DB())
            {
                AttendanceService dbSer = new AttendanceService(db);

                //Get Config header
                return dbSer.GetDataAttendanceById(id);
            }
        }

        /// <summary>
        /// Get Header Data
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        private M_User GetUser(int uid)
        {
            using (DB db = new DB())
            {
                UserService dbSer = new UserService(db);

                //Get Config header
                return dbSer.GetByID(uid);
            }
        }

        /// <summary>
        /// Calculate vacation days 
        /// </summary>
        /// <param name="oldData">old attendance data</param>
        /// <param name="newData">new attendance data</param>
        /// <returns></returns>
        private decimal CalcUseVacationDays(T_Attendance oldData, T_Attendance newData)
        {
            decimal ret = 0;

            if (oldData != null)
            {
                if (oldData.VacationFullCD == (int)Vacation.AnnualPaid)
                {
                    ret -= 1;
                }

                if (oldData.VacationMorningCD == (int)Vacation.AnnualPaid)
                {
                    ret -= new decimal(0.4);
                }

                if (oldData.VacationAfternoonCD == (int)Vacation.AnnualPaid)
                {
                    ret -= new decimal(0.6);
                }
            }

            if (newData != null)
            {
                if (newData.VacationFullCD == (int)Vacation.AnnualPaid)
                {
                    ret += 1;
                }

                if (newData.VacationMorningCD == (int)Vacation.AnnualPaid)
                {
                    ret += new decimal(0.4);
                }

                if (newData.VacationAfternoonCD == (int)Vacation.AnnualPaid)
                {
                    ret += new decimal(0.6);
                }
            }

            return ret;
        }

        /// <summary>
        /// Get Default Value
        /// </summary>
        private void GetDefaultValue()
        {
            //InitCombobox 

            InitCombobox(this.cmbAllHolidays);
            InitCombobox(this.cmbBeforeHalfHoliday);
            InitCombobox(this.cmbLateHoliday);
            InitComboboxWorkSchedule(this.cmbWorkSchedule);

            DataTable dt = new DataTable();
            dt = InitDataTableConfig4();
            using (DB db = new DB())
            {
                Config_DService dbSer = new Config_DService(db);

                //Get list detail
                IList<M_Config_D> listM_Config_D = dbSer.GetListByConfigCd(M_Config_H.CONFIG_CD_OVER_TIME_TYPE);

                int index = 0; 
                foreach (var item in listM_Config_D)
                {
                    DataRow drow = dt.NewRow();
                    dt.Rows.Add(drow);
                    string header = "<th class ='text-center'>"+item.Value2 +"</th>";
                    string data = string.Empty;
                    dt.Rows[index].SetField("Header", header);
                    dt.Rows[index].SetField("Data", data);
                    index++;
                }

                rpttbConfig4_Header.DataSource = dt;
                rpttbConfig4_Header.DataBind();
                rpttbConfig4_Data.DataSource = dt;
                rpttbConfig4_Data.DataBind();

            }
        }

        /// <summary>
        /// Get Default Value
        /// </summary>
        private void GetDefaultInsert(string UserID, string date)
        {
            string[] arrDayOfWeek = { "日", "月", "火", "水", "木", "金", "土" };
            using (DB db = new DB())
            {
                UserService userSer = new UserService(db);
                M_User user = new M_User();
                user = userSer.GetByID(int.Parse(UserID));
                txtEmployeeName.Text = user.UserName1;

                //set VacationDay
                T_PaidVacationService paidVacationSer = new T_PaidVacationService(db);
                txtVacationDay.Text = paidVacationSer.GetTotalVacationDays(int.Parse(this.UID)).ToString("#,##0.0");

                //set Verification
                AttendanceService atdSer = new AttendanceService(db);
                var dataVacation = atdSer.GetListVacation(int.Parse(this.UID));

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
            DateTime dt = DateTime.Parse(date);
            DateTime initialDate = new DateTime();
            txtDate.Text = dt.ToString(Constants.FMT_YMD_JP) + " (" + arrDayOfWeek[(int)dt.DayOfWeek] + ")";
            this.dtCopyDate.Value = dt.AddDays(-1);

            // check holiday 
            int workingSystemId = 0;
            string workingSystemCd = string.Empty;
            string nomalWorkingId = string.Empty;
            string holidayId = string.Empty;
            string legalHoliday = string.Empty;
            int checkDate = -1;
            bool CheckcmbWorkSchedule = true;
            bool paidLeave = false;
            IList<M_Config_D> lstMConfigD;
            M_WorkingSystem mWorkingSystem = new M_WorkingSystem();
            T_WorkingCalendar_H tWorkingCalendarH = new T_WorkingCalendar_H();  
            IList<T_WorkingCalendar_D> lstTWorkingCalendarD;

            int WCID = 0;

            using (DB db = new DB())
            {
                T_WorkingCalendar_HService tWorkingCalendarHSer = new T_WorkingCalendar_HService(db);
                WCID = tWorkingCalendarHSer.GetIDByUserAndDate(int.Parse(UserID), dt);
                if (WCID != 0)
                {
                    tWorkingCalendarH = tWorkingCalendarHSer.GetByID(WCID);
                    initialDate = tWorkingCalendarH.InitialDate;
                }
            }
            DateTime fromDate = new DateTime();
            DateTime toDate = new DateTime();

            for (int i = 0; i < 12; i++)
            {
                fromDate = initialDate.AddMonths(i);
                toDate = initialDate.AddMonths(i + 1).AddDays(-1);
                if(fromDate <= dt && dt <= toDate)
                {
                    checkDate = i+1;
                    break;
                }
                // set from day
                fromDate = fromDate.AddMonths(1);
            }

            switch (checkDate)
            {
                case 1:
                    if (tWorkingCalendarH.AgreementFlag1 == 1)
                    {
                        CheckcmbWorkSchedule = false;
                    }
                    break;
                case 2:
                    if (tWorkingCalendarH.AgreementFlag2 == 1)
                    {
                        CheckcmbWorkSchedule = false;
                    }
                    break;
                case 3:
                    if (tWorkingCalendarH.AgreementFlag3 == 1)
                    {
                        CheckcmbWorkSchedule = false;
                    }
                    break;
                case 4:
                    if (tWorkingCalendarH.AgreementFlag4 == 1)
                    {
                        CheckcmbWorkSchedule = false;
                    }
                    break;
                case 5:
                    if (tWorkingCalendarH.AgreementFlag5 == 1)
                    {
                        CheckcmbWorkSchedule = false;
                    }
                    break;
                case 6:
                    if (tWorkingCalendarH.AgreementFlag6 == 1)
                    {
                        CheckcmbWorkSchedule = false;
                    }
                    break;
                case 7:
                    if (tWorkingCalendarH.AgreementFlag7 == 1)
                    {
                        CheckcmbWorkSchedule = false;
                    }
                    break;
                case 8:
                    if (tWorkingCalendarH.AgreementFlag8 == 1)
                    {
                        CheckcmbWorkSchedule = false;
                    }
                    break;
                case 9:
                    if (tWorkingCalendarH.AgreementFlag9 == 1)
                    {
                        CheckcmbWorkSchedule = false;
                    }
                    break;
                case 10:
                    if (tWorkingCalendarH.AgreementFlag10 == 1)
                    {
                        CheckcmbWorkSchedule = false;
                    }
                    break;
                case 11:
                    if (tWorkingCalendarH.AgreementFlag11 == 1)
                    {
                        CheckcmbWorkSchedule = false;
                    }
                    break;
                case 12:
                    if (tWorkingCalendarH.AgreementFlag12 == 1)
                    {
                        CheckcmbWorkSchedule = false;
                    }
                    break;

            }

            cmbWorkSchedule.Enabled = CheckcmbWorkSchedule;

            using (DB db = new DB())
            {
                Config_DService configDService = new Config_DService(db);
                lstMConfigD = configDService.GetListByConfigCd(M_Config_H.CONFIG_CD_HOLIDAY_TYPE);
                T_WorkingCalendar_DService T_WorkingCalendar_DService = new T_WorkingCalendar_DService(db);
                lstTWorkingCalendarD = T_WorkingCalendar_DService.GetListWorkingCalendarByHId(WCID);

                foreach (var item in lstTWorkingCalendarD)
                {
                    if (item.WorkingDate == dt)
                    {
                        workingSystemId = item.WorkingSystemID;
                        
                    }
                }

                // get ID with workingtype;
                WorkingSystemService WorSystemService = new WorkingSystemService(db);
                mWorkingSystem = WorSystemService.GetDataWorkingSystemById(workingSystemId);

                cmbWorkSchedule.Items.FindByValue(workingSystemId.ToString()).Selected = true;
                T_PaidLeaveService tPaidLeaveSer = new T_PaidLeaveService(db);
                paidLeave = tPaidLeaveSer.IsExistsByKey(WCID, int.Parse(this.UID), dt);

                if (mWorkingSystem.WorkingSystemCD == "4" || paidLeave)
                {
                    cmbAllHolidays.Items.FindByValue(((int)Vacation.AnnualPaid).ToString()).Selected = true;
                }
            }

            //set WorkingHours
            TimeSpan workingHours = new TimeSpan(0,0,0);

            DateTime[] arrBreakTime = new DateTime[8];
            DateTime[] arrOverTime = new DateTime[10];

            if (mWorkingSystem.WorkingType == (int)WorkingType.WorkFullTime && mWorkingSystem.WorkingSystemCD != "4" && !paidLeave)
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
                    workingHours = CalDurationWorkTime(workST, workET, arrBreakTime);
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
                    workingHours = CalDurationWorkTime(workST, workET, arrBreakTime).Subtract(BreakTimeHour);

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
                        workingHours = eTime.Subtract(sTime);
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
                    workingHours = CalDurationWorkTime(workST, workET, arrBreakTime_Type2);
                }

                txtWorkingHours.Text = FormatTimeResult(workingHours);
                string strWorkingStart = string.Empty;
                string strWorkingEnd = string.Empty;
                if (mWorkingSystem.Working_Start != null)
                {
                    strWorkingStart = Utilities.CommonUtil.IntToTime((int)mWorkingSystem.Working_Start, false);
                    strWorkingEnd = Utilities.CommonUtil.IntToTime((int)mWorkingSystem.Working_End, false);
                }

                if (strWorkingStart != string.Empty)
                {
                    txtEntryTime.Text = strWorkingStart;
                }
                if (strWorkingEnd != string.Empty)
                {
                    txtExitTime.Text = strWorkingEnd;
                }

                if (mWorkingSystem.WorkingSystemCD == "4")
                {
                    txtEntryTime.Text = string.Empty;
                    txtExitTime.Text = string.Empty;
                }
                //set workingHours
                txtTheTotalWorkingHours.Text = FormatTimeResult(workingHours);
                
            }
            else if (!paidLeave)
            {
                txtDate.ForeColor = System.Drawing.Color.Red;
            }
        }

        /// <summary>
        /// Init Combobox
        /// </summary>
        private void InitCombobox(DropDownList ddl)
        {
            using (DB db = new DB())
            {
                Config_DService dbSer = new Config_DService(db);

                //Get list detail
                IList<M_Config_D> listM_Config_D = dbSer.GetListByConfigCdWithVisibleSetting(M_Config_H.CONFIG_CD_VACATION_TYPE);
                if (listM_Config_D != null)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Value", typeof(string));
                    dt.Columns.Add("DisplayName", typeof(string));
                    DataRow drow = dt.NewRow();
                    dt.Rows.Add(drow);
                    dt.Rows[0].SetField("Value", -1);
                    dt.Rows[0].SetField("DisplayName", "-----------");
                    int index = 1;
                    foreach (var item in listM_Config_D)
                    {
                        DataRow drowItem = dt.NewRow();
                        dt.Rows.Add(drowItem);

                        dt.Rows[index].SetField("Value", item.Value1);
                        dt.Rows[index].SetField("DisplayName", item.Value2);
                        index = index + 1;
                    }
                    ddl.DataSource = dt;
                    ddl.DataValueField = "Value";
                    ddl.DataTextField = "DisplayName";
                    ddl.DataBind();
                }
            }
        }

        /// <summary>
        /// Init Combobox WorkSchedule
        /// </summary>
        private void InitComboboxWorkSchedule(DropDownList ddl)
        {
            using (DB db = new DB())
            {
                WorkingSystemService dbSer = new WorkingSystemService(db);

                //Get list detail
                IList<M_WorkingSystem> listM_WorkingSystem = dbSer.GetAll();
                if (listM_WorkingSystem != null)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Value", typeof(string));
                    dt.Columns.Add("DisplayName", typeof(string));
                    int index = 0;
                    foreach (var item in listM_WorkingSystem)
                    {
                        DataRow drow = dt.NewRow();
                        dt.Rows.Add(drow);

                        dt.Rows[index].SetField("Value", item.ID);
                        dt.Rows[index].SetField("DisplayName", item.WorkingSystemName);
                        index = index + 1;
                    }
                    ddl.DataSource = dt;
                    ddl.DataValueField = "Value";
                    ddl.DataTextField = "DisplayName";
                    ddl.DataBind();
                }
            }
        }

        /// <summary>
        /// Init Combobox ExchangeDate
        /// </summary>
        private void InitComboboxExchangeDate(int uid, DateTime date)
        {
            using (DB db = new DB())
            {
                AttendanceService dbSer = new AttendanceService(db);

                //Get list detail
                IList<DropDownModel> cbmSrc = dbSer.GetCmbListExchangeDate(uid, date);
                DropDownModel emptyItem = new DropDownModel("-1", "-----------");
                cbmSrc.Insert(0, emptyItem);

                cmbExchangeDate.DataSource = cbmSrc;
                cmbExchangeDate.DataValueField = "Value";
                cmbExchangeDate.DataTextField = "DisplayName";
                cmbExchangeDate.DataBind();
            }
        }
        
        
        /// <summary>
        /// InitDataTableConfig4
        /// </summary>
        private DataTable InitDataTableConfig4()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Header", typeof(string));
            dt.Columns.Add("Data", typeof(string));
            dt.Columns.Add("Value", typeof(string));
            return dt;
        }

        /// <summary>
        /// Repeater Detail Item Data Bound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptDetail_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //Set max length
                var txtProjectCDSearch = (ICodeTextBox)e.Item.FindControl("txtProjectCD");
                if (txtProjectCDSearch.Value == "0")
                {
                    txtProjectCDSearch.Text = string.Empty;
                }
                txtProjectCDSearch.MaxLength = M_Project.PROJECT_CODE_DB_MAX_LENGTH;

                ITextBox txtProjectNameSearch = (ITextBox)e.Item.FindControl("txtProjectNm");
                txtProjectNameSearch.MaxLength = M_Project.PROJECT_NAME_MAX_LENGTH;

                //Search Project
                HtmlButton btnSearchSellProduct = (HtmlButton)e.Item.FindControl("btnSearchProject");
                string onclickSearchProject = "callSearchProject('" + txtProjectCDSearch.ClientID + "','" + txtProjectNameSearch.ClientID + "')";
                btnSearchSellProduct.Attributes.Add("onclick", onclickSearchProject);

                var txtWorkPlace = (ITextBox)e.Item.FindControl("txtWorkPlace");
                txtWorkPlace.MaxLength = M_Project.PROJECT_WORKPLACE_MAX_LENGTH;

                int x = e.Item.Controls.Count; 
            }
        }

        #endregion

        #region Web Methods

        /// <summary>
        /// Get header
        /// </summary>
        /// <returns></returns>
        private T_Attendance GetHeader()
        {
            T_Attendance header = new T_Attendance();
            header.UID = int.Parse(this.UID);
            header.Date = DateTime.Parse(this.dateViewState);

            int workingType;
            int WCID = 0;

            using (DB db = new DB())
            {
                T_WorkingCalendar_HService tWorkingCalendarHSer = new T_WorkingCalendar_HService(db);
                WCID = tWorkingCalendarHSer.GetIDByUserAndDate(header.UID, header.Date);

                WorkingSystemService wksysService = new WorkingSystemService(db);
                M_WorkingSystem mWorkingSystem = new M_WorkingSystem();
                mWorkingSystem = wksysService.GetDataWorkingSystemById(int.Parse(this.cmbWorkSchedule.SelectedValue));
                workingType = mWorkingSystem.WorkingType;
            }

            foreach (RepeaterItem item in this.rpttbConfig4_Data.Items)
            {

                ITimeTextBox txtOverTime = (ITimeTextBox)item.FindControl("txtOverTime");
                switch (item.ItemIndex)
                {
                    case 0:
                        if (workingType == (int)WorkingType.WorkFullTime)
                        {
                            if (txtOverTime.Text != string.Empty && txtOverTime.Text != HOURS_EMPTY)
                            {
                                header.OverTimeHours1 = Utilities.CommonUtil.TimeToInt(txtOverTime.Text);
                            }

                        }
                        else if (workingType == (int)WorkingType.WorkHoliDay)
                        {
                            if (txtOverTime.Text != string.Empty && txtOverTime.Text != HOURS_EMPTY)
                            {
                                header.SH_OverTimeHours1 = Utilities.CommonUtil.TimeToInt(txtOverTime.Text);
                            }
                        }
                        else
                        {
                            if (txtOverTime.Text != string.Empty && txtOverTime.Text != HOURS_EMPTY)
                            {
                                header.LH_OverTimeHours1 = Utilities.CommonUtil.TimeToInt(txtOverTime.Text);
                            }
                        }
                        break;
                    case 1:
                        if (workingType == (int)WorkingType.WorkFullTime)
                        {
                            if (txtOverTime.Text != string.Empty && txtOverTime.Text != HOURS_EMPTY)
                            {
                                header.OverTimeHours2 = Utilities.CommonUtil.TimeToInt(txtOverTime.Text);
                            }
                        }
                        else if (workingType == (int)WorkingType.WorkHoliDay)
                        {
                            if (txtOverTime.Text != string.Empty && txtOverTime.Text != HOURS_EMPTY)
                            {
                                header.SH_OverTimeHours2 = Utilities.CommonUtil.TimeToInt(txtOverTime.Text);
                            }
                        }
                        else
                        {
                            if (txtOverTime.Text != string.Empty && txtOverTime.Text != HOURS_EMPTY)
                            {
                                header.LH_OverTimeHours2 = Utilities.CommonUtil.TimeToInt(txtOverTime.Text);
                            }
                        }
                        break;
                    case 2:
                        if (workingType == (int)WorkingType.WorkFullTime)
                        {
                            if (txtOverTime.Text != string.Empty && txtOverTime.Text != HOURS_EMPTY)
                            {
                                header.OverTimeHours3 = Utilities.CommonUtil.TimeToInt(txtOverTime.Text);
                            }
                        }
                        else if (workingType == (int)WorkingType.WorkHoliDay)
                        {
                            if (txtOverTime.Text != string.Empty && txtOverTime.Text != HOURS_EMPTY)
                            {
                                header.SH_OverTimeHours3 = Utilities.CommonUtil.TimeToInt(txtOverTime.Text);
                            }
                        }
                        else
                        {
                            if (txtOverTime.Text != string.Empty && txtOverTime.Text != HOURS_EMPTY)
                            {
                                header.LH_OverTimeHours3 = Utilities.CommonUtil.TimeToInt(txtOverTime.Text);
                            }
                        }
                        break;
                    case 3:
                        if (workingType == (int)WorkingType.WorkFullTime)
                        {
                            if (txtOverTime.Text != string.Empty && txtOverTime.Text != HOURS_EMPTY)
                            {
                                header.OverTimeHours4 = Utilities.CommonUtil.TimeToInt(txtOverTime.Text);
                            }
                        }
                        else if (workingType == (int)WorkingType.WorkHoliDay)
                        {
                            if (txtOverTime.Text != string.Empty && txtOverTime.Text != HOURS_EMPTY)
                            {
                                header.SH_OverTimeHours4 = Utilities.CommonUtil.TimeToInt(txtOverTime.Text);
                            }
                        }
                        else
                        {
                            if (txtOverTime.Text != string.Empty && txtOverTime.Text != HOURS_EMPTY)
                            {
                                header.LH_OverTimeHours4 = Utilities.CommonUtil.TimeToInt(txtOverTime.Text);
                            }
                        }
                        break;
                    case 4:
                        if (workingType == (int)WorkingType.WorkFullTime)
                        {
                            if (txtOverTime.Text != string.Empty && txtOverTime.Text != HOURS_EMPTY)
                            {
                                header.OverTimeHours5 = Utilities.CommonUtil.TimeToInt(txtOverTime.Text);
                            }
                        }
                        else if (workingType == (int)WorkingType.WorkHoliDay)
                        {
                            if (txtOverTime.Text != string.Empty && txtOverTime.Text != HOURS_EMPTY)
                            {
                                header.SH_OverTimeHours5 = Utilities.CommonUtil.TimeToInt(txtOverTime.Text);
                            }
                        }
                        else
                        {
                            if (txtOverTime.Text != string.Empty && txtOverTime.Text != HOURS_EMPTY)
                            {
                                header.LH_OverTimeHours5 = Utilities.CommonUtil.TimeToInt(txtOverTime.Text);
                            }
                        }
                        break;
                }
            }
            if (this.cmbAllHolidays.SelectedValue == "-1")
            {
                if (this.cmbBeforeHalfHoliday.SelectedValue != "-1" && this.cmbLateHoliday.SelectedValue != "-1")
                {
                    header.VacationFlag = (int)VacationFlag.MorningAndAfternoon;
                    header.VacationMorningCD = int.Parse(cmbBeforeHalfHoliday.SelectedValue);
                    header.VacationAfternoonCD = int.Parse(cmbLateHoliday.SelectedValue);
                }
                else if (this.cmbBeforeHalfHoliday.SelectedValue != "-1")
                {
                    header.VacationFlag = (int)VacationFlag.Morning;
                    header.VacationMorningCD = int.Parse(cmbBeforeHalfHoliday.SelectedValue);
                }
                else if (this.cmbLateHoliday.SelectedValue != "-1")
                {
                    header.VacationFlag = (int)VacationFlag.Afternoon;
                    header.VacationAfternoonCD = int.Parse(cmbLateHoliday.SelectedValue);
                }
            }
            else
            {
                header.VacationFlag = (int)VacationFlag.AllHolidays;
                header.VacationFullCD = int.Parse(this.cmbAllHolidays.SelectedValue);
            }

            // when dayoff empty
            if (txtEntryTime.Text != string.Empty)
            {
                header.EntryTime = Utilities.CommonUtil.TimeToInt(txtEntryTime.Text);
                header.ExitTime = Utilities.CommonUtil.TimeToInt(txtExitTime.Text);
            }

            //set WorkingHours
            if (workingType == (int)WorkingType.WorkFullTime)
            {
                if (txtWorkingHours.Text != string.Empty)
                {
                    header.WorkingHours = Utilities.CommonUtil.TimeToInt(txtWorkingHours.Text);
                }
                if (txtLate.Text != string.Empty && txtLate.Text != HOURS_EMPTY)
                {
                    header.LateHours = Utilities.CommonUtil.TimeToInt(txtLate.Text);
                }
                if (txtEarlyHours.Text != string.Empty && txtEarlyHours.Text != HOURS_EMPTY)
                {
                    header.EarlyHours = Utilities.CommonUtil.TimeToInt(txtEarlyHours.Text);
                }
            }
            else if (workingType == (int)WorkingType.WorkHoliDay)
            {
                if (txtCertainHoliday.Text != string.Empty)
                {
                    header.SH_Hours = Utilities.CommonUtil.TimeToInt(txtCertainHoliday.Text);
                }
            }
            else if (workingType == (int)WorkingType.LegalHoliDay)
            {
                if (txtLegalHoliday.Text != string.Empty)
                {
                    header.LH_Hours = Utilities.CommonUtil.TimeToInt(txtLegalHoliday.Text);
                }
            }

            if (this.chkExchangeFlag.Checked)
            {
                header.ExchangeFlag = 1;
            }
            else
            {
                header.ExchangeFlag = 0;
            }

            if (this.cmbExchangeDate.SelectedValue == "-1")
            {
                header.ExchangeDate = null;
            }
            else
            {
                header.ExchangeDate = DateTime.Parse(this.cmbExchangeDate.SelectedValue);
            }

            if (txtTheTotalOvertime.Text != string.Empty)
            {
                header.TotalOverTimeHours = Utilities.CommonUtil.TimeToInt(txtTheTotalOvertime.Text);
            }
            if (txtTheTotalWorkingHours.Text != string.Empty)
            {
                header.TotalWorkingHours = Utilities.CommonUtil.TimeToInt(txtTheTotalWorkingHours.Text);
            }

            header.WSID = int.Parse(this.cmbWorkSchedule.SelectedValue);
            header.Memo = txtMemo.Value;
            if (this.Mode == Mode.Insert)
            {
                header.CreateUID = this.LoginInfo.User.ID;
                header.UpdateUID = this.LoginInfo.User.ID;
            }
            if (this.Mode == Mode.Update)
            {
                header.UpdateDate = this.OldUpdateDate;
                header.UpdateUID = this.LoginInfo.User.ID;
            }

            
            if (header.VacationFlag.HasValue
                || ((header.LateHours.HasValue || header.EarlyHours.HasValue) && GetIsUseApply(M_Config_H.CONFIG_USE_TIME_APPROVAL))
                || ((header.SH_Hours.HasValue || header.LH_Hours.HasValue || header.TotalOverTimeHours.HasValue) && GetIsUseApply(M_Config_H.CONFIG_USE_TIME_APPROVAL)))
            {
                header.ApprovalStatus = (int)AttendanceApprovalStatus.NeedApproval;
            }
            else
            {
                header.ApprovalStatus = (int)AttendanceApprovalStatus.None;
            }
            header.ApprovalUID = 0;
            header.ApprovalDate = null;
            header.ApprovalNote = string.Empty;
            header.RequestNote = string.Empty;

            header.StatusFlag = 0;
            return header;
        }

        /// <summary>
        /// setValueWorkingHour
        /// </summary>
        private void setValueWorkingHour()
        {
            int workingType;
            int totalWorkingHours = 0;
            int shTotalWorkingHours = 0;
            int lhTotalWorkingHours = 0;
            int flag = 0;

            M_WorkingSystem mWorkingSystem = new M_WorkingSystem();
            using (DB db = new DB())
            {
                T_WorkingCalendar_HService tWorkingCalendarHSer = new T_WorkingCalendar_HService(db);

                WorkingSystemService wksysService = new WorkingSystemService(db);

                mWorkingSystem = wksysService.GetDataWorkingSystemById(int.Parse(this.cmbWorkSchedule.SelectedValue));
                workingType = mWorkingSystem.WorkingType;
            }

            foreach (RepeaterItem item in this.rpttbConfig4_Data.Items)
            {

                ITimeTextBox txtOverTime = (ITimeTextBox)item.FindControl("txtOverTime");
                switch (item.ItemIndex)
                {
                    case 0:
                        if (workingType == (int)WorkingType.WorkFullTime)
                        {
                            if (txtOverTime.Text != string.Empty)
                            {
                                totalWorkingHours = totalWorkingHours + Utilities.CommonUtil.TimeToInt(txtOverTime.Text);
                            }

                        }
                        else if (workingType == (int)WorkingType.WorkHoliDay)
                        {
                            if (txtOverTime.Text != string.Empty)
                            {
                                shTotalWorkingHours = shTotalWorkingHours + Utilities.CommonUtil.TimeToInt(txtOverTime.Text);
                            }
                        }
                        else
                        {
                            if (txtOverTime.Text != string.Empty)
                            {
                                lhTotalWorkingHours = lhTotalWorkingHours + Utilities.CommonUtil.TimeToInt(txtOverTime.Text);
                            }
                        }
                        break;
                    case 1:
                        if (workingType == (int)WorkingType.WorkFullTime)
                        {
                            if (txtOverTime.Text != string.Empty)
                            {
                                totalWorkingHours = totalWorkingHours + Utilities.CommonUtil.TimeToInt(txtOverTime.Text);
                            }
                        }
                        else if (workingType == (int)WorkingType.WorkHoliDay)
                        {
                            if (txtOverTime.Text != string.Empty)
                            {
                                shTotalWorkingHours = shTotalWorkingHours + Utilities.CommonUtil.TimeToInt(txtOverTime.Text);
                            }
                        }
                        else
                        {
                            if (txtOverTime.Text != string.Empty)
                            {
                                lhTotalWorkingHours = lhTotalWorkingHours + Utilities.CommonUtil.TimeToInt(txtOverTime.Text);
                            }
                        }
                        break;
                    case 2:
                        if (workingType == (int)WorkingType.WorkFullTime)
                        {
                            if (txtOverTime.Text != string.Empty)
                            {
                                totalWorkingHours = totalWorkingHours + Utilities.CommonUtil.TimeToInt(txtOverTime.Text);
                            }
                        }
                        else if (workingType == (int)WorkingType.WorkHoliDay)
                        {
                            if (txtOverTime.Text != string.Empty)
                            {
                                shTotalWorkingHours = shTotalWorkingHours + Utilities.CommonUtil.TimeToInt(txtOverTime.Text);
                            }
                        }
                        else
                        {
                            if (txtOverTime.Text != string.Empty)
                            {
                                lhTotalWorkingHours = lhTotalWorkingHours + Utilities.CommonUtil.TimeToInt(txtOverTime.Text);
                            }
                        }
                        break;
                    case 3:
                        if (workingType == (int)WorkingType.WorkFullTime)
                        {
                            if (txtOverTime.Text != string.Empty)
                            {
                                totalWorkingHours = totalWorkingHours + Utilities.CommonUtil.TimeToInt(txtOverTime.Text);
                            }
                        }
                        else if (workingType == (int)WorkingType.WorkHoliDay)
                        {
                            if (txtOverTime.Text != string.Empty)
                            {
                                shTotalWorkingHours = shTotalWorkingHours + Utilities.CommonUtil.TimeToInt(txtOverTime.Text);
                            }
                        }
                        else
                        {
                            if (txtOverTime.Text != string.Empty)
                            {
                                lhTotalWorkingHours = lhTotalWorkingHours + Utilities.CommonUtil.TimeToInt(txtOverTime.Text);
                            }
                        }
                        break;
                    case 4:
                        if (workingType == (int)WorkingType.WorkFullTime)
                        {
                            if (txtOverTime.Text != string.Empty)
                            {
                                totalWorkingHours = totalWorkingHours + Utilities.CommonUtil.TimeToInt(txtOverTime.Text);
                            }
                        }
                        else if (workingType == (int)WorkingType.WorkHoliDay)
                        {
                            if (txtOverTime.Text != string.Empty)
                            {
                                shTotalWorkingHours = shTotalWorkingHours + Utilities.CommonUtil.TimeToInt(txtOverTime.Text);
                            }
                        }
                        else
                        {
                            if (txtOverTime.Text != string.Empty)
                            {
                                lhTotalWorkingHours = lhTotalWorkingHours + Utilities.CommonUtil.TimeToInt(txtOverTime.Text);
                            }
                        }
                        break;
                }
            }

            //mWorkingSystem
            DateTime[] arrBreakTime = new DateTime[8];
            DateTime[] arrOverTime = new DateTime[10];
            string[] strEntryTime = new string[2];
            string[] strExitTime = new string[2];
            if (txtEntryTime.Text != string.Empty)
            {
                strEntryTime = txtEntryTime.Text.Split(':');
                strExitTime = txtExitTime.Text.Split(':');
            }
            else
            {
                strEntryTime[0] = "0";
                strEntryTime[1] = "0";
                strExitTime[0] = "0";
                strExitTime[1] = "0";
            }
            string[] systemWorkingStart = new string[2];
            string[] systemWorkingEnd = new string[2];
            string[] systemWorkingEnd_2 = new string[2];
            int breakType = mWorkingSystem.BreakType;
            workingType = mWorkingSystem.WorkingType;

            TimeSpan atWork = new TimeSpan();
            TimeSpan beLate = new TimeSpan();
            TimeSpan leaveEarly = new TimeSpan();
            DateTime workOTBeforeST = new DateTime(1, 1, 1);
            DateTime workOTBeforeET = new DateTime(1, 1, 1);
            DateTime workSTWork = new DateTime(1, 1, 1);
            DateTime workETWork = new DateTime(1, 1, 1);
            DateTime workOTAfterST = new DateTime(1, 1, 1);
            DateTime workOTAfterET = new DateTime(1, 1, 1);

            TimeSpan earlyOT = new TimeSpan();
            TimeSpan normalOT = new TimeSpan();
            TimeSpan lateNightOT = new TimeSpan();
            TimeSpan OT_04 = new TimeSpan();
            TimeSpan OT_05 = new TimeSpan();

            if (mWorkingSystem.Working_Start != null)
            {
                systemWorkingStart = (Utilities.CommonUtil.IntToTime((int)mWorkingSystem.Working_Start, true)).Split(':');
                systemWorkingEnd = (Utilities.CommonUtil.IntToTime((int)mWorkingSystem.Working_End, true)).Split(':');
                if (mWorkingSystem.Working_End_2 != null)
                {
                    systemWorkingEnd_2 = (Utilities.CommonUtil.IntToTime((int)mWorkingSystem.Working_End_2, true)).Split(':');
                }
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

            //get OverTime
            if (mWorkingSystem.OverTime1_Start != null)
            {
                arrOverTime[0] = ConvertIntToDateTime((int)mWorkingSystem.OverTime1_Start);
            }
            if (mWorkingSystem.OverTime1_End != null)
            {
                arrOverTime[1] = ConvertIntToDateTime((int)mWorkingSystem.OverTime1_End);
            }
            if (mWorkingSystem.OverTime2_Start != null)
            {
                arrOverTime[2] = ConvertIntToDateTime((int)mWorkingSystem.OverTime2_Start);
            }
            if (mWorkingSystem.OverTime2_End != null)
            {
                arrOverTime[3] = ConvertIntToDateTime((int)mWorkingSystem.OverTime2_End);
            }
            if (mWorkingSystem.OverTime3_Start != null)
            {
                arrOverTime[4] = ConvertIntToDateTime((int)mWorkingSystem.OverTime3_Start);
            }
            if (mWorkingSystem.OverTime3_End != null)
            {
                arrOverTime[5] = ConvertIntToDateTime((int)mWorkingSystem.OverTime3_End);
            }
            if (mWorkingSystem.OverTime4_Start != null)
            {
                arrOverTime[6] = ConvertIntToDateTime((int)mWorkingSystem.OverTime4_Start);
            }
            if (mWorkingSystem.OverTime4_End != null)
            {
                arrOverTime[7] = ConvertIntToDateTime((int)mWorkingSystem.OverTime4_End);
            }
            if (mWorkingSystem.OverTime5_Start != null)
            {
                arrOverTime[8] = ConvertIntToDateTime((int)mWorkingSystem.OverTime5_Start);
            }
            if (mWorkingSystem.OverTime5_End != null)
            {
                arrOverTime[9] = ConvertIntToDateTime((int)mWorkingSystem.OverTime5_End);
            }

            //Time Work reality
            DateTime workST = new DateTime(1, 1, 1).AddHours(int.Parse(strEntryTime[0])).AddMinutes(int.Parse(strEntryTime[1]));
            DateTime workET = new DateTime(1, 1, 1).AddHours(int.Parse(strExitTime[0])).AddMinutes(int.Parse(strExitTime[1]));
            DateTime startWorkTime = workST;
            DateTime endWorkTime = workET;

            DateTime systemST = new DateTime(1, 1, 1);
            DateTime systemET = new DateTime(1, 1, 1);
            DateTime systemET2 = new DateTime(1, 1, 1);

            if (mWorkingSystem.WorkingType == (int)WorkingType.WorkFullTime)
            {
                if (mWorkingSystem.WorkingSystemCD != "4")
                {
                    string[] First_End = (Utilities.CommonUtil.IntToTime((int)mWorkingSystem.First_End, true)).Split(':');
                    string[] Latter_Start = (Utilities.CommonUtil.IntToTime((int)mWorkingSystem.Latter_Start, true)).Split(':');

                    if (this.cmbAllHolidays.SelectedValue != "-1")
                    {
                        systemST = new DateTime(1, 1, 1).AddHours(0).AddMinutes(0);
                        systemET = new DateTime(1, 1, 1).AddHours(0).AddMinutes(0);
                    }
                    else
                    {
                        if (mWorkingSystem.Working_End_2 != null)
                        {
                            systemET2 = new DateTime(1, 1, 1).AddHours(int.Parse(systemWorkingEnd_2[0])).AddMinutes(int.Parse(systemWorkingEnd_2[1]));
                        }

                        if (this.cmbBeforeHalfHoliday.SelectedValue != "-1" && this.cmbLateHoliday.SelectedValue != "-1")
                        {
                            systemST = new DateTime(1, 1, 1).AddHours(0).AddMinutes(0);
                            systemET = new DateTime(1, 1, 1).AddHours(0).AddMinutes(0);
                        }
                        else if (this.cmbBeforeHalfHoliday.SelectedValue != "-1" && this.cmbLateHoliday.SelectedValue == "-1")
                        {
                            systemST = new DateTime(1, 1, 1).AddHours(int.Parse(Latter_Start[0])).AddMinutes(int.Parse(Latter_Start[1]));
                            systemET = new DateTime(1, 1, 1).AddHours(int.Parse(systemWorkingEnd[0])).AddMinutes(int.Parse(systemWorkingEnd[1]));
                        }
                        else if (this.cmbBeforeHalfHoliday.SelectedValue == "-1" && this.cmbLateHoliday.SelectedValue != "-1")
                        {
                            systemST = new DateTime(1, 1, 1).AddHours(int.Parse(systemWorkingStart[0])).AddMinutes(int.Parse(systemWorkingStart[1]));
                            systemET = new DateTime(1, 1, 1).AddHours(int.Parse(First_End[0])).AddMinutes(int.Parse(First_End[1]));
                            systemET2 = systemET;
                        }
                        else
                        {
                            systemST = new DateTime(1, 1, 1).AddHours(int.Parse(systemWorkingStart[0])).AddMinutes(int.Parse(systemWorkingStart[1]));
                            systemET = new DateTime(1, 1, 1).AddHours(int.Parse(systemWorkingEnd[0])).AddMinutes(int.Parse(systemWorkingEnd[1]));
                        }
                    }
                }
            }

            //Check WorkingType 
            if (mWorkingSystem.WorkingType == (int)WorkingType.WorkFullTime)
            {
                //work time
                workSTWork = startWorkTime >= systemST ? startWorkTime : systemST;
                workETWork = endWorkTime <= systemET ? endWorkTime : systemET;

                //over time morning
                if (startWorkTime < systemST)
                {
                    workOTBeforeST = startWorkTime;
                    workOTBeforeET = endWorkTime < systemST ? endWorkTime : systemST;
                }

                //over time everning
                if (endWorkTime > systemET)
                {
                    workOTAfterST = startWorkTime > systemET ? startWorkTime : systemET;
                    workOTAfterET = endWorkTime;
                }

                if (mWorkingSystem.BreakType == 0)
                {
                    //late and leave early
                    if (systemST < systemET)
                    {
                        if (workST > systemET)
                        {
                            beLate = CalDurationWorkTime(systemST, systemET, arrBreakTime);
                        }
                        else
                        {
                            beLate = CalDurationWorkTime(systemST, workSTWork, arrBreakTime);
                        }

                        if (workET < systemST)
                        {
                            leaveEarly = CalDurationWorkTime(systemST, systemET, arrBreakTime);
                        }
                        else
                        {
                            if (systemET2 != new DateTime())
                            {
                                if (systemET2 <= workETWork && workETWork <= systemET)
                                {
                                    leaveEarly = CalDurationWorkTime(systemET, systemET, arrBreakTime);
                                }
                                else
                                {
                                    if (workETWork < systemET2)
                                    {
                                        leaveEarly = CalDurationWorkTime(workETWork, systemET, arrBreakTime);
                                    }
                                }
                            }
                            else
                            {
                                leaveEarly = CalDurationWorkTime(workETWork, systemET, arrBreakTime);
                            }
                        }

                        //Working time
                        atWork = CalDurationWorkTime(systemST, systemET, arrBreakTime).Subtract(beLate).Subtract(leaveEarly);
                    }
                }

                else if (mWorkingSystem.BreakType == 1)
                {
                    arrBreakTime = new DateTime[8];

                    if (mWorkingSystem.Break1_Start != null)
                    {
                        arrBreakTime[0] = ConvertIntToDateTime((int)mWorkingSystem.Break1_Start);
                    }

                    TimeSpan BreakTimeHour = new TimeSpan(arrBreakTime[0].Hour, arrBreakTime[0].Minute, 0);

                    //late and leave early
                    if (systemST < systemET)
                    {
                        if(workST >= systemET.Subtract(BreakTimeHour))
                        {
                            beLate = CalDurationWorkTime(systemST, systemET, arrBreakTime);
                        }
                        else
                        {
                            beLate = CalDurationWorkTime(systemST, workSTWork, arrBreakTime);
                        }

                        if (workET < systemST || (workET >= systemST && workET <= systemST.Add(BreakTimeHour.Subtract(systemET.Subtract(systemET2)))))
                        {
                            leaveEarly = CalDurationWorkTime(systemST, systemET, arrBreakTime);
                        }
                        else
                        {
                            if (systemET2 != new DateTime())
                            {
                                if (systemET2 <= workETWork && workETWork <= systemET)
                                {
                                    leaveEarly = CalDurationWorkTime(systemET, systemET, arrBreakTime);
                                }
                                else
                                {
                                    if (workETWork < systemET2)
                                    {
                                        leaveEarly = CalDurationWorkTime(workETWork, systemET, arrBreakTime);
                                    }
                                }
                            }
                            else
                            {
                                leaveEarly = CalDurationWorkTime(workETWork, systemET, arrBreakTime);
                            }
                        }

                        //Working time
                        atWork = CalDurationWorkTime(systemST, systemET, arrBreakTime).Subtract(beLate).Subtract(leaveEarly).Subtract(BreakTimeHour);
                    }
                }
                else if (mWorkingSystem.BreakType == 2)
                {
                    DateTime[] arrBreakTime_Type2 = new DateTime[40];
                    TimeSpan TimeHourToBreak = new TimeSpan();
                    TimeSpan TimeBreak = new TimeSpan();
                    DateTime sTime = new DateTime(1, 1, 1).AddHours(startWorkTime.Hour).AddMinutes(startWorkTime.Minute);
                    DateTime eTime = new DateTime(1, 1, 1).AddHours(24);
                    if (startWorkTime.Date == endWorkTime.Date)
                    {
                        eTime = new DateTime(1, 1, 1).AddHours(endWorkTime.Hour).AddMinutes(endWorkTime.Minute);
                    }
                    else
                    {
                        eTime = new DateTime(1, 1, 2).AddHours(endWorkTime.Hour).AddMinutes(endWorkTime.Minute);
                    }

                    // BreakTime Hours
                    if (mWorkingSystem.Break1_Start == null || mWorkingSystem.Break1_End == null)
                    {
                        atWork = endWorkTime.Subtract(startWorkTime);
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

                    //late and leave early
                    if (systemST < systemET)
                    {
                        if (workST > systemET)
                        {
                            beLate = CalDurationWorkTime(systemST, systemET, arrBreakTime_Type2);
                        }
                        else
                        {
                            beLate = CalDurationWorkTime(systemST, workSTWork, arrBreakTime_Type2);
                        }

                        if (workET < systemST)
                        {
                            leaveEarly = CalDurationWorkTime(systemST, systemET, arrBreakTime_Type2);
                        }
                        else
                        {
                            if (systemET2 != new DateTime())
                            {
                                if (systemET2 <= workETWork && workETWork <= systemET)
                                {
                                    leaveEarly = CalDurationWorkTime(systemET, systemET, arrBreakTime_Type2);
                                }
                                else
                                {
                                    if (workETWork < systemET2)
                                    {
                                        leaveEarly = CalDurationWorkTime(workETWork, systemET, arrBreakTime_Type2);
                                    }
                                }
                            }
                            else
                            {
                                leaveEarly = CalDurationWorkTime(workETWork, systemET, arrBreakTime_Type2);
                            }
                        }

                        //Working time
                        atWork = CalDurationWorkTime(systemST, systemET, arrBreakTime_Type2).Subtract(beLate).Subtract(leaveEarly);
                    }

                    // Overtime calculate
                    if (workOTBeforeST != new DateTime(1, 1, 1) || workOTBeforeET != new DateTime(1, 1, 1))
                    {
                        ClassifyOTTime(workOTBeforeST, workOTBeforeET, arrBreakTime_Type2, arrOverTime, ref earlyOT, ref normalOT, ref lateNightOT, ref OT_04, ref OT_05);
                    }
                    if (workOTAfterST != new DateTime(1, 1, 1) || workOTAfterET != new DateTime(1, 1, 1))
                    {
                        ClassifyOTTime(workOTAfterST, workOTAfterET, arrBreakTime_Type2, arrOverTime, ref earlyOT, ref normalOT, ref lateNightOT, ref OT_04, ref OT_05);
                    }
                }

                if (txtWorkingHours.Text == string.Empty || txtWorkingHours.Text == HOURS_EMPTY)
                {
                    //set workingHours
                    txtWorkingHours.Text = FormatTimeResult(atWork);
                } 
                flag = 1;
                totalWorkingHours = totalWorkingHours + Utilities.CommonUtil.TimeToInt(txtWorkingHours.Text);
                
            }
            else if ((workingType == (int)WorkingType.WorkHoliDay) || (workingType == (int)WorkingType.LegalHoliDay))
            {
                

                if (mWorkingSystem.BreakType == 0)
                {
                    ClassifyOTTime(workST, workET, arrBreakTime, arrOverTime, ref earlyOT, ref normalOT, ref lateNightOT, ref OT_04, ref OT_05);
                    //Working time
                    atWork = CalDurationWorkTime(workST, workET, arrBreakTime).Subtract(earlyOT)
                                                                              .Subtract(normalOT)
                                                                              .Subtract(lateNightOT)
                                                                              .Subtract(OT_04)
                                                                              .Subtract(OT_05);
                }

                else if (mWorkingSystem.BreakType == 1)
                {
                    ClassifyOTTime(workST, workET, arrBreakTime, arrOverTime, ref earlyOT, ref normalOT, ref lateNightOT, ref OT_04, ref OT_05);

                    // BreakTime Hours
                    TimeSpan BreakTimeHour = new TimeSpan(arrBreakTime[0].Hour, arrBreakTime[0].Minute, 0);

                    //Working time
                    atWork = endWorkTime.Subtract(startWorkTime).Subtract(BreakTimeHour)
                                                                .Subtract(earlyOT)
                                                                .Subtract(normalOT)
                                                                .Subtract(lateNightOT)
                                                                .Subtract(OT_04)
                                                                .Subtract(OT_05);
                    if (atWork < new TimeSpan(0, 0, 0))
                    {
                        atWork = new TimeSpan(0, 0, 0);
                    }
                }

                // When breakType = 2 
                else if (mWorkingSystem.BreakType == 2)
                {
                    TimeSpan TimeHourToBreak = new TimeSpan();
                    TimeSpan TimeBreak = new TimeSpan();

                    DateTime[] arrBreakTime_Type2 = new DateTime[40];
                    DateTime sTime = new DateTime(1, 1, 1).AddHours(workST.Hour).AddMinutes(workST.Minute);
                    DateTime eTime = new DateTime(1, 1, 1).AddHours(24);

                    //Working time
                    TimeSpan TotalWorkInDay = endWorkTime.Subtract(startWorkTime);

                    // BreakTime Hours
                    if (mWorkingSystem.Break1_Start == null || mWorkingSystem.Break1_End == null)
                    {
                        atWork = TotalWorkInDay;
                    }
                    else
                    {
                        
                        if (workST.Date == workET.Date)
                        {
                            eTime = new DateTime(1, 1, 1).AddHours(workET.Hour).AddMinutes(workET.Minute);
                        }
                        else
                        {
                            eTime = new DateTime(1, 1, 2).AddHours(workET.Hour).AddMinutes(workET.Minute);
                        }

                        // BreakTime Hours
                        if (mWorkingSystem.Break1_Start == null)
                        {
                            atWork = endWorkTime.Subtract(startWorkTime);
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
                    }
                    ClassifyOTTime(workST, workET, arrBreakTime_Type2, arrOverTime, ref earlyOT, ref normalOT, ref lateNightOT, ref OT_04, ref OT_05);

                    //Working time
                    atWork = CalDurationWorkTime(workST, workET, arrBreakTime_Type2).Subtract(earlyOT)
                                                                                .Subtract(normalOT)
                                                                                .Subtract(lateNightOT)
                                                                                .Subtract(OT_04)
                                                                                .Subtract(OT_05);
                }

                string strOverTime = string.Empty;
                if (txtTheTotalOvertime.Text == string.Empty)
                {
                    strOverTime = HOURS_EMPTY;
                }
                else
                {
                    strOverTime = txtTheTotalOvertime.Text;
                }
                if (workingType == (int)WorkingType.WorkHoliDay)
                {
                    string strAtWork = string.Empty;

                    if (atWork.Days > 0)
                    {
                        string[] arrAtWork = FormatTimeResult(atWork).Split(':');
                        strAtWork = (int.Parse(arrAtWork[0]) + (atWork.Days * 24)).ToString() + ":" + arrAtWork[1];
                    }
                    else
                    {
                        strAtWork = FormatTimeResult(atWork);
                    }

                    if (txtCertainHoliday.Text == string.Empty || txtCertainHoliday.Text == HOURS_EMPTY)
                    {
                        txtCertainHoliday.Text = strAtWork;
                    }
                    shTotalWorkingHours = Utilities.CommonUtil.TimeToInt(txtCertainHoliday.Text) + Utilities.CommonUtil.TimeToInt(strOverTime);
                    flag = 1;
                }

                if (workingType == (int)WorkingType.LegalHoliDay)
                {
                    string strAtWork = string.Empty;
                    
                    if (atWork.Days > 0)
                    {
                        string[] arrAtWork = FormatTimeResult(atWork).Split(':');
                        strAtWork = (int.Parse(arrAtWork[0]) + (atWork.Days * 24)).ToString() + ":" + arrAtWork[1];
                    }
                    else
                    {
                        strAtWork = FormatTimeResult(atWork);
                    }

                    if (txtLegalHoliday.Text == string.Empty || txtLegalHoliday.Text == HOURS_EMPTY)
                    {
                        txtLegalHoliday.Text = strAtWork;
                    }

                    lhTotalWorkingHours = Utilities.CommonUtil.TimeToInt(txtLegalHoliday.Text) + Utilities.CommonUtil.TimeToInt(strOverTime);
                    flag = 1;
                }
            }

            // check flag
            if (flag != 0)
            {   
                if (workingType == (int)WorkingType.WorkFullTime)
                {
                    txtTheTotalWorkingHours.Text = Utilities.CommonUtil.IntToTime(totalWorkingHours,false);
                }
                else if (workingType == (int)WorkingType.WorkHoliDay)
                {
                    txtTheTotalWorkingHours.Text = Utilities.CommonUtil.IntToTime(shTotalWorkingHours, false);
                }
                else if (workingType == (int)WorkingType.LegalHoliDay)
                {
                    txtTheTotalWorkingHours.Text = Utilities.CommonUtil.IntToTime(lhTotalWorkingHours,false);
                }
            }
        }

        /// <summary>
        /// Get getDetail T_Work_H
        /// </summary>
        /// <returns></returns>
        private T_Work_H getDetailWorkH(int Id)
        {
            T_Work_H detailWorkH = new T_Work_H();
            if (this.Mode == Mode.Insert)
            {
                
                detailWorkH.HID = Id;
                detailWorkH.UID =int.Parse(this.UID);
                detailWorkH.createUID = this.LoginInfo.User.ID;
                detailWorkH.UpdateUID = this.LoginInfo.User.ID;
                detailWorkH.Date = DateTime.Parse(this.dateViewState);
            }
            else
            {
                using (DB db = new DB())
                {
                    Work_HService detailWorkHService = new Work_HService(db);
                    detailWorkH = detailWorkHService.GetByHID(this.TAttendanceId);
                    detailWorkH.UpdateUID = this.LoginInfo.User.ID;
                }
            }
            return detailWorkH;
        }

        /// <summary>
        /// Get detail list from screen
        /// </summary>
        /// <returns></returns>
        private List<T_Work_D> GetDetailList(bool isProcess)
        {
            List<T_Work_D> results = new List<T_Work_D>();

            foreach (RepeaterItem item in this.rptDetail.Items)
            {
                if (isProcess && this.IsEmptyRow(item))
                {
                    continue;
                }

                HtmlInputCheckBox chkDelFlg = (HtmlInputCheckBox)item.FindControl("deleteFlag");
                ICodeTextBox txtProjectCD = (ICodeTextBox)item.FindControl("txtProjectCD");
                ITextBox txtProjectNm = (ITextBox)item.FindControl("txtProjectNm");
                ITextBox txtStartTime = (ITextBox)item.FindControl("txtStartTime");
                ITextBox txtEndTime = (ITextBox)item.FindControl("txtEndTime");
                ITextBox txtWorkTime = (ITextBox)item.FindControl("txtWorkTime");
                ITextBox txtWorkPlace = (ITextBox)item.FindControl("txtWorkPlace");
                ITextBox txtMemo = (ITextBox)item.FindControl("txtMemo");

                T_Work_D addItem = new T_Work_D();

                //Delete flag
                if (chkDelFlg != null)
                {
                    addItem.DelFlag = (chkDelFlg.Checked) ? true : false;
                }
                
                // for up/dow 
                if (chkDelFlg != null)
                {
                    addItem.Checked = (chkDelFlg.Checked) ? true : false;
                }

                //Project ID
                addItem.PID = txtProjectCD.Text == string.Empty ? 0 : GetProjectIdByCd(txtProjectCD.Text, new DB());

                addItem.ProjectCD = txtProjectCD.Text;

                //Project Name
                addItem.ProjectName = txtProjectNm.Text;

                //StartTime
                addItem.StartTime = Utilities.CommonUtil.TimeToInt(txtStartTime.Value);

                //EndTime
                addItem.EndTime = Utilities.CommonUtil.TimeToInt(txtEndTime.Value);

                //WorkTime
                addItem.WorkTime = Utilities.CommonUtil.TimeToInt(txtWorkTime.Value);

                //WorkPlace
                addItem.WorkPlace = txtWorkPlace.Value;

                //txtMemo
                addItem.Memo = txtMemo.Value;

                results.Add(addItem);
            }

            //Reset index for row
            this._indexSell = 1;
            for (int i = 0; i < results.Count; i++)
            {
                var row = results[i];
                row.LineNo = this._indexSell++;
            }
            return results;
        }

        /// <summary>
        /// Insert data
        /// </summary>
        private bool InsertData(ref int newId)
        {
            try
            {
                int totalWorkingHours = 0;
                T_Attendance header = this.GetHeader();

                using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                {
                    AttendanceService headerService = new AttendanceService(db);
                    Work_HService detailWorkHService = new Work_HService(db);
                    Work_DService detailWorkDService = new Work_DService(db);

                    if (!ExchangeDateConfirmCheck(db))
                    {
                        return false;
                    }

                    // Insert Header (T_Attendance)
                    headerService.Insert(header);

                    newId = db.GetIdentityId<T_Attendance>();

                    T_Work_H detailWorkH = this.getDetailWorkH(newId);
                    detailWorkHService.Insert(detailWorkH);

                    List<T_Work_D> detailWorkD = this.GetDetailList(true);                    

                    foreach (var item in detailWorkD)
                    {
                        totalWorkingHours = totalWorkingHours + item.WorkTime;
                        item.HID = newId;
                        item.Date = detailWorkH.Date;
                        detailWorkDService.Insert(item);
                    }
                    detailWorkH = detailWorkHService.GetByHID(newId);
                    detailWorkH.TotalWorkingHours = totalWorkingHours;
                    detailWorkHService.Update(detailWorkH);

                    //********* update vacation days of user ***********
                    decimal useVacationDays = this.CalcUseVacationDays(null, header);
                    if (useVacationDays != 0)
                    {

                        if (!this.PaidVacationProcess(db, useVacationDays))
                        {
                            this.SetMessage(string.Empty, M_Message.MSG_DATA_CHANGED);
                            return false;
                        }
                    }
                    //**************************************************


                    db.Commit();
                }
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains(Models.Constant.T_ATTENDANCE_UN))
                {
                    //data had changed
                    this.SetMessage(string.Empty, M_Message.MSG_DATA_CHANGED);
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
        /// Update Data
        /// </summary>
        /// <returns>Success:true, Faile:false</returns>
        private bool UpdateData()
        {
            try
            {
                int ret = 0;
                T_Attendance header = this.GetHeaderData(this.TAttendanceId);
                T_Attendance oldHeader = this.GetHeaderData(this.TAttendanceId);
                T_Work_H detailtWorkH;
                int totalWorkingHours = 0;
                if (header != null)
                {
                    header = this.GetHeader();
                    header.ID = this.TAttendanceId;
                    header.RequestNote = string.Empty;
                    
                    //Update                     
                    using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                    {
                        if (!ExchangeDateConfirmCheck(db))
                        {
                            return false;
                        }
                        if (header.Status == DataStatus.Changed || this.IsDetailChange(db))
                        {
                            //Update header
                            AttendanceService headerService = new AttendanceService(db);
                            ret = headerService.Update(header);
                            if (ret != 0)
                            {
                                //Update detail
                                Work_HService detailWork_HService = new Work_HService(db);
                                detailtWorkH = getDetailWorkH(this.TAttendanceId);
                                

                                Work_DService detailWork_DService = new Work_DService(db);
                                detailWork_DService.DeleteAllByHId(this.TAttendanceId);

                                List<T_Work_D> detail = this.GetDetailList(true);
                                foreach (var item in detail)
                                {
                                    totalWorkingHours = totalWorkingHours + item.WorkTime;
                                    item.HID = header.ID;
                                    item.Date = header.Date;
                                    detailWork_DService.Insert(item);
                                }
                                detailtWorkH.TotalWorkingHours = totalWorkingHours;
                                detailWork_HService.Update(detailtWorkH);
                            }

                            //********* update vacation days of user ***********
                            decimal useVacationDays = this.CalcUseVacationDays(oldHeader, header);
                            if (useVacationDays != 0)
                            {
                                if (!this.PaidVacationProcess(db, useVacationDays))
                                {
                                    this.SetMessage(string.Empty, M_Message.MSG_DATA_CHANGED);
                                    return false;
                                }
                            }
                            //**************************************************

                            db.Commit();
                        }
                        else
                        {
                            return true;
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
        /// Update Data
        /// </summary>
        /// <returns>Success:true, Faile:false</returns>
        private bool ApprovalData()
        {
            try
            {
                int ret = 0;
                T_Attendance header = this.GetHeaderData(this.TAttendanceId);

                if (header != null)
                {
                    header.ApprovalStatus = (int)AttendanceApprovalStatus.Approved;
                    header.ID = this.TAttendanceId;
                    header.ApprovalNote = this.txtApprovalNote.Value;
                    header.ApprovalUID = this.LoginInfo.User.ID;
                    header.UpdateDate = this.OldUpdateDate;

                    //Update                     
                    using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                    {
                        //Update header
                        AttendanceService headerService = new AttendanceService(db);
                        ret = headerService.UpdateApproval(header);

                        db.Commit();

                    }

                    //Check result update
                    if (ret == 0)
                    {
                        //data had changed
                        this.SetMessage(string.Empty, M_Message.MSG_DATA_CHANGED);
                        return false;
                    }
                    else
                    {
                        this.SendEmail(AttendanceApprovalStatus.Approved, header);
                    }
                }
            }
            catch (Exception ex)
            {
                this.SetMessage(string.Empty, M_Message.MSG_UPDATE_FAILE, "承認");

                Log.Instance.WriteLog(ex);

                return false;
            }

            return true;
        }

        /// <summary>
        /// Update Data
        /// </summary>
        /// <returns>Success:true, Faile:false</returns>
        private bool CancelApprovalData()
        {
            try
            {
                int ret = 0;
                T_Attendance header = this.GetHeaderData(this.TAttendanceId);

                if (header != null)
                {
                    header.ApprovalStatus = (int)AttendanceApprovalStatus.Cancel;
                    header.ID = this.TAttendanceId;
                    header.ApprovalNote = this.txtApprovalNote.Value;
                    header.ApprovalUID = this.LoginInfo.User.ID;
                    header.UpdateDate = this.OldUpdateDate;

                    //Update                     
                    using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                    {
                        //Update header
                        AttendanceService headerService = new AttendanceService(db);
                        ret = headerService.UpdateApproval(header);

                        db.Commit();

                    }

                    //Check result update
                    if (ret == 0)
                    {
                        //data had changed
                        this.SetMessage(string.Empty, M_Message.MSG_DATA_CHANGED);
                        return false;
                    }
                    else
                    {
                        this.SendEmail(AttendanceApprovalStatus.Cancel, header);
                    }
                }
            }
            catch (Exception ex)
            {
                this.SetMessage(string.Empty, M_Message.MSG_UPDATE_FAILE, "取り消し");

                Log.Instance.WriteLog(ex);

                return false;
            }

            return true;
        }

        /// <summary>
        /// Update Data
        /// </summary>
        /// <returns>Success:true, Faile:false</returns>
        private bool RequestApprovalData()
        {
            try
            {
                int ret = 0;
                T_Attendance header = this.GetHeaderData(this.TAttendanceId);

                if (header != null)
                {
                    header.ApprovalStatus = (int)AttendanceApprovalStatus.Request;
                    header.ID = this.TAttendanceId;
                    header.RequestNote = this.txtRequestNote.Value;
                    header.UpdateDate = this.OldUpdateDate;

                    //Update                     
                    using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                    {
                        //Update header
                        AttendanceService headerService = new AttendanceService(db);
                        ret = headerService.UpdateApproval(header);

                        db.Commit();
                    }

                    //Check result update
                    if (ret == 0)
                    {
                        //data had changed
                        this.SetMessage(string.Empty, M_Message.MSG_DATA_CHANGED);
                        return false;
                    }
                    else
                    {
                        this.SendEmail(AttendanceApprovalStatus.Request, header);
                    }
                }
            }
            catch (Exception ex)
            {
                this.SetMessage(string.Empty, M_Message.MSG_UPDATE_FAILE, "申請");

                Log.Instance.WriteLog(ex);

                return false;
            }

            return true;
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <returns>Success:true, Faile:false</returns>
        private bool DeleteData()
        {
            try
            {
                T_Attendance oldHeader = this.GetHeaderData(this.TAttendanceId);
                int ret = 0;
                using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                {
                    if (!ExchangeDateConfirmCheck(db))
                    {
                        return false;
                    }

                    //Delete detailWorkD
                    Work_DService detailWork_DService = new Work_DService(db);
                    detailWork_DService.DeleteAllByHId(this.TAttendanceId);
                    
                    //Delete detailWorkH
                    Work_HService detailWork_HService = new Work_HService(db);
                    detailWork_HService.Delete(this.TAttendanceId);

                    //Delete Header
                    AttendanceService headerService = new AttendanceService(db);
                    ret = headerService.Delete(this.TAttendanceId, this.OldUpdateDate);
                    if (ret == 1)
                    {
                        //********* update vacation days of user ***********
                        decimal useVacationDays = this.CalcUseVacationDays(oldHeader, null);
                        if (useVacationDays != 0)
                        {
                            if (!this.PaidVacationProcess(db, useVacationDays))
                            {
                                this.SetMessage(string.Empty, M_Message.MSG_DATA_CHANGED);
                                return false;
                            }
                        }
                        //**************************************************

                        db.Commit();
                    }
                }

                //Check result update
                if (ret == 0)
                {
                    //data change (check two session)
                    this.SetMessage(string.Empty, M_Message.MSG_DATA_CHANGED);
                    return false;
                }
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
        /// Process Paid Vacation day of user
        /// </summary>
        /// <param name="db"></param>
        /// <param name="useVacationDays"></param>
        /// <returns></returns>
        private bool PaidVacationProcess(DB db, decimal useVacationDays)
        {
            T_PaidVacationService vacationDaySer = new T_PaidVacationService(db);
            IList<T_PaidVacation> lstPaidVacation = vacationDaySer.GetListValid(int.Parse(this.UID));
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

        /// <summary>
        /// Check detail change data
        /// </summary>
        /// <returns></returns>
        private bool IsDetailChange(DB db)
        {
            //Get list from data
            IList<T_Work_D> listDetailData;

            Work_DService work_DService = new Work_DService(db);
            listDetailData = work_DService.GetListByHID(this.TAttendanceId);

            //Get list from screen
            List<T_Work_D> listDetailSrceen = this.GetDetailList(true);

            //Check count change
            if (listDetailSrceen.Count != listDetailData.Count)
            {
                return true;
            }
            else
            {
                for (int i = 0; i < listDetailSrceen.Count; i++)
                {
                    var oldData = listDetailData[i];
                    var newData = listDetailSrceen[i];

                    //Check PID change
                    if (oldData.PID.CompareTo(newData.PID) != 0)
                    {
                        return true;
                    }

                    //Check workPlace change
                    if (oldData.WorkPlace.CompareTo(newData.WorkPlace) != 0)
                    {
                        return true;
                    }

                    //Check startTime change
                    if (oldData.StartTime != newData.StartTime)
                    {
                        return true;
                    }

                    //Check EndTime change
                    if (oldData.EndTime != newData.EndTime)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Check empty row
        /// </summary>
        /// <returns></returns>
        private bool IsEmptyRow(RepeaterItem item)
        {
            bool ret = true;

            //ProjectCD
            ICodeTextBox txtProjectCD = (ICodeTextBox)item.FindControl("txtProjectCD");
            if (txtProjectCD != null)
            {
                if (!string.IsNullOrEmpty(txtProjectCD.Value))
                {
                    ret = false;
                }
            }

            //ProjectNm
            ITextBox txtProjectNm = (ITextBox)item.FindControl("txtProjectNm");
            if (txtProjectNm != null)
            {
                if (!string.IsNullOrEmpty(txtProjectNm.Value))
                {
                    ret = false;
                }
            }

            //StartTime
            ITextBox txtStartTime = (ITextBox)item.FindControl("txtStartTime");
            if (txtStartTime != null)
            {
                if (!string.IsNullOrEmpty(txtStartTime.Value))
                {
                    ret = false;
                }
            }

            //EndingTime
            ITextBox txtEndTime = (ITextBox)item.FindControl("txtEndTime");
            if (txtEndTime != null)
            {
                if (!string.IsNullOrEmpty(txtEndTime.Value))
                {
                    ret = false;
                }
            }

            //WorkingHours
            ITextBox txtWorkTime = (ITextBox)item.FindControl("txtWorkTime");
            if (txtWorkTime != null)
            {
                if (!string.IsNullOrEmpty(txtWorkTime.Value))
                {
                    ret = false;
                }
            }

            //txtWorkPlace
            ITextBox txtWorkPlace = (ITextBox)item.FindControl("txtWorkPlace");
            if (txtWorkPlace != null)
            {
                if (!string.IsNullOrEmpty(txtWorkPlace.Value))
                {
                    ret = false;
                }
            }

            //txtRemarks
            ITextBox txtMemo = (ITextBox)item.FindControl("txtMemo");
            if (txtMemo != null)
            {
                if (!string.IsNullOrEmpty(txtMemo.Value))
                {
                    ret = false;
                }
            }

            return ret;
        }

        /// <summary>
        /// Up or Down Item
        /// </summary>
        /// <param name="up">Up Flg</param>
        private void SwapDetail(bool up)
        {
            IList<T_Work_D> details = this.GetDetailList(true);

            if (details.Count == 0)
            {
                return;
            }

            IList<T_Work_D> results = new List<T_Work_D>();

            if (up)
            {
                results.Add(details.First());
                for (int i = 1; i < details.Count; i++)
                {
                    var item = details[i];
                    var itemPre = results[i - 1];

                    if (item.Checked)
                    {
                        if (itemPre.Checked)
                        {
                            results.Add(item);
                        }
                        else
                        {
                            results.Insert(i - 1, item);
                        }
                    }
                    else
                    {
                        results.Add(item);
                    }
                }
            }
            else
            {
                results.Add(details.Last());
                for (int i = details.Count - 2; i >= 0; i--)
                {
                    var item = details[i];
                    if (item.Checked)
                    {
                        if (results[0].Checked)
                        {
                            results.Insert(0, item);
                        }
                        else
                        {
                            results.Insert(1, item);
                        }
                    }
                    else
                    {
                        results.Insert(0, item);
                    }
                }
            }

            this._indexSell = 1;
            for (int i = 0; i < results.Count; i++)
            {
                var row = results[i];
                row.LineNo = this._indexSell++;
            }

            this.rptDetail.DataSource = results;
            this.rptDetail.DataBind();
        }

        /// <summary>
        /// Show Header Data on form
        /// </summary>
        /// <param name="T_WorkingCalendar_H">T_WorkingCalendar_H</param>
        private void ShowHeaderData(T_Attendance tAttendance, bool isShowDetail = true)
        {
            int workingType;

            if (tAttendance != null)
            {
                string[] arrDayOfWeek = {"日","月","火","水","木","金","土"}; 
                
                this.txtDate.Value = tAttendance.Date.ToString(Constants.FMT_YMD_JP);
                txtDate.Text = tAttendance.Date.ToString(Constants.FMT_YMD_JP) + " (" + arrDayOfWeek[(int)tAttendance.Date.DayOfWeek] + ")";
                dtCopyDate.Value = tAttendance.Date.AddDays(-1);
                using (DB db = new DB())
                {
                    UserService userService = new UserService(db);
                    txtEmployeeName.Text = userService.GetByID(tAttendance.UID).UserName1.ToString();

                    //set VacationDay
                    T_PaidVacationService paidVacationSer = new T_PaidVacationService(db);
                    txtVacationDay.Text = paidVacationSer.GetTotalVacationDays(int.Parse(this.UID)).ToString("#,##0.0");

                    //set Verification
                    AttendanceService atdSer = new AttendanceService(db);
                    var dataVacation = atdSer.GetListVacation(int.Parse(this.UID));

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
                string strWorkingStart = string.Empty;
                string strWorkingEnd = string.Empty;
                if (tAttendance.EntryTime != null)
                {
                    strWorkingStart = Utilities.CommonUtil.IntToTime((int)tAttendance.EntryTime, true);

                }
                if (tAttendance.ExitTime != null)
                {
                    strWorkingEnd = Utilities.CommonUtil.IntToTime((int)tAttendance.ExitTime, false);
                }

                if (strWorkingStart != string.Empty)
                {
                    txtEntryTime.Text = strWorkingStart;
                }
                else
                {
                    txtEntryTime.Text = string.Empty;
                }

                if (strWorkingStart != string.Empty)
                {
                    txtExitTime.Text = strWorkingEnd;
                }
                else
                {
                    txtExitTime.Text = string.Empty;
                }

                if (tAttendance.ExchangeFlag == 1)
                {
                    this.chkExchangeFlag.Checked = true;
                }
                else
                {
                    this.chkExchangeFlag.Checked = false;
                }

                if (tAttendance.ExchangeDate == null)
                {
                    this.cmbExchangeDate.SelectedValue = "-1";
                }
                else
                {
                    this.cmbExchangeDate.SelectedValue = tAttendance.ExchangeDate.Value.ToString(Constants.FMT_DATE_EN);
                }

                using (DB db = new DB())
                {
                    WorkingSystemService wkSystemService = new WorkingSystemService(db);
                    M_WorkingSystem mWorkingSystem = new M_WorkingSystem();
                    mWorkingSystem = wkSystemService.GetDataWorkingSystemById(tAttendance.WSID);
                    workingType = mWorkingSystem.WorkingType;
                }
                cmbWorkSchedule.SelectedValue = tAttendance.WSID.ToString();

                if (workingType == (int)WorkingType.WorkFullTime)
                {
                    if (tAttendance.WorkingHours != null)
                    {
                        txtWorkingHours.Text = Utilities.CommonUtil.IntToTime((int)tAttendance.WorkingHours, true);
                    }
                    else
                    {
                        txtWorkingHours.Text = string.Empty;
                    }
                    if (tAttendance.LateHours != null)
                    {
                        txtLate.Text = Utilities.CommonUtil.IntToTime((int)tAttendance.LateHours, true);
                    }
                    else
                    {
                        txtLate.Text = string.Empty;
                    }
                    if (tAttendance.EarlyHours != null)
                    {
                        txtEarlyHours.Text = Utilities.CommonUtil.IntToTime((int)tAttendance.EarlyHours, true);
                    }
                    else
                    {
                        txtEarlyHours.Text = string.Empty;
                    }
                }
                else if (workingType == (int)WorkingType.WorkHoliDay)
                {
                    if (tAttendance.SH_Hours != null)
                    {
                        txtCertainHoliday.Text = Utilities.CommonUtil.IntToTime((int)tAttendance.SH_Hours, true);
                    }
                    else
                    {
                        txtCertainHoliday.Text = string.Empty;
                    }

                    txtDate.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    if (tAttendance.LH_Hours != null)
                    {
                        txtLegalHoliday.Text = Utilities.CommonUtil.IntToTime((int)tAttendance.LH_Hours, true);
                    }
                    else
                    {
                        txtLegalHoliday.Text = string.Empty;
                    }

                    txtDate.ForeColor = System.Drawing.Color.Red;
                }

                foreach (RepeaterItem item in this.rpttbConfig4_Data.Items)
                {

                    ITimeTextBox txtOverTime = (ITimeTextBox)item.FindControl("txtOverTime");
                    switch (item.ItemIndex)
                    {
                        case 0:
                            if (workingType == (int)WorkingType.WorkFullTime)
                            {
                                if (tAttendance.OverTimeHours1 != null)
                                {
                                    txtOverTime.Text = Utilities.CommonUtil.IntToTime((int)tAttendance.OverTimeHours1, true);
                                }
                                else
                                {
                                    txtOverTime.Text = string.Empty;
                                }
                            }
                            else if (workingType == (int)WorkingType.WorkHoliDay)
                            {
                                if (tAttendance.SH_OverTimeHours1 != null)
                                {
                                    txtOverTime.Text = Utilities.CommonUtil.IntToTime((int)tAttendance.SH_OverTimeHours1, true);
                                }
                                else
                                {
                                    txtOverTime.Text = string.Empty;
                                }
                            }
                            else
                            {
                                if (tAttendance.LH_OverTimeHours1 != null)
                                {
                                    txtOverTime.Text = Utilities.CommonUtil.IntToTime((int)tAttendance.LH_OverTimeHours1, true);
                                }
                                else
                                {
                                    txtOverTime.Text = string.Empty;
                                }
                            }
                            break;
                        case 1:
                            if (workingType == (int)WorkingType.WorkFullTime)
                            {
                                if (tAttendance.OverTimeHours2 != null)
                                {
                                    txtOverTime.Text = Utilities.CommonUtil.IntToTime((int)tAttendance.OverTimeHours2, true);
                                }
                                else
                                {
                                    txtOverTime.Text = string.Empty;
                                }
                            }
                            else if (workingType == (int)WorkingType.WorkHoliDay)
                            {
                                if (tAttendance.SH_OverTimeHours2 != null)
                                {
                                    txtOverTime.Text = Utilities.CommonUtil.IntToTime((int)tAttendance.SH_OverTimeHours2, true);
                                }
                                else
                                {
                                    txtOverTime.Text = string.Empty;
                                }
                            }
                            else
                            {
                                if (tAttendance.LH_OverTimeHours2 != null)
                                {
                                    txtOverTime.Text = Utilities.CommonUtil.IntToTime((int)tAttendance.LH_OverTimeHours2, true);
                                }
                                else
                                {
                                    txtOverTime.Text = string.Empty;
                                }
                            }
                            break;
                        case 2:
                            if (workingType == (int)WorkingType.WorkFullTime)
                            {
                                if (tAttendance.OverTimeHours3 != null)
                                {
                                    txtOverTime.Text = Utilities.CommonUtil.IntToTime((int)tAttendance.OverTimeHours3, true);
                                }
                                else
                                {
                                    txtOverTime.Text = string.Empty;
                                }
                            }
                            else if (workingType == (int)WorkingType.WorkHoliDay)
                            {
                                if (tAttendance.SH_OverTimeHours3 != null)
                                {
                                    txtOverTime.Text = Utilities.CommonUtil.IntToTime((int)tAttendance.SH_OverTimeHours3, true);
                                }
                                else
                                {
                                    txtOverTime.Text = string.Empty;
                                }
                            }
                            else
                            {
                                if (tAttendance.LH_OverTimeHours3 != null)
                                {
                                    txtOverTime.Text = Utilities.CommonUtil.IntToTime((int)tAttendance.LH_OverTimeHours3, true);
                                }
                                else
                                {
                                    txtOverTime.Text = string.Empty;
                                }
                            }
                            break;
                        case 3:
                            if (workingType == (int)WorkingType.WorkFullTime)
                            {
                                if (tAttendance.OverTimeHours4 != null)
                                {
                                    txtOverTime.Text = Utilities.CommonUtil.IntToTime((int)tAttendance.OverTimeHours4, true);
                                }
                                else
                                {
                                    txtOverTime.Text = string.Empty;
                                }
                            }
                            else if (workingType == (int)WorkingType.WorkHoliDay)
                            {
                                if (tAttendance.SH_OverTimeHours4 != null)
                                {
                                    txtOverTime.Text = Utilities.CommonUtil.IntToTime((int)tAttendance.SH_OverTimeHours4, true);
                                }
                                else
                                {
                                    txtOverTime.Text = string.Empty;
                                }
                            }
                            else
                            {
                                if (tAttendance.LH_OverTimeHours4 != null)
                                {
                                    txtOverTime.Text = Utilities.CommonUtil.IntToTime((int)tAttendance.LH_OverTimeHours4, true);
                                }
                                else
                                {
                                    txtOverTime.Text = string.Empty;
                                }
                            }
                            break;
                        case 4:
                            if (workingType == (int)WorkingType.WorkFullTime)
                            {
                                if (tAttendance.OverTimeHours5 != null)
                                {
                                    txtOverTime.Text = Utilities.CommonUtil.IntToTime((int)tAttendance.OverTimeHours5, true);
                                }
                                else
                                {
                                    txtOverTime.Text = string.Empty;
                                }
                            }
                            else if (workingType == (int)WorkingType.WorkHoliDay)
                            {
                                if (tAttendance.SH_OverTimeHours5 != null)
                                {
                                    txtOverTime.Text = Utilities.CommonUtil.IntToTime((int)tAttendance.SH_OverTimeHours5, true);
                                }
                                else
                                {
                                    txtOverTime.Text = string.Empty;
                                }
                            }
                            else
                            {
                                if (tAttendance.LH_OverTimeHours5 != null)
                                {
                                    txtOverTime.Text = Utilities.CommonUtil.IntToTime((int)tAttendance.LH_OverTimeHours5, true);
                                }
                                else
                                {
                                    txtOverTime.Text = string.Empty;
                                }
                            }
                            break;
                    }
                }

                if (tAttendance.VacationFlag == (int)VacationFlag.AllHolidays)
                {
                    cmbAllHolidays.SelectedValue = tAttendance.VacationFullCD.ToString();
                    cmbBeforeHalfHoliday.SelectedValue = "-1";
                    cmbLateHoliday.SelectedValue = "-1";
                }
                else if (tAttendance.VacationFlag == (int)VacationFlag.MorningAndAfternoon)
                {
                    cmbBeforeHalfHoliday.SelectedValue = tAttendance.VacationMorningCD.ToString();
                    cmbLateHoliday.SelectedValue = tAttendance.VacationAfternoonCD.ToString();
                    cmbAllHolidays.SelectedValue = "-1";
                }
                else if (tAttendance.VacationFlag == (int)VacationFlag.Morning)
                {
                    cmbBeforeHalfHoliday.SelectedValue = tAttendance.VacationMorningCD.ToString();
                    cmbAllHolidays.SelectedValue = "-1";
                    cmbLateHoliday.SelectedValue = "-1";

                }
                else if (tAttendance.VacationFlag == (int)VacationFlag.Afternoon)
                {
                    cmbAllHolidays.SelectedValue = "-1";
                    cmbBeforeHalfHoliday.SelectedValue = "-1";
                    cmbLateHoliday.SelectedValue = tAttendance.VacationAfternoonCD.ToString();
                }
                else
                {
                    cmbAllHolidays.SelectedValue = "-1";
                    cmbBeforeHalfHoliday.SelectedValue = "-1";
                    cmbLateHoliday.SelectedValue = "-1";
                }

                if (tAttendance.TotalOverTimeHours != null)
                {
                    txtTheTotalOvertime.Text = Utilities.CommonUtil.IntToTime((int)tAttendance.TotalOverTimeHours, false);
                }
                else
                {
                    txtTheTotalOvertime.Text = string.Empty;
                }

                if (tAttendance.TotalWorkingHours != null)
                {
                    txtTheTotalWorkingHours.Text = Utilities.CommonUtil.IntToTime((int)tAttendance.TotalWorkingHours, false);
                }
                else
                {
                    txtTheTotalWorkingHours.Text = string.Empty;
                }

                if (tAttendance.Memo != null)
                {
                    txtMemo.Text = tAttendance.Memo;
                }
                else
                {
                    txtMemo.Text = string.Empty;
                }

                if (tAttendance.ApprovalNote != null)
                {
                    txtApprovalNote.Text = tAttendance.ApprovalNote;
                }
                else
                {
                    txtApprovalNote.Text = string.Empty;
                }

                if (tAttendance.RequestNote != null)
                {
                    txtRequestNote.Text = tAttendance.RequestNote;
                }
                else
                {
                    txtRequestNote.Text = string.Empty;
                }

                //Save UpdateDate
                this.OldUpdateDate = tAttendance.UpdateDate;

                this.OldT_Attendance = tAttendance;

                //Set viewState
                this.TAttendanceId = tAttendance.ID;

                if (isShowDetail)
                {
                    //Show detail Data
                    this.ShowDetailData(tAttendance.ID);
                }
            }
        }

        /// <summary>
        /// Show detail data on form
        /// </summary>
        /// <param name="headerID">Header ID</param>
        private void ShowDetailData(int HID)
        {
            IList<T_Work_D> ListTWorkD;
            M_Project mProject;
            using (DB db = new DB())
            {
                
                Work_DService work_DService = new Work_DService(db);
                ProjectService proService = new ProjectService(db);
                ListTWorkD = this.GetListWorkDById(HID, db);

                foreach (var item in ListTWorkD)
                {
                    mProject = proService.GetDataProjectById(item.PID);
                    item.ProjectCD = mProject.ProjectCD;
                    item.ProjectName = mProject.ProjectName;

                }
                
                if (ListTWorkD.Count == 0)
                {
                    //Get new list from screen
                    List<T_Work_D> listDetail = new List<T_Work_D>();
                    listDetail.Add(new T_Work_D());
                    rptDetail.DataSource = listDetail;
                    rptDetail.DataBind();
                }
                else
                {
                    rptDetail.DataSource = ListTWorkD;
                    rptDetail.DataBind();
                }
            }
        }

        /// <summary>
        /// Check input Request
        /// </summary>
        /// <returns>Valid:true, Invalid:false</returns>
        private bool CheckInputRequest()
        {
            if (this.txtRequestNote.IsEmpty)
            {
                base.SetMessage(this.txtApprovalNote.ID, M_Message.MSG_REQUIRE, "申請備考");
            }
            //Check error
            return !base.HaveError;
        }

        /// <summary>
        /// Check input
        /// </summary>
        /// <returns>Valid:true, Invalid:false</returns>
        private bool CheckInput()
        {
            int workingType;
            int checkProjectExist = 0;
            decimal vacationDays = 0;
            using (DB db = new DB())
            {
                WorkingSystemService workingSystemSer = new WorkingSystemService(db);
                workingType = workingSystemSer.GetDataWorkingSystemById(int.Parse(cmbWorkSchedule.SelectedValue)).WorkingType;

                T_PaidVacationService paidVacationSer = new T_PaidVacationService(db);
                vacationDays = paidVacationSer.GetTotalVacationDays(int.Parse(this.UID));
            }

            // check exist
            if (!((cmbAllHolidays.SelectedValue != "-1") ||
                ((cmbAllHolidays.SelectedValue == "-1") &&
                (cmbBeforeHalfHoliday.SelectedValue != "-1" && cmbLateHoliday.SelectedValue != "-1"))))
            {
                if (this.txtEntryTime.IsEmpty)
                {
                    base.SetMessage(this.txtEntryTime.ID, M_Message.MSG_REQUIRE, "出勤");
                }
                if (this.txtExitTime.IsEmpty)
                {
                    base.SetMessage(this.txtExitTime.ID, M_Message.MSG_REQUIRE, "退出");
                }
            }
            else
            {
                if (!(this.txtEntryTime.IsEmpty && this.txtExitTime.IsEmpty))
                {
                    if (this.txtEntryTime.IsEmpty)
                    {
                        base.SetMessage(this.txtEntryTime.ID, M_Message.MSG_REQUIRE, "出勤");
                    }
                    if (this.txtExitTime.IsEmpty)
                    {
                        base.SetMessage(this.txtExitTime.ID, M_Message.MSG_REQUIRE, "退出");
                    }
                }
            }

            if (txtEntryTime.Text != string.Empty && txtExitTime.Text != string.Empty)
            {
                int entryTime = Utilities.CommonUtil.TimeToInt(txtEntryTime.Text);
                int exitTime = Utilities.CommonUtil.TimeToInt(txtExitTime.Text);

                if (exitTime <= entryTime)
                {
                    base.SetMessage(this.txtEntryTime.ID, M_Message.MSG_LESS_THAN, "出勤", "退出");
                }
            }

            //********************** Check vacation days **************
            if (this.Mode == Utilities.Mode.Update)
            {
                using (DB db = new DB())
                {
                    AttendanceService attSer = new AttendanceService(db);
                    if (!this.chkExchangeFlag.Checked && attSer.ExchangeDateIsUsed(int.Parse(this.UID), DateTime.Parse(this.dateViewState), DateTime.Parse(this.dateViewState)))
                    {
                        base.SetMessage(this.cmbExchangeDate.ID, M_Message.MSG_DATE_IS_EXCHANGED);
                    }
                }
            }

            if (int.Parse(this.cmbAllHolidays.SelectedValue) == (int)Vacation.Transfer
                || int.Parse(this.cmbBeforeHalfHoliday.SelectedValue) == (int)Vacation.Transfer
                || int.Parse(this.cmbLateHoliday.SelectedValue) == (int)Vacation.Transfer)
            {
                if (this.cmbExchangeDate.SelectedValue == "-1")
                {
                    base.SetMessage(this.cmbExchangeDate.ID, M_Message.MSG_REQUIRE, "振休対象日");
                }
                else
                {
                    using (DB db = new DB())
                    {
                        
                        AttendanceService attSer = new AttendanceService(db);
                        T_Attendance exchangeModel = attSer.GetAttendanceByUidAndDate(int.Parse(this.UID), this.cmbExchangeDate.SelectedValue);
                        if (exchangeModel == null || exchangeModel.ExchangeFlag != 1)
                        {
                            base.SetMessage(this.cmbExchangeDate.ID, M_Message.MSG_ISNOT_EXCHANGE_DATE);
                        }
                        else if (attSer.ExchangeDateIsUsed(int.Parse(this.UID), DateTime.Parse(this.dateViewState), DateTime.Parse(this.cmbExchangeDate.SelectedValue)))
                        {
                            base.SetMessage(this.cmbExchangeDate.ID, M_Message.MSG_EXCHANGE_DATE_IS_USED);
                        }

                    }
                }
            }

            decimal useVacationDays = 0;
            if (this.Mode == Utilities.Mode.Update)
            {
                T_Attendance oldData = this.GetHeaderData(this.TAttendanceId);
                useVacationDays = CalcUseVacationDays(oldData, null);
            }

            string cmbControlID = string.Empty;
            if (int.Parse(this.cmbLateHoliday.SelectedValue) == (int)Vacation.AnnualPaid)
            {
                useVacationDays += new decimal(0.6);
                cmbControlID = this.cmbLateHoliday.ID;
            }

            if (int.Parse(this.cmbBeforeHalfHoliday.SelectedValue) == (int)Vacation.AnnualPaid)
            {
                useVacationDays += new decimal(0.4);
                cmbControlID = this.cmbBeforeHalfHoliday.ID;
            }

            if (int.Parse(this.cmbAllHolidays.SelectedValue) == (int)Vacation.AnnualPaid)
            {
                useVacationDays += 1;
                cmbControlID = this.cmbAllHolidays.ID;
            }

            if (useVacationDays > vacationDays)
            {
                base.SetMessage(cmbControlID, M_Message.MSG_NOT_ENOUGH_VACATION_DAYS);
            }

            //*********************************************************

            int startTime = 0;
            int endTime = 0;
            HtmlGenericControl divValue1 = new HtmlGenericControl();
            foreach (RepeaterItem item in this.rptDetail.Items)
            {
                
                if (this.IsEmptyRow(item))
                {
                    continue;
                }

                int rowIndex = item.ItemIndex + 1;
                
                //txtProjectCD
                ICodeTextBox txtProjectCD = (ICodeTextBox)item.FindControl("txtProjectCD");

                //txtStartTime
                ITimeTextBox txtStartTime = (ITimeTextBox)item.FindControl("txtStartTime");

                //txtEndTime
                ITimeTextBox txtEndtime = (ITimeTextBox)item.FindControl("txtEndTime");

                //txtWorkTime
                ITimeTextBox txtWorkTime = (ITimeTextBox)item.FindControl("txtWorkTime");

                //arrCheckTime = 
                if (!string.IsNullOrEmpty(txtProjectCD.Text))
                {
                    using (DB db = new DB())
                    {
                        M_Project mProject;
                        ProjectService proService = new ProjectService(db);
                        mProject = proService.GetByProjectCd(txtProjectCD.Text);
                        HtmlGenericControl divValue = (HtmlGenericControl)item.FindControl("divProjectCD");
                        divValue.Attributes.Remove("class");
                        if (mProject == null || mProject.StatusFlag != 0)
                        {
                            string errorId = txtProjectCD.ID + "_" + item.ItemIndex.ToString();
                            base.SetMessage(errorId, M_Message.MSG_NOT_EXIST_CODE_GRID, "プロジェクトコード", rowIndex);
                            this.AddErrorForListItem(divValue, errorId);
                        }
                        else
                        {
                            checkProjectExist = checkProjectExist + 1;
                        }
                    }
                    if (txtStartTime != null)
                    {
                        HtmlGenericControl divValueStartTime = (HtmlGenericControl)item.FindControl("divStartTime");
                        divValueStartTime.Attributes.Remove("class");
                        string errorStartTimeId = txtStartTime.ID + "_" + item.ItemIndex.ToString();
                        if (string.IsNullOrEmpty(txtStartTime.Value))
                        {
                            base.SetMessage(errorStartTimeId, M_Message.MSG_REQUIRE_GRID, "開始時間", rowIndex);
                            this.AddErrorForListItem(divValueStartTime, errorStartTimeId);
                        }
                        else
                        {
                            txtStartTime.Value = (txtStartTime.Value).ToString();
                        }
                    }

                    if (txtEndtime != null)
                    {
                        HtmlGenericControl divValueEndTime = (HtmlGenericControl)item.FindControl("divEndTime");
                        divValueEndTime.Attributes.Remove("class");
                        string errorEndTimeId = txtEndtime.ID + "_" + item.ItemIndex.ToString();
                        if (string.IsNullOrEmpty(txtEndtime.Value))
                        {
                            base.SetMessage(errorEndTimeId, M_Message.MSG_REQUIRE_GRID, "終了時間", rowIndex);
                            this.AddErrorForListItem(divValueEndTime, errorEndTimeId);
                        }
                        else
                        {
                            txtEndtime.Value = (txtEndtime.Value).ToString();
                        }
                    }

                    if (txtWorkTime != null)
                    {
                        HtmlGenericControl divWorkTimeValue = (HtmlGenericControl)item.FindControl("divWorkTime");
                        divWorkTimeValue.Attributes.Remove("class");
                        string errorWorkTimeId = txtWorkTime.ID + "_" + item.ItemIndex.ToString();
                        if (string.IsNullOrEmpty(txtWorkTime.Value) || (txtWorkTime.Value == HOURS_EMPTY))
                        {
                            base.SetMessage(errorWorkTimeId, M_Message.MSG_REQUIRE_GRID, "作業時間", rowIndex);
                            this.AddErrorForListItem(divWorkTimeValue, errorWorkTimeId);
                        }
                        else
                        {
                            txtWorkTime.Value = (txtWorkTime.Value).ToString();
                        }
                    }
                }
                else
                {
                    if (txtProjectCD != null)
                    {
                        HtmlGenericControl divValue = (HtmlGenericControl)item.FindControl("divProjectCD");
                        divValue.Attributes.Remove("class");
                        string errorId = txtProjectCD.ID + "_" + item.ItemIndex.ToString();
                        if (string.IsNullOrEmpty(txtProjectCD.Value))
                        {
                            base.SetMessage(errorId, M_Message.MSG_REQUIRE_GRID, "プロジェクトコード", rowIndex);
                            this.AddErrorForListItem(divValue, errorId);
                        }
                        else
                        {
                            txtProjectCD.Value = (txtProjectCD.Value).ToString();
                        }
                    }

                    HtmlGenericControl divValueStartTime = (HtmlGenericControl)item.FindControl("divStartTime");
                    divValueStartTime.Attributes.Remove("class");
                    string errorStartTimeId = txtStartTime.ID + "_" + item.ItemIndex.ToString();

                    HtmlGenericControl divValueEndTime = (HtmlGenericControl)item.FindControl("divEndTime");
                    divValueEndTime.Attributes.Remove("class");
                    string errorEndTimeId = txtEndtime.ID + "_" + item.ItemIndex.ToString();

                    HtmlGenericControl divWorkTimeValue = (HtmlGenericControl)item.FindControl("divWorkTime");
                    divWorkTimeValue.Attributes.Remove("class");
                    string errorWorkTimeId = txtWorkTime.ID + "_" + item.ItemIndex.ToString();

                    if (txtStartTime.Text != string.Empty || txtEndtime.Text != string.Empty || txtWorkTime.Text != string.Empty)
                    {
                        if (txtStartTime.Text != string.Empty)
                        {
                            if (txtEndtime != null)
                            {
                                if (string.IsNullOrEmpty(txtEndtime.Value))
                                {
                                    base.SetMessage(errorEndTimeId, M_Message.MSG_REQUIRE_GRID, "終了時間", rowIndex);
                                    this.AddErrorForListItem(divValueEndTime, errorEndTimeId);
                                }
                                else
                                {
                                    txtEndtime.Value = (txtEndtime.Value).ToString();
                                }
                            }

                            if (txtWorkTime != null)
                            {
                                if (string.IsNullOrEmpty(txtWorkTime.Value))
                                {
                                    base.SetMessage(errorWorkTimeId, M_Message.MSG_REQUIRE_GRID, "作業時間", rowIndex);
                                    this.AddErrorForListItem(divWorkTimeValue, errorWorkTimeId);
                                }
                                else
                                {
                                    txtWorkTime.Value = (txtWorkTime.Value).ToString();
                                }
                            }
                        }
                        if (txtEndtime.Text != string.Empty)
                        {
                            if (txtStartTime != null)
                            {
                                if (string.IsNullOrEmpty(txtStartTime.Value))
                                {
                                    base.SetMessage(errorStartTimeId, M_Message.MSG_REQUIRE_GRID, "終了時間", rowIndex);
                                    this.AddErrorForListItem(divValueStartTime, errorStartTimeId);
                                }
                                else
                                {
                                    txtEndtime.Value = (txtEndtime.Value).ToString();
                                }
                            }
                            else
                            {
                                if (txtWorkTime != null)
                                {
                                    if (string.IsNullOrEmpty(txtWorkTime.Value))
                                    {
                                        base.SetMessage(errorWorkTimeId, M_Message.MSG_REQUIRE_GRID, "作業時間", rowIndex);
                                        this.AddErrorForListItem(divWorkTimeValue, errorWorkTimeId);
                                    }
                                    else
                                    {
                                        txtWorkTime.Value = (txtWorkTime.Value).ToString();
                                    }
                                }
                            }
                        }

                        if (txtWorkTime.Text != string.Empty)
                        {
                            if (txtStartTime != null)
                            {
                                if (string.IsNullOrEmpty(txtEndtime.Value))
                                {
                                    base.SetMessage(errorEndTimeId, M_Message.MSG_REQUIRE_GRID, "終了時間", rowIndex);
                                    this.AddErrorForListItem(divValueStartTime, errorEndTimeId);
                                }
                                else
                                {
                                    txtEndtime.Value = (txtEndtime.Value).ToString();
                                }
                            }

                            if (txtEndtime != null)
                            {
                                if (string.IsNullOrEmpty(txtEndtime.Value))
                                {
                                    base.SetMessage(errorEndTimeId, M_Message.MSG_REQUIRE_GRID, "終了時間", rowIndex);
                                    this.AddErrorForListItem(divValueEndTime, errorEndTimeId);
                                }
                                else
                                {
                                    txtEndtime.Value = (txtEndtime.Value).ToString();
                                }
                            }
                        }
                        if (txtStartTime.Text != string.Empty && txtEndtime.Text != string.Empty)
                        {
                            startTime = Utilities.CommonUtil.TimeToInt(txtStartTime.Value);
                            endTime = Utilities.CommonUtil.TimeToInt(txtEndtime.Value);

                            if (startTime > endTime)
                            {
                                HtmlGenericControl divValue = (HtmlGenericControl)item.FindControl("divStartTime");
                                divValue.Attributes.Remove("class");
                                string errorId = txtStartTime.ID + "_" + item.ItemIndex.ToString();
                                base.SetMessage(errorId, M_Message.MSG_LESS_THAN_GRID, "開始時間", "終了時間", rowIndex);
                                this.AddErrorForListItem(divValue, errorId);
                            }
                        }
                    }
                }

                // check startime > endtime
                if (txtStartTime.Text != string.Empty && txtEndtime.Text != string.Empty)
                {
                    startTime = Utilities.CommonUtil.TimeToInt(txtStartTime.Value);
                    endTime = Utilities.CommonUtil.TimeToInt(txtEndtime.Value);

                    if (startTime >= endTime)
                    {
                        HtmlGenericControl divValue = (HtmlGenericControl)item.FindControl("divStartTime");
                        divValue.Attributes.Remove("class");
                        string errorId = txtStartTime.ID + "_" + item.ItemIndex.ToString();
                        base.SetMessage(errorId, M_Message.MSG_LESS_THAN_GRID, "開始時間", "終了時間", rowIndex);
                        this.AddErrorForListItem(divValue, errorId);
                    }
                }

                //if (txtWorkTime != null)
                //{
                //    if (txtWorkTime.Text != string.Empty)
                //    {
                //        this.totalWorkingHours = totalWorkingHours + Utilities.CommonUtil.TimeToInt(txtWorkTime.Text);
                //    }
                //}

                //if (rowIndex == 1)
                //{
                //    divValue1 = (HtmlGenericControl)item.FindControl("divWorkTime");
                //}
                
                //if (rowIndex == rptDetail.Items.Count)
                //{
                //    if (checkProjectExist != 0)
                //    {
                //        if (txtTheTotalWorkingHours.Text == string.Empty)
                //        {
                //            totalWorkingHoursHeader = 0;
                //        }
                //        else
                //        {
                //            totalWorkingHoursHeader = Utilities.CommonUtil.TimeToInt(txtTheTotalWorkingHours.Text);
                //        }

                //        if (this.totalWorkingHours > totalWorkingHoursHeader)
                //        {
                //            divValue1.Attributes.Remove("class");
                //            string errorId = txtWorkTime.ID + "_" + "0";
                //            base.SetMessage(errorId, M_Message.MSG_LESS_THAN_EQUAL, "総作業時間", "総労働時間", 1);
                //            this.AddErrorForListItem(divValue1, errorId);
                //        }
                //    }
                //}
            }
            
            //Check error
            return !base.HaveError;
        }

        private bool ExchangeDateConfirmCheck(DB db)
        {
            AttendanceService attSer = new AttendanceService(db);


            if (this.cmbExchangeDate.SelectedValue != "-1")
            {
                T_Attendance exchangeModel = attSer.GetAttendanceByUidAndDate(int.Parse(this.UID), this.cmbExchangeDate.SelectedValue);
                if (exchangeModel == null || exchangeModel.ExchangeFlag != 1)
                {
                    base.SetMessage(this.cmbExchangeDate.ID, M_Message.MSG_ISNOT_EXCHANGE_DATE);
                    return false;
                }
                else if (attSer.ExchangeDateIsUsed(int.Parse(this.UID), DateTime.Parse(this.dateViewState), DateTime.Parse(this.cmbExchangeDate.SelectedValue)))
                {
                    base.SetMessage(this.cmbExchangeDate.ID, M_Message.MSG_EXCHANGE_DATE_IS_USED);
                    return false;
                }
            }

            if (this.Mode == Utilities.Mode.Update || this.Mode == Utilities.Mode.Delete)
            {
                if (!this.chkExchangeFlag.Checked && attSer.ExchangeDateIsUsed(int.Parse(this.UID), DateTime.Parse(this.dateViewState), DateTime.Parse(this.dateViewState)))
                {
                    base.SetMessage(this.cmbExchangeDate.ID, M_Message.MSG_DATE_IS_EXCHANGED);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Add display error for control
        /// </summary>
        /// <param name="divCtrl">div error control</param>
        /// <param name="errorKey">Error Control ID</param>
        private void AddErrorForListItem(HtmlGenericControl divCtrl, string errorKey)
        {
            divCtrl.Attributes.Add("class", "form-group " + base.GetClassError(errorKey));
        }

        /// <summary>
        /// Get class name for div of control
        /// </summary>
        /// <param name="option">option</param>
        /// <returns>class name</returns>
        protected virtual string GetApprovalStatus(string option)
        {
            if (this.Mode == Mode.Insert)
            {
                return "display: none";
            }
            string rs = string.Empty;
            T_Attendance attendance;
            using (DB db = new DB())
            {
                AttendanceService attendance_sevice = new AttendanceService(db);
                attendance = attendance_sevice.GetDataAttendanceById(TAttendanceId);
            }
            if (option.Equals("class"))
            {
                if (attendance.ApprovalStatus == AttendanceApprovalStatus.NeedApproval.GetHashCode())
                {
                    rs = "label label-primary";
                }
                else if (attendance.ApprovalStatus == AttendanceApprovalStatus.Request.GetHashCode())
                {
                    rs = "label label-info";
                }
                else if (attendance.ApprovalStatus == AttendanceApprovalStatus.Approved.GetHashCode())
                {
                    rs = "label label-success";
                }
                else if (attendance.ApprovalStatus == AttendanceApprovalStatus.Cancel.GetHashCode())
                {
                    rs = "label label-danger";
                }
            }
            else if (option.Equals("text"))
            {

                if (attendance.ApprovalStatus == AttendanceApprovalStatus.NeedApproval.GetHashCode())
                {
                    rs = AttendanceApprovalStatus.NeedApproval.GetDescription();
                }
                else if (attendance.ApprovalStatus == AttendanceApprovalStatus.Request.GetHashCode())
                {
                    rs = AttendanceApprovalStatus.Request.GetDescription(); ;
                }
                else if (attendance.ApprovalStatus == AttendanceApprovalStatus.Approved.GetHashCode())
                {
                    rs = AttendanceApprovalStatus.Approved.GetDescription(); ;
                }
                else if (attendance.ApprovalStatus == AttendanceApprovalStatus.Cancel.GetHashCode())
                {
                    rs = AttendanceApprovalStatus.Cancel.GetDescription(); ;
                }
            }
            else
            {
                if (attendance.ApprovalStatus == AttendanceApprovalStatus.None.GetHashCode())
                {
                    rs = "display: none";
                }
            }

            return rs;
        }

        /// <summary>
        /// Get T_Attendance BY ID
        /// </summary>
        /// <param name="ID">T_Attendance ID</param>
        /// <returns>T_Attendance</returns>
        private T_Attendance GetTAttendanceById(int Id, DB db)
        {
            AttendanceService attendanceService = new AttendanceService(db);

            //Get WorkingCalendar
            return attendanceService.GetDataAttendanceById(Id);
        }

        /// <summary>
        /// Get T_Work_H BY ID
        /// </summary>
        /// <param name="workHID">T_work_H ID</param>
        /// <returns>T_Work_H</returns>
        private T_Work_H GetWorkHById(int workHID, DB db)
        {
            Work_HService work_HService = new Work_HService(db);

            //Get WorkingCalendar
            return work_HService.GetByHID(workHID);
        }

        /// <summary>
        /// Get T_Work_D BY ID
        /// </summary>
        /// <param name="tWorkD_ID">T_work_D ID</param>
        /// <returns>T_Work_D</returns>
        private IList<T_Work_D> GetListWorkDById(int workDId,DB db)
        {
            Work_DService work_DService = new Work_DService(db);

            //Get WorkingCalendar
            return work_DService.GetListByHID(workDId);
        }

        /// <summary>
        /// Get ProjectId ProjectCd
        /// </summary>
        /// <param name="ProjectCd">ProjectCd</param>
        /// <returns>ProjectId</returns>
        private int GetProjectIdByCd(string projectCd,DB db)
        {
            M_Project mProject = new M_Project();
            ProjectService proService = new ProjectService(db);
            mProject = proService.GetByProjectCd(projectCd);
            if (mProject != null)
            {
                return mProject.ID;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Get Project Name By Project Code
        /// </summary>
        /// <param name="groupCd">Project Code</param>
        /// <returns>Project Name</returns>
        [System.Web.Services.WebMethod]
        public static string GetProjectName(string projectCD)
        {
            System.Text.StringBuilder result = new System.Text.StringBuilder();
            string projectName = string.Empty;
            string workPlace = string.Empty;

            var projectCd = projectCD;

            try
            {
                using (DB db = new DB())
                {
                    ProjectService proSer = new ProjectService(db);
                    M_Project model = proSer.GetByProjectCd(projectCD);
                    if (model != null && model.StatusFlag != 1)
                    {
                        projectName = model.ProjectName;
                        workPlace = model.WorkPlace;
                    }
                }
                result.Append("{");
                result.Append(string.Format("\"projectCd\":\"{0}\"", projectCd));
                result.Append(string.Format(", \"ProjectName\":\"{0}\"", projectName));
                result.Append(string.Format(", \"WorkPlace\":\"{0}\"", workPlace));
                result.Append("}");
                return result.ToString();
            }
            catch (Exception ex)
            {
                Log.Instance.WriteLog(ex);
                return null;
            }
        }

        /// <summary>
        /// GetValueByWorkingSystemId
        /// </summary>
        /// <param name="workingSystemId"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string GetValueByWorkingSystemId(int workingSystemId)
        {
            try
            {
                System.Text.StringBuilder result = new System.Text.StringBuilder();

                M_WorkingSystem mWorkingSystem;

                using (DB db = new DB())
                {
                    WorkingSystemService workingSystemService = new WorkingSystemService(db);
                    mWorkingSystem = workingSystemService.GetDataWorkingSystemById(workingSystemId);

                    // caculate Working System
                    int workingType = mWorkingSystem.WorkingType;
                    string workingCd = mWorkingSystem.WorkingSystemCD;
                    int strWorkingStart = 0;
                    int strWorkingEnd = 0;
                    if (mWorkingSystem.Working_Start != null)
                    {
                        strWorkingStart = Utilities.CommonUtil.TimeToMinute(Utilities.CommonUtil.IntToTime((int)mWorkingSystem.Working_Start, true));
                        strWorkingEnd = Utilities.CommonUtil.TimeToMinute(Utilities.CommonUtil.IntToTime((int)mWorkingSystem.Working_End, true));
                    }
                    result.Append("{");

                    result.Append(string.Format(" \"WorkingStart\":\"{0}\"", strWorkingStart));
                    result.Append(string.Format(", \"WorkingEnd\":\"{0}\"", strWorkingEnd));
                    result.Append(string.Format(", \"WorkingType\":\"{0}\"", workingType));
                    result.Append(string.Format(", \"WorkingCd\":\"{0}\"", workingCd));

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

        /// <summary>
        /// GetValueFirstHour_LatterStart 
        /// </summary>
        /// <param name="workingSystemId"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string GetValueFirstHour_LatterStart(int workingSystemId, int beforeHalfHoliday, int lateHoliday)
        {
            try
            {
                System.Text.StringBuilder result = new System.Text.StringBuilder();

                M_WorkingSystem mWorkingSystem;

                using (DB db = new DB())
                {
                    WorkingSystemService workingSystemService = new WorkingSystemService(db);
                    mWorkingSystem = workingSystemService.GetDataWorkingSystemById(workingSystemId);

                    // Caculate FirstMoring
                    int workingType = mWorkingSystem.WorkingType;
                    string workingCd = mWorkingSystem.WorkingSystemCD;
                    string strFirstHourMorning = string.Empty;
                    string strLatterStart = string.Empty;
                    if (mWorkingSystem.First_End != null)
                    {
                        if (beforeHalfHoliday != -1)
                        {
                            strFirstHourMorning = Utilities.CommonUtil.IntToTime((int)mWorkingSystem.Latter_Start, true);
                        }
                        else
                        {
                            strFirstHourMorning = Utilities.CommonUtil.IntToTime((int)mWorkingSystem.Working_Start, true);
                        }
                        if (lateHoliday != -1)
                        {
                            strLatterStart = Utilities.CommonUtil.IntToTime((int)mWorkingSystem.First_End, true);
                        }
                        else
                        {
                            strLatterStart = Utilities.CommonUtil.IntToTime((int)mWorkingSystem.Working_End, true);
                        }
                    }
                    result.Append("{");
                    result.Append(string.Format(" \"WorkingStart\":\"{0}\"", strFirstHourMorning));
                    result.Append(string.Format(", \"WorkEnd\":\"{0}\"", strLatterStart));
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

        ///// <summary>
        ///// GetWorkingHourByVacation 
        ///// </summary>
        ///// <param name="workingSystemId"></param>
        ///// <returns></returns>
        //[System.Web.Services.WebMethod]
        //public static string GetWorkingHourByVacation(int workingSystemId,int offMorning,int offAfternoon)
        //{
        //    try
        //    {
        //        System.Text.StringBuilder result = new System.Text.StringBuilder();
        //        M_WorkingSystem mWorkingSystem;

        //        using (DB db = new DB())
        //        {
        //            WorkingSystemService workingSystemService = new WorkingSystemService(db);
        //            mWorkingSystem = workingSystemService.GetDataWorkingSystemById(workingSystemId);

        //            TimeSpan workingHours = new TimeSpan(0, 0, 0);
        //            DateTime[] arrBreakTime = new DateTime[8];
        //            DateTime[] arrOverTime = new DateTime[10];

        //            DateTime systemST = new DateTime(1, 1, 1);
        //            DateTime systemET = new DateTime(1, 1, 1);

        //            TimeSpan timeHoursAnnualPaid = new TimeSpan(8, 0, 0);
        //            TimeSpan timeBreakAnnualPaid = new TimeSpan(1, 0, 0);
        //            string[] First_End = new string[2];
        //            string[] Latter_Start = new string[2];

        //            string[] systemWorkingStart = new string[2];
        //            string[] systemWorkingEnd = new string[2];

        //            if (mWorkingSystem.Working_Start != null)
        //            {
        //                systemWorkingStart = (Utilities.CommonUtil.IntToTime((int)mWorkingSystem.Working_Start, true)).Split(':');
        //                systemWorkingEnd = (Utilities.CommonUtil.IntToTime((int)mWorkingSystem.Working_End, true)).Split(':');
        //            }
        //            else
        //            {
        //                systemWorkingStart[0] = "0";
        //                systemWorkingStart[1] = "0";
        //                systemWorkingEnd[0] = "0";
        //                systemWorkingEnd[1] = "0";
        //            }

        //            if (mWorkingSystem.WorkingType == (int)WorkingType.WorkFullTime)
        //            {
        //                First_End = (Utilities.CommonUtil.IntToTime((int)mWorkingSystem.First_End, true)).Split(':');
        //                Latter_Start = (Utilities.CommonUtil.IntToTime((int)mWorkingSystem.Latter_Start, true)).Split(':');

        //                if (offMorning != 0 && offAfternoon == 0)
        //                {
        //                    systemST = new DateTime(1, 1, 1).AddHours(int.Parse(Latter_Start[0])).AddMinutes(int.Parse(Latter_Start[1]));
        //                    systemET = new DateTime(1, 1, 1).AddHours(int.Parse(systemWorkingEnd[0])).AddMinutes(int.Parse(systemWorkingEnd[1]));
        //                }
        //                else if (offMorning == 0 && offAfternoon != 0)
        //                {
        //                    systemST = new DateTime(1, 1, 1).AddHours(int.Parse(systemWorkingStart[0])).AddMinutes(int.Parse(systemWorkingStart[1]));
        //                    systemET = new DateTime(1, 1, 1).AddHours(int.Parse(First_End[0])).AddMinutes(int.Parse(First_End[1]));
        //                }
        //                else
        //                {
        //                    systemST = new DateTime(1, 1, 1).AddHours(int.Parse(systemWorkingStart[0])).AddMinutes(int.Parse(systemWorkingStart[1]));
        //                    systemET = new DateTime(1, 1, 1).AddHours(int.Parse(systemWorkingEnd[0])).AddMinutes(int.Parse(systemWorkingEnd[1]));
        //                }
        //            }

        //            if (mWorkingSystem.WorkingType == (int)WorkingType.WorkFullTime)
        //            {

        //                //get breakTime
        //                if (mWorkingSystem.Break1_Start != null)
        //                {
        //                    arrBreakTime[0] = ConvertIntToDateTime((int)mWorkingSystem.Break1_Start);
        //                }
        //                if (mWorkingSystem.Break1_End != null)
        //                {
        //                    arrBreakTime[1] = ConvertIntToDateTime((int)mWorkingSystem.Break1_End);
        //                }
        //                if (mWorkingSystem.Break2_Start != null)
        //                {
        //                    arrBreakTime[2] = ConvertIntToDateTime((int)mWorkingSystem.Break2_Start);
        //                }
        //                if (mWorkingSystem.Break2_End != null)
        //                {
        //                    arrBreakTime[3] = ConvertIntToDateTime((int)mWorkingSystem.Break2_End);
        //                }
        //                if (mWorkingSystem.Break3_Start != null)
        //                {
        //                    arrBreakTime[4] = ConvertIntToDateTime((int)mWorkingSystem.Break3_Start);
        //                }
        //                if (mWorkingSystem.Break3_End != null)
        //                {
        //                    arrBreakTime[5] = ConvertIntToDateTime((int)mWorkingSystem.Break3_End);
        //                }
        //                if (mWorkingSystem.Break4_Start != null)
        //                {
        //                    arrBreakTime[6] = ConvertIntToDateTime((int)mWorkingSystem.Break4_Start);
        //                }
        //                if (mWorkingSystem.Break4_End != null)
        //                {
        //                    arrBreakTime[7] = ConvertIntToDateTime((int)mWorkingSystem.Break4_End);
        //                }

        //                if (mWorkingSystem.BreakType == 0)
        //                {
        //                    //Working time
        //                    workingHours = CalDurationWorkTime(systemST, systemET, arrBreakTime);
        //                }
        //                else if (mWorkingSystem.BreakType == 1)
        //                {
        //                    TimeSpan BreakTimeHour = new TimeSpan();

        //                    // BreakTime Hours
        //                    if (arrBreakTime[0] != null)
        //                    {
        //                        BreakTimeHour = new TimeSpan(arrBreakTime[0].Hour, arrBreakTime[0].Minute, 0);
        //                    }

        //                    //Working time
        //                    workingHours = CalDurationWorkTime(systemST, systemET, arrBreakTime).Subtract(BreakTimeHour);

        //                }
        //                else if (mWorkingSystem.BreakType == 2)
        //                {

        //                    DateTime[] arrBreakTime_Type2 = new DateTime[40];
        //                    TimeSpan TimeHourToBreak = new TimeSpan();
        //                    TimeSpan TimeBreak = new TimeSpan();
        //                    DateTime sTime = new DateTime(1, 1, 1).AddHours(systemST.Hour).AddMinutes(systemST.Minute);
        //                    DateTime eTime = new DateTime(1, 1, 1).AddHours(24);
        //                    if (systemST.Date == systemET.Date)
        //                    {
        //                        eTime = new DateTime(1, 1, 1).AddHours(systemET.Hour).AddMinutes(systemET.Minute);
        //                    }
        //                    else
        //                    {
        //                        eTime = new DateTime(1, 1, 2).AddHours(systemET.Hour).AddMinutes(systemET.Minute);
        //                    }

        //                    // BreakTime Hours
        //                    if (mWorkingSystem.Break1_Start == null || mWorkingSystem.Break1_End == null)
        //                    {
        //                        workingHours = eTime.Subtract(sTime);
        //                    }
        //                    else
        //                    {
        //                        TimeHourToBreak = new TimeSpan(arrBreakTime[0].Hour, arrBreakTime[0].Minute, 0);
        //                        TimeBreak = new TimeSpan(arrBreakTime[1].Hour, arrBreakTime[1].Minute, 0);

        //                        if (eTime.Subtract(sTime) > TimeHourToBreak)
        //                        {
        //                            arrBreakTime_Type2[0] = sTime.Add(TimeHourToBreak);
        //                            arrBreakTime_Type2[1] = sTime.Add(TimeHourToBreak).Add(TimeBreak);
        //                            sTime = sTime.Add(TimeHourToBreak).Add(TimeBreak);

        //                            int i = 2;
        //                            while (eTime > sTime)
        //                            {
        //                                arrBreakTime_Type2[i] = sTime.Add(TimeHourToBreak);
        //                                arrBreakTime_Type2[i + 1] = sTime.Add(TimeHourToBreak).Add(TimeBreak);
        //                                if (arrBreakTime_Type2[i + 1] > eTime)
        //                                {
        //                                    arrBreakTime_Type2[i] = new DateTime();
        //                                    arrBreakTime_Type2[i + 1] = new DateTime();
        //                                    break;
        //                                }
        //                                else
        //                                {
        //                                    sTime = sTime.Add(TimeHourToBreak).Add(TimeBreak);
        //                                    i = i + 2;
        //                                }
        //                            }
        //                        }
        //                        //Working time
        //                        workingHours = CalDurationWorkTime(systemST, systemET, arrBreakTime_Type2);
        //                    }

        //                    string strWorkingStart = string.Empty;
        //                    string strWorkingEnd = string.Empty;
        //                    if (mWorkingSystem.Working_Start != null)
        //                    {
        //                        strWorkingStart = Utilities.CommonUtil.IntToTime((int)mWorkingSystem.Working_Start, false);
        //                        strWorkingEnd = Utilities.CommonUtil.IntToTime((int)mWorkingSystem.Working_End, false);
        //                    }
        //                }
        //                result.Append("{");
        //                result.Append(string.Format(" \"WorkingHours\":\"{0}\"", FormatTimeResult(workingHours)));
        //                result.Append(string.Format(", \"TotalWorkingHours\":\"{0}\"", FormatTimeResult(workingHours)));
        //                result.Append("}");
        //            }
        //        }
        //        return result.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Instance.WriteLog(ex);
        //        return null;
        //    }
        //}

        /// <summary>
        /// GetValueTableClosingTime
        /// </summary>
        /// <param name="workingSystemId"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string GetValueTableClosingTime(int workingSystemId, string entryTimeValue, string exitTimeValue, int offAllDay, int offMorning,int offAfternoon)
        {
            System.Text.StringBuilder result = new System.Text.StringBuilder();

            if ((string.IsNullOrEmpty(entryTimeValue) || string.IsNullOrEmpty(exitTimeValue))
                || (Utilities.CommonUtil.TimeToInt(exitTimeValue) < Utilities.CommonUtil.TimeToInt(entryTimeValue)))
            {
                result.Append("{");
                result.Append(string.Format("\"WorkingHours\":\"{0}\"", ""));
                result.Append(string.Format(", \"LateHours\":\"{0}\"", ""));
                result.Append(string.Format(", \"EarlyHours\":\"{0}\"", ""));
                result.Append(string.Format(", \"OverTimeEarly\":\"{0}\"", ""));
                result.Append(string.Format(", \"OverTimeLate\":\"{0}\"", ""));
                result.Append("}");

                return result.ToString();
            }

            try
            {
                int breakType;
                int workingType;
                //bool check_AnnualPaidMorning = false;
                //bool check_AnnualPaidAfternoon = false;
                int[] entryTime = new int[2];
                int[] exitTime = new int[2];
                
                string[] arrEntryTime = entryTimeValue.Split(':');
                string[] arryExitTime = exitTimeValue.Split(':');
                string entryTimeHourValue = arrEntryTime[0];
                string entryTimeMinuteValue = arrEntryTime[1];
                string exitTimeHourValue = arryExitTime[0];
                string exitTimeMinuteValue = arryExitTime[1];

                DateTime[] arrBreakTime = new DateTime[10];
                DateTime[] arrOverTime = new DateTime[10];

                TimeSpan atWork = new TimeSpan();
                TimeSpan beLate = new TimeSpan();
                TimeSpan leaveEarly = new TimeSpan();
                DateTime workOTBeforeST = new DateTime(1, 1, 1);
                DateTime workOTBeforeET = new DateTime(1, 1, 1);
                DateTime workSTWork = new DateTime(1, 1, 1);
                DateTime workETWork = new DateTime(1, 1, 1);
                DateTime workOTAfterST = new DateTime(1, 1, 1);
                DateTime workOTAfterET = new DateTime(1, 1, 1);

                TimeSpan earlyOT = new TimeSpan();
                TimeSpan normalOT = new TimeSpan();
                TimeSpan lateNightOT = new TimeSpan();
                TimeSpan OT_04 = new TimeSpan();
                TimeSpan OT_05 = new TimeSpan();

                //work morning and work afternoon
                TimeSpan morningWork = new TimeSpan();
                TimeSpan afternoonWork = new TimeSpan();

                M_WorkingSystem mWorkingSystem;

                // Entry Time
                entryTime[0] = int.Parse(entryTimeHourValue);
                entryTime[1] = int.Parse(entryTimeMinuteValue);
                
                // Exit Time
                exitTime[0] = int.Parse(exitTimeHourValue);
                exitTime[1] = int.Parse(exitTimeMinuteValue);

                using (DB db = new DB())
                {
                    WorkingSystemService workingSystemService = new WorkingSystemService(db);
                    mWorkingSystem = workingSystemService.GetDataWorkingSystemById(workingSystemId);

                    string[] systemWorkingStart = new string[2];
                    string[] systemWorkingEnd = new string[2];
                    string[] systemWorkingEnd_2 = new string[2];
                    breakType = mWorkingSystem.BreakType;
                    workingType = mWorkingSystem.WorkingType;
                    if (mWorkingSystem.Working_Start != null)
                    {
                        systemWorkingStart = (Utilities.CommonUtil.IntToTime((int)mWorkingSystem.Working_Start, true)).Split(':');
                        systemWorkingEnd = (Utilities.CommonUtil.IntToTime((int)mWorkingSystem.Working_End, true)).Split(':');
                        if (mWorkingSystem.Working_End_2 != null)
                        {
                            systemWorkingEnd_2 = (Utilities.CommonUtil.IntToTime((int)mWorkingSystem.Working_End_2, true)).Split(':');
                        }
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

                    //get OverTime
                    if (mWorkingSystem.OverTime1_Start != null)
                    {
                        arrOverTime[0] = ConvertIntToDateTime((int)mWorkingSystem.OverTime1_Start);
                    }
                    if (mWorkingSystem.OverTime1_End != null)
                    {
                        arrOverTime[1] = ConvertIntToDateTime((int)mWorkingSystem.OverTime1_End);
                    }
                    if (mWorkingSystem.OverTime2_Start != null)
                    {
                        arrOverTime[2] = ConvertIntToDateTime((int)mWorkingSystem.OverTime2_Start);
                    }
                    if (mWorkingSystem.OverTime2_End != null)
                    {
                        arrOverTime[3] = ConvertIntToDateTime((int)mWorkingSystem.OverTime2_End);
                    }
                    if (mWorkingSystem.OverTime3_Start != null)
                    {
                        arrOverTime[4] = ConvertIntToDateTime((int)mWorkingSystem.OverTime3_Start);
                    }
                    if (mWorkingSystem.OverTime3_End != null)
                    {
                        arrOverTime[5] = ConvertIntToDateTime((int)mWorkingSystem.OverTime3_End);
                    }
                    if (mWorkingSystem.OverTime4_Start != null)
                    {
                        arrOverTime[6] = ConvertIntToDateTime((int)mWorkingSystem.OverTime4_Start);
                    }
                    if (mWorkingSystem.OverTime4_End != null)
                    {
                        arrOverTime[7] = ConvertIntToDateTime((int)mWorkingSystem.OverTime4_End);
                    }
                    if (mWorkingSystem.OverTime5_Start != null)
                    {
                        arrOverTime[8] = ConvertIntToDateTime((int)mWorkingSystem.OverTime5_Start);
                    }
                    if (mWorkingSystem.OverTime5_End != null)
                    {
                        arrOverTime[9] = ConvertIntToDateTime((int)mWorkingSystem.OverTime5_End);
                    }

                    //Time Work reality
                    DateTime workST = new DateTime(1, 1, 1).AddHours(entryTime[0]).AddMinutes(entryTime[1]);
                    DateTime workET = new DateTime(1, 1, 1).AddHours(exitTime[0]).AddMinutes(exitTime[1]);
                    DateTime startWorkTime = workST;
                    DateTime endWorkTime = workET;

                    DateTime systemST = new DateTime(1, 1, 1);
                    DateTime systemET = new DateTime(1, 1, 1);
                    DateTime systemET2 = new DateTime(1, 1, 1);

                    TimeSpan timeHoursAnnualPaid = new TimeSpan(8, 0, 0);
                    TimeSpan timeBreakAnnualPaid = new TimeSpan(1, 0, 0);
                    string[] First_End = new string[2];
                    string[] Latter_Start = new string[2];

                    DateTime workSystemST = new DateTime(1, 1, 1).AddHours(int.Parse(systemWorkingStart[0])).AddMinutes(int.Parse(systemWorkingStart[1]));
                    DateTime workSystemET = new DateTime(1, 1, 1).AddHours(int.Parse(systemWorkingEnd[0])).AddMinutes(int.Parse(systemWorkingEnd[1]));

                    if (mWorkingSystem.WorkingType == (int)WorkingType.WorkFullTime)
                    {
                        First_End = (Utilities.CommonUtil.IntToTime((int)mWorkingSystem.First_End, true)).Split(':');
                        Latter_Start = (Utilities.CommonUtil.IntToTime((int)mWorkingSystem.Latter_Start, true)).Split(':');

                        if (offAllDay != -1)
                        {
                            systemST = new DateTime(1, 1, 1).AddHours(0).AddMinutes(0);
                            systemET = new DateTime(1, 1, 1).AddHours(0).AddMinutes(0);
                        }
                        else
                        {
                            if (mWorkingSystem.Working_End_2 != null)
                            {
                                systemET2 = new DateTime(1, 1, 1).AddHours(int.Parse(systemWorkingEnd_2[0])).AddMinutes(int.Parse(systemWorkingEnd_2[1]));
                            }

                            if(offMorning != -1 && offAfternoon != -1)
                            {
                                systemST = new DateTime(1, 1, 1).AddHours(0).AddMinutes(0);
                                systemET = new DateTime(1, 1, 1).AddHours(0).AddMinutes(0);
                            }
                            else if (offMorning != -1 && offAfternoon == -1)
                            {
                                systemST = new DateTime(1, 1, 1).AddHours(int.Parse(Latter_Start[0])).AddMinutes(int.Parse(Latter_Start[1]));
                                systemET = new DateTime(1, 1, 1).AddHours(int.Parse(systemWorkingEnd[0])).AddMinutes(int.Parse(systemWorkingEnd[1]));
                                
                                //when startWorkTime < systemST 
                                if(startWorkTime < systemST)
                                {
                                    startWorkTime = systemST;
                                }
                                /*if (offMorning == (int)Vacation.AnnualPaid)
                                {
                                    check_AnnualPaidMorning = true;
                                    //systemET2 = systemET;
                                }*/
                                
                            }
                            else if (offMorning == -1 && offAfternoon != -1)
                            {
                                systemST = new DateTime(1, 1, 1).AddHours(int.Parse(systemWorkingStart[0])).AddMinutes(int.Parse(systemWorkingStart[1]));
                                systemET = new DateTime(1, 1, 1).AddHours(int.Parse(First_End[0])).AddMinutes(int.Parse(First_End[1]));
                                if (endWorkTime > systemET)
                                {
                                    endWorkTime = systemET;
                                }
                                /*if (offAfternoon == (int)Vacation.AnnualPaid)
                                {
                                    check_AnnualPaidAfternoon = true;
                                    systemET2 = systemET;
                                }*/
                            }
                            else
                            {
                                systemST = new DateTime(1, 1, 1).AddHours(int.Parse(systemWorkingStart[0])).AddMinutes(int.Parse(systemWorkingStart[1]));
                                systemET = new DateTime(1, 1, 1).AddHours(int.Parse(systemWorkingEnd[0])).AddMinutes(int.Parse(systemWorkingEnd[1]));
                            }
                        }
                       
                    }

                    //Check WorkingType 
                    if (mWorkingSystem.WorkingType == (int)WorkingType.WorkFullTime)
                    {
                        //work time
                        workSTWork = startWorkTime >= systemST ? startWorkTime : systemST;
                        workETWork = endWorkTime <= systemET ? endWorkTime : systemET;

                        //over time morning
                        if (startWorkTime < systemST)
                        {
                            workOTBeforeST = startWorkTime;
                            workOTBeforeET = endWorkTime < systemST ? endWorkTime : systemST;
                        }

                        //over time everning
                        if (endWorkTime > systemET)
                        {
                            workOTAfterST = startWorkTime > systemET ? startWorkTime : systemET;
                            workOTAfterET = endWorkTime;
                        }

                        // check when time out limit
                        /*if (check_AnnualPaidMorning || check_AnnualPaidAfternoon)
                        {
                            if (check_AnnualPaidMorning)
                            {
                                if (endWorkTime <= systemST)
                                {
                                    result.Append("{");
                                    result.Append(string.Format("\"WorkingHours\":\"{0}\"", ""));
                                    result.Append(string.Format(", \"LateHours\":\"{0}\"", ""));
                                    result.Append(string.Format(", \"EarlyHours\":\"{0}\"", ""));
                                    result.Append(string.Format(", \"OverTimeEarly\":\"{0}\"", ""));
                                    result.Append(string.Format(", \"OverTimeLate\":\"{0}\"", ""));
                                    result.Append("}");

                                    return result.ToString();
                                }
                            }
                            if (check_AnnualPaidAfternoon)
                            {
                                if (startWorkTime >= systemET)
                                {
                                    result.Append("{");
                                    result.Append(string.Format("\"WorkingHours\":\"{0}\"", ""));
                                    result.Append(string.Format(", \"LateHours\":\"{0}\"", ""));
                                    result.Append(string.Format(", \"EarlyHours\":\"{0}\"", ""));
                                    result.Append(string.Format(", \"OverTimeEarly\":\"{0}\"", ""));
                                    result.Append(string.Format(", \"OverTimeLate\":\"{0}\"", ""));
                                    result.Append("}");

                                    return result.ToString();
                                }
                            }
                        }*/

                        if (mWorkingSystem.BreakType == 0)
                        {
                            //late and leave early
                            if (systemST < systemET)
                            {
                                if (workST > systemET)
                                {
                                    beLate = CalDurationWorkTime(systemST, systemET, arrBreakTime);
                                }
                                else
                                {
                                    beLate = CalDurationWorkTime(systemST, workSTWork, arrBreakTime);
                                }

                                if (workET < systemST)
                                {
                                    leaveEarly = CalDurationWorkTime(systemST, systemET, arrBreakTime);
                                }
                                else
                                {
                                    if (systemET2 != new DateTime())
                                    {
                                        if (systemET2 <= workETWork && workETWork <= systemET)
                                        {
                                            leaveEarly = CalDurationWorkTime(systemET, systemET, arrBreakTime);
                                        }
                                        else
                                        {
                                            if (workETWork < systemET2)
                                            {
                                                leaveEarly = CalDurationWorkTime(workETWork, systemET, arrBreakTime);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        leaveEarly = CalDurationWorkTime(workETWork, systemET, arrBreakTime);
                                    }
                                }

                                // morning and afternoon select AnnualPaid
                                /*if (check_AnnualPaidMorning || check_AnnualPaidAfternoon)
                                {
                                    DateTime[] arrOverTimeAnnualPaid = new DateTime[10];
                                    if (mWorkingSystem.OverTime3_Start != null)
                                    {
                                        arrOverTimeAnnualPaid[4] = ConvertIntToDateTime((int)mWorkingSystem.OverTime3_Start);
                                    }
                                    if (mWorkingSystem.OverTime3_End != null)
                                    {
                                        arrOverTimeAnnualPaid[5] = ConvertIntToDateTime((int)mWorkingSystem.OverTime3_End);
                                    }

                                    DateTime lateStart = new DateTime(1, 1, 1).AddHours(int.Parse(Latter_Start[0])).AddMinutes(int.Parse(Latter_Start[1]));
                                    morningWork = (new DateTime(1, 1, 1).AddHours(int.Parse(First_End[0])).AddMinutes(int.Parse(First_End[1]))).Subtract(workSystemST);
                                    afternoonWork = CalDurationWorkTime(lateStart, workSystemET, arrBreakTime);
                                    //workSystemET.Subtract(new DateTime(1, 1, 1).AddHours(int.Parse(Latter_Start[0])).AddMinutes(int.Parse(Latter_Start[1])));

                                    // Overtime calculate
                                    if (workOTBeforeST != new DateTime(1, 1, 1) || workOTBeforeET != new DateTime(1, 1, 1))
                                    {
                                        ClassifyOTTime(workOTBeforeST, workOTBeforeET, arrBreakTime, arrOverTimeAnnualPaid, ref earlyOT, ref normalOT, ref lateNightOT, ref OT_04, ref OT_05);
                                    }
                                    if (workOTAfterST != new DateTime(1, 1, 1) || workOTAfterET != new DateTime(1, 1, 1))
                                    {
                                        ClassifyOTTime(workOTAfterST, workOTAfterET, arrBreakTime, arrOverTimeAnnualPaid, ref earlyOT, ref normalOT, ref lateNightOT, ref OT_04, ref OT_05);
                                    }

                                    //arrOverTimeAnnualPaid
                                    if (check_AnnualPaidMorning)
                                    {
                                        
                                        if (endWorkTime > systemET)
                                        {
                                            atWork = CalDurationWorkTime(systemST, endWorkTime, arrBreakTime).Subtract(beLate).Subtract(leaveEarly);
                                        }
                                        else
                                        {
                                            atWork = CalDurationWorkTime(systemST, systemET, arrBreakTime).Subtract(beLate).Subtract(leaveEarly);
                                            
                                        }
                                        atWork = atWork.Add(morningWork).Subtract(lateNightOT);
                                    }
                                    if (check_AnnualPaidAfternoon)
                                    {
                                        //  convert overtime (24:00->29:00) -->(00:00->05:00) hour
                                        if (arrOverTime[5].Day > 1)
                                        {
                                            arrOverTimeAnnualPaid[4] = new DateTime(1, 1, 1).AddHours(0).AddMinutes(0);
                                            arrOverTimeAnnualPaid[5] = arrOverTime[5].AddDays(-1);
                                        }

                                        // Overtime calculate
                                        if (workOTBeforeST != new DateTime(1, 1, 1) || workOTBeforeET != new DateTime(1, 1, 1))
                                        {
                                            ClassifyOTTime(workOTBeforeST, workOTBeforeET, arrBreakTime, arrOverTimeAnnualPaid, ref earlyOT, ref normalOT, ref lateNightOT, ref OT_04, ref OT_05);
                                        }
                                        if (workOTAfterST != new DateTime(1, 1, 1) || workOTAfterET != new DateTime(1, 1, 1))
                                        {
                                            ClassifyOTTime(workOTAfterST, workOTAfterET, arrBreakTime, arrOverTimeAnnualPaid, ref earlyOT, ref normalOT, ref lateNightOT, ref OT_04, ref OT_05);
                                        }

                                        if (startWorkTime < systemST)
                                        {
                                            atWork = CalDurationWorkTime(startWorkTime, systemET, arrBreakTime).Subtract(beLate).Subtract(leaveEarly);
                                        }
                                        else
                                        {
                                            atWork = CalDurationWorkTime(systemST, systemET, arrBreakTime).Subtract(beLate).Subtract(leaveEarly);
                                        }
                                        atWork = atWork.Add(afternoonWork).Subtract(lateNightOT);
                                    }
                                }
                                else
                                {
                                    //Working time
                                    atWork = CalDurationWorkTime(systemST, systemET, arrBreakTime).Subtract(beLate).Subtract(leaveEarly);

                                    // Overtime calculate
                                    if (workOTBeforeST != new DateTime(1, 1, 1) || workOTBeforeET != new DateTime(1, 1, 1))
                                    {
                                        ClassifyOTTime(workOTBeforeST, workOTBeforeET, arrBreakTime, arrOverTime, ref earlyOT, ref normalOT, ref lateNightOT, ref OT_04, ref OT_05);
                                    }
                                    if (workOTAfterST != new DateTime(1, 1, 1) || workOTAfterET != new DateTime(1, 1, 1))
                                    {
                                        ClassifyOTTime(workOTAfterST, workOTAfterET, arrBreakTime, arrOverTime, ref earlyOT, ref normalOT, ref lateNightOT, ref OT_04, ref OT_05);
                                    }
                                }*/
                                //Working time
                                atWork = CalDurationWorkTime(systemST, systemET, arrBreakTime).Subtract(beLate).Subtract(leaveEarly);

                                // Overtime calculate
                                if (workOTBeforeST != new DateTime(1, 1, 1) || workOTBeforeET != new DateTime(1, 1, 1))
                                {
                                    ClassifyOTTime(workOTBeforeST, workOTBeforeET, arrBreakTime, arrOverTime, ref earlyOT, ref normalOT, ref lateNightOT, ref OT_04, ref OT_05);
                                }
                                if (workOTAfterST != new DateTime(1, 1, 1) || workOTAfterET != new DateTime(1, 1, 1))
                                {
                                    ClassifyOTTime(workOTAfterST, workOTAfterET, arrBreakTime, arrOverTime, ref earlyOT, ref normalOT, ref lateNightOT, ref OT_04, ref OT_05);
                                }
                            }
                        }
                        else if (mWorkingSystem.BreakType == 1)
                        {
                            arrBreakTime = new DateTime[8];
                            
                            if (mWorkingSystem.Break1_Start != null)
                            {
                                arrBreakTime[0] = ConvertIntToDateTime((int)mWorkingSystem.Break1_Start);
                            }

                            TimeSpan BreakTimeHour = new TimeSpan(arrBreakTime[0].Hour, arrBreakTime[0].Minute, 0);

                            //late and leave early
                            if (systemST < systemET)
                            {
                                if (workST >= systemET.Subtract(BreakTimeHour))
                                {
                                    beLate = CalDurationWorkTime(systemST, systemET, arrBreakTime).Subtract(BreakTimeHour);
                                }
                                else
                                {
                                    beLate = CalDurationWorkTime(systemST, workSTWork, arrBreakTime);
                                }

                                if (workET < systemST || (workET >= systemST && workET <= systemST.Add(BreakTimeHour.Subtract(systemET.Subtract(systemET2)))))
                                {
                                    leaveEarly = CalDurationWorkTime(systemST, systemET, arrBreakTime).Subtract(BreakTimeHour);
                                }
                                else
                                {
                                    if (systemET2 != new DateTime())
                                    {
                                        if (systemET2 <= workETWork && workETWork <= systemET)
                                        {
                                            leaveEarly = CalDurationWorkTime(systemET, systemET, arrBreakTime);
                                        }
                                        else
                                        {
                                            if (workETWork < systemET2)
                                            {
                                                leaveEarly = CalDurationWorkTime(workETWork, systemET, arrBreakTime);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        leaveEarly = CalDurationWorkTime(workETWork, systemET, arrBreakTime);
                                    }
                                }

                                // morning and afternoon select AnnualPaid
                                /*if (check_AnnualPaidMorning || check_AnnualPaidAfternoon)
                                {
                                    DateTime[] arrOverTimeAnnualPaid = new DateTime[10];
                                    if (mWorkingSystem.OverTime3_Start != null)
                                    {
                                        arrOverTimeAnnualPaid[4] = ConvertIntToDateTime((int)mWorkingSystem.OverTime3_Start);
                                    }
                                    if (mWorkingSystem.OverTime3_End != null)
                                    {
                                        arrOverTimeAnnualPaid[5] = ConvertIntToDateTime((int)mWorkingSystem.OverTime3_End);
                                    }

                                    morningWork = (new DateTime(1, 1, 1).AddHours(int.Parse(First_End[0])).AddMinutes(int.Parse(First_End[1]))).Subtract(workSystemST);
                                    afternoonWork = workSystemET.Subtract(new DateTime(1, 1, 1).AddHours(int.Parse(Latter_Start[0])).AddMinutes(int.Parse(Latter_Start[1])));

                                    // Overtime calculate
                                    if (workOTBeforeST != new DateTime(1, 1, 1) || workOTBeforeET != new DateTime(1, 1, 1))
                                    {
                                        ClassifyOTTime(workOTBeforeST, workOTBeforeET, arrBreakTime, arrOverTimeAnnualPaid, ref earlyOT, ref normalOT, ref lateNightOT, ref OT_04, ref OT_05);
                                    }
                                    if (workOTAfterST != new DateTime(1, 1, 1) || workOTAfterET != new DateTime(1, 1, 1))
                                    {
                                        ClassifyOTTime(workOTAfterST, workOTAfterET, arrBreakTime, arrOverTimeAnnualPaid, ref earlyOT, ref normalOT, ref lateNightOT, ref OT_04, ref OT_05);
                                    }

                                    //arrOverTimeAnnualPaid
                                    if (check_AnnualPaidMorning)
                                    {
                                        if (endWorkTime > systemET)
                                        {
                                            atWork = CalDurationWorkTime(systemST, endWorkTime, arrBreakTime).Subtract(beLate).Subtract(leaveEarly);
                                        }
                                        else
                                        {
                                            atWork = CalDurationWorkTime(systemST, systemET, arrBreakTime).Subtract(beLate).Subtract(leaveEarly);

                                        }
                                        atWork = atWork.Add(morningWork).Subtract(lateNightOT).Subtract(BreakTimeHour);
                                    }
                                    if (check_AnnualPaidAfternoon)
                                    {
                                        //  convert overtime (24:00->29:00) -->(00:00->05:00)
                                        if (arrOverTime[5].Day > 1)
                                        {
                                            arrOverTimeAnnualPaid[4] = new DateTime(1, 1, 1).AddHours(0).AddMinutes(0);
                                            arrOverTimeAnnualPaid[5] = arrOverTime[5].AddDays(-1);
                                        }

                                        // Overtime calculate
                                        if (workOTBeforeST != new DateTime(1, 1, 1) || workOTBeforeET != new DateTime(1, 1, 1))
                                        {
                                            ClassifyOTTime(workOTBeforeST, workOTBeforeET, arrBreakTime, arrOverTimeAnnualPaid, ref earlyOT, ref normalOT, ref lateNightOT, ref OT_04, ref OT_05);
                                        }
                                        if (workOTAfterST != new DateTime(1, 1, 1) || workOTAfterET != new DateTime(1, 1, 1))
                                        {
                                            ClassifyOTTime(workOTAfterST, workOTAfterET, arrBreakTime, arrOverTimeAnnualPaid, ref earlyOT, ref normalOT, ref lateNightOT, ref OT_04, ref OT_05);
                                        }

                                        if (startWorkTime < systemST)
                                        {
                                            atWork = CalDurationWorkTime(startWorkTime, systemET, arrBreakTime).Subtract(beLate).Subtract(leaveEarly);
                                        }
                                        else
                                        {
                                            atWork = CalDurationWorkTime(systemST, systemET, arrBreakTime).Subtract(beLate).Subtract(leaveEarly);
                                        }
                                        atWork = atWork.Add(afternoonWork).Subtract(lateNightOT).Subtract(BreakTimeHour);
                                    }
                                }
                                else
                                {
                                    //Working time
                                    atWork = CalDurationWorkTime(systemST, systemET, arrBreakTime).Subtract(beLate).Subtract(leaveEarly).Subtract(BreakTimeHour);

                                    // Overtime calculate
                                    if (workOTBeforeST != new DateTime(1, 1, 1) || workOTBeforeET != new DateTime(1, 1, 1))
                                    {
                                        ClassifyOTTime(workOTBeforeST, workOTBeforeET, arrBreakTime, arrOverTime, ref earlyOT, ref normalOT, ref lateNightOT, ref OT_04, ref OT_05);
                                    }
                                    if (workOTAfterST != new DateTime(1, 1, 1) || workOTAfterET != new DateTime(1, 1, 1))
                                    {
                                        ClassifyOTTime(workOTAfterST, workOTAfterET, arrBreakTime, arrOverTime, ref earlyOT, ref normalOT, ref lateNightOT, ref OT_04, ref OT_05);
                                    }
                                }*/

                                //Working time
                                atWork = CalDurationWorkTime(systemST, systemET, arrBreakTime).Subtract(beLate).Subtract(leaveEarly).Subtract(BreakTimeHour);

                                // Overtime calculate
                                if (workOTBeforeST != new DateTime(1, 1, 1) || workOTBeforeET != new DateTime(1, 1, 1))
                                {
                                    ClassifyOTTime(workOTBeforeST, workOTBeforeET, arrBreakTime, arrOverTime, ref earlyOT, ref normalOT, ref lateNightOT, ref OT_04, ref OT_05);
                                }
                                if (workOTAfterST != new DateTime(1, 1, 1) || workOTAfterET != new DateTime(1, 1, 1))
                                {
                                    ClassifyOTTime(workOTAfterST, workOTAfterET, arrBreakTime, arrOverTime, ref earlyOT, ref normalOT, ref lateNightOT, ref OT_04, ref OT_05);
                                }
                            }
                        }
                        else if (mWorkingSystem.BreakType == 2)
                        {
                            DateTime[] arrBreakTime_Type2 = new DateTime[40];
                            TimeSpan TimeHourToBreak = new TimeSpan();
                            TimeSpan TimeBreak = new TimeSpan();
                            DateTime sTime = new DateTime(1, 1, 1).AddHours(startWorkTime.Hour).AddMinutes(startWorkTime.Minute);
                            DateTime eTime = new DateTime(1, 1, 1).AddHours(24);
                            if (startWorkTime.Date == endWorkTime.Date)
                            {
                                eTime = new DateTime(1, 1, 1).AddHours(endWorkTime.Hour).AddMinutes(endWorkTime.Minute);
                            }
                            else
                            {
                                eTime = new DateTime(1, 1, 2).AddHours(endWorkTime.Hour).AddMinutes(endWorkTime.Minute);
                            }

                            // BreakTime Hours
                            if (mWorkingSystem.Break1_Start == null || mWorkingSystem.Break1_End == null)
                            {
                                atWork = endWorkTime.Subtract(startWorkTime);
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

                            //late and leave early
                            if (systemST < systemET)
                            {
                                if (workST > systemET)
                                {
                                    beLate = CalDurationWorkTime(systemST, systemET, arrBreakTime_Type2);
                                }
                                else
                                {
                                    beLate = CalDurationWorkTime(systemST, workSTWork, arrBreakTime_Type2);
                                }

                                if (workET < systemST)
                                {
                                    leaveEarly = CalDurationWorkTime(systemST, systemET, arrBreakTime_Type2);
                                }
                                else
                                {
                                    if (systemET2 != new DateTime())
                                    {
                                        if (systemET2 <= workETWork && workETWork <= systemET)
                                        {
                                            leaveEarly = CalDurationWorkTime(systemET, systemET, arrBreakTime_Type2);
                                        }
                                        else
                                        {
                                            if (workETWork < systemET2)
                                            {
                                                leaveEarly = CalDurationWorkTime(workETWork, systemET, arrBreakTime_Type2);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        leaveEarly = CalDurationWorkTime(workETWork, systemET, arrBreakTime_Type2);
                                    }
                                }

                                // morning and afternoon select AnnualPaid
                                /*if (check_AnnualPaidMorning || check_AnnualPaidAfternoon)
                                {
                                    DateTime[] arrOverTimeAnnualPaid = new DateTime[10];
                                    if (mWorkingSystem.OverTime3_Start != null)
                                    {
                                        arrOverTimeAnnualPaid[4] = ConvertIntToDateTime((int)mWorkingSystem.OverTime3_Start);
                                    }
                                    if (mWorkingSystem.OverTime3_End != null)
                                    {
                                        arrOverTimeAnnualPaid[5] = ConvertIntToDateTime((int)mWorkingSystem.OverTime3_End);
                                    }

                                    morningWork = (new DateTime(1, 1, 1).AddHours(int.Parse(First_End[0])).AddMinutes(int.Parse(First_End[1]))).Subtract(workSystemST);
                                    afternoonWork = workSystemET.Subtract(new DateTime(1, 1, 1).AddHours(int.Parse(Latter_Start[0])).AddMinutes(int.Parse(Latter_Start[1])));

                                    // Overtime calculate
                                    if (workOTBeforeST != new DateTime(1, 1, 1) || workOTBeforeET != new DateTime(1, 1, 1))
                                    {
                                        ClassifyOTTime(workOTBeforeST, workOTBeforeET, arrBreakTime_Type2, arrOverTimeAnnualPaid, ref earlyOT, ref normalOT, ref lateNightOT, ref OT_04, ref OT_05);
                                    }
                                    if (workOTAfterST != new DateTime(1, 1, 1) || workOTAfterET != new DateTime(1, 1, 1))
                                    {
                                        ClassifyOTTime(workOTAfterST, workOTAfterET, arrBreakTime_Type2, arrOverTimeAnnualPaid, ref earlyOT, ref normalOT, ref lateNightOT, ref OT_04, ref OT_05);
                                    }

                                    //arrOverTimeAnnualPaid
                                    if (check_AnnualPaidMorning)
                                    {
                                        if (endWorkTime > systemET)
                                        {
                                            atWork = CalDurationWorkTime(systemST, endWorkTime, arrBreakTime_Type2).Subtract(beLate).Subtract(leaveEarly);
                                        }
                                        else
                                        {
                                            atWork = CalDurationWorkTime(systemST, systemET, arrBreakTime_Type2).Subtract(beLate).Subtract(leaveEarly);

                                        }
                                        atWork = atWork.Add(morningWork).Subtract(lateNightOT);
                                    }

                                    if (check_AnnualPaidAfternoon)
                                    {
                                        //  convert overtime (24:00->29:00) -->(00:00->05:00)
                                        if (arrOverTime[5].Day > 1)
                                        {
                                            arrOverTimeAnnualPaid[4] = new DateTime(1, 1, 1).AddHours(0).AddMinutes(0);
                                            arrOverTimeAnnualPaid[5] = arrOverTime[5].AddDays(-1);
                                        }

                                        // Overtime calculate
                                        if (workOTBeforeST != new DateTime(1, 1, 1) || workOTBeforeET != new DateTime(1, 1, 1))
                                        {
                                            ClassifyOTTime(workOTBeforeST, workOTBeforeET, arrBreakTime_Type2, arrOverTimeAnnualPaid, ref earlyOT, ref normalOT, ref lateNightOT, ref OT_04, ref OT_05);
                                        }
                                        if (workOTAfterST != new DateTime(1, 1, 1) || workOTAfterET != new DateTime(1, 1, 1))
                                        {
                                            ClassifyOTTime(workOTAfterST, workOTAfterET, arrBreakTime_Type2, arrOverTimeAnnualPaid, ref earlyOT, ref normalOT, ref lateNightOT, ref OT_04, ref OT_05);
                                        }

                                        if (startWorkTime < systemST)
                                        {
                                            atWork = CalDurationWorkTime(startWorkTime, systemET, arrBreakTime_Type2).Subtract(beLate).Subtract(leaveEarly);
                                        }
                                        else
                                        {
                                            atWork = CalDurationWorkTime(systemST, systemET, arrBreakTime_Type2).Subtract(beLate).Subtract(leaveEarly);
                                        }
                                        atWork = atWork.Add(afternoonWork).Subtract(lateNightOT);
                                    }
                                }
                                else
                                {
                                    //Working time
                                    atWork = CalDurationWorkTime(systemST, systemET, arrBreakTime_Type2).Subtract(beLate).Subtract(leaveEarly);

                                    // Overtime calculate
                                    if (workOTBeforeST != new DateTime(1, 1, 1) || workOTBeforeET != new DateTime(1, 1, 1))
                                    {
                                        ClassifyOTTime(workOTBeforeST, workOTBeforeET, arrBreakTime_Type2, arrOverTime, ref earlyOT, ref normalOT, ref lateNightOT, ref OT_04, ref OT_05);
                                    }
                                    if (workOTAfterST != new DateTime(1, 1, 1) || workOTAfterET != new DateTime(1, 1, 1))
                                    {
                                        ClassifyOTTime(workOTAfterST, workOTAfterET, arrBreakTime_Type2, arrOverTime, ref earlyOT, ref normalOT, ref lateNightOT, ref OT_04, ref OT_05);
                                    }
                                }*/

                                //Working time
                                atWork = CalDurationWorkTime(systemST, systemET, arrBreakTime_Type2).Subtract(beLate).Subtract(leaveEarly);

                                // Overtime calculate
                                if (workOTBeforeST != new DateTime(1, 1, 1) || workOTBeforeET != new DateTime(1, 1, 1))
                                {
                                    ClassifyOTTime(workOTBeforeST, workOTBeforeET, arrBreakTime_Type2, arrOverTime, ref earlyOT, ref normalOT, ref lateNightOT, ref OT_04, ref OT_05);
                                }
                                if (workOTAfterST != new DateTime(1, 1, 1) || workOTAfterET != new DateTime(1, 1, 1))
                                {
                                    ClassifyOTTime(workOTAfterST, workOTAfterET, arrBreakTime_Type2, arrOverTime, ref earlyOT, ref normalOT, ref lateNightOT, ref OT_04, ref OT_05);
                                }
                            }
                        }
                    }

                    else if (workingType == (int)WorkingType.WorkHoliDay || workingType == (int)WorkingType.LegalHoliDay)
                    {
                        // When breakType = 0
                        if (mWorkingSystem.BreakType == 0)
                        {
                            ClassifyOTTime(workST, workET, arrBreakTime, arrOverTime, ref earlyOT, ref normalOT, ref lateNightOT, ref OT_04, ref OT_05);

                            //Working time
                            atWork = CalDurationWorkTime(workST, workET, arrBreakTime).Subtract(earlyOT)
                                                                                      .Subtract(normalOT)
                                                                                      .Subtract(lateNightOT)
                                                                                      .Subtract(OT_04)
                                                                                      .Subtract(OT_05);
                        }
                        // When breakType = 1
                        else if(mWorkingSystem.BreakType == 1)
                        {
                            ClassifyOTTime(workST, workET, arrBreakTime, arrOverTime, ref earlyOT, ref normalOT, ref lateNightOT, ref OT_04, ref OT_05);

                            // BreakTime Hours
                            TimeSpan BreakTimeHour = new TimeSpan(arrBreakTime[0].Hour, arrBreakTime[0].Minute, 0);

                            //Working time
                            atWork = endWorkTime.Subtract(startWorkTime).Subtract(BreakTimeHour)
                                                                        .Subtract(earlyOT)
                                                                        .Subtract(normalOT)
                                                                        .Subtract(lateNightOT)
                                                                        .Subtract(OT_04)
                                                                        .Subtract(OT_05);
                            if (atWork < new TimeSpan(0, 0, 0))
                            {
                                atWork = new TimeSpan(0, 0, 0);
                            }
                        }

                        // When breakType = 2 
                        else if(mWorkingSystem.BreakType == 2)
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
                                atWork = endWorkTime.Subtract(startWorkTime);
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

                            ClassifyOTTime(workST, workET, arrBreakTime_Type2, arrOverTime, ref earlyOT, ref normalOT, ref lateNightOT, ref OT_04, ref OT_05);

                            //Working time
                            atWork = CalDurationWorkTime(workST, workET, arrBreakTime_Type2).Subtract(earlyOT)
                                                                                      .Subtract(normalOT)
                                                                                      .Subtract(lateNightOT)
                                                                                      .Subtract(OT_04)
                                                                                      .Subtract(OT_05);
                        }
                    }

                    string strTotalWorkingHours = string.Empty;
                    string strTotalOverTimeHours = string.Empty;
                    string strAtWork = string.Empty;
                    var totalOverTimeHours = earlyOT + normalOT + lateNightOT + OT_04 + OT_05;
                    var totalWorkingHours = atWork + totalOverTimeHours;

                    if (atWork.Days > 0)
                    {
                        string[] arrAtWork = FormatTimeResult(atWork).Split(':');
                        strAtWork = (int.Parse(arrAtWork[0]) + (atWork.Days * 24)).ToString() + ":" + arrAtWork[1];
                    }
                    else
                    {
                        strAtWork = FormatTimeResult(atWork);
                    }

                    // Convert TotalWorkingHours
                    if (totalWorkingHours.Days > 0)
                    {
                        string[] arrTotalWorkingHours = FormatTimeResult(totalWorkingHours).Split(':');
                        strTotalWorkingHours = (int.Parse(arrTotalWorkingHours[0]) + (totalWorkingHours.Days * 24)).ToString() + ":" + arrTotalWorkingHours[1];
                    }
                    else
                    {
                        strTotalWorkingHours = FormatTimeResult(totalWorkingHours);
                    }

                    // Convert TotalOverTimeHours
                    if (totalOverTimeHours.Days > 0)
                    {
                        string[] arrTotalOverTimeHours = FormatTimeResult(totalOverTimeHours).Split(':');
                        strTotalOverTimeHours = (int.Parse(arrTotalOverTimeHours[0]) + (totalOverTimeHours.Days * 24)).ToString() + ":" + arrTotalOverTimeHours[1];
                    }
                    else
                    {
                        strTotalOverTimeHours = FormatTimeResult(totalOverTimeHours);
                    }
                    result.Append("{");

                    result.Append(string.Format("\"WorkingHours\":\"{0}\"", strAtWork));
                    result.Append(string.Format(", \"LateHours\":\"{0}\"", FormatTimeResult(beLate)));
                    result.Append(string.Format(", \"EarlyHours\":\"{0}\"", FormatTimeResult(leaveEarly)));
                    result.Append(string.Format(", \"EarlyOT\":\"{0}\"", FormatTimeResult(earlyOT)));
                    result.Append(string.Format(", \"NormalOT\":\"{0}\"", FormatTimeResult(normalOT)));
                    result.Append(string.Format(", \"LateNightOT\":\"{0}\"", FormatTimeResult(lateNightOT)));
                    result.Append(string.Format(", \"OT_04\":\"{0}\"", FormatTimeResult(OT_04)));
                    result.Append(string.Format(", \"OT_05\":\"{0}\"", FormatTimeResult(OT_05)));
                    result.Append(string.Format(", \"TotalOverTimeHours\":\"{0}\"", strTotalOverTimeHours));
                    result.Append(string.Format(", \"TotalWorkingHours\":\"{0}\"", strTotalWorkingHours));

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

         /// <summary>
        /// GetWorkingTimeProjectValue
        /// </summary>
        /// <param name="workingSystemId"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string GetWorkingTimeProjectValue(int workingSystemId, string entryTimeValue, string exitTimeValue)
        {
            TimeSpan workingTimeProject = new TimeSpan();
            System.Text.StringBuilder result = new System.Text.StringBuilder();
            try
            {
                string[] arrEntryTime = entryTimeValue.Split(':');
                string[] arryExitTime = exitTimeValue.Split(':');
                string entryTimeHourValue = arrEntryTime[0];
                string entryTimeMinuteValue = arrEntryTime[1];
                string exitTimeHourValue = arryExitTime[0];
                string exitTimeMinuteValue = arryExitTime[1];
                DateTime[] arrBreakTime = new DateTime[8];

                //Time Work reality
                DateTime workST = new DateTime(1, 1, 1).AddHours(int.Parse(arrEntryTime[0])).AddMinutes(int.Parse(arrEntryTime[1]));
                DateTime workET = new DateTime(1, 1, 1).AddHours(int.Parse(arryExitTime[0])).AddMinutes(int.Parse(arryExitTime[1]));

                M_WorkingSystem mWorkingSystem = new M_WorkingSystem();
                using (DB db = new DB())
                {
                    WorkingSystemService workingSystemService = new WorkingSystemService(db);
                    mWorkingSystem = workingSystemService.GetDataWorkingSystemById(workingSystemId);
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

                    if (mWorkingSystem.BreakType == 0)
                    {
                        //Working time
                        workingTimeProject = CalDurationWorkTime(workST, workET, arrBreakTime);
                    }
                    else
                    {
                        //Working time
                        workingTimeProject = workET.Subtract(workST);
                    }
                }

                result.Append("{");

                result.Append(string.Format("\"workingTimeProject\":\"{0}\"", FormatTimeResult(workingTimeProject)));

                result.Append("}");

                return result.ToString();
            }
                
            catch (Exception ex)
            {
                Log.Instance.WriteLog(ex);
                return null;
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
                DateTime sTime = new DateTime();
                DateTime eTime = new DateTime();
                if (startTime.Day == 1)
                {
                    sTime = new DateTime(1, 1, 1).AddHours(startTime.Hour).AddMinutes(startTime.Minute);
                    eTime = new DateTime(1, 1, 1).AddHours(24);
                    if (startTime.Date == endTime.Date)
                    {
                        eTime = new DateTime(1, 1, 1).AddHours(endTime.Hour).AddMinutes(endTime.Minute);
                    }
                    else
                    {
                        eTime = new DateTime(1, 1, 2).AddHours(endTime.Hour).AddMinutes(endTime.Minute);
                    }
                }
                else
                {
                    sTime = new DateTime(1, 1, 2).AddHours(startTime.Hour).AddMinutes(startTime.Minute);
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

        /// <summary>
        /// Classify overtime time
        /// </summary>
        /// <param name="stOT">start time</param>
        /// <param name="etOT">end time</param>
        /// <param name="arrBreakTime">array break time</param>
        /// <param name="arrOverTime">array over time</param>
        /// <param name="earlyOT">ref early overtim</param>
        /// <param name="normalOT">ref normal overtim</param>
        /// <param name="lateOT">ref late night overtim</param>
        /// <param name="OT_04">ref OT_04</param>
        /// <param name="OT_05">ref OT_05</param>
        public static void ClassifyOTTime(DateTime stOT, DateTime etOT, DateTime[] arrBreakTime,DateTime[] arrOverTime, ref TimeSpan earlyOT, ref TimeSpan normalOT, ref TimeSpan lateOT, ref TimeSpan OT_04, ref TimeSpan OT_05)
        {
            DateTime earlyOT_ST = arrOverTime[0];
            DateTime earlyOT_ET = arrOverTime[1];
            DateTime nomalOT_ST = arrOverTime[2];
            DateTime nomalOT_ET = arrOverTime[3];
            
            DateTime lateOT_ST = arrOverTime[4];
            DateTime lateOT_ET = arrOverTime[5];
            DateTime OT_4_ST = arrOverTime[6];
            DateTime OT_4_ET = arrOverTime[7];
            DateTime OT_5_ST = arrOverTime[8];
            DateTime OT_5_ET = arrOverTime[9];

            while (stOT < etOT)
            {
                bool isBrekTime = false;
                for (int i = 0; i < arrBreakTime.Length - 1; i = i + 2)
                {
                    if (stOT >= arrBreakTime[i] && stOT < arrBreakTime[i + 1])
                    {
                        isBrekTime = true;
                        break;
                    }
                }

                if (!isBrekTime)
                {
                    if ((earlyOT_ST != new DateTime(1, 1, 1)) && (stOT >= earlyOT_ST && stOT < earlyOT_ET))
                    {
                        earlyOT = earlyOT.Add(new TimeSpan(0, 1, 0));
                    }
                    else if ((nomalOT_ST != new DateTime(1, 1, 1)) && (nomalOT_ST < nomalOT_ET) && (stOT >= nomalOT_ST && stOT < nomalOT_ET))
                    {
                        normalOT = normalOT.Add(new TimeSpan(0, 1, 0));
                    }
                    else if ((lateOT_ST < lateOT_ET) && (stOT >= lateOT_ST && stOT < lateOT_ET))
                    {
                        lateOT = lateOT.Add(new TimeSpan(0, 1, 0));
                    }
                    else if ((OT_4_ST != new DateTime(1, 1, 1)) && (OT_4_ST < OT_4_ET) && (stOT >= OT_4_ST && stOT < OT_4_ET))
                    {
                        OT_04 = OT_04.Add(new TimeSpan(0, 1, 0));
                    }
                    else if ((OT_5_ST != new DateTime(1, 1, 1)) && (OT_5_ST < OT_5_ET) && (stOT >= OT_5_ST && stOT < OT_5_ET))
                    {
                        OT_05 = OT_05.Add(new TimeSpan(0, 1, 0));
                    }
                }
                stOT = stOT.AddMinutes(1);
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
                return string.Format("{0:00}:{1:00}", inVal.Hours + inVal.Days * 24, inVal.Minutes);
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// StatusFlag=1 is true
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        private bool checkAttendanceStatusFlag(int UID, DateTime startTime, DateTime endTime)
        {
            bool result = false;
            int count = 0;
            using (DB db = new DB())
            {
                AttendanceService atdService = new AttendanceService(db);
                count = atdService.checkAttendanceSubmit(UID, startTime, endTime);
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
        /// Create New Message
        /// </summary>
        /// <returns>new MimeMessage</returns>
        private MimeMessage CreateMessage()
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress(this.MAIL_NAME, this.MAIL_ID));
            return message;
        }

        /// <summary>
        /// Send Messagae
        /// </summary>
        /// <param name="client">SmtpClient</param>
        /// <param name="message">MimeMessage</param>
        /// <returns></returns>
        private bool SendMessage(SmtpClient client, MimeMessage message)
        {
            try
            {
                //send message
                client.Send(message);
            }
            //catch status email
            catch (SmtpCommandException exSmtpCommand)
            {
                switch (exSmtpCommand.ErrorCode)
                {
                    case SmtpErrorCode.RecipientNotAccepted:
                        Log.Instance.WriteLog(string.Format("Recipient not accepted: {0}", exSmtpCommand.Mailbox));
                        return false;
                    case SmtpErrorCode.SenderNotAccepted:
                        Log.Instance.WriteLog(string.Format("Sender not accepted: {0}", exSmtpCommand.Mailbox));
                        return false;
                    case SmtpErrorCode.MessageNotAccepted:
                        Log.Instance.WriteLog(string.Format("Message not accepted", exSmtpCommand.Mailbox));
                        return false;
                    case SmtpErrorCode.UnexpectedStatusCode:
                        Log.Instance.WriteLog(string.Format("An unexpected status code {0}", exSmtpCommand.Mailbox));
                        return false;
                }
            }
            catch (SmtpProtocolException exProtocol)
            {
                Log.Instance.WriteLog(string.Format("Protocol error while sending message {0}", exProtocol.Message));
                return false;
            }
            catch (ServiceNotConnectedException exService)
            {
                Log.Instance.WriteLog(string.Format("MailTransport is not connected {0}", exService.Message));
                return false;
            }
            //mail sent success
            return true;
        }

        /// <summary>
        /// get config name
        /// </summary>
        /// <returns></returns>
        private string GetNameConfig()
        {
            using (DB db = new DB())
            {
                try
                {
                    var config_D = new Config_DService(db);
                    var config_H = new Config_HService(db);
                    var Isdefalt = config_H.GetDefaultValueDrop(M_Config_H.CONFIG_MAIL_SEND_NAME);
                    if (Isdefalt != null && Isdefalt == "1")
                    {
                        return config_D.GetValue2(M_Config_H.CONFIG_MAIL_SEND_NAME, 1);
                    }
                    return string.Empty;
                }
                catch (Exception ex)
                {
                    Log.Instance.WriteLog(ex);
                    return string.Empty;
                }
            }
        }

        private ApprovalMailInfo GetApprovalMailInfo(int AttendanceId, bool isUseTimeApproval)
        {
            using (DB db = new DB())
            {
                ApprovalService userService = new ApprovalService(db);
                return userService.GetApprovalMailInfo(AttendanceId, isUseTimeApproval);
            }
        }

        /// <summary>
        /// メールの内容
        /// </summary>
        /// <returns>内容</returns>
        private string GetMailApplyContent(ApprovalMailInfo approvalMailInfo)
        {
            List<string> lstApplyContent = new List<string>();

            lstApplyContent.Add(string.Format("部門：{0}",approvalMailInfo.DepartmentName));
            lstApplyContent.Add(string.Format("社員名：{0}", approvalMailInfo.UserName));
            lstApplyContent.Add(string.Format("対象日：{0}", approvalMailInfo.Date + approvalMailInfo.ExchangeDate));
            lstApplyContent.Add(string.Format("申請内容：{0}", approvalMailInfo.ApplyContent));
            lstApplyContent.Add(string.Format("事由：{0}", approvalMailInfo.RequestNote));
            if (!string.IsNullOrEmpty(approvalMailInfo.EntryTime))
            {
                lstApplyContent.Add(string.Format("出勤：{0}", approvalMailInfo.EntryTime));
            }
            if (!string.IsNullOrEmpty(approvalMailInfo.ExitTime))
            {
                lstApplyContent.Add(string.Format("退勤：{0}", approvalMailInfo.ExitTime));
            }
            return string.Join("\n", lstApplyContent);
        }

        /// <summary>
        /// メールの内容
        /// </summary>
        /// <returns>内容</returns>
        private string GetMailApprovalContent(string approvalUserLable,ApprovalMailInfo approvalMailInfo)
        {
            List<string> lstApprovalContent = new List<string>();

            lstApprovalContent.Add(string.Format("{0}：{1}", approvalUserLable, approvalMailInfo.ApprovalUserName));
            lstApprovalContent.Add(string.Format("事由：{0}", approvalMailInfo.ApprovalNote));

            return string.Join("\n", lstApprovalContent);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        private XmlNode getNodeXml(string mailType)
        {
            XmlDocument xd = new XmlDocument();
            xd.LoadXml(Resources.Email);

            XmlNodeList nodelist = xd.SelectNodes("/sendmail/email");

            foreach (XmlNode node in nodelist)
            {
                string type = node.SelectSingleNode("type").InnerText;

                if (type == mailType)
                {
                    return node;
                }
            }

            return null;
        }

        /// <summary>
        /// Send Email
        /// </summary>
        /// <returns></returns>
        private bool SendEmail(AttendanceApprovalStatus approvalStatus, T_Attendance tAttendance)
        {
            try
            {
                var client = new SmtpClient();
                var body = new BodyBuilder();
                var message = this.CreateMessage();
                var result = true;
                var isUseTimeApproval = this.GetIsUseApply(M_Config_H.CONFIG_USE_TIME_APPROVAL);
                string mailType = string.Empty;
                string subject;
                string content;

                switch (approvalStatus)
                {
                    case AttendanceApprovalStatus.Request:
                        mailType = "attendance_apply";
                        break;
                    case AttendanceApprovalStatus.Approved:
                        mailType = "attendance_approved";
                        break;
                    case AttendanceApprovalStatus.Cancel:
                        mailType = "attendance_cancel";
                        break;
                }

                var approvalMailInfo = this.GetApprovalMailInfo(tAttendance.ID, isUseTimeApproval);
                XmlNode node_email = this.getNodeXml(mailType);
                subject = node_email.SelectSingleNode("subject").InnerText.Trim();
                content = node_email.SelectSingleNode("content").InnerText.Trim();
                string mailBody = string.Empty;

                switch (approvalStatus)
                {
                    case AttendanceApprovalStatus.Request:
                        mailBody = string.Format(content, this.GetMailApplyContent(approvalMailInfo));
                        break;
                    case AttendanceApprovalStatus.Approved:
                        mailBody = string.Format(content
                                                , this.GetMailApplyContent(approvalMailInfo)
                                                , this.GetMailApprovalContent("承認者",approvalMailInfo));
                        break;
                    case AttendanceApprovalStatus.Cancel:
                        mailBody = string.Format(content
                                                , this.GetMailApplyContent(approvalMailInfo)
                                                , this.GetMailApprovalContent("差戻者",approvalMailInfo));
                        break;
                }

                message.Subject = string.Format(subject, approvalMailInfo.MailSubject);
                body.TextBody = mailBody;
                message.Body = body.ToMessageBody();

                if (ConnectedServerSmtp(client))
                {
                    List<M_User> lstMail = new List<M_User>();
                    using (DB db = new DB())
                    {
                        UserService userService = new UserService(db);
                        if (approvalStatus == AttendanceApprovalStatus.Request)
                        {
                            lstMail = userService.GetListApprovalMails(tAttendance.UID).ToList();
                        }
                        else
                        {
                            if (tAttendance.ApprovalUID != tAttendance.UID)
                            {
                                lstMail.Add(userService.GetByID(tAttendance.UID));
                            }
                            if (approvalStatus == AttendanceApprovalStatus.Approved)
                            {
                                lstMail.AddRange(userService.GetListConfirmMails(tAttendance.UID).ToList());
                            }
                        }
                    }

                    foreach (var item in lstMail)
                    {
                        if (item.MailAddress.Length <= 0)
                        {
                            continue;
                        }

                        message.To.Clear();
                        message.To.Add(new MailboxAddress(item.UserName1, item.MailAddress));
                        if (!SendMessage(client, message))
                        {
                            base.SetMessage(string.Empty, M_Message.MSG_USER_SEND_ERMAIL_ERROR, item.UserName1);
                            result = false;
                        }
                    }
                    this.CloseConnectSmtp(client);
                    return result;
                }
                else
                {
                    this.SetMessage(string.Empty, M_Message.MSG_CONNECT_ERMAIL_ERROR, "");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Instance.WriteLog(ex);
                this.SetMessage(string.Empty, M_Message.MSG_UPDATE_FAILE, "メール");
                return false;
            }
        }
        #endregion

        #region Connection Email
        /// <summary>
        /// Connected server Smtp
        /// </summary>
        /// <param name="client">SmtpClient</param>
        /// <returns></returns>
        private bool ConnectedServerSmtp(SmtpClient client)
        {
            // connect mail server
            try
            {
                var options = SecureSocketOptions.Auto;
                if (this.PROTOCOL_SSL)
                {
                    options = SecureSocketOptions.SslOnConnect;
                }
                client.Connect(this.HOST_SMTP, this.PORT_SMTP, options);
            }
            catch (Exception ex)
            {
                Log.Instance.WriteLog(ex);
                return false;
            }
            try
            {
                // client eamil and password
                client.Authenticate(this.MAIL_ID, this.MAIL_PASSWORD);
            }
            //Some SMTP servers support authentication.
            catch (AuthenticationException exAuthen)
            {
                Log.Instance.WriteLog(string.Format("Invalid: {0}", exAuthen.Message));
                return false;
            }
            catch (SmtpCommandException exSmtpCommand)
            {
                Log.Instance.WriteLog(string.Format("Error trying to authenticate: {0}", exSmtpCommand.Message));
                return false;
            }
            catch (SmtpProtocolException exProtocol)
            {
                Log.Instance.WriteLog(string.Format("Protocol error while trying to authenticate: {0}", exProtocol.Message));
                return false;
            }
            //connecion susscess
            return client.IsConnected;
        }

        /// <summary>
        /// Close Connection Smtp
        /// </summary>
        /// <param name="client">SmtpClient</param>
        private void CloseConnectSmtp(SmtpClient client)
        {
            client.Disconnect(true);
        }

        #endregion

    }
}