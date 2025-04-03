using System;
using System.Collections;
using System.Collections.Generic;

using OMS.Models;
using OMS.Utilities;
using System.Text;
using System.Linq;

namespace OMS.DAC
{
    public class ProjectProfitService : BaseService
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        private ProjectProfitService()
            : base()
        {
        }

        /// <summary>
        /// Constructor with param
        /// </summary>
        /// <param name="db">Database</param>
        public ProjectProfitService(DB db)
            : base(db)
        {
        }
        #endregion

        #region Get Data

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="projectIDTarget"></param>
        /// <returns></returns>
        public IList<ProjectProfitFullInfo> GetFullListByCondition(DateTime? startDate, DateTime? endDate, int projectIDTarget, int sortField = 2, int sortDirec = 2)
        {
            List<int> lstId = new List<int>();
            lstId.Add(projectIDTarget);
            return GetFullListByCondition(startDate, endDate, lstId, sortField, sortDirec);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="lstProjectIDTarget"></param>
        /// <param name="sortField"></param>
        /// <param name="sortDirec"></param>
        /// <returns></returns>
        public IList<ProjectProfitFullInfo> GetFullListByCondition(DateTime? startDate, DateTime? endDate, List<int> lstProjectIDTarget = null, int sortField = 2, int sortDirec = 2)
        {
            if (lstProjectIDTarget == null || lstProjectIDTarget.Count == 0)
            {
                return new List<ProjectProfitFullInfo>();
            }

            string[] fields = new string[] { "", "ProjectCD", "DepartmentName", "StartDate", "AcceptanceFlag", "OrderAmount" };
            string[] direc = new string[] { "ASC", "DESC" };
            string RowNumber = fields[sortField - 1] + " " + direc[sortDirec - 1];
            if (sortField == 3)
            {
                RowNumber = fields[sortField - 1] + " " + direc[sortDirec - 1] + ", PrUserName " + direc[sortDirec - 1];
            }
            //Parameter
            Hashtable paras = new Hashtable();
            //SQL Project
            StringBuilder cmdTextProject = new StringBuilder();
            cmdTextProject.AppendLine(" WITH Project AS( ");
            cmdTextProject.AppendLine("     SELECT * FROM M_Project Pr");
            cmdTextProject.AppendLine(" WHERE 1 = 1 ");
            cmdTextProject.AppendLine(" AND Pr.ID  IN (" + String.Join(",", lstProjectIDTarget) + ")");

            cmdTextProject.AppendLine(")");

            //SQL String
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine(cmdTextProject.ToString());

            cmdText.AppendLine(" SELECT ");
            cmdText.AppendLine("     Pr.ID AS ProjectID ");
            cmdText.AppendLine("     , Pr.ProjectCD ");
            cmdText.AppendLine("     , Pr.ProjectName ");
            cmdText.AppendLine("     , MUser.DepartmentID ");
            cmdText.AppendLine("     , MDepartment.DepartmentName ");
            cmdText.AppendLine("     , Pr.UserID AS PrUserID");
            cmdText.AppendLine("     , MUser.UserName1 AS PrUserName ");
            cmdText.AppendLine("     , Pr.StartDate ");
            cmdText.AppendLine("     , Pr.EndDate ");
            cmdText.AppendLine("     , Pr.AcceptanceDate ");
            cmdText.AppendLine("     , Pr.AcceptanceFlag ");
            cmdText.AppendLine("     , Pr.OrderAmount ");
            cmdText.AppendLine("     , MUserWork.ID AS WorkUserID ");
            cmdText.AppendLine("     , MUserWork.UserCD AS WorkUserCD ");
            cmdText.AppendLine("     , MUserWork.UserName1 AS WorkUserName ");
            cmdText.AppendLine("     , WorkH.HID ");
            cmdText.AppendLine("     , WorkD.ID AS Work_D_ID ");
            cmdText.AppendLine("     , WorkD.StartTime ");
            cmdText.AppendLine("     , WorkD.EndTime ");
            cmdText.AppendLine("     , ConWorkingSystem.Value3 AS WorkSystemSuu ");
            cmdText.AppendLine("     , Attendance.EntryTime ");
            cmdText.AppendLine("     , Attendance.ExitTime ");
            cmdText.AppendLine("     , WorkingSystem.* ");
            cmdText.AppendLine("     , CostDirect.CostAmount AS DirectCost ");
            cmdText.AppendLine("     , CostInDirect.CostAmount AS IndirectCost ");
            cmdText.AppendLine(" FROM Project Pr ");
            cmdText.AppendLine(" LEFT JOIN M_User MUser ");
            cmdText.AppendLine("     ON Pr.UserID = MUser.ID ");
            cmdText.AppendLine(" LEFT JOIN M_Department MDepartment ");
            cmdText.AppendLine("     ON Pr.DepartmentID = MDepartment.ID ");
            cmdText.AppendLine(" LEFT JOIN T_Work_D WorkD ");
            cmdText.AppendLine("     ON Pr.ID = WorkD.PID ");
            if (startDate.HasValue)
            {
                cmdText.AppendLine("     AND WorkD.Date >= @IN_StartDate ");
                base.AddParam(paras, "IN_StartDate", startDate, true);
            }
            if (endDate.HasValue)
            {
                cmdText.AppendLine("     AND WorkD.Date <= @IN_EndDate ");
                base.AddParam(paras, "IN_EndDate", endDate, true);
            }
            cmdText.AppendLine(" LEFT JOIN T_Work_H WorkH ");
            cmdText.AppendLine("     ON WorkD.HID = WorkH.HID ");
            cmdText.AppendLine(" LEFT JOIN M_User MUserWork ");
            cmdText.AppendLine("     ON WorkH.UID = MUserWork.ID ");
            cmdText.AppendLine(" LEFT JOIN T_Attendance Attendance ");
            cmdText.AppendLine("     ON WorkH.HID = Attendance.ID ");
            cmdText.AppendLine(" LEFT JOIN M_WorkingSystem WorkingSystem ");
            cmdText.AppendLine("     ON Attendance.WSID = WorkingSystem.ID ");
            cmdText.AppendLine(" LEFT JOIN ");
            cmdText.AppendLine("     ( ");
            cmdText.AppendLine("         SELECT D.* FROM M_Config_D D ");
            cmdText.AppendLine("         INNER JOIN M_Config_H H ");
            cmdText.AppendLine("             ON D.HID = H.ID ");
            cmdText.AppendLine("         WHERE H.ConfigCD = @IN_AttendanceConfigCD ");
            base.AddParam(paras, "IN_AttendanceConfigCD", "C001", true);
            cmdText.AppendLine("     ) ConWorkingSystem ");
            cmdText.AppendLine("     ON WorkingSystem.WorkingType = ConWorkingSystem.Value1 ");
            cmdText.AppendLine(" LEFT JOIN ");
            cmdText.AppendLine("     ( ");
            cmdText.AppendLine("         SELECT D.* FROM T_Cost_D D ");
            cmdText.AppendLine("         INNER JOIN T_Cost_H H ");
            cmdText.AppendLine("             ON D.HID = H.ID ");
            cmdText.AppendLine("         WHERE H.CostCode = '01' ");
            cmdText.AppendLine("     ) CostDirect ");
            cmdText.AppendLine("     ON WorkD.Date >= CostDirect.EffectDate ");
            cmdText.AppendLine("         AND WorkD.Date <= CostDirect.ExpireDate ");
            cmdText.AppendLine(" LEFT JOIN ");
            cmdText.AppendLine("     ( ");
            cmdText.AppendLine("         SELECT D.* FROM T_Cost_D D ");
            cmdText.AppendLine("         INNER JOIN T_Cost_H H ");
            cmdText.AppendLine("             ON D.HID = H.ID ");
            cmdText.AppendLine("         WHERE H.CostCode = '02' ");
            cmdText.AppendLine("     ) CostInDirect ");
            cmdText.AppendLine("     ON WorkD.Date >= CostInDirect.EffectDate ");
            cmdText.AppendLine("         AND WorkD.Date <= CostInDirect.ExpireDate ");
            cmdText.AppendLine(" WHERE 1 = 1 ");
            cmdText.AppendLine(string.Format("ORDER BY {0}", RowNumber));

            return this.db.FindList1<ProjectProfitFullInfo>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public decimal GetExpenseProject(int projectID, DateTime? startDate, DateTime? endDate)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            Hashtable paras = new Hashtable();
            cmdText.Append(" SELECT SUM(ExpenceAmount) ");
            cmdText.Append("   FROM T_Expence_H ");
            cmdText.Append("   WHERE ProjectID = @IN_ProjectID ");
            base.AddParam(paras, "IN_ProjectID", projectID);

            if (startDate.HasValue)
            {
                cmdText.AppendLine("     AND Date >= @IN_StartDate ");
                base.AddParam(paras, "IN_StartDate", startDate, true);
            }
            if (endDate.HasValue)
            {
                cmdText.AppendLine("     AND Date <= @IN_EndDate ");
                base.AddParam(paras, "IN_EndDate", endDate, true);
            }

            var Result = this.db.ExecuteScalar1(cmdText.ToString(), paras).ToString();
            return (!string.IsNullOrEmpty(Result) ? decimal.Parse(Result) : 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IList<ProjectProfitDateDetailInfo> GetExpenseDetailProject(int projectID, DateTime? startDate, DateTime? endDate)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            Hashtable paras = new Hashtable();
            cmdText.AppendLine(" SELECT ");
            cmdText.AppendLine(" Expence_D.Date ");
            cmdText.AppendLine(" , MUser.UserName1 AS UserName ");
            cmdText.AppendLine(" , Config_D.Value2 AS Type ");
            cmdText.AppendLine(" , Expence_D.PaidTo ");
            cmdText.AppendLine(" , Expence_H.ApprovedFlag ");

            cmdText.AppendLine(" , CASE WHEN Expence_D.TaxType = 1 THEN Expence_D.Amount + Expence_D.TaxAmount ");
            cmdText.AppendLine("    ELSE Expence_D.Amount ");
            cmdText.AppendLine("    END AS Amount ");

            cmdText.AppendLine("  , CASE WHEN Config_D.Value4 = 1 then Config_D2.Value2 + ': ' + Expence_D.RouteFrom + '～' + Expence_D.RouteTo ");
            cmdText.AppendLine("    ELSE Expence_D.Note ");
            cmdText.AppendLine("    END AS Note ");

            cmdText.AppendLine(" FROM T_Expence_H Expence_H ");
            cmdText.AppendLine(" LEFT JOIN M_User MUser ");
            cmdText.AppendLine("    ON Expence_H.UserID = MUser.ID ");
            cmdText.AppendLine(" LEFT JOIN T_Expence_D Expence_D ");
            cmdText.AppendLine("    ON Expence_H.ID = Expence_D.HID ");

            cmdText.AppendLine(" LEFT JOIN( ");
            cmdText.AppendLine("             SELECT D.* ");
            cmdText.AppendLine("             FROM M_Config_D D ");
            cmdText.AppendLine("             INNER JOIN M_Config_H H ");
            cmdText.AppendLine("            ON D.HID = H.ID ");
            cmdText.AppendLine("            WHERE H.ConfigCD = @IN_ExpenceType ");
            cmdText.AppendLine("            ) Config_D ");
            cmdText.AppendLine("         ON Expence_H.AccountCD = Config_D.Value1 ");

            cmdText.AppendLine(" LEFT JOIN( ");
            cmdText.AppendLine("             SELECT D.* ");
            cmdText.AppendLine("             FROM M_Config_D D ");
            cmdText.AppendLine("             INNER JOIN M_Config_H H ");
            cmdText.AppendLine("             ON D.HID = H.ID ");
            cmdText.AppendLine("             WHERE H.ConfigCD = @IN_RouteType ");
            cmdText.AppendLine("            ) Config_D2 ");
            cmdText.AppendLine("        ON Expence_D.RouteType = Config_D2.Value1 ");

            cmdText.AppendLine(" WHERE Expence_H.ProjectID = @IN_ProjectID ");
            base.AddParam(paras, "IN_ProjectID", projectID);
            if (startDate.HasValue)
            {
                cmdText.AppendLine("     AND Expence_H.Date >= @IN_StartDate ");
                base.AddParam(paras, "IN_StartDate", startDate, true);
            }
            if (endDate.HasValue)
            {
                cmdText.AppendLine("     AND Expence_H.Date <= @IN_EndDate ");
                base.AddParam(paras, "IN_EndDate", endDate, true);
            }
            cmdText.AppendLine(" ORDER BY Expence_H.Date ");

            //Parameter

            base.AddParam(paras, "IN_ExpenceType", M_Config_H.CONFIG_CD_EXPENCE_TYPE);

            base.AddParam(paras, "IN_RouteType", M_Config_H.CONFIG_CD_ROUTE_TYPE);

            base.AddParam(paras, "IN_TaxType", M_Config_H.CONFIG_CD_TAX_TYPE);
            return this.db.FindList1<ProjectProfitDateDetailInfo>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectCD"></param>
        /// <param name="projectName"></param>
        /// <param name="departmentID"></param>
        /// <param name="userID"></param>
        /// <param name="status"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="sortDirec"></param>
        /// <returns></returns>
        public IList<M_Project> GetListProjectByCondition(string projectCD, string projectName, int departmentID,
                                                          int userID, int status, int pageIndex, int pageSize, int sortField, int sortDirec)
        {
            string[] fields = new string[] { "", "ProjectCD", "DepartmentName", "StartDate", "AcceptanceFlag", "OrderAmount" };
            string[] direc = new string[] { "ASC", "DESC" };
            string RowNumber = fields[sortField - 1] + " " + direc[sortDirec - 1];
            if (sortField == 3)
            {
                RowNumber = fields[sortField - 1] + " " + direc[sortDirec - 1] + ", UserName1 " + direc[sortDirec - 1];
            }
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            //Parameter
            Hashtable paras = new Hashtable();
            cmdText.AppendLine(" SELECT Pr.*");
            cmdText.AppendLine(" ," + string.Format("ROW_NUMBER() OVER(ORDER BY {0}) AS RowNumber", RowNumber));
            cmdText.AppendLine(" FROM M_Project Pr ");
            cmdText.AppendLine(" LEFT JOIN M_User MUser ");
            cmdText.AppendLine("     ON Pr.UserID = MUser.ID ");
            cmdText.AppendLine(" LEFT JOIN M_Department MDepartment ");
            cmdText.AppendLine("     ON Pr.DepartmentID = MDepartment.ID ");
            cmdText.AppendLine(" WHERE 1 = 1 ");

            if (!string.IsNullOrEmpty(projectCD))
            {
                cmdText.AppendLine("     AND Pr.ProjectCD LIKE '%' + @IN_ProjectCD + '%' ");
                base.AddParam(paras, "IN_ProjectCD", projectCD, true);
            }
            if (!string.IsNullOrEmpty(projectName))
            {
                cmdText.AppendLine("     AND Pr.ProjectName LIKE '%' + @IN_ProjectName + '%' ");
                base.AddParam(paras, "IN_ProjectName", projectName, true);
            }
            if (departmentID != -1)
            {
                cmdText.AppendLine("     AND Pr.DepartmentID = @IN_DepartmentID ");
                base.AddParam(paras, "IN_DepartmentID", departmentID, true);
            }
            if (userID != -1)
            {
                cmdText.AppendLine("     AND Pr.UserID = @IN_UserID ");
                base.AddParam(paras, "IN_UserID", userID, true);
            }
            if (status != -1)
            {
                cmdText.AppendLine("     AND Pr.AcceptanceFlag = @IN_AcceptanceFlag ");
                base.AddParam(paras, "IN_AcceptanceFlag", status.ToString(), true);
            }

            //SQL OUT
            base.AddParam(paras, "IN_PageIndex", pageIndex);
            base.AddParam(paras, "IN_PageSize", pageSize);

            StringBuilder cmdOutText = new StringBuilder();
            cmdOutText.AppendLine(" SELECT");
            cmdOutText.AppendLine(" *");
            cmdOutText.AppendLine(" FROM");
            cmdOutText.AppendLine(" (");
            cmdOutText.AppendLine(cmdText.ToString());
            cmdOutText.AppendLine(" ) AS  VIEW_1");
            cmdOutText.AppendLine(" WHERE");
            cmdOutText.AppendLine(" VIEW_1.RowNumber BETWEEN(@IN_PageIndex -1) * @IN_PageSize + 1 AND(((@IN_PageIndex -1) * @IN_PageSize + 1) + @IN_PageSize) - 1");
            cmdOutText.AppendLine(" ORDER BY VIEW_1.RowNumber");

            return this.db.FindList1<M_Project>(cmdOutText.ToString(), paras);
        }

        /// <summary>
        /// getTotalRowForList
        /// </summary>
        /// <param name="projectCD"></param>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public int GetTotalRowForProjectList(string projectCD, string projectName, int departmentID, int userID, int status)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            //Parameter
            Hashtable paras = new Hashtable();
            cmdText.AppendLine(" SELECT ");
            cmdText.AppendLine(" COUNT(*)");
            cmdText.AppendLine(" FROM M_Project Pr ");
            cmdText.AppendLine(" WHERE 1 = 1 ");

            if (!string.IsNullOrEmpty(projectCD))
            {
                cmdText.AppendLine("     AND Pr.ProjectCD LIKE '%' + @IN_ProjectCD + '%' ");
                base.AddParam(paras, "IN_ProjectCD", projectCD, true);
            }
            if (!string.IsNullOrEmpty(projectName))
            {
                cmdText.AppendLine("     AND Pr.ProjectName LIKE '%' + @IN_ProjectName + '%' ");
                base.AddParam(paras, "IN_ProjectName", projectName, true);
            }
            if (departmentID != -1)
            {
                cmdText.AppendLine("     AND Pr.DepartmentID = @IN_DepartmentID ");
                base.AddParam(paras, "IN_DepartmentID", departmentID, true);
            }
            if (userID != -1)
            {
                cmdText.AppendLine("     AND Pr.UserID = @IN_UserID ");
                base.AddParam(paras, "IN_UserID", userID, true);
            }
            if (status != -1)
            {
                cmdText.AppendLine("     AND Pr.AcceptanceFlag = @IN_AcceptanceFlag ");
                base.AddParam(paras, "IN_AcceptanceFlag", status.ToString(), true);
            }
            return int.Parse(this.db.ExecuteScalar1(cmdText.ToString(), paras).ToString());
        }

        #endregion

        #region Entity
        #endregion
        #region Method 
        public IList<ProjectProfitInfo> CreateProjectProfitInfoGroupProject(IList<ProjectProfitFullInfo> lstFull, DateTime? startDate, DateTime? endDate, string OT_CONFIG_CD)
        {
            if (lstFull == null || lstFull.Count == 0)
            {
                return new List<ProjectProfitInfo>();
            }

            List<ProjectProfitInfo> lstResult = new List<ProjectProfitInfo>();
            IList<M_Config_D> lstConfigOT;
            using (var db = new DB())
            {
                ProjectProfitService ProfitSV = new ProjectProfitService(db);
                Config_DService ConfigSV = new Config_DService(db);
                lstConfigOT = ConfigSV.GetListByConfigCd(OT_CONFIG_CD);
                decimal OT1_Val = 0;
                decimal.TryParse(lstConfigOT.First(m => m.No == 1).Value3, out OT1_Val);
                decimal OT2_Val = 0;
                decimal.TryParse(lstConfigOT.First(m => m.No == 2).Value3, out OT2_Val);
                decimal OT3_Val = 0;
                decimal.TryParse(lstConfigOT.First(m => m.No == 3).Value3, out OT3_Val);
                decimal OT4_Val = 0;
                decimal.TryParse(lstConfigOT.First(m => m.No == 4).Value3, out OT4_Val);
                decimal OT5_Val = 0;
                decimal.TryParse(lstConfigOT.First(m => m.No == 5).Value3, out OT5_Val);

                var lstProjectID = lstFull.Select(m => m.ProjectID).Distinct().ToList();
                var IntNum = 1;
                foreach (var PrID in lstProjectID)
                {
                    var itemResult = new ProjectProfitInfo();
                    var fstItem = lstFull.First(m => m.ProjectID.Equals(PrID));
                    itemResult.ID = PrID;
                    itemResult.RowNumber = IntNum;
                    IntNum++;
                    itemResult.ProjectCD = fstItem.ProjectCD;
                    itemResult.ProjectName = fstItem.ProjectName;
                    itemResult.DepartmentID = fstItem.DepartmentID;
                    itemResult.DepartmentName = fstItem.DepartmentName;
                    itemResult.UserID = fstItem.PrUserID;
                    itemResult.UserName = fstItem.PrUserName;
                    itemResult.StartDate = fstItem.StartDate;
                    itemResult.ProjectTime = (!fstItem.StartDate.Equals(DateTime.MaxValue) ? fstItem.StartDate.ToString("yyyy/MM/dd") : "") + "～" + (fstItem.EndDate != null ? fstItem.EndDate.Value.ToString("yyyy/MM/dd") : "");
                    itemResult.EndDate = fstItem.EndDate;
                    itemResult.AcceptanceDate = !fstItem.AcceptanceDate.Equals(DateTime.MaxValue) ? fstItem.AcceptanceDate : null;
                    itemResult.AcceptanceFlag = fstItem.AcceptanceFlag;
                    itemResult.OrderAmount = Math.Round(fstItem.OrderAmount, 1);
                    foreach (var itemCost in lstFull.Where(m => m.ProjectID.Equals(PrID)))
                    {
                        var arrBreakTime = new int[] {
                            itemCost.Break1_Start,itemCost.Break1_End,itemCost.Break2_Start,itemCost.Break2_End
                            ,itemCost.Break3_Start,itemCost.Break3_End,itemCost.Break4_Start,itemCost.Break4_End
                        };
                        Dictionary<int, int> DicBreakType2 = new Dictionary<int, int>();
                        if (itemCost.BreakType == 2)
                        {
                            DicBreakType2 = GetDicBreakType2(itemCost);
                        }
                        var TimeWork = GetTimeWork(itemCost, arrBreakTime, DicBreakType2);
                        var TimeOT1 = GetTimeOT(itemCost, 1, arrBreakTime, DicBreakType2);
                        var TimeOT2 = GetTimeOT(itemCost, 2, arrBreakTime, DicBreakType2);
                        var TimeOT3 = GetTimeOT(itemCost, 3, arrBreakTime, DicBreakType2);
                        var TimeOT4 = GetTimeOT(itemCost, 4, arrBreakTime, DicBreakType2);
                        var TimeOT5 = GetTimeOT(itemCost, 5, arrBreakTime, DicBreakType2);
                        var totalTime = TimeWork * itemCost.WorkSystemSuu
                                      + TimeOT1 * OT1_Val
                                      + TimeOT2 * OT2_Val
                                      + TimeOT3 * OT3_Val
                                      + TimeOT4 * OT4_Val
                                      + TimeOT5 * OT5_Val;
                        itemResult.DirectCost += Convert.ToDecimal(totalTime) * itemCost.DirectCost;
                        itemResult.IndirectCosts += Convert.ToDecimal(TimeWork + TimeOT1 + TimeOT2 + TimeOT3 + TimeOT4 + TimeOT5) * itemCost.IndirectCost;
                    }
                    itemResult.DirectCost = itemResult.DirectCost;
                    itemResult.IndirectCosts = itemResult.IndirectCosts;
                    itemResult.Expense = ProfitSV.GetExpenseProject(itemResult.ID, startDate, endDate);
                    itemResult.CostTotal = itemResult.DirectCost + itemResult.IndirectCosts + itemResult.Expense;
                    if (itemResult.OrderAmount != 0)
                    {
                        itemResult.ProfitRate = /*itemResult.CostTotal > itemResult.OrderAmount ? 0 : */(itemResult.OrderAmount - itemResult.CostTotal) / itemResult.OrderAmount;
                    }
                    itemResult.SC_StartDate = startDate;
                    itemResult.SC_EndDate = endDate;
                    lstResult.Add(itemResult);
                }
            }

            return lstResult;
        }

        public IList<ProjectProfitUserDetailInfo> CreateProjectProfitUserDetail(IList<ProjectProfitFullInfo> lstFull, string OT_CONFIG_CD, int projectID)
        {

            IList<ProjectProfitFullInfo> lstFullOfProject = lstFull.Where(m => m.ProjectID.Equals(projectID)).ToList();
            List<ProjectProfitUserDetailInfo> lstResult = new List<ProjectProfitUserDetailInfo>();
            IList<M_Config_D> lstConfigOT;
            decimal OT1_Val = 0;
            decimal OT2_Val = 0;
            decimal OT3_Val = 0;
            decimal OT4_Val = 0;
            decimal OT5_Val = 0;
            using (var db = new DB())
            {
                Config_DService ConfigSV = new Config_DService(db);
                lstConfigOT = ConfigSV.GetListByConfigCd(OT_CONFIG_CD);
                decimal.TryParse(lstConfigOT.First(m => m.No == 1).Value3, out OT1_Val);
                decimal.TryParse(lstConfigOT.First(m => m.No == 2).Value3, out OT2_Val);
                decimal.TryParse(lstConfigOT.First(m => m.No == 3).Value3, out OT3_Val);
                decimal.TryParse(lstConfigOT.First(m => m.No == 4).Value3, out OT4_Val);
                decimal.TryParse(lstConfigOT.First(m => m.No == 5).Value3, out OT5_Val);
            }

            var lstUserID = lstFullOfProject.Select(m => m.WorkUserID).Distinct().ToList();
            foreach (var ItemUserID in lstUserID)
            {
                ProjectProfitUserDetailInfo itemResult = new ProjectProfitUserDetailInfo();
                ProjectProfitFullInfo fstItem = lstFullOfProject.First(m => m.WorkUserID.Equals(ItemUserID));
                itemResult.UserID = ItemUserID;
                itemResult.UserCD = fstItem.WorkUserCD;
                itemResult.UserName = fstItem.WorkUserName;
                foreach (var itemRowUser in lstFullOfProject.Where(m => m.WorkUserID.Equals(ItemUserID)))
                {
                    var arrBreakTime = new int[]
                    {
                        itemRowUser.Break1_Start,itemRowUser.Break1_End,itemRowUser.Break2_Start,itemRowUser.Break2_End
                            ,itemRowUser.Break3_Start,itemRowUser.Break3_End,itemRowUser.Break4_Start,itemRowUser.Break4_End
                    };
                    Dictionary<int, int> DicBreakType2 = new Dictionary<int, int>();
                    if (itemRowUser.BreakType == 2)
                    {
                        DicBreakType2 = GetDicBreakType2(itemRowUser);
                    }
                    var TimeWork = GetTimeWork(itemRowUser, arrBreakTime, DicBreakType2);
                    var TimeOT1 = GetTimeOT(itemRowUser, 1, arrBreakTime, DicBreakType2);
                    var TimeOT2 = GetTimeOT(itemRowUser, 2, arrBreakTime, DicBreakType2);
                    var TimeOT3 = GetTimeOT(itemRowUser, 3, arrBreakTime, DicBreakType2);
                    var TimeOT4 = GetTimeOT(itemRowUser, 4, arrBreakTime, DicBreakType2);
                    var TimeOT5 = GetTimeOT(itemRowUser, 5, arrBreakTime, DicBreakType2);

                    var timeDirect = TimeWork * itemRowUser.WorkSystemSuu;
                    var timeDirectAfter = TimeOT1 * OT1_Val
                                      + TimeOT2 * OT2_Val
                                      + TimeOT3 * OT3_Val
                                      + TimeOT4 * OT4_Val
                                      + TimeOT5 * OT5_Val;

                    itemResult.DirectCost += itemRowUser.WorkingType == (int)WorkingType.WorkFullTime ? (Convert.ToDecimal(timeDirect) * itemRowUser.DirectCost) : 0;
                    itemResult.DirectCostAfter += Convert.ToDecimal(timeDirectAfter) * itemRowUser.DirectCost + itemRowUser.WorkingType != (int)WorkingType.WorkFullTime ? (Convert.ToDecimal(timeDirect) * itemRowUser.DirectCost) : 0;
                    itemResult.IndirectCosts += Convert.ToDecimal(TimeWork + TimeOT1 + TimeOT2 + TimeOT3 + TimeOT4 + TimeOT5) * itemRowUser.IndirectCost;
                }
                itemResult.Total = itemResult.DirectCost + itemResult.DirectCostAfter + itemResult.IndirectCosts;
                lstResult.Add(itemResult);
            }

            return lstResult;
        }

        private static decimal GetTimeWork(ProjectProfitFullInfo record, int[] arrBreak, Dictionary<int, int> DicBreakType2)
        {
            if (record.Working_Start != -1 && record.Working_End != -1)
            {
                if (record.StartTime >= record.Working_End) return 0;//after work
                if (record.EndTime <= record.Working_Start) return 0;//before work
            }
            int StartTime = Math.Max(record.StartTime, record.Working_Start);
            int EndTime = Math.Min(record.EndTime, record.Working_End);
            if (EndTime == -1)
            {
                EndTime = record.EndTime;
            }
            int BreakTime = 0;
            if (record.BreakType == 0)
            {
                BreakTime = GetTimeBreak(StartTime, EndTime, arrBreak);
            }
            else if (record.BreakType == 1)
            {
                if (record.Working_End != record.Working_Start)
                {
                    BreakTime = (int)((decimal)(EndTime - StartTime) * (decimal)arrBreak[0] / (record.Working_End - record.Working_Start));
                }
            }
            else if (record.BreakType == 2)
            {
                foreach (var itemBreak2 in DicBreakType2)
                {
                    if (itemBreak2.Key <= EndTime && itemBreak2.Value >= StartTime)
                    {
                        BreakTime += Math.Min(itemBreak2.Value, EndTime) - Math.Max(itemBreak2.Key, StartTime);
                    }
                }
            }

            int OverTime = 0;
            if (record.OverTime1_Start <= EndTime && record.OverTime1_End >= StartTime)
            {
                OverTime += Math.Min(record.OverTime1_End, EndTime) - Math.Max(record.OverTime1_Start, StartTime);
            }

            if (record.OverTime2_Start <= EndTime && record.OverTime2_End >= StartTime)
            {
                OverTime += Math.Min(record.OverTime2_End, EndTime) - Math.Max(record.OverTime2_Start, StartTime);
            }

            if (record.OverTime3_Start <= EndTime && record.OverTime3_End >= StartTime)
            {
                OverTime += Math.Min(record.OverTime3_End, EndTime) - Math.Max(record.OverTime3_Start, StartTime);
            }

            if (record.OverTime4_Start <= EndTime && record.OverTime4_End >= StartTime)
            {
                OverTime += Math.Min(record.OverTime4_End, EndTime) - Math.Max(record.OverTime4_Start, StartTime);
            }

            if (record.OverTime5_Start <= EndTime && record.OverTime5_End >= StartTime)
            {
                OverTime += Math.Min(record.OverTime5_End, EndTime) - Math.Max(record.OverTime5_Start, StartTime);
            }

            return Utilities.CommonUtil.IntToHours(EndTime - StartTime - BreakTime - OverTime);
        }
        private static decimal GetTimeOT(ProjectProfitFullInfo record, int index, int[] arrBreak, Dictionary<int, int> DicBreakType2)
        {
            var WS_OT_StartTime = 0;
            var WS_OT_EndTime = 0;
            switch (index)
            {
                case 1:
                    WS_OT_StartTime = record.OverTime1_Start;
                    WS_OT_EndTime = record.OverTime1_End;
                    break;
                case 2:
                    WS_OT_StartTime = record.OverTime2_Start;
                    WS_OT_EndTime = record.OverTime2_End;
                    break;
                case 3:
                    WS_OT_StartTime = record.OverTime3_Start;
                    WS_OT_EndTime = record.OverTime3_End;
                    break;
                case 4:
                    WS_OT_StartTime = record.OverTime4_Start;
                    WS_OT_EndTime = record.OverTime4_End;
                    break;
                case 5:
                    WS_OT_StartTime = record.OverTime5_Start;
                    WS_OT_EndTime = record.OverTime5_End;
                    break;
            }
            if (WS_OT_StartTime != -1 && WS_OT_EndTime != -1)
            {
                if (WS_OT_StartTime <= record.EndTime && WS_OT_EndTime >= record.StartTime)
                {
                    int OT_StartTime_Result = Math.Max(record.StartTime, WS_OT_StartTime);
                    int OT_EndTime_Result = Math.Min(record.EndTime, WS_OT_EndTime);
                    int BreakTime = 0;
                    if (record.BreakType == 0)
                    {
                        BreakTime = GetTimeBreak(OT_StartTime_Result, OT_EndTime_Result, arrBreak);
                    }
                    else if (record.BreakType == 1)
                    {
                        BreakTime = (int)((decimal)(OT_EndTime_Result - OT_StartTime_Result) * (decimal)arrBreak[0] / (record.Working_End - record.Working_Start));
                    }
                    else if (record.BreakType == 2)
                    {
                        foreach (var itemBreak2 in DicBreakType2)
                        {
                            if (itemBreak2.Key <= OT_EndTime_Result && itemBreak2.Value >= OT_StartTime_Result)
                            {
                                BreakTime += Math.Min(itemBreak2.Value, OT_EndTime_Result) - Math.Max(itemBreak2.Key, OT_StartTime_Result);
                            }
                        }
                    }
                    return Utilities.CommonUtil.IntToHours(OT_EndTime_Result - OT_StartTime_Result - BreakTime);
                }
            }

            return 0;

        }
        private static int GetTimeBreak(int StartTime, int EndTime, int[] arrBreak)
        {
            var Br_St_1 = arrBreak[0];
            var Br_En_1 = arrBreak[1];
            var Br_St_2 = arrBreak[2];
            var Br_En_2 = arrBreak[3];
            var Br_St_3 = arrBreak[4];
            var Br_En_3 = arrBreak[5];
            var Br_St_4 = arrBreak[6];
            var Br_En_4 = arrBreak[7];

            var Result_Br1 = 0;
            if (Br_St_1 != -1 && Br_En_1 != -1)
            {
                if (Br_St_1 <= EndTime && Br_En_1 >= StartTime)
                {
                    Result_Br1 = Math.Min(EndTime, Br_En_1) - Math.Max(StartTime, Br_St_1);
                }
            }
            var Result_Br2 = 0;
            if (Br_St_2 != -1 && Br_En_2 != -1)
            {
                if (Br_St_2 <= EndTime && Br_En_2 >= StartTime)
                {
                    Result_Br2 = Math.Min(EndTime, Br_En_2) - Math.Max(StartTime, Br_St_2);
                }
            }
            var Result_Br3 = 0;
            if (Br_St_3 != -1 && Br_En_3 != -1)
            {
                if (Br_St_3 <= EndTime && Br_En_3 >= StartTime)
                {
                    Result_Br3 = Math.Min(EndTime, Br_En_3) - Math.Max(StartTime, Br_St_3);
                }
            }
            var Result_Br4 = 0;
            if (Br_St_4 != -1 && Br_En_4 != -1)
            {
                if (Br_St_4 <= EndTime && Br_En_4 >= StartTime)
                {
                    Result_Br4 = Math.Min(EndTime, Br_En_4) - Math.Max(StartTime, Br_St_4);
                }
            }
            return Result_Br1 + Result_Br2 + Result_Br3 + Result_Br4;
        }

        private Dictionary<int, int> GetDicBreakType2(ProjectProfitFullInfo record)
        {
            var Result = new Dictionary<int, int>();
            if (record.Break1_Start <= 0)
            {
                return Result;
            }
            var timeTemp = record.EntryTime;
            while (timeTemp < record.ExitTime)
            {
                timeTemp += record.Break1_Start;
                if (timeTemp < record.ExitTime)
                {
                    if (timeTemp + record.Break1_End <= record.ExitTime)
                    {
                        Result.Add(timeTemp, timeTemp + record.Break1_End);
                        timeTemp += record.Break1_End;
                    }
                    else
                    {
                        Result.Add(timeTemp, record.ExitTime);
                        timeTemp += record.ExitTime;
                    }
                }
            }
            return Result;
        }
        #endregion
    }
}
