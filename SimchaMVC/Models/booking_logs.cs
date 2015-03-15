namespace SimchaMVC
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class booking_logs
    {
        public int id { get; set; }

        public int? booking_id { get; set; }

        [StringLength(500)]
        public string notes { get; set; }

        public DateTime? log_date { get; set; }

        public int? user_id { get; set; }
    }
}
