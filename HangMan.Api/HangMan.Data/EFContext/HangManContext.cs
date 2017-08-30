using HangMan.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace HangMan.Data.EFContext
{
    public class HangManContext : DbContext
    {
        public HangManContext() : base("name=HangMan")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            SetGameEntry(modelBuilder);
            SetPlayerEntry(modelBuilder);
            SetUserEntry(modelBuilder);
            SetWordEntry(modelBuilder);
        }

        private void SetGameEntry(DbModelBuilder modelBuilder)
        {
            SetIdAndRowVersion(modelBuilder.Entity<Game>());
            modelBuilder.Entity<Game>().HasRequired(u => u.Word).WithMany()
                .Map(m => m.MapKey("WordId"));
            modelBuilder.Entity<Game>().HasRequired(u => u.Player).WithMany(p => p.Games)
                .Map(m => m.MapKey("PlayerId"));
        }

        private void SetPlayerEntry(DbModelBuilder modelBuilder)
        {
            SetIdAndRowVersion(modelBuilder.Entity<Player>());
            modelBuilder.Entity<Player>().Property(u => u.NickName)
                .HasMaxLength(50)
                .IsRequired()
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("UX_UserName") { IsUnique = true }));
        }

        private void SetUserEntry(DbModelBuilder modelBuilder)
        {
            SetIdAndRowVersion(modelBuilder.Entity<User>());
            modelBuilder.Entity<User>().Property(u => u.UserName)
                .HasMaxLength(50)
                .IsRequired()
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("UX_UserName") { IsUnique = true }));
            modelBuilder.Entity<User>().Property(u => u.Password)
                .HasMaxLength(50)
                .IsRequired();
            modelBuilder.Entity<User>().HasRequired(u => u.Player).WithMany()
                .Map(m => m.MapKey("PlayerId"));
        }

        private void SetWordEntry(DbModelBuilder modelBuilder)
        {
            SetIdAndRowVersion(modelBuilder.Entity<Word>());
            modelBuilder.Entity<Word>().Property(w => w.GameWord)
                .HasMaxLength(50)
                .IsRequired()
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("UX_GameWord") { IsUnique = true }));
            modelBuilder.Entity<Word>().Property(u => u.Letters)
                .IsRequired();
            modelBuilder.Entity<Word>().Property(e => e.Difficulty)
                .HasColumnName("WordDifficultyId");
        }

        protected void SetIdAndRowVersion<T>(EntityTypeConfiguration<T> entity) where T : class, IEntity
        {
            var tableName = entity.GetType().GenericTypeArguments[0].Name;

            SetIdAndRowVersion(entity, tableName);
        }

        protected void SetIdAndRowVersion<T>(EntityTypeConfiguration<T> entity, string tableName) where T : class, IEntity
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName($"{tableName}Id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}