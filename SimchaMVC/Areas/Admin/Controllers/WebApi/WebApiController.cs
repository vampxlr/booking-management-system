using System.Web.Mvc;
using System.Web.WebPages;
using EO.Pdf.Internal;
using SimchaMVC.Models;
using SimchaMVC.Models.ViewModelsWebApi;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web.Http;
using WebMatrix.WebData;

namespace SimchaMVC.Areas.Admin.Controllers
{
    public class WebApiController : ApiController
    {

        private SimchaDB DB = new SimchaDB();

        [System.Web.Http.HttpGet]
        public IQueryable<BookingDate> GetBooking()
        {
            IQueryable<booking> bookingAll;

            if (User.IsInRole("HallAdmin"))
            {
                var halls =
                    DB.halls.AsQueryable().AsNoTracking().Where(r => r.admin_user_id == WebSecurity.CurrentUserId);
                bookingAll = (from b in DB.bookings
                    join sa in halls on b.hall_id.Value equals sa.id
                              select b).ToList().AsQueryable().AsNoTracking();
            }
            else
            {
                try
                {
                    bookingAll = DB.bookings.ToList().AsQueryable().AsNoTracking();
                }
                catch (Exception e)
                {
                    var exp = e;
                    throw;
                }


            }



            var bookingDate = from U in bookingAll.AsNoTracking().AsQueryable()
                select new BookingDate {booking_date = U.booking_date.Date.ToString("M/d/yyyy")};
            return bookingDate;
        }

        [System.Web.Http.HttpPost]
        public IQueryable<ViewModelBookingShortDetails> GetBookingRecord(BookingDate date)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime DateString = DateTime.ParseExact(date.booking_date, "M/d/yyyy", provider);
            var bookingAll = DB.bookings.AsQueryable().AsNoTracking().Where(t => t.booking_date == DateString);






            if (User.IsInRole("HallAdmin"))
            {
                var halls =
                    DB.halls.AsQueryable().AsNoTracking().Where(r => r.admin_user_id == WebSecurity.CurrentUserId);
                bookingAll = from b in bookingAll
                    join sa in halls on b.hall_id equals sa.id
                    select b;




            }




            var AllHall = DB.halls.AsQueryable().AsNoTracking();
            var AllUser = DB.users.AsQueryable().AsNoTracking();

            IQueryable<ViewModelBookingShortDetails> bookingShortDetails =
                from b in bookingAll.ToList().AsQueryable().AsNoTracking()
                select new ViewModelBookingShortDetails
                {
                    booking_id = b.id,
                    hall_name =
                        (AllHall.Any(h => h.id == b.hall_id))
                            ? AllHall.FirstOrDefault(h => h.id == b.hall_id).hall_name
                            : "Hall Deleted",
                    user_infromation =
                        (AllUser.Any(h => h.id == b.user_id))
                            ? AllUser.FirstOrDefault(h => h.id == b.user_id).user_name
                            : "User Removed",
                    fullbooking_url = b.id.ToString(),
                    booking_date = b.booking_date.ToString("M/d/yyyy")
                };


            return bookingShortDetails;


        }

        public IEnumerable<DisabledDate> GetDisabledHallDates(int id)
        {

            IEnumerable<calendar> CdisabledDates = DB.calendars.Where(x => x.not_active == 1 && x.hall_id == id);


            CdisabledDates = DB.calendars.Where(x => x.not_active == 1 && x.hall_id == id);
            IEnumerable<DisabledDate> disabledDates = from c in CdisabledDates
                select
                    new DisabledDate
                    {
                        disabled_date = c.calendar_date.HasValue ? c.calendar_date.Value.ToString("MM/dd/yyyy") : null
                    };

            return disabledDates;


        }

        [System.Web.Http.HttpPost]
        public string DeletBookingRecord(ViewModelDeleteOrEnableDate date)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime DateString = DateTime.ParseExact(date.booking_date, "M/d/yyyy", provider);

            string Operation = "No Operation performed";

