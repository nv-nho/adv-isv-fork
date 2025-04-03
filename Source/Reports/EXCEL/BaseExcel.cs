using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using OMS.Utilities;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace OMS.Reports.EXCEL
{
    public class BaseExcel
    {
        #region Constant
        protected DateTime DATE_TIME_DEFAULT = new DateTime(1900, 1, 1);
        public const string SPACE_EN = " ";
        public const string SPACE_WITH_POINT = "......";
        public const string SPACE_WITH_POINT_DATE = "....................................";
        #endregion

        /// <summary>
        /// Create Workbook
        /// </summary>
        /// <param name="fileName">File Name</param>
        /// <returns>Workbook</returns>
        protected IWorkbook CreateWorkbook(string fileName, string extension = ".xls", bool isIsvExcel = false)
        {
            if (!isIsvExcel)
            {
                if (extension.ToLower() == ".xls")
                {
                    using (FileStream temp = new FileStream(HttpContext.Current.Server.MapPath("~") + "/TemplateExcel/" + fileName + ".xls", FileMode.Open, FileAccess.Read))
                    {
                        return new HSSFWorkbook(temp);
                    }
                }
                else
                {
                    using (FileStream temp = new FileStream(HttpContext.Current.Server.MapPath("~") + "/TemplateExcel/" + fileName + ".xlsx", FileMode.Open, FileAccess.Read))
                    {
                        return new XSSFWorkbook(temp);
                    }
                }
            }
            else
            {
                if (extension.ToLower() == ".xls")
                {
                    using (FileStream temp = new FileStream(HttpContext.Current.Server.MapPath("~") + "/TemplateExcel_ISV/" + fileName + ".xls", FileMode.Open, FileAccess.Read))
                    {
                        return new HSSFWorkbook(temp);
                    }
                }
                else
                {
                    using (FileStream temp = new FileStream(HttpContext.Current.Server.MapPath("~") + "/TemplateExcel_ISV/" + fileName + ".xlsx", FileMode.Open, FileAccess.Read))
                    {
                        return new XSSFWorkbook(temp);
                    }
                }
            }
        }

        /// <summary>
        /// Copy Row
        /// </summary>
        /// <param name="workBook">IWorkbook</param>
        /// <param name="workSheet">ISheet</param>
        /// <param name="sourceRowNum">SourceRowNum</param>
        /// <param name="destinationRowNum">DestinationRowNum</param>
        protected void CopyRow(IWorkbook workBook, ISheet workSheet, int sourceRowNum, int destinationRowNum)
        {

            // Get the source / new row
            IRow newRow = workSheet.GetRow(destinationRowNum);
            IRow sourceRow = workSheet.GetRow(sourceRowNum);

            // If the row exist in destination, push down all rows by 1 else create a new row
            if (newRow != null)
            {
                workSheet.ShiftRows(destinationRowNum, workSheet.LastRowNum, 1, true, true);
            }
            else
            {
                newRow = workSheet.CreateRow(destinationRowNum);
            }

            // Loop through source columns to add to new row
            for (int i = 0; i < sourceRow.LastCellNum; i++)
            {
                // Grab a copy of the old/new cell
                ICell oldCell = sourceRow.GetCell(i);
                ICell newCell = newRow.CreateCell(i);

                // If the old cell is null jump to next cell
                if (oldCell == null)
                {
                    newCell = null;
                    continue;
                }

                // Copy style from old cell and apply to new cell
                //ICellStyle newCellStyle = workbook.CreateCellStyle();
                //newCellStyle.CloneStyleFrom(oldCell.CellStyle); ;
                newCell.CellStyle = oldCell.CellStyle;

                // If there is a cell comment, copy
                if (newCell.CellComment != null) newCell.CellComment = oldCell.CellComment;

                // If there is a cell hyperlink, copy
                if (oldCell.Hyperlink != null) newCell.Hyperlink = oldCell.Hyperlink;

                // Set the cell data type
                newCell.SetCellType(oldCell.CellType);

                // Set the cell data value
                switch (oldCell.CellType)
                {
                    case CellType.Blank:
                        newCell.SetCellValue(oldCell.StringCellValue);
                        break;
                    case CellType.Boolean:
                        newCell.SetCellValue(oldCell.BooleanCellValue);
                        break;
                    case CellType.Error:
                        newCell.SetCellErrorValue(oldCell.ErrorCellValue);
                        break;
                    case CellType.Formula:
                        newCell.SetCellFormula(oldCell.CellFormula);
                        break;
                    case CellType.Numeric:
                        newCell.SetCellValue(oldCell.NumericCellValue);
                        break;
                    case CellType.String:
                        newCell.SetCellValue(oldCell.RichStringCellValue);
                        break;
                    case CellType.Unknown:
                        newCell.SetCellValue(oldCell.StringCellValue);
                        break;
                }
            }

            // If there are are any merged regions in the source row, copy to new row
            for (int i = 0; i < workSheet.NumMergedRegions; i++)
            {
                CellRangeAddress cellRangeAddress = workSheet.GetMergedRegion(i);
                if (cellRangeAddress.FirstRow == sourceRow.RowNum)
                {
                    CellRangeAddress newCellRangeAddress = new CellRangeAddress(newRow.RowNum,
                                                                                (newRow.RowNum +
                                                                                 (cellRangeAddress.FirstRow -
                                                                                  cellRangeAddress.LastRow)),
                                                                                cellRangeAddress.FirstColumn,
                                                                                cellRangeAddress.LastColumn);
                    workSheet.AddMergedRegion(newCellRangeAddress);
                }
            }

        }

        /// <summary>
        /// Get Or create cell
        /// </summary>
        /// <param name="rowTemp"></param>
        /// <param name="colIndex"></param>
        /// <returns></returns>
        protected ICell GetOrCreateCell(IRow rowTemp, int colIndex)
        {

            ICell newCell = rowTemp.GetCell(colIndex);
            if (newCell == null)
            {
                return rowTemp.CreateCell(colIndex);
            }
            else
            {
                return newCell;
            }
        }

        /// <summary>
        /// Get Or create row
        /// </summary>
        /// <param name="rowTemp"></param>
        /// <param name="colIndex"></param>
        /// <returns></returns>
        protected IRow GetOrCreateRow(ISheet sheet, int rowIndex)
        {

            IRow newRow = sheet.GetRow(rowIndex);
            if (newRow == null)
            {
                return sheet.CreateRow(rowIndex);
            }
            else
            {
                return newRow;
            }
        }

        /// <summary>
        /// Set Value Index
        /// </summary>
        /// <param name="wb">Workbook</param>
        /// <param name="sheet">sheet</param>
        /// <param name="keyMap">keyMap</param>
        /// <param name="value">value</param>
        /// <param name="replaceStr">replaceStr</param>
        /// <param name="fomular">fomular</param>
        /// <param name="replaceAll">replaceAll</param>
        protected void SetValueIndex(IWorkbook wb, ISheet sheet, string keyMap, Object value, string replaceStr = "", bool fomular = false, bool replaceAll = false)
        {
            IName range = wb.GetName(keyMap);
            if (range != null && (!range.RefersToFormula.Equals(string.Format("'{0}'!#REF!", sheet.SheetName))
                && !range.RefersToFormula.Equals(string.Format("{0}!#REF!", sheet.SheetName))))
            {
                CellReference cellRef = new CellReference(range.RefersToFormula);
                IRow row = sheet.GetRow(cellRef.Row);
                ICell cell = row.GetCell(cellRef.Col);
                var findCellString = cell.StringCellValue;

                var cellString = string.Empty;

                if (value != null)
                {
                    cellString = value.ToString();
                }

                if (string.IsNullOrEmpty(findCellString))
                {
                    cell.SetCellValue(cellString);
                }
                else
                {
                    if (!string.IsNullOrEmpty(replaceStr) && findCellString.IndexOf(replaceStr) >= 0)
                    {
                        if (replaceAll)
                        {
                            findCellString = findCellString.Replace(replaceStr, cellString);
                        }
                        else
                        {
                            var endPoint = findCellString.IndexOf(replaceStr);
                            var tempStr1 = findCellString.Substring(0, endPoint + replaceStr.Length);
                            var tempStr2 = tempStr1.Replace(replaceStr, cellString);
                            findCellString = findCellString.Replace(tempStr1, tempStr2);
                        }
                        cell.SetCellValue(findCellString);
                    }
                }
            }
        }

        /// <summary>
        /// Get Day String
        /// </summary>
        /// <param name="value">Date</param>
        /// <returns></returns>
        protected string GetDayString(DateTime value)
        {
            switch (value.Day)
            {
                case 1:
                case 21:
                case 31:
                    return value.Day.ToString("00") + "st";
                case 2:
                case 22:
                    return value.Day.ToString("00") + "nd";
                case 3:
                case 23:
                    return value.Day.ToString("00") + "rd";
                default:
                    return value.Day.ToString("00") + "th";
            }
        }

        /// <summary>
        /// Get Month String
        /// </summary>
        /// <param name="value">Date</param>
        /// <returns></returns>
        protected string GetMonthString(DateTime value)
        {
            switch (value.Month)
            {
                case 1:
                    return "January";
                case 2:
                    return "Feburary";
                case 3:
                    return "March";
                case 4:
                    return "April ";
                case 5:
                    return "May";
                case 6:
                    return "June ";
                case 7:
                    return "July";
                case 8:
                    return "August";
                case 9:
                    return "September";
                case 10:
                    return "October";
                case 11:
                    return "November";
                default:
                    return "December";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DateValue"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        protected string GetDateString(DateTime DateValue, Language language)
        {
            if (language == Language.Vietnam)
            {
                dynamic StrDate = string.Format("ngày {0} tháng {1} năm {2}", DateValue.Day.ToString("00"), DateValue.Month.ToString("00"), DateValue.Year.ToString("0000"));
                return StrDate;
            }
            else if (language == Language.Japan)
            {
                dynamic StrDate = string.Format("{0}年月{1}日{2}", DateValue.Year.ToString("0000"), DateValue.Month.ToString("00"), DateValue.Day.ToString("00"));//DateValue.ToString("yyyy年MM月dd日");
                return StrDate;
            }
            else
            {
                dynamic SuffixDay = string.Empty;
                dynamic SuffixMoth = string.Empty;

                switch (DateValue.Day)
                {
                    case 1:
                    case 21:
                    case 31:
                        SuffixDay = "st";
                        break;
                    case 2:
                    case 22:
                        SuffixDay = "nd";
                        break;
                    case 3:
                    case 23:
                        SuffixDay = "rd";
                        break;
                    default:
                        SuffixDay = "th";
                        break;
                }
                switch (DateValue.Month)
                {
                    case 1:
                        SuffixMoth = "January";
                        break;
                    case 2:
                        SuffixMoth = "Feburary";
                        break;
                    case 3:
                        SuffixMoth = "March";
                        break;
                    case 4:
                        SuffixMoth = "April ";
                        break;
                    case 5:
                        SuffixMoth = "May";
                        break;
                    case 6:
                        SuffixMoth = "June ";
                        break;
                    case 7:
                        SuffixMoth = "July";
                        break;
                    case 8:
                        SuffixMoth = "August";
                        break;
                    case 9:
                        SuffixMoth = "September";
                        break;
                    case 10:
                        SuffixMoth = "October";
                        break;
                    case 11:
                        SuffixMoth = "November";
                        break;
                    case 12:
                        SuffixMoth = "December";
                        break;
                }
                return string.Format("{0} {1}{2}, {3}", SuffixMoth, DateValue.Day.ToString("00"), SuffixDay, DateValue.Year.ToString("0000"));
            }
        }

        /// <summary>
        /// Get Row Height
        /// </summary>
        /// <param name="enterList">enterList</param>
        /// <param name="rowCount">rowCount</param>
        /// <param name="length">length</param>
        /// <returns>row Height</returns>
        protected short GetHeight(string[] enterList, int rowCount, int length, int defaultRowHeight)
        {
            foreach (var item in enterList)
            {
                if (item.Length > length)
                {
                    int _temp = 0;
                    _temp = (int)Math.Truncate((double)item.Length / length);
                    rowCount += _temp;
                }
            }
            return (short)(rowCount * defaultRowHeight);
        }
    }
}
