using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Collections;

namespace OMS.Models.Type
{
    class WorkingCalendarInfo
    {
    }
    /// <summary>
    /// Class WorkingCalendarHeaderSearch
    /// </summary>
    [Serializable]
    public class WorkingCalendarHeaderSearch
    {
        /// <summary>
        /// Get or set ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Get or set CalendarCD
        /// </summary>
        public string CalendarCD { get; set; }

        /// <summary>
        /// Get or set CalendarName
        /// </summary>
        public string CalendarName { get; set; }

        /// <summary>
        /// Get or set InitialDate
        /// </summary>
        public DateTime? InitialDate { get; set; }

        /// <summary>
        /// Get or set AnnualWorkingDays
        /// </summary>
        public int AnnualWorkingDays { get; set; }

        /// <summary>
        /// Get or set AnnualWorkingHours
        /// </summary>
        public int AnnualWorkingHours { get; set; }

    }
    /// <summary>
    /// Class SalesHeaderResult
    /// </summary>
    [Serializable]
    public class WorkingCalendarResult
    {
        #region Constant
        public readonly DateTime DEFAULT_DATE_TIME = new DateTime(1900, 1, 1);
        #endregion

        /// <summary>
        /// Get or set RowNumber
        /// </summary>
        public long RowNumber { get; set; }

        /// <summary>
        /// Get or set ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Get or set CalendarCD
        /// </summary>
        public string CalendarCD { get; set; }

        /// <summary>
        /// Get or set CalendarName
        /// </summary>
        public string CalendarName { get; set; }

        /// <summary>
        /// Get or set InitialDate
        /// </summary>
        public DateTime InitialDate { get; set; }

        /// <summary>
        /// Get or set AnnualWorkingDays
        /// </summary>
        public int AnnualWorkingDays { get; set; }

        /// <summary>
        /// Get or set AnnualWorkingHours
        /// </summary>
        public int AnnualWorkingHours { get; set; }

        /// <summary>
        /// Get or set AgreementFlag1
        /// </summary>
        public short AgreementFlag1 { get; set; }

        /// <summary>
        /// Get or set AgreementFlag2
        /// </summary>
        public short AgreementFlag2 { get; set; }

        /// <summary>
        /// Get or set AgreementFlag3
        /// </summary>
        public short AgreementFlag3 { get; set; }

        /// <summary>
        /// Get or set AgreementFlag4
        /// </summary>
        public short AgreementFlag4 { get; set; }

        /// <summary>
        /// Get or set AgreementFlag5
        /// </summary>
        public short AgreementFlag5 { get; set; }

        /// <summary>
        /// Get or set AgreementFlag6
        /// </summary>
        public short AgreementFlag6 { get; set; }

        /// <summary>
        /// Get or set AgreementFlag7
        /// </summary>
        public short AgreementFlag7 { get; set; }

        /// <summary>
        /// Get or set AgreementFlag8
        /// </summary>
        public short AgreementFlag8 { get; set; }

        /// <summary>
        /// Get or set AgreementFlag9
        /// </summary>
        public short AgreementFlag9 { get; set; }

        /// <summary>
        /// Get or set AgreementFlag10
        /// </summary>
        public short AgreementFlag10 { get; set; }

        /// <summary>
        /// Get or set AgreementFlag11
        /// </summary>
        public short AgreementFlag11 { get; set; }

        /// <summary>
        /// Get or set AgreementFlag12
        /// </summary>
        public short AgreementFlag12 { get; set; }

        /// <summary>
        /// Get or set StatusFlag
        /// </summary>
        public short StatusFlag { get; set; }

        /// <summary>
        /// Get or set CreateDate
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Get or set CreateUID
        /// </summary>
        public int CreateUID { get; set; }

        /// <summary>
        /// Get or set UpdateDate
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// Get or set UpdateUID
        /// </summary>
        public int UpdateUID { get; set; }

