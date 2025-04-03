using System;

namespace OMS.FrmErrorPages
{
    public partial class Oops : FrmBase
    {
        /// <summary>
        /// Init page
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //Set Title
            base.FormTitle = "Error";
        }

        public string ErrorMessage { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.IsPostBack) return;
            var ex = System.Web.HttpContext.Current.Cache["LST_EX"] as Exception;
            if (ex != null)
            {
                this.ErrorMessage = ex.ToString();
            }
        }
    }
}