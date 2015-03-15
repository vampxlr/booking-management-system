namespace SimchaMVC
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class hashgacha_images
    {
        public int id { get; set; }

        [StringLength(425)]
        public string hashgacha_name { get; set; }

        [StringLength(245)]
        public string hashgacha_image { get; set; }

        [StringLength(5045)]
        public string notes { get; set; }

        public int? order_by { get; set; }
    }
}
