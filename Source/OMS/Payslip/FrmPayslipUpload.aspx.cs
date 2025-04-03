using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using OMS.Models;
using OMS.DAC;
using OMS.Utilities;
using System.IO;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI;

namespace OMS.Payslip
{
    public partial class FrmPayslipUpload : FrmBaseDetail
    {
        #region Constant
        private const string URL_LIST = "~/Payslip/FrmPayslipList.aspx";
        private const string CONST_KAKUNIN_TEXT = "確認済";
        #endregion

        #region Property

        /// <summary>
        /// Get or set Success
        /// </summary>
        public bool Init_flag { get; set; }

        /// <summary>
        /// Get or set Back button
        /// </summary>
        public bool back_Flag { get; set; }

        /// <summary>
        /// Get or set ConfigID
        /// </summary>
        public int TotalRow
        {
            get { return (int)ViewState["TotalRow"]; }
            set { ViewState["TotalRow"] = value; }
        }

        /// <summary>
        /// Get or set text Kakunin
        /// </summary>
        public string textKakunin
        {
            get { return CONST_KAKUNIN_TEXT; }
        }

        #endregion

        #region Event

        /// <summary>
        /// On Init
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //Set Title
            base.FormTitle = "給与・賞与明細一覧";
            base.FormSubTitle = "";

            // header grid sort
            this.HeaderGrid.OnSortClick += Sort_Click;

            //Init Event
            LinkButton btnYes = (LinkButton)this.Master.FindControl("btnYes");
            btnYes.Click += new EventHandler(btnProcessData);

            LinkButton btnNo = (LinkButton)this.Master.FindControl("btnNo");
            btnNo.Click += new EventHandler(btnNo_Click);

            this.Label0.Text = this.Messages["0090"].Message3;
            this.Label1.Text = this.Messages["0091"].Message3;
            this.Label2.Text = this.Messages["0092"].Message3;
            this.Label3.Text = this.Messages["0093"].Message3;
            this.Label4.Text = this.Messages[M_Message.MSG_QUESTION_DELETE].Message3;
            
            //Download File Click
            this.btnDownload.ServerClick += new EventHandler(btnDownload_Click);

            //Search button
            this.btnSearch.ServerClick += new EventHandler(btnSearch_Click);

            //Remove File Click
            this.btnRemove.ServerClick += new EventHandler(btnRemove_Click);

            //Remove All File Click
            this.btnRemoveAllOk.ServerClick += new EventHandler(btnRemoveAllOk_Click);

            //Back To List Click
            this.btnBackToList.ServerClick += new EventHandler(btnBackToList_Click);
            
        }

        /// <summary>
        /// Process Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnProcessData(object sender, EventArgs e)
        {
            this.Remove();
        }

        /// <summary>
        /// Search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // header grid
            this.HeaderGrid.SortDirec = "1";
            this.HeaderGrid.SortField = "2";

