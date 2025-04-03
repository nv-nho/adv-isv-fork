using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using OMS.Models;

namespace OMS.DAC
{
    public class SettingService : BaseService
    {
        #region Constructor

        private SettingService()
            : base()
        {
        }

        public SettingService(DB db)
            : base(db)
        {
        }

        #endregion

        /// <summary>
        /// Get data
        /// </summary>
        /// <returns>M_Setting</returns>
        public M_Setting GetData()
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine(" SELECT	ID,");
            cmdText.AppendLine(" Logo1,");
            cmdText.AppendLine(" Logo2,");
            cmdText.AppendLine(" AttachPath,");
            cmdText.AppendLine(" Extension,");
            cmdText.AppendLine(" CreateDate,");
            cmdText.AppendLine(" CreateUID,");
            cmdText.AppendLine(" UpdateDate,");
            cmdText.AppendLine(" UpdateUID ");
            cmdText.AppendLine(" FROM ");
            cmdText.AppendLine(" dbo.M_Setting");

            //Para
            Hashtable paras = new Hashtable();
            return this.db.Find1<M_Setting>(cmdText.ToString(), paras);
        }

        #region Insert

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="setting">M_Setting</param>
        /// <returns></returns>
        public int Insert(M_Setting setting)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();

            cmdText.AppendLine(" INSERT INTO dbo.M_Setting");
            cmdText.AppendLine(" (");

            cmdText.AppendLine(" Logo1 ");
            cmdText.AppendLine(" ,Logo2 ");
            cmdText.AppendLine(" ,AttachPath ");
            cmdText.AppendLine(" ,Extension ");
            
            cmdText.AppendLine(" ,CreateDate ");
            cmdText.AppendLine(" ,CreateUID ");
            cmdText.AppendLine(" ,UpdateDate ");
            cmdText.AppendLine(" ,UpdateUID ");

            cmdText.AppendLine(" )");
            cmdText.AppendLine(" VALUES");
            cmdText.AppendLine(" (");

            cmdText.AppendLine(" @IN_Logo1 ");
            cmdText.AppendLine(" ,@IN_Logo2 ");
            cmdText.AppendLine(" ,@IN_AttachPath ");
            cmdText.AppendLine(" ,@IN_Extension ");

            cmdText.AppendLine(" ,GETDATE() ");
            cmdText.AppendLine(" ,@IN_CreateUID ");
            cmdText.AppendLine(" ,GETDATE() ");
            cmdText.AppendLine(" ,@IN_UpdateUID ");
            cmdText.AppendLine(" )");

            //Para
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_Logo1", setting.Logo1);
            base.AddParam(paras, "IN_Logo2", setting.Logo2);
            base.AddParam(paras, "IN_AttachPath", setting.AttachPath);
            base.AddParam(paras, "IN_Extension", setting.Extension);
            base.AddParam(paras, "IN_CreateUID", setting.CreateUID);
            base.AddParam(paras, "IN_UpdateUID", setting.UpdateUID);

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }

        #endregion

        #region Update

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="setting">M_Setting</param>
        /// <returns></returns>
        public int Update(M_Setting setting)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();
            cmdText.AppendLine(" UPDATE dbo.M_Setting");
            cmdText.AppendLine(" SET");
            cmdText.AppendLine(" Logo1				= @IN_Logo1, ");
            cmdText.AppendLine(" Logo2				= @IN_Logo2, ");
            cmdText.AppendLine(" AttachPath			= @IN_AttachPath, ");
            cmdText.AppendLine(" Extension			= @IN_Extension, ");
            cmdText.AppendLine(" UpdateDate = GETDATE(),");
            cmdText.AppendLine(" UpdateUID	 = @IN_UpdateUID");

            //Para
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_Logo1", setting.Logo1);
            base.AddParam(paras, "IN_Logo2", setting.Logo2);
            base.AddParam(paras, "IN_AttachPath", setting.AttachPath);
            base.AddParam(paras, "IN_Extension", setting.Extension);
            base.AddParam(paras, "IN_UpdateDate", setting.UpdateDate, true);
            base.AddParam(paras, "IN_UpdateUID", setting.UpdateUID);

            cmdWhere.AppendLine(" UpdateDate = @IN_UpdateDate");

            //Check SQL
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }

        #endregion
    }
}

