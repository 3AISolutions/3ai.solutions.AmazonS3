using _3ai.solutions.Core.Interfaces;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace _3ai.solutions.AmazonS3
{
    public class AmazonS3Service : IStorageService
    {

        private readonly string _bucketName;
        private readonly string _accessKey;
        private readonly string _secretAccessKey;
        private readonly Amazon.RegionEndpoint _region;

        public AmazonS3Service(AmazonS3Options options)
        {
            _bucketName = options.BucketName;
            _accessKey = options.AccessKey;
            _secretAccessKey = options.SecretAccessKey;
            _region = RegionEndpoint.GetBySystemName(options.Region);
        }

        public async Task<string> AddAsync(string relativePath, byte[] data, string contentType, CancellationToken token = default)
        {
            using AmazonS3Client client = new(_accessKey, _secretAccessKey, _region);
            PutObjectRequest request = new()
            {
                BucketName = _bucketName,
                Key = relativePath,
                ContentType = contentType
            };
            using MemoryStream ms = new(data);
            await ms.CopyToAsync(request.InputStream);
            var response = await client.PutObjectAsync(request);
            return relativePath;
        }

        public async Task<bool> DeleteAsync(string remotePath, CancellationToken token = default)
        {
            using AmazonS3Client client = new(awsAccessKeyId: _accessKey, awsSecretAccessKey: _secretAccessKey, region: _region);
            var response = await client.DeleteObjectAsync(_bucketName, remotePath, token);
            return response.HttpStatusCode == System.Net.HttpStatusCode.Accepted;
        }

        public string GetAccessURL(string remotePath, string ip)
        {
            throw new NotImplementedException();
        }

        public async Task<byte[]> GetAsync(string remotePath, CancellationToken token = default)
        {
            using AmazonS3Client client = new(_accessKey, _secretAccessKey, _region);
            var response = await client.GetObjectAsync(_bucketName, remotePath);
            using var stream = response.ResponseStream;
            using StreamReader reader = new(stream);
            using MemoryStream ms = new();
            await reader.BaseStream.CopyToAsync(ms);
            return ms.ToArray();
        }

        public string GetURI(string remotePath)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ZipAsync(string remotePaths, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}