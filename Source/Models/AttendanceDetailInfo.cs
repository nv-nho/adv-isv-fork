using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Collections;

namespace OMS.Models
{
    /// <summary>
    /// Class AttendanceDetailInfo
    /// ISV-THAN
    /// </summary>
    [Serializable]
    public class AttendanceDetailInfo
    {
        //ID
        public int ID { get; set; }

        //ID
        public int UID { get; set; }
        
        //日付
        public DateTime Date { get; set; }
        public string StringDate { get; set; }

        //勤務体系
        public string WorkingSystemName { get; set; }

        //出勤時刻
        public string EntryTime { get; set; }

        //退出時刻
        public string ExitTime { get; set; }

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

        //早出残業
        public string PrematureOvertimeWork { get; set; }

        //通常残業
        public string RegularOvertime { get; set; }

        //深夜残業
        public string LateNightOvertime { get; set; }

        //所定休日深夜
        public string PredeterminedHolidayLateNight { get; set; }

        //法定休日深夜
        public string LegalHolidayLateNight { get; set; }

        //休暇コード
        public int VacationFlag { get; set; }
        public int VacationFullCD { get; set; }
        public int VacationMorningCD { get; set; }
        public int VacationAfternoonCD { get; set; }

        //TextColorClass
        public string TextColorClass { get; set; }
        //TextColorBackgroundClass
        public string TextColorBackgroundClass { get; set; }
        //TextColorCurrentDate
        public string TextColorCurrentDate { get; set; }

        //RowNumber
        public int RowNumber { get; set; }

        //excel
        public string ProjectInfo { get; set; }
        public List<string> VacaitionColor{ get; set; }
        public string TotalOverTimeForDay { get; set; }
        public string TotalWorkingHoursForDay { get; set; }
        public string HolidayName { get; set; }
        public string Memo { get; set; }
        public string ExchangeStatus { get; set; }

        /*Isv-Tinh 2020/04/13 ADD Start*/
        //interval time
        public bool OutIntervalTime { get; set; }
        public string BackColorInterValTime { get; set; }
        /*Isv-Tinh 2020/04/13 ADD End*/

        //************* Approval ***************
        public string ApprovalNote { get; set; }
        public int ApprovalUID { get; set; }
        public int ApprovalStatus { get; set; }
        public string StatusName { get; set; }
        public string ApprovalDate { get; set; }
        //**************************************

        public int Late_Early_Hours_Flg { get; set; }
        public int SH_Hours_Flg { get; set; }
        public int LH_Hours_Flg { get; set; }
        public int SH_OverTimeHours_Flg { get; set; }


        /// <summary>
        /// Constructor class AttendanceDetailInfo
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public AttendanceDetailInfo(DbDataReader dr)
        {
            //ID
            this.ID = int.Parse(dr["ID"].ToString());
            
            //UID
            this.UID = int.Parse(dr["UID"].ToString());

            //日付
            this.Date =  (DateTime)dr["Date"];
            
            // string date
            this.StringDate = ConvertDate(this.Date);

            //Working System Name
            this.WorkingSystemName = dr["WorkingSystemName"].ToString();
           

            //勤務体系
            this.WorkingSystemName = dr["WorkingSystemName"].ToString();

            //出勤時刻
            this.EntryTime =dr["EntryTime"].ToString()!=string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["EntryTime"].ToString()), true):string.Empty;

