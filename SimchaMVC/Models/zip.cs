namespace SimchaMVC
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
 

    public partial class zip
    {
        [Key]
        [Column("zip")]
        [StringLength(5)]
        public string zip1 { get; set; }

        [StringLength(64)]
        public string city { get; set; }

        [StringLength(2)]
        public string state { get; set; }

        [StringLength(1)]
        public string type { get; set; }

        public int? timezone { get; set; }

        [StringLength(145)]
        public string lat { get; set; }

        [StringLength(145)]
        public string lon { get; set; }

        [StringLength(45)]
        public string dst { get; set; }
    }
}
