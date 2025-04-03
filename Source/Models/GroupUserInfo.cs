using System;
using System.Data.Common;
using OMS.Utilities;

namespace OMS.Models
{
    /// <summary>
    /// Class GroupUser Info
    /// ISV-TRUC
    /// </summary>
    [Serializable]
    public class GroupUserInfo
    {
        public int ID { get; set; }
        public long RowNumber { get; set; }
        public string GroupCD { get; set; }
        public string GroupName { get; set; }

        /// <summary>
        /// Constructor class GroupUserInfo
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public GroupUserInfo(DbDataReader dr)
        {
            this.ID = int.Parse(dr["ID"].ToString());
            this.RowNumber = (long)dr["RowNumber"];
            this.GroupCD = EditDataUtil.ToFixCodeShow((string)dr["GroupCD"], M_GroupUser_H.GROUP_CODE_SHOW_MAX_LENGTH);
            this.GroupName = (string)dr["GroupName"];
        }

        /// <summary>
        /// Constructor class GroupUserInfo
        /// </summary>
        public GroupUserInfo()
        {
            this.RowNumber = 0;
            this.GroupCD = null;
            this.GroupName = null;
        }
    }

    [Serializable]
    public class GroupUserSearchInfo
    {
        public long RowNumber { get; set; }
        public string GroupCD { get; set; }
        public string GroupName { get; set; }

        /// <summary>
        /// Constructor class GroupUserSearchInfo
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public GroupUserSearchInfo(DbDataReader dr)
        {
            this.RowNumber = (long)dr["RowNumber"];
            this.GroupCD = EditDataUtil.ToFixCodeShow((string)dr["GroupCD"], M_GroupUser_H.GROUP_CODE_SHOW_MAX_LENGTH);
            this.GroupName = (string)dr["GroupName"];
        }

        /// <summary>
        /// Constructor class GroupUserSearchInfo
        /// </summary>
        public GroupUserSearchInfo()
        {
            this.RowNumber = 0;
            this.GroupCD = null;
            this.GroupName = null;
        }
    }

}
