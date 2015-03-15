using SimchaMVC.Models.ViewModelsWebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimchaMVC.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

     
        public ActionResult Blank()
        {

            return View();

        }


       
        public ActionResult Index()
        {
           

            //return RedirectToAction("Login", "Home", new { area = "Admin" });
            return RedirectToAction("Index", "Home", new { area = "Site" });

        }



    }
}
