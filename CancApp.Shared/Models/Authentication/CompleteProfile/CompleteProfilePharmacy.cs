﻿using Microsoft.AspNetCore.Http;

namespace CancApp.Shared.Models.Authentication.CompleteProfile
{
    public record CompleteProfilePharmacy(
        string Email,
        IFormFile ImageId,
        IFormFile ImagePharmacyLicense,
        int NumberOfWorkingHours,
        bool IsDeliveryEnabled
        );
}
