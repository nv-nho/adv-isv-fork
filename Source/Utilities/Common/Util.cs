using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

using OMS.Models;
using OMS.DAC;

namespace OMS.Utity
{
    public class Util
    {
        /// <summary>
        /// Cach resource        
        /// </summary>
        public static void InitCache()
        {
            Cache cache = new Cache();

            using (DB db = new DB())
            {
                try
                {
                    MessageService msgSer = new MessageService(db);
                    IList<M_Message> lstMsg = msgSer.GetAll();

                    //Cache message
                    cache.AddMessages(lstMsg.ToList());

                    UserSession.Session.SetCacheSession(cache);
                }
                catch (Exception)
                {
                    throw;
                }
            }            
        }
    }
}