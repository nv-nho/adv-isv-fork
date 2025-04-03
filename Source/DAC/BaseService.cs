using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using OMS.Models;
using OMS.Utilities;

namespace OMS.DAC
{
    public abstract class BaseService
    {
        #region Contructor

        /// <summary>
        /// Class DB
        /// </summary>
        protected DB db;

        /// <summary>
        /// Contructor of base struct service
        /// </summary>
        protected BaseService()
        {
            this.db = new DB();
        }

        /// <summary>
        /// Contructor of base struc service
        /// </summary>
        /// <param name="db">Class DB</param>
        protected BaseService(DB db)
        {
            this.db = db;
        }

        #endregion

        /// <summary>
        /// set Parameter
        /// </summary>
        /// <param name="cmd">SqlCommand</param>
        /// <param name="parameter">parameter</param>
        /// <param name="value">value</param>
        /// <param name="defaultValue">TRUE: IF value is default of it's type then set to null/ FALSE: set to normal</param>
        protected void AddParam(DbCommand cmd, string parameter, object value, bool defaultValueIsNull = false)
        {
            if (CommonUtil.IsNullVal(value, defaultValueIsNull))
            {
                cmd.Parameters.Add(new SqlParameter(parameter, DBNull.Value));
            }
            else
            {
                cmd.Parameters.Add(new SqlParameter(parameter, value));
            }
        }

        /// <summary>
        /// set Parameter
        /// </summary>
        /// <param name="htbl">Param List</param>
        /// <param name="key">Name</param>
        /// <param name="value">Value</param>
        /// <param name="defaultValue">TRUE: IF value is default of it's type then set to null/ FALSE: set to normal</param>
        protected void AddParam(Hashtable htbl, string key, object value, bool defaultValueIsNull = false)
        {
            if (CommonUtil.IsNullVal(value, defaultValueIsNull))
            {
                htbl.Add(key, DBNull.Value);
            }
            else
            {
                htbl.Add(key, value);
            }
        }    
    }
}
