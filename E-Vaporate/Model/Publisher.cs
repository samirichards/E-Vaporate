namespace E_Vaporate.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Publisher")]
    public partial class Publisher
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PublisherID { get; set; }

        [StringLength(10)]
        public string DeveloperName { get; set; }

        public virtual User User { get; set; }
    }
}
