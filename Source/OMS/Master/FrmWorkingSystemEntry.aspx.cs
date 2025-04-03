using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OMS.Utilities;
using OMS.Models;
using OMS.DAC;
using OMS.Controls;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Collections;


namespace OMS.WorkingSystem
{
    public partial class FrmWorkingSystemEntry : FrmBaseDetail
    {
        #region Cosntant
        /// <summary>
        /// Default admin id
        /// </summary>
        private const int ADMIN_ID = 1;

        private const string URL_LIST = "~/Master/FrmWorkingSystemList.aspx";
       
        private const int OPT_WORKING_TYPE_0 = 0;
        private const int OPT_WORKING_TYPE_1 = 1;
        private const int OPT_WORKING_TYPE_2 = 2;
        private const int OPT_BREAK_TYPE_0 = 0;
        private const int OPT_BREAK_TYPE_1 = 1;
        private const int OPT_BREAK_TYPE_2 = 2;

        #endregion

        #region Property

        /// <summary>
        /// Get or set WorkingSystemID
        /// </summary>
        public int WorkingSystemID
        {
            get { return (int)ViewState["WorkingSystemID"]; }
            set { ViewState["WorkingSystemID"] = value; }
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
        /// Get or set FlagRequre
        /// </summary>
        public int FlagRequire
        {
            get { return (int)ViewState["FlagRequre"]; }
            set { ViewState["FlagRequre"] = value; }
        }

        /// <summary>
        /// StatusDisplayBreakType
        /// </summary>
        public int StatusDisplayBreakType { get; set; }

        /// <summary>
        /// StatusDisplayWorking
        /// </summary>
        public int StatusDisplayWorking { get; set; }

        // <summary>
        /// IsDisplayDiveOverTime
        /// </summary>
        public bool IsDisplayDiveOverTime { get; set; }
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
            base.FormTitle = "勤務体系登録";
            base.FormSubTitle = "Detail";

            this.txtWorkingSystemCD.MaxLength = M_WorkingSystem.WORKING_SYSTEM_CODE_SHOW_MAX_LENGTH;
            this.txtWorkingSystemName.MaxLength = M_WorkingSystem.WORKING_SYSTEM_NAME_MAX_LENGTH;
            this.txtWorkingSystemName2.MaxLength = M_WorkingSystem.WORKING_SYSTEM_NAME2_MAX_LENGTH;
            this.StatusDisplayBreakType = OPT_BREAK_TYPE_0;
            this.StatusDisplayWorking = OPT_WORKING_TYPE_0;
            this.IsDisplayDiveOverTime = true;
            //Init Event
            LinkButton btnYes = (LinkButton)this.Master.FindControl("btnYes");
            btnYes.Click += new EventHandler(btnProcessData);

            LinkButton btnNo = (LinkButton)this.Master.FindControl("btnNo");
            btnNo.Click += new EventHandler(btnShowData);
        }

        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Check authority of login user
            base.SetAuthority(FormId.WorkingSystem);
            if (!this._authority.IsMasterView)
            {
                Response.Redirect("~/Menu/FrmMasterMenu.aspx");
            }

