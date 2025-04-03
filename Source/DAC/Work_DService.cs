using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using OMS.Utilities;
using OMS.Models;

namespace OMS.DAC
{
    public class Work_DService : BaseService
    {
        #region Constructor
        /// <summary>
        /// Contructor of T_Work_D service
        /// </summary>        
        private Work_DService()
            : base()
        {
        }

        /// <summary>
        /// Contructor of T_Work_D service
        /// </summary>
        /// <param name="db">Class DB</param>
        public Work_DService(DB db)
            : base(db)
        {
        }
        #endregion

        #region Get Data
        /// <summary>
        /// Get List by HID
        /// </summary>
        /// <returns>T_Work_D</returns>
        public IList<T_Work_D> GetListByHID(int HID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  1 AS RowNumber");
            cmdText.AppendLine(" ,T1.*");
            cmdText.AppendLine(" FROM dbo.T_Work_D AS T1");
            cmdWhere.AppendLine(" HID = @IN_HID");

            //Parameter
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_HID", HID);
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.FindList1<T_Work_D>(cmdText.ToString(), paras);

        }

        /// <summary>
        /// Get List by HID
        /// </summary>
        /// <returns>T_Work_D</returns>
        public IList<T_WorkResult> getDataWorkResult(List<int> listId)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("-1 as HID, A.ProjectID,sum(A.WorkTime) AS WorkTime,min(A.ProjectCD) AS ProjectCD,min(A.ProjectName) AS ProjectName,min(A.WorkPlace) AS WorkPlace");
            cmdText.AppendLine(" FROM(");
            cmdText.AppendLine(" select WD.PID AS ProjectID,");
            cmdText.AppendLine(" WD.WorkTime as WorkTime,");
            cmdText.AppendLine(" P.ProjectCD ,P.ProjectName,P.WorkPlace");
            cmdText.AppendLine(" from dbo.T_Work_D WD");
            cmdText.AppendLine(" left join dbo.M_Project P");
            cmdText.AppendLine(" ON WD.PID =P.ID");

            cmdWhere.AppendLine("WD.HID in (");

            int i = 0;
            foreach (var item in listId)
            {
                if (i < listId.Count-1)
                {
                    cmdWhere.AppendFormat("{0},", item);
                }
                else
                {
                    cmdWhere.AppendFormat("{0}", item);
                }
               
                i++;
            }
            cmdWhere.AppendLine(")");

            //Parameter
            Hashtable paras = new Hashtable();
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }
            cmdText.AppendLine(")A");
            cmdText.AppendLine("group by A.ProjectID");

