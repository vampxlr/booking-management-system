namespace SimchaMVC
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("calendar")]
    public partial class calendar
    {
        public int id { get; set; }

        public DateTime? calendar_date { get; set; }

        [StringLength(2045)]
        public string comments { get; set; }

        [StringLength(101)]
        public string special_price { get; set; }

        public int? not_active { get; set; }

        public int? hall_id { get; set; }

        public int? timespan { get; set; }
    }
}
