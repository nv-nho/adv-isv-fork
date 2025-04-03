using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using OMS.Utilities;

namespace OMS.Models
{
    /// <summary>
    /// UserCalendar Model
    /// </summary>
    [Serializable]
    public class UserCalendar
    {
        public string CalendarName{get;set;}
        public DateTime InitialDate { get; set; }


        /// <summary>
        /// Constructor  UserCalendar
        /// </summary>
        /// <param name="dr"></param>
        public UserCalendar(DbDataReader dr)
        {
            this.CalendarName = (string)dr["CalendarName"];
            this.InitialDate = (DateTime)dr["InitialDate"];
        }

        public UserCalendar()
        {
            
        }
    }
}
