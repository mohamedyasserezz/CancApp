using CanaApp.Domain.Contract.Service.File;
using CanaApp.Domain.Entities.Models;
using CancApp.Shared.Common.Consts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace CanaApp.Application.Services.Files
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly string _imagesPath;

        public FileService(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _imagesPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");
        }
        public async Task<string> SaveFileAsync(IFormFile imageFile, string subfolder)
        {
            if (imageFile is null)
                throw new ArgumentNullException(nameof(imageFile));
            var path = Path.Combine(_imagesPath, subfolder);

            // Ensure the directory exists
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            // Get the file extension from the uploaded file
            var extension = Path.GetExtension(imageFile.FileName);

            // Generate a unique name for the file
            var fileName = $"{Guid.NewGuid()}{extension}";
            var fileNamePath = Path.Combine(path, fileName);

            // Save the file to the uploads directory
            using var stream = new FileStream(fileNamePath, FileMode.Create);
            await imageFile.CopyToAsync(stream);

            return fileName;
        }


        public void DeleteFile(string file, string subfolder)
        {
            if (string.IsNullOrEmpty(file))
                throw new ArgumentNullException(nameof(file));


            var path = Path.Combine(_imagesPath, subfolder, file);

            if (!File.Exists(path))
                throw new FileNotFoundException($"Invalid File Path");

            File.Delete(path);
        }

        public string GetProfileUrl(ApplicationUser user)
        {
            var request = _httpContextAccessor.HttpContext?.Request;

            if (request is null)
                return null!;

            if (user is null || string.IsNullOrEmpty(user.Image))
                return $"{request!.Scheme}://{request.Host}/images/profiles/default.png";


            return $"{request.Scheme}://{request.Host}/images/profiles/{user.Image}";
        }
        public string GetProfileUrl(string image)
        {
            var request = _httpContextAccessor.HttpContext?.Request;

            if (request is null)
                return null!;

            if (string.IsNullOrEmpty(image))
                return $"{request!.Scheme}://{request.Host}/images/profiles/default.png";


            return $"{request.Scheme}://{request.Host}/images/profiles/{image}";
        }
        public string GetImageUrl(string subFolder,string Image)
        {

            var request = _httpContextAccessor.HttpContext?.Request;

            if (request is null)
                return null!;

            return $"{request.Scheme}://{request.Host}/images/{subFolder}/{Image}";
        }
    }
}
