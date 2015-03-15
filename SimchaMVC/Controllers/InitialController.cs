using SimchaMVC.Models.ViewModelsWebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.SessionState;

namespace SimchaMVC.Controllers
{
    public class InitialController : Controller
    {
        //
        // GET: /Home/

     
        public ActionResult Blank()
        {

            return View();

        }


       
        public ActionResult Index()
        {
            //ActionResult x = RedirectToAction("Login", "Home", new { area = "admin" });
            ActionResult x = RedirectToAction("index", "trelo", new { area = "site" });
            return x;
          //  return RedirectToAction("Index", "Home", new { area = "Site" });

        }



    }
}
