using System;
using System.Collections;
using System.Collections.Generic;

using OMS.Models;
using OMS.Utilities;
using System.Text;

namespace OMS.DAC
{
    /// Class WorkingSystemService DAC
    /// Create Date: 2017/09/19
    /// Create Author: ISV-Than
    public class WorkingSystemService : BaseService
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        private WorkingSystemService()
            : base()
        {
        }

        /// <summary>
        /// Constructor with param
        /// </summary>
        /// <param name="db">Database</param>
        public WorkingSystemService(DB db)
            : base(db)
        {
        }
        #endregion

        #region Get Data
    
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
        public IList<WorkingSystemInfo> GetListByCond(string workingSystemCD, string workingSystemName,
                                                  int pageIndex, int pageSize, int sortField, int sortDirec)
        {
            string[] fields = new string[] { "UpdateDate", "", "WorkingSystemCD", "WorkingSystemName", "WorkingSystemName2"};
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
            cmdText.AppendLine(" ,WorkingSystemCD");
            cmdText.AppendLine(" ,WorkingSystemName");
            cmdText.AppendLine(" ,WorkingSystemName2");

            cmdText.AppendLine(" ," + string.Format("ROW_NUMBER() OVER(ORDER BY {0}) AS RowNumber", RowNumber));
            cmdText.AppendLine(" FROM dbo.M_WorkingSystem AS ws");

            //Parameter
            Hashtable paras = new Hashtable();

            if (!string.IsNullOrEmpty(workingSystemCD))
            {

                cmdWhere.AppendLine("AND ws.WorkingSystemCD LIKE '%' + @IN_WorkingSystemCD + '%'");
                base.AddParam(paras, "IN_WorkingSystemCD", workingSystemCD, true);
            }

            if (!string.IsNullOrEmpty(workingSystemName))
            {
                cmdWhere.AppendLine("AND ws.WorkingSystemName LIKE '%' + @IN_WorkingSystemName + '%'");
                base.AddParam(paras, "IN_WorkingSystemName", workingSystemName, true);
            }

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

            return this.db.FindList1<WorkingSystemInfo>(cmdOutText.ToString(), paras);
        }


        #endregion 

