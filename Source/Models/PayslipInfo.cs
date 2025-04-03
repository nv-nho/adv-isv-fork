using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace OMS.Models
{
    [Serializable]
    public class PayslipInfo : M_Base<M_Setting>
    {
        public long RowNumber { get; set; }
        public int ID { get; set; }
        public string Year { get; set; }
        public string Type { get; set; }
        public int UserID { get; set; }
        public string Filepath { get; set; }
        public string UploadDate_fm { get; set; }
        public string DownloadDate_fm { get; set; }

        public DateTime UploadDate { get; set; }
        public DateTime DownloadDate { get; set; }


        public PayslipInfo(){ }

        /// <summary>
        /// Constructor class PayslipInfo
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public PayslipInfo(DbDataReader dr)
        {
            this.RowNumber = (long)dr["RowNumber"];
            this.ID = int.Parse(dr["ID"].ToString());
            this.Year = (string)dr["Year"];
            this.Type = (string)dr["Type"];
            this.UserID = int.Parse(dr["UserID"].ToString());

            this.Filepath = (DBNull.Value.Equals(dr["Filepath"]) ? "" : ((string)dr["Filepath"]));
            this.UploadDate_fm = (DBNull.Value.Equals(dr["UploadDate"]) ? "" : ((string)dr["UploadDate"]));
            this.DownloadDate_fm = (DBNull.Value.Equals(dr["DownloadDate"]) ? "" : ((string)dr["DownloadDate"]));
        }
    }


    [Serializable]
    public class PayslipInfo_Checkfile : M_Base<M_Setting>
    {
        public int ID { get; set; }
        public string Year { get; set; }
        public int Type { get; set; }
        public int UserID { get; set; }
        public string Filepath { get; set; }
        public DateTime UploadDate { get; set; }
        public DateTime DownloadDate { get; set; }


        public PayslipInfo_Checkfile() { }

        /// <summary>
        /// Constructor class PayslipInfo
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public PayslipInfo_Checkfile(DbDataReader dr)
        {
            this.ID = int.Parse(dr["ID"].ToString());
            this.Year = (string)dr["Year"];
            this.Type = int.Parse(dr["Type"].ToString());
            this.UserID = int.Parse(dr["UserID"].ToString());
        }
    }

}
