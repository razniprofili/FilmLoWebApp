﻿// <auto-generated />
using System;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Domain.Migrations
{
    [DbContext(typeof(FilmLoWebAppContext))]
    [Migration("20201109212130_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Domain.Friendship", b =>
                {
                    b.Property<long>("UserRecipientId")
                        .HasColumnType("bigint");

                    b.Property<long>("UserSenderId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("FriendshipDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("StatusCodeID")
                        .IsRequired()
                        .HasColumnType("nvarchar(1)");

                    b.HasKey("UserRecipientId", "UserSenderId");

                    b.HasIndex("StatusCodeID");

                    b.HasIndex("UserSenderId");

                    b.ToTable("Friendship");
                });

            modelBuilder.Entity("Domain.MovieDetailsJMDBApi", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Actors")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Director")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Duration")
                        .HasColumnType("int");

                    b.Property<string>("Genre")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("MovieDetailsJMDBApi");
                });

            modelBuilder.Entity("Domain.MovieJMDBApi", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Poster")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("MovieJMDBApi");
                });

            modelBuilder.Entity("Domain.SavedMovie", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<string>("MovieJMDBApiId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("SavingDate")
                        .HasColumnType("datetime2");

                    b.HasKey("UserId", "MovieJMDBApiId");

                    b.HasIndex("MovieJMDBApiId");

                    b.ToTable("SavedMovie");
                });

            modelBuilder.Entity("Domain.StatusCode", b =>
                {
                    b.Property<string>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(1)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Code");

                    b.ToTable("StatusCode");
                });

            modelBuilder.Entity("Domain.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Picture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Domain.WatchedMovie", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<long>("MovieDetailsJMDBApiId")
                        .HasColumnType("bigint");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<string>("WatchingDate")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "MovieDetailsJMDBApiId");

                    b.HasIndex("MovieDetailsJMDBApiId");

                    b.ToTable("WatchedMovie");
                });

            modelBuilder.Entity("Domain.Friendship", b =>
                {
                    b.HasOne("Domain.StatusCode", "StatusCode")
                        .WithMany()
                        .HasForeignKey("StatusCodeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.User", "UserRecipient")
                        .WithMany("FriendsReceived")
                        .HasForeignKey("UserRecipientId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Domain.User", "UserSender")
                        .WithMany("FriendsSent")
                        .HasForeignKey("UserSenderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.SavedMovie", b =>
                {
                    b.HasOne("Domain.MovieJMDBApi", "MovieJMDBApi")
                        .WithMany("Users")
                        .HasForeignKey("MovieJMDBApiId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.User", "User")
                        .WithMany("SavedMovies")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.WatchedMovie", b =>
                {
                    b.HasOne("Domain.MovieDetailsJMDBApi", "MovieDetailsJMDBApi")
                        .WithMany("Users")
                        .HasForeignKey("MovieDetailsJMDBApiId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.User", "User")
                        .WithMany("WatchedMovies")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}