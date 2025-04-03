using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMS.Models
{
    public class AuthorityInfo
    {
        // for Master 
        public bool IsMasterView { get; set; }
        public bool IsMasterNew { get; set; }
        public bool IsMasterEdit { get; set; }
        public bool IsMasterCopy { get; set; }
        public bool IsMasterDelete { get; set; }
        public bool IsMasterExcel { get; set; }

        // for Expence

        public bool IsExpenceView { get; set; }
        public bool IsExpenceNew { get; set; }
        public bool IsExpenceEdit { get; set; }
        public bool IsExpenceDelete { get; set; }
        public bool IsExpenceOtherApply { get; set; }
        public bool IsExpenceAccept { get; set; }
        public bool IsExpencemutilAccept { get; set; }
        public bool IsExpenceAllApproved { get; set; }
        public bool IsExpenceExportExcel { get; set; }
        public bool IsExpenceExportExcel2 { get; set; }
        public bool IsExpenceMail { get; set; }
        public bool IsExpenceCopy { get; set; }

        // for WorkingCalendar
        public bool IsWorkCalendarView { get; set; }
        public bool IsWorkCalendarNew { get; set; }
        public bool IsWorkCalendarEdit { get; set; }
        public bool IsWorkCalendarDelete { get; set; }
        public bool IsWorkCalendarAgreementSetting { get; set; }
        public bool IsWorkCalendarExportExcel { get; set; }

        // for Attendance
        public bool IsAttendanceView { get; set; }
        public bool IsAttendanceNew { get; set; }
        public bool IsAttendanceEdit { get; set; }
        public bool IsAttendanceDelete { get; set; }
        public bool IsAttendanceOtherDepartments { get; set; }
        public bool IsAttendanceOtherEmployees { get; set; }
        public bool IsAttendanceOtherUpdates { get; set; }
        public bool IsAttendanceShelfRegistration { get; set; }
        public bool IsAttendanceApproval { get; set; }
        public bool IsAttendanceExportExcel { get; set; }

        // for Attendance Approval
        public bool IsAttendanceApprovalView { get; set; }
        public bool IsAttendanceApprovalApprovel { get; set; }
        public bool IsAttendanceApprovalReject { get; set; }

        // for Attendance Summary
        public bool IsAttendanceSummaryView { get; set; }
        public bool IsAttendanceSummaryExportExcel { get; set; }

        // for Attendance Payslip
        public bool IsAttendancePayslipView { get; set; }
        public bool IsAttendancePayslipDepartments { get; set; }
        public bool IsAttendancePayslipEmployees { get; set; }
        public bool IsAttendancePayslipUpload { get; set; }

        // for Send Mail
        public bool IsMailView { get; set; }
        public bool IsMailSend { get; set; }
        public bool IsMailEdit { get; set; }
        public bool IsMailReSend { get; set; }
        public bool IsMailDelete { get; set; }

        // for Approval
        public bool IsApproval { get; set; }
        public bool IsApprovalALL { get; set; }
        public bool IsApprovalMail { get; set; }
        public bool IsConfirmMail { get; set; }
    }
}
