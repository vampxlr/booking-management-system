namespace SimchaMVC
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class slot_numbers
    {
        public int id { get; set; }

        public int? slot_number { get; set; }
    }
}
