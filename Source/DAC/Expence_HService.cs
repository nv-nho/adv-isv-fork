using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using OMS.Utilities;
using OMS.Models;

namespace OMS.DAC
{
    /// <summary>
    /// Class Expence_HService DAC
    /// </summary>
    public class Expence_HService : BaseService
    {
        #region Constructor
        /// <summary>
        /// Contructor of T_Expence_H service
        /// </summary>        
        private Expence_HService()
            : base()
        {
        }

        /// <summary>
        /// Contructor of T_Expence_H service
        /// </summary>
        /// <param name="db">Class DB</param>
        public Expence_HService(DB db)
            : base(db)
        {
        }
        #endregion

        #region Get Data

        /// <summary>
        /// Get By T_Expence
        /// </summary>
        /// <returns>M_Project</returns>
        public T_Expence_H GetByExpenceNo(string expenceNo)
        {
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT * ");


            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.T_Expence_H");

            //Para
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine(" RTRIM(ExpenceNo) = RTRIM(@IN_ExpenceNo)");
            base.AddParam(paras, "IN_ExpenceNo", EditDataUtil.ToFixCodeDB(expenceNo, T_Expence_H.EXPENCE_CODE_DB_MAX_LENGTH));
            cmdWhere.AppendLine(" AND ID >= 10");

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.Find1<T_Expence_H>(cmdText.ToString(), paras);
        }



        /// <summary>
        /// Get by ID
        /// </summary>
        /// <returns>T_Expence_H</returns>
        public T_Expence_H GetByID(int ID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  1 AS RowNumber");
            cmdText.AppendLine(" ,T1.*");
            cmdText.AppendLine(" FROM dbo.T_Expence_H AS T1");
            cmdWhere.AppendLine(" ID = @IN_ID");

            //Parameter
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_ID", ID);
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.Find1<T_Expence_H>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// GetListByCond
        /// </summary>
        /// <param name="userCD"></param>
        /// <param name="departmentCD"></param>
        /// <param name="projectCD"></param>
        /// <param name="accountCD"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="sortDirec"></param>
        /// <returns></returns>
        public IList<ExpenceInfo> GetListByCond(string userID, string userUpdate, string departmentID, string projectCD, string accountCD,
                                                DateTime? accountingDate1, DateTime? accountingDate2,
                                                DateTime? theDateOfUse1, DateTime? theDateOfUse2,
                                                int pageIndex, int pageSize, int approvedFlag, int UserLogin, bool IsExpenceAllApproved,
                                                string SortDirec = "2", string SortField = "ExpenceNo")
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" EH.ID");
            cmdText.AppendLine(" ,EH.ExpenceNo");
            cmdText.AppendLine(" ,EH.Date");
            cmdText.AppendLine(" ,EH.AccountCD");
            cmdText.AppendLine(" ,MConfigD.Value2");
            cmdText.AppendLine(" ,EH.DepartmentID");
            cmdText.AppendLine(" ,Mdepartment.DepartmentName");
            cmdText.AppendLine(" ,EH.ProjectID");
            cmdText.AppendLine(" ,MProject.ProjectName");
            cmdText.AppendLine(" ,EH.ExpenceAmount");
            cmdText.AppendLine(" ,Muser.UserName1");
            cmdText.AppendLine(" ,Muser2.UserName1 AS UserName2");
            cmdText.AppendLine(" ,Muser3.UserName1 AS UserName3");
            cmdText.AppendLine(" ,EH.UserID");
            cmdText.AppendLine(" ,EH.UpdateUID");
            cmdText.AppendLine(" ,EH.UpdateDate");
            cmdText.AppendLine(" ,EH.Memo");
            if (SortDirec == "1")
            {
                cmdText.AppendLine(" ,ROW_NUMBER() OVER(ORDER BY " + SortField + " ASC) AS RowNumber");
            }
            else
            {
                cmdText.AppendLine(" ,ROW_NUMBER() OVER(ORDER BY " + SortField + " DESC) AS RowNumber");
            }


            cmdText.AppendLine(" FROM dbo.T_Expence_H AS EH");

