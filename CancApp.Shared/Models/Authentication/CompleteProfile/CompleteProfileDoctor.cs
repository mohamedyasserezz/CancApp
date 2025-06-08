using Microsoft.AspNetCore.Http;

namespace CancApp.Shared.Models.Authentication.CompleteProfile
{
    public record CompleteProfileDoctorRequest(
        string Email,
        IFormFile MedicalSyndicatePhoto,
        IFormFile ImageId
        );
}
