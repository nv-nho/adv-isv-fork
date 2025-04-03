using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Reflection;
using System.Collections;
using System.IO;

using Microsoft.Reporting.WebForms;

using OMS.DAC;
using OMS.Models;
using OMS.Utilities;

using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.Util;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Windows.Forms;

namespace OMS
{
    /// <summary>
    /// Class Form Base
    /// </summary>
    public abstract class FrmBase : System.Web.UI.Page
    {
        #region Variable
        protected AuthorityInfo _authority;
        #endregion

        #region Property
        /// <summary>
        /// Message List
        /// </summary>
        protected IDictionary<string, M_Message> Messages { get; private set; }

        protected IList<string> CtrlIDErrors
        {
            get
            {
                return (IList<string>)base.ViewState["CTRL_ERROR_ID"];
            }
            private set
            {
                base.ViewState["CTRL_ERROR_ID"] = value;
            }
        }
        protected IList<string> MsgErrors
        {
            get
            {
                return (IList<string>)base.ViewState["MSG_ERROR"];
            }
            private set
            {
                base.ViewState["MSG_ERROR"] = value;
            }
        }

        protected IList<string> CtrlIDInfos
        {
            get
            {
                return (IList<string>)base.ViewState["CTRL_INFO"];
            }
            set
            {
                base.ViewState["CTRL_INFO"] = value;
            }
        }
        protected IList<string> MsgInfos
        {
            get
            {
                return (IList<string>)base.ViewState["MSG_INFO"];
            }
            set
            {
                base.ViewState["MSG_INFO"] = value;
            }
        }

        /// <summary>
        /// Get LoginInfo
        /// </summary>
        protected LoginInfo LoginInfo
        {
            get { return (LoginInfo)Session["LoginInfo"]; }
        }

        /// <summary>
        /// Get PreviousPageViewState
        /// </summary>
        protected StateBag PreviousPageViewState
        {
            get
            {
                StateBag returnValue = null;
                if (PreviousPage != null)
                {
                    Object objPreviousPage = (Object)PreviousPage;
                    MethodInfo objMethod = objPreviousPage.GetType().GetMethod("ReturnViewState");
                    return (StateBag)objMethod.Invoke(objPreviousPage, null);
                }
                return returnValue;
            }
        }

