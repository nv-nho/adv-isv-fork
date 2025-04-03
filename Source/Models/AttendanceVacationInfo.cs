using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Collections;

namespace OMS.Models
{
    /// <summary>
    /// Class AttendanceVacationInfo
    /// ISV-NHAN
    /// </summary>
    [Serializable]
    public class AttendanceVacationInfo
    {
        //Year
        public int Year { get; set; }

        //Start Month
        public int StartMonth { get; set; }

        //Next Month
        public int NextMonth1 { get; set; }

        //Next Month
        public int NextMonth2 { get; set; }

        //Next Month
        public int NextMonth3 { get; set; }

        //Next Month
        public int NextMonth4 { get; set; }

        //Next Month
        public int NextMonth5 { get; set; }

        //Next Month
        public int NextMonth6 { get; set; }

        //Next Month
        public int NextMonth7 { get; set; }

        //Next Month
        public int NextMonth8 { get; set; }

        //Next Month
        public int NextMonth9 { get; set; }

        //Next Month
        public int NextMonth10 { get; set; }

        //End Month
        public int EndMonth { get; set; }

        //dayOff
        public decimal? SumDayOff1 { get; set; }

        //dayOff
        public decimal? SumDayOff2 { get; set; }

        //dayOff
        public decimal? SumDayOff3 { get; set; }

        //dayOff
        public decimal? SumDayOff4 { get; set; }

        //dayOff
        public decimal? SumDayOff5 { get; set; }

        //dayOff
        public decimal? SumDayOff6 { get; set; }

        //dayOff
        public decimal? SumDayOff7 { get; set; }

        //dayOff
        public decimal? SumDayOff8 { get; set; }

        //dayOff
        public decimal? SumDayOff9 { get; set; }

        //dayOff
        public decimal? SumDayOff10 { get; set; }

        //dayOff
        public decimal? SumDayOff11 { get; set; }

        //dayOff
        public decimal? SumDayOff12 { get; set; }
    }
}
