using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.SS.UserModel;
using OMS.Models;
using OMS.Models.Type;
using NPOI.HSSF.UserModel;
using System.Collections;
using System.Drawing;
using OMS.Utilities;

namespace OMS.Reports.EXCEL
{
    public class WorkingCalendarEntryExcelFormatB : BaseExcel
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
            wb = this.CreateWorkbook("年間勤務カレンダー");

            // Get Sheet
            ISheet sheet = wb.GetSheet("年間勤務カレンダー");

            //Fill data
            this.FillData(wb, sheet, modelInput);
            
            return wb;
        }

        /// <summary>
        /// set color cell anh shape for month 
        /// </summary>
        /// <param name="arrColor"></param>
        /// <param name="valueEcxeclB"></param>
        /// <param name="wb"></param>
        /// <param name="sheet"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private HSSFCellStyle setClorloCellExcel(ArrayList arrColor, ExcelFormatBValue valueEcxeclB,IWorkbook wb, ISheet sheet, int row, int col)
        {
            HSSFCellStyle hStyle = (HSSFCellStyle)arrColor[0];
            short indexColor = NPOI.HSSF.Util.HSSFColor.Black.Index;

            if (valueEcxeclB.day != "0")
            {
                if (valueEcxeclB.workingSystemCD == "4")
                {
                    hStyle = (HSSFCellStyle)arrColor[1];
                }
                else if (!string.IsNullOrEmpty(valueEcxeclB.workingSystemCDType))
                {
                    WorkingType workingType = (WorkingType)int.Parse(valueEcxeclB.workingSystemCDType);
                    switch (workingType)
                    {
                        case WorkingType.LegalHoliDay:
                            if (valueEcxeclB.holiday != "")
                            {
                                hStyle = (HSSFCellStyle)arrColor[2];
                            }
                            else
                            {
                                hStyle = (HSSFCellStyle)arrColor[4];
                            }
                            indexColor = NPOI.HSSF.Util.HSSFColor.Red.Index;
                            break;
                        case WorkingType.WorkHoliDay:
                            if (valueEcxeclB.holiday != "")
                            {
                                hStyle = (HSSFCellStyle)arrColor[2];
                                indexColor = NPOI.HSSF.Util.HSSFColor.Red.Index;
                            }
                            else
                            {
                                hStyle = (HSSFCellStyle)arrColor[3];
                                indexColor = NPOI.HSSF.Util.HSSFColor.Blue.Index;
                            }
                            break;
                        case WorkingType.WorkFullTime:
                            if (valueEcxeclB.holiday != "")
                            {
                                createShapeFoWorkDay(sheet, row, col, 255, 153, 153);
                            }
                            else if (valueEcxeclB.currentDate.DayOfWeek == DayOfWeek.Sunday)
                            {
                                createShapeFoWorkDay(sheet, row, col, 255, 204, 255);
                            }
                            else if (valueEcxeclB.currentDate.DayOfWeek == DayOfWeek.Saturday)
                            {
                                createShapeFoWorkDay(sheet, row, col, 189, 215, 238);
                            }
                            indexColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                            break;
                    }
                }

                if (valueEcxeclB.checkBoxIvent == "1")
                {
                    createShapeForCheckBoxIvent(sheet, row, col);
                }

                //set font cell
                hStyle.SetFont(setFontCellExcel(wb, indexColor));
  
            }
                      
            return hStyle;
        }

        /// <summary>
        /// create shape for check box
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private HSSFSimpleShape createShapeForCheckBoxIvent(ISheet sheet, int row, int col)
        {
            HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
            HSSFClientAnchor a = new HSSFClientAnchor(340,30, 700, 200, (short)col, row, (short)col, row);
            HSSFSimpleShape shape1 = patriarch.CreateSimpleShape(a);

            shape1.ShapeType = (HSSFSimpleShape.OBJECT_TYPE_OVAL);
            shape1.SetLineStyleColor(10, 10, 10);
            shape1.IsNoFill = true;
            shape1.LineWidth = 3;
            shape1.LineStyle = LineStyle.Solid;

            return shape1;
        }

        /// <summary>
        /// creted shape for word day
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        /// <returns></returns>
        private HSSFSimpleShape createShapeFoWorkDay(ISheet sheet, int row, int col,int red,int green,int blue)
        {
            HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
            HSSFClientAnchor a = new HSSFClientAnchor(10,10, 1000, 240, (short)col, row, (short)col, row);
            HSSFSimpleShape shape1 = patriarch.CreateSimpleShape(a);

            shape1.ShapeType = (HSSFSimpleShape.OBJECT_TYPE_COMMENT);
            shape1.SetLineStyleColor(red, green, blue);
            shape1.IsNoFill = true;
            shape1.LineWidth = 25000;
            shape1.LineStyle = LineStyle.Solid;

            return shape1;
        }

        /// <summary>
        /// custom color background HSSFCellStyle
        /// </summary>
        /// <param name="wb"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private HSSFPalette SetPaletteColor(IWorkbook wb, int r, int g, int b, short index)
        {
            HSSFPalette XlPalette = ((HSSFWorkbook)wb).GetCustomPalette();
            short colorIndex = index;
            XlPalette.SetColorAtIndex(colorIndex, (byte)r, (byte)g, (byte)b);
            return XlPalette;
        }

        /// <summary>
        /// setFontCellExcel 
        /// </summary>
        /// <param name="wb"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private IFont setFontCellExcel(IWorkbook wb, short indexColor )
        {
            IFont font = (IFont)wb.CreateFont();
            font.Color = indexColor;
            font.Boldweight = 11;
            font.FontHeightInPoints = 11;
            font.FontName = "メイリオ";
            return font;
        }

        /// <summary>
        /// printDayForMonth
        /// </summary>
        /// <param name="wb"></param>
        /// <param name="sheet"></param>
        /// <param name="rowStart"></param>
        /// <param name="rowIndex"></param>
        /// <param name="colIndex"></param>
        /// <param name="rowTemp"></param>
        /// <param name="arrDay"></param>
        /// <param name="arrColor"></param>
        private void printDayForMonth(IWorkbook wb,ISheet sheet, int rowStart,int rowIndex,int colIndex, IRow rowTemp, ArrayList arrDay, ArrayList arrColor)
        {
            rowTemp = sheet.GetRow(rowStart + rowIndex);
            int j = 0;
            int row = 0;
            for (int i = 0; i < 42; i++)
            {

                if (j > 6)
                {
                    row++;
                    rowTemp = sheet.GetRow(rowStart + rowIndex + row);
                    j = 0;
                }
                else
                {

                }

                ExcelFormatBValue valueEcxeclB = (ExcelFormatBValue)arrDay[i];

                rowTemp.GetCell(j + colIndex).SetCellValue(valueEcxeclB.day == "0" ? "" : valueEcxeclB.day);
                rowTemp.GetCell(j + colIndex).CellStyle = setClorloCellExcel(arrColor, valueEcxeclB, wb, sheet, rowStart + rowIndex + row, j + colIndex);

                j++;
            }
        }
    
        /// <summary>
        /// Fill data on excel
        /// </summary>
        /// <param name="sheet">ISheet</param>
        /// <param name="lstData">Data list  of billing</param>
        private void FillData(IWorkbook wb, ISheet sheet, WorkingCalendarEntryExcelModal workingCalendarEntryExcelModal)
        {
            int rowStart = 0;

            ArrayList arrColor = new ArrayList();

            HSSFCellStyle hStyleDefault = (HSSFCellStyle)wb.CreateCellStyle();
            hStyleDefault.FillPattern = FillPattern.SolidForeground;
            hStyleDefault.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.White.Index;
            hStyleDefault.Alignment = HorizontalAlignment.Center;

            HSSFCellStyle hStyleYellow = (HSSFCellStyle)wb.CreateCellStyle();
            hStyleYellow.FillPattern = FillPattern.SolidForeground;
            hStyleYellow.FillForegroundColor = SetPaletteColor(wb, 255, 230, 153, (short)45).FindColor(255, 230, 153).Indexed;
            hStyleYellow.Alignment = HorizontalAlignment.Center;

            HSSFCellStyle hStyleOrange = (HSSFCellStyle)wb.CreateCellStyle();
            hStyleOrange.FillPattern = FillPattern.SolidForeground;
            hStyleOrange.FillForegroundColor = SetPaletteColor(wb, 255, 153, 153,46).FindColor(255, 153, 153).Indexed;
            hStyleOrange.Alignment = HorizontalAlignment.Center;

            HSSFCellStyle hStyleGreen = (HSSFCellStyle)wb.CreateCellStyle();  
            hStyleGreen.FillPattern = FillPattern.SolidForeground;
            hStyleGreen.FillForegroundColor = SetPaletteColor(wb, 189, 215, 238,47).FindColor(189, 215, 238).Indexed;
            hStyleGreen.Alignment = HorizontalAlignment.Center;

            HSSFCellStyle hStylePink = (HSSFCellStyle)wb.CreateCellStyle();
            hStylePink.FillPattern = FillPattern.SolidForeground;
            hStylePink.FillForegroundColor = SetPaletteColor(wb, 255, 204, 255,48).FindColor(255, 204, 255).Indexed;
            hStylePink.Alignment = HorizontalAlignment.Center;
            
            arrColor.Add(hStyleDefault);
            arrColor.Add(hStyleYellow);
            arrColor.Add(hStyleOrange);
            arrColor.Add(hStyleGreen);
            arrColor.Add(hStylePink);

            //create cell value tilte
            IRow rowTemp = sheet.GetRow(rowStart+1);
            ICell cellTilte = rowTemp.GetCell(1);
            cellTilte.SetCellValue(workingCalendarEntryExcelModal.Tilte);

            //create cell value InitialDate
            rowTemp = sheet.GetRow(rowStart + 2);
            ICell cellInitialDate = rowTemp.GetCell(14);
            cellInitialDate.SetCellValue(workingCalendarEntryExcelModal.InitialDateFomatB);

            //create cell month index
            rowTemp = sheet.GetRow(rowStart + 6);
            ICell cellMonthIndex1 = rowTemp.GetCell(1);
            cellMonthIndex1.SetCellValue(workingCalendarEntryExcelModal.MonthIndex1);

            ICell cellMonthIndex2 = rowTemp.GetCell(9);
            cellMonthIndex2.SetCellValue(workingCalendarEntryExcelModal.MonthIndex2);

            rowTemp = sheet.GetRow(rowStart + 15);
            ICell cellMonthIndex3 = rowTemp.GetCell(1);
            cellMonthIndex3.SetCellValue(workingCalendarEntryExcelModal.MonthIndex3);

            ICell cellMonthIndex4 = rowTemp.GetCell(9);
            cellMonthIndex4.SetCellValue(workingCalendarEntryExcelModal.MonthIndex4);

            rowTemp = sheet.GetRow(rowStart + 24);
            ICell cellMonthIndex5 = rowTemp.GetCell(1);
            cellMonthIndex5.SetCellValue(workingCalendarEntryExcelModal.MonthIndex5);

            ICell cellMonthIndex6 = rowTemp.GetCell(9);
            cellMonthIndex6.SetCellValue(workingCalendarEntryExcelModal.MonthIndex6);

            rowTemp = sheet.GetRow(rowStart + 33);
            ICell cellMonthIndex7 = rowTemp.GetCell(1);
            cellMonthIndex7.SetCellValue(workingCalendarEntryExcelModal.MonthIndex7);

            ICell cellMonthIndex8 = rowTemp.GetCell(9);
            cellMonthIndex8.SetCellValue(workingCalendarEntryExcelModal.MonthIndex8);

            rowTemp = sheet.GetRow(rowStart + 42);
            ICell cellMonthIndex9 = rowTemp.GetCell(1);
            cellMonthIndex9.SetCellValue(workingCalendarEntryExcelModal.MonthIndex9);

            ICell cellMonthIndex10 = rowTemp.GetCell(9);
            cellMonthIndex10.SetCellValue(workingCalendarEntryExcelModal.MonthIndex10);

            rowTemp = sheet.GetRow(rowStart + 51);
            ICell cellMonthIndex11 = rowTemp.GetCell(1);
            cellMonthIndex11.SetCellValue(workingCalendarEntryExcelModal.MonthIndex11);

            ICell cellMonthIndex12 = rowTemp.GetCell(9);
            cellMonthIndex12.SetCellValue(workingCalendarEntryExcelModal.MonthIndex12);
          
            //created cell day month 1th
            printDayForMonth(wb, sheet, rowStart, 8, 1, rowTemp, workingCalendarEntryExcelModal.arrDay1, arrColor);
          
            //created cell day month 2th
            printDayForMonth(wb, sheet, rowStart, 8, 9, rowTemp, workingCalendarEntryExcelModal.arrDay2, arrColor);

            //created cell day month 3rd
            printDayForMonth(wb, sheet, rowStart, 17, 1, rowTemp, workingCalendarEntryExcelModal.arrDay3, arrColor);

            //created cell day month 4th
            printDayForMonth(wb, sheet, rowStart, 17, 9, rowTemp, workingCalendarEntryExcelModal.arrDay4, arrColor);

            //created cell day month 5th
            printDayForMonth(wb, sheet, rowStart, 26, 1, rowTemp, workingCalendarEntryExcelModal.arrDay5, arrColor);

            //created cell day month 6th
            printDayForMonth(wb, sheet, rowStart, 26, 9, rowTemp, workingCalendarEntryExcelModal.arrDay6, arrColor);

            //created cell day month 7th
            printDayForMonth(wb, sheet, rowStart, 35, 1, rowTemp, workingCalendarEntryExcelModal.arrDay7, arrColor);

            //created cell day month 8th
            printDayForMonth(wb, sheet, rowStart, 35, 9, rowTemp, workingCalendarEntryExcelModal.arrDay8, arrColor);

            //created cell day month 9th
            printDayForMonth(wb, sheet, rowStart, 44, 1, rowTemp, workingCalendarEntryExcelModal.arrDay9, arrColor);

            //created cell day month 10th
            printDayForMonth(wb, sheet, rowStart, 44, 9, rowTemp, workingCalendarEntryExcelModal.arrDay10, arrColor);

            //created cell day month 11th
            printDayForMonth(wb, sheet, rowStart, 53, 1, rowTemp, workingCalendarEntryExcelModal.arrDay11, arrColor);

            //created cell day month 12th
            printDayForMonth(wb, sheet, rowStart, 53, 9, rowTemp, workingCalendarEntryExcelModal.arrDay12, arrColor);

            rowTemp = sheet.GetRow(rowStart + 62);
            ICell cellHeaderMonthIndex1 = rowTemp.GetCell(2);
            cellHeaderMonthIndex1.SetCellValue(workingCalendarEntryExcelModal.MonthIndex1);
            ICell cellHeaderMonthIndex2 = rowTemp.GetCell(3);
            cellHeaderMonthIndex2.SetCellValue(workingCalendarEntryExcelModal.MonthIndex2);
            ICell cellHeaderMonthIndex3 = rowTemp.GetCell(4);
            cellHeaderMonthIndex3.SetCellValue(workingCalendarEntryExcelModal.MonthIndex3);
            ICell cellHeaderMonthIndex4 = rowTemp.GetCell(5);
            cellHeaderMonthIndex4.SetCellValue(workingCalendarEntryExcelModal.MonthIndex4);
            ICell cellHeaderMonthIndex5 = rowTemp.GetCell(6);
            cellHeaderMonthIndex5.SetCellValue(workingCalendarEntryExcelModal.MonthIndex5);
            ICell cellHeaderMonthIndex6 = rowTemp.GetCell(7);
            cellHeaderMonthIndex6.SetCellValue(workingCalendarEntryExcelModal.MonthIndex6);
            ICell cellHeaderMonthIndex7 = rowTemp.GetCell(9);
            cellHeaderMonthIndex7.SetCellValue(workingCalendarEntryExcelModal.MonthIndex7);
            ICell cellHeaderMonthIndex8 = rowTemp.GetCell(10);
            cellHeaderMonthIndex8.SetCellValue(workingCalendarEntryExcelModal.MonthIndex8);
            ICell cellHeaderMonthIndex9 = rowTemp.GetCell(11);
            cellHeaderMonthIndex9.SetCellValue(workingCalendarEntryExcelModal.MonthIndex9);
            ICell cellHeaderMonthIndex10 = rowTemp.GetCell(12);
            cellHeaderMonthIndex10.SetCellValue(workingCalendarEntryExcelModal.MonthIndex10);
            ICell cellHeaderMonthIndex11 = rowTemp.GetCell(13);
            cellHeaderMonthIndex11.SetCellValue(workingCalendarEntryExcelModal.MonthIndex11);
            ICell cellHeaderMonthIndex12 = rowTemp.GetCell(14);
            cellHeaderMonthIndex12.SetCellValue(workingCalendarEntryExcelModal.MonthIndex12);

            rowTemp = sheet.GetRow(rowStart + 63);
            ICell cellHeaderCountDayInMonth1 = rowTemp.GetCell(2);
            cellHeaderCountDayInMonth1.SetCellValue(workingCalendarEntryExcelModal.CountDayInMonth1);
            ICell cellHeaderCountDayInMonth2 = rowTemp.GetCell(3);
            cellHeaderCountDayInMonth2.SetCellValue(workingCalendarEntryExcelModal.CountDayInMonth2);
            ICell cellHeaderCountDayInMonth3 = rowTemp.GetCell(4);
            cellHeaderCountDayInMonth3.SetCellValue(workingCalendarEntryExcelModal.CountDayInMonth3);
            ICell cellHeaderCountDayInMonth4 = rowTemp.GetCell(5);
            cellHeaderCountDayInMonth4.SetCellValue(workingCalendarEntryExcelModal.CountDayInMonth4);
            ICell cellHeaderCountDayInMonth5 = rowTemp.GetCell(6);
            cellHeaderCountDayInMonth5.SetCellValue(workingCalendarEntryExcelModal.CountDayInMonth5);
            ICell cellHeaderCountDayInMonth6 = rowTemp.GetCell(7);
            cellHeaderCountDayInMonth6.SetCellValue(workingCalendarEntryExcelModal.CountDayInMonth6);
            ICell cellHeaderCountDayInMonth7 = rowTemp.GetCell(9);
            cellHeaderCountDayInMonth7.SetCellValue(workingCalendarEntryExcelModal.CountDayInMonth7);
            ICell cellHeaderCountDayInMonth8 = rowTemp.GetCell(10);
            cellHeaderCountDayInMonth8.SetCellValue(workingCalendarEntryExcelModal.CountDayInMonth8);
            ICell cellHeaderCountDayInMonth9 = rowTemp.GetCell(11);
            cellHeaderCountDayInMonth9.SetCellValue(workingCalendarEntryExcelModal.CountDayInMonth9);
            ICell cellHeaderCountDayInMonth10 = rowTemp.GetCell(12);
            cellHeaderCountDayInMonth10.SetCellValue(workingCalendarEntryExcelModal.CountDayInMonth10);
            ICell cellHeaderCountDayInMonth11 = rowTemp.GetCell(13);
            cellHeaderCountDayInMonth11.SetCellValue(workingCalendarEntryExcelModal.CountDayInMonth11);
            ICell cellHeaderCountDayInMonth12 = rowTemp.GetCell(14);
            cellHeaderCountDayInMonth12.SetCellValue(workingCalendarEntryExcelModal.CountDayInMonth12);
            ICell cellHeaderCountDayInMonthTotal = rowTemp.GetCell(15);
            cellHeaderCountDayInMonthTotal.SetCellValue(workingCalendarEntryExcelModal.CountDayInMonthTotal.ToString().Replace("日",""));

            rowTemp = sheet.GetRow(rowStart + 64);
            ICell cellHeaderHolidayInMonth1 = rowTemp.GetCell(2);
            cellHeaderHolidayInMonth1.SetCellValue(workingCalendarEntryExcelModal.HolidayInMonth1);
            ICell cellHeaderHolidayInMonth2 = rowTemp.GetCell(3);
            cellHeaderHolidayInMonth2.SetCellValue(workingCalendarEntryExcelModal.HolidayInMonth2);
            ICell cellHeaderHolidayInMonth3 = rowTemp.GetCell(4);
            cellHeaderHolidayInMonth3.SetCellValue(workingCalendarEntryExcelModal.HolidayInMonth3);
            ICell cellHeaderHolidayInMonth4 = rowTemp.GetCell(5);
            cellHeaderHolidayInMonth4.SetCellValue(workingCalendarEntryExcelModal.HolidayInMonth4);
            ICell cellHeaderHolidayInMonth5 = rowTemp.GetCell(6);
            cellHeaderHolidayInMonth5.SetCellValue(workingCalendarEntryExcelModal.HolidayInMonth5);
            ICell cellHeaderHolidayInMonth6 = rowTemp.GetCell(7);
            cellHeaderHolidayInMonth6.SetCellValue(workingCalendarEntryExcelModal.HolidayInMonth6);
            ICell cellHeaderHolidayInMonth7 = rowTemp.GetCell(9);
            cellHeaderHolidayInMonth7.SetCellValue(workingCalendarEntryExcelModal.HolidayInMonth7);
            ICell cellHeaderHolidayInMonth8 = rowTemp.GetCell(10);
            cellHeaderHolidayInMonth8.SetCellValue(workingCalendarEntryExcelModal.HolidayInMonth8);
            ICell cellHeaderHolidayInMonth9 = rowTemp.GetCell(11);
            cellHeaderHolidayInMonth9.SetCellValue(workingCalendarEntryExcelModal.HolidayInMonth9);
            ICell cellHeaderHolidayInMonth10 = rowTemp.GetCell(12);
            cellHeaderHolidayInMonth10.SetCellValue(workingCalendarEntryExcelModal.HolidayInMonth10);
            ICell cellHeaderHolidayInMonth11 = rowTemp.GetCell(13);
            cellHeaderHolidayInMonth11.SetCellValue(workingCalendarEntryExcelModal.HolidayInMonth11);
            ICell cellHeaderHolidayInMonth12 = rowTemp.GetCell(14);
            cellHeaderHolidayInMonth12.SetCellValue(workingCalendarEntryExcelModal.HolidayInMonth12);
            ICell cellHeaderHolidayInMonthTotal = rowTemp.GetCell(15);
            cellHeaderHolidayInMonthTotal.SetCellValue(workingCalendarEntryExcelModal.HolidayInMonthTotal.ToString().Replace("日", ""));

            rowTemp = sheet.GetRow(rowStart + 65);
            ICell cellHeaderWorkingDate1 = rowTemp.GetCell(2);
            cellHeaderWorkingDate1.SetCellValue(workingCalendarEntryExcelModal.WorkingDate1);
            ICell cellHeaderWorkingDate2 = rowTemp.GetCell(3);
            cellHeaderWorkingDate2.SetCellValue(workingCalendarEntryExcelModal.WorkingDate2);
            ICell cellHeaderWorkingDate3 = rowTemp.GetCell(4);
            cellHeaderWorkingDate3.SetCellValue(workingCalendarEntryExcelModal.WorkingDate3);
            ICell cellHeaderWorkingDate4 = rowTemp.GetCell(5);
            cellHeaderWorkingDate4.SetCellValue(workingCalendarEntryExcelModal.WorkingDate4);
            ICell cellHeaderWorkingDate5 = rowTemp.GetCell(6);
            cellHeaderWorkingDate5.SetCellValue(workingCalendarEntryExcelModal.WorkingDate5);
            ICell cellHeaderWorkingDate6 = rowTemp.GetCell(7);
            cellHeaderWorkingDate6.SetCellValue(workingCalendarEntryExcelModal.WorkingDate6);
            ICell cellHeaderWorkingDate7 = rowTemp.GetCell(9);
            cellHeaderWorkingDate7.SetCellValue(workingCalendarEntryExcelModal.WorkingDate7);
            ICell cellHeaderWorkingDate8 = rowTemp.GetCell(10);
            cellHeaderWorkingDate8.SetCellValue(workingCalendarEntryExcelModal.WorkingDate8);
            ICell cellHeaderWorkingDate9 = rowTemp.GetCell(11);
            cellHeaderWorkingDate9.SetCellValue(workingCalendarEntryExcelModal.WorkingDate9);
            ICell cellHeaderWorkingDate10 = rowTemp.GetCell(12);
            cellHeaderWorkingDate10.SetCellValue(workingCalendarEntryExcelModal.WorkingDate10);
            ICell cellHeaderWorkingDate11 = rowTemp.GetCell(13);
            cellHeaderWorkingDate11.SetCellValue(workingCalendarEntryExcelModal.WorkingDate11);
            ICell cellHeaderWorkingDate12 = rowTemp.GetCell(14);
            cellHeaderWorkingDate12.SetCellValue(workingCalendarEntryExcelModal.WorkingDate12);
            ICell cellHeaderWorkingDateTotal = rowTemp.GetCell(15);
            cellHeaderWorkingDateTotal.SetCellValue(workingCalendarEntryExcelModal.WorkingDateTotal.ToString().Replace("日", ""));

            rowTemp = sheet.GetRow(rowStart + 66);
            ICell cellHeaderWorkingTimeInMonth1 = rowTemp.GetCell(2);
            cellHeaderWorkingTimeInMonth1.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeInMonth1);
            ICell cellHeaderWorkingTimeInMonth2 = rowTemp.GetCell(3);
            cellHeaderWorkingTimeInMonth2.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeInMonth2);
            ICell cellHeaderWorkingTimeInMonth3 = rowTemp.GetCell(4);
            cellHeaderWorkingTimeInMonth3.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeInMonth3);
            ICell cellHeaderWorkingTimeInMonth4 = rowTemp.GetCell(5);
            cellHeaderWorkingTimeInMonth4.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeInMonth4);
            ICell cellHeaderWorkingTimeInMonth5 = rowTemp.GetCell(6);
            cellHeaderWorkingTimeInMonth5.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeInMonth5);
            ICell cellHeaderWorkingTimeInMonth6 = rowTemp.GetCell(7);
            cellHeaderWorkingTimeInMonth6.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeInMonth6);
            ICell cellHeaderWorkingTimeInMonth7 = rowTemp.GetCell(9);
            cellHeaderWorkingTimeInMonth7.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeInMonth7);
            ICell cellHeaderWorkingTimeInMonth8 = rowTemp.GetCell(10);
            cellHeaderWorkingTimeInMonth8.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeInMonth8);
            ICell cellHeaderWorkingTimeInMonth9 = rowTemp.GetCell(11);
            cellHeaderWorkingTimeInMonth9.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeInMonth9);
            ICell cellHeaderWorkingTimeInMonth10 = rowTemp.GetCell(12);
            cellHeaderWorkingTimeInMonth10.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeInMonth10);
            ICell cellHeaderWorkingTimeInMonth11 = rowTemp.GetCell(13);
            cellHeaderWorkingTimeInMonth11.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeInMonth11);
            ICell cellHeaderWorkingTimeInMonth12 = rowTemp.GetCell(14);
            cellHeaderWorkingTimeInMonth12.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeInMonth12);
            ICell cellHeaderWorkingTimeInMonthTotal = rowTemp.GetCell(15);
            cellHeaderWorkingTimeInMonthTotal.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeInMonthTotal.ToString().Replace("時間", ":").Replace("分", ""));

            rowTemp = sheet.GetRow(rowStart + 67);

            ICell cellHeaderWorkingTimeWeeklyAverage1 = rowTemp.GetCell(2);
            cellHeaderWorkingTimeWeeklyAverage1.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeWeeklyAverage1);
            ICell cellHeaderWorkingTimeWeeklyAverage2 = rowTemp.GetCell(3);
            cellHeaderWorkingTimeWeeklyAverage2.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeWeeklyAverage2);
            ICell cellHeaderWorkingTimeWeeklyAverage3 = rowTemp.GetCell(4);
            cellHeaderWorkingTimeWeeklyAverage3.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeWeeklyAverage3);
            ICell cellHeaderWorkingTimeWeeklyAverage4 = rowTemp.GetCell(5);
            cellHeaderWorkingTimeWeeklyAverage4.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeWeeklyAverage4);
            ICell cellHeaderWorkingTimeWeeklyAverage5 = rowTemp.GetCell(6);
            cellHeaderWorkingTimeWeeklyAverage5.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeWeeklyAverage5);
            ICell cellHeaderWorkingTimeWeeklyAverage6 = rowTemp.GetCell(7);
            cellHeaderWorkingTimeWeeklyAverage6.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeWeeklyAverage6);
            ICell cellHeaderWorkingTimeWeeklyAverage7 = rowTemp.GetCell(9);
            cellHeaderWorkingTimeWeeklyAverage7.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeWeeklyAverage7);
            ICell cellHeaderWorkingTimeWeeklyAverage8 = rowTemp.GetCell(10);
            cellHeaderWorkingTimeWeeklyAverage8.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeWeeklyAverage8);
            ICell cellHeaderWorkingTimeWeeklyAverage9 = rowTemp.GetCell(11);
            cellHeaderWorkingTimeWeeklyAverage9.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeWeeklyAverage9);
            ICell cellHeaderWorkingTimeWeeklyAverage10 = rowTemp.GetCell(12);
            cellHeaderWorkingTimeWeeklyAverage10.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeWeeklyAverage10);
            ICell cellHeaderWorkingTimeWeeklyAverage11 = rowTemp.GetCell(13);
            cellHeaderWorkingTimeWeeklyAverage11.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeWeeklyAverage11);
            ICell cellHeaderWorkingTimeWeeklyAverage12 = rowTemp.GetCell(14);
            cellHeaderWorkingTimeWeeklyAverage12.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeWeeklyAverage12);
            ICell cellHeaderWorkingTimeWeeklyAverageTotal = rowTemp.GetCell(15);
            cellHeaderWorkingTimeWeeklyAverageTotal.SetCellValue(workingCalendarEntryExcelModal.WorkingTimeWeeklyAverageTotal.ToString().Replace("時間", ":").Replace("分", ""));

        }
    }
}
