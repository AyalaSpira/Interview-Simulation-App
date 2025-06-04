//using InterviewSim.DAL.Entities;
//using InterviewSim.DAL;
//using InterviewSim.Shared.DTOs;
//using Microsoft.EntityFrameworkCore;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using InterviewSim.BLL.Helpers;

//public class UserRepository : IUserRepository
//{
//    private readonly InterviewSimContext _context;

//    public UserRepository(InterviewSimContext context)
//    {
//        _context = context;
//    }

//    public async Task<UserDTO> GetUserByIdAsync(int userId)
//    {
//        var user = await _context.Users
//                        .FirstOrDefaultAsync(u => u.UserId == userId);

//        return user == null ? null : new UserDTO
//        {
//            UserId = user.UserId,
//            ResumePath = user.ResumePath,
//            Email = user.Email,
//            Name = user.Username // משתמש ב-Username של User
//        };
//    }

//    public async Task<UserDTO> GetUserByEmailAsync(string email)
//    {
//        Console.WriteLine($"🔍 מחפש משתמש עם אימייל: {email}");

//        var user = await _context.Users
//            .AsNoTracking() // שיפור ביצועים - לא עוקב אחרי אובייקט
//            .FirstOrDefaultAsync(u => u.Email == email);

//        if (user is null)
//        {
//            Console.WriteLine("⚠️ משתמש לא נמצא.");
//            return null;
//        }

//        Console.WriteLine($"✅ משתמש נמצא: {user.Email}");

//        return new UserDTO
//        {
//            UserId = user.UserId,
//            ResumePath = user.ResumePath,
//            Email = user.Email,
//            Name = user.Username
//        };
//    }

//    public async Task<UserDTO> GetUserByIdAndEmailAsync(int userId, string email)
//    {
//        var user = await _context.Users
//                             .FirstOrDefaultAsync(u => u.UserId == userId && u.Email == email);

//        return user == null ? null : new UserDTO
//        {
//            UserId = user.UserId,
//            ResumePath = user.ResumePath,
//            Email = user.Email,
//            Name = user.Username
//        };
//    }


//    public async Task<User> GetUserEntityByEmailAsync(string email)
//    {
//        var a= await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
//        if (a != null)
//        { Console.WriteLine(a.UserId +" +"+ a.Email +"GetUserEntityByEmailAsync"); }
//        return a;
//    }
//    public async Task SaveAsync(User user)
//    {
//        if (user == null)
//        {
//            throw new ArgumentNullException(nameof(user), "User cannot be null.");
//        }

//        _context.Users.Update(user);
//        await _context.SaveChangesAsync();
//    }

//    public async Task UpdateUserAsync(UserDTO userDTO)
//    {
//        if (userDTO == null)
//        {
//            throw new ArgumentNullException(nameof(userDTO), "UserDTO cannot be null.");
//        }

//        var user = await _context.Users.FindAsync(userDTO.UserId);
//        if (user == null)
//        {
//            throw new Exception("User not found.");
//        }

//        // המרת UserDTO ל-User
//        user.ResumePath = userDTO.ResumePath;
//        user.Email = userDTO.Email;
//        user.Username = userDTO.Name;

//        _context.Users.Update(user);
//        await _context.SaveChangesAsync();
//    }

//    public async Task DeleteAsync(int userId)
//    {
//        var user = await _context.Users.FindAsync(userId);
//        if (user != null)
//        {
//            _context.Users.Remove(user);
//            await _context.SaveChangesAsync();
//        }
//    }

//    public async Task<List<UserDTO>> GetAllUsersAsync()
//    {
//        return await _context.Users
//            .Select(user => new UserDTO
//            {
//                UserId = user.UserId,
//                ResumePath = user.ResumePath,
//                Email = user.Email,
//                Name = user.Username
//            })
//            .ToListAsync();
//    }

//    public async Task<IEnumerable<string>> GetAllResumeUrlsAsync()
//    {
//        return await _context.Users
//            .Where(u => u.ResumePath != null)
//            .Select(r => r.ResumePath)
//            .ToListAsync();
//    }

//    public async Task<IEnumerable<string>> GetAllReportUrlsAsync()
//    {
//        return await _context.Interviews
//            .Where(i => i.Summary != null)
//            .Select(r => r.Summary)
//            .ToListAsync();
//    }

//    public async Task UpdateResumeUrlToNullAsync(string fileUrl)
//    {
//        var user = await _context.Users.FirstOrDefaultAsync(u => u.ResumePath == fileUrl);
//        if (user != null)
//        {
//            user.ResumePath = null;
//            await _context.SaveChangesAsync();
//        }
//    }

//    public async Task<Admin> GetAdminByCredentialsAsync(string email, string password)
//    {
//        var admin = await _context.Admins
//            .FirstOrDefaultAsync(u => u.Email == email); // נניח שיש שדה IsAdmin

