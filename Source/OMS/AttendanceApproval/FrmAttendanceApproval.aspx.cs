using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using OMS.Utilities;
using OMS.DAC;
using OMS.Models;
using System.Data;
using System.Web.UI.HtmlControls;

namespace OMS.AttendanceApproval
{
    public partial class FrmAttendanceApproval : FrmBaseList
    {
        #region Constant
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
        /// Get or set Collapse
        /// </summary>
        public bool isShowButtonAction
        {
            get { return (bool)ViewState["isShowButtonAction"]; }
            set { ViewState["isShowButtonAction"] = value; }
        }

        /// <summary>
        /// Get or set Collapse
        /// </summary>
        public Hashtable ListAttendanceApprovaId
        {
            get { return (Hashtable)ViewState["ListAttendanceApprovaId"]; }
            set { ViewState["ListAttendanceApprovaId"] = value; }
        }


        public IList<AttendanceApprovalInfo> ListAttendanceApprovalInfoViewState
        {
            get { return (IList<AttendanceApprovalInfo>)ViewState["ListAttendanceApprovalInfoViewState"]; }
            set { ViewState["ListAttendanceApprovalInfoViewState"] = value; }
        }

        /// <summary>
        /// Get or set flagApproval
        /// </summary>
        public bool flagApproval
        {
            get { return (bool)ViewState["flagApproval"]; }
            set { ViewState["flagApproval"] = value; }
        }

        /// <summary>
        /// Get or set flagRelease
        /// </summary>
        public bool flagRelease
        {
            get { return (bool)ViewState["flagRelease"]; }
            set { ViewState["flagRelease"] = value; }
        }

        /// <summary>
        /// Get or set RowNumberTo
        /// </summary>
        public int RowNumberToViewState
        {
            get { return (int)ViewState["RowNumberTo"]; }
            set { ViewState["RowNumberTo"] = value; }
        }

        /// <summary>
        /// Get or set RowNumberFrom
        /// </summary>
        public int RowNumberFromViewState
        {
            get { return (int)ViewState["RowNumberFrom"]; }
            set { ViewState["RowNumberFrom"] = value; }
        }

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

        /// <summary>
        /// Get or set NumVacation
        /// </summary>
        public int NumVacation { get; set; }

        /// <summary>
        /// Get or set NumVacation
        /// </summary>
        public int NumOvertime { get; set; }

        /// <summary>
        /// Get or set Success
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Get or ser IsShowQuestion
        /// </summary>
        public bool IsShowQuestion { get; set; }

        /// <summary>
        /// Get or ser DefaultButton
        /// </summary>
        public string DefaultButton { get; set; }

        public Hashtable HashtableListChecked { get; set; }
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
            base.FormTitle = "勤務表承認";
            base.FormSubTitle = "List";

            // paging footer
            this.PagingFooter.OnClick += PagingFooter_Click;

            // paging header
            this.PagingHeader.OnClick += PagingHeader_Click;
            this.PagingHeader.OnPagingClick += PagingFooter_Click;
            this.PagingHeader.NumRowOnPage = base.NumRowOnPageDefault;
            this.PagingHeader.CurrentPage = base.CurrentPageDefault;
            this.PagingHeader.IsShowColor = false;

            //Init Event
            LinkButton btnYes = (LinkButton)this.Master.FindControl("btnYes");
            btnYes.Click += new EventHandler(btnProcess);

            LinkButton btnNo = (LinkButton)this.Master.FindControl("btnNo");
            btnNo.Click += new EventHandler(btCancelSubmit);

            //define event button search
            this.btnSearch.ServerClick += new EventHandler(btnSearch_Click);

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
            base.SetAuthority(FormId.AttendanceApproval);
            if (!this._authority.IsAttendanceApprovalView)
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
                    if (this.PreviousPageViewState["ConditionApproval"] != null)
                    {
                        Hashtable data = (Hashtable)PreviousPageViewState["ConditionApproval"];

                        this.ShowCondition(data);

                        LoadDataForPagingHeader(this.RowNumberToViewState, this.RowNumberFromViewState, false);
                    }
                    else
                    {
                        //Show data on grid
                        this.LoadDataGrid();
                    }
                }
                else
                {
                    //Show data on grid
                    this.LoadDataGrid();
                }

