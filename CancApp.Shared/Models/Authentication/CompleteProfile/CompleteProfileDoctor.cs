using Microsoft.AspNetCore.Http;

namespace CancApp.Shared.Models.Authentication.CompleteProfile
{
    public record CompleteProfileDoctor(
        string Email,
        IFormFile MedicalSyndicatePhoto,
        IFormFile ImageId
        );
}
