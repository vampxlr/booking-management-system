using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimchaMVC.Areas.Admin.Controllers;

namespace SimchaMVC.Areas.Site.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Site/Home/
        SimchaDB DB = new SimchaDB();



     





        [HttpGet]
        public ActionResult Index()
        {
            IQueryable<service_areas> ServiceAreas = DB.service_areas.AsNoTracking().AsQueryable() ;
            return View(ServiceAreas);                                         
        }
        [HttpPost]
        public ActionResult Index(string StartDate, string EndDate, int[] area_ids, int[] event_type_ids, int[] couple_ids)
        {
            DateTime startDate = DateTime.Parse(StartDate);
            DateTime endDate = DateTime.Parse(EndDate);
            ViewBag.startDate = startDate;
            ViewBag.endDate = endDate;
            IEnumerable<int> type_names = event_type_ids;

            IEnumerable<int> couple_Ids = couple_ids;

            ViewBag.event_type_ids = event_type_ids;
            ViewBag.area_ids = area_ids;
            ViewBag.couple_ids = couple_ids;

            IEnumerable<int?> hallidsfortype_names = null;

            if (type_names != null)
            {

                hallidsfortype_names = from xx in DB.hall_event_types
                                       where type_names.Contains(xx.id)
                                       select xx.hall_id;

            }
            

            IQueryable<hall> Halls = DB.halls.AsNoTracking().AsQueryable();
            IQueryable<service_areas> ServiceAreas = DB.service_areas.AsNoTracking().AsQueryable();
            IQueryable<event_types> Eventypes = DB.event_types.AsNoTracking().AsQueryable();



            IQueryable<hall_images> AllHallsImage = DB.hall_images.AsNoTracking().AsQueryable();

            IQueryable<hall> AllHalls = DB.halls.AsNoTracking().AsQueryable();

            
            IEnumerable<int> service_area_ids = area_ids;
         

            IEnumerable<int?> hallidsforServiceArea = null;


            if (service_area_ids != null)
            {

                hallidsforServiceArea = from xx in DB.hall_areas
                                        where service_area_ids.Contains(xx.id)
                                        select xx.hall_id;
            }


            IEnumerable<hall> selectedHalls = AllHalls;
          
            if (service_area_ids != null)
            {

                selectedHalls = from xx in selectedHalls

                                where hallidsforServiceArea.Contains(xx.id)

                                select xx;
            }


            if (type_names != null)
            {

                selectedHalls = from xx in selectedHalls

                                where hallidsfortype_names.Contains(xx.id)
                                select xx;
            }

            if (couple_Ids != null)
            {

                foreach (int id in couple_Ids) {

                    switch (id)
                    {
                        case 1:
                            selectedHalls = selectedHalls.Where(r => Int32.Parse(r.capacity) > 0 && Int32.Parse(r.capacity) < 100);

                            break;
                        case 2:
                            selectedHalls = selectedHalls.Where(r => Int32.Parse(r.capacity) > 100 && Int32.Parse(r.capacity) < 200);
                            break;
                        case 3:
                            selectedHalls = selectedHalls.Where(r => Int32.Parse(r.capacity) > 200 && Int32.Parse(r.capacity) < 300);
                            break;
                        case 4:
                            selectedHalls = selectedHalls.Where(r => Int32.Parse(r.capacity) > 300 && Int32.Parse(r.capacity) < 400);
                            break;
                        case 5:
                            selectedHalls = selectedHalls.Where(r => Int32.Parse(r.capacity) > 400);
                            break;
                        default:
                       
                            break;
                    }


                    
                }

               
            }



            IQueryable<booking> Allbookings = from bookings in DB.bookings.AsNoTracking().AsQueryable().Where(r => r.booking_date >= startDate && r.booking_date <= endDate)
                                               select bookings;
            IQueryable<calendar> AlldisabledDates = from DD in DB.calendars.AsNoTracking().AsQueryable()
                                                     select DD;


            IQueryable<service_areas> AllServiceArea = from DD in DB.service_areas.AsNoTracking().AsQueryable()
                                                     select DD;

            ViewBag.AllHalls = AllHalls;

            ViewBag.Allbookings = Allbookings;
            ViewBag.AlldisabledDates = AlldisabledDates;
            ViewBag.AllHallsImage = AllHallsImage;


            if (service_area_ids != null || type_names != null || couple_Ids != null)
            {
                ViewBag.Halls = selectedHalls;
            }
            else
            {
                ViewBag.Halls = Halls;
            }

            ViewBag.Neighborhoods = ServiceAreas;
            ViewBag.SimchaTypes = Eventypes;

            return View("SearchResult");
        }
        [HttpGet]
        public ActionResult MyAccount()
        {
            return View();
        }
          [HttpPost]
        public ActionResult MyAccount(user user)
        {
            if (ModelState.IsValid)
            {
                DB.users.Add(user);
                DB.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);

        }
        public ActionResult Search()
        {
            IQueryable<event_types> Eventypes = DB.event_types.AsNoTracking().AsQueryable();

            IQueryable<service_areas> AllServiceArea = from DD in DB.service_areas.AsNoTracking().AsQueryable()
                                                        select DD;

            ViewBag.Neighborhoods = AllServiceArea;
            ViewBag.SimchaTypes = Eventypes;
           
            return View();
        }

        public ActionResult BookPage() {
            return View();
        }

        public ActionResult Book(string hall_id, DateTime event_date)
        {
            hall hall = DB.halls.Find(int.Parse(hall_id));
            IQueryable<time_slots> time_slots = DB.time_slots.AsNoTracking().AsQueryable().Where(r => r.hall_id == hall.id);

            IQueryable<event_types> all_event_types = DB.event_types.AsNoTracking().AsQueryable();
            IQueryable<hall_event_types> hall_event_typess = DB.hall_event_types.AsNoTracking().AsQueryable().Where(r => r.hall_id == hall.id);

            IEnumerable<string> typeIds = from h in hall_event_typess
                                       select h.type_id.ToString() ;

            IQueryable<event_types> selected_event_types = from xx in all_event_types

                                                            where typeIds.Contains(xx.id.ToString())

                                                            select xx;

            ViewBag.time_slots = time_slots;
            ViewBag.selected_event_types = selected_event_types;
            ViewBag.hall = hall;
            ViewBag.event_date = event_date;
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult SearchResult() {
            return View();
        }

    }
}
