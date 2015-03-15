using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace SimchaMVC.Areas.Admin.Controllers
{
     [Authorize(Roles="SuperAdmin")]
    public class AdminUsersController : Controller
    {
        SimchaDB DB = new SimchaDB();

              
        public ActionResult Index()
        {

         

            var users = from s in DB.admin_users
                       
                       select s;
           

          
            ViewBag.AdminUsers = users;
          
            
            return View();
            
            
            
        }



        public ActionResult AddNewRecord()
        {
            List<SelectListItem> roleList = new List<SelectListItem> { 
                new SelectListItem { 
                Text="Super Admin",
                Value="SuperAdmin"},
                 new SelectListItem { 
                Text="Hall Admin",
                Value="HallAdmin"},
                 new SelectListItem { 
                Text="Office",
                Value="office"}
            };
            ViewBag.roleList = roleList;
            return View();
        }

      

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewRecord(admin_users user)
        {                                                   

  

            if (ModelState.IsValid)
            {    
                WebSecurity.CreateUserAndAccount(user.user_name, user.user_password, new
                {
                    first_name = user.first_name,
                    last_name = user.last_name,
                    address = user.address,
                    city = user.city,
                    state = user.state,
                    zipcode = user.zipcode,
                    user_role = user.user_role

                });
               
                Roles.AddUserToRole(user.user_name,user.user_role);
                //DB.admin_users.Add(user);
                //DB.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(user);
        }
	

        public ActionResult Edit(int id = 0)
        {
            admin_users user = DB.admin_users.Find(id);







            if (user == null)
            {
                return HttpNotFound();
            }


            List<SelectListItem> roleList = new List<SelectListItem> { 
                new SelectListItem { 
                Selected=(Roles.IsUserInRole(user.user_name,"SuperAdmin")) ? true: false,
                Text="Super Admin",
                Value="SuperAdmin"},
                new SelectListItem { 
                Selected=(Roles.IsUserInRole(user.user_name,"HallAdmin")) ? true: false,

               Text="Hall Admin",
                Value="HallAdmin"}
            };
            ViewBag.roleList = roleList;


            return View(user);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(admin_users user)
        {

             
              string old_role;
              string old_username;
              using (var context = new SimchaDB())
              {
                  admin_users old = context.admin_users.Find(user.id);
                  old_role = old.user_role ?? "not assigned";
                  old_username = old.user_name;
              } 



            if (ModelState.IsValid)
            {
                var token = WebSecurity.GeneratePasswordResetToken(old_username, 1440);
                WebSecurity.ResetPassword(token, user.user_password);

                if (Roles.IsUserInRole(old_username, old_role))
                {
                    Roles.RemoveUserFromRole(old_username, old_role);
                
                }
                if (!Roles.IsUserInRole(old_username, user.user_role))
                {
                    Roles.AddUserToRole(old_username, user.user_role);
                }
               
                DB.Entry(user).State = EntityState.Modified;
                DB.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

   
 

        [HttpGet, ActionName("Delete")]
       
        public ActionResult DeleteConfirmed(int id)
        {
            admin_users user = DB.admin_users.Find(id);
            DB.admin_users.Remove(user);
            DB.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Assign(int id)
        {
            admin_users admin = DB.admin_users.Find(id);
            IQueryable<hall> AllHalls = DB.halls.AsNoTracking().AsQueryable();
            IQueryable<admin_users> AllAdminUsers = DB.admin_users.AsNoTracking().AsQueryable();

            IQueryable<SelectListItem> halllistSelect = from h in AllHalls
                                      select new SelectListItem
                                      {
                                          Selected = (h.admin_user_id==id),
                                          Text = h.hall_name,
                                          Value = h.id.ToString()
                                      };
            ViewBag.halllistSelect = halllistSelect;
            ViewBag.Admin = admin;
            ViewBag.AdminId = id;
            ViewBag.AllHalls = AllHalls;
            ViewBag.AllAdminUsers = AllAdminUsers;
             return View();
         }

        [HttpPost]
        public ActionResult Assign(int[] halls_list, int admin_user_id)
        {


            foreach (hall hall in DB.halls.Where(r=>r.admin_user_id==admin_user_id).AsNoTracking().AsQueryable())
            {
                using (var db = new SimchaDB())
                {
                 
                    hall.admin_user_id = 0;
                    db.Entry(hall).State = EntityState.Modified;
                    db.SaveChanges();

                }
            }


            foreach (int hall_id in halls_list)
            {

                using (var db = new SimchaDB())
                {
                    hall hall = db.halls.Find(hall_id);
                    hall.admin_user_id = admin_user_id;
                    db.Entry(hall).State = EntityState.Modified;
                    db.SaveChanges();

                }
               
            }


            return RedirectToAction("Assign", new {id = admin_user_id});
        }


        protected override void Dispose(bool disposing)
        {
            DB.Dispose();
            base.Dispose(disposing);
        }

    }
}
