using System;
using System.Data.Common;
using OMS.Utilities;
using System.Collections.Generic;

namespace OMS.Models
{
    /// <summary>
    /// Class DepartmentInfo
    /// ISV-Giao
    /// </summary>
    [Serializable]
    public class DepartmentInfo
    {
        public int ID { get; set; }
        public long RowNumber { get; set; }
        public string DepartmentCD { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentName2 { get; set; }

        /// <summary>
        /// Constructor class ProjectInfo
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public DepartmentInfo(DbDataReader dr)
        {
            this.ID = int.Parse(dr["ID"].ToString());
            this.RowNumber = (long)dr["RowNumber"];
            this.DepartmentCD = EditDataUtil.ToFixCodeShow((string)dr["DepartmentCD"], M_Department.DEPARTMENT_CODE_SHOW_MAX_LENGTH);
            this.DepartmentName = (string)dr["DepartmentName"];
            this.DepartmentName2 = (string)dr["DepartmentName2"];        
        }

        /// <summary>
        /// Constructor class DepartmentInfo
        /// </summary>
        public DepartmentInfo()
        {
            this.RowNumber = 0;
            this.DepartmentCD = null;
            this.DepartmentName = null;
            this.DepartmentName2 = null;
        }
    }

    [Serializable]
    public class DepartmentSearchInfo
    {
        public long RowNumber { get; set; }
        public string DepartmentCD { get; set; }
        public string DepartmentName { get; set; }

        /// <summary>
        /// Constructor class DepartmentSearchInfo
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public DepartmentSearchInfo(DbDataReader dr)
        {
            this.RowNumber = (long)dr["RowNumber"];
            this.DepartmentCD = EditDataUtil.ToFixCodeShow((string)dr["DepartmentCD"], M_Department.DEPARTMENT_CODE_SHOW_MAX_LENGTH);
            this.DepartmentName = (string)dr["DepartmentName"];
        }

        /// <summary>
        /// Constructor class DepartmentSearchInfo
        /// </summary>
        public DepartmentSearchInfo()
        {
            this.RowNumber = 0;
            this.DepartmentCD = null;
            this.DepartmentName = null;
        }
    }

    /// <summary>
    /// Class DepartmentTreeView
    /// </summary>
    [Serializable]
    public class DepartmentTreeView
    {
        public int id { get; set; }

        public string text { get; set; }

        public virtual List<UserTreeView> children { get; set; }
 
    }

}
