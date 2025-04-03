using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using OMS.Utilities;

namespace OMS.Models
{
    [Serializable]
    public class T_Cost_H
    {
        #region Variant
        /// <summary>
        /// id
        /// </summary>
        private int id { get; set; }
        /// <summary>
        /// costCode
        /// </summary>
        private string costcode { get; set; }
        /// <summary>
        /// costName
        /// </summary>
        private string costname { get; set; }
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

        #region Constant
        /// <summary>
        /// Max length of Cost code
        /// </summary>
        public const int COST_CODE_MAX_LENGTH = 5;
        /// <summary>
        /// Max length of Cost name
        /// </summary>
        public const int COST_NAME_MAX_LENGTH = 30;
        #endregion

        #region Property
        /// <summary>
        /// Get or set HID
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
        /// Get or set CostCode
        /// </summary>
        public string CostCode
        {
            get { return costcode; }
            set
            {
                if (value != costcode)
                {
                    costcode = value;
                    this._status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set CostName
        /// </summary>
        public string CostName
        {
            get { return costname; }
            set
            {
                if (value != costname)
                {
                    costname = value;
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

        /// <summary>
        /// Delete Flag
        /// Use for delete row
        /// </summary>
        public bool DelFlag { get; set; }

        /// <summary>
        /// Checked
        /// </summary>
        public bool Checked { get; set; }


        #endregion

        #region Contructor

        /// <summary>
        /// Contructor T_Work_H
        /// </summary>
        public T_Cost_H()
            : base()
        {
            ///
        }
        /// <summary>
        /// Contructor T_Cost_H
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public T_Cost_H(DbDataReader dr)
        {
            ISecurity sec = Security.Instance;
            this.id = (int)dr["ID"];
            this.costcode = (string)dr["CostCode"];
            this.costname = (string)dr["CostName"];
            this.createdate = (DateTime)dr["CreateDate"];
            this.createUID = (int)dr["CreateUID"];
            this.updateDate = (DateTime)dr["UpdateDate"];
            this.updateUID = (int)dr["UpdateUID"];
        }
        #endregion
    }
}