            try
            {
                calendar CdisabledDates =
                    DB.calendars.FirstOrDefault(x => x.hall_id == date.hall_id && x.calendar_date == DateString);

                if (CdisabledDates != null && CdisabledDates.not_active == 1)
                {
                    CdisabledDates.not_active = 0;
                    if (ModelState.IsValid)
                    {
                        DB.Entry(CdisabledDates).State = EntityState.Modified;
                        DB.SaveChanges();
                        Operation = "Date Disabled";
                    }
                }

                else if (CdisabledDates != null && CdisabledDates.not_active == 0)
                {
                    CdisabledDates.not_active = 1;
                    if (ModelState.IsValid)
                    {
                        DB.Entry(CdisabledDates).State = EntityState.Modified;
                        DB.SaveChanges();
                        Operation = "Date Enabled";
                    }
                }

                else if (CdisabledDates == null)
                {
                    if (ModelState.IsValid)
                    {
                        calendar calender = new calendar
                        {
                            calendar_date = DateString,
                            hall_id = date.hall_id,
                            not_active = 1
                        };

                        DB.calendars.Add(calender);
                        DB.SaveChanges();
                        Operation = "Date Disabled with new entry";
                    }

                }


            }
            catch (Exception)
            {
                calendar calender = new calendar {calendar_date = DateString, hall_id = date.hall_id, not_active = 1};
                if (ModelState.IsValid)
                {
                    DB.calendars.Add(calender);
                    DB.SaveChanges();
                    Operation = "Date Disabled with new entry";
                }
            }


