using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
   public class FilmLoWebAppContext : DbContext
    {
        public virtual DbSet<WatchedMovie> WatchedMovie { get; set; }
        public virtual DbSet<SavedMovie> SavedMovie { get; set; }
        public virtual DbSet<Friendship> Friendship { get; set; }
        public virtual DbSet<MovieDetailsJMDBApi> MovieDetailsJMDBApi { get; set; }
        public virtual DbSet<MovieJMDBApi> MovieJMDBApi { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<StatusCode> StatusCode { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer("Server=DESKTOP-S892R9E\\TICASQL;Database=FilmLoTestWebApp;Trusted_Connection=True");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieDetailsJMDBApi>(entity => entity.HasMany(u => u.Users));
            modelBuilder.Entity<MovieJMDBApi>(entity => entity.HasMany(u => u.Users));
            modelBuilder.Entity<User>(entity => entity.HasMany(u => u.WatchedMovies));
            modelBuilder.Entity<User>(entity => entity.HasMany(u => u.SavedMovies));

            modelBuilder.Entity<WatchedMovie>(entity =>
            {
                entity.HasKey(m => new { m.UserId, m.MovieDetailsJMDBApiId });       

            });
           
            
            modelBuilder.Entity<SavedMovie>().HasKey(m => new { m.UserId, m.MovieJMDBApiId });

            modelBuilder.Entity<Friendship>(entity =>
            {

                entity.HasKey(m => new { m.UserRecipientId, m.UserSenderId });

                entity.HasOne(t => t.StatusCode)
                      .WithMany(u => u.Friendships)
                      .HasForeignKey(d => d.StatusCodeID)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_Friendship_StatusCode");

            });

        }

    }
}
