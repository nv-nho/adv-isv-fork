using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Collections;
using System.Text;

using OMS.Utilities;

using OMS.DAC;
using OMS.Models;
using System.Web;

namespace OMS.Login
{
    public partial class FrmLogin : System.Web.UI.Page
    {
        private const string SESSION_COMP_KEY = "COMP";
        private const string SESSION_LGN_KEY = "LGN";
        private const string SESSION_PSW_KEY = "PSW";

        protected IList<string> CtrlIDErrors
        {
            get
            {
                return (IList<string>)base.ViewState["CTRL_ERROR_ID"];
            }
            private set
            {
                base.ViewState["CTRL_ERROR_ID"] = value;
            }
        }
        protected IList<string> MsgErrors
        {
            get
            {
                return (IList<string>)base.ViewState["MSG_ERROR"];
            }
            private set
            {
                base.ViewState["MSG_ERROR"] = value;
            }
        }

        /// <summary>
        /// Get or set Image Control
        /// </summary>
        public string ImageCtrl
        {
            get
            {
                if (ViewState["ImageCtrl"] != null)
                {
                    return (string)ViewState["ImageCtrl"];
                }
                return string.Empty;
            }
            set
            {
                ViewState["ImageCtrl"] = value;
            }
        }

        /// <summary>
        /// Message List
        /// </summary>
        private IDictionary<string, M_Message> Messages { get; set; }

