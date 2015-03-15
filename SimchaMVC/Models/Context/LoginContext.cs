using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SimchaMVC.Models.Context
{
    public class LoginContext : DbContext
    {

        public DbSet<admin_users> Users { get; set; }
        public LoginContext()
            : base("name=SimchaDB")
        {

        }
    }
}