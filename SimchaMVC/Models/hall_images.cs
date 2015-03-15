namespace SimchaMVC
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class hall_images
    {
        [Key]
        public int id { get; set; }

        public int? hall_id { get; set; }

        [StringLength(245)]
        public string image_name { get; set; }

        [StringLength(800)]
        public string image_caption { get; set; }

        public int? order_by { get; set; }
    }
}
