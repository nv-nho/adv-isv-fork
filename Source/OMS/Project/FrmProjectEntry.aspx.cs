using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OMS.Utilities;
using OMS.Models;
using OMS.DAC;
using System.Data.SqlClient;

namespace OMS.Project
{
    public partial class FrmProjectEntry : FrmBaseDetail
    {
        #region Cosntant
        private const int ADMIN_ID = 1;

        private const string URL_LIST = "~/Project/FrmProjectList.aspx";
        #endregion

        #region Property

        /// <summary>
        /// Get or set ProjectID
        /// </summary>
        public int ProjectID
        {
            get { return (int)ViewState["ProjectID"]; }
            set { ViewState["ProjectID"] = value; }
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
        /// Event Init
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //Set Title
            base.FormTitle = "プロジェクト登録";
            base.FormSubTitle = "Detail";

            this.txtProjectCD.MaxLength = M_Project.PROJECT_CODE_SHOW_MAX_LENGTH;
            this.txtProjectName.MaxLength = M_Project.PROJECT_NAME_MAX_LENGTH;
            this.txtWorkPlace.MaxLength = M_Project.PROJECT_WORKPLACE_MAX_LENGTH;
            this.txtMemo.MaxLength = M_Project.PORJECT_MEMO_LENGTH;
            this.txtOrderAmount.MaximumValue = M_Project.PROJECT_ORDER_AMOUNT_MAX_LENGTH;
            this.dtStartDate.Value = DateTime.Now;

            //Init Event
            LinkButton btnYes = (LinkButton)this.Master.FindControl("btnYes");
            btnYes.Click += new EventHandler(btnProcessData);

            LinkButton btnNo = (LinkButton)this.Master.FindControl("btnNo");
            btnNo.Click += new EventHandler(btnShowData);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Check authority of login user
            base.SetAuthority(FormId.Project);
            if (!this._authority.IsMasterView)
            {
                Response.Redirect("~/Menu/FrmMainMenu.aspx");
            }

            if (!this.IsPostBack)
            {
                this.InitData();

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
                        this.ProjectID = int.Parse(PreviousPageViewState["ID"].ToString());
                        M_Project m_Project = this.GetDataProjectById(this.ProjectID);

                        //Check m_Project
                        if (m_Project != null)
                        {
                            //Show data
                            this.ShowData(m_Project);

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
        /// Init Data
        /// </summary>
        private void InitData()
        {
            InitCombobox();

            this.ddlDepartment.Enabled = false;
            this.ddlUser.Enabled = false;
        }

        /// <summary>
        /// InitCombobox
        /// </summary>
        private void InitCombobox()
        {
            //Load Department combobox data
            this.LoadDepartmentComboboxData();

            //Load user combobox data
            this.LoadUserComboboxData(-1, false);
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
                departmentList = departmentService.GetDepartmentCbbData();
                ddlDepartment.Items.Clear();

                if (departmentList.Count > 0)
                {
                    departmentList.Insert(0, new DropDownModel("-1", "---"));
                    ddlDepartment.DataSource = departmentList;
                    ddlDepartment.DataValueField = "Value";
                    ddlDepartment.DataTextField = "DisplayName";
                    if (this.LoginInfo.User.DepartmentID >= 10)
                    {
                        ddlDepartment.SelectedValue = this.LoginInfo.User.DepartmentID.ToString();
                    }
                    ddlDepartment.DataBind();
                }
                else
                {
                    //msg Please input Department
                    ddlDepartment.DataSource = null;
                    ddlDepartment.DataBind();
                }
            }
        }

        /// <summary>
        /// Load user combobox data
        /// </summary>
        private void LoadUserComboboxData(int userID, bool isCopy)
        {
            using (DB db = new DB())
            {
                IList<DropDownModel> userList = new List<DropDownModel>();
                //UserService userService = new UserService(db);
                ProjectService prService = new ProjectService(db);

                DropDownModel emptOption = new DropDownModel();
                emptOption.DisplayName = "---";
                emptOption.Value = "-1";

                userList = prService.GetCbbUserDataByDepartmentIDAndUserID(-1, isCopy ? -1 : userID, true);

                userList.Insert(0, emptOption);
                ddlUser.Items.Clear();

                if (userList.Count > 0)
                {
                    ddlUser.DataSource = userList;
                    ddlUser.DataValueField = "Value";
                    ddlUser.DataTextField = "DisplayName";
                    ddlUser.SelectedValue = "-1";

                    foreach (var item in userList)
                    {
                        if (item.Value == userID.ToString())
                        {
                            ddlUser.SelectedValue = userID.ToString();
                        }
                    }

                    this.UserList = userList;
                }
                else
                {
                    ddlUser.DataSource = null;
                    this.UserList = null;
                }


                ddlUser.DataBind();
                LoadUserComboboxAttribute();
            }
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
                    }
                    index++;
                }

                if (this.UserList[this.ddlUser.SelectedIndex].Value != "-1" && this.UserList[this.ddlUser.SelectedIndex].Status != "0")
                {
                    ddlUser.CssClass = "form-control input-sm bg-danger";
                }
                else
                {
                    ddlUser.CssClass = "form-control input-sm";
                }

            }
        }

