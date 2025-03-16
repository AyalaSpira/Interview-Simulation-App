using InterviewSim.DAL.Entities;
using InterviewSim.BLL.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InterviewSim.DAL;

public class UserRepository : IUserRepository
{
    private readonly InterviewSimContext _context;

    public UserRepository(InterviewSimContext context)
    {
        _context = context;
    }

    // שמירת משתמש חדש במסד הנתונים
    public async Task SaveAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    // מימוש המתודה שמחזירה את המשתמש על פי שם המשתמש והסיסמה
    public async Task<User> GetUserByIdAsync(string username, string password)
    {
        // מחפשים את המשתמש לפי שם המשתמש
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user != null && PasswordHelper.VerifyPassword(password, user.Password))
        {
            // אם הסיסמה נכונה, מחזירים את המשתמש
            return user;
        }

        // אם לא נמצא או שהסיסמה לא נכונה
        return null;
    }

    // הוספת משתמש חדש למסד הנתונים
    public async Task AddUserAsync(User newUser)
    {
        if (newUser == null)
        {
            throw new ArgumentNullException(nameof(newUser), "User cannot be null.");
        }

        // הוספת המשתמש החדש למסד הנתונים
        await _context.Users.AddAsync(newUser);

        // שמירה של השינויים במסד הנתונים
        await _context.SaveChangesAsync();
    }

    // מחזיר את כל המשתמשים
    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> GetUserByIdAsync(int userId)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
    }

}
