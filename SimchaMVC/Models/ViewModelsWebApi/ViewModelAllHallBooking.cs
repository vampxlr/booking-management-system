using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimchaMVC.Models.ViewModelsWebApi
{
    public class ViewModelAllHallBooking
    {
         public int hallsid { get; set; }                              
          public string hallscapacity { get; set; }                       
          public string hallsaddress1 { get; set; }                    
          public string hallsaddress2 { get; set; }                    
          public string hallsstate { get; set; }                       
          public string hallscity { get; set; }
          public string hallszip_code { get; set; }
          public string eventypestype_name { get; set; }
          public string servicearesservice_area { get; set; }
          public string hallimagesimage_name { get; set; }
          public DateTime hallbookingsbooking_date { get; set; }
          public string hallbookingsbooking_status { get; set; }
          public string hallbookingstime_slot { get; set; }
          public DateTime? halldisableddatescalendar_date { get; set; }               
    }
}