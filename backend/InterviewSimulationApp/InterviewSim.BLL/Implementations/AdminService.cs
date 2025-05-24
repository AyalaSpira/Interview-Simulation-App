using InterviewSim.DAL.Repositories;
using InterviewSim.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            return JwtHelper.GenerateJwtToken(admin.Id, admin.Password, admin.Email);
        }


        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                return false;

            await _userRepository.DeleteAsync(id);
            return true;
        }



    }
}
