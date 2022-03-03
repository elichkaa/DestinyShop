using CloudinaryDotNet;
using EzBuy.Models;
using Microsoft.AspNetCore.Http;

namespace EzBuy.Services.Contracts
{
    public interface ICloudinaryService
    {
        public Task<ICollection<Image>> UploadAsync(ICollection<IFormFile> files, string basePath);

        public Task DeleteImageAsync(Cloudinary cloudinary, string path);

        public Task DeleteImagesAsync(Cloudinary cloudinary, string[] paths);
    }
}
