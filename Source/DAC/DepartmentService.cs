using System;
using System.Collections;
using System.Collections.Generic;

using OMS.Models;
using OMS.Utilities;
using System.Text;

namespace OMS.DAC
{
    /// Class DepartmentService DAC
    /// Create Date: 2017/11/3
    /// Create Author: ISV-Giao
    public class DepartmentService : BaseService
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        private DepartmentService()
            : base()
        {
        }

        /// <summary>
        /// Constructor with param
        /// </summary>
        /// <param name="db">Database</param>
        public DepartmentService(DB db)
            : base(db)
        {
        }
        #endregion

        #region Get Data


        public IList<DepartmentInfo> GetListByCond(string departmentCD, string departmentName, string departmentName2,                                                                                           
                                                int pageIndex, int pageSize, int sortField, int sortDirec)
        {
            string[] fields = new string[] { "UpdateDate", "", "DepartmentCD", "DepartmentName", "DepartmentName2"};
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
            cmdText.AppendLine(" ID");
            cmdText.AppendLine(" ,DepartmentCD");
            cmdText.AppendLine(" ,DepartmentName");
            cmdText.AppendLine(" ,DepartmentName2");

            cmdText.AppendLine(" ," + string.Format("ROW_NUMBER() OVER(ORDER BY {0}) AS RowNumber", RowNumber));
            cmdText.AppendLine(" FROM dbo.M_Department AS pr");
            cmdWhere.AppendLine(" pr.ID >= 10");

            //Parameter
            Hashtable paras = new Hashtable();

            if (!string.IsNullOrEmpty(departmentCD))
            {

                cmdWhere.AppendLine("AND pr.DepartmentCD LIKE '%' + @IN_DepartmentCD + '%'");
                base.AddParam(paras, "IN_DepartmentCD", departmentCD);
            }

            if (!string.IsNullOrEmpty(departmentName))
            {
                cmdWhere.AppendLine("AND pr.DepartmentName LIKE '%' + @IN_DepartmentName + '%'");
                base.AddParam(paras, "IN_DepartmentName", departmentName);
            }

            if (!string.IsNullOrEmpty(departmentName2))
            {
                cmdWhere.AppendLine("AND pr.DepartmentName2 LIKE '%' + @IN_DepartmentName2 + '%'");
                base.AddParam(paras, "IN_DepartmentName2", departmentName2);
            }
         
            //Check SQL
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE");
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
            return this.db.FindList1<DepartmentInfo>(cmdOutText.ToString(), paras);
        }


