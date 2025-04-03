using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace OMS.UserControls
{
    public delegate void ActionPagingFooterClick(object sender, EventArgs e);

    public partial class PagingFooterControl : System.Web.UI.UserControl
    {
        public event ActionPagingFooterClick OnClick;

        private int PageSizeOnPage = 10;

        /// <summary>
        /// Record Total
        /// </summary>
        public int TotalRow { get; set; }

        /// <summary>
        /// Number Record On Page
        /// </summary>
        public int NumberOnPage { get; set; }

        /// <summary>
        /// Current Page
        /// </summary>
        public int CurrentPage
        {
            get { return (int)ViewState["CurrentPage"]; }
            set { ViewState["CurrentPage"] = value; }
        }

        /// <summary>
        /// Event changed page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Paging_Click(object sender, EventArgs e)
        {
            if (OnClick != null)
            {
                OnClick(sender, e);                
            }                                
        }

         /// <summary>
        /// Page PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (this.TotalRow > 1)
            {
                int pageTotal = (int)Math.Ceiling((double)this.TotalRow / this.NumberOnPage);

                if (pageTotal > 1)
                {
                    int mod = this.CurrentPage % this.PageSizeOnPage;

                    int firstPage = 0;
                    int endPage = 0;

                    //First Page
                    if (mod < 7)
                    {
                        firstPage = this.CurrentPage - mod;
                        if (firstPage == 0)
                        {
                            firstPage = 1;
                        }
                        if (mod == 0)
                        {
                            firstPage = this.CurrentPage - 4;
                        }
                    }
                    else
                    {
                        firstPage = (this.CurrentPage - mod) + 5;
                    }

                    //End Page
                    endPage = firstPage + this.PageSizeOnPage;
                    if (endPage > pageTotal)
                    {
                        endPage = pageTotal + 1;
                    }

                    //First Page
                    if (endPage - firstPage < this.PageSizeOnPage)
                    {
                        firstPage = endPage - this.PageSizeOnPage;
                        if (firstPage <= 0)
                        {
                            firstPage = 1;
                        }
                    }

                    string prevPage = "1";
                    string nextPage = pageTotal.ToString();

                    if (this.CurrentPage == pageTotal)
                    {
                        nextPage = string.Empty;
                    }

                    if (this.CurrentPage == 1)
                    {
                        prevPage = string.Empty;
                    }
                    List<ListItem> pages = new List<ListItem>();

                    pages.Add(new ListItem("&laquo;", prevPage, !string.IsNullOrEmpty(prevPage)));
                    for (int i = firstPage; i < endPage; i++)
                    {
                        pages.Add(new ListItem(i.ToString(), i.ToString(), i != this.CurrentPage));
                    }
                    pages.Add(new ListItem("&raquo;", nextPage, !string.IsNullOrEmpty(nextPage)));

                    rptPager.DataSource = pages; 
                }
            }
            else
            {
                rptPager.DataSource = null;
            }
            rptPager.DataBind();
        }

        /// <summary>
        /// Event ItemDataBound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptPager_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ListItem item = (ListItem)e.Item.DataItem;
                HtmlGenericControl li = (HtmlGenericControl)e.Item.FindControl("liPage");
                LinkButton link = (LinkButton)e.Item.FindControl("linkPage");

                if (!item.Enabled)
                {
                    if (string.IsNullOrEmpty(item.Value))
                    {
                        li.Attributes.Add("class", "disabled");
                    }
                    else
                    {
                        li.Attributes.Add("class", "active");
                    }
                    link.Enabled = false;
                }                
            }
        }          
    }
}