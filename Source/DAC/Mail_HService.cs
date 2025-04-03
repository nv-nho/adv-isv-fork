using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using OMS.Utilities;
using OMS.Models;

namespace OMS.DAC
{
    /// <summary>
    /// Class Mail_HService DAC
    /// </summary>
    public class Mail_HService : BaseService
    {
        #region Constructor
        /// <summary>
        /// Contructor of T_Mail_H service
        /// </summary>        
        private Mail_HService()
            : base()
        {
        }

        /// <summary>
        /// Contructor of T_Mail_H service
        /// </summary>
        /// <param name="db">Class DB</param>
        public Mail_HService(DB db)
            : base(db)
        {
        }
        #endregion

        #region Get Data
        /// <summary>
        /// Get by ID
        /// </summary>
        /// <returns>T_Mail_H</returns>
        public T_Mail_H GetByID(int ID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  1 AS RowNumber");
            cmdText.AppendLine(" ,T1.*");
            cmdText.AppendLine(" FROM dbo.T_Mail_H AS T1");
            cmdWhere.AppendLine(" ID = @IN_ID");

            //Parameter
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_ID", ID);
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.Find1<T_Mail_H>(cmdText.ToString(), paras);
        }

        /// <summary>
        /// GetListByCond
        /// ISV-Nhan
        /// </summary>
        /// <param name="mailId"></param>
        /// <param name="subject"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="sortDirec"></param>
        /// <returns></returns>
        public IList<MailInfo> GetListByCond(string mailId, string subject, int pageIndex, int pageSize, int sortField, int sortDirec)
        {
            string[] fields = new string[] { "","","ID", "CASE WHEN MH.DraftFlag = 1 THEN NULL ELSE MH.UpdateDate END", "ReplyDueDate", "Subject", "countMailRep", "DraftFlag" };
            string[] direc = new string[] { "ASC", "DESC" };

            string RowNumber = fields[sortField - 1] + " " + direc[sortDirec - 1];
            if (sortField == 1)
            {
                RowNumber = fields[sortField - 1] + " " + direc[1];
            }

            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" ID");
            cmdText.AppendLine(" ,CASE WHEN MH.DraftFlag = 1 THEN NULL ELSE MH.UpdateDate END UpdateDate");
            cmdText.AppendLine(" ,ReplyDueDate");
            cmdText.AppendLine(" ,Subject");
            cmdText.AppendLine(" ,BodyMail");
            cmdText.AppendLine(" ,FilePath1");
            cmdText.AppendLine(" ,FilePath2");
            cmdText.AppendLine(" ,FilePath3");
            cmdText.AppendLine(" ,DraftFlag");
            cmdText.AppendLine(" ,ISNULL(countMailRep, 0) AS countMailRep");
            cmdText.AppendLine(" ,ISNULL(countList, 0) AS countList");
            cmdText.AppendLine(string.Format(" ,ROW_NUMBER() OVER(ORDER BY {0}) AS RowNumber", RowNumber));
            cmdText.AppendLine(" FROM dbo.T_Mail_H AS MH");
            cmdText.AppendLine(" LEFT JOIN");
            cmdText.AppendLine(" ( SELECT");
            cmdText.AppendLine(" MD.HID");
            cmdText.AppendLine(" ,SUM(CASE WHEN MD.ReceiveFlag = 1 THEN 1 ELSE 0 END) countMailRep");
            cmdText.AppendLine(" ,COUNT(*) countList");
            cmdText.AppendLine(" FROM dbo.T_Mail_D AS MD");
            cmdText.AppendLine(" GROUP BY MD.HID");
            cmdText.AppendLine(" ) as CountRep");
            cmdText.AppendLine(" ON CountRep.HID = MH.ID");
            cmdText.AppendLine(" WHERE MH.ID >= 10");

            //Parameter
            Hashtable paras = new Hashtable();

            if (!string.IsNullOrEmpty(mailId))
            {
                cmdWhere.AppendLine(" AND MH.ID = @IN_ID");
                base.AddParam(paras, "IN_ID", mailId);
            }

