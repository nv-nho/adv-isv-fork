using System;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Linq;

using OMS.DAC;
using OMS.Models;
using OMS.Utilities;
using OMS.Controls;
using System.Collections.Generic;

namespace OMS.Master
{
    /// <summary>
    /// Setting
    /// TRAM
    /// </summary>
    public partial class FrmSetting : FrmBaseDetail
    {
        #region Constant

        private const string CONST_IMAGES_PATH = "~/Logo/";
        private const string CONST_IMAGES_PATH_TEMP = "~/TempLogo";
        private const string CONST_FILE_PATH = "~/TemplateExcel/";
        private const string CONST_FILE_PATH_TEMP = "~/TempExcel";
        private const long   CONST_IMAGE_MAX_SIZE = 2 * 1024 * 1024;
        private const string CONST_IMAGE_MAX_SIZE_STRING = "2 MB";
        private const string CONST_NO_IMAGE_AVAILABLE = "no-image.png";
        private const string CONST_TARGET_EVENT = "__EVENTTARGET";

        #endregion

        #region Property

        /// <summary>
        /// focus Label SaleID
        /// </summary>
        public string focusLabelSaleID = "";

        /// <summary>
        /// focus Label Accept
        /// </summary>
        public string focusLabelAcceptID = "";

        /// <summary>
        /// Get or set SettingID
        /// </summary>
        public int SettingID
        {
            get { return (int)ViewState["SettingID"]; }
            set { ViewState["SettingID"] = value; }
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

        #region Variable

        /// <summary>
        /// Logo1Size
        /// </summary>
        private long _logo1Size;

        /// <summary>
        /// Logo2Size
        /// </summary>
        private long _logo2Size;

        /// <summary>
        /// File1Size
        /// </summary>
        private long _file1Size;

        /// <summary>
        /// File2Size
        /// </summary>
        private long _file2Size;

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
            base.FormTitle = "Setting";
            base.FormSubTitle = "";

            this.InitMaxLengthControl();

            //Download Excel Click
            this.btnDownload.ServerClick += new EventHandler(btnDownload_Click);
            this.btnClear.ServerClick += new EventHandler(btnClear_Click);

            //Init Event
            LinkButton btnYes = (LinkButton)this.Master.FindControl("btnYes");
            btnYes.Click += new EventHandler(btnProcessData);

        }

        /// <summary>
        /// Load page 
        /// ISV-TRAM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            base.SetAuthority(FormId.Setting);
            if (!this._authority.IsMasterView)
            {
                Response.Redirect("~/Menu/FrmMasterMenu.aspx");
            }

            if (!this.IsPostBack)
            {

                M_Setting setting = this.GetSetting();
                if (setting == null)
                {
                    this.imgLogo1.Src = CONST_IMAGES_PATH + CONST_NO_IMAGE_AVAILABLE;
                    this.imgLogo2.Src = CONST_IMAGES_PATH + CONST_NO_IMAGE_AVAILABLE;
                    this.imgLogo1.Attributes.Add("style", "width:175px;height:160px");
                    this.imgLogo2.Attributes.Add("style", "width:175px;height:160px");
                    //Set Mode
                    this.ProcessMode(Mode.Insert);
                }
                else
                {
                    //Show data
                    this.ShowData(setting);

                    //Set Mode
                    this.ProcessMode(Mode.View);
                }
            }
            else
            {
                //Process Upload File
                this.ProcessUploadFile();
            }
        }

        /// <summary>
        /// Event Edit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            //Get setting
            M_Setting setting = this.GetSetting();

