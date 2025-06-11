using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancApp.Shared.Models.User.ChangePassword
{
    public record ChangePasswordRequest(
        string OldPassword,
        string NewPassword
        );
}