        #region Entity
        /// <summary>
        /// Get By Working System Cd
        /// </summary>
        /// <returns>M_WorkingSystem</returns>
        public M_WorkingSystem GetByWorkingSystemCd(string workingSystemCd)
        {
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  ID");

            cmdText.AppendLine(" ,WorkingSystemCD ");
            cmdText.AppendLine(" ,WorkingSystemName ");
            cmdText.AppendLine(" ,WorkingSystemName2 ");
            cmdText.AppendLine(" ,WorkingType ");
            cmdText.AppendLine(" ,Working_Start ");
            cmdText.AppendLine(" ,Working_End ");
            cmdText.AppendLine(" ,Working_End_2 ");

            cmdText.AppendLine(" ,OverTime1_Start ");
            cmdText.AppendLine(" ,OverTime1_End ");
            cmdText.AppendLine(" ,OverTime2_Start ");
            cmdText.AppendLine(" ,OverTime2_End ");
            cmdText.AppendLine(" ,OverTime3_Start ");
            cmdText.AppendLine(" ,OverTime3_End ");
            cmdText.AppendLine(" ,OverTime4_Start ");
            cmdText.AppendLine(" ,OverTime4_End ");
            cmdText.AppendLine(" ,OverTime5_Start ");
            cmdText.AppendLine(" ,OverTime5_End ");

            cmdText.AppendLine(" ,BreakType ");
            cmdText.AppendLine(" ,Break1_Start ");
            cmdText.AppendLine(" ,Break1_End ");
            cmdText.AppendLine(" ,Break2_Start ");
            cmdText.AppendLine(" ,Break2_End ");
            cmdText.AppendLine(" ,Break3_Start ");
            cmdText.AppendLine(" ,Break3_End ");
            cmdText.AppendLine(" ,Break4_Start ");
            cmdText.AppendLine(" ,Break4_End ");

            cmdText.AppendLine(" ,DateSwitchTime ");
            cmdText.AppendLine(" ,First_End ");
            cmdText.AppendLine(" ,Latter_Start ");
            cmdText.AppendLine(" ,AllOff_Hours ");
            cmdText.AppendLine(" ,FirstOff_Hours ");
            cmdText.AppendLine(" ,LatterOff_Hours ");

            cmdText.AppendLine(" ,CreateDate ");
            cmdText.AppendLine(" ,CreateUID ");
            cmdText.AppendLine(" ,UpdateDate ");
            cmdText.AppendLine(" ,UpdateUID ");

            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.M_WorkingSystem");

            //Para
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine(" RTRIM(WorkingSystemCD) = RTRIM(@IN_WorkingSystemCD)");
            base.AddParam(paras, "IN_WorkingSystemCD", workingSystemCd);

            cmdWhere.AppendLine(" AND ID >= 10");

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.Find1<M_WorkingSystem>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Get Data Working System By Id
        /// </summary>
        /// <param name="workingSystemId">workingSystemId</param>
        /// <returns></returns>
        public M_WorkingSystem GetDataWorkingSystemById(int workingSystemId)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  ID");

            cmdText.AppendLine(" ,WorkingSystemCD ");
            cmdText.AppendLine(" ,WorkingSystemName ");
            cmdText.AppendLine(" ,WorkingSystemName2 ");
            cmdText.AppendLine(" ,WorkingType ");
            cmdText.AppendLine(" ,Working_Start ");
            cmdText.AppendLine(" ,Working_End ");
            cmdText.AppendLine(" ,Working_End_2 ");

            cmdText.AppendLine(" ,OverTime1_Start ");
            cmdText.AppendLine(" ,OverTime1_End ");
            cmdText.AppendLine(" ,OverTime2_Start ");
            cmdText.AppendLine(" ,OverTime2_End ");
            cmdText.AppendLine(" ,OverTime3_Start ");
            cmdText.AppendLine(" ,OverTime3_End ");
            cmdText.AppendLine(" ,OverTime4_Start ");
            cmdText.AppendLine(" ,OverTime4_End ");
            cmdText.AppendLine(" ,OverTime5_Start ");
            cmdText.AppendLine(" ,OverTime5_End ");

            cmdText.AppendLine(" ,BreakType ");
            cmdText.AppendLine(" ,Break1_Start ");
            cmdText.AppendLine(" ,Break1_End ");
            cmdText.AppendLine(" ,Break2_Start ");
            cmdText.AppendLine(" ,Break2_End ");
            cmdText.AppendLine(" ,Break3_Start ");
            cmdText.AppendLine(" ,Break3_End ");
            cmdText.AppendLine(" ,Break4_Start ");
            cmdText.AppendLine(" ,Break4_End ");

            cmdText.AppendLine(" ,DateSwitchTime ");
            cmdText.AppendLine(" ,First_End ");
            cmdText.AppendLine(" ,Latter_Start ");
            cmdText.AppendLine(" ,AllOff_Hours ");
            cmdText.AppendLine(" ,FirstOff_Hours ");
            cmdText.AppendLine(" ,LatterOff_Hours ");

            cmdText.AppendLine(" ,CreateDate ");
            cmdText.AppendLine(" ,CreateUID ");
            cmdText.AppendLine(" ,UpdateDate ");
            cmdText.AppendLine(" ,UpdateUID ");

            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.M_WorkingSystem");

            cmdWhere.AppendLine(" ID = @IN_WorkingSystemID");

            //Para
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_WorkingSystemID", workingSystemId);

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.Find1<M_WorkingSystem>(cmdText.ToString(), paras);
        }



        #endregion

