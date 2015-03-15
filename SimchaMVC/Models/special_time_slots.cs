using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SimchaMVC.Models
{
    public class special_time_slots
    {   [Key]
        public int id { get; set; }
    public string special_time_slot { get; set; }
    public DateTime special_date { get; set; }
    public int disable_time_slot_id { get; set; }
    public int hall_id { get; set; }
    public bool is_disable_record { get; set; }
    public bool is_special_slot { get; set; }
    public int weekday_id { get; set; }
    public int special_time_slot_id { get; set; }
    public int? slot_amount { get; set; }
    public decimal? slot_price { get; set; }  
    }
}