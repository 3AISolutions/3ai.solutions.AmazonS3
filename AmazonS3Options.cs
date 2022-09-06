namespace _3ai.solutions.AmazonS3
{
    public record AmazonS3Options()
    {
        public string BucketName { get; init; } = string.Empty;
        public string AccessKey { get; init; } = string.Empty;
        public string SecretAccessKey { get; init; } = string.Empty;
        public string Region { get; init; } = "EUWest1";
    }
}