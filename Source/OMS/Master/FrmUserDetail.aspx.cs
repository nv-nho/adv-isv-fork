using System;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

using OMS.DAC;
using OMS.Models;
using OMS.Utilities;
using System.Text.RegularExpressions;
using OMS.Controls;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;

namespace OMS.Master
{
    /// <summary>
    /// User Detail
    /// ISV-TRUC    
    /// </summary>
    public partial class FrmUserDetail : FrmBaseDetail
    {
        #region Constants
        private const string URL_LIST = "~/Master/FrmUserList.aspx";
        #endregion

        #region Property

        /// <summary>
        /// Get or set UserID
        /// </summary>
        public int UserID
        {
            get { return (int)ViewState["UserID"]; }
            set { ViewState["UserID"] = value; }
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
        /// DefaultMaxUser
        /// </summary>
        public int DefaultMaxActiveUser
        {
            get { return (int)ViewState["DefaultMaxActiveUser"]; }
            set { ViewState["DefaultMaxActiveUser"] = value; }
        }

        /// <summary>
        /// LatestUpdateDate
        /// </summary>
        public DateTime LatestUpdateDate
        {
            get { return (DateTime)ViewState["LatestUpdateDate"]; }
            set { ViewState["LatestUpdateDate"] = value; }
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
            base.FormTitle = "社員登録";
            base.FormSubTitle = "Detail";

            //Init Max Length
            this.txtGroupCD.MaxLength = M_GroupUser_H.GROUP_CODE_SHOW_MAX_LENGTH;
            this.txtGroupNm.MaxLength = M_GroupUser_H.GROUP_NAME_MAX_LENGTH;
            this.txtLoginID.MaxLength = M_User.LOGIN_ID_MAX_LENGTH;
            this.txtPassword.MaxLength = M_User.PASSWORD_MAX_LENGTH;
            this.txtUserCode.MaxLength = M_User.MAX_USER_CODE_SHOW;
            this.txtUserName1.MaxLength = M_User.USER_NAME_1_MAX_LENGTH;
            //this.txtUserName2.MaxLength = M_User.USER_NAME_2_MAX_LENGTH;
            this.txtPosition1.MaxLength = M_User.POSITION_1_MAX_LENGTH;
            // this.txtPosition2.MaxLength = M_User.POSITION_2_MAX_LENGTH;
            this.txtDepartmentCD.MaxLength = M_Department.DEPARTMENT_CODE_SHOW_MAX_LENGTH;
            this.txtDepartmentNm.MaxLength = M_Department.DEPARTMENT_NAME_MAX_LENGTH;
            this.txtEmailAddress.MaxLength = M_User.MAIL_ADDRESS_MAX_LENGTH;

            //Init Event
            LinkButton btnYes = (LinkButton)this.Master.FindControl("btnYes");
            btnYes.Click += new EventHandler(btnProcessData);
        }

        /// <summary>
        /// Page Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            base.SetAuthority(FormId.User);
            if (!base._authority.IsMasterView)
            {
                Response.Redirect("~/Menu/FrmMasterMenu.aspx");
            }

            //Get Default Data
            this.GetDefaultValue();

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

                        //Init paid vacation list
                        this.InitPaidVacationList();
                    }
                    else
                    {
                        //Get User ID
                        this.UserID = int.Parse(PreviousPageViewState["ID"].ToString());
                        M_User user = this.GetUser(this.UserID, new DB());

                        //Check user
                        if (user != null)
                        {
                            //Show data
                            this.ShowData(user);

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

                    //Init paid vacation list
                    this.InitPaidVacationList();
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
            //Get User
            M_User user = this.GetUser(this.UserID, new DB());

            //Check user
            if (user != null)
            {
                //Show data
                this.ShowData(user);

                //Clear loginID and password
                this.txtLoginID.Value = string.Empty;
                this.txtPassword.Attributes.Add("value", string.Empty);

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
            //Get User
            M_User user = this.GetUser(this.UserID, new DB());

            //Check user
            if (user != null)
            {
                //Show data
                this.ShowData(user);

                //Set Mode
                this.ProcessMode(Mode.Update);
            }
            else
            {
                Server.Transfer(URL_LIST);
            }
        }

        /// <summary>
        /// Event Insert Submit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnInsert_Click(object sender, EventArgs e)
        {
            this.ResetGroupName();
            this.ResetDepartmentName();

            var password = this.GetPassword(this.txtPassword);
            if (!string.IsNullOrEmpty(password))
            {
                password = Security.Instance.Encrypt(password);
            }
            this.txtPassword.Value = password;
            this.txtPassword.Attributes.Add("value", this.txtPassword.Value);

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
            this.ResetGroupName();
            this.ResetDepartmentName();
            var password = this.GetPassword(this.txtPassword);
            if (!string.IsNullOrEmpty(password))
            {
                password = Security.Instance.Encrypt(password);
            }
            this.txtPassword.Value = password;
            this.txtPassword.Attributes.Add("value", this.txtPassword.Value);

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
            this.txtUserCode.Value = string.Empty;
            this.txtUserName1.Value = string.Empty;
            // this.txtUserName2.Value = string.Empty;
            this.txtPosition1.Value = string.Empty;
            // this.txtPosition2.Value = string.Empty;
            this.txtLoginID.Value = string.Empty;
            this.txtPassword.Value = string.Empty;
            this.txtPassword.Attributes.Add("value", string.Empty);
            this.txtPassword.Value = string.Empty;

            this.txtDepartmentCD.Value = string.Empty;
            this.txtDepartmentNm.Value = string.Empty;
            this.txtGroupCD.Value = string.Empty;
            this.txtGroupNm.Value = string.Empty;
            this.txtEmailAddress.Attributes.Add("value", string.Empty);
            this.txtEmailAddress.Value=string.Empty;
            this.chkStatusFlag.Checked = true;

            //Init Paid Vacation list
            this.InitPaidVacationList();

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
            //Get User
            M_User user = this.GetUser(this.UserID, new DB());

            //Check user
            if (user != null)
            {
                //Show data
                this.ShowData(user);

                //Set Mode
                this.ProcessMode(Mode.View);
            }
            else
            {
                Server.Transfer(URL_LIST);
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
                        M_User user = this.GetUser(this.txtUserCode.Value, new DB());

                        //Show data
                        this.ShowData(user);

                        //Set Mode
                        this.ProcessMode(Mode.View);

                        //Set Success
                        this.Success = true;
                    }
                    break;

                default:

                    //Update Data
                    if (this.UpdateData())
                    {
                        M_User user = this.GetUser(this.UserID, new DB());

                        //Show data
                        this.ShowData(user);

                        //Set Mode
                        this.ProcessMode(Mode.View);

                        //Set Success
                        this.Success = true;
                    }

                    break;

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
                listDetail.Add(new T_PaidVacation());
            }

            //Process list view
            this.rptList.DataSource = listDetail;
            this.rptList.DataBind();
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
                listDetail.Add(new T_PaidVacation());
            }

            //Process list view
            this.rptList.DataSource = listDetail;
            this.rptList.DataBind();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get Default Value
        /// </summary>
        private void GetDefaultValue()
        {
            using (DB db = new DB())
            {
                Config_HService configHSer = new Config_HService(db);
                this.DefaultMaxActiveUser = int.Parse(configHSer.GetDefaultValueDrop(M_Config_H.CONFIG_CD_MAX_ACTIVE_USER));
            }
        }

        /// <summary>
        /// Process Mode
        /// </summary>
        /// <param name="mode">Mode</param>
        private void ProcessMode(Mode mode)
        {
            bool enable;

            //Set Model
            this.Mode = mode;

            //Check model
            switch (mode)
            {
                case Mode.Insert:
                case Mode.Copy:
                case Mode.Update:

                    if (this.Mode == Mode.Update)
                    {
                        this.txtUserCode.ReadOnly = false;
                        this.chkStatusFlag.Disabled = false;
                    }
                    else
                    {
                        this.txtUserCode.Value = string.Empty;
                        this.txtUserCode.ReadOnly = false;
                        this.chkStatusFlag.Disabled = true;
                        this.chkStatusFlag.Checked = true;
                    }

                    enable = false;

                    break;

                default:

                    this.txtUserCode.ReadOnly = true;
                    this.chkStatusFlag.Disabled = true;

                    base.DisabledLink(this.btnEdit, !base._authority.IsMasterEdit);
                    base.DisabledLink(this.btnCopy, !base._authority.IsMasterCopy);
                    //---------------Add 2014/12/29 ISV-HUNG--------------------//
                    base.DisabledLink(this.btnNew, !base._authority.IsMasterNew);
                    //---------------Add 2014/12/29 ISV-HUNG--------------------//
                    enable = true;
                    break;
            }

            //Lock control
            this.txtUserName1.ReadOnly = enable;     //
            //  this.txtUserName2.ReadOnly = enable;     //
            this.txtPosition1.ReadOnly = enable;     //
            // this.txtPosition2.ReadOnly = enable;     //
            this.txtLoginID.ReadOnly = enable;       //
            this.txtPassword.ReadOnly = enable;      //
            this.btnSearchGroup.Disabled = enable; //
            this.txtGroupCD.ReadOnly = enable;       //
            this.txtDepartmentCD.ReadOnly = enable;       //
            this.btnSearchDepartment.Disabled = enable;
            this.txtEmailAddress.ReadOnly = enable;
        }

        /// <summary>
        /// Show data on form
        /// </summary>
        /// <param name="user">User</param>
        private void ShowData(M_User user)
        {
            M_GroupUser_H grp = null;

            M_Department dp = null;
            //Get data
            using (DB db = new DB())
            {
                GroupUserService groupSer = new GroupUserService(db);

                DepartmentService departmentSer = new DepartmentService(db);

                //Get Group User
                grp = groupSer.GetByGroupID(user.GroupID);

                dp = departmentSer.GetDataDepartmentById(user.DepartmentID);
            }

            //Show data
            if (user != null)
            {

                this.txtUserCode.Value = Utilities.EditDataUtil.ToFixCodeShow(user.UserCD, M_User.MAX_USER_CODE_SHOW);
                this.txtUserName1.Value = user.UserName1;
                // this.txtUserName2.Value = user.UserName2;
                this.txtPosition1.Value = user.Position1;
                //  this.txtPosition2.Value = user.Position2;
                // this.txtGroupCD.Value = grp != null ? user.depa : string.Empty;
                this.txtGroupCD.Value = grp != null ? Utilities.EditDataUtil.ToFixCodeShow(grp.GroupCD, M_GroupUser_H.GROUP_CODE_SHOW_MAX_LENGTH) : string.Empty;
                this.txtGroupNm.Value = grp.GroupName;
                this.txtDepartmentCD.Value = dp != null ? Utilities.EditDataUtil.ToFixCodeShow(dp.DepartmentCD, M_Department.DEPARTMENT_CODE_SHOW_MAX_LENGTH) : string.Empty;
                this.txtDepartmentNm.Value = dp.DepartmentName;
                this.txtLoginID.Value = user.LoginID;

                this.txtPassword.Value = Security.Instance.Encrypt(user.Password);
                this.txtPassword.Attributes.Add("value", this.txtPassword.Value);
                this.txtEmailAddress.Value = user.MailAddress;
                this.chkStatusFlag.Checked = user.StatusFlag == 0 ? true : false;

                //Save UserID and UpdateDate
                this.UserID = user.ID;
                this.OldUpdateDate = user.UpdateDate;

                this.ShowPaidVacationData(user.ID);
            }
        }

        /// <summary>
        /// Get User BY User ID
        /// </summary>
        /// <param name="userID">UserID</param>
        /// <returns>User</returns>
        private M_User GetUser(int userID, DB db)
        {
            UserService userSer = new UserService(db);

            //Get User
            return userSer.GetByID(userID);
        }

        /// <summary>
        /// Get User By User Code
        /// </summary>
        /// <param name="userCD">UserCD</param>
        /// <returns>User</returns>
        private M_User GetUser(string userCD, DB db, bool includeDelete = true)
        {
            UserService userSer = new UserService(db);

            //Get User
            return userSer.GetByUserCD(userCD, includeDelete);

        }

        /// <summary>
        /// Get User By User Code and Login ID
        /// </summary>
        /// <param name="userCD"></param>
        /// <param name="loginID"></param>
        /// <returns></returns>
        private M_User GetUser(string userCD, string loginID, DB db)
        {
            UserService userSer = new UserService(db);

            //Get User
            return userSer.GetByKey(userCD, loginID);
        }

        /// <summary>
        /// Get User By User ID and Login ID
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="loginID"></param>
        /// <returns></returns>
        private M_User GetUser(int userID, string loginID, DB db)
        {
            UserService userSer = new UserService(db);

            //Get User
            return userSer.GetByKey(userID, loginID);
        }

        /// <summary>
        /// Get User By LoginID
        /// </summary>
        /// <param name="loginID"></param>
        /// <returns></returns>
        private M_User GetUserByLoginID(string loginID, DB db)
        {
            UserService userSer = new UserService(db);

            //Get User
            return userSer.GetByLoginID(loginID);
        }

        /// <summary>
        /// Get Count Active User
        /// </summary>
        /// <returns></returns>
        private int GetCountActiveUser()
        {
            using (DB db = new DB())
            {
                UserService userSer = new UserService(db);

                //Get Count Active User
                return userSer.GetCountActiveUser();
            }
        }

        /// <summary>
        /// Get group by group code
        /// </summary>
        /// <param name="groupCD"></param>
        /// <returns></returns>
        private M_GroupUser_H GetGroup(string groupCD)
        {
            using (DB db = new DB())
            {
                GroupUserService grpSer = new GroupUserService(db);

                //Get Group User
                return grpSer.GetByGroupCD(groupCD);
            }
        }

        /// <summary>
        /// Get Group By Group ID
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        private M_GroupUser_H GetGroup(int groupID)
        {
            using (DB db = new DB())
            {
                GroupUserService grpSer = new GroupUserService(db);

                //Get Group User
                return grpSer.GetByGroupID(groupID);
            }
        }

        /// <summary>
        /// Get group by group code
        /// </summary>
        /// <param name="groupCD"></param>
        /// <returns></returns>
        private M_Department GetDepartment(string departmentCD)
        {
            using (DB db = new DB())
            {
                DepartmentService departmentSer = new DepartmentService(db);

                //Get Group User
                return departmentSer.GetByDepartmentCd(departmentCD);
            }
        }

        /// <summary>
        /// Get Group By Group ID
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        private M_Department GetDepartment(int departmentID)
        {
            using (DB db = new DB())
            {
                DepartmentService departmentSer = new DepartmentService(db);

                //Get Group User
                return departmentSer.GetDataDepartmentById(departmentID);
            }
        }

        /// <summary>
        /// Check input
        /// </summary>
        /// <returns>Valid:true, Invalid:false</returns>
        private bool CheckInput()
        {
            using (DB db = new DB())
            {
                //UserCD
                if (this.txtUserCode.IsEmpty)
                {
                    this.SetMessage(this.txtUserCode.ID, M_Message.MSG_REQUIRE, "社員コード");
                }
                else
                {
                    if (this.Mode == Mode.Insert || this.Mode == Mode.Copy || this.Mode == Mode.Update)
                    {
                        if (this.txtUserCode.Value == Constant.M_USER_DEFAULT_CODE)
                        {
                            //Diff '0000'
                            this.SetMessage(this.txtUserCode.ID, M_Message.MSG_MUST_BE_DIFFERENT, "社員コード", Constant.M_USER_DEFAULT_CODE);
                        }
                        else
                        {
                            if (this.Mode == Mode.Update)
                            {
                                M_User m_User = null;
                                m_User = this.GetUser(this.txtUserCode.Value, db);

                                // Check user by userCD 
                                if (m_User != null)
                                {
                                    if (m_User.ID != this.UserID)
                                    {
                                        this.SetMessage(this.txtUserCode.ID, M_Message.MSG_EXIST_CODE, "社員コード");
                                    }

                                }
                            }
                            else
                            {
                                if (this.GetUser(this.txtUserCode.Value, db) != null)
                                {
                                    this.SetMessage(this.txtUserCode.ID, M_Message.MSG_EXIST_CODE, "社員コード");
                                }
                            }

                        }
                    }
                }

                //UserName1
                if (this.txtUserName1.IsEmpty)
                {
                    this.SetMessage(this.txtUserName1.ID, M_Message.MSG_REQUIRE, "社員名");
                }

                //UserName2
                //if (this.txtUserName2.IsEmpty)
                //{
                //    this.SetMessage(this.txtUserName2.ID, M_Message.MSG_REQUIRE, "User Name 2");
                //}

                //GroupCD
                if (this.txtGroupCD.IsEmpty)
                {
                    this.SetMessage(this.txtGroupCD.ID, M_Message.MSG_REQUIRE, "権限グループコード");
                }
                else
                {
                    //check group by groupCD
                    if (this.GetGroup(this.txtGroupCD.Value) == null)
                    {
                        this.SetMessage(this.txtGroupCD.ID, M_Message.MSG_NOT_EXIST_CODE, "権限グループコード");
                    }
                }

                //GroupCD
                if (this.txtDepartmentCD.IsEmpty)
                {
                    this.SetMessage(this.txtDepartmentCD.ID, M_Message.MSG_REQUIRE, "部門コード");
                }
                else
                {
                    //check group by groupCD
                    if (this.GetDepartment(this.txtDepartmentCD.Value) == null)
                    {
                        this.SetMessage(this.txtDepartmentCD.ID, M_Message.MSG_NOT_EXIST_CODE, "部門コード");
                    }
                }

                //LoginID
                if (this.txtLoginID.IsEmpty)
                {
                    this.SetMessage(this.txtLoginID.ID, M_Message.MSG_REQUIRE, "ログインID");
                }
                else
                {
                    if (this.Mode == Mode.Insert || this.Mode == Mode.Copy)
                    {
                        //check user by LoginID 
                        if (this.GetUserByLoginID(this.txtLoginID.Value, db) != null)
                        {
                            this.SetMessage(this.txtLoginID.ID, M_Message.MSG_EXIST_CODE, "ログインID");
                        }
                    }
                    else if (this.Mode == Mode.Update)
                    {

                        M_User m_User = null;
                        m_User = this.GetUser(this.txtUserCode.Value, db);

                        if (m_User != null)
                        {
                            if (this.UserID == m_User.ID)
                            {
                                //Check userCd and LoginId exist
                                if (this.GetUser(this.txtUserCode.Value, this.txtLoginID.Value, db) == null)
                                {
                                    //UserCd and LoginId not exist, check exist loginID
                                    if (this.GetUserByLoginID(this.txtLoginID.Value, db) != null)
                                    {
                                        this.SetMessage(this.txtLoginID.ID, M_Message.MSG_EXIST_CODE, "ログインID");
                                    }
                                }
                            }
                            else
                            {
                                //Check userCd and LoginId exist
                                if (this.GetUser(this.UserID, this.txtLoginID.Value, db) == null)
                                {
                                    //check user by LoginID 
                                    if (this.GetUserByLoginID(this.txtLoginID.Value, db) != null)
                                    {
                                        this.SetMessage(this.txtLoginID.ID, M_Message.MSG_EXIST_CODE, "ログインID");
                                    }
                                }

                            }
                        }
                        else
                        {
                            //Check userID and LoginId exist
                            if (this.GetUser(this.UserID, this.txtLoginID.Value, db) == null)
                            {
                                //UserID and LoginId not exist, check exist loginID
                                if (this.GetUserByLoginID(this.txtLoginID.Value, db) != null)
                                {
                                    this.SetMessage(this.txtLoginID.ID, M_Message.MSG_EXIST_CODE, "ログインID");
                                }
                            }
                        }

                        //This data has been already updated by another user
                        T_PaidVacationService dbSer = new T_PaidVacationService(db);
                        T_PaidVacation LatestData = dbSer.GetLatestData(this.UserID);
                        if (LatestData != null)
                        {
                            if (this.LatestUpdateDate < LatestData.UpdateDate)
                            {
                                this.SetMessage(string.Empty, M_Message.MSG_DATA_CHANGED);
                            }
                        }
                    }
                }
                var password = this.GetPassword(this.txtPassword);
                //Password
                if (this.txtPassword.IsEmpty)
                {
                    this.SetMessage(this.txtPassword.ID, M_Message.MSG_REQUIRE, "パスワード");
                }
                else if (password.Length < 8 || Regex.Matches(password, "[a-zA-Z]").Count == 0 || Regex.Matches(password, "[0-9]").Count == 0)
                {
                    this.SetMessage(this.txtPassword.ID, M_Message.MSG_PASSWORD_RULE);
                }

                //Email
                if (!this.txtEmailAddress.IsEmpty)
                {
                    //EmailAddress
                    if (!CheckDataUtil.IsEmail(this.txtEmailAddress.Text))
                    {
                        this.SetMessage(this.txtEmailAddress.ID, M_Message.MSG_INCORRECT_FORMAT, "Email");
                    }
                }
                else
                {
                    this.SetMessage(this.txtEmailAddress.ID, M_Message.MSG_REQUIRE, "EMail");
                }

                //Condition input is correct
                //Check max active user
                if (!base.HaveError)
                {
                    //Insert New User
                    if (this.Mode == Mode.Copy || this.Mode == Mode.Insert)
                    {
                        if (this.GetCountActiveUser() >= this.DefaultMaxActiveUser)
                        {
                            this.SetMessage(this.txtUserCode.ID, M_Message.MSG_LESS_THAN_EQUAL, "All user", this.DefaultMaxActiveUser);
                        }
                    }

                    //Update Status Of User Deleted->Not Delete
                    //this.chkStatusFlag.Checked = true: On-> Status = 0, this.chkStatusFlag.Checked = false: Off-> Status = 1
                    if (this.Mode == Mode.Update)
                    {
                        M_User u = this.GetUser(this.UserID, db);
                        if (u != null)
                        {
                            u.StatusFlag = short.Parse(this.chkStatusFlag.Checked ? ((short)DeleteFlag.NotDelete).ToString() : ((short)DeleteFlag.Deleted).ToString());
                            if (u.Status == DataStatus.Changed && u.StatusFlag == (short)DeleteFlag.NotDelete)//statusFlag changed : delete ~~> not delete
                            {
                                if (this.GetCountActiveUser() >= this.DefaultMaxActiveUser)
                                {
                                    this.SetMessage(this.txtUserName1.ID, M_Message.MSG_LESS_THAN_EQUAL, "All user", this.DefaultMaxActiveUser);
                                }
                            }
                        }
                    }

                }
            }

            List<int> lstDup = new List<int>();

            bool hasData = false;

            foreach (RepeaterItem item in this.rptList.Items)
            {
                if (this.IsEmptyRow(item))
                {
                    continue;
                }

                hasData = true;
                int rowIndex = item.ItemIndex + 1;

                //Year
                ICodeTextBox txtYear = (ICodeTextBox)item.FindControl("txtYear");
                //VacationDay
                INumberTextBox txtVacationDay = (INumberTextBox)item.FindControl("txtVacationDay");

                if (txtYear != null)
                {
                    //対象年
                    HtmlGenericControl divYear = (HtmlGenericControl)item.FindControl("divYear");
                    divYear.Attributes.Remove("class");
                    string errorId = txtYear.ID + "_" + item.ItemIndex.ToString();

                    //有給休暇日数
                    HtmlGenericControl divVacationDay = (HtmlGenericControl)item.FindControl("divVacationDay");
                    divVacationDay.Attributes.Remove("class");
                    string errorIdVacationDay = txtVacationDay.ID + "_" + item.ItemIndex.ToString();

                    if (string.IsNullOrEmpty(txtYear.Value))
                    {
                        base.SetMessage(errorId, M_Message.MSG_REQUIRE_GRID, "対象年", rowIndex);
                        this.AddErrorForListItem(divYear, errorId);
                    }
                    else if (decimal.Parse(txtYear.Value) <= 0 && txtVacationDay.Value > 0)
                    {
                        base.SetMessage(errorId, M_Message.MSG_REQUIRE_GRID, "対象年", rowIndex);
                        this.AddErrorForListItem(divYear, errorId);
                    }
                    else if (txtVacationDay.Value == null)
                    {
                        base.SetMessage(errorIdVacationDay, M_Message.MSG_REQUIRE_GRID, "有給休暇日数", rowIndex);
                        this.AddErrorForListItem(divVacationDay, errorIdVacationDay);
                    }
                    else
                    {
                        if (lstDup.Contains(int.Parse(txtYear.Value)))
                        {
                            base.SetMessage(errorId, M_Message.MSG_DUPLICATE_GRID, "対象年", rowIndex);
                            this.AddErrorForListItem(divYear, errorId);
                        }
                        else
                        {
                            lstDup.Add(int.Parse(txtYear.Value));
                        }
                        txtYear.Value = int.Parse(txtYear.Value).ToString();
                    }
                }
            }

            if (!hasData)
            {
                ICodeTextBox txtYear = (ICodeTextBox)this.rptList.Items[0].FindControl("txtYear");
                HtmlGenericControl divValue = (HtmlGenericControl)this.rptList.Items[0].FindControl("divYear");
                string errorId = txtYear.ID + "_" + this.rptList.Items[0].ItemIndex.ToString();
                base.SetMessage(errorId, M_Message.MSG_REQUIRE_GRID, "対象年", 1);
                this.AddErrorForListItem(divValue, errorId);
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
                M_User user = new M_User();
                user.UserCD = this.txtUserCode.Value;
                user.UserName1 = this.txtUserName1.Value;
                user.UserName2 = string.Empty;
                user.Position1 = this.txtPosition1.Value;
                user.Position2 = this.txtPosition1.Value;
                user.LoginID = this.txtLoginID.Value;
                user.Password = this.GetPassword(this.txtPassword);
                user.MailAddress = this.txtEmailAddress.Value;
                user.StatusFlag = Convert.ToInt16(this.chkStatusFlag.Checked == true ? 0 : 1);

                user.CreateUID = this.LoginInfo.User.ID;
                user.UpdateUID = this.LoginInfo.User.ID;

                //Insert User
                using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                {
                    UserService userSer = new UserService(db);
                    GroupUserService groupSer = new GroupUserService(db);
                    DepartmentService departmentSer = new DepartmentService(db);

                    //Get Group User
                    M_GroupUser_H groupUser = groupSer.GetByGroupCD(this.txtGroupCD.Value);
                    M_Department departmentUser = departmentSer.GetByDepartmentCd(this.txtDepartmentCD.Value);
                    if (groupUser != null)
                    {
                        user.GroupID = groupUser.ID;
                    }

                    if (departmentUser != null)
                    {
                        user.DepartmentID = departmentUser.ID;
                    }

                    //Insert User
                    userSer.Insert(user);

                    user = userSer.GetByKey(user.UserCD, user.LoginID);
                    List<T_PaidVacation> detail = this.GetDetailList(true);
                    T_PaidVacationService paidVacationService = new T_PaidVacationService(db);
                    foreach (var item in detail)
                    {
                        if (item.Year == 0 && item.VacationDay == 0)
                        {
                            continue;
                        }
                        item.UID = user.ID;
                        item.CreateUID = user.CreateUID;
                        item.UpdateUID = user.UpdateUID;
                        paidVacationService.Insert(item);
                    }

                    db.Commit();
                }
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains(Models.Constant.M_USER_UN_CODE))
                {
                    this.SetMessage(this.txtUserCode.ID, M_Message.MSG_EXIST_CODE, "社員コード");
                }

                if (ex.Message.Contains(Models.Constant.M_USER_UN_LOGINID))
                {
                    this.SetMessage(this.txtLoginID.ID, M_Message.MSG_EXIST_CODE, "ログインID");
                }

                if (ex.Message.Contains(Models.Constant.M_USER_FK_GROUPID))
                {
                    this.SetMessage(this.txtGroupCD.ID, M_Message.MSG_NOT_EXIST_CODE, "権限グループ");
                }

                if (ex.Message.Contains(Models.Constant.M_USER_FK_DEPARTMENTID))
                {
                    this.SetMessage(this.txtDepartmentCD.ID, M_Message.MSG_NOT_EXIST_CODE, "部門");
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
                using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                {
                    M_User user = this.GetUser(this.UserID, db);
                    if (user != null)
                    {
                        //Create model
                        user.UserCD = this.txtUserCode.Value;
                        user.UserName1 = this.txtUserName1.Value;
                        user.UserName2 = string.Empty;
                        user.Position1 = this.txtPosition1.Value;
                        user.Position2 = this.txtPosition1.Value;
                        user.LoginID = this.txtLoginID.Value;
                        user.Password = this.GetPassword(this.txtPassword);
                        user.MailAddress = this.txtEmailAddress.Value;
                        user.StatusFlag = Convert.ToInt16(this.chkStatusFlag.Checked == true ? 0 : 1);

                        user.UpdateDate = this.OldUpdateDate;
                        user.UpdateUID = this.LoginInfo.User.ID;

                        //Update User                    

                        UserService userSer = new UserService(db);
                        GroupUserService groupSer = new GroupUserService(db);
                        DepartmentService departmentSer = new DepartmentService(db);

                        //Get Group User
                        M_GroupUser_H groupUser = groupSer.GetByGroupCD(this.txtGroupCD.Value);

                        M_Department departmentUser = departmentSer.GetByDepartmentCd(this.txtDepartmentCD.Value);
                        if (groupUser != null)
                        {
                            user.GroupID = groupUser.ID;
                        }

                        if (departmentUser != null)
                        {
                            user.DepartmentID = departmentUser.ID;
                        }

                        //Update User
                        if (user.Status == DataStatus.Changed)
                        {
                            ret = userSer.Update(user);

                            if (ret == 1)
                            {
                                //Update Paid Vacation
                                T_PaidVacationService paidVacationService = new T_PaidVacationService(db);
                                IList<T_PaidVacation> listPaidVacation_Old = paidVacationService.GetByUID(user.ID);
                                paidVacationService.Delete(user.ID);

                                List<T_PaidVacation> detail = this.GetDetailList(true);
                                foreach (var item in detail)
                                {
                                    bool update_flg = true;
                                    if (item.Year == 0 && item.VacationDay == 0)
                                    {
                                        continue;
                                    }

                                    foreach (var vacation_old in listPaidVacation_Old)
                                    {
                                        if (vacation_old.UID == user.ID && vacation_old.Year == item.Year)
                                        {
                                            item.UID = user.ID;
                                            item.CreateUID = vacation_old.CreateUID;
                                            item.CreateDate = vacation_old.CreateDate;
                                            item.UpdateUID = user.UpdateUID;
                                            paidVacationService.InsertForOldData(item);

                                            update_flg = false;
                                            break;
                                        }
                                    }

                                    if (update_flg)
                                    {
                                        item.UID = user.ID;
                                        item.Year = item.Year;
                                        item.VacationDay = item.VacationDay;
                                        item.CreateUID = this.LoginInfo.User.ID;
                                        item.UpdateUID = user.UpdateUID;
                                        paidVacationService.Insert(item);
                                    }
                                }
                            }
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
                    //du lieu da thay doi
                    this.SetMessage(string.Empty, M_Message.MSG_DATA_CHANGED);
                    return false;
                }
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains(Models.Constant.M_USER_UN_LOGINID))
                {
                    this.SetMessage(this.txtLoginID.ClientID, M_Message.MSG_EXIST_CODE, "ログインID");
                }

                if (ex.Message.Contains(Models.Constant.M_USER_FK_GROUPID))
                {

                    this.SetMessage(this.txtGroupCD.ClientID, M_Message.MSG_NOT_EXIST_CODE, "権限グループ");
                }

                if (ex.Message.Contains(Models.Constant.M_USER_FK_DEPARTMENTID))
                {
                    this.SetMessage(this.txtDepartmentCD.ID, M_Message.MSG_NOT_EXIST_CODE, "部門");
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
        /// Reset Group Name
        /// </summary>
        private void ResetGroupName()
        {
            this.txtGroupNm.Value = string.Empty;
            if (!this.txtGroupCD.IsEmpty)
            {
                using (DB db = new DB())
                {
                    GroupUserService groupSer = new GroupUserService(db);
                    M_GroupUser_H grp = groupSer.GetByGroupCD(this.txtGroupCD.Value);
                    if (grp != null)
                    {
                        this.txtGroupNm.Value = grp.GroupName;
                    }
                }
            }
        }

        /// <summary>
        /// Reset Group Name
        /// </summary>
        private void ResetDepartmentName()
        {
            this.txtDepartmentNm.Value = string.Empty;
            if (!this.txtDepartmentCD.IsEmpty)
            {
                using (DB db = new DB())
                {
                    DepartmentService departmentSer = new DepartmentService(db);
                    M_Department dp = departmentSer.GetByDepartmentCd(this.txtDepartmentCD.Value);
                    if (dp != null)
                    {
                        this.txtDepartmentNm.Value = dp.DepartmentName;
                    }
                }
            }
        }

        /// <summary>
        /// Get Password from textbox
        /// </summary>
        /// <param name="txtPassword"></param>
        /// <returns></returns>
        protected string GetPassword(ITextBox txtPassword)
        {
            try
            {
                return Security.Instance.Decrypt(txtPassword.Value);
            }
            catch (Exception)
            {
                return txtPassword.Value;
            }
        }

        /// <summary>
        /// Init PaidVacation List
        /// </summary>
        private void InitPaidVacationList()
        {
            //Add data
            IList<T_PaidVacation> listPaidVacation = new List<T_PaidVacation>();
            listPaidVacation.Add(new T_PaidVacation());

            //Process list view
            this.rptList.DataSource = listPaidVacation;
            this.rptList.DataBind();
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
        /// Check empty row
        /// </summary>
        /// <returns></returns>
        private bool IsEmptyRow(RepeaterItem item)
        {
            bool ret = true;

            //Year
            ICodeTextBox txtYear = (ICodeTextBox)item.FindControl("txtYear");
            if (txtYear != null)
            {
                if (!string.IsNullOrEmpty(txtYear.Value))
                {
                    ret = false;
                }
            }

            //VacationDay
            ITextBox txtVacationDay = (ITextBox)item.FindControl("txtVacationDay");
            if (txtVacationDay != null)
            {
                if (!string.IsNullOrEmpty(txtVacationDay.Value))
                {
                    ret = false;
                }
            }

            return ret;
        }

        /// <summary>
        /// Get detail list from screen
        /// </summary>
        /// <returns></returns>
        private List<T_PaidVacation> GetDetailList(bool isProcess)
        {
            List<T_PaidVacation> results = new List<T_PaidVacation>();

            foreach (RepeaterItem item in this.rptList.Items)
            {
                if (isProcess && this.IsEmptyRow(item))
                {
                    continue;
                }

                HtmlInputCheckBox chkDelFlg = (HtmlInputCheckBox)item.FindControl("deleteFlag");
                ICodeTextBox txtYear = (ICodeTextBox)item.FindControl("txtYear");
                ITextBox txtVacationDay = (ITextBox)item.FindControl("txtVacationDay");
                HtmlInputCheckBox chkInvalidFlag = (HtmlInputCheckBox)item.FindControl("invalidFlag");

                T_PaidVacation addItem = new T_PaidVacation();

                //Delete flag
                if (chkDelFlg != null)
                {
                    addItem.DelFlag = (chkDelFlg.Checked) ? true : false;
                }

                //Year
                int year = 0;
                if (int.TryParse(txtYear.Value, out year))
                {
                    addItem.Year = year;
                }

                //VacationDay
                decimal vacationday = 0;
                if (decimal.TryParse(txtVacationDay.Value, out vacationday))
                {
                    addItem.VacationDay = vacationday;
                }

                //Invalid flag
                if (chkInvalidFlag != null)
                {
                    addItem.InvalidFlag = (chkInvalidFlag.Checked) ? (short)1 : (short)0;
                }

                results.Add(addItem);
            }

            return results;
        }


        /// <summary>
        /// Show Paid Vacation data on form
        /// </summary>
        /// <param name="UID">uid</param>
        private void ShowPaidVacationData(int uid)
        {
            using (DB db = new DB())
            {
                T_PaidVacationService dbSer = new T_PaidVacationService(db);

                //Get list detail
                IList<T_PaidVacation> listDetail = dbSer.GetByUID(uid);
                if (listDetail != null)
                {
                    if (listDetail.Count != 0)
                    {
                        this.rptList.DataSource = listDetail;
                        this.LatestUpdateDate = dbSer.GetLatestData(uid).UpdateDate;
                    }
                    else
                    {
                        //Init paid vacation list
                        this.InitPaidVacationList();
                        this.LatestUpdateDate = DateTime.MinValue;
                    }
                }

                this.rptList.DataBind();
            }
        }
        #endregion

        #region Web Methods

        /// <summary>
        /// Get Group Name By Group Code
        /// </summary>
        /// <param name="groupCd">Group Code</param>
        /// <returns>Group Name</returns>
        [System.Web.Services.WebMethod]
        public static string GetGroupName(string in1)
        {
            var groupCd = in1;
            var groupCdShow = in1;
            groupCd = OMS.Utilities.EditDataUtil.ToFixCodeDB(groupCd, M_GroupUser_H.GROUP_CODE_DB_MAX_LENGTH);
            groupCdShow = EditDataUtil.ToFixCodeShow(groupCd, M_GroupUser_H.GROUP_CODE_SHOW_MAX_LENGTH);

            try
            {
                using (DB db = new DB())
                {
                    GroupUserService grpSer = new GroupUserService(db);
                    M_GroupUser_H model = grpSer.GetByGroupCD(groupCd);
                    if (model != null)
                    {
                        var result = new
                        {
                            groupCD = groupCdShow,
                            groupNm = model.GroupName
                        };
                        return OMS.Utilities.EditDataUtil.JsonSerializer<object>(result);
                    }
                    var groupCD = new
                    {
                        groupCD = groupCdShow
                    };
                    return OMS.Utilities.EditDataUtil.JsonSerializer<object>(groupCD);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }


        /// <summary>
        /// Get Department Name By Group Code
        /// </summary>
        /// <param name="groupCd">Department Code</param>
        /// <returns>Department Name</returns>
        [System.Web.Services.WebMethod]
        public static string GetDepartmentName(string in1)
        {
            var departmentCd = in1;
            var departmentCdShow = in1;

            departmentCd = OMS.Utilities.EditDataUtil.ToFixCodeDB(departmentCd, M_Department.DEPARTMENT_CODE_DB_MAX_LENGTH);
            departmentCdShow = OMS.Utilities.EditDataUtil.ToFixCodeShow(departmentCd, M_Department.DEPARTMENT_CODE_SHOW_MAX_LENGTH);
            try
            {
                using (DB db = new DB())
                {
                    DepartmentService grpSer = new DepartmentService(db);
                    M_Department model = grpSer.GetByDepartmentCd(departmentCd);
                    if (model != null)
                    {
                        var result = new
                        {
                            departmentCD = departmentCdShow,
                            departmentNm = model.DepartmentName
                        };
                        return OMS.Utilities.EditDataUtil.JsonSerializer<object>(result);
                    }
                    var departmentCD = new
                    {
                        departmentCD = departmentCdShow
                    };
                    return OMS.Utilities.EditDataUtil.JsonSerializer<object>(departmentCD);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion
    }
}