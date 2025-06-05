//using InterviewSim.BLL.Interfaces;
//using InterviewSim.DAL.Entities;
//using InterviewSim.DAL.Repositories;
//using InterviewSim.Shared.DTOs;
//using InterviewSim.Shared.Helpers;
//using Microsoft.AspNetCore.Http;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//public class UserService : IUserService
//{
//    private readonly IUserRepository _userRepository;
//    private readonly IInterviewRepository _interviewRepository;
//    private readonly S3Service _s3Service;
//    private readonly string _bucketName;

//    public UserService(IUserRepository userRepository, IInterviewRepository interviewRepository, S3Service s3Service, string bucketName)
//    {
//        _userRepository = userRepository;
//        _interviewRepository = interviewRepository;
//        _s3Service = s3Service;
//        _bucketName = bucketName;
//    }

//    public async Task<UserDTO> GetUserByEmailAsync(string email)
//    {
//        var user = await _userRepository.GetUserByEmailAsync(email); // חיפוש לפי Email

//        if (user == null)
//        {
//            throw new UnauthorizedAccessException("User not found.");
//        }

//        return new UserDTO
//        {
//            UserId = user.UserId,
//            ResumePath = user.ResumePath,
//            Email = user.Email,
//            Name = user.Name
//        };
//    }

//    public async Task<UserDTO> GetUserByIdAndEmailAsync(int userId, string email)
//    {
//        var user = await _userRepository.GetUserByIdAndEmailAsync(userId, email); // חיפוש לפי ID ו-Email

//        if (user == null)
//        {
//            throw new UnauthorizedAccessException("User not found.");
//        }

//        return new UserDTO
//        {
//            UserId = user.UserId,
//            ResumePath = user.ResumePath,
//            Email = user.Email,
//            Name = user.Name
//        };
//    }

//    public async Task<UserDTO> GetUserByIdAsync(int userId)
//    {
//        var user = await _userRepository.GetUserByIdAsync(userId); // חיפוש לפי ID

//        if (user == null)
//        {
//            throw new UnauthorizedAccessException("User not found.");
//        }

//        return new UserDTO
//        {
//            UserId = user.UserId,
//            ResumePath = user.ResumePath,
//            Email = user.Email,
//            Name = user.Name
//        };
//    }

//    public async Task UpdateUserResumeAsync(int userId, IFormFile resume)
//    {
//        var user = await _userRepository.GetUserByIdAsync(userId);

//        if (user != null && resume != null)
//        {
//            if (!string.IsNullOrEmpty(user.ResumePath))
//            {
//                var oldFileKey = user.ResumePath.Substring(user.ResumePath.LastIndexOf("/") + 1);
//                await _s3Service.DeleteFileByUrlAsync(oldFileKey, _bucketName);
//            }

//            var resumeUrl = await _s3Service.UploadFileAsync(resume, _bucketName);
//            user.ResumePath = resumeUrl;
//            await _userRepository.UpdateUserAsync(user);
//        }
//    }

//    public async Task<List<UserDTO>> GetAllUsersAsync()
//    {
//        var users = await _userRepository.GetAllUsersAsync();
//        var userDtos = new List<UserDTO>();

//        foreach (var user in users)
//        {
//            userDtos.Add(new UserDTO
//            {
//                UserId = user.UserId,
//                ResumePath = user.ResumePath,
//                Email = user.Email,
//                Name = user.Name
//            });
//        }

//        return userDtos;
//    }

//    public async Task UpdateUserAsync(UserDTO userDto)
//    {
//        // חיפוש משתמש לפי ID ו-Email
//        var user = await _userRepository.GetUserByIdAndEmailAsync(userDto.UserId, userDto.Email);

//        if (user != null)
//        {
//            // המרת UserDTO ל-User
//            user.UserId = userDto.UserId;
//            user.ResumePath = userDto.ResumePath;
//            user.Email = userDto.Email;
//            user.Name = userDto.Name; // עדכון שם המשתמש ב-Username של User

//            // עדכון הנתונים במסד
//            await _userRepository.UpdateUserAsync(user);
//        }
//    }


//    public async Task<bool> DeleteUserAsync(int id)
//    {
//        var user = await _userRepository.GetUserByIdAsync(id);
//        if (user == null)
//            return false;

//        await _userRepository.DeleteAsync(id);
//        return true;
//    }

//    public async Task<IEnumerable<string>> GetAllResumeUrlsAsync()
//    {
//        return await _userRepository.GetAllResumeUrlsAsync();
//    }

//    public async Task<IEnumerable<string>> GetAllReportUrlsAsync()
//    {
//        return await _userRepository.GetAllReportUrlsAsync();
//    }