            if (!string.IsNullOrEmpty(subject))
            {
                cmdWhere.AppendLine(" AND UPPER(MH.Subject) LIKE N'%' + UPPER(@IN_Subject) + '%' ");
                base.AddParam(paras, "IN_Subject", subject);
            }

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(cmdWhere.ToString());
            }

            base.AddParam(paras, "IN_PageIndex", pageIndex);
            base.AddParam(paras, "IN_PageSize", pageSize);

            //SQL OUT

            StringBuilder cmdOutText = new StringBuilder();
            cmdOutText.AppendLine(" SELECT");
            cmdOutText.AppendLine(" *");
            cmdOutText.AppendLine(" FROM");
            cmdOutText.AppendLine(" (");
            cmdOutText.AppendLine(cmdText.ToString());
            cmdOutText.AppendLine(" ) AS  VIEW_1");
            cmdOutText.AppendLine(" WHERE");
            cmdOutText.AppendLine(" VIEW_1.RowNumber BETWEEN(@IN_PageIndex -1) * @IN_PageSize + 1 AND(((@IN_PageIndex -1) * @IN_PageSize + 1) + @IN_PageSize) - 1");
            cmdOutText.AppendLine(" ORDER BY VIEW_1.RowNumber");

            return this.db.FindList1<MailInfo>(cmdOutText.ToString(), paras);
        }

        /// <summary>
        /// getTotalRow
        /// ISV-Nhan
        /// </summary>
        /// <param name="mailId"></param>
        /// <param name="subject"></param>
        /// <returns></returns>
        public int getTotalRow(string mailId, string subject)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" COUNT(*)");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.T_Mail_H AS M");
            cmdText.AppendLine(" WHERE M.ID >= 10");

            //Parameter
            Hashtable paras = new Hashtable();

            if (!string.IsNullOrEmpty(mailId))
            {
                cmdWhere.AppendLine(" AND M.ID = @IN_ID");
                base.AddParam(paras, "IN_ID", mailId);
            }

            if (!string.IsNullOrEmpty(subject))
            {
                cmdWhere.AppendLine(" AND UPPER(M.Subject) LIKE N'%' + UPPER(@IN_Subject) + '%' ");
                base.AddParam(paras, "IN_Subject", subject);
            }

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return int.Parse(this.db.ExecuteScalar1(cmdText.ToString(), paras).ToString());
        }

        /// <summary>
        /// Get Current ID 
        /// </summary>
        /// <returns></returns>
        public int GetCurrentID()
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine(" MAX(ID)");
            cmdText.AppendLine(" FROM");
            cmdText.AppendLine(" dbo.T_Mail_H");
             
            return int.Parse(this.db.ExecuteScalar1(cmdText.ToString()).ToString());
        }

        #endregion

        #region Insert
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="t_Mail_H">T_Mail_H</param>
        /// <returns></returns>
        public int Insert(T_Mail_H t_Mail_H)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine(" INSERT INTO");
            cmdText.AppendLine(" dbo.T_Mail_H");
            cmdText.AppendLine(" (");
            cmdText.AppendLine("  ReplyDueDate");
            cmdText.AppendLine(" ,Subject");
            cmdText.AppendLine(" ,BodyMail");
            cmdText.AppendLine(" ,FilePath1");
            cmdText.AppendLine(" ,FilePath2");
            cmdText.AppendLine(" ,FilePath3");
            cmdText.AppendLine(" ,DraftFlag");
            cmdText.AppendLine(" ,ResendFlag");
            cmdText.AppendLine(" ,BaseDivision");
            cmdText.AppendLine(" ,StartDate");
            cmdText.AppendLine(" ,StartDivision");
            cmdText.AppendLine(" ,EndDate");
            cmdText.AppendLine(" ,EndDivision");
            cmdText.AppendLine(" ,ResendTime");
            cmdText.AppendLine(" ,ResendInterval");
            cmdText.AppendLine(" ,CreateDate");
            cmdText.AppendLine(" ,CreateUID");
            cmdText.AppendLine(" ,UpdateDate");
            cmdText.AppendLine(" ,UpdateUID");
            cmdText.AppendLine(" )");
            cmdText.AppendLine(" VALUES");
            cmdText.AppendLine(" (");
            cmdText.AppendLine("  @IN_ReplyDueDate");
            cmdText.AppendLine(" ,@IN_Subject");
            cmdText.AppendLine(" ,@IN_BodyMail");
            cmdText.AppendLine(" ,@IN_FilePath1");
            cmdText.AppendLine(" ,@IN_FilePath2");
            cmdText.AppendLine(" ,@IN_FilePath3");
            cmdText.AppendLine(" ,@IN_DraftFlag");
            cmdText.AppendLine(" ,@IN_ResendFlag");
            cmdText.AppendLine(" ,@IN_BaseDivision");
            cmdText.AppendLine(" ,@IN_StartDate");
            cmdText.AppendLine(" ,@IN_StartDivision");
            cmdText.AppendLine(" ,@IN_EndDate");
            cmdText.AppendLine(" ,@IN_EndDivision");
            cmdText.AppendLine(" ,@IN_ResendTime");
            cmdText.AppendLine(" ,@IN_ResendInterval");
            cmdText.AppendLine(" ,GETDATE()");
            cmdText.AppendLine(" ,@IN_CreateUID");
            cmdText.AppendLine(" ,GETDATE()");
            cmdText.AppendLine(" ,@IN_UpdateUID");
            cmdText.AppendLine(" )");

            //Para
            Hashtable paras = new Hashtable();
            ISecurity sec = Security.Instance;

            base.AddParam(paras, "IN_ReplyDueDate", t_Mail_H.ReplyDueDate, true);
            base.AddParam(paras, "IN_Subject", t_Mail_H.Subject);
            base.AddParam(paras, "IN_BodyMail", t_Mail_H.BodyMail);
            base.AddParam(paras, "IN_FilePath1", t_Mail_H.FilePath1);
            base.AddParam(paras, "IN_FilePath2", t_Mail_H.FilePath2);
            base.AddParam(paras, "IN_FilePath3", t_Mail_H.FilePath3);
            base.AddParam(paras, "IN_DraftFlag", t_Mail_H.DraftFlag);
            base.AddParam(paras, "IN_ResendFlag", t_Mail_H.ResendFlag);
            base.AddParam(paras, "IN_BaseDivision", t_Mail_H.BaseDivision);
            base.AddParam(paras, "IN_StartDate", t_Mail_H.StartDate);
            base.AddParam(paras, "IN_StartDivision", t_Mail_H.StartDivision);
            base.AddParam(paras, "IN_EndDate", t_Mail_H.EndDate);
            base.AddParam(paras, "IN_EndDivision", t_Mail_H.EndDivison);
            base.AddParam(paras, "IN_ResendTime", t_Mail_H.ResendTime);
            base.AddParam(paras, "IN_ResendInterval", t_Mail_H.ResendInterval);
            base.AddParam(paras, "IN_CreateUID", t_Mail_H.CreateUID);
            base.AddParam(paras, "IN_UpdateUID", t_Mail_H.UpdateUID);
            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }
        #endregion

        #region Update
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="T_Mail_H">T_Mail_H</param>
        /// <returns></returns>
        public int Update(T_Mail_H t_Mail_H)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" UPDATE");
            cmdText.AppendLine(" dbo.T_Mail_H");
            cmdText.AppendLine(" SET");
            cmdText.AppendLine("  ReplyDueDate     = @IN_ReplyDueDate");
            cmdText.AppendLine(" ,Subject    = @IN_Subject");
            cmdText.AppendLine(" ,BodyMail  = @IN_BodyMail");
            cmdText.AppendLine(" ,FilePath1  = @IN_FilePath1");
            cmdText.AppendLine(" ,FilePath2  = @IN_FilePath2");
            cmdText.AppendLine(" ,FilePath3  = @IN_FilePath3");
            cmdText.AppendLine(" ,DraftFlag = @IN_DraftFlag");
            cmdText.AppendLine(" ,ResendFlag = @IN_ResendFlag");
            cmdText.AppendLine(" ,BaseDivision = @IN_BaseDivision");
            cmdText.AppendLine(" ,StartDate = @IN_StartDate");
            cmdText.AppendLine(" ,StartDivision = @IN_StartDivision");
            cmdText.AppendLine(" ,EndDate = @IN_EndDate");
            cmdText.AppendLine(" ,EndDivision = @IN_EndDivision");
            cmdText.AppendLine(" ,ResendTime = @IN_ResendTime");
            cmdText.AppendLine(" ,ResendInterval = @IN_ResendInterval");
            cmdText.AppendLine(" ,UpdateDate = GETDATE()");
            cmdText.AppendLine(" ,UpdateUID  = @IN_UpdateUID");
            cmdText.AppendLine(" WHERE ID = @IN_ID");

            //Para
            Hashtable paras = new Hashtable();

            base.AddParam(paras, "IN_ID", t_Mail_H.ID);
            base.AddParam(paras, "IN_ReplyDueDate", t_Mail_H.ReplyDueDate, true);
            base.AddParam(paras, "IN_Subject", t_Mail_H.Subject);
            base.AddParam(paras, "IN_BodyMail", t_Mail_H.BodyMail);
            base.AddParam(paras, "IN_FilePath1", t_Mail_H.FilePath1);
            base.AddParam(paras, "IN_FilePath2", t_Mail_H.FilePath2);
            base.AddParam(paras, "IN_FilePath3", t_Mail_H.FilePath3);
            base.AddParam(paras, "IN_DraftFlag", t_Mail_H.DraftFlag);
            base.AddParam(paras, "IN_ResendFlag", t_Mail_H.ResendFlag);
            base.AddParam(paras, "IN_BaseDivision", t_Mail_H.BaseDivision);
            base.AddParam(paras, "IN_StartDate", t_Mail_H.StartDate);
            base.AddParam(paras, "IN_StartDivision", t_Mail_H.StartDivision);
            base.AddParam(paras, "IN_EndDate", t_Mail_H.EndDate);
            base.AddParam(paras, "IN_EndDivision", t_Mail_H.EndDivison);
            base.AddParam(paras, "IN_ResendTime", t_Mail_H.ResendTime);
            base.AddParam(paras, "IN_ResendInterval", t_Mail_H.ResendInterval);
            base.AddParam(paras, "IN_UpdateUID", t_Mail_H.UpdateUID);
           
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="T_Mail_H">T_Mail_H</param>
        /// <returns></returns>
        public int UpdateDate(int HID, int UID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine(" UPDATE");
            cmdText.AppendLine(" dbo.T_Mail_H");
            cmdText.AppendLine(" SET");
            cmdText.AppendLine(" UpdateDate = GETDATE()");
            cmdText.AppendLine(" ,UpdateUID  = @IN_UpdateUID");
            cmdText.AppendLine(" WHERE ID = @IN_ID");

            //Para
            Hashtable paras = new Hashtable();

            base.AddParam(paras, "IN_ID", HID);
            base.AddParam(paras, "IN_UpdateUID", UID);

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="T_Mail_H">T_Mail_H</param>
        /// <returns></returns>
        public int UpdateFlgDraft(int HID, int UID, int draftFlag)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine(" UPDATE");
            cmdText.AppendLine(" dbo.T_Mail_H");
            cmdText.AppendLine(" SET");
            cmdText.AppendLine(" DraftFlag = @IN_DraftFlag");
            cmdText.AppendLine(" ,UpdateDate = GETDATE()");
            cmdText.AppendLine(" ,UpdateUID  = @IN_UpdateUID");
            cmdText.AppendLine(" WHERE ID = @IN_ID");

            //Para
            Hashtable paras = new Hashtable();

            base.AddParam(paras, "IN_DraftFlag", draftFlag);
            base.AddParam(paras, "IN_ID", HID);
            base.AddParam(paras, "IN_UpdateUID", UID);

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }

        #endregion


        #region Delete By Id
        /// <summary>
        /// Delete T_Mail_H By Id 
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        /// 
        public int Delete(int mailID)
        {
            //SQL String 
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine("DELETE [dbo].[T_Mail_H]");

            //Parameter
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine("ID = @IN_ID");

            base.AddParam(paras, "IN_ID", mailID);
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
