using System;
using System.Data.Common;
using OMS.Utilities;


namespace OMS.Models
{
    /// <summary>
    /// InformationInfo model 
    /// TRAM
    /// </summary>
    [Serializable]
    public class InformationInfo
    {
        public int ID { get; set; }
        public long RowNumber { get; set; }
        public string InformationName { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public string InformationContent { get; set; }
        public string BeginDateStr { get; set; }
        public string EndDateStr { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public InformationInfo()
        {
            this.ID = -1;
            this.RowNumber = -1;

            this.InformationName = string.Empty;
            this.BeginDate = DateTime.MinValue;
            this.EndDate = DateTime.MinValue;
            this.InformationContent = string.Empty;
            this.BeginDateStr = string.Empty;
            this.EndDateStr = string.Empty;
            this.CreateDate = DateTime.MinValue;
            this.UpdateDate = DateTime.MinValue;
        }
        
        /// <summary>
        /// Constructor class InformationInfo
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public InformationInfo(DbDataReader dr)
        {
            this.ID = int.Parse(dr["ID"].ToString());
            this.RowNumber = (long)dr["RowNumber"];

            this.BeginDate = (DateTime)dr["BeginDate"];
            this.BeginDateStr = this.BeginDate.ToString(Constants.FMT_DATE_TIME);
            this.EndDate = (DateTime)dr["EndDate"];
            this.EndDateStr = this.EndDate.ToString(Constants.FMT_DATE_TIME);
            this.InformationName = (string)dr["InformationName"];
            this.InformationContent = (string)dr["InformationContent"];
            this.CreateDate = (DateTime)dr["CreateDate"];
            this.UpdateDate = (DateTime)dr["UpdateDate"];
        }
    }
}
