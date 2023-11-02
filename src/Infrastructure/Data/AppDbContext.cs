using oksei_fsot_api.src.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace oksei_fsot_api.src.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration config;

        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration config) : base(options)
        {
            this.config = config;
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<MarkModel> Marks { get; set; }
        public DbSet<CriterionModel> Criterions { get; set; }
        public DbSet<MarkLogModel> MarkLogs { get; set; }
        public DbSet<ReportTeacherModel> ReportTeachers { get; set; }
        public DbSet<EvaluatedAppraiserModel> EvaluatedAppraisers { get; set; }
        public DbSet<PremiumReportModel> PremiumReports { get; set; }
        public DbSet<OrganizationModel> Organizations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = config.GetConnectionString("Default");
            optionsBuilder.UseSqlite(connectionString);
            // optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EvaluatedAppraiserModel>()
                .HasKey(ua => new { ua.EvaluatedId, ua.AppraiserId });

            modelBuilder.Entity<EvaluatedAppraiserModel>()
                .HasOne(ua => ua.Evaluated)
                .WithMany(u => u.UserAppraisers)
                .HasForeignKey(ua => ua.EvaluatedId);

            modelBuilder.Entity<EvaluatedAppraiserModel>()
                .HasOne(ua => ua.Appraiser)
                .WithMany()
                .HasForeignKey(ua => ua.AppraiserId);

            base.OnModelCreating(modelBuilder);
        }
    }
}