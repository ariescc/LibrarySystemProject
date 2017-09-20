using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace LibraryProject.Models
{
    public class CheckLogin
    {
        public readonly static CheckLogin Instance = new CheckLogin();

        public bool IsLogin;

        public User GetUser()
        {
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if(authCookie == null)
            {
                return null;
            }
            else
            {
                FormsAuthenticationTicket Ticket = FormsAuthentication.Decrypt(authCookie.Value);
                return JsonConvert.DeserializeObject<User>(Ticket.UserData);
            }
        }
    }
}