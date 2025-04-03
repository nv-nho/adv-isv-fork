using System;
using System.Data.Common;
using OMS.Utilities;

namespace OMS.Models
{
    /// <summary>
    /// Class T_Expence_H Model
    /// </summary>
    [Serializable]
    public class T_Payslip : M_Base<T_Payslip>
    {
        #region Constant
        #endregion

        #region Variant

        /// <summary>
        /// Year
        /// </summary>
        private string _year;

        /// <summary>
        /// Type
        /// </summary>
        private int _type;

        /// <summary>
        /// UserID
        /// </summary>
        private int _userID;

        /// <summary>
        /// Filepath
        /// </summary>
        private string _filepath;

        /// <summary>
        /// UploadDate
        /// </summary>
        private DateTime? _uploadDate;

        /// <summary>
        /// DownloadDate
        /// </summary>
        private DateTime? _downloadDate;

        #endregion

        #region Property
        /// <summary>
        /// Get or set Year
        /// </summary>
        public string Year
        {
            get { return _year; }
            set
            {
                if (value != _year)
                {
                    _year = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set Type
        /// </summary>
        public int Type
        {
            get { return _type; }
            set
            {
                if (value != _type)
                {
                    _type = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set UserID
        /// </summary>
        public int UserID
        {
            get { return _userID; }
            set
            {
                if (value != _userID)
                {
                    _userID = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        ///  Get or set Filepath
        /// </summary>
        public string Filepath
        {
            get { return _filepath; }
            set
            {
                if (value != _filepath)
                {
                    _filepath = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set Date
        /// </summary>
        public DateTime? UploadDate
        {
            get { return _uploadDate; }
            set
            {
                if (value != _uploadDate)
                {
                    _uploadDate = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set Date
        /// </summary>
        public DateTime? DownloadDate
        {
            get { return _downloadDate; }
            set
            {
                if (value != _downloadDate)
                {
                    _downloadDate = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        #endregion

        #region Contructor

        /// <summary>
        /// Contructor T_Expence_H
        /// </summary>
        public T_Payslip()
            : base()
        {

        }

        /// <summary>
        /// Contructor T_Expence_H
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public T_Payslip(DbDataReader dr)
            : base(dr)
        {
            this.Year = (string)dr["Year"];
            this.Type = int.Parse(dr["Type"].ToString());
            this.UserID = int.Parse(dr["UserID"].ToString());
            this.Filepath = string.Format("{0}", dr["Filepath"]);
            if (dr["UploadDate"] == DBNull.Value)
            {
                this.UploadDate = null;
            }else
            {
                this.UploadDate = (DateTime)dr["UploadDate"];
                
            }
            if (dr["DownloadDate"] == DBNull.Value)
            {
                this.DownloadDate = null;
            }
            else
            {
                this.DownloadDate = (DateTime)dr["DownloadDate"];

            }
        }

        #endregion
    }
}
