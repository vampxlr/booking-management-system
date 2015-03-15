namespace SimchaMVC
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class event_types
    {
        public int id { get; set; }

        [StringLength(145)]
        public string type_name { get; set; }

        public int? order_by { get; set; }
    }
}
