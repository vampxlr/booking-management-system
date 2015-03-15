namespace SimchaMVC
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class user
    {
        public int id { get; set; }

        [StringLength(145)]
        public string user_name { get; set; }

        [StringLength(145)]
        public string user_password { get; set; }

        [StringLength(145)]
        public string first_name { get; set; }

        [StringLength(145)]
        public string last_name { get; set; }

        [StringLength(145)]
        public string address1 { get; set; }

        [StringLength(145)]
        public string address2 { get; set; }

        [StringLength(145)]
        public string city { get; set; }

        [StringLength(45)]
        public string state { get; set; }

        [StringLength(45)]
        public string zipcode { get; set; }

        [StringLength(45)]
        public string phone { get; set; }

        [StringLength(145)]
        public string fax { get; set; }

        [StringLength(145)]
        public string email { get; set; }

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
