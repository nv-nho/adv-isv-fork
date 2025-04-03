using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using OMS.Utilities;

namespace OMS.Models
{
    /// <summary>
    /// Class T_AttendanceRsult
    /// </summary>
    [Serializable]
    public class T_AttendanceResult : M_Base<T_AttendanceResult>
    {
        #region Variant

        //ID
        public int ID;

        //ID
        public int uID;

        public int callendarID;

        public int Month;

        public DateTime startDate;

        public DateTime endDate;

        private int? workingHours;
        private int? lateHours;
        private int? earlyHours;
        private int? sH_Hours;
        private int? lH_Hours;

        private int? workingDays;
        private int? lateDays;
        private int? earlyDays;
        private int? sH_Days;
        private int? lH_Days;

        private int? overTimeHours1;
        private int? overTimeHours2;
        private int? overTimeHours3;
        private int? overTimeHours4;
        private int? overTimeHours5;

        private int? sH_OverTimeHours1;
        private int? sH_OverTimeHours2;
        private int? sH_OverTimeHours3;
        private int? sH_OverTimeHours4;
        private int? sH_OverTimeHours5;

        private int? lH_OverTimeHours1;
        private int? lH_OverTimeHours2;
        private int? lH_OverTimeHours3;
        private int? lH_OverTimeHours4;
        private int? lH_OverTimeHours5;

        private int? totalOverTimeHours;
        private int? totalWorkingHours;

        #endregion

        #region Property
        /// <summary>
        /// Get or set HID
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
        /// Get or set uID
        /// </summary>
        public int CallendarID
        {
            get { return callendarID; }
            set
            {
                if (value != callendarID)
                {
                    callendarID = value;
                }
            }
        }
        public int? WorkingHours
        {
            get { return workingHours; }
            set
            {
                if (value != workingHours)
                {
                    workingHours = value;
                }
            }
        }

        public int? LateHours
        {
            get { return lateHours; }
            set
            {
                if (value != lateHours)
                {
                    lateHours = value;
                }
            }
        }

        public int? EarlyHours
        {
            get { return earlyHours; }
            set
            {
                if (value != earlyHours)
                {
                    earlyHours = value;
                }
            }
        }

        public int? SH_Hours
        {
            get { return sH_Hours; }
            set
            {
                if (value != sH_Hours)
                {
                    sH_Hours = value;
                }
            }
        }

        public int? LH_Hours
        {
            get { return lH_Hours; }
            set
            {
                if (value != lH_Hours)
                {
                    lH_Hours = value;

                }
            }
        }

      //-------------
        public int? WorkingDays
        {
            get { return workingDays; }
            set
            {
                if (value != workingDays)
                {
                    workingDays = value;
                }
            }
        }

        public int? LateDays
        {
            get { return lateDays; }
            set
            {
                if (value != lateDays)
                {
                    lateDays = value;
                }
            }
        }

        public int? EarlyDays
        {
            get { return earlyDays; }
            set
            {
                if (value != earlyDays)
                {
                    earlyDays = value;
                }
            }
        }

        public int? SH_Days
        {
            get { return sH_Days; }
            set
            {
                if (value != sH_Days)
                {
                    sH_Days = value;
                }
            }
        }

        public int? LH_Days
        {
            get { return lH_Days; }
            set
            {
                if (value != lH_Days)
                {
                    lH_Days = value;

                }
            }
        }
        //--------

        public int? OverTimeHours1
        {
            get { return overTimeHours1; }
            set
            {
                if (value != overTimeHours1)
                {
                    overTimeHours1 = value;

                }
            }
        }

        public int? OverTimeHours2
        {
            get { return overTimeHours2; }
            set
            {
                if (value != overTimeHours2)
                {
                    overTimeHours2 = value;
                }
            }
        }

        public int? OverTimeHours3
        {
            get { return overTimeHours3; }
            set
            {
                if (value != overTimeHours3)
                {
                    overTimeHours3 = value;
                }
            }
        }

        public int? OverTimeHours4
        {
            get { return overTimeHours4; }
            set
            {
                if (value != overTimeHours4)
                {
                    overTimeHours4 = value;
                }
            }
        }

        public int? OverTimeHours5
        {
            get { return overTimeHours5; }
            set
            {
                if (value != overTimeHours5)
                {
                    overTimeHours5 = value;
                }
            }
        }

        public int? SH_OverTimeHours1
        {
            get { return sH_OverTimeHours1; }
            set
            {
                if (value != sH_OverTimeHours1)
                {
                    sH_OverTimeHours1 = value;
                }
            }
        }
        public int? SH_OverTimeHours2
        {
            get { return sH_OverTimeHours2; }
            set
            {
                if (value != sH_OverTimeHours2)
                {
                    sH_OverTimeHours2 = value;
                }
            }
        }

