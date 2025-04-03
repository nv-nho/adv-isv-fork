using System;
using System.Data.Common;

namespace OMS.Models
{
    /// <summary>
    /// Class SettingInfo
    /// </summary>
    [Serializable]
    public class SettingInfo
    {
        public int ID { get; set; }
        public string Logo1 { get; set; }
        public string Logo2 { get; set; }

        /// <summary>
        /// Constructor class SettingInfo
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public SettingInfo(DbDataReader dr)
        {
            this.ID = int.Parse(dr["ID"].ToString());

            this.Logo1 = (string)dr["Logo1"];
            this.Logo2 = (string)dr["Logo2"];
        }

        /// <summary>
        /// Constructor class SettingInfo
        /// </summary>
        public SettingInfo()
        {
 
            this.Logo1 = string.Empty;
            this.Logo2 = string.Empty;
        }
    }
}
