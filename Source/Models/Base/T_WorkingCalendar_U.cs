using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using OMS.Utilities;

namespace OMS.Models
{
    /// <summary>
    /// Class T_WorkingCalendar_U
    /// </summary>
    [Serializable]
    public class T_WorkingCalendar_U
    {
        #region Variant

        /// <summary>
        /// hID
        /// </summary>
        private int hID;

        /// <summary>
        /// uID
        /// </summary>
        private int uID;

        /// <summary>
        /// Status
        /// </summary>
        private DataStatus _status;

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
        public int UID
        {
            get { return uID; }
            set
            {
                if (value != uID)
                {
                    uID = value;
                }
            }
        }

        /// <summary>
        /// Status
        /// </summary>
        public DataStatus Status
        {
            get
            {
                return this._status;
            }
        }

        #endregion

         #region Contructor

        /// <summary>
        /// Contructor T_WorkingCalendar_U
        /// </summary>
        public T_WorkingCalendar_U()
            : base()
        {
            
        }
        /// <summary>
        /// Contructor T_WorkingCalendar_U
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public T_WorkingCalendar_U(DbDataReader dr)
        {
            ISecurity sec = Security.Instance;
            this.HID = (int)dr["HID"];
            this.UID = (int)dr["UID"];
        }

        #endregion
    }
}
