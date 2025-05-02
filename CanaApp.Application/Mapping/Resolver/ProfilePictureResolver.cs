using AutoMapper;
using CanaApp.Domain.Entities.Models;
using CancApp.Shared.Models.Authentication;
using Microsoft.AspNetCore.Http;

namespace CanaApp.Application.Mapping.Resolver
{
    public class ProfilePictureResolver(IHttpContextAccessor httpContextAccessor) : IValueResolver<ApplicationUser, AuthResponse, string>
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public string Resolve(ApplicationUser source, AuthResponse destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.Image))
                return null!;

            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null) return null!;

            // Map UserType to the appropriate subfolder


            return $"{request.Scheme}://{request.Host}/images/profiles/{source.Image}";
        }
    }
}
