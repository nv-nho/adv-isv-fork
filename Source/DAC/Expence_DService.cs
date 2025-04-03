using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using OMS.Utilities;
using OMS.Models;

namespace OMS.DAC
{
    public class Expence_DService: BaseService
    {
        #region Constructor
        /// <summary>
        /// Contructor of T_Expence_D service
        /// </summary>        
        private Expence_DService()
            : base()
        {
        }

        /// <summary>
        /// Contructor of T_Expence_D service
        /// </summary>
        /// <param name="db">Class DB</param>
        public Expence_DService(DB db)
            : base(db)
        {
        }
        #endregion

        #region Get Data
        /// <summary>
        /// Get List by HID
        /// </summary>
        /// <returns>T_Expence_D</returns>
        public IList<T_Expence_D> GetListByHID(int HID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  1 AS RowNumber");
            cmdText.AppendLine(" ,T1.*");
            cmdText.AppendLine(" FROM dbo.T_Expence_D AS T1");
            cmdWhere.AppendLine(" HID = @IN_HID");

            //Parameter
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_HID", HID);
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.FindList1<T_Expence_D>(cmdText.ToString(), paras);

        }
        /// <summary>
        /// Get List by cond
        /// </summary>
        /// <returns>T_Expence_D</returns>
        public IList<T_Expence_D> GetListByCond(int HID,DateTime? dateFrom, DateTime? dateTo)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  1 AS RowNumber");
            cmdText.AppendLine(" ,T1.*");
            cmdText.AppendLine(" FROM dbo.T_Expence_D AS T1");
            cmdWhere.AppendLine(" HID = @IN_HID");
            //Parameter
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_HID", HID);
            if ((!string.IsNullOrEmpty(dateFrom.ToString())) && (!string.IsNullOrEmpty(dateTo.ToString())))
            {
                cmdWhere.AppendLine("AND (T1.Date  >= @IN_Date1 AND T1.Date  <= @IN_Date2) ");
                base.AddParam(paras, "IN_Date1", dateFrom, true);
                base.AddParam(paras, "IN_Date2", dateTo, true);
            }
            else if ((!string.IsNullOrEmpty(dateFrom.ToString())) && (string.IsNullOrEmpty(dateTo.ToString())))
            {
                cmdWhere.AppendLine("AND (T1.Date  >= @IN_Date1) ");
                base.AddParam(paras, "IN_Date1", dateFrom, true);
            }
            else if ((string.IsNullOrEmpty(dateFrom.ToString())) && (!string.IsNullOrEmpty(dateTo.ToString())))
            {
                cmdWhere.AppendLine("AND (T1.Date  <= @IN_Date2) ");
                base.AddParam(paras, "IN_Date2", dateTo, true);
            }
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.FindList1<T_Expence_D>(cmdText.ToString(), paras);

        }


