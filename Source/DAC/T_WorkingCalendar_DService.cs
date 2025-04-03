using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using OMS.Models;

namespace OMS.DAC
{
    public class T_WorkingCalendar_DService : BaseService
    {
        #region Contructor
        /// <summary>
        /// Contructor of T_WorkingCalendar_D service
        /// </summary>        
        private T_WorkingCalendar_DService()
            : base()
        {
        }

        /// <summary>
        /// Contructor of T_WorkingCalendar_D service
        /// </summary>
        /// <param name="db">Class DB</param>
        public T_WorkingCalendar_DService(DB db)
            : base(db)
        {
        }

        #endregion

        #region Get Data
        /// <summary>
        /// Get the list
        /// </summary>
        /// <param name="model">T_WorkingCalendar_D</param>
        /// <param name="pageIndex">pageIndex</param>
        /// <param name="pageSize">pageSize</param>
        /// <param name="sortField">sortField</param>
        /// <param name="sortDirec">sortDirec</param>
        /// <returns></returns>
        ///
        public IList<T_WorkingCalendar_D> GetListInMonth(int workingCalendarID,DateTime fromDate, DateTime toDate)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" T.*");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.T_WorkingCalendar_D AS T");
            cmdText.AppendLine(" WHERE ");
            cmdText.AppendLine(" T.HID = @IN_HID");
            cmdText.AppendLine(" AND T.WorkingDate BETWEEN @IN_WorkingCalendarFromDate AND @IN_WorkingCalendarToDate");
            
            //Parameter
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_HID", workingCalendarID);
            base.AddParam(paras, "IN_WorkingCalendarFromDate", fromDate, true);
            base.AddParam(paras, "IN_WorkingCalendarToDate", toDate, true);

            return this.db.FindList1<T_WorkingCalendar_D>(cmdText.ToString(), paras);
        }

        public T_WorkingCalendar_D GetWorkingCalendarByKey(int HID,DateTime workingDate)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();
            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" T.*");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.T_WorkingCalendar_D AS T");

            //Parameter
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine(" T.HID = @IN_HID");
            cmdWhere.AppendLine(" AND T.WorkingDate = @IN_WorkingDate");

            base.AddParam(paras, "IN_HID", HID);
            base.AddParam(paras, "IN_WorkingDate", workingDate, true);
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }
            return this.db.Find1<T_WorkingCalendar_D>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Get the list for searching
        /// </summary>
        /// <param name="HID">HID</param>
        /// <returns></returns>
        ///
        public IList<T_WorkingCalendar_D> GetListWorkingCalendarByHId(int HID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();
            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" T.*");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.T_WorkingCalendar_D AS T");

            //Parameter
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine(" T.HID = @IN_HID");

            base.AddParam(paras, "IN_HID", HID);
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }
            return this.db.FindList1<T_WorkingCalendar_D>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Get All
        /// </summary>
        /// <returns></returns>
        ///
        public IList<T_WorkingCalendar_D> GetAll()
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();
            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" T.*");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.T_WorkingCalendar_D AS T");

            //Parameter
            Hashtable paras = new Hashtable();

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }
            return this.db.FindList1<T_WorkingCalendar_D>(cmdText.ToString(), paras);
        }

        #endregion

        #region Insert
        /// <summary>
        /// Insert t_WorkingCalendar_D
        /// </summary>
        /// <param name="t_WorkingCalendar_D"></param>
        /// <returns></returns>
        /// 
        public int Insert(T_WorkingCalendar_D t_WorkingCalendar_D)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine("INSERT INTO [dbo].[T_WorkingCalendar_D]");
            cmdText.AppendLine("(");
            cmdText.AppendLine("HID");
            cmdText.AppendLine(",WorkingDate");
            cmdText.AppendLine(",WorkingSystemID");
            cmdText.AppendLine(",Ivent1");
            cmdText.AppendLine(")");
            cmdText.AppendLine("VALUES");
            cmdText.AppendLine("(");
            cmdText.AppendLine("@IN_HID");
            cmdText.AppendLine(",@IN_WorkingDate");
            cmdText.AppendLine(",@IN_WorkingSystemID");
            cmdText.AppendLine(",@IN_Ivent1");
            cmdText.AppendLine(")");
            //Add Param
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_HID", t_WorkingCalendar_D.HID);
            base.AddParam(paras, "IN_WorkingDate", t_WorkingCalendar_D.WorkingDate, true);
            base.AddParam(paras, "IN_WorkingSystemID", t_WorkingCalendar_D.WorkingSystemID);
            base.AddParam(paras, "IN_Ivent1", t_WorkingCalendar_D.Ivent1);
            
            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }
        #endregion

        #region Update
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="t_WorkingCalendar_D">t_WorkingCalendar_D</param>
        /// <returns></returns>
        public int Update(T_WorkingCalendar_D t_WorkingCalendar_D)
        {
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" UPDATE [dbo].[T_WorkingCalendar_D]");
            cmdText.AppendLine(" SET");
            cmdText.AppendLine(" WorkingSystemCD = @IN_WorkingSystemID");
            cmdText.AppendLine(" Ivent1 = @IN_Ivent1");

            //Param
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_HID", t_WorkingCalendar_D.HID);
            base.AddParam(paras, "IN_WorkingDate", t_WorkingCalendar_D.WorkingDate, true);
            base.AddParam(paras, "IN_WorkingSystemID", t_WorkingCalendar_D.WorkingSystemID);
            base.AddParam(paras, "IN_Ivent1", t_WorkingCalendar_D.Ivent1);

            cmdWhere.AppendLine(" HID = @IN_HID AND WorkingDate = @IN_WorkingDate");

            //Check SQL
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }
         #endregion

        #region DeleteAll
        /// <summary>
        /// Delete All t_WorkingCalendar_D
        /// </summary>
        /// <param name="workingDateID"></param>
        /// <returns></returns>
        /// 
        public int DeleteAll(int workingDateID)
        {
            //SQL String 
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine("DELETE [dbo].[T_WorkingCalendar_D]");

            //Parameter
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine("HID = @IN_HID");

            base.AddParam(paras, "IN_HID", workingDateID);
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.ExecuteNonQuery1(cmdText.ToString(),paras);
        }
        #endregion

    }
}