            //Check setting
            if (setting != null)
            {
                //Show data
                this.ShowData(setting);

                //Set Mode
                this.ProcessMode(Mode.Update);
            }
            else
            {
                Server.Transfer("../Menu/FrmMasterMenu.aspx");
            }
        }

        /// <summary>
        /// Event Insert Submit
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
        /// Event Update Submit
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
        /// Event Back
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            //Get setting
            M_Setting setting = this.GetSetting();

            //Check setting
            if (setting != null)
            {
                //Show data
                this.ShowData(setting);

                //Set Mode
                this.ProcessMode(Mode.View);
            }
            else
            {
                Server.Transfer("../Menu/FrmMasterMenu.aspx");
            }
        }

        /// <summary>
        /// Process Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnProcessData(object sender, EventArgs e)
        {
            //Save the images into the Logo folder
            this.SaveImage();

            //Save the file into the Logo folder
            this.SaveFile();

            //Check Mode
            switch (this.Mode)
            {
                case Utilities.Mode.Insert:
                    //Insert Data
                    if (this.InsertData())
                    {
                        M_Setting setting = this.GetSetting();

                        //Show data
                        this.ShowData(setting);

                        //Set Mode
                        this.ProcessMode(Mode.View);

                        //Set Success
                        this.Success = true;
                    }
                    break;

                default:

                    //Update Data
                    if (this.UpdateData())
                    {

                        M_Setting setting = this.GetSetting();

                        //Show data
                        this.ShowData(setting);

                        //Set Mode
                        this.ProcessMode(Mode.View);

                        //Set Success
                        this.Success = true;
                    }

                    break;
            }

        }

        #endregion

        #region Method

        /// <summary>
        /// Event changed page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDownload_Click(object sender, EventArgs e)
        {
            M_Setting set = this.GetSetting();
            string pathFile = string.Empty;
            string filename = string.Empty;
            string filePath = string.Empty;
   
            if (this.hdnFileName.Value == "11")
            {
                pathFile = MapPath(CONST_IMAGES_PATH);
                var logo = set.Logo1;
                if (File.Exists(pathFile + logo))
                {
                    filePath = pathFile + logo;
                    filename = set.Logo1;
                }
            }
            else if (this.hdnFileName.Value == "12")
            {
                pathFile = MapPath(CONST_IMAGES_PATH);
                var logo = set.Logo2;
                if (File.Exists(pathFile + logo))
                {
                    filePath = pathFile + logo;
                    filename = set.Logo2;
                }
            }

            if (File.Exists(filePath))
            {
                Response.ClearContent();
                Response.Clear();
                Response.ContentType = "text/plain";
                Response.AddHeader("Content-Disposition", string.Format("attachment; filename = \"{0}\"", filename));
                Response.TransmitFile(filePath);
                Response.Flush();
                Response.End();
            }
        }

        /// <summary>
        /// Event changed page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClear_Click(object sender, EventArgs e)
        {
           
        }

        /// <summary>
        /// Init Maxlength for the controls
        /// </summary>
        private void InitMaxLengthControl()
        {
            this.txtAttachPath.MaxLength = M_Setting.ATTACH_PATH_MAX_LENGTH;
            this.txtExtension.MaxLength = M_Setting.EXTENSION_MAX_LENGTH;
           
        }

        #region Get

        /// <summary>
        /// Get active control
        /// </summary>
        /// <returns>ControlID</returns>
        private string GetActiveControl()
        {
            Control control = null;
            string ctrlname = this.Request.Params.Get(CONST_TARGET_EVENT);
            if (ctrlname != null && ctrlname != string.Empty)
            {
                control = this.FindControl(ctrlname);
            }
            else
            {
                foreach (string ctl in this.Request.Form)
                {
                    Control c = this.FindControl(ctl);
                    if (c is System.Web.UI.WebControls.Button)
                    {
                        control = c;
                        break;
                    }
                }
            }

            return control == null ? String.Empty : control.ID;
        }

        /// Get setting
        /// </summary>
        /// <returns>M_Setting model</returns>
        private M_Setting GetSetting()
        {
            using (DB db = new DB())
            {
                SettingService settingSer = new SettingService(db);

                //Get Setting
                return settingSer.GetData();
            }
        }

        /// <summary>
        /// Get the images source 
        /// </summary>
        /// <param name="fileUpload">FileUpload</param>
        /// <param name="txtLogo">Logo Textbox</param>
        /// <param name="img">Image</param>
        /// <param name="logoSize">LogoSize</param>
        private void GetImageSrc(FileUpload fileUpload, Controls.ITextBox txtLogo, HtmlImage img, ref long logoSize, int number)
        {
            //Create the LogoTemp directory
            string tmpDirectory = CONST_IMAGES_PATH_TEMP;
            string fileName;
            if (!Directory.Exists(MapPath(tmpDirectory)))
            {
                Directory.CreateDirectory(MapPath(tmpDirectory));
            }

            //Get file name, file size and save the Logo into the Logo directory
            string tmpPathLogo;
            string pathLogo = MapPath(CONST_IMAGES_PATH);

            string extension = string.Empty;

            //
            img.Src = CONST_IMAGES_PATH + CONST_NO_IMAGE_AVAILABLE;

            if ((fileUpload.PostedFile != null && fileUpload.PostedFile.ContentLength > 0) || (!txtLogo.IsEmpty))
            {
                if (fileUpload.PostedFile != null && fileUpload.PostedFile.ContentLength > 0)
                {
                    txtLogo.Value = fileUpload.PostedFile.FileName;
                    var fileNameTemp = this.FormatFileName(txtLogo.Value, number);
                    extension = System.IO.Path.GetExtension(fileNameTemp);
                    fileUpload.SaveAs(MapPath(tmpDirectory + "/" + fileNameTemp));
                    tmpPathLogo = tmpDirectory + "/" + fileNameTemp;
                }
                else
                {
                    fileName = txtLogo.Value;
                    var fileNameTemp = this.FormatFileName(txtLogo.Value, number);
                    tmpPathLogo = tmpDirectory + "/" + fileNameTemp;
                    if (File.Exists(pathLogo + fileName) && !File.Exists(MapPath(tmpPathLogo)))
                    {
                        File.Copy(pathLogo + fileName, MapPath(tmpPathLogo));
                    }
                }

                if (File.Exists(MapPath(tmpPathLogo)))
                {
                    img.Src = tmpPathLogo + "?time=" + DateTime.Now.ToString();
                    logoSize = new FileInfo(MapPath(tmpPathLogo)).Length;
                    img.Style.Clear();
                }
            }

        }

        /// <summary>
        /// Get the File source 
        /// </summary>
        /// <param name="fileUpload">FileUpload</param>
        /// <param name="txtFile">Logo Textbox</param>
        private void GetFileSrc(FileUpload fileUpload, Controls.ITextBox txtFile, ref long fileSize, int number)
        {
            //Create the LogoTemp directory
            string tmpDirectory = CONST_FILE_PATH_TEMP;
            string fileName;
            if (!Directory.Exists(MapPath(tmpDirectory)))
            {
                Directory.CreateDirectory(MapPath(tmpDirectory));
            }

            //Get file name, file size and save the Logo into the Logo directory
            string tmpPathFile;
            string pathFile = MapPath(CONST_FILE_PATH);

            string extension = string.Empty;

            if ((fileUpload.PostedFile != null && fileUpload.PostedFile.ContentLength > 0) || (!txtFile.IsEmpty))
            {
                if (fileUpload.PostedFile != null && fileUpload.PostedFile.ContentLength > 0)
                {
                    txtFile.Value = fileUpload.PostedFile.FileName;
                    var fileNameTemp = this.FormatFileName(txtFile.Value, number);
                    extension = System.IO.Path.GetExtension(fileNameTemp);
                    fileUpload.SaveAs(MapPath(tmpDirectory + "/" + fileNameTemp));
                    tmpPathFile = tmpDirectory + "/" + fileNameTemp;
                }
                else
                {
                    fileName = txtFile.Value;
                    var fileNameTemp = this.FormatFileName(txtFile.Value, number);
                    extension = System.IO.Path.GetExtension(fileName);
                    tmpPathFile = tmpDirectory + "/" + fileNameTemp;
                    if (File.Exists(pathFile + fileName) && !File.Exists(MapPath(tmpPathFile)))
                    {
                        File.Copy(pathFile + fileName, MapPath(tmpPathFile));
                    }
                }

                if (File.Exists(MapPath(tmpPathFile)))
                {
                    fileSize = new FileInfo(MapPath(tmpPathFile)).Length;
                }
            }
        }

        #endregion

        #region Process upload logo

        /// <summary>
        /// Process Upload File
        /// </summary>
        private void ProcessUploadFile()
        {
            //Get active control
            string activeCtrlId = this.GetActiveControl();
            if (activeCtrlId == btnBack1.ID || activeCtrlId == btnEdit.ID) return;

            //Get the image source
            this.GetImageSrc(this.fileLogo1, this.txtLogo1, this.imgLogo1, ref this._logo1Size, 1);
            this.GetImageSrc(this.fileLogo2, this.txtLogo2, this.imgLogo2, ref this._logo2Size, 2);
        }

        /// <summary>
        /// Save the images into the Logo folder
        /// </summary>
        private void SaveImage()
        {
            //Check the LogoTemp folder's existing
            if (Directory.Exists(MapPath(CONST_IMAGES_PATH_TEMP)))
            {
                //Get files in the LogoTemp folder
                string[] fileTemp = Directory.GetFiles(MapPath(CONST_IMAGES_PATH_TEMP));
                string[] logoList = Directory.GetFiles(MapPath(CONST_IMAGES_PATH));
                string tmpPathLogo1 = CONST_IMAGES_PATH_TEMP + "/" + this.FormatFileName(this.txtLogo1.Value, 1);
                string tmpPathLogo2 = CONST_IMAGES_PATH_TEMP + "/" + this.FormatFileName(this.txtLogo2.Value, 2);

                string pathLogo1 = MapPath(CONST_IMAGES_PATH) + this.txtLogo1.Value;
                string pathLogo2 = MapPath(CONST_IMAGES_PATH) + this.txtLogo2.Value;

                M_Setting setting = new M_Setting();
                using (var db = new DB())
                {
                    SettingService settingSer = new SettingService(db);
                    setting = settingSer.GetData();
                }

                foreach (string file in fileTemp)
                {
                    //Logo1
                    if (file.Equals(MapPath(tmpPathLogo1)))
                    {
                        //Delete Logo1 in the Logo folder
                        foreach (string logo in logoList)
                        {
                            if (!logo.Contains(this.txtLogo1.Value) && !logo.Contains(this.txtLogo2.Value) && !logo.Contains("no-image.png"))
                            {
                                File.Delete(logo);
                            }
                        }

                        if (setting != null && !string.IsNullOrEmpty(setting.Logo1) && File.Exists(MapPath(CONST_IMAGES_PATH + this.txtLogo1.Value)))
                        {

                            File.Delete(MapPath(CONST_IMAGES_PATH + this.txtLogo1.Value));
                        }

                        //Move logo1 from the LogoTemp folder to the Logo folder
                        File.Copy(file, pathLogo1);

                    }

                    //Logo2
                    if (file.Equals(MapPath(tmpPathLogo2)))
                    {
                        //Delete Logo2 in the Logo folder
                        foreach (string logo in logoList)
                        {
                            if (!logo.Contains(this.txtLogo1.Value) && !logo.Contains(this.txtLogo2.Value) && !logo.Contains("no-image.png"))
                            {
                                File.Delete(logo);
                            }
                        }

                        if (setting != null && !string.IsNullOrEmpty(setting.Logo2) && File.Exists(MapPath(CONST_IMAGES_PATH + this.txtLogo2.Value)))
                        {

                            File.Delete(MapPath(CONST_IMAGES_PATH + this.txtLogo2.Value));
                        }

                        //Move logo2 from the LogoTemp folder to the Logo folder
                        File.Copy(file, pathLogo2);
                    }
                }
            }
        }

        /// <summary>
        /// Save the file into the Logo folder
        /// </summary>
        private void SaveFile()
        {
            //Check the LogoTemp folder's existing
            if (Directory.Exists(MapPath(CONST_FILE_PATH_TEMP)))
            {
                //Get files in the LogoTemp folder
                string[] fileTemp = Directory.GetFiles(MapPath(CONST_FILE_PATH_TEMP));
                string[] fileList = Directory.GetFiles(MapPath(CONST_FILE_PATH));

                M_Setting setting = new M_Setting();
                using (var db = new DB())
                {
                    SettingService settingSer = new SettingService(db);
                    setting = settingSer.GetData();
                }

                foreach (string files in fileTemp)
                {
         
                }

            }
        }

        #endregion

        #region Process Mode

        /// <summary>
        /// Process Mode
        /// </summary>
        /// <param name="mode">Mode</param>
        private void ProcessMode(Mode mode)
        {
            bool enable;

            //Set Model
            this.Mode = mode;

            //Check model
            switch (mode)
            {
                case Mode.Insert:
                case Mode.Update:
                    enable = false;
                   
                    break;

                default:
                    enable = true;
                   

                    base.DisabledLink(this.btnEdit, !base._authority.IsMasterEdit);
                    break;
            }

            //Lock control
            this.txtAttachPath.ReadOnly = enable;
           
            this.btnBrowse11.Disabled = enable;
            this.btnBrowse12.Disabled = enable;

            this.btnDownloadLogo1.Disabled = !enable;
            this.btnDownloadLogo2.Disabled = !enable;

            this.txtExtension.ReadOnly = enable;
        }

        #endregion

        #region Show data

        /// <summary>
        /// Show data on form
        /// <param name="setting">M_Setting</param>
        /// </summary>
        private void ShowData(M_Setting setting)
        {
            if (setting != null)
            {
                this.txtAttachPath.Value = setting.AttachPath;
               
                this.txtLogo1.Value = setting.Logo1;
                this.txtLogo2.Value = setting.Logo2;

                string pathLogo1 = CONST_IMAGES_PATH + this.txtLogo1.Value;
                if (File.Exists(MapPath(pathLogo1)))
                {
                    this.imgLogo1.Src = pathLogo1 + "?time=" + DateTime.Now.ToString();
                    this.imgLogo1.Style.Clear();
                }
                else
                {
                    this.imgLogo1.Src = CONST_IMAGES_PATH + CONST_NO_IMAGE_AVAILABLE;
                    this.imgLogo1.Attributes.Add("style", "width:175px;height:160px");
                }

                string pathLogo2 = CONST_IMAGES_PATH + this.txtLogo2.Value;
                if (File.Exists(MapPath(pathLogo2)))
                {
                    this.imgLogo2.Src = pathLogo2 + "?time=" + DateTime.Now.ToString();
                    this.imgLogo2.Style.Clear();
                }
                else
                {
                    this.imgLogo2.Src = CONST_IMAGES_PATH + CONST_NO_IMAGE_AVAILABLE;
                    this.imgLogo2.Attributes.Add("style", "width:175px;height:160px");
                }

                this.txtExtension.Value = setting.Extension;
                //Save SettingID and UpdateDate
                this.SettingID = setting.ID;
                this.OldUpdateDate = setting.UpdateDate;
            }

            //Delete the LogoTemp folder
            if (Directory.Exists(MapPath(CONST_IMAGES_PATH_TEMP)))
            {
                string[] fileList = Directory.GetFiles(MapPath(CONST_IMAGES_PATH_TEMP));
                foreach (string file in fileList)
                {
                    File.Delete(file);
                }

                //Directory.Delete(MapPath(CONST_IMAGES_PATH_TEMP), true);
            }

            //Delete the LogoTemp folder
            if (Directory.Exists(MapPath(CONST_FILE_PATH_TEMP)))
            {
                string[] fileList = Directory.GetFiles(MapPath(CONST_FILE_PATH_TEMP));
                foreach (string file in fileList)
                {
                    File.Delete(file);
                }

                //Directory.Delete(MapPath(CONST_IMAGES_PATH_TEMP), true);
            }

        }


        #endregion

        #region Check Input

        /// <summary>
        /// Check Input
        /// </summary>
        /// <returns>Valid:true, Invalid:false</returns>
        private bool CheckInput()
        {
            //Attach Path.
            if (this.txtAttachPath.IsEmpty)
            {
                this.SetMessage(this.txtAttachPath.ID, M_Message.MSG_REQUIRE, "Attach Path");
            }
            else
            {
                if (!Directory.Exists(this.txtAttachPath.Value))
                {
                    this.SetMessage(this.txtAttachPath.ID, M_Message.MSG_VALUE_NOT_EXIST, "Attach Path");
                }
            }

            var lstFileName = new List<string>();
            var lstFileNameSale = new List<string>();

            //Logo1
            if (this.txtLogo1.IsEmpty)
            {
                this.SetMessage(this.txtLogo1.ID, M_Message.MSG_REQUIRE, "Logo1");
                this.imgLogo1.Src = CONST_IMAGES_PATH + CONST_NO_IMAGE_AVAILABLE;
                this.imgLogo1.Attributes.Add("style", "width:175px;height:160px");
            }
            else
            {
                //Check the existing of Logo1
                string pathLogo1 = MapPath(CONST_IMAGES_PATH + this.txtLogo1.Value);
                string pathTemp1 = MapPath(CONST_IMAGES_PATH_TEMP + "/" + this.FormatFileName(this.txtLogo1.Value, 1));
                if (!File.Exists(pathLogo1) && !File.Exists(pathTemp1))
                {
                    this.SetMessage(this.txtLogo1.ID, M_Message.MSG_VALUE_NOT_EXIST, "Logo1");
                    this.imgLogo1.Src = CONST_IMAGES_PATH + CONST_NO_IMAGE_AVAILABLE;
                    this.imgLogo1.Attributes.Add("style", "width:175px;height:160px");
                }
                else
                {
                    if (!this.IsImage(this.txtLogo1.Value))
                    {
                        this.SetMessage(this.txtLogo1.ID, M_Message.MSG_INPUT_IMAGE, "Logo1");
                        this.imgLogo1.Src = CONST_IMAGES_PATH + CONST_NO_IMAGE_AVAILABLE;
                        this.imgLogo1.Attributes.Add("style", "width:175px;height:160px");
                    }
                    else
                    //Check logo1 size
                    {
                        if (this._logo1Size > CONST_IMAGE_MAX_SIZE)
                        {
                            this.SetMessage(this.txtLogo1.ID, M_Message.MSG_SIZE_FILE_UPLOAD_LESS_THAN_EQUAL, "Logo1", CONST_IMAGE_MAX_SIZE_STRING);
                            this.imgLogo1.Src = CONST_IMAGES_PATH + CONST_NO_IMAGE_AVAILABLE;
                            this.imgLogo1.Attributes.Add("style", "width:175px;height:160px");
                        }
                    }
                }
            }

            //Logo2
            if (this.txtLogo2.IsEmpty)
            {
                this.SetMessage(this.txtLogo2.ID, M_Message.MSG_REQUIRE, "Logo2");
                this.imgLogo2.Src = CONST_IMAGES_PATH + CONST_NO_IMAGE_AVAILABLE;
                this.imgLogo2.Attributes.Add("style", "width:175px;height:160px");
            }
            else
            {
                //Check the existing of Logo2
                string pathLogo10 = MapPath(CONST_IMAGES_PATH + this.txtLogo2.Value);
                string pathTemp10 = MapPath(CONST_IMAGES_PATH_TEMP + "/" + this.FormatFileName(this.txtLogo2.Value, 2));
                if (!File.Exists(pathLogo10) && !File.Exists(pathTemp10))
                {
                    this.SetMessage(this.txtLogo2.ID, M_Message.MSG_VALUE_NOT_EXIST, "Logo2");
                    this.imgLogo2.Src = CONST_IMAGES_PATH + CONST_NO_IMAGE_AVAILABLE;
                    this.imgLogo2.Attributes.Add("style", "width:175px;height:160px");
                }
                else
                {
                    if (!this.IsImage(this.txtLogo2.Value))
                    {
                        this.SetMessage(this.txtLogo2.ID, M_Message.MSG_INPUT_IMAGE, "Logo2");
                        this.imgLogo2.Src = CONST_IMAGES_PATH + CONST_NO_IMAGE_AVAILABLE;
                        this.imgLogo2.Attributes.Add("style", "width:175px;height:160px");
                    }
                    else
                        if (this._logo2Size > CONST_IMAGE_MAX_SIZE)
                        {
                            this.SetMessage(this.txtLogo2.ID, M_Message.MSG_SIZE_FILE_UPLOAD_LESS_THAN_EQUAL, "Logo2", CONST_IMAGE_MAX_SIZE_STRING);
                            this.imgLogo2.Src = CONST_IMAGES_PATH + CONST_NO_IMAGE_AVAILABLE;
                            this.imgLogo2.Attributes.Add("style", "width:175px;height:160px");
                        }
                }
            }

            if (this.txtLogo1.Value == this.txtLogo2.Value)
            {
                this.SetMessage(this.txtLogo2.ID, M_Message.MSG_MUST_BE_DIFFERENT, "Logo2", "Logo1");
            }

            //Check error
            return !base.HaveError;
        }

        /// <summary>
        /// Format File Name
        /// </summary>
        /// <param name="textBox">textBox</param>
        /// <param name="number">number</param>
        /// <returns>fileName</returns>
        private string FormatFileName(string value, int number)
        {
            string fileName = string.Empty;
            if (!string.IsNullOrEmpty(value))
            {
                var extension = Path.GetExtension(value);
                fileName = Path.GetFileNameWithoutExtension(value) + "_" + number.ToString() + extension;
            }
            return fileName;
        }

        /// <summary>
        /// Check file is image
        /// </summary>
        /// <param name="postedFile"></param>
        /// <returns>Valid: True, Invalid: False</returns>
        private bool IsImage(string fileName)
        {
            //-------------------------------------------
            //  Check the image extension
            //-------------------------------------------
            if (Path.GetExtension(fileName).ToLower() != ".jpg"
                && Path.GetExtension(fileName).ToLower() != ".png"
                && Path.GetExtension(fileName).ToLower() != ".gif"
                && Path.GetExtension(fileName).ToLower() != ".jpeg"
                && Path.GetExtension(fileName).ToLower() != ".bmp")
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Check file is excel
        /// </summary>
        /// <param name="postedFile"></param>
        /// <returns>Valid: True, Invalid: False</returns>
        private bool IsExcel(string fileName)
        {
            //-------------------------------------------
            //  Check the image extension
            //-------------------------------------------
            if (Path.GetExtension(fileName).ToLower() != ".xls" && Path.GetExtension(fileName).ToLower() != ".xlsx")
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Create data

        /// <summary>
        /// Get data to insert
        /// </summary>
        /// <returns>M_Setting model</returns>
        private M_Setting CreateDataInsert()
        {
            M_Setting setting = new M_Setting();
            setting.AttachPath = this.txtAttachPath.Value;
            setting.Logo1 = this.txtLogo1.Value;
            setting.Logo2 = this.txtLogo2.Value;
            setting.Extension = this.txtExtension.Value;
            setting.CreateUID = this.LoginInfo.User.ID;
            setting.UpdateUID = this.LoginInfo.User.ID;

            return setting;
        }

        /// <summary>
        /// Set data to update
        /// </summary>
        /// <returns>M_Setting model</returns>
        private M_Setting CreateDataUpdate()
        {
            M_Setting setting = new M_Setting();

            setting.AttachPath = this.txtAttachPath.Value;
            setting.Logo1 = this.txtLogo1.Value;
            setting.Logo2 = this.txtLogo2.Value;
            setting.Extension = this.txtExtension.Value;
            setting.UpdateDate = this.OldUpdateDate;
            setting.UpdateUID = this.LoginInfo.User.ID;
            return setting;
        }

        #endregion

        #region Insert data

        /// <summary>
        /// Insert
        /// </summary>
        private bool InsertData()
        {
            try
            {
                M_Setting setting = new M_Setting();

                //Create model
                setting = this.CreateDataInsert();

                //Insert Data
                using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                {
                    SettingService settingSer = new SettingService(db);
                    settingSer.Insert(setting);
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
        #endregion

        #region Update data

        /// <summary>
        /// Update Data
        /// </summary>
        private bool UpdateData()
        {
            try
            {
                int ret = 0;
                M_Setting setting = new M_Setting();

                //Create model
                setting = this.CreateDataUpdate();
                using (DB db = new DB(System.Data.IsolationLevel.Serializable))
                {
                    SettingService settingSer = new SettingService(db);
                    if (setting.Status == DataStatus.Changed)
                    {
                        ret = settingSer.Update(setting);
                        db.Commit();
                    }
                }

                if (ret == 0)
                {
                    this.SetMessage(string.Empty, M_Message.MSG_DATA_CHANGED);
                    return false;
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

        #endregion

        #endregion

    }
}

