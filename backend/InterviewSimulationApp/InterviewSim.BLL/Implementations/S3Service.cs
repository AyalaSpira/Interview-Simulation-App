using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

public class S3Service
{
    private readonly IAmazonS3 _s3Client;

    public S3Service(IAmazonS3 s3Client)
    {
        _s3Client = s3Client;
    }

    // העלאת קובץ ל-S3 עם פרטי Bucket
    public async Task<string> UploadFileAsync(IFormFile file, string bucketName)
    {
        try
        {
            // יצירת שם קובץ ייחודי על בסיס GUID וההרחבה של הקובץ
            var fileKey = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            using (var stream = file.OpenReadStream())
            {
                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = stream,
                    Key = fileKey,
                    BucketName = bucketName,
                    ContentType = file.ContentType
                };

                // יצירת אובייקט להעברת הקובץ ל-S3
                var fileTransferUtility = new TransferUtility(_s3Client);
                await fileTransferUtility.UploadAsync(uploadRequest); // העלאת הקובץ ל-S3
            }

            // מחזירים את ה-URL של הקובץ שהועלה ל-S3
            return $"https://{_s3Client.Config.RegionEndpoint.SystemName}.amazonaws.com/{bucketName}/{fileKey}";
        }
        catch (AmazonS3Exception amazonS3Exception)
        {
            // טיפול בשגיאות S3
            throw new Exception("There was an error uploading the file to S3: " + amazonS3Exception.Message, amazonS3Exception);
        }
        catch (Exception ex)
        {
            // טיפול בשגיאות כלליות
            throw new Exception("An unexpected error occurred while uploading the file.", ex);
        }
    }

    // מחיקת קובץ מ-S3
    public async Task DeleteFileAsync(string fileKey, string bucketName)
    {
        var deleteRequest = new Amazon.S3.Model.DeleteObjectRequest
        {
            BucketName = bucketName,
            Key = fileKey
        };

        await _s3Client.DeleteObjectAsync(deleteRequest);
    }
}
