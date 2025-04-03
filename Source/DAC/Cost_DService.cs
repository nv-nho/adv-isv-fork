using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using OMS.Models;

namespace OMS.DAC
{
    public class Cost_DService : BaseService
    {


        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public Cost_DService()
            : base()
        {
        }

        /// <summary>
        /// Constructor with param
        /// </summary>
        /// <param name="db">Database</param>
        public Cost_DService(DB db)
            : base(db)
        {
        }
        #endregion

        #region Get data
        /// <summary>
        /// Get list by header ID
        /// </summary>
        /// <param name="headerID">HeaderID</param>
        /// <returns></returns>
        public IList<T_Cost_D> GetByListByHeaderID(int headerID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine(" SELECT D.*, ");
            cmdText.AppendLine(" D.HID AS ID ");
            cmdText.AppendLine(" FROM dbo.T_Cost_D AS D ");
            cmdText.AppendLine(" WHERE D.HID = @IN_HID ");
            cmdText.AppendLine(" ORDER BY D.EffectDate desc ");
            //Param
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_HID", headerID);

            return this.db.FindList1<T_Cost_D>(cmdText.ToString(), paras);
        }
        #endregion

        #region Insert
        /// <summary>
        /// Insert data
        /// </summary>
        /// <param name="detailItem">T_Cost_D</param>
        /// <returns></returns>
        public int Update(T_Cost_D detailItem)
        {
            return 0;
        }
        #endregion

        #region Insert
        /// <summary>
        /// Insert data
        /// </summary>
        /// <param name="detailItem">T_Cost_D</param>
        /// <returns></returns>
        public int Insert(T_Cost_D detailItem)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine(" INSERT INTO dbo.T_Cost_D");

            cmdText.AppendLine(" (");
            cmdText.AppendLine("  HID");
            cmdText.AppendLine(" ,EffectDate");
            cmdText.AppendLine(" ,ExpireDate");
            cmdText.AppendLine(" ,CostAmount");
            cmdText.AppendLine(" )");

            cmdText.AppendLine(" VALUES");
            cmdText.AppendLine(" (");
            cmdText.AppendLine("  @IN_HID");
            cmdText.AppendLine(" ,@IN_EffectDate");
            cmdText.AppendLine(" ,@IN_ExpireDate");
            cmdText.AppendLine(" ,@IN_CostAmount");
            cmdText.AppendLine(" )");

            //Param
            Hashtable paras = new Hashtable();
            base.AddParam(paras, "IN_HID", detailItem.HID);
            base.AddParam(paras, "IN_EffectDate", detailItem.EffectDate);
            base.AddParam(paras, "IN_ExpireDate", detailItem.ExpireDate);
            base.AddParam(paras, "IN_CostAmount", detailItem.CostAmount);

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }
        #endregion


        #region Delete
        /// <summary>
        /// Delete data
        /// </summary>
        /// <param name="headerID">HeaderID</param>
        /// <returns></returns>
        public int Delete(int headerID)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" DELETE FROM dbo.T_Cost_D");

            //Param
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine(" HID = @IN_HID");
            base.AddParam(paras, "IN_HID", headerID);

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
