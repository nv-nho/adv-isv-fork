using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OMS.DAC;
using OMS.Models;
using OMS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMS.Reports.EXCEL
{
    public class ExpenceSeisanListExcel : BaseExcel
    {
        #region Variable
        /// <summary>
        /// 
        /// </summary>
        public IList<ExpenceSeisanExcel> modelInput;
        public bool isAccountingDate { get; set; }
        public string dateForm { get; set; }
        public string dateTo { get; set; }
        #endregion       

        /// <summary>
        /// Output Excel
        /// </summary>
        /// <returns></returns>
        public IWorkbook OutputExcel()
        {
            IWorkbook wb = null;

            //Create Sheet
            wb = this.CreateWorkbook("交通・宿泊費清算書", ".xlsx");

            // Get Sheet
            string sheetIndex = "Sheet";

            List<ExpenceSeisanExcel> lstUser = this.modelInput.GroupBy(item => item.UserCD).Select(grp => grp.First()).OrderBy(x => x.UserCD).ToList();

            int idx = 0;
            foreach (ExpenceSeisanExcel item in lstUser)
            {
                ISheet sheet = wb.GetSheet(sheetIndex);
                sheet.CopySheet(item.UserCD, true);
                sheet = wb.GetSheet(item.UserCD);

                // *昇順（小さい順）
                List<ExpenceSeisanExcel> lstDetail = this.modelInput.Where(itemE => itemE.UserCD == item.UserCD).OrderBy(x => x.Date).ThenBy(x => x.ExpenceNo).ToList();

                this.FillData(wb, sheet, lstDetail);

                idx++;
            }

            if (lstUser.Count > 0)
            {
                wb.RemoveSheetAt(0);
            }

            return wb;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="wb"></param>
        /// <param name="sheet"></param>
        /// <param name="ExpenceInfo"></param>
        private void FillData(IWorkbook wb, ISheet sheet, List<ExpenceSeisanExcel> lstItem)
        {
            try
            {
                if (lstItem == null) return;

                int sRow = 0;
                int hRow = 5;
                List<XSSFCellStyle> lstCellStyle = new List<XSSFCellStyle>();
                XSSFCellStyle _cellStyle_TJ = (XSSFCellStyle)wb.CreateCellStyle();
                _cellStyle_TJ.BorderRight = BorderStyle.Thin;
                _cellStyle_TJ.BorderTop = BorderStyle.Thin;
                _cellStyle_TJ.BorderBottom = BorderStyle.Thin;

                XSSFCellStyle _cellStyle_TI = (XSSFCellStyle)wb.CreateCellStyle();
                _cellStyle_TI.BorderTop = BorderStyle.Thin;
                _cellStyle_TI.BorderBottom = BorderStyle.Thin;
                _cellStyle_TI.BorderRight = BorderStyle.Hair;
                _cellStyle_TI.BorderLeft = BorderStyle.Hair;
                _cellStyle_TI.Alignment = HorizontalAlignment.Center;

                for (int i = 0; i < 10; i++)
                {
                    ICell oldCell = sheet.GetRow(hRow + 1).GetCell(i);

                    XSSFCellStyle newCellStyle = (XSSFCellStyle)wb.CreateCellStyle();
                    newCellStyle.CloneStyleFrom(oldCell.CellStyle);
                    lstCellStyle.Add(newCellStyle);

                    _cellStyle_TJ.SetFont(newCellStyle.GetFont());
                    _cellStyle_TJ.DataFormat = newCellStyle.DataFormat;

                    _cellStyle_TI.SetFont(newCellStyle.GetFont());
                }

                // ---- HEAD
                // 申請日
                IRow row_2 = base.GetOrCreateRow(sheet, sRow + 1);
                ICell cell_2F = base.GetOrCreateCell(row_2, 7);
                cell_2F.SetCellValue(DateTime.Now.ToString("yyyy/MM/dd"));

                // -- Row 3
                IRow row_3 = base.GetOrCreateRow(sheet, sRow + 2);
                // 申請者
                ICell cell_3F = base.GetOrCreateCell(row_3, 7);
                cell_3F.SetCellValue(EditDataUtil.ToFixCodeShow(lstItem[0].UserCD, M_User.MAX_USER_CODE_SHOW) + " " + lstItem[0].UserName);

                // 集計期間
                ICell cell_3C = base.GetOrCreateCell(row_3, 2);
                // -- Row 4      
                IRow row_4 = base.GetOrCreateRow(sheet, sRow + 4);
                ICell cell_4A = base.GetOrCreateCell(row_4, 0);

                if (this.isAccountingDate)
                {
                    cell_3C.SetCellValue("集計期間（計上日）");
                    cell_4A.SetCellValue("計上日");
                }
                else
                {
                    cell_3C.SetCellValue("集計期間（利用日）");
                    cell_4A.SetCellValue("利用日");
                }

                if (!string.IsNullOrEmpty(dateForm) || !string.IsNullOrEmpty(dateTo))
                {
                    ICell cell_3D = base.GetOrCreateCell(row_3, 3);
                    cell_3D.SetCellValue(dateForm + "～" + dateTo);
                }

                // ---- DETAIL
                int idx = 0;
                foreach (ExpenceSeisanExcel item in lstItem)
                {
                    IRow rowTemp = base.GetOrCreateRow(sheet, hRow + idx);

                    for (int i = 0; i < 10; i++)
                    {
                        ICell cell = base.GetOrCreateCell(rowTemp, i);
                        cell.CellStyle = lstCellStyle[i];

                        switch (i)
                        {
                            case 0:
                                cell.SetCellValue(item.Date);
                                break;
                            case 1:
                                cell.SetCellValue(item.ExpenceNo);
                                break;
                            case 2:
                                // オーダー名
                                cell.SetCellValue(item.ProjectName);
                                break;
                            case 3:
                                // 用件
                                cell.SetCellValue(item.Memo);
                                break;
                            case 4:
                                // 宿泊施設/交通期間
                                cell.SetCellValue(item.PaidTo);
                                break;
                            case 5:
                                // 乗車区間（交通機関）
                                cell.SetCellValue(item.RouteFrom);
                                break;
                            case 6:
                                // 乗車区間（交通機関）
                                if (!string.IsNullOrEmpty(item.RouteFrom) || !string.IsNullOrEmpty(item.RouteTo))
                                {
                                    ICell cellRoute = base.GetOrCreateCell(rowTemp, 6);
                                    cell.SetCellValue("～");
                                }
                                break;
                            case 7:
                                // 乗車区間（交通機関）
                                cell.SetCellValue(item.RouteTo);
                                break;
                            case 8:
                                // 区分
                                cell.SetCellValue(item.RouteType);
                                break;
                            case 9:
                                // 金額
                                if (!string.IsNullOrEmpty(item.Amount))
                                {
                                    cell.SetCellValue(double.Parse(item.Amount));
                                }
                                break;
                            default:
                                break;
                        }
                    }

                    idx++;
                }

                // 合計
                IRow rowTotal = null;
                int idxT = 0;
                if (idx < 23)
                {
                    idxT = 23;
                }
                else
                {
                    idxT = hRow + idx;
                }
                rowTotal = base.GetOrCreateRow(sheet, idxT);

                for (int i = 0; i < 10; i++)
                {
                    ICell cell_T = base.GetOrCreateCell(rowTotal, i);
                    cell_T.CellStyle = _cellStyle_TI;
                    switch (i)
                    {
                        // I
                        case 8:
                            cell_T.SetCellValue("合計");
                            break;

                        // J
                        case 9:
                            cell_T.SetCellType(CellType.Formula);
                            cell_T.SetCellFormula(String.Format("SUM(J{0}:J{1})", hRow + 1, idxT));

                            cell_T.CellStyle = _cellStyle_TJ;
                            break;

                        default:

                            break;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
