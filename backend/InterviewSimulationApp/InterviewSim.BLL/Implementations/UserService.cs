using InterviewSim.BLL.Interfaces;
using InterviewSim.Shared.DTOs;
using InterviewSim.DAL.Repositories;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.S3; // הוספת AWS S3
using Amazon.S3.Transfer;

namespace InterviewSim.BLL.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAIService _aiService;
        private readonly IAmazonS3 _s3Client; // הוספנו את שירות ה-S3 (אם אתה משתמש ב-AWS)

        public UserService(IUserRepository userRepository, IAIService aiService, IAmazonS3 s3Client)
        {
            _userRepository = userRepository;
            _aiService = aiService;
            _s3Client = s3Client; // העברת ה-S3 ל-UserService
        }

        public async Task<UserDTO> GetUserDetailsAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return new UserDTO { Username = user.Username, Password = user.Password };
        }


        public async Task UpdateUserResumeAsync(int userId, IFormFile resume)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (resume != null)
            {
                // העלאת קובץ קורות חיים ל-S3 ושמירת ה-URL
                var resumeUrl = await UploadFileToS3Async(resume);
                user.Resume = resumeUrl;
                await _userRepository.SaveAsync(user);
            }
        }

        public async Task<List<string>> GenerateQuestionsFromResumeAsync(string resumeText)
        {
            // יצירת שאלות על בסיס קורות החיים
            return await _aiService.GenerateQuestionsAsync(resumeText);
        }

        // פונקציה להעלאת קובץ ל-S3
        private async Task<string> UploadFileToS3Async(IFormFile file)
        {
            var fileTransferUtility = new TransferUtility(_s3Client);

            var fileKey = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            using (var stream = file.OpenReadStream())
            {
                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = stream,
                    Key = fileKey,
                    BucketName = "ayala-spira-testpnoren",
                    ContentType = file.ContentType
                };

                await fileTransferUtility.UploadAsync(uploadRequest);
            }

            return $"https://{_s3Client.Config.RegionEndpoint.SystemName}.amazonaws.com/ayala-spira-testpnoren/{fileKey}";
        }
    }
}
