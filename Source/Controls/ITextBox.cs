using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OMS.Controls
{
    /// <summary>
    /// Class ITextBox
    /// </summary>
    public class ITextBox : TextBox
    {
        /// <summary>
        /// Get IsEmpty
        /// </summary>
        public bool IsEmpty
        {
            get { return string.IsNullOrEmpty(this.Value.Trim()); }
        }

        /// <summary>
        /// Get or set Value
        /// </summary>
        public virtual string Value
        {
            //get { return Utilities.EditDataUtil.Decode(this.Text.Trim()); }
            //set { this.Text = Utilities.EditDataUtil.Decode(value); }

            get { return this.Text; }
            set { this.Text = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isreadOnly"></param>
        public virtual void SetReadOnly(bool isreadOnly)
        {
            if (isreadOnly)
            {
                this.Attributes.Add("readonly", "readonly");
            }
            else
            {
                this.Attributes.Remove("readonly");
            }
        }

        /// <summary>
        /// Create Viewstate Key
        /// </summary>
        /// <param name="property">property</param>
        /// <returns>Key</returns>
        protected string ViewStateKey(string property)
        {
            return string.Format("{0}", property);
        }

        /// <summary>
        /// Get Value
        /// </summary>
        /// <typeparam name="T">Type of value</typeparam>
        /// <param name="property">property</param>
        /// <returns></returns>
        protected T GetValueViewState<T>(string property)
        {
            var val = base.ViewState[this.ViewStateKey(property)];
            if (val == null)
            {
                return default(T);
            }
            return (T)val;
        }

        /// <summary>
        /// On PreRender
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            if (this.TextMode == TextBoxMode.MultiLine)
            {
                if (MaxLength > 0)
                {
                    this.Attributes.Add("maxLength", this.MaxLength.ToString());
                }

            }
            base.OnPreRender(e);
        }
    }
}