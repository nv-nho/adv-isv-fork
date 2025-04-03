using System;
using System.Data.Common;
using OMS.Utilities;

namespace OMS.Models
{
    /// <summary>
    /// Class ProjectProfit Info
    /// ISV-Tinh
    /// </summary>
    [Serializable]
    public class ProjectProfitInfo
    {
        public int ID { get; set; }//
        public long RowNumber { get; set; }//
        public string ProjectCD { get; set; }//
        public string ProjectName { get; set; }//
        public int DepartmentID { get; set; }//
        public string DepartmentName { get; set; }//
        public int UserID { get; set; }//
        public string UserName { get; set; }//
        public string ProjectTime { get; set; }//
        public int AcceptanceFlag { get; set; }//
        public decimal OrderAmount { get; set; }//
        public decimal ProfitRate { get; set; }//
        public decimal CostTotal { get; set; }//
        public decimal DirectCost { get; set; }//
        public decimal IndirectCosts { get; set; }//
        public decimal Expense { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? AcceptanceDate { get; set; }

        public DateTime? SC_StartDate { get; set; }
        public DateTime? SC_EndDate { get; set; }

        public ProjectProfitInfo()
        {
            this.RowNumber = 0;
            this.ProjectCD = string.Empty;
            this.ProjectName = string.Empty;
            this.DepartmentID = -1;
            this.DepartmentName = string.Empty;
            this.UserID = -1;
            this.UserName = string.Empty;
            this.ProjectTime = string.Empty;
            this.AcceptanceFlag = 0;
            this.OrderAmount = 0;
            this.ProfitRate = 0;
            this.CostTotal = 0;
            this.DirectCost = 0;
            this.IndirectCosts = 0;
            this.Expense = 0;
        }
    }
    public class ProjectProfitUserDetailInfo
    {
        public long UserID { get; set; }
        public string UserCD { get; set; }
        public string UserName { get; set; }
        public decimal DirectCost { get; set; }
        public decimal DirectCostAfter { get; set; }
        public decimal IndirectCosts { get; set; }
        public decimal Total { get; set; }
        public ProjectProfitUserDetailInfo()
        {
            this.UserID = 0;
            this.UserCD = string.Empty;
            this.UserName = string.Empty;
            this.DirectCost = 0;
            this.DirectCostAfter = 0;
            this.IndirectCosts = 0;
            this.Total = 0;

        }
    }
    public class ProjectProfitDateDetailInfo
    {
        public DateTime Date { get; set; }
        public string DateStr { get; set; }
        public long RegisterPersonID { get; set; }
        public string RegisterPersonName { get; set; }
        public string Type { get; set; }
        public long DestinationID { get; set; }
        public string DestinationName { get; set; }
        public decimal ExpenceAmount { get; set; }
        public string Memo { get; set; }
        public string ApprovedStatus { get; set; }
        public string ApprovedFlag { get; set; }

        public ProjectProfitDateDetailInfo()
        {
            this.Date = DateTime.Today;
            this.DateStr = DateTime.Today.ToString("yyyy/MM/dd");
            this.RegisterPersonID = 0;
            this.RegisterPersonName = string.Empty;
            this.Type = string.Empty;
            this.DestinationID = 0;
            this.DestinationName = string.Empty;
            this.ExpenceAmount = 0;
            this.Memo = string.Empty;
            this.ApprovedStatus = string.Empty;
            this.ApprovedFlag = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        public ProjectProfitDateDetailInfo(DbDataReader dr)
        {
            this.Date = !string.IsNullOrEmpty(dr["Date"].ToString()) ? (DateTime)dr["Date"] : DateTime.MaxValue;
            this.DateStr = !this.Date.Equals(DateTime.MaxValue) ? this.Date.ToString("yyyy/MM/dd") : string.Empty;
            //this.RegisterPersonID = int.Parse(dr["RegisterPersonID"].ToString());
            this.RegisterPersonName = dr["UserName"].ToString();
            this.Type = dr["Type"].ToString();
            //this.DestinationID = int.Parse(dr["DestinationID"].ToString());
            this.DestinationName = dr["PaidTo"].ToString();
            this.ExpenceAmount = decimal.Parse(dr["Amount"].ToString());
            this.Memo = dr["Note"].ToString();
            this.ApprovedStatus = dr["ApprovedFlag"].ToString() == "0" ? "<span class='label label-warning'>申請中</span>" : "<span class='label label-primary'>承認済</span>";
            this.ApprovedFlag = dr["ApprovedFlag"].ToString();
        }
    }

    public class ProjectProfitFullInfo
    {

