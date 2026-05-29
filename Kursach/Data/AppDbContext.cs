using Microsoft.EntityFrameworkCore;
using Kursach.Models;

namespace Kursach.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<College> Colleges { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Athlete> Athletes { get; set; }
    public DbSet<Achievement> Achievements { get; set; }
    public DbSet<News> News { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Athlete>()
            .HasOne(a => a.Team)
            .WithMany(t => t.Athletes)
            .HasForeignKey(a => a.TeamId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Team>()
            .HasOne(t => t.College)
            .WithMany(c => c.Teams)
            .HasForeignKey(t => t.CollegeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Achievement>()
            .HasOne(a => a.Athlete)
            .WithMany(at => at.Achievements)
            .HasForeignKey(a => a.AthleteId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Achievement>()
            .HasOne(a => a.Team)
            .WithMany(t => t.TeamAchievements)
            .HasForeignKey(a => a.TeamId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<News>()
            .HasOne(n => n.Athlete)
            .WithMany(at => at.News)
            .HasForeignKey(n => n.AthleteId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}