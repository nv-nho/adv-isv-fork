using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using OMS.Models;
using OMS.Utilities;
using OMS.DAC;
using OMS.Controls;
using System.Web.UI.HtmlControls;

namespace OMS.Master
{
    /// <summary>
    /// Class Config Detail
    /// Create Date: 2014/07/31
    /// Create Author: VN-Nho
    /// </summary>
    public partial class FrmConfigDetail : FrmBaseDetail
    {
        private const string URL_LIST = "~/Master/FrmConfigList.aspx";
        #region Property

        /// <summary>
        /// Get or set ConfigID
        /// </summary>
        public int ConfigID
        {
            get { return (int)ViewState["ConfigID"]; }
            set { ViewState["ConfigID"] = value; }
        }

        /// <summary>
        /// Get or set OldUpdateDate
        /// </summary>
        public DateTime OldUpdateDate
        {
            get { return (DateTime)ViewState["OldUpdateDate"]; }
            set { ViewState["OldUpdateDate"] = value; }
        }

        #endregion

        #region Event

        /// <summary>
        /// Event Init
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //Set Title
            base.FormTitle = "Config Master";
            base.FormSubTitle = "Detail";

            //Init Max Length
            this.txtConfigCD.MaxLength = M_Config_H.CONFIG_CODE_MAX_LENGTH;
            this.txtConfigName.MaxLength = M_Config_H.CONFIG_NAME_MAX_LENGTH;

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
            if (base.LoginInfo == null || !base.IsAdmin())
            {
                Response.Redirect("~/Menu/FrmMasterMenu.aspx");
            }

            if (!this.IsPostBack)
            {
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
                        //Get config data by ID
                        this.ConfigID = int.Parse(base.PreviousPageViewState["ID"].ToString());
                        M_Config_H hModel = this.GetHeaderData(this.ConfigID);

                        if (hModel != null)
                        {
                            //Show data
                            this.ShowHeaderData(hModel);

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
                    //Set mode
                    this.ProcessMode(Mode.Insert);

                    //Init detail list
                    this.InitDetailList();
                }
            }

            //Set init
            this.Success = false;
        }

        /// <summary>
        /// Edit Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            //Get Config data by ID
            M_Config_H hModel = this.GetHeaderData(this.ConfigID);

