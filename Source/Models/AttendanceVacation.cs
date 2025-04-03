using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Collections;

namespace OMS.Models
{
    /// <summary>
    /// Class AttendanceVacation
    /// ISV-NHAN
    /// </summary>
    [Serializable]
    public class AttendanceVacation
    {
        //CalendarCD
        public int CalendarCD { get; set; }

        //month_innit
        public DateTime month_innit { get; set; }

        //month
        public int month { get; set; }

        //dayOff
        public decimal? dayOff { get; set; }

        /// <summary>
        /// Constructor class AttendanceVerification
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public AttendanceVacation(DbDataReader dr)
        {
            //CalendarCD
            this.CalendarCD = int.Parse(dr["CalendarCD"].ToString());

            //month_innit
            this.month_innit = DateTime.Parse(dr["month_innit"].ToString());

            //month
            this.month = int.Parse(dr["month"].ToString());

            //dayOff
            this.dayOff = decimal.Parse(dr["dayOff"].ToString());
        }

        /// <summary>
        /// Constructor class AttendanceVerification
        /// </summary>
        public AttendanceVacation()
        {
            this.CalendarCD = 0;
            this.month = 0;
            this.dayOff = 0;
        }
    }
}
