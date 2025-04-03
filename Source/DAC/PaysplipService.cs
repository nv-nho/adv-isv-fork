using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OMS.Models;
using OMS.Utilities;

namespace OMS.DAC
{
    public class PaysplipService : BaseService
    {
        #region Contructor
        /// <summary>
        /// Contructor of User service
        /// </summary>        
        private PaysplipService()
            : base()
        {
        }

        /// <summary>
        /// Contructor of User service
        /// </summary>
        /// <param name="db">Class DB</param>
        public PaysplipService(DB db)
            : base(db)
        {
        }

        #endregion

        #region Get Data
        /// <summary>
        /// Get List by condition
        /// </summary>
        /// <param name="year">year</param>
        /// <param name="type">type</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="sortField">Sort Field</param>
        /// <param name="sortDirec">Sort Direction</param>
        /// <returns>List Setting Info</returns>
        public IList<T_Payslip> GetListByYearAndType(string year, string type)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  *");
            cmdText.AppendLine(" FROM T_Payslip WHERE (Year LIKE @IN_Year) AND (Type = @IN_Type)");
            //Para
            Hashtable paras = new Hashtable();

            base.AddParam(paras, "IN_Year", year, true);
            base.AddParam(paras, "IN_Type", type, true);

            return this.db.FindList1<T_Payslip>(cmdText.ToString(), paras);
        }

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
            cmdText.AppendLine(" Select Distinct DISP_YEAR AS Value, DISP_YEAR AS DisplayName");
            cmdText.AppendLine(" FROM ( SELECT");

            cmdText.AppendLine("CAST(CASE WHEN CAST(RIGHT([Year],2) AS INT) > 6 THEN CAST(LEFT([Year],4) AS INT) + 1 ELSE CAST(LEFT([Year],4) AS INT) END AS VARCHAR(4)) AS DISP_YEAR");
            cmdText.AppendLine("FROM T_Payslip Where UserID = @IN_UserID) MAIN");
            cmdText.AppendLine("ORDER BY DISP_YEAR");

            base.AddParam(paras, "IN_UserID", userID, true);

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

            cmdText.AppendLine(" Select Distinct DISP_YEAR AS Value, DISP_YEAR AS DisplayName");
            cmdText.AppendLine(" FROM ( SELECT");

            cmdText.AppendLine("CAST(CASE WHEN CAST(RIGHT([Year],2) AS INT) > 6 THEN CAST(LEFT([Year],4) AS INT) + 1 ELSE CAST(LEFT([Year],4) AS INT) END AS VARCHAR(4)) AS DISP_YEAR");
            cmdText.AppendLine("FROM T_Payslip ) MAIN");
            cmdText.AppendLine("ORDER BY DISP_YEAR");
            //cmdText.AppendLine(" Left join M_User On T_Payslip.UserID = M_User.ID ");
            //cmdText.AppendLine(" Where M_User.DepartmentID = @IN_DepartmentID ");

            //Add param
            //base.AddParam(paras, "IN_DepartmentID", departmentID);

