using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }
        public DbSet<University> Universities { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Advisor> Advisors { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<InternshipApplication> InternshipApplications { get; set; }
        public DbSet<InternshipPlace> InternshipPlaces { get; set; }
        public DbSet<InternshipDiary> InternshipDiaries { get; set; }
        public DbSet<InternshipEvaluation> InternshipEvaluations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Department)
                .WithMany(d => d.Students)
                .HasForeignKey(s => s.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Advisor)
                .WithMany(a => a.Students)
                .HasForeignKey(s => s.AdvisorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Advisor>()
                .HasOne(a => a.Department)
                .WithMany(d => d.Advisors)
                .HasForeignKey(a => a.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);


        }

    }
}
    
