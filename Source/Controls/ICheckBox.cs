using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;

namespace OMS.Controls
{
    /// <summary>
    /// Input code TextBox
    /// -------------------------
    /// Author : ISV-THUY
    /// Date   : 2017.12.22
    /// Ver    : 0.0.0.1
    /// -------------------------
    /// </summary>
    public class ICheckBox : HtmlInputCheckBox
    {
        #region Enum

        #endregion

        #region Constant
        #endregion

        #region Constructor
        #region Constructor
        public ICheckBox()
            : base()
        {
            this.InitProperties();
            this.InitDefaultStyle();
        }

        #endregion
        #endregion

        #region Variable
        #endregion

        #region Properties
        #endregion

        #region Override

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            string clsReadOnly = "bootstrap-switch-readonly";
            string clsChecked = "";
            string wMargin = "";
            string wSwitch = "";
            string wHandle = "";
            string wContainer = "";
            string data_size = !string.IsNullOrEmpty(this.GetAttribute("data-size")) ? this.GetAttribute("data-size") : "normal";
            string data_on_text = !string.IsNullOrEmpty(this.GetAttribute("data-on-text")) ? this.GetAttribute("data-on-text") : "on";
            string data_off_text = !string.IsNullOrEmpty(this.GetAttribute("data-off-text")) ? this.GetAttribute("data-off-text") : "off";
            string data_on_color = !string.IsNullOrEmpty(this.GetAttribute("data-on-color")) ? this.GetAttribute("data-on-color") : "success";
            string data_off_color = !string.IsNullOrEmpty(this.GetAttribute("data-off-color")) ? this.GetAttribute("data-off-color") : "danger";

            if (data_size == "normal")
            {
                wSwitch = "86px;";
                wHandle = "42px;";
                wContainer = "126px;";
            }

            if (data_size == "mini")
            {
                wSwitch = "54px;";
                wHandle = "26px;";
                wContainer = "78px;";
            }

            if (this.Checked)
            {
                clsChecked = "bootstrap-switch-on";
                wMargin = "0px;";
            }
            else
            {
                clsChecked = "bootstrap-switch-off";
                if (data_size == "normal")
                {
                    wMargin = "-42px;";
                }

                if (data_size == "mini")
                {
                    wMargin = "-26px;";
                }
            }

            writer.Write("<div class='switch-tmp bootstrap-switch-" + data_size + " bootstrap-switch-id-" + this.ClientID + " bootstrap-switch bootstrap-switch-wrapper " + clsChecked + " " + clsReadOnly + " bootstrap-switch-animate' style='width: " + wSwitch + "'>");
            writer.Write("  <div class='bootstrap-switch-container' style='width: " + wContainer + " margin-left: " + wMargin + "'>");
            writer.Write("      <span class='bootstrap-switch-handle-on bootstrap-switch-" + data_on_color + "' style='width: " + wHandle + "'>" + data_on_text + "</span>");
            writer.Write("      <span class='bootstrap-switch-label' style='width: " + wHandle + "'>&nbsp;</span>");
            writer.Write("      <span class='bootstrap-switch-handle-off bootstrap-switch-" + data_off_color + "' style='width: " + wHandle + "'>" + data_off_text + "</span>");
            writer.Write("  </div>");
            writer.Write("</div>");

            this.Attributes.Add("hidden", "true");
            this.Attributes.Add("data-on-text", data_on_text);
            this.Attributes.Add("data-off-text", data_off_text);
            this.Attributes.Add("data-on-color", data_on_color);
            this.Attributes.Add("data-off-color", data_off_color);

            base.Render(writer);
        }

        #endregion

        #region Method
        /// <summary>
        /// Khởi tạo giá trị mặc định cho các property
        /// </summary>
        private void InitProperties()
        {
        }

        /// <summary>
        /// Khởi tạo giá trị css mặc định cho checkbox
        /// </summary>
        private void InitDefaultStyle()
        {
        }
        #endregion
    }
}
