using OMS.DAC;
using OMS.Models;
using OMS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using OMS.Reports.EXCEL;
using NPOI.SS.UserModel;

namespace OMS.ProjectProfit
{
    public partial class FrmProjectProfitEntry : FrmBaseDetail
    {
        #region Cosntant
        private const int ADMIN_ID = 1;

        private const string URL_LIST = "~/ProjectProfit/FrmProjectProfitList.aspx";
        private const string ATTENDANCELIST_DOWNLOAD = "採算管理明細_{0}.xlsx";
        private const string FMT_YMDHMM = "yyMMddHHmm";
        #endregion

        #region Property

        /// <summary>
        /// Get or set ProjectID
        /// </summary>
        public int ProjectID
        {
            get { return (int)ViewState["ProjectID"]; }
            set { ViewState["ProjectID"] = value; }
        }
        /// <summary>
        /// Get or set OldUpdateDate
        /// </summary>
        public DateTime OldUpdateDate
        {
            get { return (DateTime)ViewState["OldUpdateDate"]; }
            set { ViewState["OldUpdateDate"] = value; }
        }
        /// <summary>
        /// Get or set ProjectID
        /// </summary>
        public ProjectProfitInfo ProjectProfitInfo
        {
            get { return (ProjectProfitInfo)ViewState["ProjectProfitInfo"]; }
            set { ViewState["ProjectProfitInfo"] = value; }
        }
        #endregion

        #region Event
        /// <summary>
        /// Event Init
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //Click the Download Excel button
            this.btnDownload.ServerClick += new EventHandler(btnDownloadExcel_Click);

