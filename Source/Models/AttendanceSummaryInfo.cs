using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Collections;
using OMS.Utilities;

namespace OMS.Models
{
    /// <summary>
    /// Class AttendanceApprovalInfo
    /// ISV-HIEN
    /// </summary>
    [Serializable]
    public class AttendanceSummaryInfo
    {
        //RowNumber
        public long RowNumber { get; set; }

        //ID
        public int ID { get; set; }

        //UID
        public int UID { get; set; }

        //社員コード
        public string UserCD { get; set; }

        //社員名1
        public string UserName1 { get; set; }

        //部門名
        public string DepartmentName { get; set; }

        //出勤
        public string WorkingHours { get; set; }

        //遅刻
        public string LateHours { get; set; }

        //早退
        public string EarlyHours { get; set; }

        //所定休日
        public string SH_Hours { get; set; }

        //法定休日
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

        public string SH_OverTimeHours1 { get; set; }
        public string SH_OverTimeHours2 { get; set; }
        public string SH_OverTimeHours3 { get; set; }
        public string SH_OverTimeHours4 { get; set; }
        public string SH_OverTimeHours5 { get; set; }

        public string LH_OverTimeHours1 { get; set; }
        public string LH_OverTimeHours2 { get; set; }
        public string LH_OverTimeHours3 { get; set; }
        public string LH_OverTimeHours4 { get; set; }
        public string LH_OverTimeHours5 { get; set; }

        public int TotalOverTimeHours1 { get; set; }
        public int TotalOverTimeHours2 { get; set; }
        public int TotalOverTimeHours3 { get; set; }
        public int TotalOverTimeHours4 { get; set; }
        public int TotalOverTimeHours5 { get; set; }

        public string TotalOverTimeHours { get; set; }
        public string TotalWorkingHours { get; set; }

        public bool IsUnSubmit { get; set; }

