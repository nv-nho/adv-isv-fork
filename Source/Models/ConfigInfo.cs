using System;
using System.Data.Common;

namespace OMS.Models
{
    /// <summary>
    /// Class Config Information Model
    /// VN-Nho
    /// </summary>
    [Serializable]
    public class ConfigInfo
    {
        public long RowNumber { get; set; }
        public int ID { get; set; }
        public string ConfigCD { get; set; }
        public string ConfigName { get; set; }

        /// <summary>
        /// Constructor class SettingInfo
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public ConfigInfo(DbDataReader dr)
        {
            this.RowNumber = (long)dr["RowNumber"];
            this.ID = int.Parse(dr["ID"].ToString());
            this.ConfigCD = (string)dr["ConfigCD"];
            this.ConfigName = (string)dr["ConfigName"];
        }

        /// <summary>
        /// Constructor class SettingInfo
        /// </summary>
        public ConfigInfo()
        {
            this.RowNumber = 0;
            this.ID = -1;
            this.ConfigCD = string.Empty;
            this.ConfigName = string.Empty;
        }
    }
}
