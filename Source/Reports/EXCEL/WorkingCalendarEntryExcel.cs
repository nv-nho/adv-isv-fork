using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.SS.UserModel;
using OMS.Models;
using OMS.Models.Type;
using NPOI.SS.Util;
using NPOI.HSSF.UserModel;

namespace OMS.Reports.EXCEL
{
    public class WorkingCalendarEntryExcel : BaseExcel
    {
        #region Variable
        /// <summary>
        /// Model Quotation Header Search
        /// </summary>
        public WorkingCalendarEntryExcelModal modelInput;
        #endregion
        
        /// <summary>
        /// Output Excel
        /// </summary>
        /// <returns></returns>
        public IWorkbook OutputExcel()
        {
            IWorkbook wb = null;

            //Create Sheet
            wb = this.CreateWorkbook("労働日数時間集計表");

            // Get Sheet
            ISheet sheet = wb.GetSheet("労働日数時間等集計表");

            //Fill data
            this.FillData(wb, sheet, modelInput);
            
            return wb;
        }

        
        /// <summary>
        /// Fill data on excel
        /// </summary>
        /// <param name="sheet">ISheet</param>
        /// <param name="lstData">Data list  of billing</param>
        private void FillData(IWorkbook wb, ISheet sheet, WorkingCalendarEntryExcelModal workingCalendarEntryExcelModal)
        {
            int rowStart = 1;
        
            IRow rowTemp = sheet.GetRow(rowStart+1);
            //Create cell InitDate
            ICell cellInitDate = rowTemp.GetCell(7);
            cellInitDate.SetCellValue(workingCalendarEntryExcelModal.InitialDate);

            rowTemp = sheet.GetRow(rowStart + 2);
            //Create cell FromInitDate
            ICell cellFromInitDate = rowTemp.GetCell(3);
            cellFromInitDate.SetCellValue(workingCalendarEntryExcelModal.FromInitDate);

            //Create cell ToInitDate
            ICell cellToInitDate = rowTemp.GetCell(7);
            cellToInitDate.SetCellValue(workingCalendarEntryExcelModal.ToInitDate);

            rowTemp = sheet.GetRow(rowStart + 3);
            //Create cell CountDayInMonthTotal
            ICell cellCountDayInMonthTotal = rowTemp.GetCell(7);
            cellCountDayInMonthTotal.SetCellValue(workingCalendarEntryExcelModal.CountDayInMonthTotal);

            rowTemp = sheet.GetRow(rowStart + 4);
            //Create cell DailyWorkingHours
            ICell cellDailyWorkingHours = rowTemp.GetCell(7);
            cellDailyWorkingHours.SetCellValue(workingCalendarEntryExcelModal.DailyWorkingHours);

            rowTemp = sheet.GetRow(rowStart + 5);
            //Create cell WorkingDateTotal
            ICell cellWorkingDateTotal = rowTemp.GetCell(7);
            cellWorkingDateTotal.SetCellValue(workingCalendarEntryExcelModal.WorkingDateTotal);

            rowTemp = sheet.GetRow(rowStart + 6);
            //Create cell WorkingDateTotal
            ICell cellHolidayInMonthTotal = rowTemp.GetCell(7);
            cellHolidayInMonthTotal.SetCellValue(workingCalendarEntryExcelModal.HolidayInMonthTotal);

            rowTemp = sheet.GetRow(rowStart + 7);
            //Create cell WorkingTimeInMonthTotal
            ICell cellWorkingTimeInMonthTotal = rowTemp.GetCell(7);
            cellWorkingTimeInMonthTotal.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeInMonthTotal);

            rowTemp = sheet.GetRow(rowStart + 8);
            //Create cell WorkingTimeWeeklyAverageTotal
            ICell cellWorkingTimeWeeklyAverageTotal = rowTemp.GetCell(7);
            cellWorkingTimeWeeklyAverageTotal.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeWeeklyAverageTotal);

