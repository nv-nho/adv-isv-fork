using System;
using System.Collections.Generic;
using System.Web.UI;
using OMS.Models;
using OMS.DAC;

namespace OMS.UserControls
{
    public delegate void ActionPagingHeaderClick(object sender, EventArgs e);

    public partial class PagingHeaderControl : System.Web.UI.UserControl
    {
        public event ActionPagingHeaderClick OnClick;

        public event ActionPagingHeaderClick OnPagingClick;

        #region Property

        /// <summary>
        /// selected value page size
        /// </summary>
        public int NumRowOnPage { get; set; }

        /// <summary>
        /// total row
        /// </summary>
        public int TotalRow { get; set; }

        /// <summary>
        /// row num form 
        /// </summary>
        public int RowNumFrom { get; set; }

        /// <summary>
        /// row num to
        /// </summary>
        public int RowNumTo { get; set; }

        /// <summary>
        /// is close form
        /// </summary>
        public Boolean IsCloseForm
        {
            get { return string.IsNullOrEmpty(this.hdIsCloseForm.Value) ? false : Boolean.Parse(this.hdIsCloseForm.Value); }
            set { this.hdIsCloseForm.Value = value.ToString(); }
        }

        /// <summary>
        /// is show color
        /// </summary>
        public Boolean IsShowColor
        {
            get { return string.IsNullOrEmpty(this.hdIsShowColor.Value) ? false : Boolean.Parse(this.hdIsShowColor.Value); }
            set { this.hdIsShowColor.Value = value.ToString(); }
        }

        /// <summary>
        /// type button
        /// </summary>
        public string AddClass
        {
            get { return string.IsNullOrEmpty(this.hdAddClass.Value) ? "btn-info" : this.hdAddClass.Value; }
            set { this.hdAddClass.Value = value; }
        }

        /// <summary>
        /// Current Page
        /// </summary>
        public int CurrentPage
        {
            get { return string.IsNullOrEmpty(this.hdCurrentPage.Value) ? 1 : int.Parse(this.hdCurrentPage.Value); }
            set { this.hdCurrentPage.Value = value.ToString(); }
        }

        /// <summary>
        /// Get or set WarningText
        /// </summary>
        public string WarningText
        {
            get
            {
                if (ViewState["WarningText"] != null)
                {
                    return (string)ViewState["WarningText"];
                }
                return string.Empty;
            }
            set
            {
                ViewState["WarningText"] = value;
            }
        }

        /// <summary>
        /// Get or set FinishText
        /// </summary>
        public string FinishText
        {
            get
            {
                if (ViewState["FinishText"] != null)
                {
                    return (string)ViewState["FinishText"];
                }
                return string.Empty;
            }
            set
            {
                ViewState["FinishText"] = value;
            }
        }

        /// <summary>
        /// Get or set InfoText
        /// </summary>
        public string InfoText
        {
            get
            {
                if (ViewState["InfoText"] != null)
                {
                    return (string)ViewState["InfoText"];
                }
                return string.Empty;
            }
            set
            {
                ViewState["InfoText"] = value;
            }
        }

        /// <summary>
        /// Get or set DangerText
        /// </summary>
        public string DangerText
        {
            get
            {
                if (ViewState["DangerText"] != null)
                {
                    return (string)ViewState["DangerText"];
                }
                return string.Empty;
            }
            set
            {
                ViewState["DangerText"] = value;
            }
        }

        /// <summary>
        /// Get or set SuccessText
        /// </summary>
        public string SuccessText
        {
            get
            {
                if (ViewState["SuccessText"] != null)
                {
                    return (string)ViewState["SuccessText"];
                }
                return string.Empty;
            }
            set
            {
                ViewState["SuccessText"] = value;
            }
        }

        /// <summary>
        /// Get or set PrimaryText
        /// </summary>
        public string PrimaryText
        {
            get
            {
                if (ViewState["PrimaryText"] != null)
                {
                    return (string)ViewState["PrimaryText"];
                }
                return string.Empty;
            }
            set
            {
                ViewState["PrimaryText"] = value;
            }
        }

        /// <summary>
        /// Get or set LightGrayText
        /// </summary>
        public string LightGrayText
        {
            get
            {
                if (ViewState["LightGrayText"] != null)
                {
                    return (string)ViewState["LightGrayText"];
                }
                return string.Empty;
            }
            set
            {
                ViewState["LightGrayText"] = value;
            }
        }

        #endregion

        /// <summary>
        /// Page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // get data from Config table 
                IList<DropDownModel> lstItem;
                using (DB db = new DB())
                {
                    Config_HService service = new Config_HService(db);
                    lstItem = service.GetDataForDropDownList(M_Config_H.CONFIG_CD_PAGING);
                }

                DropDownModel drop = new DropDownModel();
                this.dlTotalRowOnPage.DataValueField = "Value";
                this.dlTotalRowOnPage.DataTextField = "DisplayName";
                this.dlTotalRowOnPage.DataSource = lstItem;
                this.dlTotalRowOnPage.SelectedValue = this.NumRowOnPage.ToString();
                this.dlTotalRowOnPage.DataBind();
            }
            else
            {
                NumRowOnPage = int.Parse(dlTotalRowOnPage.SelectedValue.ToString());
            }
        }

        /// <summary>
        /// Selected Index Changed page size
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dlTotalRowOnPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OnClick != null)
            {
                NumRowOnPage = int.Parse(dlTotalRowOnPage.SelectedValue.ToString());
                OnClick(sender, e);
            }
        }

        /// <summary>
        /// next paging || prev paging
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Paging_OnClick(object sender, EventArgs e)
        {
            if (OnPagingClick != null)
            {
                OnPagingClick(sender, e);
            }
        }

        /// <summary>
        /// PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            string enableClass = "btn btn-default btn-sm";
            string disableClass = "btn btn-default btn-sm disabled";

            if (this.IsCloseForm && this.TotalRow == 0)
            {
                this.btn_Prev.Attributes.Add("class", disableClass);
                this.btn_Next.Attributes.Add("class", disableClass);

                this.dlTotalRowOnPage.Enabled = false;
                this.dlTotalRowOnPage.CssClass += "form-control input-sm";
            }
            else
            {
                this.btn_Prev.Attributes.Add("class", enableClass);
                this.btn_Next.Attributes.Add("class", enableClass);
                this.dlTotalRowOnPage.Enabled = true;

                if (RowNumFrom == 1)
                {
                    this.btn_Prev.Attributes.Add("class", disableClass);
                    this.btn_Prev.OnClientClick = "return false;";
                }
                else
                {
                    this.btn_Prev.Attributes.Add("class", enableClass);
                    this.btn_Prev.OnClientClick = string.Empty;
                }

                if (RowNumTo == TotalRow)
                {
                    this.btn_Next.Attributes.Add("class", disableClass);
                    this.btn_Next.OnClientClick = "return false;";
                }
                else
                {
                    this.btn_Next.Attributes.Add("class", enableClass);
                    this.btn_Next.OnClientClick = string.Empty;
                }
            }

            this.btn_Prev.CommandArgument = (this.CurrentPage - 1).ToString();
            this.btn_Next.CommandArgument = (this.CurrentPage + 1).ToString();
        }        
    }
}