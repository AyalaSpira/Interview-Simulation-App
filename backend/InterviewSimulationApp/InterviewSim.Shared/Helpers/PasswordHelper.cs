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
            Console.WriteLine("--------- Start VerifyPassword ---------");
            Console.WriteLine("Input password: " + password);
            Console.WriteLine("Input hashedPassword: " + hashedPassword);

            if (string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("❌ password is null or empty");
                return false;
            }

            if (string.IsNullOrWhiteSpace(hashedPassword))
            {
                Console.WriteLine("❌ hashedPassword is null or empty");
                return false;
            }

            Console.WriteLine("🔎 Checking hashedPassword format...");
            Console.WriteLine("HashedPassword length: " + hashedPassword.Length);
            Console.WriteLine("HashedPassword starts with: " + hashedPassword.Substring(0, Math.Min(6, hashedPassword.Length)));

            try
            {
                Console.WriteLine("✅ Trying to verify password with hashedPassword...");
                bool result = BCrypt.Net.BCrypt.Verify(password, hashedPassword);
                Console.WriteLine("✅ Verification result: " + result);
                return result;
            }
            catch (BCrypt.Net.SaltParseException ex)
            {
                Console.WriteLine("❗ Caught SaltParseException!");
                Console.WriteLine("❗ Exception message: " + ex.Message);
                Console.WriteLine("⚠️ Not verifying against a newly hashed password – security risk");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("❗ General exception during verification:");
                Console.WriteLine("❗ Exception: " + ex.GetType().Name);
                Console.WriteLine("❗ Message: " + ex.Message);
                return false;
            }
        }


        public static string UpgradePasswordHash(string oldHashedPassword)
        {
            // אם ה-salt הישן לא תואם או נדרש שדרוג, ניתן ליצור Hash חדש עם WorkFactor
            return BCrypt.Net.BCrypt.HashPassword(oldHashedPassword, WorkFactor);
        }
    }
}
