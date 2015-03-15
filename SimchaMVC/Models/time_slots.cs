namespace SimchaMVC
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class time_slots
    {
        [Key]
        public int id { get; set; }

        public int? weekday_id { get; set; }

        public int? slot_number { get; set; }

        [StringLength(145)]
        public string time_slot { get; set; }

        [StringLength(145)]
        public string slot_amount { get; set; }

        public int? order_by { get; set; }

        public int? hall_id { get; set; }


        public decimal? slot_price { get; set; }
    }
}