        /// <summary>
        /// Get Data Project
        /// </summary>
        /// <returns></returns>
        public IList<M_Department> GetAll()
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();
            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" *");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.M_Department");
            cmdText.AppendLine(" WHERE ID >= 10");

            return this.db.FindList1<M_Department>(cmdText.ToString());
        }

        /// <summary>
        /// Get Data Project
        /// </summary>
        /// <returns></returns>
        public IList<DropDownModel> GetDepartmentCbbData(int calendarID = -1, int departmentId = -1)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            Hashtable paras = new Hashtable();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" de.ID AS Value");
            cmdText.AppendLine(" ,de.DepartmentName AS DisplayName");
            cmdText.AppendLine(" FROM dbo.M_Department AS de");
            cmdText.AppendLine(" WHERE EXISTS (SELECT us.DepartmentID FROM dbo.M_User AS us WHERE de.ID = us.DepartmentID");
            if (calendarID != -1)
            {
                cmdText.AppendLine("AND EXISTS(SELECT 1 FROM T_WorkingCalendar_U WU WHERE WU.HID = @IN_CalendarID AND WU.UID = us.ID)");
                base.AddParam(paras, "IN_CalendarID", calendarID);
            }

            if (departmentId != -1)
            {
                cmdText.AppendLine("AND de.ID = @IN_DepartmentID");
                base.AddParam(paras, "IN_DepartmentID", departmentId);
            }

            cmdText.AppendLine(" )");

            cmdText.AppendLine(" AND de.ID >= 10");
            cmdText.AppendLine(" ORDER BY de.DepartmentCD");

            return this.db.FindList1<DropDownModel>(cmdText.ToString(), paras);
        }       

        /// <summary>
        /// getTotalRowForList
        /// </summary>
        /// <param name="projectCD"></param>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public int getTotalRowForList(string departmentCD, string departmentName, string departmentName2)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();
            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" COUNT(*)");
            cmdText.AppendLine(" FROM dbo.M_Department AS pr");
            
            cmdWhere.AppendLine(" pr.ID >= 10");

            //Parameter
            Hashtable paras = new Hashtable();
            if (!string.IsNullOrEmpty(departmentCD))
            {
                cmdWhere.AppendLine("AND pr.DepartmentCD LIKE '%' +  @IN_DepartmentCD + '%'");
                base.AddParam(paras, "IN_DepartmentCD", departmentCD);
            }

            if (!string.IsNullOrEmpty(departmentName))
            {
                cmdWhere.AppendLine("AND pr.DepartmentName LIKE '%' + @IN_DepartmentName + '%'");
                base.AddParam(paras, "IN_DepartmentName", departmentName);
            }

            if (!string.IsNullOrEmpty(departmentName2))
            {
                cmdWhere.AppendLine("AND pr.DepartmentName2 LIKE '%' + @IN_DepartmentName2 + '%'");
                base.AddParam(paras, "IN_DepartmentName2", departmentName2);
            }

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE");
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
        public IList<DepartmentSearchInfo> GetListByConditionForSearch(string departmentCD, string departmentName,
                                                                        int pageIndex, int pageSize, int sortField, int sortDirec)
        {
            string[] fields = new string[] {"UpdateDate", "", "DepartmentCD", "DepartmentName" };
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
            cmdText.AppendLine(" FROM dbo.M_Department AS G");

            cmdWhere.AppendLine(" ID >= 10");

            //Para
            Hashtable paras = new Hashtable();

            if (!(string.IsNullOrEmpty(departmentCD)))
            {
                cmdWhere.AppendLine(" AND UPPER(G.DepartmentCD) LIKE UPPER(@IN_DepartmentCD) + '%'");
                base.AddParam(paras, "IN_DepartmentCD", departmentCD);
            }

            if (!(string.IsNullOrEmpty(departmentName)))
            {
                cmdWhere.AppendLine(" AND (UPPER(G.DepartmentName) LIKE '%' + UPPER(@IN_DepartmentName) + '%' OR @IN_DepartmentName IS NULL)");
                base.AddParam(paras, "IN_DepartmentName", departmentName);
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
            return this.db.FindList1<DepartmentSearchInfo>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Get Count By Condition For Search
        /// </summary>
        /// <param name="departmentCD"></param>
        /// <param name="departmentName"></param>
        /// <returns></returns>
        public int GetCountByConditionForSearch(string departmentCD, string departmentName)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  count(*)");
            cmdText.AppendLine(" FROM dbo.M_Department AS G");

            //Para
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine(" ID >= 10");

            if (!string.IsNullOrEmpty(departmentCD))
            {
                cmdWhere.AppendLine(" AND UPPER(G.DepartmentCD) LIKE UPPER(@IN_DepartmentCD) + '%'");
                base.AddParam(paras, "IN_DepartmentCD", departmentCD);
            }

            if (!string.IsNullOrEmpty(departmentName))
            {
                cmdWhere.AppendLine(" AND UPPER(G.DepartmentName) LIKE '%' + UPPER(@IN_DepartmentName) + '%'	");
                base.AddParam(paras, "IN_DepartmentName", departmentName);
            }

            //Check SQL
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return int.Parse(this.db.ExecuteScalar1(cmdText.ToString(), paras).ToString());
        }
        #endregion

        #region Entity
        /// <summary>
        /// Get By M_Department Cd
        /// </summary>
        /// <returns>M_Department</returns>
        public M_Department GetByDepartmentCd(string departmentCd)
        {
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  ID");

            cmdText.AppendLine(" ,DepartmentCD ");
            cmdText.AppendLine(" ,DepartmentName ");
            cmdText.AppendLine(" ,DepartmentName2 ");
            cmdText.AppendLine(" ,CreateDate ");
            cmdText.AppendLine(" ,CreateUID ");
            cmdText.AppendLine(" ,UpdateDate ");
            cmdText.AppendLine(" ,UpdateUID ");

            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.M_Department");

            //Para
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine(" RTRIM(DepartmentCD) = RTRIM(@IN_DepartmentCD)");
            base.AddParam(paras, "IN_DepartmentCD", EditDataUtil.ToFixCodeDB(departmentCd, M_Department.DEPARTMENT_CODE_DB_MAX_LENGTH));
            cmdWhere.AppendLine(" AND ID >= 10");

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.Find1<M_Department>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Get M_Department By Id
        /// </summary>
        /// <param name="projectId">departmentId</param>
        /// <returns>M_Department</returns>
        public M_Department GetDataDepartmentById(int departmentId)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  ID");

            cmdText.AppendLine(" ,DepartmentCD ");
            cmdText.AppendLine(" ,DepartmentName ");
            cmdText.AppendLine(" ,DepartmentName2 ");

            cmdText.AppendLine(" ,CreateDate ");
            cmdText.AppendLine(" ,CreateUID ");
            cmdText.AppendLine(" ,UpdateDate ");
            cmdText.AppendLine(" ,UpdateUID ");

            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.M_Department");

            cmdWhere.AppendLine(" ID = @IN_DepartmentID");

            //Para
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_DepartmentID", departmentId);

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.Find1<M_Department>(cmdText.ToString(), paras);
        }

        #endregion

        #region Insert
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="user">M_Department</param>
        /// <returns></returns>
        public int Insert(M_Department m_Department)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine(" INSERT INTO dbo.M_Department");
            cmdText.AppendLine(" (");

            cmdText.AppendLine("  DepartmentCD ");
            cmdText.AppendLine(" ,DepartmentName ");
            cmdText.AppendLine(" ,DepartmentName2 ");

            cmdText.AppendLine(" ,CreateDate ");
            cmdText.AppendLine(" ,CreateUID ");
            cmdText.AppendLine(" ,UpdateDate ");
            cmdText.AppendLine(" ,UpdateUID ");

            cmdText.AppendLine(" )");
            cmdText.AppendLine(" VALUES");
            cmdText.AppendLine(" (");

            cmdText.AppendLine("  @IN_DepartmentCD ");
            cmdText.AppendLine(" ,@IN_DepartmentName ");
            cmdText.AppendLine(" ,@IN_DepartmentName2 ");

            cmdText.AppendLine(" ,GETDATE() ");
            cmdText.AppendLine(" ,@IN_CreateUID ");
            cmdText.AppendLine(" ,GETDATE() ");
            cmdText.AppendLine(" ,@IN_UpdateUID ");
            cmdText.AppendLine(" )");

            //Para
            Hashtable paras = new Hashtable();

            base.AddParam(paras, "IN_DepartmentCD", EditDataUtil.ToFixCodeDB(m_Department.DepartmentCD, M_Department.DEPARTMENT_CODE_DB_MAX_LENGTH));
            base.AddParam(paras, "IN_DepartmentName", m_Department.DepartmentName);
            base.AddParam(paras, "IN_DepartmentName2", m_Department.DepartmentName2);

            base.AddParam(paras, "IN_CreateUID", m_Department.CreateUID);
            base.AddParam(paras, "IN_UpdateUID", m_Department.UpdateUID);

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }
        #endregion

        #region Update
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="m_Project">m_Department</param>
        /// <returns></returns>
        public int Update(M_Department m_Department)
        {
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" UPDATE dbo.M_Department");
            cmdText.AppendLine(" SET");
            cmdText.AppendLine("  DepartmentCD = @IN_DepartmentCD ");
            cmdText.AppendLine(" ,DepartmentName = @IN_DepartmentName ");
            cmdText.AppendLine(" ,DepartmentName2 = @IN_DepartmentName2 ");

            cmdText.AppendLine(" ,UpdateDate = GETDATE()");
            cmdText.AppendLine(" ,UpdateUID	 = @IN_UpdateUID");

            //Para
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_DepartmentID", m_Department.ID);

            base.AddParam(paras, "IN_DepartmentCD", EditDataUtil.ToFixCodeDB(m_Department.DepartmentCD, M_Department.DEPARTMENT_CODE_DB_MAX_LENGTH));
            base.AddParam(paras, "IN_DepartmentName", m_Department.DepartmentName);
            base.AddParam(paras, "IN_DepartmentName2", m_Department.DepartmentName2);

            base.AddParam(paras, "IN_UpdateDate", m_Department.UpdateDate);
            base.AddParam(paras, "IN_UpdateUID", m_Department.UpdateUID);

            cmdWhere.AppendLine(" ID = @IN_DepartmentID AND UpdateDate = @IN_UpdateDate");

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
        /// DeleteDepartmentByID
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public int DeleteDepartmentByID(int DepartmentID)
        {
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" DELETE	FROM dbo.M_Department");

            //Params
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine(" ID = @IN_DepartmentID");
            base.AddParam(paras, "IN_DepartmentID", DepartmentID);


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