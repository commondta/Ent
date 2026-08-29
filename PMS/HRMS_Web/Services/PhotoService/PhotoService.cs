using B_Utility.Common;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace HRMS_Web.Services.PhotoService
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;

        public PhotoService(IConfiguration configuration)
        {
            var account = new Account(
                configuration["CloudinarySettings:CloudName"],
                configuration["CloudinarySettings:APIKey"],
                configuration["CloudinarySettings:APISecret"]);

            _cloudinary = new Cloudinary(account);
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);

            return result;
        }

        public async Task<ImageUploadResult> UploadPhotoAsync(string base64DataUri)
        {
            string extenstion = UHelper.ExtractExtensionFromBase64DataUri(base64DataUri);
            var fileName = $"{Guid.NewGuid()}.extenstion";
            var base64 = base64DataUri.Split(',')[1];
            var bytes = Convert.FromBase64String(base64);

            using var stream = new MemoryStream(bytes);
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, stream),
                Transformation = new Transformation().Width(500).Height(500)
            };

            return await _cloudinary.UploadAsync(uploadParams);
        }
    }
}
