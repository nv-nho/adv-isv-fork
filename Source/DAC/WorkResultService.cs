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
    public class WorkResultService : BaseService
    {
        #region Contructor
        /// <summary>
        /// Constructor
        /// </summary>
        private WorkResultService()
            : base()
        {
        }

        /// <summary>
        /// Constructor with param
        /// </summary>
        /// <param name="db">Database</param>
        public WorkResultService(DB db)
            : base(db)
        {
        }
        #endregion

        #region GetData
        /// <summary>
        /// Get Data T_WorkResult By Id
        /// </summary>
        /// <param name="hID"></param>
        /// <returns></returns>
        public IList<T_WorkResult> GetListWorkResultByHid(int hID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  *");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.T_WorkResult");
            cmdWhere.AppendLine(" HID = @IN_HID");

            //Para
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_HID", hID);

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.FindList1<T_WorkResult>(cmdText.ToString(), paras);
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
        /// <param name="T_WorkResult">T_WorkResult</param>
        /// <returns></returns>
        public int Insert(int Hid, T_WorkResult workResult)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine(" INSERT INTO");
            cmdText.AppendLine(" dbo.T_WorkResult");
            cmdText.AppendLine(" (");
            cmdText.AppendLine("  [HID]");
            cmdText.AppendLine(" ,[ProjectID]");
            cmdText.AppendLine(" ,[ProjectCD]");
            cmdText.AppendLine(" ,[ProjectName]");
            cmdText.AppendLine(" ,[WorkPlace]");
            cmdText.AppendLine(" ,[WorkTime]");
            cmdText.AppendLine(" )");
            cmdText.AppendLine(" VALUES");
            cmdText.AppendLine(" (");

            cmdText.AppendLine("  @IN_HID");
            cmdText.AppendLine(" ,@IN_ProjectID");
            cmdText.AppendLine(" ,@IN_ProjectCD");
            cmdText.AppendLine(" ,@IN_ProjectName");
            cmdText.AppendLine(" ,@IN_WorkPlace");
            cmdText.AppendLine(" ,@IN_WorkTime");

            cmdText.AppendLine(" )");

            //Param
            Hashtable paras = new Hashtable();
            ISecurity sec = Security.Instance;

            base.AddParam(paras, "IN_HID", Hid);
            base.AddParam(paras, "IN_ProjectID", workResult.ProjectID);
            base.AddParam(paras, "IN_ProjectCD", workResult.ProjectCD);
            base.AddParam(paras, "IN_ProjectName", workResult.ProjectName);
            base.AddParam(paras, "IN_WorkPlace", workResult.WorkPlace);
            base.AddParam(paras, "IN_WorkTime", workResult.WorkTime);

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
        public int Delete(int HID, int projectId)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" DELETE	dbo.T_WorkResult");
            cmdWhere.AppendLine(" HID=@IN_HID");
            cmdWhere.AppendLine("AND  ProjectID=@IN_PROJECTID");

            //Param
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_HID", HID);
            base.AddParam(paras, "IN_PROJECTID", projectId);

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