        /// <summary>
        /// Contructor
        /// </summary>
        /// 
        public WorkingCalendarResult()
        {
            this.RowNumber = 0;
            this.ID = 0;
            this.CalendarCD = string.Empty;
            this.CalendarName = string.Empty;
            this.InitialDate = DateTime.MinValue;
            this.AnnualWorkingDays = 0;
            this.AnnualWorkingHours = 0;
            this.AgreementFlag1 = 0;
            this.AgreementFlag2 = 0;
            this.AgreementFlag3 = 0;
            this.AgreementFlag4 = 0;
            this.AgreementFlag5 = 0;
            this.AgreementFlag6 = 0;
            this.AgreementFlag7 = 0;
            this.AgreementFlag8 = 0;
            this.AgreementFlag9 = 0;
            this.AgreementFlag10 = 0;
            this.AgreementFlag11 = 0;
            this.AgreementFlag12 = 0;
            this.StatusFlag = 0;
            this.CreateDate = DateTime.MinValue;
            this.CreateUID = 0;
            this.UpdateDate = DateTime.MinValue;
            this.UpdateUID = 0;
            
        }
         /// <summary>
        /// Constructor with param
        /// </summary>
        /// <param name="dr"></param>
        public WorkingCalendarResult(DbDataReader dr)
        {

            this.RowNumber = (long)dr["RowNumber"];
            this.ID = int.Parse(dr["ID"].ToString());
            this.CalendarCD = (string)dr["CalendarCD"];
            this.CalendarName = (string)dr["CalendarName"];
            this.InitialDate = (DateTime)dr["InitialDate"];
            this.AnnualWorkingDays = dr["AnnualWorkingDays"] != DBNull.Value ? (int)dr["AnnualWorkingDays"] : 0;
            this.AnnualWorkingHours = dr["AnnualWorkingHours"] != DBNull.Value ? (int)dr["AnnualWorkingHours"] : 0;

        }
    }

     /// <summary>
    /// Class ExcelFormatBValue
    /// </summary>
    [Serializable]
    public class ExcelFormatBValue
    {
        public string day { get; set; }
        public DateTime currentDate { get; set; }
        public string holiday { get; set; }
        public string workingSystemCD { get; set; }
        public string workingSystemCDType { get; set; }
        public string checkBoxIvent { get; set; }
    }
    /// <summary>
    /// Class WorkingCalendarExcel
    /// </summary>
    [Serializable]
    public class WorkingCalendarEntryExcelModal
    {
        /// <summary>
        /// Get or set arrDay: month 1-month 12
        /// </summary>
        public ArrayList arrDay1 { get; set; }
        public ArrayList arrDay2 { get; set; }
        public ArrayList arrDay3 { get; set; }
        public ArrayList arrDay4 { get; set; }
        public ArrayList arrDay5 { get; set; }
        public ArrayList arrDay6 { get; set; }
        public ArrayList arrDay7 { get; set; }
        public ArrayList arrDay8 { get; set; }
        public ArrayList arrDay9 { get; set; }
        public ArrayList arrDay10 { get; set; }
        public ArrayList arrDay11 { get; set; }
        public ArrayList arrDay12 { get; set; }

        /// <summary>
        /// Get or set CalendarCD
        /// </summary>
        public string CalendarCD { get; set; }

        /// <summary>
        /// Get or set CalendarName
        /// </summary>
        public string CalendarName { get; set; }

        /// <summary>
        /// Get or set Tilte
        /// </summary>
        public string Tilte { get; set; }

        /// <summary>
        /// Get or set InitialDate
        /// </summary>
        public string InitialDate { get; set; }

        /// <summary>
        /// Get or set InitialDate FomatB
        /// </summary>
        public string InitialDateFomatB { get; set; }

        /// <summary>
        /// Get or set FromInitDate
        /// </summary>
        public string FromInitDate { get; set; }

        /// <summary>
        /// Get or set FromInitDate
        /// </summary>
        public string ToInitDate { get; set; }

