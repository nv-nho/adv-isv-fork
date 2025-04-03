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
    public class T_VacationResult : M_Base<T_VacationResult>
    {
        #region Variant

        //ID
        public int hID;

        public int vacationID;

        public string vacationName;

        public decimal vacationDate;

        #endregion

        #region Property
        /// <summary>
        /// Get or set HID
        /// </summary>
        public int HID
        {
            get { return hID; }
            set
            {
                if (value != hID)
                {
                    hID = value;
                }
            }
        }

        /// <summary>
        /// Get or set uID
        /// </summary>
        public int VacationID
        {
            get { return vacationID; }
            set
            {
                if (value != vacationID)
                {
                    vacationID = value;
                }
            }
        }
 
        #endregion

        #region Contructor

        /// <summary>
        /// Contructor T_AttendanceRsult
        /// </summary>
        public T_VacationResult()
            : base()
        {

        }
        /// <summary>
        /// Contructor T_AttendanceRsult
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public T_VacationResult(DbDataReader dr)
        {
            ISecurity sec = Security.Instance;

            this.HID = (int)dr["HID"];

            this.VacationID = int.Parse(dr["VacationID"].ToString());
            this.vacationName = dr["vacationName"].ToString();
            this.vacationDate = decimal.Parse(dr["vacationDate"].ToString());
        }
        #endregion
    }
}
