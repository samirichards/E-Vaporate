namespace E_Vaporate.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PublisherCode")]
    public partial class PublisherCode
    {
        [Key]
        public int PubCodeID { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        public int NumberOfUses { get; set; }
    }
}
