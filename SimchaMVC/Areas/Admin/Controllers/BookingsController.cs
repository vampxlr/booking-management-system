using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using SimchaMVC.Models.ViewModelsWebApi;
using SimchaMVC.Models;
using System.Data;
using System.Text;
using WebMatrix.WebData;
using System.IO;
using System.Globalization;

namespace SimchaMVC.Areas.Admin.Controllers
{
    [Authorize]

    public class BookingsController : Controller
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
        public ActionResult Index(int?[] hall_id)
        {
            IQueryable<booking> bookingAll;

            if (User.IsInRole("HallAdmin"))
            {
                IQueryable<hall> halls = DB.halls.AsNoTracking().AsQueryable().Where(r => r.admin_user_id == WebSecurity.CurrentUserId);
                bookingAll = from b in DB.bookings.AsNoTracking().AsQueryable()
                             join sa in halls on b.hall_id equals sa.id
                             select b;
            }
            else
            {
                bookingAll = DB.bookings.AsNoTracking().AsQueryable();

            }

                    

            var bookings = from s in bookingAll.ToList()
                join sa in DB.users on s.user_id equals sa.id
                           where (hall_id == null) || hall_id.Contains(s.hall_id) 
                           select new ViewModelBookingUser
                           {
                               id = s.id.ToString(),
                               user_name = sa.user_name ?? "Custom User",
                               time_slot = s.time_slot,
                               booking_date = s.booking_date,
                               booking_status = s.booking_status,
                               total = s.total,
                               create_date = s.create_date.Value.ToString("yyyy/MM/dd"),
                               customer_notes = s.customer_notes,
                               payment_type = s.payment_type,
                               card_type = s.card_type,
                               card_expiration = s.card_number,
                               cvv = s.cvv
                           };
           
            ViewBag.Bookings = bookings;
            return View(bookings);


        }

        [HttpGet]
        public ActionResult AddNewRecord()
        {
            ViewBag.isPost = false;
            var dates = new DateTime[7];
            var today = DateTime.Now.Date;
            for (var x = 0; x < dates.Length; x++)
            {
                dates[x] = today.AddDays(x);
            }


            var ServiceAreas = DB.service_areas.AsNoTracking().ToList().AsQueryable();
            var Eventypes = DB.event_types.AsNoTracking().ToList().AsQueryable();


            ViewBag.Neighborhoods = ServiceAreas.AsQueryable();
            ViewBag.SimchaTypes = Eventypes.AsQueryable();
            ViewBag.CurrentDate = DateTime.Now.Date.ToString("M/d/yyyy");


            IQueryable AllHalls;

            if (User.IsInRole("HallAdmin"))
            {
                AllHalls = DB.halls.AsNoTracking().AsQueryable().Where(r => r.admin_user_id == WebSecurity.CurrentUserId);

            }
            else
            {
                AllHalls = from s in DB.halls.AsQueryable().AsNoTracking()
                        select s;
            }




            ViewBag.AllHalls = AllHalls.AsQueryable();

            ViewBag.AllDates = dates;
            ViewBag.PostPage = false;
   

            return View();

        

        }

