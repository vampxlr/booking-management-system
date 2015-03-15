using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SimchaMVC
{
    public class ViewModelBookingUser
    {


 
        public string id { get; set; }

        public int? user_id { get; set; }

        [StringLength(150)]
        public string time_slot { get; set; }

        public double? total { get; set; }

        public string create_date { get; set; }

 
        public DateTime booking_date { get; set; }

        [StringLength(145)]
        public string booking_status { get; set; }

        [StringLength(4045)]
        public string customer_notes { get; set; }

        [StringLength(45)]
        public string card_type { get; set; }

        [StringLength(45)]
        public string card_expiration { get; set; }

        [StringLength(45)]
        public string card_number { get; set; }

        [StringLength(415)]
        public string payment_type { get; set; }

        [StringLength(45)]
        public string cvv { get; set; }

        public int? hall_id { get; set; }

        [StringLength(145)]
        public string simcha_type { get; set; }

      

        [StringLength(100)]
        public string internal_status { get; set; }

        [StringLength(45)]
        public string first_name { get; set; }

        [StringLength(45)]
        public string last_name { get; set; }

        [StringLength(45)]
        public string address1 { get; set; }

        [StringLength(45)]
        public string address2 { get; set; }

        [StringLength(45)]
        public string city { get; set; }

        [StringLength(45)]
        public string state { get; set; }

        [StringLength(45)]
        public string zip_code { get; set; }

        [StringLength(45)]
        public string phone { get; set; }

        [StringLength(45)]
        public string fax { get; set; }

        [StringLength(45)]
        public string email { get; set; }

        [StringLength(245)]
        public string name_on_card { get; set; }

        public int? wishlist { get; set; }



        [StringLength(145)]
        public string user_name { get; set; }

        [StringLength(145)]
        public string user_password { get; set; }

        

        [StringLength(45)]
        public string zipcode { get; set; }

       

       

        [StringLength(4045)]
        [DataType(DataType.MultilineText)]
        public string internal_notes { get; set; }

        public DateTime? signup_date { get; set; }

        public int? booking_level { get; set; }

        public int? hall_level { get; set; }

        [StringLength(500)]
        public string password_hint { get; set; }

        [StringLength(145)]
        public string cell_phone { get; set; }

        [StringLength(245)]
        public string best_time { get; set; }

        [StringLength(150)]
        public string contact_method { get; set; }

        public int? agree { get; set; }

        public int? mailing_list { get; set; }

        public int? keep_signed { get; set; }

    }
}