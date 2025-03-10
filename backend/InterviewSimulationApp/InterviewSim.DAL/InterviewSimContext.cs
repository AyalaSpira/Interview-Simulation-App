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

    }
}