        /// <summary>
        /// Edit Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            M_Project m_Project = this.GetDataProjectById(this.ProjectID);

            if (m_Project != null)
            {
                //Show data
                this.ShowData(m_Project);

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
            M_Project m_Project = this.GetDataProjectById(this.ProjectID);

            if (m_Project != null)
            {
                //Show data
                this.ShowData(m_Project, true);

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
            this.txtProjectCD.Value = string.Empty;
            this.txtProjectName.Value = string.Empty;
            this.txtWorkPlace.Value = string.Empty;
            this.txtMemo.Value = string.Empty;
            this.dtStartDate.Value = DateTime.Now;
            this.dtEndDate.Value = (DateTime?)null;
            this.chkStatusFlag.Checked = true;
            this.LoadUserComboboxData(int.Parse(this.ddlUser.SelectedValue), true);
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
                M_Project m_Project = this.GetDataProjectById(this.ProjectID);

                //Check M_WorkingSystem
                if (m_Project != null)
                {
                    //Set Mode
                    this.ProcessMode(Mode.View);

                    //Show data
                    this.ShowData(m_Project);
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
                        M_Project m_Project = this.GetDataProjectByCd(this.txtProjectCD.Value);

                        //Show data
                        this.ShowData(m_Project);

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
                        M_Project m_Project = this.GetDataProjectById(this.ProjectID);

                        //Set Mode
                        this.ProcessMode(Mode.View);

                        //Show data
                        this.ShowData(m_Project);

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
            M_Project m_Project = this.GetDataProjectById(this.ProjectID);

            if (m_Project != null)
            {
                //Show data
                this.ShowData(m_Project);

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
        public static string GetProject(string in1)
        {
            return EditDataUtil.JsonSerializer(new
            {
                //txtProjectCD = EditDataUtil.ToFixCodeShow(EditDataUtil.ToFixCodeDB(in1, M_Project.PROJECT_CODE_DB_MAX_LENGTH), M_Project.PROJECT_CODE_SHOW_MAX_LENGTH)

                txtProjectCD = in1
            });
        }

        #endregion

        #region Method
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
                    this.txtProjectCD.Value = this.GetDefaultProjectCode("E007");
                    this.txtProjectCD.ReadOnly = false;
                    this.txtProjectName.ReadOnly = false;
                    this.txtWorkPlace.ReadOnly = false;
                    this.ddlDepartment.Enabled = true;
                    this.ddlUser.Enabled = true;
                    this.txtMemo.ReadOnly = false;
                    this.dtStartDate.ReadOnly = false;
                    this.dtEndDate.ReadOnly = false;
                    this.dtDeliveryDate.ReadOnly = false;
                    this.dtAcceptanceDate.ReadOnly = false;
                    this.chkAcceptanceFlag.Disabled = false;
                    this.txtOrderAmount.ReadOnly = false;
                    this.chkStatusFlag.Disabled = true;
                    this.chkStatusFlag.Checked = true;
                    break;

                case Mode.Update:
                    this.txtProjectCD.ReadOnly = false;
                    this.txtProjectName.ReadOnly = false;
                    this.txtWorkPlace.ReadOnly = false;
                    this.ddlDepartment.Enabled = true;
                    this.ddlUser.Enabled = true;
                    this.txtMemo.ReadOnly = false;
                    this.dtStartDate.ReadOnly = false;
                    this.dtEndDate.ReadOnly = false;
                    this.dtDeliveryDate.ReadOnly = false;
                    this.dtAcceptanceDate.ReadOnly = false;
                    this.chkAcceptanceFlag.Disabled = false;
                    this.txtOrderAmount.ReadOnly = false;
                    this.chkStatusFlag.Disabled = false;
                    break;

                default:
                    this.txtProjectCD.ReadOnly = true;
                    this.txtProjectName.ReadOnly = true;
                    this.txtWorkPlace.ReadOnly = true;
                    this.ddlDepartment.Enabled = false;
                    this.ddlUser.Enabled = false;
                    this.txtMemo.ReadOnly = true;
                    this.dtStartDate.ReadOnly = true;
                    this.dtEndDate.ReadOnly = true;
                    this.dtDeliveryDate.ReadOnly = true;
                    this.dtAcceptanceDate.ReadOnly = true;
                    this.chkAcceptanceFlag.Disabled = true;
                    this.txtOrderAmount.ReadOnly = true;
                    this.chkStatusFlag.Disabled = true;

                    if (this.ProjectID == ADMIN_ID)
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
        /// Get Data Project By Id
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        private M_Project GetDataProjectById(int projectID)
        {
            using (DB db = new DB())
            {
                ProjectService projectSer = new ProjectService(db);
                return projectSer.GetDataProjectById(projectID);
            }
        }

        /// <summary>
        /// Get Default Project Code
        /// </summary>
        /// <param name="configCD"></param>
        /// <returns></returns>
        private string GetDefaultProjectCode(string configCD)
        {
            try
            {
                using (DB db = new DB())
                {
                    ProjectService projectSer = new ProjectService(db);
                    return projectSer.GetDefaultProjectCode(configCD);
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Get Data Project Cd
        /// </summary>
        /// <param name="projectCd"></param>
        /// <returns></returns>
        private M_Project GetDataProjectByCd(string projectCd)
        {
            using (DB db = new DB())
            {
                ProjectService projectSer = new ProjectService(db);
                return projectSer.GetByProjectCd(projectCd);
            }
        }


        /// <summary>
        /// Show Data
        /// </summary>
        /// <param name="m_Project"></param>
        private void ShowData(M_Project m_Project, bool isCopy = false)
        {
            if (m_Project != null)
            {
                InitData();

                M_Project m_ProjectData = GetDataProjectById(m_Project.ID);
                this.txtProjectCD.Value = m_Project.ProjectCD;
                this.txtProjectName.Value = m_Project.ProjectName;
                this.txtWorkPlace.Value = m_Project.WorkPlace;
                this.txtMemo.Value = m_Project.Memo;

                this.ddlDepartment.SelectedValue = m_Project.DepartmentID.ToString();
                this.LoadUserComboboxData(m_Project.UserID, isCopy);
                this.dtStartDate.Value = m_Project.StartDate;
                this.dtEndDate.Value = m_Project.EndDate;
                this.dtDeliveryDate.Value = m_Project.DeliveryDate;
                this.dtAcceptanceDate.Value = m_Project.AcceptanceDate;
                this.chkAcceptanceFlag.Checked = m_Project.AcceptanceFlag == 1 ? true : false;
                this.txtOrderAmount.Value = m_Project.OrderAmount;
                this.chkStatusFlag.Checked = m_Project.StatusFlag == 0 ? true : false;
                ///Save UserID and UpdateDate
                this.ProjectID = m_ProjectData.ID;
                this.OldUpdateDate = m_Project.UpdateDate;
            }
        }

        /// <summary>
        /// Check input
        /// </summary>
        /// <returns>Valid:true, Invalid:false</returns>
        private bool CheckInput()
        {
            //txtWorkingSystemCD
            if (this.txtProjectCD.IsEmpty)
            {
                this.SetMessage(this.txtProjectCD.ID, M_Message.MSG_REQUIRE, "プロジェクトコード");
            }
            else
            {
                if (this.Mode == Mode.Insert || this.Mode == Mode.Copy || this.Mode == Mode.Update)
                {
                    if (this.Mode == Mode.Update)
                    {
                        M_Project m_project = null;
                        m_project = this.GetDataProjectByCd(this.txtProjectCD.Value);

                        // Check Project by projectCd
                        if (m_project != null)
                        {
                            if (m_project.ID != this.ProjectID)
                            {
                                this.SetMessage(this.txtProjectCD.ID, M_Message.MSG_EXIST_CODE, "プロジェクトコード");
                            }

                        }
                    }
                    else
                    {
                        // Check Project by projectCd
                        if (this.GetDataProjectByCd(this.txtProjectCD.Value) != null)
                        {
                            this.SetMessage(this.txtProjectCD.ID, M_Message.MSG_EXIST_CODE, "プロジェクトコード");
                        }
                    }
                }
            }

            //txtProjectName
            if (this.txtProjectName.IsEmpty)
            {
                this.SetMessage(this.txtProjectName.ID, M_Message.MSG_REQUIRE, "プロジェクト名");
            }

            //department
            if (this.ddlDepartment.SelectedValue.Equals("-1"))
            {
                this.SetMessage(this.ddlDepartment.ID, M_Message.MSG_REQUIRE, "部門");
            }

            //user
            if (this.ddlUser.SelectedValue.Equals("-1"))
            {
                this.SetMessage(this.ddlUser.ID, M_Message.MSG_REQUIRE, "管理者");
            }
            //dtStartDate
            if (this.dtStartDate.IsEmpty)
            {
                this.SetMessage(this.dtStartDate.ID, M_Message.MSG_REQUIRE, "開始日");
            }

            if (!this.dtStartDate.IsEmpty && !this.dtEndDate.IsEmpty)
            {
                if (this.dtStartDate.Value > this.dtEndDate.Value)
                {
                    this.SetMessage(this.dtStartDate.ID, M_Message.MSG_LESS_THAN_EQUAL, "開始日", "終了日");
                }
            }

            // Check chkAcceptanceFlag == True
            if (this.chkAcceptanceFlag.Checked == true)
            {
                //dtDeliveryDate
                if (this.dtAcceptanceDate.IsEmpty)
                {
                    this.SetMessage(this.dtAcceptanceDate.ID, M_Message.MSG_REQUIRE, "検収日（検収完了）");
                }
            }

            //txtOrderAmount
            if (this.txtOrderAmount.IsEmpty)
            {
                this.SetMessage(this.txtOrderAmount.ID, M_Message.MSG_REQUIRE, "受注金額");
            }

            //Check error
            return !base.HaveError;
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
                M_Project m_Project = new M_Project();

                m_Project.ProjectCD = this.txtProjectCD.Value;
                m_Project.ProjectName = this.txtProjectName.Value;
                m_Project.WorkPlace = this.txtWorkPlace.Value;
                m_Project.Memo = this.txtMemo.Value;

                m_Project.StartDate = this.dtStartDate.Value.GetValueOrDefault();
                m_Project.EndDate = this.dtEndDate.Value;

                m_Project.DeliveryDate = this.dtDeliveryDate.Value;
                m_Project.AcceptanceDate = this.dtAcceptanceDate.Value;
                m_Project.AcceptanceFlag = Convert.ToInt16(this.chkAcceptanceFlag.Checked == true ? 1 : 0);
                m_Project.OrderAmount = decimal.Parse(this.txtOrderAmount.Value.ToString());
                m_Project.UserID = int.Parse(this.ddlUser.SelectedItem.Value);
                m_Project.DepartmentID = int.Parse(this.ddlDepartment.SelectedItem.Value);

                m_Project.StatusFlag = Convert.ToInt16(this.chkStatusFlag.Checked == true ? 0 : 1);

                m_Project.CreateUID = this.LoginInfo.User.ID;
                m_Project.UpdateUID = this.LoginInfo.User.ID;

                using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                {
                    ProjectService proService = new ProjectService(db);
                    proService.Insert(m_Project);
                    db.Commit();
                }
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains(Models.Constant.M_WORKINGSYSTEM_UN))
                {
                    this.SetMessage(this.txtProjectCD.ID, M_Message.MSG_EXIST_CODE, "プロジェクトコード");
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
                M_Project m_Project = this.GetDataProjectById(this.ProjectID);

                if (m_Project != null)
                {
                    // Create model
                    m_Project.ID = this.ProjectID;

                    m_Project.ProjectCD = this.txtProjectCD.Value;
                    m_Project.ProjectName = this.txtProjectName.Value;
                    m_Project.WorkPlace = this.txtWorkPlace.Value;
                    m_Project.Memo = this.txtMemo.Value;

                    m_Project.StartDate = this.dtStartDate.Value;
                    m_Project.EndDate = this.dtEndDate.Value;
                    m_Project.StatusFlag = Convert.ToInt16(this.chkStatusFlag.Checked == true ? 0 : 1);

                    m_Project.UserID = int.Parse(this.ddlUser.SelectedItem.Value);
                    m_Project.DepartmentID = int.Parse(this.ddlDepartment.SelectedItem.Value);
                    m_Project.DeliveryDate = this.dtDeliveryDate.Value;
                    m_Project.AcceptanceDate = this.dtAcceptanceDate.Value;
                    m_Project.AcceptanceFlag = Convert.ToInt16(this.chkAcceptanceFlag.Checked == true ? 1 : 0);
                    m_Project.OrderAmount = decimal.Parse(this.txtOrderAmount.Value.ToString());

                    m_Project.UpdateDate = this.OldUpdateDate;
                    m_Project.UpdateUID = this.LoginInfo.User.ID;

                    // Update
                    using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                    {
                        ProjectService projectSer = new ProjectService(db);

                        //Update User
                        if (m_Project.Status == DataStatus.Changed)
                        {
                            ret = projectSer.Update(m_Project);

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
                if (ex.Message.Contains(Models.Constant.M_PROJECT_UN))
                {
                    this.SetMessage(this.txtProjectCD.ID, M_Message.MSG_EXIST_CODE, "プロジェクトコード");
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
                int ret = 0;
                using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                {
                    ProjectService projectSer = new ProjectService(db);
                    ret = projectSer.DeleteProjectByID(this.ProjectID);

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
                if (ex.Message.Contains(Models.Constant.T_WORK_D_FK_PROJECT) || ex.Message.Contains(Models.Constant.T_EXPENCE_H_FK_PROJECTID))
                {
                    this.SetMessage(string.Empty, M_Message.MSG_EXIST_CANT_DELETE, "プロジェクトコード" + this.txtProjectCD.Value);
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

        #endregion
    }
}