        public int? SH_OverTimeHours3
        {
            get { return sH_OverTimeHours3; }
            set
            {
                if (value != sH_OverTimeHours3)
                {
                    sH_OverTimeHours3 = value;

                }
            }
        }

        public int? SH_OverTimeHours4
        {
            get { return sH_OverTimeHours4; }
            set
            {
                if (value != sH_OverTimeHours4)
                {
                    sH_OverTimeHours4 = value;
                }
            }
        }

        public int? SH_OverTimeHours5
        {
            get { return sH_OverTimeHours5; }
            set
            {
                if (value != sH_OverTimeHours5)
                {
                    sH_OverTimeHours5 = value;
                }
            }
        }

        public int? LH_OverTimeHours1
        {
            get { return lH_OverTimeHours1; }
            set
            {
                if (value != lH_OverTimeHours1)
                {
                    lH_OverTimeHours1 = value;
                }
            }
        }

        public int? LH_OverTimeHours2
        {
            get { return lH_OverTimeHours2; }
            set
            {
                if (value != lH_OverTimeHours2)
                {
                    lH_OverTimeHours2 = value;

                }
            }
        }

        public int? LH_OverTimeHours3
        {
            get { return lH_OverTimeHours3; }
            set
            {
                if (value != lH_OverTimeHours3)
                {
                    lH_OverTimeHours3 = value;
                }
            }
        }

        public int? LH_OverTimeHours4
        {
            get { return lH_OverTimeHours4; }
            set
            {
                if (value != lH_OverTimeHours4)
                {
                    lH_OverTimeHours4 = value;

                }
            }
        }

        public int? LH_OverTimeHours5
        {
            get { return lH_OverTimeHours5; }
            set
            {
                if (value != lH_OverTimeHours5)
                {
                    lH_OverTimeHours5 = value;
                }
            }
        }

        public int? TotalOverTimeHours
        {
            get { return totalOverTimeHours; }
            set
            {
                if (value != totalOverTimeHours)
                {
                    totalOverTimeHours = value;
                }
            }
        }

        public int? TotalWorkingHours
        {
            get { return totalWorkingHours; }
            set
            {
                if (value != totalWorkingHours)
                {
                    totalWorkingHours = value;
                }
            }
        }

        #endregion

        #region Contructor

