using System;
using System.Collections.Generic;

namespace OMS.Models
{
    [Serializable]
    public class LoginInfo : System.IDisposable
    {
        /// <summary>
        /// User class
        /// </summary>
        public M_User User { get; set; }

        /// <summary>
        /// Group User class
        /// </summary>
        public M_GroupUser_H GroupUser { get; set; }

        /// <summary>
        /// List Group Detail
        /// </summary>
        public List<M_GroupUser_D> ListGroupDetail { get; set; }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            this.User = null;
            this.GroupUser = null;
            this.ListGroupDetail = null;
        }
    }
}