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
    /// Author : ISV-PHUONG
    /// Date   : 2014/07/24
    /// Ver    : 0.0.0.1
    /// -------------------------
    /// </summary>
    public class ICodeTextBox : ITextBox
    {
        #region Enum

        public enum PatternType
        {
            /// <summary>
            /// 0 - 9
            /// </summary>
            Numeric = 1,

            /// <summary>
            ///  A - Z, 0 - 9
            /// </summary>
            AlphaNumeric = 2,

            /// <summary>
            /// A-Z, 0-> 9, /
            /// </summary>
            AlphaNumericFlash = 3,

            /// <summary>
            /// A-Z, 0-> 9, /, " "
            /// </summary>
            AlphaNumericFlashSpace = 4,

            /// <summary>
            ///  A - Z
            /// </summary>
            Alpha = 5
        }

        #endregion

        #region Constant
        #endregion

        #region Constructor
        #region Constructor
        public ICodeTextBox()
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
                base.CssClass += " " + this.Attributes["Class"];
            }
        }

        /// <summary>
        /// Code Type
        /// </summary>
        public PatternType CodeType
        {
            get
            {
                return this.GetValueViewState<PatternType>("CodeType");
            }
            set
            {
                base.ViewState[ViewStateKey("CodeType")] = value;
            }
        }

        /// <summary>
        /// Ajax Method 
        /// </summary>
        public string AjaxUrlMethod
        {
            get
            {
                return this.GetValueViewState<string>("AjaxUrlMethod");
            }
            set
            {
                base.ViewState[ViewStateKey("AjaxUrlMethod")] = value;
            }
        }

        /// <summary>
        /// CallBack Method 
        /// </summary>
        public string CallBackMethod
        {
            get
            {
                return this.GetValueViewState<string>("CallBackMethod");
            }
            set
            {
                base.ViewState[ViewStateKey("CallBackMethod")] = value;
            }
        }

        /// <summary>
        /// Params
        /// ----------------------------
        /// Params = "in1:txtCode,in2:txtName, ...."
        /// ----------------------------
        /// </summary>
        public string Params
        {
            get
            {
                return this.GetValueViewState<string>("Params");
            }
            set
            {
                base.ViewState[ViewStateKey("Params")] = value;
            }
        }

        /// <summary>
        /// Names Label
        /// ----------------------------
        /// LabelNames = "Label1, Label2, Label3, ....."
        /// ----------------------------
        /// </summary>
        public string LabelNames
        {
            get
            {
                return this.GetValueViewState<string>("LabelNames");
            }
            set
            {
                base.ViewState[ViewStateKey("LabelNames")] = value;
            }
        }

        /// <summary>
        /// Fill charactor
        /// </summary>
        public string FillChar
        {
            get
            {
                return this.GetValueViewState<string>("FillChar");
            }
            set
            {
                base.ViewState[ViewStateKey("FillChar")] = value;
            }
        }

        /// <summary>
        /// Alllow input char(s) 
        /// </summary>
        public string AllowChars
        {
            get
            {
                return this.GetValueViewState<string>("AllowChars");
            }
            set
            {
                base.ViewState[ViewStateKey("AllowChars")] = value;
            }
        }

        /// <summary>
        /// Server Button Search ID
        /// </summary>
        public string SearchButtonID
        {
            get
            {
                return this.GetValueViewState<string>("SearchButtonID");
            }
            set
            {
                base.ViewState[ViewStateKey("SearchButtonID")] = value;
            }
        }

        /// <summary>
        /// Button Search
        /// </summary>
        public HtmlButton SearchButton
        {
            get;
            set;
        }

        /// <summary>
        /// upper code when pressing
        /// </summary>
        public bool UpperCode
        {
            get
            {
                return this.GetValueViewState<bool>("UpperCode");
            }
            set
            {
                base.ViewState[ViewStateKey("UpperCode")] = value;
                var cssClass = this.Attributes["Class"];

                if (value == true)
                {
                    if (string.IsNullOrEmpty(cssClass))
                    {
                        cssClass = "input-code upper disable-ime";
                    }
                    else
                    {
                        var k = "input-code";
                        var idx = cssClass.IndexOf(k);
                        cssClass.Insert(idx + k.Length, " upper");
                    }
                }
                else
                {
                    cssClass = cssClass.Replace(" upper", "");
                }
                this.Attributes["Class"] = cssClass;
            }
        }

        #endregion

        #region Override

        public override void SetReadOnly(bool isreadOnly)
        {
            base.SetReadOnly(isreadOnly);
            if (this.SearchButton != null)
            {
                this.SearchButton.Disabled = isreadOnly;
                return;
            }
            if (!string.IsNullOrEmpty(this.SearchButtonID))
            {
                var searchButton = (HtmlButton)this.Parent.FindControl(this.SearchButtonID);
                if (searchButton != null)
                {
                    searchButton.Disabled = isreadOnly;
                }
            }
        }
       

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            var codeType = string.Format("{0}", (int)this.CodeType);
            this.Attributes.Add("code-type", codeType);

            var fillChar = this.FillChar;
            if (!string.IsNullOrEmpty(fillChar))
            {
                this.Attributes.Add("fill-char", this.FillChar);
            }

            var allowChars = this.AllowChars;
            this.Attributes.Add("allow-char", allowChars);

            var ajaxUrl = this.AjaxUrlMethod;
            var names = this.LabelNames;
            if (!string.IsNullOrEmpty(names))
            {
                this.Attributes.Add("ajax-url-method", ajaxUrl);

                this.Attributes.Add("label-name", names);
            }
            var parameters = this.Params;
            if (!string.IsNullOrEmpty(parameters))
            {
                this.Attributes.Add("params", parameters);
            }

            var callBack = this.CallBackMethod;
            if (!string.IsNullOrEmpty(callBack))
            {
                this.Attributes.Add("call-back", callBack);
            }

            if (this.SearchButton != null)
            {
                this.Attributes.Add("search-button", this.SearchButton.ClientID);
            }else
            {
                if (!string.IsNullOrEmpty(this.SearchButtonID))
                {
                    this.Attributes.Add("search-button", this.SearchButtonID);
                }
            }

            base.Render(writer);
        }

        #endregion

        #region Method
        /// <summary>
        /// Khởi tạo giá trị mặc định cho các property
        /// </summary>
        private void InitProperties()
        {
            this.UpperCode = true;
            this.MaxLength = 10;
            this.CodeType = PatternType.AlphaNumeric;
            this.AjaxUrlMethod = null;
            this.LabelNames = null;
        }

        /// <summary>
        /// Khởi tạo giá trị css mặc định cho numbertextbox
        /// </summary>
        private void InitDefaultStyle()
        {
            //if (this.UpperCode)
            //{
            //    this.Attributes.Add("Class", "input-code upper disable-ime");
            //}
            //else
            //{
            //    this.Attributes.Add("Class", "input-code disable-ime");
            //}
            
        }
        #endregion
    }
}
