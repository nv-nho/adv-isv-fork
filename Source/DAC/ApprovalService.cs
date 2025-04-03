using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OMS.Models;
using OMS.Utilities;

namespace OMS.DAC
{
    public class ApprovalService : BaseService
    {
        #region Contructor
        /// <summary>
        /// Contructor of User service
        /// </summary>        
        private ApprovalService()
            : base()
        {
        }

        /// <summary>
        /// Contructor of User service
        /// </summary>
        /// <param name="db">Class DB</param>
        public ApprovalService(DB db)
            : base(db)
        {
        }

        #endregion

        #region Get Data

        /// <summary>
        /// Get year Combobox data by user id
        /// </summary>
        /// <param name="userID">user id</param>
        /// <returns></returns>
        public IList<DropDownModel> GetCbbTargetYear(String userID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            //Parameter
            Hashtable paras = new Hashtable();

            cmdText.AppendLine(" Select Distinct year([Date]) AS Value, year([Date]) AS DisplayName From T_Attendance Where UID = @IN_UID");

            base.AddParam(paras, "IN_UID", userID, true);

            return this.db.FindList1<DropDownModel>(cmdText.ToString(), paras);
        }


        /// <summary>
        /// Get year Combobox data by department id
        /// </summary>
        /// <returns></returns>
        public IList<DropDownModel> GetCbbTargetYearByDepartment(int departmentID = -1)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            //Parameter
            Hashtable paras = new Hashtable();

            cmdText.AppendLine(" Select Distinct year([Date]) AS Value, year([Date]) AS DisplayName From T_Attendance ");

            return this.db.FindList1<DropDownModel>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Get total row for list
        /// </summary>
        /// <param name="year">year</param>
        /// <param name="userID">userID</param>
        /// <returns></returns>
        public int getTotalRow(string departmentID, string userID, string type, DateTime? startDate, DateTime? endDate)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();
            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  count(*)");
            cmdText.AppendLine(" FROM dbo.T_Attendance AS S");
            cmdText.AppendLine(" LEFT JOIN M_User U ON S.UID = U.ID");
            cmdText.AppendLine(" WHERE ApprovalStatus <> @IN_ApprovalStatus");

            //Para
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_ApprovalStatus", Utilities.AttendanceApprovalStatus.None, true);

            if (!string.IsNullOrEmpty(departmentID))
            {
                cmdWhere.AppendLine(" DepartmentID = @IN_DepartmentID");
                base.AddParam(paras, "IN_DepartmentID", departmentID, true);
            }

            if (!string.IsNullOrEmpty(userID))
            {
                if (cmdWhere.Length != 0)
                {
                    cmdWhere.AppendLine(" AND");
                }
                cmdWhere.AppendLine(" UID = @IN_UID");
                base.AddParam(paras, "IN_UID", userID, true);
            }

            if (!string.IsNullOrEmpty(type))
            {
                if (cmdWhere.Length != 0)
                {
                    cmdWhere.AppendLine(" AND");
                }
                cmdWhere.AppendLine(" ApprovalStatus = @IN_Type");
                base.AddParam(paras, "IN_Type", type, true);
            }
            else
            {
                if (cmdWhere.Length != 0)
                {
                    cmdWhere.AppendLine(" AND");
                }
                cmdWhere.AppendLine(" ApprovalStatus <> @IN_Type");
                base.AddParam(paras, "IN_Type", AttendanceApprovalStatus.NeedApproval.GetHashCode().ToString(), true);
            }

            if (startDate != null)
            {
                if (cmdWhere.Length != 0)
                {
                    cmdWhere.AppendLine(" AND");
                }
                cmdWhere.AppendLine(" S.Date >= @IN_StartdateOfServiceFrom");
                base.AddParam(paras, "IN_StartdateOfServiceFrom", startDate);

            }

            if (endDate != null)
            {
                if (cmdWhere.Length != 0)
                {
                    cmdWhere.AppendLine(" AND");
                }
                cmdWhere.AppendLine(" S.Date <= @IN_EnddateOfServiceFrom");
                base.AddParam(paras, "IN_EnddateOfServiceFrom", endDate);
            }

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" AND ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return int.Parse(this.db.ExecuteScalar1(cmdText.ToString(), paras).ToString());
        }