            return Operation;


        }


        public user GetUserInfo(int id)
        {

            user user = DB.users.Find(id);
            return user;

        }

        public List<object> GetDisabledAndBookingDates(int id)
        {
            IEnumerable<calendar> CdisabledDates = DB.calendars.Where(x => x.not_active == 1 && x.hall_id == id);
            IEnumerable<DisabledDate> disabledDates = from c in CdisabledDates
                select
                    new DisabledDate
                    {
                        disabled_date = c.calendar_date.HasValue ? c.calendar_date.Value.ToString("M/d/yyyy") : null
                    };

            IEnumerable<booking> bookingAll = DB.bookings.Where(r => r.hall_id == id).ToList();
            IEnumerable<BookingDate> bookingDate = from U in bookingAll
                select new BookingDate {booking_date = U.booking_date.ToString("M/d/yyyy")};

            List<object> BookingAndDisabledDates = new List<object> {bookingDate, disabledDates};

            return BookingAndDisabledDates;
        }

        [System.Web.Http.HttpPost]
        public IEnumerable<ViewModelBookingShortDetails> GetBookingRecordWithHallIdAndDate(ViewModelIsDisabled date)
        {

            IEnumerable<booking> bookingAll =
                DB.bookings.Where(t => t.booking_date == date.date && t.hall_id == date.hall_id);
            IEnumerable<hall> AllHall = DB.halls.ToList();
            IEnumerable<user> AllUser = DB.users.ToList();
            IEnumerable<ViewModelBookingShortDetails> nullBook = null;
            IEnumerable<ViewModelBookingShortDetails> bookingShortDetails;
            bookingShortDetails = from b in bookingAll
                select new ViewModelBookingShortDetails
                {
                    booking_id = b.id,
                    hall_name =
                        (AllHall.Any((h => h.id == b.hall_id)))
                            ? AllHall.FirstOrDefault(h => h.id == b.hall_id).hall_name
                            : "Hall Deleted",
                    user_infromation =
                        ((AllUser.Any(h => h.id == b.user_id))
                            ? AllUser.FirstOrDefault(h => h.id == b.user_id).user_name
                            : "User Deleted"),
                    fullbooking_url = b.id.ToString(),
                    booking_date = b.booking_date.ToString("M/d/yyyy"),
                    time_slot = b.time_slot
                };


            return bookingShortDetails;


        }

        [System.Web.Http.HttpPost]
        public bool IsDisabled(ViewModelIsDisabled DateId)
        {
            bool CdisabledDates =
                DB.calendars.AsQueryable()
                    .AsNoTracking()
                    .Any(x => x.not_active == 1 && x.hall_id == DateId.hall_id && x.calendar_date == DateId.date);
            return !CdisabledDates;
        }

        public bool IsBooked(ViewModelIsDisabled DateId)
        {
            bool CBookedDate = DB.bookings.AsQueryable().AsNoTracking().Any(r => r.booking_date == DateId.date.Date && r.hall_id.Value == DateId.hall_id);
            return CBookedDate;
        }

        [System.Web.Http.HttpPost]

        public IEnumerable<ViewModelHallCalenderTimeSlot> GetTimeSlotsForHallCalender(ViewModelIsDisabled DateId)
        {
            var weekdayid = 0;
            var day = DateId.date.ToString("dddd");
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
                DB.time_slots.Where(x => x.hall_id == DateId.hall_id && x.weekday_id == weekdayid).ToList();

            IEnumerable<special_time_slots> disabledSlotsId =
                DB.special_time_slots.Where(
                    r => r.is_disable_record == true && r.is_special_slot == false && r.special_date == DateId.date)
                    .ToList();
            IEnumerable<special_time_slots> specialdisabledSlotsId =
                DB.special_time_slots.Where(
                    r => r.is_disable_record == true && r.is_special_slot == true && r.special_date == DateId.date)
                    .ToList();


            IEnumerable<ViewModelHallCalenderTimeSlot> timeSlots = from t in TimeSlots
                join disabled in disabledSlotsId on t.id equals disabled.disable_time_slot_id into gj
                from subdisabled in gj.DefaultIfEmpty()
                select new ViewModelHallCalenderTimeSlot
                {
                    id = t.id,
                    date = DateId.date.ToString("M/d/yyyy"),
                    hall_id = t.hall_id,
                    time_slot = t.time_slot,
                    weekday_id = weekdayid,
                    disabled = (subdisabled != null && subdisabled.is_disable_record)
                };

            IEnumerable<special_time_slots> Special_timeslots =
                DB.special_time_slots.Where(r => r.special_date == DateId.date && r.hall_id == DateId.hall_id).ToList();

            IEnumerable<ViewModelHallCalenderTimeSlot> vSpecial_timeslots = from s in Special_timeslots


                select new ViewModelHallCalenderTimeSlot
                {
                    id = 0,
                    date = DateId.date.ToString("M/d/yyyy"),
                    hall_id = s.hall_id,
                    disabled = s.is_disable_record,
                    time_slot =
                        ((s.special_time_slot != null) ? s.special_time_slot += "[Special Slot]" : s.special_time_slot)


                };

            IEnumerable<ViewModelHallCalenderTimeSlot> join = timeSlots.Union(vSpecial_timeslots);

            var filtered = join.Where(i => i.time_slot != null);
            return filtered;

        }

        [System.Web.Http.HttpGet]
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

         [System.Web.Http.HttpPost]
        public List<ViewModelFrontEndSearchResult> GetBookingsWithStartDateAndEndDate(ViewModel_GetBookingsWithStartDateAndEndDate Info)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime startDate = DateTime.ParseExact(Info.StartDate, "M/d/yyyy", provider);
            DateTime endDate = DateTime.ParseExact(Info.EndDate, "M/d/yyyy", provider);

             int numberOfDays = (endDate.Date - startDate.Date).Days;
             DateTime[] Dates = new DateTime[numberOfDays+1];
             int count = 0;
             for (DateTime i = startDate;i.Date<=endDate.Date;i=i.AddDays(1))
             {

                 Dates[count] = i;

                 count++;
               
             }







             IQueryable<hall> Halls;
            
             Halls = from s in DB.halls.AsNoTracking().AsQueryable()
                         select s;
          


             var ServiceAreas = DB.service_areas.AsNoTracking().AsQueryable();
             var Eventypes = DB.event_types.AsNoTracking().AsQueryable();



             IQueryable<hall> AllHalls;

             
                 AllHalls = from s in DB.halls.AsNoTracking().AsQueryable()
                            select s;
             
             IEnumerable<int?> hall_idss = Info.Hall_Ids;
             IEnumerable<int?> service_area_ids = Info.LocationIds;
             IEnumerable<int?> type_names = Info.SimchaTypeIds;

             IEnumerable<int?> hallidsforServiceArea = null;

             if (service_area_ids != null)
             {

                 hallidsforServiceArea = from xx in DB.hall_areas
                                         where service_area_ids.Contains(xx.area_id)
                                         select xx.hall_id;
             }


             IEnumerable<int?> hallidsfortype_names = null;

             if (type_names != null)
             {

                 hallidsfortype_names = from xx in DB.hall_event_types
                                        where type_names.Contains(xx.type_id)
                                        select xx.hall_id;

             }
          

             IQueryable<hall> selectedHalls = AllHalls;
             if (hall_idss != null && hall_idss.Count()!=0)
             {

                 selectedHalls = from xx in AllHalls.AsQueryable().AsNoTracking()
                                 where hall_idss.Contains(xx.id)

                                 select xx;
             }
             if (service_area_ids != null && service_area_ids.Count() != 0)
             {

                 selectedHalls = from xx in selectedHalls.AsQueryable().AsNoTracking()

                                 where hallidsforServiceArea.Contains(xx.id)

                                 select xx;
             }
             if (type_names != null && type_names.Count() !=0)
             {

                 selectedHalls = from xx in selectedHalls.AsQueryable().AsNoTracking()

                                 where hallidsfortype_names.Contains(xx.id)
                                 select xx;
             }

             var zipOut = Info.ZipCode;
             var zip_existsOut = DB.zips.AsQueryable().AsNoTracking().Any(rec => rec.zip1.Equals(zipOut));
             if (!zip_existsOut && !string.IsNullOrEmpty(zipOut))
             {
                 return null;
             }
             if (!string.IsNullOrEmpty(Info.ZipCode) && zip_existsOut)
             {
                 IList<hall> tempSelectedHalls = selectedHalls.ToList();

                 var zipSelectedHalls = (from hallItem in tempSelectedHalls let zip = hallItem.zip_code let zip_exists = DB.zips.AsQueryable().AsNoTracking().Any(rec => rec.zip1.Equals(zip)) where zip_exists let distanceDiffB2nzips = distancezip(Info.ZipCode, hallItem.zip_code) - ((Info.ZipAprox.HasValue) ? Info.ZipAprox.Value : 0) where distanceDiffB2nzips < 0 select hallItem).ToList();

                 selectedHalls = zipSelectedHalls.AsQueryable().AsNoTracking();
             }




             if ((service_area_ids != null) || type_names != null || (hall_idss != null) || !string.IsNullOrEmpty(Info.ZipCode))
             {
                 Halls = selectedHalls;
             }
         



























            

             int[] hall_ids = new int[Halls.Count()];
                 hall_ids = Halls.AsEnumerable().Select(r => r.id).ToArray();
           
               

        

         //    DateTime[,] Hall_Dates = new DateTime[hall_ids.Count(), numberOfDays+1];

             Dictionary<int, IEnumerable<DateTime>> Hall_Dates = new Dictionary<int, IEnumerable<DateTime>>();

             foreach (int hall_id in hall_ids)
             {

                 DateTime[] dHallDates = DB.calendars.Where(r=>r.not_active==1 && r.hall_id==hall_id).AsEnumerable().Select(r => (DateTime)r.calendar_date).ToArray();

                 DateTime[] aHallDates = Dates.Except(dHallDates).ToArray();
                 aHallDates = aHallDates.Except(GetNoTimeSlots(hall_id, aHallDates.ToArray())).ToArray();

              
                 
                 Hall_Dates[hall_id] = aHallDates;

                
             }
             List<ViewModelFrontEndSearchResult> Result = new List<ViewModelFrontEndSearchResult>();
             
             foreach (int hall_id in hall_ids)
             {
                 IEnumerable<DateTime> dates = Hall_Dates[hall_id];
                 hall hall = DB.halls.Find(hall_id);
              
                 

                 foreach (DateTime date in dates)
                 {
                     DateTime date1 = date;
                   int  weekdayid = GetWeekDayId(date);
                   int CountNormalBookingsForThatDay = DB.bookings.Count(r => r.hall_id == hall_id && r.booking_date == date1);
                     int CountTimeSlotsForThatDay =
                         DB.time_slots.Count(r => r.hall_id == hall_id && r.weekday_id == weekdayid);
                     int CountSpecialSlotsThatDay =
                         DB.special_time_slots.Where(r => r.is_special_slot && r.special_date == date1).ToList().Count();
                     int?[] CaterersIds =
                         DB.hall_caterers.Where(r => r.hall_id == hall_id).Select(r => r.caterer_id).ToArray();
                     string[] CaterersName = (from xx in DB.caterers
                                             where CaterersIds.Contains(xx.id)
                                             select xx.caterer_name).ToArray();

                     int?[] LocationsIds =
                               DB.hall_areas.Where(r => r.hall_id == hall_id).Select(r => r.area_id).ToArray();
                     string[] LocationNames = (from xx in DB.service_areas
                                               where LocationsIds.Contains(xx.id)
                                              select xx.service_area).ToArray();

                     int?[] EventTypeIds =
                               DB.hall_event_types.Where(r => r.hall_id == hall_id).Select(r => r.type_id).ToArray();
                     string[] EventTypes = (from xx in DB.event_types
                                            where EventTypeIds.Contains(xx.id)
                                              select xx.type_name).ToArray();
                      
                     if (CountNormalBookingsForThatDay >= (CountTimeSlotsForThatDay + CountSpecialSlotsThatDay))
                     {
                         continue;
                     }
                     try
                     {
                         ViewModelIsDisabled DateId = new ViewModelIsDisabled() ;
                         DateId.hall_id = hall_id;
                         DateId.date = date;
                       IEnumerable<ViewModelHallCalenderTimeSlot> TimeSlots = GetTimeSlotsForHallCalender(DateId);
                         string[] ETimeSlots =
                             TimeSlots.Where(r => r.disabled == false).Select(r=>r.time_slot).ToArray();

                         ViewModelFrontEndSearchResult instance = new ViewModelFrontEndSearchResult
                         {
                             hall_id = hall_id ,
                             booking_date = date.ToString("dddd, dd MMMM yyyy"),
                             hall_name = hall.hall_name,
                             image_name =
                                 DB.hall_images.Any(r => r.hall_id == hall_id)
                                     ? DB.hall_images.FirstOrDefault(r => r.hall_id == hall_id).image_name
                                     : "NO Image found",
                             address1 = hall.address1,
                             capacity = hall.capacity,
                             Caterers = CaterersName,
                             phone = hall.phone  ,
                             Locations = LocationNames,
                             EventTypes = EventTypes,
                             city = hall.city,
                             state =hall.state,
                             zipcode = hall.zip_code,
                             timeslots = ETimeSlots,
                             event_date = date.ToString("MM/dd/yyyy")
                         };
                         Result.Add(instance);
                     }
                     catch (Exception e)
                     {
                         var x = e;
                         throw;
                     }
                  
                     
                 }

             }


             return Result;
        }

        public IEnumerable<DateTime> GetNoTimeSlots(int hall_id , DateTime[] dates)
        {
            int[] activeWeekDaysIdInts =
                DB.time_slots.Where(r => !string.IsNullOrEmpty(r.time_slot) && r.weekday_id != 0 && r.weekday_id!=null && r.hall_id == hall_id)
                    .AsEnumerable()
                    .Select(r => r.weekday_id != null ? (int)r.weekday_id : 0).Distinct()
                    .ToArray();
         
            DateTime[] EnabledDates = new DateTime[dates.Count()];
                                 int count = 0;

            foreach (var date in dates)
            {

                int weekdayid = GetWeekDayId(date);
           
                var isSpecialDisabledDate = DB.special_time_slots.Any(r => r.is_disable_record && r.hall_id==hall_id && r.is_special_slot &&r.special_date == date);
                bool contains = activeWeekDaysIdInts.Contains(weekdayid);
                if (contains && !isSpecialDisabledDate)
                {
                    EnabledDates[count++] = date;
                }
               


            }


           

            return dates.Except(EnabledDates) ;
        }

   


        public int GetWeekDayId(DateTime day)
        {
            string dayofWeek = day.ToString("dddd");
            int weekdayid;
            switch (dayofWeek)
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
            return weekdayid;
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


    }
}
