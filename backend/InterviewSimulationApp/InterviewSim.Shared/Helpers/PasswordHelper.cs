using BCrypt.Net;

namespace InterviewSim.BLL.Helpers
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            // חוזר על יצירת הסיסמה החדשה 
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                // מנסה לאמת את הסיסמה
                return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            }
            catch (BCrypt.Net.SaltParseException)
            {
                // במידה ויש בעיה עם ה-Salt, שדרג את ההצפנה עם Salt חדש
                var newHashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
                return BCrypt.Net.BCrypt.Verify(password, newHashedPassword);
            }
        }

        public static string UpgradePasswordHash(string oldHashedPassword)
        {
            // אם ה-salt הישן לא תואם או נדרש שדרוג, ניתן ליצור Hash חדש
            // תחזיר את ההשמה החדשה
            return oldHashedPassword;
        }
    }
}