        [HttpPost]
        public ActionResult AddNewRecord(string zipcode, string zipprox, string[] hall_id, string[] service_area_id, string[] type_name, string CurrentDate)
        {
            ViewBag.CurrentDate = CurrentDate;
            String date = CurrentDate;
            ViewBag.hall_id = hall_id;
            ViewBag.service_area_id = service_area_id;
            ViewBag.type_name = type_name;
            ViewBag.isPost = true;
            ViewBag.date = date;
            ViewBag.zipcode = zipcode;
            ViewBag.zipprox = zipprox;

            var dates = new DateTime[7];
            ViewBag.PostPage = true;
            DateTime today= DateTime.Now.Date;
            if (date != "")
            {
                String format = "M/d/yyyy";

               today = DateTime.ParseExact(date,format, CultureInfo.InvariantCulture).AddDays(-3);
               
            }

            for (var x = 0; x < dates.Length; x++)
            {
                dates[x] = today.AddDays(x);
            }


            var today7 = today.AddDays(7);




      


            IQueryable<hall> Halls;

            if (User.IsInRole("HallAdmin"))
            {
                Halls = DB.halls.AsNoTracking().AsQueryable().Where(r => r.admin_user_id == WebSecurity.CurrentUserId);

            }
            else
            {
                Halls = from s in DB.halls.AsNoTracking().AsQueryable()
                           select s;
            }
            
            
            var ServiceAreas = DB.service_areas.AsNoTracking().AsQueryable();
            var Eventypes = DB.event_types.AsNoTracking().AsQueryable();



            var AllHallsImage = DB.hall_images.AsNoTracking().AsQueryable();

            IQueryable<hall> AllHalls;

            if (User.IsInRole("HallAdmin"))
            {
                AllHalls = DB.halls.AsNoTracking().AsQueryable().Where(r => r.admin_user_id == WebSecurity.CurrentUserId);

            }
            else
            {
                AllHalls = from s in DB.halls.AsNoTracking().AsQueryable()
                           select s;
            }
            IEnumerable<string> hall_ids = hall_id;
            IEnumerable<string> service_area_ids = service_area_id;
            IEnumerable<string> type_names = type_name;

            IEnumerable<string> hallidsforServiceArea = null;


            if (service_area_ids != null)
            {

                hallidsforServiceArea = from xx in DB.hall_areas
                                        where service_area_ids.Contains(xx.area_id.ToString())
                                        select xx.hall_id.ToString();
            }


            IEnumerable<string> hallidsfortype_names = null;

            if (type_names != null)
            {

                hallidsfortype_names = from xx in DB.hall_event_types
                                       where type_names.Contains(xx.type_id.ToString())
                                       select xx.hall_id.ToString();

            }

            IQueryable<hall> selectedHalls = AllHalls;
            if (hall_ids != null)
            {

                selectedHalls = from xx in AllHalls.AsQueryable().AsNoTracking()
                                where hall_ids.Contains(xx.id.ToString())

                                select xx;
            }
            if (service_area_ids != null)
            {

                selectedHalls = from xx in selectedHalls.AsQueryable().AsNoTracking()

                                where hallidsforServiceArea.Contains(xx.id.ToString())

                                select xx;
            }
            if (type_names != null)
            {

                selectedHalls = from xx in selectedHalls.AsQueryable().AsNoTracking()

                                where hallidsfortype_names.Contains(xx.id.ToString())
                                select xx;
            }

            var zipOut = zipcode;
            var zip_existsOut = DB.zips.AsQueryable().AsNoTracking().Any(rec => rec.zip1.Equals(zipOut));
            if (!zip_existsOut && zipcode != "")
            {
                ViewBag.Error = "Given zip Code is not a valid zip code";
                ViewBag.PostPage = false;
            }
            if (!string.IsNullOrEmpty(zipcode) && zip_existsOut)
            {
                IList<hall> tempSelectedHalls = selectedHalls.ToList();

                var zipSelectedHalls = (from hallItem in tempSelectedHalls let zip = hallItem.zip_code let zip_exists = DB.zips.AsQueryable().AsNoTracking().Any(rec => rec.zip1.Equals(zip)) where zip_exists let distanceDiffB2nzips = distancezip(zipcode, hallItem.zip_code) - double.Parse(zipprox) where distanceDiffB2nzips < 0 select hallItem).ToList();

                selectedHalls = zipSelectedHalls.AsQueryable().AsNoTracking();
            }



            var Alltimeslotss = from t in DB.time_slots.AsQueryable().AsNoTracking()
                                                                      select new ViewModelHallCalenderTimeSlot
                                                                      { id = t.id,
                                                                        hall_id =t.hall_id,
                                                                        slot_amount =t.slot_amount,
                                                                        time_slot= t.time_slot,
                                                                        weekday_id = t.weekday_id
                                                                      
                                                                      
                                                                      };

            IQueryable<ViewModelHallCalenderTimeSlot> AllSpecialTimeSlots = from t in DB.special_time_slots.Where(r => r.is_special_slot == true && r.is_disable_record == false) .ToList().AsQueryable().AsNoTracking()
                                                                             select new ViewModelHallCalenderTimeSlot
                                                                             {
                                                                                 id = t.id,
                                                                                 hall_id = t.hall_id,
                                                                                 slot_amount = t.slot_amount.ToString(),
                                                                                 time_slot = t.special_time_slot,
                                                                                 date = t.special_date.ToString("MM/dd/yyyy")


                                                                            };
            IQueryable<ViewModelDisabledHallCalenderTimeSlot> AllSpecialDisabledTimeSlots = from t in DB.special_time_slots.Where(r => r.is_special_slot == false && r.is_disable_record == true).ToList().AsQueryable().AsNoTracking()
                                                                                             select new ViewModelDisabledHallCalenderTimeSlot
                                                                                     {
                                                                                         id = t.id,
                                                                                         hall_id = t.hall_id,
                                                                                         slot_amount = t.slot_amount.ToString(),
                                                                                         time_slot = t.special_time_slot,
                                                                                         weekday_id = t.weekday_id,
                                                                                         date = t.special_date.ToString("MM/dd/yyyy"),
                                                                                         disable_time_slot_id = t.disable_time_slot_id
                                                                                        

                                                                                     };
            ViewBag.AllSpecialDisabledTimeSlots = AllSpecialDisabledTimeSlots.AsQueryable().AsNoTracking();
            var Alltimeslots = Alltimeslotss.AsQueryable().AsNoTracking();

            var Allbookings = from bookings in DB.bookings.AsQueryable().AsNoTracking().Where(r => r.booking_date >= today && r.booking_date <= today7)
                                               select bookings;
            var AlldisabledDates = from DD in DB.calendars.AsQueryable().AsNoTracking()
                                                     select DD;

            var AllWeekDays = from wd in DB.weekdays.AsQueryable().AsNoTracking()
                                               select wd;
            ViewBag.AllHalls = AllHalls;
            ViewBag.Alltimeslots = Alltimeslots;
            ViewBag.AllSpecialTimeSlots = AllSpecialTimeSlots;
            
            ViewBag.Allbookings = Allbookings;
            ViewBag.AlldisabledDates = AlldisabledDates;
            ViewBag.AllHallsImage = AllHallsImage;
            ViewBag.AllWeekDays = AllWeekDays;
            ViewBag.AllDates = dates;

            if ((service_area_ids != null) || type_names != null || (hall_ids != null) || !string.IsNullOrEmpty(zipcode))
            {
                ViewBag.Halls = selectedHalls;
            }
            else
            {
                ViewBag.Halls = Halls;
            }

            ViewBag.Neighborhoods = ServiceAreas;
            ViewBag.SimchaTypes = Eventypes;

          

            return View();


        }
        [HttpGet]
        public ActionResult CreateBooking(int hall_id , DateTime event_date, string returnUrl) {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.errorOccured = false;
            int weekdayid = 0;
            string day = event_date.ToString("dddd");
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





            hall hall = DB.halls.Find(hall_id);
            IEnumerable<user> Users = DB.users.ToList();
            hall_images hallimage = DB.hall_images.FirstOrDefault(i => i.hall_id == hall.id);
            ViewBag.Image = hallimage;
            IEnumerable<time_slots> Alltime_slots = DB.time_slots.Where(r => r.hall_id == hall_id && r.weekday_id == weekdayid).ToList();
            IEnumerable<time_slots> DisabledTime_Slots = from d in Alltime_slots
                                                  join t in DB.special_time_slots.Where(r=>r.special_date==event_date).ToList() on d.id equals t.disable_time_slot_id
                                                  select d;

            IEnumerable<time_slots> EnabledTimeSlots = Alltime_slots.Except(DisabledTime_Slots);
            IEnumerable<ViewModelHallCalenderTimeSlot> vEnabledTimeSlots = from e in EnabledTimeSlots
                                                                           select new ViewModelHallCalenderTimeSlot
                                                                           {
                                                                               id = e.id,
                                                                               time_slot = e.time_slot ,
                                                                               is_special_slot = false,
                                                                               slot_amount=e.slot_amount,
                                                                               slot_price = e.slot_price

                                                                           };
            IEnumerable<ViewModelHallCalenderTimeSlot> SEnabledTimeSlots = from d in DB.special_time_slots.Where(r => r.is_disable_record == false && r.is_special_slot == true && r.hall_id == hall_id && r.special_date==event_date).ToList()
                                                                          
                                                                           select new ViewModelHallCalenderTimeSlot
                                                                           {
                                                                               id = d.id,
                                                                               time_slot = d.special_time_slot,
                                                                               is_special_slot = true  ,
                                                                               slot_amount = d.slot_amount.ToString(),
                                                                               slot_price = d.slot_price
                                                                           };

            IEnumerable<ViewModelHallCalenderTimeSlot> union = vEnabledTimeSlots.Union(SEnabledTimeSlots);
            ViewBag.Time_Slots = union;
            ViewBag.Event_Date = event_date;
            ViewBag.Hall = hall;
            ViewBag.Users = Users;
            
             booking book = new booking();
             return View(book);
        }


