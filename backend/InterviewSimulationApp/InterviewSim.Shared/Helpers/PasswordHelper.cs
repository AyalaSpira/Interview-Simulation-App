using BCrypt.Net;

namespace InterviewSim.BLL.Helpers
{
    public static class PasswordHelper
    {
        private const int WorkFactor = 12; // WorkFactor ���� Bcrypt

        public static string HashPassword(string password)
        {
            // ���� �� ����� ������ ����� �� WorkFactor
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
            // �� �-salt ���� �� ���� �� ���� �����, ���� ����� Hash ��� �� WorkFactor
            return BCrypt.Net.BCrypt.HashPassword(oldHashedPassword, WorkFactor);
        }
    }
}