                this.Collapse = "in";
            }

            CheckAuthority();
            LoadUserComboboxAttribute();

        }

        protected void btCancelSubmit(object sender, EventArgs e)
        {
            LoadDataForPagingHeader(this.RowNumberToViewState, this.RowNumberFromViewState, false);
            CheckAuthority();
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

                LoadDataForPagingHeader(this.PagingHeader.NumRowOnPage * (curPage - 1), this.PagingHeader.NumRowOnPage * curPage);
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

                LoadDataForPagingHeader(0, this.PagingHeader.NumRowOnPage);
            }
        }

        /// <summary>
        /// rptAttendanceList_ItemDataBound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptAttendanceApprovalList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                DataRowView dr = e.Item.DataItem as DataRowView;
                //  int ID = int.Parse(dr["ID"].ToString());

                Repeater rptOverTimeH = e.Item.FindControl("rptOverTimeH") as Repeater;
                setHeaderOverTime(rptOverTimeH);

                Repeater rptVacationH = e.Item.FindControl("rptVacationH") as Repeater;
                getVacationConfig(rptVacationH, M_Config_H.CONFIG_CD_VACATION_TYPE, DateTime.Parse(dr["StartDate"].ToString()), DateTime.Parse(dr["EndDate"].ToString()), int.Parse(dr["UID"].ToString()));

                Repeater rptVacationD = e.Item.FindControl("rptVacationD") as Repeater;
                getVacationConfig(rptVacationD, M_Config_H.CONFIG_CD_VACATION_TYPE, DateTime.Parse(dr["StartDate"].ToString()), DateTime.Parse(dr["EndDate"].ToString()), int.Parse(dr["UID"].ToString()));

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
        /// ddlWorkingCalendar_SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlWorkingCalendar_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadDdlDateOfServiceCombobox();
            this.LoadDepartmentComboboxData();
            this.LoadUserComboboxData();
            this.Collapse = "in";
            LoadDataForPagingHeader(this.RowNumberToViewState, this.RowNumberFromViewState, false);
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
            this.Collapse = "in";
            LoadDataForPagingHeader(this.RowNumberToViewState, this.RowNumberFromViewState, false);
            CheckAuthority();
        }

        /// <summary>
        /// btnProcess approval or Release
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnProcess(object sender, EventArgs e)
        {
            if (flagApproval)
            {
                IList<AttendanceApprovalInfo> listDetail = this.GetData();

                try
                {
                    using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                    {
                        bool result = false;
                        bool isRowApproved = false;
                        List<int> listId = new List<int>();
                        foreach (var lstDetail in listDetail)
                        {
                            if (string.Compare(lstDetail.StatusFlag, ((int)AttendanceStatusFlag.Approved).ToString()) == 0)
                            {
                                isRowApproved = true;
                                result = false;
                                break;
                            }

                            IList<AttendanceApprovalIDAndUpdateDate> listIDAndUpdateDate = (IList<AttendanceApprovalIDAndUpdateDate>)ListAttendanceApprovaId[lstDetail.RowNumber];

                            AttendanceService attendanceService = new AttendanceService(db);
                            foreach (var list in listIDAndUpdateDate)
                            {
                                listId.Add(list.Id);
                                T_Attendance attendance;
                                attendance = attendanceService.GetDataAttendanceById(list.Id);
                                attendance.UpdateDate = list.UpdateDate;

                                result = updateStatusFlag(db, attendance, (int)AttendanceStatusFlag.Approved);

                                if (!result)
                                {
                                    break;
                                }
                            }

                            if (result)
                            {
                                attendanceService = new AttendanceService(db);
                                T_AttendanceResult attendanceResult = null;
                                attendanceResult = attendanceService.GetAttendanceApprovalForInsertAttendanceResult(lstDetail.CallendarID, lstDetail.UID, lstDetail.StartDate, lstDetail.EndDate);

                                Config_DService config_DService = new Config_DService(db);
                                IList<VacationDateInFoByAttendanceApproval> configDList;
                                configDList = config_DService.GetListVacationDateForAttendanceApproval(M_Config_H.CONFIG_CD_VACATION_TYPE, lstDetail.StartDate, lstDetail.EndDate, lstDetail.UID);

                                int AttendanceResultId = 0;
                                result = InsertAttendanceResult(db, attendanceResult, ref AttendanceResultId);

                                if (!result)
                                {
                                    break;
                                }

                                foreach (var item in configDList)
                                {

                                    result = InsertVacationResult(db, AttendanceResultId, item);
                                    if (!result)
                                    {
                                        break;
                                    }
                                }

                                IList<T_WorkResult> workResult;
                                Work_DService dService = new Work_DService(db);
                                workResult = dService.getDataWorkResult(listId);

                                if (!result)
                                {
                                    break;
                                }

                                foreach (var item in workResult)
                                {
                                    result = InsertWorkResult(db, AttendanceResultId, item);
                                    if (!result)
                                    {
                                        break;
                                    }
                                }
                            }
                        }

                        if (result)
                        {
                            //Set Success
                            this.Success = true;
                            db.Commit();

                            this.LoadDataGrid();
                        }
                        else
                        {

                            if (isRowApproved)
                            {
                                this.SetMessage(string.Empty, M_Message.MSG_APPROVAL_FAILE);
                            }
                            else
                            {
                                this.SetMessage(string.Empty, M_Message.MSG_DATA_CHANGED);
                            }

                            this.Success = false;
                        }

                    }

                }
                catch (Exception ex)
                {
                    this.SetMessage(string.Empty, M_Message.MSG_UPDATE_FAILE, "更新");
                    Log.Instance.WriteLog(ex);
                }
            }

            if (flagRelease)
            {
                try
                {
                    IList<AttendanceApprovalInfo> listDetail = this.GetData();
                    using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                    {
                        bool result = false;
                        bool isRowRelease = false;
                        List<int> listId = new List<int>();
                        foreach (var lstDetail in listDetail)
                        {
                            if (string.Compare(lstDetail.StatusFlag, ((int)AttendanceStatusFlag.Submitted).ToString().ToString()) == 0)
                            {
                                isRowRelease = true;
                                result = false;
                                break;
                            }
                            IList<AttendanceApprovalIDAndUpdateDate> listIDAndUpdateDate = (IList<AttendanceApprovalIDAndUpdateDate>)ListAttendanceApprovaId[lstDetail.RowNumber];

                            AttendanceService attendanceService = new AttendanceService(db);
                            foreach (var list in listIDAndUpdateDate)
                            {

                                T_Attendance attendance;
                                attendance = attendanceService.GetDataAttendanceById(list.Id);
                                attendance.UpdateDate = list.UpdateDate;

                                result = updateStatusFlag(db, attendance, (int)AttendanceStatusFlag.Submitted);

                                if (!result)
                                {
                                    break;
                                }

                            }

                            if (result)
                            {
                                AttendanceResultService attendanceResultService = new AttendanceResultService(db);
                                VacationResultService vacationResultService = new VacationResultService(db);
                                WorkResultService workResultService = new WorkResultService(db);
                                T_AttendanceResult attendanceResult = null;
                                attendanceResult = attendanceResultService.GetDataAttendanceResultByIdAndStartDate(lstDetail.UID, lstDetail.StartDate);

                                if (attendanceResult != null)
                                {

                                    IList<T_VacationResult> vacationResultList;
                                    IList<T_WorkResult> workResult;
                                    vacationResultList = vacationResultService.GetListVacationResultByHid(attendanceResult.ID);
                                    workResult = workResultService.GetListWorkResultByHid(attendanceResult.ID);

                                    foreach (var item in vacationResultList)
                                    {
                                        //delete
                                        result = DeleteVacationResult(db, item.HID, item.VacationID);

                                        if (!result)
                                        {
                                            break;
                                        }
                                    }

                                    if (!result)
                                    {
                                        break;
                                    }

                                    foreach (var item in workResult)
                                    {
                                        result = DeleteWorkResult(db, item.HID, item.ProjectID);

                                        if (!result)
                                        {
                                            break;
                                        }
                                    }

                                    if (!result)
                                    {
                                        break;
                                    }

                                    result = DeleteAttendanceResult(db, attendanceResult.ID);
                                }

                                if (!result)
                                {
                                    break;
                                }
                            }
                        }

                        if (result)
                        {
                            //Set Success
                            this.Success = true;
                            db.Commit();

                            this.LoadDataGrid();
                        }
                        else
                        {

                            if (isRowRelease)
                            {
                                this.SetMessage(string.Empty, M_Message.MSG_RELEASE_FAILE);
                            }
                            else
                            {
                                this.SetMessage(string.Empty, M_Message.MSG_DATA_CHANGED);
                            }


                            this.Success = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.SetMessage(string.Empty, M_Message.MSG_UPDATE_FAILE, "更新");
                    Log.Instance.WriteLog(ex);
                }
            }
        }

        #region event Click
        /// <summary>
        ///btnApproval_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnApproval_Click(object sender, CommandEventArgs e)
        {
            //Get Data
            IList<AttendanceApprovalInfo> listDetail = this.GetData();

            if (listDetail.Count == 0)
            {
                this.SetMessage(string.Empty, M_Message.MSG_SELECT_ROW_DATA);
                LoadDataForPagingHeader(this.RowNumberToViewState, this.RowNumberFromViewState, false);
                return;
            }

            bool isRowApproved = false;
            foreach (var lstDetail in listDetail)
            {
                if (string.Compare(lstDetail.StatusFlag, ((int)AttendanceStatusFlag.Approved).ToString()) == 0)
                {
                    isRowApproved = true;
                    break;
                }
            }

            if (isRowApproved)
            {
                this.SetMessage(string.Empty, M_Message.MSG_APPROVAL_FAILE);
                LoadDataForPagingHeader(this.RowNumberToViewState, this.RowNumberFromViewState, false);
                return;
            }

            ShowMessageApprovalOrRelease(M_Message.MSG_QUESTION_APPROVAL, Models.DefaultButton.No, true);

            CheckAuthority();

        }

        /// <summary>
        /// Event btnRelease_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRelease_Click(object sender, CommandEventArgs e)
        {
            //Get Data
            IList<AttendanceApprovalInfo> listDetail = this.GetData();

            if (listDetail.Count == 0)
            {
                this.SetMessage(string.Empty, M_Message.MSG_SELECT_ROW_DATA);
                LoadDataForPagingHeader(this.RowNumberToViewState, this.RowNumberFromViewState, false);
                return;
            }

            bool isRowRelease = false;
            foreach (var lstDetail in listDetail)
            {
                if (string.Compare(lstDetail.StatusFlag, ((int)AttendanceStatusFlag.Submitted).ToString()) == 0)
                {
                    isRowRelease = true;
                    break;
                }
            }

            if (isRowRelease)
            {
                this.SetMessage(string.Empty, M_Message.MSG_RELEASE_FAILE);
                LoadDataForPagingHeader(this.RowNumberToViewState, this.RowNumberFromViewState, false);
                return;
            }

            ShowMessageApprovalOrRelease(M_Message.MSG_QUESTION_RELEASE, Models.DefaultButton.No, false);

            CheckAuthority();

        }

        /// <summary>
        /// Event btnDetail_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDetail_Click(object sender, CommandEventArgs e)
        {
            int index = e.CommandArgument.ToString().IndexOf(" ");
            this.ViewState["ID"] = e.CommandArgument.ToString();


            foreach (RepeaterItem item in this.rptAttendanceApprovalList.Items)
            {
                HtmlInputCheckBox chkAttendanceApprovaDel = (HtmlInputCheckBox)item.FindControl("chkSelectlg");
                HiddenField hidUid = (HiddenField)item.FindControl("hinUID");
                HiddenField hidStartdate = (HiddenField)item.FindControl("hinStartDate");

                AttendanceApprovalInfo temp = ListAttendanceApprovalInfoViewState.Where(attendanceApproval => attendanceApproval.UID == int.Parse(hidUid.Value) && attendanceApproval.StartDate == DateTime.Parse(hidStartdate.Value)).SingleOrDefault();

                if (temp != null)
                {
                    temp.CheckFlag = chkAttendanceApprovaDel.Checked;
                }

            }

            //Save condition
            this.SaveCondSearch();
        }

        /// <summary>
        /// Search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (!checkSearchData())
            {
            }
            else
            {
                // Refresh load grid
                this.LoadDataGrid();
            }

            this.Collapse = "in";

            CheckAuthority();
        }

        #endregion

        #endregion

        #region Method
        /// <summary>
        /// Init Data
        /// </summary>
        private void InitData()
        {
            // Default data valide
            this.hdInValideDefault.Value = this.GetDefaultValueForDropdownList(M_Config_H.CONFIG_CD_INVALID_TYPE);
            this.isShowButtonAction = false;

            InitCombobox();
            base.DisabledLink(this.btnApproval, !base._authority.IsMasterNew);
        }

        /// <summary>
        /// Save condition search
        /// </summary>
        private void SaveCondSearch()
        {

            Hashtable hashId = new Hashtable();

            foreach (var item in ListAttendanceApprovalInfoViewState)
            {
                string key = "";
                key = item.UID + "_" + item.StartDate.ToString("yyyyMMdd");
                hashId.Add(key, item.CheckFlag);

            }

            Hashtable hash = new Hashtable();
            hash.Add(this.ddlWorkingCalendar.ID, this.ddlWorkingCalendar.SelectedValue);
            hash.Add(this.ddlDateOfServiceTo.ID, this.ddlDateOfServiceTo.SelectedValue);
            hash.Add(this.ddlDateOfServiceFrom.ID, this.ddlDateOfServiceFrom.SelectedValue);
            hash.Add(this.ddlDepartment.ID, this.ddlDepartment.SelectedValue);
            hash.Add(this.ddlUser.ID, this.ddlUser.SelectedValue);
            hash.Add(this.cmbInvalidData.ID, this.cmbInvalidData.SelectedValue);
            hash.Add("ListCheck", hashId);
            hash.Add("ListAttendanceApprovalInfoViewState", ListAttendanceApprovalInfoViewState);
            hash.Add("ListAttendanceApprovaId", ListAttendanceApprovaId);
            hash.Add("RowNumberToViewState", this.RowNumberToViewState);
            hash.Add("RowNumberFromViewState", this.RowNumberFromViewState);
            hash.Add("NumRowOnPage", this.PagingHeader.NumRowOnPage);
            hash.Add("CurrentPage", this.PagingHeader.CurrentPage);

            this.ViewState["ConditionApproval"] = hash;
        }

        /// <summary>
        /// Show Condition
        /// </summary>
        private void ShowCondition(Hashtable data)
        {

            Hashtable hash = new Hashtable();
            this.LoadWorkingCalendarComboboxData();
            this.ddlWorkingCalendar.SelectedValue = data[this.ddlWorkingCalendar.ID].ToString();

            this.LoadDdlDateOfServiceCombobox();

            this.LoadDepartmentComboboxData();

            this.LoadInvalidDataCombobox();

            this.ddlDateOfServiceFrom.SelectedValue = data[this.ddlDateOfServiceFrom.ID].ToString();
            this.ddlDateOfServiceTo.SelectedValue = data[this.ddlDateOfServiceTo.ID].ToString();
            this.ddlDepartment.SelectedValue = data[this.ddlDepartment.ID].ToString();
            this.cmbInvalidData.SelectedValue = data[this.cmbInvalidData.ID].ToString();
            this.ddlUser.SelectedValue = data[this.ddlUser.ID].ToString();
            hash = (Hashtable)data["ListCheck"];
            this.ListAttendanceApprovaId = (Hashtable)data["ListAttendanceApprovaId"];
            this.ListAttendanceApprovalInfoViewState = (IList<AttendanceApprovalInfo>)data["ListAttendanceApprovalInfoViewState"];

            this.RowNumberToViewState = int.Parse(data["RowNumberToViewState"].ToString());
            this.RowNumberFromViewState = int.Parse(data["RowNumberFromViewState"].ToString());
            int curPage = int.Parse(data["CurrentPage"].ToString());
            this.PagingHeader.CurrentPage = curPage;
            this.PagingFooter.CurrentPage = curPage;

            int rowOfPage = int.Parse(data["NumRowOnPage"].ToString());
            this.PagingHeader.NumRowOnPage = rowOfPage;

            foreach (var item in ListAttendanceApprovalInfoViewState)
            {
                string key = "";
                key = item.UID + "_" + item.StartDate.ToString("yyyyMMdd");

                item.CheckFlag = (bool)hash[key];

            }
        }

        /// <summary>
        /// check data by btn search
        /// </summary>
        /// <returns></returns>
        private bool checkSearchData()
        {
            bool result = true;

            if (this.ddlWorkingCalendar.SelectedValue == string.Empty || this.ddlWorkingCalendar.SelectedValue == null)
            {

                result = false;
                this.SetMessage(this.ddlWorkingCalendar.ID, M_Message.MSG_REQUIRE, "勤務カレンダー");
            }

            if (this.ddlDateOfServiceFrom.SelectedValue != "-1" && this.ddlDateOfServiceTo.SelectedValue != "-1")
            {

                DateTime startDateFrom = DateTime.MinValue;

                DateTime startDateTo = DateTime.MinValue;
                if (!string.IsNullOrEmpty(this.ddlDateOfServiceFrom.SelectedValue))
                {
                    startDateFrom = DateTime.Parse(this.ddlDateOfServiceFrom.SelectedValue);
                    startDateTo = DateTime.Parse(this.ddlDateOfServiceTo.SelectedValue);
                }

                if (startDateFrom > startDateTo)
                {

                    result = false;
                    this.SetMessage(this.ddlDateOfServiceFrom.ID, M_Message.MSG_LESS_THAN_EQUAL, "勤務年月From", "勤務年月To");
                }

            }
            return result;
        }

        /// <summary>
        /// Init Combobox
        /// </summary>
        private void InitCombobox()
        {
            this.LoadWorkingCalendarComboboxData();

            this.LoadDdlDateOfServiceCombobox();

            this.LoadDepartmentComboboxData();

            //Load user combobox data
            this.LoadUserComboboxData();

            this.LoadInvalidDataCombobox();
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
        /// CheckAuthority
        /// </summary>
        private void CheckAuthority()
        {

            base.DisabledLink(this.btnApproval, !base._authority.IsAttendanceApprovalApprovel);
            base.DisabledLink(this.btnApprovalButtom, !base._authority.IsAttendanceApprovalApprovel);

            base.DisabledLink(this.btnRelease, !base._authority.IsAttendanceApprovalReject);
            base.DisabledLink(this.btnReleaseButtom, !base._authority.IsAttendanceApprovalReject);
        }

        #region loadData

        /// <summary>
        /// load data gird
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="numOnPage"></param>
        private void LoadDataGrid()
        {
            ListAttendanceApprovaId = new Hashtable();
            this.RowNumberToViewState = 0;
            this.RowNumberFromViewState = this.PagingHeader.NumRowOnPage;
            IList<AttendanceApprovalInfo> listAttendanceApproval = null;
            //  DataTable AttendanceApprovalListInfo;
            try
            {
                using (DB db = new DB())
                {
                    AttendanceService attendanceService = new AttendanceService(db);

                    listAttendanceApproval = attendanceService.GetListAttendanceApprovalByCond(ddlWorkingCalendar.SelectedValue, this.ddlUser.SelectedValue, ddlDateOfServiceFrom.SelectedValue, ddlDateOfServiceTo.SelectedValue, ddlDepartment.SelectedValue, cmbInvalidData.SelectedValue);
                    ListAttendanceApprovalInfoViewState = listAttendanceApproval;

                    //   AttendanceService attendanceService = new AttendanceService(db);
                    IList<AttendanceApprovalIDAndUpdateDate> attendanceApprovalIDAndUpdateDate;

                    foreach (var item in listAttendanceApproval)
                    {
                        attendanceApprovalIDAndUpdateDate = attendanceService.getListIdAndUpdateDate(item.StartDate, item.EndDate, item.UID);
                        ListAttendanceApprovaId.Add(item.RowNumber, attendanceApprovalIDAndUpdateDate);
                    }

                }

                if (listAttendanceApproval.Count == 0)
                {
                    this.rptAttendanceApprovalList.DataSource = null;
                    this.rptAttendanceApprovalList.DataBind();
                    isShowButtonAction = false;

                    this.PagingHeader.TotalRow = 0;
                    this.PagingHeader.CurrentPage = 0;

                }
                else
                {

                    isShowButtonAction = true;
                    this.PagingHeader.CurrentPage = 1;

                    // paging footer
                    this.PagingFooter.CurrentPage = 1;
                    LoadDataForPagingHeader(0, this.PagingHeader.NumRowOnPage, false);
                }

                CheckAuthority();
            }
            catch (Exception e)
            {
                Log.Instance.WriteLog(e);
            }
        }

        /// <summary>
        /// LoadDataForPaging
        /// </summary>
        private void LoadDataForPagingHeader(int RowNumberTo, int RowNumberFrom, bool getForm = true)
        {
            this.RowNumberToViewState = RowNumberTo;
            this.RowNumberFromViewState = RowNumberFrom;
            IList<AttendanceApprovalInfo> attendanceApprovalInfo = null;
            DataTable AttendanceApprovalListInfo;
            if (getForm)
            {
                foreach (RepeaterItem item in this.rptAttendanceApprovalList.Items)
                {
                    HtmlInputCheckBox chkAttendanceApprovaDel = (HtmlInputCheckBox)item.FindControl("chkSelectlg");
                    HiddenField hidUid = (HiddenField)item.FindControl("hinUID");
                    HiddenField hidStartdate = (HiddenField)item.FindControl("hinStartDate");

                    AttendanceApprovalInfo temp = ListAttendanceApprovalInfoViewState.Where(attendanceApproval => attendanceApproval.UID == int.Parse(hidUid.Value) && attendanceApproval.StartDate == DateTime.Parse(hidStartdate.Value)).SingleOrDefault();

                    if (temp != null)
                    {
                        temp.CheckFlag = chkAttendanceApprovaDel.Checked;
                    }

                }
            }


            attendanceApprovalInfo = (IList<AttendanceApprovalInfo>)ListAttendanceApprovalInfoViewState.Where(item => item.RowNumber > RowNumberTo && item.RowNumber <= RowNumberFrom).ToList();

            if (attendanceApprovalInfo.Count > 0)
            {
                // paging header
                this.PagingHeader.RowNumFrom = int.Parse(attendanceApprovalInfo[0].RowNumber.ToString());
                this.PagingHeader.RowNumTo = int.Parse(attendanceApprovalInfo[attendanceApprovalInfo.Count - 1].RowNumber.ToString());
                this.PagingHeader.TotalRow = ListAttendanceApprovalInfoViewState.Count;
                this.PagingHeader.CurrentPage = this.PagingHeader.CurrentPage;

                // paging footer
                this.PagingFooter.CurrentPage = this.PagingHeader.CurrentPage;
                this.PagingFooter.NumberOnPage = this.PagingHeader.NumRowOnPage;
                this.PagingFooter.TotalRow = ListAttendanceApprovalInfoViewState.Count;

                AttendanceApprovalListInfo = ConvertListToDataTable(attendanceApprovalInfo);
                this.rptAttendanceApprovalList.DataSource = AttendanceApprovalListInfo;

                isShowButtonAction = true;
            }
            else
            {
                this.rptAttendanceApprovalList.DataSource = null;
                isShowButtonAction = false;
            }

            this.rptAttendanceApprovalList.DataBind();
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
                T_WorkingCalendar_HService workingCalendarService = new T_WorkingCalendar_HService(db);
                workingCalendarList = workingCalendarService.GetWorkingCalendarCbbData(ref defaultVal, LoginInfo.User.ID);
                ddlWorkingCalendar.Items.Clear();

                if (workingCalendarList.Count > 0)
                {
                    hdCalendarDefault.Value = defaultVal;
                    ddlWorkingCalendar.DataSource = workingCalendarList;
                    ddlWorkingCalendar.DataValueField = "Value";
                    ddlWorkingCalendar.DataTextField = "DisplayName";
                    ddlWorkingCalendar.SelectedValue = hdCalendarDefault.Value;
                    ddlWorkingCalendar.DataBind();

                }
                else
                {
                    ddlWorkingCalendar.DataSource = null;
                    ddlWorkingCalendar.DataBind();
                }
            }
        }

        /// <summary>
        /// Load Department Combobox data
        /// </summary>
        private void LoadDepartmentComboboxData()
        {
            using (DB db = new DB())
            {
                IList<DropDownModel> departmentList;
                DepartmentService departmentService = new DepartmentService(db);
                departmentList = departmentService.GetDepartmentCbbData(int.Parse(this.ddlWorkingCalendar.SelectedValue));
                ddlDepartment.Items.Clear();

                ddlDepartment.DataSource = setDataTableCombobox(departmentList);
                ddlDepartment.DataValueField = "Value";
                ddlDepartment.DataTextField = "DisplayName";
                ddlDepartment.DataBind();

            }
        }

        /// <summary>
        /// Load user combobox data
        /// </summary>
        private void LoadUserComboboxData()
        {
            using (DB db = new DB())
            {
                IList<DropDownModel> userList;
                UserService userService = new UserService(db);
                userList = userService.GetCbbUserDataByDepartmentID(int.Parse(ddlDepartment.SelectedValue), int.Parse(ddlWorkingCalendar.SelectedValue));
                ddlUser.Items.Clear();
                this.UserList = userList;

                ddlUser.DataSource = setDataTableCombobox(userList);
                ddlUser.DataValueField = "Value";
                ddlUser.DataTextField = "DisplayName";

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
        /// Combobox Invalid Data
        /// </summary>
        private void LoadInvalidDataCombobox()
        {
            // init combox 
            cmbInvalidData.Items.Clear();
            cmbInvalidData.DataSource = this.GetDataForDropdownList(M_Config_H.CONFIG_CD_INVALID_TYPE);
            cmbInvalidData.DataValueField = "Value";
            cmbInvalidData.DataTextField = "DisplayName";
            cmbInvalidData.DataBind();
            cmbInvalidData.SelectedValue = this.hdInValideDefault.Value;
        }

        /// <summary>
        /// Load data DateOfService Combobox
        /// </summary>
        private void LoadDdlDateOfServiceCombobox()
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
            }

        }
        #endregion

        #region get data

        /// <summary>
        /// GetData gird
        /// </summary>
        /// <returns></returns>
        private IList<AttendanceApprovalInfo> GetData()
        {
            IList<AttendanceApprovalInfo> ret = new List<AttendanceApprovalInfo>();

            foreach (RepeaterItem item in this.rptAttendanceApprovalList.Items)
            {
                HtmlInputCheckBox chkAttendanceApprovaDel = (HtmlInputCheckBox)item.FindControl("chkSelectlg");
                HiddenField hidUid = (HiddenField)item.FindControl("hinUID");
                HiddenField hidStartdate = (HiddenField)item.FindControl("hinStartDate");

                AttendanceApprovalInfo temp = ListAttendanceApprovalInfoViewState.Where(attendanceApproval => attendanceApproval.UID == int.Parse(hidUid.Value) && attendanceApproval.StartDate == DateTime.Parse(hidStartdate.Value)).SingleOrDefault();

                if (temp != null)
                {
                    temp.CheckFlag = chkAttendanceApprovaDel.Checked;
                }

            }

            foreach (var item in ListAttendanceApprovalInfoViewState)
            {

                if (!item.CheckFlag)
                {
                    continue;
                }

                ret.Add(item);
            }

            return ret;
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
        /// getVacationConfig
        /// </summary>
        /// <param name="rpChildContentRested"></param>
        /// <param name="constants"></param>
        /// <param name="startDate"></param>
        /// <param name="endStart"></param>
        /// <param name="uId"></param>
        private void getVacationConfig(Repeater rpChildContentRested, string constants, DateTime startDate, DateTime endStart, int uId)
        {
            DataTable VacationConfigInfo;
            using (DB db = new DB())
            {
                Config_DService config_DService = new Config_DService(db);

                IList<VacationDateInFoByAttendanceApproval> configDList;

                configDList = config_DService.GetListVacationDateForAttendanceApproval(constants, startDate, endStart, uId);

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
        #endregion

        #region set data
        /// <summary>
        /// ConvertListToDataTable
        /// </summary>
        /// <param name="listAttendanceDetailInfo"></param>
        /// <returns></returns>
        private DataTable ConvertListToDataTable(IList<AttendanceApprovalInfo> AttendanceApprovalInfolist)
        {
            try
            {
                DataTable dataAttendanceDetailInfo = new DataTable();
                dataAttendanceDetailInfo.Columns.Add("RowNumber", typeof(int));
                dataAttendanceDetailInfo.Columns.Add("UID", typeof(int));
                dataAttendanceDetailInfo.Columns.Add("CallendarID", typeof(int));
                dataAttendanceDetailInfo.Columns.Add("UserCD", typeof(string));
                dataAttendanceDetailInfo.Columns.Add("UserNm", typeof(string));
                dataAttendanceDetailInfo.Columns.Add("DepartmentID", typeof(int));
                dataAttendanceDetailInfo.Columns.Add("DepartmentName", typeof(string));
                dataAttendanceDetailInfo.Columns.Add("InitialDate", typeof(DateTime));
                dataAttendanceDetailInfo.Columns.Add("Month", typeof(int));
                dataAttendanceDetailInfo.Columns.Add("StartDate", typeof(DateTime));
                dataAttendanceDetailInfo.Columns.Add("EndDate", typeof(DateTime));
                dataAttendanceDetailInfo.Columns.Add("RangeDate", typeof(string));
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
                dataAttendanceDetailInfo.Columns.Add("StatusFlag", typeof(string));
                dataAttendanceDetailInfo.Columns.Add("CheckFlag", typeof(bool));
                dataAttendanceDetailInfo.Columns.Add("NameStatus", typeof(string));
                dataAttendanceDetailInfo.Columns.Add("CssNameStatus", typeof(string));

                // int rowIndex = 0;
                foreach (AttendanceApprovalInfo item in AttendanceApprovalInfolist)
                {
                    DataRow dr = dataAttendanceDetailInfo.NewRow();
                    dr["RowNumber"] = item.RowNumber;
                    dr["UID"] = item.UID;
                    dr["CallendarID"] = item.CallendarID;

                    dr["UserCD"] = Utilities.EditDataUtil.ToFixCodeShow(item.UserCD, M_User.MAX_USER_CODE_SHOW); ;
                    dr["UserNm"] = item.UserName1;
                    dr["DepartmentID"] = item.DepartmentID;
                    dr["DepartmentName"] = item.DepartmentName;


                    dr["InitialDate"] = item.InitialDate;
                    dr["Month"] = item.Month;
                    dr["RangeDate"] = item.StartDate.ToString("yyyy/MM") + "~" + item.EndDate.ToString("yyyy/MM");
                    dr["StartDate"] = item.StartDate;
                    dr["EndDate"] = item.EndDate;

                    dr["WorkingHours"] = item.WorkingHours.ToString() != "0" ? item.WorkingHours : "&nbsp;";
                    dr["LateHours"] = item.LateHours.ToString() != "0" ? item.LateHours : "&nbsp;";
                    dr["EarlyHours"] = item.EarlyHours.ToString() != "0" ? item.EarlyHours : "&nbsp;";
                    dr["SH_Hours"] = item.SH_Hours.ToString() != "0" ? item.SH_Hours : "&nbsp;";
                    dr["LH_Hours"] = item.LH_Hours.ToString() != "0" ? item.LH_Hours : "&nbsp;";

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
                    dr["StatusFlag"] = item.StatusFlag;

                    if (item.StatusFlag == ((int)AttendanceStatusFlag.Submitted).ToString())
                    {
                        dr["NameStatus"] = "未承認";
                        dr["CssNameStatus"] = "label label-default";

                    }
                    else
                    {
                        dr["NameStatus"] = "承認済";
                        dr["CssNameStatus"] = "label label-primary";
                    }

                    dr["CheckFlag"] = item.CheckFlag;

                    dataAttendanceDetailInfo.Rows.Add(dr);
                    // rowIndex++;
                }
                dataAttendanceDetailInfo.AcceptChanges();

                DataView dataAttendanceDetailInfoView = new DataView(dataAttendanceDetailInfo);

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
        /// ConvertConfigDListToDataTable
        /// </summary>
        /// <param name="vacationDateInFoByAttendanceApproval"></param>
        /// <returns></returns>
        private DataTable ConvertConfigDListToDataTable(IList<VacationDateInFoByAttendanceApproval> vacationDateInFoByAttendanceApproval)
        {
            try
            {
                DataTable dataVacationDDetailInfo = new DataTable();
                dataVacationDDetailInfo.Columns.Add("Value1", typeof(int));
                dataVacationDDetailInfo.Columns.Add("Value3", typeof(string));
                dataVacationDDetailInfo.Columns.Add("VacationDate", typeof(string));

                foreach (VacationDateInFoByAttendanceApproval item in vacationDateInFoByAttendanceApproval)
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
        #endregion

        #region delete data

        /// <summary>
        /// delete AttendanceResult
        /// </summary>
        /// <param name="db"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool DeleteAttendanceResult(DB db, int id)
        {
            try
            {
                int ret = 0;

                AttendanceResultService attendanceResultService = new AttendanceResultService(db);
                ret = attendanceResultService.Delete(id);

                if (ret > 0)
                {
                    return true;
                }


                if (ret == 0)
                {
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
        /// DeleteVacationResult
        /// </summary>
        /// <param name="db"></param>
        /// <param name="hId"></param>
        /// <param name="vacationID"></param>
        /// <returns></returns>
        private bool DeleteVacationResult(DB db, int hId, int vacationID)
        {
            try
            {
                int ret = 0;

                VacationResultService vacationResultService = new VacationResultService(db);
                ret = vacationResultService.Delete(hId, vacationID);

                if (ret > 0)
                {
                    return true;
                }


                if (ret == 0)
                {
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
        /// DeleteWorkResult
        /// </summary>
        /// <param name="db"></param>
        /// <param name="hId"></param>
        /// <param name="projectID"></param>
        /// <returns></returns>
        private bool DeleteWorkResult(DB db, int hId, int projectID)
        {
            try
            {
                int ret = 0;

                WorkResultService workResultService = new WorkResultService(db);
                ret = workResultService.Delete(hId, projectID);

                if (ret > 0)
                {
                    return true;
                }


                if (ret == 0)
                {
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

        #endregion

        #region insert data

        /// <summary>
        /// InsertAttendanceResult
        /// </summary>
        /// <param name="db"></param>
        /// <param name="attendanceResult"></param>
        /// <param name="newId"></param>
        /// <returns>Success:true, Faile:false</returns>
        private bool InsertAttendanceResult(DB db, T_AttendanceResult attendanceResult, ref int newId)
        {
            try
            {
                AttendanceResultService attendanceResultService = new AttendanceResultService(db);

                attendanceResult.CreateUID = this.LoginInfo.User.ID;
                attendanceResult.UpdateUID = this.LoginInfo.User.ID;

                //Insert User
                attendanceResultService.Insert(attendanceResult);
                newId = db.GetIdentityId<T_AttendanceResult>();
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
        /// InsertVacationResult
        /// </summary>
        /// <param name="db"></param>
        /// <param name="Hid"></param>
        /// <param name="config"></param>
        /// <returns>Success:true, Faile:false</returns>
        private bool InsertVacationResult(DB db, int Hid, VacationDateInFoByAttendanceApproval config)
        {
            try
            {
                VacationResultService attendanceResultService = new VacationResultService(db);

                //Insert User
                attendanceResultService.Insert(Hid, config);
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
        /// InsertWorkResult
        /// </summary>
        /// <param name="db"></param>
        /// <param name="Hid"></param>
        /// <param name="workResult"></param>
        /// <returns>Success:true, Faile:false</returns>
        private bool InsertWorkResult(DB db, int Hid, T_WorkResult workResult)
        {
            try
            {
                WorkResultService workResultService = new WorkResultService(db);

                //Insert User
                workResultService.Insert(Hid, workResult);
            }
            catch (Exception ex)
            {
                this.SetMessage(string.Empty, M_Message.MSG_UPDATE_FAILE, "新規");

                Log.Instance.WriteLog(ex);

                return false;
            }

            return true;
        }

        #endregion

        #region update data

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

        #endregion

        #region show messdialog
        /// <summary>
        /// Show message question when btnApproval_Click OR btnRelease
        /// </summary>
        /// <param name="messageID">MessageID</param>
        /// <param name="defaultButton">Default Button</param>
        protected void ShowMessageApprovalOrRelease(string messageID, DefaultButton defaultButton, bool isApprovalbool = false, params string[] args)
        {
            flagApproval = isApprovalbool;
            flagRelease = !isApprovalbool;

            //Get Message
            M_Message mess = (M_Message)this.Messages[messageID];
            HtmlGenericControl questionMessage = (HtmlGenericControl)this.Master.FindControl("questionMessage");
            questionMessage.InnerHtml = "<p>" + " " + string.Format(mess.Message3, args) + "</p>";

            this.IsShowQuestion = true;

            LinkButton btnNo = (LinkButton)this.Master.FindControl("btnNo");
            btnNo.Visible = true;
            if (defaultButton == Models.DefaultButton.Yes)
            {
                this.DefaultButton = "#btnYes";
            }
            else
            {

                this.DefaultButton = "#btnNo";
            }
        }
        #endregion

        #endregion

    }
}