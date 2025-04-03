using System;
using System.Collections;
using System.Collections.Generic;
using OMS.Models;
using OMS.Utilities;
using System.Text;

namespace OMS.DAC
{
    /// <summary>
    /// Class M_Event DAC
    /// TRAM
    /// </summary>
    public class InformationService : BaseService
    {
        #region Contructor
        /// <summary>
        /// Contructor of event service
        /// </summary>        
        private InformationService()
            : base()
        {
        }

        /// <summary>
        /// Contructor of event service
        /// </summary>
        /// <param name="db">Class DB</param>
        public InformationService(DB db)
            : base(db)
        {
        }

        #endregion

        #region Get Data
        /// <summary>
        /// Get by id
        /// </summary>
        /// <returns></returns>
        public M_Information GetByID(int id)
        {
            // Command text
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();
            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" MI.*");
            cmdText.AppendLine(" FROM ");
            cmdText.AppendLine(" dbo.M_Information AS MI");
            
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine(" ID = @IN_ID");
            base.AddParam(paras, "@IN_ID", id);

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }
            return this.db.Find1<M_Information>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Get by condition
        /// </summary>
        /// <param name="informationName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="sortDirec"></param>
        /// <returns></returns>
        public IList<InformationInfo> GetListByCond(string informationName,
                                            int pageIndex, int pageSize, int sortField, int sortDirec)
        {
            string[] fields = new string[] { "UpdateDate", "", "InformationName", "BeginDate", "EndDate" };
            string[] direc = new string[] { "ASC", "DESC" };

            string RowNumber = fields[sortField - 1] + " " + direc[sortDirec - 1];
            if (sortField == 1)
            {
                RowNumber = fields[sortField - 1] + " " + direc[1];
            }
            // Command text
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();
            cmdText.AppendLine(" SELECT	");
            cmdText.AppendLine(" MI.*,");
            cmdText.AppendLine( string.Format("ROW_NUMBER() OVER(ORDER BY {0}) AS RowNumber", RowNumber));
            cmdText.AppendLine(" FROM ");
            cmdText.AppendLine("    dbo.M_Information AS MI	");

            //Para
            Hashtable paras = new Hashtable();
            //cmdWhere.AppendLine("AND RowNumber BETWEEN(@IN_PageIndex -1) * @IN_PageSize + 1 AND(((@IN_PageIndex -1) * @IN_PageSize + 1) + @IN_PageSize) - 1 ORDER BY RowNumber");

            if (!string.IsNullOrEmpty(informationName))
            {

                cmdWhere.AppendLine("AND  (@IN_InformationName IS NULL OR @IN_InformationName ='') OR UPPER(MI.InformationName) LIKE '%' + UPPER(@IN_InformationName) + '%'");
                base.AddParam(paras, "IN_InformationName", informationName);
            }
            
            base.AddParam(paras, "IN_SortField", sortField);
            base.AddParam(paras, "IN_SortDirec", sortDirec);

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

            return this.db.FindList1<InformationInfo>(cmdOutText.ToString(), paras);
        }

        /// <summary>
        /// Get All
        /// </summary>
        /// <returns></returns>
        public IList<M_Information> GetAll()
        {
            // Command text
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine(" SELECT	*");
            cmdText.AppendLine(" FROM dbo.M_Information	");
            cmdText.AppendLine(" WHERE BeginDate<=GETDATE() AND GETDATE()<=EndDate");
            cmdText.AppendLine(" ORDER BY BeginDate");

            return this.db.FindList1<M_Information>(cmdText.ToString());
        }