        /// <summary>
        /// Set error messsage
        /// </summary>
        /// <param name="ctrlID">Error ControlID</param>
        /// <param name="msgID">Message Id</param>
        /// <param name="args">List argument of messsage</param>
        protected void SetMessage(string ctrlID, string msgID, params object[] args)
        {
            //Get Message
            M_Message mess = (M_Message)this.Messages[msgID];
            //Check Message Type
            switch (mess.Type)
            {
                case "E":
                    this.MsgErrors.Add(string.Format("<h5>{0}</h5>", string.Format(mess.Message3, args)));
                    //this.MsgErrors.Add(string.Format("<h5>{0}</h5>", string.Format(mess.Message2, args)));
                    this.CtrlIDErrors.Add(ctrlID);
                    break;
                default:
                    break;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.ImageCtrl = string.Format("<img id='imgLogo' src='../Logo/no-image.png'>");

            //Get list message
            using (DB db = new DB())
            {
                MessageService msgSer = new MessageService(db);
                this.Messages = msgSer.GetAll().ToDictionary(key => key.MessageID, value => value);

                SettingService dbSer = new SettingService(db);
                var setting = dbSer.GetData();
                if (setting != null)
                {
                    var logo = setting.Logo2;
                    var fullPath = Server.MapPath(string.Format("../Logo/{0}", logo));
                    if (System.IO.File.Exists(fullPath))
                    {
                        this.ImageCtrl = string.Format("<img id='imgLogo' src='../Logo/{0}'>", logo);
                    }
                }                
            }

            this.CtrlIDErrors = new List<string>();
            this.MsgErrors = new List<string>();

            this.txtLoginID.MaxLength = M_User.LOGIN_ID_MAX_LENGTH;
            this.txtPassword.MaxLength = M_User.PASSWORD_MAX_LENGTH;
        }

        /// <summary>
        /// page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session.Clear();
                
                //Clear intput
                this.ClearInput();

                //Set focus
                this.txtLoginID.Focus();
                
                var lgnK = HttpUtility.UrlEncode(Utilities.Security.Instance.Encrypt(SESSION_LGN_KEY));
                var pswK = HttpUtility.UrlEncode(Utilities.Security.Instance.Encrypt(SESSION_PSW_KEY));

                //restore Login Infomation
                if (Request.Cookies[lgnK] != null && Request.Cookies[pswK] != null)
                {
                    this.txtLoginID.Text = Utilities.Security.Instance.Decrypt(Request.Cookies[lgnK].Value);
                    this.txtPassword.Text = Utilities.Security.Instance.Decrypt(Request.Cookies[pswK].Value);

                    string script = String.Format("document.getElementById('{0}').value = '{1}';", this.txtPassword.ClientID, this.txtPassword.Text);
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "SetPassword", script, true);

                    this.ckbRemember.Checked = true;
                }
            }
        }

        /// <summary>
        /// Sign Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSignIn_Click(object sender, EventArgs e)
        {
            //Check input
            if (!this.CheckInput())
            {
                this.FocusAndAddClassError();
                return;
            }

            using (DB db = new DB())
            {
                try
                {
                    UserService userService = new UserService(db);
                    GroupUserService groupUserService = new GroupUserService(db);
                    M_User user = userService.GetByLoginID(this.txtLoginID.Text.Trim());
                    
                    //check exists user
                    if (user == null || this.txtPassword.Text.Trim() != user.Password)
                    {
                        this.SetMessage("txtLoginID", M_Message.MSG_LOGIN_INFO_INCORRECT);
                        this.CtrlIDErrors.Add("txtLoginID");
                        this.CtrlIDErrors.Add("txtPassword");
                        this.FocusAndAddClassError();
                        return;
                    }

                    if (user.StatusFlag == 1)
                    {
                        this.SetMessage("txtLoginID", M_Message.MSG_LOGIN_INFO_INCORRECT, "LoginID");
                        this.CtrlIDErrors.Add("txtLoginID");
                        this.FocusAndAddClassError();
                        return;
                    }

                    Models.LoginInfo loginInfo = new Models.LoginInfo();
                    loginInfo.User = user;
                    loginInfo.GroupUser = groupUserService.GetByGroupID(user.GroupID);
                    loginInfo.ListGroupDetail = groupUserService.GetListGroupUserDetail(user.GroupID).ToList();


                    //Store login info
                    Session["LoginInfo"] = loginInfo;

                    var lgnK = HttpUtility.UrlEncode(Utilities.Security.Instance.Encrypt(SESSION_LGN_KEY));
                    var pswK = HttpUtility.UrlEncode(Utilities.Security.Instance.Encrypt(SESSION_PSW_KEY));

                    //store Login Infomation
                    if (this.ckbRemember.Checked)
                    {
                        Response.Cookies[lgnK].Expires = DateTime.Now.AddDays(30);
                        Response.Cookies[pswK].Expires = DateTime.Now.AddDays(30);
                    }
                    else
                    {
                        Response.Cookies[lgnK].Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies["Password"].Expires = DateTime.Now.AddDays(-1);

                    }
                    Response.Cookies[lgnK].Value = Utilities.Security.Instance.Encrypt(this.txtLoginID.Text.Trim());
                    Response.Cookies[pswK].Value = Utilities.Security.Instance.Encrypt(this.txtPassword.Text.Trim());

                    Response.Redirect("~/Menu/FrmMainMenu.aspx");
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Input check
        /// </summary>
        /// <returns></returns>
        private bool CheckInput()
        {
            bool ret = true;

            //Check login ID
            if (string.IsNullOrEmpty(this.txtLoginID.Text))
            {
                this.SetMessage("txtLoginID", M_Message.MSG_REQUIRE, "LoginID");
                ret = false;
            }

            //Check password
            if (string.IsNullOrEmpty(this.txtPassword.Text))
            {
                this.SetMessage("txtPassword", M_Message.MSG_REQUIRE, "Password");
                ret = false;
            }

            return ret;
        }

        private void FocusAndAddClassError()
        {
            StringBuilder script = new StringBuilder();
            if (this.CtrlIDErrors.Count > 0)
            {
                foreach (string item in this.CtrlIDErrors)
                {
                    script.AppendLine(String.Format("document.getElementById('{0}').parentNode.className = 'has-error';", item));
                }

                script.AppendLine(string.Format("document.getElementById('{0}').focus();", this.CtrlIDErrors[0]));

                Page.ClientScript.RegisterStartupScript(this.GetType(), "FocusAndAddClassError", script.ToString(), true);
            }
        }

        /// <summary>
        /// Clear Input
        /// </summary>
        private void ClearInput()
        {
            this.txtLoginID.Text = string.Empty;
            this.txtPassword.Text = string.Empty;
        }

        /// <summary>
        /// Get display message
        /// </summary>
        /// <returns>HTML of error message</returns>
        public string GetMessage()
        {
            string ret = string.Empty;
            if (this.MsgErrors.Count > 0)
            {
                ret += "<div id='panelError' class='alert alert-danger alert-dismissible' role='alert' runat='server'>";
                ret += "<button type='button' class='close' data-dismiss='alert'><span aria-hidden='true'>&times;</span><span class='sr-only'></span></button>";
                //ret += "<span class='glyphicon glyphicon-remove-sign'></span><strong> Warning!</strong>";
                ret += "<span class='glyphicon glyphicon-remove-sign'></span><strong> 警告!</strong>";

                int i = 0;
                string msg1 = string.Empty;
                //string msg2 = string.Empty;
                foreach (var msg in this.MsgErrors)
                {
                    //if (i % 2 == 0)
                    //{
                        msg1 += msg;
                    //}
                    //else
                    //{
                    //    msg2 += msg;
                    //}
                    i++;
                }

                ret += msg1;
                //ret += "<span class='glyphicon glyphicon-remove-sign'></span><strong> Cảnh báo!</strong>";
                //ret += msg2;
                ret += "</div>";

            }
            return ret;
        }

        /// <summary>
        /// Get display Logo Image
        /// </summary>
        /// <returns>Get display Logo Image</returns>
        public string GetLogoImage()
        {
            return this.ImageCtrl;
        }
    }
}