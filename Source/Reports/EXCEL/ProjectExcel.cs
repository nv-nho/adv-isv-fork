using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OMS.Utilities;

namespace OMS.Reports.EXCEL
{
    public class ProjectExcel : BaseExcel
    {
        #region Variable
        /// <summary>
        /// 
        /// </summary>
        public IList<ProjectExcelInfo> ListProject { get; set; }
        public IList<M_User> ListUser { get; set; }

        private ICellStyle cellStyle;
        #endregion
        /// <summary>
        /// Output Excel
        /// </summary>
        /// <returns></returns>
        public IWorkbook OutputExcel()
        {
            IWorkbook wb = null;

            //Create Sheet
            wb = this.CreateWorkbook("オーダーマスタ", ".xls");


            IFont font = (IFont)wb.CreateFont();
            font.Boldweight = 10;
            font.FontHeightInPoints = 10;
            font.FontName = "ＭＳ Ｐゴシック";

            cellStyle = wb.CreateCellStyle();
            cellStyle.BorderTop = BorderStyle.Thin;
            cellStyle.BorderLeft = BorderStyle.Thin;
            cellStyle.BorderRight = BorderStyle.Thin;
            cellStyle.BorderBottom = BorderStyle.Hair;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cellStyle.SetFont(font);

            // Get Sheet
            ISheet sheet = wb.GetSheet("オーダーマスタ");

            //Fill data
            this.FillProjectData(wb, sheet, ListProject);


            sheet = wb.GetSheet("社員マスタ");

            //Fill data
            this.FillUserData(wb, sheet, ListUser);

            return wb;
        }

        /// <summary>
        /// Fill data on excel
        /// </summary>
        /// <param name="sheet">ISheet</param>
        /// <param name="lstData">Data list  of billing</param>
        private void FillProjectData(IWorkbook wb, ISheet sheet, IList<ProjectExcelInfo> lstData)
        {
            int rowStart = 1;
            foreach (var item in lstData)
            {
                IRow rowTemp = GetOrCreateRow(sheet, rowStart);
                ICell cell = GetOrCreateCell(rowTemp, 0);
                cell.SetCellValue(item.ProjectCD);
                cell.CellStyle = cellStyle;

                cell = GetOrCreateCell(rowTemp, 1);
                cell.SetCellValue(item.ProjectName);
                cell.CellStyle = cellStyle;

                cell = GetOrCreateCell(rowTemp, 3);
                cell.SetCellValue(item.Memo);
                cell.CellStyle = cellStyle;

                rowStart++;
            }
        }

        /// <summary>
        /// Fill data on excel
        /// </summary>
        /// <param name="sheet">ISheet</param>
        /// <param name="lstData">Data list  of billing</param>
        private void FillUserData(IWorkbook wb, ISheet sheet, IList<M_User> lstData)
        {
            int rowStart = 2;
            foreach (var item in lstData)
            {
                IRow rowTemp = GetOrCreateRow(sheet, rowStart);
                ICell cell = GetOrCreateCell(rowTemp, 0);
                cell.SetCellValue(EditDataUtil.ToFixCodeShow(item.UserCD, 3));
                cell.CellStyle = cellStyle;

                cell = GetOrCreateCell(rowTemp, 1);
                cell.SetCellValue(item.UserName1);
                cell.CellStyle = cellStyle;

                rowStart++;
            }
        }
    }
}