            rowTemp = sheet.GetRow(rowStart + 12);
            //Create cell MonthIndex1
            ICell cellMonthIndex1 = rowTemp.GetCell(2);
            cellMonthIndex1.SetCellValue(workingCalendarEntryExcelModal.MonthIndex1);
            //Create cell CountDayInMonth1
            ICell cellCountDayInMonth1 = rowTemp.GetCell(3);
            cellCountDayInMonth1.SetCellValue(workingCalendarEntryExcelModal.CountDayInMonth1);
            //Create cell HolidayInMonth1
            ICell cellHolidayInMonth1 = rowTemp.GetCell(4);
            cellHolidayInMonth1.SetCellValue(workingCalendarEntryExcelModal.HolidayInMonth1);
            //Create cell WorkingDate1
            ICell cellWorkingDate1 = rowTemp.GetCell(5);
            cellWorkingDate1.SetCellValue(workingCalendarEntryExcelModal.WorkingDate1);
            //Create cell WorkingTimeInMonth1
            ICell cellWorkingTimeInMonth1 = rowTemp.GetCell(6);
            cellWorkingTimeInMonth1.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeInMonth1);
            //Create cell WorkingTimeWeeklyAverage1
            ICell cellWorkingTimeWeeklyAverage1 = rowTemp.GetCell(7);
            cellWorkingTimeWeeklyAverage1.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeWeeklyAverage1);

            rowTemp = sheet.GetRow(rowStart + 13);
            //Create cell MonthIndex2
            ICell cellMonthIndex2 = rowTemp.GetCell(2);
            cellMonthIndex2.SetCellValue(workingCalendarEntryExcelModal.MonthIndex2);
            //Create cell CountDayInMonth1
            ICell cellCountDayInMonth2 = rowTemp.GetCell(3);
            cellCountDayInMonth2.SetCellValue(workingCalendarEntryExcelModal.CountDayInMonth2);
            //Create cell HolidayInMonth1
            ICell cellHolidayInMonth2 = rowTemp.GetCell(4);
            cellHolidayInMonth2.SetCellValue(workingCalendarEntryExcelModal.HolidayInMonth2);
            //Create cell WorkingDate1
            ICell cellWorkingDate2 = rowTemp.GetCell(5);
            cellWorkingDate2.SetCellValue(workingCalendarEntryExcelModal.WorkingDate2);
            //Create cell WorkingTimeInMonth1
            ICell cellWorkingTimeInMonth2 = rowTemp.GetCell(6);
            cellWorkingTimeInMonth2.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeInMonth2);
            //Create cell WorkingTimeWeeklyAverage1
            ICell cellWorkingTimeWeeklyAverage2 = rowTemp.GetCell(7);
            cellWorkingTimeWeeklyAverage2.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeWeeklyAverage2);

            rowTemp = sheet.GetRow(rowStart + 14);
            //Create cell MonthIndex3
            ICell cellMonthIndex3 = rowTemp.GetCell(2);
            cellMonthIndex3.SetCellValue(workingCalendarEntryExcelModal.MonthIndex3);
            //Create cell CountDayInMonth1
            ICell cellCountDayInMonth3 = rowTemp.GetCell(3);
            cellCountDayInMonth3.SetCellValue(workingCalendarEntryExcelModal.CountDayInMonth3);
            //Create cell HolidayInMonth1
            ICell cellHolidayInMonth3 = rowTemp.GetCell(4);
            cellHolidayInMonth3.SetCellValue(workingCalendarEntryExcelModal.HolidayInMonth3);
            //Create cell WorkingDate1
            ICell cellWorkingDate3 = rowTemp.GetCell(5);
            cellWorkingDate3.SetCellValue(workingCalendarEntryExcelModal.WorkingDate3);
            //Create cell WorkingTimeInMonth1
            ICell cellWorkingTimeInMonth3 = rowTemp.GetCell(6);
            cellWorkingTimeInMonth3.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeInMonth3);
            //Create cell WorkingTimeWeeklyAverage1
            ICell cellWorkingTimeWeeklyAverage3 = rowTemp.GetCell(7);
            cellWorkingTimeWeeklyAverage3.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeWeeklyAverage3);


            rowTemp = sheet.GetRow(rowStart + 15);
            //Create cell MonthIndex4
            ICell cellMonthIndex4 = rowTemp.GetCell(2);
            cellMonthIndex4.SetCellValue(workingCalendarEntryExcelModal.MonthIndex4);
            //Create cell CountDayInMonth4
            ICell cellCountDayInMonth4 = rowTemp.GetCell(3);
            cellCountDayInMonth4.SetCellValue(workingCalendarEntryExcelModal.CountDayInMonth4);
            //Create cell HolidayInMonth3
            ICell cellHolidayInMonth4 = rowTemp.GetCell(4);
            cellHolidayInMonth4.SetCellValue(workingCalendarEntryExcelModal.HolidayInMonth4);
            //Create cell WorkingDate4
            ICell cellWorkingDate4 = rowTemp.GetCell(5);
            cellWorkingDate4.SetCellValue(workingCalendarEntryExcelModal.WorkingDate4);
            //Create cell WorkingTimeInMonth4
            ICell cellWorkingTimeInMonth4 = rowTemp.GetCell(6);
            cellWorkingTimeInMonth4.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeInMonth4);
            //Create cell WorkingTimeWeeklyAverage1
            ICell cellWorkingTimeWeeklyAverage4 = rowTemp.GetCell(7);
            cellWorkingTimeWeeklyAverage4.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeWeeklyAverage4);

            rowTemp = sheet.GetRow(rowStart + 16);
            //Create cell MonthIndex5
            ICell cellMonthIndex5 = rowTemp.GetCell(2);
            cellMonthIndex5.SetCellValue(workingCalendarEntryExcelModal.MonthIndex5);
            //Create cell CountDayInMonth1
            ICell cellCountDayInMonth5 = rowTemp.GetCell(3);
            cellCountDayInMonth5.SetCellValue(workingCalendarEntryExcelModal.CountDayInMonth5);
            //Create cell HolidayInMonth1
            ICell cellHolidayInMonth5 = rowTemp.GetCell(4);
            cellHolidayInMonth5.SetCellValue(workingCalendarEntryExcelModal.HolidayInMonth5);
            //Create cell WorkingDate1
            ICell cellWorkingDate5 = rowTemp.GetCell(5);
            cellWorkingDate5.SetCellValue(workingCalendarEntryExcelModal.WorkingDate5);
            //Create cell WorkingTimeInMonth5
            ICell cellWorkingTimeInMonth5 = rowTemp.GetCell(6);
            cellWorkingTimeInMonth5.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeInMonth5);
            //Create cell WorkingTimeWeeklyAverage1
            ICell cellWorkingTimeWeeklyAverage5 = rowTemp.GetCell(7);
            cellWorkingTimeWeeklyAverage5.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeWeeklyAverage5);


            rowTemp = sheet.GetRow(rowStart + 17);
            //Create cell MonthIndex6
            ICell cellMonthIndex6 = rowTemp.GetCell(2);
            cellMonthIndex6.SetCellValue(workingCalendarEntryExcelModal.MonthIndex6);
            //Create cell CountDayInMonth1
            ICell cellCountDayInMonth6 = rowTemp.GetCell(3);
            cellCountDayInMonth6.SetCellValue(workingCalendarEntryExcelModal.CountDayInMonth6);
            //Create cell HolidayInMonth1
            ICell cellHolidayInMonth6 = rowTemp.GetCell(4);
            cellHolidayInMonth6.SetCellValue(workingCalendarEntryExcelModal.HolidayInMonth6);
            //Create cell WorkingDate1
            ICell cellWorkingDate6 = rowTemp.GetCell(5);
            cellWorkingDate6.SetCellValue(workingCalendarEntryExcelModal.WorkingDate6);
            //Create cell WorkingTimeInMonth6
            ICell cellWorkingTimeInMonth6 = rowTemp.GetCell(6);
            cellWorkingTimeInMonth6.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeInMonth6);
            //Create cell WorkingTimeWeeklyAverage1
            ICell cellWorkingTimeWeeklyAverage6 = rowTemp.GetCell(7);
            cellWorkingTimeWeeklyAverage6.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeWeeklyAverage6);


            rowTemp = sheet.GetRow(rowStart + 18);
            //Create cell MonthIndex7
            ICell cellMonthIndex7 = rowTemp.GetCell(2);
            cellMonthIndex7.SetCellValue(workingCalendarEntryExcelModal.MonthIndex7);
            //Create cell CountDayInMonth7
            ICell cellCountDayInMonth7 = rowTemp.GetCell(3);
            cellCountDayInMonth7.SetCellValue(workingCalendarEntryExcelModal.CountDayInMonth7);
            //Create cell HolidayInMonth7
            ICell cellHolidayInMonth7 = rowTemp.GetCell(4);
            cellHolidayInMonth7.SetCellValue(workingCalendarEntryExcelModal.HolidayInMonth7);
            //Create cell WorkingDate7
            ICell cellWorkingDate7 = rowTemp.GetCell(5);
            cellWorkingDate7.SetCellValue(workingCalendarEntryExcelModal.WorkingDate7);
            //Create cell WorkingTimeInMonth7
            ICell cellWorkingTimeInMonth7 = rowTemp.GetCell(6);
            cellWorkingTimeInMonth7.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeInMonth7);
            //Create cell WorkingTimeWeeklyAverage1
            ICell cellWorkingTimeWeeklyAverage7 = rowTemp.GetCell(7);
            cellWorkingTimeWeeklyAverage7.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeWeeklyAverage7);

            rowTemp = sheet.GetRow(rowStart + 19);
            //Create cell MonthIndex8
            ICell cellMonthIndex8 = rowTemp.GetCell(2);
            cellMonthIndex8.SetCellValue(workingCalendarEntryExcelModal.MonthIndex8);
            //Create cell CountDayInMonth8
            ICell cellCountDayInMonth8 = rowTemp.GetCell(3);
            cellCountDayInMonth8.SetCellValue(workingCalendarEntryExcelModal.CountDayInMonth8);
            //Create cell HolidayInMonth8
            ICell cellHolidayInMonth8 = rowTemp.GetCell(4);
            cellHolidayInMonth8.SetCellValue(workingCalendarEntryExcelModal.HolidayInMonth8);
            //Create cell WorkingDate8
            ICell cellWorkingDate8 = rowTemp.GetCell(5);
            cellWorkingDate8.SetCellValue(workingCalendarEntryExcelModal.WorkingDate8);
            //Create cell WorkingTimeInMonth8
            ICell cellWorkingTimeInMonth8 = rowTemp.GetCell(6);
            cellWorkingTimeInMonth8.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeInMonth8);
            //Create cell WorkingTimeWeeklyAverage1
            ICell cellWorkingTimeWeeklyAverage8 = rowTemp.GetCell(7);
            cellWorkingTimeWeeklyAverage8.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeWeeklyAverage8);

            rowTemp = sheet.GetRow(rowStart + 20);
            //Create cell MonthIndex9
            ICell cellMonthIndex9 = rowTemp.GetCell(2);
            cellMonthIndex9.SetCellValue(workingCalendarEntryExcelModal.MonthIndex9);
            //Create cell CountDayInMonth9
            ICell cellCountDayInMonth9 = rowTemp.GetCell(3);
            cellCountDayInMonth9.SetCellValue(workingCalendarEntryExcelModal.CountDayInMonth9);
            //Create cell HolidayInMonth9
            ICell cellHolidayInMonth9 = rowTemp.GetCell(4);
            cellHolidayInMonth9.SetCellValue(workingCalendarEntryExcelModal.HolidayInMonth9);
            //Create cell WorkingDate9
            ICell cellWorkingDate9 = rowTemp.GetCell(5);
            cellWorkingDate9.SetCellValue(workingCalendarEntryExcelModal.WorkingDate9);
            //Create cell WorkingTimeInMonth9
            ICell cellWorkingTimeInMonth9 = rowTemp.GetCell(6);
            cellWorkingTimeInMonth9.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeInMonth9);
            //Create cell WorkingTimeWeeklyAverage1
            ICell cellWorkingTimeWeeklyAverage9 = rowTemp.GetCell(7);
            cellWorkingTimeWeeklyAverage9.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeWeeklyAverage9);

            rowTemp = sheet.GetRow(rowStart + 21);
            //Create cell MonthIndex10
            ICell cellMonthIndex10 = rowTemp.GetCell(2);
            cellMonthIndex10.SetCellValue(workingCalendarEntryExcelModal.MonthIndex10);
            //Create cell CountDayInMonth10
            ICell cellCountDayInMonth10 = rowTemp.GetCell(3);
            cellCountDayInMonth10.SetCellValue(workingCalendarEntryExcelModal.CountDayInMonth10);
            //Create cell HolidayInMonth10
            ICell cellHolidayInMonth10 = rowTemp.GetCell(4);
            cellHolidayInMonth10.SetCellValue(workingCalendarEntryExcelModal.HolidayInMonth10);
            //Create cell WorkingDate10
            ICell cellWorkingDate10 = rowTemp.GetCell(5);
            cellWorkingDate10.SetCellValue(workingCalendarEntryExcelModal.WorkingDate10);
            //Create cell WorkingTimeInMonth10
            ICell cellWorkingTimeInMonth10 = rowTemp.GetCell(6);
            cellWorkingTimeInMonth10.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeInMonth10);
            //Create cell WorkingTimeWeeklyAverage1
            ICell cellWorkingTimeWeeklyAverage10 = rowTemp.GetCell(7);
            cellWorkingTimeWeeklyAverage10.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeWeeklyAverage10);

            rowTemp = sheet.GetRow(rowStart + 22);
            //Create cell MonthIndex11
            ICell cellMonthIndex11 = rowTemp.GetCell(2);
            cellMonthIndex11.SetCellValue(workingCalendarEntryExcelModal.MonthIndex11);
            //Create cell CountDayInMonth11
            ICell cellCountDayInMonth11 = rowTemp.GetCell(3);
            cellCountDayInMonth11.SetCellValue(workingCalendarEntryExcelModal.CountDayInMonth11);
            //Create cell HolidayInMonth11
            ICell cellHolidayInMonth11 = rowTemp.GetCell(4);
            cellHolidayInMonth11.SetCellValue(workingCalendarEntryExcelModal.HolidayInMonth11);
            //Create cell WorkingDate11
            ICell cellWorkingDate11 = rowTemp.GetCell(5);
            cellWorkingDate11.SetCellValue(workingCalendarEntryExcelModal.WorkingDate11);
            //Create cell WorkingTimeInMonth11
            ICell cellWorkingTimeInMonth11 = rowTemp.GetCell(6);
            cellWorkingTimeInMonth11.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeInMonth11);
            //Create cell WorkingTimeWeeklyAverage1
            ICell cellWorkingTimeWeeklyAverage11 = rowTemp.GetCell(7);
            cellWorkingTimeWeeklyAverage11.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeWeeklyAverage11);

            rowTemp = sheet.GetRow(rowStart + 23);
            //Create cell MonthIndex12
            ICell cellMonthIndex12 = rowTemp.GetCell(2);
            cellMonthIndex12.SetCellValue(workingCalendarEntryExcelModal.MonthIndex12);
            //Create cell CountDayInMonth12
            ICell cellCountDayInMonth12 = rowTemp.GetCell(3);
            cellCountDayInMonth12.SetCellValue(workingCalendarEntryExcelModal.CountDayInMonth12);
            //Create cell HolidayInMonth12
            ICell cellHolidayInMonth12 = rowTemp.GetCell(4);
            cellHolidayInMonth12.SetCellValue(workingCalendarEntryExcelModal.HolidayInMonth12);
            //Create cell WorkingDate12
            ICell cellWorkingDate12 = rowTemp.GetCell(5);
            cellWorkingDate12.SetCellValue(workingCalendarEntryExcelModal.WorkingDate12);
            //Create cell WorkingTimeInMonth12
            ICell cellWorkingTimeInMonth12 = rowTemp.GetCell(6);
            cellWorkingTimeInMonth12.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeInMonth12);
            //Create cell WorkingTimeWeeklyAverage1
            ICell cellWorkingTimeWeeklyAverage12 = rowTemp.GetCell(7);
            cellWorkingTimeWeeklyAverage12.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeWeeklyAverage12);
            
            rowTemp = sheet.GetRow(rowStart + 26);
            //Create cell LongestWorkingHoursDay
            ICell cellLongestWorkingHoursDay = rowTemp.GetCell(7);
            cellLongestWorkingHoursDay.SetCellValue(workingCalendarEntryExcelModal.LongestWorkingHoursDay);
            
            rowTemp = sheet.GetRow(rowStart + 27);
            //Create cell LongestWorkingHourseWeek
            ICell cellLongestWorkingHourseWeek = rowTemp.GetCell(7);
            cellLongestWorkingHourseWeek.SetCellValue(workingCalendarEntryExcelModal.LongestWorkingHourseWeek);

            rowTemp = sheet.GetRow(rowStart + 28);
            //Create cell WorkingDayWeeklarge48HourseContinuity
            ICell cellWorkingDayWeeklarge48HourseContinuity = rowTemp.GetCell(7);
            cellWorkingDayWeeklarge48HourseContinuity.SetCellValue(workingCalendarEntryExcelModal.WorkingDayWeeklarge48HourseContinuity);

            rowTemp = sheet.GetRow(rowStart + 29);
            //Create cell WorkingDayWeeklarge48Hourse
            ICell cellWorkingDayWeeklarge48Hourse = rowTemp.GetCell(7);
            cellWorkingDayWeeklarge48Hourse.SetCellValue(workingCalendarEntryExcelModal.WorkingDayWeeklarge48Hourse);

            rowTemp = sheet.GetRow(rowStart + 30);
            //Create cell WorkingDayLarge
            ICell cellWorkingDayLarge = rowTemp.GetCell(7);
            cellWorkingDayLarge.SetCellValue(workingCalendarEntryExcelModal.WorkingDayLarge);
        }
    }
}
