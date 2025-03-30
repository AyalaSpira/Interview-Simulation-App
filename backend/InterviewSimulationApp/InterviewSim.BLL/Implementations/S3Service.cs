using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using ServiceStack.Text;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

public class S3Service
{
    private readonly IAmazonS3 _s3Client;

    public S3Service(IConfiguration configuration)
    {
        Console.WriteLine("----------------------------------------------------------");
        var awsOptions = configuration.GetSection("AWS");

        var accessKey = Environment.GetEnvironmentVariable("AccessKey");
        var secretKey = Environment.GetEnvironmentVariable("SecretKey");
     
        var region= Environment.GetEnvironmentVariable("Region");
        if(region == null)
        {
            region = awsOptions["Region"];
            Console.WriteLine("נכנס לIF");
        }
        var _bucketName = Environment.GetEnvironmentVariable("BucketName");

        if (_bucketName == null)
        {
            _bucketName = awsOptions["BucketName"];
            Console.WriteLine("נכנס לIF");

        }
        Console.WriteLine("AccessKey", accessKey, "SecretKey", secretKey);
        Console.WriteLine("Region", region, "BucketName", _bucketName);
        _s3Client = new AmazonS3Client(accessKey, secretKey, Amazon.RegionEndpoint.GetBySystemName(region));
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

 
    public async Task<byte[]> DownloadFileAsByteArrayAsync(string bucketName, string key)
    {
        try
        {
            var request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = key
            };

            using (var response = await _s3Client.GetObjectAsync(request))
            using (var memoryStream = new MemoryStream())
            {
                await response.ResponseStream.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
        catch (Exception ex)
        {
            // אפשר להוסיף לוגים כדי לבדוק את השגיאה
            Console.WriteLine($"Error downloading file from S3: {ex.Message}");
            return null ; // מחזיר מערך ריק במקרה של שגיאה
        }
    }


    // מחיקת קובץ מ-S3 על פי URL
    public async Task<bool> DeleteFileByUrlAsync(string fileUrl, string bucketName)
    {
        try
        {
            // חילוץ ה-File Key מתוך ה-URL
            string fileKey = GetFileKeyFromUrl(fileUrl);

            // קריאה לפונקציה למחיקת הקובץ
            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = fileKey
            };

            var deleteResponse = await _s3Client.DeleteObjectAsync(deleteRequest);

            // אם המחיקה הצליחה
            if (deleteResponse.HttpStatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (AmazonS3Exception amazonS3Exception)
        {
            throw new Exception("There was an error deleting the file from S3: " + amazonS3Exception.Message, amazonS3Exception);
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred while deleting the file.", ex);
        }
    }

    // פונקציה לחילוץ ה-File Key מתוך ה-URL של הקובץ
    private string GetFileKeyFromUrl(string fileUrl)
    {
        // מניח שה-URL כולל את שם ה-Bucket וגם את ה-File Key בסופו
        var uri = new Uri(fileUrl);
        return uri.AbsolutePath.TrimStart('/'); // מקבל את ה-File Key מתוך ה-URL
    }

}

