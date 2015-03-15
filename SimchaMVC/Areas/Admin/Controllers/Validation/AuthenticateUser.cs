using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimchaMVC.Areas.Admin.Controllers.Validation
{
    public class AuthenticateUser
    {
        SimchaDB DB = new SimchaDB();
        public bool isUserAuthentic(string UserName,string Password) {
            bool Authentic = false;
            admin_users User = DB.admin_users.SingleOrDefault(u => u.user_name == UserName);
           
            if (User != null)
            {
                if (User.user_password == Password)
                {
                    Authentic = true;

                }
                else
                {
                    Authentic = false;
                }


            }
            return Authentic;
            
        }
    }
}