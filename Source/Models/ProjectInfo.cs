using System;
using System.Data.Common;
using OMS.Utilities;

namespace OMS.Models
{
    /// <summary>
    /// Class Project Info
    /// ISV-THAN
    /// </summary>
    [Serializable]
    public class ProjectInfo
    {
        public int ID { get; set; }
        public long RowNumber { get; set; }
        public string ProjectCD { get; set; }
        public string ProjectName { get; set; }
        public int UserID { get; set; }
        public string DepartmentName { get; set; }
        public string UserName { get; set; }
        public decimal OrderAmount { get; set; }
        public int AcceptanceFlag { get; set; }
        public string WorkPlace { get; set; }
        public string Memo { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StartDateStr { get; set; }
        public string EndDateStr { get; set; }
        public int StatusFlag { get; set; }
        public int Color { get; set; }

        /// <summary>
        /// Constructor class ProjectInfo
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public ProjectInfo(DbDataReader dr)
        {
            this.ID = int.Parse(dr["ID"].ToString());
            this.RowNumber = (long)dr["RowNumber"];
            this.ProjectCD = (string)dr["ProjectCD"];
            this.ProjectName = (string)dr["ProjectName"];
            this.UserID = int.Parse(dr["UserID"].ToString());
            this.DepartmentName = dr["DepartmentName"].ToString();
            this.UserName = dr["UserName"].ToString();
            this.OrderAmount = !string.IsNullOrEmpty(dr["OrderAmount"].ToString())?(decimal)dr["OrderAmount"]:0;
            this.AcceptanceFlag =!string.IsNullOrEmpty(dr["AcceptanceFlag"].ToString())?int.Parse(dr["AcceptanceFlag"].ToString()):0;
            this.WorkPlace = (string)dr["WorkPlace"];
            this.Memo = dr["Memo"].ToString();
            this.StartDate = (DateTime)dr["StartDate"];
            this.EndDate = dr["EndDate"].ToString()!=string.Empty?(DateTime)dr["EndDate"]:DateTime.MaxValue;
            this.StartDateStr = this.StartDate.ToString(Constants.FMT_DATE_EN);
            this.EndDateStr = this.EndDate!=DateTime.MaxValue? this.EndDate.ToString(Constants.FMT_DATE_EN):string.Empty;
            this.StatusFlag = int.Parse(dr["StatusFlag"].ToString() != string.Empty ? dr["StatusFlag"].ToString() : "0");
            this.Color = -1;
            if (this.StatusFlag == (int)DeleteFlag.Deleted)
            {
                this.Color = (int)ColorList.Danger;
            }  
        }

        /// <summary>
        /// Constructor class ProjectInfo
        /// </summary>
        public ProjectInfo()
        {
            this.RowNumber = 0;
            this.ProjectCD = null;
            this.ProjectName = null;
            this.WorkPlace = string.Empty;
            this.Memo = string.Empty;
            this.StartDate = DateTime.MinValue;
            this.EndDate = DateTime.MaxValue;
            this.StartDateStr = string.Empty;
            this.EndDateStr = string.Empty;
        }
    }

    [Serializable]
    public class ProjectSearchInfo
    {
        public long RowNumber { get; set; }
        public string ProjectCD { get; set; }
        public string ProjectName { get; set; }

        /// <summary>
        /// Constructor class ProjectSearchInfo
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public ProjectSearchInfo(DbDataReader dr)
        {
            this.RowNumber = (long)dr["RowNumber"];
            //this.ProjectCD = EditDataUtil.ToFixCodeShow((string)dr["ProjectCD"], M_Project.PROJECT_CODE_SHOW_MAX_LENGTH);
            this.ProjectCD = (string)dr["ProjectCD"];
            this.ProjectName = (string)dr["ProjectName"];
        }

        /// <summary>
        /// Constructor class ProjectSearchInfo
        /// </summary>
        public ProjectSearchInfo()
        {
            this.RowNumber = 0;
            this.ProjectCD = null;
            this.ProjectName = null;
        }
    }

    /// <summary>
    /// Class Project Excel
    /// </summary>
    [Serializable]
    public class ProjectExcelInfo
    {
        public string ProjectCD { get; set; }
        public string ProjectName { get; set; }
        public string Memo { get; set; }

        /// <summary>
        /// Constructor class ProjectInfo
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public ProjectExcelInfo(DbDataReader dr)
        {
            this.ProjectCD = (string)dr["ProjectCD"];
            this.ProjectName = (string)dr["ProjectName"];
            this.Memo = dr["Memo"].ToString();
        }

        /// <summary>
        /// Constructor class ProjectInfo
        /// </summary>
        public ProjectExcelInfo()
        {
            this.ProjectCD = string.Empty;
            this.ProjectName = string.Empty;
            this.Memo = string.Empty;
        }
    }

}
