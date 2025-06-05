using AutoMapper;
using CanaApp.Application.Mapping.Resolver;
using CanaApp.Domain.Entities.Comunity;
using CancApp.Shared.Models.Community.Comments;
using CancApp.Shared.Models.Community.Post;
using CancApp.Shared.Models.Community.Reactions;
using Microsoft.AspNetCore.Http;

namespace CanaApp.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Reaction, ReactionResponse>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.UserProfilePictureUrl, opt => opt.MapFrom<ReactionProfileResolver>())
                .ForMember(dest => dest.IsComment, opt => opt.MapFrom(src => src.CommentId.HasValue));

            CreateMap<Comment, CommentResponse>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.UserImageUrl, otp => otp.MapFrom<CommentProfileResolver>());



            CreateMap<Post, PostResponse>()
                .ForMember(dest => dest.UserProgilePictureUrl, opt => opt.MapFrom<PostPictureResolver>())
                .ForMember(dest => dest.CommentsCount, opt => opt.MapFrom(src => src.Comments.Count))
                .ForMember(dest => dest.ReactionsCount, opt => opt.MapFrom(src => src.Reactions!.Count));

        }
    }
}