            cmdText.AppendLine(" LEFT JOIN M_User AS Muser ON Muser.ID = EH.UserID");
            cmdText.AppendLine(" LEFT JOIN M_Department AS Mdepartment ON Mdepartment.ID = EH.DepartmentID");
            cmdText.AppendLine(" LEFT JOIN M_Project AS MProject ON MProject.ID = EH.ProjectID");
            cmdText.AppendLine(" LEFT JOIN M_User AS Muser3 ON Muser3.ID = MProject.UserID");
            cmdText.AppendLine(" LEFT JOIN M_User AS Muser2 ON Muser2.ID = EH.UpdateUID");
            cmdText.AppendLine("LEFT JOIN M_Config_H AS MConfigH ON MConfigH.ConfigCD = '" + M_Config_H.CONFIG_CD_EXPENCE_TYPE + "'");
            cmdText.AppendLine("LEFT JOIN M_Config_D AS MConfigD ON MConfigH.ID = MConfigD.HID AND EH.AccountCD= MConfigD.Value1");

            cmdText.AppendLine(" WHERE EXISTS(SELECT 1 FROM dbo.T_Expence_D ED");
            cmdText.AppendLine(" WHERE EH.ID = ED.HID ");
            cmdWhere.AppendLine(" AND EH.ID>=10 ");
            cmdWhere.AppendLine(string.Format(" AND EH.ApprovedFlag = {0}", approvedFlag));
            if (!IsExpenceAllApproved)
            {
                if (approvedFlag == 0)
                {
                    cmdWhere.AppendLine(string.Format(" AND ( EH.UserID = {0} OR MProject.UserID ={0} )", UserLogin));
                }
                else
                {
                    cmdWhere.AppendLine(string.Format(" AND ( EH.UserID = {0} OR  EH.UpdateUID ={0} )", UserLogin));
                }
            }
            //Parameter
            Hashtable paras = new Hashtable();

            if (departmentID != "-1")
            {
                cmdWhere.AppendLine(" AND Mdepartment.ID = @IN_DepartmentID");
                base.AddParam(paras, "IN_DepartmentID", EditDataUtil.ToFixCodeDB(departmentID, M_Department.DEPARTMENT_CODE_DB_MAX_LENGTH));
            }

            if (!string.IsNullOrEmpty(projectCD))
            {

                cmdWhere.AppendLine(" AND MProject.ProjectCD LIKE '%' + @IN_ProjectCD + '%'");
                base.AddParam(paras, "IN_ProjectCD", projectCD);
            }

            if (accountCD != "-1")
            {
                cmdWhere.AppendLine(" AND EH.AccountCD = @IN_AccountCD");
                base.AddParam(paras, "IN_AccountCD", accountCD);
            }

            if (userID != "-1")
            {
                cmdWhere.AppendLine(" AND Muser.ID = @IN_userID");
                base.AddParam(paras, "IN_userID", userID, true);
            }
            if (userUpdate != "-1")
            {
                if (approvedFlag == 1)
                {
                    cmdWhere.AppendLine(" AND Muser2.ID = @IN_userID2");
                    base.AddParam(paras, "IN_userID2", userUpdate, true);
                }
                else
                {
                    cmdWhere.AppendLine(" AND Muser3.ID = @IN_userID2");
                    base.AddParam(paras, "IN_userID2", userUpdate, true);
                }
            }

            if ((!string.IsNullOrEmpty(accountingDate1.ToString())) && (!string.IsNullOrEmpty(accountingDate2.ToString())))
            {
                cmdWhere.AppendLine("AND (EH.Date  >= @IN_Date1 AND EH.Date  <= @IN_Date2) ");
                base.AddParam(paras, "IN_Date1", accountingDate1, true);
                base.AddParam(paras, "IN_Date2", accountingDate2, true);
            }
            else if ((!string.IsNullOrEmpty(accountingDate1.ToString())) && (string.IsNullOrEmpty(accountingDate2.ToString())))
            {
                cmdWhere.AppendLine("AND (EH.Date  >= @IN_Date1) ");
                base.AddParam(paras, "IN_Date1", accountingDate1, true);
            }
            else if ((string.IsNullOrEmpty(accountingDate1.ToString())) && (!string.IsNullOrEmpty(accountingDate2.ToString())))
            {
                cmdWhere.AppendLine("AND (EH.Date  <= @IN_Date2) ");
                base.AddParam(paras, "IN_Date2", accountingDate2, true);
            }

