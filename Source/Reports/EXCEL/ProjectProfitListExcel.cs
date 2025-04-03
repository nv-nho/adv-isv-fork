using NPOI.SS.UserModel;
using OMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.XSSF.UserModel;

namespace OMS.Reports.EXCEL
{
    public class ProjectProfitListExcel : BaseExcel
    {
        #region Variable
        /// <summary>
        /// 
        /// </summary>
        public List<ProjectProfitInfo> modelInput;
        #endregion
        /// <summary>
        /// Output Excel
        /// </summary>
        /// <returns></returns>
        public IWorkbook OutputExcel()
        {
            IWorkbook wb = null;

            //Create Sheet
            wb = this.CreateWorkbook("採算管理一覧表", ".xlsx");

            // Get Sheet
            ISheet sheet = wb.GetSheet("採算管理一覧表");

            //Fill data
            this.FillData(wb, sheet, modelInput);

            return wb;
        }
        /// <summary>
        /// Fill data on excel
        /// </summary>
        /// <param name="sheet">ISheet</param>
        /// <param name="lstData">Data list  of billing</param>
        private void FillData(IWorkbook wb, ISheet sheet, List<ProjectProfitInfo> ProjectProfitInfo)
        {

            XSSFCellStyle hStyNormal = (XSSFCellStyle)wb.CreateCellStyle();
            hStyNormal.BorderTop = BorderStyle.Thin;
            hStyNormal.BorderBottom = BorderStyle.Thin;
            hStyNormal.BorderLeft = BorderStyle.Thin;
            hStyNormal.BorderRight = BorderStyle.Thin;
            hStyNormal.Alignment = HorizontalAlignment.Left;
            hStyNormal.VerticalAlignment = VerticalAlignment.Center;

            XSSFCellStyle hStyExpenceAmount = (XSSFCellStyle)wb.CreateCellStyle();
            hStyExpenceAmount.BorderTop = BorderStyle.Thin;
            hStyExpenceAmount.BorderBottom = BorderStyle.Thin;
            hStyExpenceAmount.BorderLeft = BorderStyle.Thin;
            hStyExpenceAmount.BorderRight = BorderStyle.Thin;
            hStyExpenceAmount.Alignment = HorizontalAlignment.Right;
            hStyExpenceAmount.VerticalAlignment = VerticalAlignment.Center;
            hStyExpenceAmount.DataFormat = wb.CreateDataFormat().GetFormat("#,##0");

            XSSFCellStyle hStyPercent = (XSSFCellStyle)wb.CreateCellStyle();
            hStyPercent.BorderTop = BorderStyle.Thin;
            hStyPercent.BorderBottom = BorderStyle.Thin;
            hStyPercent.BorderLeft = BorderStyle.Thin;
            hStyPercent.BorderRight = BorderStyle.Thin;
            hStyPercent.Alignment = HorizontalAlignment.Left;
            hStyPercent.VerticalAlignment = VerticalAlignment.Center;
            hStyPercent.DataFormat = wb.CreateDataFormat().GetFormat("0.00%");

            int rowStart = 1;
            if (ProjectProfitInfo == null) return;
            foreach (var ProjectInfo in ProjectProfitInfo)
            {

                IRow rowTemp = sheet.CreateRow(rowStart);
                //Create cell RowNumber
                ICell cellRowNum = rowTemp.CreateCell(0);
                cellRowNum.CellStyle = hStyNormal;
                cellRowNum.SetCellValue(rowStart);

                //Create cell Project Code
                ICell cellProjectCode = rowTemp.CreateCell(1);
                cellProjectCode.CellStyle = hStyNormal;
                cellProjectCode.SetCellValue(ProjectInfo.ProjectCD);

                //Create cell ProjectName
                ICell cellProjectName = rowTemp.CreateCell(2);
                cellProjectName.CellStyle = hStyNormal;
                cellProjectName.SetCellValue(ProjectInfo.ProjectName);

                //Create cell DeparmentUser
                ICell cellDeparment = rowTemp.CreateCell(3);
                cellDeparment.CellStyle = hStyNormal;
                cellDeparment.SetCellValue(ProjectInfo.DepartmentName);

                ICell cellUser = rowTemp.CreateCell(4);
                cellUser.CellStyle = hStyNormal;
                cellUser.SetCellValue(ProjectInfo.UserName);

                //Create cell Date
                ICell cellStartDate = rowTemp.CreateCell(5);
                cellStartDate.CellStyle = hStyNormal;
                cellStartDate.SetCellValue(string.Format("{0:yyyy/MM/dd}", ProjectInfo.StartDate));

                //Create cell Date
                ICell cellEndDate = rowTemp.CreateCell(6);
                cellEndDate.CellStyle = hStyNormal;
                cellEndDate.SetCellValue(string.Format("{0:yyyy/MM/dd}", ProjectInfo.EndDate));

                //Create cell status
                ICell cellStatus = rowTemp.CreateCell(7);
                cellStatus.CellStyle = hStyNormal;
                cellStatus.SetCellValue(ProjectInfo.AcceptanceFlag.Equals(1) ? "検収" : "仕掛");

                //Create cell OrderAmount
                ICell cellOrderAmount = rowTemp.CreateCell(8);
                cellOrderAmount.CellStyle = hStyExpenceAmount;
                cellOrderAmount.SetCellValue((double)ProjectInfo.OrderAmount);

                //Create cell ProfitRate
                ICell cellProfitRate = rowTemp.CreateCell(9);
                cellProfitRate.CellStyle = hStyPercent;
                cellProfitRate.SetCellFormula(string.Format("(I{0} - K{0})/I{0}", rowStart + 1));

                //Create cell TotalCost
                ICell cellTotalCost = rowTemp.CreateCell(10);
                cellTotalCost.CellStyle = hStyExpenceAmount;
                cellTotalCost.SetCellFormula(String.Format("L{0} + M{0} + N{0}", rowStart + 1));
                 
                //Create cell DirectCost
                ICell cellDirectCost = rowTemp.CreateCell(11);
                cellDirectCost.CellStyle = hStyExpenceAmount;
                cellDirectCost.SetCellValue((double)ProjectInfo.DirectCost);

                //Create cell IndirectCost
                ICell cellIndirectCost = rowTemp.CreateCell(12);
                cellIndirectCost.CellStyle = hStyExpenceAmount;
                cellIndirectCost.SetCellValue((double)ProjectInfo.IndirectCosts);

                //Create cell Expense
                ICell cellExpense = rowTemp.CreateCell(13);
                cellExpense.CellStyle = hStyExpenceAmount;
                cellExpense.SetCellValue((double)ProjectInfo.Expense);

                rowStart++;
            }
        }
    }
}
