using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using OMS.Utilities;
using OMS.Models;

namespace OMS.DAC
{
    public class Mail_DService : BaseService
    {
        #region Constructor
        /// <summary>
        /// Contructor of T_Mail_D service
        /// </summary>        
        private Mail_DService()
            : base()
        {
        }

        /// <summary>
        /// Contructor of T_Mail_D service
        /// </summary>
        /// <param name="db">Class DB</param>
        public Mail_DService(DB db)
            : base(db)
        {
        }
        #endregion

        #region Get Data

        public IList<MailDetailUserInfo> GetListUnReplyUserInfo(int HID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" U.UserCD");
            cmdText.AppendLine(" ,U.UserName1 AS UserName");
            cmdText.AppendLine(" ,M.MailPath");
            cmdText.AppendLine(" FROM dbo.T_Mail_D M");
            cmdText.AppendLine(" LEFT JOIN dbo.M_User U ON U.ID = M.UID");
            cmdText.AppendLine(" WHERE M.HID = @IN_HID");
            cmdText.AppendLine(" AND M.ReceiveDate IS NULL");
            cmdText.AppendLine(" ORDER BY U.UserCD");

            //Parameter
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_HID", HID);

            return this.db.FindList1<MailDetailUserInfo>(cmdText.ToString(), paras);
        }

        public IList<MailDetailUserInfo> GetListReplyedUserInfo(int HID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" U.UserCD");
            cmdText.AppendLine(" ,U.UserName1 AS UserName");
            cmdText.AppendLine(" ,M.MailPath");
            cmdText.AppendLine(" FROM dbo.T_Mail_D M");
            cmdText.AppendLine(" LEFT JOIN dbo.M_User U ON U.ID = M.UID");
            cmdText.AppendLine(" WHERE M.HID = @IN_HID");
            cmdText.AppendLine(" AND M.ReceiveDate IS NOT NULL");

            //Parameter
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_HID", HID);

            return this.db.FindList1<MailDetailUserInfo>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Get List by HID
        /// </summary>
        /// <returns>T_Mail_D</returns>
        public IList<T_Mail_D> GetListByHID(int HID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  1 AS RowNumber");
            cmdText.AppendLine(" ,T1.*");
            cmdText.AppendLine(" FROM dbo.T_Mail_D AS T1");
            cmdWhere.AppendLine(" HID = @IN_HID");

            //Parameter
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_HID", HID);
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.FindList1<T_Mail_D>(cmdText.ToString(), paras);

        }
        /// <summary>
        /// Get By PK
        /// </summary>
        /// <param name="HID"></param>
        /// <param name="UID"></param>
        /// <returns></returns>
        public T_Mail_D GetByPK(int HID, int UID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" T1.*");
            cmdText.AppendLine(" FROM dbo.T_Mail_D AS T1");
            cmdText.AppendLine(" WHERE ");
            cmdText.AppendLine(" T1.HID = @IN_HID ");
            cmdText.AppendLine(" AND T1.UID = @IN_UID ");

            //Parameter
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_HID", HID);
            base.AddParam(paras, "IN_UID", UID);

            return this.db.Find1<T_Mail_D>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// GetByListNotReply
        /// </summary>
        /// <param name="HID"></param>
        /// <param name="UID"></param>
        /// <returns></returns>
        public IList<T_Mail_D> GetByListNotReply(int HID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" T1.*");
            cmdText.AppendLine(" FROM dbo.T_Mail_D AS T1");
            cmdText.AppendLine(" WHERE ");
            cmdText.AppendLine(" T1.HID = @IN_HID ");
            cmdText.AppendLine(" AND T1.ReceiveDate IS NULL");

            //Parameter
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_HID", HID);

            return this.db.FindList1<T_Mail_D>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// GetCountUserNotReply
        /// </summary>
        /// <param name="HID"></param>
        /// <param name="UID"></param>
        /// <returns></returns>
        public int GetCountUserNotReply(int HID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" COUNT(*)");
            cmdText.AppendLine(" FROM dbo.T_Mail_D AS T1");
            cmdText.AppendLine(" WHERE ");
            cmdText.AppendLine(" T1.HID = @IN_HID ");
            cmdText.AppendLine(" AND T1.ReceiveDate IS NULL");

