using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

public class S3Service
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public S3Service(IConfiguration configuration)
    {
        _s3Client = new AmazonS3Client(configuration["AWS:AccessKey"], configuration["AWS:SecretKey"], Amazon.RegionEndpoint.USEast1);
        _bucketName = configuration["AWS:BucketName"];
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        if (file.ContentType != "application/pdf")
        {
            throw new Exception("Only PDF files are allowed.");
        }

        using (var stream = file.OpenReadStream())
        {
            var putRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = file.FileName,
                InputStream = stream,
                ContentType = file.ContentType
            };

            var response = await _s3Client.PutObjectAsync(putRequest);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK ? $"https://{_bucketName}.s3.amazonaws.com/{file.FileName}" : null;
        }
    }
}
