using System;
using System.Data.Common;
using OMS.Models;
using OMS.Utilities;
using System.Collections.Generic;

    /// <summary>
    /// Class MailInfo Model
    /// </summary>
    [Serializable]
    public class MailInfo
    {
        public long RowNumber { get; set; }
        public int ID { get; set; }
        public DateTime ReplyDueDate { get; set; }
        public string Subject { get; set; }
        public string BodyMail { get; set; }
        public string FilePath1 { get; set; }
        public string FilePath2 { get; set; }
        public string FilePath3 { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string StrUpdateDate
        {
            get
            {
                if (this.DraftFlag == "1")
                {
                    return string.Empty;
                }
                else
                {
                    return String.Format(Constants.FMR_DATE_YMD_HMS, this.UpdateDate);
                }
            }
            set{}
        }
        public string StrReplyDueDate
        {
            get
            {
                return String.Format(Constants.FMR_DATE_YMD, this.ReplyDueDate);
            }
            set{}
        }

        public string DraftFlag { get; set; }
        public string Content { get; set; }
        public string Class { get; set; }
        public string CountRep { get; set; }
        public int countMailRep { get; set; }
        public int countList { get; set; }

    /// <summary>
    /// Constructor class MailInfo
    /// </summary>
    /// <param name="dr">DbDataReader</param>
    public MailInfo(DbDataReader dr)
    {
        ISecurity sec = Security.Instance;
        this.RowNumber = (long)dr["RowNumber"];
        this.ID = int.Parse(dr["ID"].ToString());
        this.ReplyDueDate = (DateTime)dr["ReplyDueDate"];
        this.Subject = (string)dr["Subject"];
        this.BodyMail = (string)dr["BodyMail"];
        this.FilePath1 = (string)dr["FilePath1"];
        this.FilePath2 = (string)dr["FilePath2"];
        this.FilePath3 = (string)dr["FilePath3"];
        DateTime? dtUpdateDate = dr["UpdateDate"] == DBNull.Value || dr["UpdateDate"] == null ? (DateTime?)null : (DateTime)dr["UpdateDate"];
        this.UpdateDate = dtUpdateDate;
        this.DraftFlag = dr["DraftFlag"].ToString();
        this.countMailRep = (int)dr["countMailRep"];
        this.countList = (int)dr["countList"];
        if (this.DraftFlag == "1")
        {
            this.Content = "下書き";
            this.Class = "label label-success";
            this.CountRep = string.Empty;
        }
        else
        {
            this.Content = "送信済";
            this.Class = "label label-primary";
            this.CountRep = this.countMailRep + "/" + this.countList;
        }

    }

        /// <summary>
        /// Constructor class MailInfo
        /// </summary>
        public MailInfo()
        {
            this.RowNumber = 0;
            this.ReplyDueDate = DateTime.Now;
            this.Subject = string.Empty;
            this.BodyMail = string.Empty;
            this.FilePath1 = string.Empty;
            this.FilePath2 = string.Empty;
            this.FilePath3 = string.Empty;
            this.UpdateDate = DateTime.Now;
        }
    }

/// <summary>
/// Class MailSearchInfo Model
/// </summary>
[Serializable]
    public class  MailSearchInfo
    {
        public long RowNumber { get; set; }
        public string ID { get; set; }
        public string Subject { get; set; }

        /// <summary>
        /// Constructor class MailInfo
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public MailSearchInfo(DbDataReader dr)
        {
            this.RowNumber = (long)dr["RowNumber"];
            this.ID = (string)dr["ID"];
            this.Subject = (string)dr["Subject"];
           
        }

        /// <summary>
        /// Constructor class MailInfo
        /// </summary>
        public MailSearchInfo()
        {
            this.RowNumber = 0;
            this.ID = null;
            this.Subject = null;
        }
    }
    /// <summary>
    /// MailDetailInfo
    /// </summary>
    [Serializable]
    public class MailDetailInfo
    {
        public int ID { get; set; }
        public int HID { get; set; }
        public string UserCD { get; set; }
        public string UserName1 { get; set; }
        public string UserName2 { get; set; }
        public string MailAddress { get; set; }
        public string ReplyDueDate { get; set; }
        public string ReceiveDate { get; set; }
        public string Subject { get; set; }
        public string BodyMail { get; set; }
        public string FilePath1 { get; set; }
        public string FilePath2 { get; set; }
        public string FilePath3 { get; set; }

        public MailDetailInfo() {
            this.UserCD = string.Empty;
            this.UserName1 = string.Empty;
            this.UserName2 = string.Empty;
            this.ReceiveDate = string.Empty;
            this.FilePath1 = string.Empty;
            this.FilePath2 = string.Empty;
            this.FilePath3 = string.Empty;
        }
    }

    /// <summary>
    /// Class MailInfo Model
    /// </summary>
    [Serializable]
    public class MailDetailUserInfo
    {
        public string UserCD { get; set; }
        public string UserName { get; set; }
        public string MailPath { get; set; }

        /// <summary>
        /// Constructor class MailInfo
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public MailDetailUserInfo(DbDataReader dr)
        {
            this.UserCD = EditDataUtil.ToFixCodeShow((string)dr["UserCD"], M_User.MAX_USER_CODE_SHOW);
            this.UserName = (string)dr["UserName"];
            this.MailPath = (string)dr["MailPath"];
        }

        /// <summary>
        /// Constructor class MailInfo
        /// </summary>
        public MailDetailUserInfo()
        {
        }
    }