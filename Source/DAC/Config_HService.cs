using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using OMS.Models;
using OMS.Utilities;

namespace OMS.DAC
{
    /// <summary>
    /// Class M_Config_H Service
    /// </summary>
    public class Config_HService: BaseService
    {
        #region Contructor

        /// <summary>
        /// Contructor of M_Setting_H Service
        /// </summary>
        private Config_HService(): base()
        {
        }

        /// <summary>
        /// Contructor of M_Setting_H Service
        /// </summary>
        /// <param name="db">Class DB</param>
        public Config_HService(DB db): base(db)
        {
            
        }

        #endregion

        #region Get Data
        /// <summary>
        /// Create  :isv.thuy
        /// Date    :04/08/2014
        /// Get Default Value DropDownList
        /// </summary>
        /// <param name="configCd">Config Code</param>
        /// <returns></returns>
        public string GetDefaultValueDrop(string configCd)
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
            base.AddParam(paras, "IN_ConfigCD", configCd);
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

            return string.Format("{0}", this.db.ExecuteScalar1(cmdOutText.ToString(), paras));
        }

        /// <summary>
        /// Create  :isv.thuy
        /// Date    :04/08/2014
        /// Get Data For DropDownList
        /// </summary>
        /// <param name="configCd"></param>
        /// <param name="withBlank"></param>
        /// <returns></returns>
        public IList<DropDownModel> GetDataForDropDownList(string configCd, bool withBlank = false , Utilities.ConfigSort sortIndex = ConfigSort.value1)
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
            base.AddParam(paras, "IN_ConfigCD", configCd);
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            //SQL OUT
            StringBuilder cmdOutText = new StringBuilder();
            StringBuilder cmdOutWhere = new StringBuilder();
            cmdOutText.AppendLine(" SELECT");
            cmdOutText.AppendLine("  SD.Value1	as Value");
            cmdOutText.AppendLine(" ,SD.Value2	as DisplayName");
            cmdOutText.AppendLine(" FROM");
            cmdOutText.AppendLine(" (");
            cmdOutText.AppendLine(cmdText.ToString());
            cmdOutText.AppendLine(" ) AS  VIEW_1");
            cmdOutText.AppendLine(" INNER JOIN dbo.M_Config_D AS SD ON SD.HID = VIEW_1.ID");

            switch (sortIndex)
            {
                case Utilities.ConfigSort.value1:
                    cmdOutText.AppendLine(" ORDER BY SD.Value1");
                    break;

                case Utilities.ConfigSort.value2:
                    cmdOutText.AppendLine(" ORDER BY SD.Value2");
                    break;

                case Utilities.ConfigSort.value3:
                    cmdOutText.AppendLine(" ORDER BY SD.Value3");
                    break;

                case Utilities.ConfigSort.value4:
                    cmdOutText.AppendLine(" ORDER BY SD.Value4");
                    break;

                default:
                    break;
            }
            

            IList<DropDownModel> ret = this.db.FindList1<DropDownModel>(cmdOutText.ToString(), paras);

            if (withBlank)
            {
                ret.Insert(0, new DropDownModel("-1", "---"));
            }

