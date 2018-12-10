namespace E_Vaporate.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Game")]
    public partial class Game
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Game()
        {
            CategoryAssignments = new HashSet<CategoryAssignment>();
            GameOwnerships = new HashSet<GameOwnership>();
        }

        public int GameID { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public int Publisher { get; set; }

        [Required]
        public string Directory { get; set; }

        public byte[] Thumbnail { get; set; }

        public byte[] HeaderImage { get; set; }

        public double Price { get; set; }

        public bool Available { get; set; }

        public DateTime? TimeAdded { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CategoryAssignment> CategoryAssignments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GameOwnership> GameOwnerships { get; set; }
    }
}