        [HttpPost]
        public ActionResult CreateBooking(booking book, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            bool errorOccured = false;
            StringBuilder error = new StringBuilder();
            
            if (string.IsNullOrEmpty(book.address1))
            {
                errorOccured = true;
                error.Append("Please enter address1 <br/>");
            }

            if (string.IsNullOrEmpty(book.address2))
            {
                errorOccured = true;

                error.Append("Please enter address2 <br/>");
            }

            if (string.IsNullOrEmpty(book.card_expiration))
            {
                errorOccured = true;

                error.Append("Please enter Card Expiration <br/>");
            }

            if (string.IsNullOrEmpty(book.card_number))
            {
                errorOccured = true;

                error.Append("Please enter Card Number <br/>");
            }

            if (string.IsNullOrEmpty(book.card_type))
            {
                errorOccured = true;

                error.Append("Please enter Card Type <br/>");
            }

            if (string.IsNullOrEmpty(book.customer_notes))
            {
                errorOccured = true;

                error.Append("Please enter Order Notes <br/>");
            }
            if (string.IsNullOrEmpty(book.cvv))
            {
                errorOccured = true;

                error.Append("Please enter cvv <br/>");
            }
            if (string.IsNullOrEmpty(book.email))
            {
                errorOccured = true;

                error.Append("Please enter email <br/>");
            }
          

            if (string.IsNullOrEmpty(book.first_name))
            {
                errorOccured = true;

                error.Append("Please enter first_name <br/>");
            }

            if ( book.hall_id == null)
            {
                errorOccured = true;

                error.Append("Please enter hall_id <br/>");
            }

           
            if (string.IsNullOrEmpty(book.phone))
            {
                errorOccured = true;

                error.Append("Please enter phone <br/>");
            }
        
            if (string.IsNullOrEmpty(book.time_slot))
            {
                errorOccured = true;

                error.Append("Please enter Time Slot <br/>");
            }
    

            if (string.IsNullOrEmpty(book.zip_code))
            {
                errorOccured = true;

                error.Append("Please Enter Zip Code <br/>");
            }

            if (string.IsNullOrEmpty(book.city))
            {
                errorOccured = true;

                error.Append("Please enter a city <br/>");
            }
            ViewBag.Error = error.ToString();



            int weekdayid = 0;
            string day = book.booking_date.ToString("dddd");
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



            int? hall_id = book.hall_id;
            DateTime event_date = book.booking_date;
            hall hall = DB.halls.Find(hall_id);
            IEnumerable<user> Users = DB.users.ToList();
            hall_images hallimage = DB.hall_images.FirstOrDefault(i => i.hall_id == hall.id);
            ViewBag.Image = hallimage;
            IEnumerable<time_slots> Alltime_slots = DB.time_slots.Where(r => r.hall_id == hall_id && r.weekday_id == weekdayid).ToList();
            IEnumerable<time_slots> DisabledTime_Slots = from d in Alltime_slots
                                                         join t in DB.special_time_slots.Where(r => r.special_date == event_date).ToList() on d.id equals t.disable_time_slot_id
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
            IEnumerable<ViewModelHallCalenderTimeSlot> SEnabledTimeSlots = from d in DB.special_time_slots.Where(r => r.is_disable_record == false && r.is_special_slot == true && r.hall_id == hall_id && r.special_date == event_date).ToList()

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
            ViewBag.Event_Date = event_date;
            ViewBag.Hall = hall;
            ViewBag.Users = Users;
          







            if (errorOccured) {
                ViewBag.errorOccured = errorOccured;
                return View(book);
            }


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

            book.total= Convert.ToDouble(price);
            book.time_slot_id = slot_id;
           
            book.is_special_slot = isSpecialSlot;
            if (ModelState.IsValid)
            {
                DB.bookings.Add(book);
                DB.SaveChanges();
                      if (isSpecialSlot)
            {
                DisableTimeSlot(slot_id, hall_id.Value, book.booking_date, isSpecialSlot);
            }
            else
            {
                DisableTimeSlot(slot_id, hall_id.Value, book.booking_date, isSpecialSlot);
            }
                      if (returnUrl != null || returnUrl !="")
                      {
                          return Redirect(returnUrl);
                      }
                return RedirectToAction("Index");
            }

           
          
           
            return View(book);
        }

