using Microsoft.EntityFrameworkCore;
using StudentManagementApp.DAL.Models;

namespace StudentManagementApp.DAL.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Class> Classes => Set<Class>();
    public DbSet<ClassSchedule> ClassSchedules => Set<ClassSchedule>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<Attendance> Attendances => Set<Attendance>();
    public DbSet<OtpCode> OtpCodes => Set<OtpCode>();
    
    // Quiz System
    public DbSet<Quiz> Quizzes => Set<Quiz>();
    public DbSet<QuizQuestion> QuizQuestions => Set<QuizQuestion>();
    public DbSet<QuizOption> QuizOptions => Set<QuizOption>();
    public DbSet<QuizResult> QuizResults => Set<QuizResult>();
    public DbSet<StudentAnswer> StudentAnswers => Set<StudentAnswer>();
    public DbSet<CourseProgress> CourseProgresses => Set<CourseProgress>();
    
    // Email System
    public DbSet<EmailOtp> EmailOtps => Set<EmailOtp>();
    public DbSet<EmailLog> EmailLogs => Set<EmailLog>();

    // Payment & Products
    public DbSet<PurchasableItem> PurchasableItems => Set<PurchasableItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<CartItem> CartItems => Set<CartItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Teacher nullable FK - tránh multiple cascade paths
        modelBuilder.Entity<Class>()
            .HasOne(c => c.Teacher)
            .WithMany(u => u.TeachingClasses)
            .HasForeignKey(c => c.TeacherId)
            .OnDelete(DeleteBehavior.SetNull);

        // Enrollment - Student: tránh multiple cascade paths
        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Student)
            .WithMany(u => u.Enrollments)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Enrollment unique (StudentId, ClassId)
        modelBuilder.Entity<Enrollment>()
            .HasIndex(e => new { e.StudentId, e.ClassId })
            .IsUnique();

        // Attendance - Student: tránh multiple cascade paths
        modelBuilder.Entity<Attendance>()
            .HasOne(a => a.Student)
            .WithMany(u => u.Attendances)
            .HasForeignKey(a => a.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Attendance unique (ClassId, StudentId, Date)
        modelBuilder.Entity<Attendance>()
            .HasIndex(a => new { a.ClassId, a.StudentId, a.Date })
            .IsUnique();

        // Quiz - Course relationship
        modelBuilder.Entity<Quiz>()
            .HasOne(q => q.Course)
            .WithMany(c => c.Quizzes)
            .HasForeignKey(q => q.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        // QuizQuestion - Quiz relationship
        modelBuilder.Entity<QuizQuestion>()
            .HasOne(q => q.Quiz)
            .WithMany(q => q.Questions)
            .HasForeignKey(q => q.QuizId)
            .OnDelete(DeleteBehavior.Cascade);

        // QuizOption - Question relationship
        modelBuilder.Entity<QuizOption>()
            .HasOne(o => o.Question)
            .WithMany(q => q.Options)
            .HasForeignKey(o => o.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        // QuizResult relationships
        modelBuilder.Entity<QuizResult>()
            .HasOne(r => r.Student)
            .WithMany(u => u.QuizResults)
            .HasForeignKey(r => r.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<QuizResult>()
            .HasOne(r => r.Quiz)
            .WithMany(q => q.Results)
            .HasForeignKey(r => r.QuizId)
            .OnDelete(DeleteBehavior.Cascade);

        // StudentAnswer relationships - Use Restrict to avoid multiple cascade paths
        modelBuilder.Entity<StudentAnswer>()
            .HasOne(a => a.QuizResult)
            .WithMany(r => r.StudentAnswers)
            .HasForeignKey(a => a.QuizResultId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StudentAnswer>()
            .HasOne(a => a.Question)
            .WithMany()
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        // CourseProgress unique (StudentId, CourseId)
        modelBuilder.Entity<CourseProgress>()
            .HasIndex(cp => new { cp.StudentId, cp.CourseId })
            .IsUnique();

        // EmailOtps - Index on Email + Type
        modelBuilder.Entity<EmailOtp>()
            .HasIndex(e => new { e.Email, e.Type });
    }
}
