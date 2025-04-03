using OMS.Utilities;
using System;
using System.Data.Common;
using System.Web.Script.Serialization;

namespace OMS.Models
{
    /// <summary>
    /// Class T_Cost_D Model
    /// </summary>
    [Serializable]
    public class T_Cost_D : M_Base<T_Cost_D>
    {
        #region Variant
        /// <summary>
        /// Delete flag
        /// </summary>
        private bool delFlg;

        /// <summary>
        /// hid
        /// </summary>
        private int hid { get; set; }

        /// <summary>
        /// effectDate
        /// </summary>
        public DateTime? effectdate { get; set; }

        /// <summary>
        /// expireDate
        /// </summary>
        public DateTime? expiredate { get; set; }

        /// <summary>
        /// costAmount
        /// </summary>
        public Decimal? costamount { get; set; }
        #endregion

        #region Constant
        /// <summary>
        /// Max length of Cost amount
        /// </summary>
        public const decimal COST_AMOUNT_MAX_VALUE = 999999.99M;
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
        /// Get or set HID
        /// </summary>
        public int HID
        {
            get { return hid; }
            set
            {
                if (value != hid)
                {
                    hid = value;
                }
            }
        }

        /// <summary>
        /// Get or set EffectDate
        /// </summary>
        [ScriptIgnore]
        public DateTime? EffectDate
        {
            get { return this.effectdate; }
            set { this.effectdate = value; }
        }

        /// <summary>
        /// Get or set ExpireDate
        /// </summary>
        [ScriptIgnore]
        public DateTime? ExpireDate
        {
            get { return this.expiredate; }
            set { this.expiredate = value; }
        }

        /// <summary>
        /// Get or set CostAmount
        /// </summary>
        public decimal? CostAmount
        {
            get { return this.costamount; }
            set { this.costamount = value; }
        }
        #endregion

        #region Contructor

        /// <summary>
        /// Contructor T_Cost_D
        /// </summary>
        public T_Cost_D()
            : base()
        {

        }
        /// <summary>
        /// Contructor T_Cost_D
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public T_Cost_D(DbDataReader dr)
        {
            ISecurity sec = Security.Instance;
            this.hid = (int)dr["HID"];
            this.effectdate = (DateTime)dr["EffectDate"];
            this.expiredate = (DateTime)dr["ExpireDate"];
            this.costamount = decimal.Parse(dr["CostAmount"].ToString() != string.Empty ? dr["CostAmount"].ToString() : "0");
        }
        #endregion
    }
}
