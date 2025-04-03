using System;
using System.Data.Common;

namespace OMS.Models
{
    /// <summary>
    /// Base Class
    /// </summary>
    [Serializable]
    public class M_Base<T>
         where T : class 
    {
        #region Property

        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

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
        /// Contructor M_Base
        /// </summary>
        public M_Base()
        {
            this.Status = DataStatus.None;
        }

        /// <summary>
        /// Contructor M_Base
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public M_Base(DbDataReader dr)
        {
            this.ID = (int)dr["ID"];
            this.CreateDate = (DateTime)dr["CreateDate"];
            this.CreateUID = (int)dr["CreateUID"];
            this.UpdateDate = (DateTime)dr["UpdateDate"];
            this.UpdateUID = (int)dr["UpdateUID"];
            this.Status = DataStatus.None;
        }

        #endregion

        /// <summary>
        /// Clone Object
        /// </summary>
        /// <typeparam name="T">Class to clone</typeparam>
        /// <returns></returns>
        public T Clone()
        {
            return (T)base.MemberwiseClone();
        }

    }

    /// <summary>
    /// Base Report
    /// Author: ISV-GIAM
    /// </summary>
    public abstract class BaseReport<T>
    {
        public T Clone()
        {
            return (T)this.MemberwiseClone();
        }


        public System.Collections.Generic.List<T> DataItems()
        {
            return null;
        }
    }

    /// <summary>
    /// Base search model
    /// </summary>
    public class BaseSearch
    {
        /// <summary>
        /// PageIndex
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Page Size
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Sort Field
        /// </summary>
        public int SortField { get; set; }

        /// <summary>
        /// Sort Direc
        /// </summary>
        public int SortDirec { get; set; }

    }
}
