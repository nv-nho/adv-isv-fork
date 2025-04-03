using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using OMS.Utilities;
using OMS.Models;

namespace OMS.DAC
{
    public class T_WorkingCalendar_UService : BaseService
    {
        #region Constructor
        /// <summary>
        /// Contructor of T_WorkingCalendar_U service
        /// </summary>        
        private T_WorkingCalendar_UService()
            : base()
        {
        }

        /// <summary>
        /// Contructor of T_WorkingCalendar_U service
        /// </summary>
        /// <param name="db">Class DB</param>
        public T_WorkingCalendar_UService(DB db)
            : base(db)
        {
        }
        #endregion

        #region Get Data
        /// <summary>
        /// Get List by HID
        /// </summary>
        /// <returns>T_WorkingCalendar_U</returns>
        public IList<T_WorkingCalendar_U> GetListByID(int HID, int UID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  1 AS RowNumber");
            cmdText.AppendLine(" ,T1.*");
            cmdText.AppendLine(" FROM dbo.T_WorkingCalendar_U AS T1");
            cmdWhere.AppendLine(" HID = @IN_HID");
            cmdWhere.AppendLine("AND UID = @IN_UID");
            //Parameter
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_HID", HID);
            base.AddParam(paras, "IN_HID", UID);
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.FindList1<T_WorkingCalendar_U>(cmdText.ToString(), paras);
        }

        #endregion

        #region GetByHId
        /// <summary>
        /// Get List by HID
        /// </summary>
        /// <returns>IList<T_WorkingCalendar_U></returns>
        public IList<T_WorkingCalendar_U> GetByHId(int HID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  1 AS RowNumber");
            cmdText.AppendLine(" ,T1.*");
            cmdText.AppendLine(" FROM dbo.T_WorkingCalendar_U AS T1");
            cmdWhere.AppendLine(" HID = @IN_HID");
            
            //Parameter
            Hashtable paras = new Hashtable();

            base.AddParam(paras, "IN_HID", HID);

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.FindList1<T_WorkingCalendar_U>(cmdText.ToString(), paras);
        }

        #endregion

        #region GetByUID
        /// <summary>
        /// Get List by UID
        /// </summary>
        /// <returns>IList<T_WorkingCalendar_U></returns>
        public IList<T_WorkingCalendar_U> GetByUId(int UID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  1 AS RowNumber");
            cmdText.AppendLine(" ,T1.*");
            cmdText.AppendLine(" FROM dbo.T_WorkingCalendar_U AS T1");
            cmdWhere.AppendLine(" UID = @IN_UID");

            //Parameter
            Hashtable paras = new Hashtable();

            base.AddParam(paras, "IN_UID", UID);

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.FindList1<T_WorkingCalendar_U>(cmdText.ToString(), paras);
        }

        #endregion

        #region GetAll
        /// <summary>
        /// GetAll
        /// </summary>
        /// <returns>T_WorkingCalendar_U</returns>
        public IList<T_WorkingCalendar_U> GetAll()
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  1 AS RowNumber");
            cmdText.AppendLine(" ,T1.*");
            cmdText.AppendLine(" FROM dbo.T_WorkingCalendar_U AS T1");

            //Parameter
            Hashtable paras = new Hashtable();
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.FindList1<T_WorkingCalendar_U>(cmdText.ToString(), paras);
        }

        #endregion

        #region Insert
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="T_WorkingCalendar_U">T_WorkingCalendar_U</param>
        /// <returns></returns>
        public int Insert(T_WorkingCalendar_U T_WorkingCalendar_U)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine(" INSERT INTO");
            cmdText.AppendLine(" dbo.T_WorkingCalendar_U");
            cmdText.AppendLine(" (");
            cmdText.AppendLine("  HID");
            cmdText.AppendLine(" ,UID");
            cmdText.AppendLine(" )");
            cmdText.AppendLine(" VALUES");
            cmdText.AppendLine(" (");
            cmdText.AppendLine("  @IN_HID");
            cmdText.AppendLine(" ,@IN_UID");
            cmdText.AppendLine(" )");

            //Param
            Hashtable paras = new Hashtable();
            ISecurity sec = Security.Instance;

            base.AddParam(paras, "IN_HID", T_WorkingCalendar_U.HID);
            base.AddParam(paras, "IN_UID", T_WorkingCalendar_U.UID);

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }
        #endregion

        #region Delete List By HId
        /// <summary>
        /// Delete List<T_WorkingCalendar_Us> By Id 
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        /// 
        public int DeleteAllByID(int HID, int UID)
        {
            //SQL String 
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine("DELETE [dbo].[T_WorkingCalendar_U]");

            //Parameter
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine("HID = @IN_HID");
            cmdWhere.AppendLine("AND UID = @IN_UID");
            base.AddParam(paras, "IN_HID", HID);
            base.AddParam(paras, "IN_HID", UID);
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }

        #endregion


        #region DeleteAll

        /// <summary>
        /// DeleteAll
        /// </summary>
        /// <returns></returns>
        public int DeleteAll(int HID)
        {
            //SQL String 
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine("DELETE [dbo].[T_WorkingCalendar_U]");

            //Parameter
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine("HID = @IN_HID");

            base.AddParam(paras, "IN_HID", HID);
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
