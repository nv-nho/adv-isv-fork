using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OMS.Models;

namespace OMS.DAC
{
    /// <summary>
    /// Class M_Config_D Service
    /// </summary>
    public class Config_DService : BaseService
    {
        #region Contructor
        /// <summary>
        /// Contructor M_Config_DSetting Service
        /// </summary>
        private Config_DService()
            : base()
        {
        }

        /// <summary>
        /// Contructor M_Config_DSetting Service
        /// </summary>
        /// <param name="db"></param>
        public Config_DService(DB db)
            : base(db)
        {
        }

        #endregion

        #region Get Data

        /// <summary>
        /// Get List By Config Cd
        /// </summary>
        /// <param name="configCd"></param>
        /// <returns></returns>
        public IList<M_Config_D> GetListByConfigCd(string configCd)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" ID");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.M_Config_H SH");
            cmdWhere.AppendLine(" SH.ConfigCD = @IN_ConfigCd");

            //Param
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "@IN_ConfigCd", configCd);
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            StringBuilder cmdOutText = new StringBuilder();
            cmdOutText.AppendLine(" SELECT");
            cmdOutText.AppendLine("   SD.*");
            cmdOutText.AppendLine(" FROM");
            cmdOutText.AppendLine(" (");
            cmdOutText.AppendLine(cmdText.ToString());
            cmdOutText.AppendLine(" ) AS  VIEW_1");
            cmdOutText.AppendLine(" INNER JOIN dbo.M_Config_D AS SD ON SD.HID = VIEW_1.ID");

            return this.db.FindList1<M_Config_D>(cmdOutText.ToString(), paras);
        }

        /// <summary>
        /// Get List By Config Cd
        /// </summary>
        /// <param name="configCd"></param>
        /// <returns></returns>
        public IList<M_Config_D> GetListByConfigCdWithVisibleSetting(string configCd)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" ID");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.M_Config_H SH");
            cmdWhere.AppendLine(" SH.ConfigCD = @IN_ConfigCd");

            //Param
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "@IN_ConfigCd", configCd);
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            StringBuilder cmdOutText = new StringBuilder();
            cmdOutText.AppendLine(" SELECT");
            cmdOutText.AppendLine("   SD.*");
            cmdOutText.AppendLine(" FROM");
            cmdOutText.AppendLine(" (");
            cmdOutText.AppendLine(cmdText.ToString());
            cmdOutText.AppendLine(" ) AS  VIEW_1");
            cmdOutText.AppendLine(" INNER JOIN dbo.M_Config_D AS SD ON SD.HID = VIEW_1.ID");
            cmdOutText.AppendLine(" WHERE SD.Value4 <> '0'");

            return this.db.FindList1<M_Config_D>(cmdOutText.ToString(), paras);
        }

        /// <summary>
        /// GetListVacationDateForAttendanceApproval
        /// </summary>
        /// <param name="configCd"></param>
        /// <returns></returns>
        public IList<VacationDateInFoByAttendanceApproval> GetListVacationDateForAttendanceApproval(string configCd, DateTime startDate, DateTime endDate, int uId)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" A.Value1");
            cmdText.AppendLine(" ,min(A.Value3) AS Value3");
            cmdText.AppendLine(" ,SUM(A.VacationDate) AS VacationDate");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" ( SELECT");
            cmdText.AppendLine(" C.HID");
            cmdText.AppendLine(" ,C.Value1");
            cmdText.AppendLine(" ,C.Value2");
            cmdText.AppendLine(" ,C.Value3");
            cmdText.AppendLine(" ,C.Value4");
            cmdText.AppendLine(" , CASE WHEN AF.ID IS NULL THEN 0 ELSE 1 END AS VacationDate");
            cmdText.AppendLine("  FROM [dbo].[M_Config_D] C");
            cmdText.AppendLine("  LEFT JOIN T_Attendance AF ON C.Value1 = AF.VacationFullCD");
            cmdText.AppendLine(" AND AF.UID = @IN_UID");
            cmdText.AppendLine("  AND AF.Date BETWEEN @IN_StartDate AND @IN_EndDate");

            cmdText.AppendLine(" UNION ALL");
            cmdText.AppendLine(" SELECT C.HID,C.Value1,C.Value2,C.Value3,C.Value4, CASE WHEN AM.ID IS NULL THEN 0 ELSE 0.4 END AS VacationDate");
            cmdText.AppendLine(" FROM [dbo].[M_Config_D] C");
            cmdText.AppendLine(" LEFT JOIN T_Attendance AM ON C.Value1 = AM.VacationMorningCD");
            cmdText.AppendLine(" AND AM.UID =@IN_UID");
            cmdText.AppendLine(" AND AM.Date BETWEEN @IN_StartDate AND @IN_EndDate");

            cmdText.AppendLine(" UNION ALL");
            cmdText.AppendLine("SELECT C.HID,C.Value1,C.Value2,C.Value3,C.Value4, CASE WHEN AA.ID IS NULL THEN 0 ELSE 0.6 END AS VacationDate");
            cmdText.AppendLine(" FROM [dbo].[M_Config_D] C");
            cmdText.AppendLine(" LEFT JOIN T_Attendance AA ON C.Value1 = AA.VacationAfternoonCD");
            cmdText.AppendLine(" AND AA.UID = @IN_UID");
            cmdText.AppendLine(" AND AA.Date BETWEEN @IN_StartDate AND @IN_EndDate");
            cmdText.AppendLine(" ) A");

            cmdText.AppendLine(" left join dbo.M_Config_H CH");
            cmdText.AppendLine(" ON CH.ID = A.HID");
            cmdText.AppendLine(" and CH.ConfigCD= @IN_ConfigCd");
            cmdText.AppendLine(" where CH.ID IS not null");

            cmdText.AppendLine("GROUP BY A.Value1");
            //Param
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "@IN_StartDate", startDate, true);
            base.AddParam(paras, "@IN_EndDate", endDate, true);
            base.AddParam(paras, "@IN_UID", uId);
            base.AddParam(paras, "@IN_ConfigCd", configCd);

            return this.db.FindList1<VacationDateInFoByAttendanceApproval>(cmdText.ToString(), paras);
        }


        /// <summary>
        /// Get List By Config Cd
        /// </summary>
        /// <param name="configCd"></param>
        /// <returns></returns>
        public IList<VacationDateInFoByAttendanceSummary> GetListVacationDateForAttendanceSummary(string configCd, DateTime startDate, DateTime endDate, int uId)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" A.Value1");
            cmdText.AppendLine(" ,MIN(A.Value3) AS Value3");
            cmdText.AppendLine(" ,SUM(A.VacationDate) AS VacationDate");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" ( SELECT");
            cmdText.AppendLine(" C.HID");
            cmdText.AppendLine(" ,C.Value1");
            cmdText.AppendLine(" ,C.Value2");
            cmdText.AppendLine(" ,C.Value3");
            cmdText.AppendLine(" ,C.Value4");
            cmdText.AppendLine(" , CASE WHEN AF.ID IS NULL THEN 0 ELSE 1 END AS VacationDate");
            cmdText.AppendLine(" FROM [dbo].[M_Config_D] C");
            cmdText.AppendLine(" LEFT JOIN T_Attendance AF ON C.Value1 = AF.VacationFullCD");
            cmdText.AppendLine(" AND AF.UID = @IN_UID");
            cmdText.AppendLine("  AND AF.Date BETWEEN @IN_StartDate AND @IN_EndDate");

            cmdText.AppendLine(" UNION ALL");
            cmdText.AppendLine(" SELECT C.HID,C.Value1,C.Value2,C.Value3,C.Value4, CASE WHEN AM.ID IS NULL THEN 0 ELSE 0.4 END AS VacationDate");
            cmdText.AppendLine(" FROM [dbo].[M_Config_D] C");
            cmdText.AppendLine(" LEFT JOIN T_Attendance AM ON C.Value1 = AM.VacationMorningCD");
            cmdText.AppendLine(" AND AM.UID =@IN_UID");
            cmdText.AppendLine(" AND AM.Date BETWEEN @IN_StartDate AND @IN_EndDate");

            cmdText.AppendLine(" UNION ALL");
            cmdText.AppendLine("SELECT C.HID,C.Value1,C.Value2,C.Value3,C.Value4, CASE WHEN AA.ID IS NULL THEN 0 ELSE 0.6 END AS VacationDate");
            cmdText.AppendLine(" FROM [dbo].[M_Config_D] C");
            cmdText.AppendLine(" LEFT JOIN T_Attendance AA ON C.Value1 = AA.VacationAfternoonCD");
            cmdText.AppendLine(" AND AA.UID = @IN_UID");
            cmdText.AppendLine(" AND AA.Date BETWEEN @IN_StartDate AND @IN_EndDate");
            cmdText.AppendLine(" ) A");

            cmdText.AppendLine(" left join dbo.M_Config_H CH");
            cmdText.AppendLine(" ON CH.ID = A.HID");
            cmdText.AppendLine(" and CH.ConfigCD= @IN_ConfigCd");
            cmdText.AppendLine(" where CH.ID IS not null");

            cmdText.AppendLine("GROUP BY A.Value1");
            //Param
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "@IN_StartDate", startDate, true);
            base.AddParam(paras, "@IN_EndDate", endDate, true);
            base.AddParam(paras, "@IN_UID", uId);
            base.AddParam(paras, "@IN_ConfigCd", configCd);

            return this.db.FindList1<VacationDateInFoByAttendanceSummary>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Get list by header ID
        /// Create Author: ISV-HUNG
        /// </summary>
        /// <param name="headerID">HeaderID</param>
        /// <returns></returns>
        public IList<M_Config_D> GetByListByHeaderID(int headerID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  [HID]");
            cmdText.AppendLine(" ,[No]");
            cmdText.AppendLine(" ,[Value1]");
            cmdText.AppendLine(" ,[Value2]");
            cmdText.AppendLine(" ,[Value3]");
            cmdText.AppendLine(" ,[Value4]");
            cmdText.AppendLine(" FROM	dbo.M_Config_D");
            cmdWhere.AppendLine(" [HID] = @IN_HID");

            //Param
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_HID", headerID);

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            cmdText.AppendLine(" ORDER BY Value1");

            return this.db.FindList1<M_Config_D>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Get Value
        /// Create Author: ISV-HUNG
        /// </summary>
        /// <param name="configCd">ConfigCd</param>
        /// <param name="value1">Value1</param>
        /// <returns></returns>
        public int? GetValue(string configCd, int value1)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  ID");
            cmdText.AppendLine(" FROM dbo.M_Config_H SH");

            //Param
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine(" SH.ConfigCD = @IN_ConfigCd");
            base.AddParam(paras, "IN_ConfigCd", configCd);

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            StringBuilder cmdOutText = new StringBuilder();
            cmdOutText.AppendLine(" SELECT");
            cmdOutText.AppendLine("   SD.Value3");
            cmdOutText.AppendLine(" FROM");
            cmdOutText.AppendLine(" (");
            cmdOutText.AppendLine(cmdText.ToString());
            cmdOutText.AppendLine(" ) AS  VIEW_1");
            cmdOutText.AppendLine(" INNER JOIN dbo.M_Config_D AS SD ON SD.HID = VIEW_1.ID AND SD.Value1 = @IN_Value1");
            //cmdOutText.AppendLine(" )");

            base.AddParam(paras, "IN_Value1", value1);

            //---------------Add 2015/01/06 ISV-HUNG-----------------//
            var ret = string.Format("{0}", this.db.ExecuteScalar1(cmdOutText.ToString(), paras));
            if (string.IsNullOrEmpty(ret))
            {
                return null;
            }
            else
            {
                return Convert.ToInt32(this.db.ExecuteScalar1(cmdOutText.ToString(), paras));
            }
            //---------------Add 2015/01/06 ISV-HUNG-----------------//
        }

        /// <summary>
        /// Get Value2 by value1
        /// Create Author: ISV-GIAM
        /// </summary>
        /// <param name="configCd">ConfigCd</param>
        /// <param name="value1">Value1</param>
        /// <returns></returns>
        public string GetValue2(string configCd, int value1)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  ID");
            cmdText.AppendLine(" FROM dbo.M_Config_H SH");

            //Param
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine(" SH.ConfigCD = @IN_ConfigCd");
            base.AddParam(paras, "IN_ConfigCd", configCd);

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            StringBuilder cmdOutText = new StringBuilder();
            cmdOutText.AppendLine(" SELECT");
            cmdOutText.AppendLine("   SD.Value2");
            cmdOutText.AppendLine(" FROM");
            cmdOutText.AppendLine(" (");
            cmdOutText.AppendLine(cmdText.ToString());
            cmdOutText.AppendLine(" ) AS  VIEW_1");
            cmdOutText.AppendLine(" INNER JOIN dbo.M_Config_D AS SD ON SD.HID = VIEW_1.ID AND SD.Value1 = @IN_Value1");

            base.AddParam(paras, "IN_Value1", value1);

            return string.Format("{0}", this.db.ExecuteScalar1(cmdOutText.ToString(), paras));
        }

        /// <summary>
        /// Get Value3 by value1
        /// Create Author: ISV-KIET
        /// </summary>
        /// <param name="configCd">ConfigCd</param>
        /// <param name="value1">Value1</param>
        /// <returns></returns>
        public string GetValue3(string configCd, int value1)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  ID");
            cmdText.AppendLine(" FROM dbo.M_Config_H SH");

            //Param
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine(" SH.ConfigCD = @IN_ConfigCd");
            base.AddParam(paras, "IN_ConfigCd", configCd);

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            StringBuilder cmdOutText = new StringBuilder();
            cmdOutText.AppendLine(" SELECT");
            cmdOutText.AppendLine("   SD.Value3");
            cmdOutText.AppendLine(" FROM");
            cmdOutText.AppendLine(" (");
            cmdOutText.AppendLine(cmdText.ToString());
            cmdOutText.AppendLine(" ) AS  VIEW_1");
            cmdOutText.AppendLine(" INNER JOIN dbo.M_Config_D AS SD ON SD.HID = VIEW_1.ID AND SD.Value1 = @IN_Value1");

            base.AddParam(paras, "IN_Value1", value1);

            return string.Format("{0}", this.db.ExecuteScalar1(cmdOutText.ToString(), paras));
        }

        /// <summary>
        /// Get Value4 by value1
        /// Create Author: ISV-KIET
        /// </summary>
        /// <param name="configCd">ConfigCd</param>
        /// <param name="value1">Value1</param>
        /// <returns></returns>
        public string GetValue4(string configCd, int value1)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  ID");
            cmdText.AppendLine(" FROM dbo.M_Config_H SH");

            //Param
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine(" SH.ConfigCD = @IN_ConfigCd");
            base.AddParam(paras, "IN_ConfigCd", configCd);

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            StringBuilder cmdOutText = new StringBuilder();
            cmdOutText.AppendLine(" SELECT");
            cmdOutText.AppendLine("   SD.Value4");
            cmdOutText.AppendLine(" FROM");
            cmdOutText.AppendLine(" (");
            cmdOutText.AppendLine(cmdText.ToString());
            cmdOutText.AppendLine(" ) AS  VIEW_1");
            cmdOutText.AppendLine(" INNER JOIN dbo.M_Config_D AS SD ON SD.HID = VIEW_1.ID AND SD.Value1 = @IN_Value1");

            base.AddParam(paras, "IN_Value1", value1);

            return string.Format("{0}", this.db.ExecuteScalar1(cmdOutText.ToString(), paras));
        }

        /// <summary>
        /// Get List By Config Cd For Combobox
        /// </summary>
        /// <param name="configCd"></param>
        /// <returns></returns>
        public IList<DropDownModel> GetListByConfigCdComboboxData(string configCd)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" ID");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.M_Config_H SH");
            cmdWhere.AppendLine(" SH.ConfigCD = @IN_ConfigCd");

            //Param
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "@IN_ConfigCd", configCd);
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            StringBuilder cmdOutText = new StringBuilder();
            cmdOutText.AppendLine(" SELECT");
            cmdOutText.AppendLine(" SD.Value1 AS Value");
            cmdOutText.AppendLine(" ,SD.Value2 AS DisplayName");
            cmdOutText.AppendLine(" FROM");
            cmdOutText.AppendLine(" (");
            cmdOutText.AppendLine(cmdText.ToString());
            cmdOutText.AppendLine(" ) AS  VIEW_1");
            cmdOutText.AppendLine(" INNER JOIN dbo.M_Config_D AS SD ON SD.HID = VIEW_1.ID");

            return this.db.FindList1<DropDownModel>(cmdOutText.ToString(), paras);
        }

        #endregion

        #region Insert
        /// <summary>
        /// Insert data
        /// </summary>
        /// <param name="detailItem">M_Config_D</param>
        /// <returns></returns>
        public int Insert(M_Config_D detailItem)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine(" INSERT INTO dbo.M_Config_D");
            cmdText.AppendLine(" (");
            cmdText.AppendLine("  HID");
            cmdText.AppendLine(" ,[No]");
            cmdText.AppendLine(" ,Value1");
            cmdText.AppendLine(" ,Value2");
            cmdText.AppendLine(" ,Value3");
            cmdText.AppendLine(" ,Value4");
            cmdText.AppendLine(" )");
            cmdText.AppendLine(" VALUES");
            cmdText.AppendLine(" (");
            cmdText.AppendLine("  @IN_HID");
            cmdText.AppendLine(" ,@IN_No");
            cmdText.AppendLine(" ,@IN_Value1");
            cmdText.AppendLine(" ,@IN_Value2");
            cmdText.AppendLine(" ,@IN_Value3");
            cmdText.AppendLine(" ,@IN_Value4");
            cmdText.AppendLine(" )");

            //Param
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_HID", detailItem.HID);
            base.AddParam(paras, "IN_No", detailItem.No);
            base.AddParam(paras, "IN_Value1", detailItem.Value1);
            base.AddParam(paras, "IN_Value2", detailItem.Value2);
            base.AddParam(paras, "IN_Value3", detailItem.Value3);
            base.AddParam(paras, "IN_Value4", detailItem.Value4);

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }
        #endregion

        #region Delete

        /// <summary>
        /// Delete by headerID
        /// </summary>
        /// <param name="headerID">Header ID</param>
        /// <returns></returns>
        public int Delete(int headerID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" DELETE FROM dbo.M_Config_D");

            //Param
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine(" HID = @IN_HID");
            base.AddParam(paras, "IN_HID", headerID);

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
