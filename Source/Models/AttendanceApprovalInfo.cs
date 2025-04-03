using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Collections;

namespace OMS.Models
{
    /// <summary>
    /// Class AttendanceApprovalInfo
    /// ISV-Giao
    /// </summary>
    [Serializable]
    public class AttendanceApprovalInfo
    {
        //row index
        public int RowNumber { get; set; }

        //ID
        public int ID { get; set; }

        //uID
        public int UID { get; set; }

        //CallendarID
        public int CallendarID { get; set; }

        //社員CD
        public string UserCD { get; set; }

        //社員名1
        public string UserName1 { get; set; }

        //部署ID
        public int DepartmentID { get; set; }

        //部署名
        public string DepartmentName { get; set; }

        //対象月
        public DateTime InitialDate { get; set; }

        //月
        public int Month { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

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
        public string StatusFlag { get; set; }

        public bool CheckFlag { get; set; }

        /// <summary>
        /// Constructor class AttendanceApprovalInfo
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public AttendanceApprovalInfo(DbDataReader dr)
        {
            //RowNumber
            this.RowNumber = int.Parse(dr["RowNumber"].ToString());

            //UID
            this.UID = int.Parse(dr["UID"].ToString());
            this.CallendarID = int.Parse(dr["CallendarID"].ToString());
            this.UserCD = dr["UserCD"].ToString();
            this.UserName1 = dr["UserName1"].ToString();
            this.DepartmentID = int.Parse(dr["DepartmentID"].ToString());
            this.DepartmentName = dr["DepartmentName"].ToString();

            //日付
            this.InitialDate = (DateTime)dr["InitialDate"];

            //Working System Name
            this.Month = int.Parse(dr["Month"].ToString());

            //日付
            this.StartDate = (DateTime)dr["StartDate"];

            //日付
            this.EndDate = (DateTime)dr["EndDate"];

            //出勤
            this.WorkingHours = dr["WorkingHours"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["WorkingHours"].ToString()), true) : string.Empty;
            //遅刻
            this.LateHours = dr["LateHours"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["LateHours"].ToString()), true) : string.Empty;
            //早退
            this.EarlyHours = dr["EarlyHours"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["EarlyHours"].ToString()), true) : string.Empty;
            //所定休日
            this.SH_Hours = dr["SH_Hours"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["SH_Hours"].ToString()), true) : string.Empty;
            //法定休日
            this.LH_Hours = dr["LH_Hours"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["LH_Hours"].ToString()), true) : string.Empty;

            this.NumWorkingDays = (int)dr["NumWorkingDays"];
            this.NumLateDays = (int)dr["NumLateDays"];
            this.NumEarlyDays = (int)dr["NumEarlyDays"];
            this.NumSH_Days = (int)dr["NumSH_Days"];
            this.NumLH_Days = (int)dr["NumLH_Days"];


            //出勤
            this.OverTimeHours1 = dr["OverTimeHours1"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["OverTimeHours1"].ToString()), true) : "0";
            //遅刻
            this.OverTimeHours2 = dr["OverTimeHours2"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["OverTimeHours2"].ToString()), true) : "0";
            //早退
            this.OverTimeHours3 = dr["OverTimeHours3"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["OverTimeHours3"].ToString()), true) : "0";
            //所定休日
            this.OverTimeHours4 = dr["OverTimeHours4"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["OverTimeHours4"].ToString()), true) : "0";
            //法定休日
            this.OverTimeHours5 = dr["OverTimeHours5"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["OverTimeHours5"].ToString()), true) : "0";

            //出勤
            this.SH_OverTimeHours1 = dr["SH_OverTimeHours1"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["SH_OverTimeHours1"].ToString()), true) : "0";
            //遅刻
            this.SH_OverTimeHours2 = dr["SH_OverTimeHours2"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["SH_OverTimeHours2"].ToString()), true) : "0";
            //早退
            this.SH_OverTimeHours3 = dr["SH_OverTimeHours3"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["SH_OverTimeHours3"].ToString()), true) : "0";
            //所定休日
            this.SH_OverTimeHours4 = dr["SH_OverTimeHours4"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["SH_OverTimeHours4"].ToString()), true) : "0";
            //法定休日
            this.SH_OverTimeHours5 = dr["SH_OverTimeHours5"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["SH_OverTimeHours5"].ToString()), true) : "0";

            //出勤
            this.LH_OverTimeHours1 = dr["LH_OverTimeHours1"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["LH_OverTimeHours1"].ToString()), true) : "0";
            //遅刻
            this.LH_OverTimeHours2 = dr["LH_OverTimeHours2"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["LH_OverTimeHours2"].ToString()), true) : "0";
            //早退
            this.LH_OverTimeHours3 = dr["LH_OverTimeHours3"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["LH_OverTimeHours3"].ToString()), true) : "0";
            //所定休日
            this.LH_OverTimeHours4 = dr["LH_OverTimeHours4"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["LH_OverTimeHours4"].ToString()), true) : "0";
            //法定休日
            this.LH_OverTimeHours5 = dr["LH_OverTimeHours5"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["LH_OverTimeHours5"].ToString()), true) : "0";

            this.TotalOverTimeHours1 = Utilities.CommonUtil.TimeToInt(this.OverTimeHours1) + Utilities.CommonUtil.TimeToInt(this.SH_OverTimeHours1) + Utilities.CommonUtil.TimeToInt(this.LH_OverTimeHours1);
            this.TotalOverTimeHours2 = Utilities.CommonUtil.TimeToInt(this.OverTimeHours2) + Utilities.CommonUtil.TimeToInt(this.SH_OverTimeHours2) + Utilities.CommonUtil.TimeToInt(this.LH_OverTimeHours2);
            this.TotalOverTimeHours3 = Utilities.CommonUtil.TimeToInt(this.OverTimeHours3) + Utilities.CommonUtil.TimeToInt(this.SH_OverTimeHours3) + Utilities.CommonUtil.TimeToInt(this.LH_OverTimeHours3);
            this.TotalOverTimeHours4 = Utilities.CommonUtil.TimeToInt(this.OverTimeHours4) + Utilities.CommonUtil.TimeToInt(this.SH_OverTimeHours4) + Utilities.CommonUtil.TimeToInt(this.LH_OverTimeHours4);
            this.TotalOverTimeHours5 = Utilities.CommonUtil.TimeToInt(this.OverTimeHours5) + Utilities.CommonUtil.TimeToInt(this.SH_OverTimeHours5) + Utilities.CommonUtil.TimeToInt(this.LH_OverTimeHours5);

            this.TotalOverTimeHours = dr["TotalOverTimeHours"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["TotalOverTimeHours"].ToString()), true) : string.Empty;
            //法定休日
            this.TotalWorkingHours = dr["TotalWorkingHours"].ToString() != string.Empty ? Utilities.CommonUtil.IntToTime(int.Parse(dr["TotalWorkingHours"].ToString()), true) : string.Empty;

            this.StatusFlag = dr["StatusFlag"].ToString();

        }

        /// <summary>
        /// Constructor class AttendanceDetailInfo
        /// </summary>
        public AttendanceApprovalInfo()
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
            StatusFlag = string.Empty;

            CheckFlag = false;
        }

        /// <summary>
        /// Constructor class AttendanceApprovalInfo
        /// </summary>
        public AttendanceApprovalInfo(DateTime date)
        {
        }

    }

    /// <summary>
    /// Class VacationDateInFoByAttendanceApproval
    /// </summary>
    [Serializable]
    public class VacationDateInFoByAttendanceApproval
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
        /// Contructor of M_Config_D
        /// </summary>
        public VacationDateInFoByAttendanceApproval()
        {
        }

        /// <summary>
        /// Contructor of M_Setting_D
        /// </summary>
        /// <param name="dr">Database data reader</param>
        public VacationDateInFoByAttendanceApproval(DbDataReader dr)
        {
            this.Value1 = (int)dr["Value1"];
            this.Value3 = (string)dr["Value3"];
            this.VacationDate = (string)dr["VacationDate"].ToString();
        }

        #endregion
    }

    /// <summary>
    /// Class VacationDateInFoByAttendanceApproval
    /// </summary>
    [Serializable]
    public class AttendanceApprovalIDAndUpdateDate
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// UpdateDate
        /// </summary>
        public DateTime UpdateDate { get; set; }

        #region Contructor

        /// <summary>
        /// Contructor of AttendanceApprovalIDAndUpdateDate
        /// </summary>
        public AttendanceApprovalIDAndUpdateDate()
        {
        }

        /// <summary>
        /// Contructor of AttendanceApprovalIDAndUpdateDate
        /// </summary>
        /// <param name="dr">Database data reader</param>
        public AttendanceApprovalIDAndUpdateDate(DbDataReader dr)
        {
            this.Id = (int)dr["Id"];
            this.UpdateDate = (DateTime)dr["UpdateDate"];
        }

        #endregion
    }
}
