using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimchaMVC.Models.ViewModelsWebApi
{
    public class ViewModelHallCalenderTimeSlot
    {
       
        public int id { get; set; }

        public int? weekday_id { get; set; }

        public string date { get; set; }

   
        public string time_slot { get; set; }


        public string slot_amount { get; set; }

        public bool disabled { get; set; }

        public int? hall_id { get; set; }
        public int? disable_time_slot_id { get; set; }
        public bool? is_special_slot { get; set; }
        public decimal? slot_price { get; set; }
    }
}