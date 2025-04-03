using System;
using System.Data.Common;
using OMS.Utilities;
using System.IO;

namespace OMS.Models
{
    [Serializable]
    public class PayslipUploadInfo
    {
        public long RowNumber { get; set; }
        public string ID { get; set; }
        public string UserCD { get; set; }
        public string UserCDLong{ get; set; }
        public string UserName1 { get; set; }
        public string DepartmentName { get; set; }
        public string Filepath { get; set; }
        public string UploadDate_fm { get; set; }
        public string DownloadDate_fm { get; set; }
        public string OldFilepath { get; set; }
        /// <summary>
        /// Constructor class PayslipInfo
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public PayslipUploadInfo(DbDataReader dr)
        {
            this.RowNumber = (long)dr["RowNumber"];
            this.ID = (DBNull.Value.Equals(dr["ID"]) ? "0" : ((string)dr["ID"].ToString()));
            this.UserCD = EditDataUtil.ToFixCodeShow((string)dr["UserCD"], M_User.MAX_USER_CODE_SHOW);
            this.UserCDLong = EditDataUtil.ToFixCodeShow((string)dr["UserCD"], 6); ;

            this.UserName1 = (string)dr["UserName1"];
            this.DepartmentName = (string)dr["DepartmentName"];
            String path = (DBNull.Value.Equals(dr["Filepath"]) ? "" : ((string)dr["Filepath"]));
            if (!String.IsNullOrEmpty(path))
            {
                this.Filepath = Path.GetFileName(path);
            }
            else
            {
                this.Filepath = path;
            }
             
            this.UploadDate_fm = (DBNull.Value.Equals(dr["UploadDate"]) ? "" : ((string)dr["UploadDate"]));
            this.DownloadDate_fm = (DBNull.Value.Equals(dr["DownloadDate"]) ? "" : ((string)dr["DownloadDate"]));

            this.OldFilepath = path;
        }
    }

    [Serializable]
    public class PayslipDowloadInfo
    {
        public string UserCD { get; set; }
        public string Filepath { get; set; }

        /// <summary>
        /// Constructor class PayslipInfo
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public PayslipDowloadInfo(DbDataReader dr)
        {
            this.UserCD = EditDataUtil.ToFixCodeShow((string)dr["UserCD"], M_User.MAX_USER_CODE_SHOW); ;
            this.Filepath = (DBNull.Value.Equals(dr["Filepath"]) ? "" : ((string)dr["Filepath"]));
        }
    }
}
