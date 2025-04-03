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
    /// Class M_User DAC
    /// </summary>
    public class UserService : BaseService
    {
        #region Contructor
        /// <summary>
        /// Contructor of User service
        /// </summary>        
        private UserService()
            : base()
        {
        }

        /// <summary>
        /// Contructor of User service
        /// </summary>
        /// <param name="db">Class DB</param>
        public UserService(DB db)
            : base(db)
        {
        }

        #endregion

        #region Get Data        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IList<DropDownModel> GetCbbUserExistExpense(string userId = "")
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine("SELECT DISTINCT");
            cmdText.AppendLine(" MU.ID AS Value");
            cmdText.AppendLine(" ,MU.UserName1 AS DisplayName");
            cmdText.AppendLine(" ,MU.StatusFlag AS StatusFlag");
            cmdText.AppendLine("FROM    T_Expence_H EH");
            cmdText.AppendLine("        LEFT JOIN M_User MU ON EH.UserID = MU.ID");
            cmdText.AppendLine("        LEFT JOIN M_Project AS MProject ON MProject.ID = EH.ProjectID");
            cmdText.AppendLine("WHERE   EH.ID >= 10");

            if (!string.IsNullOrEmpty(userId))
            {
                cmdText.AppendLine(String.Format("AND EH.UserID = {0} OR (EH.UpdateUID = {0} AND EH.ApprovedFlag = 1) OR (MProject.UserID = {0} AND EH.ApprovedFlag = 0)", userId));
            }

            cmdText.AppendLine("AND MU.ID is not null");

            return this.db.FindList1<DropDownModel>(cmdText.ToString());
        }

        /// <summary>
        /// Get by LoginID
        /// </summary>
        /// <returns>M_User</returns>
        public M_User GetByLoginID(string loginID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  1 AS RowNumber");
            cmdText.AppendLine(" ,T1.*");
            cmdText.AppendLine(" ,T2.GroupCD");
            cmdText.AppendLine(" ,T2.GroupName");
            cmdText.AppendLine(" ,T2.GroupName");
            cmdText.AppendLine(" FROM dbo.M_User AS T1");
            cmdText.AppendLine(" INNER JOIN  dbo.M_GroupUser_H AS T2 ON T2.ID = T1.GroupID");
            cmdWhere.AppendLine(" LoginID = @IN_LoginID");

            //Parameter
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_LoginID", loginID);
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.Find1<M_User>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// GetListByCond
        /// ISV-TRUC
        /// </summary>
        /// <param name="userName1"></param>
        /// <param name="userName2"></param>
        /// <param name="groupCD"></param>
        /// <param name="groupName"></param>
        /// <param name="inValid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="sortDirec"></param>
        /// <returns></returns>
        public IList<UserInfo> GetListByCond(string userName1, string groupCD, string groupName, string departmentCD, string departmentName, string inValid,
                                            int pageIndex, int pageSize, int sortField, int sortDirec)
        {
            string[] fields = new string[] { "UpdateDate", "UpdateDate", "UserCD", "LoginID", "UserName1", "DepartmentName", "CalendarName", "GroupName", "TotalVacationDays" };
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
            cmdText.AppendLine("  U.*");
            cmdText.AppendLine(" ,G.GroupCD");
            cmdText.AppendLine(" ,G.GroupName");
            cmdText.AppendLine(" ,D.DepartmentCD");
            cmdText.AppendLine(" ,D.DepartmentName");
            cmdText.AppendLine(" ," + string.Format("ROW_NUMBER() OVER(ORDER BY {0}) AS RowNumber", RowNumber));
            cmdText.AppendLine(" ,ISNULL(C.CalendarName, '') AS CalendarName");
            cmdText.AppendLine(" ,ISNULL(V.TotalVacationDays, 0) AS TotalVacationDays");

            cmdText.AppendLine(" FROM dbo.M_User AS U");
            cmdText.AppendLine(" INNER JOIN dbo.M_GroupUser_H AS G ON U.GroupID=G.ID");
            cmdText.AppendLine(" INNER JOIN dbo.M_Department AS D ON U.DepartmentID=D.ID");
            cmdText.AppendLine(" LEFT JOIN (SELECT UID, SUM(VacationDay) AS TotalVacationDays");
            cmdText.AppendLine("            FROM dbo.T_PaidVacation");
            cmdText.AppendLine("            WHERE InvalidFlag = 0");
            cmdText.AppendLine("            GROUP BY UID) V ON V.UID = U.ID");

            cmdText.AppendLine(" LEFT JOIN (SELECT");
            cmdText.AppendLine("                 wcu.UID,");
            cmdText.AppendLine("                 CalendarName");
            cmdText.AppendLine("                 FROM");
            cmdText.AppendLine("                           dbo.T_WorkingCalendar_U AS wcu");
            cmdText.AppendLine("                 LEFT JOIN dbo.T_WorkingCalendar_H AS wch ON wcu.HID = wch.ID");
            cmdText.AppendLine("                 WHERE InitialDate <= @IN_CurrentDate AND @IN_CurrentDate <= DATEADD(DAY, -1, DATEADD(year, 1, InitialDate)))");
            cmdText.AppendLine("             C ON C.UID = U.ID");

            cmdWhere.AppendLine(" U.UserCD NOT IN(SELECT Value3 FROM M_Config_D DD WHERE  EXISTS(SELECT ID FROM M_Config_H HH WHERE HH.ConfigCD=@IN_GROUPADMIN AND HH.ID=DD.HID)) ");


            //Parameter
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_GROUPADMIN", M_Config_H.CONFIG_GROUP_USERCD_ADMIN);
            base.AddParam(paras, "IN_CurrentDate", DateTime.Now.Date);

            if (!string.IsNullOrEmpty(userName1))
            {
                cmdWhere.AppendLine(" AND (");
                cmdWhere.AppendLine(" UPPER(U.UserName1) LIKE '%' + UPPER(@IN_UserName1) + '%' ");
                cmdWhere.AppendLine(" OR UPPER(U.UserName2) LIKE '%' + UPPER(@IN_UserName1) + '%' ");
                cmdWhere.AppendLine(" ) ");
                base.AddParam(paras, "IN_UserName1", userName1);
            }

            if (!string.IsNullOrEmpty(groupCD))
            {
                cmdWhere.AppendLine(" AND G.GroupCD = @IN_GroupCD");
                base.AddParam(paras, "IN_GroupCD", EditDataUtil.ToFixCodeDB(groupCD, M_GroupUser_H.GROUP_CODE_DB_MAX_LENGTH));
            }

            if (!string.IsNullOrEmpty(groupName))
            {
                cmdWhere.AppendLine(" AND UPPER(G.GroupName) LIKE '%' + UPPER(@IN_GroupName) + '%' ");
                base.AddParam(paras, "IN_GroupName", groupName);
            }

            if (!string.IsNullOrEmpty(departmentCD))
            {
                cmdWhere.AppendLine(" AND D.DepartmentCD = @IN_DepartmentCD");
                base.AddParam(paras, "IN_DepartmentCD", EditDataUtil.ToFixCodeDB(departmentCD, M_Department.DEPARTMENT_CODE_DB_MAX_LENGTH));
            }

            if (!string.IsNullOrEmpty(departmentName))
            {
                cmdWhere.AppendLine(" AND UPPER(D.DepartmentName) LIKE '%' + UPPER(@IN_DepartmentName) + '%' ");
                base.AddParam(paras, "IN_DepartmentName", departmentName);
            }

            cmdWhere.AppendLine(" AND ((@IN_InValid = 0 AND StatusFlag = 0) OR (@IN_InValid = 2) OR (@IN_InValid = 1 AND StatusFlag = 1)) ");
            base.AddParam(paras, "IN_InValid", inValid);

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            base.AddParam(paras, "IN_PageIndex", pageIndex);
            base.AddParam(paras, "IN_PageSize", pageSize);

            //SQL OUT

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

            return this.db.FindList1<UserInfo>(cmdOutText.ToString(), paras);
        }

        public IList<UserInfo> GetAllUser()
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();
            string RowNumber = "LoginID ASC";

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  U.*");
            cmdText.AppendLine(" ,G.GroupCD");
            cmdText.AppendLine(" ,G.GroupName");
            cmdText.AppendLine(" ,D.DepartmentCD");
            cmdText.AppendLine(" ,D.DepartmentName");
            cmdText.AppendLine(" ," + string.Format("ROW_NUMBER() OVER(ORDER BY {0}) AS RowNumber", RowNumber));
            cmdText.AppendLine(" FROM dbo.M_User AS U");

            cmdText.AppendLine(" INNER JOIN dbo.M_Department AS D ON U.DepartmentID=D.ID");
            cmdText.AppendLine(" INNER JOIN dbo.M_GroupUser_H AS G ON U.GroupID=G.ID");
            //cmdWhere.AppendLine(" U.UserCD NOT IN(SELECT Value3 FROM M_Config_D DD WHERE  EXISTS(SELECT ID FROM M_Config_H HH WHERE HH.ConfigCD='L018' AND HH.ID=DD.HID)) ");

            //Parameter
            Hashtable paras = new Hashtable();

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.FindList1<UserInfo>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Get user Combobox data by department id
        /// </summary>
        /// <param name="departmentID">department id</param>
        /// <returns></returns>
        public IList<DropDownModel> GetCbbUserDataByDepartmentID(int departmentID, int calendarId = -1, int option = -1, string year = "", int userId = -1)
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

            cmdWhere.AppendLine(" U.UserCD NOT IN(SELECT Value3 FROM M_Config_D DD WHERE  EXISTS(SELECT ID FROM M_Config_H HH WHERE HH.ConfigCD=@IN_GROUPADMIN AND HH.ID=DD.HID)) ");

            base.AddParam(paras, "IN_GROUPADMIN", M_Config_H.CONFIG_GROUP_USERCD_ADMIN);

            if (departmentID != -1)
            {
                cmdWhere.AppendLine(" AND U.DepartmentID = @IN_DepartmentID");

                //Add param
                base.AddParam(paras, "IN_DepartmentID", departmentID);
            }

            if (calendarId != -1)
            {
                cmdWhere.AppendLine("AND EXISTS(SELECT 1 FROM T_WorkingCalendar_U WU WHERE WU.HID = @IN_CalendarID AND WU.UID = U.ID)");
                base.AddParam(paras, "IN_CalendarID", calendarId);
            }

            if (option == 0)
            {
                cmdWhere.AppendLine("AND StatusFlag = 0");
            }
            else if (option == 1)
            {
                cmdWhere.AppendLine("AND StatusFlag = 1");
            }
            if (!string.IsNullOrEmpty(year))
            {
                cmdWhere.AppendLine(" AND (U.StatusFlag = 0 OR EXISTS(SELECT 1 FROM T_Payslip WHERE UserID = U.ID AND Left([Year],4) = @IN_Year)) ");
                base.AddParam(paras, "IN_Year", year);
            }

            if (userId != -1)
            {
                cmdWhere.AppendLine(" AND U.ID = @IN_UserID");

                //Add param
                base.AddParam(paras, "IN_UserID", userId);
            }

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }
            cmdText.AppendLine(" ORDER BY U.StatusFlag, U.UserCD");
            return this.db.FindList1<DropDownModel>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Get user Combobox data by department id
        /// </summary>
        /// <param name="departmentID">department id</param>
        /// <returns></returns>
        public IList<DropDownModel> GetCbbUserManagementProject()
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

            cmdWhere.AppendLine(" U.UserCD NOT IN(SELECT Value3 FROM M_Config_D DD WHERE  EXISTS(SELECT ID FROM M_Config_H HH WHERE HH.ConfigCD=@IN_GROUPADMIN AND HH.ID=DD.HID)) ");

            base.AddParam(paras, "IN_GROUPADMIN", M_Config_H.CONFIG_GROUP_USERCD_ADMIN);
            cmdWhere.AppendLine(" AND U.ID IN (SELECT UserID FROM M_Project)");

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }
            cmdText.AppendLine(" ORDER BY U.StatusFlag, U.UserCD");
            return this.db.FindList1<DropDownModel>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Get user Combobox data by department id
        /// </summary>
        /// <param name="departmentID">department id</param>
        /// <returns></returns>
        public IList<DropDownModel> GetCbbUserAllData(int calendarId = -1, int option = -1, string year = "")
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

            cmdWhere.AppendLine(" U.UserCD NOT IN(SELECT Value3 FROM M_Config_D DD WHERE  EXISTS(SELECT ID FROM M_Config_H HH WHERE HH.ConfigCD=@IN_GROUPADMIN AND HH.ID=DD.HID)) ");

            base.AddParam(paras, "IN_GROUPADMIN", M_Config_H.CONFIG_GROUP_USERCD_ADMIN);

            if (calendarId != -1)
            {
                cmdWhere.AppendLine("AND EXISTS(SELECT 1 FROM T_WorkingCalendar_U WU WHERE WU.HID = @IN_CalendarID AND WU.UID = U.ID)");
                base.AddParam(paras, "IN_CalendarID", calendarId);
            }

            if (option == 0)
            {
                cmdWhere.AppendLine("AND StatusFlag = 0");
            }
            else if (option == 1)
            {
                cmdWhere.AppendLine("AND StatusFlag = 1");
            }
            if (!string.IsNullOrEmpty(year))
            {
                cmdWhere.AppendLine(" AND (U.StatusFlag = 0 OR EXISTS(SELECT 1 FROM T_Payslip WHERE UserID = U.ID AND Left([Year],4) = @IN_Year)) ");
                base.AddParam(paras, "IN_Year", year);
            }

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }
            cmdText.AppendLine(" ORDER BY U.StatusFlag, U.UserCD");
            return this.db.FindList1<DropDownModel>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Get list by conditions for the search form
        /// </summary>
        /// <returns>UserSearchInfo</returns>
        public IList<UserSearchInfo> GetListByConditionForSearch(string userCd, string loginID, string userName1, string groupCD, string departmentCD, int pageIndex, int pageSize, int sortField, int sortDirec)
        {
            string[] fields = new string[] { "UserCD", "LoginID", "UserName1", "UserName2", "GroupName", "DepartmentName", "StatusFlag" };
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
            cmdText.AppendLine("  U.*");
            cmdText.AppendLine(" ,GH.GroupCD");
            cmdText.AppendLine(" ,GH.GroupName");
            cmdText.AppendLine(" ,D.DepartmentCD");
            cmdText.AppendLine(" ,D.DepartmentName");
            cmdText.AppendLine(" ," + string.Format("ROW_NUMBER() OVER(ORDER BY {0}) AS RowNumber", RowNumber));
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.M_User  AS U");
            cmdText.AppendLine(" INNER JOIN dbo.M_GroupUser_H AS GH ON U.GroupID=GH.ID");
            cmdText.AppendLine(" INNER JOIN dbo.M_Department AS D ON U.GroupID=D.ID");

            cmdWhere.AppendLine(" StatusFlag = 0 ");
            cmdWhere.AppendLine(" AND U.UserCD NOT IN(SELECT Value3 FROM M_Config_D DD WHERE  EXISTS(SELECT ID FROM M_Config_H HH WHERE HH.ConfigCD=@IN_GROUPADMIN AND HH.ID=DD.HID)) ");

            //Parameter
            Hashtable paras = new Hashtable();

            base.AddParam(paras, "IN_GROUPADMIN", M_Config_H.CONFIG_GROUP_USERCD_ADMIN);

            if (!string.IsNullOrEmpty(userCd))
            {
                cmdWhere.AppendLine(" AND  UPPER(U.UserCD) LIKE UPPER(@IN_UserCD) + '%' ");
                base.AddParam(paras, "IN_UserCD", userCd);
            }

            if (!string.IsNullOrEmpty(loginID))
            {
                cmdWhere.AppendLine(" AND UPPER(U.LoginID) LIKE UPPER(@IN_LoginID) + '%'");
                base.AddParam(paras, "IN_LoginID", loginID);
            }

            if (!string.IsNullOrEmpty(userName1))
            {

                cmdWhere.AppendLine(" AND (UPPER(U.UserName1) LIKE '%' + UPPER(@IN_UserName1) + '%' ");
                cmdWhere.AppendLine(" OR UPPER(U.UserName2) LIKE '%' + UPPER(@IN_UserName1) + '%' ) ");
                base.AddParam(paras, "IN_UserName1", userName1);
            }

            if (!string.IsNullOrEmpty(groupCD))
            {
                cmdWhere.AppendLine(" AND GH.GroupCD = @IN_GroupCD ");
                base.AddParam(paras, "IN_GroupCD", groupCD);
            }

            if (!string.IsNullOrEmpty(departmentCD))
            {
                cmdWhere.AppendLine(" AND D.DepartmentCD = @IN_DepartmentCD ");
                base.AddParam(paras, "IN_DepartmentCD", departmentCD);
            }

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            base.AddParam(paras, "IN_PageIndex", pageIndex);
            base.AddParam(paras, "IN_PageSize", pageSize);

            //SQL OUT

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

            return this.db.FindList1<UserSearchInfo>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userCd"></param>
        /// <param name="userName1"></param>
        /// <param name="userName2"></param>
        /// <param name="groupCD"></param>
        /// <returns></returns>
        public int GetCountByConditionForSearch(string userCd, string loginID, string userName1, string groupCD, string departmentCD)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();
            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" count(*)");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.M_User AS U");
            cmdText.AppendLine(" INNER JOIN	dbo.M_GroupUser_H AS GH ON GH.ID = U.GroupID");
            cmdText.AppendLine(" INNER JOIN	dbo.M_Department AS D ON D.ID = U.DepartmentID");

            cmdWhere.AppendLine(" StatusFlag = 0");
            cmdWhere.AppendLine(" AND U.UserCD NOT IN(SELECT Value3 FROM M_Config_D DD WHERE  EXISTS(SELECT ID FROM M_Config_H HH WHERE HH.ConfigCD=@IN_GROUPADMIN AND HH.ID=DD.HID))");

            //Parameter
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_GROUPADMIN", M_Config_H.CONFIG_GROUP_USERCD_ADMIN);

            if (!string.IsNullOrEmpty(userCd))
            {
                cmdWhere.AppendLine(" AND UPPER(U.UserCD) LIKE UPPER(@IN_UserCD) + '%' ");
                base.AddParam(paras, "IN_UserCD", userCd);
            }

            if (!string.IsNullOrEmpty(loginID))
            {
                cmdWhere.AppendLine(" AND UPPER(U.LoginID) LIKE UPPER(@IN_LoginID) + '%' ");
                base.AddParam(paras, "IN_LoginID", loginID);
            }

            if (!string.IsNullOrEmpty(userName1))
            {
                cmdWhere.AppendLine(" AND (UPPER(U.UserName1) LIKE '%' + UPPER(@IN_UserName1) + '%'  ");
                cmdWhere.AppendLine(" OR UPPER(U.UserName2) LIKE '%' + UPPER(@IN_UserName1) + '%' ) ");
                base.AddParam(paras, "IN_UserName1", userName1);
            }

            if (!string.IsNullOrEmpty(groupCD))
            {
                cmdWhere.AppendLine(" AND GH.GroupCD = @IN_GroupCD ");
                base.AddParam(paras, "IN_GroupCD", groupCD);
            }

            if (!string.IsNullOrEmpty(departmentCD))
            {
                cmdWhere.AppendLine(" AND D.DepartmentCD = @IN_DepartmentCD ");
                base.AddParam(paras, "IN_DepartmentCD", departmentCD);
            }

            if (cmdWhere.Length != 0)
            {

                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return int.Parse(this.db.ExecuteScalar1(cmdText.ToString(), paras).ToString());
        }

        /// <summary>
        /// Get by user ID
        /// ISV-TRUC
        /// </summary>
        /// <param name="userID">userID</param>
        /// <returns></returns>
        public M_User GetByID(int userID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" U.*");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.M_User U");

            //Parameter
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine(" U.ID = @IN_UserID ");
            base.AddParam(paras, "IN_UserID", userID);

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.Find1<M_User>(cmdText.ToString(), paras);
        }
        /// <summary>
        /// Get by user ID from Project
        /// ISV-TRI
        /// </summary>
        /// <param name="userID">userID</param>
        /// <returns></returns>
        public M_User GetByID_Project(int userID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" U.*");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.M_User U");
            cmdText.AppendLine(" LEFT JOIN M_Project P ON U.ID = P.UserID");

            //Parameter
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine(" P.ID = @IN_UserID ");
            base.AddParam(paras, "IN_UserID", userID);

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.Find1<M_User>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Get By User Code
        /// ISV-TRUC
        /// </summary>
        /// <param name="userCD">userCD</param>
        /// <returns></returns>
        public M_User GetByUserCD(string userCD, bool includeDelete = true)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" T1.*");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.M_User AS T1");

            //Parameter
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine(" UserCD = @IN_UserCD ");
            base.AddParam(paras, "IN_UserCD", EditDataUtil.ToFixCodeDB(userCD, M_User.USER_CODE_MAX_LENGTH));

            cmdWhere.AppendLine(" AND (@IN_Invalid = 1 OR StatusFlag = 0) ");
            base.AddParam(paras, "IN_Invalid", includeDelete ? 1 : 0);

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.Find1<M_User>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// getTotalRow
        /// ISV-TRUC
        /// </summary>
        /// <param name="userName1"></param>
        /// <param name="userName2"></param>
        /// <param name="groupCD"></param>
        /// <param name="groupName"></param>
        /// <param name="inValid"></param>
        /// <returns></returns>
        public int getTotalRow(string userName1, string groupCD, string groupName, string departmentCD, string departmentName, string inValid)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" COUNT(*)");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.M_User AS U");
            cmdText.AppendLine(" INNER JOIN	dbo.M_GroupUser_H AS G ON G.ID = U.GroupID");
            cmdText.AppendLine(" INNER JOIN	dbo.M_Department AS D ON D.ID = U.DepartmentID");

            cmdWhere.AppendLine(" U.UserCD NOT IN(SELECT Value3 FROM M_Config_D DD WHERE  EXISTS(SELECT ID FROM M_Config_H HH WHERE HH.ConfigCD=@IN_GROUPADMIN AND HH.ID=DD.HID)) ");

            //Parameter
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_GROUPADMIN", M_Config_H.CONFIG_GROUP_USERCD_ADMIN);

            if (!string.IsNullOrEmpty(userName1))
            {
                cmdWhere.AppendLine(" AND (UPPER(U.UserName1) LIKE N'%' + UPPER(@IN_UserName1) + '%' ");
                cmdWhere.AppendLine(" OR UPPER(U.UserName2) LIKE N'%' + UPPER(@IN_UserName1) + '%') ");

                base.AddParam(paras, "IN_UserName1", userName1);
            }

            if (!string.IsNullOrEmpty(groupCD))
            {
                cmdWhere.AppendLine(" AND G.GroupCD = @IN_GroupCD ");
                base.AddParam(paras, "IN_GroupCD", EditDataUtil.ToFixCodeDB(groupCD, M_GroupUser_H.GROUP_CODE_DB_MAX_LENGTH));
            }

            if (!string.IsNullOrEmpty(groupName))
            {
                cmdWhere.AppendLine(" AND UPPER(G.GroupName) LIKE N'%' + UPPER(@IN_GroupName) + '%' ");
                base.AddParam(paras, "IN_GroupName", groupName);
            }

            if (!string.IsNullOrEmpty(departmentCD))
            {
                cmdWhere.AppendLine(" AND D.DepartmentCD = @IN_DepartmentCD ");
                base.AddParam(paras, "IN_DepartmentCD", EditDataUtil.ToFixCodeDB(departmentCD, M_Department.DEPARTMENT_CODE_DB_MAX_LENGTH));
            }

            if (!string.IsNullOrEmpty(departmentName))
            {
                cmdWhere.AppendLine(" AND UPPER(D.DepartmentName) LIKE N'%' + UPPER(@IN_DepartmentName) + '%' ");
                base.AddParam(paras, "IN_DepartmentName", departmentName);
            }

            cmdWhere.AppendLine(" AND ((@IN_InValid = 0 AND StatusFlag = 0) OR (@IN_InValid = 2) OR (@IN_InValid = 1 AND StatusFlag = 1)) ");
            base.AddParam(paras, "IN_InValid", inValid);


            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return int.Parse(this.db.ExecuteScalar1(cmdText.ToString(), paras).ToString());
        }

        /// <summary>
        /// Get By Key
        /// ISV-TRUC
        /// </summary>
        /// <param name="userCD">userCD</param>
        /// <param name="loginID">loginID</param>
        /// <returns></returns>
        public M_User GetByKey(string userCD, string loginID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" T1.*");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.M_User AS T1");

            //Parameter
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine(" UserCD = @IN_UserCD ");
            base.AddParam(paras, "IN_UserCD", EditDataUtil.ToFixCodeDB(userCD, M_User.USER_CODE_MAX_LENGTH));

            cmdWhere.AppendLine(" AND LoginID = @IN_LoginID ");
            base.AddParam(paras, "IN_LoginID", loginID);

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.Find1<M_User>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Get By Key
        /// ISV-TRUC
        /// </summary>
        /// <param name="userCD">userID</param>
        /// <param name="loginID">loginID</param>
        /// <returns></returns>
        public M_User GetByKey(int userID, string loginID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" T1.*");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.M_User AS T1");

            //Parameter
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine(" ID = @IN_ID ");
            base.AddParam(paras, "IN_ID", userID);

            cmdWhere.AppendLine(" AND LoginID = @IN_LoginID ");
            base.AddParam(paras, "IN_LoginID", loginID);

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.Find1<M_User>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// GetCountActiveUser
        /// </summary>
        /// <returns></returns>
        public int GetCountActiveUser()
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" COUNT(*) AS CurActive");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.M_User");

            //Para
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine(" UserCD NOT IN(SELECT Value3 FROM M_Config_D DD WHERE  EXISTS(SELECT ID FROM M_Config_H HH WHERE HH.ConfigCD=@IN_GROUPADMIN AND HH.ID=DD.HID)) ");
            cmdWhere.AppendLine(" AND StatusFlag <> 1 ");
            base.AddParam(paras, "IN_GROUPADMIN", M_Config_H.CONFIG_GROUP_USERCD_ADMIN);

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return int.Parse(this.db.ExecuteScalar1(cmdText.ToString(), paras).ToString());
        }

        /// <summary>
        /// Get By User Code
        /// ISV-TRUC
        /// </summary>
        /// <param name="userCD">userCD</param>
        /// <returns></returns>
        public IList<UserTreeView> GetByUserExcept(int HID, int DepartmentID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" T1.DepartmentID");
            cmdText.AppendLine(" ,T1.ID");
            cmdText.AppendLine(" ,T1.UserName1");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.M_User AS T1");

            cmdText.AppendLine(" INNER JOIN dbo.M_Department AS D ON T1.DepartmentID=D.ID");
            cmdText.AppendLine(" INNER JOIN dbo.M_GroupUser_H AS G ON T1.GroupID=G.ID");
            cmdText.AppendLine(" WHERE ");
            cmdText.AppendLine(" T1.DepartmentID = @IN_DepartmentID");
            cmdText.AppendLine(" AND T1.StatusFlag = 0");
            cmdText.AppendLine(" AND T1.ID >= 10");

            //Parameter
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine(" AND T1.ID NOT IN (");
            cmdWhere.AppendLine(" SELECT [UID]");
            cmdWhere.AppendLine(" FROM dbo.T_Mail_D");
            cmdWhere.AppendLine(" WHERE HID = @IN_HID");
            cmdWhere.AppendLine(" )");
            base.AddParam(paras, "IN_HID", HID);
            base.AddParam(paras, "IN_DepartmentID", DepartmentID);
            if (HID != 0)
            {
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.FindList1<UserTreeView>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Get By User Code
        /// ISV-TRUC
        /// </summary>
        /// <param name="userCD">userCD</param>
        /// <returns></returns>
        public IList<UserTreeView> GetListUserByHID(int HID, int DepartmentID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" T1.DepartmentID");
            cmdText.AppendLine(" ,T1.ID");
            cmdText.AppendLine(" ,T1.UserName1");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.M_User AS T1");

            cmdText.AppendLine(" INNER JOIN dbo.M_Department AS D ON T1.DepartmentID=D.ID");
            cmdText.AppendLine(" INNER JOIN dbo.M_GroupUser_H AS G ON T1.GroupID=G.ID");
            cmdText.AppendLine(" INNER JOIN dbo.T_Mail_D AS TMD ON T1.ID=TMD.[UID]");
            cmdText.AppendLine(" WHERE ");
            cmdText.AppendLine("T1.DepartmentID = @IN_DepartmentID");
            cmdText.AppendLine("AND TMD.HID = @IN_HID");

            //Parameter
            Hashtable paras = new Hashtable();

            base.AddParam(paras, "IN_HID", HID);
            base.AddParam(paras, "IN_DepartmentID", DepartmentID);

            return this.db.FindList1<UserTreeView>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DeptCode"></param>
        /// <returns></returns>
        public IList<M_User> GetListUserByDepartmentCode(string DepartmentCD)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine(" SELECT T1.*");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.M_User AS T1");
            cmdText.AppendLine(" INNER JOIN dbo.M_Department AS D ON T1.DepartmentID = D.ID");
            cmdText.AppendLine(" WHERE");
            cmdText.AppendLine(" T1.StatusFlag = 0 AND T1.ID >= 10");
            cmdText.AppendLine(" AND D.DepartmentCD = @IN_DepartmentCD");

            //Parameter
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_DepartmentCD", EditDataUtil.ToFixCodeDB(DepartmentCD, M_Department.DEPARTMENT_CODE_DB_MAX_LENGTH));

            return this.db.FindList1<M_User>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Get Users Permission Approval
        /// </summary>
        /// <returns>M_User</returns>
        public IList<M_User> GetListApprovalMails(int uid)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" U.*");
            cmdText.AppendLine(" FROM M_User U");
            cmdText.AppendLine(" INNER JOIN M_GroupUser_H GH ON U.GroupID = GH.ID");
            cmdText.AppendLine(" INNER JOIN M_GroupUser_D GD ON GD.GroupID = GH.ID AND GD.FormID = @IN_FormId AND GD.AuthorityFlag3 = 1");
            cmdText.AppendLine(" WHERE U.StatusFlag = 0");
            cmdText.AppendLine(" AND (GD.AuthorityFlag2 = 1 OR U.DepartmentID = (SELECT DepartmentID FROM M_User WHERE ID = @IN_UID))");
            cmdText.AppendLine(" AND U.ID <> @IN_UID");
            //cmdText.AppendLine(" AND U.ID NOT IN (SELECT U1.ID FROM M_User U1");
            //cmdText.AppendLine(" INNER JOIN M_GroupUser_H GH1 ON U1.GroupID = GH1.ID");
            //cmdText.AppendLine(" INNER JOIN M_GroupUser_D GD1 ON GD1.GroupID = GH1.ID AND GD1.FormID = @IN_FormId AND GD1.AuthorityFlag4 = 1)");

            //Parameter
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_FormId", FormId.Approval);
            base.AddParam(paras, "IN_UID", uid);

            return this.db.FindList1<M_User>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Get Users Permission Approval
        /// </summary>
        /// <returns>M_User</returns>
        public IList<M_User> GetListConfirmMails(int uid)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" U.*");
            cmdText.AppendLine(" FROM M_User U");
            cmdText.AppendLine(" INNER JOIN M_GroupUser_H GH ON U.GroupID = GH.ID");
            cmdText.AppendLine(" INNER JOIN M_GroupUser_D GD ON GD.GroupID = GH.ID AND GD.FormID = @IN_FormId AND GD.AuthorityFlag4 = 1");
            cmdText.AppendLine(" WHERE U.StatusFlag = 0");
            cmdText.AppendLine(" AND U.ID <> @IN_UID");

            //Parameter
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_FormId", FormId.Approval);
            base.AddParam(paras, "IN_UID", uid);

            return this.db.FindList1<M_User>(cmdText.ToString(), paras);
        }

        public IList<M_User> GetValidUsers()
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" U.*");
            cmdText.AppendLine(" FROM M_User U");
            cmdText.AppendLine(" WHERE U.StatusFlag = 0");
            cmdText.AppendLine(" AND   U.ID >= 10");
            cmdText.AppendLine(" ORDER BY U.UserCD");

            //Parameter
            return this.db.FindList1<M_User>(cmdText.ToString());
        }
       
        #endregion

        #region Insert

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="user">M_User</param>
        /// <returns></returns>
        public int Insert(M_User user)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine(" INSERT INTO");
            cmdText.AppendLine(" dbo.M_User");
            cmdText.AppendLine(" (");
            cmdText.AppendLine("  UserCD");
            cmdText.AppendLine(" ,LoginID");
            cmdText.AppendLine(" ,UserName1");
            cmdText.AppendLine(" ,UserName2");
            cmdText.AppendLine(" ,Position1");
            cmdText.AppendLine(" ,Position2");
            cmdText.AppendLine(" ,[Password]");
            cmdText.AppendLine(" ,GroupID");
            cmdText.AppendLine(" ,DepartmentID");
            cmdText.AppendLine(" ,MailAddress");
            cmdText.AppendLine(" ,StatusFlag");
            cmdText.AppendLine(" ,CreateDate");
            cmdText.AppendLine(" ,CreateUID");
            cmdText.AppendLine(" ,UpdateDate");
            cmdText.AppendLine(" ,UpdateUID");
            cmdText.AppendLine(" )");
            cmdText.AppendLine(" VALUES");
            cmdText.AppendLine(" (");
            cmdText.AppendLine("  @IN_UserCD");
            cmdText.AppendLine(" ,@IN_LoginID");
            cmdText.AppendLine(" ,@IN_UserName1");
            cmdText.AppendLine(" ,@IN_UserName2");
            cmdText.AppendLine(" ,@IN_Position1");
            cmdText.AppendLine(" ,@IN_Position2");
            cmdText.AppendLine(" ,@IN_Password");
            cmdText.AppendLine(" ,@IN_GroupID");
            cmdText.AppendLine(" ,@IN_DepartmentID");
            cmdText.AppendLine(" ,@IN_MailAddress");
            cmdText.AppendLine(" ,@IN_StatusFlag");
            cmdText.AppendLine(" ,GETDATE()");
            cmdText.AppendLine(" ,@IN_CreateUID");
            cmdText.AppendLine(" ,GETDATE()");
            cmdText.AppendLine(" ,@IN_UpdateUID");
            cmdText.AppendLine(" )");

            //Para
            Hashtable paras = new Hashtable();
            ISecurity sec = Security.Instance;

            base.AddParam(paras, "IN_UserCD", EditDataUtil.ToFixCodeDB(user.UserCD, M_User.USER_CODE_MAX_LENGTH));
            base.AddParam(paras, "IN_LoginID", user.LoginID);
            base.AddParam(paras, "IN_UserName1", user.UserName1);
            base.AddParam(paras, "IN_UserName2", user.UserName2);
            base.AddParam(paras, "IN_Position1", user.Position1);
            base.AddParam(paras, "IN_Position2", user.Position2);
            base.AddParam(paras, "IN_Password", sec.Encrypt(user.Password));
            base.AddParam(paras, "IN_GroupID", user.GroupID);
            base.AddParam(paras, "IN_DepartmentID", user.DepartmentID);
            base.AddParam(paras, "IN_MailAddress", user.MailAddress);
            base.AddParam(paras, "IN_StatusFlag", user.StatusFlag);
            base.AddParam(paras, "IN_CreateUID", user.CreateUID);
            base.AddParam(paras, "IN_UpdateUID", user.UpdateUID);
            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }
        #endregion

        #region Update
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="user">M_User</param>
        /// <returns></returns>
        public int Update(M_User user)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" UPDATE");
            cmdText.AppendLine(" dbo.M_User");
            cmdText.AppendLine(" SET");
            cmdText.AppendLine("  UserCD     = @IN_UserCD");
            cmdText.AppendLine(" ,LoginID    = @IN_LoginID");
            cmdText.AppendLine(" ,UserName1  = @IN_UserName1");
            cmdText.AppendLine(" ,UserName2  = @IN_UserName2");
            cmdText.AppendLine(" ,Position1  = @IN_Position1");
            cmdText.AppendLine(" ,Position2  = @IN_Position2");
            cmdText.AppendLine(" ,[Password] = @IN_Password");
            cmdText.AppendLine(" ,GroupID    = @IN_GroupID");
            cmdText.AppendLine(" ,DepartmentID    = @IN_DepartmentID");
            cmdText.AppendLine(" ,MailAddress    = @IN_MailAddress");
            cmdText.AppendLine(" ,StatusFlag = @IN_StatusFlag");
            cmdText.AppendLine(" ,UpdateDate = GETDATE()");
            cmdText.AppendLine(" ,UpdateUID  = @IN_UpdateUID");

            //Para
            Hashtable paras = new Hashtable();
            ISecurity sec = Security.Instance;

            cmdWhere.AppendLine(" ID = @IN_ID AND UpdateDate=@IN_UpdateDate");

            base.AddParam(paras, "IN_ID", user.ID);
            base.AddParam(paras, "IN_UserCD", EditDataUtil.ToFixCodeDB(user.UserCD, M_User.USER_CODE_MAX_LENGTH));
            base.AddParam(paras, "IN_LoginID", user.LoginID);
            base.AddParam(paras, "IN_UserName1", user.UserName1);
            base.AddParam(paras, "IN_UserName2", user.UserName2);
            base.AddParam(paras, "IN_Position1", user.Position1);
            base.AddParam(paras, "IN_Position2", user.Position2);
            base.AddParam(paras, "IN_Password", sec.Encrypt(user.Password));
            base.AddParam(paras, "IN_GroupID", user.GroupID);
            base.AddParam(paras, "IN_DepartmentID", user.DepartmentID);
            base.AddParam(paras, "IN_MailAddress", user.MailAddress);
            base.AddParam(paras, "IN_StatusFlag", user.StatusFlag);
            base.AddParam(paras, "IN_UpdateDate", user.UpdateDate);
            base.AddParam(paras, "IN_UpdateUID", user.UpdateUID);

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Update Password
        /// ISV-GIAM
        /// </summary>
        /// <param name="user">M_User</param>
        /// <param name="userPassword">new Password</param>
        /// <returns></returns>
        public int UpdatePassword(M_User user, string userPassword)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" UPDATE");
            cmdText.AppendLine(" dbo.M_User");
            cmdText.AppendLine(" SET");
            cmdText.AppendLine("  [Password] = @IN_UserPassword");
            cmdText.AppendLine(" ,UpdateDate = GETDATE()");
            cmdText.AppendLine(" ,UpdateUID	 = @IN_UpdateUID");

            cmdWhere.AppendLine(" ID = @IN_ID AND UpdateDate=@IN_UpdateDate");

            //Para

            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_ID", user.ID);
            base.AddParam(paras, "IN_UserPassword", userPassword);
            base.AddParam(paras, "IN_UpdateDate", user.UpdateDate, true);
            base.AddParam(paras, "IN_UpdateUID", user.UpdateUID);
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
