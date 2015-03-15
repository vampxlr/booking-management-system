namespace SimchaMVC
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class admin_users
    {
        public int id { get; set; }

        [StringLength(145)]
        public string user_name { get; set; }

        [StringLength(145)]
        public string user_password { get; set; }

        [StringLength(45)]
        public string first_name { get; set; }

        [StringLength(45)]
        public string last_name { get; set; }

        [StringLength(145)]
        public string address { get; set; }

        [StringLength(45)]
        public string city { get; set; }

        [StringLength(45)]
        public string state { get; set; }

        [StringLength(45)]
        public string zipcode { get; set; }

        public int? admin_level { get; set; }

        public DateTime? last_login { get; set; }

        public int? booking_level { get; set; }

        public int? hall_level { get; set; }

        public int? slots_level { get; set; }
        public string user_role { get; set; }
        
    }
}