        public int ProjectID { get; set; }
        public string ProjectCD { get; set; }
        public string ProjectName { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int PrUserID { get; set; }
        public string PrUserName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? AcceptanceDate { get; set; }
        public int AcceptanceFlag { get; set; }
        public decimal OrderAmount { get; set; }
        public int WorkUserID { get; set; }
        public string WorkUserCD { get; set; }
        public string WorkUserName { get; set; }
        public int HID { get; set; }
        public int Work_D_ID { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public decimal WorkSystemSuu { get; set; }
        public int WorkingType { get; set; }
        public int Working_Start { get; set; }
        public int Working_End { get; set; }
        public int Working_End_2 { get; set; }
        public int OverTime1_Start { get; set; }
        public int OverTime1_End { get; set; }
        public int OverTime2_Start { get; set; }
        public int OverTime2_End { get; set; }
        public int OverTime3_Start { get; set; }
        public int OverTime3_End { get; set; }
        public int OverTime4_Start { get; set; }
        public int OverTime4_End { get; set; }
        public int OverTime5_Start { get; set; }
        public int OverTime5_End { get; set; }
        public int BreakType { get; set; }
        public int EntryTime { get; set; }
        public int ExitTime { get; set; }
        public int Break1_Start { get; set; }
        public int Break1_End { get; set; }
        public int Break2_Start { get; set; }
        public int Break2_End { get; set; }
        public int Break3_Start { get; set; }
        public int Break3_End { get; set; }
        public int Break4_Start { get; set; }
        public int Break4_End { get; set; }
        public decimal DirectCost { get; set; }
        public decimal IndirectCost { get; set; }
        /// <summary>
        /// Constructor class ProjectInfo
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public ProjectProfitFullInfo(DbDataReader dr)
        {

            this.ProjectID = (int)dr["ProjectID"];
            this.ProjectCD = dr["ProjectCD"].ToString();
            this.ProjectName = dr["ProjectName"].ToString();
            this.DepartmentID = !string.IsNullOrEmpty(dr["DepartmentID"].ToString()) ? (int)dr["DepartmentID"] : -1;
            this.DepartmentName = dr["DepartmentName"].ToString();
            this.PrUserID = !string.IsNullOrEmpty(dr["PrUserID"].ToString()) ? (int)dr["PrUserID"] : -1;
            this.PrUserName = dr["PrUserName"].ToString();
            this.StartDate = dr["StartDate"].ToString() != string.Empty ? (DateTime)dr["StartDate"] : DateTime.MaxValue;
            this.EndDate = dr["EndDate"] != DBNull.Value ? (DateTime)dr["EndDate"] : this.EndDate;
            this.AcceptanceDate = dr["AcceptanceDate"].ToString() != string.Empty ? (DateTime)dr["AcceptanceDate"] : DateTime.MaxValue;
            this.AcceptanceFlag = !string.IsNullOrEmpty(dr["AcceptanceFlag"].ToString()) ? int.Parse(dr["AcceptanceFlag"].ToString()) : 0;
            this.OrderAmount = !string.IsNullOrEmpty(dr["OrderAmount"].ToString()) ? (decimal)dr["OrderAmount"] : 0;
            this.WorkUserID = !string.IsNullOrEmpty(dr["WorkUserID"].ToString()) ? (int)dr["WorkUserID"] : -1;
            this.WorkUserCD = EditDataUtil.ToFixCodeShow(dr["WorkUserCD"].ToString(), M_User.MAX_USER_CODE_SHOW);
            this.WorkUserName = dr["WorkUserName"].ToString();
            this.HID = !string.IsNullOrEmpty(dr["HID"].ToString()) ? (int)dr["HID"] : -1;
            this.Work_D_ID = !string.IsNullOrEmpty(dr["Work_D_ID"].ToString()) ? (int)dr["Work_D_ID"] : -1;
            this.StartTime = !string.IsNullOrEmpty(dr["StartTime"].ToString()) ? (int)dr["StartTime"] : -1;
            this.EndTime = !string.IsNullOrEmpty(dr["EndTime"].ToString()) ? (int)dr["EndTime"] : -1;
            this.WorkSystemSuu = !string.IsNullOrEmpty(dr["WorkSystemSuu"].ToString()) ? decimal.Parse(dr["WorkSystemSuu"].ToString()) : -1;
            this.WorkingType = !string.IsNullOrEmpty(dr["WorkingType"].ToString()) ? int.Parse(dr["WorkingType"].ToString()) : -1;
            this.Working_Start = !string.IsNullOrEmpty(dr["Working_Start"].ToString()) ? (int)dr["Working_Start"] : -1;
            this.Working_End = !string.IsNullOrEmpty(dr["Working_End"].ToString()) ? (int)dr["Working_End"] : -1;
            this.Working_End_2 = !string.IsNullOrEmpty(dr["Working_End_2"].ToString()) ? (int)dr["Working_End_2"] : -1;
            this.OverTime1_Start = !string.IsNullOrEmpty(dr["OverTime1_Start"].ToString()) ? (int)dr["OverTime1_Start"] : -1;
            this.OverTime1_End = !string.IsNullOrEmpty(dr["OverTime1_End"].ToString()) ? (int)dr["OverTime1_End"] : -1;
            this.OverTime2_Start = !string.IsNullOrEmpty(dr["OverTime2_Start"].ToString()) ? (int)dr["OverTime2_Start"] : -1;
            this.OverTime2_End = !string.IsNullOrEmpty(dr["OverTime2_End"].ToString()) ? (int)dr["OverTime2_End"] : -1;
            this.OverTime3_Start = !string.IsNullOrEmpty(dr["OverTime3_Start"].ToString()) ? (int)dr["OverTime3_Start"] : -1;
            this.OverTime3_End = !string.IsNullOrEmpty(dr["OverTime3_End"].ToString()) ? (int)dr["OverTime3_End"] : -1;
            this.OverTime4_Start = !string.IsNullOrEmpty(dr["OverTime4_Start"].ToString()) ? (int)dr["OverTime4_Start"] : -1;
            this.OverTime4_End = !string.IsNullOrEmpty(dr["OverTime4_End"].ToString()) ? (int)dr["OverTime4_End"] : -1;
            this.OverTime5_Start = !string.IsNullOrEmpty(dr["OverTime5_Start"].ToString()) ? (int)dr["OverTime5_Start"] : -1;
            this.OverTime5_End = !string.IsNullOrEmpty(dr["OverTime5_End"].ToString()) ? (int)dr["OverTime5_End"] : -1;

            this.BreakType = !string.IsNullOrEmpty(dr["BreakType"].ToString()) ? int.Parse(dr["BreakType"].ToString()) : -1;
            this.EntryTime = !string.IsNullOrEmpty(dr["EntryTime"].ToString()) ? int.Parse(dr["EntryTime"].ToString()) : -1;
            this.ExitTime = !string.IsNullOrEmpty(dr["ExitTime"].ToString()) ? int.Parse(dr["ExitTime"].ToString()) : -1;
            this.Break1_Start = !string.IsNullOrEmpty(dr["Break1_Start"].ToString()) ? int.Parse(dr["Break1_Start"].ToString()) : -1;
            this.Break1_End = !string.IsNullOrEmpty(dr["Break1_End"].ToString()) ? int.Parse(dr["Break1_End"].ToString()) : -1;
            this.Break2_Start = !string.IsNullOrEmpty(dr["Break2_Start"].ToString()) ? int.Parse(dr["Break2_Start"].ToString()) : -1;
            this.Break2_End = !string.IsNullOrEmpty(dr["Break2_End"].ToString()) ? int.Parse(dr["Break2_End"].ToString()) : -1;
            this.Break3_Start = !string.IsNullOrEmpty(dr["Break3_Start"].ToString()) ? int.Parse(dr["Break3_Start"].ToString()) : -1;
            this.Break3_End = !string.IsNullOrEmpty(dr["Break3_End"].ToString()) ? int.Parse(dr["Break3_End"].ToString()) : -1;
            this.Break4_Start = !string.IsNullOrEmpty(dr["Break4_Start"].ToString()) ? int.Parse(dr["Break4_Start"].ToString()) : -1;
            this.Break4_End = !string.IsNullOrEmpty(dr["Break4_End"].ToString()) ? int.Parse(dr["Break4_End"].ToString()) : -1;

            this.DirectCost = !string.IsNullOrEmpty(dr["DirectCost"].ToString()) ? (decimal)dr["DirectCost"] : -1;
            this.IndirectCost = !string.IsNullOrEmpty(dr["IndirectCost"].ToString()) ? (decimal)dr["IndirectCost"] : -1;
        }
        public ProjectProfitFullInfo()
        {
            this.ProjectID = 0;
            this.ProjectCD = string.Empty;
            this.ProjectName = string.Empty;
            this.DepartmentID = 0;
            this.DepartmentName = string.Empty;
            this.PrUserID = 0;
            this.PrUserName = string.Empty;
            this.StartDate = DateTime.MaxValue;
            this.EndDate = null;
            this.AcceptanceDate = DateTime.MaxValue;
            this.AcceptanceFlag = 0;
            this.OrderAmount = 0;
            this.StartTime = 0;
            this.EndTime = 0;
            this.WorkSystemSuu = 1;
            this.Working_Start = 0;
            this.Working_End = 0;
            this.Working_End_2 = 0;
            this.OverTime1_Start = 0;
            this.OverTime1_End = 0;
            this.OverTime2_Start = 0;
            this.OverTime2_End = 0;
            this.OverTime3_Start = 0;
            this.OverTime3_End = 0;
            this.OverTime4_Start = 0;
            this.OverTime4_End = 0;
            this.OverTime5_Start = 0;
            this.OverTime5_End = 0;
            this.DirectCost = 0;
            this.IndirectCost = 0;
        }
    }
}
