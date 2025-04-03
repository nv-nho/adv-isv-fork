using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using OMS.DAC;
using OMS.Models;
using OMS.Utilities;
using System.Xml;
using System.Data;
using System.Linq;
using System.Web.UI.HtmlControls;
using OMS.Controls;
using System.Data.SqlClient;

namespace OMS.Master
{
    /// <summary>
    /// Cost Detail
    /// </summary>
    public partial class FrmCostDetail : FrmBaseDetail
    {
        #region Variable

        /// <summary>
        /// Control id will be focused
        /// </summary>
        public string focusControlsID = "";

        //private int PageSizeOnPage = 10;
        #endregion

        #region Constants
        /// <summary>
        /// Page list direction
        /// </summary>
        public const string URL_LIST = "~/Master/FrmCostList.aspx";

        /// <summary>
        /// Date default
        /// </summary>
        public const string DEFAULT_DATE = "01/01/1900";

        /// <summary>
        /// Max of year
        /// </summary>
        public const int MAX_YEAR = 9999;

        /// <summary>
        /// Max of month
        /// </summary>
        public const int MAX_MONTH = 12;

        /// <summary>
        /// Max of day
        /// </summary>
        public const int MAX_DAY = 31;

        /// <summary>
        /// Min of year
        /// </summary>
        public const int MIN_YEAR = 1900;

        /// <summary>
        /// Min of month
        /// </summary>
        public const int MIN_MONTH = 1;

        /// <summary>
        /// Min of day
        /// </summary>
        public const int MIN_DAY = 1;

        #endregion

        #region Property
        /// <summary>
        /// Get or set ID
        /// </summary>
        public int CostID
        {
            get { return (int)base.ViewState["ID"]; }
            set { base.ViewState["ID"] = value; }
        }

        /// <summary>
        /// Get or set OldUpdateDate
        /// </summary>
        public DateTime OldUpdateDate
        {
            get { return (DateTime)base.ViewState["OldUpdateDate"]; }
            set { base.ViewState["OldUpdateDate"] = value; }
        }

        /// <summary>
        /// DetailLists
        /// </summary>
        public IList<T_Cost_D> DetailLists
        {
            get { return this.GetValueViewState<IList<T_Cost_D>>("CostLists"); }
            set { ViewState["CostLists"] = value; }
        }

        /// <summary>
        /// CurrenIndex
        /// </summary>
        public int CurrenIndexPage
        {
            get { return this.GetValueViewState<int>("CurrenIndexPage"); }
            set { ViewState["CurrenIndexPage"] = value; }
        }

        /// <summary>
        /// FileName
        /// </summary>
        public string FileName
        {
            get { return this.GetValueViewState<string>("FileName"); }
            set { ViewState["FileName"] = value; }
        }
        #endregion

        #region Event
        /// <summary>
        /// Event Init
        /// </summary>
        /// <param name="e">EventArgs</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //Set Title
            base.FormTitle = "原価登録";
            base.FormSubTitle = "詳細";

            // Paging footer
            this.PagingFooter.OnClick += this.Paging_Click;

            //Init Event
            LinkButton btnYes = (LinkButton)this.Master.FindControl("btnYes");
            btnYes.Click += new EventHandler(btnProcessData);
            LinkButton btnNo = (LinkButton)this.Master.FindControl("btnNo");
            btnNo.Click += new EventHandler(btnShowData);
        }

        /// <summary>
        /// Page Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            
            base.SetAuthority(FormId.Cost);
            if (!this._authority.IsMasterView)
            {
                Response.Redirect("~/Menu/FrmMasterMenu.aspx");
            }

