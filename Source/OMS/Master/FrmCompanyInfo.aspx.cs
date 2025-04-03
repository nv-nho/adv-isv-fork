using System;
using System.Web.UI.WebControls;
using OMS.DAC;
using OMS.Models;
using OMS.Utilities;

namespace OMS.Master
{
    /// <summary>
    /// CompanyInfo Form
    /// Author:TRAM
    /// </summary>
    public partial class FrmCompanyInfo : FrmBaseDetail
    {
        #region Property

        /// <summary>
        /// Get or set CompanyID
        /// </summary>
        public int CompanyID
        {
            get { return (int)ViewState["CompanyID"]; }
            set { ViewState["CompanyID"] = value; }
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
            base.FormTitle = "会社情報";
            base.FormSubTitle = "";

            //Set maxlength of textbox
            this.InitMaxLengthControl();

            //Init Event
            LinkButton btnYes = (LinkButton)this.Master.FindControl("btnYes");
            btnYes.Click += new EventHandler(btnProcessData);            
        }

        /// <summary>
        /// ISV-TRAM
        /// Company Info
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            //check authority of login user
            base.SetAuthority(FormId.CompanyInfo);
            if (!base._authority.IsMasterView)
            {
                Response.Redirect("~/Menu/FrmMasterMenu.aspx");
            }

            if (!this.IsPostBack)
            {
                M_Company company =this.GetCompany();
                if (company == null)
                {
                    //Set Mode
                    this.ProcessMode(Mode.Insert);
                }
                else
                {
                    this.CompanyID=company.ID;

                    //Show data
                    this.ShowData(company);

                    //Set Mode
                    this.ProcessMode(Mode.View);
                }
            }

        }

        /// <summary>
        /// Event Edit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            //Get Company
            M_Company company = this.GetCompany();

            //Check company
            if (company != null)
            {
                //Show data
                this.ShowData(company);

                //Set Mode
                this.ProcessMode(Mode.Update);
            }
            else
            {
                Server.Transfer("../Menu/FrmMasterMenu.aspx");
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
        /// Event Back
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            //Get company
            M_Company company = this.GetCompany();

            //Check company
            if (company != null)
            {
                //Show data
                this.ShowData(company);

                //Set Mode
                this.ProcessMode(Mode.View);
            }
            else
            {
                Server.Transfer("../Menu/FrmMasterMenu.aspx");
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
                    //Insert Data
                    if (this.InsertData())
                    {
                        //Get data
                        M_Company company = this.GetCompany();

                        //Show data
                        this.ShowData(company);

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
                        //get data
                        M_Company company = this.GetCompany();

                        //Show data
                        this.ShowData(company);

                        //Set Mode
                        this.ProcessMode(Mode.View);

                        //Set Success
                        this.Success = true;
                    }

                    break;
            }   
        }

        #endregion

        #region Method

        /// <summary>
        /// Init Maxlength for control
        /// </summary>
        private void InitMaxLengthControl()
        {
            this.txtCompanyName1.MaxLength = M_Company.COMPANY_NAME1_MAX_LENGTH;
            this.txtCompanyName2.MaxLength = M_Company.COMPANY_NAME2_MAX_LENGTH;
            this.txtAddress1.MaxLength = M_Company.COMPANY_ADDRESS1_MAX_LENGTH;
            this.txtAddress2.MaxLength = M_Company.COMPANY_ADDRESS2_MAX_LENGTH;
            this.txtAddress3.MaxLength = M_Company.COMPANY_ADDRESS3_MAX_LENGTH;
            this.txtAddress4.MaxLength = M_Company.COMPANY_ADDRESS4_MAX_LENGTH;
            this.txtAddress5.MaxLength = M_Company.COMPANY_ADDRESS5_MAX_LENGTH;
            this.txtAddress6.MaxLength = M_Company.COMPANY_ADDRESS6_MAX_LENGTH;
            this.txtTel.MaxLength = M_Company.TEL_MAX_LENGTH;
            this.txtTel2.MaxLength = M_Company.TEL2_MAX_LENGTH;
            this.txtFAX.MaxLength = M_Company.FAX_MAX_LENGTH;
            this.txtEmailAddress.MaxLength = M_Company.EMAIL_ADDRESS_MAX_LENGTH;
            this.txtRepresent.MaxLength = M_Company.REPRESENT_MAX_LENGTH;
            this.txtPosition.MaxLength = M_Company.POSITION_MAX_LENGTH;
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
                case Mode.Update:
                    enable = false;
                    break;

                default:
                    enable = true;
                    base.DisabledLink(this.btnEdit, !base._authority.IsMasterEdit);
                    break;
            }
            
