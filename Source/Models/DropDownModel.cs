using System;
using System.Data.Common;

namespace OMS.Models
{
    /// <summary>
    /// Drop down list model
    /// </summary>
    [Serializable]
    public class DropDownModel
    {
        #region Variable

        /// <summary>
        /// Value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Display Name
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Model Item
        /// </summary>
        public object DataboundItem { get; set; }

        #endregion

        #region Contructor

        /// <summary>
        /// Drop Down Model Contructor
        /// </summary>
        public DropDownModel()
        {

        }

        /// <summary>
        /// Drop Down Model Contructor
        /// </summary>
        public DropDownModel(string value, string displayName)
        {
            this.Value = value;
            this.DisplayName = displayName;
        }

        /// <summary>
        /// Drop Down Model Contructor
        /// </summary>
        public DropDownModel(string value, string displayName, string status)
        {
            this.Value = value;
            this.DisplayName = displayName;
            this.Status = status;
        }

        /// <summary>
        /// Drop down model contructor 
        /// </summary>
        /// <param name="dr"></param>
        public DropDownModel(DbDataReader dr)
        {
            this.Value = string.Format("{0}", dr["Value"]);
            this.DisplayName = (string)dr["DisplayName"];
            if (dr.FieldCount > 2)
            {
                this.Status = dr["StatusFlag"].ToString();
            }


        }

        #endregion

    }
}