        /// <summary>
        /// Get or set CountDayInMonthTotal
        /// </summary>
        public string CountDayInMonthTotal { get; set; }

        /// <summary>
        /// Get or set DailyWorkingHours
        /// </summary>
        public string DailyWorkingHours { get; set; }

        /// <summary>
        /// Get or set WorkingDateTotal
        /// </summary>
        public string WorkingDateTotal { get; set; }

        /// <summary>
        /// Get or set HolidayInMonthTotal
        /// </summary>
        public string HolidayInMonthTotal { get; set; }

        /// <summary>
        /// Get or set WorkingTimeInMonthTotal
        /// </summary>
        public string WorkingTimeInMonthTotal { get; set; }
        
        /// <summary>
        /// Get or set WorkingTimeWeeklyAverageTotal
        /// </summary>
        public string WorkingTimeWeeklyAverageTotal { get; set; }
        
        /// <summary>
        /// Get or set MonthIndex1
        /// </summary>
        public string MonthIndex1 { get; set; }

        /// <summary>
        /// Get or set MonthIndex2
        /// </summary>
        public string MonthIndex2 { get; set; }

        /// <summary>
        /// Get or set MonthIndex3
        /// </summary>
        public string MonthIndex3 { get; set; }

        /// <summary>
        /// Get or set MonthIndex4
        /// </summary>
        public string MonthIndex4 { get; set; }

        /// <summary>
        /// Get or set MonthIndex5
        /// </summary>
        public string MonthIndex5 { get; set; }

        /// <summary>
        /// Get or set MonthIndex6
        /// </summary>
        public string MonthIndex6 { get; set; }

        /// <summary>
        /// Get or set MonthIndex7
        /// </summary>
        public string MonthIndex7 { get; set; }

        /// <summary>
        /// Get or set MonthIndex8
        /// </summary>
        public string MonthIndex8 { get; set; }

        /// <summary>
        /// Get or set MonthIndex9
        /// </summary>
        public string MonthIndex9 { get; set; }

        /// <summary>
        /// Get or set MonthIndex10
        /// </summary>
        public string MonthIndex10 { get; set; }

        /// <summary>
        /// Get or set MonthIndex11
        /// </summary>
        public string MonthIndex11 { get; set; }

        /// <summary>
        /// Get or set MonthIndex12
        /// </summary>
        public string MonthIndex12 { get; set; }

        /// <summary>
        /// Get or set CountDayInMonth1
        /// </summary>
        public int CountDayInMonth1 { get; set; }

        /// <summary>
        /// Get or set CountDayInMonth2
        /// </summary>
        public int CountDayInMonth2 { get; set; }

        /// <summary>
        /// Get or set CountDayInMonth3
        /// </summary>
        public int CountDayInMonth3 { get; set; }

        /// <summary>
        /// Get or set CountDayInMonth4
        /// </summary>
        public int CountDayInMonth4 { get; set; }

        /// <summary>
        /// Get or set CountDayInMonth5
        /// </summary>
        public int CountDayInMonth5 { get; set; }

        /// <summary>
        /// Get or set CountDayInMonth6
        /// </summary>
        public int CountDayInMonth6 { get; set; }

        /// <summary>
        /// Get or set CountDayInMonth7
        /// </summary>
        public int CountDayInMonth7 { get; set; }

        /// <summary>
        /// Get or set CountDayInMonth8
        /// </summary>
        public int CountDayInMonth8 { get; set; }

        /// <summary>
        /// Get or set CountDayInMonth9
        /// </summary>
        public int CountDayInMonth9 { get; set; }

        /// <summary>
        /// Get or set CountDayInMonth10
        /// </summary>
        public int CountDayInMonth10 { get; set; }

        /// <summary>
        /// Get or set CountDayInMonth11
        /// </summary>
        public int CountDayInMonth11 { get; set; }

