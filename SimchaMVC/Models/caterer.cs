namespace SimchaMVC
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class caterer
    {
        public int id { get; set; }

        [StringLength(245)]
        public string caterer_name { get; set; }

        public int? hashgachah { get; set; }

        [StringLength(2045)]
        public string description { get; set; }

        [StringLength(400)]
        public string price { get; set; }
    }
}
