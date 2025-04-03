using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using OMS.Utilities;

namespace OMS.Models
{
    [Serializable]
    public class T_Work_H
    {
        #region Variant

        /// <summary>
        /// hid
        /// </summary>
        private int hid { get; set; }

        /// <summary>
        /// uid
        /// </summary>
        private int uid { get; set; }

        /// <summary>
        /// date
        /// </summary>
        private DateTime date { get; set; }

        /// <summary>
        /// totalWorkingHours
        /// </summary>
        private int totalworkinghours { get; set; }

        /// <summary>
        /// createDate
        /// </summary>
        public DateTime createdate { get; set; }

        /// <summary>
        /// createUID
        /// </summary>
        public int createUID { get; set; }
        
        /// <summary>
        /// updateDate
        /// </summary>
        public DateTime updateDate { get; set; }
        
        /// <summary>
        /// updateUID
        /// </summary>
        public int updateUID { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        private DataStatus _status;

        #endregion
        #region Property
        /// <summary>
        /// Get or set HID
        /// </summary>
        public int HID
        {
            get { return hid; }
            set
            {
                if (value != hid)
                {
                    hid = value;
                    this._status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set UID
        /// </summary>
        public int UID
        {
            get { return uid; }
            set
            {
                if (value != uid)
                {
                    uid = value;
                    this._status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set Date
        /// </summary>
        public DateTime Date
        {
            get { return date; }
            set
            {
                if (value != date)
                {
                    date = value;
                    this._status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set TotalWorkingHours
        /// </summary>
        public int TotalWorkingHours
        {
            get { return totalworkinghours; }
            set
            {
                if (value != totalworkinghours)
                {
                    totalworkinghours = value;
                    this._status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set CreateDate
        /// </summary>
        public DateTime CreateDate
        {
            get { return createdate; }
            set
            {
                if (value != createdate)
                {
                    createdate = value;
                    this._status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set CreateUID
        /// </summary>
        public int CreateUID
        {
            get { return createUID; }
            set
            {
                if (value != createUID)
                {
                    createUID = value;
                    this._status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set UpdateDate
        /// </summary>
        public DateTime UpdateDate
        {
            get { return updateDate; }
            set
            {
                if (value != updateDate)
                {
                    updateDate = value;
                    this._status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set UpdateUID
        /// </summary>
        public int UpdateUID
        {
            get { return updateUID; }
            set
            {
                if (value != updateUID)
                {
                    updateUID = value;
                    this._status = DataStatus.Changed;
                }
            }
        }

        #endregion

        #region Contructor

        /// <summary>
        /// Contructor T_Work_H
        /// </summary>
        public T_Work_H()
            : base()
        {
            this.totalworkinghours = 0;
        }
        /// <summary>
        /// Contructor T_Work_H
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public T_Work_H(DbDataReader dr)
        {
            ISecurity sec = Security.Instance;
            this.hid = (int)dr["HID"];
            this.uid = (int)dr["UID"];
            this.date = (DateTime)dr["Date"];
            this.totalworkinghours = (int)dr["TotalWorkingHours"];
            this.createdate = (DateTime)dr["CreateDate"];
            this.createUID = (int)dr["CreateUID"];
            this.updateDate = (DateTime)dr["UpdateDate"];
            this.updateUID = (int)dr["UpdateUID"];
        }
        #endregion
    }
}