        /// <summary>
        /// </summary>
        /// <param name="configCd"></param>
        /// <returns></returns>
        public IList<DropDownModel> GetRouteStyle(string configCd,int Routestyle)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" ID");
            cmdText.AppendLine(" FROM dbo.M_Config_H SH");

            //Param
            Hashtable paras = new Hashtable();

            cmdWhere.AppendLine(" SH.ConfigCD = @IN_ConfigCD");
            base.AddParam(paras, "IN_ConfigCD", configCd);
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            //SQL OUT
            StringBuilder cmdOutText = new StringBuilder();
            StringBuilder cmdOutWhere = new StringBuilder();
            cmdOutText.AppendLine(" SELECT");
            cmdOutText.AppendLine("  SD.Value1	as Value");
            cmdOutText.AppendLine(" ,SD.Value2	as DisplayName");
            cmdOutText.AppendLine(" FROM");
            cmdOutText.AppendLine(" (");
            cmdOutText.AppendLine(cmdText.ToString());
            cmdOutText.AppendLine(" ) AS  VIEW_1");
            cmdOutText.AppendLine(" INNER JOIN dbo.M_Config_D AS SD ON SD.HID = VIEW_1.ID");

            cmdOutText.AppendLine(" Where SD.Value1 = @IN_RouteStyle");
            base.AddParam(paras, "IN_RouteStyle", Routestyle);


            IList<DropDownModel> ret = this.db.FindList1<DropDownModel>(cmdOutText.ToString(), paras);
            return ret;
        }

        ///// <summary>
        ///// Get List by HID
        ///// </summary>
        ///// <returns>T_Expence_D</returns>
        //public IList<T_ExpenceResult> getDataExpenceResult(List<int> listId)
        //{
        //    //SQL String
        //    StringBuilder cmdText = new StringBuilder();
        //    StringBuilder cmdWhere = new StringBuilder();

        //    cmdText.AppendLine(" SELECT");
        //    cmdText.AppendLine("-1 as HID, A.ProjectID,sum(A.WorkTime) AS WorkTime,min(A.ProjectCD) AS ProjectCD,min(A.ProjectName) AS ProjectName,min(A.WorkPlace) AS WorkPlace");
        //    cmdText.AppendLine(" FROM(");
        //    cmdText.AppendLine(" select WD.PID AS ProjectID,");
        //    cmdText.AppendLine(" WD.WorkTime as WorkTime,");
        //    cmdText.AppendLine(" P.ProjectCD ,P.ProjectName,P.WorkPlace");
        //    cmdText.AppendLine(" from dbo.T_Expence_D WD");
        //    cmdText.AppendLine(" left join dbo.M_Project P");
        //    cmdText.AppendLine(" ON WD.PID =P.ID");

        //    cmdWhere.AppendLine("WD.HID in (");

        //    int i = 0;
        //    foreach (var item in listId)
        //    {
        //        if (i < listId.Count - 1)
        //        {
        //            cmdWhere.AppendFormat("{0},", item);
        //        }
        //        else
        //        {
        //            cmdWhere.AppendFormat("{0}", item);
        //        }

        //        i++;
        //    }
        //    cmdWhere.AppendLine(")");

        //    //Parameter
        //    Hashtable paras = new Hashtable();
        //    if (cmdWhere.Length != 0)
        //    {
        //        cmdText.AppendLine(" WHERE ");
        //        cmdText.AppendLine(cmdWhere.ToString());
        //    }
        //    cmdText.AppendLine(")A");
        //    cmdText.AppendLine("group by A.ProjectID");

        //    return this.db.FindList1<T_WorkResult>(cmdText.ToString(), paras);

        //}

        ///// <summary>
        ///// Get List by HID
        ///// </summary>
        ///// <returns>WorkDInfoEXcel</returns>
        //public IList<WorkDInfoEXcel> GetListWorkDInfoEXcelByHID(int HID)
        //{
        //    //SQL String
        //    StringBuilder cmdText = new StringBuilder();
        //    StringBuilder cmdWhere = new StringBuilder();

        //    cmdText.AppendLine(" SELECT");
        //    cmdText.AppendLine("  w.WorkTime as workTime");
        //    cmdText.AppendLine(" ,p.ID as projectId");
        //    cmdText.AppendLine(" ,p.ProjectName as projectNm");
        //    cmdText.AppendLine(" FROM dbo.T_Work_D AS w");
        //    cmdText.AppendLine(" inner join dbo.M_Project as p");
        //    cmdText.AppendLine(" on w.PID = p.ID");
        //    cmdWhere.AppendLine(" w.HID = @IN_HID");

        //    //Parameter
        //    Hashtable paras = new Hashtable();
        //    base.AddParam(paras, "IN_HID", HID);
        //    if (cmdWhere.Length != 0)
        //    {
        //        cmdText.AppendLine(" WHERE ");
        //        cmdText.AppendLine(cmdWhere.ToString());
        //    }

        //    return this.db.FindList1<WorkDInfoEXcel>(cmdText.ToString(), paras);

        //}

        #endregion

        #region Insert
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="T_Expence_D">T_Expence_D</param>
        /// <returns></returns>
        public int Insert(T_Expence_D t_Expence_D)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine(" INSERT INTO");
            cmdText.AppendLine(" dbo.T_Expence_D");
            cmdText.AppendLine(" (");
            cmdText.AppendLine("  HID");
            cmdText.AppendLine(" ,[LineNo]");
            cmdText.AppendLine(" ,Date");
            cmdText.AppendLine(" ,PaidTo");
            cmdText.AppendLine(" ,RouteFrom");
            cmdText.AppendLine(" ,RouteTo");
            cmdText.AppendLine(" ,RouteType");
            cmdText.AppendLine(" ,Note");
            cmdText.AppendLine(" ,TaxType");
            cmdText.AppendLine(" ,TaxRate");
            cmdText.AppendLine(" ,Amount");
            cmdText.AppendLine(" ,TaxAmount");
            cmdText.AppendLine(" )");
            cmdText.AppendLine(" VALUES");
            cmdText.AppendLine(" (");
            cmdText.AppendLine("  @IN_HID");
            cmdText.AppendLine(" ,@IN_LineNo");
            cmdText.AppendLine(" ,@IN_Date");
            cmdText.AppendLine(" ,@IN_PaidTo");
            cmdText.AppendLine(" ,@IN_RouteFrom");
            cmdText.AppendLine(" ,@IN_RouteTo");
            cmdText.AppendLine(" ,@IN_RouteType");
            cmdText.AppendLine(" ,@IN_Note");
            cmdText.AppendLine(" ,@IN_TaxType");
            cmdText.AppendLine(" ,@IN_TaxRate");
            cmdText.AppendLine(" ,@IN_Amount");
            cmdText.AppendLine(" ,@IN_TaxAmount");
            cmdText.AppendLine(" )");

            //Param
            Hashtable paras = new Hashtable();
            ISecurity sec = Security.Instance;

            base.AddParam(paras, "IN_HID", t_Expence_D.HID, true);
            base.AddParam(paras, "IN_LineNo", t_Expence_D.LineNo, true);
            base.AddParam(paras, "IN_Date", t_Expence_D.Date);
            base.AddParam(paras, "IN_PaidTo", t_Expence_D.PaidTo);
            base.AddParam(paras, "IN_RouteFrom", t_Expence_D.RouteFrom);
            base.AddParam(paras, "IN_RouteTo", t_Expence_D.RouteTo);
            base.AddParam(paras, "IN_RouteType", t_Expence_D.RouteType);
            base.AddParam(paras, "IN_Note", t_Expence_D.Note);
            base.AddParam(paras, "IN_TaxType", t_Expence_D.TaxType);
            base.AddParam(paras, "IN_TaxRate", t_Expence_D.TaxRate);
            base.AddParam(paras, "IN_Amount", t_Expence_D.Amount);
            base.AddParam(paras, "IN_TaxAmount", t_Expence_D.TaxAmount);

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }
        #endregion

        #region Update
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="T_Expence_D">T_Expence_D</param>
        /// <returns></returns>
        public int Update(T_Expence_D t_Expence_D)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            //StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" UPDATE");
            cmdText.AppendLine(" dbo.T_Expence_D");
            cmdText.AppendLine(" SET");
            cmdText.AppendLine("  LineNo  = @IN_LineNo");
            cmdText.AppendLine(" ,Date    = @IN_Date");
            cmdText.AppendLine(" ,PaidTo  = @IN_PaidTo");
            cmdText.AppendLine(" ,RouteFrom = @IN_RouteFrom");
            cmdText.AppendLine(" ,RouteTo  = @IN_RouteTo");
            cmdText.AppendLine(" ,RouteType  = @IN_RouteType");
            cmdText.AppendLine(" ,Note  = @IN_Note");

            cmdText.AppendLine(" ,TaxType  = @IN_TaxType");
            cmdText.AppendLine(" ,TaxRate  = @IN_TaxRate");
            cmdText.AppendLine(" ,Amount  = @IN_Amount");
            cmdText.AppendLine(" ,TaxAmount  = @IN_TaxAmount");
            cmdText.AppendLine(" WHERE HID = @IN_HID");

            //Para
            Hashtable paras = new Hashtable();
            ISecurity sec = Security.Instance;

            base.AddParam(paras, "IN_HID", t_Expence_D.HID);
            base.AddParam(paras, "IN_LineNo", t_Expence_D.LineNo, true);
            base.AddParam(paras, "IN_Date", t_Expence_D.Date);
            base.AddParam(paras, "IN_PaidTo", t_Expence_D.PaidTo);
            base.AddParam(paras, "IN_RouteFrom", t_Expence_D.RouteFrom);
            base.AddParam(paras, "IN_RouteTo", t_Expence_D.RouteTo);
            base.AddParam(paras, "IN_RouteType", t_Expence_D.RouteType);
            base.AddParam(paras, "IN_Note", t_Expence_D.Note);
            base.AddParam(paras, "IN_TaxType", t_Expence_D.TaxType);
            base.AddParam(paras, "IN_TaxRate", t_Expence_D.TaxRate);
            base.AddParam(paras, "IN_Amount", t_Expence_D.Amount);
            base.AddParam(paras, "IN_TaxAmount", t_Expence_D.TaxAmount);

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }

        #endregion


        #region Delete By Id
        /// <summary>
        /// Delete T_Expence_D By Id 
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        /// 
        public int Delete(int HId, int Id)
        {
            //SQL String 
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine("DELETE [dbo].[T_Expence_D]");

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
        /// Delete List<T_Expence_D> By Id 
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        /// 
        public int DeleteAllByHId(int HID)
        {
            //SQL String 
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine("DELETE [dbo].[T_Expence_D]");

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
