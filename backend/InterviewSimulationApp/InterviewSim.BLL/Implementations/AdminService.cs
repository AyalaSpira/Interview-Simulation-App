using InterviewSim.DAL.Repositories;
using InterviewSim.Shared.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; // Required for FirstOrDefaultAsync, OrderByDescending

namespace InterviewSim.BLL.Implementations
{
    public class AdminService
    {
        private readonly IUserRepository _userRepository;
        private readonly IInterviewRepository _interviewRepository;
        private readonly S3Service _s3Service;
        private readonly string _bucketName;

        public AdminService(IUserRepository userRepository, IInterviewRepository interviewRepository, S3Service s3Service, string bucketName)
        {
            _userRepository = userRepository;
            _interviewRepository = interviewRepository;
            _s3Service = s3Service;
            _bucketName = bucketName;
        }

        public async Task<string> LoginAdminAsync(string email, string password)
        {
            var admin = await _userRepository.GetAdminByCredentialsAsync(email, password);

            if (admin == null)
                throw new UnauthorizedAccessException("Email or password is incorrect.");

            return JwtHelper.GenerateAdminJwtToken(admin.Id, admin.Password, admin.Email, "Admin");
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                return false;

            await _userRepository.DeleteAsync(id);
            return true;
        }

        /// <summary>
        /// Retrieves the score (MARK=) from the summary of the user's last interview.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The score as a string, or null if no interview or score is found.</returns>
        public async Task<string> GetLastInterviewScoreAsync(int userId)
        {
            var lastInterview = await _interviewRepository.GetLastInterviewByUserIdAsync(userId);

            if (lastInterview == null || string.IsNullOrWhiteSpace(lastInterview.Summary))
            {
                return null; // No interview or summary is empty
            }

            var summary = lastInterview.Summary;
            const string markPrefix = "MARK=";
            var markIndex = summary.LastIndexOf(markPrefix);

            if (markIndex != -1)
            {
                var scoreCandidate = summary.Substring(markIndex + markPrefix.Length).Trim();

                // Assuming the score is a number or short string, and can be terminated by a space or newline
                var endIndex = scoreCandidate.IndexOfAny(new char[] { ' ', '\n', '\r' });
                if (endIndex != -1)
                {
                    scoreCandidate = scoreCandidate.Substring(0, endIndex);
                }

                return scoreCandidate;
            }

            return null; // "MARK=" not found in the summary
        }
    }
}