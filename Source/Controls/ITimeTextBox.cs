using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OMS.Utilities;

namespace OMS.Controls
{   
    public class ITimeTextBox : ITextBox
    {
        #region Constructor
        public ITimeTextBox()
            : base()
        {
            this.InitProperties();
            this.InitDefaultStyle();
        }

        #endregion        

        new public string Value
        {
            get
            {
                if (base.IsEmpty)
                {
                    return null;
                }
                try
                {                    
                    return base.Value;
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                if (value != null)
                {                    
                    this.Text = value;
                }
                else
                {
                    this.Text = string.Empty;
                }

            }
        }

        /// <summary>
        /// Css Class
        /// </summary>
        public override string CssClass
        {
            get
            {
                return base.CssClass;
            }
            set
            {
                base.CssClass = value;
                base.CssClass += " input-time disable-ime";
            }
        }

        #region Override

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {               
            this.MaxLength = 5;            

            base.Render(writer);
        }

        #endregion

        #region Method
        /// <summary>
        /// Khởi tạo giá trị mặc định cho các property
        /// </summary>
        private void InitProperties()
        {                   
            this.MaxLength = 5;
        }

        /// <summary>
        /// constructor default value
        /// </summary>
        private void InitDefaultStyle()
        {
            this.Attributes.Add("Class", "input-time disable-ime");

        }
        #endregion


    }
}
