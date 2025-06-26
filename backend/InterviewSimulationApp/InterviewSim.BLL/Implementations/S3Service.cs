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
        Console.WriteLine("S3Service: Constructor started.");
        var awsOptions = configuration.GetSection("AWS");

        var accessKey = Environment.GetEnvironmentVariable("AccessKey");
        var secretKey = Environment.GetEnvironmentVariable("SecretKey");

        var region = Environment.GetEnvironmentVariable("Region");
        if (region == null)
        {
            region = awsOptions["Region"];
            Console.WriteLine("S3Service: Region from app settings.");
        }
        var _bucketName = Environment.GetEnvironmentVariable("BucketName");

        if (_bucketName == null)
        {
            _bucketName = awsOptions["BucketName"];
            Console.WriteLine("S3Service: BucketName from app settings.");
        }
        Console.WriteLine($"S3Service: AccessKey (truncated): {(accessKey != null && accessKey.Length > 4 ? accessKey.Substring(0, 4) + "..." : "N/A")}");
        // לעולם אל תדפיס SecretKey בלוגים אמיתיים!
        Console.WriteLine($"S3Service: Region: {region}, BucketName: {_bucketName}");
        _s3Client = new AmazonS3Client(accessKey, secretKey, Amazon.RegionEndpoint.GetBySystemName(region));
        Console.WriteLine("S3Service: AmazonS3Client initialized.");
    }

    public async Task<string> UploadFileAsync(IFormFile file, string bucketName)
    {
        Console.WriteLine($"S3Service: Attempting to upload file '{file.FileName}' to bucket '{bucketName}'.");
        try
        {
            var fileKey = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            Console.WriteLine($"S3Service: Generated file key for upload: {fileKey}");

            using (var stream = file.OpenReadStream())
            {
                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = stream,
                    Key = fileKey,
                    BucketName = bucketName,
                    ContentType = file.ContentType
                };

                var fileTransferUtility = new TransferUtility(_s3Client);
                await fileTransferUtility.UploadAsync(uploadRequest);
            }

            string fileUrl = $"https://{_s3Client.Config.RegionEndpoint.SystemName}.amazonaws.com/{bucketName}/{fileKey}";
            Console.WriteLine($"S3Service: File uploaded successfully. URL: {fileUrl}");
            return fileUrl;
        }
        catch (AmazonS3Exception amazonS3Exception)
        {
            Console.Error.WriteLine($"S3Service ERROR: AmazonS3Exception during upload: {amazonS3Exception.Message}");
            Console.Error.WriteLine($"  ErrorCode: {amazonS3Exception.ErrorCode}");
            Console.Error.WriteLine($"  StatusCode: {amazonS3Exception.StatusCode}");
            throw new Exception("There was an error uploading the file to S3: " + amazonS3Exception.Message, amazonS3Exception);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"S3Service ERROR: An unexpected error occurred while uploading the file: {ex.Message}");
            throw new Exception("An unexpected error occurred while uploading the file.", ex);
        }
    }

    public async Task<byte[]> DownloadFileAsByteArrayAsync(string bucketName, string key)
    {
        Console.WriteLine($"S3Service: Attempting to download file '{key}' from bucket '{bucketName}'.");
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
                Console.WriteLine($"S3Service: Download response status: {response.HttpStatusCode}");
                await response.ResponseStream.CopyToAsync(memoryStream);
                Console.WriteLine($"S3Service: File '{key}' downloaded successfully. Size: {memoryStream.Length} bytes.");
                return memoryStream.ToArray();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"S3Service ERROR: Error downloading file '{key}' from S3: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> DeleteFileByUrlAsync(string fileUrl, string bucketName)
    {
        Console.WriteLine($"S3Service: Attempting to delete file from URL: '{fileUrl}' from bucket '{bucketName}'.");
        try
        {
            string fileKey = GetFileKeyFromUrl(fileUrl);
            Console.WriteLine($"S3Service: Extracted file key for deletion: '{fileKey}'.");

            if (string.IsNullOrEmpty(fileKey))
            {
                Console.WriteLine("S3Service: File key is empty or null after extraction, skipping deletion.");
                return false; // לא ניתן למחוק קובץ ללא מפתח
            }

            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = fileKey
            };

            Console.WriteLine("S3Service: Sending delete request to S3.");
            var deleteResponse = await _s3Client.DeleteObjectAsync(deleteRequest);
            Console.WriteLine($"S3Service: Delete response received. HTTP Status: {deleteResponse.HttpStatusCode}");

            if (deleteResponse.HttpStatusCode == System.Net.HttpStatusCode.NoContent)
            {
                Console.WriteLine($"S3Service: File '{fileKey}' deleted successfully from bucket '{bucketName}'.");
                return true;
            }
            else
            {
                Console.WriteLine($"S3Service: File '{fileKey}' deletion failed. Unexpected HTTP Status: {deleteResponse.HttpStatusCode}");
                return false;
            }
        }
        catch (AmazonS3Exception amazonS3Exception)
        {
            Console.Error.WriteLine($"S3Service ERROR: AmazonS3Exception during deletion of '{fileUrl}':");
            Console.Error.WriteLine($"  Message: {amazonS3Exception.Message}");
            Console.Error.WriteLine($"  ErrorCode: {amazonS3Exception.ErrorCode}"); // זה קריטי!
            Console.Error.WriteLine($"  StatusCode: {amazonS3Exception.StatusCode}");
            Console.Error.WriteLine($"  RequestId: {amazonS3Exception.RequestId}");
            Console.Error.WriteLine($"  Amazon Error Type: {amazonS3Exception.ErrorType}");
            Console.Error.WriteLine($"  Stack Trace: {amazonS3Exception.StackTrace}");
            throw new Exception("There was an error deleting the file from S3: " + amazonS3Exception.Message, amazonS3Exception);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"S3Service ERROR: An unexpected error occurred while deleting file from '{fileUrl}': {ex.Message}");
            Console.Error.WriteLine($"  Stack Trace: {ex.StackTrace}");
            throw new Exception("An unexpected error occurred while deleting the file.", ex);
        }
    }

    private string GetFileKeyFromUrl(string fileUrl)
    {
        Console.WriteLine($"S3Service: Extracting file key from URL: '{fileUrl}'.");
        if (string.IsNullOrEmpty(fileUrl))
        {
            Console.WriteLine("S3Service: fileUrl is null or empty, cannot extract key.");
            return string.Empty;
        }
        try
        {
            var uri = new Uri(fileUrl);
            string key = uri.AbsolutePath.TrimStart('/');
            Console.WriteLine($"S3Service: Extracted key: '{key}'.");
            return key;
        }
        catch (UriFormatException uriEx)
        {
            Console.Error.WriteLine($"S3Service ERROR: Invalid URL format '{fileUrl}': {uriEx.Message}");
            return string.Empty;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"S3Service ERROR: General error extracting key from URL '{fileUrl}': {ex.Message}");
            return string.Empty;
        }
    }
}