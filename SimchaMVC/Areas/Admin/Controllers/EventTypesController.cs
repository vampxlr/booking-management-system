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
    public class EventTypesController : Controller
    {


        SimchaDB DB = new SimchaDB();
        [HttpGet]
        public ActionResult Index()
        {

      


            var events = from s in DB.event_types
                        select s;
           


          
            ViewBag.Events = events;
            return View(events);


        }

        public ActionResult AddNewRecord()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewRecord(event_types eventT)
        {
            if (ModelState.IsValid)
            {
                DB.event_types.Add(eventT);
                DB.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(eventT);
        }

        public ActionResult Edit(int id = 0)
        {
            event_types eventT = DB.event_types.Find(id);
            if (eventT == null)
            {
                return HttpNotFound();
            }
            return View(eventT);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(event_types eventT)
        {
            if (ModelState.IsValid)
            {
                DB.Entry(eventT).State = EntityState.Modified;
                DB.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(eventT);
        }




        [HttpGet, ActionName("Delete")]

        public ActionResult DeleteConfirmed(int id)
        {
            event_types eventT = DB.event_types.Find(id);
            DB.event_types.Remove(eventT);
            DB.SaveChanges();


            if (DB.hall_event_types.Any(r => r.type_id == id))
            {
                IEnumerable<hall_event_types> hallEventsToRemove = DB.hall_event_types.Where(r => r.type_id == id).ToList();

                foreach (var item in hallEventsToRemove)
                {
                    DB.hall_event_types.Remove(item);
                    DB.SaveChanges();

                }
            }

            return RedirectToAction("Index");
        }



        protected override void Dispose(bool disposing)
        {
            DB.Dispose();
            base.Dispose(disposing);
        }


    }
}
