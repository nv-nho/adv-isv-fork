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
    /// Class Work_HService DAC
    /// </summary>
    public class Work_HService : BaseService
    {
        #region Constructor
        /// <summary>
        /// Contructor of T_Work_H service
        /// </summary>        
        private Work_HService()
            : base()
        {
        }

        /// <summary>
        /// Contructor of T_Work_H service
        /// </summary>
        /// <param name="db">Class DB</param>
        public Work_HService(DB db)
            : base(db)
        {
        }
        #endregion

        #region Get Data
        /// <summary>
        /// Get by HID
        /// </summary>
        /// <returns>T_Work_H</returns>
        public T_Work_H GetByHID(int HID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  1 AS RowNumber");
            cmdText.AppendLine(" ,T1.*");
            cmdText.AppendLine(" FROM dbo.T_Work_H AS T1");
            cmdWhere.AppendLine(" HID = @IN_HID");

            //Parameter
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_HID", HID);
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.Find1<T_Work_H>(cmdText.ToString(), paras);
        }

        #endregion

        #region Insert
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="T_Work_H">T_Work_H</param>
        /// <returns></returns>
        public int Insert( T_Work_H t_Work_H)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine(" INSERT INTO");
            cmdText.AppendLine(" dbo.T_Work_H");
            cmdText.AppendLine(" (");
            cmdText.AppendLine("  HID");
            cmdText.AppendLine(" ,UID");
            cmdText.AppendLine(" ,Date");
            cmdText.AppendLine(" ,TotalWorkingHours");
            cmdText.AppendLine(" ,CreateDate");
            cmdText.AppendLine(" ,CreateUID");
            cmdText.AppendLine(" ,UpdateDate");
            cmdText.AppendLine(" ,UpdateUID");
            cmdText.AppendLine(" )");
            cmdText.AppendLine(" VALUES");
            cmdText.AppendLine(" (");
            cmdText.AppendLine("  @IN_HID");
            cmdText.AppendLine(" ,@IN_UID");
            cmdText.AppendLine(" ,@IN_Date");
            cmdText.AppendLine(" ,@IN_TotalWorkingHours");
            cmdText.AppendLine(" ,GETDATE()");
            cmdText.AppendLine(" ,@IN_CreateUID");
            cmdText.AppendLine(" ,GETDATE()");
            cmdText.AppendLine(" ,@IN_UpdateUID");
            cmdText.AppendLine(" )");

            //Para
            Hashtable paras = new Hashtable();
            ISecurity sec = Security.Instance;

            base.AddParam(paras, "IN_HID", t_Work_H.HID);
            base.AddParam(paras, "IN_UID", t_Work_H.UID);
            base.AddParam(paras, "IN_Date", t_Work_H.Date, true);
            base.AddParam(paras, "IN_TotalWorkingHours", t_Work_H.TotalWorkingHours);
            base.AddParam(paras, "IN_CreateUID", t_Work_H.CreateUID);
            base.AddParam(paras, "IN_UpdateUID", t_Work_H.UpdateUID);
            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }
        #endregion

        #region Update
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="T_Work_H">T_Work_H</param>
        /// <returns></returns>
        public int Update(T_Work_H t_Work_H)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" UPDATE");
            cmdText.AppendLine(" dbo.T_Work_H");
            cmdText.AppendLine(" SET");
            cmdText.AppendLine("  UID     = @IN_UID");
            cmdText.AppendLine(" ,Date    = @IN_Date");
            cmdText.AppendLine(" ,TotalWorkingHours  = @IN_TotalWorkingHours");
            cmdText.AppendLine(" ,UpdateDate = GETDATE()");
            cmdText.AppendLine(" ,UpdateUID  = @IN_UpdateUID");

            //Para
            Hashtable paras = new Hashtable();
            ISecurity sec = Security.Instance;

            cmdWhere.AppendLine(" HID = @IN_ID");
            base.AddParam(paras, "IN_ID", t_Work_H.HID);
            base.AddParam(paras, "IN_UID", t_Work_H.UID);
            base.AddParam(paras, "IN_Date", t_Work_H.Date, true);
            base.AddParam(paras, "IN_TotalWorkingHours", t_Work_H.TotalWorkingHours);
            base.AddParam(paras, "IN_UpdateUID", t_Work_H.UpdateUID);
           
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }

        #endregion

        
        #region Delete By Id
        /// <summary>
        /// Delete T_Work_H By Id 
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        /// 
        public int Delete(int workID)
        {
            //SQL String 
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine("DELETE [dbo].[T_Work_H]");

            //Parameter
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine("HID = @IN_ID");

            base.AddParam(paras, "IN_ID", workID);
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
