using InterviewSim.DAL.Entities;

public interface IUserRepository
{
    Task SaveAsync(User user);
    Task<User> GetByIdAsync(int id);
    Task GetUserDetailsAsync(int userId);
    User GetUserByIdAsync(int userId);
}
