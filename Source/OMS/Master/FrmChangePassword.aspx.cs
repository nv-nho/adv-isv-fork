using System;
using System.Web.UI.WebControls;
using OMS.DAC;
using OMS.Models;
using OMS.Utilities;
using System.Text.RegularExpressions;
using OMS.Controls;

namespace OMS.ChangePassword
{
    /// <summary>
    /// Change Password Form
    /// ISV-GIAM
    /// </summary>
    public partial class FrmChangePassword : FrmBaseDetail
    {
        #region Property
        /// <summary>
        /// User
        /// </summary>
        private M_User MUser
        {
            get
            {
                if (base.ViewState["data"] != null)
                {
                    return (M_User)base.ViewState["data"];
                }
                return null;
            }
            set
            {
                base.ViewState["data"] = value;
            }
        }
        #endregion

        #region Event
        /// <summary>
        /// Init
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //Set Title
            base.FormTitle = "パスワード変更";
            base.FormSubTitle = "";

            //Init Max Length
            this.txtUserName.MaxLength = M_User.USER_NAME_1_MAX_LENGTH;
            this.txtLoginID.MaxLength = M_User.LOGIN_ID_MAX_LENGTH;
            this.txtOldPassword.MaxLength = M_User.PASSWORD_MAX_LENGTH;
            this.txtNewPassword.MaxLength = M_User.PASSWORD_MAX_LENGTH;
            this.txtConfirm.MaxLength = M_User.PASSWORD_MAX_LENGTH;

            //Lock control
            this.txtUserName.ReadOnly = true;
            this.txtLoginID.ReadOnly = true;

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
            //Set mode
            this.Mode = Mode.Update;

            if (!this.IsPostBack)
            {
                //Get user
                using (DB db = new DB())
                {
                    UserService userSer = new UserService(db);
                    this.MUser = userSer.GetByLoginID(this.LoginInfo.User.LoginID);
                }

                //Init
                this.txtUserName.Value = this.LoginInfo.User.UserName2;
                this.txtLoginID.Value = this.LoginInfo.User.LoginID;
                this.txtOldPassword.Value = string.Empty;
                this.txtNewPassword.Value = string.Empty;
                this.txtConfirm.Value = string.Empty;

                //Set Attributes value
                //this.txtUserName.Attributes.Add("value", this.LoginInfo.User.UserName2);
                this.txtOldPassword.Attributes.Add("value", string.Empty);
                this.txtNewPassword.Attributes.Add("value", string.Empty);
                this.txtConfirm.Attributes.Add("value", string.Empty);
            }
        }

        /// <summary>
        /// Change Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnChange_Click(object sender, EventArgs e)
        {
            //Set Attributes value
            this.txtUserName.Attributes.Add("value", this.txtUserName.Value);
            
            var oldPassword = this.GetPassword(this.txtOldPassword);
            var newPassword = this.GetPassword(this.txtNewPassword);
            var cfmPassword = this.GetPassword(this.txtConfirm);


            this.txtOldPassword.Value = Security.Instance.Encrypt(oldPassword);
            this.txtNewPassword.Value = Security.Instance.Encrypt(newPassword);
            this.txtConfirm.Value = Security.Instance.Encrypt(cfmPassword);
            if (!string.IsNullOrEmpty(oldPassword))
            {
                this.txtOldPassword.Attributes.Add("value", this.txtOldPassword.Value);
            }
            if (!string.IsNullOrEmpty(newPassword))
            {
                this.txtNewPassword.Attributes.Add("value", this.txtNewPassword.Value);
            }
            if (!string.IsNullOrEmpty(cfmPassword))
            {
                this.txtConfirm.Attributes.Add("value", this.txtConfirm.Value);
            }

            //Check input
            if (!this.CheckInput())
            {
                return;
            }

            //Show question insert
            base.ShowQuestionMessage(M_Message.MSG_QUESTION_UPDATE, Models.DefaultButton.Yes, false);
        }
        #endregion

