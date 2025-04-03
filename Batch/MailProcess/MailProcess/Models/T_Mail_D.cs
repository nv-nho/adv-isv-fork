using System;
using System.Data.Common;

namespace MailProcess.Models
{
    /// <summary>
    /// Class T_Mail_D Model
    /// </summary>
    [Serializable]
    public class T_Mail_D
    {
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

        /// <summary>
        /// IsChange
        /// </summary>
        public bool IsChange { get; set; }

        #endregion

        #region Contructor

        /// <summary>
        /// Contructor T_Mail_D
        /// </summary>
        public T_Mail_D()
        {
            this.IsChange = false;
        }

        /// <summary>
        /// Contructor M_User
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public T_Mail_D(DbDataReader dr)
        {
            this.HID = (int)dr["HID"];
            this.UID = (int)dr["UID"];
            this.MailAddress = (string)dr["MailAddress"];
            this.ReceiveDate = string.IsNullOrEmpty(dr["ReceiveDate"].ToString()) ? null : (DateTime?)dr["ReceiveDate"];
            this.ReceiveFlag = int.Parse(string.Format("{0}", dr["ReceiveFlag"]));
            this.MailPath = (string)dr["MailPath"];
            this.IsChange = false;
        }

        #endregion

        #region Method
        /// <summary>
        /// SetValue
        /// </summary>
        /// <param name="receiveDate"></param>
        /// <param name="receiveFlag"></param>
        /// <param name="mailPath"></param>
        public void SetValue(DateTime receiveDate, int receiveFlag, string mailPath)
        {
            if (receiveDate != this.ReceiveDate)
            {
                this.ReceiveDate = receiveDate;
                this.IsChange = true;
            }

            if (receiveFlag != this.ReceiveFlag)
            {
                this.ReceiveFlag = receiveFlag;
                this.IsChange = true;
            }

            if (mailPath != this.MailPath)
            {
                this.MailPath = mailPath;
                this.IsChange = true;
            }
        }
        #endregion
    }
}
