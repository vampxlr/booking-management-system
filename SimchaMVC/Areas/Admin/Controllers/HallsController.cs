using System;
using System.Collections.Generic;
using System.Linq;

using System.Web.Mvc;
using System.Data.Entity;
using System.IO;


using System.Net;
using EO.Pdf.Internal;
using SimchaMVC.Models;

using WebMatrix.WebData;

namespace SimchaMVC.Areas.Admin.Controllers
{
     [Authorize]
    public class HallsController : Controller
    {


        SimchaDB DB = new SimchaDB();

        public ActionResult Index()
        {

            IQueryable<hall> halls;

            if (User.IsInRole("HallAdmin"))
            {
                halls = DB.halls.AsNoTracking().AsQueryable().Where(r => r.admin_user_id == WebSecurity.CurrentUserId);
          
            }
            else
            {
                halls = from s in DB.halls.AsNoTracking().AsQueryable()
                            select s;
            }
          


           
           


            ViewBag.Halls = halls;
            return View(halls);


        }


        public ActionResult AddNewRecord()
        {
            var sa = DB.service_areas.AsNoTracking().AsQueryable();
            var et = DB.event_types.AsNoTracking().AsQueryable();

            var halladminlist = DB.admin_users.AsNoTracking().AsQueryable().Where(r => r.user_role == "HallAdmin");

            var halladminlistSelect = from h in halladminlist
                                                   select new SelectListItem
                                                   {
                                                       Selected = (WebSecurity.CurrentUserId==h.id),
                                                       Text = h.user_name ,
                                                       Value = h.id.ToString()
                                                   };

            ViewBag.halladminlist = halladminlistSelect;
            
            ViewBag.isPost = false;

            var etSelect = et.Select(e => new SelectListItem
            {
                Text = e.type_name,
                Value = e.id.ToString()
            });

            ViewBag.EventTypesSelectList = etSelect;
            var saSelect = sa.Select(s => new SelectListItem
            {
                Text = s.service_area,
                Value = s.id.ToString()
            });
            ViewBag.ServiceAreaSelectList = saSelect;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewRecord(hall hall, string[] service_area_id, string[] event_type_id)
        {
            hall.policies = WebUtility.HtmlDecode(hall.policies);
            hall.features = WebUtility.HtmlDecode(hall.features);
            ViewBag.isPost = true;
            var zip = hall.zip_code;
            var zip_exists = DB.zips.AsNoTracking().AsQueryable().Any(rec => rec.zip1.Equals(zip));

            if (!zip_exists)
            {
                var id = hall.id;

                var halladminlist = DB.admin_users.AsNoTracking().AsQueryable().Where(r => r.user_role == "HallAdmin");

                var halladminlistSelect = from h in halladminlist.AsNoTracking().AsQueryable()
                                          select new SelectListItem
                                          {
                                              Selected = (WebSecurity.CurrentUserId == h.id),
                                              Text = h.user_name,
                                              Value = h.id.ToString()
                                          };

                ViewBag.halladminlist = halladminlistSelect;

                var sa = DB.service_areas.AsNoTracking().AsQueryable();
                var et = DB.event_types.AsNoTracking().AsQueryable();

                var etSelect = from e in et.AsNoTracking().AsQueryable()
                                                       select new SelectListItem
                                                       {
                                                          
                                                           Text = e.type_name,
                                                           Value = e.id.ToString()
                                                       };
                ViewBag.EventTypesSelectList = etSelect;

                var saSelect = from s in sa.AsNoTracking().AsQueryable()
                                                       select new SelectListItem
                                                       {
                                                          
                                                           Text = s.service_area,
                                                           Value = s.id.ToString()
                                                       };


               
                ViewBag.ServiceAreaSelectList = saSelect;
                ViewBag.Error = "Zip Code does not exist";
                return View(hall);
            }
            
         
            if (ModelState.IsValid)
            {
                DB.halls.Add(hall);
                DB.SaveChanges();
              //  return RedirectToAction("Index");
            }

            foreach (var he in event_type_id.Select(item => new hall_event_types {hall_id = hall.id, type_id = Int32.Parse(item)}).Where(he => ModelState.IsValid))
            {
                DB.hall_event_types.Add(he);
                DB.SaveChanges();
            }


            foreach (var ha in service_area_id.Select(item => new hall_areas {hall_id = hall.id, area_id = Int32.Parse(item)}).Where(ha => ModelState.IsValid))
            {
                DB.hall_areas.Add(ha);
                DB.SaveChanges();
            }
            return RedirectToAction("Index");
            
        }



        public ActionResult Edit(int id)
        {
            ViewBag.isPost = false;

            var halladminlist = DB.admin_users.Where(r => r.user_role == "HallAdmin").ToList();

            IEnumerable<SelectListItem> halladminlistSelect = from h in halladminlist
                                                              select new SelectListItem
                                                              {
                                                                  Selected = (WebSecurity.CurrentUserId == h.id) ? true : false,
                                                                  Text = h.user_name,
                                                                  Value = h.id.ToString()
                                                              };

            ViewBag.halladminlist = halladminlistSelect;



        
            hall hall = DB.halls.Find(id);
            //IEnumerable<int?> hallareas= DB.hall_areas.Where(r=>r.hall_id==id).ToList().Select(r=>r.area_id);
            IList<hall_areas> HAreas = DB.hall_areas.Where(r => r.hall_id == id).ToList();

          IList<hall_event_types> HeventTypes = DB.hall_event_types.Where(r => r.hall_id == id).ToList();
          
            
            IEnumerable<service_areas> sa = DB.service_areas.ToList();
           IEnumerable<event_types> et = DB.event_types.ToList();

           IEnumerable<SelectListItem> etSelect = from e in et
                                                  select new SelectListItem
                                                  {
                                                      Selected = HeventTypes.Any(r=>r.type_id==e.id),
                                                      Text = e.type_name,
                                                      Value = e.id.ToString()
                                                  };
           ViewBag.EventTypesSelectList = etSelect;

            IEnumerable<SelectListItem> saSelect = from s in sa
                                            select new SelectListItem
                                            {
                                                Selected = (HAreas.Any(r => r.area_id == s.id)),

                                              Text=s.service_area,
                                              Value=s.id.ToString()
                                            };

            ViewBag.ServiceAreaSelectList = saSelect;
            if (hall == null)
            {
                return HttpNotFound();
            }

            hall.policies = WebUtility.HtmlDecode(hall.policies);
            hall.features = WebUtility.HtmlDecode(hall.features);


            return View(hall);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(hall hall, string[] service_area_id, string[] event_type_id)
        {
            hall.policies = WebUtility.HtmlDecode(hall.policies);
            hall.features = WebUtility.HtmlDecode(hall.features);

            ViewBag.isPost = true;
            string zip = hall.zip_code;
            bool zip_exists = DB.zips.Any(rec => rec.zip1.Equals(zip));

            if (!zip_exists)
            {
                int id = hall.id;

                var halladminlist = DB.admin_users.Where(r => r.user_role == "HallAdmin").ToList();

                IEnumerable<SelectListItem> halladminlistSelect = from h in halladminlist
                                                                  select new SelectListItem
                                                                  {
                                                                      Selected = (WebSecurity.CurrentUserId == h.id) ? true : false,
                                                                      Text = h.user_name,
                                                                      Value = h.id.ToString()
                                                                  };

                ViewBag.halladminlist = halladminlistSelect;

                IEnumerable<service_areas> sa = DB.service_areas.ToList();
                IEnumerable<event_types> et = DB.event_types.ToList();

                IEnumerable<SelectListItem> etSelect = from e in et
                                                       select new SelectListItem
                                                       {

                                                           Text = e.type_name,
                                                           Value = e.id.ToString()
                                                       };
                ViewBag.EventTypesSelectList = etSelect;

                IEnumerable<SelectListItem> saSelect = from s in sa
                                                       select new SelectListItem
                                                       {

                                                           Text = s.service_area,
                                                           Value = s.id.ToString()
                                                       };

                ViewBag.ServiceAreaSelectList = saSelect;
                ViewBag.Error = "Zip Code does not exist";
                return View(hall);
            }



            if (service_area_id == null)
            {

                int id = hall.id;

                var halladminlist = DB.admin_users.Where(r => r.user_role == "HallAdmin").ToList();

                IEnumerable<SelectListItem> halladminlistSelect = from h in halladminlist
                                                                  select new SelectListItem
                                                                  {
                                                                      Selected = (WebSecurity.CurrentUserId == h.id) ? true : false,
                                                                      Text = h.user_name,
                                                                      Value = h.id.ToString()
                                                                  };

                ViewBag.halladminlist = halladminlistSelect;

                IEnumerable<service_areas> sa = DB.service_areas.ToList();
                IEnumerable<event_types> et = DB.event_types.ToList();

                IEnumerable<SelectListItem> etSelect = from e in et
                                                       select new SelectListItem
                                                       {

                                                           Text = e.type_name,
                                                           Value = e.id.ToString()
                                                       };
                ViewBag.EventTypesSelectList = etSelect;

                IEnumerable<SelectListItem> saSelect = from s in sa
                                                       select new SelectListItem
                                                       {

                                                           Text = s.service_area,
                                                           Value = s.id.ToString()
                                                       };

                ViewBag.ServiceAreaSelectList = saSelect;



                ViewBag.Error = "Select atleast one service area";
                return View(hall);
             
            }


            if ((event_type_id==null))
            {
                int id = hall.id;

                var halladminlist = DB.admin_users.Where(r => r.user_role == "HallAdmin").ToList();

                IEnumerable<SelectListItem> halladminlistSelect = from h in halladminlist
                                                                  select new SelectListItem
                                                                  {
                                                                      Selected = (WebSecurity.CurrentUserId == h.id) ? true : false,
                                                                      Text = h.user_name,
                                                                      Value = h.id.ToString()
                                                                  };

                ViewBag.halladminlist = halladminlistSelect;

                IEnumerable<service_areas> sa = DB.service_areas.ToList();
                IEnumerable<event_types> et = DB.event_types.ToList();

                IEnumerable<SelectListItem> etSelect = from e in et
                                                       select new SelectListItem
                                                       {

                                                           Text = e.type_name,
                                                           Value = e.id.ToString()
                                                       };
                ViewBag.EventTypesSelectList = etSelect;

                IEnumerable<SelectListItem> saSelect = from s in sa
                                                       select new SelectListItem
                                                       {

                                                           Text = s.service_area,
                                                           Value = s.id.ToString()
                                                       };

                ViewBag.ServiceAreaSelectList = saSelect;

                ViewBag.Error = "Select atleast one event type";
                return View(hall);

            }

            if (ModelState.IsValid)
            {
                DB.Entry(hall).State = EntityState.Modified;
                DB.SaveChanges();
               
            }

             IList<hall_event_types> heList = DB.hall_event_types.Where(r=>r.hall_id==hall.id).ToList();
                foreach(var h in heList){
                    DB.hall_event_types.Remove(h);
                    DB.SaveChanges();
                }

                IList<hall_areas> haList = DB.hall_areas.Where(r => r.hall_id == hall.id).ToList();
                foreach (var h in haList)
                {
                    DB.hall_areas.Remove(h);
                    DB.SaveChanges();
                }

                foreach (var item in event_type_id)
                {
                    hall_event_types he = new hall_event_types();
                    he.hall_id = hall.id;

                    he.type_id = Int32.Parse(item);

                    if (ModelState.IsValid)
                    {
                        DB.hall_event_types.Add(he);
                        DB.SaveChanges();

                    }


                }

                foreach (var item in service_area_id)
                {
                    hall_areas ha = new hall_areas();
                    ha.hall_id = hall.id;

                    ha.area_id = Int32.Parse(item);

                    if (ModelState.IsValid)
                    {
                        DB.hall_areas.Add(ha);
                        DB.SaveChanges();

                    }


                }


     




                return RedirectToAction("Edit", new {id =hall.id });
        }




        //[HttpGet, ActionName("Delete")]

        //public ActionResult DeleteConfirmed(int id)
        //{
        //    hall hall = DB.halls.Find(id);
        //    DB.halls.Remove(hall);
        //    DB.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        [HttpGet]
       public ActionResult TimeSlots(int id , string returnUrl)
        {
           IEnumerable<time_slots> timeslots = DB.time_slots.Where(t=>t.hall_id==id);
           if (timeslots == null)
            {
                return HttpNotFound();
            }

          IEnumerable<weekday> weekdays = DB.weekdays.ToList();
          ViewBag.Weekdays = weekdays;
          ViewBag.ReturnUrl = returnUrl;
          return View(timeslots);
        }

         [HttpPost]
       public ActionResult AddTimeSlots(time_slots timeslot, string hall_id,string weekday_id)
       {
           int hallid = Convert.ToInt32(hall_id); 
           timeslot.hall_id = Convert.ToInt32(hall_id);
           timeslot.weekday_id = Convert.ToInt32(weekday_id);
           time_slots newtimeslot = new time_slots();

           newtimeslot.slot_price = timeslot.slot_price;
           newtimeslot.slot_amount = timeslot.slot_amount;
           newtimeslot.slot_number = timeslot.slot_number;
           newtimeslot.time_slot = timeslot.time_slot;
           newtimeslot.weekday_id = timeslot.weekday_id;
           newtimeslot.hall_id = timeslot.hall_id;

           if (ModelState.IsValid)
           {
               DB.time_slots.Add(newtimeslot);
               DB.SaveChanges();
               return RedirectToAction("TimeSlots", new { id = hallid });
           }


           return RedirectToAction("TimeSlots", new { id = hallid });
       }


         public ActionResult EditTimeSlots(time_slots timeslot, string hall_id, string weekday_id,string slot_id)
         {
             int slot_idint = Convert.ToInt32(slot_id);
             time_slots DbTimeslot = DB.time_slots.Find(slot_idint);
             int hallid = Convert.ToInt32(hall_id); 
             timeslot.hall_id = Convert.ToInt32(hall_id);
             timeslot.weekday_id = Convert.ToInt32(weekday_id);
             DbTimeslot.hall_id = timeslot.hall_id;
             DbTimeslot.slot_price = timeslot.slot_price;
             DbTimeslot.slot_amount = timeslot.slot_amount;
             DbTimeslot.time_slot = timeslot.time_slot;
             DbTimeslot.weekday_id = timeslot.weekday_id;

             if (ModelState.IsValid)
             {
                 DB.Entry(DbTimeslot).State = EntityState.Modified;
                 DB.SaveChanges();
                 return RedirectToAction("TimeSlots", new { id = hallid });
             }




             return RedirectToAction("TimeSlots", Convert.ToInt32(hallid));
         }

         //[HttpGet, ActionName("DeleteTimeSlot")]

         //public ActionResult DeleteTimeSlot(int id)
         //{
         //    time_slots DbTimeslot = DB.time_slots.Find(id);
         //    DB.time_slots.Remove(DbTimeslot);
         //    DB.SaveChanges();
         //    return RedirectToAction("TimeSlots", new { id = DbTimeslot.hall_id });
         //}



        public ActionResult Calendar(int id)
        {
            hall hall = DB.halls.Find(id);
            if (hall == null)
            {
                return HttpNotFound();
            }
            return View(hall);
           
        }

      
         [HttpGet]
         public ActionResult Caterers(int id)
        {
            IEnumerable<caterer> caterers = DB.caterers.ToList();
            IEnumerable<hall_caterers> AllhallCaterers = DB.hall_caterers.Where(h => h.hall_id == id);

            IEnumerable<caterer> hallCaterers =  from xx in caterers
                                                 join halC in AllhallCaterers on xx.id equals halC.caterer_id 
                                                 select xx;
            IEnumerable<caterer> NotHallCaterers = caterers.Except(hallCaterers);
            ViewBag.NotHallCaterers = NotHallCaterers;
            return View(hallCaterers);
        }

         [HttpPost]

         public ActionResult Caterers(string caterer_id, string hall_id)
         {

             hall_caterers newHallCaterer = new hall_caterers();
             newHallCaterer.caterer_id = Int32.Parse(caterer_id);
             newHallCaterer.hall_id = Int32.Parse(hall_id);

             if (ModelState.IsValid)
             {
                 DB.hall_caterers.Add(newHallCaterer);
                 DB.SaveChanges();
       
             }
             
             
             
             int id = Int32.Parse(hall_id);
             IEnumerable<caterer> caterers = DB.caterers.ToList();
             IEnumerable<hall_caterers> AllhallCaterers = DB.hall_caterers.Where(h => h.hall_id == id);

             IEnumerable<caterer> hallCaterers = from xx in caterers
                                                 join halC in AllhallCaterers on xx.id equals halC.caterer_id
                                                 select xx;
             IEnumerable<caterer> NotHallCaterers = caterers.Except(hallCaterers);
             ViewBag.NotHallCaterers = NotHallCaterers;

             return View(hallCaterers);
         }
               
        // public ActionResult deleteCaterer(int id,string hall_id)
        //{
        //    int catererId = id;
        //    int hallid = Convert.ToInt32(hall_id);
        //    hall_caterers hall_caterers = DB.hall_caterers.FirstOrDefault(r=>r.hall_id==hallid && r.caterer_id==catererId);

        //    DB.hall_caterers.Remove(hall_caterers);
        //    DB.SaveChanges();
        //    return RedirectToAction("Caterers", new { id = hallid });
        //}
       
  

         [HttpGet]
         public ActionResult Directions(int id)
        {
            hall hall = DB.halls.Find(id);
            return View(hall);
        }
          [HttpPost]
         public ActionResult Directions(hall hall)
         {
             hall updatedHall = DB.halls.Find(hall.id);

             updatedHall.directions = WebUtility.HtmlDecode(hall.directions);
             hall = updatedHall;
             if (ModelState.IsValid)
             {
                 DB.Entry(hall).State = EntityState.Modified;
                 DB.SaveChanges();
             
             }


             return RedirectToAction("Directions",hall.id);
         }









        [HttpGet]
         public ActionResult Images(int id)
         {
             IEnumerable<hall_images> HI = DB.hall_images.Where(x=>x.hall_id==id).OrderBy(r=>r.order_by).ToList();

             return View(HI);
         }

       [HttpPost]
        public ActionResult Images(int hall_images_id,int hall_id, string image_caption,string image_name, int order_by)
         {

             hall_images HI = new hall_images();
             HI.id = hall_images_id;
             HI.hall_id = hall_id;
             HI.image_caption = image_caption;
             HI.image_name = image_name;
             HI.order_by = order_by;
       
             if (ModelState.IsValid)
             {
                 DB.Entry(HI).State = EntityState.Modified;
                 DB.SaveChanges();
                 return RedirectToAction("Images", new { id = hall_id });
             }
             return View(HI);
         }

       //public ActionResult ImagesDelete(int id, string hall_id)
       //  {
       //      int hallid = Convert.ToInt32(hall_id); 
       //      hall_images HI = DB.hall_images.Find(id);
       //      DB.hall_images.Remove(HI);
       //      DB.SaveChanges();
       //      return RedirectToAction("Images", new { id = hallid });
       //  }



         [HttpPost]
         [ValidateAntiForgeryToken]
         public ActionResult ImagesAddNewRecord(hall_images HI, string hall_id)
         {
            
             hall_images HX = new hall_images();

             int hallid = Convert.ToInt32(hall_id); 
             var file = Request.Files[0];

             var fileNameForDB = Path.GetFileName(file.FileName);
             var extensionForDB = Path.GetExtension(fileNameForDB);

             if (file != null && file.ContentLength > 0)
             {

                 var fileName = Path.GetFileName(file.FileName);
                 var extension = Path.GetExtension(fileName);

                 var path = Path.Combine(Server.MapPath("~/Content/thumbs"), fileName);
                 var pathwrite = Path.Combine(Server.MapPath("~/Content/thumbs"), fileName);
                 file.SaveAs(path);


             }

             HI.image_name = fileNameForDB;

             if (ModelState.IsValid)
             {
                 DB.hall_images.Add(HI);
                 DB.SaveChanges();
                 return RedirectToAction("Images", new { id = hall_id });
             }

             return RedirectToAction("Images", new { id = hall_id });
         }
        
         [HttpGet]
         public ActionResult SpecialTimeSlots(int hall_id, DateTime date)
         {
             int weekdayid = 0;
             string day =date.ToString("dddd");
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



             IEnumerable<time_slots> timeslots = DB.time_slots.Where(r => r.hall_id == hall_id && r.weekday_id == weekdayid);
             IEnumerable<special_time_slots> special_timeslotss = DB.special_time_slots.Where(r => r.hall_id == hall_id && r.special_date == date);
             hall Hall = DB.halls.Find(hall_id);
             ViewBag.Hall = Hall;
             ViewBag.date = date;
             ViewBag.hall_id = hall_id;
             ViewBag.timeslots = timeslots;
             ViewBag.special_timeslots = special_timeslotss;
             ViewBag.weekdayid = weekdayid;
        
             return View();
         
         }
         [HttpPost]
         public ActionResult SpecialTimeSlots(special_time_slots slot)
         {
           if (ModelState.IsValid)
            {
                DB.special_time_slots.Add(slot);
                DB.SaveChanges();
                return RedirectToAction("SpecialTimeSlots", new { hall_id=slot.hall_id, date= slot.special_date });
            }
              return View(slot);

         }
             public ActionResult DisableEnableTimeSlot(int time_slot_id, int hall_id, DateTime date , bool special_slot)
         {

                   int weekdayid = 0;
             string day =date.ToString("dddd");
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

             if (special_slot == false) {

                 IEnumerable<special_time_slots> special = DB.special_time_slots.Where(r => r.special_date == date && r.disable_time_slot_id== time_slot_id && r.is_disable_record==true && r.hall_id==hall_id && r.is_special_slot==false).ToList();
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
                         return RedirectToAction("SpecialTimeSlots", new { hall_id = hall_id, date = date });
                     }

                 }
                 else
                 {

                     foreach (var item in special)
                     {

                         DB.special_time_slots.Remove(item);
                         DB.SaveChanges();

                     }
                     return RedirectToAction("SpecialTimeSlots", new { hall_id = hall_id, date = date });
                 }

             }
             else if (special_slot == true) {

                special_time_slots special2 = DB.special_time_slots.FirstOrDefault(r => r.id == time_slot_id &&r.special_date == date && r.is_special_slot==true);



                if (special2.is_disable_record == false)
                         {

                             special2.is_disable_record = true;

                             if (ModelState.IsValid)
                             {
                                 DB.Entry(special2).State = EntityState.Modified;
                                 DB.SaveChanges();
                                 return RedirectToAction("SpecialTimeSlots", new { hall_id = hall_id, date = date });
                             }




                         }
                else if (special2.is_disable_record == true)
                         {

                             special2.is_disable_record = false;

                             if (ModelState.IsValid)
                             {
                                 DB.Entry(special2).State = EntityState.Modified;
                                 DB.SaveChanges();
                                 return RedirectToAction("SpecialTimeSlots", new { hall_id = hall_id, date = date });
                             }

                         
                         }

                     

                 
                     return RedirectToAction("SpecialTimeSlots", new { hall_id = hall_id, date = date });
                 
             
             }
             
             return View();

         }

        

        protected override void Dispose(bool disposing)
        {
            DB.Dispose();
            base.Dispose(disposing);
        }

    }


}