            if (!this.IsPostBack)
            {
                this.InitRadio();
                this.InitOverTimeCtrl();
                if (this.PreviousPage != null)
                {
                    //Save condition of previous page
                    this.ViewState["Condition"] = this.PreviousPageViewState["Condition"];

                    //Check mode
                    if (this.PreviousPageViewState["ID"] == null)
                    {
                        //Set mode
                        this.ProcessMode(Mode.Insert);
                    }
                    else
                    {
                        //Get User ID
                        this.WorkingSystemID = int.Parse(PreviousPageViewState["ID"].ToString());
                        M_WorkingSystem m_WorkingSystem = this.GetDataWorkingSystemById(this.WorkingSystemID);

                        //Check m_WorkingSystem
                        if (m_WorkingSystem != null)
                        {
                            //Show data
                            this.ShowData(m_WorkingSystem);

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
        /// Edit Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            M_WorkingSystem m_WorkingSystem = this.GetDataWorkingSystemById(this.WorkingSystemID);

            if (m_WorkingSystem != null)
            {
                //Show data
                this.ShowData(m_WorkingSystem);

                //Set Mode
                this.ProcessMode(Mode.Update);
            }
            else
            {
                Server.Transfer(URL_LIST);
            }
        }

        /// <summary>
        /// Copy Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCopy_Click(object sender, EventArgs e)
        {
            M_WorkingSystem m_WorkingSystem = this.GetDataWorkingSystemById(this.WorkingSystemID);

            if (m_WorkingSystem != null)
            {
                //Show data
                this.ShowData(m_WorkingSystem);

                //Set Mode
                this.ProcessMode(Mode.Copy);
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
            this.txtWorkingSystemCD.Value = string.Empty;
            this.txtWorkingSystemName.Value = string.Empty;
            this.txtWorkingSystemName2.Value = string.Empty;
            this.optWorkingType.SelectedIndex = OPT_WORKING_TYPE_0;
            this.timeWorking_Start.Value = string.Empty;
            this.timeWorking_End.Value = string.Empty;
            this.timeWorking_End_2.Value = string.Empty;

            foreach (RepeaterItem item in rptOverTime.Items)
            {
                ITimeTextBox timeOverTime_Start = (ITimeTextBox)item.FindControl("timeOverTimeStart");
                ITimeTextBox timeOverTime_End = (ITimeTextBox)item.FindControl("timeOverTimeEnd");
                if (timeOverTime_Start != null)
                {
                    timeOverTime_Start.Value = string.Empty;
                    timeOverTime_End.Value = string.Empty;
                }
            }

            this.optBreakType.SelectedIndex = OPT_BREAK_TYPE_0;
            this.timeBreak1_Start.Value = string.Empty;
            this.timeBreak1_End.Value = string.Empty;
            this.timeBreak2_Start.Value = string.Empty;
            this.timeBreak2_End.Value = string.Empty;
            this.timeBreak3_Start.Value = string.Empty;
            this.timeBreak3_End.Value = string.Empty;
            this.timeBreak4_Start.Value = string.Empty;
            this.timeBreak4_End.Value = string.Empty;

            this.timeDateSwitchTime.Value = string.Empty;
            this.timeFirst_End.Value = string.Empty;
            this.timeLatter_Start.Value = string.Empty;
            this.timeAllOff_Hours.Value = string.Empty;
            this.timeFirstOff_Hours.Value = string.Empty;
            this.timeLatterOff_Hours.Value = string.Empty;
            //Set mode
            this.ProcessMode(Mode.Insert);
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
                //Get M_WorkingSystem
                M_WorkingSystem m_WorkingSystem = this.GetDataWorkingSystemById(this.WorkingSystemID);

                //Check M_WorkingSystem
                if (m_WorkingSystem != null)
                {
                    //Set Mode
                    this.ProcessMode(Mode.View);

                    //Show data
                    this.ShowData(m_WorkingSystem);
                }
                else
                {
                    Server.Transfer(URL_LIST);
                }
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

                    //Insert Data
                    if (this.InsertData())
                    {
                        M_WorkingSystem m_WorkingSystem = this.GetDataWorkingSystemByCd(this.txtWorkingSystemCD.Value);

                        //Show data
                        this.ShowData(m_WorkingSystem);

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
                        M_WorkingSystem m_WorkingSystem = this.GetDataWorkingSystemById(this.WorkingSystemID);

                        //Set Mode
                        this.ProcessMode(Mode.View);

                        //Show data
                        this.ShowData(m_WorkingSystem);

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
            M_WorkingSystem m_WorkingSystem = this.GetDataWorkingSystemById(this.WorkingSystemID);

            if (m_WorkingSystem != null)
            {

                //Show data
                this.ShowData(m_WorkingSystem);

                //Set Mode
                this.ProcessMode(Mode.View);
            }
            else
            {
                Server.Transfer(URL_LIST);
            }
        }


        /// <summary>
        /// Get Format GroupCode
        /// </summary>
        /// <param name="in1"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string GetWorkingSystem(string in1)
        {
            return EditDataUtil.JsonSerializer(new
            {
                txtWorkingSystemCD = in1
            });
        }

        #endregion

        #region Methods
        /// <summary>
        /// Init Radio
        /// </summary>
        private void InitRadio()
        {
            using (DB db = new DB(System.Data.IsolationLevel.Serializable))
            {
                Config_DService config_DService = new Config_DService(db);

                IList<M_Config_D> working_TypeList;

                working_TypeList = config_DService.GetListByConfigCd(Models.M_Config_H.CONFIG_CD_WORKING_TYPE);

                optWorkingType.DataTextField = "Value2";
                optWorkingType.DataValueField = "Value1";
                optWorkingType.DataSource = working_TypeList;
                if (working_TypeList.Count > 0)
                {
                    optWorkingType.SelectedIndex = 0;
                }
                optWorkingType.DataBind();
                for (int i = 0; i < working_TypeList.Count; i++)
                {
                    optWorkingType.Items[i].Text = optWorkingType.Items[i].Text + "&nbsp;&nbsp;&nbsp;&nbsp;";
                }

                IList<M_Config_D> break_TypeList;
                break_TypeList = config_DService.GetListByConfigCd(Models.M_Config_H.CONFIG_CD_BREAK_TYPE);
                optBreakType.DataTextField = "Value2";
                optBreakType.DataValueField = "Value1";
                optBreakType.DataSource = break_TypeList;
                if (break_TypeList.Count > 0)
                {
                    optBreakType.SelectedIndex = 0;
                }
                optBreakType.DataBind();
                for (int i = 0; i < break_TypeList.Count; i++)
                {
                    optBreakType.Items[i].Text = optBreakType.Items[i].Text + "&nbsp;&nbsp;&nbsp;&nbsp;";
                }

            }
        }

        /// <summary>
        /// InitOverTimeCtrl
        /// </summary>
        private void InitOverTimeCtrl()
        {
            using (DB db = new DB(System.Data.IsolationLevel.Serializable))
            {
                Config_DService config_DService = new Config_DService(db);
                IList<M_Config_D> over_TimeList = config_DService.GetListByConfigCd(Models.M_Config_H.CONFIG_CD_OVER_TIME_TYPE).Take(5).ToList();
                rptOverTime.DataSource = over_TimeList;
                rptOverTime.DataBind();

                if (over_TimeList.Count() == 0)
                {
                    this.IsDisplayDiveOverTime = false;
                }
                else
                {
                    this.IsDisplayDiveOverTime = true;
                }

            }
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
                case Mode.Copy:
                    this.txtWorkingSystemCD.Value = string.Empty;
                    this.txtWorkingSystemCD.ReadOnly = false;
                    this.txtWorkingSystemName.ReadOnly = false;
                    this.txtWorkingSystemName2.ReadOnly = false;

                    this.optWorkingType.Enabled = true;
                    this.timeWorking_Start.ReadOnly = false;
                    this.timeWorking_End.ReadOnly = false;
                    this.timeWorking_End_2.ReadOnly = false;

                    foreach (RepeaterItem item in rptOverTime.Items)
                    {
                        ITimeTextBox timeOverTime_Start = (ITimeTextBox)item.FindControl("timeOverTimeStart");
                        ITimeTextBox timeOverTime_End = (ITimeTextBox)item.FindControl("timeOverTimeEnd");
                        if (timeOverTime_Start != null)
                        {
                            timeOverTime_Start.ReadOnly = false;
                            timeOverTime_End.ReadOnly = false;
                        }
                    }

                    this.optBreakType.Enabled = true;
                    this.timeBreak1_Start.ReadOnly = false;
                    this.timeBreak1_End.ReadOnly = false;
                    this.timeBreak2_Start.ReadOnly = false;
                    this.timeBreak2_End.ReadOnly = false;
                    this.timeBreak3_Start.ReadOnly = false;
                    this.timeBreak3_End.ReadOnly = false;
                    this.timeBreak4_Start.ReadOnly = false;
                    this.timeBreak4_End.ReadOnly = false;

                    this.timeDateSwitchTime.ReadOnly = false;
                    this.timeFirst_End.ReadOnly = false;
                    this.timeLatter_Start.ReadOnly = false;
                    this.timeAllOff_Hours.ReadOnly = false;
                    this.timeFirstOff_Hours.ReadOnly = false;
                    this.timeLatterOff_Hours.ReadOnly = false;
                    break;

                case Mode.Update:
                    this.txtWorkingSystemCD.ReadOnly = true;
                    this.txtWorkingSystemName.ReadOnly = false;
                    this.txtWorkingSystemName2.ReadOnly = false;

                    this.optWorkingType.Enabled = true;

                    this.timeWorking_Start.ReadOnly = false;
                    this.timeWorking_End.ReadOnly = false;
                    this.timeWorking_End_2.ReadOnly = false;

                    foreach (RepeaterItem item in rptOverTime.Items)
                    {
                        ITimeTextBox timeOverTime_Start = (ITimeTextBox)item.FindControl("timeOverTimeStart");
                        ITimeTextBox timeOverTime_End = (ITimeTextBox)item.FindControl("timeOverTimeEnd");
                        if (timeOverTime_Start != null)
                        {
                            timeOverTime_Start.ReadOnly = false;
                            timeOverTime_End.ReadOnly = false;
                        }
                    }

                    this.optBreakType.Enabled = true;
                    this.timeBreak1_Start.ReadOnly = false;
                    this.timeBreak1_End.ReadOnly = false;
                    this.timeBreak2_Start.ReadOnly = false;
                    this.timeBreak2_End.ReadOnly = false;
                    this.timeBreak3_Start.ReadOnly = false;
                    this.timeBreak3_End.ReadOnly = false;
                    this.timeBreak4_Start.ReadOnly = false;
                    this.timeBreak4_End.ReadOnly = false;

                    this.timeDateSwitchTime.ReadOnly = false;
                    this.timeFirst_End.ReadOnly = false;
                    this.timeLatter_Start.ReadOnly = false;
                    this.timeAllOff_Hours.ReadOnly = false;
                    this.timeFirstOff_Hours.ReadOnly = false;
                    this.timeLatterOff_Hours.ReadOnly = false;

                    break;

                default:
                    this.txtWorkingSystemCD.ReadOnly = true;
                    this.txtWorkingSystemName.ReadOnly = true;
                    this.txtWorkingSystemName2.ReadOnly = true;

                    this.optWorkingType.Enabled = false;
                    this.timeWorking_Start.ReadOnly = true;
                    this.timeWorking_End.ReadOnly = true;
                    this.timeWorking_End_2.ReadOnly = true;

                    foreach (RepeaterItem item in rptOverTime.Items)
                    {
                        ITimeTextBox timeOverTime_Start = (ITimeTextBox)item.FindControl("timeOverTimeStart");
                        ITimeTextBox timeOverTime_End = (ITimeTextBox)item.FindControl("timeOverTimeEnd");

                        HtmlGenericControl divOverTimeStart = item.FindControl("divOverTimeStart") as HtmlGenericControl;
                        divOverTimeStart.Attributes.Remove("class");
                        HtmlGenericControl divOverTimeEnd = item.FindControl("divOverTimeEnd") as HtmlGenericControl;
                        divOverTimeEnd.Attributes.Remove("class");

                        if (timeOverTime_Start != null)
                        {
                            timeOverTime_Start.ReadOnly = true;
                            timeOverTime_End.ReadOnly = true;
                        }
                    }

                    this.optBreakType.Enabled = false;
                    this.timeBreak1_Start.ReadOnly = true;
                    this.timeBreak1_End.ReadOnly = true;
                    this.timeBreak2_Start.ReadOnly = true;
                    this.timeBreak2_End.ReadOnly = true;
                    this.timeBreak3_Start.ReadOnly = true;
                    this.timeBreak3_End.ReadOnly = true;
                    this.timeBreak4_Start.ReadOnly = true;
                    this.timeBreak4_End.ReadOnly = true;

                    this.timeDateSwitchTime.ReadOnly = true;
                    this.timeFirst_End.ReadOnly = true;
                    this.timeLatter_Start.ReadOnly = true;
                    this.timeAllOff_Hours.ReadOnly = true;
                    this.timeFirstOff_Hours.ReadOnly = true;
                    this.timeLatterOff_Hours.ReadOnly = true;

                    if (this.WorkingSystemID == ADMIN_ID)
                    {
                        base.DisabledLink(this.btnEdit, true);
                        base.DisabledLink(this.btnDelete, true);
                        base.DisabledLink(this.btnCopy, true);
                        base.DisabledLink(this.btnNew, true);
                    }
                    else
                    {
                        base.DisabledLink(this.btnEdit, !base._authority.IsMasterEdit);
                        base.DisabledLink(this.btnDelete, !base._authority.IsMasterDelete);
                        base.DisabledLink(this.btnCopy, !base._authority.IsMasterCopy);
                        base.DisabledLink(this.btnNew, !base._authority.IsMasterNew);
                    }

                    break;
            }
        }

        /// <summary>
        /// Show Data
        /// </summary>
        /// <param name="m_WorkingSystem"></param>
        private void ShowData(M_WorkingSystem m_WorkingSystem)
        {
            if (m_WorkingSystem != null)
            {
                M_WorkingSystem m_WorkingSystemData = GetDataWorkingSystemById(m_WorkingSystem.ID);

                if (m_WorkingSystemData.BreakType == OPT_BREAK_TYPE_0)
                {
                    StatusDisplayBreakType = OPT_BREAK_TYPE_0;
                }
                else if (m_WorkingSystemData.BreakType == OPT_BREAK_TYPE_1)
                {
                    StatusDisplayBreakType = OPT_BREAK_TYPE_1;
                }
                else
                {
                    StatusDisplayBreakType = OPT_BREAK_TYPE_2;
                }

                if (m_WorkingSystem.WorkingType == OPT_WORKING_TYPE_0)
                {
                    StatusDisplayWorking = OPT_BREAK_TYPE_0;
                }
                else if (m_WorkingSystemData.WorkingType == OPT_WORKING_TYPE_1)
                {
                    StatusDisplayWorking = OPT_WORKING_TYPE_1;
                }
                else
                {
                    StatusDisplayWorking = OPT_WORKING_TYPE_2;
                }
                if (rptOverTime.Items.Count == 0)
                {
                    this.IsDisplayDiveOverTime = false;
                }
                else
                {
                    this.IsDisplayDiveOverTime = true;
                }
                this.WorkingSystemID = m_WorkingSystemData.ID;
                this.OldUpdateDate = m_WorkingSystemData.UpdateDate;
                this.txtWorkingSystemCD.Value = m_WorkingSystemData.WorkingSystemCD;
                this.txtWorkingSystemName.Value = m_WorkingSystemData.WorkingSystemName;
                this.txtWorkingSystemName2.Value = m_WorkingSystemData.WorkingSystemName2;
                this.timeWorking_Start.Value = m_WorkingSystemData.Working_Start != null ? Utilities.CommonUtil.IntToTime((int)m_WorkingSystemData.Working_Start, true) : string.Empty;
                this.timeWorking_End.Value = m_WorkingSystemData.Working_End != null ? Utilities.CommonUtil.IntToTime((int)m_WorkingSystemData.Working_End, true) : string.Empty;
                this.timeWorking_End_2.Value = m_WorkingSystemData.Working_End_2 != null ? Utilities.CommonUtil.IntToTime((int)m_WorkingSystemData.Working_End_2, true) : string.Empty;
                this.optWorkingType.SelectedValue = m_WorkingSystemData.WorkingType.ToString();

                foreach (RepeaterItem item in rptOverTime.Items)
                {
                    ITimeTextBox timeOverTime_Start = (ITimeTextBox)item.FindControl("timeOverTimeStart");
                    ITimeTextBox timeOverTime_End = (ITimeTextBox)item.FindControl("timeOverTimeEnd");

                    switch (item.ItemIndex)
                    {
                        case 0:
                            timeOverTime_Start.Value = m_WorkingSystemData.OverTime1_Start != null ? Utilities.CommonUtil.IntToTime((int)m_WorkingSystemData.OverTime1_Start, true) : string.Empty;
                            timeOverTime_End.Value = m_WorkingSystemData.OverTime1_End != null ? Utilities.CommonUtil.IntToTime((int)m_WorkingSystemData.OverTime1_End, true) : string.Empty;
                            break;
                        case 1:
                            timeOverTime_Start.Value = m_WorkingSystemData.OverTime2_Start != null ? Utilities.CommonUtil.IntToTime((int)m_WorkingSystemData.OverTime2_Start, true) : string.Empty;
                            timeOverTime_End.Value = m_WorkingSystemData.OverTime2_End != null ? Utilities.CommonUtil.IntToTime((int)m_WorkingSystemData.OverTime2_End, true) : string.Empty;
                            break;
                        case 2:
                            timeOverTime_Start.Value = m_WorkingSystemData.OverTime3_Start != null ? Utilities.CommonUtil.IntToTime((int)m_WorkingSystemData.OverTime3_Start, true) : string.Empty;
                            timeOverTime_End.Value = m_WorkingSystemData.OverTime3_End != null ? Utilities.CommonUtil.IntToTime((int)m_WorkingSystemData.OverTime3_End, true) : string.Empty;
                            break;
                        case 3:
                            timeOverTime_Start.Value = m_WorkingSystemData.OverTime4_Start != null ? Utilities.CommonUtil.IntToTime((int)m_WorkingSystemData.OverTime4_Start, true) : string.Empty;
                            timeOverTime_End.Value = m_WorkingSystemData.OverTime4_End != null ? Utilities.CommonUtil.IntToTime((int)m_WorkingSystemData.OverTime4_End, true) : string.Empty;
                            break;
                        case 4:
                            timeOverTime_Start.Value = m_WorkingSystemData.OverTime5_Start != null ? Utilities.CommonUtil.IntToTime((int)m_WorkingSystemData.OverTime5_Start, true) : string.Empty;
                            timeOverTime_End.Value = m_WorkingSystemData.OverTime5_Start != null ? Utilities.CommonUtil.IntToTime((int)m_WorkingSystemData.OverTime5_End, true) : string.Empty;
                            break;
                    }
                }

                this.optBreakType.SelectedValue = m_WorkingSystemData.BreakType.ToString();
                this.timeBreak1_Start.Value = m_WorkingSystemData.Break1_Start != null ? Utilities.CommonUtil.IntToTime((int)m_WorkingSystemData.Break1_Start, true) : string.Empty;
                this.timeBreak1_End.Value = m_WorkingSystemData.Break1_End != null ? Utilities.CommonUtil.IntToTime((int)m_WorkingSystemData.Break1_End, true) : string.Empty;
                this.timeBreak2_Start.Value = m_WorkingSystemData.Break2_Start != null ? Utilities.CommonUtil.IntToTime((int)m_WorkingSystemData.Break2_Start, true) : string.Empty;
                this.timeBreak2_End.Value = m_WorkingSystemData.Break2_End != null ? Utilities.CommonUtil.IntToTime((int)m_WorkingSystemData.Break2_End, true) : string.Empty;
                this.timeBreak3_Start.Value = m_WorkingSystemData.Break3_Start != null ? Utilities.CommonUtil.IntToTime((int)m_WorkingSystemData.Break3_Start, true) : string.Empty;
                this.timeBreak3_End.Value = m_WorkingSystemData.Break3_End != null ? Utilities.CommonUtil.IntToTime((int)m_WorkingSystemData.Break3_End, true) : string.Empty;
                this.timeBreak4_Start.Value = m_WorkingSystemData.Break4_Start != null ? Utilities.CommonUtil.IntToTime((int)m_WorkingSystemData.Break4_Start, true) : string.Empty;
                this.timeBreak4_End.Value = m_WorkingSystemData.Break4_End != null ? Utilities.CommonUtil.IntToTime((int)m_WorkingSystemData.Break4_End, true) : string.Empty;

                this.timeDateSwitchTime.Value = m_WorkingSystemData.DateSwitchTime != null ? Utilities.CommonUtil.IntToTime(m_WorkingSystemData.DateSwitchTime.Value, false) : string.Empty;
                this.timeFirst_End.Value = m_WorkingSystemData.First_End != null ? Utilities.CommonUtil.IntToTime(m_WorkingSystemData.First_End.Value, false) : string.Empty;
                this.timeLatter_Start.Value = m_WorkingSystemData.Latter_Start != null ? Utilities.CommonUtil.IntToTime(m_WorkingSystemData.Latter_Start.Value, false) : string.Empty;
                this.timeAllOff_Hours.Value = m_WorkingSystemData.AllOff_Hours != null ? Utilities.CommonUtil.IntToTime(m_WorkingSystemData.AllOff_Hours.Value, false) : string.Empty;
                this.timeFirstOff_Hours.Value = m_WorkingSystemData.FirstOff_Hours != null ? Utilities.CommonUtil.IntToTime(m_WorkingSystemData.FirstOff_Hours.Value, false) : string.Empty;
                this.timeLatterOff_Hours.Value = m_WorkingSystemData.LatterOff_Hours != null ? Utilities.CommonUtil.IntToTime(m_WorkingSystemData.LatterOff_Hours.Value, false) : string.Empty;
                //this.timeFirst_End.Value = Utilities.CommonUtil.IntToTime(m_WorkingSystemData.First_End, false);
                //this.timeLatter_Start.Value = Utilities.CommonUtil.IntToTime(m_WorkingSystemData.Latter_Start, false);
                //this.timeAllOff_Hours.Value = Utilities.CommonUtil.IntToTime(m_WorkingSystemData.AllOff_Hours, false);
                //this.timeFirstOff_Hours.Value = Utilities.CommonUtil.IntToTime(m_WorkingSystemData.FirstOff_Hours, false);
                //this.timeLatterOff_Hours.Value = Utilities.CommonUtil.IntToTime(m_WorkingSystemData.LatterOff_Hours, false);

            }
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
                M_WorkingSystem m_WorkingSystem = new M_WorkingSystem();

                m_WorkingSystem.WorkingSystemCD = this.txtWorkingSystemCD.Value;
                m_WorkingSystem.WorkingSystemName = this.txtWorkingSystemName.Value;
                m_WorkingSystem.WorkingSystemName2 = this.txtWorkingSystemName2.Value;

                m_WorkingSystem.WorkingType = int.Parse(this.optWorkingType.SelectedValue);
                if (this.timeWorking_Start.Value != null)
                {
                    m_WorkingSystem.Working_Start = Utilities.CommonUtil.TimeToInt(this.timeWorking_Start.Value);
                }
                if (this.timeWorking_End.Value != null)
                {
                    m_WorkingSystem.Working_End = Utilities.CommonUtil.TimeToInt(this.timeWorking_End.Value);
                }
                if (this.timeWorking_End_2.Value != null)
                {
                    m_WorkingSystem.Working_End_2 = Utilities.CommonUtil.TimeToInt(this.timeWorking_End_2.Value);
                }

                foreach (RepeaterItem item in rptOverTime.Items)
                {
                    ITimeTextBox timeOverTime_Start = (ITimeTextBox)item.FindControl("timeOverTimeStart");
                    ITimeTextBox timeOverTime_End = (ITimeTextBox)item.FindControl("timeOverTimeEnd");

                    switch (item.ItemIndex)
                    {
                        case 0:
                            if (timeOverTime_Start.Value != null)
                            {
                                m_WorkingSystem.OverTime1_Start = Utilities.CommonUtil.TimeToInt(timeOverTime_Start.Value);
                            }
                            if (timeOverTime_End.Value != null)
                            {
                                m_WorkingSystem.OverTime1_End = Utilities.CommonUtil.TimeToInt(timeOverTime_End.Value);
                            }
                            break;
                        case 1:
                            if (timeOverTime_Start.Value != null)
                            {
                                m_WorkingSystem.OverTime2_Start = Utilities.CommonUtil.TimeToInt(timeOverTime_Start.Value);
                            }
                            if (timeOverTime_End.Value != null)
                            {
                                m_WorkingSystem.OverTime2_End = Utilities.CommonUtil.TimeToInt(timeOverTime_End.Value);
                            }
                            break;
                        case 2:
                            if (timeOverTime_Start.Value != null)
                            {
                                m_WorkingSystem.OverTime3_Start = Utilities.CommonUtil.TimeToInt(timeOverTime_Start.Value);
                            }
                            if (timeOverTime_End.Value != null)
                            {
                                m_WorkingSystem.OverTime3_End = Utilities.CommonUtil.TimeToInt(timeOverTime_End.Value);
                            }
                            break;
                        case 3:
                            if (timeOverTime_Start.Value != null)
                            {
                                m_WorkingSystem.OverTime4_Start = Utilities.CommonUtil.TimeToInt(timeOverTime_Start.Value);
                            }
                            if (timeOverTime_End.Value != null)
                            {
                                m_WorkingSystem.OverTime4_End = Utilities.CommonUtil.TimeToInt(timeOverTime_End.Value);
                            }
                            break;
                        case 4:
                            if (timeOverTime_Start.Value != null)
                            {
                                m_WorkingSystem.OverTime5_Start = Utilities.CommonUtil.TimeToInt(timeOverTime_Start.Value);
                            }
                            if (timeOverTime_End.Value != null)
                            {
                                m_WorkingSystem.OverTime5_End = Utilities.CommonUtil.TimeToInt(timeOverTime_End.Value);
                            }
                            break;
                    }
                }

                m_WorkingSystem.BreakType = int.Parse(this.optBreakType.SelectedValue);

                switch (int.Parse(this.optBreakType.SelectedValue))
                {
                    case OPT_BREAK_TYPE_0:
                        if (this.timeBreak1_Start.Value != null)
                        {
                            m_WorkingSystem.Break1_Start = Utilities.CommonUtil.TimeToInt(this.timeBreak1_Start.Value);
                        }
                        if (this.timeBreak1_End.Value != null)
                        {
                            m_WorkingSystem.Break1_End = Utilities.CommonUtil.TimeToInt(this.timeBreak1_End.Value);

                        }
                        if (this.timeBreak2_Start.Value != null)
                        {
                            m_WorkingSystem.Break2_Start = Utilities.CommonUtil.TimeToInt(this.timeBreak2_Start.Value);
                        }
                        if (this.timeBreak2_End.Value != null)
                        {
                            m_WorkingSystem.Break2_End = Utilities.CommonUtil.TimeToInt(this.timeBreak2_End.Value);

                        }
                        if (this.timeBreak3_Start.Value != null)
                        {
                            m_WorkingSystem.Break3_Start = Utilities.CommonUtil.TimeToInt(this.timeBreak3_Start.Value);
                        }
                        if (this.timeBreak3_End.Value != null)
                        {
                            m_WorkingSystem.Break3_End = Utilities.CommonUtil.TimeToInt(this.timeBreak3_End.Value);

                        }
                        if (this.timeBreak4_Start.Value != null)
                        {
                            m_WorkingSystem.Break4_Start = Utilities.CommonUtil.TimeToInt(this.timeBreak4_Start.Value);
                        }
                        if (this.timeBreak4_End.Value != null)
                        {
                            m_WorkingSystem.Break4_End = Utilities.CommonUtil.TimeToInt(this.timeBreak4_End.Value);

                        }

                        break;
                    case OPT_BREAK_TYPE_1:
                        if (this.timeBreak1_Start.Value != null)
                        {
                            m_WorkingSystem.Break1_Start = Utilities.CommonUtil.TimeToInt(this.timeBreak1_Start.Value);
                        }
                        break;
                    case OPT_BREAK_TYPE_2:
                        if (this.timeBreak1_Start.Value != null)
                        {
                            m_WorkingSystem.Break1_Start = Utilities.CommonUtil.TimeToInt(this.timeBreak1_Start.Value);
                        }
                        if (this.timeBreak1_End.Value != null)
                        {
                            m_WorkingSystem.Break1_End = Utilities.CommonUtil.TimeToInt(this.timeBreak1_End.Value);

                        }
                        if (this.timeBreak2_Start.Value != null)
                        {
                            m_WorkingSystem.Break2_Start = Utilities.CommonUtil.TimeToInt(this.timeBreak2_Start.Value);
                        }
                        if (this.timeBreak2_End.Value != null)
                        {
                            m_WorkingSystem.Break2_End = Utilities.CommonUtil.TimeToInt(this.timeBreak2_End.Value);

                        }
                        break;

                }


                if (this.timeDateSwitchTime.Value != null)
                {
                    m_WorkingSystem.DateSwitchTime = Utilities.CommonUtil.TimeToInt(this.timeDateSwitchTime.Value);
                }

                if (this.timeFirst_End.Value != null)
                {
                    m_WorkingSystem.First_End = Utilities.CommonUtil.TimeToInt(this.timeFirst_End.Value);
                }

                if (this.timeLatter_Start.Value != null)
                {
                    m_WorkingSystem.Latter_Start = Utilities.CommonUtil.TimeToInt(this.timeLatter_Start.Value);
                }

                if (this.timeAllOff_Hours.Value != null)
                {
                    m_WorkingSystem.AllOff_Hours = Utilities.CommonUtil.TimeToInt(this.timeAllOff_Hours.Value);
                }

                if (this.timeFirstOff_Hours.Value != null)
                {
                    m_WorkingSystem.FirstOff_Hours = Utilities.CommonUtil.TimeToInt(this.timeFirstOff_Hours.Value);
                }

                if (this.timeLatterOff_Hours.Value != null)
                {
                    m_WorkingSystem.LatterOff_Hours = Utilities.CommonUtil.TimeToInt(this.timeLatterOff_Hours.Value);
                }

                //m_WorkingSystem.DateSwitchTime = this.timeDateSwitchTime.Value != null ? Utilities.CommonUtil.TimeToInt(this.timeDateSwitchTime.Value) : 0;
                //m_WorkingSystem.First_End = this.timeFirst_End.Value != null ? Utilities.CommonUtil.TimeToInt(this.timeFirst_End.Value) : 0;
                //m_WorkingSystem.Latter_Start = this.timeLatter_Start.Value != null ? Utilities.CommonUtil.TimeToInt(this.timeLatter_Start.Value) : 0;
                //m_WorkingSystem.AllOff_Hours = this.timeAllOff_Hours.Value != null ? Utilities.CommonUtil.TimeToInt(this.timeAllOff_Hours.Value) : 0;
                //m_WorkingSystem.FirstOff_Hours = this.timeFirstOff_Hours.Value != null ? Utilities.CommonUtil.TimeToInt(this.timeFirstOff_Hours.Value) : 0;
                //m_WorkingSystem.LatterOff_Hours = this.timeLatterOff_Hours.Value != null ? Utilities.CommonUtil.TimeToInt(this.timeLatterOff_Hours.Value) : 0;

                m_WorkingSystem.CreateUID = this.LoginInfo.User.ID;
                m_WorkingSystem.UpdateUID = this.LoginInfo.User.ID;

                using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                {
                    WorkingSystemService workingSystemSer = new WorkingSystemService(db);
                    workingSystemSer.Insert(m_WorkingSystem);
                    db.Commit();
                }
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains(Models.Constant.M_WORKINGSYSTEM_UN))
                {
                    this.SetMessage(this.txtWorkingSystemCD.ID, M_Message.MSG_EXIST_CODE, "Working System CD");
                }

                Log.Instance.WriteLog(ex);

                return false;
            }
            catch (Exception ex)
            {
                this.SetMessage(string.Empty, M_Message.MSG_UPDATE_FAILE, "Insert");

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
                M_WorkingSystem m_WorkingSystem = this.GetDataWorkingSystemByCd(this.txtWorkingSystemCD.Value);

                if (m_WorkingSystem != null)
                {
                    ///Create model
                    m_WorkingSystem.ID = this.WorkingSystemID;

                    m_WorkingSystem.WorkingSystemCD = this.txtWorkingSystemCD.Value;
                    m_WorkingSystem.WorkingSystemName = this.txtWorkingSystemName.Value;
                    m_WorkingSystem.WorkingSystemName2 = this.txtWorkingSystemName2.Value;

                    m_WorkingSystem.WorkingType = int.Parse(this.optWorkingType.SelectedValue);
                    switch (int.Parse(this.optWorkingType.SelectedValue))
                    {
                        case OPT_WORKING_TYPE_0:
                            if (this.timeWorking_Start.Value != null)
                            {
                                m_WorkingSystem.Working_Start = Utilities.CommonUtil.TimeToInt(this.timeWorking_Start.Value);
                            }
                            else
                            {
                                m_WorkingSystem.Working_Start = null;
                            }
                            if (this.timeWorking_End.Value != null)
                            {
                                m_WorkingSystem.Working_End = this.timeWorking_End.Value != null ? Utilities.CommonUtil.TimeToInt(this.timeWorking_End.Value) : 0;
                            }
                            else
                            {
                                m_WorkingSystem.Working_End = null;
                            }
                            if (this.timeWorking_End_2.Value != null)
                            {
                                m_WorkingSystem.Working_End_2 = this.timeWorking_End_2.Value != null ? Utilities.CommonUtil.TimeToInt(this.timeWorking_End_2.Value) : 0;
                            }
                            else
                            {
                                m_WorkingSystem.Working_End_2 = null;
                            }
                            if (this.timeDateSwitchTime.Value != null)
                            {
                                m_WorkingSystem.DateSwitchTime = Utilities.CommonUtil.TimeToInt(this.timeDateSwitchTime.Value);
                            }
                            else
                            {
                                m_WorkingSystem.DateSwitchTime = null;
                            }

                            if (this.timeFirst_End.Value != null)
                            {
                                m_WorkingSystem.First_End = Utilities.CommonUtil.TimeToInt(this.timeFirst_End.Value);
                            }
                            else
                            {
                                m_WorkingSystem.First_End = null;
                            }

                            if (this.timeLatter_Start.Value != null)
                            {
                                m_WorkingSystem.Latter_Start = Utilities.CommonUtil.TimeToInt(this.timeLatter_Start.Value);
                            }
                            else
                            {
                                m_WorkingSystem.Latter_Start = null;
                            }

                            if (this.timeAllOff_Hours.Value != null)
                            {
                                m_WorkingSystem.AllOff_Hours = Utilities.CommonUtil.TimeToInt(this.timeAllOff_Hours.Value);
                            }
                            else
                            {
                                m_WorkingSystem.AllOff_Hours = null;
                            }

                            if (this.timeFirstOff_Hours.Value != null)
                            {
                                m_WorkingSystem.FirstOff_Hours = Utilities.CommonUtil.TimeToInt(this.timeFirstOff_Hours.Value);
                            }
                            else
                            {
                                m_WorkingSystem.FirstOff_Hours = null;
                            }

                            if (this.timeLatterOff_Hours.Value != null)
                            {
                                m_WorkingSystem.LatterOff_Hours = Utilities.CommonUtil.TimeToInt(this.timeLatterOff_Hours.Value);
                            }
                            else
                            {
                                m_WorkingSystem.LatterOff_Hours = null;
                            }
                            break;
                        case OPT_WORKING_TYPE_1:
                        case OPT_WORKING_TYPE_2:
                            m_WorkingSystem.Working_Start = null;
                            m_WorkingSystem.Working_End = null;
                            m_WorkingSystem.DateSwitchTime = null;
                            m_WorkingSystem.First_End = null;
                            m_WorkingSystem.Latter_Start = null;
                            m_WorkingSystem.AllOff_Hours = null;
                            m_WorkingSystem.FirstOff_Hours = null;
                            m_WorkingSystem.LatterOff_Hours = null;
                            break;
                    }

                    m_WorkingSystem.OverTime1_Start = null;
                    m_WorkingSystem.OverTime1_End = null;
                    m_WorkingSystem.OverTime2_Start = null;
                    m_WorkingSystem.OverTime2_End = null;
                    m_WorkingSystem.OverTime3_Start = null;
                    m_WorkingSystem.OverTime3_End = null;
                    m_WorkingSystem.OverTime4_Start = null;
                    m_WorkingSystem.OverTime4_End = null;
                    m_WorkingSystem.OverTime5_Start = null;
                    m_WorkingSystem.OverTime5_End = null;
                    foreach (RepeaterItem item in rptOverTime.Items)
                    {
                        ITimeTextBox timeOverTime_Start = (ITimeTextBox)item.FindControl("timeOverTimeStart");
                        ITimeTextBox timeOverTime_End = (ITimeTextBox)item.FindControl("timeOverTimeEnd");

                        switch (item.ItemIndex)
                        {
                            case 0:
                                if (timeOverTime_Start.Value != null)
                                {
                                    m_WorkingSystem.OverTime1_Start = Utilities.CommonUtil.TimeToInt(timeOverTime_Start.Value);
                                }
                                if (timeOverTime_End.Value != null)
                                {
                                    m_WorkingSystem.OverTime1_End = Utilities.CommonUtil.TimeToInt(timeOverTime_End.Value);
                                }
                                break;
                            case 1:
                                if (timeOverTime_Start.Value != null)
                                {
                                    m_WorkingSystem.OverTime2_Start = Utilities.CommonUtil.TimeToInt(timeOverTime_Start.Value);
                                }
                                if (timeOverTime_End.Value != null)
                                {
                                    m_WorkingSystem.OverTime2_End = Utilities.CommonUtil.TimeToInt(timeOverTime_End.Value);
                                }
                                break;
                            case 2:
                                if (timeOverTime_Start.Value != null)
                                {
                                    m_WorkingSystem.OverTime3_Start = Utilities.CommonUtil.TimeToInt(timeOverTime_Start.Value);
                                }
                                if (timeOverTime_End.Value != null)
                                {
                                    m_WorkingSystem.OverTime3_End = Utilities.CommonUtil.TimeToInt(timeOverTime_End.Value);
                                }
                                break;
                            case 3:
                                if (timeOverTime_Start.Value != null)
                                {
                                    m_WorkingSystem.OverTime4_Start = Utilities.CommonUtil.TimeToInt(timeOverTime_Start.Value);
                                }
                                if (timeOverTime_End.Value != null)
                                {
                                    m_WorkingSystem.OverTime4_End = Utilities.CommonUtil.TimeToInt(timeOverTime_End.Value);
                                }
                                break;
                            case 4:
                                if (timeOverTime_Start.Value != null)
                                {
                                    m_WorkingSystem.OverTime5_Start = Utilities.CommonUtil.TimeToInt(timeOverTime_Start.Value);
                                }
                                if (timeOverTime_End.Value != null)
                                {
                                    m_WorkingSystem.OverTime5_End = Utilities.CommonUtil.TimeToInt(timeOverTime_End.Value);
                                }
                                break;
                        }
                    }
                    m_WorkingSystem.BreakType = int.Parse(this.optBreakType.SelectedValue);

                    m_WorkingSystem.Break1_Start = null;
                    m_WorkingSystem.Break1_End = null;
                    m_WorkingSystem.Break2_Start = null;
                    m_WorkingSystem.Break2_End = null;
                    m_WorkingSystem.Break3_Start = null;
                    m_WorkingSystem.Break3_End = null;
                    m_WorkingSystem.Break4_Start = null;
                    m_WorkingSystem.Break4_End = null;
                    switch (int.Parse(this.optBreakType.SelectedValue))
                    {
                        case OPT_BREAK_TYPE_0:
                            if (this.timeBreak1_Start.Value != null)
                            {
                                m_WorkingSystem.Break1_Start = Utilities.CommonUtil.TimeToInt(this.timeBreak1_Start.Value);
                            }
                            if (this.timeBreak1_End.Value != null)
                            {
                                m_WorkingSystem.Break1_End = Utilities.CommonUtil.TimeToInt(this.timeBreak1_End.Value);
                            }
                            if (this.timeBreak2_Start.Value != null)
                            {
                                m_WorkingSystem.Break2_Start = Utilities.CommonUtil.TimeToInt(this.timeBreak2_Start.Value);
                            }
                            if (this.timeBreak2_End.Value != null)
                            {
                                m_WorkingSystem.Break2_End = Utilities.CommonUtil.TimeToInt(this.timeBreak2_End.Value);
                            }
                            if (this.timeBreak3_Start.Value != null)
                            {
                                m_WorkingSystem.Break3_Start = Utilities.CommonUtil.TimeToInt(this.timeBreak3_Start.Value);
                            }
                            if (this.timeBreak3_End.Value != null)
                            {
                                m_WorkingSystem.Break3_End = Utilities.CommonUtil.TimeToInt(this.timeBreak3_End.Value);
                            }
                            if (this.timeBreak4_Start.Value != null)
                            {
                                m_WorkingSystem.Break4_Start = Utilities.CommonUtil.TimeToInt(this.timeBreak4_Start.Value);
                            }
                            if (this.timeBreak4_End.Value != null)
                            {
                                m_WorkingSystem.Break4_End = Utilities.CommonUtil.TimeToInt(this.timeBreak4_End.Value);
                            }
                            break;
                        case OPT_BREAK_TYPE_1:
                            if (this.timeBreak1_Start.Value != null)
                            {
                                m_WorkingSystem.Break1_Start = Utilities.CommonUtil.TimeToInt(this.timeBreak1_Start.Value);
                            }
                            break;
                        case OPT_BREAK_TYPE_2:
                            if (this.timeBreak1_Start.Value != null)
                            {
                                m_WorkingSystem.Break1_Start = Utilities.CommonUtil.TimeToInt(this.timeBreak1_Start.Value);
                            }
                            if (this.timeBreak1_End.Value != null)
                            {
                                m_WorkingSystem.Break1_End = Utilities.CommonUtil.TimeToInt(this.timeBreak1_End.Value);
                            }
                            break;

                    }

                    //m_WorkingSystem.DateSwitchTime = this.timeDateSwitchTime.Value != null ? Utilities.CommonUtil.TimeToInt(this.timeDateSwitchTime.Value) : 0;
                    //m_WorkingSystem.First_End = this.timeFirst_End.Value != null ? Utilities.CommonUtil.TimeToInt(this.timeFirst_End.Value) : 0;
                    //m_WorkingSystem.Latter_Start = this.timeLatter_Start.Value != null ? Utilities.CommonUtil.TimeToInt(this.timeLatter_Start.Value) : 0;
                    //m_WorkingSystem.AllOff_Hours = this.timeAllOff_Hours.Value != null ? Utilities.CommonUtil.TimeToInt(this.timeAllOff_Hours.Value) : 0;
                    //m_WorkingSystem.FirstOff_Hours = this.timeFirstOff_Hours.Value != null ? Utilities.CommonUtil.TimeToInt(this.timeFirstOff_Hours.Value) : 0;
                    //m_WorkingSystem.LatterOff_Hours = this.timeLatterOff_Hours.Value != null ? Utilities.CommonUtil.TimeToInt(this.timeLatterOff_Hours.Value) : 0;

                    m_WorkingSystem.UpdateDate = this.OldUpdateDate;
                    m_WorkingSystem.UpdateUID = this.LoginInfo.User.ID;

                    //Update                     
                    using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                    {
                        WorkingSystemService workingSystemSer = new WorkingSystemService(db);

                        //Update User
                        if (m_WorkingSystem.Status == DataStatus.Changed)
                        {
                            ret = workingSystemSer.Update(m_WorkingSystem);

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
                if (ex.Message.Contains(Models.Constant.M_WORKINGSYSTEM_UN))
                {
                    this.SetMessage(this.txtWorkingSystemCD.ID, M_Message.MSG_EXIST_CODE, "Working System CD");
                }

                Log.Instance.WriteLog(ex);

                return false;
            }
            catch (Exception ex)
            {
                this.SetMessage(string.Empty, M_Message.MSG_UPDATE_FAILE, "Update");

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
                int ret = 0;
                using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                {
                    WorkingSystemService workingSystemSer = new WorkingSystemService(db);
                    ret = workingSystemSer.DeleteWorkingSystemByID(this.WorkingSystemID);

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
                if (ex.Message.Contains(Models.Constant.T_ATTENDANCE_FK_WORKINGSYSTEM) || ex.Message.Contains(Models.Constant.T_WORKINGCALENDAR_D_FK_WORKINGSYSTEM))
                {
                    this.SetMessage(string.Empty, M_Message.MSG_EXIST_CANT_DELETE, "Working System CD " + this.txtWorkingSystemCD.Value);
                }

                Log.Instance.WriteLog(ex);

                return false;
            }
            catch (Exception ex)
            {
                this.SetMessage(string.Empty, M_Message.MSG_UPDATE_FAILE, "Delete");

                Log.Instance.WriteLog(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Get Data Working System Cd
        /// </summary>
        /// <param name="workingSystemCd"></param>
        /// <returns></returns>
        private M_WorkingSystem GetDataWorkingSystemByCd(string workingSystemCd)
        {
            using (DB db = new DB())
            {
                WorkingSystemService workingSystemSer = new WorkingSystemService(db);
                return workingSystemSer.GetByWorkingSystemCd(workingSystemCd);
            }
        }

        /// <summary>
        /// Get Data Working System By Id
        /// </summary>
        /// <param name="workingSystemId"></param>
        /// <returns></returns>
        private M_WorkingSystem GetDataWorkingSystemById(int workingSystemId)
        {
            using (DB db = new DB())
            {
                WorkingSystemService workingSystemSer = new WorkingSystemService(db);
                return workingSystemSer.GetDataWorkingSystemById(workingSystemId);
            }
        }

        /// <summary>
        /// Check input
        /// </summary>
        /// <returns>Valid:true, Invalid:false</returns>
        private bool CheckInput()
        {
            //txtWorkingSystemCD
            if (this.txtWorkingSystemCD.IsEmpty)
            {
                this.SetMessage(this.txtWorkingSystemCD.ID, M_Message.MSG_REQUIRE, "勤務体系コード");
            }
            else
            {
                if (this.Mode == Mode.Insert || this.Mode == Mode.Copy)
                {

                    // Check WorkingSystem by workingSystemCd 
                    if (this.GetDataWorkingSystemByCd(this.txtWorkingSystemCD.Value) != null)
                    {
                        this.SetMessage(this.txtWorkingSystemCD.ID, M_Message.MSG_EXIST_CODE, "勤務体系コード");
                    }

                }
            }

            //txtWorkingSystemName
            if (this.txtWorkingSystemName.IsEmpty)
            {
                this.SetMessage(this.txtWorkingSystemName.ID, M_Message.MSG_REQUIRE, "勤務体系名称");
            }

            if (int.Parse(this.optWorkingType.SelectedValue) == OPT_WORKING_TYPE_0)
            {
                StatusDisplayWorking = OPT_WORKING_TYPE_0;

                //timeWorking_Start
                if (this.timeWorking_Start.IsEmpty)
                {
                    this.SetMessage(this.timeWorking_Start.ID, M_Message.MSG_REQUIRE, "就業時間（開始）");
                }

                //timeWorking_End
                if (this.timeWorking_End.IsEmpty)
                {
                    this.SetMessage(this.timeWorking_End.ID, M_Message.MSG_REQUIRE, "就業時間（終了）");
                }
            }
            else if (int.Parse(this.optWorkingType.SelectedValue) == OPT_WORKING_TYPE_1)
            {
                StatusDisplayWorking = OPT_WORKING_TYPE_1;
            }
            else
            {
                StatusDisplayWorking = OPT_WORKING_TYPE_2;
            }

            int index = 0;
            ITimeTextBox timeOverTime_StartBefore = null;
            ITimeTextBox timeOverTime_EndBefore = null;
            HtmlGenericControl divOverTimeStartBefore = null;

            if (rptOverTime.Items.Count > 0)
            {
                foreach (RepeaterItem item in rptOverTime.Items)
                {
                    index++;

                    if (index == 6)
                    {
                        break;
                    }
                    ITimeTextBox timeOverTime_Start = (ITimeTextBox)item.FindControl("timeOverTimeStart");
                    ITimeTextBox timeOverTime_End = (ITimeTextBox)item.FindControl("timeOverTimeEnd");

                    int rowIndex = item.ItemIndex + 1;
                    HtmlGenericControl divOverTimeStart = (HtmlGenericControl)item.FindControl("divOverTimeStart");
                    divOverTimeStart.Attributes.Remove("class");
                    HtmlGenericControl divOverTimeEnd = (HtmlGenericControl)item.FindControl("divOverTimeEnd");
                    divOverTimeEnd.Attributes.Remove("class");
                    if (timeOverTime_Start != null && timeOverTime_End != null)
                    {
                        string errorId;
                        string errorMsg;
                        if ((timeOverTime_Start.IsEmpty && (!timeOverTime_End.IsEmpty))
                    || ((!timeOverTime_Start.IsEmpty) && timeOverTime_End.IsEmpty)
                    )
                        {
                            if (timeOverTime_Start.IsEmpty)
                            {
                                errorId = timeOverTime_Start.ClientID;
                                errorMsg = "残業時間" + rowIndex + "（開始）";
                                this.SetMessage(errorId, M_Message.MSG_REQUIRE_GRID, errorMsg, rowIndex);
                                this.AddErrorForListItem(divOverTimeStart, errorId);
                            }
                            if (timeOverTime_End.IsEmpty)
                            {
                                errorId = timeOverTime_End.ClientID;
                                errorMsg = "残業時間" + rowIndex + "（終了）";
                                this.SetMessage(errorId, M_Message.MSG_REQUIRE_GRID, errorMsg, rowIndex);
                                this.AddErrorForListItem(divOverTimeEnd, errorId);
                            }
                        }

                        if ((!timeOverTime_Start.IsEmpty) && (!timeOverTime_End.IsEmpty))
                        {
                            if (Utilities.CommonUtil.TimeToInt(timeOverTime_Start.Value) >= Utilities.CommonUtil.TimeToInt(timeOverTime_End.Value))
                            {
                                errorId = timeOverTime_Start.ClientID;
                                this.SetMessage(errorId, M_Message.MSG_GREATER_THAN_GRID, "残業時間" + rowIndex + "（終了）", "残業時間" + rowIndex + "（開始）", rowIndex);
                                this.AddErrorForListItem(divOverTimeStart, errorId);
                            }
                        }

                        if (index > 1)
                        {
                            if (!timeOverTime_StartBefore.IsEmpty && !timeOverTime_EndBefore.IsEmpty && !timeOverTime_Start.IsEmpty && !timeOverTime_End.IsEmpty &&
                                Utilities.CommonUtil.TimeToInt(timeOverTime_Start.Value) < Utilities.CommonUtil.TimeToInt(timeOverTime_EndBefore.Value)
                                )
                            {
                                errorId = timeOverTime_Start.ClientID;
                                this.SetMessage(errorId, M_Message.MSG_GREATER_THAN_EQUAL_GRID, "残業時間" + rowIndex + "（開始）", "残業時間" + (rowIndex - 1) + "（終了）", rowIndex);
                                this.AddErrorForListItem(divOverTimeStart, errorId);
                            }
                        }

                        if ((!this.timeWorking_Start.IsEmpty) && (!this.timeWorking_End.IsEmpty)
                            && (!timeOverTime_Start.IsEmpty) && (!timeOverTime_End.IsEmpty))
                        {
                            int work_Start = Utilities.CommonUtil.TimeToInt(this.timeWorking_Start.Value);
                            int work_End = Utilities.CommonUtil.TimeToInt(this.timeWorking_End.Value);
                            int over_Start = Utilities.CommonUtil.TimeToInt(timeOverTime_Start.Value);
                            int over_End = Utilities.CommonUtil.TimeToInt(timeOverTime_End.Value);

                            if (!((over_Start <= over_End && over_End <= work_Start) || (work_End <= over_Start && over_Start <= over_End)))
                            {
                                errorId = timeOverTime_Start.ClientID;
                                errorMsg = "残業時間" + rowIndex + "（開始）, 残業時間" + (rowIndex) + "（終了）";
                                this.SetMessage(errorId, M_Message.MSG_INVALID_GRID, errorMsg, rowIndex);
                                this.AddErrorForListItem(divOverTimeStart, errorId);
                            }
                        }


                        timeOverTime_StartBefore = (ITimeTextBox)item.FindControl("timeOverTimeStart");
                        timeOverTime_EndBefore = (ITimeTextBox)item.FindControl("timeOverTimeEnd");
                        divOverTimeStartBefore = (HtmlGenericControl)item.FindControl("divOverTimeStart");
                    }

                }
                //Check validate time
                ArrayList arr = new ArrayList();
                for (int i = 0; i < rptOverTime.Items.Count; i++)
                {
                    string errorMsg;
                    RepeaterItem item = rptOverTime.Items[i];
                    ITimeTextBox timeOverTime_Start = (ITimeTextBox)item.FindControl("timeOverTimeStart");
                    ITimeTextBox timeOverTime_End = (ITimeTextBox)item.FindControl("timeOverTimeEnd");
                    int rowIndex = item.ItemIndex + 1;
                    HtmlGenericControl divOverTimeStart = (HtmlGenericControl)item.FindControl("divOverTimeStart");
                    HtmlGenericControl divOverTimeEnd = (HtmlGenericControl)item.FindControl("divOverTimeEnd");

                    if (!timeOverTime_Start.IsEmpty && !timeOverTime_End.IsEmpty)
                    {
                        string errorId;
                        if (!CheckValidateOverTime(i, timeOverTime_Start, timeOverTime_End))
                        {
                            errorId = timeOverTime_Start.ClientID;
                            errorMsg = "残業時間" + rowIndex + "（開始）, 残業時間" + (rowIndex) + "（終了）";
                            this.SetMessage(errorId, M_Message.MSG_INVALID_GRID, errorMsg, rowIndex);
                            this.AddErrorForListItem(divOverTimeStart, errorId);
                        }
                    }
                }
            }

            if (int.Parse(this.optWorkingType.SelectedValue) == OPT_WORKING_TYPE_0)
            {

                //timeFirst_End
                if (this.timeFirst_End.IsEmpty)
                {
                    this.SetMessage(this.timeFirst_End.ID, M_Message.MSG_REQUIRE, "前半終了時刻");
                }

                //timeLatter_Start
                if (this.timeLatter_Start.IsEmpty)
                {
                    this.SetMessage(this.timeLatter_Start.ID, M_Message.MSG_REQUIRE, "後半開始時刻");
                }
            }

            int timeBreak1Start = this.timeBreak1_Start.Value != null ? Utilities.CommonUtil.TimeToInt(this.timeBreak1_Start.Value) : -1;
            int timeBreak2Start = this.timeBreak2_Start.Value != null ? Utilities.CommonUtil.TimeToInt(this.timeBreak2_Start.Value) : -1;
            int timeBreak3Start = this.timeBreak3_Start.Value != null ? Utilities.CommonUtil.TimeToInt(this.timeBreak3_Start.Value) : -1;
            int timeBreak4Start = this.timeBreak4_Start.Value != null ? Utilities.CommonUtil.TimeToInt(this.timeBreak4_Start.Value) : -1;
            int timeBreak1End = this.timeBreak1_End.Value != null ? Utilities.CommonUtil.TimeToInt(this.timeBreak1_End.Value) : -1;
            int timeBreak2End = this.timeBreak2_End.Value != null ? Utilities.CommonUtil.TimeToInt(this.timeBreak2_End.Value) : -1;
            int timeBreak3End = this.timeBreak3_End.Value != null ? Utilities.CommonUtil.TimeToInt(this.timeBreak3_End.Value) : -1;
            int timeBreak4End = this.timeBreak4_End.Value != null ? Utilities.CommonUtil.TimeToInt(this.timeBreak4_End.Value) : -1;
            if (int.Parse(this.optBreakType.SelectedValue) == OPT_BREAK_TYPE_0)
            {
                //TimeBreak1
                if ((this.timeBreak1_Start.IsEmpty && (!this.timeBreak1_End.IsEmpty))
                    || ((!this.timeBreak1_Start.IsEmpty) && this.timeBreak1_End.IsEmpty)
                    )
                {
                    if (this.timeBreak1_Start.IsEmpty)
                    {
                        this.SetMessage(this.timeBreak1_Start.ID, M_Message.MSG_REQUIRE, "休憩時間1（開始）");
                    }
                    if (this.timeBreak1_End.IsEmpty)
                    {
                        this.SetMessage(this.timeBreak1_End.ID, M_Message.MSG_REQUIRE, "休憩時間1（終了）");
                    }
                }

                if ((!this.timeBreak1_Start.IsEmpty) && (!this.timeBreak1_End.IsEmpty))
                {
                    if (timeBreak1Start > timeBreak1End)
                    {
                        this.SetMessage(this.timeBreak1_Start.ID, M_Message.MSG_LESS_THAN, "休憩時間1（開始）", "休憩時間1（終了）");
                    }
                }

                //TimeBreak2
                if ((this.timeBreak2_Start.IsEmpty && (!this.timeBreak2_End.IsEmpty))
                    || ((!this.timeBreak2_Start.IsEmpty) && this.timeBreak2_End.IsEmpty)
                    )
                {
                    if (this.timeBreak2_Start.IsEmpty)
                    {
                        this.SetMessage(this.timeBreak2_Start.ID, M_Message.MSG_REQUIRE, "休憩時間2（開始）");
                    }
                    if (this.timeBreak2_End.IsEmpty)
                    {
                        this.SetMessage(this.timeBreak2_End.ID, M_Message.MSG_REQUIRE, "休憩時間2（終了）");
                    }
                }

                if ((!this.timeBreak2_Start.IsEmpty) && (!this.timeBreak2_End.IsEmpty))
                {
                    if (timeBreak2Start >= timeBreak2End)
                    {
                        this.SetMessage(this.timeBreak2_Start.ID, M_Message.MSG_LESS_THAN, "休憩時間2（開始）", "休憩時間2（終了）");
                    }
                }

                //TimeBreak3
                if ((this.timeBreak3_Start.IsEmpty && (!this.timeBreak3_End.IsEmpty))
                    || ((!this.timeBreak3_Start.IsEmpty) && this.timeBreak3_End.IsEmpty)
                    )
                {
                    if (this.timeBreak3_Start.IsEmpty)
                    {
                        this.SetMessage(this.timeBreak3_Start.ID, M_Message.MSG_REQUIRE, "休憩時間3（開始）");
                    }
                    if (this.timeBreak3_End.IsEmpty)
                    {
                        this.SetMessage(this.timeBreak3_End.ID, M_Message.MSG_REQUIRE, "休憩時間3（終了）");
                    }
                }

                if ((!this.timeBreak3_Start.IsEmpty) && (!this.timeBreak3_End.IsEmpty))
                {
                    if (timeBreak3Start >= timeBreak3End)
                    {
                        this.SetMessage(this.timeBreak3_Start.ID, M_Message.MSG_LESS_THAN, "休憩時間3（開始）", "休憩時間3（終了）");
                    }
                }

                //TimeBreak4
                if ((this.timeBreak4_Start.IsEmpty && (!this.timeBreak4_End.IsEmpty))
                    || ((!this.timeBreak4_Start.IsEmpty) && this.timeBreak4_End.IsEmpty)
                    )
                {
                    if (this.timeBreak4_Start.IsEmpty)
                    {
                        this.SetMessage(this.timeBreak4_Start.ID, M_Message.MSG_REQUIRE, "休憩時間4（開始）");
                    }
                    if (this.timeBreak4_End.IsEmpty)
                    {
                        this.SetMessage(this.timeBreak4_End.ID, M_Message.MSG_REQUIRE, "休憩時間4（終了）");
                    }
                }

                if ((!this.timeBreak4_Start.IsEmpty) && (!this.timeBreak4_End.IsEmpty))
                {
                    if (timeBreak4Start >= timeBreak4End)
                    {
                        this.SetMessage(this.timeBreak4_Start.ID, M_Message.MSG_LESS_THAN, "休憩時間4（開始）", "休憩時間4（終了）");
                    }
                }

                //Check duplicate
                if (timeBreak1Start != -1 && timeBreak1End != -1)
                {
                    if (timeBreak2Start != -1 && timeBreak2End != -1)
                    {
                        if (
                            !((timeBreak1Start < timeBreak1End) && (timeBreak1End <= timeBreak2Start))
                            || ((timeBreak2Start < timeBreak2End) && (timeBreak2End <= timeBreak1Start))
                            )
                        {
                            this.SetMessage(this.timeBreak2_Start.ID, M_Message.MSG_INVALID_GRID, "休憩時間2", 2);
                        }
                    }
                    if (timeBreak3Start != -1 && timeBreak3End != -1)
                    {
                        if (
                            !((timeBreak1Start < timeBreak1End) && (timeBreak1End <= timeBreak3Start))
                            || ((timeBreak3Start < timeBreak3End) && (timeBreak3End <= timeBreak1Start))
                            )
                        {
                            this.SetMessage(this.timeBreak3_Start.ID, M_Message.MSG_INVALID_GRID, "休憩時間3", 3);
                        }
                    }

                    if (timeBreak4Start != -1 && timeBreak4End != -1)
                    {
                        if (
                            !((timeBreak1Start < timeBreak1End) && (timeBreak1End <= timeBreak4Start))
                            || ((timeBreak4Start < timeBreak4End) && (timeBreak4End <= timeBreak1Start))
                            )
                        {
                            this.SetMessage(this.timeBreak4_Start.ID, M_Message.MSG_INVALID_GRID, "休憩時間4", 4);
                        }
                    }
                }

                if (timeBreak2Start != -1 && timeBreak2End != -1)
                {
                    if (timeBreak3Start != -1 && timeBreak3End != -1)
                    {
                        if (
                            !((timeBreak2Start < timeBreak2End) && (timeBreak2End <= timeBreak3Start))
                            || ((timeBreak3Start < timeBreak3End) && (timeBreak3End <= timeBreak2Start))
                            )
                        {
                            this.SetMessage(this.timeBreak3_Start.ID, M_Message.MSG_INVALID_GRID, "休憩時間3", 3);
                        }
                    }
                    if (timeBreak4Start != -1 && timeBreak4End != -1)
                    {
                        if (
                            !((timeBreak2Start < timeBreak2End) && (timeBreak2End <= timeBreak4Start))
                            || ((timeBreak4Start < timeBreak4End) && (timeBreak4End <= timeBreak2Start))
                            )
                        {
                            this.SetMessage(this.timeBreak4_Start.ID, M_Message.MSG_INVALID_GRID, "休憩時間4", 4);
                        }
                    }

                }
                if (timeBreak3Start != -1 && timeBreak3End != -1)
                {
                    if (timeBreak4Start != -1 && timeBreak4End != -1)
                    {
                        if (
                            !((timeBreak3Start < timeBreak3End) && (timeBreak3End <= timeBreak4Start))
                            || ((timeBreak4Start < timeBreak4End) && (timeBreak4End <= timeBreak3Start))
                            )
                        {
                            this.SetMessage(this.timeBreak4_Start.ID, M_Message.MSG_INVALID_GRID, "休憩時間4", 4);
                        }
                    }

                }

            }
            else if (int.Parse(this.optBreakType.SelectedValue) == OPT_BREAK_TYPE_1)
            {

            }
            else
            {
                //TimeBreak1
                if ((this.timeBreak1_Start.IsEmpty && (!this.timeBreak1_End.IsEmpty))
                    || ((!this.timeBreak1_Start.IsEmpty) && this.timeBreak1_End.IsEmpty)
                    )
                {
                    if (this.timeBreak1_Start.IsEmpty)
                    {
                        this.SetMessage(this.timeBreak1_Start.ID, M_Message.MSG_REQUIRE, "休憩時間");
                    }
                    if (this.timeBreak1_End.IsEmpty)
                    {
                        this.SetMessage(this.timeBreak1_End.ID, M_Message.MSG_REQUIRE, "毎");
                    }
                }
            }

            if (int.Parse(this.optBreakType.SelectedValue) == OPT_BREAK_TYPE_0)
            {
                StatusDisplayBreakType = OPT_BREAK_TYPE_0;
            }
            else if (int.Parse(this.optBreakType.SelectedValue) == OPT_BREAK_TYPE_1)
            {
                StatusDisplayBreakType = OPT_BREAK_TYPE_1;
            }
            else
            {
                StatusDisplayBreakType = OPT_BREAK_TYPE_2;
            }       

            //Check error
            return !base.HaveError;
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
        /// CheckValidateOverTime
        /// </summary>
        /// <param name="index"> index item</param>
        private bool CheckValidateOverTime(int index, ITimeTextBox timeOverTime_StartCurrent, ITimeTextBox timeOverTime_EndCurrent)
        {
            bool result = false;
            int overTime_StartCurrent = timeOverTime_StartCurrent.Value != null ? Utilities.CommonUtil.TimeToInt(timeOverTime_StartCurrent.Value) : -1;
            int overTime_EndCurrent = timeOverTime_EndCurrent.Value != null ? Utilities.CommonUtil.TimeToInt(timeOverTime_EndCurrent.Value) : -1;

            for (int i = index + 1; i < rptOverTime.Items.Count; i++)
            {
                RepeaterItem item = rptOverTime.Items[i];
                ITimeTextBox timeOverTime_Start = (ITimeTextBox)item.FindControl("timeOverTimeStart");
                ITimeTextBox timeOverTime_End = (ITimeTextBox)item.FindControl("timeOverTimeEnd");
                int overTime_Start = timeOverTime_Start.Value != null ? Utilities.CommonUtil.TimeToInt(timeOverTime_Start.Value) : -1;
                int overTime_End = timeOverTime_End.Value != null ? Utilities.CommonUtil.TimeToInt(timeOverTime_End.Value) : -1;

                if ((!timeOverTime_Start.IsEmpty) && (!timeOverTime_End.IsEmpty)
                    && overTime_StartCurrent != -1 && overTime_EndCurrent != -1
                    && overTime_Start != -1 && overTime_End != -1)
                {
                    if (!((overTime_StartCurrent < overTime_EndCurrent) && (overTime_EndCurrent <= overTime_Start))
                        || (((overTime_Start < overTime_End) && (overTime_End <= overTime_StartCurrent))
                        ))
                    {
                        return result;
                    }

                }
            }
            result = true;
            return result;
        }
        #endregion
    }
}