using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data.Entity;
using System.Data;

namespace SimchaMVC.Areas.Admin.Controllers
{
    [Authorize]
    public class ServiceAreasController : Controller
    {


        SimchaDB DB = new SimchaDB();
    
        public ActionResult Index()
        {



            var serviceAreas = from s in DB.service_areas
                         select s;
           



            ViewBag.ServiceAreas = serviceAreas;
            return View(serviceAreas);


        }

        public ActionResult AddNewRecord()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewRecord(service_areas serviceArea)
        {
            if (ModelState.IsValid)
            {
                DB.service_areas.Add(serviceArea);
                DB.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(serviceArea);
        }

        public ActionResult Edit(int id = 0)
        {
            service_areas serviceArea = DB.service_areas.Find(id);
            if (serviceArea == null)
            {
                return HttpNotFound();
            }
            return View(serviceArea);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(service_areas serviceArea)
        {
            if (ModelState.IsValid)
            {
                DB.Entry(serviceArea).State = EntityState.Modified;
                DB.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(serviceArea);
        }




        //[HttpGet, ActionName("Delete")]

        //public ActionResult DeleteConfirmed(int id)
        //{
        //    service_areas serviceArea = DB.service_areas.Find(id);
        //    DB.service_areas.Remove(serviceArea);
        //    DB.SaveChanges();
           
        //    if (DB.hall_areas.Any(r => r.area_id == id)) {
        //        IEnumerable<hall_areas> areaToRemove = DB.hall_areas.Where(r => r.area_id == id).ToList();

        //        foreach (var item in areaToRemove)
        //        {
        //            DB.hall_areas.Remove(item);
        //            DB.SaveChanges();
           
        //        }
        //    }

        //    return RedirectToAction("Index");
        //}



        protected override void Dispose(bool disposing)
        {
            DB.Dispose();
            base.Dispose(disposing);
        }



    }
}
