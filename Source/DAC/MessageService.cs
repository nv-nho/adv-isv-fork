using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OMS.Models;

namespace OMS.DAC
{
    /// <summary>
    /// Class M_Message Service
    /// </summary>
    public class MessageService
    {
        #region Constructor
        /// <summary>
        /// Class DB
        /// </summary>
        private DB db;

        private MessageService() 
        {
        }

        public MessageService(DB db)
        {
            this.db = db;
        }

        #endregion

        #region Get Data

        /// <summary>
        /// Get All
        /// </summary>
        /// <returns>List Message</returns>
        public IList<M_Message> GetAll()
        {
            //SQL String
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine(" SELECT ");
            cmdText.AppendLine(" *");
            cmdText.AppendLine(" FROM ");
            cmdText.AppendLine(" dbo.M_Message");
            return this.db.FindList1<M_Message>(cmdText.ToString(), new Hashtable());
        }

        #endregion
    }
}