            return ret;
        }

        /// <summary>
        /// Get Data For DropDownList Without Status = Lost and Sales
        /// ISV-TRUC
        /// </summary>
        /// <param name="configCd">configCd</param>
        /// <param name="withBlank">has blank</param>
        /// <returns></returns>
        public IList<DropDownModel> GetDataForDropDownListWithoutLostSales(string configCd, bool withBlank = false)
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
            base.AddParam(paras, "IN_ConfigCD", configCd);

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            //SQL OUT
            StringBuilder cmdOutText = new StringBuilder();
            StringBuilder cmdOutWhere = new StringBuilder();
            cmdOutText.AppendLine(" SELECT");
            cmdOutText.AppendLine("   SD.Value1	as Value");
            cmdOutText.AppendLine("  ,SD.Value2	as DisplayName");
            cmdOutText.AppendLine(" FROM");
            cmdOutText.AppendLine(" (");
            cmdOutText.AppendLine(cmdText.ToString());
            cmdOutText.AppendLine(" ) AS  VIEW_1");
            cmdOutText.AppendLine(" INNER JOIN dbo.M_Config_D AS SD ON SD.HID = VIEW_1.ID");

            cmdOutWhere.AppendLine(" SD.Value1 not in (@IN_StatusFlgLost, @IN_StatusFlgSales)");
            base.AddParam(paras, "IN_StatusFlgLost", Models.StatusFlag.Lost);
            base.AddParam(paras, "IN_StatusFlgSales", Models.StatusFlag.Sales);
            if (cmdOutWhere.Length != 0)
            {
                cmdOutText.AppendLine(" WHERE ");
                cmdOutText.AppendLine(cmdOutWhere.ToString());
            }

            IList<DropDownModel> ret = this.db.FindList1<DropDownModel>(cmdOutText.ToString(), paras);

            if (withBlank)
            {
                ret.Insert(0, new DropDownModel("-1", ""));
            }

            return ret;
        }

        /// <summary>
        /// Get List by condition
        /// </summary>
        /// <param name="configCD">Config Code</param>
        /// <param name="configName">Config Name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="sortField">Sort Field</param>
        /// <param name="sortDirec">Sort Direction</param>
        /// <returns>List Setting Info</returns>
        public IList<ConfigInfo> GetListByCond(string configCD, string configName,
                                            int pageIndex, int pageSize, int sortField, int sortDirec)
        {
            string[] fields = new string[] {"","", "ConfigCD", "ConfigName"};
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
            cmdText.AppendLine("  S.ID");
            cmdText.AppendLine(" ,S.ConfigCD");
            cmdText.AppendLine(" ,S.ConfigName");
            cmdText.AppendLine(" ," + string.Format("ROW_NUMBER() OVER(ORDER BY {0}) AS RowNumber", RowNumber));
            cmdText.AppendLine(" FROM dbo.M_Config_H AS S");

            //Para
            Hashtable paras = new Hashtable();
            if (!string.IsNullOrEmpty(configCD))
            {
                cmdWhere.AppendLine(" UPPER(S.ConfigCD) LIKE '%' + UPPER(@IN_ConfigCD) + '%'");
                base.AddParam(paras, "IN_ConfigCD", configCD, true);
            }

            if (!string.IsNullOrEmpty(configName))
            {
                if (cmdWhere.Length != 0)
                {
                    cmdWhere.AppendLine(" AND");
                }
                cmdWhere.AppendLine(" UPPER(S.ConfigName) LIKE '%' + UPPER(@IN_ConfigName) + '%'");
                base.AddParam(paras, "IN_ConfigName", configName, true);
            }

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            StringBuilder cmdOutText = new StringBuilder();
            StringBuilder cmdOutWhere = new StringBuilder();
            cmdOutText.AppendLine(" SELECT");
            cmdOutText.AppendLine("   *");
            cmdOutText.AppendLine(" FROM");
            cmdOutText.AppendLine(" (");
            cmdOutText.AppendLine(cmdText.ToString());
            cmdOutText.AppendLine(" ) AS  VIEW_1");

            cmdOutWhere.AppendLine(" RowNumber BETWEEN(@IN_PageIndex -1) * @IN_PageSize + 1 AND(((@IN_PageIndex -1) * @IN_PageSize + 1) + @IN_PageSize) - 1 ");
            base.AddParam(paras, "IN_PageIndex", pageIndex);
            base.AddParam(paras, "IN_PageSize", pageSize);
            base.AddParam(paras, "IN_SortField", sortField);
            base.AddParam(paras, "IN_SortDirec", sortDirec);
            if (cmdOutWhere.Length != 0)
            {
                cmdOutText.AppendLine(" WHERE ");
                cmdOutText.AppendLine(cmdOutWhere.ToString());
            }

            return this.db.FindList1<ConfigInfo>(cmdOutText.ToString(), paras);
        }

        /// <summary>
        /// Get total row for list
        /// </summary>
        /// <param name="configCD">Config Code</param>
        /// <param name="configName">Config Name</param>
        /// <returns></returns>
        public int getTotalRow(string configCD, string configName)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();
            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  count(*)");
            cmdText.AppendLine(" FROM dbo.M_Config_H AS S");

            //Para
            Hashtable paras = new Hashtable();
            if (!string.IsNullOrEmpty(configCD))
            {
                cmdWhere.AppendLine(" UPPER(S.ConfigCD) LIKE '%' + UPPER(@IN_ConfigCD) + '%'");
                base.AddParam(paras, "IN_ConfigCD", configCD, true);
            }

            if (!string.IsNullOrEmpty(configName))
            {
                if (cmdWhere.Length != 0)
                {
                    cmdWhere.AppendLine(" AND");
                }
                cmdWhere.AppendLine(" UPPER(S.ConfigName) LIKE '%' + UPPER(@IN_ConfigName) + '%'");
                base.AddParam(paras, "IN_ConfigName", configName, true);
            }

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }
            
            return int.Parse(this.db.ExecuteScalar1(cmdText.ToString(), paras).ToString());
        }

        /// <summary>
        /// Get by ID
        /// Create Author: ISV-HUNG
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public M_Config_H GetByID(int id)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  S.*");
            cmdText.AppendLine(" FROM dbo.M_Config_H S");

            //Param
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine(" S.ID = @IN_ID");
            base.AddParam(paras, "IN_ID", id);
            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.Find1<M_Config_H>(cmdText.ToString(), paras);
        }

        #endregion

        #region Check data

        /// <summary>
        /// Check exist Config by Config Code
        /// </summary>
        /// <param name="configCode">Config Code</param>
        /// <returns></returns>
        public bool IsExistConfigCode(string configCode)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" SELECT");
            cmdText.AppendLine("  COUNT(*)");
            cmdText.AppendLine(" FROM dbo.M_Config_H");

            //Param
            Hashtable paras = new Hashtable();
            cmdWhere.AppendLine(" ConfigCD = @IN_configCD");
            base.AddParam(paras, "IN_ConfigCD", configCode);

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            int count = int.Parse(this.db.ExecuteScalar1(cmdText.ToString(), paras).ToString());

            return count > 0;
        }

        #endregion

        #region Insert
        /// <summary>
        /// Insert data
        /// </summary>
        /// <param name="header">M_Config_H</param>
        /// <returns></returns>
        public int Insert(M_Config_H header)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            
            cmdText.AppendLine(" INSERT INTO dbo.M_Config_H ");
            cmdText.AppendLine("  (");
            cmdText.AppendLine("  [ConfigCD]");
            cmdText.AppendLine(" ,[ConfigName]");
            cmdText.AppendLine(" ,[CreateDate]");
            cmdText.AppendLine(" ,[CreateUID]");
            cmdText.AppendLine(" ,[UpdateDate]");
            cmdText.AppendLine(" ,[UpdateUID]");
            cmdText.AppendLine("  )");
            cmdText.AppendLine(" VALUES");
            cmdText.AppendLine(" (");
            cmdText.AppendLine("  @IN_ConfigCD");
            cmdText.AppendLine(" ,@IN_ConfigName");
            cmdText.AppendLine(" ,GETDATE()");
            cmdText.AppendLine(" ,@IN_CreateUID");
            cmdText.AppendLine(" ,GETDATE()");
            cmdText.AppendLine(" ,@IN_UpdateUID");
            cmdText.AppendLine(" )");

            //Param
            Hashtable paras = new Hashtable();
            base.AddParam(paras,"IN_ConfigCD", header.ConfigCD);
            base.AddParam(paras,"IN_ConfigName", header.ConfigName);

            base.AddParam(paras,"IN_CreateUID", header.CreateUID);
            base.AddParam(paras,"IN_UpdateUID", header.UpdateUID);

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }
        #endregion

        #region Update
        /// <summary>
        /// Update data
        /// </summary>
        /// <param name="header">M_Config_H</param>
        /// <returns></returns>
        public int Update(M_Config_H header)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" UPDATE dbo.M_Config_H ");
            cmdText.AppendLine(" SET ");
            cmdText.AppendLine("  ConfigName = @IN_ConfigName ");
            cmdText.AppendLine(" ,UpdateDate = GETDATE() ");
            cmdText.AppendLine(" ,UpdateUID	 = @IN_UpdateUID ");
            cmdWhere.AppendLine(" ID = @IN_ID AND UpdateDate = @IN_UpdateDate ");
            
            //Param
            Hashtable paras = new Hashtable();
            
            base.AddParam(paras,"IN_ID", header.ID);
            base.AddParam(paras,"IN_ConfigName", header.ConfigName);

            base.AddParam(paras,"IN_UpdateDate", header.UpdateDate, true);
            base.AddParam(paras,"IN_UpdateUID", header.UpdateUID);

            if (cmdWhere.Length != 0)
            {
                cmdText.AppendLine(" WHERE ");
                cmdText.AppendLine(cmdWhere.ToString());
            }

            return this.db.ExecuteNonQuery1(cmdText.ToString(), paras);
        }

        #endregion

        #region Delete
        /// <summary>
        /// Delete data
        /// </summary>
        /// <param name="ID">id</param>
        /// <param name="updateDate">updateDate</param>
        /// <returns></returns>
        public int Delete(int ID, DateTime updateDate)
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            StringBuilder cmdWhere = new StringBuilder();

            cmdText.AppendLine(" DELETE	dbo.M_Config_H");
            cmdWhere.AppendLine(" ID=@IN_ID AND UpdateDate = @IN_UpdateDate");

            //Param
            Hashtable paras = new Hashtable();
            base.AddParam(paras,"IN_ID", ID);
            base.AddParam(paras,"IN_UpdateDate", updateDate, true);

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
