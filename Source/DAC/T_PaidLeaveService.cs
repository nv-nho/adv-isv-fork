using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using OMS.Utilities;
using OMS.Models;

namespace OMS.DAC
{
    /// <summary>
    /// Class T_PaidLeave Service
    /// </summary>
    public class T_PaidLeaveService : BaseService
    {
        #region Constructor
        /// <summary>
        /// Contructor of T_PaidLeave service
        /// </summary>        
        private T_PaidLeaveService()
            : base()
        {
        }

        /// <summary>
        /// Contructor of T_PaidLeave service
        /// </summary>
        /// <param name="db">Class DB</param>
        public T_PaidLeaveService(DB db)
            : base(db)
        {
        }
        #endregion

        #region Get Data
        /// <summary>
        /// Get by id
        /// </summary>
        /// <returns>T_PaidLeave</returns>
        public T_PaidLeave GetByID(int id)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" ,T1.*");
            cmdText.AppendLine(" FROM dbo.T_PaidLeave AS T1");
            cmdText.AppendLine(" WHERE ID = @IN_ID");

            //Parameter
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_ID", id);

            return this.db.Find1<T_PaidLeave>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Get by key
        /// </summary>
        /// <returns>list T_PaidLeave</returns>
        public bool IsExistsByKey(int calendarId, int userId, DateTime date)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" Count(*) as data_count");
            cmdText.AppendLine(" FROM dbo.T_PaidLeave AS T1");
            cmdText.AppendLine(" WHERE CalendarID = @IN_CalendarID");
            cmdText.AppendLine(" AND UserID = @IN_UserID");
            cmdText.AppendLine(" AND [Date] = @IN_Date");

            //Parameter
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_CalendarID", calendarId);
            base.AddParam(paras, "IN_UserID", userId);
            base.AddParam(paras, "IN_Date", date, true);

            return (int)this.db.ExecuteScalar1(cmdText.ToString(), paras) > 0;
        }

        /// <summary>
        /// Get by key
        /// </summary>
        /// <returns>list T_PaidLeave</returns>
        public IList<T_PaidLeave> GetListInMonths(int calendarId, int userId, DateTime fromDate, DateTime toDate)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" T1.*");
            cmdText.AppendLine(" FROM dbo.T_PaidLeave AS T1");
            cmdText.AppendLine(" WHERE CalendarID = @IN_CalendarID");
            cmdText.AppendLine(" AND UserID = @IN_UserID");
            cmdText.AppendLine(" AND [Date] BETWEEN @IN_FromDate AND @IN_ToDate");

            //Parameter
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_CalendarID", calendarId);
            base.AddParam(paras, "IN_UserID", userId);
            base.AddParam(paras, "IN_FromDate", fromDate, true);
            base.AddParam(paras, "IN_ToDate", toDate, true);

            return this.db.FindList1<T_PaidLeave>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Get by key
        /// </summary>
        /// <returns>list T_PaidLeave</returns>
        public Dictionary<DateTime, T_PaidLeave> GetDicInMonths(int calendarId, int userId, DateTime fromDate, DateTime toDate)
        {
            var lst = this.GetListInMonths(calendarId, userId, fromDate, toDate);

            Dictionary<DateTime, T_PaidLeave> ret = new Dictionary<DateTime, T_PaidLeave>();

            foreach (var item in lst)
            {
                ret.Add(item.Date, item);
            }
            return ret;
        }

        /// <summary>
        /// Get by key
        /// </summary>
        /// <returns>list T_PaidLeave</returns>
        public Dictionary<DateTime, T_PaidLeave>  GetDicByKey(int calendarId, int userId)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" T1.*");
            cmdText.AppendLine(" FROM dbo.T_PaidLeave AS T1");
            cmdText.AppendLine(" WHERE CalendarID = @IN_CalendarID");
            cmdText.AppendLine(" AND UserID = @IN_UserID");

            //Parameter
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_CalendarID", calendarId);
            base.AddParam(paras, "IN_UserID", userId);

            var lst = this.db.FindList1<T_PaidLeave>(cmdText.ToString(), paras);

            Dictionary<DateTime, T_PaidLeave> ret = new Dictionary<DateTime, T_PaidLeave>();

            foreach (var item in lst)
            {
                ret.Add(item.Date, item);
            }
            return ret;
        }

        #endregion

        #region Insert
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="data">T_PaidLeave</param>
        /// <returns></returns>
        public int Insert(T_PaidLeave data)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine(" INSERT INTO");
            cmdText.AppendLine(" dbo.T_PaidLeave");
            cmdText.AppendLine(" (");
            cmdText.AppendLine(" CalendarID");
            cmdText.AppendLine(" ,UserID");
            cmdText.AppendLine(" ,Date");
            cmdText.AppendLine(" ,CreateDate");
            cmdText.AppendLine(" ,CreateUID");
            cmdText.AppendLine(" ,UpdateDate");
            cmdText.AppendLine(" ,UpdateUID");
            cmdText.AppendLine(" )");
            cmdText.AppendLine(" VALUES");
            cmdText.AppendLine(" (");
            cmdText.AppendLine("  @IN_CalendarID");
            cmdText.AppendLine(" ,@IN_UserID");
            cmdText.AppendLine(" ,@IN_Date");
            cmdText.AppendLine(" ,GETDATE()");
            cmdText.AppendLine(" ,@IN_CreateUID");
            cmdText.AppendLine(" ,GETDATE()");
            cmdText.AppendLine(" ,@IN_UpdateUID");
            cmdText.AppendLine(" )");

            //Para
            Hashtable paras = new Hashtable();

            base.AddParam(paras, "IN_CalendarID", data.CalendarID);
            base.AddParam(paras, "IN_UserID", data.UserID);
            base.AddParam(paras, "IN_Date", data.Date, true);
            base.AddParam(paras, "IN_CreateUID", data.CreateUID);
            base.AddParam(paras, "IN_UpdateUID", data.UpdateUID);
            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete T_PaidLeave By Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        public int Delete(int id)
        {
            //SQL String 
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine("DELETE [dbo].[T_PaidLeave]");
            cmdText.AppendLine("WHERE ID = @IN_ID");

            //Parameter
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_ID", id);

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Delete T_PaidLeave By calendar Id 
        /// </summary>
        /// <param name="calendarId"></param>
        /// <returns></returns>
        /// 
        public int DeleteByCalendar(int calendarId)
        {
            //SQL String 
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine("DELETE [dbo].[T_PaidLeave]");
            cmdText.AppendLine("WHERE CalendarID = @IN_CalendarID");

            //Parameter
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_CalendarID", calendarId);

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Delete T_PaidLeave By calendar Id 
        /// </summary>
        /// <param name="calendarId"></param>
        /// <returns></returns>
        /// 
        public int DeleteInvalidData(int calendarId)
        {
            //SQL String 
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine("DELETE FROM T_PaidLeave");
            cmdText.AppendLine("WHERE CalendarID = @IN_CalendarID");
            cmdText.AppendLine("  AND (");
	        cmdText.AppendLine("    NOT EXISTS (");
			cmdText.AppendLine("            SELECT 1");
			cmdText.AppendLine("            FROM T_WorkingCalendar_U U");
			cmdText.AppendLine("            WHERE CalendarID = U.HID");
			cmdText.AppendLine("              AND UserID = U.UID)");
	        cmdText.AppendLine("    OR NOT EXISTS (");
			cmdText.AppendLine("            SELECT 1");
			cmdText.AppendLine("            FROM T_WorkingCalendar_D D");
			cmdText.AppendLine("            LEFT JOIN M_WorkingSystem S ON S.ID = D.WorkingSystemID");
			cmdText.AppendLine("            WHERE D.HID = CalendarID");
			cmdText.AppendLine("              AND D.WorkingDate = Date");
			cmdText.AppendLine("              AND S.WorkingType = 0");
            cmdText.AppendLine("              AND S.WorkingSystemCD <> '4'))");

            //Parameter
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_CalendarID", calendarId);

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Delete T_PaidLeave By Key
        /// </summary>
        /// <param name="calendarId"></param>
        /// <returns></returns>
        /// 
        public int DeleteByKey(int calendarId, int userId, DateTime date)
        {
            //SQL String 
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine("DELETE [dbo].[T_PaidLeave]");
            cmdText.AppendLine("WHERE CalendarID = @IN_CalendarID");
            cmdText.AppendLine("AND UserID = @IN_UserID");
            cmdText.AppendLine("AND [Date] = @IN_Date");

            //Parameter
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_CalendarID", calendarId);
            base.AddParam(paras, "IN_UserID", userId);
            base.AddParam(paras, "IN_Date", date);

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }
        #endregion
    }
}
