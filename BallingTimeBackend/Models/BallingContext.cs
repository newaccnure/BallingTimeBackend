using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BallingTimeBackend.Models
{
    public class BallingContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<DribblingDrill> DribblingDrills { get; set; }
        public DbSet<TrainingProgram> TrainingPrograms { get; set; }
        public DbSet<UserProgress> UserProgresses { get; set; }

        public BallingContext(DbContextOptions<BallingContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Difficulty>().ToTable("Difficulties");
            modelBuilder.Entity<DribblingDrill>().ToTable("Dribbling drills");

            modelBuilder.Entity<UserProgress>()
                .HasKey(t => new {t.DribblingDrillId, t.UserId });

            modelBuilder.Entity<UserProgress>()
                .HasOne(up => up.User)
                .WithMany(user => user.UserProgresses)
                .HasForeignKey(up => up.UserId);

            modelBuilder.Entity<UserProgress>()
                .HasOne(up => up.DribblingDrill)
                .WithMany(de => de.UserProgresses)
                .HasForeignKey(up => up.DribblingDrillId);

            modelBuilder.Entity<UserProgress>().ToTable("User progresses");

            modelBuilder.Entity<TrainingProgram>()
                .HasKey(t => new { t.DribblingDrillId, t.DifficultyId });

            modelBuilder.Entity<TrainingProgram>()
                .HasOne(tp => tp.Difficulty)
                .WithMany(diffic => diffic.TrainingPrograms)
                .HasForeignKey(tp => tp.DifficultyId);

            modelBuilder.Entity<TrainingProgram>()
                .HasOne(tp => tp.DribblingDrill)
                .WithMany(de => de.TrainingPrograms)
                .HasForeignKey(tp => tp.DribblingDrillId);

            modelBuilder.Entity<TrainingProgram>().ToTable("Training program");

            base.OnModelCreating(modelBuilder);
        }
    }
}