        #region Method
        /// <summary>
        /// Check Input
        /// </summary>
        /// <returns></returns>
        private bool CheckInput()
        {
            //Check empty and max length
            if (this.CheckIsEmptyAndMaxLength())
            {
                var oldPassword = Security.Instance.Decrypt(this.txtOldPassword.Value);
                var newPassword = Security.Instance.Decrypt(this.txtNewPassword.Value);
                var cfmPassword = Security.Instance.Decrypt(this.txtConfirm.Value);

                //Check correct old password
                if (!oldPassword.Equals(this.MUser.Password))
                {
                    this.SetMessage(this.txtOldPassword.ID, M_Message.MSG_PASS_INVALID);
                }
                //Compare between new Password and confirm Password
                else if (!newPassword.Equals(cfmPassword))
                {
                    this.SetMessage(this.txtConfirm.ID, M_Message.MSG_PASS_NOT_MATCH);

                }else if(newPassword.Equals(oldPassword))
                {
                    this.SetMessage(this.txtNewPassword.ID, M_Message.MSG_MUST_BE_DIFFERENT, "新パスワード", "旧パスワード");
                }
            }

            //Check error
            return !base.HaveError;
        }

        /// <summary>
        /// Check Is Empty
        /// </summary>
        /// <returns></returns>
        private bool CheckIsEmptyAndMaxLength()
        {
            bool result = true;

            var oldPassword = Security.Instance.Decrypt(this.txtOldPassword.Value);
            var newPassword = Security.Instance.Decrypt(this.txtNewPassword.Value);
            var cfmPassword = Security.Instance.Decrypt(this.txtConfirm.Value);

            //OldPassword
            if (string.IsNullOrEmpty(oldPassword))
            {
                this.SetMessage(this.txtOldPassword.ID, M_Message.MSG_REQUIRE, "旧パスワード");
                result = false;
            }
            else if (!this.IsAdmin() &&
                (oldPassword.Length < 8 || Regex.Matches(oldPassword, "[a-zA-Z]").Count == 0 || Regex.Matches(oldPassword, "[0-9]").Count == 0))
            {
                this.SetMessage(this.txtOldPassword.ID, M_Message.MSG_PASSWORD_RULE);
                result = false;
            }

            //NewPassword
            if (string.IsNullOrEmpty(newPassword))
            {
                this.SetMessage(this.txtNewPassword.ID, M_Message.MSG_REQUIRE, "新パスワード");
                result = false;
            }
            else if (newPassword.Length < 8 || Regex.Matches(newPassword, "[a-zA-Z]").Count == 0 || Regex.Matches(newPassword, "[0-9]").Count == 0)
            {
                this.SetMessage(this.txtNewPassword.ID, M_Message.MSG_PASSWORD_RULE);
                result = false;
            }

            //Confirm
            if (string.IsNullOrEmpty(cfmPassword))
            {
                this.SetMessage(this.txtConfirm.ID, M_Message.MSG_REQUIRE, "新パスワード（確認用）");
                result = false;
            }
            else if (cfmPassword.Length < 8 || Regex.Matches(cfmPassword, "[a-zA-Z]").Count == 0 || Regex.Matches(cfmPassword, "[0-9]").Count == 0)
            {
                this.SetMessage(this.txtConfirm.ID, M_Message.MSG_PASSWORD_RULE);
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Process Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnProcessData(object sender, EventArgs e)
        {
            try
            {
                int ret = 0;

                //Update
                using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                {
                    UserService userSer = new UserService(db);
                    ret = userSer.UpdatePassword(this.MUser, this.txtNewPassword.Value);
                    if (ret > 0)
                    {
                        db.Commit();
                    }
                }

                //Result
                if (ret == 0)
                {
                    //Data changed
                    this.SetMessage(string.Empty, M_Message.MSG_DATA_CHANGED);
                }
                else if (ret == 1)
                {
                    //Set Success
                    this.Success = true;

                    //Update ViewState
                    using (DB db = new DB())
                    {
                        UserService userSer = new UserService(db);
                        this.MUser = userSer.GetByLoginID(this.LoginInfo.User.LoginID);
                    }

                    //Clear input
                    this.txtOldPassword.Attributes.Add("value", string.Empty);
                    this.txtNewPassword.Attributes.Add("value", string.Empty);
                    this.txtConfirm.Attributes.Add("value", string.Empty);
                }
            }
            catch (Exception ex)
            {
                this.SetMessage(string.Empty, M_Message.MSG_UPDATE_FAILE, "更新");
                Log.Instance.WriteLog(ex);
            }
        }

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
        #endregion
    }
}