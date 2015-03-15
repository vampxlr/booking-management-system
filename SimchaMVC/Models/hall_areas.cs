namespace SimchaMVC
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class hall_areas
    {
        public int id { get; set; }

        public int? hall_id { get; set; }

        public int? area_id { get; set; }
    }
}
