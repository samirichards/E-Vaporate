namespace E_Vaporate.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GameOwnership")]
    public partial class GameOwnership
    {
        public int UserID { get; set; }

        public int GameID { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OwnershipID { get; set; }

        public virtual Game Game { get; set; }

        public virtual User User { get; set; }
    }
}
