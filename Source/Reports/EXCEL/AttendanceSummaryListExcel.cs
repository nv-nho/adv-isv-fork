using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OMS.Models;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using OMS.Utilities;
using NPOI.HSSF.UserModel;

namespace OMS.Reports.EXCEL
{
    public class AttendanceSummaryListExcel : BaseExcel
    {
        #region Variable
        /// <summary>
        /// Model Quotation Header Search
        /// </summary>
        public AttendanceSummaryListExcelModal modelInput;
        #endregion

        /// <summary>
        /// Output Excel
        /// </summary>
        /// <returns></returns>
        public IWorkbook OutputExcel()
        {
            IWorkbook wb = null;

            //Create Sheet
            wb = this.CreateWorkbook("勤務集計表", ".xlsx");

            // Get Sheet
            ISheet sheet = wb.GetSheet("勤務集計表");
            wb.SetSheetName(0, "勤務集計表");

            //Fill data
            this.FillData(wb, sheet, modelInput);

            return wb;
        }

        /// <summary>
        /// Fill data on excel
        /// </summary>
        /// <param name="sheet">ISheet</param>
        /// <param name="lstData">Data list  of billing</param>
        private void FillData(IWorkbook wb, ISheet sheet, AttendanceSummaryListExcelModal attendanceSummaryListExcelModal)
        {
            XSSFWorkbook hSSFWorkbook = new XSSFWorkbook();
            byte[] rgb = new byte[3] { 226, 239, 218 };
            byte[] rgbWhite = new byte[3] { 255, 255, 255 };
            byte[] rgbDanger = new byte[3] { 242, 222, 222 };

            XSSFCellStyle hStyleGreenThin = (XSSFCellStyle)wb.CreateCellStyle();
            hStyleGreenThin.FillPattern = FillPattern.SolidForeground;
            hStyleGreenThin.SetFillForegroundColor(new XSSFColor(rgb));
            hStyleGreenThin.Alignment = HorizontalAlignment.Center;
            hStyleGreenThin.VerticalAlignment = VerticalAlignment.Center;
            hStyleGreenThin.BorderRight = BorderStyle.Thin;
            hStyleGreenThin.BorderBottom = BorderStyle.Thin;
            hStyleGreenThin.SetFont(setFontCellExcel(wb, 10));

            XSSFCellStyle hStyleBorderThinDetailTop = (XSSFCellStyle)wb.CreateCellStyle();
            hStyleBorderThinDetailTop.BorderRight = BorderStyle.Thin;
            hStyleBorderThinDetailTop.BorderBottom = BorderStyle.Hair;
            hStyleBorderThinDetailTop.Alignment = HorizontalAlignment.Center;
            hStyleBorderThinDetailTop.VerticalAlignment = VerticalAlignment.Center;
            hStyleBorderThinDetailTop.DataFormat = hSSFWorkbook.CreateDataFormat().GetFormat("@");
            hStyleBorderThinDetailTop.SetFont(setFontCellExcel(wb, 10));

            XSSFCellStyle hStyleBorderThinDetailTop_D = (XSSFCellStyle)wb.CreateCellStyle();
            hStyleBorderThinDetailTop_D.FillPattern = FillPattern.SolidForeground;
            hStyleBorderThinDetailTop_D.SetFillForegroundColor(new XSSFColor(rgbDanger));
            hStyleBorderThinDetailTop_D.BorderRight = BorderStyle.Thin;
            hStyleBorderThinDetailTop_D.BorderBottom = BorderStyle.Hair;
            hStyleBorderThinDetailTop_D.Alignment = HorizontalAlignment.Center;
            hStyleBorderThinDetailTop_D.VerticalAlignment = VerticalAlignment.Center;
            hStyleBorderThinDetailTop_D.DataFormat = hSSFWorkbook.CreateDataFormat().GetFormat("@");
            hStyleBorderThinDetailTop_D.SetFont(setFontCellExcel(wb, 10));

            XSSFCellStyle hStyleBorderThinDetailBottom = (XSSFCellStyle)wb.CreateCellStyle();
            hStyleBorderThinDetailBottom.BorderRight = BorderStyle.Thin;
            hStyleBorderThinDetailBottom.BorderTop = BorderStyle.Hair;
            hStyleBorderThinDetailBottom.BorderBottom = BorderStyle.Thin;
            hStyleBorderThinDetailBottom.Alignment = HorizontalAlignment.Center;
            hStyleBorderThinDetailBottom.VerticalAlignment = VerticalAlignment.Center;
            hStyleBorderThinDetailBottom.DataFormat = hSSFWorkbook.CreateDataFormat().GetFormat("@");
            hStyleBorderThinDetailBottom.SetFont(setFontCellExcel(wb, 10));

            XSSFCellStyle hStyleBorderThinDetailBottom_D = (XSSFCellStyle)wb.CreateCellStyle();
            hStyleBorderThinDetailBottom_D.FillPattern = FillPattern.SolidForeground;
            hStyleBorderThinDetailBottom_D.SetFillForegroundColor(new XSSFColor(rgbDanger));
            hStyleBorderThinDetailBottom_D.BorderRight = BorderStyle.Thin;
            hStyleBorderThinDetailBottom_D.BorderTop = BorderStyle.Hair;
            hStyleBorderThinDetailBottom_D.BorderBottom = BorderStyle.Thin;
            hStyleBorderThinDetailBottom_D.Alignment = HorizontalAlignment.Center;
            hStyleBorderThinDetailBottom_D.VerticalAlignment = VerticalAlignment.Center;
            hStyleBorderThinDetailBottom_D.DataFormat = hSSFWorkbook.CreateDataFormat().GetFormat("@");
            hStyleBorderThinDetailBottom_D.SetFont(setFontCellExcel(wb, 10));

            XSSFCellStyle hStyleGreenDashed = (XSSFCellStyle)wb.CreateCellStyle();
            hStyleGreenDashed.FillPattern = FillPattern.SolidForeground;
            hStyleGreenDashed.SetFillForegroundColor(new XSSFColor(rgb));
            hStyleGreenDashed.Alignment = HorizontalAlignment.Center;
            hStyleGreenDashed.VerticalAlignment = VerticalAlignment.Center;
            hStyleGreenDashed.BorderRight = BorderStyle.Hair;
            hStyleGreenDashed.BorderBottom = BorderStyle.Thin;
            hStyleGreenDashed.SetFont(setFontCellExcel(wb, 10));

            XSSFCellStyle hStyleGreenDashedDetailTop = (XSSFCellStyle)wb.CreateCellStyle();
            hStyleGreenDashedDetailTop.SetFillForegroundColor(new XSSFColor(rgb));
            hStyleGreenDashedDetailTop.Alignment = HorizontalAlignment.Center;
            hStyleGreenDashedDetailTop.VerticalAlignment = VerticalAlignment.Center;
            hStyleGreenDashedDetailTop.BorderRight = BorderStyle.Hair;
            hStyleGreenDashedDetailTop.BorderBottom = BorderStyle.Hair;
            hStyleGreenDashedDetailTop.DataFormat = hSSFWorkbook.CreateDataFormat().GetFormat("@");
            hStyleGreenDashedDetailTop.SetFont(setFontCellExcel(wb, 10));

            XSSFCellStyle hStyleGreenDashedDetailTop_D = (XSSFCellStyle)wb.CreateCellStyle();
            hStyleGreenDashedDetailTop_D.FillPattern = FillPattern.SolidForeground;
            hStyleGreenDashedDetailTop_D.SetFillForegroundColor(new XSSFColor(rgbDanger));
            hStyleGreenDashedDetailTop_D.Alignment = HorizontalAlignment.Center;
            hStyleGreenDashedDetailTop_D.VerticalAlignment = VerticalAlignment.Center;
            hStyleGreenDashedDetailTop_D.BorderRight = BorderStyle.Hair;
            hStyleGreenDashedDetailTop_D.BorderBottom = BorderStyle.Hair;
            hStyleGreenDashedDetailTop_D.DataFormat = hSSFWorkbook.CreateDataFormat().GetFormat("@");
            hStyleGreenDashedDetailTop_D.SetFont(setFontCellExcel(wb, 10));

            XSSFCellStyle hStyleGreenDashedDetailBottom = (XSSFCellStyle)wb.CreateCellStyle();
            hStyleGreenDashedDetailBottom.SetFillForegroundColor(new XSSFColor(rgb));
            hStyleGreenDashedDetailBottom.Alignment = HorizontalAlignment.Center;
            hStyleGreenDashedDetailBottom.VerticalAlignment = VerticalAlignment.Center;
            hStyleGreenDashedDetailBottom.BorderRight = BorderStyle.Hair;
            hStyleGreenDashedDetailBottom.BorderTop = BorderStyle.Hair;
            hStyleGreenDashedDetailBottom.BorderBottom = BorderStyle.Thin;
            hStyleGreenDashedDetailBottom.DataFormat = hSSFWorkbook.CreateDataFormat().GetFormat("@");
            hStyleGreenDashedDetailBottom.SetFont(setFontCellExcel(wb, 10));

            XSSFCellStyle hStyleGreenDashedDetailBottom_D = (XSSFCellStyle)wb.CreateCellStyle();
            hStyleGreenDashedDetailBottom_D.FillPattern = FillPattern.SolidForeground;
            hStyleGreenDashedDetailBottom_D.SetFillForegroundColor(new XSSFColor(rgbDanger));
            hStyleGreenDashedDetailBottom_D.Alignment = HorizontalAlignment.Center;
            hStyleGreenDashedDetailBottom_D.VerticalAlignment = VerticalAlignment.Center;
            hStyleGreenDashedDetailBottom_D.BorderRight = BorderStyle.Hair;
            hStyleGreenDashedDetailBottom_D.BorderTop = BorderStyle.Hair;
            hStyleGreenDashedDetailBottom_D.BorderBottom = BorderStyle.Thin;
            hStyleGreenDashedDetailBottom_D.DataFormat = hSSFWorkbook.CreateDataFormat().GetFormat("@");
            hStyleGreenDashedDetailBottom_D.SetFont(setFontCellExcel(wb, 10));

            //----------------------------------

            XSSFCellStyle hStyle1 = (XSSFCellStyle)wb.CreateCellStyle();
            hStyle1.FillPattern = FillPattern.SolidForeground;
            hStyle1.SetFillForegroundColor(new XSSFColor(rgb));
            hStyle1.Alignment = HorizontalAlignment.Center;
            hStyle1.BorderRight = BorderStyle.Thin;
            hStyle1.BorderLeft = BorderStyle.Thin;
            hStyle1.BorderTop = BorderStyle.Thin;
            hStyle1.BorderBottom = BorderStyle.Thin;
            hStyle1.VerticalAlignment = VerticalAlignment.Center;
            hStyle1.WrapText = true;
            hStyle1.SetFont(setFontCellExcel(wb, 10));

            XSSFCellStyle hStyle2 = (XSSFCellStyle)wb.CreateCellStyle();
            hStyle2.FillPattern = FillPattern.SolidForeground;
            hStyle2.SetFillForegroundColor(new XSSFColor(rgb));
            hStyle2.Alignment = HorizontalAlignment.Center;
            hStyle2.BorderRight = BorderStyle.Hair;
            hStyle2.BorderLeft = BorderStyle.Thin;
            hStyle2.BorderTop = BorderStyle.Thin;
            hStyle2.BorderBottom = BorderStyle.Thin;
            hStyle2.VerticalAlignment = VerticalAlignment.Center;
            hStyle2.SetFont(setFontCellExcel(wb, 10));
            hStyle2.WrapText = true;

            XSSFCellStyle hStyle2_Detail = (XSSFCellStyle)wb.CreateCellStyle();
            hStyle2_Detail.Alignment = HorizontalAlignment.Center;
            hStyle2_Detail.BorderRight = BorderStyle.Hair;
            hStyle2_Detail.BorderLeft = BorderStyle.Thin;
            hStyle2_Detail.BorderTop = BorderStyle.Thin;
            hStyle2_Detail.BorderBottom = BorderStyle.Thin;
            hStyle2_Detail.VerticalAlignment = VerticalAlignment.Center;
            hStyle2_Detail.DataFormat = hSSFWorkbook.CreateDataFormat().GetFormat("@");
            hStyle2_Detail.SetFont(setFontCellExcel(wb, 10));
            hStyle2_Detail.WrapText = true;

            XSSFCellStyle hStyle2_Detail_D = (XSSFCellStyle)wb.CreateCellStyle();
            hStyle2_Detail_D.FillPattern = FillPattern.SolidForeground;
            hStyle2_Detail_D.SetFillForegroundColor(new XSSFColor(rgbDanger));
            hStyle2_Detail_D.Alignment = HorizontalAlignment.Center;
            hStyle2_Detail_D.BorderRight = BorderStyle.Hair;
            hStyle2_Detail_D.BorderLeft = BorderStyle.Thin;
            hStyle2_Detail_D.BorderTop = BorderStyle.Thin;
            hStyle2_Detail_D.BorderBottom = BorderStyle.Thin;
            hStyle2_Detail_D.VerticalAlignment = VerticalAlignment.Center;
            hStyle2_Detail_D.DataFormat = hSSFWorkbook.CreateDataFormat().GetFormat("@");
            hStyle2_Detail_D.SetFont(setFontCellExcel(wb, 10));
            hStyle2_Detail_D.WrapText = true;

            XSSFCellStyle hStyle3 = (XSSFCellStyle)wb.CreateCellStyle();
            hStyle3.FillPattern = FillPattern.SolidForeground;
            hStyle3.SetFillForegroundColor(new XSSFColor(rgb));
            hStyle3.Alignment = HorizontalAlignment.Center;
            hStyle3.BorderRight = BorderStyle.Thin;
            hStyle3.BorderLeft = BorderStyle.Hair;
            hStyle3.BorderTop = BorderStyle.Thin;
            hStyle3.BorderBottom = BorderStyle.Thin;
            hStyle3.VerticalAlignment = VerticalAlignment.Center;
            hStyle3.SetFont(setFontCellExcel(wb, 10));
            hStyle3.WrapText = true;

            XSSFCellStyle hStyle3_Detail = (XSSFCellStyle)wb.CreateCellStyle();
            hStyle3_Detail.Alignment = HorizontalAlignment.Center;
            hStyle3_Detail.BorderRight = BorderStyle.Thin;
            hStyle3_Detail.BorderLeft = BorderStyle.Hair;
            hStyle3_Detail.BorderTop = BorderStyle.Thin;
            hStyle3_Detail.BorderBottom = BorderStyle.Thin;
            hStyle3_Detail.VerticalAlignment = VerticalAlignment.Center;
            hStyle3_Detail.SetFont(setFontCellExcel(wb, 10));
            hStyle3_Detail.DataFormat = hSSFWorkbook.CreateDataFormat().GetFormat("@");
            hStyle3_Detail.WrapText = true;

            XSSFCellStyle hStyle3_Detail_D = (XSSFCellStyle)wb.CreateCellStyle();
            hStyle3_Detail_D.FillPattern = FillPattern.SolidForeground;
            hStyle3_Detail_D.SetFillForegroundColor(new XSSFColor(rgbDanger));
            hStyle3_Detail_D.Alignment = HorizontalAlignment.Center;
            hStyle3_Detail_D.BorderRight = BorderStyle.Thin;
            hStyle3_Detail_D.BorderLeft = BorderStyle.Hair;
            hStyle3_Detail_D.BorderTop = BorderStyle.Thin;
            hStyle3_Detail_D.BorderBottom = BorderStyle.Thin;
            hStyle3_Detail_D.VerticalAlignment = VerticalAlignment.Center;
            hStyle3_Detail_D.SetFont(setFontCellExcel(wb, 10));
            hStyle3_Detail_D.DataFormat = hSSFWorkbook.CreateDataFormat().GetFormat("@");
            hStyle3_Detail_D.WrapText = true;

            XSSFCellStyle hStyle4 = (XSSFCellStyle)wb.CreateCellStyle();
            hStyle4.FillPattern = FillPattern.SolidForeground;
            hStyle4.SetFillForegroundColor(new XSSFColor(rgb));
            hStyle4.Alignment = HorizontalAlignment.Center;
            hStyle4.BorderRight = BorderStyle.Hair;
            hStyle4.BorderLeft = BorderStyle.Hair;
            hStyle4.BorderTop = BorderStyle.Thin;
            hStyle4.BorderBottom = BorderStyle.Hair;
            hStyle4.VerticalAlignment = VerticalAlignment.Center;
            hStyle4.SetFont(setFontCellExcel(wb, 10));

            XSSFCellStyle hStyle5 = (XSSFCellStyle)wb.CreateCellStyle();
            hStyle5.Alignment = HorizontalAlignment.Center;
            hStyle5.BorderRight = BorderStyle.Thin;
            hStyle5.BorderLeft = BorderStyle.Thin;
            hStyle5.BorderTop = BorderStyle.Thin;
            hStyle5.BorderBottom = BorderStyle.Thin;
            hStyle5.VerticalAlignment = VerticalAlignment.Center;
            hStyle5.DataFormat = hSSFWorkbook.CreateDataFormat().GetFormat("@");
            hStyle5.SetFont(setFontCellExcel(wb, 10));

            XSSFCellStyle hStyle5_D = (XSSFCellStyle)wb.CreateCellStyle();
            hStyle5_D.FillPattern = FillPattern.SolidForeground;
            hStyle5_D.SetFillForegroundColor(new XSSFColor(rgbDanger));
            hStyle5_D.Alignment = HorizontalAlignment.Center;
            hStyle5_D.BorderRight = BorderStyle.Thin;
            hStyle5_D.BorderLeft = BorderStyle.Thin;
            hStyle5_D.BorderTop = BorderStyle.Thin;
            hStyle5_D.BorderBottom = BorderStyle.Thin;
            hStyle5_D.VerticalAlignment = VerticalAlignment.Center;
            hStyle5_D.DataFormat = hSSFWorkbook.CreateDataFormat().GetFormat("@");
            hStyle5_D.SetFont(setFontCellExcel(wb, 10));

            // UserName1 & DepartName
            XSSFCellStyle hStyle6 = (XSSFCellStyle)wb.CreateCellStyle();
            hStyle6.Alignment = HorizontalAlignment.Left;
            hStyle6.BorderRight = BorderStyle.Thin;
            hStyle6.BorderLeft = BorderStyle.Thin;
            hStyle6.BorderTop = BorderStyle.Thin;
            hStyle6.BorderBottom = BorderStyle.Thin;
            hStyle6.VerticalAlignment = VerticalAlignment.Center;
            hStyle6.SetFont(setFontCellExcel(wb, 10));
            hStyle6.WrapText = true;

            // UserName1 & DepartName
            XSSFCellStyle hStyle6_D = (XSSFCellStyle)wb.CreateCellStyle();
            hStyle6_D.FillPattern = FillPattern.SolidForeground;
            hStyle6_D.SetFillForegroundColor(new XSSFColor(rgbDanger));
            hStyle6_D.Alignment = HorizontalAlignment.Left;
            hStyle6_D.BorderRight = BorderStyle.Thin;
            hStyle6_D.BorderLeft = BorderStyle.Thin;
            hStyle6_D.BorderTop = BorderStyle.Thin;
            hStyle6_D.BorderBottom = BorderStyle.Thin;
            hStyle6_D.VerticalAlignment = VerticalAlignment.Center;
            hStyle6_D.SetFont(setFontCellExcel(wb, 10));
            hStyle6_D.WrapText = true;

            XSSFCellStyle hStyle7 = (XSSFCellStyle)wb.CreateCellStyle();
            hStyle7.BorderRight = BorderStyle.Hair;
            hStyle7.Alignment = HorizontalAlignment.Center;
            hStyle7.VerticalAlignment = VerticalAlignment.Center;
            hStyle7.SetFont(setFontCellExcel(wb, 10));
            hStyle7.DataFormat = hSSFWorkbook.CreateDataFormat().GetFormat("@");

            XSSFCellStyle hStyle7_D = (XSSFCellStyle)wb.CreateCellStyle();
            hStyle7_D.FillPattern = FillPattern.SolidForeground;
            hStyle7_D.SetFillForegroundColor(new XSSFColor(rgbDanger));
            hStyle7_D.BorderRight = BorderStyle.Hair;
            hStyle7_D.Alignment = HorizontalAlignment.Center;
            hStyle7_D.VerticalAlignment = VerticalAlignment.Center;
            hStyle7_D.SetFont(setFontCellExcel(wb, 10));
            hStyle7_D.DataFormat = hSSFWorkbook.CreateDataFormat().GetFormat("@");

            XSSFCellStyle hStyle8 = (XSSFCellStyle)wb.CreateCellStyle();
            hStyle8.BorderRight = BorderStyle.Thin;
            hStyle8.Alignment = HorizontalAlignment.Center;
            hStyle8.VerticalAlignment = VerticalAlignment.Center;
            hStyle8.SetFont(setFontCellExcel(wb, 10));
            hStyle8.DataFormat = hSSFWorkbook.CreateDataFormat().GetFormat("@");

            XSSFCellStyle hStyle8_D = (XSSFCellStyle)wb.CreateCellStyle();
            hStyle8_D.FillPattern = FillPattern.SolidForeground;
            hStyle8_D.SetFillForegroundColor(new XSSFColor(rgbDanger));
            hStyle8_D.BorderRight = BorderStyle.Thin;
            hStyle8_D.Alignment = HorizontalAlignment.Center;
            hStyle8_D.VerticalAlignment = VerticalAlignment.Center;
            hStyle8_D.SetFont(setFontCellExcel(wb, 10));
            hStyle8_D.DataFormat = hSSFWorkbook.CreateDataFormat().GetFormat("@");
           
            //-----------------------------------

            int rowStart = 0;

            int sizeVacationTypeList = attendanceSummaryListExcelModal.VacationHeaderList.Count;
            int sizeOverTimeList = attendanceSummaryListExcelModal.OverTimeHeaderList.Count;

            // set header data
            IRow rowTemp = GetOrCreateRow(sheet, rowStart + 1);
            IRow rowTemp1 = GetOrCreateRow(sheet, rowStart + 1);

            GetOrCreateCell(rowTemp, 2).SetCellValue(attendanceSummaryListExcelModal.CalendarNm);
            GetOrCreateCell(rowTemp, 19).SetCellValue("出力日時　" + DateTime.Now.ToString(Constants.FMT_DATE_TIME_EN));

            rowTemp = GetOrCreateRow(sheet, rowStart + 2);
            GetOrCreateCell(rowTemp, 2).SetCellValue(attendanceSummaryListExcelModal.FromDate);
            GetOrCreateCell(rowTemp, 6).SetCellValue(attendanceSummaryListExcelModal.ToDate);
            GetOrCreateCell(rowTemp, 10).SetCellValue(attendanceSummaryListExcelModal.DepartmentNm);

            rowTemp = GetOrCreateRow(sheet, rowStart + 3);
            GetOrCreateCell(rowTemp, 2).SetCellValue(attendanceSummaryListExcelModal.UserNm);
            
            rowTemp = GetOrCreateRow(sheet, rowStart + 4);
            GetOrCreateCell(rowTemp, 2).SetCellValue(attendanceSummaryListExcelModal.ExtractionConditions);
            GetOrCreateCell(rowTemp, 14).SetCellValue(attendanceSummaryListExcelModal.ApprovalState);

            for (int i = 0; i < attendanceSummaryListExcelModal.DataDetailList.Count; i++)
            {
                AttendanceSummaryInfo attendanceSummaryInfo = (AttendanceSummaryInfo)attendanceSummaryListExcelModal.DataDetailList[i];
                //set detail data
                rowTemp = GetOrCreateRow(sheet, rowStart + 6 + (i * 4));
                DesignMerged(wb, sheet, 6 + (i * 4), 7 + (i * 4), 0, 2, true, true, true, true, true, true, true, true);
                GetOrCreateCell(rowTemp, 0).SetCellValue("社員名");
                GetOrCreateCell(rowTemp, 0).CellStyle.Alignment = HorizontalAlignment.Center;
                GetOrCreateCell(rowTemp, 0).CellStyle = hStyle1;

                DesignMerged(wb, sheet, 6 + (i * 4), 7 + (i * 4), 3, 3, true, true, true, true, false, true, true, true);
                GetOrCreateCell(rowTemp, 3).SetCellValue("総残業" + Environment.NewLine + "時間");
                GetOrCreateCell(rowTemp, 3).CellStyle.Alignment = HorizontalAlignment.Center;
                GetOrCreateCell(rowTemp, 3).CellStyle = hStyle2;

                DesignMerged(wb, sheet, 6 + (i * 4), 7 + (i * 4), 4, 4, true, true, true, true, true, false, true, true);
                GetOrCreateCell(rowTemp, 4).SetCellValue("総労働" + Environment.NewLine + "時間");
                GetOrCreateCell(rowTemp, 4).CellStyle.Alignment = HorizontalAlignment.Center;
                GetOrCreateCell(rowTemp, 4).CellStyle = hStyle3;

                DesignMerged(wb, sheet, 6 + (i * 4), 6 + (i * 4), 5, 9, true, true, true, true, true, true, true, false);
                GetOrCreateCell(rowTemp, 5).SetCellValue("出勤");
                GetOrCreateCell(rowTemp, 5).CellStyle.Alignment = HorizontalAlignment.Center;
                GetOrCreateCell(rowTemp, 5).CellStyle = hStyle4;

                DesignMerged(wb, sheet, 6 + (i * 4), 6 + (i * 4), 10, 10 + sizeVacationTypeList - 1, true, true, true, true, true, true, true, false);
                GetOrCreateCell(rowTemp, 10).SetCellValue("休暇");
                GetOrCreateCell(rowTemp, 10).CellStyle.Alignment = HorizontalAlignment.Center;
                GetOrCreateCell(rowTemp, 10).CellStyle = hStyle4;

                DesignMerged(wb, sheet, 6 + (i * 4), 6 + (i * 4), 10 + sizeVacationTypeList, 10 + sizeVacationTypeList + sizeOverTimeList - 1, true, true, true, true, true, true, true, false);
                GetOrCreateCell(rowTemp, 10 + sizeVacationTypeList).SetCellValue("残業");
                GetOrCreateCell(rowTemp, 10 + sizeVacationTypeList).CellStyle.Alignment = HorizontalAlignment.Center;
                GetOrCreateCell(rowTemp, 10 + sizeVacationTypeList).CellStyle = hStyle4;

                rowTemp = GetOrCreateRow(sheet, rowStart + 7 + (i * 4));
                GetOrCreateCell(rowTemp, 5).SetCellValue("出勤");
                GetOrCreateCell(rowTemp, 5).CellStyle.Alignment = HorizontalAlignment.Center;
                GetOrCreateCell(rowTemp, 5).CellStyle = hStyleGreenDashed;

                GetOrCreateCell(rowTemp, 6).SetCellValue("遅刻");
                GetOrCreateCell(rowTemp, 6).CellStyle.Alignment = HorizontalAlignment.Center;
                GetOrCreateCell(rowTemp, 6).CellStyle = hStyleGreenDashed;

                GetOrCreateCell(rowTemp, 7).SetCellValue("早退");
                GetOrCreateCell(rowTemp, 7).CellStyle.Alignment = HorizontalAlignment.Center;
                GetOrCreateCell(rowTemp, 7).CellStyle = hStyleGreenDashed;

                GetOrCreateCell(rowTemp, 8).SetCellValue("所定休日");
                GetOrCreateCell(rowTemp, 8).CellStyle.Alignment = HorizontalAlignment.Center;
                GetOrCreateCell(rowTemp, 8).CellStyle = hStyleGreenDashed;

                GetOrCreateCell(rowTemp, 9).SetCellValue("法定休日");
                GetOrCreateCell(rowTemp, 9).CellStyle.Alignment = HorizontalAlignment.Center;
                GetOrCreateCell(rowTemp, 9).CellStyle = hStyleGreenThin;

                rowTemp = GetOrCreateRow(sheet, rowStart + 8 + (i * 4));
                DesignMerged(wb, sheet, 8+ (i * 4),9 + (i * 4), 0, 0, true, true, true, true, false, true, true, true);
                GetOrCreateCell(rowTemp, 0).SetCellValue((OMS.Utilities.EditDataUtil.ToFixCodeDB(attendanceSummaryInfo.UserCD, M_User.MAX_USER_CODE_SHOW)).ToString());
                GetOrCreateCell(rowTemp, 0).CellStyle.Alignment = HorizontalAlignment.Center;
                if (attendanceSummaryInfo.IsUnSubmit)
                {
                    GetOrCreateCell(rowTemp, 0).CellStyle = hStyle5_D;
                }
                else
                {
                    GetOrCreateCell(rowTemp, 0).CellStyle = hStyle5;
                }

                DesignMerged(wb, sheet, 8 + (i * 4), 9 + (i * 4), 1, 2, true, true, true, true, true, true, true, true);
                GetOrCreateCell(rowTemp, 1).SetCellValue(attendanceSummaryInfo.UserName1 + Environment.NewLine + attendanceSummaryInfo.DepartmentName);
                GetOrCreateCell(rowTemp, 1).CellStyle.Alignment = HorizontalAlignment.Center;
                if (attendanceSummaryInfo.IsUnSubmit)
                {
                    GetOrCreateCell(rowTemp, 1).CellStyle = hStyle6_D;
                }
                else
                {
                    GetOrCreateCell(rowTemp, 1).CellStyle = hStyle6;
                }

                DesignMerged(wb, sheet, 8 + (i * 4), 9 + (i * 4), 3, 3, true, true, true, true, false, true, true, true);
                GetOrCreateCell(rowTemp, 3).SetCellValue(attendanceSummaryInfo.TotalOverTimeHours);
                GetOrCreateCell(rowTemp, 3).CellStyle.Alignment = HorizontalAlignment.Center;
                if (attendanceSummaryInfo.IsUnSubmit)
                {
                    GetOrCreateCell(rowTemp, 3).CellStyle = hStyle2_Detail_D;
                }
                else
                {
                    GetOrCreateCell(rowTemp, 3).CellStyle = hStyle2_Detail;
                }

                DesignMerged(wb, sheet, 8 + (i * 4), 9 + (i * 4), 4, 4, true, true, true, true, true, false, true, true);
                GetOrCreateCell(rowTemp, 4).SetCellValue(attendanceSummaryInfo.TotalWorkingHours);
                GetOrCreateCell(rowTemp, 4).CellStyle.Alignment = HorizontalAlignment.Center;
                if (attendanceSummaryInfo.IsUnSubmit)
                {
                    GetOrCreateCell(rowTemp, 4).CellStyle = hStyle3_Detail_D;
                }
                else
                {
                    GetOrCreateCell(rowTemp, 4).CellStyle = hStyle3_Detail;
                }

                GetOrCreateCell(rowTemp, 5).SetCellValue(attendanceSummaryInfo.NumWorkingDays.ToString() != "0" ? attendanceSummaryInfo.NumWorkingDays.ToString("#.0") : string.Empty);
                GetOrCreateCell(rowTemp, 5).CellStyle.Alignment = HorizontalAlignment.Center;
                if (attendanceSummaryInfo.IsUnSubmit)
                {
                    GetOrCreateCell(rowTemp, 5).CellStyle = hStyleGreenDashedDetailTop_D;
                }
                else
                {
                    GetOrCreateCell(rowTemp, 5).CellStyle = hStyleGreenDashedDetailTop;
                }

                GetOrCreateCell(rowTemp, 6).SetCellValue(attendanceSummaryInfo.NumLateDays.ToString() != "0" ? attendanceSummaryInfo.NumLateDays.ToString("#.0") : string.Empty);
                GetOrCreateCell(rowTemp, 6).CellStyle.Alignment = HorizontalAlignment.Center;
                if (attendanceSummaryInfo.IsUnSubmit)
                {
                    GetOrCreateCell(rowTemp, 6).CellStyle = hStyleGreenDashedDetailTop_D;
                }
                else
                {
                    GetOrCreateCell(rowTemp, 6).CellStyle = hStyleGreenDashedDetailTop;
                }

                GetOrCreateCell(rowTemp, 7).SetCellValue(attendanceSummaryInfo.NumEarlyDays.ToString() != "0" ? attendanceSummaryInfo.NumEarlyDays.ToString("#.0") : string.Empty);
                GetOrCreateCell(rowTemp, 7).CellStyle.Alignment = HorizontalAlignment.Center;
                if (attendanceSummaryInfo.IsUnSubmit)
                {
                    GetOrCreateCell(rowTemp, 7).CellStyle = hStyleGreenDashedDetailTop_D;
                }
                else
                {
                    GetOrCreateCell(rowTemp, 7).CellStyle = hStyleGreenDashedDetailTop;
                }

                GetOrCreateCell(rowTemp, 8).SetCellValue(attendanceSummaryInfo.NumSH_Days.ToString() != "0" ? attendanceSummaryInfo.NumSH_Days.ToString("#.0") : string.Empty);
                GetOrCreateCell(rowTemp, 8).CellStyle.Alignment = HorizontalAlignment.Center;
                if (attendanceSummaryInfo.IsUnSubmit)
                {
                    GetOrCreateCell(rowTemp, 8).CellStyle = hStyleGreenDashedDetailTop_D;
                }
                else
                {
                    GetOrCreateCell(rowTemp, 8).CellStyle = hStyleGreenDashedDetailTop;
                }

                GetOrCreateCell(rowTemp, 9).SetCellValue(attendanceSummaryInfo.NumLH_Days.ToString() != "0" ? attendanceSummaryInfo.NumLH_Days.ToString("#.0") : string.Empty);
                GetOrCreateCell(rowTemp, 9).CellStyle.Alignment = HorizontalAlignment.Center;
                if (attendanceSummaryInfo.IsUnSubmit)
                {
                    GetOrCreateCell(rowTemp, 9).CellStyle = hStyleBorderThinDetailTop_D;
                }
                else
                {
                    GetOrCreateCell(rowTemp, 9).CellStyle = hStyleBorderThinDetailTop;
                }

                rowTemp = GetOrCreateRow(sheet, rowStart + 9 + (i * 4));
                GetOrCreateCell(rowTemp, 5).SetCellValue(attendanceSummaryInfo.WorkingHours);
                GetOrCreateCell(rowTemp, 5).CellStyle.Alignment = HorizontalAlignment.Center;
                if (attendanceSummaryInfo.IsUnSubmit)
                {
                    GetOrCreateCell(rowTemp, 5).CellStyle = hStyleGreenDashedDetailBottom_D;
                }
                else
                {
                    GetOrCreateCell(rowTemp, 5).CellStyle = hStyleGreenDashedDetailBottom;
                }

                GetOrCreateCell(rowTemp, 6).SetCellValue(attendanceSummaryInfo.LateHours);
                GetOrCreateCell(rowTemp, 6).CellStyle.Alignment = HorizontalAlignment.Center;
                if (attendanceSummaryInfo.IsUnSubmit)
                {
                    GetOrCreateCell(rowTemp, 6).CellStyle = hStyleGreenDashedDetailBottom_D;
                }
                else
                {
                    GetOrCreateCell(rowTemp, 6).CellStyle = hStyleGreenDashedDetailBottom;
                }

                GetOrCreateCell(rowTemp, 7).SetCellValue(attendanceSummaryInfo.EarlyHours);
                GetOrCreateCell(rowTemp, 7).CellStyle.Alignment = HorizontalAlignment.Center;
                if (attendanceSummaryInfo.IsUnSubmit)
                {
                    GetOrCreateCell(rowTemp, 7).CellStyle = hStyleGreenDashedDetailBottom_D;
                }
                else
                {
                    GetOrCreateCell(rowTemp, 7).CellStyle = hStyleGreenDashedDetailBottom;
                }

                GetOrCreateCell(rowTemp, 8).SetCellValue(attendanceSummaryInfo.SH_Hours);
                GetOrCreateCell(rowTemp, 8).CellStyle.Alignment = HorizontalAlignment.Center;
                if (attendanceSummaryInfo.IsUnSubmit)
                {
                    GetOrCreateCell(rowTemp, 8).CellStyle = hStyleGreenDashedDetailBottom_D;
                }
                else
                {
                    GetOrCreateCell(rowTemp, 8).CellStyle = hStyleGreenDashedDetailBottom;
                }

                GetOrCreateCell(rowTemp, 9).SetCellValue(attendanceSummaryInfo.LH_Hours);
                GetOrCreateCell(rowTemp, 9).CellStyle.Alignment = HorizontalAlignment.Center;
                if (attendanceSummaryInfo.IsUnSubmit)
                {
                    GetOrCreateCell(rowTemp, 9).CellStyle = hStyleBorderThinDetailBottom_D;
                }
                else
                {
                    GetOrCreateCell(rowTemp, 9).CellStyle = hStyleBorderThinDetailBottom;
                }

                IList<VacationDateInFoByAttendanceSummary> lstVacationDetailInfo = (IList<VacationDateInFoByAttendanceSummary>)attendanceSummaryListExcelModal.VacationDetailList[i];
                for (int j = 0; j < attendanceSummaryListExcelModal.VacationHeaderList.Count; j++)
                {
                    M_Config_D vacationHeaderInfo = (M_Config_D)attendanceSummaryListExcelModal.VacationHeaderList[j];
                    VacationDateInFoByAttendanceSummary vacationDetailInfo = lstVacationDetailInfo[j];
                    rowTemp = GetOrCreateRow(sheet, rowStart + 7 + (i * 4));
                    GetOrCreateCell(rowTemp, 10 + j).SetCellValue(vacationHeaderInfo.Value3.ToString());

                    if (j == attendanceSummaryListExcelModal.VacationHeaderList.Count - 1)
                    {
                        GetOrCreateCell(rowTemp, 10 + j).CellStyle = hStyleGreenThin;
                    }
                    else
                    {
                        GetOrCreateCell(rowTemp, 10 + j).CellStyle = hStyleGreenDashed;
                    }

                    rowTemp = GetOrCreateRow(sheet, rowStart + 8 +(i * 4));
                    sheet.AddMergedRegion(new CellRangeAddress(8 + (i * 4), 9 + (i * 4), 10 + j, 10 + j));

                    GetOrCreateCell(rowTemp, 10 + j).SetCellValue(vacationDetailInfo.VacationDate.ToString());
                    if (attendanceSummaryInfo.IsUnSubmit)
                    {
                        GetOrCreateCell(rowTemp, 10 + j).CellStyle = hStyle7_D;
                    }
                    else
                    {
                        GetOrCreateCell(rowTemp, 10 + j).CellStyle = hStyle7;
                    }

                    if (j == attendanceSummaryListExcelModal.VacationHeaderList.Count - 1)
                    {
                        if (attendanceSummaryInfo.IsUnSubmit)
                        {
                            GetOrCreateCell(rowTemp, 10 + j).CellStyle = hStyle8_D;
                        }
                        else
                        {
                            GetOrCreateCell(rowTemp, 10 + j).CellStyle = hStyle8;
                        }
                        rowTemp1 = sheet.GetRow(rowStart + 8 + (i * 4) + 1);
                        if (attendanceSummaryInfo.IsUnSubmit)
                        {
                            GetOrCreateCell(rowTemp1, 10 + j).CellStyle = hStyle8_D;
                        }
                        else
                        {
                            GetOrCreateCell(rowTemp1, 10 + j).CellStyle = hStyle8;
                        }
                    }
                    else
                    {
                        if (attendanceSummaryInfo.IsUnSubmit)
                        {
                            GetOrCreateCell(rowTemp, 10 + j).CellStyle = hStyle7_D;
                        }
                        else
                        {
                            GetOrCreateCell(rowTemp, 10 + j).CellStyle = hStyle7;
                        }
                        rowTemp1 = sheet.GetRow(rowStart + 8 + (i * 4) + 1);
                        if (attendanceSummaryInfo.IsUnSubmit)
                        {
                            GetOrCreateCell(rowTemp1, 10 + j).CellStyle = hStyle7_D;
                        }
                        else
                        {
                            GetOrCreateCell(rowTemp1, 10 + j).CellStyle = hStyle7;
                        }
                    }

                    RegionUtil.SetBorderBottom((int)BorderStyle.Thin, new CellRangeAddress(8 + (i * 4), 9 + (i * 4), 10 + j, 10 + j), sheet, wb);

                }

                IList<M_Config_D> lstOvertimeDetailInfo = (IList<M_Config_D>)attendanceSummaryListExcelModal.OverTimeDetailList[i];
                for (int k = 0; k < attendanceSummaryListExcelModal.OverTimeHeaderList.Count; k++)
                {
                    M_Config_D overTimeHeaderInfo = (M_Config_D)attendanceSummaryListExcelModal.OverTimeHeaderList[k];
                    M_Config_D overTimeDetailInfo = lstOvertimeDetailInfo[k];
                    rowTemp = sheet.GetRow(rowStart + 7 + (i * 4));
                    GetOrCreateCell(rowTemp, 10 + sizeVacationTypeList + k).SetCellValue(overTimeHeaderInfo.Value2.ToString());

                    if (k == attendanceSummaryListExcelModal.OverTimeHeaderList.Count - 1)
                    {
                        GetOrCreateCell(rowTemp, 10 + sizeVacationTypeList + k).CellStyle = hStyleGreenThin;
                    }
                    else
                    {
                        GetOrCreateCell(rowTemp, 10 + sizeVacationTypeList + k).CellStyle = hStyleGreenDashed;
                    }

                    rowTemp = sheet.GetRow(rowStart + 8 + (i * 4));
                    sheet.AddMergedRegion(new CellRangeAddress(8 + (i * 4), 9 + (i * 4), 10 + sizeVacationTypeList + k, 10 + sizeVacationTypeList + k));

                    GetOrCreateCell(rowTemp, 10 + sizeVacationTypeList + k).SetCellValue(overTimeDetailInfo.Value4);
                    if (attendanceSummaryInfo.IsUnSubmit)
                    {
                        GetOrCreateCell(rowTemp, 10 + sizeVacationTypeList + k).CellStyle = hStyle7_D;
                    }
                    else
                    {
                        GetOrCreateCell(rowTemp, 10 + sizeVacationTypeList + k).CellStyle = hStyle7;
                    }

                    if (k == attendanceSummaryListExcelModal.OverTimeHeaderList.Count - 1)
                    {
                        if (attendanceSummaryInfo.IsUnSubmit)
                        {
                            GetOrCreateCell(rowTemp, 10 + sizeVacationTypeList + k).CellStyle = hStyle8_D;
                        }
                        else
                        {
                            GetOrCreateCell(rowTemp, 10 + sizeVacationTypeList + k).CellStyle = hStyle8;
                        }
                        rowTemp1 = sheet.GetRow(rowStart + 8 + (i * 4) + 1);
                        if (attendanceSummaryInfo.IsUnSubmit)
                        {
                            GetOrCreateCell(rowTemp1, 10 + sizeVacationTypeList + k).CellStyle = hStyle8_D;
                        }
                        else
                        {
                            GetOrCreateCell(rowTemp1, 10 + sizeVacationTypeList + k).CellStyle = hStyle8;
                        }

                    }
                    else
                    {
                        if (attendanceSummaryInfo.IsUnSubmit)
                        {
                            GetOrCreateCell(rowTemp, 10 + sizeVacationTypeList + k).CellStyle = hStyle7_D;
                        }
                        else
                        {
                            GetOrCreateCell(rowTemp, 10 + sizeVacationTypeList + k).CellStyle = hStyle7;
                        }
                        rowTemp1 = sheet.GetRow(rowStart + 8 + (i * 4) + 1);
                        if (attendanceSummaryInfo.IsUnSubmit)
                        {
                            GetOrCreateCell(rowTemp1, 10 + sizeVacationTypeList + k).CellStyle = hStyle7_D;
                        }
                        else
                        {
                            GetOrCreateCell(rowTemp1, 10 + sizeVacationTypeList + k).CellStyle = hStyle7;
                        }
                    }

                    RegionUtil.SetBorderBottom((int)BorderStyle.Thin, new CellRangeAddress(8 + (i * 4), 9 + (i * 4), 10 + sizeVacationTypeList + k, 10 + sizeVacationTypeList + k), sheet, wb);
                }
            }
        }

