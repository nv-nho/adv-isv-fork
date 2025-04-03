using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OMS.Utilities;
using System.Data.Common;

namespace OMS.Models
{
    public class T_WorkingCalendar_D : M_Base<T_WorkingCalendar_D>
    {
        #region Variant

        /// <summary>
        /// HID
        /// </summary>
        public int HID { get; set; }

        /// <summary>
        /// WorkingDate
        /// </summary>
        public DateTime WorkingDate { get; set; }

        /// <summary>
        /// WorkingSystemID
        /// </summary>
        private int workingSystemID;

        private int ivent1;

        #endregion 
        #region Property

        /// <summary>
        /// Get or set CalendarCD
        /// </summary>
        public int WorkingSystemID
        {
            get { return workingSystemID; }
            set
            {
                if (value != workingSystemID)
                {
                    workingSystemID = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set CalendarCD
        /// </summary>
        public int Ivent1
        {
            get { return ivent1; }
            set
            {
                if (value != ivent1)
                {
                    ivent1 = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }
        #endregion

        #region Contructor

        /// <summary>
        /// Contructor T_WorkingCalendar_H
        /// </summary>
        public T_WorkingCalendar_D()
            : base()
        {
            this.HID = 0;
            this.WorkingDate = DateTime.MinValue;
            this.WorkingSystemID = 0;
            this.Ivent1 = 0;
           
        }

        /// <summary>
        /// Contructor T_WorkingCalendar_H
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public T_WorkingCalendar_D(DbDataReader dr)
        {
            ISecurity sec = Security.Instance;
            this.HID = (int)dr["HID"];
            this.WorkingDate = DateTime.Parse(dr["WorkingDate"].ToString());
            this.WorkingSystemID = (int)dr["WorkingSystemID"];
            this.Ivent1 = (int)dr["Ivent1"];
        }

        #endregion
    }
}
