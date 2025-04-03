using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OMS.Models;
using OMS.Utilities;


namespace OMS.DAC
{
    /// <summary>
    /// Class T_PaidVacationService DAC
    /// </summary>
    public class T_PaidVacationService : BaseService
    {
        #region Contructor
        /// <summary>
        /// Contructor of T_PaidVacation service
        /// </summary>        
        private T_PaidVacationService()
            : base()
        {
        }

        /// <summary>
        /// Contructor of T_PaidVacation service
        /// </summary>
        /// <param name="db">Class DB</param>
        public T_PaidVacationService(DB db)
            : base(db)
        {
        }

        #endregion

        #region Get Data

        /// <summary>
        /// Get latest data
        /// </summary>
        /// <param name="uid">user id</param>
        /// <returns></returns>
        public T_PaidVacation GetLatestData(int uid)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine("SELECT TOP(1) *");
            cmdText.AppendLine("FROM T_PaidVacation");
            cmdText.AppendLine("WHERE UID = @IN_UID");
            cmdText.AppendLine("ORDER BY UpdateDate DESC");

            //parameter
            Hashtable prms = new Hashtable();
            base.AddParam(prms, "IN_UID", uid);

            return this.db.Find1<T_PaidVacation>(cmdText.ToString(), prms);
        }

