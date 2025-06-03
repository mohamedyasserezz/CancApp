using AutoMapper;
using CanaApp.Domain.Entities.Comunity;
using CancApp.Shared.Models.Community.Comments;
using Microsoft.AspNetCore.Http;

namespace CanaApp.Application.Mapping.Resolver
{
    class CommentProfileResolver(IHttpContextAccessor httpContextAccessor) : IValueResolver<Comment, CommentResponse, string?>
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public string? Resolve(Comment source, CommentResponse destination, string? destMember, ResolutionContext context)
        {
            if (source?.User?.Image == null || string.IsNullOrEmpty(source.User.Image))
                return null;

            var request = _httpContextAccessor.HttpContext?.Request;

            if (request == null) return null!;
            
            return $"{request.Scheme}://{request.Host}/images/profiles/{source.User.Image}";
        }
    }

}