//    public async Task<bool> DeleteFileAsync(string fileUrl, string fileType, int? interviewId = null)
//    {
//        bool result = await _s3Service.DeleteFileByUrlAsync(fileUrl, _bucketName);

//        if (result)
//        {
//            if (fileType == "resume")
//            {
//                await _userRepository.UpdateResumeUrlToNullAsync(fileUrl);
//            }
//            if (fileType == "report")
//            {
//                await _interviewRepository.UpdateReportToNullAsync(fileUrl);
//            }
//        }
//        return result;
//    }

//    public async Task<string> LoginAdminAsync(string email, string password)
//    {
//        var admin = await _userRepository.GetAdminByCredentialsAsync(email, password);

//        if (admin == null)
//            throw new UnauthorizedAccessException("Email or password is incorrect.");

//        return JwtHelper.GenerateJwtToken(admin.Id, admin.Password, admin.Email);
//    }


//    public async Task AddUserAsync(UserDTO userDto)
//    {
//        var user = new User
//        {
//            Username = userDto.Name,
//            Email = userDto.Email,
//            ResumePath = userDto.ResumePath ?? string.Empty,
//            Password = "123456" // או קלט אחר או מחולל סיסמאות
//        };

//        await _userRepository.AddUserAsync(user);
//    }

//    public async Task UpdateUserByAdminAsync(UserDTO userDto)
//    {
//        var existingUser = await _userRepository.GetUserByIdAsync(userDto.UserId);
//        if (existingUser == null)
//            throw new InvalidOperationException("User not found.");

//        existingUser.Name = userDto.Name;
//        existingUser.Email = userDto.Email;
//        existingUser.ResumePath = userDto.ResumePath;

//        await _userRepository.UpdateUserAsync(existingUser);
//    }


//}

