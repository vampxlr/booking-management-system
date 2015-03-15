using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimchaMVC.Models.ViewModelsWebApi
{
    public class ViewModelBookingShortDetails
    {
        public int? booking_id { get; set; }
        public string hall_name { get; set; }
        public string user_infromation { get; set; }
        public string booking_date { get; set; }
        public string fullbooking_url { get; set; }
        public string time_slot { get; set; }
        
    }
}