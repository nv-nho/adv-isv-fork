using System;
using System.Data.Common;

namespace OMS.Models
{
    /// <summary>
    /// Class M_GroupUser_H
    /// Create  :isv.thuy
    /// Date    :25/07/2014
    /// </summary>
    [Serializable]
    public class M_GroupUser_H : M_Base<M_GroupUser_H>
    {
        #region Constant

        public const int GROUP_CODE_SHOW_MAX_LENGTH = 4;
        public const int GROUP_CODE_DB_MAX_LENGTH = 10;
        public const int GROUP_NAME_MAX_LENGTH = 50;
        public const string GROUP_ADMIN = "0001";
        public const string GROUP_GENERAL = "0002";
        public const string GROUP_GROUP_LEADER = "0003";
        public const string GROUP_MANAGER = "0004";

        public const string DB_GROUP_CODE_1 = "0000000001";
        public const string DB_GROUP_CODE_3 = "0000000003";

        

        #endregion

        #region Variable

        public string GroupCD { get; set; }
        private string groupName;

        #endregion

        #region Property

        public string GroupName
        {
            get { return groupName; }
            set
            {
                if (value != groupName)
                {
                    groupName = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        #endregion

        #region Contructor

        /// <summary>
        /// Contructor M_GroupUser_H
        /// </summary>
        public M_GroupUser_H()
            : base()
        {

        }

        public M_GroupUser_H(DbDataReader dr)
            : base(dr)
        {
            this.GroupCD = Utilities.EditDataUtil.ToFixCodeShow((string)dr["GroupCD"], M_GroupUser_H.GROUP_CODE_SHOW_MAX_LENGTH);
            this.groupName = (string)dr["GroupName"];
        }
        #endregion
    }
}