            if (!this.IsPostBack)
            {
                //Set default value
                this.CurrenIndexPage = 1;

                if (base.PreviousPage != null)
                {
                    //Save condition of previous page
                    base.ViewState["Condition"] = base.PreviousPageViewState["Condition"];

                    //Check mode
                    if (base.PreviousPageViewState["ID"] == null)
                    {
                        //Set mode
                        this.ProcessMode(Mode.Insert);

                        //Init detail list
                        this.InitDetailList();
                    }
                    else
                    {
                        //Get Cost data by ID
                        this.CostID = int.Parse(base.PreviousPageViewState["ID"].ToString());
                        T_Cost_H Cost = this.GetCost(this.CostID);

                        if (Cost != null)
                        {
                            //Show data
                            this.ShowHeaderData(Cost);

                            //Set Mode
                            this.ProcessMode(Mode.View);
                        }
                        else
                        {
                            Server.Transfer(URL_LIST);
                        }
                    }
                }
                else
                {
                    Server.Transfer(URL_LIST);
                }
            }

            //Set init
            this.Success = false;
        }

        /// <summary>
        /// Process Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnProcessData(object sender, EventArgs e)
        {
            bool ret;
            T_Cost_H cost = null;

            //Check Mode
            switch (this.Mode)
            {
                case Utilities.Mode.Insert:
                case Utilities.Mode.Copy:
                    //Insert Data
                    int newID = 0;
                    ret = this.InsertData(ref newID);
                    if (ret)
                    {
                        //Get cost data by ID
                        cost = this.GetCost(newID);
                    }
                    break;

                default:

                    //Update Data
                    ret = this.UpdateData();
                    if (ret)
                    {
                        //Get cost data by ID
                        cost = this.GetCost(this.CostID);
                    }

                    break;
            }

            if (ret)
            {
                //Show data
                this.ShowHeaderData(cost);

                //Set Mode
                this.ProcessMode(Mode.View);

                //Set Success
                this.Success = true;
            }
        }

        /// <summary>
        /// Show Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShowData(object sender, EventArgs e)
        {
            //Get cost header by id
            T_Cost_H cost = this.GetCost(this.CostID);
            if (cost != null)
            {
                //Show data
                this.ShowHeaderData(cost);

                //Set Mode
                this.ProcessMode(Mode.View);
            }
            else
            {
                Server.Transfer(URL_LIST);
            }
        }

        /// <summary>
        /// Event Button AddRow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddRow_Click(object sender, EventArgs e)
        {
            //Get new list from screen
            var listDetail = this.GetDetailList();

            if (listDetail != null && listDetail.Count != 0)
            {
                listDetail.Insert(0, new T_Cost_D() { EffectDate = null, ExpireDate = null, CostAmount = null });
            }

            this.DetailLists = listDetail;

            this.CurrenIndexPage = 1;

            // Paging footer
            this.SetPaging(listDetail);

            //Set focus
            IDateTextBox dtEffectDate = (IDateTextBox)this.rptListCost.Items[0].FindControl("dtEffectDate");
            this.focusControlsID = dtEffectDate.ClientID;
        }

        /// <summary>
        /// Event Button RemoveRow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRemoveRow_Click(object sender, EventArgs e)
        {
            //Get new list from screen
            var listDetail = this.GetDetailList();

            //Get list index remove item
            List<int> listDel = new List<int>();
            for (int i = listDetail.Count - 1; i >= 0; i--)
            {
                if (listDetail[i].DelFlag)
                {
                    listDel.Add(i);
                }

                listDetail[i].DelFlag = false;
            }

            //Remove row
            foreach (var item in listDel)
            {

                //Remove row
                listDetail.RemoveAt(item);
            }

            this.ProcessEffectDate(listDetail);

            //Process list view
            this.DetailLists = listDetail;

            // Paging footer
            this.SetPaging(listDetail);
        }

        /// <summary>
        /// Edit Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            //Get cost data by ID
            T_Cost_H cost = this.GetCost(this.CostID);

            //Check cost
            if (cost != null)
            {
                this.CurrenIndexPage = 1;

                //Show data
                this.ShowHeaderData(cost);

                //Set Mode
                this.ProcessMode(Mode.Update);
            }
            else
            {
                Server.Transfer(URL_LIST);
            }
        }

        /// <summary>
        /// Event Button Update Submit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            var isError = false;
            isError = this.CheckInputDetail(false, true, true) ? true : isError;

            //Check input
            if (isError)
            {
                return;
            }

            this.CurrenIndexPage = 1;

            // Paging footer
            this.SetPaging(this.DetailLists);

            //Show question update
            base.ShowQuestionMessage(M_Message.MSG_QUESTION_UPDATE, Models.DefaultButton.Yes);
        }

        /// <summary>
        /// Event Button Back
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (this.Mode == Mode.View || this.Mode == Mode.Insert)
            {
                Server.Transfer(URL_LIST);
            }
            else if (this.Mode == Mode.Update)
            {
                //Get cost data by ID
                T_Cost_H cost = this.GetCost(this.CostID);

                //Check cost
                if (cost != null)
                {
                    this.CurrenIndexPage = 1;

                    //Show data
                    this.ShowHeaderData(cost);

                    //Set Mode
                    this.ProcessMode(Mode.View);
                }
                else
                {
                    Server.Transfer(URL_LIST);
                }
            }
        }

        /// <summary>
        /// Event changed page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Paging_Click(object sender, EventArgs e)
        {
            if (sender != null)
            {
                if (this.CheckInputDetail(true, false, false))
                {
                    return;
                }

                this.CurrenIndexPage = int.Parse((sender as LinkButton).CommandArgument);
                //this.InitPagingList(this.DetailLists);

                // Paging footer
                this.SetPaging(this.DetailLists);

                //this.PagingFooter.CurrentPage = this.CurrenIndexPage;
                //this.PagingFooter.NumberOnPage = Constant.DEFAULT_NUMBER_ROW;
                //this.PagingFooter.TotalRow = this.DetailLists.Count;

                //this.SetDataSourceWithPaging(this.DetailLists);
            }
        }
        #endregion

        #region Method
        /// <summary>
        /// Process mode
        /// </summary>
        /// <param name="mode">Mode</param>
        private void ProcessMode(Mode mode)
        {
            //Set mode
            this.Mode = mode;
            base.DisabledLink(this.btnEdit, !base._authority.IsMasterEdit);
            //Lock control
            this.txtCostName.ReadOnly = true;
        }

        /// <summary>
        /// Show header data on form
        /// </summary>
        /// <param name="cost">T_Cost_H</param>
        private void ShowHeaderData(T_Cost_H cost)
        {
            //Display header
            this.CostID = cost.ID;
            this.txtCostName.Value = cost.CostName;

            this.OldUpdateDate = cost.UpdateDate;

            //Display detail
            this.ShowDetailData(cost.ID);
        }

        /// <summary>
        /// Show detail data by header id
        /// </summary>
        /// <param name="headerID">Header ID</param>
        private void ShowDetailData(int headerID)
        {
            using (DB db = new DB())
            {
                Cost_DService costDService = new Cost_DService(db);

                //Get cost
                IList<T_Cost_D> listDetail = costDService.GetByListByHeaderID(headerID);
                if (listDetail != null)
                {
                    if (listDetail.Count != 0)
                    {
                        this.DetailLists = listDetail;
                        //this.InitPagingList(listDetail);

                        // Paging footer
                        this.SetPaging(listDetail);

                        //this.PagingFooter.CurrentPage = this.CurrenIndexPage;
                        //this.PagingFooter.NumberOnPage = Constant.DEFAULT_NUMBER_ROW;
                        //this.PagingFooter.TotalRow = listDetail.Count;

                        //this.SetDataSourceWithPaging(listDetail);
                    }
                }
            }
        }

        /// <summary>
        /// Set Paging
        /// </summary>
        /// <param name="listDetail"></param>
        private void SetPaging(IList<T_Cost_D> listDetail)
        {
            // Paging footer
            this.PagingFooter.CurrentPage = this.CurrenIndexPage;
            this.PagingFooter.NumberOnPage = Constant.DEFAULT_NUMBER_ROW;
            this.PagingFooter.TotalRow = listDetail.Count;

            //Refresh data
            this.SetDataSourceWithPaging(listDetail);
        }

        /// <summary>
        /// Get header from screen
        /// </summary>
        /// <param name="header">T_Cost_H</param>
        private void GetHeader(T_Cost_H header)
        {
            header.CreateUID = this.LoginInfo.User.ID;
            header.UpdateUID = this.LoginInfo.User.ID;
            if (this.Mode == Mode.Update)
            {
                header.UpdateDate = this.OldUpdateDate;
            }
        }

        /// <summary>
        /// Set data source with paging
        /// </summary>
        /// <param name="listDetail"></param>
        private void SetDataSourceWithPaging(IList<T_Cost_D> listDetail)
        {
            var newListPaging = listDetail.Skip(Constant.DEFAULT_NUMBER_ROW * (this.CurrenIndexPage - 1)).Take(Constant.DEFAULT_NUMBER_ROW).ToList();

            this.rptListCost.DataSource = newListPaging;
            this.rptListCost.DataBind();
        }

        /// <summary>
        /// Init Combobox
        /// </summary>
        private void InitCombobox(DropDownList ddl)
        {
            // init combox 
            ddl.DataSource = this.GetDataForDropdownList(M_Config_H.CONFIG_CD_INVALID_TYPE);
            ddl.DataValueField = "Value";
            ddl.DataTextField = "DisplayName";
            ddl.DataBind();
            //ddl.SelectedValue = this.hdInValideDefault.Value;
        }

        /// <summary>
        /// Get detail list from screen
        /// </summary>
        /// <param name="isRemoveEmpty"></param>
        /// <returns>List cost detail</returns>
        private IList<T_Cost_D> GetDetailList(bool isRemoveEmpty = false)
        {
            IList<T_Cost_D> results = this.DetailLists;

            foreach (RepeaterItem item in this.rptListCost.Items)
            {
                HtmlInputCheckBox chkDelFlg = (HtmlInputCheckBox)item.FindControl("deleteFlag");
                IDateTextBox dtEffectDate = (IDateTextBox)item.FindControl("dtEffectDate");
                IDateTextBox dtExpireDate = (IDateTextBox)item.FindControl("dtExpireDate");
                INumberTextBox inumCostAmount = (INumberTextBox)item.FindControl("inumCostAmount");

                var addItem = results[(this.CurrenIndexPage - 1) * Constant.DEFAULT_NUMBER_ROW + item.ItemIndex];

                //Delete flag
                addItem.DelFlag = (chkDelFlg.Checked) ? true : false;

                //Effect date
                addItem.EffectDate = dtEffectDate.Value;

                //Exchange rate
                addItem.CostAmount = inumCostAmount.Value;
            }

            if (isRemoveEmpty)
            {
                return results.Where(d => (d.EffectDate.HasValue || d.CostAmount.HasValue)).ToList();
            }
            else
            {
                return results;
            }
        }

        /// <summary>
        /// Check Duplicate
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <param name="listData"></param>
        /// <returns></returns>
        private bool CheckDuplicate(int index, DateTime value, IList<T_Cost_D> listData)
        {
            bool ret = false;
            for (int i = index + 1; i < listData.Count; i++)
            {
                var item = listData[i];
                if (!item.EffectDate.HasValue)
                {
                    continue;
                }
                if (item.EffectDate.Value.CompareTo(value) == 0)
                {
                    ret = true;
                    break;
                }
            }
            return ret;
        }

        /// <summary>
        /// Check input
        /// </summary>
        /// <param name="isRequiredInput"></param>
        /// <param name="isCheckDuplicate"></param>
        /// <param name="isProcessDate"></param>
        /// <returns>Valid:true, Invalid:false</returns>
        private bool CheckInputDetail(bool isRequiredInput, bool isCheckDuplicate, bool isProcessDate)
        {
            //IDictionary<DateTime, int> lstDup = new Dictionary<DateTime, int>();

            var firstIndex = -1;

            var listDetail = this.GetDetailList();

            for (int i = 0; i < listDetail.Count; i++)
            {
                var index = i % Constant.DEFAULT_NUMBER_ROW;

                var item = listDetail[i];

                if (!item.EffectDate.HasValue && !item.CostAmount.HasValue)
                {
                    continue;
                }

                if (isRequiredInput && !item.EffectDate.HasValue)
                {
                    base.SetMessage(string.Format("dtEffectDate_{0}", index), M_Message.MSG_REQUIRE_GRID, "開始日", i + 1);
                }
                else
                {
                    if (!item.EffectDate.HasValue && !item.CostAmount.HasValue)
                    {
                        continue;
                    }

                    if (!item.EffectDate.HasValue)
                    {
                        base.SetMessage(string.Format("dtEffectDate_{0}", index), M_Message.MSG_REQUIRE_GRID, "開始日", i + 1);
                    }
                    else if (isCheckDuplicate && this.CheckDuplicate(i, item.EffectDate.Value, listDetail))
                    {
                        base.SetMessage(string.Format("dtEffectDate_{0}", index), M_Message.MSG_DUPLICATE_GRID, "開始日", i + 1);
                    }
                    else
                    {
                        DateTime minDate = new DateTime(MIN_YEAR, MIN_MONTH, MIN_DAY);
                        DateTime maxDate = new DateTime(MAX_YEAR, MAX_MONTH, MAX_DAY);

                        if (index != listDetail.Count - 1 && item.EffectDate.Value.CompareTo(minDate) < 0)
                        {
                            base.SetMessage(string.Format("dtEffectDate_{0}", index), M_Message.MSG_GREATER_THAN_EQUAL_GRID, "開始日", minDate.ToString("dd/MM/yyyy"), i + 1);
                        }
                        else if (index != listDetail.Count - 1 && item.EffectDate.Value.CompareTo(maxDate) > 0)
                        {
                            base.SetMessage(string.Format("dtEffectDate_{0}", index), M_Message.MSG_LESS_THAN_EQUAL_GRID, "開始日", maxDate.ToString("dd/MM/yyyy"), i + 1);
                        }
                        else
                        {
                            if (!item.CostAmount.HasValue)
                            {
                                base.SetMessage(string.Format("inumCostAmount_{0}", index), M_Message.MSG_REQUIRE_GRID, "原価", i + 1);
                            }
                            else if (item.CostAmount.Value.CompareTo(T_Cost_D.COST_AMOUNT_MAX_VALUE) > 0) //Cost amount less than max value check
                            {
                                base.SetMessage(string.Format("inumCostAmount_{0}", index), M_Message.MSG_LESS_THAN_EQUAL_GRID, "原価", T_Cost_D.COST_AMOUNT_MAX_VALUE, i + 1);
                            }
                            else if (item.CostAmount.Value == 0)
                            {
                                base.SetMessage(string.Format("inumCostAmount_{0}", index), M_Message.MSG_GREATER_THAN_GRID, "原価", 0, i + 1);
                            }
                        }
                    }
                }

                if (base.HaveError && firstIndex == -1)
                {
                    firstIndex = i + 1;
                }
            }

            if (!base.HaveError)
            {
                if (isProcessDate)
                {
                    this.SortList(listDetail);
                    this.ProcessEffectDate(listDetail);
                }
            }
            else
            {
                this.CurrenIndexPage = firstIndex / Constant.DEFAULT_NUMBER_ROW + (firstIndex % Constant.DEFAULT_NUMBER_ROW == 0 ? 0 : 1);
            }

            this.SetPaging(listDetail);

            //Check error
            return base.HaveError;
        }

        /// <summary>
        /// SortList
        /// </summary>
        /// <param name="listSort"></param>
        private void SortList(IList<T_Cost_D> listSort)
        {
            if (listSort.Count == 3)
            {
                if (listSort[1].EffectDate == null)
                {
                    return;
                }
                if (listSort[0].EffectDate < listSort[1].EffectDate)
                {
                    var temp = listSort[0].Clone();

                    listSort[0].DelFlag = listSort[1].DelFlag;
                    listSort[0].EffectDate = listSort[1].EffectDate;
                    listSort[0].CostAmount = listSort[1].CostAmount;

                    listSort[1].DelFlag = temp.DelFlag;
                    listSort[1].EffectDate = temp.EffectDate;
                    listSort[1].CostAmount = temp.CostAmount;
                }
            }
            else
            {
                for (int i = 0; i < listSort.Count - 2; i++)
                {
                    var itemBefore = listSort[i];
                    if (!itemBefore.EffectDate.HasValue)
                    {
                        continue;
                    }
                    for (int k = i + 1; k < listSort.Count - 1; k++)
                    {
                        var itemAfter = listSort[k];
                        if (!itemAfter.EffectDate.HasValue)
                        {
                            continue;
                        }

                        if (itemBefore.EffectDate.Value < itemAfter.EffectDate.Value)
                        {
                            var temp = itemBefore.Clone();

                            itemBefore.DelFlag = itemAfter.DelFlag;
                            itemBefore.EffectDate = itemAfter.EffectDate;
                            itemBefore.CostAmount = itemAfter.CostAmount;

                            itemAfter.DelFlag = temp.DelFlag;
                            itemAfter.EffectDate = temp.EffectDate;
                            itemAfter.CostAmount = temp.CostAmount;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// GetDataForDropdownList
        /// </summary>
        /// <param name="configCD"></param>
        /// <returns></returns>
        private IList<DropDownModel> GetDataForDropdownList(string configCD)
        {
            using (DB db = new DB())
            {
                Config_HService configSer = new Config_HService(db);
                return configSer.GetDataForDropDownList(configCD);

            }
        }

        /// <summary>
        /// GetDefaultValueForDropdownList
        /// </summary>
        /// <param name="configCD"></param>
        /// <returns></returns>
        private string GetDefaultValueForDropdownList(string configCD)
        {
            using (DB db = new DB())
            {
                Config_HService configSer = new Config_HService(db);
                return configSer.GetDefaultValueDrop(configCD);
            }
        }

        /// <summary>
        /// Get Cost header by id
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        private T_Cost_H GetCost(int id)
        {
            using (DB db = new DB())
            {
                Cost_HService cost_HService = new Cost_HService(db);

                //Get Cost
                return cost_HService.GetByID(id);
            }
        }

        /// <summary>
        /// Init Detail List
        /// </summary>
        private void InitDetailList()
        {
            //Add data
            IList<T_Cost_D> listDetail = new List<T_Cost_D>();
            listDetail.Add(new T_Cost_D()
            {
                EffectDate = new DateTime(MIN_YEAR, MIN_MONTH, MIN_DAY),
                ExpireDate = new DateTime(MAX_YEAR, MAX_MONTH, MAX_DAY),
                CostAmount = null
            });

            //Process list view
            this.DetailLists = listDetail;

            //this.InitPagingList(listDetail);

            // Paging footer
            this.PagingFooter.CurrentPage = this.CurrenIndexPage;
            this.PagingFooter.NumberOnPage = Constant.DEFAULT_NUMBER_ROW;
            this.PagingFooter.TotalRow = listDetail.Count;

            this.rptListCost.DataSource = listDetail;
            this.rptListCost.DataBind();
        }

        /// <summary>
        /// Insert data
        /// </summary>
        private bool InsertData(ref int newId)
        {
            try
            {
                int ret = 0;

                T_Cost_H header = new T_Cost_H();
                this.GetHeader(header);

                using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                {
                    newId = db.GetIdentityId<T_Cost_H>();

                    IList<T_Cost_D> detail = this.GetDetailList(true);

                    Cost_DService detailService = new Cost_DService(db);
                    foreach (var item in detail)
                    {
                        item.HID = newId;
                        detailService.Insert(item);
                    }
                    db.Commit();
                }
            }
            catch (SqlException ex)
            {
                //if (ex.Message.Contains(Models.Constant.T_COST_H_UN1))
                //{
                this.SetMessage(this.txtCostName.ID, M_Message.MSG_EXIST_CODE, "Cost Name");
                //}

                return false;
            }
            catch (Exception)
            {
                this.SetMessage(string.Empty, M_Message.MSG_UPDATE_FAILE, "Insert");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Update Data
        /// </summary>
        /// <returns>Success:true, Faile:false</returns>
        private bool UpdateData()
        {
            try
            {
                int ret = 0;
                T_Cost_H header = this.GetCost(this.CostID);
                if (header != null)
                {
                    this.GetHeader(header);

                    //Update
                    using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                    {
                        if (this.IsDetailChange(db))
                        {
                            //Update header
                            Cost_HService headerService = new Cost_HService(db);
                            ret = headerService.Update(header);

                            if (ret == 1)
                            {
                                //Update detail
                                Cost_DService detailService = new Cost_DService(db);
                                detailService.Delete(this.CostID);

                                IList<T_Cost_D> detail = this.GetDetailList(true);
                                foreach (var item in detail)
                                {
                                    item.HID = this.CostID;
                                    detailService.Insert(item);
                                }
                            }

                            db.Commit();
                        }
                        else
                        {
                            return true;
                        }

                    }
                }

                //Check result update
                if (ret == 0)
                {
                    // Data has changed
                    this.SetMessage(string.Empty, M_Message.MSG_DATA_CHANGED);
                    return false;
                }
            }
            catch (Exception)
            {
                this.SetMessage(string.Empty, M_Message.MSG_UPDATE_FAILE, "Update");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Check detail change data
        /// </summary>
        /// <returns></returns>
        private bool IsDetailChange(DB db)
        {
            //Get list from data
            IList<T_Cost_D> listDetailData;

            Cost_DService CostDService = new Cost_DService(db);
            listDetailData = CostDService.GetByListByHeaderID(this.CostID);
            //Get list from screen
            IList<T_Cost_D> listDetailSrceen = this.GetDetailList(true);

            //Check count change
            if (listDetailSrceen.Count != listDetailData.Count)
            {
                return true;
            }
            else
            {
                for (int i = 0; i < listDetailSrceen.Count; i++)
                {
                    var oldData = listDetailData[i];
                    var newData = listDetailSrceen[i];

                    //Check EffectDate change
                    if (oldData.EffectDate!=newData.EffectDate)
                    {
                        return true;
                    }

                    //Check ExpireDate change
                    if (oldData.ExpireDate!=newData.ExpireDate)
                    {
                        return true;
                    }

                    //Check CostAmount change
                    if (oldData.CostAmount != newData.CostAmount)
                    {
                        return true;
                    }

                   
                }
            }
            return false;
        }

        /// <summary>
        /// Find pre index with not empty data
        /// </summary>
        /// <param name="index">index</param>
        /// <param name="listDetail">listDetail</param>
        /// <returns></returns>
        private int FindPreIndexNotEmptyData(int index, IList<T_Cost_D> listDetail)
        {
            for (int i = index - 1; i >= 0; i--)
            {
                if (listDetail[i].EffectDate.HasValue && listDetail[i].CostAmount.HasValue)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Find next index with not empty data
        /// </summary>
        /// <param name="index">index</param>
        /// <param name="listDetail">listDetail</param>
        /// <returns></returns>
        private int FindNextIndexNotEmptyData(int index, IList<T_Cost_D> listDetail)
        {
            for (int i = index + 1; i < listDetail.Count; i++)
            {
                if (listDetail[i].EffectDate.HasValue && listDetail[i].CostAmount.HasValue)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Process effect date
        /// </summary>
        /// <param name="listDetail"></param>
        private void ProcessEffectDate(IList<T_Cost_D> listDetail)
        {
            if (listDetail.Count < 2)
            {
                listDetail[0].ExpireDate = new DateTime(MAX_YEAR, MAX_MONTH, MAX_DAY);
                return;
            }
            else if (listDetail.Count > 2)
            {
                bool isHaveData = false;
                for (int index = listDetail.Count - 2; index >= 0; index--)
                {
                    var item = listDetail[index];

                    if (!item.EffectDate.HasValue && !item.CostAmount.HasValue)
                    {
                        item.ExpireDate = null;
                        continue;
                    }
                    isHaveData = true;

                    //Find index previous not empty data
                    var preIndex = this.FindPreIndexNotEmptyData(index, listDetail);

                    //Find index next not empty data
                    var nextIndex = this.FindNextIndexNotEmptyData(index, listDetail);

                    var itemNext = listDetail[nextIndex];

                    var effectDate = new DateTime();

                    if (item.EffectDate.HasValue)
                    {
                        effectDate = item.EffectDate.Value.AddDays(-1);

                        //Set for Expire date with previous index
                        if (item.EffectDate.Value.CompareTo(itemNext.EffectDate) != 0)
                        {
                            itemNext.ExpireDate = effectDate;
                        }
                        else
                        {
                            itemNext.ExpireDate = item.EffectDate;
                        }

                        //Set for effect date with curren index
                        if (preIndex != -1)
                        {
                            var itemPre = listDetail[preIndex];
                            effectDate = itemPre.EffectDate.Value.AddDays(-1);

                            if (item.EffectDate.Value.CompareTo(itemPre.EffectDate.Value) != 0)
                            {
                                item.ExpireDate = effectDate;
                            }
                            else
                            {
                                item.ExpireDate = itemPre.EffectDate;
                            }
                        }
                        else
                        {
                            item.ExpireDate = new DateTime(MAX_YEAR, MAX_MONTH, MAX_DAY);
                        }
                    }
                    else
                    {
                        //Set for effect date with curren index
                        if (preIndex != -1)
                        {
                            var itemPre = listDetail[preIndex];
                            effectDate = itemPre.EffectDate.Value.AddDays(-1);
                        }
                        else
                        {
                            effectDate = new DateTime(MAX_YEAR, MAX_MONTH, MAX_DAY);
                        }

                        itemNext.ExpireDate = effectDate;
                    }
                }

                if (!isHaveData)
                {
                    listDetail[listDetail.Count - 1].ExpireDate = new DateTime(MAX_YEAR, MAX_MONTH, MAX_DAY);
                }
                else
                {
                    if (listDetail[0].EffectDate.HasValue && listDetail[0].CostAmount.HasValue)
                    {
                        listDetail[0].ExpireDate = new DateTime(MAX_YEAR, MAX_MONTH, MAX_DAY);
                    }
                }
            }
            else
            {
                var item0 = listDetail[0];
                var item1 = listDetail[1];

                //Set date
                if (item0.EffectDate.HasValue)
                {
                    item1.ExpireDate = item0.EffectDate.Value.AddDays(-1);
                    item0.ExpireDate = new DateTime(MAX_YEAR, MAX_MONTH, MAX_DAY);
                }
                else
                {
                    item1.ExpireDate = new DateTime(MAX_YEAR, MAX_MONTH, MAX_DAY);
                }
            }
            this.DetailLists = listDetail;
        }
        #endregion
    }
}