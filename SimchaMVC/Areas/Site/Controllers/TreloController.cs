using System;
using System.Collections.Generic;
using System.Linq;

using System.Web.Mvc;
using SimchaMVC.Areas.Admin.Controllers;
using SimchaMVC.Models;
using SimchaMVC.Models.ViewModelsWebApi;
using System.Data.Entity;

namespace SimchaMVC.Areas.Site.Controllers
{
    public class TreloController : Controller
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
        public JsonResult GetAreas(string text)
        {



            var Areas = DB.service_areas.Select(x => new { x.service_area, x.id });


            return Json(Areas, JsonRequestBehavior.AllowGet);
        }
         [HttpGet]
        public JsonResult GetEvents(string text)
        {



            var Events = DB.event_types.Select(x => new { x.type_name, x.id });


            return Json(Events, JsonRequestBehavior.AllowGet);
        }


                       [HttpGet]

         public JsonResult GetZips(string value)
         {



             var zipcodes = DB.zips.AsNoTracking().AsQueryable().Select(x => new { x.zip1 });

             if (!string.IsNullOrEmpty(value))
             {
                 zipcodes = zipcodes.Where(p => p.zip1.Contains(value));
             }

             return Json(zipcodes, JsonRequestBehavior.AllowGet);
         }

        
                          [HttpGet]

