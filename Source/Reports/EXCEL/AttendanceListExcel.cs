using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.SS.UserModel;
using OMS.Models;
using OMS.Models.Type;
using NPOI.SS.Util;
using NPOI.HSSF.UserModel;
using System.Collections;
using NPOI.XSSF.UserModel;
using System.Drawing;
using OMS.DAC;

namespace OMS.Reports.EXCEL
{
    public class AttendanceListExcel : BaseExcel
    {
        #region Variable
        /// <summary>
        /// Model Quotation Header Search
        /// </summary>
        public AttendanceListExcelModal modelInput;
        #endregion

        /// <summary>
        /// Output Excel
        /// </summary>
        /// <returns></returns>
        public IWorkbook OutputExcel()
        {
            IWorkbook wb = null;

            //Create Sheet
            wb = this.CreateWorkbook("勤務表", ".xlsx");

            // Get Sheet
            ISheet sheet = wb.GetSheet("年間勤務カレンダー");
            wb.SetSheetName(0, modelInput.UserCD + " " + modelInput.UserNm);

            //Fill data
            this.FillData(wb, sheet, modelInput);

            return wb;
        }

        /// <summary>
        /// Merged cell
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="firtRow"></param>
        /// <param name="lastRow"></param>
        /// <param name="FirtCol"></param>
        /// <param name="lastCol"></param>
        /// <param name="borderRight"></param>
        /// <param name="borderLeft"></param>
        /// <param name="borderTop"></param>
        /// <param name="borderButtom"></param>
        /// <param name="isThinRight"></param>
        /// <param name="isThinLeft"></param>
        /// <param name="isThinTop"></param>
        /// <param name="isThinButtom"></param>
        private void DesignMerged(IWorkbook wb, ISheet sheet, int firtRow, int lastRow, int FirtCol, int lastCol, Boolean borderRight, Boolean borderLeft, Boolean borderTop, Boolean borderButtom, Boolean isThinRight, Boolean isThinLeft, Boolean isThinTop, Boolean isThinButtom)
        {
            sheet.AddMergedRegion(new CellRangeAddress(firtRow, lastRow, FirtCol, lastCol));

            if (borderRight)
            {
                if (isThinRight)
                {
                    RegionUtil.SetBorderRight((int)BorderStyle.Thin, new CellRangeAddress(firtRow, lastRow, FirtCol, lastCol), sheet, wb);
                }
                else
                {
                    RegionUtil.SetBorderRight((int)BorderStyle.Hair, new CellRangeAddress(firtRow, lastRow, FirtCol, lastCol), sheet, wb);
                }

            }

            if (borderLeft)
            {
                if (isThinLeft)
                {

                    RegionUtil.SetBorderLeft((int)BorderStyle.Thin, new CellRangeAddress(firtRow, lastRow, FirtCol, lastCol), sheet, wb);
                }
                else
                {
                    RegionUtil.SetBorderLeft((int)BorderStyle.Hair, new CellRangeAddress(firtRow, lastRow, FirtCol, lastCol), sheet, wb);
                }

            }

            if (borderTop)
            {
                if (isThinTop)
                {

                    RegionUtil.SetBorderTop((int)BorderStyle.Thin, new CellRangeAddress(firtRow, lastRow, FirtCol, lastCol), sheet, wb);

                }
                else
                {

                    RegionUtil.SetBorderTop((int)BorderStyle.Hair, new CellRangeAddress(firtRow, lastRow, FirtCol, lastCol), sheet, wb);
                }

            }

            if (borderButtom)
            {
                if (isThinButtom)
                {

                    RegionUtil.SetBorderBottom((int)BorderStyle.Thin, new CellRangeAddress(firtRow, lastRow, FirtCol, lastCol), sheet, wb);
                }
                else
                {

                    RegionUtil.SetBorderBottom((int)BorderStyle.Hair, new CellRangeAddress(firtRow, lastRow, FirtCol, lastCol), sheet, wb);
                }

            }

        }

        /// <summary>
        /// setFontCellExcel 
        /// </summary>
        /// <param name="wb"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private IFont setFontCellExcel(IWorkbook wb, string type, short fontSize)
        {
            short color = 0;
            switch (type)
            {
                case "0":
                    color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                    break;
                case "1":
                    color = NPOI.HSSF.Util.HSSFColor.Blue.Index;
                    break;
                case "2":
                    color = NPOI.HSSF.Util.HSSFColor.Red.Index;
                    break;
            }

            IFont font = (IFont)wb.CreateFont();
            font.Color = color;
            font.Boldweight = fontSize;
            font.FontHeightInPoints = fontSize;
            font.FontName = "メイリオ";
            return font;
        }

