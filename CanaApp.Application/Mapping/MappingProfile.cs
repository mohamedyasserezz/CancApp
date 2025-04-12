using AutoMapper;
using CanaApp.Domain.Entities.Comunity;
using CanaApp.Domain.Entities.Models;
using CancApp.Shared.Common.Consts;
using CancApp.Shared.Models.Community.Reactions;
using Microsoft.AspNetCore.Http;

namespace CanaApp.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile(IHttpContextAccessor httpContextAccessor)
        {
            CreateMap<Reaction, ReactionResponse>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.ApplicationUser.UserName))
                .ForMember(dest => dest.UserProfilePictureUrl, opt => opt.MapFrom(src => GetProfilePictureUrl(httpContextAccessor, src.ApplicationUser.Image!, src.ApplicationUser.UserType)))
                .ForMember(dest => dest.ReactionType, opt => opt.MapFrom(src => src.ReactionType.ToString()))
                .ForMember(dest => dest.IsComment, opt => opt.MapFrom(src => src.CommentId.HasValue))
                .ReverseMap();

            // Add other mappings as needed
        }

        private static string? GetProfilePictureUrl(IHttpContextAccessor httpContextAccessor, string fileName, UserType userType)
        {
            if (string.IsNullOrEmpty(fileName))
                return null;

            var request = httpContextAccessor.HttpContext?.Request;
            if (request == null) return null;

            // Map UserType to the appropriate subfolder
            string subfolder = userType switch
            {
                UserType.Patient => ImageSubFolder.Patients,
                UserType.Doctor => ImageSubFolder.Doctors,
                UserType.Pharmacist => ImageSubFolder.Pharmacists,
                UserType.Psychiatrist => ImageSubFolder.Psychiatrists,
                UserType.Volunteer => ImageSubFolder.Volunteers,
                _ => ImageSubFolder.Default
            };

            return $"{request.Scheme}://{request.Host}/images/{subfolder}/{fileName}";
        }

    }
}
