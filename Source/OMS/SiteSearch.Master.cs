using System;

namespace OMS
{
    public partial class SiteSearch : System.Web.UI.MasterPage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (Session["LoginInfo"] == null)
            {
                Response.Redirect("~/Search/FrmPageDefault.aspx");
            }            
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}