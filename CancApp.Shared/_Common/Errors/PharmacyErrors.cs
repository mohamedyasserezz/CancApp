using CancApp.Shared.Abstractions;
using Microsoft.AspNetCore.Http;

namespace CancApp.Shared.Common.Errors
{
    public static class PharmacyErrors
    {
        public static readonly Error WrongData =
            new ("Pharmacy.WrongData", "Invalid ImageId/ImagePharmacyLicense", StatusCodes.Status400BadRequest);
    }
}
