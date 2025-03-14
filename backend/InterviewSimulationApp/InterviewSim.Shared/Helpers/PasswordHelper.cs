using BCrypt.Net;

namespace InterviewSim.BLL.Helpers
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            // ���� �� ����� ������ ����� 
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                // ���� ���� �� ������
                return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            }
            catch (BCrypt.Net.SaltParseException)
            {
                // ����� ��� ���� �� �-Salt, ���� �� ������ �� Salt ���
                var newHashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
                return BCrypt.Net.BCrypt.Verify(password, newHashedPassword);
            }
        }

        public static string UpgradePasswordHash(string oldHashedPassword)
        {
            // �� �-salt ���� �� ���� �� ���� �����, ���� ����� Hash ���
            // ����� �� ����� �����
            return oldHashedPassword;
        }
    }
}