        /// <summary>
        /// Get List by condition
        /// </summary>
        /// <param name="configCD">Config Code</param>
        /// <param name="configName">Config Name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="sortField">Sort Field</param>
        /// <param name="sortDirec">Sort Direction</param>
        /// <returns>List Setting Info</returns>
        public IList<ApprovalInfo> GetListByCond(string departmentID, string userID, string type, DateTime? startDate, DateTime? endDate,
                                            int pageIndex, int pageSize, int sortField, int sortDirec, bool isUseTimeApproval)
        {
            string[] fields = new string[] { "", "", "", "ApprovalStatus", "UserName1", "Date" };
            string[] direc = new string[] { "ASC", "DESC" };

            string RowNumber;
            if (fields[sortField - 1] == "Type")
            {
                RowNumber = fields[sortField - 1] + " " + direc[sortDirec - 1] + " , S.Year ASC";
                if (sortField == 1)
                {
                    RowNumber = fields[sortField - 1] + " " + direc[1] + " , S.Year ASC";
                }
            }
            else
            {
                RowNumber = fields[sortField - 1] + " " + direc[sortDirec - 1];
                if (sortField == 1)
                {
                    RowNumber = fields[sortField - 1] + " " + direc[1];
                }
            }

            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine((isUseTimeApproval ? "1" : "0") + " AS IsUseTimeApproval");
            cmdText.AppendLine(" ,S.ID");
            cmdText.AppendLine(" ,S.UID");

            cmdText.AppendLine(" ,S.ApprovalStatus");
            cmdText.AppendLine(" ,D.Value2 AS ApprovalStatusName");
            cmdText.AppendLine(" ,U.UserName1 AS UserName");
            cmdText.AppendLine(" ,Convert(varchar(10),S.Date,111) AS Date");
            cmdText.AppendLine(" ,S.StatusFlag");

            cmdText.AppendLine(" ,D1.Value2 AS VacationFull");
            cmdText.AppendLine(" ,D2.Value2 AS VacationMorning");
            cmdText.AppendLine(" ,D3.Value2 AS VacationAfternoon");

            cmdText.AppendLine(" ,LateHours");
            cmdText.AppendLine(" ,EarlyHours");

            cmdText.AppendLine(" ,CASE WHEN SH_Hours IS NOT NULL");
            cmdText.AppendLine("       THEN 'SH' ELSE '' END AS SH");

            cmdText.AppendLine(" ,CASE WHEN LH_Hours IS NOT NULL");
            cmdText.AppendLine("       THEN 'LH' ELSE '' END AS LH");

            cmdText.AppendLine(" ,CASE WHEN OverTimeHours1 IS NOT NULL");
            cmdText.AppendLine("         OR OverTimeHours2 IS NOT NULL");
            cmdText.AppendLine("         OR OverTimeHours3 IS NOT NULL");
            cmdText.AppendLine("         OR OverTimeHours4 IS NOT NULL");
            cmdText.AppendLine("         OR OverTimeHours5 IS NOT NULL");
            cmdText.AppendLine("         OR SH_OverTimeHours1 IS NOT NULL");//SH_OT
            cmdText.AppendLine("         OR SH_OverTimeHours2 IS NOT NULL");
            cmdText.AppendLine("         OR SH_OverTimeHours3 IS NOT NULL");
            cmdText.AppendLine("         OR SH_OverTimeHours4 IS NOT NULL");
            cmdText.AppendLine("         OR SH_OverTimeHours5 IS NOT NULL");
            cmdText.AppendLine("         OR LH_OverTimeHours1 IS NOT NULL");//LS_OT
            cmdText.AppendLine("         OR LH_OverTimeHours2 IS NOT NULL");
            cmdText.AppendLine("         OR LH_OverTimeHours3 IS NOT NULL");
            cmdText.AppendLine("         OR LH_OverTimeHours4 IS NOT NULL");
            cmdText.AppendLine("         OR LH_OverTimeHours5 IS NOT NULL");
            cmdText.AppendLine("       THEN 'OT' ELSE '' END AS OT");

            cmdText.AppendLine(" ," + string.Format("ROW_NUMBER() OVER(ORDER BY {0}) AS RowNumber", RowNumber));
            cmdText.AppendLine(" ,S.UpdateDate");
            cmdText.AppendLine(" FROM dbo.T_Attendance AS S");
            cmdText.AppendLine(" LEFT JOIN M_User U ON S.UID = U.ID");
            cmdText.AppendLine(" LEFT JOIN M_Config_H H ON H.ConfigCD = @IN_ConfigCD");
            cmdText.AppendLine(" LEFT JOIN M_Config_D D ON D.HID = H.ID AND D.Value1 = S.ApprovalStatus");


            cmdText.AppendLine(" LEFT JOIN M_Config_H H1 ON H1.ConfigCD = @IN_CONFIG_CD_VACATION_TYPE");
            cmdText.AppendLine(" LEFT JOIN M_Config_D D1 ON D1.HID = H1.ID AND D1.Value1 = S.VacationFullCD");
            cmdText.AppendLine(" LEFT JOIN M_Config_H H2 ON H2.ConfigCD = @IN_CONFIG_CD_VACATION_TYPE");
            cmdText.AppendLine(" LEFT JOIN M_Config_D D2 ON D2.HID = H2.ID AND D2.Value1 = S.VacationMorningCD");
            cmdText.AppendLine(" LEFT JOIN M_Config_H H3 ON H3.ConfigCD = @IN_CONFIG_CD_VACATION_TYPE");
            cmdText.AppendLine(" LEFT JOIN M_Config_D D3 ON D3.HID = H3.ID AND D3.Value1 = S.VacationAfternoonCD");

            cmdText.AppendLine(" WHERE ApprovalStatus <> @IN_ApprovalStatus");
            //Para
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_ConfigCD", M_Config_H.CONFIG_APPROVAL_TYPE.ToString(), true);
            base.AddParam(paras, "IN_ApprovalStatus", AttendanceApprovalStatus.None.GetHashCode().ToString(), true);
            base.AddParam(paras, "IN_CONFIG_CD_VACATION_TYPE", M_Config_H.CONFIG_CD_VACATION_TYPE.ToString(), true);

            if (!string.IsNullOrEmpty(departmentID))
            {
                cmdWhere.AppendLine(" DepartmentID = @IN_DepartmentID");
                base.AddParam(paras, "IN_DepartmentID", departmentID, true);
            }

            if (!string.IsNullOrEmpty(userID))
            {
                if (cmdWhere.Length != 0)
                {
                    cmdWhere.AppendLine(" AND");
                }
                cmdWhere.AppendLine(" UID = @IN_UID");
                base.AddParam(paras, "IN_UID", userID, true);
            }

            if (!string.IsNullOrEmpty(type))
            {
                if (cmdWhere.Length != 0)
                {
                    cmdWhere.AppendLine(" AND");
                }
                cmdWhere.AppendLine(" ApprovalStatus = @IN_Type");
                base.AddParam(paras, "IN_Type", type, true);
            }
            else
            {
                if (cmdWhere.Length != 0)
                {
                    cmdWhere.AppendLine(" AND");
                }
                cmdWhere.AppendLine(" ApprovalStatus <> @IN_Type");
                base.AddParam(paras, "IN_Type", AttendanceApprovalStatus.NeedApproval.GetHashCode().ToString(), true);
            }

            if (startDate != null)
            {
                if (cmdWhere.Length != 0)
                {
                    cmdWhere.AppendLine(" AND");
                }
                cmdWhere.AppendLine(" S.Date >= @IN_StartdateOfServiceFrom");
                base.AddParam(paras, "IN_StartdateOfServiceFrom", startDate);

            }

            if (endDate != null)
            {
                if (cmdWhere.Length != 0)
                {
                    cmdWhere.AppendLine(" AND");
                }
                cmdWhere.AppendLine(" S.Date <= @IN_EnddateOfServiceFrom");
                base.AddParam(paras, "IN_EnddateOfServiceFrom", endDate);
            }

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" AND ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            StringBuilder cmdOutText = new StringBuilder();
            StringBuilder cmdOutWhere = new StringBuilder();
            cmdOutText.AppendLine(" SELECT");
            cmdOutText.AppendLine("   *");
            cmdOutText.AppendLine(" FROM");
            cmdOutText.AppendLine(" (");
            cmdOutText.AppendLine(cmdText.ToString());
            cmdOutText.AppendLine(" ) AS  VIEW_1");

            cmdOutWhere.AppendLine(" RowNumber BETWEEN(@IN_PageIndex -1) * @IN_PageSize + 1 AND(((@IN_PageIndex -1) * @IN_PageSize + 1) + @IN_PageSize) - 1 ");
            base.AddParam(paras, "IN_PageIndex", pageIndex);
            base.AddParam(paras, "IN_PageSize", pageSize);

            if (cmdOutWhere.Length != 0)
            {
                cmdOutText.AppendLine(" WHERE ");
                cmdOutText.AppendLine(cmdOutWhere.ToString());
            }

            return this.db.FindList1<ApprovalInfo>(cmdOutText.ToString(), paras);
        }