        /// <summary>
        /// setFontCellExcel 
        /// </summary>
        /// <param name="wb"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private IFont setFontCellExcel(IWorkbook wb, short fontSize)
        {
            IFont font = (IFont)wb.CreateFont();
            font.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
            font.Boldweight = fontSize;
            font.FontHeightInPoints = fontSize;
            font.FontName = "メイリオ";
            return font;
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
        private void DesignMerged(IWorkbook wb, ISheet sheet, int firtRow, int lastRow, int FirtCol, int lastCol, Boolean borderRight, Boolean borderLeft, Boolean borderTop, Boolean borderButtom, Boolean isThinRight, Boolean isThinLeft, Boolean isThinTop, Boolean isThinBottom)
        {

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
                if (isThinBottom)
                {

                    RegionUtil.SetBorderBottom((int)BorderStyle.Thin, new CellRangeAddress(firtRow, lastRow, FirtCol, lastCol), sheet, wb);
                }
                else
                {

                    RegionUtil.SetBorderBottom((int)BorderStyle.Hair, new CellRangeAddress(firtRow, lastRow, FirtCol, lastCol), sheet, wb);
                }

            }

            sheet.AddMergedRegion(new CellRangeAddress(firtRow, lastRow, FirtCol, lastCol));
        }
    }
}
