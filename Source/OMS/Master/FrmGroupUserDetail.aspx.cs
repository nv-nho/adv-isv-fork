using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using OMS.DAC;
using OMS.Models;
using OMS.Utilities;

namespace OMS.Master
{
    /// <summary>
    /// FrmGroupUserDetail
    /// Create  : isv.thuy
    /// Date    : 24/07/2014
    /// </summary>
    public partial class FrmGroupUserDetail : FrmBaseDetail
    {
        private const string URL_LIST = "~/Master/FrmGroupUserList.aspx";
        #region Cosntant

        /// <summary>
        /// Default admin id
        /// </summary>
        public const int ADMIN_ID = 1;

        #endregion

        #region Property

        /// <summary>
        /// Get or set GroupID
        /// </summary>
        public int GroupID
        {
            get { return (int)ViewState["GroupID"]; }
            set { ViewState["GroupID"] = value; }
        }

        /// <summary>
        /// Get or set OldUpdateDate
        /// </summary>
        public DateTime OldUpdateDate
        {
            get { return (DateTime)ViewState["OldUpdateDate"]; }
            set { ViewState["OldUpdateDate"] = value; }
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
            base.FormTitle = "Group Master";
            base.FormSubTitle = "Detail";

            this.txtGroupCode.MaxLength = M_GroupUser_H.GROUP_CODE_SHOW_MAX_LENGTH;
            this.txtGroupName.MaxLength = M_GroupUser_H.GROUP_NAME_MAX_LENGTH;

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
            base.SetAuthority(FormId.GroupUser);
            if (!this._authority.IsMasterView)
            {
                Response.Redirect("~/Menu/FrmMasterMenu.aspx");
            }

            if (!this.IsPostBack)
            {
                if (this.PreviousPage != null)
                {
                    //Save condition of previous page
                    this.ViewState["Condition"] = this.PreviousPageViewState["Condition"];

                    //Check mode
                    if (this.PreviousPageViewState["ID"] == null)
                    {
                        //Set mode
                        this.ProcessMode(Mode.Insert);
                        this.InitDetailCtrl();
                    }
                    else
                    {
                        //Get User ID
                        this.GroupID = int.Parse(PreviousPageViewState["ID"].ToString());
                        M_GroupUser_H m_group_H = this.GetDataGroupById(this.GroupID);

                        //Check user
                        if (m_group_H != null)
                        {
                            //Show data
                            this.ShowData(m_group_H);

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
                    this.InitDetailCtrl();
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
            if (this.Mode == Mode.View || this.Mode == Mode.Insert)
            {
                Server.Transfer(URL_LIST);
            }
            else if (this.Mode == Mode.Update || this.Mode == Mode.Copy)
            {
                //Get User
                M_GroupUser_H m_group = this.GetDataGroupById(this.GroupID);

                //Check user
                if (m_group != null)
                {
                    //Set Mode
                    this.ProcessMode(Mode.View);

                    //Show data
                    this.ShowData(m_group);
                }
                else
                {
                    Server.Transfer(URL_LIST);
                }
            }
        }

        /// <summary>
        /// Copy Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCopy_Click(object sender, EventArgs e)
        {
            M_GroupUser_H m_GroupUser = this.GetDataGroupById(this.GroupID);

            if (m_GroupUser != null)
            {
                //Show data
                this.ShowData(m_GroupUser);

                //Set Mode
                this.ProcessMode(Mode.Copy);
            }
            else
            {
                Server.Transfer(URL_LIST);
            }
        }

        /// <summary>
        /// Edit Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            M_GroupUser_H m_GroupUser = this.GetDataGroupById(this.GroupID);

            if (m_GroupUser != null)
            {
                //Show data
                this.ShowData(m_GroupUser);

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
            SetConfirmHeader();
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
            SetConfirmHeader();
            //Check input
            if (!this.CheckInput())
            {
                return;
            }

            //Show question update
            base.ShowQuestionMessage(M_Message.MSG_QUESTION_UPDATE, Models.DefaultButton.Yes);
        }

        /// <summary>
        /// Init header
        /// </summary>
        private void SetConfirmHeader()
        {
            // repeater Form Header
            List<ListItem> lstHeader = new List<ListItem>();
            lstHeader.Add(new ListItem("0", "-1"));
            Array items = Enum.GetValues(typeof(AuthorTypeMaster));
            for (int i = 0; i < items.Length; i++)
            {

                string val = "1";
                foreach (RepeaterItem item in this.rptFormDetail.Items)
                {
                    if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
                    {
                        HtmlInputCheckBox chkAuth = (HtmlInputCheckBox)item.FindControl(string.Format("chkAuth{0}", i + 1));
                        if (!chkAuth.Checked)
                        {
                            val = "0";
                            break;
                        }
                    }
                }
                string description = val + GetEnumDescription((AuthorTypeMaster)items.GetValue(i));
                lstHeader.Add(new ListItem(description, (i + 1).ToString()));
            }

            this.rptFormHeader.DataSource = lstHeader;
            this.rptFormHeader.DataBind();
        }

        /// <summary>
        /// Event changed page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNew_Click(object sender, CommandEventArgs e)
        {
            this.txtGroupCode.Value = string.Empty;
            this.txtGroupName.Value = string.Empty;
            this.InitDetailCtrl();
            var demi = new M_GroupUser_D();

            //Set mode
            this.ProcessMode(Mode.Insert);

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

                    //Insert Data
                    if (this.InsertData())
                    {
                        M_GroupUser_H m_GroupUser = this.GetDataGroupByCd(this.txtGroupCode.Value);

                        //Show data
                        this.ShowData(m_GroupUser);

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

                    //Update Data
                    if (this.UpdateData())
                    {
                        M_GroupUser_H m_GroupUser = this.GetDataGroupById(this.GroupID);

                        //Set Mode
                        this.ProcessMode(Mode.View);

                        //Show data
                        this.ShowData(m_GroupUser);

                        //Set Success
                        this.Success = true;
                    }

                    break;

            }
        }

        /// <summary>
        /// Show Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShowData(object sender, EventArgs e)
        {
            M_GroupUser_H m_group_H = this.GetDataGroupById(this.GroupID);

            if (m_group_H != null)
            {

                //Show data
                this.ShowData(m_group_H);

                //Set Mode
                this.ProcessMode(Mode.View);
            }
            else
            {
                Server.Transfer(URL_LIST);
            }
        }

        /// <summary>
        /// rptFormDetail Item Data Bound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptFormDetail_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlInputCheckBox chkAuth2 = (HtmlInputCheckBox)e.Item.FindControl("chkAuth2");
                HtmlInputCheckBox chkAuth3 = (HtmlInputCheckBox)e.Item.FindControl("chkAuth3");
                HtmlInputCheckBox chkAuth4 = (HtmlInputCheckBox)e.Item.FindControl("chkAuth4");
                HtmlInputCheckBox chkAuth5 = (HtmlInputCheckBox)e.Item.FindControl("chkAuth5");
                HtmlInputCheckBox chkAuth6 = (HtmlInputCheckBox)e.Item.FindControl("chkAuth6");

                if (chkAuth6 != null)
                {
                    chkAuth6.Visible = false;
                }

                FormId index = (FormId)((M_GroupUser_D)e.Item.DataItem).FormID;
                if (index == FormId.SendMail)
                {
                    chkAuth4.Visible = false;
                }

                if (index == FormId.Cost)
                {
                    chkAuth2.Visible = false;
                    chkAuth4.Visible = false;
                    chkAuth5.Visible = false;
                }

                if (index == FormId.ProjectProfit)
                {
                    chkAuth2.Visible = false;
                    chkAuth3.Visible = false;
                    chkAuth4.Visible = false;
                    chkAuth5.Visible = false;
                }
            }
        }

        /// <summary>
        /// Get Format GroupCode
        /// </summary>
        /// <param name="in1"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string GetGroup(string in1)
        {
            return EditDataUtil.JsonSerializer(new
            {
                txtGroupCode = EditDataUtil.ToFixCodeShow(EditDataUtil.ToFixCodeDB(in1, M_GroupUser_H.GROUP_CODE_DB_MAX_LENGTH), M_GroupUser_H.GROUP_CODE_SHOW_MAX_LENGTH)
            });
        }
        #endregion

        #region Methods

        public string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        private short IsCheckAll(List<M_GroupUser_D> lstDetail, int index)
        {
            int idx = 0;
            int cnt = lstDetail.Count;
            foreach (var item in lstDetail)
            {
                // 新規
                if (index == 1 && idx == cnt - 2)
                {
                    return 1;
                }

                //　編集
                if (index == 2 && idx == cnt - 1)
                {
                    return 1;
                }

                //　複写
                if (index == 3 && idx == cnt - 3)
                {
                    return 1;
                }

                //　削除
                if (index == 4 && idx == cnt - 2)
                {
                    return 1;
                }

                PropertyInfo auth = item.GetType().GetProperty(string.Format("AuthorityFlag{0}", index + 1));
                short fld = short.Parse(auth.GetValue(item, null).ToString());
                if (fld == 0)
                {
                    return 0;
                }

                idx++;
            }
            return 1;
        }

        /// <summary>
        /// Init header
        /// </summary>
        private void InitHeader(List<M_GroupUser_D> lstDetail)
        {
            // repeater Form Header
            List<ListItem> lstHeader = new List<ListItem>();
            lstHeader.Add(new ListItem("0", "-1"));
            Array items = Enum.GetValues(typeof(AuthorTypeMaster));
            for (int i = 0; i < items.Length; i++)
            {
                string description = this.IsCheckAll(lstDetail, i).ToString() + GetEnumDescription((AuthorTypeMaster)items.GetValue(i));
                lstHeader.Add(new ListItem(description, (i + 1).ToString()));
            }

            this.rptFormHeader.DataSource = lstHeader;
            this.rptFormHeader.DataBind();

            //WorkCalendarHeader
            List<ListItem> lstWorkCalendarHeader = new List<ListItem>();
            Array itemWorkCalendar = Enum.GetValues(typeof(AuthorTypeWorkCalendar));
            for (int i = 0; i < itemWorkCalendar.Length; i++)
            {
                string description = GetEnumDescription((AuthorTypeWorkCalendar)itemWorkCalendar.GetValue(i));
                lstWorkCalendarHeader.Add(new ListItem(description, (i + 1).ToString()));
            }

            this.rptFormWorkCalendarHeader.DataSource = lstWorkCalendarHeader;
            this.rptFormWorkCalendarHeader.DataBind();

            //WorkScheHeader
            List<ListItem> lstWorkScheduleHeader = new List<ListItem>();
            Array itemlstWorkScheduleHeader = Enum.GetValues(typeof(AuthorTypeWorkSchedule));
            for (int i = 0; i < itemlstWorkScheduleHeader.Length; i++)
            {
                string description = GetEnumDescription((AuthorTypeWorkSchedule)itemlstWorkScheduleHeader.GetValue(i));
                lstWorkScheduleHeader.Add(new ListItem(description, (i + 1).ToString()));
            }

            this.rptFormWorkScheduleHeader.DataSource = lstWorkScheduleHeader;
            this.rptFormWorkScheduleHeader.DataBind();

            //AttendanceApproval
            List<ListItem> lstAttendanceApprovalHeader = new List<ListItem>();
            Array itemlstAttendanceApprovalHeader = Enum.GetValues(typeof(AuthorTypeAttendanceApproval));
            for (int i = 0; i < itemlstAttendanceApprovalHeader.Length; i++)
            {
                string description = GetEnumDescription((AuthorTypeAttendanceApproval)itemlstAttendanceApprovalHeader.GetValue(i));
                lstAttendanceApprovalHeader.Add(new ListItem(description, (i + 1).ToString()));
            }

            this.rptAttendanceApprovalHeader.DataSource = lstAttendanceApprovalHeader;
            this.rptAttendanceApprovalHeader.DataBind();

            //AttendanceSummary
            List<ListItem> lstAttendanceSummaryHeader = new List<ListItem>();
            Array itemlstAttendanceSummaryHeader = Enum.GetValues(typeof(AuthorTypeAttendanceSummary));
            for (int i = 0; i < itemlstAttendanceSummaryHeader.Length; i++)
            {
                string description = GetEnumDescription((AuthorTypeAttendanceSummary)itemlstAttendanceSummaryHeader.GetValue(i));
                lstAttendanceSummaryHeader.Add(new ListItem(description, (i + 1).ToString()));
            }

            this.rptAttendanceSummaryHeader.DataSource = lstAttendanceSummaryHeader;
            this.rptAttendanceSummaryHeader.DataBind();

            //AttendancePayslip
            List<ListItem> lstAttendancePayslipHeader = new List<ListItem>();
            Array itemlstAttendancePayslipHeader = Enum.GetValues(typeof(AuthorTypeAttendancePayslip));
            for (int i = 0; i < itemlstAttendancePayslipHeader.Length; i++)
            {
                string description = GetEnumDescription((AuthorTypeAttendancePayslip)itemlstAttendancePayslipHeader.GetValue(i));
                lstAttendancePayslipHeader.Add(new ListItem(description, (i + 1).ToString()));
            }

            this.rptAttendancePayslipHeader.DataSource = lstAttendancePayslipHeader;
            this.rptAttendancePayslipHeader.DataBind();

            //ExpenceGroup
            List<ListItem> lstExpenceGroupHeader = new List<ListItem>();
            Array itemlstExpenceGroupHeader = Enum.GetValues(typeof(AuthorTypeExpenceGroup));
            for (int i = 0; i < itemlstExpenceGroupHeader.Length; i++)
            {
                string description = GetEnumDescription((AuthorTypeExpenceGroup)itemlstExpenceGroupHeader.GetValue(i));
                lstExpenceGroupHeader.Add(new ListItem(description, (i + 1).ToString()));
            }

            this.RptExpenceGroupHeader.DataSource = lstExpenceGroupHeader;
            this.RptExpenceGroupHeader.DataBind();

            //Approval
            List<ListItem> lstApprovalHeader = new List<ListItem>();
            Array itemlstApprovalHeader = Enum.GetValues(typeof(AuthorTypeApproval));
            for (int i = 0; i < itemlstApprovalHeader.Length; i++)
            {
                string description = GetEnumDescription((AuthorTypeApproval)itemlstApprovalHeader.GetValue(i));
                lstApprovalHeader.Add(new ListItem(description, (i + 1).ToString()));
            }

            this.rptApprovalHeader.DataSource = lstApprovalHeader;
            this.rptApprovalHeader.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitDetailCtrl()
        {
            // repeater Detail
            List<M_GroupUser_D> lstDetail = new List<M_GroupUser_D>();
            Array items = Enum.GetValues(typeof(FormId));
            int i;
            string description;
            for (i = (int)FormId.User; i <= (int)FormId.SendMail; i++)
            {
                description = GetEnumDescription((FormId)items.GetValue(i - 1));
                lstDetail.Add(setDataRow(i, description));
            }

            description = GetEnumDescription(FormId.Cost);
            lstDetail.Add(setDataRow((int)FormId.Cost, description));

            description = GetEnumDescription(FormId.ProjectProfit);
            lstDetail.Add(setDataRow((int)FormId.ProjectProfit, description));

            this.rptFormDetail.DataSource = lstDetail;
            this.rptFormDetail.DataBind();

            this.InitHeader(lstDetail);

        }

        /// <summary>
        /// set data row
        /// </summary>
        /// <param name="formID"></param>
        /// <param name="formName"></param>
        /// <returns></returns>
        private M_GroupUser_D setDataRow(int formID, string formName)
        {
            M_GroupUser_D item = new M_GroupUser_D();
            item.FormID = formID;
            item.FormName = formName;

            return item;
        }

        /// <summary>
        /// Process Mode
        /// </summary>
        /// <param name="mode">Mode</param>
        private void ProcessMode(Mode mode)
        {
            //Set Model
            this.Mode = mode;

            //Check model
            switch (mode)
            {
                case Mode.Insert:
                    this.txtGroupCode.ReadOnly = false;
                    this.txtGroupName.ReadOnly = false;
                    this.ClearValue();
                    break;
                case Mode.Copy:
                    this.txtGroupCode.ReadOnly = false;
                    this.txtGroupName.ReadOnly = false;
                    break;

                case Mode.Update:
                    this.txtGroupCode.ReadOnly = true;
                    this.txtGroupName.ReadOnly = false;
                    break;

                default:
                    this.txtGroupCode.ReadOnly = true;
                    this.txtGroupName.ReadOnly = true;

                    if (this.GroupID == ADMIN_ID)
                    {
                        base.DisabledLink(this.btnEdit, true);
                        base.DisabledLink(this.btnDelete, true);
                        base.DisabledLink(this.btnCopy, true);
                        //---------------Add 2014/12/29 ISV-HUNG--------------------//
                        base.DisabledLink(this.btnNew, true);
                        //---------------Add 2014/12/29 ISV-HUNG--------------------//
                    }
                    else
                    {
                        //this.btnEdit.Attributes.Add("class", this.CheckAuthorityMaster(FormId.GroupUser, AuthorTypeMaster.Edit) ? Constants.CSS_BTN_EDIT : Constants.CSS_BTN_EDIT_DISABLED);
                        //this.btnDelete.Attributes.Add("class", this.CheckAuthorityMaster(FormId.GroupUser, AuthorTypeMaster.Delete) ? Constants.CSS_BTN_DELETE : Constants.CSS_BTN_DELETE_DISABLED);
                        //this.btnCopy.Attributes.Add("class", this.CheckAuthorityMaster(FormId.GroupUser, AuthorTypeMaster.Copy) ? Constants.CSS_BTN_COPY : Constants.CSS_BTN_COPY_DISABLED);

                        //this.btnEdit.Attributes.Add("class", this._authority.IsMasterEdit ? Constants.CSS_BTN_EDIT : Constants.CSS_BTN_EDIT_DISABLED);
                        //this.btnDelete.Attributes.Add("class", this._authority.IsMasterDelete ? Constants.CSS_BTN_DELETE : Constants.CSS_BTN_DELETE_DISABLED);
                        //this.btnCopy.Attributes.Add("class", this._authority.IsMasterCopy ? Constants.CSS_BTN_COPY : Constants.CSS_BTN_COPY_DISABLED);
                        base.DisabledLink(this.btnEdit, !base._authority.IsMasterEdit);
                        base.DisabledLink(this.btnDelete, !base._authority.IsMasterDelete);
                        base.DisabledLink(this.btnCopy, !base._authority.IsMasterCopy);
                        //---------------Add 2014/12/29 ISV-HUNG--------------------//
                        base.DisabledLink(this.btnNew, !base._authority.IsMasterNew);
                        //---------------Add 2014/12/29 ISV-HUNG--------------------//

                    }

                    break;
            }
        }

        /// <summary>
        /// Get Data Group By Cd
        /// </summary>
        /// <param name="groupCode"></param>
        /// <returns></returns>
        private M_GroupUser_H GetDataGroupByCd(string groupCode)
        {
            using (DB db = new DB())
            {
                GroupUserService groupSer = new GroupUserService(db);
                return groupSer.GetByGroupCD(groupCode);
            }
        }

        /// <summary>
        /// Get Data Group By Id
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        private M_GroupUser_H GetDataGroupById(int groupID)
        {
            using (DB db = new DB())
            {
                GroupUserService groupSer = new GroupUserService(db);
                return groupSer.GetByGroupID(groupID);
            }
        }

        /// <summary>
        /// get detail
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        private IList<M_GroupUser_D> GetDetailById(int groupID)
        {
            using (DB db = new DB())
            {
                GroupUserService ser = new GroupUserService(db);
                return ser.GetListGroupUserDetail(groupID);
            }
        }

        /// <summary>
        /// Show Data
        /// </summary>
        /// <param name="m_group"></param>
        private void ShowData(M_GroupUser_H m_group)
        {
            List<M_GroupUser_D> lstShowData = new List<M_GroupUser_D>();
            if (m_group != null)
            {
                IList<M_GroupUser_D> lst = this.GetDetailById(m_group.ID);

                this.txtGroupCode.Value = m_group.GroupCD;
                this.txtGroupName.Value = m_group.GroupName;

                this.GroupID = m_group.ID;
                this.OldUpdateDate = m_group.UpdateDate;

                if (lst.Count != 0)
                {
                    for (int i = 1; i <= lst.Count; i++)
                    {
                        if ((lst[i - 1].FormID >= 1 && lst[i - 1].FormID <= (int)FormId.SendMail) || lst[i - 1].FormID == (int)FormId.Cost || lst[i - 1].FormID == (int)FormId.ProjectProfit)
                        {
                            lstShowData.Add(lst[i - 1]);
                        }
                        else
                        {
                            switch (lst[i - 1].FormID)
                            {
                                case (int)FormId.WorkCalendar:
                                    this.ShowDetailWorkCalendar(lst[i - 1]);
                                    break;
                                case (int)FormId.Attendance:
                                    this.ShowDetailAttendance(lst[i - 1]);
                                    break;
                                case (int)FormId.AttendanceApproval:
                                    this.ShowDetailAttendanceApproval(lst[i - 1]);
                                    break;
                                case (int)FormId.AttendanceSummary:
                                    this.ShowDetailAttendanceSummary(lst[i - 1]);
                                    break;
                                case (int)FormId.AttendancePayslip:
                                    this.ShowDetailAttendancePayslip(lst[i - 1]);
                                    break;
                                case (int)FormId.ExpenceGroup:
                                    this.ShowDetailExpenceGroup(lst[i - 1]);
                                    break;
                                case (int)FormId.Approval:
                                    this.ShowDetailApproval(lst[i - 1]);
                                    break;
                            }
                        }
                    }

                    this.rptFormDetail.DataSource = lstShowData;
                    this.rptFormDetail.DataBind();
                }
            }
            this.InitHeader(lstShowData);
        }

        /// <summary>
        /// Insert Data
        /// </summary>
        /// <returns>Success:true, Faile:false</returns>
        private bool InsertData()
        {
            try
            {
                //Create model
                M_GroupUser_H m_GroupUser_H = new M_GroupUser_H();
                m_GroupUser_H.GroupCD = this.txtGroupCode.Value;
                m_GroupUser_H.GroupName = this.txtGroupName.Value;
                m_GroupUser_H.CreateUID = this.LoginInfo.User.ID;
                m_GroupUser_H.UpdateUID = this.LoginInfo.User.ID;

                using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                {
                    GroupUserService groupSer = new GroupUserService(db);

                    groupSer.Insert(m_GroupUser_H);

                    int id = db.GetIdentityId<M_GroupUser_H>();

                    foreach (RepeaterItem item in this.rptFormDetail.Items)
                    {
                        if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
                        {
                            M_GroupUser_D m_GroupUser_M = new M_GroupUser_D();
                            Label lblFormName = (Label)item.FindControl("lblFormName");

                            HtmlInputCheckBox chkAuth1 = (HtmlInputCheckBox)item.FindControl("chkAuth1");
                            m_GroupUser_M.AuthorityFlag1 = Convert.ToInt16(chkAuth1.Checked);

                            HtmlInputCheckBox chkAuth2 = (HtmlInputCheckBox)item.FindControl("chkAuth2");
                            m_GroupUser_M.AuthorityFlag2 = Convert.ToInt16(chkAuth2.Checked);

                            HtmlInputCheckBox chkAuth3 = (HtmlInputCheckBox)item.FindControl("chkAuth3");
                            m_GroupUser_M.AuthorityFlag3 = Convert.ToInt16(chkAuth3.Checked);

                            HtmlInputCheckBox chkAuth4 = (HtmlInputCheckBox)item.FindControl("chkAuth4");
                            m_GroupUser_M.AuthorityFlag4 = Convert.ToInt16(chkAuth4.Checked);

                            HtmlInputCheckBox chkAuth5 = (HtmlInputCheckBox)item.FindControl("chkAuth5");
                            m_GroupUser_M.AuthorityFlag5 = Convert.ToInt16(chkAuth5.Checked);

                            HiddenField hdFormID = (HiddenField)item.FindControl("hdFormId");
                            m_GroupUser_M.GroupID = id;
                            m_GroupUser_M.FormID = int.Parse(hdFormID.Value);
                            m_GroupUser_M.FormName = lblFormName.Text;

                            groupSer.Insert(m_GroupUser_M);
                        }
                    }

                    M_GroupUser_D m_GroupUser_DWorkCalendar = new M_GroupUser_D();
                    this.GetDetailWorkCalendar(m_GroupUser_DWorkCalendar);
                    m_GroupUser_DWorkCalendar.GroupID = id;
                    m_GroupUser_DWorkCalendar.FormID = (int)FormId.WorkCalendar;
                    m_GroupUser_DWorkCalendar.FormName = "勤務カレンダー";
                    groupSer.Insert(m_GroupUser_DWorkCalendar);

                    M_GroupUser_D m_GroupUser_DAttendance = new M_GroupUser_D();
                    this.GetDetailAttendance(m_GroupUser_DAttendance);
                    m_GroupUser_DAttendance.GroupID = id;
                    m_GroupUser_DAttendance.FormID = (int)FormId.Attendance;
                    m_GroupUser_DAttendance.FormName = "勤務表";
                    groupSer.Insert(m_GroupUser_DAttendance);

                    M_GroupUser_D m_GroupUser_DAttendanceApprove = new M_GroupUser_D();
                    this.GetDetailAttendanceApproval(m_GroupUser_DAttendanceApprove);
                    m_GroupUser_DAttendanceApprove.GroupID = id;
                    m_GroupUser_DAttendanceApprove.FormID = (int)FormId.AttendanceApproval;
                    m_GroupUser_DAttendanceApprove.FormName = "勤務表承認";
                    groupSer.Insert(m_GroupUser_DAttendanceApprove);

                    M_GroupUser_D m_GroupUser_DAttendanceSummary = new M_GroupUser_D();
                    this.GetDetailAttendanceSummary(m_GroupUser_DAttendanceSummary);
                    m_GroupUser_DAttendanceSummary.GroupID = id;
                    m_GroupUser_DAttendanceSummary.FormID = (int)FormId.AttendanceSummary;
                    m_GroupUser_DAttendanceSummary.FormName = "勤務表集計";
                    groupSer.Insert(m_GroupUser_DAttendanceSummary);

                    M_GroupUser_D m_GroupUser_DAttendancePayslip = new M_GroupUser_D();
                    this.GetDetailAttendancePayslip(m_GroupUser_DAttendancePayslip);
                    m_GroupUser_DAttendancePayslip.GroupID = id;
                    m_GroupUser_DAttendancePayslip.FormID = (int)FormId.AttendancePayslip;
                    m_GroupUser_DAttendancePayslip.FormName = "給与賞与明細";
                    groupSer.Insert(m_GroupUser_DAttendancePayslip);

                    M_GroupUser_D m_GroupUser_DExpenceGroup = new M_GroupUser_D();
                    this.GetDetailExpenceGroup(m_GroupUser_DExpenceGroup);
                    m_GroupUser_DExpenceGroup.GroupID = id;
                    m_GroupUser_DExpenceGroup.FormID = (int)FormId.ExpenceGroup;
                    m_GroupUser_DExpenceGroup.FormName = "経費申請・承認";
                    groupSer.Insert(m_GroupUser_DExpenceGroup);

                    M_GroupUser_D m_GroupUser_DApproval = new M_GroupUser_D();
                    this.GetDetailApproval(m_GroupUser_DApproval);
                    m_GroupUser_DApproval.GroupID = id;
                    m_GroupUser_DApproval.FormID = (int)FormId.Approval;
                    m_GroupUser_DApproval.FormName = "申請";
                    groupSer.Insert(m_GroupUser_DApproval);

                    db.Commit();
                }
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains(Models.Constant.M_GROUPUSER_UN))
                {
                    this.SetMessage(this.txtGroupCode.ID, M_Message.MSG_EXIST_CODE, "権限グループ");
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
                M_GroupUser_H m_GroupUser = this.GetDataGroupByCd(this.txtGroupCode.Value);

                if (m_GroupUser != null)
                {
                    //Create model
                    m_GroupUser.ID = this.GroupID;
                    m_GroupUser.GroupCD = this.txtGroupCode.Value;
                    m_GroupUser.GroupName = this.txtGroupName.Value;

                    m_GroupUser.UpdateDate = this.OldUpdateDate;
                    m_GroupUser.UpdateUID = this.LoginInfo.User.ID;

                    //Update                     
                    using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                    {
                        GroupUserService groupSer = new GroupUserService(db);
                        IList<M_GroupUser_D> lstD = groupSer.GetListGroupUserDetail(this.GroupID);

                        M_GroupUser_D m_GroupUser_DWorkCalendar = groupSer.GetByGroupAndFormID(m_GroupUser.ID, (int)FormId.WorkCalendar);
                        this.GetDetailWorkCalendar(m_GroupUser_DWorkCalendar);

                        M_GroupUser_D m_GroupUser_DAttendance = groupSer.GetByGroupAndFormID(m_GroupUser.ID, (int)FormId.Attendance);
                        this.GetDetailAttendance(m_GroupUser_DAttendance);

                        M_GroupUser_D m_GroupUser_DAttendanceApprove = groupSer.GetByGroupAndFormID(m_GroupUser.ID, (int)FormId.AttendanceApproval);
                        this.GetDetailAttendanceApproval(m_GroupUser_DAttendanceApprove);

                        M_GroupUser_D m_GroupUser_DAttendanceSummary = groupSer.GetByGroupAndFormID(m_GroupUser.ID, (int)FormId.AttendanceSummary);
                        this.GetDetailAttendanceSummary(m_GroupUser_DAttendanceSummary);

                        M_GroupUser_D m_GroupUser_DAttendancePayslip = groupSer.GetByGroupAndFormID(m_GroupUser.ID, (int)FormId.AttendancePayslip);
                        this.GetDetailAttendancePayslip(m_GroupUser_DAttendancePayslip);

                        M_GroupUser_D m_GroupUser_DExpenceGroup = groupSer.GetByGroupAndFormID(m_GroupUser.ID, (int)FormId.AttendancePayslip);
                        this.GetDetailExpenceGroup(m_GroupUser_DExpenceGroup);

                        M_GroupUser_D m_GroupUser_DApproval = groupSer.GetByGroupAndFormID(m_GroupUser.ID, (int)FormId.Approval);
                        this.GetDetailApproval(m_GroupUser_DApproval);

                        //Update User
                        if (m_GroupUser.Status == DataStatus.Changed ||
                            m_GroupUser_DWorkCalendar.Status == DataStatus.Changed ||
                            m_GroupUser_DAttendance.Status == DataStatus.Changed ||
                            m_GroupUser_DAttendanceApprove.Status == DataStatus.Changed ||
                            m_GroupUser_DAttendanceSummary.Status == DataStatus.Changed ||
                            m_GroupUser_DAttendancePayslip.Status == DataStatus.Changed ||
                            m_GroupUser_DApproval.Status == DataStatus.Changed ||
                            IsChangeDetail(db))
                        {
                            ret = groupSer.Update(m_GroupUser);

                            foreach (RepeaterItem item in this.rptFormDetail.Items)
                            {
                                if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
                                {
                                    M_GroupUser_D m_GroupUser_M = new M_GroupUser_D();
                                    Label lblFormName = (Label)item.FindControl("lblFormName");

                                    HtmlInputCheckBox chkAuth1 = (HtmlInputCheckBox)item.FindControl("chkAuth1");
                                    m_GroupUser_M.AuthorityFlag1 = Convert.ToInt16(chkAuth1.Checked);

                                    HtmlInputCheckBox chkAuth2 = (HtmlInputCheckBox)item.FindControl("chkAuth2");
                                    m_GroupUser_M.AuthorityFlag2 = Convert.ToInt16(chkAuth2.Checked);

                                    HtmlInputCheckBox chkAuth3 = (HtmlInputCheckBox)item.FindControl("chkAuth3");
                                    m_GroupUser_M.AuthorityFlag3 = Convert.ToInt16(chkAuth3.Checked);

                                    HtmlInputCheckBox chkAuth4 = (HtmlInputCheckBox)item.FindControl("chkAuth4");
                                    m_GroupUser_M.AuthorityFlag4 = Convert.ToInt16(chkAuth4.Checked);

                                    HtmlInputCheckBox chkAuth5 = (HtmlInputCheckBox)item.FindControl("chkAuth5");
                                    m_GroupUser_M.AuthorityFlag5 = Convert.ToInt16(chkAuth5.Checked);

                                    HiddenField hdFormID = (HiddenField)item.FindControl("hdFormId");
                                    m_GroupUser_M.GroupID = m_GroupUser.ID;
                                    m_GroupUser_M.FormID = int.Parse(hdFormID.Value);
                                    m_GroupUser_M.FormName = lblFormName.Text;

                                    groupSer.Update(m_GroupUser_M);
                                }
                            }

                            // Update WorkCalendar
                            m_GroupUser_DWorkCalendar.GroupID = m_GroupUser.ID;
                            m_GroupUser_DWorkCalendar.FormID = (int)FormId.WorkCalendar;
                            m_GroupUser_DWorkCalendar.FormName = "勤務カレンダー";
                            groupSer.Update(m_GroupUser_DWorkCalendar);

                            m_GroupUser_DAttendance.GroupID = m_GroupUser.ID;
                            m_GroupUser_DAttendance.FormID = (int)FormId.Attendance;
                            m_GroupUser_DAttendance.FormName = "勤務表";
                            groupSer.Update(m_GroupUser_DAttendance);

                            m_GroupUser_DAttendanceApprove.GroupID = m_GroupUser.ID;
                            m_GroupUser_DAttendanceApprove.FormID = (int)FormId.AttendanceApproval;
                            m_GroupUser_DAttendanceApprove.FormName = "勤務表承認";
                            groupSer.Update(m_GroupUser_DAttendanceApprove);

                            m_GroupUser_DAttendanceSummary.GroupID = m_GroupUser.ID;
                            m_GroupUser_DAttendanceSummary.FormID = (int)FormId.AttendanceSummary;
                            m_GroupUser_DAttendanceSummary.FormName = "勤務表集計";
                            groupSer.Update(m_GroupUser_DAttendanceSummary);

                            m_GroupUser_DAttendancePayslip.GroupID = m_GroupUser.ID;
                            m_GroupUser_DAttendancePayslip.FormID = (int)FormId.AttendancePayslip;
                            m_GroupUser_DAttendancePayslip.FormName = "給与賞与明細";
                            groupSer.Update(m_GroupUser_DAttendancePayslip);

                            m_GroupUser_DExpenceGroup.GroupID = m_GroupUser.ID;
                            m_GroupUser_DExpenceGroup.FormID = (int)FormId.ExpenceGroup;
                            m_GroupUser_DExpenceGroup.FormName = "経費申請・承認";
                            groupSer.Update(m_GroupUser_DExpenceGroup);

                            m_GroupUser_DApproval.GroupID = m_GroupUser.ID;
                            m_GroupUser_DApproval.FormID = (int)FormId.Approval;
                            m_GroupUser_DApproval.FormName = "申請";
                            groupSer.Update(m_GroupUser_DApproval);
                            
                            db.Commit();
                        }
                        else
                        {
                            return true;
                        }
                    }
                }

                //Check result update
                if (ret == 0)
                {
                    //Data is changed
                    this.SetMessage(string.Empty, M_Message.MSG_DATA_CHANGED);
                    return false;
                }
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains(Models.Constant.M_GROUPUSER_UN))
                {
                    this.SetMessage(this.txtGroupName.ID, M_Message.MSG_EXIST_CODE, "権限グループ");
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
        /// check data changed
        /// </summary>
        /// <returns></returns>
        private bool IsChangeDetail(DB db)
        {
            IList<M_GroupUser_D> lstD;

            GroupUserService groupSer = new GroupUserService(db);
            lstD = groupSer.GetListGroupUserDetail(this.GroupID);
            var arrs = Enum.GetValues(typeof(FormId));
            if (lstD.Count < arrs.Length)
            {
                return true;
            }

            var temp = lstD.Where(x => x.FormID.Equals((int)FormId.Cost) || x.FormID.Equals((int)FormId.ProjectProfit) || x.FormID <= (int)(FormId.SendMail));

            for (int i = 0; i < temp.Count(); i++)
            {
                RepeaterItem itemD = this.rptFormDetail.Items[i];

                if (lstD[i].AuthorityFlag1 != Convert.ToInt16(((HtmlInputCheckBox)itemD.FindControl("chkAuth1")).Checked))
                {
                    return true;
                }

                if (lstD[i].AuthorityFlag2 != Convert.ToInt16(((HtmlInputCheckBox)itemD.FindControl("chkAuth2")).Checked))
                {
                    return true;
                }

                if (lstD[i].AuthorityFlag3 != Convert.ToInt16(((HtmlInputCheckBox)itemD.FindControl("chkAuth3")).Checked))
                {
                    return true;
                }

                if (lstD[i].AuthorityFlag4 != Convert.ToInt16(((HtmlInputCheckBox)itemD.FindControl("chkAuth4")).Checked))
                {
                    return true;
                }

                if (lstD[i].AuthorityFlag5 != Convert.ToInt16(((HtmlInputCheckBox)itemD.FindControl("chkAuth5")).Checked))
                {
                    return true;
                }
            }



            return false;
        }

        /// <summary>
        /// Delete Data
        /// </summary>
        /// <returns></returns>
        private bool DeleteData()
        {
            try
            {
                int ret = 0;
                using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                {
                    GroupUserService groupSer = new GroupUserService(db);
                    groupSer.DeleteDetail(this.GroupID);

                    ret = groupSer.DeleteHeader(this.GroupID, this.OldUpdateDate);
                    if (ret > 0)
                    {
                        db.Commit();
                    }
                }

                if (ret == 0)
                {
                    this.SetMessage(string.Empty, M_Message.MSG_DATA_CHANGED);
                    return false;
                }
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains(Models.Constant.M_USER_FK_GROUPID))
                {
                    this.SetMessage(string.Empty, M_Message.MSG_EXIST_CANT_DELETE, "権限グループ" + this.txtGroupCode.Value);
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
        /// Check input
        /// </summary>
        /// <returns>Valid:true, Invalid:false</returns>
        private bool CheckInput()
        {
            //txtGroupCode
            if (this.txtGroupCode.IsEmpty)
            {
                this.SetMessage(this.txtGroupCode.ID, M_Message.MSG_REQUIRE, "権限グループ");
            }
            else
            {
                if (this.Mode == Mode.Insert || this.Mode == Mode.Copy)
                {
                    if (this.txtGroupCode.Value == Constant.M_GROUPUSER_DEFAULT_CODE)
                    {
                        //Diff '0000'
                        this.SetMessage(this.txtGroupCode.ID, M_Message.MSG_MUST_BE_DIFFERENT, "権限グループ", Constant.M_GROUPUSER_DEFAULT_CODE);
                    }
                    else
                    {
                        // Check user by userCD 
                        if (this.GetDataGroupByCd(this.txtGroupCode.Value) != null)
                        {
                            this.SetMessage(this.txtGroupCode.ID, M_Message.MSG_EXIST_CODE, "権限グループ");
                        }
                    }
                }
            }

            //txtGroupName
            if (this.txtGroupName.IsEmpty)
            {
                this.SetMessage(this.txtGroupName.ID, M_Message.MSG_REQUIRE, "権限グループ名");
            }

            //Check error
            return !base.HaveError;
        }

        /// <summary>
        /// Get Detail WorkCalendar
        /// </summary>
        /// <param name="m_GroupUser_D">m_GroupUser_D</param>
        private void GetDetailWorkCalendar(M_GroupUser_D m_GroupUser_D)
        {
            m_GroupUser_D.AuthorityFlag1 = Convert.ToInt16(this.chkWorkCalendarAuth1.Checked);
            m_GroupUser_D.AuthorityFlag2 = Convert.ToInt16(this.chkWorkCalendarAuth2.Checked);
            m_GroupUser_D.AuthorityFlag3 = Convert.ToInt16(this.chkWorkCalendarAuth3.Checked);
            m_GroupUser_D.AuthorityFlag4 = Convert.ToInt16(this.chkWorkCalendarAuth4.Checked);
            m_GroupUser_D.AuthorityFlag5 = Convert.ToInt16(this.chkWorkCalendarAuth5.Checked);
            m_GroupUser_D.AuthorityFlag6 = Convert.ToInt16(this.chkWorkCalendarAuth6.Checked);

        }

        /// <summary>
        /// Get Detail Attendance
        /// </summary>
        /// <param name="m_GroupUser_D">m_GroupUser_D</param>
        private void GetDetailAttendance(M_GroupUser_D m_GroupUser_D)
        {
            m_GroupUser_D.AuthorityFlag1 = Convert.ToInt16(this.chkAttendanceAuth1.Checked);
            m_GroupUser_D.AuthorityFlag2 = Convert.ToInt16(this.chkAttendanceAuth2.Checked);
            m_GroupUser_D.AuthorityFlag3 = Convert.ToInt16(this.chkAttendanceAuth3.Checked);
            m_GroupUser_D.AuthorityFlag4 = Convert.ToInt16(this.chkAttendanceAuth4.Checked);
            m_GroupUser_D.AuthorityFlag5 = Convert.ToInt16(this.chkAttendanceAuth5.Checked);
            m_GroupUser_D.AuthorityFlag6 = Convert.ToInt16(this.chkAttendanceAuth6.Checked);
            m_GroupUser_D.AuthorityFlag7 = Convert.ToInt16(this.chkAttendanceAuth7.Checked);
            m_GroupUser_D.AuthorityFlag8 = Convert.ToInt16(this.chkAttendanceAuth8.Checked);
            m_GroupUser_D.AuthorityFlag9 = Convert.ToInt16(this.chkAttendanceAuth9.Checked);
        }

        /// <summary>
        /// Get Detail AttendanceApproval
        /// </summary>
        /// <param name="m_GroupUser_D">m_GroupUser_D</param>
        private void GetDetailAttendanceApproval(M_GroupUser_D m_GroupUser_D)
        {
            m_GroupUser_D.AuthorityFlag1 = Convert.ToInt16(this.chkAttendanceApprovalAuth1.Checked);
            m_GroupUser_D.AuthorityFlag2 = Convert.ToInt16(this.chkAttendanceApprovalAuth2.Checked);
            m_GroupUser_D.AuthorityFlag3 = Convert.ToInt16(this.chkAttendanceApprovalAuth3.Checked);
        }

        /// <summary>
        /// Get Detail AttendanceSummary
        /// </summary>
        /// <param name="m_GroupUser_D">m_GroupUser_D</param>
        private void GetDetailAttendanceSummary(M_GroupUser_D m_GroupUser_D)
        {
            m_GroupUser_D.AuthorityFlag1 = Convert.ToInt16(this.chkAuthAttendanceSummaryAuth1.Checked);
            m_GroupUser_D.AuthorityFlag2 = Convert.ToInt16(this.chkAuthAttendanceSummaryAuth2.Checked);
        }

        /// <summary>
        /// Get Detail AttendancePayslip
        /// </summary>
        /// <param name="m_GroupUser_D">m_GroupUser_D</param>
        private void GetDetailAttendancePayslip(M_GroupUser_D m_GroupUser_D)
        {
            m_GroupUser_D.AuthorityFlag1 = Convert.ToInt16(this.chkAuthAttendancePayslipAuth1.Checked);
            m_GroupUser_D.AuthorityFlag2 = Convert.ToInt16(this.chkAuthAttendancePayslipAuth2.Checked);
            m_GroupUser_D.AuthorityFlag3 = Convert.ToInt16(this.chkAuthAttendancePayslipAuth3.Checked);
            m_GroupUser_D.AuthorityFlag4 = Convert.ToInt16(this.chkAuthAttendancePayslipAuth4.Checked);

        }
        /// <summary>
        /// Get Detail ExpenceGroup
        /// </summary>
        /// <param name="m_GroupUser_D">m_GroupUser_D</param>
        private void GetDetailExpenceGroup(M_GroupUser_D m_GroupUser_D)
        {
            m_GroupUser_D.AuthorityFlag1 = Convert.ToInt16(this.chkAuthExpenceGroupAuth1.Checked);
            m_GroupUser_D.AuthorityFlag2 = Convert.ToInt16(this.chkAuthExpenceGroupAuth2.Checked);
            m_GroupUser_D.AuthorityFlag3 = Convert.ToInt16(this.chkAuthExpenceGroupAuth3.Checked);
            m_GroupUser_D.AuthorityFlag4 = Convert.ToInt16(this.chkAuthExpenceGroupAuth4.Checked);
            m_GroupUser_D.AuthorityFlag5 = Convert.ToInt16(this.chkAuthExpenceGroupAuth5.Checked);
            m_GroupUser_D.AuthorityFlag6 = Convert.ToInt16(this.chkAuthExpenceGroupAuth6.Checked);
            m_GroupUser_D.AuthorityFlag7 = Convert.ToInt16(this.chkAuthExpenceGroupAuth7.Checked);
            m_GroupUser_D.AuthorityFlag8 = Convert.ToInt16(this.chkAuthExpenceGroupAuth8.Checked);
            m_GroupUser_D.AuthorityFlag9 = Convert.ToInt16(this.chkAuthExpenceGroupAuth9.Checked);
            m_GroupUser_D.AuthorityFlag10 = Convert.ToInt16(this.chkAuthExpenceGroupAuth10.Checked);
            m_GroupUser_D.AuthorityFlag11 = Convert.ToInt16(this.chkAuthExpenceGroupAuth11.Checked);
            m_GroupUser_D.AuthorityFlag12 = Convert.ToInt16(this.chkAuthExpenceGroupAuth12.Checked);

        }

        /// <summary>
        /// Get Detail Approval
        /// </summary>
        /// <param name="m_GroupUser_D">m_GroupUser_D</param>
        private void GetDetailApproval(M_GroupUser_D m_GroupUser_D)
        {
            m_GroupUser_D.AuthorityFlag1 = Convert.ToInt16(this.chkAuthApprovalAuth1.Checked);
            m_GroupUser_D.AuthorityFlag2 = Convert.ToInt16(this.chkAuthApprovalAuth2.Checked);
            m_GroupUser_D.AuthorityFlag3 = Convert.ToInt16(this.chkAuthApprovalAuth3.Checked);
            m_GroupUser_D.AuthorityFlag4 = Convert.ToInt16(this.chkAuthApprovalAuth4.Checked);

        }

        /// <summary>
        /// Get Detail WorkCalendar
        /// </summary>
        /// <param name="m_GroupUser_D">m_GroupUser_D</param>
        private void ShowDetailWorkCalendar(M_GroupUser_D m_GroupUser_D)
        {
            this.chkWorkCalendarAuth1.Checked = Convert.ToBoolean(m_GroupUser_D.AuthorityFlag1);
            this.chkWorkCalendarAuth2.Checked = Convert.ToBoolean(m_GroupUser_D.AuthorityFlag2);
            this.chkWorkCalendarAuth3.Checked = Convert.ToBoolean(m_GroupUser_D.AuthorityFlag3);
            this.chkWorkCalendarAuth4.Checked = Convert.ToBoolean(m_GroupUser_D.AuthorityFlag4);
            this.chkWorkCalendarAuth5.Checked = Convert.ToBoolean(m_GroupUser_D.AuthorityFlag5);
            this.chkWorkCalendarAuth6.Checked = Convert.ToBoolean(m_GroupUser_D.AuthorityFlag6);
        }

        /// <summary>
        /// Get Detail Attendance
        /// </summary>
        /// <param name="m_GroupUser_D">m_GroupUser_D</param>
        private void ShowDetailAttendance(M_GroupUser_D m_GroupUser_D)
        {
            this.chkAttendanceAuth1.Checked = Convert.ToBoolean(m_GroupUser_D.AuthorityFlag1);
            this.chkAttendanceAuth2.Checked = Convert.ToBoolean(m_GroupUser_D.AuthorityFlag2);
            this.chkAttendanceAuth3.Checked = Convert.ToBoolean(m_GroupUser_D.AuthorityFlag3);
            this.chkAttendanceAuth4.Checked = Convert.ToBoolean(m_GroupUser_D.AuthorityFlag4);
            this.chkAttendanceAuth5.Checked = Convert.ToBoolean(m_GroupUser_D.AuthorityFlag5);
            this.chkAttendanceAuth6.Checked = Convert.ToBoolean(m_GroupUser_D.AuthorityFlag6);
            this.chkAttendanceAuth7.Checked = Convert.ToBoolean(m_GroupUser_D.AuthorityFlag7);
            this.chkAttendanceAuth8.Checked = Convert.ToBoolean(m_GroupUser_D.AuthorityFlag8);
            this.chkAttendanceAuth9.Checked = Convert.ToBoolean(m_GroupUser_D.AuthorityFlag9);
        }

        /// <summary>
        /// Get Detail AttendanceApproval
        /// </summary>
        /// <param name="m_GroupUser_D">m_GroupUser_D</param>
        private void ShowDetailAttendanceApproval(M_GroupUser_D m_GroupUser_D)
        {
            this.chkAttendanceApprovalAuth1.Checked = Convert.ToBoolean(m_GroupUser_D.AuthorityFlag1);
            this.chkAttendanceApprovalAuth2.Checked = Convert.ToBoolean(m_GroupUser_D.AuthorityFlag2);
            this.chkAttendanceApprovalAuth3.Checked = Convert.ToBoolean(m_GroupUser_D.AuthorityFlag3);
        }

        /// <summary>
        /// Get Detail AttendanceSummary
        /// </summary>
        /// <param name="m_GroupUser_D">m_GroupUser_D</param>
        private void ShowDetailAttendanceSummary(M_GroupUser_D m_GroupUser_D)
        {
            this.chkAuthAttendanceSummaryAuth1.Checked = Convert.ToBoolean(m_GroupUser_D.AuthorityFlag1);
            this.chkAuthAttendanceSummaryAuth2.Checked = Convert.ToBoolean(m_GroupUser_D.AuthorityFlag2);
        }

        /// <summary>
        /// Get Detail AttendancePayslip
        /// </summary>
        /// <param name="m_GroupUser_D">m_GroupUser_D</param>
        private void ShowDetailAttendancePayslip(M_GroupUser_D m_GroupUser_D)
        {
            this.chkAuthAttendancePayslipAuth1.Checked = Convert.ToBoolean(m_GroupUser_D.AuthorityFlag1);
            this.chkAuthAttendancePayslipAuth2.Checked = Convert.ToBoolean(m_GroupUser_D.AuthorityFlag2);
            this.chkAuthAttendancePayslipAuth3.Checked = Convert.ToBoolean(m_GroupUser_D.AuthorityFlag3);
            this.chkAuthAttendancePayslipAuth4.Checked = Convert.ToBoolean(m_GroupUser_D.AuthorityFlag4);
        }
        /// <summary>
        /// Get Detail ExpenceGroup
        /// </summary>
        /// <param name="m_GroupUser_D">m_GroupUser_D</param>
        private void ShowDetailExpenceGroup(M_GroupUser_D m_GroupUser_D)
        {
            this.chkAuthExpenceGroupAuth1.Checked = Convert.ToBoolean(m_GroupUser_D.AuthorityFlag1);
            this.chkAuthExpenceGroupAuth2.Checked = Convert.ToBoolean(m_GroupUser_D.AuthorityFlag2);
            this.chkAuthExpenceGroupAuth3.Checked = Convert.ToBoolean(m_GroupUser_D.AuthorityFlag3);
            this.chkAuthExpenceGroupAuth4.Checked = Convert.ToBoolean(m_GroupUser_D.AuthorityFlag4);
            this.chkAuthExpenceGroupAuth5.Checked = Convert.ToBoolean(m_GroupUser_D.AuthorityFlag5);
            this.chkAuthExpenceGroupAuth6.Checked = Convert.ToBoolean(m_GroupUser_D.AuthorityFlag6);
            this.chkAuthExpenceGroupAuth7.Checked = Convert.ToBoolean(m_GroupUser_D.AuthorityFlag7);
            this.chkAuthExpenceGroupAuth8.Checked = Convert.ToBoolean(m_GroupUser_D.AuthorityFlag8);
            this.chkAuthExpenceGroupAuth9.Checked = Convert.ToBoolean(m_GroupUser_D.AuthorityFlag9);
            this.chkAuthExpenceGroupAuth10.Checked = Convert.ToBoolean(m_GroupUser_D.AuthorityFlag10);
            this.chkAuthExpenceGroupAuth11.Checked = Convert.ToBoolean(m_GroupUser_D.AuthorityFlag11);
            this.chkAuthExpenceGroupAuth12.Checked = Convert.ToBoolean(m_GroupUser_D.AuthorityFlag12);
        }
        /// <summary>
        /// Clear Value Details
        /// </summary>
        private void ClearValue()
        {
            M_GroupUser_D GroupUser_D = new M_GroupUser_D()
            {
                AuthorityFlag1 = 0,
                AuthorityFlag2 = 0,
                AuthorityFlag3 = 0,
                AuthorityFlag4 = 0,
                AuthorityFlag5 = 0,
                AuthorityFlag6 = 0,
                AuthorityFlag7 = 0,
                AuthorityFlag8 = 0,
                AuthorityFlag9 = 0,
                AuthorityFlag10 = 0,
                AuthorityFlag11 = 0,
                AuthorityFlag12 = 0,
            };
            //WorkCalendar
            ShowDetailWorkCalendar(GroupUser_D);
            //Attendance
            ShowDetailAttendance(GroupUser_D);
            //AttendanceApproval
            ShowDetailAttendanceApproval(GroupUser_D);
            //AttendanceSummary
            ShowDetailAttendanceSummary(GroupUser_D);
            //AttendanceSummary
            ShowDetailAttendancePayslip(GroupUser_D);
            //AttendanceSummary
            ShowDetailExpenceGroup(GroupUser_D);
        }

        /// <summary>
        /// Get Detail Approval
        /// </summary>
        /// <param name="m_GroupUser_D">m_GroupUser_D</param>
        private void ShowDetailApproval(M_GroupUser_D m_GroupUser_D)
        {
            this.chkAuthApprovalAuth1.Checked = Convert.ToBoolean(m_GroupUser_D.AuthorityFlag1);
            this.chkAuthApprovalAuth2.Checked = Convert.ToBoolean(m_GroupUser_D.AuthorityFlag2);
            this.chkAuthApprovalAuth3.Checked = Convert.ToBoolean(m_GroupUser_D.AuthorityFlag3);
            this.chkAuthApprovalAuth4.Checked = Convert.ToBoolean(m_GroupUser_D.AuthorityFlag4);
        }
        #endregion
    }
}