using InterviewSim.BLL.Helpers;
using InterviewSim.BLL.Interfaces;
using InterviewSim.DAL.Entities;
using InterviewSim.DAL.Repositories;
using InterviewSim.Shared.DTOs;
using InterviewSim.Shared.Helpers; // עבור PasswordHelper
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IInterviewRepository _interviewRepository;
    private readonly S3Service _s3Service;
    private readonly string _bucketName;

    public UserService(IUserRepository userRepository, IInterviewRepository interviewRepository, S3Service s3Service, string bucketName)
    {
        _userRepository = userRepository;
        _interviewRepository = interviewRepository;
        _s3Service = s3Service;
        _bucketName = bucketName;
    }

    public async Task<UserDTO> GetUserByEmailAsync(string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);

        if (user == null)
        {
            throw new UnauthorizedAccessException("User not found.");
        }

        return user; // GetUserByEmailAsync כבר מחזיר DTO
    }

    public async Task<UserDTO> GetUserByIdAndEmailAsync(int userId, string email)
    {
        var user = await _userRepository.GetUserByIdAndEmailAsync(userId, email);

        if (user == null)
        {
            throw new UnauthorizedAccessException("User not found.");
        }

        return user; // GetUserByIdAndEmailAsync כבר מחזיר DTO
    }

    public async Task<UserDTO> GetUserByIdAsync(int userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);

        if (user == null)
        {
            throw new UnauthorizedAccessException("User not found.");
        }

        return user; // GetUserByIdAsync כבר מחזיר DTO
    }

    public async Task UpdateUserResumeAsync(int userId, IFormFile resume)
    {
        // הפונקציה הזו כנראה לעדכון קורות חיים של המשתמש עצמו
        // ולא דרך ממשק האדמין. היא נראית תקינה.
        var userDto = await _userRepository.GetUserByIdAsync(userId); // קבלת DTO
        if (userDto == null) throw new InvalidOperationException("User not found.");

        var userEntity = new User
        {
            UserId = userDto.UserId,
            Username = userDto.Name,
            Email = userDto.Email,
            ResumePath = userDto.ResumePath
        };

        if (resume != null)
        {
            if (!string.IsNullOrEmpty(userEntity.ResumePath))
            {
                var oldFileKey = userEntity.ResumePath.Substring(userEntity.ResumePath.LastIndexOf("/") + 1);
                await _s3Service.DeleteFileByUrlAsync(oldFileKey, _bucketName);
            }

            var resumeUrl = await _s3Service.UploadFileAsync(resume, _bucketName);
            userEntity.ResumePath = resumeUrl;
        }
        await _userRepository.UpdateUserAsync(userEntity); // עדכון עם ה-Entity
    }

    public async Task<List<UserDTO>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllUsersAsync(); // Repository כבר מחזיר List<UserDTO>
    }

    // פונקציה זו כבר קיימת ב-UserRepository.
    // אם זו הכוונה שהיא תשמש לעדכון מ-AdminPanel, אז צריך לוודא שהיא מקבלת
    // את כל הנתונים הנדרשים (כולל סיסמה וקובץ).
    // נשתמש בפונקציה החדשה UpdateUserByAdminAsync במקום זו לעדכון אדמין.
    // את זו נשאיר לשימושים פנימיים אם ישנם.
    public async Task UpdateUserAsync(UserDTO userDto)
    {
        // קוד זה כרגע מעדכן DTO בלבד ולא מטפל בסיסמה או קובץ באופן ישיר.
        // עבור עדכון אדמין, נשתמש ב-UpdateUserByAdminAsync.
        // אם יש מקומות אחרים שקוראים לזה, זה בסדר.
        var userEntity = new User
        {
            UserId = userDto.UserId,
            Username = userDto.Name,
            Email = userDto.Email,
            ResumePath = userDto.ResumePath
        };
        await _userRepository.UpdateUserAsync(userEntity);
    }


    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null)
            return false;

        // מחיקת קורות חיים מ-S3 לפני מחיקת המשתמש
        if (!string.IsNullOrEmpty(user.ResumePath))
        {
            var fileKey = user.ResumePath.Substring(user.ResumePath.LastIndexOf("/") + 1);
            await _s3Service.DeleteFileByUrlAsync(fileKey, _bucketName);
        }

        await _userRepository.DeleteAsync(id);
        return true;
    }

    public async Task<IEnumerable<string>> GetAllResumeUrlsAsync()
    {
        return await _userRepository.GetAllResumeUrlsAsync();
    }

    public async Task<IEnumerable<string>> GetAllReportUrlsAsync()
    {
        return await _userRepository.GetAllReportUrlsAsync();
    }

    public async Task<bool> DeleteFileAsync(string fileUrl, string fileType, int? interviewId = null)
    {
        bool result = await _s3Service.DeleteFileByUrlAsync(fileUrl, _bucketName);

        if (result)
        {
            if (fileType == "resume")
            {
                await _userRepository.UpdateResumeUrlToNullAsync(fileUrl);
            }
            if (fileType == "report")
            {
                await _interviewRepository.UpdateReportToNullAsync(fileUrl);
            }
        }
        return result;
    }

    public async Task<string> LoginAdminAsync(string email, string password)
    {
        var admin = await _userRepository.GetAdminByCredentialsAsync(email, password);

        if (admin == null)
            throw new UnauthorizedAccessException("Email or password is incorrect.");

        return JwtHelper.GenerateJwtToken(admin.Id, admin.Password, admin.Email);
    }

    // הוספה: פונקציה להוספת משתמש דרך האדמין עם סיסמה וקובץ קורות חיים
    // זו הפונקציה ששונתה והותאמה ל-AddUserWithResumeAsync
    //public async Task AddUserWithResumeAsync(string name, string email, string password, IFormFile resumeFile)
    //{
    //    var existingUser = await _userRepository.GetUserEntityByEmailAsync(email); // קבלת ישות, לא DTO
    //    if (existingUser != null)
    //    {
    //        throw new InvalidOperationException("User with this email already exists.");
    //    }

    //    string resumePath = string.Empty;
    //    if (resumeFile != null)
    //    {
    //        resumePath = await _s3Service.UploadFileAsync(resumeFile, _bucketName);
    //    }

    //    var newUser = new User
    //    {
    //        Username = name,
    //        Email = email,
    //        Password = PasswordHelper.HashPassword(password), // סיסמה מגולמת
    //        ResumePath = resumePath
    //    };

    //    await _userRepository.AddUserAsync(newUser);
    //}

    //// הוספה: פונקציה לעדכון משתמש דרך האדמין (שם, אימייל, סיסמה אופציונלית, קורות חיים חדשים אופציונליים)
    //// זו הפונקציה ששונתה והותאמה ל-UpdateUserByAdminAsync
    //public async Task UpdateUserByAdminAsync(int userId, string name, string email, string? password = null, IFormFile? newResumeFile = null)
    //{
    //    var existingUser = await _userRepository.GetUserByIdAsync(userId); // קבלת DTO
    //    if (existingUser == null)
    //        throw new InvalidOperationException("User not found.");

    //    var userEntity = new User
    //    {
    //        UserId = existingUser.UserId,
    //        Username = name, // שם המשתמש
    //        Email = email,
    //        ResumePath = existingUser.ResumePath // נשמור את הנתיב הקיים כברירת מחדל
    //    };

    //    // עדכון סיסמה אם סופקה
    //    if (!string.IsNullOrEmpty(password))
    //    {
    //        userEntity.Password = PasswordHelper.HashPassword(password); // סיסמה מגולמת
    //    }
    //    else
    //    {
    //        // אם לא סופקה סיסמה חדשה, נשמור את הסיסמה הקיימת מהמסד
    //        var userFromDb = await _userRepository.GetUserEntityByEmailAsync(existingUser.Email);
    //        if (userFromDb != null)
    //        {
    //            userEntity.Password = userFromDb.Password;
    //        }
    //    }


    //    // טיפול בקורות חיים חדשים
    //    if (newResumeFile != null)
    //    {
    //        // מחיקת קובץ קורות חיים ישן אם קיים
    //        if (!string.IsNullOrEmpty(existingUser.ResumePath))
    //        {
    //            var oldFileKey = existingUser.ResumePath.Substring(existingUser.ResumePath.LastIndexOf("/") + 1);
    //            await _s3Service.DeleteFileByUrlAsync(oldFileKey, _bucketName);
    //        }
    //        // העלאת קובץ קורות חיים חדש
    //        userEntity.ResumePath = await _s3Service.UploadFileAsync(newResumeFile, _bucketName);
    //    }
    //    // אם לא סופק קובץ חדש, אבל רוצים למחוק את הקיים (תלוי בממשק המשתמש)
    //    // לדוגמה, אם הממשק מאפשר "ריקון" קורות חיים:
    //    // else if (shouldRemoveExistingResume) { userEntity.ResumePath = null; }


    //    await _userRepository.UpdateUserAsync(userEntity); // עדכון עם ה-Entity
    //}


    public async Task AddUserWithResumeAsync(string name, string email, string password, IFormFile? resumeFile)
    {
        // **בדיקת ייחודיות לאימייל בלבד**
        var existingUserByEmail = await _userRepository.GetUserEntityByEmailAsync(email);
        if (existingUserByEmail != null)
        {
            throw new InvalidOperationException("User with this email already exists.");
        }

        var newUser = new User
        {
            Username = name, // שם המשתמש לא חייב להיות ייחודי
            Email = email,
            Password = PasswordHelper.HashPassword(password),
            ResumePath = string.Empty
        };

        if (resumeFile != null)
        {
            var resumeUrl = await _s3Service.UploadFileAsync(resumeFile, _bucketName);
            newUser.ResumePath = resumeUrl;
        }

        await _userRepository.AddUserAsync(newUser);
    }

    public async Task UpdateUserByAdminAsync(int userId, string name, string email, string? password, IFormFile? newResumeFile)
    {
        var existingUserDTO = await _userRepository.GetUserByIdAsync(userId);
        if (existingUserDTO == null)
        {
            throw new InvalidOperationException("User not found.");
        }

        // **בדיקת ייחודיות לאימייל החדש בלבד (אם השתנה)**
        if (existingUserDTO.Email != email)
        {
            var userWithNewEmail = await _userRepository.GetUserEntityByEmailAsync(email);
            if (userWithNewEmail != null && userWithNewEmail.UserId != userId)
            {
                throw new InvalidOperationException("Another user with this email already exists.");
            }
        }

        // קבל את ה-Entity המקורי לעדכון
        // שים לב: GetUserEntityByEmailAsync אולי לא מספיק כאן אם האימייל משתנה והיה קיים משתמש עם האימייל הישן.
        // עדיף למצוא לפי ה-UserId ואז לעדכן.
        var userToUpdate = await _userRepository.GetUserByIdAsync(userId); // קבל את ה-UserDTO
        if (userToUpdate == null) // קייס שולי אבל חשוב
        {
            throw new InvalidOperationException("Error retrieving user entity for update.");
        }

        // נמיר ל-User Entity מלא כדי לעדכן דרך הריפוזיטורי
        var userEntity = await _userRepository.GetUserEntityByEmailAsync(existingUserDTO.Email) // או GetUserEntityByIdAsync אם הייתה כזו
                          ?? new User { UserId = existingUserDTO.UserId }; // אם לא נמצאה ישות, צור חדשה עם ה-ID

        // עדכן את השדות ב-Entity
        userEntity.Username = name;
        userEntity.Email = email;

        if (!string.IsNullOrEmpty(password))
        {
            userEntity.Password = PasswordHelper.HashPassword(password);
        }

        if (newResumeFile != null)
        {
            if (!string.IsNullOrEmpty(userEntity.ResumePath))
            {
                var oldFileKey = userEntity.ResumePath.Substring(userEntity.ResumePath.LastIndexOf("/") + 1);
                await _s3Service.DeleteFileByUrlAsync(oldFileKey, _bucketName);
            }
            var newResumeUrl = await _s3Service.UploadFileAsync(newResumeFile, _bucketName);
            userEntity.ResumePath = newResumeUrl;
        }

        await _userRepository.UpdateUserAsync(userEntity);
    }
}