            //Lock control
            this.txtCompanyName1.ReadOnly = enable;
            this.txtCompanyName2.ReadOnly = enable;
            this.txtAddress1.ReadOnly = enable;
            this.txtAddress2.ReadOnly = enable;
            this.txtAddress3.ReadOnly = enable;
            this.txtAddress4.ReadOnly = enable;
            this.txtAddress5.ReadOnly = enable;
            this.txtAddress6.ReadOnly = enable;
            this.txtTel.ReadOnly = enable;
            this.txtTel2.ReadOnly = enable;
            this.txtFAX.ReadOnly = enable;
            this.txtEmailAddress.ReadOnly = enable;
            this.txtRepresent.ReadOnly = enable;
            this.txtPosition.ReadOnly = enable;           
        }

        #region Get

        /// Get Company
        /// </summary>
        /// <returns>Company</returns>
        private M_Company GetCompany()
        {
            using (DB db = new DB())
            {
                CompanyService companySer = new CompanyService(db);

                //Get Company
                return companySer.GetData();
            }
        }

        #endregion

        #region Show data
   
        /// <summary>
        /// Show data on form
        /// <param name="company">M_Company</param>
        /// </summary>
        private void ShowData(M_Company company)
        {
            if (company != null)
            {
                this.txtCompanyName1.Value = company.CompanyName1;
                this.txtCompanyName2.Value = company.CompanyName2;
                this.txtAddress1.Value = company.CompanyAddress1;
                this.txtAddress2.Value = company.CompanyAddress2;
                this.txtAddress3.Value = company.CompanyAddress3;
                this.txtAddress4.Value = company.CompanyAddress4;
                this.txtAddress5.Value = company.CompanyAddress5;
                this.txtAddress6.Value = company.CompanyAddress6;
                this.txtTel.Value = company.Tel.Trim();
                this.txtTel2.Value = company.Tel2.Trim();
                this.txtFAX.Value = company.FAX.Trim();
                this.txtEmailAddress.Value = company.EmailAddress;
                this.txtRepresent.Value = company.Represent;
                this.txtPosition.Value = company.Position;

                //Save CompanyID and UpdateDate
                this.CompanyID = company.ID;
                this.OldUpdateDate = company.UpdateDate;
            }
        }

        #endregion

        #region Create data

        /// <summary>
        /// Get data to insert
        /// </summary>
        /// <returns></returns>
        private M_Company CreateDataInsert()
        {
            M_Company company = new M_Company();
            company.CompanyName1 = this.txtCompanyName1.Value;
            company.CompanyName2 = this.txtCompanyName2.Value;
            company.CompanyAddress1 = this.txtAddress1.Value;
            company.CompanyAddress2 = this.txtAddress2.Value;
            company.CompanyAddress3 = this.txtAddress3.Value;
            company.CompanyAddress4 = this.txtAddress4.Value;
            company.CompanyAddress5 = this.txtAddress5.Value;
            company.CompanyAddress6 = this.txtAddress6.Value;
            company.Tel = this.txtTel.Value;
            company.Tel2 = this.txtTel2.Value;
            company.FAX = this.txtFAX.Value;
            company.EmailAddress = this.txtEmailAddress.Value;
            company.TAXCode = string.Empty;
            company.CompanyBank = string.Empty;
            company.AccountCode = string.Empty;
            company.Represent = this.txtRepresent.Value;
            company.Position = this.txtPosition.Value;
            company.Position2 = string.Empty;

            company.CreateUID = this.LoginInfo.User.ID;
            company.UpdateUID = this.LoginInfo.User.ID;

            return company;
        }

        /// <summary>
        /// Set data to update
        /// </summary>
        /// <returns></returns>
        private M_Company CreateDataUpdate()
        {
            M_Company company = new M_Company();

            company.CompanyName1 = this.txtCompanyName1.Value;
            company.CompanyName2 = this.txtCompanyName2.Value;
            company.CompanyAddress1 = this.txtAddress1.Value;
            company.CompanyAddress2 = this.txtAddress2.Value;
            company.CompanyAddress3 = this.txtAddress3.Value;
            company.CompanyAddress4 = this.txtAddress4.Value;
            company.CompanyAddress5 = this.txtAddress5.Value;
            company.CompanyAddress6 = this.txtAddress6.Value;
            company.Tel = this.txtTel.Value;
            company.Tel2 = this.txtTel2.Value;
            company.FAX = this.txtFAX.Value;
            company.EmailAddress = this.txtEmailAddress.Value;
            company.TAXCode = string.Empty;
            company.CompanyBank = string.Empty;
            company.AccountCode = string.Empty;
            company.Represent = this.txtRepresent.Value;
            company.Position = this.txtPosition.Value;
            company.Position2 = string.Empty;

            company.UpdateDate = this.OldUpdateDate;
            company.UpdateUID = this.LoginInfo.User.ID;
            return company;
        }

    #endregion
                
        #region Check Input

        /// <summary>
        /// Check Input
        /// </summary>
        /// <returns></returns>
        private bool CheckInput()
        {
            //Company Name 1
            if (this.txtCompanyName1.IsEmpty)
            {
                this.SetMessage(this.txtCompanyName1.ID, M_Message.MSG_REQUIRE, "会社1");
            }

            if (!this.txtEmailAddress.IsEmpty)
            {
                //EmailAddress
                if (!CheckDataUtil.IsEmail(this.txtEmailAddress.Text))
                {
                    this.SetMessage(this.txtEmailAddress.ID, M_Message.MSG_INCORRECT_FORMAT, "Email");
                }
            }            

            //Check error
            return !base.HaveError;
        }

    #endregion

        #region Insert data

        /// <summary>
        /// Insert
        /// </summary>
        private bool InsertData()
        {
            try
            {
                M_Company company = new M_Company();

                //Create model
                company = this.CreateDataInsert();

                //Insert Data
                using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                {
                    CompanyService companySer = new CompanyService(db);
                    companySer.Insert(company);
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

   #endregion

        #region Update data

        /// <summary>
        /// Update Data
        /// </summary>
        private bool UpdateData()
        {
            try
            {
                int ret=0;
                M_Company company = new M_Company();

                //Create model
                company = this.CreateDataUpdate();

                using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                {
                    CompanyService companySer = new CompanyService(db);
                    if (company.Status == DataStatus.Changed)
                    {
                        //Update data
                        ret = companySer.Update(company);
                        db.Commit();
                    }
                }

                if(ret==0)
                {
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

        #endregion

    #endregion
    }
}