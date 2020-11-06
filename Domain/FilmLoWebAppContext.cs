using Common.Helpers;
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

            optionsBuilder.UseSqlServer(Helper.ConnectionString);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<MovieDetailsJMDBApi>(entity => entity.HasMany(u => u.Users));
            //modelBuilder.Entity<MovieJMDBApi>(entity => entity.HasMany(u => u.Users));
            //modelBuilder.Entity<User>(entity => entity.HasMany(u => u.WatchedMovies));
            //modelBuilder.Entity<User>(entity => entity.HasMany(u => u.SavedMovies));

            modelBuilder.Entity<WatchedMovie>(entity =>
            {
                entity.HasKey(m => new { m.UserId, m.MovieDetailsJMDBApiId });

                entity.HasOne(u => u.User).WithMany(m => m.WatchedMovies).HasForeignKey(f => f.UserId);

                entity.HasOne(u => u.MovieDetailsJMDBApi).WithMany(m => m.Users).HasForeignKey(f => f.MovieDetailsJMDBApiId);

            });
           
            
            modelBuilder.Entity<SavedMovie>().HasKey(m => new { m.UserId, m.MovieJMDBApiId });

            modelBuilder.Entity<Friendship>(entity =>
            {

                entity.HasKey(m => new { m.UserRecipientId, m.UserSenderId });

                entity.HasOne(t => t.StatusCode).WithMany();

              //  entity.HasOne(u => u.UserRecipient).WithMany(t => t.Friends).HasForeignKey(t => t.UserRecipientId);

                //entity.HasOne(u => u.UserSender).WithMany(t => t.Friends).HasForeignKey(t => t.UserSenderId);

            });

        }

    }
}
