using System.Collections.Generic;
using System.Web.Mvc;

namespace LoginSample.Models
{
    public class LoginInfo
    {
        /// <summary>
        /// Types of user
        /// </summary>
        public List<SelectListItem> UserType { get; set; }

        /// <summary>
        /// Selected type of user
        /// </summary>
        public string SelectedGroup { get; set; }

        /// <summary>
        /// All user name
        /// </summary>
        public List<SelectListItem> UserName { get; set; }

        public string UserSerial { get; set; }
        /// <summary>
        /// Selected username
        /// </summary>
        public string SelectedUser { get; set; }

        /// <summary>
        /// User password
        /// </summary>
        public string UserPassword { get; set; }

        /// <summary>
        /// User email
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// New user
        /// </summary>
        public string NewUser { get; set; }

        /// <summary>
        /// New user password
        /// </summary>
        public string NewUserPassword { get; set; }

        /// <summary>
        /// New user email
        /// </summary>
        public string NewUserEmail { get; set; }

        /// <summary>
        /// Value to share to user page
        /// </summary>
        public string UserPgId { get; set; }
    }
}