            return this.db.FindList1<DropDownModel>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Get total row for list
        /// </summary>
        /// <param name="configCD">Config Code</param>
        /// <param name="configName">Config Name</param>
        /// <returns></returns>
        public int getTotalRow(string year, string userID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();
            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  count(*)");
            cmdText.AppendLine(" FROM dbo.T_Payslip AS S");

            //Para
            Hashtable paras = new Hashtable();
            if (!string.IsNullOrEmpty(year))
            {
                string sMonth = string.Format("{0}/07", int.Parse(year) - 1);
                string eMonth = string.Format("{0}/06", year);
                cmdWhere.AppendLine(" [Year] >= @IN_SMonth");
                cmdWhere.AppendLine(" AND [Year] <= @IN_EMonth");
                base.AddParam(paras, "IN_SMonth", sMonth, true);
                base.AddParam(paras, "IN_EMonth", eMonth, true);
            }

            if (!string.IsNullOrEmpty(userID))
            {
                if (cmdWhere.Length != 0)
                {
                    cmdWhere.AppendLine(" AND");
                }
                cmdWhere.AppendLine(" UserID = @IN_UserID");
                base.AddParam(paras, "IN_UserID", userID, true);
            }

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
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
        public IList<PayslipInfo> GetListByCond(string year, string userID,
                                            int pageIndex, int pageSize, int sortField, int sortDirec)
        {
            string[] fields = new string[] { "", "", "Type", "Year", "UploadDate", "DownloadDate" };
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
            cmdText.AppendLine("  S.ID");
            cmdText.AppendLine(" ,S.Year");
            cmdText.AppendLine(" ,CD.Value2 AS Type");
            cmdText.AppendLine(" ,S.UserID");
            cmdText.AppendLine(" ,S.Filepath");
            cmdText.AppendLine(" ,Convert(varchar(10),S.UploadDate,111) AS UploadDate");
            cmdText.AppendLine(" ,(Convert(varchar(10),S.DownloadDate,111) + ' ' + Convert(varchar(5),S.DownloadDate,108)) AS DownloadDate");
            cmdText.AppendLine(" ," + string.Format("ROW_NUMBER() OVER(ORDER BY {0}) AS RowNumber", RowNumber));
            cmdText.AppendLine(" FROM dbo.T_Payslip AS S");
            cmdText.AppendLine(" LEFT JOIN M_Config_H AS CH ON ConfigCD = @IN_ConfigCD");
            cmdText.AppendLine(" LEFT JOIN M_Config_D AS CD ON CH.ID = CD.HID AND CD.Value1 = S.Type");
            //Para
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_ConfigCD", M_Config_H.CONFIG_UPLOAD_CLASSIFICATION, true);

            if (!string.IsNullOrEmpty(year))
            {
                string sMonth = string.Format("{0}/07", int.Parse(year) - 1);
                string eMonth = string.Format("{0}/06", year);
                cmdWhere.AppendLine(" [Year] >= @IN_SMonth");
                cmdWhere.AppendLine(" AND [Year] <= @IN_EMonth");
                base.AddParam(paras, "IN_SMonth", sMonth, true);
                base.AddParam(paras, "IN_EMonth", eMonth, true);
            }

            if (!string.IsNullOrEmpty(userID))
            {
                if (cmdWhere.Length != 0)
                {
                    cmdWhere.AppendLine(" AND");
                }
                cmdWhere.AppendLine(" UserID = @IN_UserID");
                base.AddParam(paras, "IN_UserID", userID, true);
            }

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
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

            return this.db.FindList1<PayslipInfo>(cmdOutText.ToString(), paras);
        }

        /// <summary>
        /// Get by configCD
        /// </summary>
        /// <param name="configCD">ConfigCD</param>
        /// <returns>M_Config_H</returns>
        public PayslipInfo_Checkfile GetByFilepath(string filepath, string ID)
        {
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" ROW_NUMBER() OVER(ORDER BY ID) AS RowNumber, *");
            cmdText.AppendLine(" FROM dbo.T_Payslip");

            cmdText.AppendLine(" WHERE");
            cmdText.AppendLine(" Filepath = @IN_FILEPATH");
            cmdText.AppendLine(" AND ID != @IN_ID");

            //Param
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_FILEPATH", filepath);
            base.AddParam(paras, "IN_ID", ID);

            return this.db.Find1<PayslipInfo_Checkfile>(cmdText.ToString(), paras);
        }

        #endregion

