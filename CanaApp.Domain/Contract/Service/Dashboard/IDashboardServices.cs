using CancApp.Shared.Abstractions;
using CancApp.Shared.Models.Authentication;
using CancApp.Shared.Models.Community.Comments;
using CancApp.Shared.Models.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanaApp.Domain.Contract.Service.Dashboard
{
    public  interface IDashboardServices
    {
        Task<Result<AuthResponse>> Login(string email, string password, CancellationToken cancellationToken = default);

        Task<Result<IEnumerable<CommentResponse>>> GetReportedCommentsAsync();

        Task<Result> AddWarningToUserAsync(string id);
        Task<Result> DisableUserAsync(string userId);

        Task<Result> EnableUserAsync(string userId);
        Task<Result<NumberOfUsersResponse>> GetUsersChart();
    }
}
