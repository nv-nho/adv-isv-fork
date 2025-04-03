using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using OMS.Utilities;

namespace OMS.Models
{
    /// <summary>
    /// Class T_VacationResult
    /// </summary>
    [Serializable]
    public class T_WorkResult : M_Base<T_WorkResult>
    {
        #region Variant

        //ID
        public int HID;

        public int ProjectID;

        public string ProjectCD;

        public string ProjectName;
        public string WorkPlace;
        public int WorkTime;

        #endregion

        #region Property
        /// <summary>
        /// Get or set HID
        /// </summary>


        #endregion

        #region Contructor

        /// <summary>
        /// Contructor T_AttendanceRsult
        /// </summary>
        public T_WorkResult()
            : base()
        {

        }
        /// <summary>
        /// Contructor T_AttendanceRsult
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public T_WorkResult(DbDataReader dr)
        {
            ISecurity sec = Security.Instance;

            this.HID = (int)dr["HID"];

            this.ProjectID = int.Parse(dr["ProjectID"].ToString());
            this.ProjectCD = dr["ProjectCD"].ToString();
            this.ProjectName = dr["ProjectName"].ToString();
            this.WorkPlace = dr["WorkPlace"].ToString();
            this.WorkTime = int.Parse(dr["WorkTime"].ToString());
        }
        #endregion
    }
}
