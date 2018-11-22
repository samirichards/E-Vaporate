namespace E_Vaporate.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class EVaporateModel : DbContext
    {
        public EVaporateModel()
            : base("name=ServerDB")
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(e => e.Games)
                .WithMany(e => e.Categories)
                .Map(m => m.ToTable("CategoryAssignments").MapLeftKey("CategoryID").MapRightKey("GameID"));

            modelBuilder.Entity<Game>()
                .HasMany(e => e.Users)
                .WithMany(e => e.Games)
                .Map(m => m.ToTable("GameOwnership").MapLeftKey("GameID").MapRightKey("UserID"));

            modelBuilder.Entity<Publisher>()
                .HasMany(e => e.Games)
                .WithRequired(e => e.Publisher1)
                .HasForeignKey(e => e.Publisher)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasOptional(e => e.Publisher)
                .WithRequired(e => e.User);
        }
    }
}
