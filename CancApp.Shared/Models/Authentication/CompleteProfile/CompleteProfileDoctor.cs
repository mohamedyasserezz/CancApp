using Microsoft.AspNetCore.Http;

namespace CancApp.Shared.Models.Authentication.CompleteProfile
{
    public record CompleteProfileDoctor(
        IFormFile MedicalSyndicatePhoto,
        IFormFile ImageId
        );
}
