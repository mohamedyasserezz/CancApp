using CanaApp.Domain.Contract.Service.File;
using CanaApp.Domain.Entities.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace CanaApp.Application.Services.Files
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _imagesPath;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
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

    }
}
