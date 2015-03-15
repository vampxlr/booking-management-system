using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimchaMVC.Areas.Admin.Controllers;
using SimchaMVC.Models.ViewModelsWebApi;

namespace SimchaMVC.Areas.Site.Controllers
{
    public class SemanticController : Controller
    {
        SimchaDB DB = new SimchaDB();

        [HttpGet]
        public JsonResult GetHalls(string text)
        {



            var halls = DB.halls.Select(hall => new ViewModel_Hall_Id_Name
            {
                hall_id = hall.id,
                hall_name = hall.hall_name
            });


            return Json(halls, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetLocations(string text)
        {



            var locations = DB.service_areas.Select(location => new ViewModel_Location_Id_Name
            {
                location_id = location.id,
                location_name = location.service_area
            });


            return Json(locations, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetEvents(string text)
        {



            var events = DB.event_types.Select(e=> new ViewModel_Event_Id_Name
            {
                event_id = e.id,
                event_name = e.type_name
            });


            return Json(events, JsonRequestBehavior.AllowGet);
        }



        // GET: Site/Semantic
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(user user)
        {
            if (ModelState.IsValid)
            {
                DB.users.Add(user);
                DB.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        public ActionResult Search(string startDate, string endDate, int[] locations)
        {
            ViewBag.StartDate = !String.IsNullOrEmpty(startDate)?startDate:DateTime.Now.ToString("M/dd/yyyy");
            ViewBag.EndDate = !String.IsNullOrEmpty(startDate) ? endDate : DateTime.Now.AddDays(7).ToString("M/dd/yyyy");
            ViewBag.Locations = locations;

            return View();
        }

        public ActionResult SearchView(string hall_id, DateTime event_date)
        {
            hall hall = DB.halls.Find(int.Parse(hall_id));
            IQueryable<time_slots> time_slots = DB.time_slots.AsNoTracking().AsQueryable().Where(r => r.hall_id == hall.id);

            IQueryable<event_types> all_event_types = DB.event_types.AsNoTracking().AsQueryable();
            IQueryable<hall_event_types> hall_event_typess = DB.hall_event_types.AsNoTracking().AsQueryable().Where(r => r.hall_id == hall.id);

            IEnumerable<string> typeIds = from h in hall_event_typess
                                          select h.type_id.ToString();

            IQueryable<event_types> selected_event_types = from xx in all_event_types

                                                           where typeIds.Contains(xx.id.ToString())

                                                           select xx;

            ViewBag.time_slots = time_slots;
            ViewBag.selected_event_types = selected_event_types;
            ViewBag.hall = hall;
            ViewBag.event_date = event_date;

            return View();
        }
    }
}