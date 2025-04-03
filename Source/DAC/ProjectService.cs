using System;
using System.Collections;
using System.Collections.Generic;

using OMS.Models;
using OMS.Utilities;
using System.Text;

namespace OMS.DAC
{
    /// Class ProjectService DAC
    /// Create Date: 2017/10/05
    /// Create Author: ISV-Than
    public class ProjectService : BaseService
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        private ProjectService()
            : base()
        {
        }

        /// <summary>
        /// Constructor with param
        /// </summary>
        /// <param name="db">Database</param>
        public ProjectService(DB db)
            : base(db)
        {
        }
        #endregion

        #region Get Data


        public IList<ProjectInfo> GetListByCond(string projectCD, string projectName, string workPlace,
                                                DateTime? startDate1, DateTime? startDate2,
                                                DateTime? endDate1, DateTime? endDate2,
                                                string inValid, int AcceptanceFlag, int departmentID, int userID,
                                                int pageIndex, int pageSize, int sortField, int sortDirec)
        {
            string[] fields = new string[] { "UpdateDate", "", "ProjectCD", "ProjectName", "UserName1", "OrderAmount", "StartDate", "AcceptanceFlag", "Memo" };
            string[] direc = new string[] { "ASC", "DESC" };

            string RowNumber = fields[sortField - 1] + " " + direc[sortDirec - 1];
            if (sortField == 1)
            {
                RowNumber = fields[sortField - 1] + " " + direc[1];
            }
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();
            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" pr.ID");
            cmdText.AppendLine(" ,pr.ProjectCD");
            cmdText.AppendLine(" ,pr.ProjectName");
            cmdText.AppendLine(" ,pr.UserID");
            cmdText.AppendLine(" ,MUser.UserName1 AS UserName");
            cmdText.AppendLine(" ,pr.DepartmentID");
            cmdText.AppendLine(" ,Department.DepartmentName");
            cmdText.AppendLine(" ,pr.OrderAmount");
            cmdText.AppendLine(" ,pr.AcceptanceFlag");
            cmdText.AppendLine(" ,pr.WorkPlace");
            cmdText.AppendLine(" ,pr.Memo");
            cmdText.AppendLine(" ,pr.StartDate");
            cmdText.AppendLine(" ,pr.EndDate");
            cmdText.AppendLine(" ,pr.StatusFlag");

            cmdText.AppendLine(" ," + string.Format("ROW_NUMBER() OVER(ORDER BY {0}) AS RowNumber", RowNumber));
            cmdText.AppendLine(" FROM dbo.M_Project AS pr");
            cmdText.AppendLine(" LEFT JOIN M_User AS MUser");
            cmdText.AppendLine("    ON pr.UserID=MUser.ID");
            cmdText.AppendLine(" LEFT JOIN M_Department AS Department");
            cmdText.AppendLine("    ON pr.DepartmentID=Department.ID");

            //Parameter
            Hashtable paras = new Hashtable();

            if (!string.IsNullOrEmpty(projectCD))
            {
                cmdWhere.AppendLine("AND pr.ProjectCD LIKE '%' + @IN_ProjectCD + '%'");
                base.AddParam(paras, "IN_ProjectCD", projectCD, true);
            }

            if (!string.IsNullOrEmpty(projectName))
            {
                cmdWhere.AppendLine("AND pr.ProjectName LIKE '%' + @IN_ProjectName + '%'");
                base.AddParam(paras, "IN_ProjectName", projectName, true);
            }

            if (!string.IsNullOrEmpty(workPlace))
            {
                cmdWhere.AppendLine("AND pr.WorkPlace LIKE '%' + @IN_WorkPlace + '%'");
                base.AddParam(paras, "IN_WorkPlace", workPlace, true);
            }

            if ((!string.IsNullOrEmpty(startDate1.ToString())) && (!string.IsNullOrEmpty(startDate2.ToString())))
            {
                cmdWhere.AppendLine("AND (pr.StartDate  >= @IN_StartDate1 AND pr.StartDate  <= @IN_StartDate2) ");
                base.AddParam(paras, "IN_StartDate1", startDate1, true);
                base.AddParam(paras, "IN_StartDate2", startDate2, true);
            }
            else if ((!string.IsNullOrEmpty(startDate1.ToString())) && (string.IsNullOrEmpty(startDate2.ToString())))
            {
                cmdWhere.AppendLine("AND (pr.StartDate  >= @IN_StartDate1) ");
                base.AddParam(paras, "IN_StartDate1", startDate1, true);
            }
            else if ((string.IsNullOrEmpty(startDate1.ToString())) && (!string.IsNullOrEmpty(startDate2.ToString())))
            {
                cmdWhere.AppendLine("AND (pr.StartDate  <= @IN_StartDate2) ");
                base.AddParam(paras, "IN_StartDate2", startDate2, true);
            }

