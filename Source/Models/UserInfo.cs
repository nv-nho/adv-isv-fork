using System;
using System.Data.Common;
using OMS.Models;
using OMS.Utilities;
using System.Collections.Generic;

    /// <summary>
    /// Class UserInfo Model
    /// </summary>
    [Serializable]
    public class UserInfo
    {
        public long RowNumber { get; set; }
        public int ID { get; set; }
        public string UserCD { get; set; }
        public string LoginID { get; set; }
        public string UserName1 { get; set; }
        public string UserName2 { get; set; }
        public string GroupCD { get; set; }
        public string GroupName { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentCD { get; set; }
        public string DepartmentName { get; set; }
        public string Password { get; set; }
        public int StatusFlag { get; set; }
        public int Color { get; set; }

        public string CalendarName { get; set;}
        public decimal TotalVacationDays { get; set; }
        
        /// <summary>
        /// Constructor class UserInfo
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public UserInfo(DbDataReader dr)
        {

            ISecurity sec = Security.Instance;

            this.RowNumber = (long)dr["RowNumber"];
            this.ID = int.Parse(dr["ID"].ToString());
            this.UserCD = EditDataUtil.ToFixCodeShow((string)dr["UserCD"], M_User.MAX_USER_CODE_SHOW);
            this.LoginID = (string)dr["LoginID"];
            this.UserName1 = (string)dr["UserName1"];
            this.UserName2 = (string)dr["UserName2"];
            this.GroupCD = EditDataUtil.ToFixCodeShow((string)dr["GroupCD"], M_GroupUser_H.GROUP_CODE_SHOW_MAX_LENGTH);
            this.GroupName = (string)dr["GroupName"];
            this.DepartmentID = (int)dr["DepartmentID"];
            this.DepartmentCD = (string)dr["DepartmentCD"];
            this.DepartmentName = (string)dr["DepartmentName"];
            this.Password = sec.Decrypt((string)dr["Password"]);
            this.StatusFlag = int.Parse(dr["StatusFlag"].ToString());
            this.Color = -1;
            if (this.StatusFlag == (int)DeleteFlag.Deleted)
            {
                this.Color = (int)ColorList.Danger;
            }
            if (HasColumn(dr, "CalendarName"))
            {
                this.CalendarName = (string)dr["CalendarName"];
            }
            else
            {
                this.CalendarName = string.Empty;
            }
            if (HasColumn(dr, "TotalVacationDays"))
            {
                this.TotalVacationDays = decimal.Parse(dr["TotalVacationDays"].ToString());
            }
            else
            {
                this.TotalVacationDays = 0;
            }
        }

        /// <summary>
        /// Constructor class UserInfo
        /// </summary>
        public UserInfo()
        {
            this.RowNumber = 0;
            this.UserCD = string.Empty;
            this.LoginID = string.Empty;
            this.UserName1 = string.Empty;
            this.UserName2 = string.Empty;
            this.GroupCD = string.Empty;
            this.GroupName = string.Empty;
            this.DepartmentCD = string.Empty;
            this.DepartmentName = string.Empty;
            this.Password = string.Empty;
            this.CalendarName = string.Empty;
            this.TotalVacationDays = 0;
        }

        public bool HasColumn(DbDataReader dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }
    }    

    /// <summary>
    /// Class UserSearchInfo Model
    /// </summary>
    [Serializable]
    public class  UserSearchInfo
    {
        public long RowNumber { get; set; }
        public string UserCD { get; set; }
        public string LoginID { get; set; }
        public string UserName1 { get; set; }
        public string UserName2 { get; set; }
        public string GroupCD { get; set; }
        public string GroupName { get; set; }
        public string DepartmentCD { get; set; }
        public string DepartmentName { get; set; }
        public string Password { get; set; }

        /// <summary>
        /// Constructor class UserInfo
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public UserSearchInfo(DbDataReader dr)
        {
            ISecurity sec = Security.Instance;

            this.RowNumber = (long)dr["RowNumber"];
            this.UserCD = EditDataUtil.ToFixCodeShow((string)dr["UserCD"], M_User.MAX_USER_CODE_SHOW);
            this.LoginID = (string)dr["LoginID"];
            this.UserName1 = (string)dr["UserName1"];
            this.UserName2 = (string)dr["UserName2"];
            this.GroupCD = EditDataUtil.ToFixCodeShow((string)dr["GroupCD"], M_GroupUser_H.GROUP_CODE_SHOW_MAX_LENGTH);
            this.GroupName = (string)dr["GroupName"];
            this.DepartmentCD = (string)dr["DepartmentCD"];
            this.DepartmentName = (string)dr["DepartmentName"];
            this.Password =sec.Decrypt((string)dr["Password"]);
           
        }

        /// <summary>
        /// Constructor class UserInfo
        /// </summary>
        public UserSearchInfo()
        {
            this.RowNumber = 0;
            this.UserCD = null;
            this.LoginID = null;
            this.UserName1 = null;
            this.UserName2 = null;
            this.GroupCD = null;
            this.GroupName = null;
            this.DepartmentCD = null;
            this.DepartmentName = null;
            this.Password = null;
        }
    }

    /// <summary>
    /// Class UserTreeView
    /// </summary>
    [Serializable]
    public class UserTreeView
    {
        public string id { get; set; }

        public int departmentid { get; set; }

        public string text { get; set; }


        /// <summary>
        /// Constructor class UserTreeView
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public UserTreeView(DbDataReader dr)
        {
            this.departmentid = dr["DepartmentID"] == null ? 0 : int.Parse(dr["DepartmentID"].ToString());
            this.id =   dr["ID"] == null ? string.Empty : dr["ID"].ToString();
            this.text = (string)dr["UserName1"];
        }

        /// <summary>
        /// Constructor class UserTreeView
        /// </summary>
        public UserTreeView(){ }
    }