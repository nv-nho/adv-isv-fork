using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMS.Reports.EXCEL
{
    public class ExpenceListExcel : BaseExcel
    {
        #region Variable
        /// <summary>
        /// 
        /// </summary>
        public IList<ExpenceInfo> modelInput;
        #endregion
        /// <summary>
        /// Output Excel
        /// </summary>
        /// <returns></returns>
        public IWorkbook OutputExcel()
        {
            IWorkbook wb = null;

            //Create Sheet
            wb = this.CreateWorkbook("経費一覧", ".xlsx");

            // Get Sheet
            ISheet sheet = wb.GetSheet("経費一覧");

            //Fill data
            this.FillData(wb, sheet, modelInput);

            return wb;
        }
        /// <summary>
        /// Fill data on excel
        /// </summary>
        /// <param name="sheet">ISheet</param>
        /// <param name="lstData">Data list  of billing</param>
        private void FillData(IWorkbook wb, ISheet sheet, IList<ExpenceInfo> ExpenceInfo)
        {
            XSSFWorkbook xSSFWorkbook = new XSSFWorkbook();

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
            
            int rowStart = 1;
            if (ExpenceInfo == null) return;
            foreach (var ExpenceItem in ExpenceInfo)
            {
                IRow rowTemp = sheet.CreateRow(rowStart);
                ICell cellRowNum = rowTemp.CreateCell(0);
                cellRowNum.CellStyle = hStyNormal;
                cellRowNum.SetCellValue(rowStart);

                //Create cell ExpenceNo
                ICell cellExpenceNo = rowTemp.CreateCell(1);
                cellExpenceNo.CellStyle = hStyNormal;
                cellExpenceNo.SetCellValue(ExpenceItem.ExpenceNo);

                //Create cell Date
                ICell cellDate = rowTemp.CreateCell(2);
                cellDate.CellStyle = hStyNormal;
                cellDate.SetCellValue(ExpenceItem.Date.ToString("yyyy/MM/dd"));

                //Create cell AccountCD
                ICell cellAccount = rowTemp.CreateCell(3);
                cellAccount.CellStyle = hStyNormal;
                cellAccount.SetCellValue(ExpenceItem.Value2);

                //Create cell DepartmentName
                ICell cellDepartment = rowTemp.CreateCell(4);
                cellDepartment.CellStyle = hStyNormal;
                cellDepartment.SetCellValue(ExpenceItem.DepartmentName);

                //Create cell ProjectName
                ICell cellProject = rowTemp.CreateCell(5);
                cellProject.CellStyle = hStyNormal;
                cellProject.SetCellValue(ExpenceItem.ProjectName);

                //Create cell ExpenceAmount
                ICell cellExpenceAmount = rowTemp.CreateCell(6);
                cellExpenceAmount.CellStyle = hStyExpenceAmount;
                cellExpenceAmount.SetCellValue((double)ExpenceItem.ExpenceAmount);

                //Create cell UserName1
                ICell cellUser = rowTemp.CreateCell(7);
                cellUser.CellStyle = hStyNormal;
                cellUser.SetCellValue(ExpenceItem.UserName1);

                rowStart++;
            }
            
        }
    }
}
