using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OMS.UserControls
{
    public delegate void ActionClick(object sender, EventArgs e);

    public partial class HeaderGridControl : System.Web.UI.UserControl
    {
        public event ActionClick OnSortClick;

        /// <summary>
        /// Sort Field
        /// </summary>
        public string SortField
        {
            get { return string.Format("{0}", ViewState["hdSortField"]); }
            set { ViewState["hdSortField"] = value; }
        }

        /// <summary>
        /// Sort Direc
        /// </summary>
        public string SortDirec
        {
            get { return string.Format("{0}", ViewState["hdSortDirec"]); }
            set { ViewState["hdSortDirec"] = value; }
        }

        /// <summary>
        /// item column Header
        /// </summary>
        public List<ColumnInfo> Columns { get; set; }

        /// <summary>
        /// Record Total
        /// </summary>
        public int TotalRow { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Sort_Click(object sender, EventArgs e)
        {
            string newSortField = (sender as LinkButton).CommandArgument;
            if (!this.SortField.Equals(newSortField))
            {
                this.SortDirec = "1";
                this.SortField = newSortField;
            }
            else
            {
                if (this.SortDirec.Equals("2"))
                {
                    this.SortDirec = "1";
                }
                else
                {
                    this.SortDirec = "2";
                }
            }

            if (OnSortClick != null)
            {
                OnSortClick(sender, e);
            }
        }

        /// <summary>
        /// Event Detail ItemDataBound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptHGrid_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ColumnInfo dataItem = (ColumnInfo)e.Item.DataItem;
                if (!dataItem.Sorting)
                {
                    LinkButton ctrl = (LinkButton)e.Item.FindControl("lnkSortCode");
                    DisableLinkButton(ctrl);
                }

                var th = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("th");
                if (!string.IsNullOrEmpty(dataItem.CssClass))
                {
                    th.Attributes.Add("class", dataItem.CssClass);
                }

                if (!string.IsNullOrEmpty(dataItem.Width))
                {
                    th.Attributes.Add("style", string.Format("width:{0}", dataItem.Width));
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indexField"></param>
        /// <returns></returns>
        private string SetIconSort(int indexField)
        {
            if (SortField.Equals(indexField.ToString()))
            {
                if (SortDirec.Equals("1"))
                {
                    return "&nbsp; <span class='glyphicon glyphicon-arrow-up'></span>";
                }
                else
                {
                    return "&nbsp; <span class='glyphicon glyphicon-arrow-down'></span>";
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (TotalRow == 0)
            {
                this.rptHGrid.DataSource = null;
            }
            else
            {
                List<ColumnInfo> items = new List<ColumnInfo>();
                for (int i = 0; i < this.Columns.Count; i++)
                {
                    ColumnInfo item = Columns[i];
                    if (item.Hidden)
                    {
                        continue;
                    }

                    if (item.Sorting)
                    {
                        item.Text = item.Text + SetIconSort(i + 1);
                    }

                    item.ColumnIndex = i + 1;
                    items.Add(item);
                }

                this.rptHGrid.DataSource = items;
            }

            this.rptHGrid.DataBind();
        }

        /// <summary>
        /// Disables the link button.
        /// </summary>
        /// <param name="linkButton">The LinkButton to be disabled.</param>
        public void DisableLinkButton(LinkButton linkButton)
        {
            linkButton.Attributes.Remove("href");
            linkButton.Attributes.CssStyle[HtmlTextWriterStyle.Color] = "gray";
            linkButton.Attributes.CssStyle[HtmlTextWriterStyle.Cursor] = "default";
            linkButton.Attributes.CssStyle[HtmlTextWriterStyle.TextDecoration] = "none";
            if (linkButton.Enabled != false)
            {
                linkButton.Enabled = false;
            }

            if (linkButton.OnClientClick != null)
            {
                linkButton.OnClientClick = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cssClass"></param>
        /// <param name="Width"></param>
        /// <param name="Hidden"></param>
        /// <param name="Sorting"></param>
        /// <param name="Text"></param>
        public void AddColumms(ColumnInfo item)
        {
            if (this.Columns == null)
            {
                this.Columns = new List<ColumnInfo>();
            }

            this.Columns.Add(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cssClass"></param>
        /// <param name="Width"></param>
        /// <param name="Hidden"></param>
        /// <param name="Sorting"></param>
        /// <param name="Text"></param>
        public void AddColumms(string[] items, string[] lstHideCols = null)
        {
            if (this.Columns != null && this.Columns.Count > 0)
            {
                return;
            }
            this.Columns = new List<ColumnInfo>();
            for (int i = 0; i < items.Length; i++)
            {
                ColumnInfo temp = new ColumnInfo();
                temp.Text = items[i];
                temp.ColumnIndex = i + 1;
                if (items[i].Equals("#") || string.IsNullOrEmpty(items[i]))
                {
                    temp.Sorting = false;
                }
                else
                {
                    temp.Sorting = true;
                }

                if (items[i].Contains("N#"))
                {
                    temp.CssClass = "text-right";
                    temp.Text = items[i].Substring(2);
                }

                if (lstHideCols != null)
                {
                    if (lstHideCols.Contains(temp.Text))
                    {
                        temp.Hidden = true;
                    }
                }

                this.Columns.Add(temp);
            }
        }
    }

    [Serializable]
    public partial class ColumnInfo
    {
        public int ColumnIndex { get; set; }
        public string CssClass { get; set; }
        public string Width { get; set; }
        public bool Hidden { get; set; }
        public bool Sorting { get; set; }
        public string Text { get; set; }
    }
}