        /// <summary>
        /// Fill data on excel
        /// </summary>
        /// <param name="sheet">ISheet</param>
        /// <param name="lstData">Data list  of billing</param>
        private void FillData(IWorkbook wb, ISheet sheet, AttendanceListExcelModal attendanceListExcelModal)
        {
            Color BgColog = Color.Yellow;
            using (DB db = new DB())
            {
                Config_DService config_DService = new Config_DService(db);
                IList<M_Config_D> intervalTimeList;
                intervalTimeList = config_DService.GetListByConfigCd(M_Config_H.CONFIG_CD_INTERVAL_TIME);
                if (intervalTimeList.Any() && intervalTimeList[0].Value1 == 1)
                {
                    BgColog = (Color)ColorTranslator.FromHtml(intervalTimeList[0].Value4);
                }
            }
            byte[] rgb = new byte[3] { 221, 235, 247 };
            byte[] rgbPaidLeave = new byte[3] { 226, 240, 217 };
            XSSFWorkbook xSSFWorkbook = new XSSFWorkbook();

            XSSFCellStyle hStyleBorderThinDetail = (XSSFCellStyle)wb.CreateCellStyle();
            hStyleBorderThinDetail.BorderRight = BorderStyle.Thin;
            hStyleBorderThinDetail.BorderBottom = BorderStyle.Hair;
            hStyleBorderThinDetail.Alignment = HorizontalAlignment.Center;
            hStyleBorderThinDetail.VerticalAlignment = VerticalAlignment.Center;
            hStyleBorderThinDetail.DataFormat = xSSFWorkbook.CreateDataFormat().GetFormat("@");
            hStyleBorderThinDetail.SetFont(setFontCellExcel(wb, "0", 10));

            XSSFCellStyle hStyleBorderDashedDetail = (XSSFCellStyle)wb.CreateCellStyle();
            hStyleBorderDashedDetail.BorderRight = BorderStyle.Hair;
            hStyleBorderDashedDetail.BorderBottom = BorderStyle.Hair;
            hStyleBorderDashedDetail.Alignment = HorizontalAlignment.Center;
            hStyleBorderDashedDetail.VerticalAlignment = VerticalAlignment.Center;
            hStyleBorderDashedDetail.DataFormat = xSSFWorkbook.CreateDataFormat().GetFormat("@");
            hStyleBorderDashedDetail.SetFont(setFontCellExcel(wb, "0", 10));

            XSSFCellStyle hStyleSaturday = (XSSFCellStyle)wb.CreateCellStyle();
            hStyleSaturday.Alignment = HorizontalAlignment.Center;
            hStyleSaturday.VerticalAlignment = VerticalAlignment.Center;
            hStyleSaturday.BorderLeft = BorderStyle.Thin;

            XSSFCellStyle hStyleHoliday = (XSSFCellStyle)wb.CreateCellStyle();
            hStyleHoliday.Alignment = HorizontalAlignment.Center;
            hStyleHoliday.VerticalAlignment = VerticalAlignment.Center;
            hStyleHoliday.BorderLeft = BorderStyle.Thin;
            hStyleHoliday.WrapText = true;

            XSSFCellStyle hStyleGreenThin = (XSSFCellStyle)wb.CreateCellStyle();
            hStyleGreenThin.FillPattern = FillPattern.SolidForeground;
            hStyleGreenThin.SetFillForegroundColor(new XSSFColor(rgb));
            hStyleGreenThin.Alignment = HorizontalAlignment.Center;
            hStyleGreenThin.VerticalAlignment = VerticalAlignment.Center;
            hStyleGreenThin.BorderRight = BorderStyle.Thin;
            hStyleGreenThin.BorderBottom = BorderStyle.Thin;
            hStyleGreenThin.SetFont(setFontCellExcel(wb, "0", 10));

            XSSFCellStyle hStyleGreenDashed = (XSSFCellStyle)wb.CreateCellStyle();
            hStyleGreenDashed.FillPattern = FillPattern.SolidForeground;
            hStyleGreenDashed.SetFillForegroundColor(new XSSFColor(rgb));
            hStyleGreenDashed.Alignment = HorizontalAlignment.Center;
            hStyleGreenDashed.VerticalAlignment = VerticalAlignment.Center;
            hStyleGreenDashed.BorderRight = BorderStyle.Hair;
            hStyleGreenDashed.BorderBottom = BorderStyle.Thin;
            hStyleGreenDashed.SetFont(setFontCellExcel(wb, "0", 10));

            XSSFCellStyle hStyle2 = (XSSFCellStyle)wb.CreateCellStyle();
            hStyle2.FillPattern = FillPattern.SolidForeground;
            hStyle2.SetFillForegroundColor(new XSSFColor(rgb));
            hStyle2.Alignment = HorizontalAlignment.Center;
            hStyle2.BorderRight = BorderStyle.Thin;
            hStyle2.BorderLeft = BorderStyle.Thin;
            hStyle2.BorderTop = BorderStyle.Thin;
            hStyle2.BorderBottom = BorderStyle.Hair;
            hStyle2.VerticalAlignment = VerticalAlignment.Center;
            hStyle2.SetFont(setFontCellExcel(wb, "0", 10));

            XSSFCellStyle hStyle3 = (XSSFCellStyle)wb.CreateCellStyle();
            hStyle3.FillPattern = FillPattern.SolidForeground;
            hStyle3.SetFillForegroundColor(new XSSFColor(rgb));
            hStyle3.Alignment = HorizontalAlignment.Center;
            hStyle3.BorderRight = BorderStyle.Hair;
            hStyle3.BorderLeft = BorderStyle.Thin;
            hStyle3.BorderTop = BorderStyle.Thin;
            hStyle3.BorderBottom = BorderStyle.Thin;
            hStyle3.VerticalAlignment = VerticalAlignment.Center;
            hStyle3.SetFont(setFontCellExcel(wb, "0", 10));
            hStyle3.WrapText = true;

            XSSFCellStyle hStyle4 = (XSSFCellStyle)wb.CreateCellStyle();
            hStyle4.FillPattern = FillPattern.SolidForeground;
            hStyle4.SetFillForegroundColor(new XSSFColor(rgb));
            hStyle4.Alignment = HorizontalAlignment.Center;
            hStyle4.BorderRight = BorderStyle.Hair;
            hStyle4.BorderLeft = BorderStyle.Hair;
            hStyle4.BorderTop = BorderStyle.Thin;
            hStyle4.BorderBottom = BorderStyle.Hair;
            hStyle4.VerticalAlignment = VerticalAlignment.Center;
            hStyle4.SetFont(setFontCellExcel(wb, "0", 10));

            XSSFCellStyle hStyle5 = (XSSFCellStyle)wb.CreateCellStyle();
            hStyle5.FillPattern = FillPattern.SolidForeground;
            hStyle5.SetFillForegroundColor(new XSSFColor(rgb));
            hStyle5.Alignment = HorizontalAlignment.Center;
            hStyle5.BorderRight = BorderStyle.Thin;
            hStyle5.BorderLeft = BorderStyle.Hair;
            hStyle5.BorderTop = BorderStyle.Thin;
            hStyle5.BorderBottom = BorderStyle.Thin;
            hStyle5.VerticalAlignment = VerticalAlignment.Center;
            hStyle5.SetFont(setFontCellExcel(wb, "0", 10));
            hStyle5.WrapText = true;

            // set border row 6-7 merged
            XSSFCellStyle hStyle8 = (XSSFCellStyle)wb.CreateCellStyle();
            hStyle8.BorderRight = BorderStyle.Hair;
            hStyle8.Alignment = HorizontalAlignment.Center;
            hStyle8.VerticalAlignment = VerticalAlignment.Center;
            hStyle8.SetFont(setFontCellExcel(wb, "0", 10));
            hStyle8.DataFormat = xSSFWorkbook.CreateDataFormat().GetFormat("@");

            //set border row 6-7 merged
            XSSFCellStyle hStyle9 = (XSSFCellStyle)wb.CreateCellStyle();
            hStyle9.BorderRight = BorderStyle.Thin;
            hStyle9.Alignment = HorizontalAlignment.Center;
            hStyle9.VerticalAlignment = VerticalAlignment.Center;
            hStyle9.SetFont(setFontCellExcel(wb, "0", 10));
            hStyle9.DataFormat = xSSFWorkbook.CreateDataFormat().GetFormat("@");


            //border project detail
            XSSFCellStyle hStyleProjectDetail = (XSSFCellStyle)wb.CreateCellStyle();
            hStyleProjectDetail.BorderBottom = BorderStyle.Thin;
            hStyleProjectDetail.BorderTop = BorderStyle.Hair;
            hStyleProjectDetail.BorderRight = BorderStyle.Thin;
            hStyleProjectDetail.VerticalAlignment = VerticalAlignment.Center;
            hStyleProjectDetail.SetFont(setFontCellExcel(wb, "0", 10));
            hStyleProjectDetail.DataFormat = xSSFWorkbook.CreateDataFormat().GetFormat("@");

            //border detail 残業時間 GRID
            XSSFCellStyle hStyleTotalOvertimeHours = (XSSFCellStyle)wb.CreateCellStyle();
            hStyleTotalOvertimeHours.BorderRight = BorderStyle.Hair;
            hStyleTotalOvertimeHours.VerticalAlignment = VerticalAlignment.Center;
            hStyleTotalOvertimeHours.Alignment = HorizontalAlignment.Center;
            hStyleTotalOvertimeHours.SetFont(setFontCellExcel(wb, "0", 10));
            hStyleTotalOvertimeHours.DataFormat = xSSFWorkbook.CreateDataFormat().GetFormat("@");

            //border detail 労働時間 GRID
            XSSFCellStyle hStyletotalWorkingHours = (XSSFCellStyle)wb.CreateCellStyle();
            hStyletotalWorkingHours.Alignment = HorizontalAlignment.Center;
            hStyletotalWorkingHours.BorderRight = BorderStyle.Thin;
            hStyletotalWorkingHours.VerticalAlignment = VerticalAlignment.Center;
            hStyletotalWorkingHours.SetFont(setFontCellExcel(wb, "0", 10));
            hStyletotalWorkingHours.DataFormat = xSSFWorkbook.CreateDataFormat().GetFormat("@");

            //BackGroundColor Yellow
            XSSFCellStyle hStyletotalEntryTimeOutInterVal = (XSSFCellStyle)wb.CreateCellStyle();
            hStyletotalEntryTimeOutInterVal.FillPattern = FillPattern.SolidForeground;
            hStyletotalEntryTimeOutInterVal.SetFillForegroundColor(new XSSFColor(BgColog));
            hStyletotalEntryTimeOutInterVal.Alignment = HorizontalAlignment.Center;
            hStyletotalEntryTimeOutInterVal.VerticalAlignment = VerticalAlignment.Center;
            //hStyletotalEntryTimeOutInterVal.BorderRight = BorderStyle.Thin;
            //hStyletotalEntryTimeOutInterVal.BorderBottom = BorderStyle.Thin;
            hStyletotalEntryTimeOutInterVal.SetFont(setFontCellExcel(wb, "0", 10));

            int rowStart = 0;

            int sizeVacationTypeList = attendanceListExcelModal.VacationType.Count;
            int sizeOverTimeList = attendanceListExcelModal.OverTimeList.Count;

            IRow rowTemp = sheet.GetRow(rowStart + 3);
            GetOrCreateCell(rowTemp, 6).CellStyle = hStyle2;
            GetOrCreateCell(rowTemp, 6 + sizeVacationTypeList).CellStyle = hStyle2;

            rowTemp = sheet.GetRow(rowStart + 1);
            GetOrCreateCell(rowTemp, 2).SetCellValue(attendanceListExcelModal.DateOfService);
            GetOrCreateCell(rowTemp, 7).SetCellValue(attendanceListExcelModal.Department);
            GetOrCreateCell(rowTemp, 10).SetCellValue(attendanceListExcelModal.UserCD);
            GetOrCreateCell(rowTemp, 11).SetCellValue(attendanceListExcelModal.UserNm);

            //出勤
            rowTemp = sheet.GetRow(rowStart + 5);
            GetOrCreateCell(rowTemp, 1).SetCellValue(attendanceListExcelModal.NumWorkingDays == "0" ? "" : attendanceListExcelModal.NumWorkingDays + ".0");
            GetOrCreateCell(rowTemp, 2).SetCellValue(attendanceListExcelModal.NumLateDays == "0" ? "" : attendanceListExcelModal.NumLateDays + ".0");
            GetOrCreateCell(rowTemp, 3).SetCellValue(attendanceListExcelModal.NumEarlyDays == "0" ? "" : attendanceListExcelModal.NumEarlyDays + ".0");
            GetOrCreateCell(rowTemp, 4).SetCellValue(attendanceListExcelModal.NumSH_Days == "0" ? "" : attendanceListExcelModal.NumSH_Days + ".0");
            GetOrCreateCell(rowTemp, 5).SetCellValue(attendanceListExcelModal.NumLH_Days == "0" ? "" : attendanceListExcelModal.NumLH_Days + ".0");

            rowTemp = sheet.GetRow(rowStart + 6);
            GetOrCreateCell(rowTemp, 1).SetCellValue(attendanceListExcelModal.TimeWorkingHours);
            GetOrCreateCell(rowTemp, 2).SetCellValue(attendanceListExcelModal.TimeLateHours);
            GetOrCreateCell(rowTemp, 3).SetCellValue(attendanceListExcelModal.TimeEarlyHours);
            GetOrCreateCell(rowTemp, 4).SetCellValue(attendanceListExcelModal.TimeSH_Hours);
            GetOrCreateCell(rowTemp, 5).SetCellValue(attendanceListExcelModal.TimeLH_Hours);

            DesignMerged(wb, sheet, 3, 3, 6, 6 + sizeVacationTypeList - 1, true, true, true, true, true, true, true, false);
            rowTemp = sheet.GetRow(rowStart + 3);
            GetOrCreateCell(rowTemp, 6).SetCellValue("休暇");
            GetOrCreateCell(rowTemp, 6).CellStyle.Alignment = HorizontalAlignment.Center;

            DesignMerged(wb, sheet, 3, 3, 6 + sizeVacationTypeList, 6 + sizeVacationTypeList + sizeOverTimeList - 1, true, true, true, true, true, true, true, false);
            GetOrCreateCell(rowTemp, 6 + sizeVacationTypeList).SetCellValue("残業");
            GetOrCreateCell(rowTemp, 6 + sizeVacationTypeList).CellStyle.Alignment = HorizontalAlignment.Center;

            DesignMerged(wb, sheet, 3, 4, 6 + sizeVacationTypeList + sizeOverTimeList, 6 + sizeVacationTypeList + sizeOverTimeList, true, true, true, true, false, true, true, true);
            GetOrCreateCell(rowTemp, 6 + sizeVacationTypeList + sizeOverTimeList).SetCellValue("総残業" + Environment.NewLine + "時間");
            GetOrCreateCell(rowTemp, 6 + sizeVacationTypeList + sizeOverTimeList).CellStyle.Alignment = HorizontalAlignment.Center;
            GetOrCreateCell(rowTemp, 6 + sizeVacationTypeList + sizeOverTimeList).CellStyle = hStyle3;

            DesignMerged(wb, sheet, 3, 4, 6 + sizeVacationTypeList + sizeOverTimeList + 1, 6 + sizeVacationTypeList + sizeOverTimeList + 1, true, true, true, true, true, false, true, true);
            GetOrCreateCell(rowTemp, 6 + sizeVacationTypeList + sizeOverTimeList + 1).SetCellValue("総労働" + Environment.NewLine + "時間");
            GetOrCreateCell(rowTemp, 6 + sizeVacationTypeList + sizeOverTimeList + 1).CellStyle.Alignment = HorizontalAlignment.Center;
            GetOrCreateCell(rowTemp, 6 + sizeVacationTypeList + sizeOverTimeList + 1).CellStyle = hStyle5;

            for (int i = 0; i < attendanceListExcelModal.VacationType.Count; i++)
            {
                M_Config_D vacationInfo = (M_Config_D)attendanceListExcelModal.VacationType[i];

                rowTemp = sheet.GetRow(rowStart + 4);
                GetOrCreateCell(rowTemp, 6 + i).SetCellValue(vacationInfo.Value3.ToString());

                if (i == attendanceListExcelModal.VacationType.Count - 1)
                {
                    GetOrCreateCell(rowTemp, 6 + i).CellStyle = hStyleGreenThin;
                }
                else
                {
                    GetOrCreateCell(rowTemp, 6 + i).CellStyle = hStyleGreenDashed;
                }


                rowTemp = sheet.GetRow(rowStart + 5);
                sheet.AddMergedRegion(new CellRangeAddress(5, 6, 6 + i, 6 + i));

                GetOrCreateCell(rowTemp, 6 + i).SetCellValue(vacationInfo.Value4.ToString());

                if (i == attendanceListExcelModal.VacationType.Count - 1)
                {

                    GetOrCreateCell(rowTemp, 6 + i).CellStyle = hStyle9;
                    GetOrCreateCell(sheet.GetRow(rowStart + 6), 6 + i).CellStyle = hStyle9;
                }
                else
                {
                    GetOrCreateCell(rowTemp, 6 + i).CellStyle = hStyle8;
                    GetOrCreateCell(sheet.GetRow(rowStart + 6), 6 + i).CellStyle = hStyle8;
                }

                RegionUtil.SetBorderBottom((int)BorderStyle.Thin, new CellRangeAddress(5, 6, 6 + i, 6 + i), sheet, wb);

            }

            for (int i = 0; i < attendanceListExcelModal.OverTimeList.Count; i++)
            {
                M_Config_D overTimeInfo = (M_Config_D)attendanceListExcelModal.OverTimeList[i];

                rowTemp = sheet.GetRow(rowStart + 4);
                GetOrCreateCell(rowTemp, 6 + sizeVacationTypeList + i).SetCellValue(overTimeInfo.Value2.ToString());

                if (i == attendanceListExcelModal.OverTimeList.Count - 1)
                {
                    GetOrCreateCell(rowTemp, 6 + sizeVacationTypeList + i).CellStyle = hStyleGreenThin;


                }
                else
                {
                    GetOrCreateCell(rowTemp, 6 + sizeVacationTypeList + i).CellStyle = hStyleGreenDashed;

                }

                rowTemp = sheet.GetRow(rowStart + 5);
                sheet.AddMergedRegion(new CellRangeAddress(5, 6, 6 + sizeVacationTypeList + i, 6 + sizeVacationTypeList + i));

                GetOrCreateCell(rowTemp, 6 + sizeVacationTypeList + i).SetCellValue(overTimeInfo.Value4);

                if (i == attendanceListExcelModal.OverTimeList.Count - 1)
                {

                    GetOrCreateCell(rowTemp, 6 + sizeVacationTypeList + i).CellStyle = hStyle9;
                    GetOrCreateCell(sheet.GetRow(rowStart + 6), 6 + sizeVacationTypeList + i).CellStyle = hStyle9;
                }
                else
                {

                    GetOrCreateCell(rowTemp, 6 + sizeVacationTypeList + i).CellStyle = hStyle8;
                    GetOrCreateCell(sheet.GetRow(rowStart + 6), 6 + sizeVacationTypeList + i).CellStyle = hStyle8;
                }

                RegionUtil.SetBorderBottom((int)BorderStyle.Thin, new CellRangeAddress(5, 6, 6 + sizeVacationTypeList + i, 6 + sizeVacationTypeList + i), sheet, wb);

            }

            rowTemp = sheet.GetRow(rowStart + 5);

            //総残業時間
            GetOrCreateCell(rowTemp, 6 + sizeVacationTypeList + sizeOverTimeList).CellStyle = hStyle8;
            GetOrCreateCell(sheet.GetRow(rowStart + 6), 6 + sizeVacationTypeList + sizeOverTimeList).CellStyle = hStyle8;
            DesignMerged(wb, sheet, 5, 6, 6 + sizeVacationTypeList + sizeOverTimeList, 6 + sizeVacationTypeList + sizeOverTimeList, false, false, false, true, false, true, true, true);
            GetOrCreateCell(rowTemp, 6 + sizeVacationTypeList + sizeOverTimeList).SetCellValue(attendanceListExcelModal.TotalOverTimeHours);


            //総労働時間 
            GetOrCreateCell(rowTemp, 6 + sizeVacationTypeList + sizeOverTimeList + 1).CellStyle = hStyle9;
            GetOrCreateCell(sheet.GetRow(rowStart + 6), 6 + sizeVacationTypeList + sizeOverTimeList + 1).CellStyle = hStyle9;
            DesignMerged(wb, sheet, 5, 6, 6 + sizeVacationTypeList + sizeOverTimeList + 1, 6 + sizeVacationTypeList + sizeOverTimeList + 1, false, false, false, true, true, false, true, true);
            GetOrCreateCell(rowTemp, 6 + sizeVacationTypeList + sizeOverTimeList + 1).SetCellValue(attendanceListExcelModal.TotalWorkingHours);

            DesignMerged(wb, sheet, 8, 8, 11, 11 + sizeOverTimeList - 1, true, true, true, true, true, false, true, false);
            rowTemp = sheet.GetRow(rowStart + 8);
            GetOrCreateCell(rowTemp, 11).SetCellValue("残業実績");
            GetOrCreateCell(rowTemp, 11).CellStyle.Alignment = HorizontalAlignment.Center;
            GetOrCreateCell(rowTemp, 11).CellStyle = hStyle4;

            DesignMerged(wb, sheet, 8, 9, 11 + sizeOverTimeList, 11 + sizeOverTimeList, true, true, true, true, false, true, true, true);
            GetOrCreateCell(rowTemp, 11 + sizeOverTimeList).SetCellValue("残業" + Environment.NewLine + "時間");
            GetOrCreateCell(rowTemp, 11 + sizeOverTimeList).CellStyle.Alignment = HorizontalAlignment.Center;
            GetOrCreateCell(rowTemp, 11 + sizeOverTimeList).CellStyle = hStyle3;

            DesignMerged(wb, sheet, 8, 9, 11 + sizeOverTimeList + 1, 11 + sizeOverTimeList + 1, true, true, true, true, true, false, true, true);
            GetOrCreateCell(rowTemp, 11 + sizeOverTimeList + 1).SetCellValue("労働" + Environment.NewLine + "時間");
            GetOrCreateCell(rowTemp, 11 + sizeOverTimeList + 1).CellStyle.Alignment = HorizontalAlignment.Center;
            GetOrCreateCell(rowTemp, 11 + sizeOverTimeList + 1).CellStyle = hStyle5;

            for (int i = 0; i < attendanceListExcelModal.OverTimeList.Count; i++)
            {
                M_Config_D overTimeInfo = (M_Config_D)attendanceListExcelModal.OverTimeList[i];
                rowTemp = sheet.GetRow(rowStart + 9);
                GetOrCreateCell(rowTemp, 11 + i).SetCellValue(overTimeInfo.Value2.ToString());

                if (i == attendanceListExcelModal.OverTimeList.Count - 1)
                {
                    GetOrCreateCell(rowTemp, 11 + i).CellStyle = hStyleGreenThin;
                }
                else
                {
                    GetOrCreateCell(rowTemp, 11 + i).CellStyle = hStyleGreenDashed;
                }
            }

            //list day
            for (int i = 0; i < 31; i++)
            {
                if (i < attendanceListExcelModal.DataDetailList.Count)
                {
                    AttendanceDetailInfo attendanceDetailInfo = (AttendanceDetailInfo)attendanceListExcelModal.DataDetailList[i];

                    DesignMerged(wb, sheet, 11 + i * 2, 11 + i * 2, 2, 10 + attendanceListExcelModal.OverTimeList.Count, false, false, false, false, false, false, false, true);

                    rowTemp = sheet.GetRow(rowStart + 11 + i * 2);
                    for (int indexCol = 0; indexCol < 10 + attendanceListExcelModal.OverTimeList.Count - 1; indexCol++)
                    {
                        GetOrCreateCell(rowTemp, indexCol + 2).CellStyle = hStyleProjectDetail;
                    }

                    string _memo = attendanceDetailInfo.Memo;
                    if (_memo.Length > 100)
                    {
                        _memo = attendanceDetailInfo.Memo.Substring(0, 100);
                    }

                    List<string> lstDescription = new List<string>();
                    if (!string.IsNullOrEmpty(attendanceDetailInfo.ExchangeStatus))
                    {
                        lstDescription.Add(attendanceDetailInfo.ExchangeStatus);
                    }

                    if (!string.IsNullOrEmpty(attendanceDetailInfo.ProjectInfo))
                    {
                        lstDescription.Add(attendanceDetailInfo.ProjectInfo);
                    }

                    if (!string.IsNullOrEmpty(_memo))
                    {
                        lstDescription.Add(_memo);
                    }

                    GetOrCreateCell(rowTemp, 2).SetCellValue(string.Join("\n", lstDescription));

                    GetOrCreateCell(rowTemp, 2).RichStringCellValue.ApplyFont(setFontCellExcel(wb, "0", 10));
                    GetOrCreateCell(rowTemp, 2).CellStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.White.Index;

                    foreach (var item in attendanceDetailInfo.VacaitionColor)
                    {
                        GetOrCreateCell(rowTemp, 2).RichStringCellValue.ApplyFont(int.Parse(item.Split('_')[0]), int.Parse(item.Split('_')[1]), setFontCellExcel(wb, "2", 10));
                    }

                    if (!string.IsNullOrEmpty(attendanceDetailInfo.ExchangeStatus))
                    {
                        GetOrCreateCell(rowTemp, 2).RichStringCellValue.ApplyFont(0, attendanceDetailInfo.ExchangeStatus.Length, setFontCellExcel(wb, "2", 10));
                    }

                    rowTemp = sheet.GetRow(rowStart + 10 + i * 2);


                    if (attendanceDetailInfo.HolidayName != string.Empty)
                    {
                        GetOrCreateCell(rowTemp, 0).SetCellValue(attendanceDetailInfo.StringDate + Environment.NewLine + attendanceDetailInfo.HolidayName);
                    }
                    else
                    {
                        GetOrCreateCell(rowTemp, 0).SetCellValue(attendanceDetailInfo.StringDate);
                    }

                    if (attendanceDetailInfo.TextColorClass == "text-color-saturday")
                    {
                        //set font cell
                        hStyleSaturday.SetFont(setFontCellExcel(wb, "1", 10));
                        GetOrCreateCell(rowTemp, 0).CellStyle = hStyleSaturday;
                    }
                    else
                    {
                        if (attendanceDetailInfo.TextColorClass == "text-color-holiday")
                        {
                            //set font cell
                            hStyleHoliday.SetFont(setFontCellExcel(wb, "2", 10));
                            GetOrCreateCell(rowTemp, 0).CellStyle = hStyleHoliday;
                        }
                    }

                    GetOrCreateCell(rowTemp, 2).SetCellValue(attendanceDetailInfo.WorkingSystemName.ToString().Replace("&nbsp;", ""));
                    GetOrCreateCell(rowTemp, 4).SetCellValue(attendanceDetailInfo.EntryTime);
                    /*Isv-Tinh 2020/04/13  Interval Time Config ADD Start*/
                    if (attendanceDetailInfo.OutIntervalTime)
                    {
                        GetOrCreateCell(rowTemp, 4).CellStyle = hStyletotalEntryTimeOutInterVal;
                    }
                    /*Isv-Tinh 2020/04/13  Interval Time Config ADD End*/

                    GetOrCreateCell(rowTemp, 5).SetCellValue(attendanceDetailInfo.ExitTime);
                    GetOrCreateCell(rowTemp, 6).SetCellValue(attendanceDetailInfo.WorkingHours);
                    GetOrCreateCell(rowTemp, 7).SetCellValue(attendanceDetailInfo.LateHours);
                    GetOrCreateCell(rowTemp, 8).SetCellValue(attendanceDetailInfo.EarlyHours);
                    GetOrCreateCell(rowTemp, 9).SetCellValue(attendanceDetailInfo.SH_Hours);
                    GetOrCreateCell(rowTemp, 10).SetCellValue(attendanceDetailInfo.LH_Hours);

                    for (int j = 0; j < attendanceListExcelModal.OverTimeList.Count; j++)
                    {

                        if (j == attendanceListExcelModal.OverTimeList.Count - 1)
                        {
                            GetOrCreateCell(rowTemp, 11 + j).CellStyle = hStyleBorderThinDetail;
                        }
                        else
                        {
                            GetOrCreateCell(rowTemp, 11 + j).CellStyle = hStyleBorderDashedDetail;
                        }

                        switch (j)
                        {
                            case 0:
                                GetOrCreateCell(rowTemp, 11 + j).SetCellValue(attendanceDetailInfo.PrematureOvertimeWork);
                                break;
                            case 1:
                                GetOrCreateCell(rowTemp, 11 + j).SetCellValue(attendanceDetailInfo.RegularOvertime);
                                break;
                            case 2:
                                GetOrCreateCell(rowTemp, 11 + j).SetCellValue(attendanceDetailInfo.LateNightOvertime);
                                break;
                            case 3:
                                GetOrCreateCell(rowTemp, 11 + j).SetCellValue(attendanceDetailInfo.PredeterminedHolidayLateNight);
                                break;
                            case 4:
                                GetOrCreateCell(rowTemp, 11 + j).SetCellValue(attendanceDetailInfo.LegalHolidayLateNight);
                                break;
                        }

                    }

                    GetOrCreateCell(rowTemp, 10 + attendanceListExcelModal.OverTimeList.Count + 1).CellStyle = hStyleTotalOvertimeHours;
                    GetOrCreateCell(sheet.GetRow(rowStart + 10 + i * 2 + 1), 10 + attendanceListExcelModal.OverTimeList.Count + 1).CellStyle = hStyleTotalOvertimeHours;
                    DesignMerged(wb, sheet, 10 + i * 2, 11 + i * 2, 10 + attendanceListExcelModal.OverTimeList.Count + 1, 10 + attendanceListExcelModal.OverTimeList.Count + 1, false, false, false, true, false, true, true, true);
                    GetOrCreateCell(rowTemp, 10 + attendanceListExcelModal.OverTimeList.Count + 1).SetCellValue(attendanceDetailInfo.TotalOverTimeForDay);

                    GetOrCreateCell(rowTemp, 10 + attendanceListExcelModal.OverTimeList.Count + 2).CellStyle = hStyletotalWorkingHours;
                    GetOrCreateCell(sheet.GetRow(rowStart + 10 + i * 2 + 1), 10 + attendanceListExcelModal.OverTimeList.Count + 2).CellStyle = hStyletotalWorkingHours;
                    DesignMerged(wb, sheet, 10 + i * 2, 11 + i * 2, 10 + attendanceListExcelModal.OverTimeList.Count + 2, 10 + attendanceListExcelModal.OverTimeList.Count + 2, false, false, false, true, true, false, true, true);
                    GetOrCreateCell(rowTemp, 10 + attendanceListExcelModal.OverTimeList.Count + 2).SetCellValue(attendanceDetailInfo.TotalWorkingHoursForDay);

                    if (attendanceDetailInfo.WorkingSystemName == "有休予定日")
                    {
                        var rowTemp1 = sheet.GetRow(rowStart + 11 + i * 2);
                        for (int j = 0; j <= 10 + attendanceListExcelModal.OverTimeList.Count + 2; j++)
                        {
                            XSSFCellStyle cellStyle = (XSSFCellStyle)wb.CreateCellStyle();
                            cellStyle.CloneStyleFrom((XSSFCellStyle)GetOrCreateCell(rowTemp, j).CellStyle);
                            cellStyle.FillPattern = FillPattern.SolidForeground;
                            cellStyle.SetFillForegroundColor(new XSSFColor(rgbPaidLeave));
                            GetOrCreateCell(rowTemp, j).CellStyle = cellStyle;

                            cellStyle = (XSSFCellStyle)wb.CreateCellStyle();
                            cellStyle.CloneStyleFrom((XSSFCellStyle)GetOrCreateCell(rowTemp1, j).CellStyle);
                            cellStyle.FillPattern = FillPattern.SolidForeground;
                            cellStyle.SetFillForegroundColor(new XSSFColor(rgbPaidLeave));
                            GetOrCreateCell(rowTemp1, j).CellStyle = cellStyle;
                        }
                    }

                }
                else
                {

                    sheet.RemoveRow(sheet.GetRow(rowStart + 10 + i * 2));
                    sheet.RemoveRow(sheet.GetRow(rowStart + 11 + i * 2));

                    CellReference cellDateRef = new CellReference(String.Format("A{0}", rowStart + 11 + i * 2));

                    for (int index = sheet.NumMergedRegions - 1; index >= 0; index--)
                    {
                        CellRangeAddress range = sheet.GetMergedRegion(index);

                        if (range.FirstRow == cellDateRef.Row)
                        {
                            sheet.RemoveMergedRegion(index);
                        }

                    }

                }
            }

        }
    }
}
