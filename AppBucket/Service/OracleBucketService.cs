using Oci.Common.Auth;
using Oci.ObjectstorageService.Requests;
using Oci.ObjectstorageService;

namespace AppBucket.Service
{
    public class OracleBucketService
    {
        public readonly ObjectStorageClient _client;
        private readonly string _namespaceName = "axcbgldfuawv";
        private readonly string _bucketName = "produtos-fotos";

        public OracleBucketService()
        {
            var provider = new ConfigFileAuthenticationDetailsProvider(".oci/config", "DEFAULT");
            _client = new ObjectStorageClient(provider);
            _client.SetRegion("us-phoenix-1");
        }

        public async Task<List<string>> ListObjectsAsync()
        {
            var request = new ListObjectsRequest
            {
                NamespaceName = _namespaceName,
                BucketName = _bucketName,
            };
            var response = await _client.ListObjects(request);
            return response.ListObjects.Objects.Select(o => o.Name).ToList();
        }

        public async Task UploadObjectAsync(string objectName, Stream data)
        {
            var request = new PutObjectRequest
            {
                NamespaceName = _namespaceName,
                BucketName = _bucketName,
                ObjectName = objectName,
                PutObjectBody = data,
            };
            await _client.PutObject(request);
        }

        public async Task<Stream> DownloadObjectAsync(string objectName)
        {
            var request = new GetObjectRequest
            {
                NamespaceName = _namespaceName,
                BucketName = _bucketName,
                ObjectName = objectName,
            };
            var response = await _client.GetObject(request);
            return response.InputStream;
        }

        public async Task DeleteObjectAsync(string objectName)
        {
            var request = new DeleteObjectRequest
            {
                NamespaceName = _namespaceName,
                BucketName = _bucketName,
                ObjectName = objectName,
            };
            await _client.DeleteObject(request);
        }
    }
}