            //退出時刻
            this.ExitTime = dr["ExitTime"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["ExitTime"].ToString()), true) : string.Empty;

            //出勤
            this.WorkingHours = dr["WorkingHours"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["WorkingHours"].ToString()), true) : string.Empty;

            //遅刻
            if (dr["LateHours"].ToString() != string.Empty)
            {
                this.LateHours = Utilities.CommonUtil.IntToTime(int.Parse(dr["LateHours"].ToString()), true);
                this.Late_Early_Hours_Flg = 1;
            }
            else
            {
                this.LateHours = string.Empty;
            }

            //早退
            if (dr["EarlyHours"].ToString() != string.Empty){
                this.EarlyHours = Utilities.CommonUtil.IntToTime(int.Parse(dr["EarlyHours"].ToString()), true);
                this.Late_Early_Hours_Flg = 1;
            }else{
                this.EarlyHours = string.Empty;
            }

            //所定休日
            if (dr["SH_Hours"].ToString() != string.Empty)
            {
                this.SH_Hours = Utilities.CommonUtil.IntToTime(int.Parse(dr["SH_Hours"].ToString()), true);
                this.SH_Hours_Flg = 1;
            }
            else
            {
                this.SH_Hours = string.Empty;
            }

            //法定休日
            if (dr["LH_Hours"].ToString() != string.Empty)
            {
                this.LH_Hours = Utilities.CommonUtil.IntToTime(int.Parse(dr["LH_Hours"].ToString()), true);
                this.LH_Hours_Flg = 1;
            }
            else
            {
                this.LH_Hours = string.Empty;
            }

            //早出残業
            if (dr["OverTimeHours1"].ToString() != string.Empty)
            {
                this.PrematureOvertimeWork = Utilities.CommonUtil.IntToTime(int.Parse(dr["OverTimeHours1"].ToString()), true);
                this.SH_OverTimeHours_Flg = 1;
            }
            else if (dr["SH_OverTimeHours1"].ToString() != string.Empty)
            {
                this.PrematureOvertimeWork = Utilities.CommonUtil.IntToTime(int.Parse(dr["SH_OverTimeHours1"].ToString()), true);
                this.SH_OverTimeHours_Flg = 1;
            }
            else if (dr["LH_OverTimeHours1"].ToString() != string.Empty)
            {
                this.PrematureOvertimeWork = Utilities.CommonUtil.IntToTime(int.Parse(dr["LH_OverTimeHours1"].ToString()), true);
                this.SH_OverTimeHours_Flg = 1;
            }

            //通常残業
            if (dr["OverTimeHours2"].ToString() != string.Empty)
            {
                this.RegularOvertime = Utilities.CommonUtil.IntToTime(int.Parse(dr["OverTimeHours2"].ToString()), true);
                this.SH_OverTimeHours_Flg = 1;
            }
            else if (dr["SH_OverTimeHours2"].ToString() != string.Empty)
            {
                this.RegularOvertime = Utilities.CommonUtil.IntToTime(int.Parse(dr["SH_OverTimeHours2"].ToString()), true);
                this.SH_OverTimeHours_Flg = 1;
            }
            else if (dr["LH_OverTimeHours2"].ToString() != string.Empty)
            {
                this.RegularOvertime = Utilities.CommonUtil.IntToTime(int.Parse(dr["LH_OverTimeHours2"].ToString()), true);
                this.SH_OverTimeHours_Flg = 1;
            }
            //深夜残業
            if (dr["OverTimeHours3"].ToString() != string.Empty)
            {
                this.LateNightOvertime = Utilities.CommonUtil.IntToTime(int.Parse(dr["OverTimeHours3"].ToString()), true);
                this.SH_OverTimeHours_Flg = 1;
            }
            else if (dr["SH_OverTimeHours3"].ToString() != string.Empty)
            {
                this.LateNightOvertime = Utilities.CommonUtil.IntToTime(int.Parse(dr["SH_OverTimeHours3"].ToString()), true);
                this.SH_OverTimeHours_Flg = 1;
            }
            else if (dr["LH_OverTimeHours3"].ToString() != string.Empty)
            {
                this.LateNightOvertime = Utilities.CommonUtil.IntToTime(int.Parse(dr["LH_OverTimeHours3"].ToString()), true);
                this.SH_OverTimeHours_Flg = 1;
            }
            //所定休日深夜
            if (dr["OverTimeHours4"].ToString() != string.Empty)
            {
                this.PredeterminedHolidayLateNight = Utilities.CommonUtil.IntToTime(int.Parse(dr["OverTimeHours4"].ToString()), true);
                this.SH_OverTimeHours_Flg = 1;
            }
            else if (dr["SH_OverTimeHours4"].ToString() != string.Empty)
            {
                this.PredeterminedHolidayLateNight = Utilities.CommonUtil.IntToTime(int.Parse(dr["SH_OverTimeHours4"].ToString()), true);
                this.SH_OverTimeHours_Flg = 1;
            }
            else if (dr["LH_OverTimeHours4"].ToString() != string.Empty)
            {
                this.PredeterminedHolidayLateNight = Utilities.CommonUtil.IntToTime(int.Parse(dr["LH_OverTimeHours4"].ToString()), true);
                this.SH_OverTimeHours_Flg = 1;
            }
            //法定休日深夜
            if (dr["OverTimeHours5"].ToString() != string.Empty)
            {
                this.LegalHolidayLateNight = Utilities.CommonUtil.IntToTime(int.Parse(dr["OverTimeHours5"].ToString()), true);
                this.SH_OverTimeHours_Flg = 1;
            }
            else if (dr["SH_OverTimeHours5"].ToString() != string.Empty)
            {
                this.LegalHolidayLateNight = Utilities.CommonUtil.IntToTime(int.Parse(dr["SH_OverTimeHours5"].ToString()), true);
                this.SH_OverTimeHours_Flg = 1;
            }
            else if (dr["LH_OverTimeHours5"].ToString() != string.Empty)
            {
                this.LegalHolidayLateNight = Utilities.CommonUtil.IntToTime(int.Parse(dr["LH_OverTimeHours5"].ToString()), true);
                this.SH_OverTimeHours_Flg = 1;
            }

            this.VacationFlag = dr["VacationFlag"].ToString() != string.Empty ? int.Parse(dr["VacationFlag"].ToString()) : -1;
            this.VacationFullCD = dr["VacationFullCD"].ToString() != string.Empty ? int.Parse(dr["VacationFullCD"].ToString()) : -1;
            this.VacationMorningCD = dr["VacationMorningCD"].ToString() != string.Empty ? int.Parse(dr["VacationMorningCD"].ToString()) : -1;
            this.VacationAfternoonCD = dr["VacationAfternoonCD"].ToString() != string.Empty ? int.Parse(dr["VacationAfternoonCD"].ToString()) : -1;

            if (this.Date.ToString("yyyyMMdd") == DateTime.Now.ToString("yyyyMMdd"))
            {
                this.TextColorCurrentDate  = "alert alert-warning";
            }
            else if (this.WorkingSystemName == "有休予定日")
            {
                this.TextColorCurrentDate = "paid-leave";
            }
            else
            {
                this.TextColorCurrentDate = "";
            }

            // set color Class 
            if (int.Parse(dr["WorkingType"].ToString()) == 0)
            {
                this.TextColorClass = "text-color-normalday";
            }
            else if (int.Parse(dr["WorkingType"].ToString()) == 1)
            {
                this.TextColorClass = "text-color-saturday";
            }
            else if (int.Parse(dr["WorkingType"].ToString()) == 2)
            {
                this.TextColorClass = "text-color-holiday";
            }

            this.Memo = dr["Memo"].ToString();
            this.ExchangeStatus = dr["ExchangeStatus"].ToString();
            this.ApprovalStatus = dr["ApprovalStatus"].ToString() != string.Empty ? int.Parse(dr["ApprovalStatus"].ToString()) : -1;
        }

        /// <summary>
        /// Constructor class AttendanceDetailInfo
        /// </summary>
        public AttendanceDetailInfo()
        {
            this.EntryTime = string.Empty;
            this.ExitTime = string.Empty;
            this.WorkingHours = string.Empty;
            this.LateHours = string.Empty;
            this.EarlyHours = string.Empty;
            this.SH_Hours = string.Empty;
            this.LH_Hours = string.Empty;
            this.PrematureOvertimeWork = string.Empty;
            this.RegularOvertime = string.Empty;
            this.LateNightOvertime = string.Empty;
            this.PredeterminedHolidayLateNight = string.Empty;
            this.LegalHolidayLateNight = string.Empty;
        }

        /// <summary>
        /// Constructor class AttendanceDetailInfo
        /// </summary>
        public AttendanceDetailInfo(DateTime date)
        {
            this.Date = date;
            this.StringDate = ConvertDate(this.Date);
            this.WorkingSystemName = "&nbsp;";
            this.VacationFullCD = -1;
            this.VacationMorningCD = -1;
            this.VacationAfternoonCD = -1;
            this.TextColorClass = "text-color-normalday";
            this.TextColorCurrentDate = "";
        }

        /// <summary>
        /// ConvertDate
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private string ConvertDate(DateTime date)
        {
            string strDate = date.ToString();
            string month = strDate.Substring(5, 2);
            string day = strDate.Substring(8, 2);
            string dayOfWeek;

            switch ((int)date.DayOfWeek)
            {
                case 0:
                    dayOfWeek = "日";
                    break;
                case 1:
                    dayOfWeek = "月";
                    break;
                case 2:
                    dayOfWeek = "火";
                    break;
                case 3:
                    dayOfWeek = "水";
                    break;
                case 4:
                    dayOfWeek = "木";
                    break;
                case 5:
                    dayOfWeek = "金";
                    break;
                case 6:
                    dayOfWeek = "土";
                    break;
                default:
                    dayOfWeek = string.Empty;
                    break;
            }

            strDate = date.ToString("MM月dd日") + "(" + dayOfWeek + ")";
            return strDate;
        }
    }

     /// <summary>
    /// Class WorkDInfoEXcel
    /// ISV-Giao
    /// </summary>
    [Serializable]
    public class WorkDInfoEXcel
    {
        public string workTime { get; set; }
        public int projectId { get; set; }
        public string projectNm { get; set; }

        #region Contructor

        public WorkDInfoEXcel(DbDataReader dr)
        {
            this.workTime = dr["workTime"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["workTime"].ToString()), true) : string.Empty;
            this.projectId = (int)dr["projectId"];
            this.projectNm = (string)dr["projectNm"];         
        }
        #endregion
         
    }
}
