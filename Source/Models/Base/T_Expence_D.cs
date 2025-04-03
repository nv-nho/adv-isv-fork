using System;
using System.Data.Common;
using OMS.Utilities;

namespace OMS.Models
{
    /// <summary>
    /// Class T_Expence_D Model
    /// </summary>
    [Serializable]
    public class T_Expence_D : M_Base<T_Expence_D>
    {
        #region Variant
        /// <summary>
        /// Delete flag
        /// </summary>
        private bool delFlg;

        /// <summary>
        /// hid
        /// </summary>
        private int hid;

        /// <summary>
        /// 行番号
        /// </summary>
        private int lineNo;

        /// <summary>
        /// 利用日
        /// </summary>
        private DateTime? date;

        /// <summary>
        /// 支払先
        /// </summary>
        private string paidTo;

        /// <summary>
        /// 経路1
        /// </summary>
        private string routeFrom;

        /// <summary>
        /// 経路2
        /// </summary>
        private string routeTo;

        /// <summary>
        /// 区分
        /// </summary>
        private int routeType;

        /// <summary>
        /// 目的
        /// </summary>
        private string note;

        /// <summary>
        /// 税区分
        /// </summary>
        private int taxType;

        /// <summary>
        /// 税率
        /// </summary>
        private decimal taxRate;

        /// <summary>
        /// 金額
        /// </summary>
        private decimal? amount;

        /// <summary>
        /// 消費税
        /// </summary>
        private decimal? taxAmount;

        #endregion
        #region Property
        /// <summary>
        /// Get,set DelFlag
        /// </summary>
        public bool DelFlag
        {
            get { return this.delFlg; }
            set { this.delFlg = value; }
        }
        /// <summary>
        /// Get or set hid
        /// </summary>
        public int HID
        {
            get { return hid; }
            set
            {
                if (value != hid)
                {
                    hid = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set LineNo
        /// </summary>
        public int LineNo
        {
            get { return lineNo; }
            set
            {
                if (value != lineNo)
                {
                    lineNo = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set Date
        /// </summary>
        public DateTime? Date
        {
            get { return date; }
            set
            {
                if (value != date)
                {
                    date = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set PaidTo
        /// </summary>
        public string PaidTo
        {
            get { return paidTo; }
            set
            {
                if (value != paidTo)
                {
                    paidTo = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set RouteFrom
        /// </summary>
        public string RouteFrom
        {
            get { return routeFrom; }
            set
            {
                if (value != routeFrom)
                {
                    routeFrom = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set RouteTo
        /// </summary>
        public string RouteTo
        {
            get { return routeTo; }
            set
            {
                if (value != routeTo)
                {
                    routeTo = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set RouteType
        /// </summary>
        public int RouteType
        {
            get { return routeType; }
            set
            {
                if (value != routeType)
                {
                    routeType = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set Note
        /// </summary>
        public string Note
        {
            get { return note; }
            set
            {
                if (value != note)
                {
                    note = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set TaxType
        /// </summary>
        public int TaxType
        {
            get { return taxType; }
            set
            {
                if (value != taxType)
                {
                    taxType = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set TaxRate
        /// </summary>
        public decimal TaxRate
        {
            get { return taxRate; }
            set
            {
                if (value != taxRate)
                {
                    taxRate = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set Amount
        /// </summary>
        public decimal? Amount
        {
            get { return amount; }
            set
            {
                if (value != amount)
                {
                    amount = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set TaxAmount
        /// </summary>
        public decimal? TaxAmount
        {
            get { return taxAmount; }
            set
            {
                if (value != taxAmount)
                {
                    taxAmount = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Checked
        /// </summary>
        public bool Checked { get; set; }

        #endregion

        #region Contructor

        /// <summary>
        /// Contructor T_Expence_D
        /// </summary>
        public T_Expence_D()
            : base()
        {
            this.taxRate = 0;
            this.amount = 0;
            this.taxAmount = 0;
            this.note = string.Empty;
           
        }
        /// <summary>
        /// Contructor T_Expence_D
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public T_Expence_D(DbDataReader dr)
        {
            ISecurity sec = Security.Instance;
            this.hid = int.Parse(dr["HID"].ToString());
            this.lineNo = int.Parse(dr["LineNo"].ToString());
            this.date = (DateTime)dr["Date"];
            this.paidTo = (string)dr["PaidTo"];
            this.routeFrom = dr["RouteFrom"].ToString() ?? string.Empty;
            this.routeTo = dr["RouteTo"].ToString() ?? string.Empty;
            this.routeType = int.Parse(dr["RouteType"].ToString());
            this.note = dr["Note"].ToString()??string.Empty;
            this.taxType = int.Parse(dr["TaxType"].ToString());
            this.taxRate = decimal.Parse(dr["TaxRate"].ToString());
            this.amount= decimal.Parse(dr["Amount"].ToString());
            this.taxAmount= decimal.Parse(dr["TaxAmount"].ToString());
        }
        #endregion
    }
}
