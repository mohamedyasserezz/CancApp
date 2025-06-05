using CanaApp.Domain.Entities.Models;
using Microsoft.AspNetCore.Http;

namespace CanaApp.Domain.Contract.Service.File
{
    public interface IFileService
    {
        public Task<string> SaveFileAsync(IFormFile imageFile, string subfolder);

        public string GetProfileUrl(ApplicationUser user);
        public string GetImageUrl(string Image);
        public void DeleteFile(string file, string subfolder);
    }
}