        /// <summary>
        /// Get Data Working System By Id
        /// </summary>
        /// <param name="workingSystemId">workingSystemId</param>
        /// <returns></returns>
        public IList<M_WorkingSystem> GetAll()
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();
            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" *");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.M_WorkingSystem");

            return this.db.FindList1<M_WorkingSystem>(cmdText.ToString());
        }

        #region Get Total Row
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupCD"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public int getTotalRowForList(string workingSystemCD, string workingSystemName)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();
            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" COUNT(*)");
            cmdText.AppendLine(" FROM dbo.M_WorkingSystem AS ws");

            //Parameter
            Hashtable paras = new Hashtable();
            if (!string.IsNullOrEmpty(workingSystemCD))
            {
                cmdWhere.AppendLine("AND ws.WorkingSystemCD LIKE  + '%' + @IN_WorkingSystemCD  + '%'");
                base.AddParam(paras, "IN_WorkingSystemCD", workingSystemCD, true);
            }

            if (!string.IsNullOrEmpty(workingSystemName))
            {
                cmdWhere.AppendLine("AND ws.WorkingSystemName LIKE '%' + @IN_WorkingSystemName + '%'");
                base.AddParam(paras, "IN_WorkingSystemName", workingSystemName, true);
            }

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE");
                cmdWhere = cmdWhere.Replace("AND", "", 0, 3);
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return int.Parse(this.db.ExecuteScalar1(cmdText.ToString(), paras).ToString());
        }
        #endregion

        #region Insert
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="user">M_WorkingSystem</param>
        /// <returns></returns>
        public int Insert(M_WorkingSystem m_WorkingSystem)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine(" INSERT INTO dbo.M_WorkingSystem");
            cmdText.AppendLine(" (");

            cmdText.AppendLine("  WorkingSystemCD ");
            cmdText.AppendLine(" ,WorkingSystemName ");
            cmdText.AppendLine(" ,WorkingSystemName2 ");
            cmdText.AppendLine(" ,WorkingType ");
            cmdText.AppendLine(" ,Working_Start ");
            cmdText.AppendLine(" ,Working_End ");
            cmdText.AppendLine(" ,Working_End_2 ");
                        
            cmdText.AppendLine(" ,OverTime1_Start ");
            cmdText.AppendLine(" ,OverTime1_End ");
            cmdText.AppendLine(" ,OverTime2_Start ");
            cmdText.AppendLine(" ,OverTime2_End ");
            cmdText.AppendLine(" ,OverTime3_Start ");
            cmdText.AppendLine(" ,OverTime3_End ");
            cmdText.AppendLine(" ,OverTime4_Start ");
            cmdText.AppendLine(" ,OverTime4_End ");
            cmdText.AppendLine(" ,OverTime5_Start ");
            cmdText.AppendLine(" ,OverTime5_End ");

            cmdText.AppendLine(" ,BreakType ");
            cmdText.AppendLine(" ,Break1_Start ");
            cmdText.AppendLine(" ,Break1_End ");
            cmdText.AppendLine(" ,Break2_Start ");
            cmdText.AppendLine(" ,Break2_End ");
            cmdText.AppendLine(" ,Break3_Start ");
            cmdText.AppendLine(" ,Break3_End ");
            cmdText.AppendLine(" ,Break4_Start ");
            cmdText.AppendLine(" ,Break4_End ");

            cmdText.AppendLine(" ,DateSwitchTime ");
            cmdText.AppendLine(" ,First_End ");
            cmdText.AppendLine(" ,Latter_Start ");
            cmdText.AppendLine(" ,AllOff_Hours ");
            cmdText.AppendLine(" ,FirstOff_Hours ");
            cmdText.AppendLine(" ,LatterOff_Hours ");

            cmdText.AppendLine(" ,CreateDate ");
            cmdText.AppendLine(" ,CreateUID ");
            cmdText.AppendLine(" ,UpdateDate ");
            cmdText.AppendLine(" ,UpdateUID ");

            cmdText.AppendLine(" )");
            cmdText.AppendLine(" VALUES");
            cmdText.AppendLine(" (");

            cmdText.AppendLine("  @IN_WorkingSystemCD ");
            cmdText.AppendLine(" ,@IN_WorkingSystemName ");
            cmdText.AppendLine(" ,@IN_WorkingSystemName2 ");
            cmdText.AppendLine(" ,@IN_WorkingType ");
            cmdText.AppendLine(" ,@IN_Working_Start ");
            cmdText.AppendLine(" ,@IN_Working_End ");
            cmdText.AppendLine(" ,@IN_Working_End_2 ");

            cmdText.AppendLine(" ,@IN_OverTime1_Start ");
            cmdText.AppendLine(" ,@IN_OverTime1_End ");
            cmdText.AppendLine(" ,@IN_OverTime2_Start ");
            cmdText.AppendLine(" ,@IN_OverTime2_End ");
            cmdText.AppendLine(" ,@IN_OverTime3_Start ");
            cmdText.AppendLine(" ,@IN_OverTime3_End ");
            cmdText.AppendLine(" ,@IN_OverTime4_Start ");
            cmdText.AppendLine(" ,@IN_OverTime4_End ");
            cmdText.AppendLine(" ,@IN_OverTime5_Start ");
            cmdText.AppendLine(" ,@IN_OverTime5_End ");

            cmdText.AppendLine(" ,@IN_BreakType ");
            cmdText.AppendLine(" ,@IN_Break1_Start ");
            cmdText.AppendLine(" ,@IN_Break1_End ");
            cmdText.AppendLine(" ,@IN_Break2_Start ");
            cmdText.AppendLine(" ,@IN_Break2_End ");
            cmdText.AppendLine(" ,@IN_Break3_Start ");
            cmdText.AppendLine(" ,@IN_Break3_End ");
            cmdText.AppendLine(" ,@IN_Break4_Start ");
            cmdText.AppendLine(" ,@IN_Break4_End ");

            cmdText.AppendLine(" ,@IN_DateSwitchTime ");
            cmdText.AppendLine(" ,@IN_First_End ");
            cmdText.AppendLine(" ,@IN_Latter_Start ");
            cmdText.AppendLine(" ,@IN_AllOff_Hours ");
            cmdText.AppendLine(" ,@IN_FirstOff_Hours ");
            cmdText.AppendLine(" ,@IN_LatterOff_Hours ");

            cmdText.AppendLine(" ,GETDATE() ");
            cmdText.AppendLine(" ,@IN_CreateUID ");
            cmdText.AppendLine(" ,GETDATE() ");
            cmdText.AppendLine(" ,@IN_UpdateUID ");
            cmdText.AppendLine(" )");

            //Para
            Hashtable paras = new Hashtable();

            base.AddParam(paras, "IN_WorkingSystemCD",m_WorkingSystem.WorkingSystemCD);
            base.AddParam(paras, "IN_WorkingSystemName", m_WorkingSystem.WorkingSystemName);
            base.AddParam(paras, "IN_WorkingSystemName2", m_WorkingSystem.WorkingSystemName2);
            base.AddParam(paras, "IN_WorkingType", m_WorkingSystem.WorkingType);
            base.AddParam(paras, "IN_Working_Start", m_WorkingSystem.Working_Start);
            base.AddParam(paras, "IN_Working_End", m_WorkingSystem.Working_End);
            base.AddParam(paras, "IN_Working_End_2", m_WorkingSystem.Working_End_2);

            base.AddParam(paras, "IN_OverTime1_Start", m_WorkingSystem.OverTime1_Start);
            base.AddParam(paras, "IN_OverTime1_End", m_WorkingSystem.OverTime1_End);
            base.AddParam(paras, "IN_OverTime2_Start", m_WorkingSystem.OverTime2_Start);
            base.AddParam(paras, "IN_OverTime2_End", m_WorkingSystem.OverTime2_End);
            base.AddParam(paras, "IN_OverTime3_Start", m_WorkingSystem.OverTime3_Start);
            base.AddParam(paras, "IN_OverTime3_End", m_WorkingSystem.OverTime3_End);
            base.AddParam(paras, "IN_OverTime4_Start", m_WorkingSystem.OverTime4_Start);
            base.AddParam(paras, "IN_OverTime4_End", m_WorkingSystem.OverTime4_End);
            base.AddParam(paras, "IN_OverTime5_Start", m_WorkingSystem.OverTime5_Start);
            base.AddParam(paras, "IN_OverTime5_End", m_WorkingSystem.OverTime5_End);

            base.AddParam(paras, "IN_BreakType", m_WorkingSystem.BreakType);
            base.AddParam(paras, "IN_Break1_Start", m_WorkingSystem.Break1_Start);
            base.AddParam(paras, "IN_Break1_End", m_WorkingSystem.Break1_End);
            base.AddParam(paras, "IN_Break2_Start", m_WorkingSystem.Break2_Start);
            base.AddParam(paras, "IN_Break2_End", m_WorkingSystem.Break2_End);
            base.AddParam(paras, "IN_Break3_Start", m_WorkingSystem.Break3_Start);
            base.AddParam(paras, "IN_Break3_End", m_WorkingSystem.Break3_End);
            base.AddParam(paras, "IN_Break4_Start", m_WorkingSystem.Break4_Start);
            base.AddParam(paras, "IN_Break4_End", m_WorkingSystem.Break4_End);

            base.AddParam(paras, "IN_DateSwitchTime", m_WorkingSystem.DateSwitchTime);
            base.AddParam(paras, "IN_First_End", m_WorkingSystem.First_End);
            base.AddParam(paras, "IN_Latter_Start", m_WorkingSystem.Latter_Start);
            base.AddParam(paras, "IN_AllOff_Hours", m_WorkingSystem.AllOff_Hours);
            base.AddParam(paras, "IN_FirstOff_Hours", m_WorkingSystem.FirstOff_Hours);
            base.AddParam(paras, "IN_LatterOff_Hours", m_WorkingSystem.LatterOff_Hours);

            base.AddParam(paras, "IN_CreateUID", m_WorkingSystem.CreateUID);
            base.AddParam(paras, "IN_UpdateUID", m_WorkingSystem.UpdateUID);

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }
        #endregion

        #region Update
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="m_WorkingSystem">m_WorkingSystem</param>
        /// <returns></returns>
        public int Update(M_WorkingSystem m_WorkingSystem)
        {
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" UPDATE dbo.M_WorkingSystem");
            cmdText.AppendLine(" SET");
            cmdText.AppendLine("  WorkingSystemCD = @IN_WorkingSystemCD ");
            cmdText.AppendLine(" ,WorkingSystemName = @IN_WorkingSystemName ");
            cmdText.AppendLine(" ,WorkingSystemName2 = @IN_WorkingSystemName2 ");
            cmdText.AppendLine(" ,WorkingType = @IN_WorkingType ");
            cmdText.AppendLine(" ,Working_Start = @IN_Working_Start ");
            cmdText.AppendLine(" ,Working_End = @IN_Working_End ");
            cmdText.AppendLine(" ,Working_End_2 = @IN_Working_End_2 ");

            cmdText.AppendLine(" ,OverTime1_Start = @IN_OverTime1_Start ");
            cmdText.AppendLine(" ,OverTime1_End = @IN_OverTime1_End ");
            cmdText.AppendLine(" ,OverTime2_Start = @IN_OverTime2_Start ");
            cmdText.AppendLine(" ,OverTime2_End = @IN_OverTime2_End ");
            cmdText.AppendLine(" ,OverTime3_Start = @IN_OverTime3_Start ");
            cmdText.AppendLine(" ,OverTime3_End = @IN_OverTime3_End ");
            cmdText.AppendLine(" ,OverTime4_Start = @IN_OverTime4_Start ");
            cmdText.AppendLine(" ,OverTime4_End = @IN_OverTime4_End ");
            cmdText.AppendLine(" ,OverTime5_Start = @IN_OverTime5_Start ");
            cmdText.AppendLine(" ,OverTime5_End = @IN_OverTime5_End ");

            cmdText.AppendLine(" ,BreakType = @IN_BreakType ");
            cmdText.AppendLine(" ,Break1_Start = @IN_Break1_Start ");
            cmdText.AppendLine(" ,Break1_End = @IN_Break1_End ");
            cmdText.AppendLine(" ,Break2_Start = @IN_Break2_Start ");
            cmdText.AppendLine(" ,Break2_End = @IN_Break2_End ");
            cmdText.AppendLine(" ,Break3_Start = @IN_Break3_Start ");
            cmdText.AppendLine(" ,Break3_End = @IN_Break3_End ");
            cmdText.AppendLine(" ,Break4_Start = @IN_Break4_Start ");
            cmdText.AppendLine(" ,Break4_End = @IN_Break4_End ");

            cmdText.AppendLine(" ,DateSwitchTime = @IN_DateSwitchTime ");
            cmdText.AppendLine(" ,First_End = @IN_First_End ");
            cmdText.AppendLine(" ,Latter_Start = @IN_Latter_Start ");
            cmdText.AppendLine(" ,AllOff_Hours = @IN_AllOff_Hours  ");
            cmdText.AppendLine(" ,FirstOff_Hours = @IN_FirstOff_Hours  ");
            cmdText.AppendLine(" ,LatterOff_Hours = @IN_LatterOff_Hours ");

            cmdText.AppendLine(" ,UpdateDate = GETDATE()");
            cmdText.AppendLine(" ,UpdateUID	 = @IN_UpdateUID");

            //Para
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_WorkingSystemID", m_WorkingSystem.ID);

            base.AddParam(paras, "IN_WorkingSystemCD", m_WorkingSystem.WorkingSystemCD);
            base.AddParam(paras, "IN_WorkingSystemName", m_WorkingSystem.WorkingSystemName);
            base.AddParam(paras, "IN_WorkingSystemName2", m_WorkingSystem.WorkingSystemName2);
            base.AddParam(paras, "IN_WorkingType", m_WorkingSystem.WorkingType);
            base.AddParam(paras, "IN_Working_Start", m_WorkingSystem.Working_Start);
            base.AddParam(paras, "IN_Working_End", m_WorkingSystem.Working_End);
            base.AddParam(paras, "IN_Working_End_2", m_WorkingSystem.Working_End_2);

            base.AddParam(paras, "IN_OverTime1_Start", m_WorkingSystem.OverTime1_Start);
            base.AddParam(paras, "IN_OverTime1_End", m_WorkingSystem.OverTime1_End);
            base.AddParam(paras, "IN_OverTime2_Start", m_WorkingSystem.OverTime2_Start);
            base.AddParam(paras, "IN_OverTime2_End", m_WorkingSystem.OverTime2_End);
            base.AddParam(paras, "IN_OverTime3_Start", m_WorkingSystem.OverTime3_Start);
            base.AddParam(paras, "IN_OverTime3_End", m_WorkingSystem.OverTime3_End);
            base.AddParam(paras, "IN_OverTime4_Start", m_WorkingSystem.OverTime4_Start);
            base.AddParam(paras, "IN_OverTime4_End", m_WorkingSystem.OverTime4_End);
            base.AddParam(paras, "IN_OverTime5_Start", m_WorkingSystem.OverTime5_Start);
            base.AddParam(paras, "IN_OverTime5_End", m_WorkingSystem.OverTime5_End);

            base.AddParam(paras, "IN_BreakType", m_WorkingSystem.BreakType);
            base.AddParam(paras, "IN_Break1_Start", m_WorkingSystem.Break1_Start);
            base.AddParam(paras, "IN_Break1_End", m_WorkingSystem.Break1_End);
            base.AddParam(paras, "IN_Break2_Start", m_WorkingSystem.Break2_Start);
            base.AddParam(paras, "IN_Break2_End", m_WorkingSystem.Break2_End);
            base.AddParam(paras, "IN_Break3_Start", m_WorkingSystem.Break3_Start);
            base.AddParam(paras, "IN_Break3_End", m_WorkingSystem.Break3_End);
            base.AddParam(paras, "IN_Break4_Start", m_WorkingSystem.Break4_Start);
            base.AddParam(paras, "IN_Break4_End", m_WorkingSystem.Break4_End);

            base.AddParam(paras, "IN_DateSwitchTime", m_WorkingSystem.DateSwitchTime);
            base.AddParam(paras, "IN_First_End", m_WorkingSystem.First_End);
            base.AddParam(paras, "IN_Latter_Start", m_WorkingSystem.Latter_Start);
            base.AddParam(paras, "IN_AllOff_Hours", m_WorkingSystem.AllOff_Hours);
            base.AddParam(paras, "IN_FirstOff_Hours", m_WorkingSystem.FirstOff_Hours);
            base.AddParam(paras, "IN_LatterOff_Hours", m_WorkingSystem.LatterOff_Hours);

            base.AddParam(paras, "IN_UpdateDate", m_WorkingSystem.UpdateDate);
            base.AddParam(paras, "IN_UpdateUID", m_WorkingSystem.UpdateUID);

            cmdWhere.AppendLine(" ID = @IN_WorkingSystemID AND UpdateDate = @IN_UpdateDate");

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
        /// DeleteWorkingSystemByID
        /// </summary>
        /// <param name="workingSystemID"></param>
        /// <returns></returns>
        public int DeleteWorkingSystemByID(int workingSystemID)
        {
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" DELETE	FROM dbo.M_WorkingSystem");

            //Params
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine(" ID = @IN_WorkingSystemID");
            base.AddParam(paras, "IN_WorkingSystemID", workingSystemID);


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