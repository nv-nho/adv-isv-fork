using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OMS.Models;
using System.Collections;
using OMS.Utilities;
using System.Data;

namespace OMS.DAC
{
    /// Class AttendanceService DAC
    /// Create Date: 2017/010/19
    /// Create Author: ISV-Than
    public class AttendanceService : BaseService
    {
        #region Contructor
        /// <summary>
        /// Constructor
        /// </summary>
        private AttendanceService()
            : base()
        {
        }

        /// <summary>
        /// Constructor with param
        /// </summary>
        /// <param name="db">Database</param>
        public AttendanceService(DB db)
            : base(db)
        {
        }
        #endregion

        #region GetData
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupCD"></param>
        /// <param name="groupName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="sortDirec"></param>
        /// <returns></returns>
        public IList<AttendanceDetailInfo> GetListByCond(DateTime startDate, DateTime endDate, int userID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  ISNULL(AT.ID, -1) AS ID");
            cmdText.AppendLine(" ,SC.UID");
            cmdText.AppendLine(" ,SC.WorkingDate AS Date");
            cmdText.AppendLine(" ,SC.WorkingType");
            cmdText.AppendLine(" ,AT.EntryTime");
            cmdText.AppendLine(" ,AT.ExitTime");
            cmdText.AppendLine(" ,AT.WorkingHours");
            cmdText.AppendLine(" ,AT.LateHours");
            cmdText.AppendLine(" ,AT.EarlyHours");
            cmdText.AppendLine(" ,AT.SH_Hours");
            cmdText.AppendLine(" ,AT.LH_Hours");

            cmdText.AppendLine(" ,AT.OverTimeHours1");
            cmdText.AppendLine(" ,AT.OverTimeHours2");
            cmdText.AppendLine(" ,AT.OverTimeHours3");
            cmdText.AppendLine(" ,AT.OverTimeHours4");
            cmdText.AppendLine(" ,AT.OverTimeHours5");

            cmdText.AppendLine(" ,AT.SH_OverTimeHours1");
            cmdText.AppendLine(" ,AT.SH_OverTimeHours2");
            cmdText.AppendLine(" ,AT.SH_OverTimeHours3");
            cmdText.AppendLine(" ,AT.SH_OverTimeHours4");
            cmdText.AppendLine(" ,AT.SH_OverTimeHours5");

            cmdText.AppendLine(" ,AT.LH_OverTimeHours1");
            cmdText.AppendLine(" ,AT.LH_OverTimeHours2");
            cmdText.AppendLine(" ,AT.LH_OverTimeHours3");
            cmdText.AppendLine(" ,AT.LH_OverTimeHours4");
            cmdText.AppendLine(" ,AT.LH_OverTimeHours5");

            cmdText.AppendLine(" ,AT.VacationFlag");
            cmdText.AppendLine(" ,AT.VacationFullCD");
            cmdText.AppendLine(" ,AT.VacationMorningCD");
            cmdText.AppendLine(" ,AT.VacationAfternoonCD");
            cmdText.AppendLine(" ,CASE WHEN WS.WorkingSystemName IS NULL");
            cmdText.AppendLine("       THEN CASE WHEN PL.Date IS NULL THEN '&nbsp;' ELSE '有休予定日' END");
            cmdText.AppendLine("       ELSE WS.WorkingSystemName END AS WorkingSystemName");
            cmdText.AppendLine(" ,AT.Memo");
            //************* Approval ***************
            cmdText.AppendLine(" ,AT.ApprovalStatus");
            cmdText.AppendLine(" ,AT.ApprovalUID");
            cmdText.AppendLine(" ,AT.ApprovalDate");
            cmdText.AppendLine(" ,AT.ApprovalNote");
            //************* Request ***************
            cmdText.AppendLine(" ,AT.RequestNote");
            //**************************************
            cmdText.AppendLine(" ,CASE WHEN AT.ExchangeFlag = 1 AND ATExc.ExchangeDate IS NOT NULL THEN '振休取得済 '");
            cmdText.AppendLine("                                                                        + CAST(DATEPART ( YYYY , ATExc.Date) AS varchar(4))");
            cmdText.AppendLine("                                                                        + '年' + RIGHT('0' + CAST(DATEPART ( MM , ATExc.Date) AS varchar(2)), 2)");
            cmdText.AppendLine("                                                                        + '月' + RIGHT('0' + CAST(DATEPART ( DD , ATExc.Date) AS varchar(2)), 2)");
            cmdText.AppendLine("                                                                        + '日 (' + CASE DATEPART(DW, ATExc.Date)");
            cmdText.AppendLine("                                                                                   WHEN 1 THEN '日' ");
            cmdText.AppendLine("                                                                                   WHEN 2 THEN '月' ");
            cmdText.AppendLine("                                                                                   WHEN 3 THEN '火' ");
            cmdText.AppendLine("                                                                                   WHEN 4 THEN '水' ");
            cmdText.AppendLine("                                                                                   WHEN 5 THEN '木' ");
            cmdText.AppendLine("                                                                                   WHEN 6 THEN '金' ");
            cmdText.AppendLine("                                                                                   WHEN 7 THEN '土' ");
            cmdText.AppendLine("                                                                                   END ");
            cmdText.AppendLine("                                                                         + ' )'");
            cmdText.AppendLine("       WHEN AT.ExchangeFlag = 1 AND ATExc.ExchangeDate IS NULL THEN '振休予定'");
            cmdText.AppendLine("       ELSE '' END AS ExchangeStatus");

            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" (SELECT");
            cmdText.AppendLine(" CU.UID,");
            cmdText.AppendLine(" CD.WorkingDate,");
            cmdText.AppendLine(" WS.WorkingType");
            cmdText.AppendLine(" FROM T_WorkingCalendar_D CD");
            cmdText.AppendLine(" INNER JOIN T_WorkingCalendar_U CU ON CD.HID = CU.HID");
            cmdText.AppendLine(" INNER JOIN M_WorkingSystem WS ON WS.ID = CD.WorkingSystemID");

            //Parameter
            Hashtable paras = new Hashtable();

            if (startDate != null && endDate != null)
            {
                cmdWhere.AppendLine("(CD.WorkingDate >= @IN_StartDate AND CD.WorkingDate <= @IN_EndDate)");
                base.AddParam(paras, "IN_StartDate", startDate, true);
                base.AddParam(paras, "IN_EndDate", endDate, true);
            }

            cmdWhere.AppendLine("AND (CU.UID = @IN_UID)");
            base.AddParam(paras, "IN_UID", userID);


            //Check SQL
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE");

                cmdWhere = cmdWhere.Replace("AND", "", 0, 3);
                cmdText.AppendLine(cmdWhere.ToString());
            }
            cmdText.AppendLine(" ) SC");
            cmdText.AppendLine(" LEFT JOIN dbo.T_Attendance AT ON AT.UID = SC.UID AND AT.Date = SC.WorkingDate");
            cmdText.AppendLine(" LEFT JOIN dbo.M_WorkingSystem AS WS");
            cmdText.AppendLine(" ON AT.WSID = WS.ID");
            cmdText.AppendLine(" LEFT JOIN dbo.T_Attendance AS ATExc ON ATExc.UID = AT.UID AND ATExc.ExchangeDate = AT.Date");
            cmdText.AppendLine(" LEFT JOIN dbo.T_PaidLeave PL ON SC.UID = PL.UserID AND SC.WorkingDate = PL.Date");

            return this.db.FindList1<AttendanceDetailInfo>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Get Record Previous StartDate
        /// </summary>
        /// <param name="groupCD"></param>
        /// <param name="groupName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="sortDirec"></param>
        /// <returns></returns>
        public IList<AttendanceDetailInfo> GetPreviousRecordByCond(DateTime startDate, int userID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT TOP 1");
            cmdText.AppendLine("  ISNULL(AT.ID, -1) AS ID");
            cmdText.AppendLine(" ,SC.UID");
            cmdText.AppendLine(" ,SC.WorkingDate AS Date");
            cmdText.AppendLine(" ,SC.WorkingType");
            cmdText.AppendLine(" ,AT.EntryTime");
            cmdText.AppendLine(" ,AT.ExitTime");
            cmdText.AppendLine(" ,AT.WorkingHours");
            cmdText.AppendLine(" ,AT.LateHours");
            cmdText.AppendLine(" ,AT.EarlyHours");
            cmdText.AppendLine(" ,AT.SH_Hours");
            cmdText.AppendLine(" ,AT.LH_Hours");

            cmdText.AppendLine(" ,AT.OverTimeHours1");
            cmdText.AppendLine(" ,AT.OverTimeHours2");
            cmdText.AppendLine(" ,AT.OverTimeHours3");
            cmdText.AppendLine(" ,AT.OverTimeHours4");
            cmdText.AppendLine(" ,AT.OverTimeHours5");

            cmdText.AppendLine(" ,AT.SH_OverTimeHours1");
            cmdText.AppendLine(" ,AT.SH_OverTimeHours2");
            cmdText.AppendLine(" ,AT.SH_OverTimeHours3");
            cmdText.AppendLine(" ,AT.SH_OverTimeHours4");
            cmdText.AppendLine(" ,AT.SH_OverTimeHours5");

            cmdText.AppendLine(" ,AT.LH_OverTimeHours1");
            cmdText.AppendLine(" ,AT.LH_OverTimeHours2");
            cmdText.AppendLine(" ,AT.LH_OverTimeHours3");
            cmdText.AppendLine(" ,AT.LH_OverTimeHours4");
            cmdText.AppendLine(" ,AT.LH_OverTimeHours5");

            cmdText.AppendLine(" ,AT.VacationFlag");
            cmdText.AppendLine(" ,AT.VacationFullCD");
            cmdText.AppendLine(" ,AT.VacationMorningCD");
            cmdText.AppendLine(" ,AT.VacationAfternoonCD");
            cmdText.AppendLine(" ,ISNULL(WS.WorkingSystemName, '&nbsp;') AS WorkingSystemName");
            cmdText.AppendLine(" ,AT.Memo");

            //************* Approval ***************
            cmdText.AppendLine(" ,AT.ApprovalStatus");
            cmdText.AppendLine(" ,AT.ApprovalUID");
            cmdText.AppendLine(" ,AT.ApprovalDate");
            cmdText.AppendLine(" ,AT.ApprovalNote");
            //************* Request ***************
            cmdText.AppendLine(" ,AT.RequestNote");
            //**************************************
            cmdText.AppendLine(" ,CASE WHEN AT.ExchangeFlag = 1 AND ATExc.ExchangeDate IS NOT NULL THEN '振休取得済 '");
            cmdText.AppendLine("                                                                        + CAST(DATEPART ( YYYY , ATExc.Date) AS varchar(4))");
            cmdText.AppendLine("                                                                        + '年' + RIGHT('0' + CAST(DATEPART ( MM , ATExc.Date) AS varchar(2)), 2)");
            cmdText.AppendLine("                                                                        + '月' + RIGHT('0' + CAST(DATEPART ( DD , ATExc.Date) AS varchar(2)), 2)");
            cmdText.AppendLine("                                                                        + '日 (' + CASE DATEPART(DW, ATExc.Date)");
            cmdText.AppendLine("                                                                                   WHEN 1 THEN '日' ");
            cmdText.AppendLine("                                                                                   WHEN 2 THEN '月' ");
            cmdText.AppendLine("                                                                                   WHEN 3 THEN '火' ");
            cmdText.AppendLine("                                                                                   WHEN 4 THEN '水' ");
            cmdText.AppendLine("                                                                                   WHEN 5 THEN '木' ");
            cmdText.AppendLine("                                                                                   WHEN 6 THEN '金' ");
            cmdText.AppendLine("                                                                                   WHEN 7 THEN '土' ");
            cmdText.AppendLine("                                                                                   END ");
            cmdText.AppendLine("                                                                         + ' )'");
            cmdText.AppendLine("       WHEN AT.ExchangeFlag = 1 AND ATExc.ExchangeDate IS NULL THEN '振休予定'");
            cmdText.AppendLine("       ELSE '' END AS ExchangeStatus");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" (SELECT");
            cmdText.AppendLine(" CU.UID,");
            cmdText.AppendLine(" CD.WorkingDate,");
            cmdText.AppendLine(" WS.WorkingType");
            cmdText.AppendLine(" FROM T_WorkingCalendar_D CD");
            cmdText.AppendLine(" INNER JOIN T_WorkingCalendar_U CU ON CD.HID = CU.HID");
            cmdText.AppendLine(" INNER JOIN M_WorkingSystem WS ON WS.ID = CD.WorkingSystemID");

            //Parameter
            Hashtable paras = new Hashtable();

            if (startDate != null )
            {
                cmdWhere.AppendLine("CD.WorkingDate < @IN_StartDate ");
                base.AddParam(paras, "IN_StartDate", startDate, true);
            }

            cmdWhere.AppendLine("AND CU.UID = @IN_UID");
            base.AddParam(paras, "IN_UID", userID);


            //Check SQL
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE");

