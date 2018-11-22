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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Publisher()
        {
            Games = new HashSet<Game>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PublisherID { get; set; }

        [StringLength(50)]
        public string DeveloperName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Game> Games { get; set; }

        public virtual User User { get; set; }
    }
}
