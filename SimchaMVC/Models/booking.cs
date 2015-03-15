namespace SimchaMVC
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class booking
    {
        [Key]
        [Column(Order = 0)]
        public int id { get; set; }
      
        public int? user_id { get; set; }
      
        [StringLength(150)]
        public string time_slot { get; set; }

        public double? total { get; set; }
        public DateTime? create_date { get; set; }

        [Column(Order = 1)]
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

        [StringLength(1045)]
        public string internal_notes { get; set; }

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

        public int? time_slot_id { get; set; }
        public bool? is_special_slot { get; set; }


    }
}
