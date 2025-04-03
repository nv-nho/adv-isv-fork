using System;

using OMS.DAC;
using System.IO;

namespace OMS
{
    /// <summary>
    /// Class SiteMaster
    /// </summary>
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        #region Property

        /// <summary>
        /// Get or set FormTitle
        /// </summary>
        public string FormTitle
        {
            get
            {
                if (ViewState["FormTitle"] != null)
                {
                    return (string)ViewState["FormTitle"];
                }
                return string.Empty;
            }
            set
            {
                ViewState["FormTitle"] = value;
            }
        }

        /// <summary>
        /// Get or set FormSubTitle
        /// </summary>
        public string FormSubTitle
        {
            get
            {
                if (ViewState["FormSubTitle"] != null)
                {
                    return (string)ViewState["FormSubTitle"];
                }
                return string.Empty;
            }
            set
            {
                ViewState["FormSubTitle"] = value;
            }
        }

        /// <summary>
        /// Get or set Label Mode
        /// </summary>
        public string LabelMode
        {
            get
            {
                if (ViewState["LabelMode"] != null)
                {
                    return (string)ViewState["LabelMode"];
                }
                return string.Empty;
            }
            set
            {
                ViewState["LabelMode"] = value;
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

        #endregion

        #region Event

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (Session["LoginInfo"] == null)
            {
                Response.Redirect("~/Login/FrmLogin.aspx");
            }

            //Get logo
            using (DB db = new DB())
            {
                SettingService dbSer = new SettingService(db);
                var setting =dbSer.GetData();
                if(setting != null)
                {
                    var logo = setting.Logo1;
                    var fullPath = Server.MapPath(string.Format("../Logo/{0}", logo));
                    if (System.IO.File.Exists(fullPath))
                    {
                        this.ImageCtrl = string.Format("<img id='imgLogo' src='../Logo/{0}' Height='40' Width='100'>", logo);
                        return;
                    }
                }
                this.ImageCtrl = string.Format("<img id='imgLogo' src='../Logo/no-image.png'  Height='40' Width='100'>");
            }
        }

        #endregion
    }
}