            if ((!string.IsNullOrEmpty(endDate1.ToString())) && (!string.IsNullOrEmpty(endDate2.ToString())))
            {
                cmdWhere.AppendLine("AND (pr.EndDate  >= @IN_EndDate1 AND pr.EndDate  <= @IN_EndDate2) ");
                base.AddParam(paras, "IN_EndDate1", endDate1, true);
                base.AddParam(paras, "IN_EndDate2", endDate2, true);
            }
            else if ((!string.IsNullOrEmpty(endDate1.ToString())) && (string.IsNullOrEmpty(endDate2.ToString())))
            {
                cmdWhere.AppendLine("AND (pr.EndDate  >= @IN_EndDate1) ");
                base.AddParam(paras, "IN_EndDate1", endDate1, true);
            }
            else if ((string.IsNullOrEmpty(endDate1.ToString())) && (!string.IsNullOrEmpty(endDate2.ToString())))
            {
                cmdWhere.AppendLine("AND (pr.EndDate  <= @IN_EndDate2) ");
                base.AddParam(paras, "IN_EndDate2", endDate2, true);
            }

            if (AcceptanceFlag != -1)
            {
                cmdWhere.AppendLine("AND AcceptanceFlag= @IN_AcceptanceFlag");
                base.AddParam(paras, "IN_AcceptanceFlag", AcceptanceFlag, false);
            }
            if (departmentID != -1)
            {
                cmdWhere.AppendLine("AND pr.DepartmentID= @IN_DepartmentID");
                base.AddParam(paras, "IN_DepartmentID", departmentID, true);
            }
            if (userID != -1)
            {
                cmdWhere.AppendLine("AND UserID= @IN_UserID");
                base.AddParam(paras, "IN_UserID", userID, true);
            }

            cmdWhere.AppendLine("AND ((@IN_InValid = 0 AND pr.StatusFlag = 0) OR (@IN_InValid = 2) OR (@IN_InValid = 1 AND pr.StatusFlag = 1)) ");
            base.AddParam(paras, "IN_InValid", inValid);

            //Check SQL
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE");

