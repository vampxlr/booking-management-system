using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SimchaMVC.Models.ViewModelsWebApi
{
    public class ViewModelFrontEndSearchResult
    {
       

        [StringLength(245)]
        public string hall_name { get; set; }
        public int hall_id { get; set; }
         
        public string booking_date { get; set; }
        public string event_date { get; set; }
        [StringLength(145)]
        public string address1 { get; set; }
        [StringLength(145)]
        public string address2 { get; set; }

        public string phone { get; set; }
        public string[] Locations { get; set; }
        public string[] Caterers { get; set; }
        public string[] EventTypes { get; set; }

        

        [StringLength(245)]
        public string capacity { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zipcode { get; set; }
        public string[] timeslots { get; set; }

        [StringLength(245)]
        public string image_name { get; set; }

    }
}