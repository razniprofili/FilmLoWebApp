﻿using Common.Helpers;
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
           //  base.OnConfiguring(optionsBuilder);

             optionsBuilder.UseSqlServer(Helper.ConnectionString);
            //optionsBuilder.UseSqlServer("Server=DESKTOP-S892R9E\\TICASQL;Database=FilmLoWebAPI;Trusted_Connection=True");
            //optionsBuilder.UseSqlServer("Server=TMILOSEVIC-HP; Database=Test1Database;Trusted_Connection=True");
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

        }

    }
}