        /// <summary>
        /// Get List by condition
        /// </summary>
        /// <param name="configCD">Config Code</param>
        /// <param name="configName">Config Name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="sortField">Sort Field</param>
        /// <param name="sortDirec">Sort Direction</param>
        /// <returns>List Setting Info</returns>
        public ApprovalMailInfo GetApprovalMailInfo(int AttendanceId, bool isUseTimeApproval)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine((isUseTimeApproval ? "1" : "0") + " AS IsUseTimeApproval");
            cmdText.AppendLine(" ,DP.DepartmentName");
            cmdText.AppendLine(" ,U.UserName1 AS UserName");
            cmdText.AppendLine(" ,Convert(varchar(10),S.Date,111) AS Date");
            cmdText.AppendLine(" ,Convert(varchar(10),S.ExchangeDate,111) AS ExchangeDate");
            cmdText.AppendLine(" ,S.RequestNote");
            cmdText.AppendLine(" ,EntryTime");
            cmdText.AppendLine(" ,ExitTime");
            cmdText.AppendLine(" ,U2.UserName1 AS ApprovalUserName");
            cmdText.AppendLine(" ,S.ApprovalNote");
            
            cmdText.AppendLine(" ,D1.Value2 AS VacationFull");
            cmdText.AppendLine(" ,D2.Value2 AS VacationMorning");
            cmdText.AppendLine(" ,D3.Value2 AS VacationAfternoon");
            
