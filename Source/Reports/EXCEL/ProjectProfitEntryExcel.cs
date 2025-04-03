using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using OMS.Models;
using OMS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMS.Reports.EXCEL
{
    public class ProjectProfitEntryExcel : BaseExcel
    {
        #region Variable
        /// <summary>
        /// 
        /// </summary>
        public ProjectProfitInfo headerInfo;
        /// <summary>
        /// 
        /// </summary>
        public List<ProjectProfitUserDetailInfo> listUserDetail;
        /// <summary>
        /// 
        /// </summary>
        public List<ProjectProfitDateDetailInfo> listExpenseDetail;
        #endregion
        /// <summary>
        /// Output Excel
        /// </summary>
        /// <returns></returns>
        public IWorkbook OutputExcel()
        {
            IWorkbook wb = null;

            //Create Sheet
            wb = this.CreateWorkbook("採算管理プロジェクト明細", ".xlsx");

            // Get Sheet
            ISheet sheet = wb.GetSheet("採算管理プロジェクト明細");

            //Fill data
            this.FillData(wb, sheet, headerInfo, listUserDetail, listExpenseDetail);

            return wb;
        }

        /// <summary>
        /// setFontCellExcel 
        /// </summary>
        /// <param name="wb"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private IFont setFontCellExcel(IWorkbook wb, short fontSize, bool isBold)
        {
            IFont font = (IFont)wb.CreateFont();
            font.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
            font.Boldweight = fontSize;
            font.IsBold = isBold;
            font.FontHeightInPoints = fontSize;
            font.FontName = "メイリオ";
            return font;
        }
        private void SetborderRange(CellRangeAddress Range, IWorkbook wb, ISheet sheet, BorderStyle styleLeft, BorderStyle styleRight, BorderStyle styleTop, BorderStyle stylebottom)
        {
            RegionUtil.SetBorderLeft((int)styleLeft, Range, sheet, wb);
            RegionUtil.SetBorderRight((int)styleRight, Range, sheet, wb);
            RegionUtil.SetBorderTop((int)styleTop, Range, sheet, wb);
            RegionUtil.SetBorderBottom((int)stylebottom, Range, sheet, wb);
        }
        /// <summary>
        /// Fill data on excel
        /// </summary>
        /// <param name="sheet">ISheet</param>
        /// <param name="lstData">Data list  of billing</param>
        private void FillData(IWorkbook wb, ISheet sheet, ProjectProfitInfo header, List<ProjectProfitUserDetailInfo> lstUser, List<ProjectProfitDateDetailInfo> lstExpense)
        {
            byte[] rgb = new byte[3] { 221, 235, 247 };
            //XSSFWorkbook xSSFWorkbook = new XSSFWorkbook();
            var numberFormat = wb.CreateDataFormat().GetFormat("#,##0");

            XSSFCellStyle hStyleTitleDetail = (XSSFCellStyle)wb.CreateCellStyle();
            hStyleTitleDetail.BorderTop = BorderStyle.None;
            hStyleTitleDetail.BorderBottom = BorderStyle.None;
            hStyleTitleDetail.BorderLeft = BorderStyle.None;
            hStyleTitleDetail.BorderRight = BorderStyle.None;
            hStyleTitleDetail.Alignment = HorizontalAlignment.Left;
            hStyleTitleDetail.VerticalAlignment = VerticalAlignment.Center;
            //hStyleTitleDetail.DataFormat = xSSFWorkbook.CreateDataFormat().GetFormat("@");
            hStyleTitleDetail.SetFont(setFontCellExcel(wb, 11, false));

            XSSFCellStyle hStyleHeaderFooterTable = (XSSFCellStyle)wb.CreateCellStyle();
            hStyleHeaderFooterTable.FillPattern = FillPattern.SolidForeground;
            hStyleHeaderFooterTable.SetFillForegroundColor(new XSSFColor(rgb));
            hStyleHeaderFooterTable.BorderTop = BorderStyle.Thin;
            hStyleHeaderFooterTable.BorderBottom = BorderStyle.Thin;
            hStyleHeaderFooterTable.BorderLeft = BorderStyle.Thin;
            hStyleHeaderFooterTable.BorderRight = BorderStyle.Thin;
            hStyleHeaderFooterTable.Alignment = HorizontalAlignment.Center;
            hStyleHeaderFooterTable.VerticalAlignment = VerticalAlignment.Center;
            //hStyleHeaderFooterTable.DataFormat = xSSFWorkbook.CreateDataFormat().GetFormat("@");
            hStyleHeaderFooterTable.SetFont(setFontCellExcel(wb, 11, false));

            XSSFCellStyle hStyleHeaderFooterTableAlignLeft = (XSSFCellStyle)wb.CreateCellStyle();
            hStyleHeaderFooterTableAlignLeft.FillPattern = FillPattern.SolidForeground;
            hStyleHeaderFooterTableAlignLeft.SetFillForegroundColor(new XSSFColor(rgb));
            hStyleHeaderFooterTableAlignLeft.BorderTop = BorderStyle.Thin;
            hStyleHeaderFooterTableAlignLeft.BorderBottom = BorderStyle.Thin;
            hStyleHeaderFooterTableAlignLeft.BorderLeft = BorderStyle.Thin;
            hStyleHeaderFooterTableAlignLeft.BorderRight = BorderStyle.Thin;
            hStyleHeaderFooterTableAlignLeft.Alignment = HorizontalAlignment.Left;
            hStyleHeaderFooterTableAlignLeft.VerticalAlignment = VerticalAlignment.Center;
            hStyleHeaderFooterTableAlignLeft.SetFont(setFontCellExcel(wb, 11, false));

            XSSFCellStyle hStyleHeaderFooterTableAlignRight = (XSSFCellStyle)wb.CreateCellStyle();
            hStyleHeaderFooterTableAlignRight.FillPattern = FillPattern.SolidForeground;
            hStyleHeaderFooterTableAlignRight.SetFillForegroundColor(new XSSFColor(rgb));
            hStyleHeaderFooterTableAlignRight.BorderTop = BorderStyle.Thin;
            hStyleHeaderFooterTableAlignRight.BorderBottom = BorderStyle.Thin;
            hStyleHeaderFooterTableAlignRight.BorderLeft = BorderStyle.Thin;
            hStyleHeaderFooterTableAlignRight.BorderRight = BorderStyle.Thin;
            hStyleHeaderFooterTableAlignRight.Alignment = HorizontalAlignment.Right;
            hStyleHeaderFooterTableAlignRight.VerticalAlignment = VerticalAlignment.Center;
            hStyleHeaderFooterTableAlignRight.SetFont(setFontCellExcel(wb, 11, false));

            XSSFCellStyle hStyleCellTable = (XSSFCellStyle)wb.CreateCellStyle();
            hStyleCellTable.BorderTop = BorderStyle.Thin;
            hStyleCellTable.BorderBottom = BorderStyle.Thin;
            hStyleCellTable.BorderLeft = BorderStyle.Thin;
            hStyleCellTable.BorderRight = BorderStyle.Thin;
            hStyleCellTable.Alignment = HorizontalAlignment.Center;
            hStyleCellTable.VerticalAlignment = VerticalAlignment.Center;
            //hStyleCellTable.DataFormat = xSSFWorkbook.CreateDataFormat().GetFormat("@");
            hStyleCellTable.SetFont(setFontCellExcel(wb, 11, false));

            XSSFCellStyle hStyleCellTableAlignLeft = (XSSFCellStyle)wb.CreateCellStyle();
            hStyleCellTableAlignLeft.BorderTop = BorderStyle.Thin;
            hStyleCellTableAlignLeft.BorderBottom = BorderStyle.Thin;
            hStyleCellTableAlignLeft.BorderLeft = BorderStyle.Thin;
            hStyleCellTableAlignLeft.BorderRight = BorderStyle.Thin;
            hStyleCellTableAlignLeft.Alignment = HorizontalAlignment.Left;
            hStyleCellTableAlignLeft.VerticalAlignment = VerticalAlignment.Center;
            //hStyleCellTableAlignLeft.DataFormat = xSSFWorkbook.CreateDataFormat().GetFormat("@");
            hStyleCellTableAlignLeft.SetFont(setFontCellExcel(wb, 11, false));

            XSSFCellStyle hStyleCellTableAlignRight = (XSSFCellStyle)wb.CreateCellStyle();
            hStyleCellTableAlignRight.BorderTop = BorderStyle.Thin;
            hStyleCellTableAlignRight.BorderBottom = BorderStyle.Thin;
            hStyleCellTableAlignRight.BorderLeft = BorderStyle.Thin;
            hStyleCellTableAlignRight.BorderRight = BorderStyle.Thin;
            hStyleCellTableAlignRight.Alignment = HorizontalAlignment.Right;
            hStyleCellTableAlignRight.VerticalAlignment = VerticalAlignment.Center;
            //hStyleCellTableAlignRight.DataFormat = xSSFWorkbook.CreateDataFormat().GetFormat("@");
            hStyleCellTableAlignRight.SetFont(setFontCellExcel(wb, 11, false));

            if (header == null) return;
            //title
            IRow rowTitle1 = sheet.GetRow(2);
            //プロジェクトコード
            ICell CellProjectCd = rowTitle1.GetCell(0);
            CellProjectCd.SetCellValue(header.ProjectCD);
            //プロジェクト名
            ICell CellProjectNm = rowTitle1.GetCell(2);
            CellProjectNm.SetCellValue(header.ProjectName);
            //部門
            ICell CellDepartment = rowTitle1.GetCell(7);
            CellDepartment.SetCellValue(header.DepartmentName);
            //状況
            ICell CellStatus = rowTitle1.GetCell(9);
            CellStatus.SetCellValue(header.AcceptanceFlag.Equals(0) ? "仕掛" : "検収");
            IRow rowTitle2 = sheet.GetRow(3);
            //集計対象期間
            ICell CellTime = rowTitle2.GetCell(2);
            CellTime.SetCellValue(string.Format("{0:yyyy/MM/dd}　～　{1:yyyy/MM/dd}", header.SC_StartDate, header.SC_EndDate));

            //header
            IRow rowHeader = sheet.GetRow(6);
            //受注金額
            ICell CellOrderAmount = rowHeader.GetCell(0);
            CellOrderAmount.SetCellValue((double)header.OrderAmount);
            //利益率
            ICell CellProfitRate = rowHeader.GetCell(2);
            CellProfitRate.SetCellFormula("IF(OR(D7>A7, A7 = 0), 0 ,(A7 - D7)/A7)");
            //原価計
            ICell CellCostTotal = rowHeader.GetCell(3);
            CellCostTotal.SetCellFormula("F7+H7+J7");

            //直接費計
            ICell CellDirectCost = rowHeader.GetCell(5);
            CellDirectCost.SetCellFormula(string.Format("D{0} + F{0}", 10 + lstUser.Count));
            //間接費計
            ICell CellIndirectCost = rowHeader.GetCell(7);
            CellIndirectCost.SetCellFormula(string.Format("H{0}", 10 + lstUser.Count));
            //経費計
            ICell CellExpense = rowHeader.GetCell(9);
            CellExpense.SetCellFormula(string.Format("F{0}", 13 + lstUser.Count + lstExpense.Count));

            int rowStart = 10;
            //User
            for (var i = 0; i < lstUser.Count; i++)
            {
                CellRangeAddress RangeUserNm = new CellRangeAddress(rowStart, rowStart, 1, 2);
                sheet.AddMergedRegion(RangeUserNm);

                CellRangeAddress RangeDirectCost = new CellRangeAddress(rowStart, rowStart, 3, 4);
                sheet.AddMergedRegion(RangeDirectCost);

                CellRangeAddress RangeDirectCostAfter = new CellRangeAddress(rowStart, rowStart, 5, 6);
                sheet.AddMergedRegion(RangeDirectCostAfter);

                CellRangeAddress RangeIndirectCost = new CellRangeAddress(rowStart, rowStart, 7, 8);
                sheet.AddMergedRegion(RangeIndirectCost);

                CellRangeAddress RangeTotal = new CellRangeAddress(rowStart, rowStart, 9, 10);
                sheet.AddMergedRegion(RangeTotal);

                IRow rowTemp = sheet.CreateRow(rowStart);

                //社員コード
                ICell cellUserCd = rowTemp.CreateCell(0);
                cellUserCd.SetCellValue(lstUser[i].UserCD);
                cellUserCd.CellStyle = i.Equals(lstUser.Count - 1) ? hStyleHeaderFooterTableAlignLeft : hStyleCellTableAlignLeft;
                //社員名
                ICell cellUserNm = rowTemp.CreateCell(1);
                cellUserNm.SetCellValue(lstUser[i].UserName);
                this.SetborderRange(RangeUserNm, wb, sheet, BorderStyle.Thin, BorderStyle.Thin, BorderStyle.Thin, BorderStyle.Thin);
                cellUserNm.CellStyle = i.Equals(lstUser.Count - 1) ? hStyleHeaderFooterTable : hStyleCellTableAlignLeft;
                //直接費
                ICell cellDirectCost = rowTemp.CreateCell(3);
                if (lstUser[i].DirectCost != -1)
                {
                    if (i.Equals(lstUser.Count - 1))
                    {
                        cellDirectCost.SetCellFormula(string.Format("SUM(D{0}:D{1})", rowStart + 2 - lstUser.Count, rowStart));
                    }
                    else
                    {
                        cellDirectCost.SetCellValue((double)lstUser[i].DirectCost);
                    }
                }
                else
                {
                    cellDirectCost.SetCellValue(string.Empty);
                }
                this.SetborderRange(RangeDirectCost, wb, sheet, BorderStyle.Thin, BorderStyle.Thin, BorderStyle.Thin, BorderStyle.Thin);
                cellDirectCost.CellStyle = i.Equals(lstUser.Count - 1) ? hStyleHeaderFooterTableAlignRight : hStyleCellTableAlignRight;
                cellDirectCost.CellStyle.DataFormat = numberFormat;

                //直接費（時間外）
                ICell cellDirectCostAfter = rowTemp.CreateCell(5);
                if (lstUser[i].DirectCostAfter != -1)
                {
                    if (i.Equals(lstUser.Count - 1))
                    {
                        cellDirectCostAfter.SetCellFormula(string.Format("SUM(F{0}:F{1})", rowStart + 2 - lstUser.Count, rowStart));
                    }
                    else
                    {
                        cellDirectCostAfter.SetCellValue((double)lstUser[i].DirectCostAfter);
                    }
                }
                else
                {
                    cellDirectCostAfter.SetCellValue(string.Empty);
                }
                this.SetborderRange(RangeDirectCostAfter, wb, sheet, BorderStyle.Thin, BorderStyle.Thin, BorderStyle.Thin, BorderStyle.Thin);
                cellDirectCostAfter.CellStyle = i.Equals(lstUser.Count - 1) ? hStyleHeaderFooterTableAlignRight : hStyleCellTableAlignRight;
                cellDirectCostAfter.CellStyle.DataFormat = numberFormat;

                //間接費
                ICell cellIndirectCost = rowTemp.CreateCell(7);
                if (lstUser[i].IndirectCosts != -1)
                {
                    if (i.Equals(lstUser.Count - 1))
                    {
                        cellIndirectCost.SetCellFormula(string.Format("SUM(H{0}:H{1})", rowStart + 2 - lstUser.Count, rowStart));
                    }
                    else
                    {
                        cellIndirectCost.SetCellValue((double)lstUser[i].IndirectCosts);
                    }
                }
                else
                {
                    cellIndirectCost.SetCellValue(string.Empty);
                }
                this.SetborderRange(RangeIndirectCost, wb, sheet, BorderStyle.Thin, BorderStyle.Thin, BorderStyle.Thin, BorderStyle.Thin);
                cellIndirectCost.CellStyle = i.Equals(lstUser.Count - 1) ? hStyleHeaderFooterTableAlignRight : hStyleCellTableAlignRight;
                cellIndirectCost.CellStyle.DataFormat = numberFormat;

                //計
                ICell cellTotal = rowTemp.CreateCell(9);
                if (lstUser[i].Total != -1 || i.Equals(lstUser.Count - 1))
                {
                    cellTotal.SetCellFormula(string.Format("D{0}+F{0}+H{0}", rowStart + 1));
                }
                this.SetborderRange(RangeTotal, wb, sheet, BorderStyle.Thin, BorderStyle.Thin, BorderStyle.Thin, BorderStyle.Thin);
                cellTotal.CellStyle = i.Equals(lstUser.Count - 1) ? hStyleHeaderFooterTableAlignRight : hStyleCellTableAlignRight;
                cellTotal.CellStyle.DataFormat = numberFormat;
                rowStart++;
            }

            //Expense
            rowStart += 1;//2 row blank
            IRow rowTitleExpense = sheet.CreateRow(rowStart);
            ICell cellTitleExpense = rowTitleExpense.CreateCell(0);
            cellTitleExpense.SetCellValue("経費一覧");
            cellTitleExpense.CellStyle = hStyleTitleDetail;
            rowStart++;
            CellRangeAddress RangeHDestinationName = new CellRangeAddress(rowStart, rowStart, 3, 4);
            sheet.AddMergedRegion(RangeHDestinationName);
            CellRangeAddress RangeHExpenceAmount = new CellRangeAddress(rowStart, rowStart, 5, 6);
            sheet.AddMergedRegion(RangeHExpenceAmount);
            CellRangeAddress RangeHMemo = new CellRangeAddress(rowStart, rowStart, 7, 9);
            sheet.AddMergedRegion(RangeHMemo);

            IRow rowHeaderExpense = sheet.CreateRow(rowStart);
            //年月日
            ICell HcellDate = rowHeaderExpense.CreateCell(0);
            HcellDate.SetCellValue("利用日");
            HcellDate.CellStyle = hStyleHeaderFooterTableAlignLeft;
            //登録者
            ICell HcellRegisterPersonName = rowHeaderExpense.CreateCell(1);
            HcellRegisterPersonName.SetCellValue("支払先（社員）");
            HcellRegisterPersonName.CellStyle = hStyleHeaderFooterTableAlignLeft;
            //経費種目
            ICell HcellType = rowHeaderExpense.CreateCell(2);
            HcellType.SetCellValue("経費種目");
            HcellType.CellStyle = hStyleHeaderFooterTableAlignLeft;
            //支払先
            ICell HcellDestinationName = rowHeaderExpense.CreateCell(3);
            HcellDestinationName.SetCellValue("支払先");
            this.SetborderRange(RangeHDestinationName, wb, sheet, BorderStyle.Thin, BorderStyle.Thin, BorderStyle.Thin, BorderStyle.Thin);
            HcellDestinationName.CellStyle = hStyleHeaderFooterTable;
            //金額
            ICell HcellExpenceAmount = rowHeaderExpense.CreateCell(5);
            HcellExpenceAmount.SetCellValue("金額");
            this.SetborderRange(RangeHExpenceAmount, wb, sheet, BorderStyle.Thin, BorderStyle.Thin, BorderStyle.Thin, BorderStyle.Thin);
            HcellExpenceAmount.CellStyle = hStyleHeaderFooterTable;
            //目的・経路
            ICell HcellMemo = rowHeaderExpense.CreateCell(7);
            HcellMemo.SetCellValue("目的・経路");
            this.SetborderRange(RangeHMemo, wb, sheet, BorderStyle.Thin, BorderStyle.Thin, BorderStyle.Thin, BorderStyle.Thin);
            HcellMemo.CellStyle = hStyleHeaderFooterTableAlignLeft;
            //承認状況
            ICell HcellApproveStatus = rowHeaderExpense.CreateCell(10);
            HcellApproveStatus.SetCellValue("承認状況");
            HcellApproveStatus.CellStyle = hStyleHeaderFooterTableAlignLeft;

            rowStart++;
            //DetailTable Expense
            for (var i = 0; i < lstExpense.Count; i++)
            {
                CellRangeAddress RangeDestinationName = new CellRangeAddress(rowStart, rowStart, 3, 4);
                sheet.AddMergedRegion(RangeDestinationName);
                CellRangeAddress RangeExpenceAmount = new CellRangeAddress(rowStart, rowStart, 5, 6);
                sheet.AddMergedRegion(RangeExpenceAmount);

                IRow rowExpense = sheet.CreateRow(rowStart);
                if (!i.Equals(lstExpense.Count - 1))
                {
                    //年月日
                    ICell cellDate = rowExpense.CreateCell(0);
                    cellDate.SetCellValue(lstExpense[i].DateStr);
                    cellDate.CellStyle = hStyleCellTableAlignLeft;
                    //登録者
                    ICell cellRegisterPersonName = rowExpense.CreateCell(1);
                    cellRegisterPersonName.SetCellValue(lstExpense[i].RegisterPersonName);
                    cellRegisterPersonName.CellStyle = hStyleCellTable;
                    //経費種目
                    ICell cellType = rowExpense.CreateCell(2);
                    cellType.SetCellValue(lstExpense[i].Type);
                    cellType.CellStyle = hStyleCellTable;
                }
                else
                {
                    CellRangeAddress RangeBlankTotal = new CellRangeAddress(rowStart, rowStart, 0, 2);
                    sheet.AddMergedRegion(RangeBlankTotal);
                    //支払先
                    ICell cellBlankTotal = rowExpense.CreateCell(0);
                    cellBlankTotal.SetCellValue(string.Empty);
                    this.SetborderRange(RangeBlankTotal, wb, sheet, BorderStyle.Thin, BorderStyle.Thin, BorderStyle.Thin, BorderStyle.Thin);
                    cellBlankTotal.CellStyle = hStyleHeaderFooterTable;
                }

                //支払先
                ICell cellDestinationName = rowExpense.CreateCell(3);
                cellDestinationName.SetCellValue(lstExpense[i].DestinationName);
                this.SetborderRange(RangeDestinationName, wb, sheet, BorderStyle.Thin, BorderStyle.Thin, BorderStyle.Thin, BorderStyle.Thin);
                cellDestinationName.CellStyle = i.Equals(lstExpense.Count - 1) ? hStyleHeaderFooterTable : hStyleCellTableAlignLeft;

                //金額
                ICell cellExpenceAmount = rowExpense.CreateCell(5);
                if (lstExpense[i].ExpenceAmount != -1)
                {
                    if (i.Equals(lstExpense.Count - 1))
                    {
                        cellExpenceAmount.SetCellFormula(string.Format("SUM(F{0}:F{1})", rowStart + 2 - lstExpense.Count, rowStart));
                    }
                    else
                    {
                        cellExpenceAmount.SetCellValue((double)lstExpense[i].ExpenceAmount);
                    }
                }
                else
                {
                    cellExpenceAmount.SetCellValue(string.Empty);
                }
                this.SetborderRange(RangeExpenceAmount, wb, sheet, BorderStyle.Thin, BorderStyle.Thin, BorderStyle.Thin, BorderStyle.Thin);
                cellExpenceAmount.CellStyle = i.Equals(lstExpense.Count - 1) ? hStyleHeaderFooterTableAlignRight : hStyleCellTableAlignRight;
                cellExpenceAmount.CellStyle.DataFormat = numberFormat;

                if (!i.Equals(lstExpense.Count - 1))
                {
                    //目的・経路
                    CellRangeAddress RangeMemo = new CellRangeAddress(rowStart, rowStart, 7, 9);
                    sheet.AddMergedRegion(RangeMemo);

                    ICell cellMemo = rowExpense.CreateCell(7);
                    cellMemo.SetCellValue(lstExpense[i].Memo);
                    this.SetborderRange(RangeMemo, wb, sheet, BorderStyle.Thin, BorderStyle.Thin, BorderStyle.Thin, BorderStyle.Thin);
                    cellMemo.CellStyle = i.Equals(lstExpense.Count - 1) ? hStyleHeaderFooterTable : hStyleCellTableAlignLeft;

                    //承認状況
                    ICell cellApproveFlag = rowExpense.CreateCell(10);
                    switch (lstExpense[i].ApprovedFlag)
                    {
                        case "0":
                            cellApproveFlag.SetCellValue("申請中");
                            break;
                        case "1":
                            cellApproveFlag.SetCellValue("承認済");
                            break;
                        default:
                            break;
                    }
                    cellApproveFlag.CellStyle = hStyleCellTable;
                }
                else
                {
                    CellRangeAddress RangeBlankTotal = new CellRangeAddress(rowStart, rowStart, 7, 10);
                    sheet.AddMergedRegion(RangeBlankTotal);
                    ////支払先
                    ICell cellBlankTotal = rowExpense.CreateCell(7);
                    cellBlankTotal.SetCellValue(string.Empty);
                    this.SetborderRange(RangeBlankTotal, wb, sheet, BorderStyle.Thin, BorderStyle.Thin, BorderStyle.Thin, BorderStyle.Thin);
                    cellBlankTotal.CellStyle = hStyleHeaderFooterTable;
                }

                rowStart++;
            }

            //var asd = wb.GetCreationHelper().CreateFormulaEvaluator();
            //asd.EvaluateAll();
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
        private void DesignMerged(IWorkbook wb, ISheet sheet, int firtRow, int lastRow, int FirtCol, int lastCol, bool borderRight, bool borderLeft, bool borderTop, bool borderButtom, bool isThinRight, bool isThinLeft, bool isThinTop, bool isThinButtom)
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

    }
}