            //
            if ((!string.IsNullOrEmpty(theDateOfUse1.ToString())) && (!string.IsNullOrEmpty(theDateOfUse2.ToString())))
            {
                cmdText.AppendLine("AND (ED.Date  >= @IN_DateOfUse1 AND ED.Date  <= @IN_DateOfUse2) ");
                base.AddParam(paras, "IN_DateOfUse1", theDateOfUse1, true);
                base.AddParam(paras, "IN_DateOfUse2", theDateOfUse2, true);
            }
            else if ((!string.IsNullOrEmpty(theDateOfUse1.ToString())) && (string.IsNullOrEmpty(theDateOfUse2.ToString())))
            {
                cmdText.AppendLine("AND (ED.Date  >= @IN_DateOfUse1) ");
                base.AddParam(paras, "IN_DateOfUse1", theDateOfUse1, true);
            }
            else if ((string.IsNullOrEmpty(theDateOfUse1.ToString())) && (!string.IsNullOrEmpty(theDateOfUse2.ToString())))
            {
                cmdText.AppendLine("AND (ED.Date  <= @IN_DateOfUse2) ");
                base.AddParam(paras, "IN_DateOfUse2", theDateOfUse2, true);
            }

            //Check SQL
            if (cmdWhere.Length != 0)
            {
                //cmdWhere.AppendLine(")");
                cmdText.AppendLine(")" + cmdWhere.ToString());
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

            return this.db.FindList1<ExpenceInfo>(cmdOutText.ToString(), paras);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="isAccountingDate"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public IList<ExpenceSeisanExcel> GetListByCond_SeisanExcel(string userID, bool isAccountingDate, DateTime? dateFrom, DateTime? dateTo, bool isSinsei)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" EH.ExpenceNo");
            if (!isAccountingDate)
            {
                cmdText.AppendLine(" ,ED.Date");
            }
            else
            {
                cmdText.AppendLine(" ,EH.Date");
            }
            cmdText.AppendLine(" ,MConfigD.Value2 As RouteType");
            cmdText.AppendLine(" ,MProject.ProjectName");
            cmdText.AppendLine(" ,Muser.UserName1 AS UserName");
            cmdText.AppendLine(" ,Muser.UserCD");
            cmdText.AppendLine(" ,EH.Memo");
            cmdText.AppendLine(" ,ED.RouteFrom");
            cmdText.AppendLine(" ,ED.RouteTo");
            cmdText.AppendLine(" ,ED.Amount");
            cmdText.AppendLine(" ,ED.PaidTo");
            cmdText.AppendLine(" FROM dbo.T_Expence_H AS EH");
            cmdText.AppendLine(" LEFT JOIN dbo.T_Expence_D AS ED ON ED.HID = EH.ID AND EH.ID >= 10");
            cmdText.AppendLine(" LEFT JOIN M_User AS Muser ON Muser.ID = EH.UserID");
            cmdText.AppendLine(" LEFT JOIN M_Project AS MProject ON MProject.ID = EH.ProjectID");
            cmdText.AppendLine(" LEFT JOIN M_Config_H AS MConfigH ON MConfigH.ConfigCD = '" + M_Config_H.CONFIG_CD_ROUTE_TYPE + "'");
            cmdText.AppendLine(" LEFT JOIN M_Config_D AS MConfigD ON MConfigH.ID = MConfigD.HID AND ED.RouteType= MConfigD.Value1");

            cmdText.AppendLine(" WHERE 1 = 1");

            //Parameter
            Hashtable paras = new Hashtable();

            if (userID != "-1")
            {
                cmdWhere.AppendLine(" AND Muser.ID = @IN_userID");
                base.AddParam(paras, "IN_userID", userID, true);
            }

            string _date = "EH.Date";
            if (!isAccountingDate)
            {
                _date = "ED.Date";
            }