        /// <summary>
        /// Get by UID
        /// </summary>
        /// <returns>T_PaidVacation</returns>
        public IList<T_PaidVacation> GetByUID(int uid)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" T1.*");
            cmdText.AppendLine(" FROM dbo.T_PaidVacation AS T1");
            cmdWhere.AppendLine(" UID = @IN_UID");

            //Parameter
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_UID", uid);
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.FindList1<T_PaidVacation>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Get list valid vacation data of user
        /// </summary>
        /// <param name="uid">user id</param>
        /// <returns></returns>
        public IList<T_PaidVacation> GetListValid(int uid)
        {
            //Create SQL query string
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine("SELECT *");
            cmdText.AppendLine("FROM dbo.T_PaidVacation");
            cmdText.AppendLine("WHERE UID = @IN_UID");
            cmdText.AppendLine("AND InvalidFlag = 0");
            cmdText.AppendLine("ORDER BY Year");
            
            //Add parameters
            Hashtable prms = new Hashtable();
            base.AddParam(prms, "IN_UID", uid);

            return this.db.FindList1<T_PaidVacation>(cmdText.ToString(), prms);
        }

        /// <summary>
        /// Get total vacation day of user
        /// </summary>
        /// <param name="uid">user id</param>
        /// <returns></returns>
        public decimal GetTotalVacationDays(int uid)
        {
            //Create SQL query string
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine("SELECT SUM(VacationDay) AS TotalVacationDays");
            cmdText.AppendLine("FROM dbo.T_PaidVacation");
            cmdText.AppendLine("WHERE UID = @IN_UID");
            cmdText.AppendLine("AND InvalidFlag = 0");

            //Add parameter
            Hashtable prms = new Hashtable();
            base.AddParam(prms, "IN_UID", uid);

            return decimal.Parse(string.Format("0{0}", db.ExecuteScalar1(cmdText.ToString(), prms)));
        }

        #endregion

        #region Insert

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="paidvacation">T_PaidVacation</param>
        /// <returns></returns>
        public int Insert(T_PaidVacation paidvacation)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine(" INSERT INTO");
            cmdText.AppendLine(" dbo.T_PaidVacation");
            cmdText.AppendLine(" (");
            cmdText.AppendLine(" UID");
            cmdText.AppendLine(" ,Year");
            cmdText.AppendLine(" ,VacationDay");
            cmdText.AppendLine(" ,InvalidFlag");
            cmdText.AppendLine(" ,CreateDate");
            cmdText.AppendLine(" ,CreateUID");
            cmdText.AppendLine(" ,UpdateDate");
            cmdText.AppendLine(" ,UpdateUID");
            cmdText.AppendLine(" )");
            cmdText.AppendLine(" VALUES");
            cmdText.AppendLine(" (");
            cmdText.AppendLine(" @IN_UID");
            cmdText.AppendLine(" ,@IN_Year");
            cmdText.AppendLine(" ,@IN_VacationDay");
            cmdText.AppendLine(" ,@IN_InvalidFlag");

            cmdText.AppendLine(" ,GETDATE()");
            cmdText.AppendLine(" ,@IN_CreateUID");
            cmdText.AppendLine(" ,GETDATE()");
            cmdText.AppendLine(" ,@IN_UpdateUID");
            cmdText.AppendLine(" )");

            //Para
            Hashtable paras = new Hashtable();

            base.AddParam(paras, "IN_UID", paidvacation.UID);
            base.AddParam(paras, "IN_Year", paidvacation.Year);
            base.AddParam(paras, "IN_VacationDay", paidvacation.VacationDay);
            base.AddParam(paras, "IN_InvalidFlag", paidvacation.InvalidFlag);
            base.AddParam(paras, "IN_CreateUID", paidvacation.CreateUID);
            base.AddParam(paras, "IN_UpdateUID", paidvacation.UpdateUID);
            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Insert For Old Data
        /// </summary>
        /// <param name="paidvacation">T_PaidVacation</param>
        /// <returns></returns>
        public int InsertForOldData(T_PaidVacation paidvacation)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine(" INSERT INTO");
            cmdText.AppendLine(" dbo.T_PaidVacation");
            cmdText.AppendLine(" (");
            cmdText.AppendLine(" UID");
            cmdText.AppendLine(" ,Year");
            cmdText.AppendLine(" ,VacationDay");
            cmdText.AppendLine(" ,InvalidFlag");
            cmdText.AppendLine(" ,CreateDate");
            cmdText.AppendLine(" ,CreateUID");
            cmdText.AppendLine(" ,UpdateDate");
            cmdText.AppendLine(" ,UpdateUID");
            cmdText.AppendLine(" )");
            cmdText.AppendLine(" VALUES");
            cmdText.AppendLine(" (");
            cmdText.AppendLine(" @IN_UID");
            cmdText.AppendLine(" ,@IN_Year");
            cmdText.AppendLine(" ,@IN_VacationDay");
            cmdText.AppendLine(" ,@IN_InvalidFlag");

            cmdText.AppendLine(" ,@IN_CreateDate");
            cmdText.AppendLine(" ,@IN_CreateUID");
            cmdText.AppendLine(" ,GETDATE()");
            cmdText.AppendLine(" ,@IN_UpdateUID");
            cmdText.AppendLine(" )");

            //Para
            Hashtable paras = new Hashtable();

            base.AddParam(paras, "IN_UID", paidvacation.UID);
            base.AddParam(paras, "IN_Year", paidvacation.Year);
            base.AddParam(paras, "IN_VacationDay", paidvacation.VacationDay);
            base.AddParam(paras, "IN_InvalidFlag", paidvacation.InvalidFlag);
            base.AddParam(paras, "IN_CreateUID", paidvacation.CreateUID);
            base.AddParam(paras, "IN_CreateDate", paidvacation.CreateDate);
            base.AddParam(paras, "IN_UpdateUID", paidvacation.UpdateUID);
            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }
        #endregion

        #region Update
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="paidvacation">T_PaidVacation</param>
        /// <returns></returns>
        public int UpdateVacationDays(T_PaidVacation paidvacation)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" UPDATE");
            cmdText.AppendLine(" dbo.T_PaidVacation");
            cmdText.AppendLine(" SET");
            cmdText.AppendLine(" VacationDay    = VacationDay - @IN_UseVacationDays");
            cmdText.AppendLine(" ,UpdateDate = GETDATE()");
            cmdText.AppendLine(" ,UpdateUID  = @IN_UpdateUID");

            //Para
            Hashtable paras = new Hashtable();

            cmdWhere.AppendLine(" UID = @IN_UID");
            cmdWhere.AppendLine("AND Year = @IN_Year");
            cmdWhere.AppendLine("AND UpdateDate = @IN_UpdateDate");
            cmdWhere.AppendLine("AND VacationDay - @IN_UseVacationDays >= 0");

            base.AddParam(paras, "IN_UID", paidvacation.UID);
            base.AddParam(paras, "IN_Year", paidvacation.Year);
            base.AddParam(paras, "IN_UseVacationDays", paidvacation.VacationDay);

            base.AddParam(paras, "IN_UpdateDate", paidvacation.UpdateDate);
            base.AddParam(paras, "IN_UpdateUID", paidvacation.UpdateUID);

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
        /// Delete by UID
        /// </summary>
        /// <param name="uid">User ID</param>
        /// <returns></returns>
        public int Delete(int uid)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" DELETE FROM dbo.T_PaidVacation");

            //Param
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine(" UID = @IN_UID");
            base.AddParam(paras, "IN_UID", uid);

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