        /// <summary>
        /// Contructor T_AttendanceRsult
        /// </summary>
        public T_AttendanceResult()
            : base()
        {

        }
        /// <summary>
        /// Contructor T_AttendanceRsult
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public T_AttendanceResult(DbDataReader dr)
        {
            ISecurity sec = Security.Instance;

            if (dr["ID"] != DBNull.Value)
            {
                this.ID = int.Parse(dr["ID"].ToString());
            }
            else
            {
                this.ID = -1;
            }
            

            this.UID = int.Parse(dr["UID"].ToString());
            this.callendarID = (int)dr["CallendarID"];

            this.Month = int.Parse(dr["Month"].ToString());


            this.startDate = DateTime.Parse(dr["StartDate"].ToString());

            this.endDate = DateTime.Parse(dr["EndDate"].ToString());


            if (dr["WorkingHours"] != DBNull.Value)
            {
                this.WorkingHours = Convert.ToInt32(dr["WorkingHours"]);
            }
            else
            {
                this.WorkingHours = null;
            }

            if (dr["LateHours"] != DBNull.Value)
            {
                this.LateHours = Convert.ToInt32(dr["LateHours"]);
            }
            else
            {
                this.LateHours = null;
            }

            if (dr["EarlyHours"] != DBNull.Value)
            {
                this.EarlyHours = Convert.ToInt32(dr["EarlyHours"]);
            }
            else
            {
                this.EarlyHours = null;
            }

            if (dr["SH_Hours"] != DBNull.Value)
            {
                this.SH_Hours = Convert.ToInt32(dr["SH_Hours"]);
            }
            else
            {
                this.SH_Hours = null;
            }

            if (dr["LH_Hours"] != DBNull.Value)
            {
                this.LH_Hours = Convert.ToInt32(dr["LH_Hours"]);
            }
            else
            {
                this.LH_Hours = null;
            }

            if (dr["WorkingDays"] != DBNull.Value)
            {
                this.WorkingDays = Convert.ToInt32(dr["WorkingDays"]);
            }
            else
            {
                this.WorkingDays = null;
            }

            if (dr["LateDays"] != DBNull.Value)
            {
                this.LateDays = Convert.ToInt32(dr["LateDays"]);
            }
            else
            {
                this.LateDays = null;
            }

            if (dr["EarlyDays"] != DBNull.Value)
            {
                this.EarlyDays = Convert.ToInt32(dr["EarlyDays"]);
            }
            else
            {
                this.EarlyDays = null;
            }

            if (dr["SH_Days"] != DBNull.Value)
            {
                this.SH_Days = Convert.ToInt32(dr["SH_Days"]);
            }
            else
            {
                this.SH_Days = null;
            }

            if (dr["LH_Days"] != DBNull.Value)
            {
                this.LH_Days = Convert.ToInt32(dr["LH_Days"]);
            }
            else
            {
                this.LH_Days = null;
            }

            if (dr["OverTimeHours1"] != DBNull.Value)
            {
                this.OverTimeHours1 = Convert.ToInt32(dr["OverTimeHours1"]);
            }
            else
            {
                this.OverTimeHours1 = null;
            }

            if (dr["OverTimeHours2"] != DBNull.Value)
            {
                this.OverTimeHours2 = Convert.ToInt32(dr["OverTimeHours2"]);
            }
            else
            {
                this.OverTimeHours2 = null;
            }

            if (dr["OverTimeHours3"] != DBNull.Value)
            {
                this.OverTimeHours3 = Convert.ToInt32(dr["OverTimeHours3"]);
            }
            else
            {
                this.OverTimeHours3 = null;
            }

            if (dr["OverTimeHours4"] != DBNull.Value)
            {
                this.OverTimeHours4 = Convert.ToInt32(dr["OverTimeHours4"]);
            }
            else
            {
                this.OverTimeHours4 = null;
            }

            if (dr["OverTimeHours5"] != DBNull.Value)
            {
                this.OverTimeHours5 = Convert.ToInt32(dr["OverTimeHours5"]);
            }
            else
            {
                this.OverTimeHours5 = null;
            }

            if (dr["SH_OverTimeHours1"] != DBNull.Value)
            {
                this.SH_OverTimeHours1 = Convert.ToInt32(dr["SH_OverTimeHours1"]);
            }
            else
            {
                this.SH_OverTimeHours1 = null;
            }

            if (dr["SH_OverTimeHours2"] != DBNull.Value)
            {
                this.SH_OverTimeHours2 = Convert.ToInt32(dr["SH_OverTimeHours2"]);
            }
            else
            {
                this.SH_OverTimeHours2 = null;
            }

            if (dr["SH_OverTimeHours3"] != DBNull.Value)
            {
                this.SH_OverTimeHours3 = Convert.ToInt32(dr["SH_OverTimeHours3"]);
            }
            else
            {
                this.SH_OverTimeHours3 = null;
            }

            if (dr["SH_OverTimeHours4"] != DBNull.Value)
            {
                this.SH_OverTimeHours4 = Convert.ToInt32(dr["SH_OverTimeHours4"]);
            }
            else
            {
                this.SH_OverTimeHours4 = null;
            }

            if (dr["SH_OverTimeHours5"] != DBNull.Value)
            {
                this.SH_OverTimeHours5 = Convert.ToInt32(dr["SH_OverTimeHours5"]);
            }
            else
            {
                this.SH_OverTimeHours5 = null;
            }

            if (dr["LH_OverTimeHours1"] != DBNull.Value)
            {
                this.LH_OverTimeHours1 = Convert.ToInt32(dr["LH_OverTimeHours1"]);
            }
            else
            {
                this.LH_OverTimeHours1 = null;
            }

            if (dr["LH_OverTimeHours2"] != DBNull.Value)
            {
                this.LH_OverTimeHours2 = Convert.ToInt32(dr["LH_OverTimeHours2"]);
            }
            else
            {
                this.LH_OverTimeHours2 = null;
            }

            if (dr["LH_OverTimeHours3"] != DBNull.Value)
            {
                this.LH_OverTimeHours3 = Convert.ToInt32(dr["LH_OverTimeHours3"]);
            }
            else
            {
                this.LH_OverTimeHours3 = null;
            }

            if (dr["LH_OverTimeHours4"] != DBNull.Value)
            {
                this.LH_OverTimeHours4 = Convert.ToInt32(dr["LH_OverTimeHours4"]);
            }
            else
            {
                this.LH_OverTimeHours4 = null;
            }

            if (dr["LH_OverTimeHours5"] != DBNull.Value)
            {
                this.LH_OverTimeHours5 = Convert.ToInt32(dr["LH_OverTimeHours5"]);
            }
            else
            {
                this.LH_OverTimeHours5 = null;
            }

            if (dr["TotalOverTimeHours"] != DBNull.Value)
            {
                this.TotalOverTimeHours = Convert.ToInt32(dr["TotalOverTimeHours"]);
            }
            else
            {
                this.TotalOverTimeHours = null;
            }

            if (dr["TotalWorkingHours"] != DBNull.Value)
            {
                this.TotalWorkingHours = Convert.ToInt32(dr["TotalWorkingHours"]);
            }
            else
            {
                this.TotalWorkingHours = null;
            }
        }

        #endregion
    }
}
