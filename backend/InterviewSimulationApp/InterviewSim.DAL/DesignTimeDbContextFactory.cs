using InterviewSim.DAL;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;  // הוספת הפנייה זו

public class CustomDesignTimeDbContextFactory : IDesignTimeDbContextFactory<InterviewSimContext>
{
    public InterviewSimContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<InterviewSimContext>();

        // עדכון לגרסה 9.0 ול-MySQL 8.0.41
        optionsBuilder.UseMySql(
     "server=localhost;port=3306;database=new;user=root;password=AY133244;",  // עדכון הנתיב ל-MySQL
     new MySqlServerVersion(new Version(8, 0, 41)) // גרסה תואמת של MySQL
 );

        return new InterviewSimContext(optionsBuilder.Options);
    }
}
