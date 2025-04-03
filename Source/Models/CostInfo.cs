using System;
using System.Data.Common;

namespace OMS.Models
{
    /// <summary>
    /// Class CostInfo Model
    /// </summary>
    [Serializable]
    public class CostInfo
    {
        public long RowNumber { get; set; }

        //T_Cost_H
        public int ID { get; set; }
        public string CostCode { get; set; }
        public string CostName { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateUID { get; set; }
        public DateTime UpdateDate { get; set; }
        public int UpdateUID { get; set; }

        //T_Cost_D
        public int HID { get; set; }
        public DateTime EffectDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public decimal CostAmount { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public CostInfo()
        {
            this.RowNumber = 0;

            this.ID = 0;
            this.CostCode = string.Empty;
            this.CostName = string.Empty;
            this.CreateDate = DateTime.Now;
            this.CreateUID = 0;
            this.UpdateDate = DateTime.Now;
            this.UpdateUID = 0;

            this.HID = 0;
            this.EffectDate = DateTime.Now;
            this.ExpireDate = DateTime.Now;
            this.CostAmount = 0;
        }

        /// <summary>
        /// Constructor with param
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public CostInfo(DbDataReader dr)
        {
            this.RowNumber = long.Parse(dr["RowNumber"].ToString());

            this.ID = (int)dr["ID"];
            this.CostCode = (string)dr["CostCode"];
            this.CostName = (string)dr["CostName"];
            this.CreateDate = (DateTime)dr["CreateDate"];
            this.CreateUID = (int)dr["CreateUID"];
            this.UpdateDate = (DateTime)dr["UpdateDate"];
            this.UpdateUID = (int)dr["UpdateUID"];

            this.HID = (int)dr["HID"];
            this.EffectDate = (DateTime)dr["EffectDate"];
            this.ExpireDate = (DateTime)dr["ExpireDate"];
            this.CostAmount = decimal.Parse(dr["CostAmount"].ToString() != string.Empty ? dr["CostAmount"].ToString() : "0");
        }
    }
}
