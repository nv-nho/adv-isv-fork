using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMS.Controls
{
    /// <summary>
    /// Password TextBox
    /// -------------------------
    /// Author : ISV-PHUONG
    /// Date   : 2014/09/29
    /// Ver    : 0.0.0.1
    /// -------------------------
    /// </summary>
    public class IPasswordBox : ITextBox
    {
        #region Constructor
        public IPasswordBox()
            : base()
        {
            this.InitProperties();
            this.InitDefaultStyle();
            this.TextMode = System.Web.UI.WebControls.TextBoxMode.Password;
        }

        #endregion

        #region Override

        public override string Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                var pwrd = value;
                value = "".PadLeft(pwrd.Length, '●');
                this.Password = pwrd;
                this.Attributes.Add("value", value);
                base.Value = value;
            }
        }
        public string Password
        {
            get
            {
                return this.GetValueViewState<string>("Password");
            }
            private set
            {
                base.ViewState[ViewStateKey("Password")] = value;
            }
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            //var pwrd = base.Value;
            //base.Value = "".PadLeft(pwrd.Length, '●');
            //this.Password = pwrd;
            //this.Attributes.Add("value", base.Value);

            base.Render(writer);
        }

        #endregion

        #region Method
        /// <summary>
        /// Khởi tạo giá trị mặc định cho các property
        /// </summary>
        private void InitProperties()
        {
            this.MaxLength = 32;
        }

        /// <summary>
        /// Khởi tạo giá trị css mặc định cho numbertextbox
        /// </summary>
        private void InitDefaultStyle()
        {
            this.Attributes.Add("Class", "disable-ime");

        }
        #endregion
    }
}
