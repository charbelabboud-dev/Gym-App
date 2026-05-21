using Microsoft.EntityFrameworkCore;
using GymApp.Core.Entities;
namespace GymApp.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                if (!optionsBuilder.IsConfigured)
                {
                    optionsBuilder.UseSqlServer("Server=localhost;Database=GymApp;Trusted_Connection=True;TrustServerCertificate=True",
                        b => b.MigrationsAssembly("GymApp.API"));
                }
            }


        // DbSets for all tables
        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<Dietitian> Dietitians { get; set; }
        public DbSet<MembershipPlan> MembershipPlans { get; set; }
        public DbSet<ClientMembership> ClientMemberships { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<WorkoutPlan> WorkoutPlans { get; set; }
        public DbSet<WorkoutExercise> WorkoutExercises { get; set; }
        public DbSet<DietPlan> DietPlans { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ProgressEntry> ProgressEntries { get; set; }
        public DbSet<ClientGoal> ClientGoals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ========================================
            // USER TABLE
            // ========================================
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.UserEmail).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.UserEmail).IsUnique();
                entity.Property(e => e.UserPassword).IsRequired().HasMaxLength(255);
                entity.Property(e => e.UserRole).IsRequired().HasMaxLength(20);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");
            });
            modelBuilder.Entity<ClientGoal>(entity =>
{
    entity.ToTable("ClientGoals");
    entity.HasKey(e => e.Id);
    
    entity.HasOne(e => e.Client)
        .WithMany()
        .HasForeignKey(e => e.ClientCode)
        .OnDelete(DeleteBehavior.Restrict);
});

            // ========================================
            // CLIENT TABLE
            // ========================================
            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("Clients");
                entity.HasKey(e => e.ClCode);
                entity.Property(e => e.ClCode).HasMaxLength(5);
                entity.Property(e => e.ClFname).IsRequired().HasMaxLength(20);
                entity.Property(e => e.ClLname).IsRequired().HasMaxLength(20);
                entity.Property(e => e.ClPhone).HasMaxLength(15);
                entity.Property(e => e.ClAddress).HasMaxLength(100);
                entity.Property(e => e.ClRegisterDate).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.User)
                    .WithOne(u => u.Client)
                    .HasForeignKey<Client>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Coach)
                    .WithMany(c => c.Clients)
                    .HasForeignKey(e => e.ClCoachId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Membership)
                    .WithMany(m => m.Clients)
                    .HasForeignKey(e => e.ClMembershipId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Dietitian)
                    .WithMany(d => d.Clients)
                    .HasForeignKey(e => e.DietitianId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

modelBuilder.Entity<ProgressEntry>(entity =>
{
    entity.ToTable("ProgressEntries");
    entity.HasKey(e => e.Id);
    entity.Property(e => e.EntryDate).HasDefaultValueSql("GETDATE()");
    
    entity.HasOne(e => e.Client)
        .WithMany(c => c.ProgressEntries)
        .HasForeignKey(e => e.ClientId)
        .HasPrincipalKey(c => c.UserId)
        .OnDelete(DeleteBehavior.Restrict);
});
            // ========================================
            // COACH TABLE
            // ========================================
            modelBuilder.Entity<Coach>(entity =>
            {
                entity.ToTable("Coaches");
                entity.HasKey(e => e.CoCode);
                entity.Property(e => e.CoCode).HasMaxLength(5);
                entity.Property(e => e.CoFname).IsRequired().HasMaxLength(20);
                entity.Property(e => e.CoLname).IsRequired().HasMaxLength(20);
                entity.Property(e => e.CoPhone).HasMaxLength(15);
                entity.Property(e => e.CoEmail).HasMaxLength(50);
                entity.Property(e => e.CoAddress).HasMaxLength(100);
                entity.Property(e => e.CoSpecialty).HasMaxLength(50);

                entity.HasOne(e => e.User)
                    .WithOne(u => u.Coach)
                    .HasForeignKey<Coach>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ========================================
            // DIETITIAN TABLE
            // ========================================
            modelBuilder.Entity<Dietitian>(entity =>
            {
                entity.ToTable("Dietitians");
                entity.HasKey(e => e.DietCode);
                entity.Property(e => e.DietCode).HasMaxLength(5);
                entity.Property(e => e.DietFname).IsRequired().HasMaxLength(20);
                entity.Property(e => e.DietLname).IsRequired().HasMaxLength(20);
                entity.Property(e => e.DietEmail).HasMaxLength(50);
                entity.Property(e => e.DietPhone).HasMaxLength(15);

                entity.HasOne(e => e.User)
                    .WithOne(u => u.Dietitian)
                    .HasForeignKey<Dietitian>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ========================================
            // MEMBERSHIP PLAN TABLE
            // ========================================
            modelBuilder.Entity<MembershipPlan>(entity =>
            {
                entity.ToTable("MembershipPlans");
                entity.HasKey(e => e.MemId);
                entity.Property(e => e.MemName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.MemPrice).HasPrecision(10, 2);
                entity.Property(e => e.MemDescription).HasMaxLength(200);
            });

            // ========================================
            // CLIENT MEMBERSHIP TABLE
            // ========================================
            modelBuilder.Entity<ClientMembership>(entity =>
            {
                entity.ToTable("ClientMemberships");
                entity.HasKey(e => e.CmId);
                entity.Property(e => e.Status).HasMaxLength(20);

                entity.HasOne(e => e.Client)
                    .WithMany(c => c.ClientMemberships)
                    .HasForeignKey(e => e.ClCode)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.MembershipPlan)
                    .WithMany(m => m.ClientMemberships)
                    .HasForeignKey(e => e.MemId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ========================================
            // PAYMENT TABLE
            // ========================================
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payments");
                entity.HasKey(e => e.PayId);
                entity.Property(e => e.PayAmount).HasPrecision(10, 2);
                entity.Property(e => e.PayMethod).HasMaxLength(20);
                entity.Property(e => e.PayStatus).HasMaxLength(20);
                entity.Property(e => e.PayDate).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Client)
                    .WithMany(c => c.Payments)
                    .HasForeignKey(e => e.ClCode)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.MembershipPlan)
                    .WithMany(m => m.Payments)
                    .HasForeignKey(e => e.MemId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ========================================
            // SESSION TABLE
            // ========================================
            modelBuilder.Entity<Session>(entity =>
            {
                entity.ToTable("Sessions");
                entity.HasKey(e => e.SesId);
                entity.Property(e => e.SesType).IsRequired().HasMaxLength(20);
                entity.Property(e => e.SesDescription).HasMaxLength(100);
                entity.Property(e => e.SesStatus).IsRequired().HasMaxLength(20).HasDefaultValue("Scheduled");

                entity.HasOne(e => e.Client)
                    .WithMany(c => c.Sessions)
                    .HasForeignKey(e => e.SesClCode)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Coach)
                    .WithMany(c => c.Sessions)
                    .HasForeignKey(e => e.SesCoCode)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ========================================
            // ATTENDANCE TABLE
            // ========================================
            modelBuilder.Entity<Attendance>(entity =>
            {
                entity.ToTable("Attendances");
                entity.HasKey(e => e.AttId);
                entity.Property(e => e.CheckInTime).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Client)
                    .WithMany(c => c.Attendances)
                    .HasForeignKey(e => e.ClCode)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ========================================
            // WORKOUT PLAN TABLE
            // ========================================
            modelBuilder.Entity<WorkoutPlan>(entity =>
            {
                entity.ToTable("WorkoutPlans");
                entity.HasKey(e => e.WpId);
                entity.Property(e => e.WpName).IsRequired().HasMaxLength(50);

                entity.HasOne(e => e.Client)
                    .WithMany(c => c.WorkoutPlans)
                    .HasForeignKey(e => e.ClCode)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Coach)
                    .WithMany(c => c.WorkoutPlans)
                    .HasForeignKey(e => e.CoCode)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ========================================
            // WORKOUT EXERCISE TABLE
            // ========================================
            modelBuilder.Entity<WorkoutExercise>(entity =>
            {
                entity.ToTable("WorkoutExercises");
                entity.HasKey(e => e.WeId);
                entity.Property(e => e.ExerciseName).IsRequired().HasMaxLength(50);

                entity.HasOne(e => e.WorkoutPlan)
                    .WithMany(w => w.WorkoutExercises)
                    .HasForeignKey(e => e.WpId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ========================================
            // DIET PLAN TABLE
            // ========================================
            modelBuilder.Entity<DietPlan>(entity =>
            {
                entity.ToTable("DietPlans");
                entity.HasKey(e => e.DietId);
                entity.Property(e => e.DietDescription).IsRequired().HasMaxLength(200);

                entity.HasOne(e => e.Client)
                    .WithMany(c => c.DietPlans)
                    .HasForeignKey(e => e.ClCode)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Dietitian)
                    .WithMany(d => d.DietPlans)
                    .HasForeignKey(e => e.DietitianId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ========================================
            // NOTIFICATION TABLE
            // ========================================
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("Notifications");
                entity.HasKey(e => e.NotId);
                entity.Property(e => e.Message).IsRequired().HasMaxLength(200);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.User)
                    .WithMany(u => u.Notifications)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ========================================
            // REVIEW TABLE
            // ========================================
            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("Reviews");
                entity.HasKey(e => e.RevId);
                entity.Property(e => e.Rating).IsRequired();
                entity.Property(e => e.Comment).HasMaxLength(200);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Client)
                    .WithMany(c => c.Reviews)
                    .HasForeignKey(e => e.ClCode)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Coach)
                    .WithMany(c => c.Reviews)
                    .HasForeignKey(e => e.CoCode)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}