        /// <summary>
        /// Get or set CountDayInMonth12
        /// </summary>
        public int CountDayInMonth12 { get; set; }

        /// <summary>
        /// Get or set HolidayInMonth1
        /// </summary>
        public int HolidayInMonth1 { get; set; }

        /// <summary>
        /// Get or set HolidayInMonth2
        /// </summary>
        public int HolidayInMonth2 { get; set; }

        /// <summary>
        /// Get or set HolidayInMonth3
        /// </summary>
        public int HolidayInMonth3 { get; set; }

        /// <summary>
        /// Get or set HolidayInMonth4
        /// </summary>
        public int HolidayInMonth4 { get; set; }

        /// <summary>
        /// Get or set HolidayInMonth5
        /// </summary>
        public int HolidayInMonth5 { get; set; }

        /// <summary>
        /// Get or set HolidayInMonth6
        /// </summary>
        public int HolidayInMonth6 { get; set; }

        /// <summary>
        /// Get or set HolidayInMonth7
        /// </summary>
        public int HolidayInMonth7 { get; set; }

        /// <summary>
        /// Get or set HolidayInMonth8
        /// </summary>
        public int HolidayInMonth8 { get; set; }

        /// <summary>
        /// Get or set HolidayInMonth9
        /// </summary>
        public int HolidayInMonth9 { get; set; }

        /// <summary>
        /// Get or set HolidayInMonth10
        /// </summary>
        public int HolidayInMonth10 { get; set; }

        /// <summary>
        /// Get or set HolidayInMonth11
        /// </summary>
        public int HolidayInMonth11 { get; set; }

        /// <summary>
        /// Get or set HolidayInMonth12
        /// </summary>
        public int HolidayInMonth12 { get; set; }

        /// <summary>
        /// Get or set WorkingDate1
        /// </summary>
        public int WorkingDate1 { get; set; }

        /// <summary>
        /// Get or set WorkingDate2
        /// </summary>
        public int WorkingDate2 { get; set; }

        /// <summary>
        /// Get or set WorkingDate3
        /// </summary>
        public int WorkingDate3 { get; set; }

        /// <summary>
        /// Get or set WorkingDate4
        /// </summary>
        public int WorkingDate4 { get; set; }

        /// <summary>
        /// Get or set WorkingDate5
        /// </summary>
        public int WorkingDate5 { get; set; }

        /// <summary>
        /// Get or set WorkingDate6
        /// </summary>
        public int WorkingDate6 { get; set; }

        /// <summary>
        /// Get or set WorkingDate7
        /// </summary>
        public int WorkingDate7 { get; set; }

        /// <summary>
        /// Get or set WorkingDate8
        /// </summary>
        public int WorkingDate8 { get; set; }

        /// <summary>
        /// Get or set WorkingDate9
        /// </summary>
        public int WorkingDate9 { get; set; }

        /// <summary>
        /// Get or set WorkingDate10
        /// </summary>
        public int WorkingDate10 { get; set; }

        /// <summary>
        /// Get or set WorkingDate11
        /// </summary>
        public int WorkingDate11 { get; set; }

        /// <summary>
        /// Get or set WorkingDate12
        /// </summary>
        public int WorkingDate12 { get; set; }

        /// <summary>
        /// Get or set WorkingTimeInMonth1
        /// </summary>
        public string WorkingTimeInMonth1 { get; set; }

        /// <summary>
        /// Get or set WorkingTimeInMonth2
        /// </summary>
        public string WorkingTimeInMonth2 { get; set; }

        /// <summary>
        /// Get or set WorkingTimeInMonth3
        /// </summary>
        public string WorkingTimeInMonth3 { get; set; }

        /// <summary>
        /// Get or set WorkingTimeInMonth4
        /// </summary>
        public string WorkingTimeInMonth4 { get; set; }

        /// <summary>
        /// Get or set WorkingTimeInMonth5
        /// </summary>
        public string WorkingTimeInMonth5 { get; set; }

