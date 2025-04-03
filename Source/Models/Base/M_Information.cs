using System;
using System.Data.Common;

namespace OMS.Models
{
    /// <summary>
    /// Class M_Information model
    /// </summary>
    [Serializable]
    public class M_Information : M_Base<M_Information>
    {
        #region Constant

        /// <summary>
        /// Max length of Information Name
        /// </summary>
        public const int INFORMATION_NAME_MAX_LENGTH = 150;

        /// <summary>
        /// Max length of Information Name
        /// </summary>
        public const int INFORMATION_CONTENT_MAX_LENGTH = 500;

        #endregion

        #region Variable

        /// <summary>
        /// Information Name
        /// </summary>
        private string informationName;

        /// <summary>
        /// Begin Date
        /// </summary>
        private DateTime beginDate;

        /// <summary>
        /// End Date
        /// </summary>
        private DateTime endDate;

        /// <summary>
        /// Information Content
        /// </summary>
        private string informationContent;

        #endregion

        #region Property

        /// <summary>
        /// Get or set InformationName
        /// </summary>
        public string InformationName
        {
            get { return this.informationName; }
            set
            {
                if (value != this.informationName)
                {
                    this.informationName = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set BeginDate
        /// </summary>
        public DateTime BeginDate
        {
            get { return this.beginDate; }
            set
            {
                if (value != this.beginDate)
                {
                    this.beginDate = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set EndDate
        /// </summary>
        public DateTime EndDate
        {
            get { return this.endDate; }
            set
            {
                if (value != this.endDate)
                {
                    this.endDate = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set InformationContent
        /// </summary>
        public string InformationContent
        {
            get { return this.informationContent; }
            set
            {
                if (value != this.informationContent)
                {
                    this.informationContent = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        #endregion

        #region Contructor

        /// <summary>
        /// Contructor M_Information
        /// </summary>
        public M_Information()
            : base()
        { }

        /// <summary>
        /// Contructor M_Information with data reader
        /// </summary>
        /// <param name="dr"></param>
        public M_Information(DbDataReader dr)
            : base(dr)
        {
         
            this.informationName = (string)dr["InformationName"];
            this.beginDate = (DateTime)dr["BeginDate"];
            this.endDate = (DateTime)dr["EndDate"];
            this.informationContent = (string)dr["InformationContent"];
        }

        #endregion
    }
}