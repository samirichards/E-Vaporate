namespace E_Vaporate.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Games = new HashSet<Game>();
        }

        public int UserID { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        public byte[] HashedPassword { get; set; }

        public double AccountFunds { get; set; }

        [StringLength(50)]
        public string Postcode { get; set; }

        [StringLength(50)]
        public string AddrLine1 { get; set; }

        [StringLength(50)]
        public string AddrLine2 { get; set; }

        [StringLength(50)]
        public string AddrLine3 { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        public virtual Publisher Publisher { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Game> Games { get; set; }
    }
}
