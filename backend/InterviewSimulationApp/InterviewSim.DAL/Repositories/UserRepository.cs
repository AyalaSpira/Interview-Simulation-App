using InterviewSim.DAL;
using InterviewSim.DAL.Entities;

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

    public Task GetUserDetailsAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public User GetUserByIdAsync(int userId)
    {
        throw new NotImplementedException();
    }
}