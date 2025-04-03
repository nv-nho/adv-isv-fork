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
    /// Class VacationResultService DAC
    /// Create Date: 2017/010/19
    /// Create Author: ISV-Giao
    public class VacationResultService : BaseService
    {
        #region Contructor
        /// <summary>
        /// Constructor
        /// </summary>
        private VacationResultService()
            : base()
        {
        }

        /// <summary>
        /// Constructor with param
        /// </summary>
        /// <param name="db">Database</param>
        public VacationResultService(DB db)
            : base(db)
        {
        }
        #endregion

        #region GetData

       /// <summary>
        /// Get Data T_VacationResult By Id
       /// </summary>
       /// <param name="hID"></param>
       /// <returns></returns>
        public IList<T_VacationResult> GetListVacationResultByHid(int hID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  *");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.T_VacationResult");
            cmdWhere.AppendLine(" HID = @IN_HID");
            //Para
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_HID", hID);

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.FindList1<T_VacationResult>(cmdText.ToString(), paras);
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
        /// <param name="T_Attendance">T_VacationResult</param>
        /// <returns></returns>
        public int Insert(int Hid, VacationDateInFoByAttendanceApproval cofig)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine(" INSERT INTO");
            cmdText.AppendLine(" dbo.T_VacationResult");
            cmdText.AppendLine(" (");
            cmdText.AppendLine("  [HID]");
            cmdText.AppendLine(" ,[VacationID]");
            cmdText.AppendLine(" ,[VacationName]");
            cmdText.AppendLine(" ,[VacationDate]");
            cmdText.AppendLine(" )");
            cmdText.AppendLine(" VALUES");
            cmdText.AppendLine(" (");

            cmdText.AppendLine("  @IN_HID");
            cmdText.AppendLine(" ,@IN_VacationID");
            cmdText.AppendLine(" ,@IN_VacationName");
            cmdText.AppendLine(" ,@IN_VacationDate");

            cmdText.AppendLine(" )");

            //Param
            Hashtable paras = new Hashtable();
            ISecurity sec = Security.Instance;

            base.AddParam(paras, "IN_HID", Hid);
            base.AddParam(paras, "IN_VacationID", cofig.Value1);
            base.AddParam(paras, "IN_VacationName", cofig.Value3);
            base.AddParam(paras, "IN_VacationDate", cofig.VacationDate);

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }

        #endregion

        #region Update

        #endregion

        #region Delete
        /// <summary>
        /// Delete data
        /// </summary>
        /// <param name="ID">id</param>
        /// <param name="updateDate">updateDate</param>
        /// <returns></returns>
        public int Delete(int HID,int vacationId)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" DELETE	dbo.T_VacationResult");
            cmdWhere.AppendLine(" HID=@IN_ID AND VacationID = @IN_VacationID");

            //Param
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_ID", HID);
            base.AddParam(paras, "IN_VacationID", vacationId);

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
