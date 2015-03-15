namespace SimchaMVC
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class weekday
    {
        public int id { get; set; }

        [StringLength(100)]
        public string day_name { get; set; }

        public int? order_by { get; set; }
    }
}
