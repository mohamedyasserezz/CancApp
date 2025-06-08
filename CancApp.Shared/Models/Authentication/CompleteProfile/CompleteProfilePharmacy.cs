using Microsoft.AspNetCore.Http;

namespace CancApp.Shared.Models.Authentication.CompleteProfile
{
    public record CompleteProfilePharmacyRequest(
        string Email,
        IFormFile ImageId,
        IFormFile ImagePharmacyLicense,
        TimeOnly OpenHour,
        TimeOnly ClouseHour,
        float Longitude,
        float Latitude,
        bool IsDeliveryEnabled
        );
}
