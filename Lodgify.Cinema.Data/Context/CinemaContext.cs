﻿using Lodgify.Cinema.Domain.Entitie;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Lodgify.Cinema.Infrastructure.Data.Context
{
    public class CinemaContext : DbContext, IDbContext
    {
        public CinemaContext(DbContextOptions<CinemaContext> options) : base(options)
        {

        }

        public DbSet<AuditoriumEntity> Auditoriums { get; set; }
        public DbSet<ShowtimeEntity> Showtimes { get; set; }
        public DbSet<MovieEntity> Movies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuditoriumEntity>(build =>
            {
                build.HasKey(entry => entry.Id);
                build.Property(entry => entry.Id).ValueGeneratedOnAdd();
                build.HasMany(entry => entry.Showtimes).WithOne().HasForeignKey(entity => entity.AuditoriumId);
            });

            modelBuilder.Entity<ShowtimeEntity>(build =>
            {
                build.HasKey(entry => entry.Id);
                build.Property(entry => entry.Id).ValueGeneratedOnAdd();
                build.Property(entry => entry.Schedule).HasConversion(x => string.Join(",", x), y => y.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList());
                build.HasOne(entry => entry.Movie).WithOne().HasForeignKey<MovieEntity>(entry => entry.ShowtimeId);
            });

            modelBuilder.Entity<MovieEntity>(build =>
            {
                build.HasKey(entry => entry.Id);
                build.Property(entry => entry.Id).ValueGeneratedOnAdd();
            });
        }
    }
}
