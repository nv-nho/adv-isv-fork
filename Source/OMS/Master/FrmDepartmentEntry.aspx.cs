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

namespace OMS.Master
{
    public partial class FrmDepartmentEntry : FrmBaseDetail
    {
        #region Cosntant
        private const int ADMIN_ID = 1;

        private const string URL_LIST = "~/Master/FrmDepartmentList.aspx";
        #endregion

        #region Property

        /// <summary>
        /// Get or set ProjectID
        /// </summary>
        public int DepartmentID
        {
            get { return (int)ViewState["DepartmentID"]; }
            set { ViewState["DepartmentID"] = value; }
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
            base.FormTitle = "部門登録";
            base.FormSubTitle = "Detail";

            this.txtDepartmentCD.MaxLength = M_Department.DEPARTMENT_CODE_SHOW_MAX_LENGTH;
            this.txtDepartmentName.MaxLength = M_Department.DEPARTMENT_NAME_MAX_LENGTH;
            this.txtDepartmentName2.MaxLength = M_Department.DEPARTMENT_NAME2_MAX_LENGTH;

            //Init Event
            LinkButton btnYes = (LinkButton)this.Master.FindControl("btnYes");
            btnYes.Click += new EventHandler(btnProcessData);

            LinkButton btnNo = (LinkButton)this.Master.FindControl("btnNo");
            btnNo.Click += new EventHandler(btnShowData);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Check authority of login user
            base.SetAuthority(FormId.Department);
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
                    }
                    else
                    {
                        //Get User ID
                        this.DepartmentID = int.Parse(PreviousPageViewState["ID"].ToString());
                        M_Department m_Department = this.GetDataDepartmentById(this.DepartmentID);

                        //Check m_Department
                        if (m_Department != null)
                        {
                            //Show data
                            this.ShowData(m_Department);

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
            M_Department m_Department = this.GetDataDepartmentById(this.DepartmentID);

            if (m_Department != null)
            {
                //Show data
                this.ShowData(m_Department);

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
            M_Department m_Department = this.GetDataDepartmentById(this.DepartmentID);

            if (m_Department != null)
            {
                //Show data
                this.ShowData(m_Department);

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
            this.txtDepartmentCD.Value = string.Empty;
            this.txtDepartmentName.Value = string.Empty;
            this.txtDepartmentName2.Value = string.Empty;

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
                M_Department m_Department = this.GetDataDepartmentById(this.DepartmentID);

                //Check M_WorkingSystem
                if (m_Department != null)
                {
                    //Set Mode
                    this.ProcessMode(Mode.View);

                    //Show data
                    this.ShowData(m_Department);
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
                        M_Department m_Department = this.GetDataDepartmentByCd(this.txtDepartmentCD.Value);

                        //Show data
                        this.ShowData(m_Department);

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
                        M_Department m_Department = this.GetDataDepartmentById(this.DepartmentID);

                        //Set Mode
                        this.ProcessMode(Mode.View);

                        //Show data
                        this.ShowData(m_Department);

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
            M_Department m_Department = this.GetDataDepartmentById(this.DepartmentID);

            if (m_Department != null)
            {
                //Show data
                this.ShowData(m_Department);

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
        public static string GetDepartment(string in1)
        {
            return EditDataUtil.JsonSerializer(new
            {
                txtDepartmentCD = EditDataUtil.ToFixCodeShow(EditDataUtil.ToFixCodeDB(in1, M_Department.DEPARTMENT_CODE_DB_MAX_LENGTH), M_Department.DEPARTMENT_CODE_SHOW_MAX_LENGTH)

                //txtDepartmentCD = in1
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
                    this.txtDepartmentCD.Value = string.Empty;
                    this.txtDepartmentCD.ReadOnly = false;
                    this.txtDepartmentName.ReadOnly = false;
                    this.txtDepartmentName2.ReadOnly = false;
                    break;

                case Mode.Update:
                    this.txtDepartmentCD.ReadOnly = true;
                    this.txtDepartmentName.ReadOnly = false;
                    this.txtDepartmentName2.ReadOnly = false;
                    break;

                default:
                    this.txtDepartmentCD.ReadOnly = true;
                    this.txtDepartmentName.ReadOnly = true;
                    this.txtDepartmentName2.ReadOnly = true;

                    if (this.DepartmentID == ADMIN_ID)
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
        /// <param name="workingSystemId"></param>
        /// <returns></returns>
        private M_Department GetDataDepartmentById(int departmentID)
        {
            using (DB db = new DB())
            {
                DepartmentService projectSer = new DepartmentService(db);
                return projectSer.GetDataDepartmentById(departmentID);
            }
        }

        /// <summary>
        /// Get Data Project Cd
        /// </summary>
        /// <param name="projectCd"></param>
        /// <returns></returns>
        private M_Department GetDataDepartmentByCd(string departmentCd)
        {
            using (DB db = new DB())
            {
                DepartmentService departmentSer = new DepartmentService(db);
                return departmentSer.GetByDepartmentCd(departmentCd);
            }
        }

        /// <summary>
        /// Show Data
        /// </summary>
        /// <param name="m_Project"></param>
        private void ShowData(M_Department m_Department)
        {
            if (m_Department != null)
            {
                M_Department m_DepartmentData = GetDataDepartmentById(m_Department.ID);
                this.txtDepartmentCD.Value = Utilities.EditDataUtil.ToFixCodeShow(m_Department.DepartmentCD, M_Department.DEPARTMENT_CODE_SHOW_MAX_LENGTH);
                this.txtDepartmentName.Value = m_Department.DepartmentName;
                this.txtDepartmentName2.Value = m_Department.DepartmentName2;

                ///Save UserID and UpdateDate
                this.DepartmentID = m_DepartmentData.ID;
                this.OldUpdateDate = m_Department.UpdateDate;
            }
        }

        /// <summary>
        /// Check input
        /// </summary>
        /// <returns>Valid:true, Invalid:false</returns>
        private bool CheckInput()
        {
            //txtWorkingSystemCD
            if (this.txtDepartmentCD.IsEmpty)
            {
                this.SetMessage(this.txtDepartmentCD.ID, M_Message.MSG_REQUIRE, "部門コード");
            }
            else
            {
                if (this.Mode == Mode.Insert || this.Mode == Mode.Copy)
                {

                    // Check Department by DepartmentCd 
                    if (this.GetDataDepartmentByCd(this.txtDepartmentCD.Value) != null)
                    {
                        this.SetMessage(this.txtDepartmentCD.ID, M_Message.MSG_EXIST_CODE, "部門コード");
                    }

                }
            }

            //txtDepartmentName
            if (this.txtDepartmentName.IsEmpty)
            {
                this.SetMessage(this.txtDepartmentName.ID, M_Message.MSG_REQUIRE, "部門名");
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
                M_Department m_Department = new M_Department();

                m_Department.DepartmentCD = this.txtDepartmentCD.Value;
                m_Department.DepartmentName = this.txtDepartmentName.Value;
                m_Department.DepartmentName2 = this.txtDepartmentName2.Value;

                m_Department.CreateUID = this.LoginInfo.User.ID;
                m_Department.UpdateUID = this.LoginInfo.User.ID;

                using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                {
                    DepartmentService proService = new DepartmentService(db);
                    proService.Insert(m_Department);
                    db.Commit();
                }
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains(Models.Constant.M_WORKINGSYSTEM_UN))
                {
                    this.SetMessage(this.txtDepartmentCD.ID, M_Message.MSG_EXIST_CODE, "部門コード");
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
                M_Department m_Department = this.GetDataDepartmentByCd(this.txtDepartmentCD.Value);

                if (m_Department != null)
                {
                    // Create model
                    m_Department.ID = this.DepartmentID;

                    m_Department.DepartmentCD = this.txtDepartmentCD.Value;
                    m_Department.DepartmentName = this.txtDepartmentName.Value;
                    m_Department.DepartmentName2 = this.txtDepartmentName2.Value;

                    m_Department.UpdateDate = this.OldUpdateDate;
                    m_Department.UpdateUID = this.LoginInfo.User.ID;

                    // Update                     
                    using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                    {
                        DepartmentService projectSer = new DepartmentService(db);

                        //Update User
                        if (m_Department.Status == DataStatus.Changed)
                        {
                            ret = projectSer.Update(m_Department);

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
                if (ex.Message.Contains(Models.Constant.M_DEPARTMENT_UN))
                {
                    this.SetMessage(this.txtDepartmentCD.ID, M_Message.MSG_EXIST_CODE, "部門コード");
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
                    DepartmentService projectSer = new DepartmentService(db);
                    ret = projectSer.DeleteDepartmentByID(this.DepartmentID);

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
                if (ex.Message.Contains(Models.Constant.M_USER_FK_DEPARTMENTID) || ex.Message.Contains(Models.Constant.T_EXPENCE_H_FK_DEPARTMENTID) || ex.Message.Contains(Models.Constant.M_PROJECT_FK_DEPARTMENTID))
                {
                    this.SetMessage(string.Empty, M_Message.MSG_EXIST_CANT_DELETE, "部門コード" + this.txtDepartmentCD.Value);
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