            if ((!string.IsNullOrEmpty(dateFrom.ToString())) && (!string.IsNullOrEmpty(dateTo.ToString())))
            {
                cmdWhere.AppendLine("AND (" + _date + "  >= @IN_Date1 AND " + _date + "  <= @IN_Date2) ");
                base.AddParam(paras, "IN_Date1", dateFrom, true);
                base.AddParam(paras, "IN_Date2", dateTo, true);
            }
            else if ((!string.IsNullOrEmpty(dateFrom.ToString())) && (string.IsNullOrEmpty(dateTo.ToString())))
            {

                cmdWhere.AppendLine("AND (" + _date + "  >= @IN_Date1) ");
                base.AddParam(paras, "IN_Date1", dateFrom, true);
            }
            else if ((string.IsNullOrEmpty(dateFrom.ToString())) && (!string.IsNullOrEmpty(dateTo.ToString())))
            {
                cmdWhere.AppendLine("AND (" + _date + "  <= @IN_Date2) ");
                base.AddParam(paras, "IN_Date2", dateTo, true);
            }

            if (isSinsei)
            {
                cmdWhere.AppendLine(" AND EH.ApprovedFlag = 0");
            }
            else
            {
                cmdWhere.AppendLine(" AND EH.ApprovedFlag = 1");
            }

            //Check SQL
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.FindList1<ExpenceSeisanExcel>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// getTotalRow
        /// </summary>
        /// <param name="userCD"></param>
        /// <param name="departmentCD"></param>
        /// <param name="projectCD"></param>
        /// <param name="accountCD"></param>
        /// <returns></returns>
        public int getTotalRow(string userID, string userUpdate, string departmentID, string projectCD, string accountCD, DateTime? accountingDate1, DateTime? accountingDate2,
                               DateTime? theDateOfUse1, DateTime? theDateOfUse2, int approvedFlag, int UserLogin, bool IsExpenceAllApproved)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" COUNT(*)");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.T_Expence_H AS EH");

            cmdText.AppendLine(" LEFT JOIN M_User AS Muser ON Muser.ID = EH.UserID");
            cmdText.AppendLine(" LEFT JOIN M_User AS Muser2 ON Muser2.ID = EH.UpdateUID");
            cmdText.AppendLine(" LEFT JOIN M_Department AS Mdepartment ON Mdepartment.ID = EH.DepartmentID");
            cmdText.AppendLine(" LEFT JOIN M_Project AS MProject ON MProject.ID = EH.ProjectID");
            cmdText.AppendLine(" LEFT JOIN M_User AS Muser3 ON Muser3.ID = MProject.UserID");

            cmdText.AppendLine(" WHERE EXISTS(SELECT 1 FROM dbo.T_Expence_D ED");
            cmdText.AppendLine(" WHERE EH.ID = ED.HID");
            cmdText.AppendLine(" AND EH.ID >= 10");
            cmdText.AppendLine(string.Format(" AND EH.ApprovedFlag = {0}", approvedFlag));
            if (!IsExpenceAllApproved)
            {
                if (approvedFlag == 0)
                {
                    cmdWhere.AppendLine(string.Format(" AND ( EH.UserID = {0} OR  MProject.UserID = {0} )", UserLogin));
                }
                else
                {
                    cmdWhere.AppendLine(string.Format(" AND ( EH.UserID = {0} OR EH.UpdateUID = {0} )", UserLogin));
                }
            }
            //Parameter
            Hashtable paras = new Hashtable();

            if (userID != "-1")
            {
                cmdWhere.AppendLine(" AND Muser.ID = @IN_UserID");
                base.AddParam(paras, "IN_UserID", userID, true);
            }
            if (userUpdate != "-1")
            {
                if (approvedFlag == 1)
                {
                    cmdWhere.AppendLine(" AND Muser2.ID = @IN_UserID2");
                    base.AddParam(paras, "IN_UserID2", userUpdate, true);
                }
                else
                {
                    cmdWhere.AppendLine(" AND Muser3.ID = @IN_UserID2");
                    base.AddParam(paras, "IN_UserID2", userUpdate, true);
                }
            }

