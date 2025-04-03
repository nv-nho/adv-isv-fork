using System;
using System.Data.Common;
using OMS.Models;
using OMS.Utilities;
using System.Collections.Generic;

namespace OMS.Models
{
    /// <summary>
    /// Class ExpenceInfo Model
    /// </summary>
    [Serializable]
    public class ExpenceInfo
    {
        public int ID { get; set; }
        public long RowNumber { get; set; }
        public bool CheckFlag { get; set; }
        public string ExpenceNo { get; set; }
        public DateTime Date { get; set; }
        public string DateStr { get; set; }
        public string AccountCD { get; set; }
        public string Value2 { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public decimal ExpenceAmount { get; set; }
        public int UserID { get; set; }
        public int UpdateUID { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UserName1 { get; set; }
        public string UserName2 { get; set; }
        public string UserName3 { get; set; }
        public string Memo { get; set; }
        public string ExpenceType { get; set; }
        /// <summary>
        /// Constructor class ExpenceInfo
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public ExpenceInfo(DbDataReader dr)
        {
            this.ID = int.Parse(dr["ID"].ToString());
            this.RowNumber = (long)dr["RowNumber"];
            this.ExpenceNo = EditDataUtil.ToFixCodeShow((string)dr["ExpenceNo"], T_Expence_H.EXPENCE_CODE_SHOW_MAX_LENGTH);
            this.Date = (DateTime)dr["Date"];
            this.DateStr = this.Date.ToString(Constants.FMT_DATE_EN);
            this.AccountCD = (string)dr["AccountCD"];
            this.Value2 = (string)dr["Value2"];
            this.DepartmentID = int.Parse(dr["DepartmentID"].ToString());
            this.DepartmentName = (string)dr["DepartmentName"];
            this.ProjectID = int.Parse(dr["ProjectID"].ToString());
            this.ProjectName = (string)dr["ProjectName"];
            this.ExpenceAmount = decimal.Parse(dr["ExpenceAmount"].ToString());
            this.UserID = int.Parse(dr["UserID"].ToString());
            this.UpdateUID = int.Parse(dr["UpdateUID"].ToString());
            this.UpdateDate = (DateTime)dr["UpdateDate"];
            this.UserName1 = dr["UserName1"].ToString();
            this.UserName2 = dr["UserName2"].ToString();
            this.UserName3 = dr["UserName3"].ToString();
            this.Memo = (string)dr["Memo"];
        }
        /// <summary>
        /// Constructor class ExpenceInfo
        /// </summary>
        public ExpenceInfo()
        {
            this.RowNumber = 0;
            this.ExpenceNo = string.Empty;
            this.Date = DateTime.MinValue;
            this.UpdateDate = DateTime.MinValue;
            this.AccountCD = string.Empty;
            this.DepartmentID = 0;
            this.DepartmentName = string.Empty;
            this.ProjectID = 0;
            this.ProjectName = string.Empty;
            this.ExpenceAmount = 0;
            this.UserID = 0;
            this.UpdateUID = 0;
            this.UserName1 = string.Empty;
            this.UserName2 = string.Empty;
            this.UserName3 = string.Empty;
            this.Memo = string.Empty;
        }
    }
    /// <summary>
    /// Class ExpenceInfo Model
    /// </summary>
    [Serializable]
    public class ExpenceSearchInfo
    {
        public int HID { get; set; }
        public long RowNumber { get; set; }
        public DateTime Date { get; set; }
        public string DateStr { get; set; }
        public string PaidTo { get; set; }
        public string RouteFrom { get; set; }
        public string RouteTo { get; set; }
        public int RouteType { get; set; }
        public string Note { get; set; }
        public int TaxType { get; set; }
        public int TaxRate { get; set; }
        public decimal Amount { get; set; }
        public decimal TaxAmount { get; set; }

        /// <summary>
        /// Constructor class ExpenceSearchInfo
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public ExpenceSearchInfo(DbDataReader dr)
        {
            this.HID = int.Parse(dr["HID"].ToString());
            this.RowNumber = (long)dr["RowNumber"];
            this.Date = (DateTime)dr["Date"];
            this.DateStr = this.Date.ToString(Constants.FMT_DATE_EN);
            this.PaidTo = (string)dr["PaidTo"];
            this.RouteFrom = (string)dr["RouteFrom"];
            this.RouteTo = (string)dr["RouteTo"];
            this.RouteType = int.Parse(dr["RouteType"].ToString());
            this.Note = (string)dr["Note"];
            this.TaxType = int.Parse(dr["TaxType"].ToString());
            this.TaxRate = int.Parse(dr["TaxRate"].ToString());
            this.Amount = decimal.Parse(dr["Amount"].ToString());
            this.TaxAmount = decimal.Parse(dr["TaxAmount"].ToString());

        }

        /// <summary>
        /// Constructor class ExpenceSearchInfo
        /// </summary>
        public ExpenceSearchInfo()
        {
            this.RowNumber = 0;
            this.Date = DateTime.MinValue;
            this.PaidTo = string.Empty;
            this.RouteFrom = string.Empty;
            this.RouteTo = string.Empty;
            this.RouteType = 0;
            this.Note = string.Empty;
            this.TaxType = 0;
            this.TaxRate = 0;
            this.Amount = 0;
            this.TaxAmount = 0;
        }
    }

    /// <summary>
    /// Class ExpenceSeisanExcel Model
    /// </summary>
    [Serializable]
    public class ExpenceSeisanExcel
    {
        public string Date { get; set; }
        public string ExpenceNo { get; set; }
        public string UserCD { get; set; }
        public string UserName { get; set; }
        public string ProjectName { get; set; }
        public string PaidTo { get; set; }
        public string RouteFrom { get; set; }
        public string RouteTo { get; set; }
        public string RouteType { get; set; }
        public string Memo { get; set; }
        public string Amount { get; set; }

        /// <summary>
        /// Constructor class ExpenceSeisanExcel
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public ExpenceSeisanExcel(DbDataReader dr)
        {
            this.Date = string.Format(Constants.FMR_DATE_YMD, dr["Date"]);
            this.PaidTo = dr["PaidTo"].ToString();
            this.ExpenceNo = EditDataUtil.ToFixCodeShow((string)dr["ExpenceNo"], T_Expence_H.EXPENCE_CODE_SHOW_MAX_LENGTH);
            this.UserCD = EditDataUtil.ToFixCodeShow(dr["UserCD"].ToString(), M_User.MAX_USER_CODE_SHOW);
            this.UserName = dr["UserName"].ToString();
            this.ProjectName = dr["ProjectName"].ToString();
            this.RouteFrom = dr["RouteFrom"].ToString();
            this.RouteTo = dr["RouteTo"].ToString();
            this.RouteType = dr["RouteType"].ToString();
            this.Memo = dr["Memo"].ToString();
            this.Amount = dr["Amount"].ToString();
        }

        /// <summary>
        /// Constructor class ExpenceSearchInfo
        /// </summary>
        public ExpenceSeisanExcel()
        {
            this.Date = string.Empty;
            this.PaidTo = string.Empty;
            this.ExpenceNo = string.Empty;
            this.UserCD = string.Empty;
            this.UserName = string.Empty;
            this.RouteFrom = string.Empty;
            this.RouteTo = string.Empty;
            this.RouteType = string.Empty;
            this.Memo = string.Empty;
            this.Amount = string.Empty;
        }
    }
}

