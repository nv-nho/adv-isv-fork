using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using OMS.Utilities;

namespace OMS.Models
{
    [Serializable]
    public class T_PaidLeave
    {
        #region Variant

        /// <summary>
        /// 内部ID
        /// </summary>
        private int id { get; set; }

        /// <summary>
        /// 勤務カレンダーID
        /// </summary>
        private int calendarID { get; set; }

        /// <summary>
        /// ユーザーID
        /// </summary>
        private int userID { get; set; }

        /// <summary>
        /// 有給取得予定日
        /// </summary>
        private DateTime date { get; set; }

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
        /// Get or set 内部ID
        /// </summary>
        public int ID
        {
            get { return id; }
            set
            {
                if (value != id)
                {
                    id = value;
                    this._status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set 勤務カレンダーID
        /// </summary>
        public int CalendarID
        {
            get { return calendarID; }
            set
            {
                if (value != calendarID)
                {
                    calendarID = value;
                    this._status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set ユーザーID
        /// </summary>
        public int UserID
        {
            get { return userID; }
            set
            {
                if (value != userID)
                {
                    userID = value;
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
        public T_PaidLeave()
            : base()
        {
        }
        /// <summary>
        /// Contructor T_Work_H
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public T_PaidLeave(DbDataReader dr)
        {
            ISecurity sec = Security.Instance;
            this.id = (int)dr["ID"];
            this.calendarID = (int)dr["CalendarID"];
            this.userID = (int)dr["UserID"];
            this.date = (DateTime)dr["Date"];
            this.createdate = (DateTime)dr["CreateDate"];
            this.createUID = (int)dr["CreateUID"];
            this.updateDate = (DateTime)dr["UpdateDate"];
            this.updateUID = (int)dr["UpdateUID"];
        }
        #endregion
    }
}
