namespace SimchaMVC
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("wishlist")]
    public partial class wishlist
    {
        public int id { get; set; }

        public int? user_id { get; set; }

        public int? hall_id { get; set; }

        [StringLength(105)]
        public string event_date { get; set; }

        public int? time_slot { get; set; }

        public DateTime? create_date { get; set; }

        public int? event_type { get; set; }
    }
}
