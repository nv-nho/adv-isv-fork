using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OMS.Utilities
{
    public class Constants
    {
        #region No

        public const int MAX_NO = 9999;
        public const int MAX_GRID_ROW_COUNT = 10000;
        public const int ADD_MONTH_SEARCH = -6;
        public const int MAX_TAB_COST = 100;

        public const int MAX_QUANTITY = 99999;
        public const string DEFAULT_VALUE_STRING = "-1";
        public const int DEFAULT_VALUE_INT = -1;
        #endregion

        public const string SPECIALS_D = "{\"${33}\":\"!\",\"${64}\":\"@\",\"${35}\":\"#\",\"${36}\":\"$\",\"${37}\":\"%\",\"${94}\":\"^\",\"${38}\":\"&\",\"${42}\":\"*\",\"${40}\":\"(\",\"${41}\":\")\"}";
        public const string SPECIALS_E = "{\"!\":\"${33}\",\"@\":\"${64}\",\"#\":\"${35}\",\"$\":\"${36}\",\"%\":\"${37}\",\"^\":\"${94}\",\"&\":\"${38}\",\"*\":\"${42}\",\"(\":\"${40}\",\")\":\"${41}\"}";
        public const char CHAR_ZERO = '0';

        #region pattern
        public const string PATTERN_NUMERIC = "^[0-9]*$";

        //public const string PATTERN_NUMERIC = "^[0-9]*$";
        //public const string PATTERN_ANPHA_NUMERIC = "^[a-zA-Z0-9]*$";
        //public const string PATTERN_NUMERIC_SUBTRACT = "^[0-9\\-]*$";
        //public const string PATTERN_UPPER_ANPHA_NUMERIC = "^[A-Z0-9]*$";
        //public const string PATTERN_ANPHA_NUMERIC_FLASH = "^[a-zA-Z0-9\\-\\/\\ ]*$";
        //public const string PATTERN_UPPER_ANPHA_NUMERIC_FLASH = "^[A-Z0-9\\-\\/]*$";
        //public const string PATTERN_UPPER_ANPHA_NUMERIC_SUBTRACT = "^[A-Z0-9\\-]*$";
        public const string PATTERN_TEL = "^[0-9() \\+\\-\\.]*$";
        public const string PATTERN_ACCOUNT_NO = "^[a-zA-Z0-9 \\-\\.]*$";
        public const string PATTERN_TAX_CODE = "^[0-9() \\-]*$";
        //public const string PATTERN_HAFL_WIDTH = "^[a-zA-Z0-9｡-ﾟ\\p{P}\\p{S}]*$";
        //public const string PATTERN_EMAIL = "^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$";

        #endregion

        #region Format
        public const string NUMBER_FORMAT_INT = "{0:#,##0}";
        public const string NUMBER_FORMAT_DEC_2 = "{0:#,##0.00}";

        public const string FMT_DATETIME = "yyyyMMddHHmmss";
        public const string FMT_DATETIME_SHOW = "yyyy/MM/dd HH:mm:ss";

        public const string FMT_DATETIME_REPORT_SHOW = "dd/MM/yyyy HH:mm";

        public const string FMT_YYYYMM = "yyyy/MM";
        public const string FMT_DMY = "ddMMyyyy";
        public const string FMT_YM = "yyyyMM";
        public const string FMT_YMDHMM = "yyMMddHHmm";
        public const string FMT_YMD = "yyyyMMdd";
        public const string FMT_YMD_JP = "yyyy年MM月dd日";
        public const string FMT_DATE = "dd/MM/yyyy";
        public const string FMT_DATE_EN = "yyyy/MM/dd";
        public const string FMT_DATE_TIME = "dd/MM/yyyy HH:mm:ss";
        public const string FMT_DATE_TIME_EN = "yyyy/MM/dd HH:mm:ss";
        public const string FMR_DATE_YMD_HMS = "{0:yyyy/MM/dd HH:mm:ss}";
        public const string FMR_DATE_YMD = "{0:yyyy/MM/dd}";
        public const string FMT_DATE_DPL = "{0:dd/MM/yyyy}";
        public const string FMT_YYYYMMDD = "{0:yyyyMMdd}";
        public const string FMR_DATE_YM = "{0:yyyy/MM}";
        public const string FMR_DATE_M = "{0:MM}";
        public const string FMT_INTEGER = "#,##0";
        public const string FMT_INTEGER_F = "#,##0;\"▲ \"#,##0";
        public const string FMT_DECIMAL = "#,##0.00";
        public const string FMT_DCM = "#,##0.##";
        public const string FMT_INTEGER_DPL = "{0:#,##0}";
        public const string FMT_DECIMAL_FULL = "{0:#,##0.#############################}";

        public const string MIN_DATE_TEST = "01/01/2013";
        public const string MAX_DATE_TEST = "31/12/2015";

        #endregion

        /** - */
        public const string HYPHEN = "-";
        public const char HYPHEN_CHAR = '-';
        public const string DFT_CULTURE_NAME = "ja-JP";

        /// <summary>
        /// Date Length
        /// </summary>
        public const string DATE_LENGTH = "10";
    }
}