using System;
using System.Data.Common;

namespace OMS.Models
{
    /// <summary>
    /// Class Seting header
    /// </summary>
    [Serializable]
    public class M_Config_H : M_Base<M_Config_H>
    {
        #region Constant

        /// <summary>
        /// Max length of Config code
        /// </summary>
        public const int CONFIG_CODE_MAX_LENGTH = 4;

        /// <summary>
        /// Max length of config name
        /// </summary>
        public const int CONFIG_NAME_MAX_LENGTH = 50;

        public const string CONFIG_CD_WORKING_TYPE = "C001";
        public const string CONFIG_CD_BREAK_TYPE = "C002";
        public const string CONFIG_CD_HOLIDAY_TYPE = "C003";
        public const string CONFIG_CD_OVER_TIME_TYPE = "C004";
        public const string CONFIG_CD_VACATION_TYPE = "C005";

        /// <summary>
        /// Config cd paging
        /// </summary>
        public const string CONFIG_CD_PAGING = "C006";

        /// <summary>
        /// 無効データ検索  コンボボックス
        /// </summary>
        public const string CONFIG_CD_INVALID_TYPE = "C007";

        /// <summary>
        /// ユーザー登録制限数　Max Active User No.
        /// </summary>
        public const string CONFIG_CD_MAX_ACTIVE_USER = "C008";

        /// <summary>
        /// CONFIG_GROUP_USERCD_ADMIN
        /// </summary>
        public const string CONFIG_GROUP_USERCD_ADMIN = "C009";

        /// <summary>
        /// CONFIG_PAYSLIP_UPLOAD_FILE_EXTENSION
        /// </summary>
        public const string CONFIG_PAYSLIP_UPLOAD_FILE_EXTENSION = "C010";

        /// <summary>
        /// CONFIG_UPLOAD_CLASSIFICATION
        /// </summary>
        public const string CONFIG_UPLOAD_CLASSIFICATION = "C011";

        /// <summary>
        /// CONFIG_BASE_DIVISION
        /// </summary>
        public const string CONFIG_BASE_DIVISION = "C012";

        /// <summary>
        /// CONFIG_START_DIVISION
        /// </summary>
        public const string CONFIG_START_DIVISION = "C013";

        /// <summary>
        /// CONFIG_MAIL_SEND_NAME
        /// </summary>
        public const string CONFIG_MAIL_SEND_NAME = "C014";
        

        /// <summary>
        /// 承認状況色
        /// </summary>
        public const string APPROVAL_STATUS = "C017";

        /// <summary>
        /// 申請状態
        /// </summary>
        public const string CONFIG_APPROVAL_TYPE = "C018";

        /// <summary>
        /// CONFIG_APPROVAL_USE
        /// </summary>
        public const string CONFIG_USE_TIME_APPROVAL = "C019";

        /// <summary>
        /// 経費種類
        /// </summary>
        public const string CONFIG_CD_EXPENCE_TYPE = "E001";

        /// <summary>
        /// 区分
        /// </summary>
        public const string CONFIG_CD_ROUTE_TYPE = "E002";

        /// <summary>
        /// 税区分
        /// </summary>
        public const string CONFIG_CD_TAX_TYPE = "E003";

        /// <summary>
        /// 税率
        /// </summary>
        public const string CONFIG_CD_TAX_RATE = "E004";

        /// <summary>
        /// 
        /// </summary>
        public const string CONFIG_CD_INTERVAL_TIME = "E005";

        #endregion

        #region Variable

        /// <summary>
        /// Config Code
        /// </summary>
        public string ConfigCD { get; set; }

        /// <summary>
        /// Setting Name
        /// </summary>
        private string configName;

        #endregion

        #region Property

        /// <summary>
        /// Get or Set Config Name
        /// </summary>
        public string ConfigName
        {
            get { return configName; }
            set
            {
                if (value != this.configName)
                {
                    this.configName = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        #endregion

        #region Contructor

        /// <summary>
        /// Contructor M_Setting_H
        /// </summary>
        public M_Config_H()
            : base()
        { 
        }

        /// <summary>
        /// Contructor M_Setting_H
        /// </summary>
        /// <param name="dr"></param>
        public M_Config_H(DbDataReader dr)
            : base(dr)
        {
            this.ConfigCD = (string)dr["ConfigCD"];
            this.configName = (string)dr["ConfigName"];
        }


        #endregion

    }
}
