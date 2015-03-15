using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Net;
using System.Data;
namespace SimchaMVC.Areas.Admin.Controllers
{
     [Authorize]
    public class CustomersController : Controller
    {

         SimchaDB DB = new SimchaDB();
         [HttpGet]
         public ActionResult Index()
         {


             var users = from s in DB.users
                         select s;
           
             ViewBag.Users = users;
             return View(users);


         }
     
         
      
        public ActionResult AddNewRecord()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewRecord(user user)
        {

            user.internal_notes = WebUtility.HtmlDecode(user.internal_notes);
           
            if (ModelState.IsValid)
            {
                DB.users.Add(user);
                DB.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }



        public ActionResult Edit(int id = 0)
        {
            user user = DB.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(user user)
        {
            user.internal_notes = WebUtility.HtmlDecode(user.internal_notes);
            if (ModelState.IsValid)
            {
                DB.Entry(user).State = EntityState.Modified;
                DB.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

   


        [HttpGet, ActionName("Delete")]

        public ActionResult DeleteConfirmed(int id)
        {
            user user = DB.users.Find(id);
            DB.users.Remove(user);
            DB.SaveChanges();
            return RedirectToAction("Index");
        }



        protected override void Dispose(bool disposing)
        {
            DB.Dispose();
            base.Dispose(disposing);
        }
        

    }
}
