using System;
using System.Data.Common;
using OMS.Utilities;

namespace OMS.Models
{
    /// <summary>
    /// Class T_PaidVacationService Model
    /// </summary>
    [Serializable]
    public class T_PaidVacation
    {
        #region Contanst

        /// <summary>
        /// Max length of Year
        /// </summary>
        public const int YEAR_MAX_LENGTH = 4;

        #endregion

        #region Variant

        /// <summary>
        /// UID
        /// </summary>
        public int UID { get; set; }

        /// <summary>
        /// No
        /// </summary>
        public int No { get; set; }

        /// <summary>
        /// Year
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// VacationDay
        /// </summary>
        public decimal? VacationDay { get; set; }

        /// <summary>
        /// InvalidFlag
        /// </summary>
        public short InvalidFlag { get; set; }

        /// <summary>
        /// Delete Flag
        /// Use for delete row
        /// </summary>
        public bool DelFlag { get; set; }

        /// <summary>
        /// CreateDate
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// CreateUID
        /// </summary>
        public int CreateUID { get; set; }
        /// <summary>
        /// UpdateDate
        /// </summary>
        public DateTime UpdateDate { get; set; }
        /// <summary>
        /// UpdateUID
        /// </summary>
        public int UpdateUID { get; set; }

        /// <summary>
        /// Status (Is change data)
        /// </summary>
        public DataStatus Status { get; protected set; }

        #endregion

        #region Contructor

        /// <summary>
        /// Contructor M_PaidVacation
        /// </summary>
        public T_PaidVacation()
        {
            this.Status = DataStatus.None;
        }

        /// <summary>
        /// Contructor M_PaidVacation
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public T_PaidVacation(DbDataReader dr)
        {
            this.UID = (int)dr["UID"];
            this.Year = (int)dr["Year"];
            this.VacationDay = (decimal)dr["VacationDay"];
            this.InvalidFlag = short.Parse(dr["InvalidFlag"].ToString());
            this.CreateDate = (DateTime)dr["CreateDate"];
            this.CreateUID = (int)dr["CreateUID"];
            this.UpdateDate = (DateTime)dr["UpdateDate"];
            this.UpdateUID = (int)dr["UpdateUID"];
            this.Status = DataStatus.None;
        }

        #endregion
    }
}
