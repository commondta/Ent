using CloudinaryDotNet.Actions;

namespace HRMS_Web.Services.PhotoService
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> UploadPhotoAsync(string photo);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}
