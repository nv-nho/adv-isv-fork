using System;
using System.Data.Common;
using OMS.Utilities;

namespace OMS.Models
{
    /// <summary>
    /// Class T_Mail_D Model
    /// </summary>
    [Serializable]
    public class T_Mail_D
    {
        #region Contanst

        /// <summary>
        /// Max length of MailAddress
        /// </summary>
        public const int MAIL_ADDRESS_MAX_LENGTH = 100;
        /// <summary>
        /// Max length of MailPath
        /// </summary>
        public const int MAIL_PATH_MAX_LENGTH = 255;

        #endregion

        #region Variant

        /// <summary>
        /// Header ID
        /// </summary>
        public int HID { get; set; }

        /// <summary>
        /// Header ID
        /// </summary>
        public int UID { get; set; }

        /// <summary>
        /// MailAddress
        /// </summary>
        public string MailAddress { get; set; }

        /// <summary>
        /// ReceiveDate
        /// </summary>
        public DateTime? ReceiveDate { get; set; }

        /// <summary>
        /// ReceiveFlag
        /// </summary>
        public int ReceiveFlag { get; set; }

        /// <summary>
        /// MailPath
        /// </summary>
        public string MailPath { get; set; }

        #endregion

        #region Contructor

        /// <summary>
        /// Contructor T_Mail_D
        /// </summary>
        public T_Mail_D()
        {}

        /// <summary>
        /// Contructor M_User
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public T_Mail_D(DbDataReader dr)
        {
            this.HID = (int)dr["HID"];
            this.UID = (int)dr["UID"];
            this.MailAddress = (string)dr["MailAddress"];
            this.ReceiveDate = dr["ReceiveDate"].ToString() != string.Empty ? (DateTime?)dr["ReceiveDate"] : (DateTime?)null;
            this.ReceiveFlag = int.Parse(dr["ReceiveFlag"].ToString());
            this.MailPath = (string)dr["MailPath"];
        }

        #endregion
    }
}