        [HttpGet]
        public ActionResult BookingListByCustomer(int user_id, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.UserName = DB.users.FirstOrDefault(r => r.id == user_id).user_name;
            var BookingsByUserId = DB.bookings.Where(r => r.user_id == user_id);

            ViewBag.BookingsByUserId = BookingsByUserId;
            return View(BookingsByUserId);


        }









        public FileResult PrintBooking(int id)
        {
            booking book = DB.bookings.FirstOrDefault(r => r.id == id);
            hall hall = DB.halls.Find(book.hall_id);
            user user = (DB.users.Any(r=>r.id==book.user_id))? DB.users.Find(book.user_id): new user {user_name = "Not Set"} ;

            string html = System.IO.File.ReadAllText(HttpContext.Server.MapPath(@"~/content/invoice-template/template.html"));


            System.Text.StringBuilder itemHtml = new System.Text.StringBuilder();


            html = (!String.IsNullOrEmpty(book.address1)) ? html.Replace("@address1@", book.address1) : html.Replace("@address1@","Missing");
            html = (!String.IsNullOrEmpty(book.address2)) ? html.Replace("@address2@", book.address2) : html.Replace("@address2@", "Missing");
            html = (!String.IsNullOrEmpty(book.booking_date.ToString("dddd MM yyyy"))) ? html.Replace("@booking_date@", book.booking_date.ToString("dddd MM yyyy")) : html.Replace("@booking_date@", "Missing");
            html = (!String.IsNullOrEmpty(book.booking_status)) ? html.Replace("@booking_status@", book.booking_status) : html.Replace("@booking_status@", "Missing");
            html = (!String.IsNullOrEmpty(book.card_expiration)) ? html.Replace("@card_expiration@", book.card_expiration) : html.Replace("@card_expiration@", "Missing");
            html = (!String.IsNullOrEmpty(book.card_number)) ? html.Replace("@card_number@", book.card_number) : html.Replace("@card_number@", "Missing");
            html = (!String.IsNullOrEmpty(book.card_type)) ? html.Replace("@card_type@", book.card_type) : html.Replace("@card_type@", "Missing");
            html = (!String.IsNullOrEmpty(book.city)) ? html.Replace("@city@", book.city) : html.Replace("@city@", "Missing");
            html = (book.create_date.HasValue) ? html.Replace("@create_date@", book.create_date.Value.ToString("dddd MM yyyy")) : html.Replace("@create_date@", "Missing");
            html = (!String.IsNullOrEmpty(book.customer_notes)) ? html.Replace("@customer_notes@", book.customer_notes) : html.Replace("@customer_notes@", "Missing");
            html = (!String.IsNullOrEmpty(book.cvv)) ? html.Replace("@cvv@", book.cvv) : html.Replace("@cvv@", "Missing");
            html = (!String.IsNullOrEmpty(book.email)) ? html.Replace("@email@", book.email) : html.Replace("@email@", "Missing");

            html = (!String.IsNullOrEmpty(book.first_name)) ? html.Replace("@first_name@", book.first_name) : html.Replace("@first_name@", "Missing");
            html = (!String.IsNullOrEmpty(book.last_name)) ? html.Replace("@last_name@", book.last_name) : html.Replace("@last_name@", "Missing");
            html = (!String.IsNullOrEmpty(hall.hall_name)) ? html.Replace("@hall_name@", hall.hall_name) : html.Replace("@hall_name@", "Missing");
            html = (book.total.HasValue) ? html.Replace("@hall_pricing@", book.total.ToString()) : html.Replace("@hall_pricing@", "Missing");
            html = (!String.IsNullOrEmpty(book.payment_type)) ? html.Replace("@payment_type@", book.payment_type) : html.Replace("@payment_type@", "Missing");
        //    html (!String.IsNullOrEmpty(book.address1))? = html.Replace("@phone@", book.phone);
            html = (!String.IsNullOrEmpty(book.simcha_type)) ? html.Replace("@simcha_type@", book.simcha_type) : html.Replace("@simcha_type@", "Missing");
            html = (!String.IsNullOrEmpty(book.state)) ? html.Replace("@state@", book.state) : html.Replace("@state@", "Missing");
            html = (!String.IsNullOrEmpty(book.zip_code)) ? html.Replace("@zip_code@", book.zip_code) : html.Replace("@zip_code@", "Missing");
            html = (!String.IsNullOrEmpty(book.time_slot)) ? html.Replace("@time_slot@", book.time_slot) : html.Replace("@time_slot@", "Missing");
            html = (!String.IsNullOrEmpty(book.customer_notes)) ? html.Replace("@customer_notes@", book.customer_notes) : html.Replace("@customer_notes@", "Missing");

            html = (!String.IsNullOrEmpty(user.user_name)) ? html.Replace("@user_name@", user.user_name) : html.Replace("@user_name@", "Missing");
            html = (!String.IsNullOrEmpty(hall.capacity)) ? html.Replace("@capacity@", hall.capacity) : html.Replace("@capacity@", "Missing");
  
                string path = HttpContext.Server.MapPath(@"~/content/invoice-template/print.html");
                string pathw = HttpContext.Server.MapPath(@"~/content/invoice-template/invoice" + User.Identity.Name + ".pdf");
            System.IO.File.WriteAllText(path,html);
            EO.Pdf.HtmlToPdf.ConvertUrl(path, pathw);
            return File(pathw, "application/pdf");


        }
        public static void CreateReport(int id)
        {
          

        }


