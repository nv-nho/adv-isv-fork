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
    /// Create Author: ISV-Giao
    public class AttendanceResultService : BaseService
    {
        #region Contructor
        /// <summary>
        /// Constructor
        /// </summary>
        private AttendanceResultService()
            : base()
        {
        }

        /// <summary>
        /// Constructor with param
        /// </summary>
        /// <param name="db">Database</param>
        public AttendanceResultService(DB db)
            : base(db)
        {
        }
        #endregion

        #region GetData

       /// <summary>
        /// Get Data T_AttendanceResult By uId and startDate
       /// </summary>
       /// <param name="uID"></param>
       /// <param name="startDate"></param>
       /// <returns></returns>
        public T_AttendanceResult GetDataAttendanceResultByIdAndStartDate(int uID, DateTime startDate)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  *");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.T_AttendanceResult");
            cmdWhere.AppendLine(" UID = @IN_UID");
            cmdWhere.AppendLine("AND StartDate = @IN_StartDate");

            //Para
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_UID", uID);
            base.AddParam(paras, "IN_StartDate", startDate, true);

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.Find1<T_AttendanceResult>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// checkApprovalBycalendarCD
        /// </summary>
        /// <param name="calendarCd"></param>
        /// <returns></returns>
        public int checkApprovalBycalendar(string calendarCd)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  count(A.ID)");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.T_AttendanceResult A");
            cmdText.AppendLine("inner join dbo.T_WorkingCalendar_H WH");
            cmdText.AppendLine("on A.CallendarID= WH.ID");
            cmdText.AppendLine("inner join dbo.T_WorkingCalendar_U WU");
            cmdText.AppendLine("on A.UID=WU.UID");
            cmdText.AppendLine("and WH.ID=WU.HID");
            cmdWhere.AppendLine(" WH.CalendarCD = @IN_CalendarCD");

            //Para
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_CalendarCD", calendarCd);

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }
            return int.Parse(this.db.ExecuteScalar1(cmdText.ToString(), paras).ToString());
        }

        /// <summary>
        /// checkEditBycalendar
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="calendarCd"></param>
        /// <returns></returns>
        public int checkEditBycalendar(DateTime startDate, string calendarCd)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  count(A.ID)");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.T_AttendanceResult A");
            cmdText.AppendLine("inner join dbo.T_WorkingCalendar_H WH");
            cmdText.AppendLine("on A.CallendarID= WH.ID");
            cmdWhere.AppendLine(" WH.CalendarCD = @IN_CalendarCD");

            cmdWhere.AppendLine(" AND A.StartDate = @IN_StartDate");

            //Para
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_CalendarCD", calendarCd);
            base.AddParam(paras, "IN_StartDate", startDate, true);

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());

            }
            return int.Parse(this.db.ExecuteScalar1(cmdText.ToString(), paras).ToString());
        }

        #endregion

        #region Entity

        #endregion

        #region Get Total Row
        #endregion

        #region Insert

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="T_Attendance">T_AttendanceResult</param>
        /// <returns></returns>
        public int Insert(T_AttendanceResult attendanceResult)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine(" INSERT INTO");
            cmdText.AppendLine(" dbo.T_AttendanceResult");
            cmdText.AppendLine(" (");
            cmdText.AppendLine("  [UID]");
            cmdText.AppendLine(" ,[CallendarID]");
            cmdText.AppendLine(" ,[Month]");
            cmdText.AppendLine(" ,[StartDate]");
            cmdText.AppendLine(" ,[EndDate]");
            cmdText.AppendLine(" ,WorkingHours");
            cmdText.AppendLine(" ,LateHours");
            cmdText.AppendLine(" ,EarlyHours");
            cmdText.AppendLine(" ,SH_Hours");
            cmdText.AppendLine(" ,LH_Hours");
            cmdText.AppendLine(" ,WorkingDays");
            cmdText.AppendLine(" ,LateDays");
            cmdText.AppendLine(" ,EarlyDays");
            cmdText.AppendLine(" ,SH_Days");
            cmdText.AppendLine(" ,LH_Days");
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

            cmdText.AppendLine(" ,TotalOverTimeHours");
            cmdText.AppendLine(" ,TotalWorkingHours");

            cmdText.AppendLine(" ,CreateDate");
            cmdText.AppendLine(" ,CreateUID");
            cmdText.AppendLine(" ,UpdateDate");
            cmdText.AppendLine(" ,UpdateUID");
            cmdText.AppendLine(" )");
            cmdText.AppendLine(" VALUES");
            cmdText.AppendLine(" (");

            cmdText.AppendLine("  @IN_UID");
            cmdText.AppendLine(" ,@IN_CallendarID");
            cmdText.AppendLine(" ,@IN_Month");
            cmdText.AppendLine(" ,@IN_StartDate");
            cmdText.AppendLine(" ,@IN_EndDate");
            cmdText.AppendLine(" ,@IN_WorkingHours");
            cmdText.AppendLine(" ,@IN_LateHours");
            cmdText.AppendLine(" ,@IN_EarlyHours");
            cmdText.AppendLine(" ,@IN_SH_Hours");
            cmdText.AppendLine(" ,@IN_LH_Hours");
            cmdText.AppendLine(" ,@IN_WorkingDays");
            cmdText.AppendLine(" ,@IN_LateDays");
            cmdText.AppendLine(" ,@IN_EarlyDays");
            cmdText.AppendLine(" ,@IN_SH_Days");
            cmdText.AppendLine(" ,@IN_LH_Days");
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

            cmdText.AppendLine(" ,@IN_TotalOverTimeHours");
            cmdText.AppendLine(" ,@IN_TotalWorkingHours");

            cmdText.AppendLine(" ,GETDATE()");
            cmdText.AppendLine(" ,@IN_CreateUID");
            cmdText.AppendLine(" ,GETDATE()");
            cmdText.AppendLine(" ,@IN_UpdateUID");

            cmdText.AppendLine(" )");

            //Param
            Hashtable paras = new Hashtable();
            ISecurity sec = Security.Instance;

            base.AddParam(paras, "IN_UID", attendanceResult.UID);
            base.AddParam(paras, "IN_CallendarID", attendanceResult.CallendarID);
            base.AddParam(paras, "IN_Month", attendanceResult.Month);
            base.AddParam(paras, "IN_StartDate", attendanceResult.startDate, true);
            base.AddParam(paras, "IN_EndDate", attendanceResult.endDate, true);
            base.AddParam(paras, "IN_WorkingHours", attendanceResult.WorkingHours);
            base.AddParam(paras, "IN_LateHours", attendanceResult.LateHours);
            base.AddParam(paras, "IN_EarlyHours", attendanceResult.EarlyHours);
            base.AddParam(paras, "IN_SH_Hours", attendanceResult.SH_Hours);
            base.AddParam(paras, "IN_LH_Hours", attendanceResult.LH_Hours);
            base.AddParam(paras, "IN_WorkingDays", attendanceResult.WorkingDays);
            base.AddParam(paras, "IN_LateDays", attendanceResult.LateDays);
            base.AddParam(paras, "IN_EarlyDays", attendanceResult.EarlyDays);
            base.AddParam(paras, "IN_SH_Days", attendanceResult.SH_Days);
            base.AddParam(paras, "IN_LH_Days", attendanceResult.LH_Days);
            base.AddParam(paras, "IN_OverTimeHours1", attendanceResult.OverTimeHours1);
            base.AddParam(paras, "IN_OverTimeHours2", attendanceResult.OverTimeHours2);
            base.AddParam(paras, "IN_OverTimeHours3", attendanceResult.OverTimeHours3);
            base.AddParam(paras, "IN_OverTimeHours4", attendanceResult.OverTimeHours4);
            base.AddParam(paras, "IN_OverTimeHours5", attendanceResult.OverTimeHours5);
            base.AddParam(paras, "IN_SH_OverTimeHours1", attendanceResult.SH_OverTimeHours1);
            base.AddParam(paras, "IN_SH_OverTimeHours2", attendanceResult.SH_OverTimeHours2);
            base.AddParam(paras, "IN_SH_OverTimeHours3", attendanceResult.SH_OverTimeHours3);
            base.AddParam(paras, "IN_SH_OverTimeHours4", attendanceResult.SH_OverTimeHours4);
            base.AddParam(paras, "IN_SH_OverTimeHours5", attendanceResult.SH_OverTimeHours5);
            base.AddParam(paras, "IN_LH_OverTimeHours1", attendanceResult.LH_OverTimeHours1);
            base.AddParam(paras, "IN_LH_OverTimeHours2", attendanceResult.LH_OverTimeHours2);
            base.AddParam(paras, "IN_LH_OverTimeHours3", attendanceResult.LH_OverTimeHours3);
            base.AddParam(paras, "IN_LH_OverTimeHours4", attendanceResult.LH_OverTimeHours4);
            base.AddParam(paras, "IN_LH_OverTimeHours5", attendanceResult.LH_OverTimeHours5);
            base.AddParam(paras, "IN_TotalWorkingHours", attendanceResult.TotalWorkingHours);
            base.AddParam(paras, "IN_TotalOverTimeHours", attendanceResult.TotalOverTimeHours);

            base.AddParam(paras, "IN_CreateUID", attendanceResult.CreateUID);
            base.AddParam(paras, "IN_UpdateUID", attendanceResult.UpdateUID);

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }

        #endregion

        #region Update

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="T_Attendance"></param>
        /// <returns></returns>
        public int Update(T_AttendanceResult attendanceResult)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" UPDATE");
            cmdText.AppendLine(" dbo.T_Attendance");
            cmdText.AppendLine(" SET");
            cmdText.AppendLine("  UID = @IN_UID");
            cmdText.AppendLine(" ,CallendarID = @IN_CallendarID");
            cmdText.AppendLine(" ,Month = @IN_Month");
            cmdText.AppendLine(" ,StartDate = @IN_StartDate");
            cmdText.AppendLine(" ,EndDate = @IN_EndDate");
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

            cmdText.AppendLine(" ,UpdateDate = GETDATE()");
            cmdText.AppendLine(" ,UpdateUID  = @IN_UpdateUID");

            //Param
            Hashtable paras = new Hashtable();
            ISecurity sec = Security.Instance;

            base.AddParam(paras, "IN_ID", attendanceResult.ID);
            base.AddParam(paras, "IN_UID", attendanceResult.UID);
            base.AddParam(paras, "IN_IN_CallendarID", attendanceResult.CallendarID);
            base.AddParam(paras, "IN_Month", attendanceResult.Month);
            base.AddParam(paras, "IN_StartDate", attendanceResult.startDate, true);
            base.AddParam(paras, "IN_EndDate", attendanceResult.endDate, true);
            base.AddParam(paras, "IN_WorkingHours", attendanceResult.WorkingHours);
            base.AddParam(paras, "IN_LateHours", attendanceResult.LateHours);
            base.AddParam(paras, "IN_SH_Hours", attendanceResult.SH_Hours);
            base.AddParam(paras, "IN_LH_Hours", attendanceResult.LH_Hours);
            base.AddParam(paras, "IN_EarlyHours", attendanceResult.EarlyHours);
            base.AddParam(paras, "IN_OverTimeHours1", attendanceResult.OverTimeHours1);
            base.AddParam(paras, "IN_OverTimeHours2", attendanceResult.OverTimeHours2);
            base.AddParam(paras, "IN_OverTimeHours3", attendanceResult.OverTimeHours3);
            base.AddParam(paras, "IN_OverTimeHours4", attendanceResult.OverTimeHours4);
            base.AddParam(paras, "IN_OverTimeHours5", attendanceResult.OverTimeHours5);
            base.AddParam(paras, "IN_SH_OverTimeHours1", attendanceResult.SH_OverTimeHours1);
            base.AddParam(paras, "IN_SH_OverTimeHours2", attendanceResult.SH_OverTimeHours2);
            base.AddParam(paras, "IN_SH_OverTimeHours3", attendanceResult.SH_OverTimeHours3);
            base.AddParam(paras, "IN_SH_OverTimeHours4", attendanceResult.SH_OverTimeHours4);
            base.AddParam(paras, "IN_SH_OverTimeHours5", attendanceResult.SH_OverTimeHours5);
            base.AddParam(paras, "IN_LH_OverTimeHours1", attendanceResult.LH_OverTimeHours1);
            base.AddParam(paras, "IN_LH_OverTimeHours2", attendanceResult.LH_OverTimeHours2);
            base.AddParam(paras, "IN_LH_OverTimeHours3", attendanceResult.LH_OverTimeHours3);
            base.AddParam(paras, "IN_LH_OverTimeHours4", attendanceResult.LH_OverTimeHours4);
            base.AddParam(paras, "IN_LH_OverTimeHours5", attendanceResult.LH_OverTimeHours5);
            base.AddParam(paras, "IN_TotalWorkingHours", attendanceResult.TotalWorkingHours);
            base.AddParam(paras, "IN_TotalOverTimeHours", attendanceResult.TotalOverTimeHours);

            base.AddParam(paras, "IN_UpdateDate", attendanceResult.UpdateDate);
            base.AddParam(paras, "IN_UpdateUID", attendanceResult.UpdateUID);

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
        public int Delete(int ID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" DELETE	dbo.T_AttendanceResult");
            cmdWhere.AppendLine(" ID=@IN_ID");

            //Param
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_ID", ID);

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
