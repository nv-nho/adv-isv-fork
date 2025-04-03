using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using OMS.Models;


namespace OMS.DAC
{
    /// <summary>
    /// Class Cost_HService DAC
    /// </summary>
    public class Cost_HService : BaseService
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public Cost_HService() : base()
        {
        }

        /// <summary>
        /// Constructor with param
        /// </summary>
        /// <param name="db">Database</param>
        public Cost_HService(DB db) : base(db)
        {

        }
        #endregion

        #region Get data
        /// <summary>
        /// Get List by condition
        /// </summary>
        /// <param name="costCode">Cost Code</param>
        /// <param name="costName">Cost Name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="sortField">Sort Field</param>
        /// <param name="sortDirec">Sort Direction</param>
        /// <returns>List Setting Info</returns>
        public IList<CostInfo> GetList(string costCode, string costName,
                                            int pageIndex, int pageSize, int sortField, int sortDirec)
        {
            string[] fields = new string[] { "", "", "CostName", "EffectDate", "ExpireDate", "CostAmount" };
            string[] direc = new string[] { "ASC", "DESC" };


            string RowNumber = fields[sortField - 1] + " " + direc[sortDirec - 1];
            if (sortField == 1)
            {
                RowNumber = fields[sortField - 1] + " " + direc[1];
            }

            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT H.*, D.* ");
            cmdText.AppendLine(" ," + string.Format("ROW_NUMBER() OVER(ORDER BY {0}) AS RowNumber", RowNumber));
            cmdText.AppendLine(" FROM dbo.T_Cost_H AS H");
            cmdText.AppendLine(" INNER JOIN dbo.T_Cost_D AS D");

            cmdText.AppendLine(" ON ");
            cmdText.AppendLine(" ( ");
            cmdText.AppendLine(" H.ID = D.HID ");
            cmdText.AppendLine(" AND CONVERT(VARCHAR,D.EffectDate,112)<=CONVERT(VARCHAR,GETDATE(),112) ");
            cmdText.AppendLine(" AND CONVERT(VARCHAR,D.ExpireDate,112)>=CONVERT(VARCHAR,GETDATE(),112) ");
            cmdText.AppendLine(" ) ");

            //Para
            Hashtable paras = new Hashtable();
            if (!string.IsNullOrEmpty(costCode))
            {
                cmdWhere.AppendLine(" UPPER(S.CostCode) LIKE '%' + UPPER(@IN_CostCode) + '%'");
                base.AddParam(paras, "IN_CostCode", costCode, true);
            }

            if (!string.IsNullOrEmpty(costName))
            {
                if (cmdWhere.Length != 0)
                {
                    cmdWhere.AppendLine(" AND");
                }
                cmdWhere.AppendLine(" UPPER(S.CostName) LIKE '%' + UPPER(@IN_CostName) + '%'");
                base.AddParam(paras, "IN_CostName", costName, true);
            }

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            StringBuilder cmdOutText = new StringBuilder();
            StringBuilder cmdOutWhere = new StringBuilder();
            cmdOutText.AppendLine(" SELECT * ");
            cmdOutText.AppendLine(" FROM");
            cmdOutText.AppendLine(" (");
            cmdOutText.AppendLine(cmdText.ToString());
            cmdOutText.AppendLine(" ) AS  VIEW_1");

            cmdOutWhere.AppendLine(" RowNumber BETWEEN(@IN_PageIndex -1) * @IN_PageSize + 1 AND(((@IN_PageIndex -1) * @IN_PageSize + 1) + @IN_PageSize) - 1 ");
            base.AddParam(paras, "IN_PageIndex", pageIndex);
            base.AddParam(paras, "IN_PageSize", pageSize);
            base.AddParam(paras, "IN_SortField", sortField);
            base.AddParam(paras, "IN_SortDirec", sortDirec);
            if (cmdOutWhere.Length != 0)
            {
                cmdOutText.AppendLine(" WHERE ");
                cmdOutText.AppendLine(cmdOutWhere.ToString());
            }

            return this.db.FindList1<CostInfo>(cmdOutText.ToString(), paras);
        }

        /// <summary>
        /// Get total row
        /// </summary>
        /// <param name="costCode">CostCode</param>
        /// <param name="costName">CostName</param>
        /// <returns></returns>
        public int GetTotalRow(string costCode, string costName)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();
            cmdText.AppendLine(" SELECT ");
            cmdText.AppendLine("  count(*)");
            cmdText.AppendLine(" FROM dbo.T_Cost_H AS S");

            //Para
            Hashtable paras = new Hashtable();
            if (!string.IsNullOrEmpty(costCode))
            {
                cmdWhere.AppendLine(" UPPER(S.CostCode) LIKE '%' + UPPER(@IN_CostCode) + '%'");
                base.AddParam(paras, "IN_CostCode", costCode, true);
            }

            if (!string.IsNullOrEmpty(costName))
            {
                if (cmdWhere.Length != 0)
                {
                    cmdWhere.AppendLine(" AND");
                }
                cmdWhere.AppendLine(" UPPER(S.CostName) LIKE '%' + UPPER(@IN_CostName) + '%'");
                base.AddParam(paras, "IN_CostName", costName, true);
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
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public T_Cost_H GetByID(int id)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine(" SELECT H.* ");
            cmdText.AppendLine(" FROM dbo.T_Cost_H AS H ");
            cmdText.AppendLine(" WHERE H.ID = @IN_ID ");

            //Param
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_ID", id);

            return this.db.Find1<T_Cost_H>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Get by ID and Date
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="baseDate">baseDate</param>
        /// <returns></returns>
        public T_Cost_H GetByIDAndDate(int id, DateTime baseDate)
        {
            //SQL String
            string cmdText = "P_T_Cost_H_GetByIDAndDate_W";

            //Param
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_ID", id);
            base.AddParam(paras, "IN_BaseDate", baseDate);

            return this.db.Find<T_Cost_H>(cmdText, paras);
        }
        #endregion

        #region Check data
        #endregion

        #region Update
        /// <summary>
        /// Update data
        /// </summary>
        /// <param name="header">T_Cost_H</param>
        /// <returns></returns>
        public int Update(T_Cost_H header)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" UPDATE dbo.T_Cost_H ");
            cmdText.AppendLine(" SET ");
            cmdText.AppendLine("  UpdateDate = GETDATE() ");
            cmdWhere.AppendLine(" ID = @IN_ID AND UpdateDate = @IN_UpdateDate ");

            //Param
            Hashtable paras = new Hashtable();

            base.AddParam(paras, "IN_ID", header.ID);
            base.AddParam(paras, "IN_UpdateDate", header.UpdateDate, true);

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