using System;
using System.Data.Common;
using OMS.Utilities;

namespace OMS.Models
{
    /// <summary>
    /// Class T_Expence_H Model
    /// </summary>
    [Serializable]
    public class T_Expence_H : M_Base<T_Expence_H>
    {
        #region Constant

        public const int EXPENCE_CODE_SHOW_MAX_LENGTH = 6;
        public const int EXPENCE_CODE_DB_MAX_LENGTH = 10;

        #endregion

        #region Variant

        /// <summary>
        /// 経費番号
        /// </summary>
        private string expenceNo;

        /// <summary>
        /// 計上日
        /// </summary>
        private DateTime date;

        /// <summary>
        /// 経費種別
        /// </summary>
        private string accountCD;

        /// <summary>
        /// 登録者
        /// </summary>
        private int userID;

        /// <summary>
        /// 負担部門
        /// </summary>
        private int departmentID;

        /// <summary>
        /// 負担プロジェクト
        /// </summary>
        private int projectID;

        /// <summary>
        /// 合計金額
        /// </summary>
        private decimal expenceAmount;

        /// <summary>
        /// 備考
        /// </summary>
        private string memo;

        /// <summary>
        /// 添付パス
        /// </summary>
        private string filepath;

        /// <summary>
        /// 承認フラグ
        /// </summary>
        private int approvedFlag;

        #endregion

        #region Property
        /// <summary>
        /// Get or set ReplyDueDate
        /// </summary>
        public string ExpenceNo
        {
            get { return expenceNo; }
            set
            {
                if (value != expenceNo)
                {
                    expenceNo = value;
                    this.Status = DataStatus.Changed;
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
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set accountCD
        /// </summary>
        public string AccountCD
        {
            get { return accountCD; }
            set
            {
                if (value != accountCD)
                {
                    accountCD = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set UserID
        /// </summary>
        public int UserID
        {
            get { return userID; }
            set
            {
                if (value != userID)
                {
                    userID = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set DepartmentID
        /// </summary>
        public int DepartmentID
        {
            get { return departmentID; }
            set
            {
                if (value != departmentID)
                {
                    departmentID = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set ProjectID
        /// </summary>
        public int ProjectID
        {
            get { return projectID; }
            set
            {
                if (value != projectID)
                {
                    projectID = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set ExpenceAmount
        /// </summary>
        public decimal ExpenceAmount
        {
            get { return expenceAmount; }
            set
            {
                if (value != expenceAmount)
                {
                    expenceAmount = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set Memo
        /// </summary>
        public string Memo
        {
            get { return memo; }
            set
            {
                if (value != memo)
                {
                    memo = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        ///  Get or set Filepath
        /// </summary>
        public string Filepath
        {
            get { return filepath; }
            set
            {
                if (value != filepath)
                {
                    filepath = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set approvedFlag
        /// </summary>
        public int ApprovedFlag
        {
            get { return approvedFlag; }
            set
            {
                if (value != approvedFlag)
                {
                    approvedFlag = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }
        #endregion

        #region Contructor

        /// <summary>
        /// Contructor T_Expence_H
        /// </summary>
        public T_Expence_H()
            : base()
        {
            this.expenceAmount = 0;
            this.filepath = string.Empty;
        }

        /// <summary>
        /// Contructor T_Expence_H
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public T_Expence_H(DbDataReader dr)
            : base(dr)
        {
            ISecurity sec = Security.Instance;
            this.expenceNo = (string)dr["ExpenceNo"];
            this.date = (DateTime)dr["Date"];
            this.accountCD = (string)dr["AccountCD"];
            this.userID = int.Parse(dr["UserID"].ToString());
            this.departmentID = int.Parse(dr["DepartmentID"].ToString());
            this.projectID = int.Parse(dr["ProjectID"].ToString());
            this.expenceAmount = decimal.Parse(dr["ExpenceAmount"].ToString());
            this.memo = (string)dr["Memo"];
            this.filepath = (string) dr["Filepath"];
            this.approvedFlag = int.Parse(dr["ApprovedFlag"].ToString());
        }

        #endregion
    }
}
