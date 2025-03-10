using InterviewSim.DAL.Entities;
using System.Threading.Tasks;

public interface IUserRepository
{
    Task SaveAsync(User user);
    Task<User> GetByIdAsync(int id);
    Task<User> GetUserDetailsAsync(int userId);
    Task<User> GetUserByUsernameAsync(string username);
    Task AddUserAsync(User newUser);
}