                cmdWhere = cmdWhere.Replace("AND", "", 0, 3);
                cmdText.AppendLine(cmdWhere.ToString());
            }
            cmdText.AppendLine(" ) SC");
            cmdText.AppendLine(" LEFT JOIN dbo.T_Attendance AT ON AT.UID = SC.UID AND AT.Date = SC.WorkingDate");
            cmdText.AppendLine(" LEFT JOIN dbo.M_WorkingSystem AS WS");
            cmdText.AppendLine(" ON AT.WSID = WS.ID");
            cmdText.AppendLine(" LEFT JOIN dbo.T_Attendance AS ATExc ON ATExc.UID = AT.UID AND ATExc.ExchangeDate = AT.Date");
            cmdText.AppendLine(" WHERE AT.EntryTime IS NOT NULL");
            cmdText.AppendLine(" ORDER BY SC.WorkingDate DESC");

            return this.db.FindList1<AttendanceDetailInfo>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// list<id,updatedate> for T_Attendance
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public IList<AttendanceApprovalIDAndUpdateDate> getListIdAndUpdateDate(DateTime startDate, DateTime endDate, int userID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  A.ID AS Id,");
            cmdText.AppendLine(" A.UpdateDate as UpdateDate");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine("  dbo.T_Attendance as A");
            //Parameter
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine("AND A.StatusFlag !=0");
            if (startDate != null && endDate != null)
            {
                cmdWhere.AppendLine(" AND A.Date >= @IN_StartDate AND A.Date <= @IN_EndDate");
                base.AddParam(paras, "IN_StartDate", startDate, true);
                base.AddParam(paras, "IN_EndDate", endDate, true);
            }

            cmdWhere.AppendLine("AND A.UID = @IN_UID");
            base.AddParam(paras, "IN_UID", userID);

            //Check SQL
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE");

                cmdWhere = cmdWhere.Replace("AND", "", 0, 3);
                cmdText.AppendLine(cmdWhere.ToString());
            }
            return this.db.FindList1<AttendanceApprovalIDAndUpdateDate>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// GetListAttendanceApprovalByCond
        /// </summary>
        /// <param name="WorkingCalendarId"></param>
        /// <param name="userNm"></param>
        /// <param name="dateOfServiceFrom"></param>
        /// <param name="dateOfServiceTo"></param>
        /// <param name="departmentId"></param>
        /// <param name="cmbInvalidData"></param>
        /// <returns></returns>
        public IList<AttendanceApprovalInfo> GetListAttendanceApprovalByCond(string WorkingCalendarId, string userId, string dateOfServiceFrom, string dateOfServiceTo, string departmentId, string cmbInvalidData)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" ROW_NUMBER() OVER (ORDER BY min(A.DepartmentCD),min(A.UserCD), DATEADD(MONTH, A.[Month] - 1 , A.InitialDate)) AS RowNumber ");
            cmdText.AppendLine(" ,A.UID");
            cmdText.AppendLine(" ,MIN(A.CallendarID) as CallendarID");
            cmdText.AppendLine(" ,A.InitialDate");
            cmdText.AppendLine(" ,MIN(A.UserCD) as UserCD");
            cmdText.AppendLine(" ,MIN(A.UserName1) AS UserName1");
            cmdText.AppendLine(" ,MIN(A.departmentId) AS DepartmentID,min(A.DepartmentName) AS DepartmentName");
            cmdText.AppendLine(" ,A.[Month]");
            cmdText.AppendLine(" ,DATEADD(MONTH, A.[Month] - 1 , A.InitialDate) StartDate");
            cmdText.AppendLine(" ,DATEADD(DAY, -1, DATEADD(MONTH, A.[Month] , A.InitialDate)) EndDate");
            cmdText.AppendLine(" ,SUM(A.[WorkingHours]) AS [WorkingHours]");
            cmdText.AppendLine(" ,SUM(A.[LateHours]) AS [LateHours]");
            cmdText.AppendLine(" ,SUM(A.[EarlyHours]) AS [EarlyHours]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN A.[SH_Hours] END) AS [SH_Hours]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN A.[LH_Hours] END) AS [LH_Hours]");
            //cmdText.AppendLine(" ,COUNT(nullif(A.WorkingHours,0)) AS NumWorkingDays");
            //cmdText.AppendLine(" ,COUNT(CASE  WHEN (A.EntryTime IS NOT NULL) THEN NULLIF(A.WorkingHours,0) END) AS NumWorkingDays");
            cmdText.AppendLine(string.Format(" ,COUNT(CASE  WHEN (A.EntryTime IS NOT NULL AND A.WorkingType = {0}) THEN 1 END) AS NumWorkingDays", (int)WorkingType.WorkFullTime));
            cmdText.AppendLine(" ,COUNT(NULLIF(A.LateHours,0)) AS NumLateDays");
            cmdText.AppendLine(" ,COUNT(NULLIF(A.EarlyHours,0)) AS NumEarlyDays");
            //cmdText.AppendLine(" ,COUNT(NULLIF(A.SH_Hours,0)) AS NumSH_Days");
            cmdText.AppendLine(string.Format(" ,COUNT(CASE  WHEN (A.EntryTime IS NOT NULL AND A.WorkingType = {0} AND A.ExchangeFlag = 0) THEN 1 END) AS NumSH_Days", (int)WorkingType.WorkHoliDay));
            //cmdText.AppendLine(" ,COUNT(NULLIF(A.LH_Hours,0)) AS NumLH_Days");
            cmdText.AppendLine(string.Format(" ,COUNT(CASE  WHEN (A.EntryTime IS NOT NULL AND A.WorkingType = {0} AND A.ExchangeFlag = 0) THEN 1 END) AS NumLH_Days", (int)WorkingType.LegalHoliDay));

            cmdText.AppendLine(" ,SUM(A.[OverTimeHours1]) AS [OverTimeHours1]");
            cmdText.AppendLine(" ,SUM(A.[OverTimeHours2]) AS [OverTimeHours2]");
            cmdText.AppendLine(" ,SUM(A.[OverTimeHours3]) AS [OverTimeHours3]");
            cmdText.AppendLine(" ,SUM(A.[OverTimeHours4]) AS [OverTimeHours4]");
            cmdText.AppendLine(" ,SUM(A.[OverTimeHours5]) AS [OverTimeHours5]");

            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN A.[SH_OverTimeHours1] END) AS [SH_OverTimeHours1]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN A.[SH_OverTimeHours2] END) AS [SH_OverTimeHours2]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN A.[SH_OverTimeHours3] END) AS [SH_OverTimeHours3]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN A.[SH_OverTimeHours4] END) AS [SH_OverTimeHours4]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN A.[SH_OverTimeHours5] END) AS [SH_OverTimeHours5]");

            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN A.[LH_OverTimeHours1] END) AS [LH_OverTimeHours1]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN A.[LH_OverTimeHours2] END) AS [LH_OverTimeHours2]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN A.[LH_OverTimeHours3] END) AS [LH_OverTimeHours3]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN A.[LH_OverTimeHours4] END) AS [LH_OverTimeHours4]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN A.[LH_OverTimeHours5] END) AS [LH_OverTimeHours5]");

            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN A.[TotalOverTimeHours] END) AS [TotalOverTimeHours]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN A.[TotalWorkingHours] END) AS [TotalWorkingHours]");
            cmdText.AppendLine(" ,MIN(A.StatusFlag) AS StatusFlag");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" ( SELECT");
            cmdText.AppendLine(" CH.InitialDate,CH.ID AS CallendarID,");
            cmdText.AppendLine(" U.UserCD,U.UserName1,D.ID AS departmentId, D.DepartmentCD AS DepartmentCD,D.DepartmentName,MS.WorkingType,");
            cmdText.AppendLine(" CASE");
            cmdText.AppendLine(" WHEN A.Date BETWEEN  CH.InitialDate AND DATEADD(DAY, -1, DATEADD(MONTH, 1 , CH.InitialDate)) THEN 1 ");
            cmdText.AppendLine(" WHEN A.Date BETWEEN  DATEADD(MONTH, 1 , CH.InitialDate) AND DATEADD(DAY, -1, DATEADD(MONTH, 2 , CH.InitialDate)) THEN 2");
            cmdText.AppendLine(" WHEN A.Date BETWEEN  DATEADD(MONTH, 2 , CH.InitialDate) AND DATEADD(DAY, -1, DATEADD(MONTH, 3 , CH.InitialDate)) THEN 3 ");
            cmdText.AppendLine(" WHEN A.Date BETWEEN  DATEADD(MONTH, 3 , CH.InitialDate) AND DATEADD(DAY, -1, DATEADD(MONTH, 4 , CH.InitialDate)) THEN 4");
            cmdText.AppendLine(" WHEN A.Date BETWEEN  DATEADD(MONTH, 4 , CH.InitialDate) AND DATEADD(DAY, -1, DATEADD(MONTH, 5 , CH.InitialDate)) THEN 5");
            cmdText.AppendLine(" WHEN A.Date BETWEEN  DATEADD(MONTH, 5 , CH.InitialDate) AND DATEADD(DAY, -1, DATEADD(MONTH, 6 , CH.InitialDate)) THEN 6");
            cmdText.AppendLine(" WHEN A.Date BETWEEN  DATEADD(MONTH, 6 , CH.InitialDate) AND DATEADD(DAY, -1, DATEADD(MONTH, 7 , CH.InitialDate)) THEN 7");
            cmdText.AppendLine(" WHEN A.Date BETWEEN  DATEADD(MONTH, 7 , CH.InitialDate) AND DATEADD(DAY, -1, DATEADD(MONTH, 8 , CH.InitialDate)) THEN 8");
            cmdText.AppendLine(" WHEN A.Date BETWEEN  DATEADD(MONTH, 8 , CH.InitialDate) AND DATEADD(DAY, -1, DATEADD(MONTH, 9 , CH.InitialDate)) THEN 9");
            cmdText.AppendLine(" WHEN A.Date BETWEEN  DATEADD(MONTH, 9 , CH.InitialDate) AND DATEADD(DAY, -1, DATEADD(MONTH, 10 , CH.InitialDate)) THEN 10");
            cmdText.AppendLine(" WHEN A.Date BETWEEN  DATEADD(MONTH, 10 , CH.InitialDate) AND DATEADD(DAY, -1, DATEADD(MONTH, 11 , CH.InitialDate)) THEN 11");
            cmdText.AppendLine(" WHEN A.Date BETWEEN  DATEADD(MONTH, 11 , CH.InitialDate) AND DATEADD(DAY, -1, DATEADD(MONTH, 12 , CH.InitialDate)) THEN 12");
            cmdText.AppendLine(" END [Month],");
            cmdText.AppendLine(" A.* ");
            cmdText.AppendLine(" FROM [dbo].[T_Attendance] A");
            cmdText.AppendLine(" INNER JOIN [dbo].[T_WorkingCalendar_D] CD ON A.Date = CD.WorkingDate");
            cmdText.AppendLine(" INNER JOIN [dbo].[T_WorkingCalendar_U] CU ON A.UID = CU.UID AND CD.HID = CU.HID");
            cmdText.AppendLine(" INNER JOIN [dbo].[T_WorkingCalendar_H] CH ON CD.HID = CH.ID");
            cmdText.AppendLine(" LEFT JOIN [dbo].[M_WorkingSystem] MS on MS.ID = A.WSID");
            cmdText.AppendLine(" left join dbo.M_User U on U.ID=A.UID");
            cmdText.AppendLine(" left join dbo.M_Department D on D.ID= U.DepartmentID");

            //Parameter
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine("AND A.StatusFlag != 0");
            //  cmdWhere.AppendLine("AND (A.UID = @IN_UID)");

            if (WorkingCalendarId != string.Empty && WorkingCalendarId != "-1")
            {
                cmdWhere.AppendLine("AND (CH.ID = @IN_WORKINGCALENDARID)");
                base.AddParam(paras, "IN_WORKINGCALENDARID", WorkingCalendarId);
            }

            if (dateOfServiceFrom != "-1")
            {
                DateTime startDateFrom = DateTime.MinValue;
                DateTime endDateFrom = DateTime.MinValue;
                startDateFrom = DateTime.Parse(dateOfServiceFrom);
                endDateFrom = startDateFrom.AddMonths(1).AddDays(-1);

                cmdWhere.AppendLine(" AND A.Date >= @IN_StartdateOfServiceFrom");
                base.AddParam(paras, "IN_StartdateOfServiceFrom", startDateFrom);

            }

            if (dateOfServiceTo != "-1")
            {
                DateTime startDateTo = DateTime.MinValue;
                DateTime endDateTo = DateTime.MinValue;
                startDateTo = DateTime.Parse(dateOfServiceTo);
                endDateTo = startDateTo.AddMonths(1).AddDays(-1);

                cmdWhere.AppendLine(" AND A.Date <= @IN_EnddateOfServiceFrom");
                base.AddParam(paras, "IN_EnddateOfServiceFrom", endDateTo);
            }


            if (userId != "-1" && userId != string.Empty)
            {

                cmdWhere.AppendLine("AND (U.ID = @IN_USERID)");
                base.AddParam(paras, "IN_USERID", userId);
            }

            if (departmentId != "-1" && departmentId != string.Empty)
            {
                cmdWhere.AppendLine("AND (D.ID = @IN_DepartmentID)");
                base.AddParam(paras, "IN_DepartmentID", departmentId);
            }

            if (cmbInvalidData == "0")
            {

                cmdWhere.AppendLine("AND (A.StatusFlag = @IN_StatusFlag)");
                base.AddParam(paras, "IN_StatusFlag", 1);
            }

            if (cmbInvalidData == "1")
            {
                cmdWhere.AppendLine("AND (A.StatusFlag = @IN_StatusFlag)");
                base.AddParam(paras, "IN_StatusFlag", 2);
            }

            //Check SQL
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE");

                cmdWhere = cmdWhere.Replace("AND", "", 0, 3);

                cmdText.AppendLine(cmdWhere.ToString());
            }
            cmdText.AppendLine(" ) A");

            cmdText.AppendLine(" GROUP BY A.InitialDate, A.UID, A.[Month]");

            return this.db.FindList1<AttendanceApprovalInfo>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// get data Insert AttendanceResult
        /// </summary>
        /// <param name="WorkingCalendarId"></param>
        /// <param name="userId"></param>
        /// <param name="startdate"></param>
        /// <param name="endStart"></param>
        /// <returns></returns>
        public T_AttendanceResult GetAttendanceApprovalForInsertAttendanceResult(int WorkingCalendarId, int userId, DateTime startdate, DateTime endStart)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  -1 AS ID");
            cmdText.AppendLine("  ,A.UID");
            cmdText.AppendLine(" ,MIN(A.CallendarID) as CallendarID");
            cmdText.AppendLine(" ,A.InitialDate");
            cmdText.AppendLine(" ,A.[Month]");
            cmdText.AppendLine(" ,DATEADD(MONTH, A.[Month] - 1 , A.InitialDate) StartDate");
            cmdText.AppendLine(" ,DATEADD(DAY, -1, DATEADD(MONTH, A.[Month] , A.InitialDate)) EndDate");
            cmdText.AppendLine(" ,SUM(A.[WorkingHours]) AS [WorkingHours]");
            cmdText.AppendLine(" ,SUM(A.[LateHours]) AS [LateHours]");
            cmdText.AppendLine(" ,SUM(A.[EarlyHours]) AS [EarlyHours]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN A.[SH_Hours] END) AS [SH_Hours]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN A.[LH_Hours] END) AS [LH_Hours]");
            //cmdText.AppendLine(" ,COUNT(nullif(A.WorkingHours,0)) AS WorkingDays");asd
            //cmdText.AppendLine(" ,COUNT(CASE  WHEN (A.EntryTime IS NOT NULL) THEN NULLIF(A.WorkingHours,0) END) AS WorkingDays");
            cmdText.AppendLine(string.Format(" ,COUNT(CASE  WHEN (A.EntryTime IS NOT NULL AND A.WorkingType = {0}) THEN 1 END) AS WorkingDays", (int)WorkingType.WorkFullTime));
            cmdText.AppendLine(" ,COUNT(NULLIF(A.LateHours,0)) AS LateDays");
            cmdText.AppendLine(" ,COUNT(NULLIF(A.EarlyHours,0)) AS EarlyDays");
            //cmdText.AppendLine(" ,COUNT(NULLIF(A.SH_Hours,0)) AS SH_Days");
            cmdText.AppendLine(string.Format(" ,COUNT(CASE  WHEN (A.EntryTime IS NOT NULL AND A.WorkingType = {0} AND A.ExchangeFlag = 0) THEN 1 END) AS SH_Days", (int)WorkingType.WorkHoliDay));
            //cmdText.AppendLine(" ,COUNT(NULLIF(A.LH_Hours,0)) AS LH_Days");
            cmdText.AppendLine(string.Format(" ,COUNT(CASE  WHEN (A.EntryTime IS NOT NULL AND A.WorkingType = {0} AND A.ExchangeFlag = 0) THEN 1 END) AS LH_Days", (int)WorkingType.LegalHoliDay));

            cmdText.AppendLine(" ,SUM(A.[OverTimeHours1]) AS [OverTimeHours1]");
            cmdText.AppendLine(" ,SUM(A.[OverTimeHours2]) AS [OverTimeHours2]");
            cmdText.AppendLine(" ,SUM(A.[OverTimeHours3]) AS [OverTimeHours3]");
            cmdText.AppendLine(" ,SUM(A.[OverTimeHours4]) AS [OverTimeHours4]");
            cmdText.AppendLine(" ,SUM(A.[OverTimeHours5]) AS [OverTimeHours5]");

            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN A.[SH_OverTimeHours1] END) AS [SH_OverTimeHours1]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN A.[SH_OverTimeHours2] END) AS [SH_OverTimeHours2]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN A.[SH_OverTimeHours3] END) AS [SH_OverTimeHours3]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN A.[SH_OverTimeHours4] END) AS [SH_OverTimeHours4]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN A.[SH_OverTimeHours5] END) AS [SH_OverTimeHours5]");

            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN A.[LH_OverTimeHours1] END) AS [LH_OverTimeHours1]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN A.[LH_OverTimeHours2] END) AS [LH_OverTimeHours2]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN A.[LH_OverTimeHours3] END) AS [LH_OverTimeHours3]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN A.[LH_OverTimeHours4] END) AS [LH_OverTimeHours4]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN A.[LH_OverTimeHours5] END) AS [LH_OverTimeHours5]");

            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN A.[TotalOverTimeHours] END) AS [TotalOverTimeHours]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN A.[TotalWorkingHours] END) AS [TotalWorkingHours]");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" ( SELECT");
            cmdText.AppendLine(" CH.InitialDate, CH.ID as CallendarID,MS.WorkingType,");
            cmdText.AppendLine(" CASE");
            cmdText.AppendLine(" WHEN A.Date BETWEEN  CH.InitialDate AND DATEADD(DAY, -1, DATEADD(MONTH, 1 , CH.InitialDate)) THEN 1 ");
            cmdText.AppendLine(" WHEN A.Date BETWEEN  DATEADD(MONTH, 1 , CH.InitialDate) AND DATEADD(DAY, -1, DATEADD(MONTH, 2 , CH.InitialDate)) THEN 2");
            cmdText.AppendLine(" WHEN A.Date BETWEEN  DATEADD(MONTH, 2 , CH.InitialDate) AND DATEADD(DAY, -1, DATEADD(MONTH, 3 , CH.InitialDate)) THEN 3 ");
            cmdText.AppendLine(" WHEN A.Date BETWEEN  DATEADD(MONTH, 3 , CH.InitialDate) AND DATEADD(DAY, -1, DATEADD(MONTH, 4 , CH.InitialDate)) THEN 4");
            cmdText.AppendLine(" WHEN A.Date BETWEEN  DATEADD(MONTH, 4 , CH.InitialDate) AND DATEADD(DAY, -1, DATEADD(MONTH, 5 , CH.InitialDate)) THEN 5");
            cmdText.AppendLine(" WHEN A.Date BETWEEN  DATEADD(MONTH, 5 , CH.InitialDate) AND DATEADD(DAY, -1, DATEADD(MONTH, 6 , CH.InitialDate)) THEN 6");
            cmdText.AppendLine(" WHEN A.Date BETWEEN  DATEADD(MONTH, 6 , CH.InitialDate) AND DATEADD(DAY, -1, DATEADD(MONTH, 7 , CH.InitialDate)) THEN 7");
            cmdText.AppendLine(" WHEN A.Date BETWEEN  DATEADD(MONTH, 7 , CH.InitialDate) AND DATEADD(DAY, -1, DATEADD(MONTH, 8 , CH.InitialDate)) THEN 8");
            cmdText.AppendLine(" WHEN A.Date BETWEEN  DATEADD(MONTH, 8 , CH.InitialDate) AND DATEADD(DAY, -1, DATEADD(MONTH, 9 , CH.InitialDate)) THEN 9");
            cmdText.AppendLine(" WHEN A.Date BETWEEN  DATEADD(MONTH, 9 , CH.InitialDate) AND DATEADD(DAY, -1, DATEADD(MONTH, 10 , CH.InitialDate)) THEN 10");
            cmdText.AppendLine(" WHEN A.Date BETWEEN  DATEADD(MONTH, 10 , CH.InitialDate) AND DATEADD(DAY, -1, DATEADD(MONTH, 11 , CH.InitialDate)) THEN 11");
            cmdText.AppendLine(" WHEN A.Date BETWEEN  DATEADD(MONTH, 11 , CH.InitialDate) AND DATEADD(DAY, -1, DATEADD(MONTH, 12 , CH.InitialDate)) THEN 12");
            cmdText.AppendLine(" END [Month],");
            cmdText.AppendLine(" A.* ");
            cmdText.AppendLine(" FROM [dbo].[T_Attendance] A");
            cmdText.AppendLine(" INNER JOIN [dbo].[T_WorkingCalendar_D] CD ON A.Date = CD.WorkingDate");
            cmdText.AppendLine(" INNER JOIN [dbo].[T_WorkingCalendar_U] CU ON A.UID = CU.UID AND CD.HID = CU.HID");
            cmdText.AppendLine(" INNER JOIN [dbo].[T_WorkingCalendar_H] CH ON CD.HID = CH.ID");
            cmdText.AppendLine(" LEFT JOIN [dbo].[M_WorkingSystem] MS on MS.ID = A.WSID");

            //Parameter
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine("AND A.StatusFlag != 0");

            cmdWhere.AppendLine("AND (CH.ID = @IN_WORKINGCALENDARID)");
            base.AddParam(paras, "IN_WORKINGCALENDARID", WorkingCalendarId);

            cmdWhere.AppendLine("AND (A.UID = @IN_UserID)");
            base.AddParam(paras, "IN_UserID", userId);

            cmdWhere.AppendLine(" AND  A.Date >= @IN_Startdate");
            cmdWhere.AppendLine(" AND A.Date <= @IN_EndDate");

            base.AddParam(paras, "IN_Startdate", startdate, true);
            base.AddParam(paras, "IN_EndDate", endStart, true);


            //Check SQL
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE");

                cmdWhere = cmdWhere.Replace("AND", "", 0, 3);

                cmdText.AppendLine(cmdWhere.ToString());
            }
            cmdText.AppendLine(" ) A");

            cmdText.AppendLine(" GROUP BY A.InitialDate, A.UID, A.[Month]");

            return this.db.Find1<T_AttendanceResult>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Get total row
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns></returns>
        public int GetTotalRowAll( string WorkingCalendarId,
                                List<string> lstUserId,
                                DateTime? startDate,
                                DateTime? endDate,
                                List<string> lstDeptId,
                                string Overtime_Vacation,
                                string compare, decimal hourDate)
        {
            string[] strOvertime_Vacation = new string[2];

            if (!Overtime_Vacation.Contains("TotalWorkingHours")
                && (!Overtime_Vacation.Contains("TotalOverTimeHours"))
                && (!Overtime_Vacation.Contains("SH_Hours"))
                && (!Overtime_Vacation.Contains("LH_Hours")))
            {
                strOvertime_Vacation = Overtime_Vacation.Split('_');
            }

            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" COUNT(*)");
            cmdText.AppendLine(" FROM ");
            cmdText.AppendLine(" ( ");
            cmdText.AppendLine("  SELECT");
            cmdText.AppendLine("  A.UID");
            cmdText.AppendLine(" ,MIN(A.UserCD) as UserCD");
            cmdText.AppendLine(" ,MIN(A.UserName1) AS UserName1");
            cmdText.AppendLine(" ,MIN(A.DepartmentName) AS DepartmentName");
            cmdText.AppendLine(" ,SUM(A.[WorkingHours]) AS [WorkingHours]");
            cmdText.AppendLine(" ,SUM(A.[LateHours]) AS [LateHours]");
            cmdText.AppendLine(" ,SUM(A.[EarlyHours]) AS [EarlyHours]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[SH_Hours],0) ELSE 0 END) AS [SH_Hours]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[LH_Hours],0) ELSE 0 END) AS [LH_Hours]");
            //cmdText.AppendLine(" ,COUNT(NULLIF(A.WorkingHours,0)) AS NumWorkingDays");
            //cmdText.AppendLine(" ,COUNT(CASE  WHEN (A.EntryTime IS NOT NULL) THEN NULLIF(A.WorkingHours,0) END) AS NumWorkingDays");
            cmdText.AppendLine(string.Format(" ,COUNT(CASE  WHEN (A.EntryTime IS NOT NULL AND A.WorkingType = {0}) THEN 1 END) AS NumWorkingDays", (int)WorkingType.WorkFullTime));
            cmdText.AppendLine(" ,COUNT(NULLIF(A.LateHours,0)) AS NumLateDays");
            cmdText.AppendLine(" ,COUNT(NULLIF(A.EarlyHours,0)) AS NumEarlyDays");
            //cmdText.AppendLine(" ,COUNT(NULLIF(A.SH_Hours,0)) AS NumSH_Days");
            cmdText.AppendLine(string.Format(" ,COUNT(CASE  WHEN (A.EntryTime IS NOT NULL AND A.WorkingType = {0} AND A.ExchangeFlag = 0) THEN 1 END) AS NumSH_Days", (int)WorkingType.WorkHoliDay));
            //cmdText.AppendLine(" ,COUNT(NULLIF(A.LH_Hours,0)) AS NumLH_Days");
            cmdText.AppendLine(string.Format(" ,COUNT(CASE  WHEN (A.EntryTime IS NOT NULL AND A.WorkingType = {0} AND A.ExchangeFlag = 0) THEN 1 END) AS NumLH_Days", (int)WorkingType.LegalHoliDay));

            cmdText.AppendLine(" ,SUM(ISNULL(A.[OverTimeHours1],0)) AS [OverTimeHours1]");
            cmdText.AppendLine(" ,SUM(ISNULL(A.[OverTimeHours2],0)) AS [OverTimeHours2]");
            cmdText.AppendLine(" ,SUM(ISNULL(A.[OverTimeHours3],0)) AS [OverTimeHours3]");
            cmdText.AppendLine(" ,SUM(ISNULL(A.[OverTimeHours4],0)) AS [OverTimeHours4]");
            cmdText.AppendLine(" ,SUM(ISNULL(A.[OverTimeHours5],0)) AS [OverTimeHours5]");

            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[SH_OverTimeHours1],0) ELSE 0 END) AS [SH_OverTimeHours1]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[SH_OverTimeHours2],0) ELSE 0 END) AS [SH_OverTimeHours2]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[SH_OverTimeHours3],0) ELSE 0 END) AS [SH_OverTimeHours3]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[SH_OverTimeHours4],0) ELSE 0 END) AS [SH_OverTimeHours4]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[SH_OverTimeHours5],0) ELSE 0 END) AS [SH_OverTimeHours5]");

            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[LH_OverTimeHours1],0) ELSE 0 END) AS [LH_OverTimeHours1]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[LH_OverTimeHours2],0) ELSE 0 END) AS [LH_OverTimeHours2]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[LH_OverTimeHours3],0) ELSE 0 END) AS [LH_OverTimeHours3]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[LH_OverTimeHours4],0) ELSE 0 END) AS [LH_OverTimeHours4]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[LH_OverTimeHours5],0) ELSE 0 END) AS [LH_OverTimeHours5]");

            cmdText.AppendLine(" ,SUM(ISNULL(A.[OverTimeHours1], 0 )) + SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[SH_OverTimeHours1], 0 ) ELSE 0 END) + SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[LH_OverTimeHours1], 0 ) ELSE 0 END) AS TotalOverTime_1");
            cmdText.AppendLine(" ,SUM(ISNULL(A.[OverTimeHours2], 0 )) + SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[SH_OverTimeHours2], 0 ) ELSE 0 END) + SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[LH_OverTimeHours2], 0 ) ELSE 0 END) AS TotalOverTime_2");
            cmdText.AppendLine(" ,SUM(ISNULL(A.[OverTimeHours3], 0 )) + SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[SH_OverTimeHours3], 0 ) ELSE 0 END) + SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[LH_OverTimeHours3], 0 ) ELSE 0 END) AS TotalOverTime_3");
            cmdText.AppendLine(" ,SUM(ISNULL(A.[OverTimeHours4], 0 )) + SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[SH_OverTimeHours4], 0 ) ELSE 0 END) + SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[LH_OverTimeHours4], 0 ) ELSE 0 END) AS TotalOverTime_4");
            cmdText.AppendLine(" ,SUM(ISNULL(A.[OverTimeHours5], 0 )) + SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[SH_OverTimeHours5], 0 ) ELSE 0 END) + SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[LH_OverTimeHours5], 0 ) ELSE 0 END) AS TotalOverTime_5");

            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[TotalOverTimeHours],0) ELSE 0 END) AS [TotalOverTimeHours]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[TotalWorkingHours],0) ELSE 0 END) AS [TotalWorkingHours]");

            if (Overtime_Vacation.Contains("Vacation"))
            {
                cmdText.AppendLine(" ,SUM(A.Vacation) AS Vacation");
            }
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" ( SELECT");
            cmdText.AppendLine(" CH.InitialDate,");
            cmdText.AppendLine(" U.UserCD,U.UserName1,D.DepartmentName,MS.WorkingType,");

            //Parameter
            Hashtable paras = new Hashtable();
            if (Overtime_Vacation.Contains("Vacation"))
            {
                cmdText.AppendLine(" (CASE WHEN A.VacationFullCD = @IN_VacationCd THEN 1 ELSE 0 END) +");
                cmdText.AppendLine(" (CASE WHEN A.VacationMorningCD = @IN_VacationCd then 0.4 ELSE 0 END) + ");
                cmdText.AppendLine(" (CASE WHEN A.VacationAfternoonCD = @IN_VacationCd then 0.6 ELSE 0 END) as Vacation,");
                base.AddParam(paras, "IN_VacationCd", strOvertime_Vacation[1]);
            }
            cmdText.AppendLine(" A.* ");
            cmdText.AppendLine(" FROM [dbo].[T_Attendance] A");
            cmdText.AppendLine(" INNER JOIN [dbo].[T_WorkingCalendar_D] CD ON A.Date = CD.WorkingDate");
            cmdText.AppendLine(" INNER JOIN [dbo].[T_WorkingCalendar_U] CU ON A.UID = CU.UID AND CD.HID = CU.HID");
            cmdText.AppendLine(" INNER JOIN [dbo].[T_WorkingCalendar_H] CH ON CD.HID = CH.ID");
            cmdText.AppendLine(" LEFT JOIN [dbo].[M_WorkingSystem] MS on MS.ID = A.WSID");
            cmdText.AppendLine(" LEFT JOIN dbo.M_User U on U.ID=A.UID");
            cmdText.AppendLine(" LEFT JOIN dbo.M_Department D on D.ID= U.DepartmentID");

            if (WorkingCalendarId != string.Empty && WorkingCalendarId != "-1")
            {
                cmdWhere.AppendLine("AND (CH.ID = @IN_WORKINGCALENDARID)");
                base.AddParam(paras, "IN_WORKINGCALENDARID", WorkingCalendarId);
            }

            
            if (startDate != null)
            {
                cmdWhere.AppendLine(" AND A.Date >= @IN_StartdateOfServiceFrom");
                base.AddParam(paras, "IN_StartdateOfServiceFrom", startDate);

            }

            if (endDate != null)
            {
                cmdWhere.AppendLine(" AND A.Date <= @IN_EnddateOfServiceFrom");
                base.AddParam(paras, "IN_EnddateOfServiceFrom", endDate);
            }

            if (!string.IsNullOrEmpty(lstDeptId[0] + lstDeptId[1] + lstUserId[0] + lstUserId[1]))
            {
                cmdWhere.AppendLine(" AND (");
                cmdWhere.AppendLine("  (");

                cmdWhere.AppendLine(" 1 = 1");
                if (!string.IsNullOrEmpty(lstDeptId[0]))
                {
                    cmdWhere.AppendLine(" AND D.ID = " + lstDeptId[0]);
                }
                if (!string.IsNullOrEmpty(lstUserId[0]))
                {
                    cmdWhere.AppendLine("AND A.UID IN (" + lstUserId[0] + ")");
                }
                if (string.IsNullOrEmpty(lstDeptId[0] + lstUserId[0]))
                {
                    cmdWhere.AppendLine(" AND 1 <> 1");
                }

                cmdWhere.AppendLine(" ) OR ( ");

                cmdWhere.AppendLine(" 1 = 1");
                if (!string.IsNullOrEmpty(lstDeptId[1]))
                {
                    cmdWhere.AppendLine(" AND D.ID = " + lstDeptId[1]);
                }
                if (!string.IsNullOrEmpty(lstUserId[1]))
                {
                    cmdWhere.AppendLine("AND A.UID IN (" + lstUserId[1] + ")");
                }
                if (string.IsNullOrEmpty(lstDeptId[1] + lstUserId[1]))
                {
                    cmdWhere.AppendLine(" AND 1 <> 1");
                }

                cmdWhere.AppendLine(" ) ");
                cmdWhere.AppendLine(" ) ");
            }

            //Check SQL
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE");

                cmdWhere = cmdWhere.Replace("AND", "", 0, 3);

                cmdText.AppendLine(cmdWhere.ToString());
            }
            cmdText.AppendLine(" ) A");

            cmdText.AppendLine(" GROUP BY A.UID");
            cmdText.AppendLine(" ) B");
            
            StringBuilder cmdWhere2 = new StringBuilder();
            if (Overtime_Vacation != "-1")
            {
                //TotalWorkingHours
                if (Overtime_Vacation.Contains("TotalWorkingHours"))
                {
                    if (compare == "0")
                    {
                        cmdWhere2.AppendLine(" B.TotalWorkingHours >=  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                    else
                    {
                        cmdWhere2.AppendLine(" B.TotalWorkingHours <  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                }

                //TotalOverTimeHours
                else if (Overtime_Vacation.Contains("TotalOverTimeHours"))
                {
                    if (compare == "0")
                    {
                        cmdWhere2.AppendLine(" B.TotalOverTimeHours >= @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                    else
                    {
                        cmdWhere2.AppendLine(" B.TotalOverTimeHours <  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                }

                //WorkingDays
                else if (Overtime_Vacation.Contains("WorkingDays"))
                {
                    if (compare == "0")
                    {
                        cmdWhere2.AppendLine(" B.NumWorkingDays >=  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);

                    }
                    else
                    {
                        cmdWhere2.AppendLine(" B.NumWorkingDays <  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                }

                //LateDays
                else if (Overtime_Vacation.Contains("LateDays"))
                {
                    if (compare == "0")
                    {
                        cmdWhere2.AppendLine(" B.NumLateDays >=  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);

                    }
                    else
                    {
                        cmdWhere2.AppendLine(" B.NumLateDays <  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                }

                //EarlyDays
                else if (Overtime_Vacation.Contains("EarlyDays"))
                {
                    if (compare == "0")
                    {
                        cmdWhere2.AppendLine(" B.NumEarlyDays >=  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);

                    }
                    else
                    {
                        cmdWhere2.AppendLine(" B.NumEarlyDays <  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                }

                //SH_Days
                else if (Overtime_Vacation.Contains("SH_Days"))
                {
                    if (compare == "0")
                    {
                        cmdWhere2.AppendLine(" B.NumSH_Days >=  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);

                    }
                    else
                    {
                        cmdWhere2.AppendLine(" B.NumSH_Days <  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                }

                //LH_Days
                else if (Overtime_Vacation.Contains("LH_Days"))
                {
                    if (compare == "0")
                    {
                        cmdWhere2.AppendLine(" B.NumLH_Days >=  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);

                    }
                    else
                    {
                        cmdWhere2.AppendLine(" B.NumLH_Days <  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                }

                //OverTime
                else if (Overtime_Vacation.Contains("OverTimeHours"))
                {
                    switch (Overtime_Vacation)
                    {
                        case "OverTimeHours_0":
                            if (compare == "0")
                            {
                                cmdWhere2.AppendLine(" B.TotalOverTime_1 >= @IN_HourDate");
                                base.AddParam(paras, "IN_HourDate", hourDate);
                            }
                            else
                            {
                                cmdWhere2.AppendLine(" B.TotalOverTime_1 <  @IN_HourDate");
                                base.AddParam(paras, "IN_HourDate", hourDate);
                            }
                            break;
                        case "OverTimeHours_1":
                            if (compare == "0")
                            {
                                cmdWhere2.AppendLine(" B.TotalOverTime_2 >=  @IN_HourDate");
                                base.AddParam(paras, "IN_HourDate", hourDate);
                            }
                            else
                            {
                                cmdWhere2.AppendLine(" B.TotalOverTime_2 <  @IN_HourDate");
                                base.AddParam(paras, "IN_HourDate", hourDate);
                            }
                            break;
                        case "OverTimeHours_2":
                            if (compare == "0")
                            {
                                cmdWhere2.AppendLine(" B.TotalOverTime_3 >= @IN_HourDate");
                                base.AddParam(paras, "IN_HourDate", hourDate);
                            }
                            else
                            {
                                cmdWhere2.AppendLine(" B.TotalOverTime_3 <  @IN_HourDate");
                                base.AddParam(paras, "IN_HourDate", hourDate);
                            }
                            break;
                        case "OverTimeHours_3":
                            if (compare == "0")
                            {
                                cmdWhere2.AppendLine(" B.TotalOverTime_4 >=  @IN_HourDate");
                                base.AddParam(paras, "IN_HourDate", hourDate);
                            }
                            else
                            {
                                cmdWhere2.AppendLine(" B.TotalOverTime_4 <  @IN_HourDate");
                                base.AddParam(paras, "IN_HourDate", hourDate);
                            }
                            break;
                        case "OverTimeHours_4":
                            if (compare == "0")
                            {
                                cmdWhere2.AppendLine(" B.TotalOverTime_5 >= @IN_HourDate");
                                base.AddParam(paras, "IN_HourDate", hourDate);
                            }
                            else
                            {
                                cmdWhere2.AppendLine(" B.TotalOverTime_5 <  @IN_HourDate");
                                base.AddParam(paras, "IN_HourDate", hourDate);
                            }
                            break;
                    }
                }
                else if (Overtime_Vacation.Contains("Vacation"))
                {
                    if (compare == "0")
                    {
                        cmdWhere2.AppendLine(" B.Vacation >=  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                    else
                    {
                        cmdWhere2.AppendLine(" B.Vacation <  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                }
            }

            //Check SQL
            if (cmdWhere2.Length != 0)
            {
                cmdText.AppendLine(" WHERE");

                cmdWhere2 = cmdWhere2.Replace("AND", "", 0, 3);

                cmdText.AppendLine(cmdWhere2.ToString());
            }

            return int.Parse(this.db.ExecuteScalar1(cmdText.ToString(), paras).ToString());
        }

        /// </summary>
        /// <param name="groupCD"></param>
        /// <param name="groupName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="sortDirec"></param>
        /// <returns></returns>
        public IList<AttendanceSummaryInfo> GetListAttendanceSummaryAll(string WorkingCalendarId,
                                                                          List<string> lstUserId,
                                                                          DateTime? startDate,
                                                                          DateTime? endDate,
                                                                          List<string> lstDeptId, 
                                                                          string Overtime_Vacation, 
                                                                          string compare, decimal hourDate,
                                                                          int pageIndex, int pageSize,
                                                                          string sortField, int sortDirec)
        {
            string[] strOvertime_Vacation = new string[2];
            
            if (!Overtime_Vacation.Contains("TotalWorkingHours") 
                && (!Overtime_Vacation.Contains("TotalOverTimeHours"))
                && (!Overtime_Vacation.Contains("SH_Hours"))
                && (!Overtime_Vacation.Contains("LH_Hours")))
            {
                strOvertime_Vacation = Overtime_Vacation.Split('_');
            }

            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT B.*,");
            cmdText.AppendLine("");
            cmdText.AppendFormat(" ROW_NUMBER() OVER (ORDER BY B.{0} {1}, B.{2}) AS RowNumber  "
                                    , sortField.Equals("D") ? "DepartmentCD" : "UserCD"
                                    , sortDirec == (int)SortDirec.ASC ? "ASC" : "DESC"
                                    , sortField.Equals("D") ? "UserCD" : "DepartmentCD");
            cmdText.AppendLine(" FROM ");
            cmdText.AppendLine(" ( ");
            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  A.UID");
            cmdText.AppendLine(" ,MIN(A.UserCD) as UserCD");
            cmdText.AppendLine(" ,MIN(A.UserName1) AS UserName1");
            cmdText.AppendLine(" ,MIN(A.DepartmentName) AS DepartmentName");
            cmdText.AppendLine(" ,MIN(A.DepartmentCD) AS DepartmentCD");
            cmdText.AppendLine(" ,SUM(ISNULL(A.[WorkingHours],0)) AS [WorkingHours]");
            cmdText.AppendLine(" ,SUM(ISNULL(A.[LateHours],0)) AS [LateHours]");
            cmdText.AppendLine(" ,SUM(ISNULL(A.[EarlyHours],0)) AS [EarlyHours]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[SH_Hours],0) ELSE 0 END) AS [SH_Hours]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[LH_Hours],0) ELSE 0 END) AS [LH_Hours]");
            //cmdText.AppendLine(" ,COUNT(CASE  WHEN (A.EntryTime IS NOT NULL) THEN NULLIF(A.WorkingHours,0) END) AS NumWorkingDays");
            cmdText.AppendLine(string.Format(" ,COUNT(CASE  WHEN (A.EntryTime IS NOT NULL AND A.WorkingType = {0}) THEN 1 END) AS NumWorkingDays", (int)WorkingType.WorkFullTime));
            cmdText.AppendLine(" ,COUNT(NULLIF(A.LateHours,0)) AS NumLateDays");
            cmdText.AppendLine(" ,COUNT(NULLIF(A.EarlyHours,0)) AS NumEarlyDays");
            //cmdText.AppendLine(" ,COUNT(NULLIF(A.SH_Hours,0)) AS NumSH_Days");
            cmdText.AppendLine(string.Format(" ,COUNT(CASE  WHEN (A.EntryTime IS NOT NULL AND A.WorkingType = {0} AND A.ExchangeFlag = 0) THEN 1 END) AS NumSH_Days", (int)WorkingType.WorkHoliDay));
            //cmdText.AppendLine(" ,COUNT(NULLIF(A.LH_Hours,0)) AS NumLH_Days");
            cmdText.AppendLine(string.Format(" ,COUNT(CASE  WHEN (A.EntryTime IS NOT NULL AND A.WorkingType = {0} AND A.ExchangeFlag = 0) THEN 1 END) AS NumLH_Days", (int)WorkingType.LegalHoliDay));

            cmdText.AppendLine(" ,SUM(ISNULL(A.[OverTimeHours1],0)) AS [OverTimeHours1]");
            cmdText.AppendLine(" ,SUM(ISNULL(A.[OverTimeHours2],0)) AS [OverTimeHours2]");
            cmdText.AppendLine(" ,SUM(ISNULL(A.[OverTimeHours3],0)) AS [OverTimeHours3]");
            cmdText.AppendLine(" ,SUM(ISNULL(A.[OverTimeHours4],0)) AS [OverTimeHours4]");
            cmdText.AppendLine(" ,SUM(ISNULL(A.[OverTimeHours5],0)) AS [OverTimeHours5]");

            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[SH_OverTimeHours1],0) ELSE 0 END) AS [SH_OverTimeHours1]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[SH_OverTimeHours2],0) ELSE 0 END) AS [SH_OverTimeHours2]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[SH_OverTimeHours3],0) ELSE 0 END) AS [SH_OverTimeHours3]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[SH_OverTimeHours4],0) ELSE 0 END) AS [SH_OverTimeHours4]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[SH_OverTimeHours5],0) ELSE 0 END) AS [SH_OverTimeHours5]");

            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[LH_OverTimeHours1],0) ELSE 0 END) AS [LH_OverTimeHours1]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[LH_OverTimeHours2],0) ELSE 0 END) AS [LH_OverTimeHours2]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[LH_OverTimeHours3],0) ELSE 0 END) AS [LH_OverTimeHours3]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[LH_OverTimeHours4],0) ELSE 0 END) AS [LH_OverTimeHours4]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[LH_OverTimeHours5],0) ELSE 0 END) AS [LH_OverTimeHours5]");

            cmdText.AppendLine(" ,SUM(ISNULL(A.[OverTimeHours1], 0 )) + SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[SH_OverTimeHours1], 0 ) ELSE 0 END) + SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[LH_OverTimeHours1], 0 ) ELSE 0 END) AS TotalOverTime_1");
            cmdText.AppendLine(" ,SUM(ISNULL(A.[OverTimeHours2], 0 )) + SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[SH_OverTimeHours2], 0 ) ELSE 0 END) + SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[LH_OverTimeHours2], 0 ) ELSE 0 END) AS TotalOverTime_2");
            cmdText.AppendLine(" ,SUM(ISNULL(A.[OverTimeHours3], 0 )) + SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[SH_OverTimeHours3], 0 ) ELSE 0 END) + SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[LH_OverTimeHours3], 0 ) ELSE 0 END) AS TotalOverTime_3");
            cmdText.AppendLine(" ,SUM(ISNULL(A.[OverTimeHours4], 0 )) + SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[SH_OverTimeHours4], 0 ) ELSE 0 END) + SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[LH_OverTimeHours4], 0 ) ELSE 0 END) AS TotalOverTime_4");
            cmdText.AppendLine(" ,SUM(ISNULL(A.[OverTimeHours5], 0 )) + SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[SH_OverTimeHours5], 0 ) ELSE 0 END) + SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[LH_OverTimeHours5], 0 ) ELSE 0 END) AS TotalOverTime_5");

            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[TotalOverTimeHours],0) ELSE 0 END) AS [TotalOverTimeHours]");
            cmdText.AppendLine(" ,SUM(CASE WHEN A.ExchangeFlag = 0 THEN ISNULL(A.[TotalWorkingHours],0) ELSE 0 END) AS [TotalWorkingHours]");

            if (Overtime_Vacation.Contains("Vacation"))
            {
                cmdText.AppendLine(" ,SUM(A.Vacation) AS Vacation");
            }
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" ( SELECT");
            cmdText.AppendLine(" CH.InitialDate,");
            cmdText.AppendLine(" U.UserCD,U.UserName1,D.DepartmentName, D.DepartmentCD, MS.WorkingType,");

            //Parameter
            Hashtable paras = new Hashtable();
            if (Overtime_Vacation.Contains("Vacation"))
            {            
                cmdText.AppendLine(" (CASE WHEN A.VacationFullCD = @IN_VacationCd THEN 1 ELSE 0 END) +");
                cmdText.AppendLine(" (CASE WHEN A.VacationMorningCD = @IN_VacationCd then 0.4 ELSE 0 END) + ");
                cmdText.AppendLine(" (CASE WHEN A.VacationAfternoonCD = @IN_VacationCd then 0.6 ELSE 0 END) as Vacation,");
                base.AddParam(paras, "IN_VacationCd", strOvertime_Vacation[1]);
            }
            cmdText.AppendLine(" A.* ");
            cmdText.AppendLine(" FROM [dbo].[T_Attendance] A");
            cmdText.AppendLine(" INNER JOIN [dbo].[T_WorkingCalendar_D] CD ON A.Date = CD.WorkingDate");
            cmdText.AppendLine(" INNER JOIN [dbo].[T_WorkingCalendar_U] CU ON A.UID = CU.UID AND CD.HID = CU.HID");
            cmdText.AppendLine(" INNER JOIN [dbo].[T_WorkingCalendar_H] CH ON CD.HID = CH.ID");
            cmdText.AppendLine(" LEFT JOIN [dbo].[M_WorkingSystem] MS on MS.ID = A.WSID");
            cmdText.AppendLine(" LEFT JOIN dbo.M_User U on U.ID=A.UID");
            cmdText.AppendLine(" LEFT JOIN dbo.M_Department D on D.ID= U.DepartmentID");

            if (WorkingCalendarId != string.Empty && WorkingCalendarId != "-1")
            {
                cmdWhere.AppendLine("AND (CH.ID = @IN_WORKINGCALENDARID)");
                base.AddParam(paras, "IN_WORKINGCALENDARID", WorkingCalendarId);
            }

            if (startDate != null)
            {
                cmdWhere.AppendLine(" AND  A.Date >= @IN_StartdateOfServiceFrom");

                base.AddParam(paras, "IN_StartdateOfServiceFrom", startDate);
            }

            if (endDate != null)
            {
                cmdWhere.AppendLine(" AND A.Date <= @IN_EnddateOfServiceFrom");
                base.AddParam(paras, "IN_EnddateOfServiceFrom", endDate);
            }
            if (!string.IsNullOrEmpty(lstDeptId[0] + lstDeptId[1] + lstUserId[0] + lstUserId[1]))
            {
                cmdWhere.AppendLine(" AND (");
                cmdWhere.AppendLine("  (");

                cmdWhere.AppendLine(" 1 = 1");
                if (!string.IsNullOrEmpty(lstDeptId[0]))
                {
                    cmdWhere.AppendLine(" AND D.ID = " + lstDeptId[0]);
                }
                if (!string.IsNullOrEmpty(lstUserId[0]))
                {
                    cmdWhere.AppendLine("AND A.UID IN (" + lstUserId[0] + ")");
                }
                if (string.IsNullOrEmpty(lstDeptId[0] + lstUserId[0]))
                {
                    cmdWhere.AppendLine(" AND 1 <> 1");
                }

                cmdWhere.AppendLine(" ) OR ( ");

                cmdWhere.AppendLine(" 1 = 1");
                if (!string.IsNullOrEmpty(lstDeptId[1]))
                {
                    cmdWhere.AppendLine(" AND D.ID = " + lstDeptId[1]);
                }
                if (!string.IsNullOrEmpty(lstUserId[1]))
                {
                    cmdWhere.AppendLine("AND A.UID IN (" + lstUserId[1] + ")");
                }
                if (string.IsNullOrEmpty(lstDeptId[1] + lstUserId[1]))
                {
                    cmdWhere.AppendLine(" AND 1 <> 1");
                }

                cmdWhere.AppendLine(" ) ");
                cmdWhere.AppendLine(" ) ");
            }

            //Check SQL
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE");

                cmdWhere = cmdWhere.Replace("AND", "", 0, 3);

                cmdText.AppendLine(cmdWhere.ToString());
            }
            cmdText.AppendLine(" ) A");

            cmdText.AppendLine(" GROUP BY A.UID");
            cmdText.AppendLine(" ) B");

            StringBuilder cmdWhere2 = new StringBuilder();
            if (Overtime_Vacation != "-1")
            {
                //TotalWorkingHours
                if (Overtime_Vacation.Contains("TotalWorkingHours"))
                {
                    if (compare == "0")
                    {
                        cmdWhere2.AppendLine(" B.TotalWorkingHours >=  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                    else
                    {
                        cmdWhere2.AppendLine(" B.TotalWorkingHours <  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                }

                //TotalOverTimeHours
                else if (Overtime_Vacation.Contains("TotalOverTimeHours"))
                {
                    if (compare == "0")
                    {
                        cmdWhere2.AppendLine(" B.TotalOverTimeHours >= @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                    else
                    {
                        cmdWhere2.AppendLine(" B.TotalOverTimeHours < @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                }

                //WorkingDays
                else if (Overtime_Vacation.Contains("WorkingDays"))
                {
                    if (compare == "0")
                    {
                        cmdWhere2.AppendLine(" B.NumWorkingDays >=  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);

                    }
                    else
                    {
                        cmdWhere2.AppendLine(" B.NumWorkingDays <  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                }

                //LateDays
                else if (Overtime_Vacation.Contains("LateDays"))
                {
                    if (compare == "0")
                    {
                        cmdWhere2.AppendLine(" B.NumLateDays >=  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);

                    }
                    else
                    {
                        cmdWhere2.AppendLine(" B.NumLateDays <  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                }

                //EarlyDays
                else if (Overtime_Vacation.Contains("EarlyDays"))
                {
                    if (compare == "0")
                    {
                        cmdWhere2.AppendLine(" B.NumEarlyDays >=  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);

                    }
                    else
                    {
                        cmdWhere2.AppendLine(" B.NumEarlyDays <  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                }

                //SH_Days
                else if (Overtime_Vacation.Contains("SH_Days"))
                {
                    if (compare == "0")
                    {
                        cmdWhere2.AppendLine(" B.NumSH_Days >=  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);

                    }
                    else
                    {
                        cmdWhere2.AppendLine(" B.NumSH_Days <  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                }

                //LH_Days
                else if (Overtime_Vacation.Contains("LH_Days"))
                {
                    if (compare == "0")
                    {
                        cmdWhere2.AppendLine(" B.NumLH_Days >=  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);

                    }
                    else
                    {
                        cmdWhere2.AppendLine(" B.NumLH_Days <  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                }

                //OverTime
                else if (Overtime_Vacation.Contains("OverTimeHours"))
                {
                    switch (Overtime_Vacation)
                    {
                        case "OverTimeHours_0":
                            if (compare == "0")
                            {
                                cmdWhere2.AppendLine(" B.TotalOverTime_1 >= @IN_HourDate");
                                base.AddParam(paras, "IN_HourDate", hourDate);
                            }
                            else
                            {
                                cmdWhere2.AppendLine(" B.TotalOverTime_1 <  @IN_HourDate");
                                base.AddParam(paras, "IN_HourDate", hourDate);
                            }
                            break;
                        case "OverTimeHours_1":
                            if (compare == "0")
                            {
                                cmdWhere2.AppendLine(" B.TotalOverTime_2 >=  @IN_HourDate");
                                base.AddParam(paras, "IN_HourDate", hourDate);
                            }
                            else
                            {
                                cmdWhere2.AppendLine(" B.TotalOverTime_2 <  @IN_HourDate");
                                base.AddParam(paras, "IN_HourDate", hourDate);
                            }
                            break;
                        case "OverTimeHours_2":
                            if (compare == "0")
                            {
                                cmdWhere2.AppendLine(" B.TotalOverTime_3 >= @IN_HourDate");
                                base.AddParam(paras, "IN_HourDate", hourDate);
                            }
                            else
                            {
                                cmdWhere2.AppendLine(" B.TotalOverTime_3 <  @IN_HourDate");
                                base.AddParam(paras, "IN_HourDate", hourDate);
                            }
                            break;
                        case "OverTimeHours_3":
                            if (compare == "0")
                            {
                                cmdWhere2.AppendLine(" B.TotalOverTime_4 >=  @IN_HourDate");
                                base.AddParam(paras, "IN_HourDate", hourDate);
                            }
                            else
                            {
                                cmdWhere2.AppendLine(" B.TotalOverTime_4 <  @IN_HourDate");
                                base.AddParam(paras, "IN_HourDate", hourDate);
                            }
                            break;
                        case "OverTimeHours_4":
                            if (compare == "0")
                            {
                                cmdWhere2.AppendLine(" B.TotalOverTime_5 >= @IN_HourDate");
                                base.AddParam(paras, "IN_HourDate", hourDate);
                            }
                            else
                            {
                                cmdWhere2.AppendLine(" B.TotalOverTime_5 <  @IN_HourDate");
                                base.AddParam(paras, "IN_HourDate", hourDate);
                            }
                            break;
                    }
                }
                else if (Overtime_Vacation.Contains("Vacation"))
                {
                    if (compare == "0")
                    {
                        cmdWhere2.AppendLine(" B.Vacation >=  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                    else
                    {
                        cmdWhere2.AppendLine(" B.Vacation <  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                }
            }

            //Check SQL
            if (cmdWhere2.Length != 0)
            {
                cmdText.AppendLine(" WHERE");

                cmdWhere2 = cmdWhere2.Replace("AND", "", 0, 3);

                cmdText.AppendLine(cmdWhere2.ToString());
            }

            if (pageIndex != -1)
            {
                base.AddParam(paras, "IN_PageIndex", pageIndex);
                base.AddParam(paras, "IN_PageSize", pageSize);
            }
            //SQL OUT
            StringBuilder cmdOutText = new StringBuilder();
            cmdOutText.AppendLine(" SELECT");
            cmdOutText.AppendLine(" DATA.*");
            cmdOutText.AppendLine(" FROM");
            cmdOutText.AppendLine(" (");
            cmdOutText.AppendLine(cmdText.ToString());
            cmdOutText.AppendLine(" )");
            cmdOutText.AppendLine(" AS DATA");
            if (pageIndex != -1)
            {
                cmdOutText.AppendLine(" WHERE");
                cmdOutText.AppendLine(" DATA.RowNumber BETWEEN(@IN_PageIndex -1) * @IN_PageSize + 1 AND(((@IN_PageIndex -1) * @IN_PageSize + 1) + @IN_PageSize) - 1");
            }
            return this.db.FindList1<AttendanceSummaryInfo>(cmdOutText.ToString(), paras);
        }

        /// </summary>
        /// <param name="groupCD"></param>
        /// <param name="groupName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="sortDirec"></param>
        /// <returns></returns>
        public IList<ExchangeDateExcelModel> GetListExchangeDateForExcel(string WorkingCalendarId,
                                                                          List<string> lstUserId,
                                                                          List<string> lstDeptId)
        {
            

            //SQL String
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("       U.UserCD");
            cmdText.AppendLine("     , U.UserName1 AS UserName");
            cmdText.AppendLine("     , ConfigD.Value2 AS WorkingTypeName");
            cmdText.AppendLine("     , A.Date");
            cmdText.AppendLine("     , A.EntryTime");
            cmdText.AppendLine("     , A.ExitTime");
            cmdText.AppendLine("     , ATExc.Date AS UsedDate");
            cmdText.AppendLine(" FROM [dbo].[T_Attendance] A");
            cmdText.AppendLine(" INNER JOIN [dbo].[T_WorkingCalendar_D] CD ON A.Date = CD.WorkingDate AND CD.HID = " + WorkingCalendarId);
            cmdText.AppendLine(" INNER JOIN [dbo].[T_WorkingCalendar_U] CU ON A.UID = CU.UID AND CD.HID = CU.HID");
            cmdText.AppendLine(" INNER JOIN [dbo].[T_WorkingCalendar_H] CH ON CD.HID = CH.ID");
            cmdText.AppendLine(" LEFT JOIN [dbo].[M_WorkingSystem] WS on WS.ID = A.WSID");
            cmdText.AppendLine(" LEFT JOIN dbo.M_User U on U.ID = A.UID");
            cmdText.AppendLine(" LEFT JOIN dbo.M_Department D on D.ID= U.DepartmentID");
            cmdText.AppendLine(" LEFT JOIN dbo.T_Attendance AS ATExc ON ATExc.UID = A.UID AND ATExc.ExchangeDate = A.Date");
            cmdText.AppendLine(" LEFT JOIN dbo.M_Config_D ConfigD ON ConfigD.HID = (SELECT ID FROM M_Config_H WHERE ConfigCD = '" + M_Config_H.CONFIG_CD_WORKING_TYPE + "')");
            cmdText.AppendLine("                         AND ConfigD.Value1 = WS.WorkingType");
            cmdText.AppendLine(" WHERE A.ExchangeFlag = 1");

            if (!string.IsNullOrEmpty(lstDeptId[0] + lstDeptId[1] + lstUserId[0] + lstUserId[1]))
            {
                cmdText.AppendLine(" AND (");
                cmdText.AppendLine("  (");

                cmdText.AppendLine(" 1 = 1");
                if (!string.IsNullOrEmpty(lstDeptId[0]))
                {
                    cmdText.AppendLine(" AND D.ID = " + lstDeptId[0]);
                }
                if (!string.IsNullOrEmpty(lstUserId[0]))
                {
                    cmdText.AppendLine("AND A.UID IN (" + lstUserId[0] + ")");
                }
                if (string.IsNullOrEmpty(lstDeptId[0] + lstUserId[0]))
                {
                    cmdText.AppendLine(" AND 1 <> 1");
                }

                cmdText.AppendLine(" ) OR ( ");

                cmdText.AppendLine(" 1 = 1");
                if (!string.IsNullOrEmpty(lstDeptId[1]))
                {
                    cmdText.AppendLine(" AND D.ID = " + lstDeptId[1]);
                }
                if (!string.IsNullOrEmpty(lstUserId[1]))
                {
                    cmdText.AppendLine("AND A.UID IN (" + lstUserId[1] + ")");
                }
                if (string.IsNullOrEmpty(lstDeptId[1] + lstUserId[1]))
                {
                    cmdText.AppendLine(" AND 1 <> 1");
                }

                cmdText.AppendLine(" ) ");
                cmdText.AppendLine(" ) ");
            }
            cmdText.AppendLine(" ORDER BY U.UserCD, A.Date");

            return this.db.FindList1<ExchangeDateExcelModel>(cmdText.ToString());
        }

        /// <summary>
        /// Get total row
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns></returns>
        public int GetTotalRowApprove(string WorkingCalendarId,
                                List<string> lstUserId,
                                string dateOfServiceFrom,
                                string dateOfServiceTo,
                                List<string> lstDeptId,
                                string Overtime_Vacation,
                                string compare, decimal hourDate)
        {
            string[] strOvertime_Vacation = new string[2];

            if (!Overtime_Vacation.Contains("TotalWorkingHours")
                && (!Overtime_Vacation.Contains("TotalOverTimeHours"))
                && (!Overtime_Vacation.Contains("SH_Hours"))
                && (!Overtime_Vacation.Contains("LH_Hours")))
            {
                strOvertime_Vacation = Overtime_Vacation.Split('_');
            }

            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" COUNT(*)");
            cmdText.AppendLine(" FROM ");
            cmdText.AppendLine(" ( ");
            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" A.UID");
            cmdText.AppendLine(" ,MIN(A.UserCD) AS UserCD");
            cmdText.AppendLine(" ,MIN(A.UserName1) AS UserName1");
            cmdText.AppendLine(" ,MIN(A.DepartmentName) AS DepartmentName");
            cmdText.AppendLine(" ,SUM(ISNULL(A.[WorkingHours],0)) AS [WorkingHours]");
            cmdText.AppendLine(" ,SUM(ISNULL(A.[LateHours],0)) AS [LateHours]");
            cmdText.AppendLine(" ,SUM(ISNULL(A.[EarlyHours],0)) AS [EarlyHours]");
            cmdText.AppendLine(" ,SUM(ISNULL(A.[SH_Hours],0)) AS [SH_Hours]");
            cmdText.AppendLine(" ,SUM(ISNULL(A.[LH_Hours],0)) AS [LH_Hours]");
            cmdText.AppendLine(" ,SUM(ISNULL(A.WorkingDays,0)) AS NumWorkingDays");
            cmdText.AppendLine(" ,SUM(ISNULL(A.LateDays,0)) AS NumLateDays");
            cmdText.AppendLine(" ,SUM(ISNULL(A.EarlyDays,0)) AS NumEarlyDays");
            cmdText.AppendLine(" ,SUM(ISNULL(A.SH_Days,0)) AS NumSH_Days");
            cmdText.AppendLine(" ,SUM(ISNULL(A.LH_Days,0)) AS NumLH_Days");

            cmdText.AppendLine(" ,SUM(A.[OverTimeHours1]) AS [OverTimeHours1]");
            cmdText.AppendLine(" ,SUM(A.[OverTimeHours2]) AS [OverTimeHours2]");
            cmdText.AppendLine(" ,SUM(A.[OverTimeHours3]) AS [OverTimeHours3]");
            cmdText.AppendLine(" ,SUM(A.[OverTimeHours4]) AS [OverTimeHours4]");
            cmdText.AppendLine(" ,SUM(A.[OverTimeHours5]) AS [OverTimeHours5]");

            cmdText.AppendLine(" ,SUM(A.[SH_OverTimeHours1]) AS [SH_OverTimeHours1]");
            cmdText.AppendLine(" ,SUM(A.[SH_OverTimeHours2]) AS [SH_OverTimeHours2]");
            cmdText.AppendLine(" ,SUM(A.[SH_OverTimeHours3]) AS [SH_OverTimeHours3]");
            cmdText.AppendLine(" ,SUM(A.[SH_OverTimeHours4]) AS [SH_OverTimeHours4]");
            cmdText.AppendLine(" ,SUM(A.[SH_OverTimeHours5]) AS [SH_OverTimeHours5]");

            cmdText.AppendLine(" ,SUM(A.[LH_OverTimeHours1]) AS [LH_OverTimeHours1]");
            cmdText.AppendLine(" ,SUM(A.[LH_OverTimeHours2]) AS [LH_OverTimeHours2]");
            cmdText.AppendLine(" ,SUM(A.[LH_OverTimeHours3]) AS [LH_OverTimeHours3]");
            cmdText.AppendLine(" ,SUM(A.[LH_OverTimeHours4]) AS [LH_OverTimeHours4]");
            cmdText.AppendLine(" ,SUM(A.[LH_OverTimeHours5]) AS [LH_OverTimeHours5]");

            cmdText.AppendLine(" ,SUM(ISNULL(A.[OverTimeHours1], 0 )) + SUM(ISNULL(A.[SH_OverTimeHours1], 0 )) + SUM(ISNULL(A.[LH_OverTimeHours1], 0 )) AS TotalOverTime_1");
            cmdText.AppendLine(" ,SUM(ISNULL(A.[OverTimeHours2], 0 )) + SUM(ISNULL(A.[SH_OverTimeHours2], 0 )) + SUM(ISNULL(A.[LH_OverTimeHours2], 0 )) AS TotalOverTime_2");
            cmdText.AppendLine(" ,SUM(ISNULL(A.[OverTimeHours3], 0 )) + SUM(ISNULL(A.[SH_OverTimeHours3], 0 )) + SUM(ISNULL(A.[LH_OverTimeHours3], 0 )) AS TotalOverTime_3");
            cmdText.AppendLine(" ,SUM(ISNULL(A.[OverTimeHours4], 0 )) + SUM(ISNULL(A.[SH_OverTimeHours4], 0 )) + SUM(ISNULL(A.[LH_OverTimeHours4], 0 )) AS TotalOverTime_4");
            cmdText.AppendLine(" ,SUM(ISNULL(A.[OverTimeHours5], 0 )) + SUM(ISNULL(A.[SH_OverTimeHours5], 0 )) + SUM(ISNULL(A.[LH_OverTimeHours5], 0 )) AS TotalOverTime_5");

            cmdText.AppendLine(" ,SUM(ISNULL(A.[TotalOverTimeHours],0)) AS [TotalOverTimeHours]");
            cmdText.AppendLine(" ,SUM(ISNULL(A.[TotalWorkingHours],0)) AS [TotalWorkingHours]");
            if (Overtime_Vacation.Contains("Vacation"))
            {
                cmdText.AppendLine(" ,SUM(A.VacationDate) AS [VacationDate]");
            }

            cmdText.AppendLine("  FROM");
            cmdText.AppendLine(" ( SELECT U.UserCD, U.UserName1, D.DepartmentName,");
            cmdText.AppendLine("  A.*");

            if (Overtime_Vacation.Contains("Vacation"))
            {
                cmdText.AppendLine(", VA.* ");
            }

            cmdText.AppendLine(" FROM [dbo].[T_AttendanceResult] A");
            cmdText.AppendLine(" LEFT JOIN dbo.M_User U on U.ID = A.UID");
            cmdText.AppendLine(" LEFT JOIN dbo.M_Department D on D.ID= U.DepartmentID");

            if (Overtime_Vacation.Contains("Vacation"))
            {
                cmdText.AppendLine(" LEFT JOIN dbo.T_VacationResult VA on A.ID = VA.HID");
            }

            //Parameter
            Hashtable paras = new Hashtable();

            if (Overtime_Vacation.Contains("Vacation"))
            {
                cmdWhere.AppendLine("AND VA.VacationID = @INVacationId");
                base.AddParam(paras, "INVacationId", strOvertime_Vacation[1]);
            }

            if (WorkingCalendarId != string.Empty && WorkingCalendarId != "-1")
            {
                cmdWhere.AppendLine("AND (A.CallendarID = @IN_WORKINGCALENDARID)");
                base.AddParam(paras, "IN_WORKINGCALENDARID", WorkingCalendarId);
            }

            if (dateOfServiceFrom != "-1" && dateOfServiceTo != "-1")
            {
                DateTime startDateFrom = DateTime.MinValue;
                DateTime endDateFrom = DateTime.MinValue;

                DateTime startDateTo = DateTime.MinValue;
                DateTime endDateTo = DateTime.MinValue;
                startDateFrom = DateTime.Parse(dateOfServiceFrom);
                endDateFrom = startDateFrom.AddMonths(1).AddDays(-1);

                startDateTo = DateTime.Parse(dateOfServiceTo);
                endDateTo = startDateTo.AddMonths(1).AddDays(-1);

                cmdWhere.AppendLine(" AND  A.StartDate >= @IN_StartdateOfServiceFrom");
                cmdWhere.AppendLine(" AND A.EndDate <= @IN_EnddateOfServiceFrom");

                base.AddParam(paras, "IN_StartdateOfServiceFrom", startDateFrom);
                base.AddParam(paras, "IN_EnddateOfServiceFrom", endDateTo);
            }
            else
            {
                if (dateOfServiceFrom != "-1")
                {
                    DateTime startDateFrom = DateTime.MinValue;
                    DateTime endDateFrom = DateTime.MinValue;
                    startDateFrom = DateTime.Parse(dateOfServiceFrom);
                    endDateFrom = startDateFrom.AddMonths(1).AddDays(-1);

                    cmdWhere.AppendLine(" AND A.StartDate >= @IN_StartdateOfServiceFrom");
                    base.AddParam(paras, "IN_StartdateOfServiceFrom", startDateFrom);

                }

                if (dateOfServiceTo != "-1")
                {
                    DateTime startDateTo = DateTime.MinValue;
                    DateTime endDateTo = DateTime.MinValue;
                    startDateTo = DateTime.Parse(dateOfServiceTo);
                    endDateTo = startDateTo.AddMonths(1).AddDays(-1);

                    cmdWhere.AppendLine(" AND A.EndDate <= @IN_EnddateOfServiceFrom");
                    base.AddParam(paras, "IN_EnddateOfServiceFrom", endDateTo);
                }
            }

            if (!string.IsNullOrEmpty(lstDeptId[0] + lstDeptId[1] + lstUserId[0] + lstUserId[1]))
            {
                cmdWhere.AppendLine(" AND (");
                cmdWhere.AppendLine("  (");

                cmdWhere.AppendLine(" 1 = 1");
                if (!string.IsNullOrEmpty(lstDeptId[0]))
                {
                    cmdWhere.AppendLine(" AND D.ID = " + lstDeptId[0]);
                }
                if (!string.IsNullOrEmpty(lstUserId[0]))
                {
                    cmdWhere.AppendLine("AND A.UID IN (" + lstUserId[0] + ")");
                }
                if (string.IsNullOrEmpty(lstDeptId[0] + lstUserId[0]))
                {
                    cmdWhere.AppendLine(" AND 1 <> 1");
                }

                cmdWhere.AppendLine(" ) OR ( ");

                cmdWhere.AppendLine(" 1 = 1");
                if (!string.IsNullOrEmpty(lstDeptId[1]))
                {
                    cmdWhere.AppendLine(" AND D.ID = " + lstDeptId[1]);
                }
                if (!string.IsNullOrEmpty(lstUserId[1]))
                {
                    cmdWhere.AppendLine("AND A.UID IN (" + lstUserId[1] + ")");
                }
                if (string.IsNullOrEmpty(lstDeptId[1] + lstUserId[1]))
                {
                    cmdWhere.AppendLine(" AND 1 <> 1");
                }

                cmdWhere.AppendLine(" ) ");
                cmdWhere.AppendLine(" ) ");
            }

            //Check SQL
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE");

                cmdWhere = cmdWhere.Replace("AND", "", 0, 3);

                cmdText.AppendLine(cmdWhere.ToString());
            }
            cmdText.AppendLine(" ) A");

            cmdText.AppendLine(" GROUP BY A.UID");
            cmdText.AppendLine(" ) B");

            StringBuilder cmdWhere2 = new StringBuilder();
            if (Overtime_Vacation != "-1")
            {
                //TotalWorkingHours
                if (Overtime_Vacation.Contains("TotalWorkingHours"))
                {
                    if (compare == "0")
                    {
                        cmdWhere2.AppendLine(" B.TotalWorkingHours >=  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                    else
                    {
                        cmdWhere2.AppendLine(" B.TotalWorkingHours <  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                }

                //TotalOverTimeHours
                else if (Overtime_Vacation.Contains("TotalOverTimeHours"))
                {
                    if (compare == "0")
                    {
                        cmdWhere2.AppendLine(" B.TotalOverTimeHours >=  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                    else
                    {
                        cmdWhere2.AppendLine(" B.TotalOverTimeHours <  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                }

                //WorkingDays
                else if (Overtime_Vacation.Contains("WorkingDays"))
                {
                    if (compare == "0")
                    {
                        cmdWhere2.AppendLine(" B.NumWorkingDays >=  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);

                    }
                    else
                    {
                        cmdWhere2.AppendLine(" B.NumWorkingDays <  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                }

                //LateDays
                else if (Overtime_Vacation.Contains("LateDays"))
                {
                    if (compare == "0")
                    {
                        cmdWhere2.AppendLine(" B.NumLateDays >=  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);

                    }
                    else
                    {
                        cmdWhere2.AppendLine(" B.NumLateDays <  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                }

                //EarlyDays
                else if (Overtime_Vacation.Contains("EarlyDays"))
                {
                    if (compare == "0")
                    {
                        cmdWhere2.AppendLine(" B.NumEarlyDays >=  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);

                    }
                    else
                    {
                        cmdWhere2.AppendLine(" B.NumEarlyDays <  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                }

                //SH_Days
                else if (Overtime_Vacation.Contains("SH_Days"))
                {
                    if (compare == "0")
                    {
                        cmdWhere2.AppendLine(" B.NumSH_Days >=  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);

                    }
                    else
                    {
                        cmdWhere2.AppendLine(" B.NumSH_Days <  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                }

                //LH_Days
                else if (Overtime_Vacation.Contains("LH_Days"))
                {
                    if (compare == "0")
                    {
                        cmdWhere2.AppendLine(" B.NumLH_Days >=  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);

                    }
                    else
                    {
                        cmdWhere2.AppendLine(" B.NumLH_Days <  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                }

                //OverTime
                else if (Overtime_Vacation.Contains("OverTimeHours"))
                {
                    switch (Overtime_Vacation)
                    {
                        case "OverTimeHours_0":
                            if (compare == "0")
                            {
                                cmdWhere2.AppendLine(" B.TotalOverTime_1 >=  @IN_HourDate");
                                base.AddParam(paras, "IN_HourDate", hourDate);
                            }
                            else
                            {
                                cmdWhere2.AppendLine(" B.TotalOverTime_1 <  @IN_HourDate");
                                base.AddParam(paras, "IN_HourDate", hourDate);
                            }
                            break;
                        case "OverTimeHours_1":
                            if (compare == "0")
                            {
                                cmdWhere2.AppendLine(" B.TotalOverTime_2 >=  @IN_HourDate");
                                base.AddParam(paras, "IN_HourDate", hourDate);
                            }
                            else
                            {
                                cmdWhere2.AppendLine(" B.TotalOverTime_2 <  @IN_HourDate");
                                base.AddParam(paras, "IN_HourDate", hourDate);
                            }
                            break;
                        case "OverTimeHours_2":
                            if (compare == "0")
                            {
                                cmdWhere2.AppendLine(" B.TotalOverTime_3 >=  @IN_HourDate");
                                base.AddParam(paras, "IN_HourDate", hourDate);
                            }
                            else
                            {
                                cmdWhere2.AppendLine(" B.TotalOverTime_3 <  @IN_HourDate");
                                base.AddParam(paras, "IN_HourDate", hourDate);
                            }
                            break;
                        case "OverTimeHours_3":
                            if (compare == "0")
                            {
                                cmdWhere2.AppendLine(" B.TotalOverTime_4 >=  @IN_HourDate");
                                base.AddParam(paras, "IN_HourDate", hourDate);
                            }
                            else
                            {
                                cmdWhere2.AppendLine(" B.TotalOverTime_4 <  @IN_HourDate");
                                base.AddParam(paras, "IN_HourDate", hourDate);
                            }
                            break;
                        case "OverTimeHours_4":
                            if (compare == "0")
                            {
                                cmdWhere2.AppendLine(" B.TotalOverTime_5 >=  @IN_HourDate");
                                base.AddParam(paras, "IN_HourDate", hourDate);
                            }
                            else
                            {
                                cmdWhere2.AppendLine(" B.TotalOverTime_5 <  @IN_HourDate");
                                base.AddParam(paras, "IN_HourDate", hourDate);
                            }
                            break;
                    }
                }
                else if (Overtime_Vacation.Contains("Vacation"))
                {
                    if (compare == "0")
                    {
                        cmdWhere2.AppendLine(" B.VacationDate >=  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                    else
                    {
                        cmdWhere2.AppendLine(" B.VacationDate <  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                }
            }

            //Check SQL
            if (cmdWhere2.Length != 0)
            {
                cmdText.AppendLine(" WHERE");

                cmdWhere2 = cmdWhere2.Replace("AND", "", 0, 3);

                cmdText.AppendLine(cmdWhere2.ToString());
            }

            return int.Parse(this.db.ExecuteScalar1(cmdText.ToString(), paras).ToString());
        }

        /// <summary>
        /// GetListAttendanceSummaryApprove
        /// </summary>
        /// <param name="groupCD"></param>
        /// <param name="groupName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="sortDirec"></param>
        /// <returns></returns>
        public IList<AttendanceSummaryInfo> GetListAttendanceSummaryApprove(string WorkingCalendarId,
                                                                            List<string> lstUserId, string dateOfServiceFrom,
                                                                            string dateOfServiceTo, List<string> lstDeptId,
                                                                            string Overtime_Vacation, string compare, decimal hourDate,
                                                                            int pageIndex, int pageSize,
                                                                            string sortField, int sortDirec)
        {
            string[] strOvertime_Vacation = new string[2];

            if (!Overtime_Vacation.Contains("TotalWorkingHours")
                && (!Overtime_Vacation.Contains("TotalOverTimeHours"))
                && (!Overtime_Vacation.Contains("SH_Hours"))
                && (!Overtime_Vacation.Contains("LH_Hours")))
            {
                strOvertime_Vacation = Overtime_Vacation.Split('_');
            }

            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT B.*,");
            cmdText.AppendLine("");
            cmdText.AppendFormat(" ROW_NUMBER() OVER (ORDER BY B.{0} {1}, B.{2}) AS RowNumber  "
                                    , sortField.Equals("D") ? "DepartmentCD" : "UserCD"
                                    , sortDirec == (int)SortDirec.ASC ? "ASC" : "DESC"
                                    , sortField.Equals("D") ? "UserCD" : "DepartmentCD");
            cmdText.AppendLine(" FROM ");
            cmdText.AppendLine(" ( ");
            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  A.UID");
            cmdText.AppendLine(" ,MIN(A.UserCD) AS UserCD");
            cmdText.AppendLine(" ,MIN(A.UserName1) AS UserName1");
            cmdText.AppendLine(" ,MIN(A.DepartmentName) AS DepartmentName");
            cmdText.AppendLine(" ,MIN(A.DepartmentCD) AS DepartmentCD");
            cmdText.AppendLine(" ,SUM(ISNULL(A.[WorkingHours],0)) AS [WorkingHours]");
            cmdText.AppendLine(" ,SUM(ISNULL(A.[LateHours],0)) AS [LateHours]");
            cmdText.AppendLine(" ,SUM(ISNULL(A.[EarlyHours],0)) AS [EarlyHours]");
            cmdText.AppendLine(" ,SUM(ISNULL(A.[SH_Hours],0)) AS [SH_Hours]");
            cmdText.AppendLine(" ,SUM(ISNULL(A.[LH_Hours],0)) AS [LH_Hours]");
            cmdText.AppendLine(" ,SUM(ISNULL(A.WorkingDays,0)) AS NumWorkingDays");
            cmdText.AppendLine(" ,SUM(ISNULL(A.LateDays,0)) AS NumLateDays");
            cmdText.AppendLine(" ,SUM(ISNULL(A.EarlyDays,0)) AS NumEarlyDays");
            cmdText.AppendLine(" ,SUM(ISNULL(A.SH_Days,0)) AS NumSH_Days");
            cmdText.AppendLine(" ,SUM(ISNULL(A.LH_Days,0)) AS NumLH_Days");

            cmdText.AppendLine(" ,SUM(A.[OverTimeHours1]) AS [OverTimeHours1]");
            cmdText.AppendLine(" ,SUM(A.[OverTimeHours2]) AS [OverTimeHours2]");
            cmdText.AppendLine(" ,SUM(A.[OverTimeHours3]) AS [OverTimeHours3]");
            cmdText.AppendLine(" ,SUM(A.[OverTimeHours4]) AS [OverTimeHours4]");
            cmdText.AppendLine(" ,SUM(A.[OverTimeHours5]) AS [OverTimeHours5]");

            cmdText.AppendLine(" ,SUM(A.[SH_OverTimeHours1]) AS [SH_OverTimeHours1]");
            cmdText.AppendLine(" ,SUM(A.[SH_OverTimeHours2]) AS [SH_OverTimeHours2]");
            cmdText.AppendLine(" ,SUM(A.[SH_OverTimeHours3]) AS [SH_OverTimeHours3]");
            cmdText.AppendLine(" ,SUM(A.[SH_OverTimeHours4]) AS [SH_OverTimeHours4]");
            cmdText.AppendLine(" ,SUM(A.[SH_OverTimeHours5]) AS [SH_OverTimeHours5]");

            cmdText.AppendLine(" ,SUM(A.[LH_OverTimeHours1]) AS [LH_OverTimeHours1]");
            cmdText.AppendLine(" ,SUM(A.[LH_OverTimeHours2]) AS [LH_OverTimeHours2]");
            cmdText.AppendLine(" ,SUM(A.[LH_OverTimeHours3]) AS [LH_OverTimeHours3]");
            cmdText.AppendLine(" ,SUM(A.[LH_OverTimeHours4]) AS [LH_OverTimeHours4]");
            cmdText.AppendLine(" ,SUM(A.[LH_OverTimeHours5]) AS [LH_OverTimeHours5]");

            cmdText.AppendLine(" ,SUM(ISNULL(A.[OverTimeHours1], 0 )) + SUM(ISNULL(A.[SH_OverTimeHours1], 0 )) + SUM(ISNULL(A.[LH_OverTimeHours1], 0 )) AS TotalOverTime_1");
            cmdText.AppendLine(" ,SUM(ISNULL(A.[OverTimeHours2], 0 )) + SUM(ISNULL(A.[SH_OverTimeHours2], 0 )) + SUM(ISNULL(A.[LH_OverTimeHours2], 0 )) AS TotalOverTime_2");
            cmdText.AppendLine(" ,SUM(ISNULL(A.[OverTimeHours3], 0 )) + SUM(ISNULL(A.[SH_OverTimeHours3], 0 )) + SUM(ISNULL(A.[LH_OverTimeHours3], 0 )) AS TotalOverTime_3");
            cmdText.AppendLine(" ,SUM(ISNULL(A.[OverTimeHours4], 0 )) + SUM(ISNULL(A.[SH_OverTimeHours4], 0 )) + SUM(ISNULL(A.[LH_OverTimeHours4], 0 )) AS TotalOverTime_4");
            cmdText.AppendLine(" ,SUM(ISNULL(A.[OverTimeHours5], 0 )) + SUM(ISNULL(A.[SH_OverTimeHours5], 0 )) + SUM(ISNULL(A.[LH_OverTimeHours5], 0 )) AS TotalOverTime_5");

            cmdText.AppendLine(" ,SUM(ISNULL(A.[TotalOverTimeHours],0)) AS [TotalOverTimeHours]");
            cmdText.AppendLine(" ,SUM(ISNULL(A.[TotalWorkingHours],0)) AS [TotalWorkingHours]");

            if (Overtime_Vacation.Contains("Vacation"))
            {
                cmdText.AppendLine(" ,SUM(A.VacationDate) AS [VacationDate]");
            }

            cmdText.AppendLine("  FROM");
            cmdText.AppendLine(" ( SELECT U.UserCD, U.UserName1, D.DepartmentName, D.DepartmentCD,");
            cmdText.AppendLine("  A.*");

            if (Overtime_Vacation.Contains("Vacation"))
            {
                cmdText.AppendLine(", VA.* ");
            }
            
            cmdText.AppendLine(" FROM [dbo].[T_AttendanceResult] A");
            cmdText.AppendLine(" LEFT JOIN dbo.M_User U on U.ID = A.UID");
            cmdText.AppendLine(" LEFT JOIN dbo.M_Department D on D.ID= U.DepartmentID");

            if (Overtime_Vacation.Contains("Vacation"))
            {
                cmdText.AppendLine(" LEFT JOIN dbo.T_VacationResult VA on A.ID = VA.HID");
            }

            //Parameter
            Hashtable paras = new Hashtable();

            if (Overtime_Vacation.Contains("Vacation"))
            {
                cmdWhere.AppendLine("AND VA.VacationID = @INVacationId");
                base.AddParam(paras, "INVacationId", strOvertime_Vacation[1]);
            }

            if (WorkingCalendarId != string.Empty && WorkingCalendarId != "-1")
            {
                cmdWhere.AppendLine("AND (A.CallendarID = @IN_WORKINGCALENDARID)");
                base.AddParam(paras, "IN_WORKINGCALENDARID", WorkingCalendarId);
            }

            if (dateOfServiceFrom != "-1" && dateOfServiceTo != "-1")
            {
                DateTime startDateFrom = DateTime.MinValue;
                DateTime endDateFrom = DateTime.MinValue;

                DateTime startDateTo = DateTime.MinValue;
                DateTime endDateTo = DateTime.MinValue;
                startDateFrom = DateTime.Parse(dateOfServiceFrom);
                endDateFrom = startDateFrom.AddMonths(1).AddDays(-1);

                startDateTo = DateTime.Parse(dateOfServiceTo);
                endDateTo = startDateTo.AddMonths(1).AddDays(-1);

                cmdWhere.AppendLine(" AND  A.StartDate >= @IN_StartdateOfServiceFrom");
                cmdWhere.AppendLine(" AND A.EndDate <= @IN_EnddateOfServiceFrom");

                base.AddParam(paras, "IN_StartdateOfServiceFrom", startDateFrom);
                base.AddParam(paras, "IN_EnddateOfServiceFrom", endDateTo);
            }
            else
            {
                if (dateOfServiceFrom != "-1")
                {
                    DateTime startDateFrom = DateTime.MinValue;
                    DateTime endDateFrom = DateTime.MinValue;
                    startDateFrom = DateTime.Parse(dateOfServiceFrom);
                    endDateFrom = startDateFrom.AddMonths(1).AddDays(-1);

                    cmdWhere.AppendLine(" AND A.StartDate >= @IN_StartdateOfServiceFrom");
                    base.AddParam(paras, "IN_StartdateOfServiceFrom", startDateFrom);

                }

                if (dateOfServiceTo != "-1")
                {
                    DateTime startDateTo = DateTime.MinValue;
                    DateTime endDateTo = DateTime.MinValue;
                    startDateTo = DateTime.Parse(dateOfServiceTo);
                    endDateTo = startDateTo.AddMonths(1).AddDays(-1);

                    cmdWhere.AppendLine(" AND A.EndDate <= @IN_EnddateOfServiceFrom");
                    base.AddParam(paras, "IN_EnddateOfServiceFrom", endDateTo);
                }
            }

            if (!string.IsNullOrEmpty(lstDeptId[0] + lstDeptId[1] + lstUserId[0] + lstUserId[1]))
            {
                cmdWhere.AppendLine(" AND (");
                cmdWhere.AppendLine("  (");

                cmdWhere.AppendLine(" 1 = 1");
                if (!string.IsNullOrEmpty(lstDeptId[0]))
                {
                    cmdWhere.AppendLine(" AND D.ID = " + lstDeptId[0]);
                }
                if (!string.IsNullOrEmpty(lstUserId[0]))
                {
                    cmdWhere.AppendLine("AND A.UID IN (" + lstUserId[0] + ")");
                }
                if (string.IsNullOrEmpty(lstDeptId[0] + lstUserId[0]))
                {
                    cmdWhere.AppendLine(" AND 1 <> 1");
                }

                cmdWhere.AppendLine(" ) OR ( ");

                cmdWhere.AppendLine(" 1 = 1");
                if (!string.IsNullOrEmpty(lstDeptId[1]))
                {
                    cmdWhere.AppendLine(" AND D.ID = " + lstDeptId[1]);
                }
                if (!string.IsNullOrEmpty(lstUserId[1]))
                {
                    cmdWhere.AppendLine("AND A.UID IN (" + lstUserId[1] + ")");
                }
                if (string.IsNullOrEmpty(lstDeptId[1] + lstUserId[1]))
                {
                    cmdWhere.AppendLine(" AND 1 <> 1");
                }

                cmdWhere.AppendLine(" ) ");
                cmdWhere.AppendLine(" ) ");
            }

            //Check SQL
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE");

                cmdWhere = cmdWhere.Replace("AND", "", 0, 3);

                cmdText.AppendLine(cmdWhere.ToString());
            }
            cmdText.AppendLine(" ) A");

            cmdText.AppendLine(" GROUP BY A.UID");
            cmdText.AppendLine(" ) B");

            StringBuilder cmdWhere2 = new StringBuilder();
            if (Overtime_Vacation != "-1")
            {
                //TotalWorkingHours
                if (Overtime_Vacation.Contains("TotalWorkingHours"))
                {
                    if (compare == "0")
                    {
                        cmdWhere2.AppendLine(" B.TotalWorkingHours >=  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                    else
                    {
                        cmdWhere2.AppendLine(" B.TotalWorkingHours <  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                }

                //TotalOverTimeHours
                else if (Overtime_Vacation.Contains("TotalOverTimeHours"))
                {
                    if (compare == "0")
                    {
                        cmdWhere2.AppendLine(" B.TotalOverTimeHours >= @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                    else
                    {
                        cmdWhere2.AppendLine(" B.TotalOverTimeHours < @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                }

                //WorkingDays
                else if (Overtime_Vacation.Contains("WorkingDays"))
                {
                    if (compare == "0")
                    {
                        cmdWhere2.AppendLine(" B.NumWorkingDays >=  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);

                    }
                    else
                    {
                        cmdWhere2.AppendLine(" B.NumWorkingDays <  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                }

                //LateDays
                else if (Overtime_Vacation.Contains("LateDays"))
                {
                    if (compare == "0")
                    {
                        cmdWhere2.AppendLine(" B.NumLateDays >=  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);

                    }
                    else
                    {
                        cmdWhere2.AppendLine(" B.NumLateDays <  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                }

                //EarlyDays
                else if (Overtime_Vacation.Contains("EarlyDays"))
                {
                    if (compare == "0")
                    {
                        cmdWhere2.AppendLine(" B.NumEarlyDays >=  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);

                    }
                    else
                    {
                        cmdWhere2.AppendLine(" B.NumEarlyDays <  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                }

                //SH_Days
                else if (Overtime_Vacation.Contains("SH_Days"))
                {
                    if (compare == "0")
                    {
                        cmdWhere2.AppendLine(" B.NumSH_Days >=  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);

                    }
                    else
                    {
                        cmdWhere2.AppendLine(" B.NumSH_Days <  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                }

                //LH_Days
                else if (Overtime_Vacation.Contains("LH_Days"))
                {
                    if (compare == "0")
                    {
                        cmdWhere2.AppendLine(" B.NumLH_Days >=  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);

                    }
                    else
                    {
                        cmdWhere2.AppendLine(" B.NumLH_Days <  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                }

                //OverTime
                else if (Overtime_Vacation.Contains("OverTimeHours"))
                {
                    switch (Overtime_Vacation)
                    {
                        case "OverTimeHours_0":
                            if (compare == "0")
                            {
                                cmdWhere2.AppendLine(" B.TotalOverTime_1 >=  @IN_HourDate");
                                base.AddParam(paras, "IN_HourDate", hourDate);
                            }
                            else
                            {
                                cmdWhere2.AppendLine(" B.TotalOverTime_1 <  @IN_HourDate");
                                base.AddParam(paras, "IN_HourDate", hourDate);
                            }
                            break;
                        case "OverTimeHours_1":
                            if (compare == "0")
                            {
                                cmdWhere2.AppendLine(" B.TotalOverTime_2 >=  @IN_HourDate");
                                base.AddParam(paras, "IN_HourDate", hourDate);
                            }
                            else
                            {
                                cmdWhere2.AppendLine(" B.TotalOverTime_2 < @IN_HourDate");
                                base.AddParam(paras, "IN_HourDate", hourDate);
                            }
                            break;
                        case "OverTimeHours_2":
                            if (compare == "0")
                            {
                                cmdWhere2.AppendLine(" B.TotalOverTime_3 >= @IN_HourDate");
                                base.AddParam(paras, "IN_HourDate", hourDate);
                            }
                            else
                            {
                                cmdWhere2.AppendLine(" B.TotalOverTime_3 < @IN_HourDate");
                                base.AddParam(paras, "IN_HourDate", hourDate);
                            }
                            break;
                        case "OverTimeHours_3":
                            if (compare == "0")
                            {
                                cmdWhere2.AppendLine(" B.TotalOverTime_4 >=  @IN_HourDate");
                                base.AddParam(paras, "IN_HourDate", hourDate);
                            }
                            else
                            {
                                cmdWhere2.AppendLine(" B.TotalOverTime_4 <  @IN_HourDate");
                                base.AddParam(paras, "IN_HourDate", hourDate);
                            }
                            break;
                        case "OverTimeHours_4":
                            if (compare == "0")
                            {
                                cmdWhere2.AppendLine(" B.TotalOverTime_5 >=  @IN_HourDate");
                                base.AddParam(paras, "IN_HourDate", hourDate);
                            }
                            else
                            {
                                cmdWhere2.AppendLine(" B.TotalOverTime_5 <  @IN_HourDate");
                                base.AddParam(paras, "IN_HourDate", hourDate);
                            }
                            break;
                    }
                }
                else if (Overtime_Vacation.Contains("Vacation"))
                {
                    if (compare == "0")
                    {
                        cmdWhere2.AppendLine(" B.VacationDate >=  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                    else
                    {
                        cmdWhere2.AppendLine(" B.VacationDate <  @IN_HourDate");
                        base.AddParam(paras, "IN_HourDate", hourDate);
                    }
                }
            }

            //Check SQL
            if (cmdWhere2.Length != 0)
            {
                cmdText.AppendLine(" WHERE");

                cmdWhere2 = cmdWhere2.Replace("AND", "", 0, 3);

                cmdText.AppendLine(cmdWhere2.ToString());
            }

            if (pageIndex != -1)
            {
                base.AddParam(paras, "IN_PageIndex", pageIndex);
                base.AddParam(paras, "IN_PageSize", pageSize);

            }
            //SQL OUT
            StringBuilder cmdOutText = new StringBuilder();
            cmdOutText.AppendLine(" SELECT");
            cmdOutText.AppendLine(" DATA.*");
            cmdOutText.AppendLine(" FROM");
            cmdOutText.AppendLine(" (");
            cmdOutText.AppendLine(cmdText.ToString());
            cmdOutText.AppendLine(" )");
            cmdOutText.AppendLine(" AS DATA");

            if (pageIndex != -1)
            {
                cmdOutText.AppendLine(" WHERE");
                cmdOutText.AppendLine(" DATA.RowNumber BETWEEN(@IN_PageIndex -1) * @IN_PageSize + 1 AND(((@IN_PageIndex -1) * @IN_PageSize + 1) + @IN_PageSize) - 1");
            }

            return this.db.FindList1<AttendanceSummaryInfo>(cmdOutText.ToString(), paras);

        }


        /// </summary>
        /// <param name="WorkingCalendarId"></param>
        /// <param name="dateOfServiceFrom"></param>
        /// <param name="dateOfServiceTo"></param>
        /// <returns></returns>
        public int GetListDateCSV(string WorkingCalendarId, DateTime startDate, DateTime endDate)
        {
            
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT COUNT(WKD.WorkingDate) AS TotalNomalDate");
            cmdText.AppendLine(" FROM [dbo].[T_WorkingCalendar_H] WKH");
            cmdText.AppendLine(" INNER JOIN [dbo].[T_WorkingCalendar_D] WKD ON WKH.ID = WKD.HID");
            cmdText.AppendLine(" INNER JOIN [dbo].[M_WorkingSystem] WS ON WKD.WorkingSystemID = WS.ID");

            //Parameter
            Hashtable paras = new Hashtable();

            //check workingtype = nomal
            cmdWhere.AppendLine("AND WS.WorkingType = 0");

            if (WorkingCalendarId != string.Empty && WorkingCalendarId != "-1")
            {
                cmdWhere.AppendLine(" AND (WKH.ID = @IN_WORKINGCALENDARID)");
                base.AddParam(paras, "IN_WORKINGCALENDARID", WorkingCalendarId);
            }

            
            cmdWhere.AppendLine(" AND WKD.WorkingDate BETWEEN @IN_StartdateOfServiceFrom AND @IN_EnddateOfServiceFrom");

            base.AddParam(paras, "IN_StartdateOfServiceFrom", startDate);
            base.AddParam(paras, "IN_EnddateOfServiceFrom", endDate);

            //Check SQL
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE");

                cmdWhere = cmdWhere.Replace("AND", "", 0, 3);

                cmdText.AppendLine(cmdWhere.ToString());
            }

            return int.Parse(this.db.ExecuteScalar1(cmdText.ToString(), paras).ToString());
        }

        /// <summary>
        /// GetListVacationDateForAttendanceSummary
        /// </summary>
        /// <param name="configCd"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="uId"></param>
        /// <returns></returns>
        public IList<VacationDateInFoByAttendanceSummary> GetListVacationDateForAttendanceSummary(string configCd, DateTime startDate, DateTime endDate, int uId)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();
            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" VA.VacationID AS Value1");
            cmdText.AppendLine(" ,MIN(VA.VacationName) AS Value3");
            cmdText.AppendLine(" ,SUM(VA.VacationDate) AS VacationDate");
            cmdText.AppendLine(" FROM T_AttendanceResult AT");
            cmdText.AppendLine(" INNER JOIN T_VacationResult VA");
            cmdText.AppendLine(" ON AT.ID = VA.HID");

            //Parameter
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine("AND UID = @IN_UID");
            cmdWhere.AppendLine(" AND AT.StartDate >= @IN_StartDate ");
            cmdWhere.AppendLine(" AND AT.EndDate <= @IN_EndDate");
            base.AddParam(paras, "IN_UID", uId);
            base.AddParam(paras, "IN_StartDate", startDate, true);
            base.AddParam(paras, "IN_EndDate", endDate, true);

            //Check SQL
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE");

                cmdWhere = cmdWhere.Replace("AND", "", 0, 3);
                cmdText.AppendLine(cmdWhere.ToString());
            }
            cmdText.AppendLine(" GROUP BY VA.VacationID");

            return this.db.FindList1<VacationDateInFoByAttendanceSummary>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// GetContentByID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public IList<ContentInfo> GetContentByID(int ID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();
            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" -1 AS VacationFlag,");
            cmdText.AppendLine(" -1 AS Value3,");
            cmdText.AppendLine(" wd.WorkTime ,");
            cmdText.AppendLine(" mp.ProjectName ");
            cmdText.AppendLine(" FROM dbo.T_Work_D AS wd");
            cmdText.AppendLine(" LEFT JOIN dbo.M_Project AS mp");
            cmdText.AppendLine(" ON wd.PID = mp.ID");

            //Parameter
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine("AND wd.HID = @IN_ID");
            base.AddParam(paras, "IN_ID", ID);

            //Check SQL
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE");

                cmdWhere = cmdWhere.Replace("AND", "", 0, 3);
                cmdText.AppendLine(cmdWhere.ToString());
            }
            return this.db.FindList1<ContentInfo>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// checkSubmission
        /// </summary>
        /// <param name="Uid"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public int checkSubmission(int Uid, DateTime startDate, DateTime endDate)
        {
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" COUNT(*)");
            cmdText.AppendLine(" FROM dbo.T_WorkingCalendar_D AS WD");
            cmdText.AppendLine(" INNER JOIN dbo.T_WorkingCalendar_U AS WU");
            cmdText.AppendLine(" ON WU.UID =@IN_UID");
            cmdText.AppendLine("  AND WD.HID=WU.HID");

            cmdText.AppendLine(" INNER JOIN dbo.M_WorkingSystem AS WS");
            cmdText.AppendLine(" ON WS.ID =WD.WorkingSystemID");

            cmdText.AppendLine(" LEFT JOIN dbo.T_Attendance AS A");
            cmdText.AppendLine(" ON WU.UID= A.UID");
            cmdText.AppendLine(" AND (A.Date >= @IN_StartDate AND A.Date <= @IN_EndDate)");
            cmdText.AppendLine("AND A.Date = WD.WorkingDate");

            //Parameter
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine("AND WS.WorkingType=0");
            base.AddParam(paras, "IN_UID", Uid);
            base.AddParam(paras, "IN_StartDate", startDate, true);
            base.AddParam(paras, "IN_EndDate", endDate, true);

            //Check SQL
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE");

                cmdWhere = cmdWhere.Replace("AND", "", 0, 3);
                cmdWhere.AppendLine(" AND A.ID is null");
                cmdWhere.AppendLine(" AND (WD.WorkingDate >=  @IN_StartDate AND WD.WorkingDate <= @IN_EndDate)");
                cmdText.AppendLine(cmdWhere.ToString());
            }
            return int.Parse(this.db.ExecuteScalar1(cmdText.ToString(), paras).ToString());

        }

        /// <summary>
        /// checkSubmission
        /// </summary>
        /// <param name="Uid"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public bool HaveUnAprrovaledData(int Uid, DateTime startDate, DateTime endDate)
        {
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" COUNT(*)");
            cmdText.AppendLine(" FROM dbo.T_Attendance AS A");
            cmdText.AppendLine(" WHERE A.UID = @IN_UID");
            cmdText.AppendLine(" AND A.Date >= @IN_StartDate AND A.Date <= @IN_EndDate");
            cmdText.AppendLine(" AND A.ApprovalStatus NOT IN (" + AttendanceApprovalStatus.None.GetHashCode() + "," + AttendanceApprovalStatus.Approved.GetHashCode() + ")");

            //Parameter
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_UID", Uid);
            base.AddParam(paras, "IN_StartDate", startDate, true);
            base.AddParam(paras, "IN_EndDate", endDate, true);

            return int.Parse(this.db.ExecuteScalar1(cmdText.ToString(), paras).ToString()) > 0;

        }

        /// <summary>
        /// checkApproval
        /// </summary>
        /// <param name="Uid"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public int checkAttendanceSubmit(int Uid, DateTime startDate, DateTime endDate)
        {
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" COUNT(*)");
            cmdText.AppendLine(" FROM dbo.T_Attendance AS A");

            //Parameter
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine("AND (A.Date >= @IN_StartDate AND A.Date <= @IN_EndDate)");
            base.AddParam(paras, "IN_UID", Uid);
            base.AddParam(paras, "IN_StartDate", startDate, true);
            base.AddParam(paras, "IN_EndDate", endDate, true);

            //Check SQL
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE");

                cmdWhere = cmdWhere.Replace("AND", "", 0, 3);
                cmdWhere.AppendLine(" AND A.StatusFlag != 0");
                cmdWhere.AppendLine(" AND A.UID = @IN_UID");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return int.Parse(this.db.ExecuteScalar1(cmdText.ToString(), paras).ToString());

        }

        /// <summary>
        /// checkApproval
        /// </summary>
        /// <param name="Uid"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public int checkAttendanceApproval(int Uid, DateTime startDate, DateTime endDate)
        {
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" COUNT(*)");
            cmdText.AppendLine(" FROM dbo.T_Attendance AS A");

            //Parameter
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine("AND (A.Date >= @IN_StartDate AND A.Date <= @IN_EndDate)");
            base.AddParam(paras, "IN_UID", Uid);
            base.AddParam(paras, "IN_StartDate", startDate, true);
            base.AddParam(paras, "IN_EndDate", endDate, true);

            //Check SQL
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE");

                cmdWhere = cmdWhere.Replace("AND", "", 0, 3);
                cmdWhere.AppendLine(" AND A.StatusFlag = 2");
                cmdWhere.AppendLine(" AND A.UID = @IN_UID");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return int.Parse(this.db.ExecuteScalar1(cmdText.ToString(), paras).ToString());

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public IList<ContentInfo> GetContentRestedByID(int ID, string vacationCd)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine("WITH ATD AS");
            cmdText.AppendLine(" (");
	        cmdText.AppendLine("    SELECT");
		    cmdText.AppendLine("        atd.VacationFlag,");
		    cmdText.AppendLine("        atd.VacationMorningCD,");
		    cmdText.AppendLine("         atd.VacationAfternoonCD,");
		    cmdText.AppendLine("         atd.VacationFullCD");
	        cmdText.AppendLine("    FROM");
		    cmdText.AppendLine("        [dbo].[T_Attendance] AS atd");
	        cmdText.AppendLine("    WHERE");
            cmdText.AppendLine("        atd.VacationFlag IS NOT NULL AND atd.ID = @IN_ID");
            cmdText.AppendLine(" )");
            cmdText.AppendLine(" , CONFIG AS (");
	        cmdText.AppendLine("    SELECT ");
		    cmdText.AppendLine("        Value1");
		    cmdText.AppendLine("        , Value2 AS Value3");
	        cmdText.AppendLine("     FROM [dbo].[M_Config_H] as cfh");
	        cmdText.AppendLine("     LEFT JOIN [dbo].[M_Config_D] as cfd");
		    cmdText.AppendLine("        ON cfh.ID=cfd.HID");
            cmdText.AppendLine("     WHERE cfh.ConfigCD = @IN_VACATIONCD");
            cmdText.AppendLine(" )");
            cmdText.AppendLine(" SELECT ");
	        cmdText.AppendLine("    ATD.VacationFlag ,");
	        cmdText.AppendLine("    CONFIG.Value3,");
	        cmdText.AppendLine("     '0' AS WorkTime ,");
	        cmdText.AppendLine("     '' AS ProjectName ");
	        cmdText.AppendLine("     ,0 AS RowNumber");
            cmdText.AppendLine(" FROM ");
	        cmdText.AppendLine("    ATD");
            cmdText.AppendLine(" LEFT JOIN");
	        cmdText.AppendLine("    CONFIG");
	        cmdText.AppendLine("    ON ATD.VacationFullCD = CONFIG.Value1");
            cmdText.AppendLine(" WHERE");
	        cmdText.AppendLine("    CONFIG.Value3 IS NOT NULL");
 
            cmdText.AppendLine(" UNION ALL");

            cmdText.AppendLine(" SELECT ");
	        cmdText.AppendLine("    ATD.VacationFlag ,");
	        cmdText.AppendLine("    CONFIG.Value3,");
	        cmdText.AppendLine("     '0' AS WorkTime ,");
	        cmdText.AppendLine("     '' AS ProjectName ");
	        cmdText.AppendLine("     ,1 AS RowNumber");
            cmdText.AppendLine(" FROM ");
	        cmdText.AppendLine("    ATD");
            cmdText.AppendLine(" LEFT JOIN");
	        cmdText.AppendLine("    CONFIG");
	        cmdText.AppendLine("    ON ATD.VacationMorningCD = CONFIG.Value1");
            cmdText.AppendLine(" WHERE");
            cmdText.AppendLine("    CONFIG.Value3 IS NOT NULL");

            cmdText.AppendLine("UNION ALL");

            cmdText.AppendLine("  SELECT ");
	        cmdText.AppendLine("    ATD.VacationFlag ,");
	        cmdText.AppendLine("    CONFIG.Value3,");
	        cmdText.AppendLine("     '0' AS WorkTime ,");
	        cmdText.AppendLine("     '' AS ProjectName ");
	        cmdText.AppendLine("     ,2 AS RowNumber");
            cmdText.AppendLine(" FROM ");
	        cmdText.AppendLine("    ATD");
            cmdText.AppendLine(" LEFT JOIN");
	        cmdText.AppendLine("    CONFIG");
            cmdText.AppendLine("    ON ATD.VacationAfternoonCD = CONFIG.Value1");
            cmdText.AppendLine(" WHERE");
            cmdText.AppendLine("    CONFIG.Value3 IS NOT NULL");

            //Parameter
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_ID", ID);
            base.AddParam(paras, "IN_VACATIONCD", vacationCd);
            return this.db.FindList1<ContentInfo>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Get Data Attendance By Id
        /// </summary>
        /// <param name="AttendanceId">AttendanceId</param>
        /// <returns></returns>
        public T_Attendance GetDataAttendanceById(int AttendanceId)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  *");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.T_Attendance");

            cmdWhere.AppendLine(" ID = @IN_ID");

            //Para
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_ID", AttendanceId);

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.Find1<T_Attendance>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Get Data Attendance By UId and Date
        /// </summary>
        /// <param name="AttendanceId">AttendanceId</param>
        /// <returns></returns>
        public T_Attendance GetAttendanceByUidAndDate(int UId, string Date)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  *");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.T_Attendance");

            cmdWhere.AppendLine(" UID = @IN_UId");
            cmdWhere.AppendLine("AND Date = @IN_Date");

            //Para
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_UId", UId);
            base.AddParam(paras, "IN_Date", Date, true);
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.Find1<T_Attendance>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Get Data Attendance By UId and Date
        /// </summary>
        /// <param name="AttendanceId">AttendanceId</param>
        /// <returns></returns>
        public IList<T_Attendance> GetListAttendanceByUidAndDate(int UId, DateTime startDate, DateTime endDate)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  *");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.T_Attendance");

            cmdWhere.AppendLine(" UID = @IN_UId");
            //Para
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_UId", UId);
            base.AddParam(paras, "IN_StartDate", startDate, true);
            base.AddParam(paras, "IN_EndDate", endDate, true);
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdWhere.AppendLine(" AND (Date >=  @IN_StartDate AND Date <= @IN_EndDate)");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.FindList1<T_Attendance>(cmdText.ToString(), paras);
        }
        /// <summary>
        /// GetAttendanceHeaderInfo
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public AttendanceHeaderInfo GetAttendanceHeaderInfo(DateTime startDate, DateTime endDate, int userID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" ISNULL(SUM(atd.WorkingHours),0) AS WorkingHours,");
            cmdText.AppendLine(" ISNULL(SUM(atd.LateHours),0) AS LateHours,");
            cmdText.AppendLine(" ISNULL(SUM(atd.EarlyHours),0) AS EarlyHours,");
            cmdText.AppendLine(" ISNULL(SUM(CASE WHEN atd.ExchangeFlag = 0 THEN atd.SH_Hours ELSE 0 END), 0) AS SH_Hours,");
            cmdText.AppendLine(" ISNULL(SUM(CASE WHEN atd.ExchangeFlag = 0 THEN atd.LH_Hours ELSE 0 END), 0) AS LH_Hours,");

            cmdText.AppendLine(" ISNULL(SUM(atd.OverTimeHours1),0) +ISNULL(SUM(CASE WHEN atd.ExchangeFlag = 0 THEN atd.SH_OverTimeHours1 ELSE 0 END),0)+ISNULL(SUM(CASE WHEN atd.ExchangeFlag = 0 THEN atd.LH_OverTimeHours1 ELSE 0 END),0) AS OverTimeHours1,");
            cmdText.AppendLine(" ISNULL(SUM(atd.OverTimeHours2),0) +ISNULL(SUM(CASE WHEN atd.ExchangeFlag = 0 THEN atd.SH_OverTimeHours2 ELSE 0 END),0)+ISNULL(SUM(CASE WHEN atd.ExchangeFlag = 0 THEN atd.LH_OverTimeHours2 ELSE 0 END),0) AS OverTimeHours2,");
            cmdText.AppendLine(" ISNULL(SUM(atd.OverTimeHours3),0) +ISNULL(SUM(CASE WHEN atd.ExchangeFlag = 0 THEN atd.SH_OverTimeHours3 ELSE 0 END),0)+ISNULL(SUM(CASE WHEN atd.ExchangeFlag = 0 THEN atd.LH_OverTimeHours3 ELSE 0 END),0) AS OverTimeHours3,");
            cmdText.AppendLine(" ISNULL(SUM(atd.OverTimeHours4),0) +ISNULL(SUM(CASE WHEN atd.ExchangeFlag = 0 THEN atd.SH_OverTimeHours4 ELSE 0 END),0)+ISNULL(SUM(CASE WHEN atd.ExchangeFlag = 0 THEN atd.LH_OverTimeHours4 ELSE 0 END),0) AS OverTimeHours4,");
            cmdText.AppendLine(" ISNULL(SUM(atd.OverTimeHours5),0) +ISNULL(SUM(CASE WHEN atd.ExchangeFlag = 0 THEN atd.SH_OverTimeHours5 ELSE 0 END),0)+ISNULL(SUM(CASE WHEN atd.ExchangeFlag = 0 THEN atd.LH_OverTimeHours5 ELSE 0 END),0) AS OverTimeHours5,");

            cmdText.AppendLine(" ISNULL(SUM(CASE WHEN atd.ExchangeFlag = 0 THEN atd.TotalOverTimeHours ELSE 0 END),0) AS TotalOverTimeHours,");
            cmdText.AppendLine(" ISNULL(SUM(CASE WHEN atd.ExchangeFlag = 0 THEN atd.TotalWorkingHours ELSE 0 END),0) AS TotalWorkingHours,");

            cmdText.AppendLine(string.Format(" COUNT(CASE  WHEN (atd.EntryTime IS NOT NULL AND ms.WorkingType = {0}) THEN 1 END) AS NumWorkingDays,", (int)WorkingType.WorkFullTime));
            cmdText.AppendLine(" COUNT(NULLIF(atd.LateHours,0)) AS NumLateDays,");
            cmdText.AppendLine(" COUNT(NULLIF(atd.EarlyHours,0)) AS NumEarlyDays,");
            //cmdText.AppendLine(" COUNT(NULLIF(atd.SH_Hours,0)) AS NumSH_Days,");
            //cmdText.AppendLine(" COUNT(NULLIF(atd.LH_Hours,0)) AS NumLH_Days");
            cmdText.AppendLine(string.Format(" COUNT(CASE  WHEN (atd.EntryTime IS NOT NULL AND ms.WorkingType = {0} AND atd.ExchangeFlag = 0) THEN 1 END) NumSH_Days,", (int)WorkingType.WorkHoliDay));
            cmdText.AppendLine(string.Format(" COUNT(CASE  WHEN (atd.EntryTime IS NOT NULL AND ms.WorkingType = {0} AND atd.ExchangeFlag = 0) THEN 1 END) NumLH_Days", (int)WorkingType.LegalHoliDay));

            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.T_Attendance AS atd");
            cmdText.AppendLine(" LEFT JOIN [dbo].[M_WorkingSystem] ms on ms.ID = atd.WSID");

            //Parameter
            Hashtable paras = new Hashtable();

            if (startDate != null && endDate != null)
            {
                cmdWhere.AppendLine("AND (atd.Date >= @IN_StartDate AND atd.Date <= @IN_EndDate)");
                base.AddParam(paras, "IN_StartDate", startDate, true);
                base.AddParam(paras, "IN_EndDate", endDate, true);
            }

            cmdWhere.AppendLine("AND (atd.UID = @IN_UID) ");
            base.AddParam(paras, "IN_UID", userID);

            //Check SQL
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE");

                cmdWhere = cmdWhere.Replace("AND", "", 0, 3);
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.Find1<AttendanceHeaderInfo>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// GetDataVacation
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public DataTable GetDataVacation(DateTime startDate, DateTime endDate)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" SUM(atd.WorkingHours) AS WorkingHours,");
            cmdText.AppendLine(" SUM(atd.LateHours) AS LateHours,");
            cmdText.AppendLine(" SUM(atd.EarlyHours) AS EarlyHours,");
            cmdText.AppendLine(" SUM(atd.SH_Hours) AS SH_Hours,");
            cmdText.AppendLine(" SUM(atd.LH_Hours) AS LH_Hours");

            //Parameter
            Hashtable paras = new Hashtable();

            if (startDate != null && endDate != null)
            {
                cmdWhere.AppendLine("AND (atd.Date >= @IN_StartDate AND atd.Date <= @IN_EndDate)");
                base.AddParam(paras, "IN_StartDate", startDate, true);
                base.AddParam(paras, "IN_EndDate", endDate, true);
            }

            //Check SQL
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE");

                cmdWhere = cmdWhere.Replace("AND", "", 0, 3);
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.Find1<DataTable>(cmdText.ToString(), paras);

        }

        /// <summary>
        /// GetListVerification
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public IList<AttendanceVacationInfo> GetListVacation(int userID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();
            var list = new List<AttendanceVacationInfo>();

            //Parameter
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine("A.UID = @IN_ID");
            base.AddParam(paras, "IN_ID", userID);

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" atd2.InitialDate AS month_innit");
            cmdText.AppendLine(" ,atd2.CalendarCD");
            cmdText.AppendLine(" ,atd2.month");
            cmdText.AppendLine(" , SUM(atd2.fullday + offMorning + offAffternoon) AS dayOff");
            cmdText.AppendLine(" FROM (");

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" atd1.*");
            cmdText.AppendLine(" ,Month(DATEADD(MONTH, atd1.Month -1, atd1.InitialDate)) as month");
            cmdText.AppendLine(" FROM (");

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" CH.InitialDate");
            cmdText.AppendLine(" ,CH.CalendarCD");
            cmdText.AppendLine(" ,A.Date");
            cmdText.AppendLine(" ,CASE");
            cmdText.AppendLine(" WHEN A.Date BETWEEN  CH.InitialDate AND DATEADD(DAY, -1, DATEADD(MONTH, 1 , CH.InitialDate)) THEN 1");
            cmdText.AppendLine(" WHEN A.Date BETWEEN  DATEADD(MONTH, 1 , CH.InitialDate) AND DATEADD(DAY, -1, DATEADD(MONTH, 2 , CH.InitialDate)) THEN 2");
            cmdText.AppendLine(" WHEN A.Date BETWEEN  DATEADD(MONTH, 2 , CH.InitialDate) AND DATEADD(DAY, -1, DATEADD(MONTH, 3 , CH.InitialDate)) THEN 3");
            cmdText.AppendLine(" WHEN A.Date BETWEEN  DATEADD(MONTH, 3 , CH.InitialDate) AND DATEADD(DAY, -1, DATEADD(MONTH, 4 , CH.InitialDate)) THEN 4");
            cmdText.AppendLine(" WHEN A.Date BETWEEN  DATEADD(MONTH, 4 , CH.InitialDate) AND DATEADD(DAY, -1, DATEADD(MONTH, 5 , CH.InitialDate)) THEN 5");
            cmdText.AppendLine(" WHEN A.Date BETWEEN  DATEADD(MONTH, 5 , CH.InitialDate) AND DATEADD(DAY, -1, DATEADD(MONTH, 6 , CH.InitialDate)) THEN 6");
            cmdText.AppendLine(" WHEN A.Date BETWEEN  DATEADD(MONTH, 6 , CH.InitialDate) AND DATEADD(DAY, -1, DATEADD(MONTH, 7 , CH.InitialDate)) THEN 7");
            cmdText.AppendLine(" WHEN A.Date BETWEEN  DATEADD(MONTH, 7 , CH.InitialDate) AND DATEADD(DAY, -1, DATEADD(MONTH, 8 , CH.InitialDate)) THEN 8");
            cmdText.AppendLine(" WHEN A.Date BETWEEN  DATEADD(MONTH, 8 , CH.InitialDate) AND DATEADD(DAY, -1, DATEADD(MONTH, 9 , CH.InitialDate)) THEN 9");
            cmdText.AppendLine(" WHEN A.Date BETWEEN  DATEADD(MONTH, 9 , CH.InitialDate) AND DATEADD(DAY, -1, DATEADD(MONTH, 10 , CH.InitialDate)) THEN 10");
            cmdText.AppendLine(" WHEN A.Date BETWEEN  DATEADD(MONTH, 10 , CH.InitialDate) AND DATEADD(DAY, -1, DATEADD(MONTH, 11 , CH.InitialDate)) THEN 11");
            cmdText.AppendLine(" WHEN A.Date BETWEEN  DATEADD(MONTH, 11 , CH.InitialDate) AND DATEADD(DAY, -1, DATEADD(MONTH, 12 , CH.InitialDate)) THEN 12");
            cmdText.AppendLine(" END [Month]");
            cmdText.AppendLine(" ,CASE WHEN A.VacationFullCD = 0 THEN 1 ELSE 0 END AS fullday");
            cmdText.AppendLine(" ,CASE WHEN A.VacationMorningCD = 0 THEN 0.4 ELSE 0 END AS offMorning");
            cmdText.AppendLine(" ,CASE WHEN A.VacationAfternoonCD = 0 THEN 0.6 ELSE 0 END AS offAffternoon");
            cmdText.AppendLine(" FROM dbo.T_Attendance A");

            cmdText.AppendLine(" INNER JOIN dbo.T_WorkingCalendar_D CD ON A.Date = CD.WorkingDate");
            cmdText.AppendLine(" INNER JOIN dbo.T_WorkingCalendar_U CU ON A.UID = CU.UID AND CD.HID = CU.HID");
            cmdText.AppendLine(" INNER JOIN dbo.T_WorkingCalendar_H CH ON CD.HID = CH.ID");
            cmdText.AppendLine(" LEFT JOIN dbo.M_WorkingSystem MS on MS.ID = A.WSID");
            cmdText.AppendLine(" LEFT JOIN dbo.M_User U on U.ID=A.UID");
            cmdText.AppendLine(" LEFT JOIN dbo.M_Department D on D.ID= U.DepartmentID");

            cmdText.AppendLine(" WHERE");
            cmdText.AppendLine(cmdWhere.ToString());
            cmdText.AppendLine(" ) AS atd1");
            cmdText.AppendLine(" ) AS atd2");
            cmdText.AppendLine(" GROUP BY atd2.InitialDate, atd2.CalendarCD, atd2.month");
            cmdText.AppendLine(" ORDER BY atd2.CalendarCD, atd2.month");

            var list_AttendanceVerification = this.db.FindList1<AttendanceVacation>(cmdText.ToString(), paras);
            if (list_AttendanceVerification.Count() == 0)
            {
                return list;
            }
            var month_innit = list_AttendanceVerification[0].month_innit;
            var item = new AttendanceVacationInfo()
            {
                StartMonth = month_innit.Month,
                NextMonth1 = month_innit.AddMonths(1).Month,
                NextMonth2 = month_innit.AddMonths(2).Month,
                NextMonth3 = month_innit.AddMonths(3).Month,
                NextMonth4 = month_innit.AddMonths(4).Month,
                NextMonth5 = month_innit.AddMonths(5).Month,
                NextMonth6 = month_innit.AddMonths(6).Month,
                NextMonth7 = month_innit.AddMonths(7).Month,
                NextMonth8 = month_innit.AddMonths(8).Month,
                NextMonth9 = month_innit.AddMonths(9).Month,
                NextMonth10 = month_innit.AddMonths(10).Month,
                EndMonth = month_innit.AddMonths(11).Month,
            };

            list.Add(item);

            var dayOff = list_AttendanceVerification[0].dayOff == 0 ? null : list_AttendanceVerification[0].dayOff;
            item = new AttendanceVacationInfo()
            {
                Year = list_AttendanceVerification[0].CalendarCD,
                SumDayOff1 = list_AttendanceVerification[0].month == month_innit.Month ? dayOff : null,
                SumDayOff2 = list_AttendanceVerification[0].month == month_innit.AddMonths(1).Month ? dayOff : null,
                SumDayOff3 = list_AttendanceVerification[0].month == month_innit.AddMonths(2).Month ? dayOff : null,
                SumDayOff4 = list_AttendanceVerification[0].month == month_innit.AddMonths(3).Month ? dayOff : null,
                SumDayOff5 = list_AttendanceVerification[0].month == month_innit.AddMonths(4).Month ? dayOff : null,
                SumDayOff6 = list_AttendanceVerification[0].month == month_innit.AddMonths(5).Month ? dayOff : null,
                SumDayOff7 = list_AttendanceVerification[0].month == month_innit.AddMonths(6).Month ? dayOff : null,
                SumDayOff8 = list_AttendanceVerification[0].month == month_innit.AddMonths(7).Month ? dayOff : null,
                SumDayOff9 = list_AttendanceVerification[0].month == month_innit.AddMonths(8).Month ? dayOff : null,
                SumDayOff10 = list_AttendanceVerification[0].month == month_innit.AddMonths(9).Month ? dayOff : null,
                SumDayOff11 = list_AttendanceVerification[0].month == month_innit.AddMonths(10).Month ? dayOff : null,
                SumDayOff12 = list_AttendanceVerification[0].month == month_innit.AddMonths(11).Month ? dayOff : null
            };

            for (var i = 1; i < list_AttendanceVerification.Count(); i++)
            {
                var dayOff_i = list_AttendanceVerification[i].dayOff == 0 ? null : list_AttendanceVerification[i].dayOff;
                if (list_AttendanceVerification[i].CalendarCD == list_AttendanceVerification[i - 1].CalendarCD)
                {
                    item.SumDayOff1 = list_AttendanceVerification[i].month == month_innit.Month ? dayOff_i : item.SumDayOff1;
                    item.SumDayOff2 = list_AttendanceVerification[i].month == month_innit.AddMonths(1).Month ? dayOff_i : item.SumDayOff2;
                    item.SumDayOff3 = list_AttendanceVerification[i].month == month_innit.AddMonths(2).Month ? dayOff_i : item.SumDayOff3;
                    item.SumDayOff4 = list_AttendanceVerification[i].month == month_innit.AddMonths(3).Month ? dayOff_i : item.SumDayOff4;
                    item.SumDayOff5 = list_AttendanceVerification[i].month == month_innit.AddMonths(4).Month ? dayOff_i : item.SumDayOff5;
                    item.SumDayOff6 = list_AttendanceVerification[i].month == month_innit.AddMonths(5).Month ? dayOff_i : item.SumDayOff6;
                    item.SumDayOff7 = list_AttendanceVerification[i].month == month_innit.AddMonths(6).Month ? dayOff_i : item.SumDayOff7;
                    item.SumDayOff8 = list_AttendanceVerification[i].month == month_innit.AddMonths(7).Month ? dayOff_i : item.SumDayOff8;
                    item.SumDayOff9 = list_AttendanceVerification[i].month == month_innit.AddMonths(8).Month ? dayOff_i : item.SumDayOff9;
                    item.SumDayOff10 = list_AttendanceVerification[i].month == month_innit.AddMonths(9).Month ? dayOff_i : item.SumDayOff10;
                    item.SumDayOff11 = list_AttendanceVerification[i].month == month_innit.AddMonths(10).Month ? dayOff_i : item.SumDayOff11;
                    item.SumDayOff12 = list_AttendanceVerification[i].month == month_innit.AddMonths(11).Month ? dayOff_i : item.SumDayOff12;
                }
                else
                {
                    list.Add(item);
                    item = new AttendanceVacationInfo()
                    {
                        Year = list_AttendanceVerification[i].CalendarCD,
                        SumDayOff1 = list_AttendanceVerification[i].month == month_innit.Month ? dayOff_i : null,
                        SumDayOff2 = list_AttendanceVerification[i].month == month_innit.AddMonths(1).Month ? dayOff_i : null,
                        SumDayOff3 = list_AttendanceVerification[i].month == month_innit.AddMonths(2).Month ? dayOff_i : null,
                        SumDayOff4 = list_AttendanceVerification[i].month == month_innit.AddMonths(3).Month ? dayOff_i : null,
                        SumDayOff5 = list_AttendanceVerification[i].month == month_innit.AddMonths(4).Month ? dayOff_i : null,
                        SumDayOff6 = list_AttendanceVerification[i].month == month_innit.AddMonths(5).Month ? dayOff_i : null,
                        SumDayOff7 = list_AttendanceVerification[i].month == month_innit.AddMonths(6).Month ? dayOff_i : null,
                        SumDayOff8 = list_AttendanceVerification[i].month == month_innit.AddMonths(7).Month ? dayOff_i : null,
                        SumDayOff9 = list_AttendanceVerification[i].month == month_innit.AddMonths(8).Month ? dayOff_i : null,
                        SumDayOff10 = list_AttendanceVerification[i].month == month_innit.AddMonths(9).Month ? dayOff_i : null,
                        SumDayOff11 = list_AttendanceVerification[i].month == month_innit.AddMonths(10).Month ? dayOff_i : null,
                        SumDayOff12 = list_AttendanceVerification[i].month == month_innit.AddMonths(11).Month ? dayOff_i : null
                    };
                }
            }
            list.Add(item);
            return list;
        }

        /// <summary>
        /// Get Data Attendance By UId and Date
        /// </summary>
        /// <param name="AttendanceId">AttendanceId</param>
        /// <returns></returns>
        public IList<DropDownModel> GetCmbListExchangeDate(int uid, DateTime date)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  CONVERT(VARCHAR(10), A.Date, 111) as [Value]");
            cmdText.AppendLine("  ,CAST(YEAR(A.Date) AS VARCHAR(4)) + '年' +  CAST(MONTH(A.Date) AS VARCHAR(4)) + '月' +  CAST(DAY(A.Date) AS VARCHAR(4)) + '日' as [DisplayName]");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.T_Attendance A");
            cmdText.AppendLine(" WHERE ");
            cmdText.AppendLine(" A.UID = @IN_UID");
            cmdText.AppendLine(" AND A.Date >  @IN_StartDate");
            cmdText.AppendLine(" AND A.ExchangeFlag = 1");
            cmdText.AppendLine(" AND NOT EXISTS (SELECT 1 FROM T_Attendance A1 WHERE A1.UID = A.UID AND A1.ExchangeDate = A.Date AND A1.Date <> @IN_Date)");

            //Para
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_UID", uid);
            base.AddParam(paras, "IN_StartDate", date.AddDays(-180), true);
            base.AddParam(paras, "IN_Date", date, true);

            return this.db.FindList1<DropDownModel>(cmdText.ToString(), paras);
        }

        public bool ExchangeDateIsUsed(int uid, DateTime date, DateTime exchangeDate)
        {
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("    COUNT(1)");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.T_Attendance A");
            cmdText.AppendLine(" WHERE ");
            cmdText.AppendLine(" A.UID = @IN_UID");
            cmdText.AppendLine(" AND A.ExchangeDate = @IN_ExchangeDate");
            cmdText.AppendLine(" AND A.Date <> @IN_Date");

            //Para
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_UID", uid);
            base.AddParam(paras, "IN_Date", date, true);
            base.AddParam(paras, "IN_ExchangeDate", exchangeDate, true);

            return int.Parse(string.Format("{0}", this.db.ExecuteScalar1(cmdText.ToString(), paras))) > 0;

        }

        public bool IsEnoughPaidVacation(int calendarId, int uid, DateTime fromDate, DateTime toDate)
        {
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine("SELECT");
		   
		    cmdText.AppendLine("ISNULL((");
			cmdText.AppendLine("		SELECT SUM(VacationDay)");
			cmdText.AppendLine("		FROM dbo.T_PaidVacation");
			cmdText.AppendLine("		WHERE UID = @IN_UID");
			cmdText.AppendLine("		AND InvalidFlag = 0),0)");
		   
		    cmdText.AppendLine("- ISNULL((");
			cmdText.AppendLine("		SELECT ");
			cmdText.AppendLine("			SUM(CASE WHEN A.ID IS NULL AND WS.WorkingSystemCD = '4' THEN 1 ELSE 0 END)");//-->4
			cmdText.AppendLine("			+ SUM(CASE WHEN A.ID IS NULL AND PL.ID IS NOT NULL THEN 1 ELSE 0 END)");
			cmdText.AppendLine("		FROM (");
			cmdText.AppendLine("			SELECT WCD.WorkingDate, WCD.WorkingSystemID");
			cmdText.AppendLine("			FROM T_WorkingCalendar_D WCD");
			cmdText.AppendLine("			WHERE WCD.HID = @IN_CalendarID");
			cmdText.AppendLine("			  AND WCD.WorkingDate BETWEEN @IN_FromDate AND @IN_ToDate");
			cmdText.AppendLine("		) WC");
			cmdText.AppendLine("		LEFT JOIN M_WorkingSystem WS ON WC.WorkingSystemID = WS.ID");
			cmdText.AppendLine("		LEFT JOIN T_Attendance A ON A.Date = WC.WorkingDate AND A.UID = @IN_UID");
            cmdText.AppendLine("		LEFT JOIN T_PaidLeave PL ON PL.CalendarID = @IN_CalendarID AND PL.Date = WC.WorkingDate AND PL.UserID = @IN_UID),0)");//-> IN_UID

            //Para
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_CalendarID", calendarId);
            base.AddParam(paras, "IN_UID", uid);
            base.AddParam(paras, "IN_FromDate", fromDate, true);
            base.AddParam(paras, "IN_ToDate", toDate, true);

            return decimal.Parse(string.Format("{0}", this.db.ExecuteScalar1(cmdText.ToString(), paras))) >= 0;
        }

        #endregion

        #region Entity

        #endregion

        #region Get Total Row
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupCD"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public int getTotalRowForList(DateTime startDate, DateTime endDate, string department, string employee)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();
            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" COUNT(*)");
            cmdText.AppendLine(" FROM dbo.T_Attendance AS atd");

            //Parameter
            Hashtable paras = new Hashtable();
            if (startDate != null && endDate != null)
            {
                cmdWhere.AppendLine("AND (atd.Date >= @IN_StartDate AND atd.Date <= @IN_EndDate)");
                base.AddParam(paras, "IN_StartDate", startDate, true);
                base.AddParam(paras, "IN_EndDate", endDate, true);
            }

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE");
                cmdWhere = cmdWhere.Replace("AND", "", 0, 3);
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return int.Parse(this.db.ExecuteScalar1(cmdText.ToString(), paras).ToString());
        }

        #endregion

        #region Insert

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="T_Attendance">tAttendance</param>
        /// <returns></returns>
        public int Insert(T_Attendance tAttendance)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine(" INSERT INTO");
            cmdText.AppendLine(" dbo.T_Attendance");
            cmdText.AppendLine(" (");
            cmdText.AppendLine("  UID");
            cmdText.AppendLine(" ,Date");
            cmdText.AppendLine(" ,WSID");
            cmdText.AppendLine(" ,EntryTime");
            cmdText.AppendLine(" ,ExitTime");
            cmdText.AppendLine(" ,WorkingHours");
            cmdText.AppendLine(" ,LateHours");
            cmdText.AppendLine(" ,EarlyHours");
            cmdText.AppendLine(" ,SH_Hours");
            cmdText.AppendLine(" ,LH_Hours");
            cmdText.AppendLine(" ,OverTimeHours1");
            cmdText.AppendLine(" ,OverTimeHours2");
            cmdText.AppendLine(" ,OverTimeHours3");
            cmdText.AppendLine(" ,OverTimeHours4");
            cmdText.AppendLine(" ,OverTimeHours5");
            cmdText.AppendLine(" ,SH_OverTimeHours1");
            cmdText.AppendLine(" ,SH_OverTimeHours2");
            cmdText.AppendLine(" ,SH_OverTimeHours3");
            cmdText.AppendLine(" ,SH_OverTimeHours4");
            cmdText.AppendLine(" ,SH_OverTimeHours5");
            cmdText.AppendLine(" ,LH_OverTimeHours1");
            cmdText.AppendLine(" ,LH_OverTimeHours2");
            cmdText.AppendLine(" ,LH_OverTimeHours3");
            cmdText.AppendLine(" ,LH_OverTimeHours4");
            cmdText.AppendLine(" ,LH_OverTimeHours5");

            cmdText.AppendLine(" ,TotalWorkingHours");
            cmdText.AppendLine(" ,TotalOverTimeHours");

            cmdText.AppendLine(" ,VacationFlag");
            cmdText.AppendLine(" ,VacationFullCD");
            cmdText.AppendLine(" ,VacationMorningCD");
            cmdText.AppendLine(" ,VacationAfternoonCD");
            cmdText.AppendLine(" ,Memo");
            cmdText.AppendLine(" ,StatusFlag");

            //************* Approval ***************
            cmdText.AppendLine(" ,ApprovalStatus");
            cmdText.AppendLine(" ,ApprovalUID");
            cmdText.AppendLine(" ,ApprovalDate");
            cmdText.AppendLine(" ,ApprovalNote");
            //************* Request ***************
            cmdText.AppendLine(" ,RequestNote");
            //**************************************
            cmdText.AppendLine(" ,ExchangeFlag");
            cmdText.AppendLine(" ,ExchangeDate");

            cmdText.AppendLine(" ,CreateDate");
            cmdText.AppendLine(" ,CreateUID");
            cmdText.AppendLine(" ,UpdateDate");
            cmdText.AppendLine(" ,UpdateUID");
            cmdText.AppendLine(" )");
            cmdText.AppendLine(" VALUES");
            cmdText.AppendLine(" (");

            cmdText.AppendLine("  @IN_UID");
            cmdText.AppendLine(" ,@IN_Date");
            cmdText.AppendLine(" ,@IN_WSID");
            cmdText.AppendLine(" ,@IN_EntryTime");
            cmdText.AppendLine(" ,@IN_ExitTime");
            cmdText.AppendLine(" ,@IN_WorkingHours");
            cmdText.AppendLine(" ,@IN_LateHours");
            cmdText.AppendLine(" ,@IN_EarlyHours");
            cmdText.AppendLine(" ,@IN_SH_Hours");
            cmdText.AppendLine(" ,@IN_LH_Hours");
            cmdText.AppendLine(" ,@IN_OverTimeHours1");
            cmdText.AppendLine(" ,@IN_OverTimeHours2");
            cmdText.AppendLine(" ,@IN_OverTimeHours3");
            cmdText.AppendLine(" ,@IN_OverTimeHours4");
            cmdText.AppendLine(" ,@IN_OverTimeHours5");
            cmdText.AppendLine(" ,@IN_SH_OverTimeHours1");
            cmdText.AppendLine(" ,@IN_SH_OverTimeHours2");
            cmdText.AppendLine(" ,@IN_SH_OverTimeHours3");
            cmdText.AppendLine(" ,@IN_SH_OverTimeHours4");
            cmdText.AppendLine(" ,@IN_SH_OverTimeHours5");
            cmdText.AppendLine(" ,@IN_LH_OverTimeHours1");
            cmdText.AppendLine(" ,@IN_LH_OverTimeHours2");
            cmdText.AppendLine(" ,@IN_LH_OverTimeHours3");
            cmdText.AppendLine(" ,@IN_LH_OverTimeHours4");
            cmdText.AppendLine(" ,@IN_LH_OverTimeHours5");

            cmdText.AppendLine(" ,@IN_TotalWorkingHours");
            cmdText.AppendLine(" ,@IN_TotalOverTimeHours");

            cmdText.AppendLine(" ,@IN_VacationFlag");
            cmdText.AppendLine(" ,@IN_VacationFullCD");
            cmdText.AppendLine(" ,@IN_VacationMorningCD");
            cmdText.AppendLine(" ,@IN_VacationAfternoonCD");
            cmdText.AppendLine(" ,@IN_Memo");
            cmdText.AppendLine(" ,@IN_StatusFlag");

            //************** Approval *******************
            cmdText.AppendLine(" ,@IN_ApprovalStatus");
            cmdText.AppendLine(" ,@IN_ApprovalUID");
            cmdText.AppendLine(" ,@IN_ApprovalDate");
            cmdText.AppendLine(" ,@IN_ApprovalNote");
            //************** Request ********************
            cmdText.AppendLine(" ,@IN_RequestNote");
            //*******************************************
            cmdText.AppendLine(" ,@IN_ExchangeFlag");
            cmdText.AppendLine(" ,@IN_ExchangeDate");
            
            cmdText.AppendLine(" ,GETDATE()");
            cmdText.AppendLine(" ,@IN_CreateUID");
            cmdText.AppendLine(" ,GETDATE()");
            cmdText.AppendLine(" ,@IN_UpdateUID");

            cmdText.AppendLine(" )");

            //Param
            Hashtable paras = new Hashtable();
            ISecurity sec = Security.Instance;

            base.AddParam(paras, "IN_UID", tAttendance.UID);
            base.AddParam(paras, "IN_Date", tAttendance.Date, true);
            base.AddParam(paras, "IN_WSID", tAttendance.WSID);
            base.AddParam(paras, "IN_EntryTime", tAttendance.EntryTime);
            base.AddParam(paras, "IN_ExitTime", tAttendance.ExitTime);
            base.AddParam(paras, "IN_WorkingHours", tAttendance.WorkingHours);
            base.AddParam(paras, "IN_LateHours", tAttendance.LateHours);
            base.AddParam(paras, "IN_EarlyHours", tAttendance.EarlyHours);
            base.AddParam(paras, "IN_SH_Hours", tAttendance.SH_Hours);
            base.AddParam(paras, "IN_LH_Hours", tAttendance.LH_Hours);
            base.AddParam(paras, "IN_OverTimeHours1", tAttendance.OverTimeHours1);
            base.AddParam(paras, "IN_OverTimeHours2", tAttendance.OverTimeHours2);
            base.AddParam(paras, "IN_OverTimeHours3", tAttendance.OverTimeHours3);
            base.AddParam(paras, "IN_OverTimeHours4", tAttendance.OverTimeHours4);
            base.AddParam(paras, "IN_OverTimeHours5", tAttendance.OverTimeHours5);
            base.AddParam(paras, "IN_SH_OverTimeHours1", tAttendance.SH_OverTimeHours1);
            base.AddParam(paras, "IN_SH_OverTimeHours2", tAttendance.SH_OverTimeHours2);
            base.AddParam(paras, "IN_SH_OverTimeHours3", tAttendance.SH_OverTimeHours3);
            base.AddParam(paras, "IN_SH_OverTimeHours4", tAttendance.SH_OverTimeHours4);
            base.AddParam(paras, "IN_SH_OverTimeHours5", tAttendance.SH_OverTimeHours5);
            base.AddParam(paras, "IN_LH_OverTimeHours1", tAttendance.LH_OverTimeHours1);
            base.AddParam(paras, "IN_LH_OverTimeHours2", tAttendance.LH_OverTimeHours2);
            base.AddParam(paras, "IN_LH_OverTimeHours3", tAttendance.LH_OverTimeHours3);
            base.AddParam(paras, "IN_LH_OverTimeHours4", tAttendance.LH_OverTimeHours4);
            base.AddParam(paras, "IN_LH_OverTimeHours5", tAttendance.LH_OverTimeHours5);
            base.AddParam(paras, "IN_TotalWorkingHours", tAttendance.TotalWorkingHours);
            base.AddParam(paras, "IN_TotalOverTimeHours", tAttendance.TotalOverTimeHours);

            base.AddParam(paras, "IN_VacationFlag", tAttendance.VacationFlag);
            base.AddParam(paras, "IN_VacationFullCD", tAttendance.VacationFullCD);
            base.AddParam(paras, "IN_VacationMorningCD", tAttendance.VacationMorningCD);
            base.AddParam(paras, "IN_VacationAfternoonCD", tAttendance.VacationAfternoonCD);
            base.AddParam(paras, "IN_Memo", tAttendance.Memo);
            base.AddParam(paras, "IN_StatusFlag", tAttendance.StatusFlag);

            //******************* Approval ****************************************
            base.AddParam(paras, "IN_ApprovalStatus", tAttendance.ApprovalStatus);
            base.AddParam(paras, "IN_ApprovalUID", tAttendance.ApprovalUID);
            base.AddParam(paras, "IN_ApprovalDate", tAttendance.ApprovalDate);
            base.AddParam(paras, "IN_ApprovalNote", tAttendance.ApprovalNote);
            //******************* Approval ****************************************
            base.AddParam(paras, "IN_RequestNote", tAttendance.RequestNote);
            //*********************************************************************
            base.AddParam(paras, "IN_ExchangeFlag", tAttendance.ExchangeFlag);
            base.AddParam(paras, "IN_ExchangeDate", tAttendance.ExchangeDate);
            
            base.AddParam(paras, "IN_CreateUID", tAttendance.CreateUID);
            base.AddParam(paras, "IN_UpdateUID", tAttendance.UpdateUID);

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }

        #endregion

        #region Update

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="T_Attendance"></param>
        /// <returns></returns>
        public int Update(T_Attendance tAttendance)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" UPDATE");
            cmdText.AppendLine(" dbo.T_Attendance");
            cmdText.AppendLine(" SET");
            cmdText.AppendLine("  UID = @IN_UID");
            cmdText.AppendLine(" ,Date = @IN_Date");
            cmdText.AppendLine(" ,WSID = @IN_WSID");
            cmdText.AppendLine(" ,EntryTime = @IN_EntryTime");
            cmdText.AppendLine(" ,ExitTime = @IN_ExitTime");
            cmdText.AppendLine(" ,WorkingHours = @IN_WorkingHours");
            cmdText.AppendLine(" ,LateHours = @IN_LateHours");
            cmdText.AppendLine(" ,EarlyHours = @IN_EarlyHours");
            cmdText.AppendLine(" ,SH_Hours = @IN_SH_Hours");
            cmdText.AppendLine(" ,LH_Hours = @IN_LH_Hours");

            cmdText.AppendLine(" ,OverTimeHours1 = @IN_OverTimeHours1");
            cmdText.AppendLine(" ,OverTimeHours2 = @IN_OverTimeHours2");
            cmdText.AppendLine(" ,OverTimeHours3 = @IN_OverTimeHours3");
            cmdText.AppendLine(" ,OverTimeHours4 = @IN_OverTimeHours4");
            cmdText.AppendLine(" ,OverTimeHours5 = @IN_OverTimeHours5");
            cmdText.AppendLine(" ,SH_OverTimeHours1 = @IN_SH_OverTimeHours1");
            cmdText.AppendLine(" ,SH_OverTimeHours2 = @IN_SH_OverTimeHours2");
            cmdText.AppendLine(" ,SH_OverTimeHours3 = @IN_SH_OverTimeHours3");
            cmdText.AppendLine(" ,SH_OverTimeHours4 = @IN_SH_OverTimeHours4");
            cmdText.AppendLine(" ,SH_OverTimeHours5 = @IN_SH_OverTimeHours5");
            cmdText.AppendLine(" ,LH_OverTimeHours1 = @IN_LH_OverTimeHours1");
            cmdText.AppendLine(" ,LH_OverTimeHours2 = @IN_LH_OverTimeHours2");
            cmdText.AppendLine(" ,LH_OverTimeHours3 = @IN_LH_OverTimeHours3");
            cmdText.AppendLine(" ,LH_OverTimeHours4 = @IN_LH_OverTimeHours4");
            cmdText.AppendLine(" ,LH_OverTimeHours5 = @IN_LH_OverTimeHours5");
            cmdText.AppendLine(" ,TotalWorkingHours = @IN_TotalWorkingHours");
            cmdText.AppendLine(" ,TotalOverTimeHours = @IN_TotalOverTimeHours");
            cmdText.AppendLine(" ,VacationFlag = @IN_VacationFlag");
            cmdText.AppendLine(" ,VacationFullCD = @IN_VacationFullCD");
            cmdText.AppendLine(" ,VacationMorningCD = @IN_VacationMorningCD");
            cmdText.AppendLine(" ,VacationAfternoonCD = @IN_VacationAfternoonCD");
            cmdText.AppendLine(" ,Memo = @IN_Memo");
            cmdText.AppendLine(" ,StatusFlag = @IN_StatusFlag");
            //*********************** Approval *************************
            cmdText.AppendLine(" ,ApprovalStatus = @IN_ApprovalStatus");
            cmdText.AppendLine(" ,ApprovalUID = @IN_ApprovalUID");
            cmdText.AppendLine(" ,ApprovalDate = @IN_ApprovalDate");
            cmdText.AppendLine(" ,ApprovalNote = @IN_ApprovalNote");
            //*********************** Approval *************************
            cmdText.AppendLine(" ,RequestNote = @IN_RequestNote");
            //**********************************************************
            cmdText.AppendLine(" ,ExchangeFlag = @IN_ExchangeFlag");
            cmdText.AppendLine(" ,ExchangeDate = @IN_ExchangeDate");
            cmdText.AppendLine(" ,UpdateDate = GETDATE()");
            cmdText.AppendLine(" ,UpdateUID  = @IN_UpdateUID");

            //Param
            Hashtable paras = new Hashtable();
            ISecurity sec = Security.Instance;

            base.AddParam(paras, "IN_ID", tAttendance.ID);
            base.AddParam(paras, "IN_UID", tAttendance.UID);
            base.AddParam(paras, "IN_Date", tAttendance.Date, true);
            base.AddParam(paras, "IN_WSID", tAttendance.WSID);
            base.AddParam(paras, "IN_EntryTime", tAttendance.EntryTime);
            base.AddParam(paras, "IN_ExitTime", tAttendance.ExitTime);
            base.AddParam(paras, "IN_WorkingHours", tAttendance.WorkingHours);
            base.AddParam(paras, "IN_LateHours", tAttendance.LateHours);
            base.AddParam(paras, "IN_SH_Hours", tAttendance.SH_Hours);
            base.AddParam(paras, "IN_LH_Hours", tAttendance.LH_Hours);
            base.AddParam(paras, "IN_EarlyHours", tAttendance.EarlyHours);
            base.AddParam(paras, "IN_OverTimeHours1", tAttendance.OverTimeHours1);
            base.AddParam(paras, "IN_OverTimeHours2", tAttendance.OverTimeHours2);
            base.AddParam(paras, "IN_OverTimeHours3", tAttendance.OverTimeHours3);
            base.AddParam(paras, "IN_OverTimeHours4", tAttendance.OverTimeHours4);
            base.AddParam(paras, "IN_OverTimeHours5", tAttendance.OverTimeHours5);
            base.AddParam(paras, "IN_SH_OverTimeHours1", tAttendance.SH_OverTimeHours1);
            base.AddParam(paras, "IN_SH_OverTimeHours2", tAttendance.SH_OverTimeHours2);
            base.AddParam(paras, "IN_SH_OverTimeHours3", tAttendance.SH_OverTimeHours3);
            base.AddParam(paras, "IN_SH_OverTimeHours4", tAttendance.SH_OverTimeHours4);
            base.AddParam(paras, "IN_SH_OverTimeHours5", tAttendance.SH_OverTimeHours5);
            base.AddParam(paras, "IN_LH_OverTimeHours1", tAttendance.LH_OverTimeHours1);
            base.AddParam(paras, "IN_LH_OverTimeHours2", tAttendance.LH_OverTimeHours2);
            base.AddParam(paras, "IN_LH_OverTimeHours3", tAttendance.LH_OverTimeHours3);
            base.AddParam(paras, "IN_LH_OverTimeHours4", tAttendance.LH_OverTimeHours4);
            base.AddParam(paras, "IN_LH_OverTimeHours5", tAttendance.LH_OverTimeHours5);
            base.AddParam(paras, "IN_TotalWorkingHours", tAttendance.TotalWorkingHours);
            base.AddParam(paras, "IN_TotalOverTimeHours", tAttendance.TotalOverTimeHours);
            base.AddParam(paras, "IN_VacationFlag", tAttendance.VacationFlag);
            base.AddParam(paras, "IN_VacationFullCD", tAttendance.VacationFullCD);
            base.AddParam(paras, "IN_VacationMorningCD", tAttendance.VacationMorningCD);
            base.AddParam(paras, "IN_VacationAfternoonCD", tAttendance.VacationAfternoonCD);
            base.AddParam(paras, "IN_Memo", tAttendance.Memo);
            base.AddParam(paras, "IN_StatusFlag", tAttendance.StatusFlag);

            //************************ Approval **********************************
            base.AddParam(paras, "IN_ApprovalStatus", tAttendance.ApprovalStatus);
            base.AddParam(paras, "IN_ApprovalUID", tAttendance.ApprovalUID);
            base.AddParam(paras, "IN_ApprovalDate", tAttendance.ApprovalDate);
            base.AddParam(paras, "IN_ApprovalNote", tAttendance.ApprovalNote);
            //************************ Approval **********************************
            base.AddParam(paras, "IN_RequestNote", tAttendance.RequestNote);
            //********************************************************************
            base.AddParam(paras, "IN_ExchangeFlag", tAttendance.ExchangeFlag);
            base.AddParam(paras, "IN_ExchangeDate", tAttendance.ExchangeDate);
            
            base.AddParam(paras, "IN_UpdateDate", tAttendance.UpdateDate, true);
            base.AddParam(paras, "IN_UpdateUID", tAttendance.UpdateUID);

            cmdWhere.AppendLine(" ID = @IN_ID AND UpdateDate=@IN_UpdateDate");
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="T_Attendance"></param>
        /// <returns></returns>
        public int UpdateApproval(T_Attendance tAttendance)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" UPDATE");
            cmdText.AppendLine(" dbo.T_Attendance");
            cmdText.AppendLine(" SET");

            //*********************** Approval *************************
            cmdText.AppendLine(" ApprovalStatus = @IN_ApprovalStatus");
            cmdText.AppendLine(" ,ApprovalUID = @IN_ApprovalUID");
            cmdText.AppendLine(" ,ApprovalDate = GETDATE()");
            cmdText.AppendLine(" ,ApprovalNote = @IN_ApprovalNote");
            //*********************** Request *************************
            cmdText.AppendLine(" ,RequestNote = @IN_RequestNote");
            //**********************************************************
            cmdText.AppendLine(" ,UpdateDate = GETDATE()");
            cmdText.AppendLine(" ,UpdateUID  = @IN_UpdateUID");

            //Param
            Hashtable paras = new Hashtable();
            ISecurity sec = Security.Instance;
            base.AddParam(paras, "IN_UID", tAttendance.UID);
            base.AddParam(paras, "IN_ID", tAttendance.ID);
            //************************ Approval **********************************
            base.AddParam(paras, "IN_ApprovalStatus", tAttendance.ApprovalStatus);
            base.AddParam(paras, "IN_ApprovalUID", tAttendance.ApprovalUID);
            base.AddParam(paras, "IN_ApprovalNote", tAttendance.ApprovalNote);
            //************************ Approval **********************************
            base.AddParam(paras, "IN_RequestNote", tAttendance.RequestNote);
            //********************************************************************
            base.AddParam(paras, "IN_UpdateDate", tAttendance.UpdateDate, true);
            base.AddParam(paras, "IN_UpdateUID", tAttendance.UpdateUID);


            cmdWhere.AppendLine(" ID = @IN_ID AND UpdateDate=@IN_UpdateDate");
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete data
        /// </summary>
        /// <param name="ID">id</param>
        /// <param name="updateDate">updateDate</param>
        /// <returns></returns>
        public int Delete(int ID, DateTime updateDate)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" DELETE	dbo.T_Attendance");
            cmdWhere.AppendLine(" ID=@IN_ID AND UpdateDate = @IN_UpdateDate");

            //Param
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_ID", ID);
            base.AddParam(paras, "IN_UpdateDate", updateDate, true);

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }
        #endregion

    }
}
