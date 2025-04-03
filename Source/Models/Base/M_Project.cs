using System;
using System.Data.Common;
using System.Web.Script.Serialization;

namespace OMS.Models
{
    /// <summary>
    /// Class M_Project
    /// Create  :isv.than
    /// Date    :05/10/2017
    /// </summary>
    [Serializable]
    public class M_Project : M_Base<M_Project>
    {
        #region Constant

        public const int PROJECT_CODE_SHOW_MAX_LENGTH = 10;
        public const int PROJECT_CODE_DB_MAX_LENGTH = 10;
        public const int PROJECT_NAME_MAX_LENGTH = 100;
        public const int PROJECT_WORKPLACE_MAX_LENGTH = 50;
        public const decimal PROJECT_ORDER_AMOUNT_MAX_LENGTH = 9999999999.99M;
        public const int PORJECT_MEMO_LENGTH = 50;

        #endregion

        #region Variable
        public string projectCD { get; set; }
        private string projectName { get; set; }
        private string workPlace { get; set; }
        private string memo { get; set; }
        private DateTime? startDate { get; set; }
        private DateTime? endDate { get; set; }
        private int userID { get; set; }
        private int departmentID { get; set; }
        private DateTime? deliveryDate { get; set; }
        private DateTime? acceptanceDate { get; set; }
        private int acceptanceFlag { get; set; }
        private int statusFlag { get; set; }
        private decimal orderAmount { get; set; }
        #endregion

        #region Property

        public string ProjectCD
        {
            get { return projectCD; }
            set
            {
                if (value != projectCD)
                {
                    projectCD = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public string ProjectName
        {
            get { return projectName; }
            set
            {
                if (value != projectName)
                {
                    projectName = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public string WorkPlace
        {
            get { return workPlace; }
            set
            {
                if (value != workPlace)
                {
                    workPlace = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public string Memo
        {
            get { return memo; }
            set
            {
                if (value != memo)
                {
                    memo = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public DateTime? StartDate
        {
            get { return startDate; }
            set
            {
                if (value != startDate)
                {
                    startDate = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }
        public DateTime? EndDate
        {
            get { return endDate; }
            set
            {
                if (value != endDate)
                {
                    endDate = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int UserID
        {
            get { return userID; }
            set
            {
                if (value != userID)
                {
                    userID = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int DepartmentID
        {
            get { return departmentID; }
            set
            {
                if (value != departmentID)
                {
                    departmentID = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        [ScriptIgnore]
        public DateTime? DeliveryDate
        {
            get { return deliveryDate; }
            set
            {
                if (value != deliveryDate)
                {
                    deliveryDate = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }


        public DateTime? AcceptanceDate
        {
            get { return acceptanceDate; }
            set
            {
                if (value != acceptanceDate)
                {
                    acceptanceDate = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public decimal OrderAmount
        {
            get { return orderAmount; }
            set
            {
                if (value != orderAmount)
                {
                    orderAmount = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int AcceptanceFlag
        {
            get { return acceptanceFlag; }
            set
            {
                if (value != acceptanceFlag)
                {
                    acceptanceFlag = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        public int StatusFlag
        {
            get { return statusFlag; }
            set
            {
                if (value != statusFlag)
                {
                    statusFlag = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        #endregion

        #region Contructor

        /// <summary>
        /// Contructor M_WorkingSystem
        /// </summary>
        public M_Project()
            : base()
        {

        }

        public M_Project(DbDataReader dr)
            : base(dr)
        {
            this.ProjectCD = (string)dr["ProjectCD"];
            this.projectName = (string)dr["ProjectName"];
            this.workPlace = (string)dr["WorkPlace"];
            this.memo = dr["Memo"].ToString();
            this.startDate = (DateTime?)dr["StartDate"];
            this.EndDate = dr["EndDate"].ToString() != string.Empty ? (DateTime?)dr["EndDate"] : (DateTime?)null;
            this.userID = (int)dr["UserID"];
            this.departmentID = (int)dr["DepartmentID"];
            this.deliveryDate = dr["DeliveryDate"].ToString() != string.Empty ? (DateTime?)dr["DeliveryDate"] : (DateTime?)null;
            this.acceptanceDate = dr["AcceptanceDate"].ToString() != string.Empty ? (DateTime?)dr["AcceptanceDate"] : (DateTime?)null;
            this.acceptanceFlag = int.Parse(dr["AcceptanceFlag"].ToString() != string.Empty ? dr["AcceptanceFlag"].ToString() : "0");
            this.orderAmount = decimal.Parse(dr["OrderAmount"].ToString() != string.Empty ? dr["OrderAmount"].ToString() : "0");
            this.statusFlag = int.Parse(dr["StatusFlag"].ToString() != string.Empty ? dr["StatusFlag"].ToString() : "0");
        }
        #endregion
    }
}
