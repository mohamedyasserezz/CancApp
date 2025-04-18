using Microsoft.AspNetCore.Http;

namespace CancApp.Shared.Models.Authentication.CompleteProfile
{
    public record CompleteProfilePharmacy(
        string UserId,
        IFormFile ImageId,
        IFormFile ImagePharmacyLicense,
        int NumberOfWorkingHours,
        bool IsDeliveryEnabled
        );
}
