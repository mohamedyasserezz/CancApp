using AutoMapper;
using CanaApp.Domain.Entities.Comunity;
using CancApp.Shared.Models.Community.Post;
using Microsoft.AspNetCore.Http;

namespace CanaApp.Application.Mapping.Resolver
{
    public class PostPictureResolver(IHttpContextAccessor httpContextAccessor) : IValueResolver<Post, PostResponse, string?>
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public string Resolve(Post source, PostResponse destination, string? destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.Image))
                return null!;

            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null)
                return null!;

            return $"{request.Scheme}://{request.Host}/images/posts/{source.Image}";
        }
    }

}
