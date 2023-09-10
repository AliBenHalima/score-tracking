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

                    b.Property<DateTimeOffset?>("CanceledAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("canceled_at");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("code");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTimeOffset?>("EndedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("ended_at");

                    b.Property<int?>("EndingType")
                        .HasColumnType("integer")
                        .HasColumnName("ending_type");

                    b.Property<bool>("HasJokerPenalty")
                        .HasColumnType("boolean")
                        .HasColumnName("has_joker_penalty");

                    b.Property<int>("JokerPenaltyValue")
                        .HasColumnType("integer")
                        .HasColumnName("joker_penalty_value");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<int>("Score")
                        .HasColumnType("integer")
                        .HasColumnName("score");

                    b.Property<DateTimeOffset?>("StartedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("started_at");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_games");

                    b.ToTable("games", (string)null);
                });

            modelBuilder.Entity("ScoreTracking.App.Models.Round", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<int>("Number")
                        .HasColumnType("integer")
                        .HasColumnName("number");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_rounds");

                    b.ToTable("rounds", (string)null);
                });

            modelBuilder.Entity("ScoreTracking.App.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("first_name");

                    b.Property<string>("ImagePath")
                        .HasColumnType("text")
                        .HasColumnName("image_path");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("last_name");

                    b.Property<string>("Password")
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<DateTimeOffset?>("PasswordChangedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("password_changed_at");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("phone");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<DateTimeOffset?>("VerifiedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("verified_at");

                    b.Property<uint>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid")
                        .HasColumnName("xmin");

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
                            CreatedAt = new DateTimeOffset(new DateTime(2023, 9, 9, 18, 17, 41, 829, DateTimeKind.Unspecified).AddTicks(6385), new TimeSpan(0, 0, 0, 0, 0)),
                            Email = "Test1@gmail.com",
                            FirstName = "Robert",
                            LastName = "Lara",
                            Phone = "+21600000001",
                            UpdatedAt = new DateTimeOffset(new DateTime(2023, 9, 9, 18, 17, 41, 829, DateTimeKind.Unspecified).AddTicks(6423), new TimeSpan(0, 0, 0, 0, 0))
                        },
                        new
                        {
                            Id = 2,
                            CreatedAt = new DateTimeOffset(new DateTime(2023, 9, 9, 18, 17, 41, 829, DateTimeKind.Unspecified).AddTicks(6441), new TimeSpan(0, 0, 0, 0, 0)),
                            Email = "Test2@gmail.com",
                            FirstName = "John",
                            LastName = "Doe",
                            Phone = "+21600000002",
                            UpdatedAt = new DateTimeOffset(new DateTime(2023, 9, 9, 18, 17, 41, 829, DateTimeKind.Unspecified).AddTicks(6442), new TimeSpan(0, 0, 0, 0, 0))
                        });
                });

            modelBuilder.Entity("ScoreTracking.App.Models.UserGame", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<int>("GameId")
                        .HasColumnType("integer")
                        .HasColumnName("game_id");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_user_games");

                    b.HasIndex("GameId")
                        .HasDatabaseName("ix_user_games_game_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_user_games_user_id");

                    b.ToTable("user_games", (string)null);
                });

            modelBuilder.Entity("ScoreTracking.App.Models.UserGameRound", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<int>("Jokers")
                        .HasColumnType("integer")
                        .HasColumnName("jokers");

                    b.Property<int>("RoundId")
                        .HasColumnType("integer")
                        .HasColumnName("round_id");

                    b.Property<int>("Score")
                        .HasColumnType("integer")
                        .HasColumnName("score");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<int>("UserGameId")
                        .HasColumnType("integer")
                        .HasColumnName("user_game_id");

                    b.HasKey("Id")
                        .HasName("pk_user_game_rounds");

                    b.HasIndex("RoundId")
                        .HasDatabaseName("ix_user_game_rounds_round_id");

                    b.HasIndex("UserGameId")
                        .HasDatabaseName("ix_user_game_rounds_user_game_id");

                    b.ToTable("user_game_rounds", (string)null);
                });

            modelBuilder.Entity("ScoreTracking.App.Models.UserGame", b =>
                {
                    b.HasOne("ScoreTracking.App.Models.Game", "Game")
                        .WithMany("UserGames")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_games_games_game_id");

                    b.HasOne("ScoreTracking.App.Models.User", "User")
                        .WithMany("UserGames")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_games_users_user_id");

                    b.Navigation("Game");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ScoreTracking.App.Models.UserGameRound", b =>
                {
                    b.HasOne("ScoreTracking.App.Models.Round", "Round")
                        .WithMany("UserGameRounds")
                        .HasForeignKey("RoundId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_game_rounds_rounds_round_id1");

                    b.HasOne("ScoreTracking.App.Models.UserGame", "UserGame")
                        .WithMany("UserGameRounds")
                        .HasForeignKey("UserGameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_game_rounds_user_games_user_game_id1");

                    b.Navigation("Round");

                    b.Navigation("UserGame");
                });

            modelBuilder.Entity("ScoreTracking.App.Models.Game", b =>
                {
                    b.Navigation("UserGames");
                });

            modelBuilder.Entity("ScoreTracking.App.Models.Round", b =>
                {
                    b.Navigation("UserGameRounds");
                });

            modelBuilder.Entity("ScoreTracking.App.Models.User", b =>
                {
                    b.Navigation("UserGames");
                });

            modelBuilder.Entity("ScoreTracking.App.Models.UserGame", b =>
                {
                    b.Navigation("UserGameRounds");
                });
#pragma warning restore 612, 618
        }
    }
}
