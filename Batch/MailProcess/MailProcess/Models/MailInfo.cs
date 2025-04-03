using System;
using System.Data.Common;

    /// <summary>
    /// Class MailRepInfo Model
    /// </summary>
    [Serializable]
    public class MailRepInfo
    {
        public int HID { get; set; }
        public int UID { get; set; }
        public string Subject { get; set; }
        public string BodyMail { get; set; }
        public string FilePath1 { get; set; }
        public string FilePath2 { get; set; }
        public string FilePath3 { get; set; }
        public string MailAddress { get; set; }
        public string UserNm { get; set; }

    /// <summary>
    /// Constructor class MailInfo
    /// </summary>
    /// <param name="dr">DbDataReader</param>
    public MailRepInfo(DbDataReader dr)
    {
        this.HID = int.Parse(dr["HID"].ToString());
        this.UID = int.Parse(dr["UID"].ToString());
        this.Subject = (string)dr["Subject"];
        this.BodyMail = (string)dr["BodyMail"];
        this.FilePath1 = (string)dr["FilePath1"];
        this.FilePath2 = (string)dr["FilePath2"];
        this.FilePath3 = (string)dr["FilePath3"];
        this.MailAddress = (string)dr["MailAddress"];
        this.UserNm = (string)dr["UserNm"];

    }

    /// <summary>
    /// Constructor class MailInfo
    /// </summary>
    public MailRepInfo()
        {
            this.Subject = string.Empty;
            this.BodyMail = string.Empty;
            this.FilePath1 = string.Empty;
            this.FilePath2 = string.Empty;
            this.FilePath3 = string.Empty;
            this.MailAddress = string.Empty;
            this.UserNm = string.Empty;
        }
    }

    /// <summary>
    /// Class ConfigName Model
    /// </summary>
    [Serializable]
    public class ConfigValue3
    {
        public string Value3 { get; set; }

        /// <summary>
        /// Constructor class ConfigName
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public ConfigValue3(DbDataReader dr)
        {
            this.Value3 = (dr["Value3"]) == null ? string.Empty : (string)dr["Value3"];
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ConfigValue3() {
            this.Value3 = string.Empty;
        }
    }

    /// <summary>
    /// Class ConfigName Model
    /// </summary>
    [Serializable]
    public class ConfigValue2
{
        public string Value2 { get; set; }

        /// <summary>
        /// Constructor class ConfigName
        /// </summary>
        /// <param name="dr">DbDataReader</param>
        public ConfigValue2(DbDataReader dr)
        {
            this.Value2 = (dr["Value2"]) == null ? string.Empty : (string)dr["Value2"];
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public ConfigValue2()
        {
            this.Value2 = string.Empty;
        }
    }