            if (departmentID != "-1")
            {
                cmdWhere.AppendLine(" AND Mdepartment.ID = @IN_DepartmentID");
                base.AddParam(paras, "IN_DepartmentID", EditDataUtil.ToFixCodeDB(departmentID, M_Department.DEPARTMENT_CODE_DB_MAX_LENGTH));
            }

            if (!string.IsNullOrEmpty(projectCD))
            {
                cmdWhere.AppendLine(" AND MProject.ProjectCD LIKE '%' + @IN_ProjectCD + '%'");
                base.AddParam(paras, "IN_ProjectCD", projectCD);
            }

            if (accountCD != "-1")
            {
                cmdWhere.AppendLine(" AND EH.AccountCD = @IN_AccountCD");
                base.AddParam(paras, "IN_AccountCD", accountCD);
            }
            if ((!string.IsNullOrEmpty(accountingDate1.ToString())) && (!string.IsNullOrEmpty(accountingDate2.ToString())))
            {
                cmdWhere.AppendLine("AND (EH.Date  >= @IN_Date1 AND EH.Date  <= @IN_Date2) ");
                base.AddParam(paras, "IN_Date1", accountingDate1, true);
                base.AddParam(paras, "IN_Date2", accountingDate2, true);
            }
            else if ((!string.IsNullOrEmpty(accountingDate1.ToString())) && (string.IsNullOrEmpty(accountingDate2.ToString())))
            {
                cmdWhere.AppendLine("AND (EH.Date  >= @IN_Date1) ");
                base.AddParam(paras, "IN_Date1", accountingDate1, true);
            }
            else if ((string.IsNullOrEmpty(accountingDate1.ToString())) && (!string.IsNullOrEmpty(accountingDate2.ToString())))
            {
                cmdWhere.AppendLine("AND (EH.Date  <= @IN_Date2) ");
                base.AddParam(paras, "IN_Date2", accountingDate2, true);
            }

            //
            if ((!string.IsNullOrEmpty(theDateOfUse1.ToString())) && (!string.IsNullOrEmpty(theDateOfUse2.ToString())))
            {
                cmdWhere.AppendLine("AND (ED.Date  >= @IN_DateOfUse1 AND ED.Date  <= @IN_DateOfUse2) ");
                base.AddParam(paras, "IN_DateOfUse1", theDateOfUse1, true);
                base.AddParam(paras, "IN_DateOfUse2", theDateOfUse2, true);
            }
            else if ((!string.IsNullOrEmpty(theDateOfUse1.ToString())) && (string.IsNullOrEmpty(theDateOfUse2.ToString())))
            {
                cmdWhere.AppendLine("AND (ED.Date  >= @IN_DateOfUse1) ");
                base.AddParam(paras, "IN_DateOfUse1", theDateOfUse1, true);
            }
            else if ((string.IsNullOrEmpty(theDateOfUse1.ToString())) && (!string.IsNullOrEmpty(theDateOfUse2.ToString())))
            {
                cmdWhere.AppendLine("AND (ED.Date  <= @IN_DateOfUse2) ");
                base.AddParam(paras, "IN_DateOfUse2", theDateOfUse2, true);
            }


            cmdWhere.AppendLine(")");
            cmdText.AppendLine(cmdWhere.ToString());