        /// <summary>
        /// Constructor class AttendanceApprovalInfo
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public AttendanceSummaryInfo(DbDataReader dr)
        {
            //RowNumber
            this.RowNumber = (long)dr["RowNumber"];

            //UID
            this.UID = int.Parse(dr["UID"].ToString());

            //社員コード
            this.UserCD = dr["UserCD"].ToString();

            //部門名1
            this.UserName1 = dr["UserName1"].ToString();
            
            //部門名
            this.DepartmentName = dr["DepartmentName"].ToString();

            //出勤
            this.WorkingHours = dr["WorkingHours"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["WorkingHours"].ToString()), false) : string.Empty;
            //遅刻
            this.LateHours = dr["LateHours"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["LateHours"].ToString()), false) : string.Empty;
            //早退
            this.EarlyHours = dr["EarlyHours"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["EarlyHours"].ToString()), false) : string.Empty;
            //所定休日
            this.SH_Hours = dr["SH_Hours"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["SH_Hours"].ToString()), false) : string.Empty;
            //法定休日
            this.LH_Hours = dr["LH_Hours"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["LH_Hours"].ToString()), false) : string.Empty;

            this.NumWorkingDays = (int)dr["NumWorkingDays"];
            this.NumLateDays = (int)dr["NumLateDays"];
            this.NumEarlyDays = (int)dr["NumEarlyDays"];
            this.NumSH_Days = (int)dr["NumSH_Days"];
            this.NumLH_Days = (int)dr["NumLH_Days"];

            //出勤
            this.OverTimeHours1 = dr["OverTimeHours1"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["OverTimeHours1"].ToString()), false) : "0";
            //遅刻
            this.OverTimeHours2 = dr["OverTimeHours2"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["OverTimeHours2"].ToString()), false) : "0";
            //早退
            this.OverTimeHours3 = dr["OverTimeHours3"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["OverTimeHours3"].ToString()), false) : "0";
            //所定休日
            this.OverTimeHours4 = dr["OverTimeHours4"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["OverTimeHours4"].ToString()), false) : "0";
            //法定休日
            this.OverTimeHours5 = dr["OverTimeHours5"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["OverTimeHours5"].ToString()), false) : "0";

            //出勤
            this.SH_OverTimeHours1 = dr["SH_OverTimeHours1"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["SH_OverTimeHours1"].ToString()), false) : "0";
            //遅刻
            this.SH_OverTimeHours2 = dr["SH_OverTimeHours2"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["SH_OverTimeHours2"].ToString()), false) : "0";
            //早退
            this.SH_OverTimeHours3 = dr["SH_OverTimeHours3"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["SH_OverTimeHours3"].ToString()), false) : "0";
            //所定休日
            this.SH_OverTimeHours4 = dr["SH_OverTimeHours4"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["SH_OverTimeHours4"].ToString()), false) : "0";
            //法定休日
            this.SH_OverTimeHours5 = dr["SH_OverTimeHours5"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["SH_OverTimeHours5"].ToString()), false) : "0";

            //出勤
            this.LH_OverTimeHours1 = dr["LH_OverTimeHours1"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["LH_OverTimeHours1"].ToString()), false) : "0";
            //遅刻
            this.LH_OverTimeHours2 = dr["LH_OverTimeHours2"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["LH_OverTimeHours2"].ToString()), false) : "0";
            //早退
            this.LH_OverTimeHours3 = dr["LH_OverTimeHours3"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["LH_OverTimeHours3"].ToString()), false) : "0";
            //所定休日
            this.LH_OverTimeHours4 = dr["LH_OverTimeHours4"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["LH_OverTimeHours4"].ToString()), false) : "0";
            //法定休日
            this.LH_OverTimeHours5 = dr["LH_OverTimeHours5"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["LH_OverTimeHours5"].ToString()), false) : "0";

            this.TotalOverTimeHours1 = Utilities.CommonUtil.TimeToInt(this.OverTimeHours1) + Utilities.CommonUtil.TimeToInt(this.SH_OverTimeHours1) + Utilities.CommonUtil.TimeToInt(this.LH_OverTimeHours1);
            this.TotalOverTimeHours2 = Utilities.CommonUtil.TimeToInt(this.OverTimeHours2) + Utilities.CommonUtil.TimeToInt(this.SH_OverTimeHours2) + Utilities.CommonUtil.TimeToInt(this.LH_OverTimeHours2);
            this.TotalOverTimeHours3 = Utilities.CommonUtil.TimeToInt(this.OverTimeHours3) + Utilities.CommonUtil.TimeToInt(this.SH_OverTimeHours3) + Utilities.CommonUtil.TimeToInt(this.LH_OverTimeHours3);
            this.TotalOverTimeHours4 = Utilities.CommonUtil.TimeToInt(this.OverTimeHours4) + Utilities.CommonUtil.TimeToInt(this.SH_OverTimeHours4) + Utilities.CommonUtil.TimeToInt(this.LH_OverTimeHours4);
            this.TotalOverTimeHours5 = Utilities.CommonUtil.TimeToInt(this.OverTimeHours5) + Utilities.CommonUtil.TimeToInt(this.SH_OverTimeHours5) + Utilities.CommonUtil.TimeToInt(this.LH_OverTimeHours5);

            this.TotalOverTimeHours = dr["TotalOverTimeHours"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["TotalOverTimeHours"].ToString()), false) : string.Empty;
            this.TotalWorkingHours = dr["TotalWorkingHours"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["TotalWorkingHours"].ToString()), false) : string.Empty;
            IsUnSubmit = false;

        }

        /// <summary>
        /// Constructor class AttendanceSummaryInfo
        /// </summary>
        public AttendanceSummaryInfo()
        {

            WorkingHours = string.Empty;
            UserCD = string.Empty;
            UserName1 = string.Empty;
            DepartmentName = string.Empty;
            LateHours = string.Empty;
            EarlyHours = string.Empty;
            SH_Hours = string.Empty;
            LH_Hours = string.Empty;

            OverTimeHours1 = string.Empty;
            OverTimeHours2 = string.Empty;
            OverTimeHours3 = string.Empty;
            OverTimeHours4 = string.Empty;
            OverTimeHours5 = string.Empty;

            SH_OverTimeHours1 = string.Empty;
            SH_OverTimeHours2 = string.Empty;
            SH_OverTimeHours3 = string.Empty;
            SH_OverTimeHours4 = string.Empty;
            SH_OverTimeHours5 = string.Empty;

            LH_OverTimeHours1 = string.Empty;
            LH_OverTimeHours2 = string.Empty;
            LH_OverTimeHours3 = string.Empty;
            LH_OverTimeHours4 = string.Empty;
            LH_OverTimeHours5 = string.Empty;

            TotalOverTimeHours1 = 0;
            TotalOverTimeHours2 = 0;
            TotalOverTimeHours3 = 0;
            TotalOverTimeHours4 = 0;
            TotalOverTimeHours5 = 0;

            TotalOverTimeHours = string.Empty;
            TotalWorkingHours = string.Empty;
            IsUnSubmit = false;
        }
    }

    /// <summary>
    /// Class VacationDateInFoByAttendanceApproval
    /// </summary>
    [Serializable]
    public class VacationDateInFoByAttendanceSummary
    {
        /// <summary>
        /// Value1
        /// </summary>
        public int Value1 { get; set; }

        /// <summary>
        /// Value2
        /// </summary>
        public string Value3 { get; set; }

        /// <summary>
        /// Value2
        /// </summary>
        public string VacationDate { get; set; }

        #region Contructor

        /// <summary>
        /// Contructor of M_Setting_D
        /// </summary>
        /// <param name="dr">Database data reader</param>
        public VacationDateInFoByAttendanceSummary(DbDataReader dr)
        {
            this.Value1 = (int)dr["Value1"];
            this.Value3 = (string)dr["Value3"];
            this.VacationDate = (string)dr["VacationDate"].ToString();
        }

        #endregion
    }

    /// <summary>
    /// Class AttendanceSummaryListExcelModal
    /// </summary>
    [Serializable]
    public class AttendanceSummaryListExcelModal
    {
        /// <summary>
        /// Get or set CalendarNm
        /// </summary>
        public string CalendarNm { get; set; }

        /// <summary>
        /// Get or set FromDate
        /// </summary>
        public string FromDate { get; set; }

        /// <summary>
        /// Get or set ToDate
        /// </summary>
        public string ToDate { get; set; }

        /// <summary>
        /// Get or set DepartmentNm
        /// </summary>
        public string DepartmentNm { get; set; }

        /// <summary>
        /// Get or set UserNm
        /// </summary>
        public string UserNm { get; set; }

        /// <summary>
        /// Get or set ExtractionConditions
        /// </summary>
        public string ExtractionConditions { get; set; }

        /// <summary>
        /// Get or set ApprovalState
        /// </summary>
        public string ApprovalState { get; set; }

        public IList<M_Config_D> VacationHeaderList { get; set; }

        public IList<M_Config_D> OverTimeHeaderList { get; set; }
        
        public ArrayList VacationDetailList { get; set; }

        public ArrayList OverTimeDetailList { get; set; }

        public ArrayList DataDetailList { get; set; }

        /// <summary>
        /// Constructor class AttendanceSummaryInfo
        /// </summary>
        public AttendanceSummaryListExcelModal()
        {
            CalendarNm = string.Empty;
            FromDate = string.Empty;
            ToDate = string.Empty;
            DepartmentNm = string.Empty;
            UserNm = string.Empty;
            ExtractionConditions = string.Empty;
            ApprovalState = string.Empty;
            VacationDetailList = new ArrayList();
            OverTimeDetailList = new ArrayList();
            DataDetailList = new ArrayList();
        }
    }

    /// <summary>
    /// Class AttendanceSummaryListExcelModal
    /// </summary>
    [Serializable]
    public class AttendanceSummaryExchangeDateExcelModal
    {
        /// <summary>
        /// Get or set DepartmentNm
        /// </summary>
        public string DepartmentNm { get; set; }

        public IList<ExchangeDateExcelModel> DataList { get; set; }


        /// <summary>
        /// Constructor class AttendanceSummaryExchangeDateExcelModal
        /// </summary>
        public AttendanceSummaryExchangeDateExcelModal()
        {
            DepartmentNm = string.Empty;
            DataList = new List<ExchangeDateExcelModel>();
        }
    }

    /// <summary>
    /// Class AttendanceApprovalInfo
    /// </summary>
    [Serializable]
    public class ExchangeDateExcelModel
    {
        //社員番号
        public string UserCD { get; set; }

        //社員名
        public string UserName { get; set; }

        //種類
        public string WorkingTypeName { get; set; }

        //休日出勤日
        public DateTime Date { get; set; }

        //開始
        public string EntryTime { get; set; }

        //終了
        public string ExitTime { get; set; }

        //振休取得日
        public DateTime? UsedDate { get; set; }

        /// <summary>
        /// Constructor class AttendanceApprovalInfo
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public ExchangeDateExcelModel(DbDataReader dr)
        {
            //社員番号
            this.UserCD = EditDataUtil.ToFixCodeShow((string)dr["UserCD"], M_User.MAX_USER_CODE_SHOW);
            //社員名
            this.UserName = dr["UserName"].ToString();
            //種類
            this.WorkingTypeName = dr["WorkingTypeName"].ToString();
            //休日出勤日
            this.Date = (DateTime)dr["Date"];
            //開始
            this.EntryTime = dr["EntryTime"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["EntryTime"].ToString()), true) : string.Empty;
            //終了
            this.ExitTime = dr["ExitTime"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["ExitTime"].ToString()), true) : string.Empty;
            //振休取得日
            if (dr["UsedDate"] == DBNull.Value) { 
                this.UsedDate = null;
            } else {
                this.UsedDate = (DateTime)dr["UsedDate"];
            }
        }

        /// <summary>
        /// Constructor class AttendanceSummaryInfo
        /// </summary>
        public ExchangeDateExcelModel()
        {
        }
    }

    /// <summary>
    /// Class AttendanceSummaryListExcelModal
    /// </summary>
    [Serializable]
    public class AttendanceSummaryListCSVModal
    {
        public StringBuilder HeaderDataCSV { get; set; }
        
        public IList<M_Config_D> VacationHeaderList { get; set; }

        public IList<M_Config_D> OverTimeHeaderList { get; set; }

        public ArrayList VacationDetailList { get; set; }

        public ArrayList OverTimeDetailList { get; set; }

        public ArrayList DetailDataList { get; set; }

        /// <summary>
        /// Constructor class AttendanceSummaryInfo
        /// </summary>
        public AttendanceSummaryListCSVModal()
        {
            HeaderDataCSV = new StringBuilder();
            VacationDetailList = new ArrayList();
            OverTimeDetailList = new ArrayList();
            DetailDataList = new ArrayList();
        }
    }

    [Serializable]
    public class AttendanceSummaryCSVInfo
    {
        //社員コード
        public string UserCD { get; set; }

        //就業日数
        public string WorkingDays { get; set; }

        //出勤日数
        public string AttendanceDays { get; set; }

        //欠勤日数
        public string[] DaysWithoutWork { get; set; }

        //有休日数
        public string[] DaysLeft { get; set; }

        //特休日数
        public string[] SpecialHolidayNumber { get; set; }

        //休出日数
        public string SubstituteHoliday { get; set; }

        //代休日数
        public string[] DayOff { get; set; }

        //遅早回数
        public string LateEarlyDays { get; set; }

        //出勤時間
        public string AttendanceTimeHours { get; set; }

        //遅早時間
        public string LateTimeHours { get; set; }

        //平日普通残業時間
        public string[] WeekdayNormalOvertimeHours { get; set; }

        //平日深夜残業時間
        public string[] OvertimeLate { get; set; }

        //休日残業時間
        public string OvertimeOnholiday { get; set; }

        //休日深夜残業時間
        public string[] OvertimeMidNight { get; set; }

        //法定内休出日数
        public string StatutoryHoliday { get; set; }

        //法定休日普通残業時間
        public string LH_Hours { get; set; }

        //法定休日深夜残業時間
        public string[] LH_OverTimeHours { get; set; }

        //法定内残業時間
        public string StatutoryOverTimeHours { get; set; }

        //早出残業時間
        public string[] PrepaidOvertimeHours { get; set; }

        //予備項目21
        public string PreliminaryItem21 { get; set; }

        //予備項目22
        public string PreliminaryItem22 { get; set; }

        //予備項目23
        public string PreliminaryItem23 { get; set; }

        //予備項目24
        public string PreliminaryItem24 { get; set; }

        //予備項目25
        public string PreliminaryItem25 { get; set; }

        //予備項目26
        public string PreliminaryItem26 { get; set; }

        //予備項目27
        public string PreliminaryItem27 { get; set; }

        //予備項目28
        public string PreliminaryItem28 { get; set; }

        //予備項目29
        public string PreliminaryItem29 { get; set; }

        //予備項目30
        public string PreliminaryItem30 { get; set; }

        /// <summary>
        /// Constructor class AttendanceSummaryInfo
        /// </summary>
        public AttendanceSummaryCSVInfo()
        {

            UserCD = string.Empty;
            WorkingDays = string.Empty;
            AttendanceDays = string.Empty;
            DaysWithoutWork = new string[2];
            DaysWithoutWork[0] = string.Empty;
            DaysWithoutWork[1] = string.Empty;
            DaysLeft = new string[2];
            DaysLeft[0] = string.Empty;
            DaysLeft[1] = "-1";
            SpecialHolidayNumber = new string[2];
            SpecialHolidayNumber[0] = string.Empty;
            SpecialHolidayNumber[1] = "-1";
            SubstituteHoliday = string.Empty;
            DayOff = new string[2];
            DayOff[0] = string.Empty;
            DayOff[1] = "-1";
            LateEarlyDays = string.Empty;
            AttendanceTimeHours = string.Empty;
            LateTimeHours = string.Empty;
            WeekdayNormalOvertimeHours = new string[2];
            WeekdayNormalOvertimeHours[0] = string.Empty;
            WeekdayNormalOvertimeHours[1] = "-1";
            OvertimeLate = new string[2];
            OvertimeLate[0] = string.Empty;
            OvertimeLate[1] = "-1";
            OvertimeOnholiday = string.Empty;
            OvertimeMidNight = new string[2];
            OvertimeMidNight[0] = string.Empty;
            OvertimeMidNight[1] = "-1";
            StatutoryHoliday = string.Empty;
            LH_Hours = string.Empty;
            LH_OverTimeHours = new string[2];
            LH_OverTimeHours[0] = string.Empty;
            LH_OverTimeHours[1] = "-1";
            StatutoryOverTimeHours = string.Empty;
            PrepaidOvertimeHours = new string[2];
            PrepaidOvertimeHours[0] = string.Empty;
            PrepaidOvertimeHours[1] = "-1";
            PreliminaryItem21 = string.Empty;
            PreliminaryItem22 = string.Empty;
            PreliminaryItem23 = string.Empty;
            PreliminaryItem24 = string.Empty;
            PreliminaryItem25 = string.Empty;
            PreliminaryItem26 = string.Empty;
            PreliminaryItem27 = string.Empty;
            PreliminaryItem28 = string.Empty;
            PreliminaryItem29 = string.Empty;
            PreliminaryItem30 = string.Empty;
        }
    }
}