                cmdWhere = cmdWhere.Replace("AND", "", 0, 3);
                cmdText.AppendLine(cmdWhere.ToString());
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
            return this.db.FindList1<ProjectInfo>(cmdOutText.ToString(), paras);
        }


        /// <summary>
        /// Get Data Project
        /// </summary>
        /// <returns></returns>
        public IList<M_Project> GetAll()
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();
            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" *");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.M_Project");

            return this.db.FindList1<M_Project>(cmdText.ToString());
        }

        /// <summary>
        /// getTotalRowForList
        /// </summary>
        /// <param name="projectCD"></param>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public int getTotalRowForList(string projectCD, string projectName, string workPlace,
                                      DateTime? startDate1, DateTime? startDate2,
                                      DateTime? endDate1, DateTime? endDate2,
                                      string inValid, int AcceptanceFlag, int departmentID, int userID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();
            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" COUNT(*)");
            cmdText.AppendLine(" FROM dbo.M_Project AS pr");
            //Parameter
            Hashtable paras = new Hashtable();
            if (!string.IsNullOrEmpty(projectCD))
            {
                cmdWhere.AppendLine("AND pr.ProjectCD LIKE '%' +  @IN_ProjectCD + '%'");
                base.AddParam(paras, "IN_ProjectCD", projectCD, true);
            }

            if (!string.IsNullOrEmpty(projectName))
            {
                cmdWhere.AppendLine("AND pr.ProjectName LIKE '%' + @IN_ProjectName + '%'");
                base.AddParam(paras, "IN_ProjectName", projectName, true);
            }

            if (!string.IsNullOrEmpty(workPlace))
            {
                cmdWhere.AppendLine("AND pr.WorkPlace LIKE '%' + @IN_WorkPlace + '%'");
                base.AddParam(paras, "IN_WorkPlace", workPlace, true);
            }

            if (!string.IsNullOrEmpty(startDate1.ToString()) && (!string.IsNullOrEmpty(startDate2.ToString())))
            {
                cmdWhere.AppendLine("AND (pr.StartDate  >= @IN_StartDate1 AND pr.StartDate  <= @IN_StartDate2) ");
                base.AddParam(paras, "IN_StartDate1", startDate1, true);
                base.AddParam(paras, "IN_StartDate2", startDate2, true);
            }
            else if ((!string.IsNullOrEmpty(startDate1.ToString())) && (string.IsNullOrEmpty(startDate2.ToString())))
            {
                cmdWhere.AppendLine("AND (pr.StartDate  >= @IN_StartDate1) ");
                base.AddParam(paras, "IN_StartDate1", startDate1, true);
            }
            else if ((string.IsNullOrEmpty(startDate1.ToString())) && (!string.IsNullOrEmpty(startDate2.ToString())))
            {
                cmdWhere.AppendLine("AND (pr.StartDate  <= @IN_StartDate2) ");
                base.AddParam(paras, "IN_StartDate2", startDate2, true);
            }

            if (!string.IsNullOrEmpty(endDate1.ToString()) && (!string.IsNullOrEmpty(endDate2.ToString())))
            {
                cmdWhere.AppendLine("AND (pr.EndDate  >= @IN_EndDate1 AND pr.EndDate  <= @IN_EndDate2) ");
                base.AddParam(paras, "IN_EndDate1", endDate1, true);
                base.AddParam(paras, "IN_EndDate2", endDate2, true);
            }
            else if ((!string.IsNullOrEmpty(endDate1.ToString())) && (string.IsNullOrEmpty(endDate2.ToString())))
            {
                cmdWhere.AppendLine("AND (pr.EndDate  >= @IN_EndDate1) ");
                base.AddParam(paras, "IN_EndDate1", endDate1, true);
            }
            else if ((string.IsNullOrEmpty(endDate1.ToString())) && (!string.IsNullOrEmpty(endDate2.ToString())))
            {
                cmdWhere.AppendLine("AND (pr.EndDate  <= @IN_EndDate2) ");
                base.AddParam(paras, "IN_EndDate2", endDate2, true);
            }
            if (AcceptanceFlag != -1)
            {
                cmdWhere.AppendLine("AND pr.AcceptanceFlag= @IN_AcceptanceFlag");
                base.AddParam(paras, "IN_AcceptanceFlag", AcceptanceFlag, false);
            }
            if (departmentID != -1)
            {
                cmdWhere.AppendLine("AND pr.DepartmentID= @IN_DepartmentID");
                base.AddParam(paras, "IN_DepartmentID", departmentID, true);
            }
            if (userID != -1)
            {
                cmdWhere.AppendLine("AND pr.UserID= @IN_UserID");
                base.AddParam(paras, "IN_UserID", userID, true);
            }

            cmdWhere.AppendLine("AND ((@IN_InValid = 0 AND pr.StatusFlag = 0) OR (@IN_InValid = 2) OR (@IN_InValid = 1 AND pr.StatusFlag = 1)) ");
            base.AddParam(paras, "IN_InValid", inValid);

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE");
                cmdWhere = cmdWhere.Replace("AND", "", 0, 3);
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return int.Parse(this.db.ExecuteScalar1(cmdText.ToString(), paras).ToString());
        }

        /// <summary>
        /// Get List By Condition For Search
        /// </summary>
        /// <param name="projectCD"></param>
        /// <param name="projectName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="sortDirec"></param>
        /// <returns></returns>
        public IList<ProjectSearchInfo> GetListByConditionForSearch(string projectCD, string projectName, string initDate,
                                                                        int pageIndex, int pageSize, int sortField, int sortDirec)
        {
            string[] fields = new string[] { "UpdateDate", "", "ProjectCD", "ProjectName" };
            string[] direc = new string[] { "ASC", "DESC" };

            string RowNumber = fields[sortField - 1] + " " + direc[sortDirec - 1];
            if (sortField == 1)
            {
                RowNumber = fields[sortField - 1] + " " + direc[1];
            }

            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  G.*");
            cmdText.AppendLine(" ," + string.Format("ROW_NUMBER() OVER(ORDER BY {0}) AS RowNumber", RowNumber));
            cmdText.AppendLine(" FROM dbo.M_Project AS G");

            cmdWhere.AppendLine(" ID >= 10");
            cmdWhere.AppendLine(" AND StatusFlag = 0");
            cmdWhere.AppendLine(" AND (EndDate IS NULL OR EndDate >= @IN_initDate)");

            //Para
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_initDate", initDate);

            if (!(string.IsNullOrEmpty(projectCD)))
            {
                cmdWhere.AppendLine(" AND UPPER(G.ProjectCD) LIKE UPPER(@IN_ProjectCD) + '%'");
                base.AddParam(paras, "IN_ProjectCD", projectCD, true);
            }

            if (!(string.IsNullOrEmpty(projectName)))
            {
                cmdWhere.AppendLine(" AND (UPPER(G.ProjectName) LIKE '%' + UPPER(@IN_ProjectName) + '%' OR @IN_ProjectName IS NULL)");
                base.AddParam(paras, "IN_ProjectName", projectName, true);
            }

            //Check SQL
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            //SQL OUT
            StringBuilder cmdOutText = new StringBuilder();

            cmdOutText.AppendLine(" SELECT");
            cmdOutText.AppendLine(" *");
            cmdOutText.AppendLine(" FROM");
            cmdOutText.AppendLine(" (");
            cmdOutText.AppendLine(cmdText.ToString());
            cmdOutText.AppendLine(" ) AS VIEW_1");
            cmdOutText.AppendLine(" WHERE");
            cmdOutText.AppendLine(" RowNumber BETWEEN(@IN_PageIndex -1) * @IN_PageSize + 1 AND(((@IN_PageIndex -1) * @IN_PageSize + 1) + @IN_PageSize) - 1");
            cmdOutText.AppendLine(" ORDER BY RowNumber");

            base.AddParam(paras, "IN_PageIndex", pageIndex);
            base.AddParam(paras, "IN_PageSize", pageSize);
            base.AddParam(paras, "IN_SortField", sortField);
            base.AddParam(paras, "IN_SortDirec", sortDirec);
            return this.db.FindList1<ProjectSearchInfo>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Get Count By Condition For Search
        /// </summary>
        /// <param name="projectCD"></param>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public int GetCountByConditionForSearch(string projectCD, string projectName, string initDate)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  count(*)");
            cmdText.AppendLine(" FROM dbo.M_Project AS G");

            //Para

            cmdWhere.AppendLine(" ID >= 10");
            cmdWhere.AppendLine(" AND StatusFlag = 0");
            cmdWhere.AppendLine(" AND (EndDate IS NULL OR EndDate >= @IN_initDate)");

            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_initDate", initDate, true);

            if (!string.IsNullOrEmpty(projectCD))
            {
                cmdWhere.AppendLine(" AND UPPER(G.ProjectCD) LIKE UPPER(@IN_ProjectCD) + '%'");
                base.AddParam(paras, "IN_ProjectCD", projectCD, true);
            }

            if (!string.IsNullOrEmpty(projectName))
            {
                cmdWhere.AppendLine(" AND UPPER(G.ProjectName) LIKE '%' + UPPER(@IN_ProjectName) + '%'	");
                base.AddParam(paras, "IN_ProjectName", projectName, true);
            }

            //Check SQL
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return int.Parse(this.db.ExecuteScalar1(cmdText.ToString(), paras).ToString());
        }

        /// <summary>
        /// Get user Combobox data by department id with Status=0 OR UserID
        /// </summary>
        /// <param name="departmentID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public IList<DropDownModel> GetCbbUserDataByDepartmentIDAndUserID(int departmentID, int UserID, bool isGroup = false)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            //Parameter
            Hashtable paras = new Hashtable();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  U.ID AS Value");
            cmdText.AppendLine("  ,U.UserName1 AS DisplayName");
            cmdText.AppendLine("  ,U.StatusFlag AS StatusFlag");
            cmdText.AppendLine(" FROM dbo.M_User AS U");
            cmdText.AppendLine(" LEFT JOIN  dbo.M_GroupUser_H AS G ON G.ID = U.GroupID");

            cmdWhere.AppendLine(" U.UserCD NOT IN(SELECT Value3 FROM M_Config_D DD WHERE  EXISTS(SELECT ID FROM M_Config_H HH WHERE HH.ConfigCD=@IN_GROUPADMIN AND HH.ID=DD.HID)) ");

            base.AddParam(paras, "IN_GROUPADMIN", M_Config_H.CONFIG_GROUP_USERCD_ADMIN);

            if (departmentID != -1)
            {
                cmdWhere.AppendLine(" AND U.DepartmentID = @IN_DepartmentID");

                //Add param
                base.AddParam(paras, "IN_DepartmentID", departmentID);
            }
            if (UserID != -1)
            {
                cmdWhere.AppendLine("AND (U.StatusFlag = 0 OR U.ID=@IN_UserID)");
                //Add param
                base.AddParam(paras, "IN_UserID", UserID);
            }
            else
            {
                cmdWhere.AppendLine("AND U.StatusFlag = 0");
            }

            if (isGroup)
            {
                cmdWhere.AppendLine("AND G.GroupCD IN ( '" + M_GroupUser_H.DB_GROUP_CODE_1 + "','" + M_GroupUser_H.DB_GROUP_CODE_3 + "')");
            }

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }
            cmdText.AppendLine(" ORDER BY U.UserCD");
            return this.db.FindList1<DropDownModel>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Get Data Project
        /// </summary>
        /// <returns></returns>
        public IList<ProjectExcelInfo> GetListForExcel()
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            //Parameter
            Hashtable paras = new Hashtable();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  pr.ProjectCD");
            cmdText.AppendLine(" ,pr.ProjectName");
            cmdText.AppendLine(" ,pr.Memo");

            cmdText.AppendLine(" FROM dbo.M_Project AS pr");

            cmdText.AppendLine(" WHERE pr.StartDate  >= @IN_StartDate");
            base.AddParam(paras, "IN_StartDate", DateTime.Now.AddYears(-2).Date, true);

            cmdText.AppendLine(" ORDER BY pr.ProjectCD");


            return this.db.FindList1<ProjectExcelInfo>(cmdText.ToString(), paras);
        }

        #endregion

        #region Entity
        /// <summary>
        /// Get By M_Project Cd
        /// </summary>
        /// <returns>M_Project</returns>
        public M_Project GetByProjectCd(string projectCd)
        {
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  ID");

            cmdText.AppendLine(" ,ProjectCD ");
            cmdText.AppendLine(" ,ProjectName ");
            cmdText.AppendLine(" ,WorkPlace ");
            cmdText.AppendLine(" ,Memo ");
            cmdText.AppendLine(" ,StartDate ");
            cmdText.AppendLine(" ,EndDate ");

            cmdText.AppendLine(" ,DepartmentID ");
            cmdText.AppendLine(" ,UserID ");
            cmdText.AppendLine(" ,DeliveryDate ");
            cmdText.AppendLine(" ,AcceptanceDate ");
            cmdText.AppendLine(" ,AcceptanceFlag ");
            cmdText.AppendLine(" ,OrderAmount ");

            cmdText.AppendLine(" ,StatusFlag ");

            cmdText.AppendLine(" ,CreateDate ");
            cmdText.AppendLine(" ,CreateUID ");
            cmdText.AppendLine(" ,UpdateDate ");
            cmdText.AppendLine(" ,UpdateUID ");

            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.M_Project");

            //Para
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine(" RTRIM(ProjectCD) = RTRIM(@IN_ProjectCD)");
            base.AddParam(paras, "IN_ProjectCD", projectCd);
            cmdWhere.AppendLine(" AND ID >= 10");

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.Find1<M_Project>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Get Data Project By Id
        /// </summary>
        /// <param name="projectId">projectId</param>
        /// <returns></returns>
        public M_Project GetDataProjectById(int projectId)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  ID");

            cmdText.AppendLine(" ,ProjectCD ");
            cmdText.AppendLine(" ,ProjectName ");
            cmdText.AppendLine(" ,WorkPlace ");
            cmdText.AppendLine(" ,Memo ");
            cmdText.AppendLine(" ,StartDate ");
            cmdText.AppendLine(" ,EndDate ");
            cmdText.AppendLine(" ,DepartmentID ");
            cmdText.AppendLine(" ,UserID ");
            cmdText.AppendLine(" ,DeliveryDate ");
            cmdText.AppendLine(" ,AcceptanceDate ");
            cmdText.AppendLine(" ,AcceptanceFlag ");
            cmdText.AppendLine(" ,OrderAmount ");

            cmdText.AppendLine(" ,StatusFlag ");

            cmdText.AppendLine(" ,CreateDate ");
            cmdText.AppendLine(" ,CreateUID ");
            cmdText.AppendLine(" ,UpdateDate ");
            cmdText.AppendLine(" ,UpdateUID ");

            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.M_Project");

            cmdWhere.AppendLine(" ID = @IN_ProjectID");

            //Para
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_ProjectID", projectId);

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.Find1<M_Project>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Get Default Project Code
        /// </summary>
        /// <param name="configCD">projectId</param>
        /// <returns></returns>
        public string GetDefaultProjectCode(string configCD)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine(" WITH getYears AS( ");
            cmdText.AppendLine("        SELECT TOP 1 DATEDIFF(yy, d.Value2, GETDATE()) - ");
            cmdText.AppendLine("                    CASE ");
            cmdText.AppendLine("                        WHEN MONTH(d.Value2) > MONTH(GETDATE()) THEN 1 ");
            cmdText.AppendLine("                        WHEN MONTH(d.Value2) < MONTH(GETDATE()) THEN 0 ");
            cmdText.AppendLine("                        WHEN DAY(d.Value2) > DAY(GETDATE()) THEN 1 ");
            cmdText.AppendLine("                        ELSE 0 ");
            cmdText.AppendLine("                    END AS Years ");
            cmdText.AppendLine("                  , d.Value3 ");
            cmdText.AppendLine("        FROM M_Config_H h ");
            cmdText.AppendLine("        INNER JOIN M_Config_D d ");
            cmdText.AppendLine("        ON h.ID = d.HID ");
            cmdText.AppendLine("        AND h.ConfigCD = @IN_ConfigCD), ");
            cmdText.AppendLine("    getCode AS( ");
            cmdText.AppendLine("        SELECT RIGHT(REPLICATE('0', LEN(Value3)) + CAST(Value3 + Years AS nvarchar), LEN(Value3)) AS code ");
            cmdText.AppendLine("        FROM getYears), ");
            cmdText.AppendLine("    getMaxCD AS(");
            cmdText.AppendLine("        SELECT getCode.code, ");
            cmdText.AppendLine("        MAX(CAST(SUBSTRING(ProjectCD, CHARINDEX('-', ProjectCD) + 1,LEN(ProjectCD)) AS int)) AS ProjectCD ");
            cmdText.AppendLine("        FROM getCode ");
            cmdText.AppendLine("        LEFT JOIN M_Project ");
            cmdText.AppendLine("        ON  SUBSTRING(ProjectCD, CHARINDEX('-', ProjectCD) + 1, LEN(ProjectCD)) NOT LIKE '%[^0-9]%' ");
            cmdText.AppendLine("        AND ProjectCD LIKE getCode.code + '-%' ");
            cmdText.AppendLine("        GROUP BY getCode.code ");
            cmdText.AppendLine("    ) ");

            cmdText.AppendLine(" SELECT code + '-' +  ");
            cmdText.AppendLine("        ISNULL( ");
            cmdText.AppendLine("        CASE ");
            cmdText.AppendLine("            WHEN LEN(ProjectCD + 1) < 3 ");
            cmdText.AppendLine("                THEN RIGHT(REPLICATE('0', 3) + CAST(ProjectCD + 1 AS nvarchar), 3) ");
            cmdText.AppendLine("            ELSE CAST(ProjectCD + 1 AS nvarchar) ");
            cmdText.AppendLine("        END");
            cmdText.AppendLine("        ,'001' ) AS ProjectCD ");
            cmdText.AppendLine(" FROM getMaxCD ");

            //Para
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_ConfigCD", configCD);

            return this.db.ExecuteScalar1(cmdText.ToString(), paras).ToString();
        }

        ///// <summary>
        ///// Get DepartmentID By UserId
        ///// </summary>
        ///// <param name="userID">userID</param>
        ///// <returns></returns>
        //public M_Department GetDepartmentIDByUserId(string userID)
        //{
        //    //SQL String
        //    StringBuilder cmdText = new StringBuilder();
        //    StringBuilder cmdWhere = new StringBuilder();

        //    cmdText.AppendLine(" SELECT");
        //    cmdText.AppendLine("  D.* ");

        //    cmdText.AppendLine(" FROM ");
        //    cmdText.AppendLine(" dbo.M_User AS U ");

        //    cmdText.AppendLine(" INNER JOIN dbo.M_Department AS D ");
        //    cmdText.AppendLine(" ON U.DepartmentID = D.ID ");

        //    cmdWhere.AppendLine(" U.ID = @IN_UserID ");

        //    //Para
        //    Hashtable paras = new Hashtable();
        //    base.AddParam(paras, "IN_UserID", userID);

        //    if (cmdWhere.Length != 0)
        //    {
        //        cmdText.AppendLine(" WHERE ");
        //        cmdText.AppendLine(cmdWhere.ToString());
        //        cmdText.AppendLine(" ORDER BY U.DepartmentID ");
        //    }

        //    return this.db.Find1<M_Department>(cmdText.ToString(), paras);
        //}

        #endregion

        #region Insert
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="user">M_Project</param>
        /// <returns></returns>
        public int Insert(M_Project m_Project)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine(" INSERT INTO dbo.M_Project");
            cmdText.AppendLine(" (");

            cmdText.AppendLine("  ProjectCD ");
            cmdText.AppendLine(" ,ProjectName ");
            cmdText.AppendLine(" ,WorkPlace ");

            cmdText.AppendLine(" ,StartDate ");
            cmdText.AppendLine(" ,EndDate ");

            cmdText.AppendLine(" ,DepartmentID ");
            cmdText.AppendLine(" ,UserID ");
            cmdText.AppendLine(" ,DeliveryDate ");
            cmdText.AppendLine(" ,AcceptanceDate ");
            cmdText.AppendLine(" ,AcceptanceFlag ");
            cmdText.AppendLine(" ,OrderAmount ");

            cmdText.AppendLine(" ,Memo ");
            cmdText.AppendLine(" ,StatusFlag ");

            cmdText.AppendLine(" ,CreateDate ");
            cmdText.AppendLine(" ,CreateUID ");
            cmdText.AppendLine(" ,UpdateDate ");
            cmdText.AppendLine(" ,UpdateUID ");

            cmdText.AppendLine(" )");
            cmdText.AppendLine(" VALUES");
            cmdText.AppendLine(" (");

            cmdText.AppendLine("  @IN_ProjectCD ");
            cmdText.AppendLine(" ,@IN_ProjectName ");
            cmdText.AppendLine(" ,@IN_WorkPlace ");
            cmdText.AppendLine(" ,@IN_StartDate ");
            cmdText.AppendLine(" ,@IN_EndDate ");

            cmdText.AppendLine(" ,@IN_DepartmentID ");
            cmdText.AppendLine(" ,@IN_UserID ");
            cmdText.AppendLine(" ,@IN_DeliveryDate ");
            cmdText.AppendLine(" ,@IN_AcceptanceDate ");
            cmdText.AppendLine(" ,@IN_AcceptanceFlag ");
            cmdText.AppendLine(" ,@IN_OrderAmount ");

            cmdText.AppendLine(" ,@IN_Memo ");
            cmdText.AppendLine(" ,@IN_StatusFlag ");

            cmdText.AppendLine(" ,GETDATE() ");
            cmdText.AppendLine(" ,@IN_CreateUID ");
            cmdText.AppendLine(" ,GETDATE() ");
            cmdText.AppendLine(" ,@IN_UpdateUID ");
            cmdText.AppendLine(" )");

            //Para
            Hashtable paras = new Hashtable();

            base.AddParam(paras, "IN_ProjectCD", m_Project.ProjectCD);
            base.AddParam(paras, "IN_ProjectName", m_Project.ProjectName);
            base.AddParam(paras, "IN_WorkPlace", m_Project.WorkPlace);

            base.AddParam(paras, "IN_StartDate", m_Project.StartDate, true);
            if (m_Project.EndDate != null)
            {
                base.AddParam(paras, "IN_EndDate", m_Project.EndDate, true);
            }
            else
            {
                base.AddParam(paras, "IN_EndDate", null);
            }

            base.AddParam(paras, "IN_DepartmentID", m_Project.DepartmentID);
            base.AddParam(paras, "IN_UserID", m_Project.UserID);
            base.AddParam(paras, "IN_DeliveryDate", m_Project.DeliveryDate, true);

            if (m_Project.AcceptanceDate != null)
            {
                base.AddParam(paras, "IN_AcceptanceDate", m_Project.AcceptanceDate, true);
            }
            else
            {
                base.AddParam(paras, "IN_AcceptanceDate", null);
            }

            base.AddParam(paras, "IN_AcceptanceFlag", m_Project.AcceptanceFlag);
            base.AddParam(paras, "IN_OrderAmount", m_Project.OrderAmount);

            base.AddParam(paras, "IN_Memo", m_Project.Memo);
            base.AddParam(paras, "IN_StatusFlag", m_Project.StatusFlag);

            base.AddParam(paras, "IN_CreateUID", m_Project.CreateUID);
            base.AddParam(paras, "IN_UpdateUID", m_Project.UpdateUID);

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }
        #endregion

        #region Update
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="m_Project">m_Project</param>
        /// <returns></returns>
        public int Update(M_Project m_Project)
        {
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" UPDATE dbo.M_Project");
            cmdText.AppendLine(" SET");
            cmdText.AppendLine("  ProjectCD = @IN_ProjectCD ");
            cmdText.AppendLine(" ,ProjectName = @IN_ProjectName ");
            cmdText.AppendLine(" ,WorkPlace = @IN_WorkPlace ");
            cmdText.AppendLine(" ,StartDate = @IN_StartDate ");
            cmdText.AppendLine(" ,EndDate = @IN_EndDate ");

            cmdText.AppendLine(" ,DepartmentID = @IN_DepartmentID ");
            cmdText.AppendLine(" ,UserID = @IN_UserID ");
            cmdText.AppendLine(" ,DeliveryDate = @IN_DeliveryDate ");
            cmdText.AppendLine(" ,AcceptanceDate = @IN_AcceptanceDate ");
            cmdText.AppendLine(" ,AcceptanceFlag = @IN_AcceptanceFlag ");
            cmdText.AppendLine(" ,OrderAmount = @IN_OrderAmount ");

            cmdText.AppendLine(" ,Memo = @IN_Memo ");
            cmdText.AppendLine(" ,StatusFlag = @IN_StatusFlag ");

            cmdText.AppendLine(" ,UpdateDate = GETDATE()");
            cmdText.AppendLine(" ,UpdateUID	 = @IN_UpdateUID");

            //Para
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_ProjectID", m_Project.ID);

            base.AddParam(paras, "IN_ProjectCD", m_Project.ProjectCD);
            base.AddParam(paras, "IN_ProjectName", m_Project.ProjectName);
            base.AddParam(paras, "IN_WorkPlace", m_Project.WorkPlace);
            base.AddParam(paras, "IN_StartDate", m_Project.StartDate, true);
            base.AddParam(paras, "IN_EndDate", m_Project.EndDate);

            base.AddParam(paras, "IN_DepartmentID", m_Project.DepartmentID);
            base.AddParam(paras, "IN_UserID", m_Project.UserID);
            base.AddParam(paras, "IN_DeliveryDate", m_Project.DeliveryDate, true);
            base.AddParam(paras, "IN_AcceptanceDate", m_Project.AcceptanceDate);
            base.AddParam(paras, "IN_AcceptanceFlag", m_Project.AcceptanceFlag);
            base.AddParam(paras, "IN_OrderAmount", m_Project.OrderAmount);

            base.AddParam(paras, "IN_Memo", m_Project.Memo);
            base.AddParam(paras, "IN_StatusFlag", m_Project.StatusFlag);
            base.AddParam(paras, "IN_UpdateDate", m_Project.UpdateDate, true);
            base.AddParam(paras, "IN_UpdateUID", m_Project.UpdateUID);

            cmdWhere.AppendLine(" ID = @IN_ProjectID AND UpdateDate = @IN_UpdateDate");

            //Check SQL
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }
        #endregion

        #region Delete

        /// <summary>
        /// DeleteProjectByID
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public int DeleteProjectByID(int ProjectID)
        {
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" DELETE	FROM dbo.M_Project");

            //Params
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine(" ID = @IN_ProjectID");
            base.AddParam(paras, "IN_ProjectID", ProjectID);


            //Check SQL
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }
        #endregion
    }
}