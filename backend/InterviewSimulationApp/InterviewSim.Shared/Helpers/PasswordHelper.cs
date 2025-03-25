using BCrypt.Net;

namespace InterviewSim.BLL.Helpers
{
    public static class PasswordHelper
    {
        private const int WorkFactor = 12; // WorkFactor עבור Bcrypt

        public static string HashPassword(string password)
        {
            // חוזר על יצירת הסיסמה החדשה עם WorkFactor
            return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            }
            catch (BCrypt.Net.SaltParseException)
            {
                var newHashedPassword = BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
                return BCrypt.Net.BCrypt.Verify(password, newHashedPassword);
            }
        }

        public static string UpgradePasswordHash(string oldHashedPassword)
        {
            // אם ה-salt הישן לא תואם או נדרש שדרוג, ניתן ליצור Hash חדש עם WorkFactor
            return BCrypt.Net.BCrypt.HashPassword(oldHashedPassword, WorkFactor);
        }
    }
}