        /// <summary>
        /// Get Total row number
        /// </summary>
        /// <param name="informationName">InformationName</param>
        /// <returns></returns>
        public int GetTotalRow(string informationName)
        {
            // Command text
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT COUNT(*)");
            cmdText.AppendLine(" FROM dbo.M_Information AS MI ");

            //Para
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine("AND (@IN_InformationName IS NULL OR @IN_InformationName ='') OR UPPER(MI.InformationName) LIKE '%' + UPPER(@IN_InformationName) + '%' ");
            base.AddParam(paras, "IN_InformationName", informationName);

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE");
                cmdWhere = cmdWhere.Replace("AND", "", 0, 3);
                cmdText.AppendLine(cmdWhere.ToString());
            }
            return int.Parse(this.db.ExecuteScalar1(cmdText.ToString(), paras).ToString());
        }

        /// <summary>
        /// Get by Information model
        /// </summary>
        /// <param name="model">Information model</param>
        /// <returns>Information model</returns>
        public M_Information GetByInformation(M_Information model)
        {
            // Command text
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT	MI.*");
            cmdText.AppendLine(" FROM		");
            cmdText.AppendLine(" dbo.M_Information AS MI ");

            //Para
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine("AND MI.InformationName		= @IN_InformationName");
            base.AddParam(paras, "IN_InformationName", model.InformationName);
            cmdWhere.AppendLine("AND	MI.BeginDate			= @IN_BeginDate");
            base.AddParam(paras, "IN_BeginDate", model.BeginDate, true);
            cmdWhere.AppendLine("AND	MI.EndDate				= @IN_EndDate");
            base.AddParam(paras, "IN_EndDate", model.EndDate, true);
            cmdWhere.AppendLine("AND	MI.InformationContent	= @IN_InformationContent");
            base.AddParam(paras, "IN_InformationContent", model.InformationContent);
            cmdWhere.AppendLine("AND	MI.CreateUID			= @IN_CreateUID ");
            base.AddParam(paras, "IN_CreateUID", model.CreateUID);
            cmdWhere.AppendLine("AND	MI.UpdateUID			= @IN_UpdateUID ");
            base.AddParam(paras, "IN_UpdateUID", model.UpdateUID);

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE");
                cmdWhere = cmdWhere.Replace("AND", "", 0, 3);
                cmdText.AppendLine(cmdWhere.ToString());
            }
            return this.db.Find1<M_Information>(cmdText.ToString(), paras);
        }

        #endregion

        #region Insert
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="data">M_Information</param>
        /// <returns></returns>
        public int Insert(M_Information data)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine(" INSERT INTO dbo.M_Information");
            cmdText.AppendLine(" (");
            cmdText.AppendLine(" BeginDate");
            cmdText.AppendLine(" ,EndDate");
            cmdText.AppendLine(" ,InformationName");
            cmdText.AppendLine(" ,InformationContent");
            cmdText.AppendLine(" ,CreateDate");
            cmdText.AppendLine(" ,CreateUID");
            cmdText.AppendLine(" ,UpdateDate");
            cmdText.AppendLine(" ,UpdateUID");
            cmdText.AppendLine(" )");
            cmdText.AppendLine(" VALUES");
            cmdText.AppendLine(" (");
            cmdText.AppendLine(" @IN_BeginDate");
            cmdText.AppendLine(" ,@IN_EndDate");
            cmdText.AppendLine(" ,@IN_InformationName");
            cmdText.AppendLine(" ,@IN_InformationContent");
            cmdText.AppendLine(" ,GETDATE()");
            cmdText.AppendLine(" ,@IN_CreateUID");
            cmdText.AppendLine(" ,GETDATE()");
            cmdText.AppendLine(" ,@IN_UpdateUID");
            cmdText.AppendLine(" )");
            //Para
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_BeginDate", data.BeginDate, true);
            base.AddParam(paras, "IN_EndDate", data.EndDate, true);
            base.AddParam(paras, "IN_InformationName", data.InformationName);
            base.AddParam(paras, "IN_InformationContent", data.InformationContent);
            base.AddParam(paras, "IN_CreateUID", data.CreateUID);
            base.AddParam(paras, "IN_UpdateUID", data.UpdateUID);

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }
        #endregion

        #region Update
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="data">M_Information</param>
        /// <returns></returns>
        public int Update(M_Information data)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" UPDATE	dbo.M_Information" );
            cmdText.AppendLine(" SET ");
            cmdText.AppendLine(" BeginDate			= @IN_BeginDate,");
            cmdText.AppendLine(" EndDate			= @IN_EndDate, ");
            cmdText.AppendLine(" InformationName	= @IN_InformationName, ");
            cmdText.AppendLine(" InformationContent	= @IN_InformationContent, ");
            cmdText.AppendLine(" UpdateDate			= GETDATE(), ");
            cmdText.AppendLine(" UpdateUID			= @IN_UpdateUID ");

            //Para
            Hashtable paras = new Hashtable();
            base.AddParam(paras,"IN_ID", data.ID);

            base.AddParam(paras, "IN_BeginDate", data.BeginDate, true);
            base.AddParam(paras, "IN_EndDate", data.EndDate, true);
            base.AddParam(paras, "IN_InformationName", data.InformationName);
            base.AddParam(paras, "IN_InformationContent", data.InformationContent);
            
            base.AddParam(paras, "IN_UpdateDate", data.UpdateDate, true);
            base.AddParam(paras, "IN_UpdateUID", data.UpdateUID);

            cmdWhere.AppendLine(" ID = @IN_ID AND UpdateDate = @IN_UpdateDate");

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
        /// Delete
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="updateDate"></param>
        /// <returns></returns>
        public int Delete(int ID, DateTime updateDate)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" DELETE	FROM dbo.M_Information");

            //Params
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine(" ID=@IN_ID AND UpdateDate=@IN_UpdateDate");
            base.AddParam(paras,"IN_ID", ID);
            base.AddParam(paras,"IN_UpdateDate", updateDate, true);

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
