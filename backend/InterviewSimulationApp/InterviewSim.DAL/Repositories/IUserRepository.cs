using InterviewSim.DAL.Entities;
using System.Threading.Tasks;

public interface IUserRepository
{

    Task SaveAsync(User user);
    Task<User> GetUserByIdAsync(string Username, string Password);
    Task AddUserAsync(User newUser);
    Task<List<User>> GetAllUsersAsync();

    Task<User> GetUserByIdAsync(int userId); // פונקציה חדשה לשליפה לפי userId



}