            cmdText.AppendLine(" ,LateHours");
            cmdText.AppendLine(" ,EarlyHours");

            cmdText.AppendLine(" ,CASE WHEN SH_Hours IS NOT NULL");
            cmdText.AppendLine("       THEN 'SH' ELSE '' END AS SH");

            cmdText.AppendLine(" ,CASE WHEN LH_Hours IS NOT NULL");
            cmdText.AppendLine("       THEN 'LH' ELSE '' END AS LH");

            cmdText.AppendLine(" ,CASE WHEN OverTimeHours1 IS NOT NULL");
            cmdText.AppendLine("         OR OverTimeHours2 IS NOT NULL");
            cmdText.AppendLine("         OR OverTimeHours3 IS NOT NULL");
            cmdText.AppendLine("         OR OverTimeHours4 IS NOT NULL");
            cmdText.AppendLine("         OR OverTimeHours5 IS NOT NULL");
            cmdText.AppendLine("         OR SH_OverTimeHours1 IS NOT NULL");//SH_OT
            cmdText.AppendLine("         OR SH_OverTimeHours2 IS NOT NULL");
            cmdText.AppendLine("         OR SH_OverTimeHours3 IS NOT NULL");
            cmdText.AppendLine("         OR SH_OverTimeHours4 IS NOT NULL");
            cmdText.AppendLine("         OR SH_OverTimeHours5 IS NOT NULL");
            cmdText.AppendLine("         OR LH_OverTimeHours1 IS NOT NULL");//LS_OT
            cmdText.AppendLine("         OR LH_OverTimeHours2 IS NOT NULL");
            cmdText.AppendLine("         OR LH_OverTimeHours3 IS NOT NULL");
            cmdText.AppendLine("         OR LH_OverTimeHours4 IS NOT NULL");
            cmdText.AppendLine("         OR LH_OverTimeHours5 IS NOT NULL");
            cmdText.AppendLine("       THEN 'OT' ELSE '' END AS OT");

            cmdText.AppendLine(" FROM dbo.T_Attendance AS S");
            cmdText.AppendLine(" LEFT JOIN M_User U ON S.UID = U.ID");
            cmdText.AppendLine(" LEFT JOIN M_User U2 ON S.ApprovalUID = U2.ID");

            cmdText.AppendLine(" LEFT JOIN M_Config_H H ON H.ConfigCD = @IN_CONFIG_CD_VACATION_TYPE");
            cmdText.AppendLine(" LEFT JOIN M_Config_D D1 ON D1.HID = H.ID AND D1.Value1 = S.VacationFullCD");
            cmdText.AppendLine(" LEFT JOIN M_Config_D D2 ON D2.HID = H.ID AND D2.Value1 = S.VacationMorningCD");
            cmdText.AppendLine(" LEFT JOIN M_Config_D D3 ON D3.HID = H.ID AND D3.Value1 = S.VacationAfternoonCD");
            
            cmdText.AppendLine(" LEFT JOIN M_WorkingSystem WS ON WS.ID = S.WSID");
            cmdText.AppendLine(" LEFT JOIN M_Department DP ON U.DepartmentID = DP.ID");

            cmdText.AppendLine(" WHERE S.ID = @IN_ID");
            //Para
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_CONFIG_CD_VACATION_TYPE", M_Config_H.CONFIG_CD_VACATION_TYPE.ToString(), true);
            base.AddParam(paras, "IN_ID", AttendanceId, true);

            return this.db.Find1<ApprovalMailInfo>(cmdText.ToString(), paras);
        }
        #endregion

    }
}