        #region Update
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="setting">M_Setting</param>
        /// <returns></returns>
        public int UpdateDownloadDate(int payslipID, int userID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();
            cmdText.AppendLine(" UPDATE T_Payslip");
            cmdText.AppendLine(" SET");
            cmdText.AppendLine(" DownloadDate				= GETDATE()");
            cmdText.AppendLine(" ,UpdateDate				= GETDATE()");
            cmdText.AppendLine(" ,UpdateUID				= @IN_USERID");
            //Para
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_ID", payslipID);
            base.AddParam(paras, "IN_USERID", userID);

            cmdWhere.AppendLine(" ID = @IN_ID");

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
        /// <param name="setting">M_Setting</param>
        /// <returns></returns>
        public int Update(PayslipInfo payslip)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();
            cmdText.AppendLine(" UPDATE dbo.T_Payslip");
            cmdText.AppendLine(" SET");
            cmdText.AppendLine(" Filepath				= @IN_Filepath, ");
            cmdText.AppendLine(" UploadDate				= GETDATE(), ");
            cmdText.AppendLine(" DownloadDate			= NULL, ");
            cmdText.AppendLine(" UpdateDate  = GETDATE(),");
            cmdText.AppendLine(" UpdateUID	 = @IN_UpdateUID");

            //Para
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_Filepath", payslip.Filepath);
            base.AddParam(paras, "IN_UpdateUID", payslip.UpdateUID);
            base.AddParam(paras, "IN_ID", payslip.ID);

            cmdWhere.AppendLine(" ID = @IN_ID");

            //Check SQL
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }
        #endregion

        #region Insert
        /// <summary>
        /// Insert data
        /// </summary>
        /// <param name="PayslipInfo">PayslipInfo</param>
        /// <returns></returns>
        public int Insert(PayslipInfo payslip)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine(" INSERT INTO dbo.T_Payslip ");
            cmdText.AppendLine("  (");
            cmdText.AppendLine("  [Year]");
            cmdText.AppendLine(" ,[Type]");
            cmdText.AppendLine(" ,[UserID]");
            cmdText.AppendLine(" ,[Filepath]");
            cmdText.AppendLine(" ,[UploadDate]");
            cmdText.AppendLine(" ,[CreateDate]");
            cmdText.AppendLine(" ,[CreateUID]");
            cmdText.AppendLine(" ,[UpdateDate]");
            cmdText.AppendLine(" ,[UpdateUID]");
            cmdText.AppendLine("  )");
            cmdText.AppendLine(" VALUES");
            cmdText.AppendLine(" (");
            cmdText.AppendLine("  @IN_Year");
            cmdText.AppendLine(" ,@IN_Type");
            cmdText.AppendLine(" ,@IN_UserID");
            cmdText.AppendLine(" ,@IN_Filepath");
            cmdText.AppendLine(" ,GETDATE()");
            cmdText.AppendLine(" ,GETDATE()");
            cmdText.AppendLine(" ,@IN_CreateUID");
            cmdText.AppendLine(" ,GETDATE()");
            cmdText.AppendLine(" ,@IN_UpdateUID");
            cmdText.AppendLine(" )");

            //Param
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_Year", payslip.Year);
            base.AddParam(paras, "IN_Type", payslip.Type);
            base.AddParam(paras, "IN_UserID", payslip.UserID);
            base.AddParam(paras, "IN_Filepath", payslip.Filepath);

            base.AddParam(paras, "IN_CreateUID", payslip.CreateUID);
            base.AddParam(paras, "IN_UpdateUID", payslip.UpdateUID);

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }

        #endregion

        #region Delete
        /// <summary>
        /// Delete data
        /// </summary>
        /// <param name="ID">id</param>
        /// <param name="updateDate">updateDate</param>
        /// <returns></returns>
        public int Delete(int ID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" DELETE	dbo.T_Payslip");
            cmdWhere.AppendLine(" ID=@IN_ID");

            //Param
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_ID", ID);

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }
        #endregion

        #region Check data
        /// <summary>
        /// Check exist Config by Config Code
        /// </summary>
        /// <param name="configCode">Config Code</param>
        /// <returns></returns>
        public bool IsExistFile(string configCode, string ID)
        {
            PayslipInfo_Checkfile file = GetByFilepath(configCode, ID);
            if (file != null)
            {
                return true;
            }

            return false;
        }
        #endregion Check data
    }
}