        /// <summary>
        /// Get HaveError
        /// </summary>
        protected bool HaveError
        {
            get
            {
                if (this.MsgErrors.Count == 0 && this.MsgInfos.Count == 0)
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Get or set FormTitle
        /// </summary>
        public string FormTitle
        {
            get
            {
                SiteMaster master = (SiteMaster)this.Master;
                return master.FormTitle;
            }
            set
            {
                SiteMaster master = (SiteMaster)this.Master;
                master.FormTitle = value;
            }
        }

        /// <summary>
        /// Get or set FormSubTitle
        /// </summary>
        public string FormSubTitle
        {
            get
            {
                SiteMaster master = (SiteMaster)this.Master;
                return master.FormSubTitle;
            }
            set
            {
                SiteMaster master = (SiteMaster)this.Master;
                master.FormSubTitle = value;
            }
        }

        /// <summary>
        /// Get IsOutFile
        /// </summary>
        protected bool IsOutFile
        {
            get
            {
                if (ViewState["OUTFILE"] != null)
                {
                    if (File.Exists(ViewState["OUTFILE"].ToString()))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        #endregion

        #region Event

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //Get list message

            using (DB db = new DB())
            {
                MessageService msgSer = new MessageService(db);
                this.Messages = msgSer.GetAll().ToDictionary(key => key.MessageID, value => value);
            }

            this.CtrlIDErrors = new List<string>();
            this.MsgErrors = new List<string>();

            this.CtrlIDInfos = new List<string>();
            this.MsgInfos = new List<string>();
        }

        #endregion

        #region Method

        /// <summary>
        /// Get ReturnViewState
        /// </summary>
        public StateBag ReturnViewState()
        {
            return ViewState;
        }

        /// <summary>
        /// Set error messsage
        /// </summary>
        /// <param name="ctrlID">Error ControlID</param>
        /// <param name="msgID">Message Id</param>
        /// <param name="args">List argument of messsage</param>
        protected void SetMessage(string ctrlID, string msgID, params object[] args)
        {
            //Get Message
            M_Message mess = (M_Message)this.Messages[msgID];
            //Check Message Type
            switch (mess.Type)
            {
                case "E":
                    this.MsgErrors.Add(string.Format("<h5>{0}</h5>", string.Format(mess.Message3, args)));
                    this.CtrlIDErrors.Add(ctrlID);

                    break;
                case "I":
                    this.MsgErrors.Add(string.Format("<h5>{0}</h5>", string.Format(mess.Message3, args)));
                    this.CtrlIDInfos.Add(ctrlID);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Get error class name for div of control input
        /// </summary>
        /// <param name="ctrlID">ControlID</param>
        /// <returns>error class name</returns>
        protected virtual string GetClassError(string ctrlID)
        {
            if (this.CtrlIDErrors.Contains(ctrlID))
            {
                return "has-error";
            }
            if (this.CtrlIDInfos.Contains(ctrlID))
            {
                return " has-warning";
            }
            return string.Empty;
        }

        /// <summary>
        /// Get display message
        /// </summary>
        /// <returns>HTML of error message</returns>
        public string GetMessage()
        {
            string ret = string.Empty;
            if (this.MsgErrors.Count > 0)
            {
                ret += "<div id='panelError' class='alert alert-danger alert-dismissible' role='alert'>";
                ret += "<button type='button' class='close' data-dismiss='alert'><span aria-hidden='true'>&times;</span><span class='sr-only'></span></button>";
                ret += "<span class='glyphicon glyphicon-remove-sign'></span><strong> 警告!</strong>";

                int i = 0;
                string msg1 = string.Empty;
                foreach (var msg in this.MsgErrors)
                {
                    msg1 += msg;
                    i++;
                }

                ret += msg1;
                ret += "</div>";

            }

            if (this.MsgInfos.Count > 0)
            {
                ret += "<div id='panelError' class='alert alert-info alert-dismissible' role='alert' runat='server'>";
                ret += "<button type='button' class='close' data-dismiss='alert'><span aria-hidden='true'>&times;</span><span class='sr-only'></span></button>";
                ret += "<span class='glyphicon glyphicon-info-sign'></span><strong> 情報!</strong>";

                int i = 0;
                string msg1 = string.Empty;
                foreach (var msg in this.MsgInfos)
                {
                    msg1 += msg;
                    i++;
                }

                ret += msg1;
                ret += "</div>";
            }

            return ret;
        }

        /// <summary>
        /// Get Color For Grid Data
        /// ISV-TRUC
        /// </summary>
        /// <param name="color">color</param>
        /// <returns></returns>
        public string GetColorClass(int color)
        {
            string ret = string.Empty;

            if (color == (int)ColorList.Danger)
            {
                ret = "danger";
            }
            else if (color == (int)ColorList.Warning)
            {
                ret = "warning";
            }
            else if (color == (int)ColorList.Info)
            {
                ret = "info";
            }
            else if (color == (int)ColorList.Success)
            {
                ret = "success";
            }
            else if (color == (int)ColorList.Finish)
            {
                ret = "finish";
            }

            return ret;
        }

        /// <summary>
        /// Get Authority
        /// </summary>
        /// <param name="formId"></param>
        /// <returns></returns>
        protected void SetAuthority(Utilities.FormId formId)
        {
            this._authority = new AuthorityInfo();
            M_GroupUser_D grpUser = this.LoginInfo.ListGroupDetail.Where(m => m.FormID == (int)formId).FirstOrDefault();

            if (grpUser != null)
            {
                //Work Calendar
                if (formId == FormId.WorkCalendar)
                {
                    if (grpUser.AuthorityFlag1 == 1)
                    {
                        this._authority.IsWorkCalendarView = true;
                    }

                    if (grpUser.AuthorityFlag2 == 1)
                    {
                        this._authority.IsWorkCalendarNew = true;
                    }

                    if (grpUser.AuthorityFlag3 == 1)
                    {
                        this._authority.IsWorkCalendarEdit = true;
                    }

                    if (grpUser.AuthorityFlag4 == 1)
                    {
                        this._authority.IsWorkCalendarDelete = true;
                    }

                    if (grpUser.AuthorityFlag5 == 1)
                    {
                        this._authority.IsWorkCalendarAgreementSetting = true;
                    }

                    if (grpUser.AuthorityFlag6 == 1)
                    {
                        this._authority.IsWorkCalendarExportExcel = true;
                    }
                }

                //Work Schedule
                if (formId == FormId.Attendance)
                {
                    if (grpUser.AuthorityFlag1 == 1)
                    {
                        this._authority.IsAttendanceView = true;
                    }

                    if (grpUser.AuthorityFlag2 == 1)
                    {
                        this._authority.IsAttendanceNew = true;
                    }

                    if (grpUser.AuthorityFlag3 == 1)
                    {
                        this._authority.IsAttendanceEdit = true;
                    }

                    if (grpUser.AuthorityFlag4 == 1)
                    {
                        this._authority.IsAttendanceDelete = true;
                    }

                    if (grpUser.AuthorityFlag5 == 1)
                    {
                        this._authority.IsAttendanceOtherDepartments = true;
                    }

                    if (grpUser.AuthorityFlag6 == 1)
                    {
                        this._authority.IsAttendanceOtherEmployees = true;
                    }
                    if (grpUser.AuthorityFlag7 == 1)
                    {
                        this._authority.IsAttendanceOtherUpdates = true;
                    }
                    if (grpUser.AuthorityFlag8 == 1)
                    {
                        this._authority.IsAttendanceShelfRegistration = true;
                    }

                    if (grpUser.AuthorityFlag9 == 1)
                    {
                        this._authority.IsAttendanceExportExcel = true;
                    }
                }

                //Attendance Approvel
                if (formId == FormId.AttendanceApproval)
                {
                    if (grpUser.AuthorityFlag1 == 1)
                    {
                        this._authority.IsAttendanceApprovalView = true;
                    }

                    if (grpUser.AuthorityFlag2 == 1)
                    {
                        this._authority.IsAttendanceApprovalApprovel = true;
                    }
                    if (grpUser.AuthorityFlag3 == 1)
                    {
                        this._authority.IsAttendanceApprovalReject = true;
                    }
                }

                //Attendance Summary
                if (formId == FormId.AttendanceSummary)
                {
                    if (grpUser.AuthorityFlag1 == 1)
                    {
                        this._authority.IsAttendanceSummaryView = true;
                    }

                    if (grpUser.AuthorityFlag2 == 1)
                    {
                        this._authority.IsAttendanceSummaryExportExcel = true;
                    }
                }

                //Master
                //if (grpUser.AuthorityFlag1 == 1)
                //{
                this._authority.IsMasterView = true;
                //}

                if (grpUser.AuthorityFlag2 == 1)
                {
                    this._authority.IsMasterNew = true;
                }

                if (grpUser.AuthorityFlag3 == 1)
                {
                    this._authority.IsMasterEdit = true;
                }

                if (grpUser.AuthorityFlag4 == 1)
                {
                    this._authority.IsMasterCopy = true;
                }

                if (grpUser.AuthorityFlag5 == 1)
                {
                    this._authority.IsMasterDelete = true;
                }

                if (grpUser.AuthorityFlag6 == 1)
                {
                    this._authority.IsMasterExcel = true;
                }

                //Attendance Payslip
                if (formId == FormId.AttendancePayslip)
                {
                    if (grpUser.AuthorityFlag1 == 1)
                    {
                        this._authority.IsAttendancePayslipView = true;
                    }

                    if (grpUser.AuthorityFlag2 == 1)
                    {
                        this._authority.IsAttendancePayslipDepartments = true;
                    }

                    if (grpUser.AuthorityFlag3 == 1)
                    {
                        this._authority.IsAttendancePayslipEmployees = true;
                    }

                    if (grpUser.AuthorityFlag4 == 1)
                    {
                        this._authority.IsAttendancePayslipUpload = true;
                    }
                }
                //mail entry
                if (formId == FormId.SendMail)
                {
                    // 表示
                    if (grpUser.AuthorityFlag1 == 1)
                    {
                        this._authority.IsMailView = true;
                    }
                    // 新規
                    if (grpUser.AuthorityFlag2 == 1)
                    {
                        this._authority.IsMailSend = true;
                        //this._authority.IsMailReSend = true;
                    }
                    // 編集
                    if (grpUser.AuthorityFlag3 == 1)
                    {
                        // 【編集】＝OFFの場合、「編集」、「再送信」、「未返信者のみ再送信」ボタンは押下できないようにする。
                        this._authority.IsMailEdit = true;
                        this._authority.IsMailReSend = true;
                    }
                    // 削除 
                    if (grpUser.AuthorityFlag5 == 1)
                    {
                        this._authority.IsMailDelete = true;
                    }
                }


                if (formId == FormId.Cost)
                {
                    if (grpUser.AuthorityFlag1 == 1)
                    {
                        this._authority.IsMasterView = true;
                    }
                    else
                    {
                        this._authority.IsMasterView = false;
                    }
                }

                if (formId == FormId.ExpenceGroup)
                {
                    if (grpUser.AuthorityFlag1 == 1)
                    {
                        this._authority.IsExpenceView = true;
                    }

                    if (grpUser.AuthorityFlag2 == 1)
                    {
                        this._authority.IsExpenceNew = true;
                    }

                    if (grpUser.AuthorityFlag3 == 1)
                    {
                        this._authority.IsExpenceEdit = true;
                    }

                    if (grpUser.AuthorityFlag4 == 1)
                    {
                        this._authority.IsExpenceCopy = true;
                    }
                    if (grpUser.AuthorityFlag5 == 1)
                    {
                        this._authority.IsExpenceDelete = true;
                    }
                    if (grpUser.AuthorityFlag6 == 1)
                    {
                        this._authority.IsExpenceOtherApply = true;
                    }

                    if (grpUser.AuthorityFlag7 == 1)
                    {
                        this._authority.IsExpenceAccept = true;
                    }

                    if (grpUser.AuthorityFlag8 == 1)
                    {
                        this._authority.IsExpencemutilAccept = true;
                    }

                    if (grpUser.AuthorityFlag9 == 1)
                    {
                        this._authority.IsExpenceAllApproved = true;
                    }
                    if (grpUser.AuthorityFlag10 == 1)
                    {
                        this._authority.IsExpenceExportExcel = true;
                    }
                    if (grpUser.AuthorityFlag11 == 1)
                    {
                        this._authority.IsExpenceExportExcel2= true;
                    }
                    if (grpUser.AuthorityFlag12 == 1)
                    {
                        this._authority.IsExpenceMail = true;
                    }
                }

                if (formId == FormId.ProjectProfit)
                {
                    if (grpUser.AuthorityFlag1 == 1)
                    {
                        this._authority.IsMasterView = true;
                    }
                    else
                    {
                        this._authority.IsMasterView = false;
                    }
                }

                if (formId == FormId.Approval)
                {
                    if (grpUser.AuthorityFlag1 == 1)
                    {
                        this._authority.IsApproval = true;
                    }

                    if (grpUser.AuthorityFlag2 == 1)
                    {
                        this._authority.IsApprovalALL = true;
                    }

                    if (grpUser.AuthorityFlag3 == 1)
                    {
                        this._authority.IsApprovalMail = true;
                    }

                    if (grpUser.AuthorityFlag4 == 1)
                    {
                        this._authority.IsConfirmMail = true;
                    }
                }
            }
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="button"></param>
        /// <param name="disabled"></param>
        protected void DisabledLink(LinkButton button, bool disabled)
        {
            if (disabled)
            {
                button.Enabled = false;
                button.CssClass += " disabled";
            }
            else
            {
                button.Enabled = true;
                button.CssClass = button.CssClass.Replace("disabled", "");
            }
            button.DataBind();
        }

        /// <summary>
        /// Get Value
        /// </summary>
        /// <typeparam name="T">Type of value</typeparam>
        /// <param name="property">property</param>
        /// <returns></returns>
        protected T GetValueViewState<T>(string property)
        {
            var val = base.ViewState[property];
            if (val == null)
            {
                return default(T);
            }
            return (T)val;
        }

        #endregion

        #region Report

        /// <summary>
        /// 
        /// </summary>
        /// <param name="report"></param>
        /// <param name="fileName"></param>
        /// <param name="isPortrait"></param>
        public void ExportPDF(LocalReport report, string fileName, bool isPortrait)
        {
            try
            {
                string REPORT_TYPE = "OUTFILE";
                string MimeType;
                string Encoding;
                string FilenameExtension;

                //Page size
                double pageWidth = isPortrait ? 21 : 29.7;
                double pageHeight = isPortrait ? 29.7 : 21;

                //Margin
                double maginTop = 0.5;
                double maginRight = 0.75;
                double maginBottom = 0.5;
                double maginLeft = 1;

                string DEVICE_INFO = "<DeviceInfo>";
                DEVICE_INFO += "  <OutputFormat>PDF</OutputFormat>";
                DEVICE_INFO += "  <PageWidth>" + pageWidth + "cm</PageWidth>";
                DEVICE_INFO += "  <PageHeight>" + pageHeight + "cm</PageHeight>";
                DEVICE_INFO += "  <MarginTop>" + maginTop + "cm</MarginTop>";
                DEVICE_INFO += "  <MarginRight>" + maginRight + "cm</MarginRight>";
                DEVICE_INFO += "  <MarginBottom>" + maginBottom + "cm</MarginBottom>";
                DEVICE_INFO += "  <MarginLeft>" + maginLeft + "cm</MarginLeft>";
                DEVICE_INFO += "  </DeviceInfo>";

                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                //Render the report
                renderedBytes = report.Render("PDF",
                                                DEVICE_INFO,
                                                out MimeType,
                                                out Encoding,
                                                out FilenameExtension,
                                                out streams,
                                                out warnings);

                var filePath = Server.MapPath("~") + "/TempDownload/" + Guid.NewGuid().ToString() + ".pdf";
                File.WriteAllBytes(filePath, renderedBytes);
                ViewState[REPORT_TYPE] = filePath;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Excel

        /// <summary>
        /// Save File
        /// </summary>
        /// <param name="workBook"></param>
        protected void SaveFile(IWorkbook workBook, string extension = ".xls")
        {
            ViewState["OUTFILE"] = Server.MapPath("~") + "/TempDownload/" + Guid.NewGuid().ToString() + extension;
            using (FileStream temp = new FileStream(ViewState["OUTFILE"].ToString(), FileMode.CreateNew, FileAccess.Write))
            {
                workBook.Write(temp);
            }
        }

        /// <summary>
        /// Create File CSV
        /// </summary>
        protected void CreateFileCSV()
        {
            ViewState["OUTFILE"] = Server.MapPath("~") + "/TempDownload/" + Guid.NewGuid().ToString() + ".csv";
        }

        /// <summary>
        /// Get Data Excel
        /// </summary>
        /// <returns>MemoryStream</returns>
        protected MemoryStream GetFileStream(string fileType)
        {
            var filePath = ViewState[fileType].ToString();
            MemoryStream ret = new MemoryStream();
            using (FileStream temp = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                temp.CopyTo(ret);
            }
            File.Delete(filePath);
            ViewState[fileType] = null;
            return ret;
        }

        protected bool IsAdmin()
        {
            using (DB db = new DB())
            {
                Config_DService ser = new Config_DService(db);
                return ser.GetListByConfigCd(M_Config_H.CONFIG_GROUP_USERCD_ADMIN).Any(c => c.Value3.Equals(this.LoginInfo.User.UserCD));
            }
        }

        #endregion

        #region URL

        #endregion

        #region REF

        /// <summary>
        /// Redirect
        /// </summary>
        /// <param name="url"></param>
        protected void RedirectUrl(string url)
        {
            ViewState.Clear();
            Response.Redirect(url);
        }

        /// <summary>
        /// Transfer
        /// </summary>
        /// <param name="url"></param>
        protected void TransferUrl(string url)
        {
            this.BackPage();
            Server.Transfer(url);
        }

        /// <summary>
        /// Next Page
        /// </summary>
        /// <param name="currentPage">Current Page</param>
        /// <param name="nextPage">Next Page</param>
        /// <param name="backCurentURL">Back Curent URL</param>
        /// <param name="currentUrl">Current Url</param>
        protected void NextPage(Hashtable currentPage, Hashtable nextPage, string backCurentURL, string currentUrl)
        {
            Stack<Hashtable> temp = new Stack<Hashtable>();
            if (this.ViewState["REF"] != null)
            {
                temp = (Stack<Hashtable>)this.ViewState["REF"];
            }

            //Current Page
            currentPage.Add("BACK_URL", backCurentURL);
            temp.Push(currentPage);
            this.ViewState["REF"] = temp;

            //Next Page
            nextPage.Add("BACK_URL", currentUrl);
            this.ViewState["PARAMATER"] = nextPage;
        }

        /// <summary>
        /// Back Page
        /// </summary>
        protected void BackPage()
        {
            if (this.ViewState["REF"] != null)
            {
                Stack<Hashtable> temp = (Stack<Hashtable>)this.ViewState["REF"];
                this.ViewState["PARAMATER"] = temp.Pop();
                this.ViewState["REF"] = temp;
            }
        }

        /// <summary>
        /// Get Paramater
        /// </summary>
        /// <returns>Paramater</returns>
        protected Hashtable GetParamater()
        {
            if (this.ViewState["PARAMATER"] != null)
            {
                return (Hashtable)this.ViewState["PARAMATER"];
            }
            return null;
        }

        /// <summary>
        /// Save Back Page
        /// </summary>
        protected void SaveBackPage()
        {
            if (this.PreviousPageViewState["REF"] != null)
            {
                this.ViewState["REF"] = this.PreviousPageViewState["REF"];
            }

            if (this.PreviousPageViewState["PARAMATER"] != null)
            {
                this.ViewState["PARAMATER"] = this.PreviousPageViewState["PARAMATER"];
            }
        }

        #endregion
    }
}