            return int.Parse(this.db.ExecuteScalar1(cmdText.ToString(), paras).ToString());
        }
        /// <summary>
        /// Get UserCD
        /// </summary>
        /// <returns></returns>
        public int GetUser(string ID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            //Parameter
            Hashtable paras = new Hashtable();
            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" UserCD");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.M_User");
            cmdText.AppendLine(" WHERE ID = @IN_ID");
            base.AddParam(paras, "IN_ID", ID, true);

            return int.Parse(this.db.ExecuteScalar1(cmdText.ToString(), paras).ToString());
        }
        /// <summary>
        /// Get Current ID 
        /// </summary>
        /// <returns></returns>
        public int GetCurrentID()
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" MAX(ExpenceNo)");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.T_Expence_H");

            return int.Parse(this.db.ExecuteScalar1(cmdText.ToString()).ToString());
        }

        #endregion

        #region Insert
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="t_Expence_H">T_Expence_H</param>
        /// <returns></returns>
        public int Insert(T_Expence_H t_Expence_H)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine(" INSERT INTO");
            cmdText.AppendLine(" dbo.T_Expence_H");
            cmdText.AppendLine(" (");
            cmdText.AppendLine("  ExpenceNo");
            cmdText.AppendLine(" ,Date");
            cmdText.AppendLine(" ,AccountCD");
            cmdText.AppendLine(" ,UserID");
            cmdText.AppendLine(" ,DepartmentID");
            cmdText.AppendLine(" ,ProjectID");
            cmdText.AppendLine(" ,ExpenceAmount");
            cmdText.AppendLine(" ,Memo");
            cmdText.AppendLine(" ,Filepath");
            cmdText.AppendLine(" ,ApprovedFlag");
            cmdText.AppendLine(" ,CreateDate");
            cmdText.AppendLine(" ,CreateUID");
            cmdText.AppendLine(" ,UpdateDate");
            cmdText.AppendLine(" ,UpdateUID");
            cmdText.AppendLine(" )");
            cmdText.AppendLine(" VALUES");
            cmdText.AppendLine(" (");
            cmdText.AppendLine("  (select right(REPLICATE('0',10) + cast((Select ISNULL(max(ExpenceNo)+1,'0000000001') from dbo.T_Expence_H) as varchar), 10))");
            cmdText.AppendLine(" ,@IN_Date");
            cmdText.AppendLine(" ,@IN_AccountCD");
            cmdText.AppendLine(" ,@IN_UserID");
            cmdText.AppendLine(" ,@IN_DepartmentID");
            cmdText.AppendLine(" ,@IN_ProjectID");
            cmdText.AppendLine(" ,@IN_ExpenceAmount");
            cmdText.AppendLine(" ,@IN_Memo");
            cmdText.AppendLine(" ,@IN_Filepath");
            cmdText.AppendLine(" ,@IN_ApprovedFlag");
            cmdText.AppendLine(" ,GETDATE()");
            cmdText.AppendLine(" ,@IN_CreateUID");
            cmdText.AppendLine(" ,GETDATE()");
            cmdText.AppendLine(" ,@IN_UpdateUID");
            cmdText.AppendLine(" )");

            //Para
            Hashtable paras = new Hashtable();
            ISecurity sec = Security.Instance;

            //base.AddParam(paras, "IN_ExpenceNo", EditDataUtil.ToFixCodeDB(t_Expence_H.ExpenceNo, M_User.USER_CODE_MAX_LENGTH), true);
            base.AddParam(paras, "IN_Date", t_Expence_H.Date);
            base.AddParam(paras, "IN_AccountCD", t_Expence_H.AccountCD);
            base.AddParam(paras, "IN_UserID", t_Expence_H.UserID);
            base.AddParam(paras, "IN_DepartmentID", t_Expence_H.DepartmentID);
            base.AddParam(paras, "IN_ProjectID", t_Expence_H.ProjectID);
            base.AddParam(paras, "IN_ExpenceAmount", t_Expence_H.ExpenceAmount);
            base.AddParam(paras, "IN_Memo", t_Expence_H.Memo);
            base.AddParam(paras, "IN_Filepath", t_Expence_H.Filepath);
            base.AddParam(paras, "IN_ApprovedFlag", t_Expence_H.ApprovedFlag);
            base.AddParam(paras, "IN_CreateUID", t_Expence_H.CreateUID);
            base.AddParam(paras, "IN_UpdateUID", t_Expence_H.UpdateUID);
            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }
        #endregion

        #region Update
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="T_Expence_H">T_Expence_H</param>
        /// <returns></returns>
        public int Update(T_Expence_H t_Expence_H)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" UPDATE");
            cmdText.AppendLine(" dbo.T_Expence_H");
            cmdText.AppendLine(" SET");
            cmdText.AppendLine("  ExpenceNo     = @IN_ExpenceNo");
            cmdText.AppendLine(" ,Date    = @IN_Date");
            cmdText.AppendLine(" ,AccountCD  = @IN_AccountCD");
            cmdText.AppendLine(" ,UserID  = @IN_UserID");
            cmdText.AppendLine(" ,DepartmentID  = @IN_DepartmentID");
            cmdText.AppendLine(" ,ProjectID  = @IN_ProjectID");
            cmdText.AppendLine(" ,ExpenceAmount = @IN_ExpenceAmount");
            cmdText.AppendLine(" ,Memo = @IN_Memo");
            cmdText.AppendLine(" ,UpdateDate = GETDATE()");
            cmdText.AppendLine(" ,UpdateUID  = @IN_UpdateUID");
            cmdText.AppendLine(" ,ApprovedFlag   = @IN_ApprovedFlag");
            //cmdText.AppendLine(" WHERE ID = @IN_ID");

            //Para
            Hashtable paras = new Hashtable();

            base.AddParam(paras, "IN_ID", t_Expence_H.ID);
            base.AddParam(paras, "IN_ExpenceNo", EditDataUtil.ToFixCodeDB(t_Expence_H.ExpenceNo, M_User.USER_CODE_MAX_LENGTH), true);
            base.AddParam(paras, "IN_Date", t_Expence_H.Date);
            base.AddParam(paras, "IN_AccountCD", t_Expence_H.AccountCD);
            base.AddParam(paras, "IN_UserID", t_Expence_H.UserID);
            base.AddParam(paras, "IN_DepartmentID", t_Expence_H.DepartmentID);
            base.AddParam(paras, "IN_ProjectID", t_Expence_H.ProjectID);
            base.AddParam(paras, "IN_ExpenceAmount", t_Expence_H.ExpenceAmount);
            base.AddParam(paras, "IN_Memo", t_Expence_H.Memo);
            base.AddParam(paras, "IN_UpdateDate", t_Expence_H.UpdateDate);
            base.AddParam(paras, "IN_UpdateUID", t_Expence_H.UpdateUID);
            base.AddParam(paras, "IN_ApprovedFlag", t_Expence_H.ApprovedFlag);

            cmdWhere.AppendLine(" ID = @IN_ID AND UpdateDate = @IN_UpdateDate");

            //Check SQL
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="T_Mail_H">T_Mail_H</param>
        /// <returns></returns>
        public int UpdateDate(int HID, int UID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine(" UPDATE");
            cmdText.AppendLine(" dbo.T_Mail_H");
            cmdText.AppendLine(" SET");
            cmdText.AppendLine(" UpdateDate = GETDATE()");
            cmdText.AppendLine(" ,UpdateUID  = @IN_UpdateUID");
            cmdText.AppendLine(" WHERE ID = @IN_ID");

            //Para
            Hashtable paras = new Hashtable();

            base.AddParam(paras, "IN_ID", HID);
            base.AddParam(paras, "IN_UpdateUID", UID);

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="T_Mail_H">T_Mail_H</param>
        /// <returns></returns>
        public int UpdateFlgDraft(int HID, int UID, int draftFlag)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine(" UPDATE");
            cmdText.AppendLine(" dbo.T_Mail_H");
            cmdText.AppendLine(" SET");
            cmdText.AppendLine(" DraftFlag = @IN_DraftFlag");
            cmdText.AppendLine(" ,UpdateDate = GETDATE()");
            cmdText.AppendLine(" ,UpdateUID  = @IN_UpdateUID");
            cmdText.AppendLine(" WHERE ID = @IN_ID");

            //Para
            Hashtable paras = new Hashtable();

            base.AddParam(paras, "IN_DraftFlag", draftFlag);
            base.AddParam(paras, "IN_ID", HID);
            base.AddParam(paras, "IN_UpdateUID", UID);

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }

        #endregion


        #region Delete By Id
        /// <summary>
        /// Delete T_Expence_H By Id 
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        /// 
        public int Delete(int expenceID, DateTime updateDate)
        {
            //SQL String 
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine("DELETE [dbo].[T_Expence_H]");

            //Parameter
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine("ID = @IN_ID AND UpdateDate = @IN_UpdateDate");

            base.AddParam(paras, "IN_ID", expenceID);
            base.AddParam(paras, "IN_UpdateDate", updateDate, true);
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
