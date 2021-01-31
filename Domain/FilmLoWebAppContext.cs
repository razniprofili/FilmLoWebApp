//using Common.Helpers;
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
        public virtual DbSet<WatchedMoviesStats> WatchedMoviesStats { get; set; }
        public virtual DbSet<PopularMovies> PopularMovies { get; set; }
        public virtual DbSet<YearStatistic> YearStatistic { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           //  base.OnConfiguring(optionsBuilder);

             optionsBuilder.UseSqlServer(Helper.ConnectionString);
           // optionsBuilder.UseSqlServer("Server=DESKTOP-S892R9E\\TICASQL;Database=Test1Database;Trusted_Connection=True");
            // optionsBuilder.UseSqlServer("Server=TMILOSEVIC-HP; Database=Test1Database;Trusted_Connection=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<WatchedMovie>(entity =>
            {
                entity.HasKey(m => new { m.UserId, m.MovieJMDBApiId });

                entity.HasOne(u => u.User).WithMany(m => m.WatchedMovies).HasForeignKey(f => f.UserId);

                entity.HasOne(u => u.MovieJMDBApi).WithMany(m => m.WatchedUsers).HasForeignKey(f => f.MovieJMDBApiId);

            });

            modelBuilder.Entity<MovieJMDBApi>().OwnsOne(m => m.MovieDetailsJMDBApi); 
            

            modelBuilder.Entity<SavedMovie>().HasKey(m => new { m.UserId, m.MovieJMDBApiId });

            modelBuilder.Entity<Friendship>(entity =>
            {

                entity.HasKey(m => new { m.UserRecipientId, m.UserSenderId });

                entity.HasOne(t => t.StatusCode).WithMany();

                entity.HasOne(u => u.UserRecipient).WithMany(t => t.FriendsReceived).HasForeignKey(t => t.UserRecipientId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(u => u.UserSender).WithMany(t => t.FriendsSent).HasForeignKey(t => t.UserSenderId).OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<Notification>(entity =>
            {

                entity.HasKey(m => m.Id);

                entity.HasOne(u => u.UserRecipient).WithMany(t => t.NotificationsReceived).HasForeignKey(t => t.UserRecipientId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(u => u.UserSender).WithMany(t => t.NotificationsSent).HasForeignKey(t => t.UserSenderId).OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<WatchedMoviesStats>(stats => {
                stats.HasNoKey();
                stats.ToView("WatchedMoviestStats");
                stats.Property(s => s.UserId).HasColumnName("userId");
                stats.Property(s => s.TotalCount).HasColumnName("totalCount");
                stats.Property(s => s.TotalTime).HasColumnName("totalTime");
                stats.Property(s => s.AverageRate).HasColumnName("averageRate");
            });

            modelBuilder.Entity<PopularMovies>(popmovies => {
                popmovies.HasNoKey();
                popmovies.ToView("PopularMovies");
                popmovies.Property(s => s.UserId).HasColumnName("Id");
                popmovies.Property(s => s.MovieId).HasColumnName("MovieJMDBApiId");
                popmovies.Property(s => s.MovieName).HasColumnName("Name");
            });

            modelBuilder.Entity<YearStatistic>(yearStat => {
                yearStat.HasNoKey();
                yearStat.ToView("YearStatistic");
                yearStat.Property(s => s.UserId).HasColumnName("userId");
                yearStat.Property(s => s.Year).HasColumnName("watchingYear");
                yearStat.Property(s => s.Count).HasColumnName("totalMovies");
            });

        }

    }
}
