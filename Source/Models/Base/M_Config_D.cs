using System;
using System.Data.Common;

namespace OMS.Models
{
    /// <summary>
    /// Class M_Config_D
    /// </summary>
    [Serializable]
    public class M_Config_D
    {
        #region Constant

        /// <summary>
        /// Max length of Value2
        /// </summary>
        public const int VALUE2_MAX_LENGTH = 50;

        /// <summary>
        /// Max length of Value3
        /// </summary>
        public const int VALUE3_MAX_LENGTH = 50;

        /// <summary>
        /// Max length of Value4
        /// </summary>
        public const int VALUE4_MAX_LENGTH = 50;

        #endregion

        #region VALUE_CONST

        /// <summary>
        /// DATA : WITHOUT
        /// </summary>
        public const string DATA_WITHOUT = "0";

        /// <summary>
        /// DATA : ONLY
        /// </summary>
        public const string DATA_ONLY = "1";

        /// <summary>
        /// DATA : INCLUDE
        /// </summary>
        public const string DATA_INCLUDE = "2";
        
        /// <summary>
        /// VAT TYPE : EXCLUDE
        /// </summary>
        public const string VAT_TYPE_EXCLUDE = "0";

        /// <summary>
        /// VAT_TYPE : INCLUDE
        /// </summary>
        public const string VAT_TYPE_INCLUDE = "1";


        /// <summary>
        /// QUANTITY_NOT_DECIMAL : Not Decimal
        /// </summary>
        public const string QUANTITY_NOT_DECIMAL = "0";

        /// <summary>
        /// QUANTITY_DECIMAL : Decimal
        /// </summary>
        public const string QUANTITY_DECIMAL = "1";

        /// <summary>
        /// DEFAULT_VIEW : Hide
        /// </summary>
        public const string DEFAULT_VIEW_HIDE = "0";

        /// <summary>
        /// DEFAULT_VIEW : Show
        /// </summary>
        public const string DEFAULT_VIEW_SHOW = "1";

        #endregion

        #region Variable
        
        /// <summary>
        /// Header ID
        /// </summary>
        public int HID { get; set; }
        
        /// <summary>
        /// No
        /// </summary>
        public int No { get; set; }
        
        /// <summary>
        /// Value1
        /// </summary>
        public int Value1 { get; set; }
        
        /// <summary>
        /// Value2
        /// </summary>
        public string Value2 { get; set; }
        
        /// <summary>
        /// Value3
        /// </summary>
        public string Value3 { get; set; }
        
        /// <summary>
        /// Value4
        /// </summary>
        public string Value4 { get; set; }

        /// <summary>
        /// Delete Flag
        /// Use for delete row
        /// </summary>
        public bool DelFlag { get; set; }
        
        #endregion

        #region Contructor

        /// <summary>
        /// Contructor of M_Config_D
        /// </summary>
        public M_Config_D()
        {
        }

        /// <summary>
        /// Contructor of M_Setting_D
        /// </summary>
        /// <param name="dr">Database data reader</param>
        public M_Config_D(DbDataReader dr)
        {
            this.HID = (int)dr["HID"];
            this.No = (int)dr["No"];
            this.Value1 = (int)dr["Value1"];
            this.Value2 = (string)dr["Value2"];
            this.Value3 = (string)dr["Value3"];
            this.Value4 = (string)dr["Value4"];
        }

        #endregion
    }
}
