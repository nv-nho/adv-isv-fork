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
    public class AttendanceSummaryExchangeDateExcel : BaseExcel
    {
        #region Variable
        /// <summary>
        /// Model Quotation Header Search
        /// </summary>
        public AttendanceSummaryExchangeDateExcelModal modelInput;
        #endregion

        /// <summary>
        /// Output Excel
        /// </summary>
        /// <returns></returns>
        public IWorkbook OutputExcel()
        {
            IWorkbook wb = null;

            //Create Sheet
            wb = this.CreateWorkbook("振替休日取得一覧表", ".xlsx");

            // Get Sheet
            ISheet sheet = wb.GetSheet("振休一覧表");
            wb.SetSheetName(0, "振休一覧表");

            //Fill data
            this.FillData(wb, sheet, modelInput);

            return wb;
        }

        /// <summary>
        /// Fill data on excel
        /// </summary>
        /// <param name="sheet">ISheet</param>
        /// <param name="lstData">Data list  of billing</param>
        private void FillData(IWorkbook wb, ISheet sheet, AttendanceSummaryExchangeDateExcelModal excelModel)
        {
            XSSFWorkbook hSSFWorkbook = new XSSFWorkbook();

            XSSFCellStyle csThinL = (XSSFCellStyle)wb.CreateCellStyle();
            csThinL.BorderLeft = BorderStyle.Thin;
            csThinL.BorderRight = BorderStyle.Hair;
            csThinL.BorderBottom = BorderStyle.Hair;
            csThinL.Alignment = HorizontalAlignment.Left;
            csThinL.VerticalAlignment = VerticalAlignment.Center;
            csThinL.DataFormat = hSSFWorkbook.CreateDataFormat().GetFormat("@");
            csThinL.SetFont(setFontCellExcel(wb, 10));

            XSSFCellStyle csHair = (XSSFCellStyle)wb.CreateCellStyle();
            csHair.BorderLeft = BorderStyle.Hair;
            csHair.BorderRight = BorderStyle.Hair;
            csHair.BorderBottom = BorderStyle.Hair;
            csHair.Alignment = HorizontalAlignment.Left;
            csHair.VerticalAlignment = VerticalAlignment.Center;
            csHair.SetFont(setFontCellExcel(wb, 10));

            XSSFCellStyle csHairAlignC = (XSSFCellStyle)wb.CreateCellStyle();
            csHairAlignC.BorderLeft = BorderStyle.Hair;
            csHairAlignC.BorderRight = BorderStyle.Hair;
            csHairAlignC.BorderBottom = BorderStyle.Hair;
            csHairAlignC.Alignment = HorizontalAlignment.Center;
            csHairAlignC.VerticalAlignment = VerticalAlignment.Center;
            csHairAlignC.SetFont(setFontCellExcel(wb, 10));

            XSSFCellStyle csThinR = (XSSFCellStyle)wb.CreateCellStyle();
            csThinR.BorderLeft = BorderStyle.Hair;
            csThinR.BorderBottom = BorderStyle.Hair;
            csThinR.BorderRight = BorderStyle.Thin;
            csThinR.Alignment = HorizontalAlignment.Left;
            csThinR.VerticalAlignment = VerticalAlignment.Center;
            csThinR.SetFont(setFontCellExcel(wb, 10));

            XSSFCellStyle csThinLB = (XSSFCellStyle)wb.CreateCellStyle();
            csThinLB.BorderLeft = BorderStyle.Thin;
            csThinLB.BorderRight = BorderStyle.Hair;
            csThinLB.BorderBottom = BorderStyle.Thin;
            csThinLB.Alignment = HorizontalAlignment.Left;
            csThinLB.VerticalAlignment = VerticalAlignment.Center;
            csThinLB.DataFormat = hSSFWorkbook.CreateDataFormat().GetFormat("@");
            csThinLB.SetFont(setFontCellExcel(wb, 10));

            XSSFCellStyle csHairThinB = (XSSFCellStyle)wb.CreateCellStyle();
            csHairThinB.BorderLeft = BorderStyle.Hair;
            csHairThinB.BorderRight = BorderStyle.Hair;
            csHairThinB.BorderBottom = BorderStyle.Thin;
            csHairThinB.Alignment = HorizontalAlignment.Left;
            csHairThinB.VerticalAlignment = VerticalAlignment.Center;
            csHairThinB.SetFont(setFontCellExcel(wb, 10));

            XSSFCellStyle csHairThinBAlignC = (XSSFCellStyle)wb.CreateCellStyle();
            csHairThinBAlignC.BorderLeft = BorderStyle.Hair;
            csHairThinBAlignC.BorderRight = BorderStyle.Hair;
            csHairThinBAlignC.BorderBottom = BorderStyle.Thin;
            csHairThinBAlignC.Alignment = HorizontalAlignment.Center;
            csHairThinBAlignC.VerticalAlignment = VerticalAlignment.Center;
            csHairThinBAlignC.SetFont(setFontCellExcel(wb, 10));

            XSSFCellStyle csThinRB = (XSSFCellStyle)wb.CreateCellStyle();
            csThinRB.BorderLeft = BorderStyle.Hair;
            csThinRB.BorderBottom = BorderStyle.Thin;
            csThinRB.BorderRight = BorderStyle.Thin;
            csThinRB.Alignment = HorizontalAlignment.Left;
            csThinRB.VerticalAlignment = VerticalAlignment.Center;
            csThinRB.SetFont(setFontCellExcel(wb, 10));

            // set header data
            IRow rowTemp = GetOrCreateRow(sheet, 1);

            GetOrCreateCell(rowTemp, 1).SetCellValue(excelModel.DepartmentNm);
            GetOrCreateCell(rowTemp, 6).SetCellValue(DateTime.Now);

            int rowStart = 3;
            string oldUserCd = string.Empty;
            for (int i = 0; i < excelModel.DataList.Count; i++)
            {
                ExchangeDateExcelModel data = excelModel.DataList[i];
                rowTemp = GetOrCreateRow(sheet, rowStart + i);

                //社員番号
                ICell UserCDCell = GetOrCreateCell(rowTemp, 0);
                if (oldUserCd != data.UserCD)
                {
                    UserCDCell.SetCellValue(data.UserCD);
                }
                if (i == excelModel.DataList.Count - 1 || data.UserCD != excelModel.DataList[i + 1].UserCD)
                {
                    UserCDCell.CellStyle = csThinLB;
                }
                else
                {
                    UserCDCell.CellStyle = csThinL;
                }

                //社員名
                ICell UserNameCell = GetOrCreateCell(rowTemp, 1);
                if (oldUserCd != data.UserCD)
                {
                    UserNameCell.SetCellValue(data.UserName);
                }
                if (i == excelModel.DataList.Count - 1 || data.UserCD != excelModel.DataList[i + 1].UserCD)
                {
                    UserNameCell.CellStyle = csHairThinB;
                }
                else
                {
                    UserNameCell.CellStyle = csHair;
                }

                //種類
                ICell typeNameCell = GetOrCreateCell(rowTemp, 2);
                typeNameCell.SetCellValue(data.WorkingTypeName);
                if (i == excelModel.DataList.Count - 1 || data.UserCD != excelModel.DataList[i + 1].UserCD)
                {
                    typeNameCell.CellStyle = csHairThinB;
                }
                else
                {
                    typeNameCell.CellStyle = csHair;
                }

                //種類
                ICell dateCell = GetOrCreateCell(rowTemp, 3);
                dateCell.SetCellValue(data.Date.ToString(Constants.FMT_DATE_EN));
                if (i == excelModel.DataList.Count - 1 || data.UserCD != excelModel.DataList[i + 1].UserCD)
                {
                    dateCell.CellStyle = csHairThinB;
                }
                else
                {
                    dateCell.CellStyle = csHair;
                }

                //開始
                ICell entryTimeCell = GetOrCreateCell(rowTemp, 4);
                entryTimeCell.SetCellValue(data.EntryTime);
                if (i == excelModel.DataList.Count - 1 || data.UserCD != excelModel.DataList[i + 1].UserCD)
                {
                    entryTimeCell.CellStyle = csHairThinBAlignC;
                }
                else
                {
                    entryTimeCell.CellStyle = csHairAlignC;
                }

                //終了
                ICell exitTimeCell = GetOrCreateCell(rowTemp, 5);
                exitTimeCell.SetCellValue(data.ExitTime);
                if (i == excelModel.DataList.Count - 1 || data.UserCD != excelModel.DataList[i + 1].UserCD)
                {
                    exitTimeCell.CellStyle = csHairThinBAlignC;
                }
                else
                {
                    exitTimeCell.CellStyle = csHairAlignC;
                }

                //振休取得日
                ICell usedDateCell = GetOrCreateCell(rowTemp, 6);
                if (data.UsedDate != null)
                {
                    usedDateCell.SetCellValue(data.UsedDate.Value.ToString(Constants.FMT_DATE_EN));
                }
                else
                {
                    usedDateCell.SetCellValue("未取得");
                }

                if (i == excelModel.DataList.Count - 1 || data.UserCD != excelModel.DataList[i + 1].UserCD)
                {
                    usedDateCell.CellStyle = csThinRB;
                }
                else
                {
                    usedDateCell.CellStyle = csThinR;
                }

                oldUserCd = data.UserCD;
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
    }
}