            return this.db.FindList1<T_WorkResult>(cmdText.ToString(), paras);

        }

        /// <summary>
        /// Get List by HID
        /// </summary>
        /// <returns>WorkDInfoEXcel</returns>
        public IList<WorkDInfoEXcel> GetListWorkDInfoEXcelByHID(int HID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  w.WorkTime as workTime");
            cmdText.AppendLine(" ,p.ID as projectId");
            cmdText.AppendLine(" ,p.ProjectName as projectNm");
            cmdText.AppendLine(" FROM dbo.T_Work_D AS w");
            cmdText.AppendLine(" inner join dbo.M_Project as p");
            cmdText.AppendLine(" on w.PID = p.ID");
            cmdWhere.AppendLine(" w.HID = @IN_HID");

            //Parameter
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_HID", HID);
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.FindList1<WorkDInfoEXcel>(cmdText.ToString(), paras);

        }

        #endregion

        #region Insert
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="T_Work_D">T_Work_D</param>
        /// <returns></returns>
        public int Insert(T_Work_D t_Work_D)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine(" INSERT INTO");
            cmdText.AppendLine(" dbo.T_Work_D");
            cmdText.AppendLine(" (");
            cmdText.AppendLine("  HID");
            cmdText.AppendLine(" ,Date");
            cmdText.AppendLine(" ,[LineNo]");
            cmdText.AppendLine(" ,PID");
            cmdText.AppendLine(" ,WorkPlace");
            cmdText.AppendLine(" ,StartTime");
            cmdText.AppendLine(" ,EndTime");
            cmdText.AppendLine(" ,WorkTime");
            cmdText.AppendLine(" ,Memo");
            cmdText.AppendLine(" )");
            cmdText.AppendLine(" VALUES");
            cmdText.AppendLine(" (");
            cmdText.AppendLine("  @IN_HID");
            cmdText.AppendLine(" ,@IN_Date");
            cmdText.AppendLine(" ,@IN_LineNo");
            cmdText.AppendLine(" ,@IN_PID");
            cmdText.AppendLine(" ,@IN_WorkPlace");
            cmdText.AppendLine(" ,@IN_StartTime");
            cmdText.AppendLine(" ,@IN_EndTime");
            cmdText.AppendLine(" ,@IN_WorkTime");
            cmdText.AppendLine(" ,@IN_Memo");
            cmdText.AppendLine(" )");

            //Param
            Hashtable paras = new Hashtable();
            ISecurity sec = Security.Instance;

            base.AddParam(paras, "IN_HID", t_Work_D.HID);
            base.AddParam(paras, "IN_Date", t_Work_D.Date, true);
            base.AddParam(paras, "IN_LineNo", t_Work_D.LineNo);
            base.AddParam(paras, "IN_PID", t_Work_D.PID);
            base.AddParam(paras, "IN_WorkPlace", t_Work_D.WorkPlace);
            base.AddParam(paras, "IN_StartTime", t_Work_D.StartTime);
            base.AddParam(paras, "IN_EndTime", t_Work_D.EndTime);
            base.AddParam(paras, "IN_WorkTime", t_Work_D.WorkTime);
            base.AddParam(paras, "IN_Memo", t_Work_D.Memo);

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }
        #endregion

        #region Update
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="T_Work_D">T_Work_D</param>
        /// <returns></returns>
        public int Update(T_Work_D t_Work_D)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" UPDATE");
            cmdText.AppendLine(" dbo.T_Work_D");
            cmdText.AppendLine(" SET");
            cmdText.AppendLine("  LineNo  = @IN_LineNo");
            cmdText.AppendLine(" ,PID    = @IN_PID");
            cmdText.AppendLine(" ,WorkPlace  = @IN_WorkPlace");
            cmdText.AppendLine(" ,StartTime = $IN_StartTime");
            cmdText.AppendLine(" ,EndTime  = @IN_EndTime");
            cmdText.AppendLine(" ,WorkTime  = @IN_WorkTime");
            cmdText.AppendLine(" ,Memo  = @IN_Memo");

            //Para
            Hashtable paras = new Hashtable();
            ISecurity sec = Security.Instance;

            cmdWhere.AppendLine(" ID = @IN_HID AND Date=@IN_Date");
            base.AddParam(paras, "IN_HID", t_Work_D.HID);
            base.AddParam(paras, "IN_Date", t_Work_D.Date, true);
            base.AddParam(paras, "IN_LineNo", t_Work_D.LineNo);
            base.AddParam(paras, "IN_PID", t_Work_D.PID);
            base.AddParam(paras, "IN_WorkPlace", t_Work_D.WorkPlace);
            base.AddParam(paras, "IN_StartTime", t_Work_D.StartTime);
            base.AddParam(paras, "IN_EndTime", t_Work_D.EndTime);
            base.AddParam(paras, "IN_WorkTime", t_Work_D.WorkTime);
            base.AddParam(paras, "IN_Memo", t_Work_D.Memo);

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
        /// Delete T_Work_D By Id 
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        /// 
        public int Delete(int HId, int Id)
        {
            //SQL String 
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine("DELETE [dbo].[T_Work_D]");

            //Parameter
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine("HID = @IN_HId");
            cmdWhere.AppendLine("AND ID = @IN_ID");
            base.AddParam(paras, "IN_HId", HId);
            base.AddParam(paras, "IN_ID", Id);
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }

        #endregion

        #region Delete List By HId
        /// <summary>
        /// Delete List<T_Work_D> By Id 
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        /// 
        public int DeleteAllByHId(int HID)
        {
            //SQL String 
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine("DELETE [dbo].[T_Work_D]");

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
