namespace SimchaMVC
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class service_areas
    {
        public int id { get; set; }

        [StringLength(145)]
        public string service_area { get; set; }

        public int? order_by { get; set; }
    }
}