//        if (admin == null) return null;

//        if (admin.Password == password)
//            return admin;

//        return null;
//    }





//    public async Task AddUserAsync(User newUser)
//    {
//        if (newUser == null)
//            throw new ArgumentNullException(nameof(newUser));

//        newUser.Password = PasswordHelper.HashPassword(newUser.Password);
//        await _context.Users.AddAsync(newUser);
//        await _context.SaveChangesAsync();
//    }

//    public async Task UpdateUserAsync(User user)
//    {
//        var existingUser = await _context.Users.FindAsync(user.UserId);
//        if (existingUser == null)
//            throw new InvalidOperationException("User not found.");

//        existingUser.Username = user.Username;
//        existingUser.Email = user.Email;
//        existingUser.ResumePath = user.ResumePath;
//        // לא משנים סיסמה אלא אם ממש צריך

//        _context.Users.Update(existingUser);
//        await _context.SaveChangesAsync();
//    }

//}


using InterviewSim.DAL.Entities;
using InterviewSim.DAL;
using InterviewSim.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InterviewSim.BLL.Helpers; // ודא שזה מיובא
using System; // עבור ArgumentNullException, InvalidOperationException

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
            Name = user.Username
        };
    }

    public async Task<UserDTO> GetUserByEmailAsync(string email)
    {
        Console.WriteLine($"🔍 מחפש משתמש עם אימייל: {email}");

        var user = await _context.Users
            .AsNoTracking()
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
        var a = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (a != null)
        { Console.WriteLine(a.UserId + " +" + a.Email + "GetUserEntityByEmailAsync"); }
        return a;
    }

    // פונקציית SaveAsync נראית כמו כפילות ל-UpdateUserAsync(User user)
    // אם זו אותה מטרה, עדיף למחוק אותה ולהשתמש רק ב-UpdateUserAsync(User user).
    // אם היא משמשת למטרה אחרת (לדוגמה, שמירת ישות חדשה), עליה להיות בעלת שם שונה.
    // אשאיר אותה כרגע, אך שים לב לכפילות הפוטנציאלית.
    public async Task SaveAsync(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "User cannot be null.");
        }

        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    // זו פונקציה שצריך לשנות/למחוק
    // היא מקבלת UserDTO ומעדכנת חלק מהשדות.
    // מכיוון שיש לנו UpdateUserAsync(User user) שיכולה לקבל את ה-Entity המלא,
    // עדיף להשתמש בה ולדאוג למיפוי ב-Service Layer.
    // אם אתה רוצה לשמור אותה, היא תצטרך לטפל גם בסיסמה ובקורות חיים.
    // עבור ניהול משתמשים ע"י אדמין, לא הייתי משתמש ב-DTO פה.
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

        user.ResumePath = userDTO.ResumePath;
        user.Email = userDTO.Email;
        user.Username = userDTO.Name; // שינוי ל-Username

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
            .FirstOrDefaultAsync(u => u.Email == email);

        if (admin == null) return null;

        if (admin.Password == password)
            return admin;

        return null;
    }

    // פונקציה זו טובה, היא מוסיפה משתמש חדש
    public async Task AddUserAsync(User newUser)
    {
        if (newUser == null)
            throw new ArgumentNullException(nameof(newUser));

        // ה-hashing של הסיסמה צריך לקרות בשכבת ה-Service או ה-Business Logic,
        // לא בשכבת ה-Repository. ה-Repository צריך רק לשמור את מה שהוא מקבל.
        // אם PasswordHelper הוא ב-BLL, אז זה בסדר.
        // אם לא, שקול להעביר את זה ל-UserService.
        newUser.Password = PasswordHelper.HashPassword(newUser.Password);
        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();
    }

    // פונקציה זו טובה לעדכון User Entity מלא
    public async Task UpdateUserAsync(User user) // זו הפונקציה המועדפת לעדכון User Entity
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        var existingUser = await _context.Users.FindAsync(user.UserId);
        if (existingUser == null)
            throw new InvalidOperationException("User not found.");

        existingUser.Username = user.Username;
        existingUser.Email = user.Email;
        existingUser.ResumePath = user.ResumePath;

        // אם רוצים לעדכן סיסמה, יש לטפל ב-hashing שלה כאן או בשכבת ה-Service
        // ולא לשנות אותה אם היא ריקה או לא נשלחה.
        // לדוגמה:
        // if (!string.IsNullOrEmpty(user.Password))
        // {
        //     existingUser.Password = PasswordHelper.HashPassword(user.Password);
        // }
        // אם הסיסמה מגיעה כבר מהושט, אז אין צורך ב-hashing נוסף.

        _context.Users.Update(existingUser);
        await _context.SaveChangesAsync();
    }
}
