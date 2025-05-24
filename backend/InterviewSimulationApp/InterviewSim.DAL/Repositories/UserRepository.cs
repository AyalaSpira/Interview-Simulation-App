using InterviewSim.DAL.Entities;
using InterviewSim.DAL;
using InterviewSim.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InterviewSim.BLL.Helpers;

public class UserRepository : IUserRepository
{
    private readonly InterviewSimContext _context;

    public UserRepository(InterviewSimContext context)
    {
        _context = context;
    }

    public async Task<UserDTO> GetUserByIdAsync(int userId)
    {
        var user = await _context.Users
                        .FirstOrDefaultAsync(u => u.UserId == userId);

        return user == null ? null : new UserDTO
        {
            UserId = user.UserId,
            ResumePath = user.ResumePath,
            Email = user.Email,
            Name = user.Username // משתמש ב-Username של User
        };
    }

    public async Task<UserDTO> GetUserByEmailAsync(string email)
    {
        Console.WriteLine($"🔍 מחפש משתמש עם אימייל: {email}");

        var user = await _context.Users
            .AsNoTracking() // שיפור ביצועים - לא עוקב אחרי אובייקט
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user is null)
        {
            Console.WriteLine("⚠️ משתמש לא נמצא.");
            return null;
        }

        Console.WriteLine($"✅ משתמש נמצא: {user.Email}");

        return new UserDTO
        {
            UserId = user.UserId,
            ResumePath = user.ResumePath,
            Email = user.Email,
            Name = user.Username
        };
    }

    public async Task<UserDTO> GetUserByIdAndEmailAsync(int userId, string email)
    {
        var user = await _context.Users
                             .FirstOrDefaultAsync(u => u.UserId == userId && u.Email == email);

        return user == null ? null : new UserDTO
        {
            UserId = user.UserId,
            ResumePath = user.ResumePath,
            Email = user.Email,
            Name = user.Username
        };
    }

   
    public async Task<User> GetUserEntityByEmailAsync(string email)
    {
        var a= await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (a != null)
        { Console.WriteLine(a.UserId +" +"+ a.Email +"GetUserEntityByEmailAsync"); }
        return a;
    }
    public async Task SaveAsync(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "User cannot be null.");
        }

        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(UserDTO userDTO)
    {
        if (userDTO == null)
        {
            throw new ArgumentNullException(nameof(userDTO), "UserDTO cannot be null.");
        }

        var user = await _context.Users.FindAsync(userDTO.UserId);
        if (user == null)
        {
            throw new Exception("User not found.");
        }

        // המרת UserDTO ל-User
        user.ResumePath = userDTO.ResumePath;
        user.Email = userDTO.Email;
        user.Username = userDTO.Name;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<UserDTO>> GetAllUsersAsync()
    {
        return await _context.Users
            .Select(user => new UserDTO
            {
                UserId = user.UserId,
                ResumePath = user.ResumePath,
                Email = user.Email,
                Name = user.Username
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<string>> GetAllResumeUrlsAsync()
    {
        return await _context.Users
            .Where(u => u.ResumePath != null)
            .Select(r => r.ResumePath)
            .ToListAsync();
    }

    public async Task<IEnumerable<string>> GetAllReportUrlsAsync()
    {
        return await _context.Interviews
            .Where(i => i.Summary != null)
            .Select(r => r.Summary)
            .ToListAsync();
    }

    public async Task UpdateResumeUrlToNullAsync(string fileUrl)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.ResumePath == fileUrl);
        if (user != null)
        {
            user.ResumePath = null;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Admin> GetAdminByCredentialsAsync(string email, string password)
    {
        var admin = await _context.Admins
            .FirstOrDefaultAsync(u => u.Email == email); // נניח שיש שדה IsAdmin

        if (admin == null) return null;

        if (admin.Password == password)
            return admin;

        return null;
    }


  


    public async Task AddUserAsync(User newUser)
    {
        if (newUser == null)
            throw new ArgumentNullException(nameof(newUser));

        newUser.Password = PasswordHelper.HashPassword(newUser.Password);
        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(User user)
    {
        var existingUser = await _context.Users.FindAsync(user.UserId);
        if (existingUser == null)
            throw new InvalidOperationException("User not found.");

        existingUser.Username = user.Username;
        existingUser.Email = user.Email;
        existingUser.ResumePath = user.ResumePath;
        // לא משנים סיסמה אלא אם ממש צריך

        _context.Users.Update(existingUser);
        await _context.SaveChangesAsync();
    }

}