        private double distancezip(string zip1, string zip2)
        {

            double zip1long = Double.Parse(getLongLat(zip1)[0]);
            double zip1lat = Double.Parse(getLongLat(zip1)[1]);
            double zip2long = Double.Parse(getLongLat(zip2)[0]);
            double zip2lat = Double.Parse(getLongLat(zip2)[1]);
            double distanceMile = distance(zip1lat, zip1long, zip2lat, zip2long, 'M');
            return distanceMile;
        }

        private string[] getLongLat(string zip)
        {
            zip lonLat = DB.zips.FirstOrDefault(r => r.zip1 == zip);

            string[] lonLatStringArray = { lonLat.lon, lonLat.lat };
            return lonLatStringArray;
        }

        private double distance(double lat1, double lon1, double lat2, double lon2, char unit)
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = dist * 60 * 1.1515;
            if (unit == 'K')
            {
                dist = dist * 1.609344;
            }
            else if (unit == 'N')
            {
                dist = dist * 0.8684;
            }
            return (dist);
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts decimal degrees to radians             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        private double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts radians to decimal degrees             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        private double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }




        public ActionResult Edit(int id, string returnUrl)
        {
            booking bookSingle = DB.bookings.SingleOrDefault(r => r.id==id);
            user user = DB.users.SingleOrDefault(r => r.id == bookSingle.user_id);
            hall hall = DB.halls.SingleOrDefault(r => r.id == bookSingle.hall_id);

            var book =  new ViewModelBookingUserHall
                       {
                           user_id = bookSingle.user_id,
                           id = bookSingle.id,
                           first_name = bookSingle.first_name,
                           last_name = bookSingle.last_name,
                           address1 = bookSingle.address1,
                           address2 = bookSingle.address2,
                           city = bookSingle.city,
                           zipcode = bookSingle.zip_code,
                           phone = bookSingle.phone,
                           email = bookSingle.email,
                           user_name = (user != null) ? user.user_name : "Not Set",
                           time_slot = bookSingle.time_slot,
                           booking_date = bookSingle.booking_date.ToString("MM/dd/yyyy"),
                           booking_status = bookSingle.booking_status,
                           total = bookSingle.total,
                           create_date = bookSingle.create_date.Value.ToString("MM/dd/yyyy"),
                           customer_notes = bookSingle.customer_notes,
                           payment_type = bookSingle.payment_type,
                           card_type = bookSingle.card_type,
                           card_expiration = bookSingle.card_expiration,
                           cvv = bookSingle.cvv,
                           hall_name = (hall!=null)? hall.hall_name : "Not Set",
                           state = bookSingle.state,
                           card_number = bookSingle.card_number 
                          
                           
                       };
            ViewModelBookingUserHall book1 = book;
            ViewBag.BookInfo = book1;
            ViewBag.ReturnUrl = returnUrl;


            

            List<SelectListItem> BookingStatusList = new List<SelectListItem> {
                                                                 new SelectListItem { Text = "Hold", Value = "Hold"},
                                                                 new SelectListItem { Text = "Reserved", Value = "Reserved"},
                                                                 new SelectListItem { Text = "Booked (Paid)", Value = "Booked"},
                                                                 new SelectListItem { Text = "Canceled", Value = "Canceled"},
                                                                 new SelectListItem { Text = "Refunded", Value = "Refunded"}


                            };



            List<SelectListItem> Card_Type_List = new List<SelectListItem> {
                                                                 new SelectListItem { Text = "Mastercard", Value = "Mastercard"},
                                                                 new SelectListItem { Text = "Visa", Value = "Visa"},
                                                                 new SelectListItem { Text = "American Express", Value = "Amrican Express"}
                             };

            IEnumerable<SelectListItem> Card_Type_Select = from c in Card_Type_List
                                                       select new SelectListItem
                                                       {
                                                           Selected = (c.Value == book1.card_type) ? true : false,
                                                           Text = c.Text,
                                                           Value = c.Value
                                                       };

            ViewBag.Card_Type_Select = Card_Type_Select;

            IEnumerable<SelectListItem> StatusSelect = from b in BookingStatusList
                                                   select new SelectListItem
                                                   {
                                                       Selected = (b.Value==book1.booking_status) ? true : false,
                                                       Text = b.Text,
                                                       Value = b.Value
                                                   };

            ViewBag.StatusSelect = StatusSelect;



            if (book1 == null)
            {
                return HttpNotFound();
            }
            return View();
        }
                                                                                          

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ViewModelBookingUserHall book, string returnUrl)
        {
            

                booking updatedBooking = DB.bookings.SingleOrDefault(x => x.id == book.id);
              //  user updatedUser = DB.users.SingleOrDefault(x => x.id == book.user_id);
                updatedBooking.first_name = book.first_name;
                updatedBooking.last_name = book.last_name;
                updatedBooking.address1 = book.address1;
                updatedBooking.address2 = book.address2;
                updatedBooking.city = book.city;
      
                updatedBooking.zip_code = book.zipcode;
                updatedBooking.email = book.email;
                updatedBooking.booking_status = book.booking_status;
                updatedBooking.card_number = book.card_number;
                updatedBooking.card_type = book.card_type;
                updatedBooking.card_expiration = book.card_expiration;
                updatedBooking.cvv = book.cvv;
                updatedBooking.customer_notes = book.customer_notes;
                updatedBooking.state = book.state;


                if (ModelState.IsValid)
                {
                    DB.Entry(updatedBooking).State = EntityState.Modified;
                    DB.SaveChanges();
                }




                return RedirectToAction("Edit", new { id = book.id, returnUrl = returnUrl });
        }




        [HttpGet, ActionName("Cancel")]

        public ActionResult CancelConfirmed(int id)
        {
            booking book = DB.bookings.Find(id);

            //if (book.time_slot_id.HasValue && book.hall_id.HasValue && book.booking_date!=null && book.is_special_slot.HasValue)
            //{
            
            //DisableTimeSlot(book.time_slot_id.Value, book.hall_id.Value, book.booking_date, book.is_special_slot.Value);

            //}
            book.booking_status = "Canceled";
            if (ModelState.IsValid)
            {
                DB.Entry(book).State = EntityState.Modified;
                DB.SaveChanges();
            }
            return RedirectToAction("Index");
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



        protected override void Dispose(bool disposing)
        {
            DB.Dispose();
            base.Dispose(disposing);
        }



    }
}
