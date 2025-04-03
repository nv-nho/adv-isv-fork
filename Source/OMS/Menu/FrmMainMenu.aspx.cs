using System;
using System.Collections.Generic;

using OMS.Models;
using OMS.DAC;
using System.Collections;
using OMS.Utilities;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace OMS.Menu
{
    public partial class FrmMainMenu : FrmBase
    {

        #region Property

        /// <summary>
        /// Get or ser IsShowQuestion
        /// </summary>
        public bool IsShowQuestion { get; set; }

        /// <summary>
        /// Get or ser DefaultButton
        /// </summary>
        public string DefaultButton { get; set; }

        #endregion

        #region Event

        /// <summary>
        /// Init page
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //Set Title
            base.FormTitle = "メインメニュー";

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string enableClass = "btn btn-default btn-lg btn-block";
            string disableClass = "btn btn-default btn-lg btn-block disabled";
            
            //Project            
            base.SetAuthority(FormId.Project);
            this.btnProject.Attributes.Add("class", base._authority.IsMasterView ? enableClass : disableClass);

            //WorkCalendar
            bool isExistsCalendar = false;
            using (DB db = new DB())
            {
                T_WorkingCalendar_HService calendarHSer = new T_WorkingCalendar_HService(db);
                isExistsCalendar = calendarHSer.IsExistsByUID(this.LoginInfo.User.ID, -1);
            }
            base.SetAuthority(FormId.WorkCalendar);
            this.btnWorkingCalendar.Attributes.Add("class", base._authority.IsWorkCalendarView || isExistsCalendar ? enableClass : disableClass);

            //Attendance            
            base.SetAuthority(FormId.Attendance);
            this.btnTAttendance.Attributes.Add("class", base._authority.IsAttendanceView ? enableClass : disableClass);

            //AttendanceApproval            
            base.SetAuthority(FormId.AttendanceApproval);
            this.btnAttendanceApproval.Attributes.Add("class", base._authority.IsAttendanceApprovalView ? enableClass : disableClass);

            //AttendanceSummary            
            base.SetAuthority(FormId.AttendanceSummary);
            this.btnAttendanceSummary.Attributes.Add("class", base._authority.IsAttendanceSummaryView ? enableClass : disableClass);

            //AttendancePayslip
            base.SetAuthority(FormId.AttendancePayslip);
            this.btnAttendancePayslip.Attributes.Add("class", base._authority.IsAttendancePayslipView ? enableClass : disableClass);

            //Sent Mail            
            base.SetAuthority(FormId.SendMail);
            this.btnSentMail.Attributes.Add("class", base._authority.IsMasterView ? enableClass : disableClass);

            //Expence
            base.SetAuthority(FormId.ExpenceGroup); 
            this.btnExpenseRegistration.Attributes.Add("class", base._authority.IsExpenceView ? enableClass : disableClass);

            //ProjectProfit
            base.SetAuthority(FormId.ProjectProfit);
            this.btnProfitManagement.Attributes.Add("class", base._authority.IsMasterView ? enableClass : disableClass);

            IList<M_Information> data;

            //Get data
            using (DB db = new DB())
            {
                InformationService service = new InformationService(db);
                data = service.GetAll();                
            }

            this.rptData.DataSource = data;
            this.rptData.DataBind();

        }

        /// <summary>
        /// Event changed page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnWorkingCalendar_Click(object sender, CommandEventArgs e)
        {
            IList<M_WorkingSystem> lstMWorkingSystem;
            using (DB db = new DB())
            {
                WorkingSystemService workingSystemSer = new WorkingSystemService(db);
                lstMWorkingSystem = workingSystemSer.GetAll();
            }
            if (lstMWorkingSystem.Count == 0)
            {
                //Show question update
                ShowQuestionMessage(M_Message.MSG_UPDATE_TABLE_DATA, Models.DefaultButton.Yes, true, "WorkingSystem");
            }
            else
            {
                //PostBackUrl
                Response.Redirect("~/WorkingCalendar/FrmWorkingCalendarList.aspx");
            }
        }

        /// <summary>
        /// Event changed page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnTAttendance_Click(object sender, CommandEventArgs e)
        {
            //IList<T_WorkingCalendar_H> lstTWorkingCalendarH;
            //using (DB db = new DB())
            //{
            //    T_WorkingCalendar_HService tWorkingCalendarHSer = new T_WorkingCalendar_HService(db);
            //    lstTWorkingCalendarH = tWorkingCalendarHSer.GetAll();
            //}
            //if (lstTWorkingCalendarH.Count == 0)
            //{
            //    //Show question
            //    ShowQuestionMessage(M_Message.MSG_UPDATE_TABLE_DATA, Models.DefaultButton.Yes, true, "T_Working_Calendar_H");
            //}
            //else
            //{
                //PostBackUrl
                Response.Redirect("~/Attendance/FrmAttendanceList.aspx");
            //}
        }

        /// <summary>
        /// Show message question
        /// </summary>
        /// <param name="messageID">MessageID</param>
        /// <param name="defaultButton">Default Button</param>
        protected void ShowQuestionMessage(string messageID, DefaultButton defaultButton, bool isDelete = false, params string[] args)
        {

            //Get Message
            M_Message mess = (M_Message)this.Messages[messageID];
            HtmlGenericControl questionMessage = (HtmlGenericControl)this.Master.FindControl("questionMessage");
            questionMessage.InnerHtml = "<p>" + " " + string.Format(mess.Message3, args) + "</p>";            
            
            this.IsShowQuestion = true;

            if (defaultButton == Models.DefaultButton.Yes)
            {
                this.DefaultButton = "#btnYes";
            }
        }

        #endregion
    }
}