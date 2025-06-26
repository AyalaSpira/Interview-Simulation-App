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
"Server=bffwvcdddr7vfdd4p741-mysql.services.clever-cloud.com;Port=3306;Database=bffwvcdddr7vfdd4p741;User=upjd37wpujsehb9v;Password=NCQzJ9fHtMUblJmMILBI;"
, new MySqlServerVersion(new Version(8, 0, 41)) // גרסה תואמת של MySQL
 );

        return new InterviewSimContext(optionsBuilder.Options);
    }
}
