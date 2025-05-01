using AutoMapper;
using CanaApp.Domain.Entities.Comunity;
using CanaApp.Domain.Entities.Models;
using CancApp.Shared.Common.Consts;
using CancApp.Shared.Models.Community.Comments;
using CancApp.Shared.Models.Community.Post;
using CancApp.Shared.Models.Community.Reactions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing.Constraints;

namespace CanaApp.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile(IHttpContextAccessor httpContextAccessor)
        {
            CreateMap<Reaction, ReactionResponse>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.ApplicationUser.UserName))
                .ForMember(dest => dest.UserProfilePictureUrl, opt => opt.MapFrom(src => GetProfilePictureUrl(httpContextAccessor, src.ApplicationUser.Image!)))
                .ForMember(dest => dest.ReactionType, opt => opt.MapFrom(src => src.ReactionType.ToString()))
                .ForMember(dest => dest.IsComment, opt => opt.MapFrom(src => src.CommentId.HasValue));

            CreateMap<Comment, CommentResponse>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.UserImage, opt => opt.MapFrom(src => GetProfilePictureUrl(httpContextAccessor, src.User.Image!)));

            CreateMap<Post, PostResponse>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => GetPostImageUrl(httpContextAccessor, src.User.Image!)))
                .ForMember(dest => dest.CommentsCount, opt => opt.MapFrom(src => src.Comments.Count))
                .ForMember(dest => dest.ReactionsCount, opt => opt.MapFrom(src => src.Reactions!.Count));

        }

        private static string? GetProfilePictureUrl(IHttpContextAccessor httpContextAccessor, string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return null;

            var request = httpContextAccessor.HttpContext?.Request;
            if (request == null) return null;

            // Map UserType to the appropriate subfolder


            return $"{request.Scheme}://{request.Host}/images/profiles/{fileName}";
        }

        private static string? GetPostImageUrl(IHttpContextAccessor httpContextAccessor, string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return null;

            var request = httpContextAccessor.HttpContext?.Request;
            if (request == null) return null;

            // Map UserType to the appropriate subfolder


            return $"{request.Scheme}://{request.Host}/images/posts/{fileName}";
        }
    }
}
