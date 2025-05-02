using AutoMapper;
using CanaApp.Domain.Entities.Comunity;
using CancApp.Shared.Models.Community.Reactions;
using Microsoft.AspNetCore.Http;

namespace CanaApp.Application.Mapping.Resolver
{
    public class ReactionProfileResolver(IHttpContextAccessor httpContextAccessor) : IValueResolver<Reaction, ReactionResponse, string>
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public string Resolve(Reaction source, ReactionResponse destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.ApplicationUser.Image))
                return null!;
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null) return null!;
            return $"{request.Scheme}://{request.Host}/images/profiles/{source.ApplicationUser.Image}";
        }
    }
}