            //Check Config exists
            if (hModel != null)
            {
                //Show data
                this.ShowHeaderData(hModel);

                //Set Mode
                this.ProcessMode(Mode.Update);
            }
            else
            {
                Server.Transfer(URL_LIST);
            }
        }

        /// <summary>
        /// Copy Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCopy_Click(object sender, EventArgs e)
        {
            //Get config data by ID
            M_Config_H hModel = this.GetHeaderData(this.ConfigID);

            //Check Config exists
            if (hModel != null)
            {
                //Show data
                this.ShowHeaderData(hModel);

                //Set Mode
                this.ProcessMode(Mode.Copy);
            }
            else
            {
                Server.Transfer(URL_LIST);
            }
        }

        /// <summary>
        /// Delete Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            //Set Model
            this.Mode = Mode.Delete;

            //Show question insert
            base.ShowQuestionMessage(M_Message.MSG_QUESTION_DELETE, Models.DefaultButton.No, true);
        }

        /// <summary>
        /// Event Button Insert Submit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnInsert_Click(object sender, EventArgs e)
        {
            //Check input
            if (!this.CheckInput())
            {
                return;
            }

            //Show question insert
            base.ShowQuestionMessage(M_Message.MSG_QUESTION_INSERT, Models.DefaultButton.Yes);
        }

        /// <summary>
        /// Event Button Update Submit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            //Check input
            if (!this.CheckInput())
            {
                return;
            }

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
            else if (this.Mode == Mode.Update || this.Mode == Mode.Copy)
            {
                //Get Config by ID
                M_Config_H hModel = this.GetHeaderData(this.ConfigID);

                //Check Config exists
                if (hModel != null)
                {
                    //Show data
                    this.ShowHeaderData(hModel);

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
        /// Event Button AddRow
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnAddRow_Click(object sender, EventArgs e)
        {

            //Get new list from screen
            var listDetail = this.GetDetailList(false);
            if (listDetail != null)
            {
                listDetail.Add(new M_Config_D());
            }

            //Process list view
            this.rptList.DataSource = listDetail;
            this.rptList.DataBind();
        }

        /// <summary>
        /// Event Button RemoveRow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRemoveRow_Click(object sender, EventArgs e)
        {
            //Get new list from screen
            var listDetail = this.GetDetailList(false);

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
                listDetail.RemoveAt(item);
            }

            if (listDetail.Count == 0)
            {
                listDetail.Add(new M_Config_D());
            }

            //Process list view
            this.rptList.DataSource = listDetail;
            this.rptList.DataBind();
        }

        /// <summary>
        /// Process Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnProcessData(object sender, EventArgs e)
        {
            bool ret;
            M_Config_H hModel = null;

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
                        //Get config data by ID
                        hModel = this.GetHeaderData(newID);
                    }
                    break;
                case Utilities.Mode.Delete:

                    //Delete data
                    ret = this.DeleteData();
                    if (!ret)
                    {
                        //Set Mode
                        this.ProcessMode(Mode.View);
                    }
                    else
                    {
                        Server.Transfer(URL_LIST);
                    }
                    break;

                default:

                    //Update Data
                    ret = this.UpdateData();
                    if (ret)
                    {
                        //Get config data by ID
                        hModel = this.GetHeaderData(this.ConfigID);
                    }

                    break;
            }

            if (ret)
            {
                //Show data
                this.ShowHeaderData(hModel);

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
            //Get Config Header
            M_Config_H hModel = this.GetHeaderData(this.ConfigID);
            if (hModel != null)
            {
                //Show data
                this.ShowHeaderData(hModel);

                //Set Mode
                this.ProcessMode(Mode.View);
            }
            else
            {
                Server.Transfer(URL_LIST);
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
            bool enable;

            //Set mode
            this.Mode = mode;

            //Check model
            switch (mode)
            {
                case Mode.Insert:
                case Mode.Copy:
                case Mode.Update:
                    if (mode == Mode.Update)
                    {
                        this.txtConfigCD.ReadOnly = true;
                    }
                    else
                    {
                        this.txtConfigCD.ReadOnly = false;
                    }

                    enable = false;
                    break;

                default:
                    this.txtConfigCD.ReadOnly = true;

                    enable = true;

                    break;
            }

            //Lock control
            this.txtConfigName.ReadOnly = enable;
        }

        /// <summary>
        /// Init Detail List
        /// </summary>
        private void InitDetailList()
        {
            //Add data
            IList<M_Config_D> listDetail = new List<M_Config_D>();
            listDetail.Add(new M_Config_D());

            //Process list view
            this.rptList.DataSource = listDetail;
            this.rptList.DataBind();
        }

        /// <summary>
        /// Get Header Data
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        private M_Config_H GetHeaderData(int id)
        {
            using (DB db = new DB())
            {
                Config_HService dbSer = new Config_HService(db);

                //Get Config header
                return dbSer.GetByID(id);
            }
        }

        /// <summary>
        /// Show header data on form
        /// </summary>
        /// <param name="model">M_Config_H</param>
        private void ShowHeaderData(M_Config_H model)
        {
            //Display header
            this.ConfigID = model.ID;
            this.txtConfigCD.Value = model.ConfigCD;
            this.txtConfigName.Value = model.ConfigName;

            this.OldUpdateDate = model.UpdateDate;

            this.ShowDetailData(model.ConfigCD);
        }

        /// <summary>
        /// Show detail data on form
        /// </summary>
        /// <param name="headerID">Header ID</param>
        private void ShowDetailData(string configCode)
        {
            using (DB db = new DB())
            {
                Config_DService dbSer = new Config_DService(db);

                //Get list detail
                IList<M_Config_D> listDetail = dbSer.GetListByConfigCd(configCode);
                if (listDetail != null)
                {
                    if (listDetail.Count != 0)
                    {
                        this.rptList.DataSource = listDetail;
                    }
                }

                this.rptList.DataBind();
            }
        }

        /// <summary>
        /// Check input
        /// </summary>
        /// <returns>Valid:true, Invalid:false</returns>
        private bool CheckInput()
        {
            //Config Code
            if (this.txtConfigCD.IsEmpty)
            {
                base.SetMessage(this.txtConfigCD.ID, M_Message.MSG_REQUIRE, "Config Code");
            }
            else
            {
                //Check exist by Config code
                if (this.Mode == Mode.Insert || this.Mode == Mode.Copy)
                {
                    if (this.IsExistConfigCode(this.txtConfigCD.Value))
                    {
                        base.SetMessage(this.txtConfigCD.ID, M_Message.MSG_EXIST_CODE, "Config Code");
                    }
                }
            }

            //Config Name
            if (this.txtConfigName.IsEmpty)
            {
                base.SetMessage(this.txtConfigName.ID, M_Message.MSG_REQUIRE, "Config Name");
            }

            List<int> lstDup = new List<int>();

            bool hasData = false;

            foreach (RepeaterItem item in this.rptList.Items)
            {
                if (this.IsEmptyRow(item))
                {
                    continue;
                }

                hasData = true;
                int rowIndex = item.ItemIndex + 1;

                //Value1
                ICodeTextBox txtValue1 = (ICodeTextBox)item.FindControl("txtValue1");
                if (txtValue1 != null)
                {
                    HtmlGenericControl divValue = (HtmlGenericControl)item.FindControl("divValue1");
                    divValue.Attributes.Remove("class");
                    string errorId = txtValue1.ID + "_" + item.ItemIndex.ToString();
                    if (string.IsNullOrEmpty(txtValue1.Value))
                    {
                        base.SetMessage(errorId, M_Message.MSG_REQUIRE_GRID, "Value 1", rowIndex);
                        this.AddErrorForListItem(divValue, errorId);
                    }
                    else
                    {
                        if (lstDup.Contains(int.Parse(txtValue1.Value)))
                        {
                            base.SetMessage(errorId, M_Message.MSG_DUPLICATE_GRID, "Value 1", rowIndex);
                            this.AddErrorForListItem(divValue, errorId);
                        }
                        else
                        {
                            lstDup.Add(int.Parse(txtValue1.Value));
                        }
                        txtValue1.Value = int.Parse(txtValue1.Value).ToString();
                    }
                }

                ////Value2
                //ITextBox txtValue2 = (ITextBox)item.FindControl("txtValue2");
                //if (txtValue2 != null)
                //{
                //    HtmlGenericControl divValue = (HtmlGenericControl)item.FindControl("divValue2");
                //    divValue.Attributes.Remove("class");
                //    string errorId = txtValue2.ID + "_" + item.ItemIndex.ToString();
                //    if (string.IsNullOrEmpty(txtValue2.Value))
                //    {
                //        base.SetMessage(errorId, M_Message.MSG_REQUIRE, "Value2");
                //        this.AddErrorForListItem(divValue, errorId);
                //    }
                //}

                ////Value3
                //ITextBox txtValue3 = (ITextBox)item.FindControl("txtValue3");
                //if (txtValue3 != null)
                //{
                //    HtmlGenericControl divValue = (HtmlGenericControl)item.FindControl("divValue3");
                //    divValue.Attributes.Remove("class");
                //    string errorId = txtValue3.ID + "_" + item.ItemIndex.ToString();
                //    if (string.IsNullOrEmpty(txtValue3.Value))
                //    {
                //        base.SetMessage(errorId, M_Message.MSG_REQUIRE, "Value3");
                //        this.AddErrorForListItem(divValue, errorId);
                //    }
                //}

                ////Value4
                //ITextBox txtValue4 = (ITextBox)item.FindControl("txtValue4");
                //if (txtValue4 != null)
                //{
                //    HtmlGenericControl divValue = (HtmlGenericControl)item.FindControl("divValue4");
                //    divValue.Attributes.Remove("class");
                //    string errorId = txtValue4.ID + "_" + item.ItemIndex.ToString();
                //    if (string.IsNullOrEmpty(txtValue4.Value))
                //    {
                //        base.SetMessage(errorId, M_Message.MSG_REQUIRE, "Value4");
                //        this.AddErrorForListItem(divValue, errorId);
                //    }
                //}
            }

            if (!hasData)
            {
                ICodeTextBox txtValue1 = (ICodeTextBox)this.rptList.Items[0].FindControl("txtValue1");
                HtmlGenericControl divValue = (HtmlGenericControl)this.rptList.Items[0].FindControl("divValue1");
                string errorId = txtValue1.ID + "_" + this.rptList.Items[0].ItemIndex.ToString();
                base.SetMessage(errorId, M_Message.MSG_REQUIRE_GRID, "Value 1", 1);
                this.AddErrorForListItem(divValue, errorId);
            }

            //Check error
            return !base.HaveError;
        }

        /// <summary>
        /// Add display error for control
        /// </summary>
        /// <param name="divCtrl">div error control</param>
        /// <param name="errorKey">Error Control ID</param>
        private void AddErrorForListItem(HtmlGenericControl divCtrl, string errorKey)
        {
            divCtrl.Attributes.Add("class", "form-group " + base.GetClassError(errorKey));
            //Control ctrError = ParseControl(base.GetSpanError(errorKey));
            //divCtrl.Controls.Add(ctrError);
        }

        /// <summary>
        /// Check empty row
        /// </summary>
        /// <returns></returns>
        private bool IsEmptyRow(RepeaterItem item)
        {
            bool ret = true;

            //Value1
            ICodeTextBox txtValue1 = (ICodeTextBox)item.FindControl("txtValue1");
            if (txtValue1 != null)
            {
                if (!string.IsNullOrEmpty(txtValue1.Value))
                {
                    ret = false;
                }
            }

            //Value2
            ITextBox txtValue2 = (ITextBox)item.FindControl("txtValue2");
            if (txtValue2 != null)
            {
                if (!string.IsNullOrEmpty(txtValue2.Value))
                {
                    ret = false;
                }
            }

            //Value3
            ITextBox txtValue3 = (ITextBox)item.FindControl("txtValue3");
            if (txtValue3 != null)
            {
                if (!string.IsNullOrEmpty(txtValue3.Value))
                {
                    ret = false;
                }
            }

            //Value4
            ITextBox txtValue4 = (ITextBox)item.FindControl("txtValue4");
            if (txtValue4 != null)
            {
                if (!string.IsNullOrEmpty(txtValue4.Value))
                {
                    ret = false;
                }
            }

            return ret;
        }

        /// <summary>
        /// Check exist Config by Config Code
        /// </summary>
        /// <param name="ConfigCD">Config Code</param>
        /// <returns></returns>
        private bool IsExistConfigCode(string ConfigCD)
        {
            using (DB db = new DB())
            {
                Config_HService dbSer = new Config_HService(db);

                //Check Exist
                return dbSer.IsExistConfigCode(ConfigCD);
            }
        }

        /// <summary>
        /// Get header
        /// </summary>
        /// <returns></returns>
        private M_Config_H GetHeader()
        {
            M_Config_H header = new M_Config_H();
            header.ConfigCD = this.txtConfigCD.Value;
            header.ConfigName = this.txtConfigName.Value;
            header.CreateUID = this.LoginInfo.User.ID;
            header.UpdateUID = this.LoginInfo.User.ID;
            if (this.Mode == Mode.Update)
            {
                header.UpdateDate = this.OldUpdateDate;
            }

            return header;
        }

        /// <summary>
        /// Get detail list from screen
        /// </summary>
        /// <returns></returns>
        private List<M_Config_D> GetDetailList(bool isProcess)
        {
            List<M_Config_D> results = new List<M_Config_D>();

            foreach (RepeaterItem item in this.rptList.Items)
            {
                if (isProcess && this.IsEmptyRow(item))
                {
                    continue;
                }

                HtmlInputCheckBox chkDelFlg = (HtmlInputCheckBox)item.FindControl("deleteFlag");
                ICodeTextBox txtValue1 = (ICodeTextBox)item.FindControl("txtValue1");
                ITextBox txtValue2 = (ITextBox)item.FindControl("txtValue2");
                ITextBox txtValue3 = (ITextBox)item.FindControl("txtValue3");
                ITextBox txtValue4 = (ITextBox)item.FindControl("txtValue4");

                M_Config_D addItem = new M_Config_D();

                //Delete flag
                if (chkDelFlg != null)
                {
                    addItem.DelFlag = (chkDelFlg.Checked) ? true : false;
                }

                //Value1
                int value1 = 0;
                if (int.TryParse(txtValue1.Value, out value1))
                {
                    addItem.Value1 = value1;
                }

                //Value2
                addItem.Value2 = txtValue2.Value;

                //Value3
                addItem.Value3 = txtValue3.Value;

                //Value4
                addItem.Value4 = txtValue4.Value;

                results.Add(addItem);
            }

            return results;
        }

        /// <summary>
        /// Insert data
        /// </summary>
        private bool InsertData(ref int newId)
        {
            try
            {
                M_Config_H header = this.GetHeader();

                using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                {
                    Config_HService headerService = new Config_HService(db);
                    headerService.Insert(header);

                    newId = db.GetIdentityId<M_Config_H>();

                    List<M_Config_D> detail = this.GetDetailList(true);
                    Config_DService detailService = new Config_DService(db);
                    int detailID = 1;
                    foreach (var item in detail)
                    {
                        item.HID = newId;
                        item.No = detailID;
                        detailService.Insert(item);
                        detailID += 1;
                    }

                    db.Commit();
                }
            }
            catch (Exception ex)
            {
                this.SetMessage(string.Empty, M_Message.MSG_UPDATE_FAILE, "Insert");

                Log.Instance.WriteLog(ex);

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
                M_Config_H header = this.GetHeaderData(this.ConfigID);
                if (header != null)
                {
                    header = this.GetHeader();
                    header.ID = this.ConfigID;
                    //Update                     
                    using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                    {
                        if (header.Status == DataStatus.Changed || this.IsDetailChange(db))
                        {
                            //Update header
                            Config_HService headerService = new Config_HService(db);
                            ret = headerService.Update(header);
                            if (ret == 1)
                            {
                                //Update detail
                                Config_DService detailService = new Config_DService(db);
                                detailService.Delete(this.ConfigID);

                                List<M_Config_D> detail = this.GetDetailList(true);
                                int detailID = 1;
                                foreach (var item in detail)
                                {
                                    item.HID = header.ID;
                                    item.No = detailID;
                                    detailService.Insert(item);
                                    detailID += 1;
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
            }
            catch (Exception ex)
            {
                this.SetMessage(string.Empty, M_Message.MSG_UPDATE_FAILE, "Update");

                Log.Instance.WriteLog(ex);

                return false;
            }

            return true;
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <returns>Success:true, Faile:false</returns>
        private bool DeleteData()
        {
            try
            {
                int ret = 0;
                using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                {
                    Config_DService detailService = new Config_DService(db);
                    detailService.Delete(this.ConfigID);

                    Config_HService headerService = new Config_HService(db);
                    ret = headerService.Delete(this.ConfigID, this.OldUpdateDate);
                    if (ret == 1)
                    {
                        db.Commit();
                    }
                }

                //Check result update
                if (ret == 0)
                {
                    //du lieu thay doi
                    this.SetMessage(string.Empty, M_Message.MSG_DATA_CHANGED);
                    return false;
                }
            }
            catch (Exception ex)
            {
                this.SetMessage(string.Empty, M_Message.MSG_UPDATE_FAILE, "Delete");

                Log.Instance.WriteLog(ex);
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
            IList<M_Config_D> listDetailData;

            Config_DService detailService = new Config_DService(db);
            listDetailData = detailService.GetByListByHeaderID(this.ConfigID);

            //Get list from screen
            List<M_Config_D> listDetailSrceen = this.GetDetailList(true);

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

                    //Check value1 change
                    if (oldData.Value1.CompareTo(newData.Value1) != 0)
                    {
                        return true;
                    }

                    //Check value2 change
                    if (oldData.Value2.CompareTo(newData.Value2) != 0)
                    {
                        return true;
                    }

                    //Check value3 change
                    if (oldData.Value3 != newData.Value3)
                    {
                        return true;
                    }

                    //Check value4 change
                    if (oldData.Value4 != newData.Value4)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion
    }
}