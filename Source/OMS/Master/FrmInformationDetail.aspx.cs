using System;
using System.Data.SqlClient;

using OMS.DAC;
using OMS.Models;
using OMS.Utilities;
using System.Web.UI.WebControls;


namespace OMS.Master
{
    /// <summary>
    /// Form:   Information Detail
    /// Author: ISV-TRAM
    /// </summary>
    public partial class FrmInformationDetail : FrmBaseDetail
    {
        private const string URL_LIST = "~/Master/FrmInformationList.aspx";
        #region Property

        /// <summary>
        /// Get or set UserID
        /// </summary>
        public int DataID
        {
            get { return (int)ViewState["DataID"]; }
            set { ViewState["DataID"] = value; }
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
            base.FormTitle = "お知らせ情報";
            base.FormSubTitle = "Detail";

            //Init Max Length                        
            this.txtInformationName.MaxLength = M_Information.INFORMATION_NAME_MAX_LENGTH;
            this.txtInformationContent.MaxLength = M_Information.INFORMATION_CONTENT_MAX_LENGTH;

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
            base.SetAuthority(FormId.Information);
            if (!base._authority.IsMasterView)
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
                    if (this.PreviousPageViewState["DataID"] == null)
                    {
                        //Set mode
                        this.ProcessMode(Mode.Insert);
                    }
                    else
                    {
                        //Set data ID
                        this.DataID = int.Parse(PreviousPageViewState["DataID"].ToString());
                        M_Information data = this.GetInformationByID(this.DataID);

                        //Check event
                        if (data != null)
                        {
                            //Show data
                            this.ShowData(data);

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
        }

        /// <summary>
        /// Copy Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCopy_Click(object sender, EventArgs e)
        {
            //Get data
            M_Information data = this.GetInformationByID(this.DataID);

            //Check user
            if (data != null)
            {
                //Show data
                this.ShowData(data);

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
            //Get Data
            M_Information data = this.GetInformationByID(this.DataID);

            //Check data
            if (data != null)
            {
                //Show data
                this.ShowData(data);

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
        /// Event Back
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            //Get Information
            M_Information data = this.GetInformationByID(this.DataID);

            //Check Information
            if (data != null)
            {
                //Show data
                this.ShowData(data);

                //Set Mode
                this.ProcessMode(Mode.View);
            }
            else
            {
                Server.Transfer(URL_LIST);
            }
        }
        
        /// <summary>
        /// Event changed page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNew_Click(object sender, CommandEventArgs e)
        {
            this.dtBeginDate.Value = null;
            this.dtEndDate.Value = null;
            this.txtInformationName.Value = string.Empty;
            this.txtInformationContent.Value = string.Empty;

            //Set Mode
            this.ProcessMode(Mode.Insert);
        }

        /// <summary>
        /// Process Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnProcessData(object sender, EventArgs e)
        {
            bool ret;
            M_Information data = null;

            //Check Mode
            switch (this.Mode)
            {
                case Utilities.Mode.Insert:
                case Utilities.Mode.Copy:

                    //Insert Data
                    ret = this.InsertData();
                    if (ret)
                    {
                        M_Information model = this.SetModelInformation();
                        //Get Information
                        data = this.GetInformation(model);

                        //Show data
                        this.ShowData(data);

                        //Set Mode
                        this.ProcessMode(Mode.View);

                        //Set Success
                        this.Success = true;
                    }
                    break;

                case Utilities.Mode.Delete:

                    //Delete Information
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
                case Utilities.Mode.Update:

                    //Update Data
                    ret = this.UpdateData();
                    if (ret)
                    {
                        //Get Information
                        data = this.GetInformationByID(this.DataID);

                        //Show data
                        this.ShowData(data);

                        //Set Mode
                        this.ProcessMode(Mode.View);

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
            //Get Information
            M_Information data = this.GetInformationByID(this.DataID);
            if (data != null)
            {
                //Show data
                this.ShowData(data);

                //Set Mode
                this.ProcessMode(Mode.View);
            }
            else
            {
                Server.Transfer(URL_LIST);
            }
        }
        #endregion

        #region Methods

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
                    this.txtInformationName.ReadOnly = false;
                    this.dtBeginDate.ReadOnly = false;
                    this.dtEndDate.ReadOnly = false;
                    this.txtInformationContent.ReadOnly = false;
                    break;

                case Mode.Update:
                    this.txtInformationName.ReadOnly = false;
                    this.dtBeginDate.ReadOnly = false;
                    this.dtEndDate.ReadOnly = false;
                    this.txtInformationContent.ReadOnly = false;
                    break;

                default:
                    this.txtInformationName.ReadOnly = true;
                    this.dtBeginDate.ReadOnly = true;
                    this.dtEndDate.ReadOnly = true;
                    this.txtInformationContent.ReadOnly = true;

                    base.DisabledLink(this.btnEdit, !base._authority.IsMasterEdit);
                    base.DisabledLink(this.btnDelete, !base._authority.IsMasterDelete);
                    base.DisabledLink(this.btnCopy, !base._authority.IsMasterCopy);
                    //---------------Add 2014/12/29 ISV-HUNG--------------------//
                    base.DisabledLink(this.btnNew, !base._authority.IsMasterNew);
                    //---------------Add 2014/12/29 ISV-HUNG--------------------//
                    break;
            }
        }

        /// <summary>
        /// Show data on form
        /// </summary>
        /// <param name="model">M_Information</param>
        private void ShowData(M_Information model)
        {
            //Show data
            if (model != null)
            {
                this.dtBeginDate.Value = model.BeginDate;
                this.dtEndDate.Value = model.EndDate;
                this.txtInformationName.Value = model.InformationName;
                this.txtInformationContent.Value = model.InformationContent;

                //Save UserID and UpdateDate
                this.DataID = model.ID;
                this.OldUpdateDate = model.UpdateDate;
            }
        }

        /// <summary>
        /// Get Information by InformationID
        /// </summary>
        /// <param name="informationId">Information id</param>
        /// <returns>Information data</returns>
        private M_Information GetInformationByID(int informationId)
        {
            using (DB db = new DB())
            {
                InformationService service = new InformationService(db);

                //Get User
                return service.GetByID(informationId);
            }
        }


        /// <summary>
        /// Get Information by Information model
        /// </summary>
        /// <param name="informationId">Information id</param>
        /// <returns>Information data</returns>
        private M_Information GetInformation(M_Information model)
        {
            using (DB db = new DB())
            {
                InformationService service = new InformationService(db);

                //Get User
                return service.GetByInformation(model);
            }
        }

        /// <summary>
        /// Set the Information model
        /// </summary>
        /// <returns>Information Model</returns>
        private M_Information SetModelInformation()
        { 
            M_Information model =new M_Information();
            model.InformationName=this.txtInformationName.Value;
            model.BeginDate=this.dtBeginDate.Value.Value;
            model.EndDate = this.dtEndDate.Value.Value;
            model.InformationContent = this.txtInformationContent.Value;
            model.CreateUID = this.LoginInfo.User.ID;
            model.UpdateUID = this.LoginInfo.User.ID;
            return model;
        }

        /// <summary>
        /// Check input
        /// </summary>
        /// <returns>Valid:true, Invalid:false</returns>
        private bool CheckInput()
        {
            //InformationName
            if (this.txtInformationName.IsEmpty)
            {
                this.SetMessage(this.txtInformationName.ID, M_Message.MSG_REQUIRE, "お知らせ情報名");
            }

            //BeginDate
            if (this.dtBeginDate.IsEmpty)
            {
                this.SetMessage(this.dtBeginDate.ID, M_Message.MSG_REQUIRE, "開始日");
            }

            //EndDate
            if (this.dtEndDate.IsEmpty)
            {
                this.SetMessage(this.dtEndDate.ID, M_Message.MSG_REQUIRE, "終了日");
            }

            if (!this.dtBeginDate.IsEmpty && !this.dtEndDate.IsEmpty)
            {
                if (this.dtBeginDate.Value > this.dtEndDate.Value)
                {
                    this.SetMessage(this.dtBeginDate.ID, M_Message.MSG_LESS_THAN_EQUAL, "開始日", "終了日");
                }
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
                M_Information data = new M_Information();
                data.BeginDate =this.dtBeginDate.Value.GetValueOrDefault();
                data.EndDate = this.dtEndDate.Value.GetValueOrDefault();
                data.InformationName = this.txtInformationName.Value;
                data.InformationContent = this.txtInformationContent.Value;
                data.CreateUID = base.LoginInfo.User.ID;
                data.UpdateUID = base.LoginInfo.User.ID;

                //Insert Event
                using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                {
                    InformationService serive = new InformationService(db);

                    //Insert Event
                    serive.Insert(data);
                    
                    db.Commit();
                }
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
                M_Information data = this.GetInformationByID(this.DataID);
                if (data != null)
                {
                    //Create model
                    data.BeginDate =this.dtBeginDate.Value.GetValueOrDefault();
                    data.EndDate = this.dtEndDate.Value.GetValueOrDefault();
                    data.InformationName = this.txtInformationName.Value;
                    data.InformationContent = this.txtInformationContent.Value;
                    data.UpdateDate = this.OldUpdateDate;
                    data.UpdateUID = base.LoginInfo.User.ID;

                    //Update Event
                    using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                    {
                        InformationService service = new InformationService(db);

                        //Update Event
                        if (data.Status == DataStatus.Changed)
                        {
                            ret = service.Update(data);

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
        /// <returns>True: delete success/False: delete fail</returns>
        private bool DeleteData()
        {
            try
            {
                int ret = 0;
                using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                {
                    InformationService service = new InformationService(db);

                    //Delete Vendor
                    ret = service.Delete(this.DataID, this.OldUpdateDate);
                    db.Commit();
                }

                //Check result update
                if (ret == 0)
                {
                    //Data is changed
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
    }
}