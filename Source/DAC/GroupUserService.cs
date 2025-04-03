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
    /// class GroupUser DAC
    /// ISV-TRUC
    /// </summary>
    public class GroupUserService: BaseService
    {
        #region Constructor
        private GroupUserService() :base()
        {

        }

        public GroupUserService(DB db):base(db)
        {
        }
        #endregion

        #region Get Data

        #region Entity

        /// <summary>
        /// Get by Group CD
        /// </summary>
        /// <returns>M_GroupUser</returns>
        public M_GroupUser_H GetByGroupCD(string groupCD)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  ID");
            cmdText.AppendLine(" ,GroupCD");
            cmdText.AppendLine(" ,GroupName");
            cmdText.AppendLine(" ,CreateDate");
            cmdText.AppendLine(" ,CreateUID");
            cmdText.AppendLine(" ,UpdateDate");
            cmdText.AppendLine(" ,UpdateUID");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.M_GroupUser_H");

            //Para
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine(" RTRIM(GroupCD) = RTRIM(@IN_GroupCD)");
            base.AddParam(paras, "IN_GroupCD", EditDataUtil.ToFixCodeDB(groupCD, M_GroupUser_H.GROUP_CODE_DB_MAX_LENGTH));

            cmdWhere.AppendLine(" AND ID >= 10");

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.Find1<M_GroupUser_H>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Get By Group ID
        /// </summary>
        /// <param name="groupID">groupID</param>
        /// <returns></returns>
        public M_GroupUser_H GetByGroupID(int groupID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  ID");
            cmdText.AppendLine(" ,GroupCD");
            cmdText.AppendLine(" ,GroupName");
            cmdText.AppendLine(" ,CreateDate");
            cmdText.AppendLine(" ,CreateUID");
            cmdText.AppendLine(" ,UpdateDate");
            cmdText.AppendLine(" ,UpdateUID");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.M_GroupUser_H");

            cmdWhere.AppendLine(" ID = @IN_GroupID");

            //Para
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_GroupID", groupID);

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.Find1<M_GroupUser_H>(cmdText.ToString(), paras);
        }

        #endregion

        #region List Entity

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupCD"></param>
        /// <param name="groupName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="sortDirec"></param>
        /// <returns></returns>
        public IList<GroupUserInfo> GetListByCond(string groupCD, string groupName,
                                                  int pageIndex, int pageSize, int sortField, int sortDirec)
        {
            string[] fields = new string[] { "", "", "GroupCD", "GroupName" };
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
            cmdText.AppendLine(" FROM dbo.M_GroupUser_H AS G");

            cmdWhere.AppendLine(" G.ID >= 10");

            //Para
            Hashtable paras = new Hashtable();

            if (!string.IsNullOrEmpty(groupCD))
            {
                cmdWhere.AppendLine(" AND UPPER(G.GroupCD) =  UPPER(@IN_GroupCode)");
                base.AddParam(paras, "IN_GroupCode", EditDataUtil.ToFixCodeDB(groupCD, M_GroupUser_H.GROUP_CODE_DB_MAX_LENGTH), true);
            }

            if (!string.IsNullOrEmpty(groupName))
            {
                cmdWhere.AppendLine(" AND UPPER(G.GroupName) LIKE '%' + UPPER(@IN_GroupName) + '%'");
                base.AddParam(paras, "IN_GroupName", groupName, true);
            }

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
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

            return this.db.FindList1<GroupUserInfo>(cmdOutText.ToString(), paras);
        }

        /// <summary>
        /// Get List By Condition For Search
        /// </summary>
        /// <param name="groupCD"></param>
        /// <param name="groupName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="sortDirec"></param>
        /// <returns></returns>
        public IList<GroupUserSearchInfo> GetListByConditionForSearch(string groupCD, string groupName,
                                                                        int pageIndex, int pageSize, int sortField, int sortDirec)
        {
            string[] fields = new string[] { "UpdateDate", "", "GroupCD", "GroupName" };
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
            cmdText.AppendLine(" FROM dbo.M_GroupUser_H AS G");

            cmdWhere.AppendLine(" ID >= 10");

            //Para
            Hashtable paras = new Hashtable();

            if (!(string.IsNullOrEmpty(groupCD)))
            {
                cmdWhere.AppendLine(" AND UPPER(G.GroupCD) LIKE UPPER(@IN_GroupCD) + '%'");
                base.AddParam(paras, "IN_GroupCD", groupCD, true);
            }

            if (!(string.IsNullOrEmpty(groupName)))
            {
                cmdWhere.AppendLine(" AND (UPPER(G.GroupName) LIKE '%' + UPPER(@IN_GroupName) + '%' OR @IN_GroupName IS NULL)");
                base.AddParam(paras, "IN_GroupName", groupName, true);
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
            return this.db.FindList1<GroupUserSearchInfo>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Get List Group User Detail
        /// </summary>
        /// <returns></returns>
        public IList<M_GroupUser_D> GetListGroupUserDetail(int groupID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  GroupID");
            cmdText.AppendLine(" ,FormID");
            cmdText.AppendLine(" ,FormName");
            cmdText.AppendLine(" ,AuthorityFlag1");
            cmdText.AppendLine(" ,AuthorityFlag2");
            cmdText.AppendLine(" ,AuthorityFlag3");
            cmdText.AppendLine(" ,AuthorityFlag4");
            cmdText.AppendLine(" ,AuthorityFlag5");
            cmdText.AppendLine(" ,AuthorityFlag6");
            cmdText.AppendLine(" ,AuthorityFlag7");
            cmdText.AppendLine(" ,AuthorityFlag8");
            cmdText.AppendLine(" ,AuthorityFlag9");
            cmdText.AppendLine(" ,AuthorityFlag10");
            cmdText.AppendLine(" ,AuthorityFlag11");
            cmdText.AppendLine(" ,AuthorityFlag12");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.M_GroupUser_D");
            cmdWhere.AppendLine(" GroupID = @IN_GroupID");
           
            //Para
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_GroupID", groupID);

            if (cmdText.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            cmdText.AppendLine(" ORDER BY");
            cmdText.AppendLine(" [GroupID],[FormID]");

            return this.db.FindList1<M_GroupUser_D>(cmdText.ToString(), paras);
        }

        #endregion

        #region Get Total Row

        /// <summary>
        /// Get Count By Condition For Search
        /// </summary>
        /// <param name="groupCD"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public int GetCountByConditionForSearch(string groupCD, string groupName)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  count(*)");
            cmdText.AppendLine(" FROM dbo.M_GroupUser_H AS G");

            //Para
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine(" ID >= 10");

            if (!string.IsNullOrEmpty(groupCD))
            {
                cmdWhere.AppendLine(" AND UPPER(G.GroupCD) LIKE UPPER(@IN_GroupCD) + '%'");
                base.AddParam(paras, "IN_GroupCD", groupCD, true);
            }

            if (!string.IsNullOrEmpty(groupName))
            {
                cmdWhere.AppendLine(" AND UPPER(G.GroupName) LIKE '%' + UPPER(@IN_GroupName) + '%'	");
                base.AddParam(paras, "IN_GroupName", groupName, true);
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
        /// 
        /// </summary>
        /// <param name="groupCD"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public int getTotalRowForList(string groupCD, string groupName)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  count(*)");
            cmdText.AppendLine(" FROM dbo.M_GroupUser_H AS G");

            //Para
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine(" G.ID >= 10");

            if (!string.IsNullOrEmpty(groupCD))
            {
                cmdWhere.AppendLine(" AND UPPER(G.GroupCD) LIKE '%' + UPPER(@IN_GroupCode) + '%'");
                base.AddParam(paras, "IN_GroupCode", EditDataUtil.ToFixCodeDB(groupCD, M_GroupUser_H.GROUP_CODE_DB_MAX_LENGTH), true);
            }

            if (!string.IsNullOrEmpty(groupName))
            {
                cmdWhere.AppendLine(" AND UPPER(G.GroupName) LIKE '%' + UPPER(@IN_GroupName) + '%' ");
                base.AddParam(paras, "IN_GroupName", groupName, true);
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

        public M_GroupUser_D GetByGroupAndFormID(int groupID, int formID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  *");
            cmdText.AppendLine(" FROM dbo.M_GroupUser_D");


            //Para
            Hashtable paras = new Hashtable();

            cmdWhere.AppendLine(" GroupID = @IN_GroupID ");
            base.AddParam(paras, "IN_GroupID", groupID);

            cmdWhere.AppendLine(" AND FormID = @IN_FormID ");
            base.AddParam(paras, "IN_FormID", formID);

            //Check SQL
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.Find1<M_GroupUser_D>(cmdText.ToString(), paras);
        }
        #endregion

        #region Insert

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="user">M_GroupUser</param>
        /// <returns></returns>
        public int Insert(M_GroupUser_H m_GroupUser)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            
            cmdText.AppendLine(" INSERT INTO dbo.M_GroupUser_H");
            cmdText.AppendLine(" (");
            cmdText.AppendLine("  GroupCD ");
            cmdText.AppendLine(" ,GroupName ");
            cmdText.AppendLine(" ,CreateDate ");
            cmdText.AppendLine(" ,CreateUID ");
            cmdText.AppendLine(" ,UpdateDate ");
            cmdText.AppendLine(" ,UpdateUID ");
            cmdText.AppendLine(" )");
            cmdText.AppendLine(" VALUES");
            cmdText.AppendLine(" (");
            cmdText.AppendLine("  @IN_GroupCD ");
            cmdText.AppendLine(" ,@IN_GroupName ");
            cmdText.AppendLine(" ,GETDATE() ");
            cmdText.AppendLine(" ,@IN_CreateUID ");
            cmdText.AppendLine(" ,GETDATE() ");
            cmdText.AppendLine(" ,@IN_UpdateUID ");
            cmdText.AppendLine(" )");

            //Para
            Hashtable paras = new Hashtable();
            base.AddParam(paras,"IN_GroupCD", EditDataUtil.ToFixCodeDB(m_GroupUser.GroupCD, M_GroupUser_H.GROUP_CODE_DB_MAX_LENGTH));
            base.AddParam(paras,"IN_GroupName", m_GroupUser.GroupName);

            base.AddParam(paras,"IN_CreateUID", m_GroupUser.CreateUID);
            base.AddParam(paras,"IN_UpdateUID", m_GroupUser.UpdateUID);

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="m_GroupUser">M_GroupUser_M</param>
        /// <returns></returns>
        public int Insert(M_GroupUser_D m_GroupUser)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine(" INSERT INTO [dbo].[M_GroupUser_D]");
            cmdText.AppendLine(" (");
            cmdText.AppendLine("  GroupID ");
            cmdText.AppendLine(" ,FormID ");
            cmdText.AppendLine(" ,FormName ");
            cmdText.AppendLine(" ,AuthorityFlag1 ");
            cmdText.AppendLine(" ,AuthorityFlag2 ");
            cmdText.AppendLine(" ,AuthorityFlag3 ");
            cmdText.AppendLine(" ,AuthorityFlag4 ");
            cmdText.AppendLine(" ,AuthorityFlag5 ");
            cmdText.AppendLine(" ,AuthorityFlag6 ");
            cmdText.AppendLine(" ,AuthorityFlag7 ");
            cmdText.AppendLine(" ,AuthorityFlag8 ");
            cmdText.AppendLine(" ,AuthorityFlag9 ");
            cmdText.AppendLine(" ,AuthorityFlag10 ");
            cmdText.AppendLine(" ,AuthorityFlag11 ");
            cmdText.AppendLine(" ,AuthorityFlag12 ");
            cmdText.AppendLine(" )");
            cmdText.AppendLine(" VALUES");
            cmdText.AppendLine(" (");
            cmdText.AppendLine("  @IN_GroupID ");
            cmdText.AppendLine(" ,@IN_FormID ");
            cmdText.AppendLine(" ,@IN_FormName ");
            cmdText.AppendLine(" ,@IN_AuthorityFlag1 ");
            cmdText.AppendLine(" ,@IN_AuthorityFlag2 ");
            cmdText.AppendLine(" ,@IN_AuthorityFlag3 ");
            cmdText.AppendLine(" ,@IN_AuthorityFlag4 ");
            cmdText.AppendLine(" ,@IN_AuthorityFlag5 ");
            cmdText.AppendLine(" ,@IN_AuthorityFlag6 ");
            cmdText.AppendLine(" ,@IN_AuthorityFlag7 ");
            cmdText.AppendLine(" ,@IN_AuthorityFlag8 ");
            cmdText.AppendLine(" ,@IN_AuthorityFlag9 ");
            cmdText.AppendLine(" ,@IN_AuthorityFlag10 ");
            cmdText.AppendLine(" ,@IN_AuthorityFlag11 ");
            cmdText.AppendLine(" ,@IN_AuthorityFlag12 ");
            cmdText.AppendLine(" )");

            //Para
            Hashtable paras = new Hashtable();
            base.AddParam(paras,"IN_GroupID", m_GroupUser.GroupID);            
            base.AddParam(paras,"IN_FormID", m_GroupUser.FormID);
            base.AddParam(paras,"IN_FormName", m_GroupUser.FormName);

            base.AddParam(paras,"IN_AuthorityFlag1", m_GroupUser.AuthorityFlag1);
            base.AddParam(paras,"IN_AuthorityFlag2", m_GroupUser.AuthorityFlag2);
            base.AddParam(paras,"IN_AuthorityFlag3", m_GroupUser.AuthorityFlag3);
            base.AddParam(paras,"IN_AuthorityFlag4", m_GroupUser.AuthorityFlag4);
            base.AddParam(paras,"IN_AuthorityFlag5", m_GroupUser.AuthorityFlag5);
            base.AddParam(paras,"IN_AuthorityFlag6", m_GroupUser.AuthorityFlag6);
            base.AddParam(paras,"IN_AuthorityFlag7", m_GroupUser.AuthorityFlag7);
            base.AddParam(paras,"IN_AuthorityFlag8", m_GroupUser.AuthorityFlag8);
            base.AddParam(paras,"IN_AuthorityFlag9", m_GroupUser.AuthorityFlag9);
            base.AddParam(paras,"IN_AuthorityFlag10", m_GroupUser.AuthorityFlag10);
            base.AddParam(paras, "IN_AuthorityFlag11", m_GroupUser.AuthorityFlag11);
            base.AddParam(paras, "IN_AuthorityFlag12", m_GroupUser.AuthorityFlag12);

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }

        #endregion

        #region Update

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="m_GroupUser_H">M_GroupUser</param>
        /// <returns></returns>
        public int Update(M_GroupUser_H m_GroupUser_H)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" UPDATE dbo.M_GroupUser_H");
            cmdText.AppendLine(" SET");
            cmdText.AppendLine("  GroupCD	 = @IN_GroupCD");
            cmdText.AppendLine(" ,GroupName  = @IN_GroupName	");
            cmdText.AppendLine(" ,UpdateDate = GETDATE()");
            cmdText.AppendLine(" ,UpdateUID	 = @IN_UpdateUID");
            
            //Para
            Hashtable paras = new Hashtable();
            base.AddParam(paras,"IN_GroupID", m_GroupUser_H.ID);
            base.AddParam(paras,"IN_GroupCD", EditDataUtil.ToFixCodeDB(m_GroupUser_H.GroupCD, M_GroupUser_H.GROUP_CODE_DB_MAX_LENGTH));
            base.AddParam(paras,"IN_GroupName", m_GroupUser_H.GroupName);

            base.AddParam(paras,"IN_UpdateDate", m_GroupUser_H.UpdateDate, true);
            base.AddParam(paras,"IN_UpdateUID", m_GroupUser_H.UpdateUID);

            cmdWhere.AppendLine(" ID = @IN_GroupID AND UpdateDate = @IN_UpdateDate");

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
        /// <param name="m_GroupUser_M"></param>
        /// <returns></returns>
        public int Update(M_GroupUser_D m_GroupUser_M)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" UPDATE dbo.M_GroupUser_D");
            cmdText.AppendLine(" SET");
            cmdText.AppendLine("  [FormName]       = @IN_FormName");
            cmdText.AppendLine(" ,[AuthorityFlag1] = @IN_AuthorityFlag1");
            cmdText.AppendLine(" ,[AuthorityFlag2] = @IN_AuthorityFlag2");
            cmdText.AppendLine(" ,[AuthorityFlag3] = @IN_AuthorityFlag3");
            cmdText.AppendLine(" ,[AuthorityFlag4] = @IN_AuthorityFlag4");
            cmdText.AppendLine(" ,[AuthorityFlag5] = @IN_AuthorityFlag5");
            cmdText.AppendLine(" ,[AuthorityFlag6] = @IN_AuthorityFlag6");
            cmdText.AppendLine(" ,[AuthorityFlag7] = @IN_AuthorityFlag7");
            cmdText.AppendLine(" ,[AuthorityFlag8] = @IN_AuthorityFlag8");
            cmdText.AppendLine(" ,[AuthorityFlag9] = @IN_AuthorityFlag9");
            cmdText.AppendLine(" ,[AuthorityFlag10] = @IN_AuthorityFlag10");
            cmdText.AppendLine(" ,[AuthorityFlag11] = @IN_AuthorityFlag11");
            cmdText.AppendLine(" ,[AuthorityFlag12] = @IN_AuthorityFlag12");

            cmdWhere.AppendLine("    [GroupID] = @IN_GroupID");
            cmdWhere.AppendLine(" AND [FormID] = @IN_FormID");

            //Para
            Hashtable paras = new Hashtable();
            base.AddParam(paras,"IN_GroupID", m_GroupUser_M.GroupID);            
            base.AddParam(paras,"IN_FormID", m_GroupUser_M.FormID);
            base.AddParam(paras,"IN_FormName", m_GroupUser_M.FormName);
            base.AddParam(paras,"IN_AuthorityFlag1", m_GroupUser_M.AuthorityFlag1);
            base.AddParam(paras,"IN_AuthorityFlag2", m_GroupUser_M.AuthorityFlag2);
            base.AddParam(paras,"IN_AuthorityFlag3", m_GroupUser_M.AuthorityFlag3);
            base.AddParam(paras,"IN_AuthorityFlag4", m_GroupUser_M.AuthorityFlag4);
            base.AddParam(paras,"IN_AuthorityFlag5", m_GroupUser_M.AuthorityFlag5);
            base.AddParam(paras,"IN_AuthorityFlag6", m_GroupUser_M.AuthorityFlag6);
            base.AddParam(paras,"IN_AuthorityFlag7", m_GroupUser_M.AuthorityFlag7);
            base.AddParam(paras,"IN_AuthorityFlag8", m_GroupUser_M.AuthorityFlag8);
            base.AddParam(paras,"IN_AuthorityFlag9", m_GroupUser_M.AuthorityFlag9);
            base.AddParam(paras,"IN_AuthorityFlag10", m_GroupUser_M.AuthorityFlag10);
            base.AddParam(paras, "IN_AuthorityFlag11", m_GroupUser_M.AuthorityFlag11);
            base.AddParam(paras, "IN_AuthorityFlag12", m_GroupUser_M.AuthorityFlag12);


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
        /// Delete Group detail
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public int DeleteDetail(int groupID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" DELETE	FROM dbo.M_GroupUser_D");

            //Params
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine(" GroupID = @IN_ID");
            base.AddParam(paras,"IN_ID", groupID);


            //Check SQL
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Delete group header
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="updateDate"></param>
        /// <returns></returns>
        public int DeleteHeader(int ID, DateTime updateDate)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" DELETE FROM dbo.M_GroupUser_H");

            //Params
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine(" ID = @IN_ID AND UpdateDate = @IN_UpdateDate");

            base.AddParam(paras, "IN_ID", ID);
            base.AddParam(paras, "IN_UpdateDate", updateDate, true);

            //Check SQL
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }
        #endregion

        #region Check
        #endregion
    }
}

