using CancApp.Shared.Abstractions;
using CancApp.Shared.Models.User.ChangePassword;

namespace CanaApp.Domain.Contract.Service.User
{
    public interface IUserServices
    {
        Task<Result> ChangePassword (string userId, ChangePasswordRequest request);
        //Task<Result>
       
    }
}
