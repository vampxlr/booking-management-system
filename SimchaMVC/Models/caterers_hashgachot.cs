namespace SimchaMVC
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class caterers_hashgachot
    {
        public int id { get; set; }

        public int? caterer_id { get; set; }

        public int? hashgacha_id { get; set; }
    }
}
