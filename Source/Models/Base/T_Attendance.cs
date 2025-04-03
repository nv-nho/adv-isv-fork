using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace OMS.Models
{
    /// <summary>
    /// Class T_Attendance
    /// Create  :isv.than
    /// Date    :19/10/2017
    /// </summary>
    [Serializable]
    public class T_Attendance : M_Base<T_Attendance>
    {
        #region Constant

        public const string GROUP_ADMIN = "0001";

        #endregion

        #region Variable

        public int UID { get; set; }

        private DateTime date;
        private int wSID;

        private int? entryTime;
        private int? exitTime;

        private int? workingHours;
        private int? lateHours;
        private int? earlyHours;
        private int? sH_Hours;
        private int? lH_Hours;

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

        private int? vacationFlag;
        private int? vacationFullCD;
        private int? vacationMorningCD;
        private int? vacationAfternoonCD;

        private string meMo;
        private int? statusFlag;

        //************ Approval variable ***************
        private int approvalStatus;
        private int approvalUID;
        private DateTime? approvalDate;
        private string approvalNote;
        //************ Approval variable ***************
        private string requestNote;
        //**********************************************

        private int exchangeFlag;
        private DateTime? exchangeDate;

        #endregion

        #region Property
        public DateTime Date
        {
            get { return date; }
            set
            {
                if (value != date)
                {
                    date = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int WSID
        {
            get { return wSID; }
            set
            {
                if (value != wSID)
                {
                    wSID = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int? EntryTime
        {
            get { return entryTime; }
            set
            {
                if (value != entryTime)
                {
                    entryTime = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int? ExitTime
        {
            get { return exitTime; }
            set
            {
                if (value != exitTime)
                {
                    exitTime = value;
                    this.Status = DataStatus.Changed;
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
                    this.Status = DataStatus.Changed;
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
                    this.Status = DataStatus.Changed;
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
                    this.Status = DataStatus.Changed;
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
                    this.Status = DataStatus.Changed;
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
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int? OverTimeHours1
        {
            get { return overTimeHours1; }
            set
            {
                if (value != overTimeHours1)
                {
                    overTimeHours1 = value;
                    this.Status = DataStatus.Changed;
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
                    this.Status = DataStatus.Changed;
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
                    this.Status = DataStatus.Changed;
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
                    this.Status = DataStatus.Changed;
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
                    this.Status = DataStatus.Changed;
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
                    this.Status = DataStatus.Changed;
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
                    this.Status = DataStatus.Changed;
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
                    this.Status = DataStatus.Changed;
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
                    this.Status = DataStatus.Changed;
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
                    this.Status = DataStatus.Changed;
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
                    this.Status = DataStatus.Changed;
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
                    this.Status = DataStatus.Changed;
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
                    this.Status = DataStatus.Changed;
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
                    this.Status = DataStatus.Changed;
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
                    this.Status = DataStatus.Changed;
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
                    this.Status = DataStatus.Changed;
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
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int? VacationFlag
        {
            get { return vacationFlag; }
            set
            {
                if (value != vacationFlag)
                {
                    vacationFlag = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int? VacationFullCD
        {
            get { return vacationFullCD; }
            set
            {
                if (value != vacationFullCD)
                {
                    vacationFullCD = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int? VacationMorningCD
        {
            get { return vacationMorningCD; }
            set
            {
                if (value != vacationMorningCD)
                {
                    vacationMorningCD = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int? VacationAfternoonCD
        {
            get { return vacationAfternoonCD; }
            set
            {
                if (value != vacationAfternoonCD)
                {
                    vacationAfternoonCD = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public string Memo
        {
            get { return meMo; }
            set
            {
                if (value != meMo)
                {
                    meMo = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int? StatusFlag
        {
            get { return statusFlag; }
            set
            {
                if (value != statusFlag)
                {
                    statusFlag = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        //**************** Approval Property ***************
        public int ApprovalStatus
        {
            get { return approvalStatus; }
            set
            {
                if (value != approvalStatus)
                {
                    approvalStatus = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int ApprovalUID
        {
            get { return approvalUID; }
            set
            {
                if (value != approvalUID)
                {
                    approvalUID = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public DateTime? ApprovalDate
        {
            get { return approvalDate; }
            set
            {
                if (value != approvalDate)
                {
                    approvalDate = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public string ApprovalNote
        {
            get { return approvalNote; }
            set
            {
                if (value != approvalNote)
                {
                    approvalNote = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }
        //**************** Approval Property ***************
        public string RequestNote
        {
            get { return requestNote; }
            set
            {
                if (value != requestNote)
                {
                    requestNote = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }
        //************************************************

        public int ExchangeFlag
        {
            get { return exchangeFlag; }
            set
            {
                if (value != exchangeFlag)
                {
                    exchangeFlag = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public DateTime? ExchangeDate
        {
            get { return exchangeDate; }
            set
            {
                if (value != exchangeDate)
                {
                    exchangeDate = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }
        
        #endregion

        #region Contructor
        /// <summary>
        /// Contructor T_Attendance
        /// </summary>
        public T_Attendance()
            : base()
        {

        }

        public T_Attendance(DbDataReader dr)
            : base(dr)
        {
            this.UID = int.Parse(dr["UID"].ToString());

            this.Date = (DateTime)dr["Date"];
            this.WSID = int.Parse(dr["WSID"].ToString());

            if (dr["EntryTime"] != DBNull.Value)
            {
                this.EntryTime = Convert.ToInt32(dr["EntryTime"]);
            }
            else
            {
                this.EntryTime = null;
            }

            if (dr["ExitTime"] != DBNull.Value)
            {
                this.ExitTime = Convert.ToInt32(dr["ExitTime"]);
            }
            else
            {
                this.ExitTime = null;
            }

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

            if (dr["VacationFlag"] != DBNull.Value)
            {
                this.VacationFlag = Convert.ToInt32(dr["VacationFlag"]);
            }
            else
            {
                this.VacationFlag = null;
            }

            if (dr["VacationFullCD"] != DBNull.Value)
            {
                this.VacationFullCD = Convert.ToInt32(dr["VacationFullCD"]);
            }
            else
            {
                this.VacationFullCD = null;
            }
            if (dr["VacationMorningCD"] != DBNull.Value)
            {
                this.VacationMorningCD = Convert.ToInt32(dr["VacationMorningCD"]);
            }
            else
            {
                this.VacationMorningCD = null;
            }

            if (dr["VacationAfternoonCD"] != DBNull.Value)
            {
                this.VacationAfternoonCD = Convert.ToInt32(dr["VacationAfternoonCD"]);
            }
            else
            {
                this.VacationAfternoonCD = null;
            }

            if (dr["Memo"] != DBNull.Value)
            {
                this.Memo = (string)dr["Memo"];
            }
            else
            {
                this.Memo = string.Empty;
            }

            if (dr["Memo"] != DBNull.Value)
            {
                this.Memo = (string)dr["Memo"];
            }
            else
            {
                this.Memo = string.Empty;
            }

            if (dr["StatusFlag"] != DBNull.Value)
            {
                this.StatusFlag = (int)dr["StatusFlag"];
            }
            else
            {
                this.StatusFlag = null;
            }

            //*************** Approval ***********************
            this.ApprovalStatus = (int)dr["ApprovalStatus"];
            this.ApprovalUID = (int)dr["ApprovalUID"];
            if (dr["ApprovalDate"] != DBNull.Value)
            {
                this.ApprovalDate = (DateTime)dr["ApprovalDate"];
            }
            else
            {
                this.ApprovalDate = null;
            }

            if (dr["ApprovalNote"] != DBNull.Value)
            {
                this.ApprovalNote = (string)dr["ApprovalNote"];
            }
            else
            {
                this.ApprovalNote = string.Empty;
            }
            //*************** Request ***********************
            if (dr["RequestNote"] != DBNull.Value)
            {
                this.RequestNote = (string)dr["RequestNote"];
            }
            else
            {
                this.RequestNote = string.Empty;
            }
            //**************************************************

            this.ExchangeFlag = (int)dr["ExchangeFlag"];
            if (dr["ExchangeDate"] != DBNull.Value)
            {
                this.ExchangeDate = (DateTime)dr["ExchangeDate"];
            }
            else
            {
                this.ExchangeDate = null;
            }
        }
        #endregion
    }
}
