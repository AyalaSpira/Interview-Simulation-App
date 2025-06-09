using Microsoft.EntityFrameworkCore;
using InterviewSim.DAL.Entities;

namespace InterviewSim.DAL
{
    public class InterviewSimContext : DbContext
    {
        public InterviewSimContext(DbContextOptions<InterviewSimContext> options)
      : base(options)
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Interview> Interviews { get; set; }
        public DbSet<Answer> Answers { get; set; }

        public DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Interview>()
                .Property(i => i.Answers)
                .HasConversion(
                    v => string.Join("||", v),
                    v => v.Split("||", StringSplitOptions.None).ToList());

            modelBuilder.Entity<Interview>()
                .Property(i => i.Questions)
                .HasConversion(
                    v => string.Join("||", v),
                    v => v.Split("||", StringSplitOptions.None).ToList());

            modelBuilder.Entity<Interview>()
                .Property(i => i.Summary)
                .HasColumnType("TEXT"); // אם זה יכול להיות טקסט ארוך
        }

    }
}
