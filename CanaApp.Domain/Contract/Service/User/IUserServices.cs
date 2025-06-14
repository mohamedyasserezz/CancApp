using CancApp.Shared.Abstractions;
using CancApp.Shared.Models.User.ChangePassword;
using CancApp.Shared.Models.User.EditProfile;
using CancApp.Shared.Models.User.Pharmacy;

namespace CanaApp.Domain.Contract.Service.User
{
    public interface IUserServices
    {
        Task<Result> ChangePassword (string userId, ChangePasswordRequest request);
        Task<Result<EditProfileResponse>> EditUserProfile(string userId, EditProfileRequest request);

        Task<Result<IEnumerable<PharmacistResponse>>> GetAllPharmacist(GetAllPharmacistRequest request);

    }
}