        /// <summary>
        /// Get or set WorkingTimeInMonth6
        /// </summary>
        public string WorkingTimeInMonth6 { get; set; }

        /// <summary>
        /// Get or set WorkingTimeInMonth7
        /// </summary>
        public string WorkingTimeInMonth7 { get; set; }

        /// <summary>
        /// Get or set WorkingTimeInMonth8
        /// </summary>
        public string WorkingTimeInMonth8 { get; set; }

        /// <summary>
        /// Get or set WorkingTimeInMonth9
        /// </summary>
        public string WorkingTimeInMonth9 { get; set; }

        /// <summary>
        /// Get or set WorkingTimeInMonth10
        /// </summary>
        public string WorkingTimeInMonth10 { get; set; }

        /// <summary>
        /// Get or set WorkingTimeInMonth11
        /// </summary>
        public string WorkingTimeInMonth11 { get; set; }

        /// <summary>
        /// Get or set WorkingTimeInMonth12
        /// </summary>
        public string WorkingTimeInMonth12 { get; set; }
        
        /// <summary>
        /// Get or set WorkingTimeWeeklyAverage1
        /// </summary>
        public string WorkingTimeWeeklyAverage1 { get; set; }

        /// <summary>
        /// Get or set WorkingTimeWeeklyAverage2
        /// </summary>
        public string WorkingTimeWeeklyAverage2 { get; set; }

        /// <summary>
        /// Get or set WorkingTimeWeeklyAverage3
        /// </summary>
        public string WorkingTimeWeeklyAverage3 { get; set; }

        /// <summary>
        /// Get or set WorkingTimeWeeklyAverage4
        /// </summary>
        public string WorkingTimeWeeklyAverage4 { get; set; }

        /// <summary>
        /// Get or set WorkingTimeWeeklyAverage5
        /// </summary>
        public string WorkingTimeWeeklyAverage5 { get; set; }

        /// <summary>
        /// Get or set WorkingTimeWeeklyAverage6
        /// </summary>
        public string WorkingTimeWeeklyAverage6 { get; set; }

        /// <summary>
        /// Get or set WorkingTimeWeeklyAverage7
        /// </summary>
        public string WorkingTimeWeeklyAverage7 { get; set; }

        /// <summary>
        /// Get or set WorkingTimeWeeklyAverage8
        /// </summary>
        public string WorkingTimeWeeklyAverage8 { get; set; }

        /// <summary>
        /// Get or set WorkingTimeWeeklyAverage9
        /// </summary>
        public string WorkingTimeWeeklyAverage9 { get; set; }

        /// <summary>
        /// Get or set WorkingTimeWeeklyAverage10
        /// </summary>
        public string WorkingTimeWeeklyAverage10 { get; set; }

        /// <summary>
        /// Get or set WorkingTimeWeeklyAverage11
        /// </summary>
        public string WorkingTimeWeeklyAverage11 { get; set; }

        /// <summary>
        /// Get or set WorkingTimeWeeklyAverage12
        /// </summary>
        public string WorkingTimeWeeklyAverage12 { get; set; }
        
        /// <summary>
        /// Get or set LongestWorkingHours
        /// </summary>
        public string LongestWorkingHoursDay { get; set; }
        
        /// <summary>
        /// Get or set LongestWorkingHourseWeek
        /// </summary>
        public string LongestWorkingHourseWeek { get; set; }

        /// <summary>
        /// Get or set WorkingDayWeeklarge48HourseContinuity
        /// </summary>
        public string WorkingDayWeeklarge48HourseContinuity { get; set; }
        
        /// <summary>
        /// Get or set WorkingDayWeeklarge48Hourse
        /// </summary>
        public string WorkingDayWeeklarge48Hourse { get; set; }
        
        /// <summary>
        /// Get or set WorkingDayLargeEachWeek
        /// </summary>
        public string WorkingDayLarge { get; set; }

        
    }
}
