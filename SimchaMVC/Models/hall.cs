namespace SimchaMVC
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class hall
    {
        public int id { get; set; }
       
        [StringLength(245)]
        public string hall_name { get; set; }

        [StringLength(145)]
        public string contact_name { get; set; }

        [StringLength(145)]
        public string address1 { get; set; }

        [StringLength(145)]
        public string address2 { get; set; }

        [StringLength(100)]
        public string city { get; set; }

        [StringLength(45)]
        public string state { get; set; }
     
        [StringLength(45)]
        public string zip_code { get; set; }
        
        [StringLength(45)]
        public string phone { get; set; }

        [StringLength(45)]
        public string phone2 { get; set; }

        [StringLength(45)]
        public string fax { get; set; }

        [StringLength(45)]
        public string email { get; set; }

        [StringLength(415)]
        public string website { get; set; }

        [StringLength(245)]
        public string capacity { get; set; }

        [StringLength(4000)]
        public string policies { get; set; }

        [StringLength(1000)]
        public string features { get; set; }

        [StringLength(515)]
        public string direction_doc { get; set; }

        [StringLength(5415)]
        public string directions { get; set; }

        [StringLength(45)]
        public string preferred_contact_method { get; set; }

        public int? hashgachah { get; set; }

        [StringLength(545)]
        public string service_area { get; set; }

        [StringLength(45)]
        public string password { get; set; }

        
        [StringLength(200)]
        public string user_name { get; set; }

        public byte Hall_Status { get; set; }

        [StringLength(245)]
        public string hall_pricing { get; set; }

        [StringLength(245)]
        public string office_hours { get; set; }

        public int admin_user_id { get; set; }
    }
}
