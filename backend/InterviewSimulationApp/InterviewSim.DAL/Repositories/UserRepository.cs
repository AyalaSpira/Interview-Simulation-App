using InterviewSim.DAL;
using InterviewSim.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class UserRepository : IUserRepository
{
    private readonly InterviewSimContext _context;

    public UserRepository(InterviewSimContext context)
    {
        _context = context;
    }

    public async Task SaveAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    // מימוש המתודה שמחזירה את פרטי המשתמש על פי מזהה
    public async Task<User> GetUserDetailsAsync(int userId)
    {
        return await _context.Users
            .Where(u => u.UserId == userId)
            .FirstOrDefaultAsync();
    }

    // מימוש המתודה שמחזירה את המשתמש על פי שם המשתמש
    public async Task<User> GetUserByUsernameAsync(string username)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    // מימוש המתודה שמחזירה את המשתמש על פי מזהה המשתמש
    public async Task<User> GetUserByIdAsync(int userId)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.UserId == userId);
    }

    // מימוש המתודה שמחזירה את המשתמש על פי מזהה המשתמש
    public async Task<User> GetById(int userId)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.UserId == userId);
    }

    public Task AddUserAsync(User newUser)
    {
        throw new NotImplementedException();
    }
}