            //Set Title
            base.FormTitle = "採算管理詳細";
            base.FormSubTitle = "Detail";

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            base.SetAuthority(FormId.Project);
            if (!this._authority.IsMasterView)
            {
                Response.Redirect("~/Menu/FrmMainMenu.aspx");
            }
            if (!this.IsPostBack)
            {
                if (this.PreviousPage != null)
                {
                    //Save condition of previous page
                    this.ViewState["Condition"] = this.PreviousPageViewState["Condition"];
                    if (this.PreviousPageViewState["ID"] != null)
                    {
                        string[] PreviousPagePrms = base.PreviousPageViewState["ID"].ToString().Split(',');

                        this.ProjectID = int.Parse(PreviousPagePrms[0]);
                        if (!string.IsNullOrEmpty(PreviousPagePrms[1]))
                        {
                            this.dtStartDate.Value = DateTime.Parse(PreviousPagePrms[1]);
                        }

                        if (!string.IsNullOrEmpty(PreviousPagePrms[2]))
                        {
                            this.dtEndDate.Value = DateTime.Parse(PreviousPagePrms[2]);
                        }

                        this.txtProjectCode.ReadOnly = true;
                        this.txtProjectName.ReadOnly = true;
                        this.txtDepartment.ReadOnly = true;
                        this.txtUser.ReadOnly = true;
                        this.dtStartDate.ReadOnly = true;
                        this.dtEndDate.ReadOnly = true;
                        this.txtStatus.ReadOnly = true;

                        //Show data on grid
                        this.LoadDataGrid(this.ProjectID);
                    }
                    else
                    {
                        Server.Transfer(URL_LIST);
                    }

                }
                else
                {
                    Server.Transfer(URL_LIST);
                }
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcel_Command(object sender, CommandEventArgs e)
        {
            IList<ProjectProfitFullInfo> lstProjectProfitFull;
            List<ProjectProfitUserDetailInfo> listUserDetail = new List<ProjectProfitUserDetailInfo>();
            List<ProjectProfitDateDetailInfo> listExpenseDetail = new List<ProjectProfitDateDetailInfo>();
            ProjectProfitInfo HeaderInfo = null;
            IList<ProjectProfitInfo> lstProjectProfit;
            using (DB db = new DB())
            {
                ProjectProfitService prfService = new ProjectProfitService(db);
                lstProjectProfitFull = prfService.GetFullListByCondition(this.dtStartDate.Value, this.dtEndDate.Value, this.ProjectID);
                listExpenseDetail = prfService.GetExpenseDetailProject(this.ProjectID, this.dtStartDate.Value, this.dtEndDate.Value).ToList();
                lstProjectProfit = prfService.CreateProjectProfitInfoGroupProject(lstProjectProfitFull, this.dtStartDate.Value, this.dtEndDate.Value, FrmProjectProfitList.OT_CONFIG_CD);
                listUserDetail = prfService.CreateProjectProfitUserDetail(lstProjectProfitFull, FrmProjectProfitList.OT_CONFIG_CD, this.ProjectID).OrderBy(m => m.UserCD).ToList();
            }
            if (lstProjectProfit.Any())
            {
                HeaderInfo = lstProjectProfit.First();
            }
            else
            {
                Server.Transfer(URL_LIST);
            }

            while (listUserDetail.Count < 10)
            {
                listUserDetail.Add(new ProjectProfitUserDetailInfo()
                {
                    DirectCost = -1,
                    DirectCostAfter = -1,
                    IndirectCosts = -1,
                    Total = -1
                });
            }
            ProjectProfitUserDetailInfo TotalUserDetail = new ProjectProfitUserDetailInfo()
            {
                UserName = "合計",
                DirectCost = listUserDetail.Where(m => m.DirectCost != -1).Sum(m => m.DirectCost),
                DirectCostAfter = listUserDetail.Where(m => m.DirectCostAfter != -1).Sum(m => m.DirectCostAfter),
                IndirectCosts = listUserDetail.Where(m => m.IndirectCosts != -1).Sum(m => m.IndirectCosts),
                Total = listUserDetail.Where(m => m.Total != -1).Sum(m => m.Total),
            };
            listUserDetail.Add(TotalUserDetail);
            while (listExpenseDetail.Count < 10)
            {
                listExpenseDetail.Add(new ProjectProfitDateDetailInfo()
                {
                    DateStr = string.Empty,
                    ExpenceAmount = -1
                });
            }
            ProjectProfitDateDetailInfo TotalDetail2 = new ProjectProfitDateDetailInfo()
            {
                DateStr = string.Empty,
                DestinationName = "合計",
                ExpenceAmount = listExpenseDetail.Where(m => m.ExpenceAmount != -1).Sum(m => m.ExpenceAmount)

            };
            listExpenseDetail.Add(TotalDetail2);

            if (HeaderInfo == null) { }
            else
            {
                HeaderInfo.SC_StartDate = this.dtStartDate.Value;
                HeaderInfo.SC_EndDate = this.dtEndDate.Value;
                ProjectProfitEntryExcel excel = new ProjectProfitEntryExcel();
                excel.headerInfo = HeaderInfo;
                excel.listUserDetail = listUserDetail;
                excel.listExpenseDetail = listExpenseDetail;
                IWorkbook wb = excel.OutputExcel();
                if (wb != null)
                {
                    this.SaveFile(wb, ".xlsx");
                }
            }
        }
        /// <summary>
        /// btnDownloadExcel_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDownloadExcel_Click(object sender, EventArgs e)
        {
            if (this.IsOutFile)
            {
                var filename = string.Empty;

                filename = string.Format(ATTENDANCELIST_DOWNLOAD, DateTime.Now.ToString(FMT_YMDHMM));
                var filePath = this.ViewState["OUTFILE"].ToString();
                using (var exportData = base.GetFileStream("OUTFILE"))
                {
                    Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("Content-Disposition", string.Format("attachment; filename = \"{0}\"", filename));
                    Response.Clear();
                    Response.BinaryWrite(exportData.GetBuffer());
                    Response.End();
                }
            }
        }
        #endregion
        #region Method

        /// <summary>
        /// load data gird
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="numOnPage"></param>
        private void LoadDataGrid(int projectID)
        {
            IList<ProjectProfitFullInfo> lstProjectProfitFull;
            IList<ProjectProfitUserDetailInfo> listUserDetail = new List<ProjectProfitUserDetailInfo>();
            IList<ProjectProfitDateDetailInfo> listDateDetail = new List<ProjectProfitDateDetailInfo>();
            IList<ProjectProfitInfo> lstProjectProfit;
            using (DB db = new DB())
            {
                ProjectProfitService prfService = new ProjectProfitService(db);

                lstProjectProfitFull = prfService.GetFullListByCondition(this.dtStartDate.Value, this.dtEndDate.Value, this.ProjectID);
                listDateDetail = prfService.GetExpenseDetailProject(this.ProjectID, this.dtStartDate.Value, this.dtEndDate.Value);
                lstProjectProfit = prfService.CreateProjectProfitInfoGroupProject(lstProjectProfitFull, this.dtStartDate.Value, this.dtEndDate.Value, FrmProjectProfitList.OT_CONFIG_CD);
                listUserDetail = prfService.CreateProjectProfitUserDetail(lstProjectProfitFull, FrmProjectProfitList.OT_CONFIG_CD, this.ProjectID).OrderBy(m => m.UserCD).ToList();
            }
            if (lstProjectProfit.Any())
            {
                this.ProjectProfitInfo = lstProjectProfit.First();
                this.txtProjectCode.Value = lstProjectProfit.First().ProjectCD;
                this.txtProjectName.Value = lstProjectProfit.First().ProjectName;
                this.txtDepartment.Value = lstProjectProfit.First().DepartmentName;
                this.txtUser.Value = lstProjectProfit.First().UserName;
                //this.dtStartDate.Value = lstProjectProfit.First().StartDate;
                //this.dtEndDate.Value = lstProjectProfit.First().DeliveryDate;
                this.txtStatus.Value = lstProjectProfit.First().AcceptanceFlag.Equals(0) ? "仕掛" : "検収";
            }
            else
            {
                Server.Transfer(URL_LIST);
            }


            ProjectProfitUserDetailInfo TotalUserDetail = new ProjectProfitUserDetailInfo()
            {
                UserName = "合計",
                DirectCost = listUserDetail.Sum(m => m.DirectCost),
                DirectCostAfter = listUserDetail.Sum(m => m.DirectCostAfter),
                IndirectCosts = listUserDetail.Sum(m => m.IndirectCosts),
                Total = listUserDetail.Sum(m => m.Total),
            };
            listUserDetail.Add(TotalUserDetail);

            ProjectProfitDateDetailInfo TotalDetail2 = new ProjectProfitDateDetailInfo()
            {
                DateStr = string.Empty,
                DestinationName = "合計",
                ExpenceAmount = listDateDetail.Sum(m => m.ExpenceAmount)

            };
            listDateDetail.Add(TotalDetail2);

            // detail1
            this.rptDetailList1.DataSource = listUserDetail;
            this.rptDetailList1.DataBind();

            // detail2
            this.rptDetailList2.DataSource = listDateDetail;
            this.rptDetailList2.DataBind();
        }
        #endregion


    }
}