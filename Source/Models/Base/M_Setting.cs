using System;
using System.Data.Common;

namespace OMS.Models
{
    /// <summary>
    /// Config service
    /// </summary>
    [Serializable]
    public class M_Setting : M_Base<M_Setting>
    {
        #region Constant
        public const int QUOTE_NO_MAX_LENGTH = 6;
        public const int SALES_NO_MAX_LENGTH = 6;
        public const int PURCHASE_NO_MAX_LENGTH = 6;
        public const int BILLING_NO_MAX_LENGTH = 6;
        public const int DELIVERY_NO_MAX_LENGTH = 6;
        public const int ATTACH_PATH_MAX_LENGTH = 225;
        public const int SALES_CONTRACT_FILE_MAX_LENGTH = 225;
        public const int ACCEPTANCE_REPORT_FILE_MAX_LENGTH = 225;
        public const int LOGO_1_MAX_LENGTH = 225;
        public const int LOGO_2_MAX_LENGTH = 225;
        public const int EXTENSION_MAX_LENGTH = 225;

        #endregion

        #region Variable

        /// <summary>
        /// Logo1
        /// </summary>
        public string _logo1;

        /// <summary>
        /// Logo2
        /// </summary>
        public string _logo2;

        /// <summary>
        /// AttachPath
        /// </summary>
        public string _attachPath;

        /// <summary>
        /// Extension
        /// </summary>
        public string _extension;

        #endregion

        #region Property

        /// <summary>
        /// Get or set Logo1
        /// </summary>
        public string Logo1
        {
            get { return this._logo1; }
            set
            {
                if (value != this._logo1)
                {
                    this._logo1 = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set Logo2
        /// </summary>
        public string Logo2
        {
            get { return this._logo2; }
            set
            {
                if (value != this._logo2)
                {
                    this._logo2 = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set AttachPath
        /// </summary>
        public string AttachPath
        {
            get { return this._attachPath; }
            set
            {
                if (value != this._attachPath)
                {
                    this._attachPath = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set Extension
        /// </summary>
        public string Extension
        {
            get { return this._extension; }
            set
            {
                if (value != this._extension)
                {
                    this._extension = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        #endregion

        #region Contructor
        /// <summary>
        /// Contructor M_Setting
        /// </summary>
        public M_Setting()
            : base()
        {

        }

        /// <summary>
        /// Contructor M_Setting
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public M_Setting(DbDataReader dr)
            : base(dr)
        {
            this._logo1 = (string)dr["Logo1"];
            this._logo2 = (string)dr["Logo2"];
            this._attachPath = (string)dr["AttachPath"];
            this._extension = (string)dr["Extension"];
        }
        #endregion
    }
}
