using Microsoft.AspNetCore.Http;

namespace CancApp.Shared.Models.Authentication.CompleteProfile
{
    public record CompleteProfileDoctor(
        string UserId,
        IFormFile MedicalSyndicatePhoto,
        IFormFile ImageId
        );
}
