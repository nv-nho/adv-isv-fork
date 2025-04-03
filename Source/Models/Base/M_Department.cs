using System;
using System.Data.Common;

namespace OMS.Models
{
    /// <summary>
    /// Class M_Department
    /// Create  :isv.Giao
    /// Date    :03/11/2017
    /// </summary>
    [Serializable]
    public class M_Department : M_Base<M_Department>
    {
        #region Constant

        public const int DEPARTMENT_CODE_SHOW_MAX_LENGTH = 4;
        public const int DEPARTMENT_CODE_DB_MAX_LENGTH = 10;
        public const int DEPARTMENT_NAME_MAX_LENGTH = 100;
        public const int DEPARTMENT_NAME2_MAX_LENGTH = 50;
        public const string GROUP_ADMIN = "0001";

        public const string DB_DEPARTMENT_CODE_2 = "0000000002";

        #endregion

        #region Variable
        public string DepartmentCD { get; set; }
        private string departmentName;
        private string departmentName2;
        #endregion

        #region Property

        public string DepartmentName
        {
            get { return departmentName; }
            set
            {
                if (value != departmentName)
                {
                    departmentName = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public string DepartmentName2
        {
            get { return departmentName2; }
            set
            {
                if (value != departmentName2)
                {
                    departmentName2 = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        #endregion

        #region Contructor

        /// <summary>
        /// Contructor M_WorkingSystem
        /// </summary>
        public M_Department()
            : base()
        {

        }

        public M_Department(DbDataReader dr)
            : base(dr)
        {
            // this.ProjectCD = Utilities.EditDataUtil.ToFixCodeShow((string)dr["ProjectCD"], M_Project.PROJECT_CODE_SHOW_MAX_LENGTH);
            this.DepartmentCD = (string)dr["DepartmentCD"];
            this.departmentName = (string)dr["DepartmentName"];
            this.departmentName2 = (string)dr["DepartmentName2"];
        }
        #endregion
    }
}
