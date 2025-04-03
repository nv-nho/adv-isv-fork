using System;
using System.Data.Common;
using OMS.Utilities;

namespace OMS.Models
{
    /// <summary>
    /// Class T_Mail_H Model
    /// </summary>
    [Serializable]
    public class T_Mail_H : M_Base<T_Mail_H>
    {
        #region Contanst

        /// <summary>
        /// Max length of Subject
        /// </summary>
        public const int SUBJECT_MAX_LENGTH = 200;
        /// <summary>
        /// Max length of BodyMail
        /// </summary>
        public const int BODY_MAIL_MAX_LENGTH = 2000;
        /// <summary>
        /// Max length of FilePath1
        /// </summary>
        public const int FILE_PATH_1_MAX_LENGTH = 255;
        /// <summary>
        /// Max length of FilePath2
        /// </summary>
        public const int FILE_PATH_2_MAX_LENGTH = 255;
        /// <summary>
        /// Max length of FilePath3
        /// </summary>
        public const int FILE_PATH_3_MAX_LENGTH = 255;
        /// <summary>
        /// Max length of STARTDIVISION
        /// </summary>
        public const int START_DATE_MAX_LENGTH = 3;
        /// <summary>
        /// Max length of ENDDIVISION
        /// </summary>
        public const int END_DATE_MAX_LENGTH = 3;

        #endregion

        #region Variant

        /// <summary>
        /// 返信期限
        /// </summary>
        public DateTime replyDueDate;

        /// <summary>
        /// 件名
        /// </summary>
        private string subject;

        /// <summary>
        /// 本文
        /// </summary>
        private string bodyMail;

        /// <summary>
        /// 添付ファイルパス1
        /// </summary>
        private string filePath1;

        /// <summary>
        /// 添付ファイルパス2
        /// </summary>
        private string filePath2;

        /// <summary>
        /// 添付ファイルパス3
        /// </summary>
        private string filePath3;


        /// <summary>
        /// 再送信フラグ
        /// </summary>
        private int resendFlag;

        /// <summary>
        /// 下書きフラグ
        /// </summary>
        private int draftFlag;

        /// <summary>
        /// 再送基準日
        /// </summary>
        private int baseDivision;

        /// <summary>
        /// 再送開始日
        /// </summary>
        private int startDate;

        /// <summary>
        /// 再送開始区分
        /// </summary>
        private int startDivision;

        /// <summary>
        /// 再送終了日
        /// </summary>
        private int endDate;

        /// <summary>
        /// 再送終了区分
        /// </summary>
        private int endDivision;

        /// <summary>
        /// 再送信開始時刻
        /// </summary>
        private int resendTime;

        /// <summary>
        /// 再送間隔
        /// </summary>
        private int resendInterval;

        #endregion

        #region Property

        /// <summary>
        /// Get or set ReplyDueDate
        /// </summary>
        public DateTime ReplyDueDate
        {
            get { return replyDueDate; }
            set
            {
                if (value != replyDueDate)
                {
                    replyDueDate = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set Subject
        /// </summary>
        public string Subject
        {
            get { return subject; }
            set
            {
                if (value != subject)
                {
                    subject = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set BodyMail
        /// </summary>
        public string BodyMail
        {
            get { return bodyMail; }
            set
            {
                if (value != bodyMail)
                {
                    bodyMail = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set FilePath1
        /// </summary>
        public string FilePath1
        {
            get { return filePath1; }
            set
            {
                if (value != filePath1)
                {
                    filePath1 = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set FilePath2
        /// </summary>
        public string FilePath2
        {
            get { return filePath2; }
            set
            {
                if (value != filePath2)
                {
                    filePath2 = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set FilePath3
        /// </summary>
        public string FilePath3
        {
            get { return filePath3; }
            set
            {
                if (value != filePath3)
                {
                    filePath3 = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }


        /// <summary>
        /// Get or set 下書きフラグ
        /// </summary>
        public int DraftFlag
        {
            get { return draftFlag; }
            set
            {
                if (value != draftFlag)
                {
                    draftFlag = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        /// Get or set 再送信フラグ
        /// </summary>
        public int ResendFlag
        {
            get { return resendFlag; }
            set
            {
                if(value != resendFlag){
                    resendFlag = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }

        /// <summary>
        ///  再送基準日
        /// </summary>
        public int BaseDivision
        {
            get { return baseDivision; }
            set
            {
                if (value != baseDivision)
                {
                    baseDivision = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }


        /// <summary>
        /// 再送開始日
        /// </summary>
        public int StartDate
        {
            get { return startDate; }
            set
            {
                if (value != startDate)
                {
                    startDate = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }


        /// <summary>
        /// 再送開始区分
        /// </summary>
        public int StartDivision
        {
            get { return startDivision; }
            set
            {
                if (value != startDivision)
                {
                    startDivision = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }


        /// <summary>
        /// 再送終了日
        /// </summary>
        public int EndDate
        {
            get { return endDate; }
            set
            {
                if (value != endDate)
                {
                    endDate = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }


        /// <summary>
        /// 再送信フラグ
        /// </summary>
        public int EndDivison
        {
            get { return endDivision; }
            set
            {
                if (value != endDivision)
                {
                    endDivision = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }


        /// <summary>
        /// 再送信開始時刻
        /// </summary>
        public int ResendTime
        {
            get { return resendTime; }
            set
            {
                if (value != resendTime)
                {
                    resendTime = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }
        /// <summary>
        /// Get or set 再送間隔
        /// </summary>
        public int ResendInterval
        {
            get { return resendInterval; }
            set
            {
                if (value != resendInterval)
                {
                    resendInterval = value;
                    this.Status = DataStatus.Changed;
                }
            }
        }


        #endregion

        #region Contructor

        /// <summary>
        /// Contructor T_Mail_H
        /// </summary>
        public T_Mail_H()
            : base()
        {
            this.draftFlag = 0;
            this.resendTime = 0;
            this.baseDivision = 0;
            this.startDate = 0;
            this.startDivision = 0;
            this.endDate = 0;
            this.endDivision = 0;
            this.resendTime = 0;
            this.resendInterval = 0;
        }

        /// <summary>
        /// Contructor T_Mail_H
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public T_Mail_H(DbDataReader dr)
            : base(dr)
        {
            ISecurity sec = Security.Instance;
            this.replyDueDate = (DateTime)dr["ReplyDueDate"];
            this.subject = (string)dr["Subject"];
            this.bodyMail = (string)dr["BodyMail"];
            this.filePath1 = (string)dr["FilePath1"];
            this.filePath2 = (string)dr["FilePath2"];
            this.filePath3 = (string)dr["FilePath3"];
            this.draftFlag = int.Parse(dr["DraftFlag"].ToString());
            this.resendFlag = int.Parse(dr["ResendFlag"].ToString());
            this.baseDivision = int.Parse(dr["BaseDivision"].ToString());
            this.startDate = int.Parse(dr["StartDate"].ToString());
            this.startDivision = int.Parse(dr["StartDivision"].ToString());
            this.endDate = int.Parse(dr["EndDate"].ToString());
            this.endDivision = int.Parse(dr["EndDivision"].ToString());
            this.resendTime = int.Parse(dr["ResendTime"].ToString());
            this.resendInterval = int.Parse(dr["ResendInterval"].ToString());
        }

        #endregion
    }
}
