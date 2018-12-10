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
        public virtual DbSet<CategoryAssignment> CategoryAssignments { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<GameOwnership> GameOwnerships { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }
        public virtual DbSet<PublisherCode> PublisherCodes { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(e => e.CategoryAssignments)
                .WithRequired(e => e.Category)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Game>()
                .HasMany(e => e.CategoryAssignments)
                .WithRequired(e => e.Game)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Game>()
                .HasMany(e => e.GameOwnerships)
                .WithRequired(e => e.Game)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Publisher>()
                .Property(e => e.DeveloperName)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .HasMany(e => e.GameOwnerships)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasOptional(e => e.Publisher)
                .WithRequired(e => e.User);
        }
    }
}
