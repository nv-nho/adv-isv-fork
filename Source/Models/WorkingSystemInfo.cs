using System;
using System.Data.Common;
using OMS.Utilities;

namespace OMS.Models
{
    /// <summary>
    /// Class WorkingSystem Info
    /// ISV-THAN
    /// </summary>
    [Serializable]
    public class WorkingSystemInfo
    {
        public int ID { get; set; }
        public long RowNumber { get; set; }
        public string WorkingSystemCD { get; set; }
        public string WorkingSystemName { get; set; }
        public string WorkingSystemName2 { get; set; }

        /// <summary>
        /// Constructor class WorkingSystemInfo
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public WorkingSystemInfo(DbDataReader dr)
        {
            this.ID = int.Parse(dr["ID"].ToString());
            this.RowNumber = (long)dr["RowNumber"];
            //?
            //this.WorkingSystemCD = EditDataUtil.ToFixCodeShow((string)dr["WorkingSystemCD"], M_WorkingSystem.WORKING_SYSTEM_CODE_SHOW_MAX_LENGTH);
            this.WorkingSystemCD = (string)dr["WorkingSystemCD"];

            this.WorkingSystemName = (string)dr["WorkingSystemName"];
            this.WorkingSystemName2 = (string)dr["WorkingSystemName2"];
        }

        /// <summary>
        /// Constructor class WorkingSystemInfo
        /// </summary>
        public WorkingSystemInfo()
        {
            this.RowNumber = 0;
            this.WorkingSystemCD = null;
            this.WorkingSystemName = null;
            this.WorkingSystemName2 = null;
        }
    }

    [Serializable]
    public class WorkingSystemSearchInfo
    {
        public long RowNumber { get; set; }
        public string WorkingSystemCD { get; set; }
        public string WorkingSystemName { get; set; }
        public string WorkingSystemName2 { get; set; }

        /// <summary>
        /// Constructor class WorkingSystemSearchInfo
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public WorkingSystemSearchInfo(DbDataReader dr)
        {
            this.RowNumber = (long)dr["RowNumber"];
            //this.WorkingSystemCD = EditDataUtil.ToFixCodeShow((string)dr["WorkingSystemCD"], M_WorkingSystem.WORKING_SYSTEM_CODE_SHOW_MAX_LENGTH);
            this.WorkingSystemCD = (string)dr["WorkingSystemCD"];
            this.WorkingSystemName = (string)dr["WorkingSystemName"];
            this.WorkingSystemName2 = (string)dr["WorkingSystemName2"];
        }

        /// <summary>
        /// Constructor class GroupUserSearchInfo
        /// </summary>
        public WorkingSystemSearchInfo()
        {
            this.RowNumber = 0;
            this.WorkingSystemCD = null;
            this.WorkingSystemName = null;
            this.WorkingSystemName2 = null;
        }
    }

}
