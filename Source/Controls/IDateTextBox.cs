using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OMS.Utilities;

namespace OMS.Controls
{
    /// <summary>
    /// Input date TextBox
    /// -------------------------
    /// Author : ISV-PHUONG
    /// Date   : 2014/07/24
    /// Ver    : 0.0.0.1
    /// -------------------------
    /// </summary>
    public class IDateTextBox : ITextBox
    {
        #region Constructor
        public IDateTextBox()
            : base()
        {
            this.InitProperties();
            this.InitDefaultStyle();
        }

        #endregion

        #region Enum

        public string PickFormat
        {
            get
            {
                return this.GetValueViewState<string>("PickFormat");
            }
            set
            {
                base.ViewState[ViewStateKey("PickFormat")] = value;
            }
        }

        public bool PickDate
        {
            get
            {
                return this.GetValueViewState<bool>("PickDate");
            }
            set
            {
                base.ViewState[ViewStateKey("PickDate")] = value;
            }
        }

        public bool PickTime
        {
            get
            {
                return this.GetValueViewState<bool>("PickTime");
            }
            set
            {
                base.ViewState[ViewStateKey("PickTime")] = value;
            }
        }

        #endregion

        new public DateTime? Value
        {
            get
            {
                if (base.IsEmpty)
                {
                    return null;
                }
                try
                {
                    return OMS.Utilities.CommonUtil.ParseDate(base.Value, this.PickFormat.Replace("YYYY", "yyyy").Replace("DD", "dd"));
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
                    this.Text = value.Value.ToString(this.PickFormat.Replace("YYYY", "yyyy").Replace("DD", "dd"));
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
                base.CssClass += " input-date disable-ime";
            }
        }

        #region Override

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            this.MaxLength = this.PickFormat.Length;
            this.Attributes.Add("pick-time", this.PickTime.ToString());
            this.Attributes.Add("pick-date", this.PickDate.ToString());
            this.Attributes.Add("pick-format", this.PickFormat.ToString());

            base.Render(writer);
        }

        #endregion

        #region Method

        public void SetTimeValue(int hour, int minute)
        {
            this.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour, minute, 0);
        }

        public void SetTimeValue(int hour, int minute, int second)
        {
            this.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour, minute, second);
        }

        /// <summary>
        /// Khởi tạo giá trị mặc định cho các property
        /// </summary>
        private void InitProperties()
        {
            this.PickDate = true;
            this.PickTime = false;
            this.PickFormat = Constants.FMT_DATE_EN.ToUpper();
            this.MaxLength = this.PickFormat.Length;
        }

        /// <summary>
        /// constructor default value
        /// </summary>
        private void InitDefaultStyle()
        {
            this.Attributes.Add("Class", "input-date disable-ime");

        }
        #endregion


    }
}
