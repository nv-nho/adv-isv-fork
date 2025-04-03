using MailProcess.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MailProcess.DBContext
{
    public class MailsService
    {
        /// <summary>
        /// Class DB
        /// </summary>
        protected DB db;

        #region Constructor
        /// <summary>
        /// Contructor of T_Mail_H service
        /// </summary>        
        private MailsService(string connectionString)
        {
            this.db = new DB(connectionString);
        }

        /// <summary>
        /// Contructor of T_Mail_H service
        /// </summary>
        /// <param name="db">Class DB</param>
        public MailsService(DB db)
        {
            this.db = db;
        }
        #endregion

        #region Method
        /// <summary>
        /// set Parameter
        /// </summary>
        /// <param name="htbl">Param List</param>
        /// <param name="key">Name</param>
        /// <param name="value">Value</param>
        /// <param name="defaultValue">TRUE: IF value is default of it's type then set to null/ FALSE: set to normal</param>
        protected void AddParam(Hashtable htbl, string key, object value, bool defaultValueIsNull = false)
        {
            if (IsNullVal(value, defaultValueIsNull))
            {
                htbl.Add(key, DBNull.Value);
            }
            else
            {
                htbl.Add(key, value);
            }
        }

        /// <summary>
        /// Check null value
        /// </summary>
        /// <typeparam name="TypeValue"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        protected bool IsNullVal(object value, bool defaultValueIsNull)
        {
            if (defaultValueIsNull)
            {
                if (value == null)
                {
                    return true;
                }

                var type = value.GetType();
                if (type.Equals(typeof(string)))
                {
                    return string.IsNullOrEmpty((string)value);
                }
                if (type.Equals(typeof(bool)))
                {
                    return (bool)value == default(bool);
                }
                if (type.Equals(typeof(DateTime)))
                {
                    return (DateTime)value == default(DateTime);
                }
                //number 
                var number = 0m;
                if (decimal.TryParse(value.ToString(), out number))
                {
                    return number == default(decimal);
                }
            }
            else
            {
                if (value == null)
                {
                    return true;
                }
            }

            return false;
        }
        #endregion

        #region Get
        /// <summary>
        /// Get by ID
        /// </summary>
        /// <returns>T_Mail_H</returns>
        public T_Mail_D GetDetailByMailAddress(int HID, string mailAddress)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine(" SELECT TOP 1");
            cmdText.AppendLine(" *");
            cmdText.AppendLine(" FROM dbo.T_Mail_D");
            cmdText.AppendLine(" WHERE ");
            cmdText.AppendLine(" HID = @IN_HID AND MailAddress = @IN_MailAddress");

            //Parameter
            Hashtable paras = new Hashtable();
            AddParam(paras, "IN_HID", HID);
            AddParam(paras, "IN_MailAddress", mailAddress);

            return this.db.Find<T_Mail_D>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Get List Send Mail
        /// </summary>
        /// <returns></returns>
        public IList<MailRepInfo> GetListSendMail(int HID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine(" SELECT ");
            cmdText.AppendLine(" TMD.HID, TMD.UID, TMH.BodyMail, TMH.Subject, TMH.FilePath1, TMH.FilePath2, TMH.FilePath3, TMD.MailAddress , MU.UserName1 AS UserNm");
            cmdText.AppendLine(" FROM dbo.T_Mail_D AS TMD");
            cmdText.AppendLine(" INNER JOIN dbo.T_Mail_H AS TMH ON TMD.HID = TMH.ID");
            cmdText.AppendLine(" LEFT JOIN dbo.M_User AS MU ON TMD.UID = MU.ID");
            cmdText.AppendLine(" WHERE TMD.ReceiveFlag = 0 AND TMD.HID = @IN_HID");
            //Parameter
            Hashtable paras = new Hashtable();
            AddParam(paras, "IN_HID", HID);
            return this.db.FindList1<MailRepInfo>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Get Default Value DropDownList
        /// </summary>
        /// <param name="configCd">Config Code</param>
        /// <returns></returns>
        public ConfigValue3 GetDefaultValueDrop(string configCd)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" ID");
            cmdText.AppendLine(" FROM dbo.M_Config_H SH");

            //Param
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine(" SH.ConfigCD = @IN_ConfigCD");
            AddParam(paras, "IN_ConfigCD", configCd);
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            //SQL OUT
            StringBuilder cmdOutText = new StringBuilder();
            StringBuilder cmdOutWhere = new StringBuilder();
            cmdOutText.AppendLine(" SELECT");
            cmdOutText.AppendLine(" SD.Value3");
            cmdOutText.AppendLine(" FROM");
            cmdOutText.AppendLine(" (");
            cmdOutText.AppendLine(cmdText.ToString());
            cmdOutText.AppendLine(" ) AS  VIEW_1");
            cmdOutText.AppendLine(" INNER JOIN dbo.M_Config_D AS SD ON SD.HID = VIEW_1.ID");
            cmdOutWhere.AppendLine(" SD.Value3 <> ''	");
            if (cmdOutWhere.Length != 0)
            {
                cmdOutText.AppendLine(" WHERE ");
                cmdOutText.AppendLine(cmdOutWhere.ToString());
            }

            return this.db.Find<ConfigValue3>(cmdOutText.ToString(), paras);
        }

        /// <summary>
        /// Get Value2 by value1
        /// </summary>
        /// <param name="configCd">ConfigCd</param>
        /// <param name="value1">Value1</param>
        /// <returns></returns>
        public ConfigValue2 GetValue2(string configCd, int value1)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  ID");
            cmdText.AppendLine(" FROM dbo.M_Config_H SH");

            //Param
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine(" SH.ConfigCD = @IN_ConfigCd");
            AddParam(paras, "IN_ConfigCd", configCd);

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            StringBuilder cmdOutText = new StringBuilder();
            cmdOutText.AppendLine(" SELECT");
            cmdOutText.AppendLine("   SD.Value2");
            cmdOutText.AppendLine(" FROM");
            cmdOutText.AppendLine(" (");
            cmdOutText.AppendLine(cmdText.ToString());
            cmdOutText.AppendLine(" ) AS  VIEW_1");
            cmdOutText.AppendLine(" INNER JOIN dbo.M_Config_D AS SD ON SD.HID = VIEW_1.ID AND SD.Value1 = @IN_Value1");

            AddParam(paras, "IN_Value1", value1);

            return this.db.Find<ConfigValue2>(cmdOutText.ToString(), paras);
        }
        #endregion

        #region Update Details
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="T_Mail_D">T_Mail_D</param>
        /// <returns></returns>
        public int Update(T_Mail_D detail)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine(" UPDATE dbo.T_Mail_D ");
            cmdText.AppendLine(" SET ");
            cmdText.AppendLine(" ReceiveDate = @IN_ReceiveDate ");
            cmdText.AppendLine(" ,ReceiveFlag = @IN_ReceiveFlag ");
            cmdText.AppendLine(" ,MailPath = @IN_MailPath ");
            cmdText.AppendLine(" WHERE ");
            cmdText.AppendLine(" HID = @IN_HID AND MailAddress = @IN_MailAddress ");

            //Para
            Hashtable paras = new Hashtable();

            AddParam(paras, "IN_HID", detail.HID);
            AddParam(paras, "IN_MailAddress", detail.MailAddress);
            AddParam(paras, "IN_ReceiveFlag", detail.ReceiveFlag);
            AddParam(paras, "IN_ReceiveDate", detail.ReceiveDate, true);
            AddParam(paras, "IN_MailPath", detail.MailPath);

            return this.db.ExecuteNonQuery(cmdText.ToString(), paras);
        }
        #endregion
    }
}
