using Microsoft.EntityFrameworkCore;
using StudentManagementSystem_KhyatiVadgama_Domain.Entities;

namespace StudentManagementSystem_KhyatiVadgama_Infrastructure.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Student> Students => Set<Student>();
        public DbSet<Class> Classes => Set<Class>();
        public DbSet<StudentClass> StudentClasses => Set<StudentClass>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>(b =>
            {
                b.HasKey(x => x.Id);
                b.HasIndex(x => x.Email).IsUnique();
                b.HasIndex(x => x.PhoneNumber).IsUnique();
                b.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
                b.Property(x => x.LastName).HasMaxLength(100).IsRequired();
                b.Property(x => x.PhoneNumber).HasMaxLength(10).IsRequired();
                b.Property(x => x.Email).IsRequired();
            });

            modelBuilder.Entity<Class>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Name).HasMaxLength(200).IsRequired();
                b.Property(x => x.Description).HasMaxLength(100);
            });

            modelBuilder.Entity<StudentClass>(b =>
            {
                b.HasKey(sc => new { sc.StudentId, sc.ClassId });
                b.HasOne(sc => sc.Student).WithMany(s => s.StudentClasses).HasForeignKey(sc => sc.StudentId).OnDelete(DeleteBehavior.Cascade);
                b.HasOne(sc => sc.Class).WithMany(c => c.StudentClasses).HasForeignKey(sc => sc.ClassId).OnDelete(DeleteBehavior.Cascade);
                b.Property(sc => sc.EnrolledAt).HasDefaultValueSql("GETUTCDATE()");
            });
        }
    }
}
