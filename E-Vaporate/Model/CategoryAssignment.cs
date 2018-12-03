namespace E_Vaporate.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CategoryAssignment
    {
        public int GameID { get; set; }

        public int CategoryID { get; set; }

        [Key]
        public int AssignmentID { get; set; }

        public virtual Category Category { get; set; }

        public virtual Game Game { get; set; }
    }
}
