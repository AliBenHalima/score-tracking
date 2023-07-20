﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ScoreTracking.App.Database;

#nullable disable

namespace ScoreTracking.App.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ScoreTracking.App.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<DateTimeOffset?>("EndedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("ended_at");

                    b.Property<int>("EndingType")
                        .HasColumnType("integer")
                        .HasColumnName("ending_type");

                    b.Property<bool>("HasJokerPenalty")
                        .HasColumnType("boolean")
                        .HasColumnName("has_joker_penalty");

                    b.Property<int>("JokerPenaltyValue")
                        .HasColumnType("integer")
                        .HasColumnName("joker_penalty_value");

                    b.Property<int>("Score")
                        .HasColumnType("integer")
                        .HasColumnName("score");

                    b.Property<DateTimeOffset>("Updated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated");

                    b.HasKey("Id")
                        .HasName("pk_games");

                    b.ToTable("games", (string)null);
                });

            modelBuilder.Entity("ScoreTracking.App.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("last_name");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("phone");

                    b.Property<DateTimeOffset>("Updated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("ix_users_email");

                    b.HasIndex("Phone")
                        .IsUnique()
                        .HasDatabaseName("ix_users_phone");

                    b.ToTable("users", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Created = new DateTimeOffset(new DateTime(2023, 7, 20, 15, 7, 25, 595, DateTimeKind.Unspecified).AddTicks(1376), new TimeSpan(0, 0, 0, 0, 0)),
                            Email = "Test1@gmail.com",
                            FirstName = "Robert",
                            LastName = "Lara",
                            Phone = "+21600000001",
                            Updated = new DateTimeOffset(new DateTime(2023, 7, 20, 15, 7, 25, 595, DateTimeKind.Unspecified).AddTicks(1386), new TimeSpan(0, 0, 0, 0, 0))
                        },
                        new
                        {
                            Id = 2,
                            Created = new DateTimeOffset(new DateTime(2023, 7, 20, 15, 7, 25, 595, DateTimeKind.Unspecified).AddTicks(1390), new TimeSpan(0, 0, 0, 0, 0)),
                            Email = "Test2@gmail.com",
                            FirstName = "John",
                            LastName = "Doe",
                            Phone = "+21600000002",
                            Updated = new DateTimeOffset(new DateTime(2023, 7, 20, 15, 7, 25, 595, DateTimeKind.Unspecified).AddTicks(1390), new TimeSpan(0, 0, 0, 0, 0))
                        });
                });

            modelBuilder.Entity("ScoreTracking.App.Models.UserGame", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.Property<int>("GameId")
                        .HasColumnType("integer")
                        .HasColumnName("game_id");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<int>("Id")
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("Updated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated");

                    b.HasKey("UserId", "GameId")
                        .HasName("pk_user_game");

                    b.HasIndex("GameId")
                        .HasDatabaseName("ix_user_game_game_id");

                    b.ToTable("user_game", (string)null);
                });

            modelBuilder.Entity("ScoreTracking.App.Models.UserGame", b =>
                {
                    b.HasOne("ScoreTracking.App.Models.Game", "Game")
                        .WithMany("UserGames")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_game_games_game_id");

                    b.HasOne("ScoreTracking.App.Models.User", "User")
                        .WithMany("UserGames")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_game_users_user_id");

                    b.Navigation("Game");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ScoreTracking.App.Models.Game", b =>
                {
                    b.Navigation("UserGames");
                });

            modelBuilder.Entity("ScoreTracking.App.Models.User", b =>
                {
                    b.Navigation("UserGames");
                });
#pragma warning restore 612, 618
        }
    }
}