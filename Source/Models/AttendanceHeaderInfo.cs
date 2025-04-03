using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Collections;

namespace OMS.Models
{
    /// <summary>
    /// Class AttendaceHeaderInfo
    /// ISV-THAN
    /// </summary>
    [Serializable]
    public class AttendanceHeaderInfo
    {
        public string WorkingHours { get; set; }
        public string LateHours { get; set; }
        public string EarlyHours { get; set; }
        public string SH_Hours { get; set; }
        public string LH_Hours { get; set; }

        public int NumWorkingDays { get; set; }
        public int NumLateDays { get; set; }
        public int NumEarlyDays { get; set; }
        public int NumSH_Days { get; set; }
        public int NumLH_Days { get; set; }

        public string OverTimeHours1 { get; set; }
        public string OverTimeHours2 { get; set; }
        public string OverTimeHours3 { get; set; }
        public string OverTimeHours4 { get; set; }
        public string OverTimeHours5 { get; set; }

        public string TotalOverTimeHours { get; set; }
        public string TotalWorkingHours { get; set; }

        /// <summary>
        /// Constructor class AttendaceHeaderInfo
        /// </summary>
        public AttendanceHeaderInfo()
        {
            this.WorkingHours = string.Empty;
            this.LateHours = string.Empty;
            this.EarlyHours = string.Empty;
            this.SH_Hours = string.Empty;
            this.LH_Hours = string.Empty;
        }
        /// <summary>
        /// Constructor class AttendaceHeaderInfo
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public AttendanceHeaderInfo(DbDataReader dr)
        {
            this.WorkingHours =dr["WorkingHours"].ToString()!=string.Empty? Utilities.CommonUtil.IntToTime(int.Parse(dr["WorkingHours"].ToString()), false):string.Empty;
            this.LateHours = dr["LateHours"].ToString() != string.Empty? Utilities.CommonUtil.IntToTime(int.Parse(dr["LateHours"].ToString()), false):string.Empty;
            this.EarlyHours =dr["EarlyHours"].ToString() != string.Empty? Utilities.CommonUtil.IntToTime(int.Parse(dr["EarlyHours"].ToString()), false):string.Empty;
            this.SH_Hours = dr["SH_Hours"].ToString()!=string.Empty?Utilities.CommonUtil.IntToTime(int.Parse(dr["SH_Hours"].ToString()), false):string.Empty;
            this.LH_Hours = dr["LH_Hours"].ToString() != string.Empty?Utilities.CommonUtil.IntToTime(int.Parse(dr["LH_Hours"].ToString()), false):string.Empty;

            this.NumWorkingDays = (int)dr["NumWorkingDays"];
            this.NumLateDays = (int)dr["NumLateDays"];
            this.NumEarlyDays = (int)dr["NumEarlyDays"];
            this.NumSH_Days = (int)dr["NumSH_Days"];
            this.NumLH_Days = (int)dr["NumLH_Days"];

            this.OverTimeHours1 = dr["OverTimeHours1"].ToString()!=string.Empty?Utilities.CommonUtil.IntToTime(int.Parse(dr["OverTimeHours1"].ToString()), false):string.Empty;
            this.OverTimeHours2 = dr["OverTimeHours2"].ToString() != string.Empty?Utilities.CommonUtil.IntToTime(int.Parse(dr["OverTimeHours2"].ToString()), false):string.Empty;
            this.OverTimeHours3 = dr["OverTimeHours3"].ToString() != string.Empty? Utilities.CommonUtil.IntToTime(int.Parse(dr["OverTimeHours3"].ToString()), false):string.Empty;
            this.OverTimeHours4 = dr["OverTimeHours4"].ToString() != string.Empty?Utilities.CommonUtil.IntToTime(int.Parse(dr["OverTimeHours4"].ToString()), false):string.Empty;
            this.OverTimeHours5 = dr["OverTimeHours5"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["OverTimeHours5"].ToString()), false) : string.Empty;
            this.TotalOverTimeHours = dr["TotalOverTimeHours"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["TotalOverTimeHours"].ToString()), false) : string.Empty;
            this.TotalWorkingHours = dr["TotalWorkingHours"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["TotalWorkingHours"].ToString()), false) : string.Empty;
        }
    }

    /// <summary>
    /// Class AttendanceListExcelModal
    /// </summary>
    [Serializable]
    public class AttendanceListExcelModal
    {
      
        /// <summary>
        /// Get or set DateOfService
        /// </summary>
        public string DateOfService { get; set; }

        /// <summary>
        /// Get or set CalendarName
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// Get or set UserCD
        /// </summary>
        public string UserCD { get; set; }

        /// <summary>
        /// Get or set UserNm
        /// </summary>
        public string UserNm { get; set; }


        public string NumWorkingDays { get; set; }
        public string NumLateDays { get; set; }
        public string NumEarlyDays { get; set; }
        public string NumSH_Days { get; set; }
        public string NumLH_Days { get; set; }

        public string TimeWorkingHours { get; set; }
        public string TimeLateHours { get; set; }
        public string TimeEarlyHours { get; set; }
        public string TimeSH_Hours { get; set; }
        public string TimeLH_Hours { get; set; }

        public string TotalOverTimeHours { get; set; }
        public string TotalWorkingHours { get; set; }

        public ArrayList VacationType { get; set; }

        public ArrayList OverTimeList { get; set; }

        public ArrayList OverTimeForDayList { get; set; }

        public ArrayList DataDetailList { get; set; }

    }


}
