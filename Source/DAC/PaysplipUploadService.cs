using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using OMS.Models;
using OMS.Utilities;

namespace OMS.DAC
{
    public class PaysplipUploadService : BaseService
    {
        #region Contructor
        /// <summary>
        /// Contructor of User service
        /// </summary>        
        private PaysplipUploadService()
            : base()
        {
        }

        /// <summary>
        /// Contructor of User service
        /// </summary>
        /// <param name="db">Class DB</param>
        public PaysplipUploadService(DB db)
            : base(db)
        {
        }

        #endregion

        #region Get Data

        /// <summary>
        /// Get File Path by PayslipID
        /// </summary>
        /// <param name="PayslipID">Payslip ID</param>
        /// <returns></returns>
        public PayslipDowloadInfo getFilePathbyPayslipID(int PayslipID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine(" Select Filepath, UserCD From T_Payslip AS P");
            cmdText.AppendLine(" Left Join M_User AS U On P.UserID = U.ID");
            cmdText.AppendLine(" WHERE P.ID = @IN_PayslipID");
            //Para
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_PayslipID", PayslipID);

            return this.db.Find1<PayslipDowloadInfo>(cmdText.ToString(), paras);
        }

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
        public IList<PayslipUploadInfo> GetListByCond(string year, string type,int sortField, int sortDirec)
        {
            string[] fields = new string[] { "", "UserCD", "UserName1", "DepartmentName", "", "Filepath", "UploadDate", "DownloadDate" };
            string[] direc = new string[] { "ASC", "DESC" };

            string RowNumber;
            if (fields[sortField - 1] == "DepartmentName")
            {
                RowNumber = fields[sortField - 1]  + " " + direc[sortDirec - 1] + " , U.UserCD ASC"; ;
                if (sortField == 1)
                {
                    RowNumber = fields[sortField - 1] + " " + direc[1] + " , U.UserCD ASC";
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
            cmdText.AppendLine("  P.ID");
            cmdText.AppendLine(" ,U.UserCD");
            cmdText.AppendLine(" ,U.UserName1");
            cmdText.AppendLine(" ,D.DepartmentName");
            cmdText.AppendLine(" ,P.Filepath");
            cmdText.AppendLine(" ,Convert(varchar(10),P.UploadDate,111) AS UploadDate");
            cmdText.AppendLine(" ,(Convert(varchar(10),P.DownloadDate,111) + ' ' + Convert(varchar(5),P.DownloadDate,108)) AS DownloadDate");
            cmdText.AppendLine(" ," + string.Format("ROW_NUMBER() OVER(ORDER BY {0}) AS RowNumber", RowNumber));
            cmdText.AppendLine(" FROM M_User AS U");
            cmdText.AppendLine(" LEFT JOIN (SELECT * FROM T_Payslip WHERE (Year LIKE @IN_Year) AND (Type = @IN_Type)) AS P  ON P.UserID = U.ID");
            cmdText.AppendLine(" LEFT JOIN M_Department AS D ON U.DepartmentID = D.ID");
            //Para
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine(" (U.StatusFlag = 0 Or");
            cmdWhere.AppendLine(" (U.ID IN (Select UserID From T_Payslip Where IsNull(Filepath, '') <> '' AND Year LIKE @IN_Year)))");
            cmdWhere.AppendLine(" And U.UserCD <> '0000000000'");

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            base.AddParam(paras, "IN_Year", year, true);
            base.AddParam(paras, "IN_Type", type, true);

            StringBuilder cmdOutText = new StringBuilder();
            cmdOutText.AppendLine(" SELECT");
            cmdOutText.AppendLine("   *");
            cmdOutText.AppendLine(" FROM");
            cmdOutText.AppendLine(" (");
            cmdOutText.AppendLine(cmdText.ToString());
            cmdOutText.AppendLine(" ) AS  VIEW_1");

            base.AddParam(paras, "IN_SortField", sortField);
            base.AddParam(paras, "IN_SortDirec", sortDirec);

            return this.db.FindList1<PayslipUploadInfo>(cmdOutText.ToString(), paras);
        }

        /// <summary>
        /// Get total row for list
        /// </summary>
        /// <param name="year">year</param>
        /// <param name="type">type</param>
        /// <returns></returns>
        public int getTotalRow(string year, string type)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();
            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  count(*)");
            cmdText.AppendLine(" FROM M_User AS U");
            cmdText.AppendLine(" LEFT JOIN (SELECT * FROM T_Payslip WHERE (Year LIKE @IN_Year) AND (Type = @IN_Type)) AS P  ON P.UserID = U.ID");
            cmdText.AppendLine(" LEFT JOIN M_Department AS D ON U.DepartmentID = D.ID");
            
            cmdWhere.AppendLine(" (U.StatusFlag = 0 Or");
            cmdWhere.AppendLine(" (U.ID IN (Select UserID From T_Payslip Where IsNull(Filepath, '') <> '' AND Year LIKE @IN_Year)))");
            cmdWhere.AppendLine(" And U.UserCD <> '0000000000'");

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }
            //Para
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_Year", year, true);
            base.AddParam(paras, "IN_Type", type, true);

            return int.Parse(this.db.ExecuteScalar1(cmdText.ToString(), paras).ToString());
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="setting">M_Setting</param>
        /// <returns></returns>
        public int UpdateDownloadDate(int payslipID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();
            cmdText.AppendLine(" UPDATE T_Payslip");
            cmdText.AppendLine(" SET");
            cmdText.AppendLine(" DownloadDate				= GETDATE()");

            //Para
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_ID", payslipID);

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


    }
}