            // Refresh load grid
            this.hdn_txtYear.Value = this.txtYear.Value.ToString();
            this.hdn_cmbInvalidData.Value = this.cmbInvalidData.SelectedValue;
            this.Init_flag = false;
            this.back_Flag = false;
            setMode(this.LoadDataGrid());
        }

        /// <summary>
        /// Click Sort
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Sort_Click(object sender, EventArgs e)
        {
            setMode(this.LoadDataGrid());
        }

        /// <summary>
        /// Back To List
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBackToList_Click(object sender, EventArgs e)
        {
            Server.Transfer(URL_LIST);
        }

        /// <summary>
        /// No
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNo_Click(object sender, EventArgs e)
        {
            return;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Check authority
            base.SetAuthority(FormId.AttendancePayslip);
            if (!this._authority.IsAttendancePayslipUpload)
            {
                Server.Transfer(URL_LIST);
            }

            if (!this.IsPostBack)
            {
                if (base.PreviousPage != null)
                {
                    //Save condition of previous page
                    base.ViewState["Condition"] = base.PreviousPageViewState["Condition"];

                    //Init data
                    this.InitData();
                }
                else {
                    //Init data
                    this.InitData();
                }
            }

            //Set init
            this.Success = false;

            if(!this.Init_flag){
                this.GetTotalRows();
            }

        }

        /// <summary>
        /// Process mode
        /// </summary>
        /// <param name="mode">Mode</param>
        private void ProcessMode(Mode mode)
        {
            //Set mode
            this.Mode = mode;

            this.txtYear.ReadOnly = true;
            this.cmbInvalidData.Enabled = false;

            if (this.Mode == Utilities.Mode.Update)
            {
                this.btnRemoveAll.Disabled = false;
            }
            else
            {
                this.btnRemoveAll.Disabled = true;
            }
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
                this.HeaderGrid.TotalRow = 0;

                this.back_Flag = true;
                this.Init_flag = true;
                this.txtYear.Focus();

                this.rptList.DataSource = null;
                this.rptList.DataBind();
                this.ProcessMode(Mode.View);
                this.txtYear.ReadOnly = false;
                this.cmbInvalidData.Enabled = true;
            }
            else if (this.Mode == Mode.Update || this.Mode == Mode.Copy)
            {
                this.LoadDataGrid();
                this.ProcessMode(Mode.View);
            }
        }

        /// <summary>
        /// Edit Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            this.LoadDataGrid();
            this.ProcessMode(Mode.Update);
        }

        /// <summary>
        /// Event changed page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNew_Click(object sender, EventArgs e)
        {
            String month_year = (String.IsNullOrEmpty(this.hdn_txtYear.Value) ? "" : DateTime.Parse(this.hdn_txtYear.Value).ToString(Constants.FMT_YYYYMM));
            String type = this.hdn_cmbInvalidData.Value;
            bool errFlag = false;
            bool changeData_Flag = false;

            String redirect = "";

            using (DB db = new DB())
            {
                SettingService dbSer = new SettingService(db);
                M_Setting setting = dbSer.GetData();

                redirect = setting.AttachPath;
            }

            if (!Directory.Exists((redirect)))
            {
                Directory.CreateDirectory((redirect));
            }

            foreach (RepeaterItem item in this.rptList.Items)
            {
                HiddenField txtFileName = (HiddenField)item.FindControl("txtFileName");
                FileUpload fileUpload = (FileUpload)item.FindControl("authPath");
                HiddenField filepath = (HiddenField)item.FindControl("txtFilepath");
                //HiddenField txtOldFilepath = (HiddenField)item.FindControl("txtOldFilepath");

                if (overrideFlag.Value == "1")
                {
                    if (!string.IsNullOrEmpty(txtFileName.Value))
                    {
                        if (!string.IsNullOrEmpty(filepath.Value))
                        {
                            File.Delete(Path.Combine(redirect, filepath.Value));
                        }
                    }
                }

                if (!this.CheckFile(item))
                {
                    errFlag = true;
                    continue;
                }

                HiddenField userCD = (HiddenField)item.FindControl("txtUserCD");
                HiddenField ID = (HiddenField)item.FindControl("txtID");
               
                HiddenField removePath = (HiddenField)item.FindControl("hiddenRemoveFlag");

                HiddenField txtFileContent = (HiddenField)item.FindControl("txtFileContent");

                if (this.IsFile(fileUpload.FileName) || !string.IsNullOrEmpty(txtFileName.Value))
                {
                    //Save file
                    if (!string.IsNullOrEmpty(txtFileName.Value))
                    {
                        string value = txtFileContent.Value.Substring(txtFileContent.Value.IndexOf(",")+1);
                        File.WriteAllBytes(Path.Combine(redirect, txtFileName.Value), Convert.FromBase64String(value));
                    }
                    else
                    {
                        fileUpload.SaveAs(Path.Combine(redirect, fileUpload.FileName));
                    }
                    

                    //Upload data
                    using (DB db = new DB())
                    {
                        PaysplipService dbSer = new PaysplipService(db);
                        UserService userDbSer = new UserService(db);

                        PayslipInfo payslip = new PayslipInfo();
                        M_User user = userDbSer.GetByUserCD(EditDataUtil.ToFixCodeDB(userCD.Value, M_User.USER_CODE_MAX_LENGTH));

                        payslip.ID = Int32.Parse(ID.Value);
                        payslip.UpdateUID = this.LoginInfo.User.ID;
                        payslip.CreateUID = this.LoginInfo.User.ID;

                        if (!string.IsNullOrEmpty(txtFileName.Value))
                        {
                            payslip.Filepath = Path.Combine(redirect, txtFileName.Value);
                        }
                        else
                        {
                            payslip.Filepath = Path.Combine(redirect, fileUpload.FileName);
                        }

                        payslip.Year = month_year;
                        payslip.Type = type;
                        payslip.UserID = user.ID;

                        if (ID.Value == "0")
                        {
                            dbSer.Insert(payslip);
                        }
                        else
                        {
                            dbSer.Update(payslip);

                        }

                        changeData_Flag = true;
                    }

                }
            }

            overrideFlag.Value = "0";

            IList<PayslipUploadInfo> listInfo = new List<PayslipUploadInfo>();
            //Get data
            using (DB db = new DB())
            {
                PaysplipUploadService dbSer = new PaysplipUploadService(db);

                month_year = (String.IsNullOrEmpty(this.hdn_txtYear.Value) ? "" : DateTime.Parse(this.hdn_txtYear.Value).ToString(Constants.FMT_YYYYMM));

                if (string.IsNullOrEmpty(month_year))
                {
                    DateTime dateTime = DateTime.UtcNow.Date.AddMonths(-1);
                    month_year = dateTime.ToString(Constants.FMT_YYYYMM);
                    this.txtYear.Value = dateTime;
                }
                listInfo = dbSer.GetListByCond(month_year, this.hdn_cmbInvalidData.Value, int.Parse(this.HeaderGrid.SortField), int.Parse(this.HeaderGrid.SortDirec));
            }
            
            int count = listInfo.Where(x => x.OldFilepath.Trim() != "").Count();

            if (!changeData_Flag && !errFlag && this.Mode == Mode.Insert && count == 0)
            {
                this.LoadDataGrid();

                this.SetMessage(string.Empty, M_Message.MSG_0001, "ファイル名");
                return;
            }

            //Set Success
            if (!errFlag)
            {
                this.Success = true;
                setMode(this.LoadDataGrid());
            }
            else {
                this.LoadDataGrid();
            }
        }

        /// <summary>
        /// Event changed page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRemove_Click(object sender, EventArgs e)
        {
            base.ShowQuestionMessage(M_Message.MSG_QUESTION_DELETE, Models.DefaultButton.No, true);
        }

        /// <summary>
        /// Event changed page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRemoveAllOk_Click(object sender, EventArgs e)
        {
            this.RemoveAll();
        }

        /// <summary>
        /// Event btnDownload_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDownload_Click(object sender, EventArgs e)
        {
            using (DB db = new DB())
            {
                PaysplipUploadService ser = new PaysplipUploadService(db);
                int payslipID = Int32.Parse(this.hdnFileName.Value);

                PaysplipService payslipSer = new PaysplipService(db);
                PayslipDowloadInfo payslipUpload = ser.getFilePathbyPayslipID(payslipID);
                String filePath = payslipUpload.Filepath;

                if (!String.IsNullOrEmpty(filePath))
                {
                    string filename = Path.GetFileName(filePath);

                    if (File.Exists((filePath)))
                    {
                        Response.ClearContent();
                        Response.Clear();
                        Response.ContentType = "text/plain";
                        Response.AddHeader("Content-Disposition", string.Format("attachment; filename = \"{0}\"", filename));
                        Response.TransmitFile(filePath);
                        Response.Flush();
                        Response.End();
                    }
                    else
                    {
                        this.SetMessage(string.Empty, M_Message.MSG_VALUE_NOT_EXIST, "ファイル");
                    }
                }

                this.LoadDataGrid();
            }
        }

        /// <summary>
        /// Edit Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpDirUp_Click(object sender, EventArgs e)
        {
        }

        #endregion

        #region Method
        /// <summary>
        /// Init Data
        /// </summary>
        private void InitData()
        {
            // Default data valide
            this.hdInValideDefault.Value = this.GetDefaultValueForDropdownList(M_Config_H.CONFIG_UPLOAD_CLASSIFICATION);

            //Date Default
            DateTime dateTime = DateTime.Now.AddMonths(-1);
            this.hdDateDefault.Value = dateTime.ToString(Constants.FMT_YYYYMM);

            //Invalid
            this.InitCombobox(this.cmbInvalidData);

            this.txtYear.Value = dateTime;
            this.hdn_txtYear.Value = this.txtYear.Value.ToString();
            this.hdn_cmbInvalidData.Value = this.hdInValideDefault.Value;

            this.back_Flag = true;
            this.Init_flag = true;
            this.txtYear.Focus();
        }

        /// <summary>
        /// Init Combobox
        /// </summary>
        private void InitCombobox(DropDownList ddl)
        {
            // init combox 
            ddl.DataSource = this.GetDataForDropdownList(M_Config_H.CONFIG_UPLOAD_CLASSIFICATION);
            ddl.DataValueField = "Value";
            ddl.DataTextField = "DisplayName";
            ddl.DataBind();
            ddl.SelectedValue = this.hdInValideDefault.Value;
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
                return configSer.GetDataForDropDownList(configCD , sortIndex: ConfigSort.value4);
            }
        }

        /// <summary>
        /// GetDataForDropdownList
        /// </summary>
        /// <param name="configCD"></param>
        /// <returns></returns>
        private int? getValue3(string configCd, int value1)
        {
            using (DB db = new DB())
            {
                Config_DService configSer = new Config_DService(db);
                return configSer.GetValue(configCd, value1);
            }
        }

        /// <summary>
        /// GetFileExtension
        /// </summary>
        /// <param name="configCD"></param>
        /// <returns></returns>
        private IList<DropDownModel> GetFileExtension(string configCD)
        {
            using (DB db = new DB())
            {
                Config_HService configSer = new Config_HService(db);
                return configSer.GetDataForDropDownList(configCD);

            }
        }

        /// <summary>
        /// Get Total Rows
        /// </summary>
        /// <param name="configCD"></param>
        /// <returns></returns>
        private void GetTotalRows()
        {
            using (DB db = new DB())
            {
                PaysplipUploadService dbSer = new PaysplipUploadService(db);
                String month_year = (String.IsNullOrEmpty(this.hdn_txtYear.Value) ? "" : DateTime.Parse(this.hdn_txtYear.Value).ToString(Constants.FMT_YYYYMM));
                if (string.IsNullOrEmpty(month_year))
                {
                    DateTime dateTime = DateTime.UtcNow.Date.AddMonths(-1);
                    month_year = dateTime.ToString(Constants.FMT_YYYYMM);
                    this.txtYear.Value = dateTime;
                }

                this.TotalRow = dbSer.getTotalRow(month_year, this.hdn_cmbInvalidData.Value);
                this.HeaderGrid.TotalRow = this.TotalRow;
                this.HeaderGrid.AddColumms(new string[] { "#", "コード", "社員名", "部門", "", "ファイル名", "更新日", "確認日" });
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
        /// load data grid
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="numOnPage"></param>
        private IList<PayslipUploadInfo> LoadDataGrid()
        {
            IList<PayslipUploadInfo> listInfo = new List<PayslipUploadInfo>();
            
            //Get data
            using (DB db = new DB())
            {
                PaysplipUploadService dbSer = new PaysplipUploadService(db);

                String month_year = (String.IsNullOrEmpty(this.hdn_txtYear.Value) ? "" : DateTime.Parse(this.hdn_txtYear.Value).ToString(Constants.FMT_YYYYMM));

                if (string.IsNullOrEmpty(month_year)) {
                     DateTime dateTime = DateTime.UtcNow.Date.AddMonths(-1);
                     month_year = dateTime.ToString(Constants.FMT_YYYYMM);
                     this.txtYear.Value = dateTime;
                }
                listInfo = dbSer.GetListByCond(month_year, this.hdn_cmbInvalidData.Value, int.Parse(this.HeaderGrid.SortField), int.Parse(this.HeaderGrid.SortDirec));
            }

            //Show data
            if (listInfo.Count == 0)
            {
                this.rptList.DataSource = null;
            }
            else
            {
                this.TotalRow = listInfo.Count();
                this.HeaderGrid.TotalRow = this.TotalRow;
                // detail
                this.rptList.DataSource = listInfo;

                this.rptList.DataSource = listInfo;
            }

            this.rptList.DataBind();
            return listInfo;
        }

        public void setMode(IList<PayslipUploadInfo> listInfo)
        {
            using (DB db = new DB())
            {
                Boolean flag_HadData = false;

                foreach (PayslipUploadInfo payslip in listInfo)
                {
                    if (!string.IsNullOrEmpty(payslip.Filepath))
                    {
                        flag_HadData = true;
                    }
                }

                if (flag_HadData)
                {
                    //Set Mode Edit (編集)
                    this.ProcessMode(Mode.View);
                }
                else
                {
                    //Set Mode
                    this.ProcessMode(Mode.Insert);
                }

            }
        }

        public void Remove()
        {
            int payslipID = Int32.Parse(this.hdnRemoveFile.Value);

            using (DB db = new DB())
            {
                PaysplipService dbSer = new PaysplipService(db);
                PaysplipUploadService uploadser = new PaysplipUploadService(db);

                PayslipDowloadInfo payslipUpload = uploadser.getFilePathbyPayslipID(payslipID);
                dbSer.Delete(payslipID);
                if (File.Exists(payslipUpload.Filepath))
                {
                    try
                    {
                        File.Delete(payslipUpload.Filepath);

                    }
                    catch (Exception ex)
                    {
                        //Do something
                    }
                }


            }
            //Set Mode
            var saveMode = this.Mode;
            setMode(this.LoadDataGrid());

            if (this.Mode != Mode.Insert)
            {
                this.ProcessMode(saveMode);
            }
        }

        public void RemoveAll()
        {
            using (DB db = new DB())
            {
                PaysplipService dbSer = new PaysplipService(db);

                String month_year = (String.IsNullOrEmpty(this.hdn_txtYear.Value) ? "" : DateTime.Parse(this.hdn_txtYear.Value).ToString(Constants.FMT_YYYYMM));

                if (string.IsNullOrEmpty(month_year))
                {
                    return;
                }
                var listInfo = dbSer.GetListByYearAndType(month_year, this.hdn_cmbInvalidData.Value);

                foreach (var item in listInfo)
                {
                    dbSer.Delete(item.ID);
                    if (File.Exists(item.Filepath))
                    {
                        try
                        {
                            File.Delete(item.Filepath);

                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    
                }
            }

            //Set Mode
            setMode(this.LoadDataGrid());

        }

        /// <summary>
        /// Check file is image
        /// </summary>
        /// <param name="postedFile"></param>
        /// <returns>Valid: True, Invalid: False</returns>
        private bool IsFile(string fileName)
        {
            //-------------------------------------------
            //  Check the file extension
            //-------------------------------------------
            IList<DropDownModel> extension = GetDataForDropdownList(M_Config_H.CONFIG_PAYSLIP_UPLOAD_FILE_EXTENSION);
            Boolean flag = false;
            foreach (DropDownModel ex in extension)
            {
                if (Path.GetExtension(fileName).ToLower() == ex.DisplayName)
                {
                    flag = true;
                }
            }
            return flag;
        }

        /// <summary>
        /// Check file is image
        /// </summary>
        /// <param name="postedFile"></param>
        /// <returns>Valid: True, Invalid: False</returns>
        private bool CheckFile(RepeaterItem item)
        {
            //-------------------------------------------
            //  Check the file extension
            //-------------------------------------------
            IList<DropDownModel> extension = GetDataForDropdownList(M_Config_H.CONFIG_PAYSLIP_UPLOAD_FILE_EXTENSION);

            ArrayList listErr = new ArrayList();
            ArrayList idxErr = new ArrayList();
            ArrayList listErrOverSize = new ArrayList();
            ArrayList idxErrOverSize = new ArrayList();
            ArrayList listSizeLimit = new ArrayList();
            ArrayList listErrExistFile = new ArrayList();
            ArrayList idxErrExistFile = new ArrayList();
            String redirect = "";
            String str_Extension = "";

            using (DB db = new DB())
            {
                SettingService dbSer = new SettingService(db);
                PaysplipService uploadSer = new PaysplipService(db);
                M_Setting setting = dbSer.GetData();

                redirect = setting.AttachPath;


                if (!Directory.Exists((redirect)))
                {
                    Directory.CreateDirectory((redirect));
                }

                FileUpload fileUpload = (FileUpload)item.FindControl("authPath");
                HiddenField txtFileContent = (HiddenField)item.FindControl("txtFileContent");
                HiddenField txtFileName = (HiddenField)item.FindControl("txtFileName");
                HiddenField ID = (HiddenField)item.FindControl("txtID");

                string filename = null;
                if (!string.IsNullOrEmpty(txtFileName.Value))
                {
                    filename = txtFileName.Value;
                }
                else
                {
                    filename = fileUpload.FileName;
                }

                if (uploadSer.IsExistFile(Path.Combine(redirect, filename), ID.Value))
                {
                    listErrExistFile.Add(filename);
                    idxErrExistFile.Add((item.ItemIndex + 1).ToString());
                }

                HiddenField filepath = (HiddenField)item.FindControl("txtFilepath");

                Boolean flag_1 = false;
                Boolean flag_2 = false;

                foreach (DropDownModel ex in extension)
                {
                    str_Extension = str_Extension + "," + ex.DisplayName.ToLower();
                    if (Path.GetExtension(filename).ToLower() == ex.DisplayName.ToLower())
                    {
                        flag_1 = true;

                        int? fileSize = this.getValue3(M_Config_H.CONFIG_PAYSLIP_UPLOAD_FILE_EXTENSION, Int32.Parse(ex.Value));
                        float ms;

                        if (!string.IsNullOrEmpty(txtFileName.Value))
                        {
                            ms = new MemoryStream(Convert.FromBase64String(txtFileContent.Value.Substring(txtFileContent.Value.IndexOf(",") + 1))).Length;
                        }
                        else
                        {
                            ms = new MemoryStream(fileUpload.FileBytes).Length;
                        }
                         
                        if (fileSize < (ms / 1024 / 1024))
                        {
                            listErrOverSize.Add(filepath.Value);
                            idxErrOverSize.Add((item.ItemIndex + 1).ToString());
                            listSizeLimit.Add(fileSize);
                        }
                    }

                    String str_extension = Path.GetExtension(filepath.Value);
                    if (str_extension.ToLower() == ex.DisplayName.ToLower())
                    {
                        flag_2 = true;
                    }
                }

                if (filename == "")
                {
                    if (filepath.Value != "" && !flag_2)
                    {
                        listErr.Add(filepath.Value);
                        idxErr.Add((item.ItemIndex + 1).ToString());
                    }
                }
                else
                {
                    if ((!flag_1 && filename != ""))
                    {
                        listErr.Add(filepath.Value);
                        idxErr.Add((item.ItemIndex + 1).ToString());
                    }
                }

                bool err_Flag = true;

                if (listErrExistFile.Count > 0)
                {
                    for (int i = 0; i < listErrExistFile.Count; i++)
                    {
                        this.SetMessage(string.Empty, M_Message.MSG_FILE_EXIST_GRID, listErrExistFile[i].ToString(), idxErrExistFile[i].ToString());
                    }
                    err_Flag = false;
                }

                if (listErr.Count > 0)
                {

                    for (int i = 0; i < listErr.Count; i++)
                    {
                        this.SetMessage(string.Empty, M_Message.MSG_EXTENSION_GRID, str_Extension.Substring(1, str_Extension.Count() - 1), idxErr[i].ToString());
                    }
                    err_Flag = false;
                }

                if (listErrOverSize.Count > 0)
                {

                    for (int i = 0; i < listErrOverSize.Count; i++)
                    {
                        this.SetMessage(string.Empty, M_Message.MSG_LESS_THAN_GRID, listErrOverSize[i].ToString(), listSizeLimit[i].ToString() + "MB", (item.ItemIndex + 1).ToString());
                    }
                    err_Flag = false;
                }
                return err_Flag;
            }
        }
        
        #endregion
    }
}