       public JsonResult GetCaterers(string text)
        {



            var Cat = DB.caterers.Select(x => new { x.caterer_name, x.id });


            return Json(Cat, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTimeSlots(int hallId, DateTime eventDate)
        {
            DateTime date = eventDate;
            var weekdayid = 0;
            var day = eventDate.ToString("dddd");
            switch (day)
            {
                case "Sunday":
                    weekdayid = 1;
                    break;
                case "Monday":
                    weekdayid = 2;
                    break;
                case "Tuesday":
                    weekdayid = 3;
                    break;
                case "Wednesday":
                    weekdayid = 4;
                    break;
                case "Thursday":
                    weekdayid = 5;
                    break;
                case "Friday":
                    weekdayid = 6;
                    break;
                case "Saturday":
                    weekdayid = 7;
                    break;

                default:
                    weekdayid = 0;
                    break;
            }


            IEnumerable<time_slots> TimeSlots =
                DB.time_slots.Where(x => x.hall_id == hallId && x.weekday_id == weekdayid).ToList();

            IEnumerable<special_time_slots> disabledSlotsId =
                DB.special_time_slots.Where(
                    r => r.is_disable_record == true && r.is_special_slot == false && r.special_date == date)
                    .ToList();
            IEnumerable<special_time_slots> specialdisabledSlotsId =
                DB.special_time_slots.Where(
                    r => r.is_disable_record == true && r.is_special_slot == true && r.special_date == date)
                    .ToList();


            IEnumerable<ViewModelHallCalenderTimeSlot> timeSlots = from t in TimeSlots
                                                                   join disabled in disabledSlotsId on t.id equals disabled.disable_time_slot_id into gj
                                                                   from subdisabled in gj.DefaultIfEmpty()
                                                                   select new ViewModelHallCalenderTimeSlot
                                                                   {
                                                                       id = t.id,
                                                                       date = date.ToString("M/d/yyyy"),
                                                                       hall_id = t.hall_id,
                                                                       time_slot = t.time_slot,
                                                                       weekday_id = weekdayid,
                                                                       disabled = (subdisabled != null && subdisabled.is_disable_record)
                                                                   };

            IEnumerable<special_time_slots> Special_timeslots =
                DB.special_time_slots.Where(r => r.special_date == date && r.hall_id == hallId).ToList();

            IEnumerable<ViewModelHallCalenderTimeSlot> vSpecial_timeslots = from s in Special_timeslots


                                                                            select new ViewModelHallCalenderTimeSlot
                                                                            {
                                                                                id = 0,
                                                                                date = date.ToString("M/d/yyyy"),
                                                                                hall_id = s.hall_id,
                                                                                disabled = s.is_disable_record,
                                                                                time_slot =
                                                                                    ((s.special_time_slot != null) ? s.special_time_slot += "[Special Slot]" : s.special_time_slot)


                                                                            };

            IEnumerable<ViewModelHallCalenderTimeSlot> join = timeSlots.Union(vSpecial_timeslots);

            var filtered = join.Where(i => i.time_slot != null);

            var ETimeSlots =
                             filtered.Where(r => r.disabled == false).Select(r => new {r.time_slot,  r.id}).ToArray();




            return Json(ETimeSlots, JsonRequestBehavior.AllowGet);
        }

           
        
        
        
        // GET: Site/Trelo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(DateTime? startDate, DateTime? endDate, int?[] areaIds, int?[] AreaIds, int[] hallIds, int?[] eventIds)
        {

            ViewBag.startDate = startDate ?? DateTime.Now ;
            ViewBag.endDate = endDate ?? DateTime.Now.AddDays(7) ;

            var halls = DB.halls.Select(hall => new
            {
                hall_id = hall.id,
                 hall.hall_name
            }).AsEnumerable();
          


            List<SelectListItem> hallIdsSelectList = (from h in halls.ToList()
                                                       select new SelectListItem
                                                       {
                                                           Selected = (hallIds!=null) && (hallIds.Contains(h.hall_id)),
                                                           Text = h.hall_name,
                                                           Value = h.hall_id.ToString()
              
                                                       }).ToList();



            var Areas = DB.service_areas.Select(area => new
            {
                areaId = area.id,
                areaName = area.service_area
            }).AsEnumerable();


            var areaids = areaIds ?? AreaIds;
            List<SelectListItem> areaIdsSelectList = (from h in Areas.ToList()
                                                      select new SelectListItem
                                                      {
                                                          Selected = (areaids != null) && (areaids.Contains(h.areaId)),
                                                          Text = h.areaName,
                                                          Value = h.areaId.ToString()
                                                      }).ToList();


            var events = DB.event_types.Select(e => new
            {
                type_id = e.id,
                e.type_name
            }).AsEnumerable();

            List<SelectListItem> eventIdsSelectList = (from h in events.ToList()
                                                      select new SelectListItem
                                                      {
                                                          Selected = (eventIds != null) && (eventIds.Contains(h.type_id)),
                                                          Text = h.type_name,
                                                          Value = h.type_id.ToString()

                                                      }).ToList();





            ViewBag.areaIds = areaIdsSelectList;
            ViewBag.hallIds = hallIdsSelectList;
            ViewBag.eventIds = eventIdsSelectList;
           

            return View();
        }

        public ActionResult BookingDetails(int hallId , DateTime eventDate)
        {
            ViewBag.hallId = hallId;
            ViewBag.eventDate = eventDate;


            int weekdayid;

            string day = eventDate.ToString("dddd");
            switch (day)
            {
                case "Sunday":
                    weekdayid = 1;
                    break;
                case "Monday":
                    weekdayid = 2;
                    break;
                case "Tuesday":
                    weekdayid = 3;
                    break;
                case "Wednesday":
                    weekdayid = 4;
                    break;
                case "Thursday":
                    weekdayid = 5;
                    break;
                case "Friday":
                    weekdayid = 6;
                    break;
                case "Saturday":
                    weekdayid = 7;
                    break;

                default:
                    weekdayid = 0;
                    break;
            }





            hall hall = DB.halls.Find(hallId);
            ViewBag.hall = hall;
            IEnumerable<user> Users = DB.users.ToList();
            hall_images hallimage = DB.hall_images.FirstOrDefault(i => i.hall_id == hall.id);
            ViewBag.Image = hallimage;
            IEnumerable<time_slots> Alltime_slots = DB.time_slots.Where(r => r.hall_id == hallId && r.weekday_id == weekdayid).ToList();
            IEnumerable<time_slots> DisabledTime_Slots = from d in Alltime_slots
                                                         join t in DB.special_time_slots.Where(r => r.special_date == eventDate).ToList() on d.id equals t.disable_time_slot_id
                                                         select d;

            IEnumerable<time_slots> EnabledTimeSlots = Alltime_slots.Except(DisabledTime_Slots);
            IEnumerable<ViewModelHallCalenderTimeSlot> vEnabledTimeSlots = from e in EnabledTimeSlots
                                                                           select new ViewModelHallCalenderTimeSlot
                                                                           {
                                                                               id = e.id,
                                                                               time_slot = e.time_slot,
                                                                               is_special_slot = false,
                                                                               slot_amount = e.slot_amount,
                                                                               slot_price = e.slot_price

                                                                           };
            IEnumerable<ViewModelHallCalenderTimeSlot> SEnabledTimeSlots = from d in DB.special_time_slots.Where(r => r.is_disable_record == false && r.is_special_slot == true && r.hall_id == hallId && r.special_date == eventDate).ToList()

                                                                           select new ViewModelHallCalenderTimeSlot
                                                                           {
                                                                               id = d.id,
                                                                               time_slot = d.special_time_slot,
                                                                               is_special_slot = true,
                                                                               slot_amount = d.slot_amount.ToString(),
                                                                               slot_price = d.slot_price
                                                                           };

            IEnumerable<ViewModelHallCalenderTimeSlot> union = vEnabledTimeSlots.Union(SEnabledTimeSlots);
            ViewBag.Time_Slots = union;











            return View();
        }
        [HttpGet]
        public ActionResult BookingConfirm( int hallId, DateTime bookingDate,string time_slot)
        {

            booking book = new booking();
            book.hall_id = hallId;
            ViewBag.hall = DB.halls.Find(hallId);
            book.booking_date = bookingDate;
            book.time_slot = time_slot;
            string[] words = book.time_slot.Split(' ');
            int slot_id = Int32.Parse(words[0]);
            bool isSpecialSlot = (words[1].ToUpper() == "TRUE");
            string price;
            if (isSpecialSlot)
            {
                price = Math.Round(Convert.ToDecimal(DB.special_time_slots.Find(slot_id).slot_price), 2).ToString();
                book.time_slot = DB.special_time_slots.Find(slot_id).special_time_slot;
            }
            else
            {
                book.time_slot = DB.time_slots.Find(slot_id).time_slot;
                price = Math.Round(Convert.ToDecimal(DB.time_slots.Find(slot_id).slot_price), 2).ToString();
            }

            book.total = Convert.ToDouble(price);
            book.time_slot_id = slot_id;

            book.is_special_slot = isSpecialSlot;
            ViewBag.Booking = book;
            return View();
        }
        [HttpPost]
        public ActionResult BookingConfirm(booking book)
        {

          
            if (ModelState.IsValid)
            {
                
                if (book.is_special_slot.Value)
                {
                    DisableTimeSlot(book.time_slot_id.Value, book.hall_id.Value, book.booking_date, book.is_special_slot.Value);
                }
                else
                {
                    DisableTimeSlot(book.time_slot_id.Value, book.hall_id.Value, book.booking_date, book.is_special_slot.Value);
                }
                return RedirectToAction("BookingThank");
            }
            ViewBag.Booking = book;
            return View();
        }

        public ActionResult BookingThank()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        public string GetSlotPrice(string priceIsSpecialSlot)
        {


            string[] words = priceIsSpecialSlot.Split(' ');
            int slot_id = Int32.Parse(words[0]);
            bool isSpecialSlot = (words[1].ToUpper() == "TRUE") ? true : false;
            string price;
            if (isSpecialSlot)
            {

                price = Math.Round(Convert.ToDecimal(DB.special_time_slots.Find(slot_id).slot_price), 2).ToString();
            }
            else
            {

                price = Math.Round(Convert.ToDecimal(DB.time_slots.Find(slot_id).slot_price), 2).ToString();
            }

            return price;

        }
        public void DisableTimeSlot(int time_slot_id, int hall_id, DateTime date, bool special_slot)
        {

            int weekdayid = 0;
            string day = date.ToString("dddd");
            switch (day)
            {
                case "Sunday":
                    weekdayid = 1;
                    break;
                case "Monday":
                    weekdayid = 2;
                    break;
                case "Tuesday":
                    weekdayid = 3;
                    break;
                case "Wednesday":
                    weekdayid = 4;
                    break;
                case "Thursday":
                    weekdayid = 5;
                    break;
                case "Friday":
                    weekdayid = 6;
                    break;
                case "Saturday":
                    weekdayid = 7;
                    break;

                default:
                    weekdayid = 0;
                    break;
            }

            if (special_slot == false)
            {

                IEnumerable<special_time_slots> special = DB.special_time_slots.Where(r => r.special_date == date && r.disable_time_slot_id == time_slot_id && r.is_disable_record == true && r.hall_id == hall_id && r.is_special_slot == false).ToList();
                if (special.Count() == 0)
                {

                    special_time_slots specialTimeslot = new special_time_slots();

                    specialTimeslot.hall_id = hall_id;

                    specialTimeslot.disable_time_slot_id = time_slot_id;
                    specialTimeslot.is_disable_record = true;
                    specialTimeslot.is_special_slot = false;
                    specialTimeslot.special_date = date;
                    specialTimeslot.weekday_id = weekdayid;



                    if (ModelState.IsValid)
                    {
                        DB.special_time_slots.Add(specialTimeslot);
                        DB.SaveChanges();
                    }

                }
                else
                {

                    foreach (var item in special)
                    {

                        DB.special_time_slots.Remove(item);
                        DB.SaveChanges();

                    }
                }

            }
            else if (special_slot == true)
            {

                special_time_slots special2 = DB.special_time_slots.FirstOrDefault(r => r.id == time_slot_id && r.special_date == date && r.is_special_slot == true);



                if (special2.is_disable_record == false)
                {

                    special2.is_disable_record = true;

                    if (ModelState.IsValid)
                    {
                        DB.Entry(special2).State = EntityState.Modified;
                        DB.SaveChanges();
                    }




                }
                else if (special2.is_disable_record == true)
                {

                    special2.is_disable_record = false;

                    if (ModelState.IsValid)
                    {
                        DB.Entry(special2).State = EntityState.Modified;
                        DB.SaveChanges();
                    }


                }






            }


        }

    }
}