            //Parameter
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_HID", HID);

            return int.Parse(this.db.ExecuteScalar1(cmdText.ToString(), paras).ToString());
        }

        #endregion

        #region Insert
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="T_Mail_D">T_Mail_D</param>
        /// <returns></returns>
        public int Insert(T_Mail_D t_Mail_D)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine(" INSERT INTO");
            cmdText.AppendLine(" dbo.T_Mail_D");
            cmdText.AppendLine(" (");
            cmdText.AppendLine("  HID");
            cmdText.AppendLine(" ,UID");
            cmdText.AppendLine(" ,MailAddress");
            cmdText.AppendLine(" ,ReceiveDate");
            cmdText.AppendLine(" ,ReceiveFlag");
            cmdText.AppendLine(" ,MailPath");
            cmdText.AppendLine(" )");
            cmdText.AppendLine(" VALUES");
            cmdText.AppendLine(" (");
            cmdText.AppendLine("  @IN_HID");
            cmdText.AppendLine(" ,@IN_UID");
            cmdText.AppendLine(" ,@IN_MailAddress");
            cmdText.AppendLine(" ,@IN_ReceiveDate");
            cmdText.AppendLine(" ,@IN_ReceiveFlag");
            cmdText.AppendLine(" ,@IN_MailPath");
            cmdText.AppendLine(" )");

            //Param
            Hashtable paras = new Hashtable();
            ISecurity sec = Security.Instance;

            base.AddParam(paras, "IN_HID", t_Mail_D.HID);
            base.AddParam(paras, "IN_UID", t_Mail_D.UID);
            base.AddParam(paras, "IN_MailAddress", t_Mail_D.MailAddress);
            base.AddParam(paras, "IN_ReceiveDate", t_Mail_D.ReceiveDate, true);
            base.AddParam(paras, "IN_ReceiveFlag", t_Mail_D.ReceiveFlag);
            base.AddParam(paras, "IN_MailPath", t_Mail_D.MailPath);

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }
        #endregion

        #region Update
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="T_Mail_D">T_Mail_D</param>
        /// <returns></returns>
        public int Update(T_Mail_D t_Mail_D)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" UPDATE");
            cmdText.AppendLine(" dbo.T_Mail_D");
            cmdText.AppendLine(" SET");
            cmdText.AppendLine("  MailAddress  = @IN_MailAddress");
            cmdText.AppendLine(" ,ReceiveDate    = @IN_ReceiveDate");
            cmdText.AppendLine(" ,ReceiveFlag  = @IN_ReceiveFlag");
            cmdText.AppendLine(" ,MailPath = @IN_MailPath");

            //Para
            Hashtable paras = new Hashtable();
            ISecurity sec = Security.Instance;

            cmdWhere.AppendLine(" HID = @IN_HID AND UID = @IN_UID");
            base.AddParam(paras, "IN_HID", t_Mail_D.HID);
            base.AddParam(paras, "IN_UID", t_Mail_D.UID);
            base.AddParam(paras, "IN_MailAddress", t_Mail_D.MailAddress);
            base.AddParam(paras, "IN_ReceiveDate", t_Mail_D.ReceiveDate, true);
            base.AddParam(paras, "IN_ReceiveFlag", t_Mail_D.ReceiveFlag);
            base.AddParam(paras, "IN_MailPath", t_Mail_D.MailPath);

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }

        #endregion


        #region Delete By HID, UID
        /// <summary>
        /// Delete T_Mail_D By HID, UID
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        /// 
        public int Delete(int HID, int UID)
        {
            //SQL String 
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine("DELETE FROM dbo.T_Mail_D");

            //Parameter
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine("HID = @IN_HID AND UID = @IN_UID");
            base.AddParam(paras, "IN_HID", HID);
            base.AddParam(paras, "IN_UID", UID);
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }

        #endregion

        #region Delete List By HID
        /// <summary>
        /// Delete List<T_Mail_D> By HID
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        /// 
        public int DeleteAllByHId(int HID)
        {
            //SQL String 
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine("DELETE FROM dbo.T_Mail_D");

            //Parameter
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine("HID = @IN_HID");
            base.AddParam(paras, "IN_HID", HID);
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }
        #endregion
    }
}
