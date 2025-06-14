using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancApp.Shared.Models.User.EditProfile
{
    public record UserResponse(
        string Id,
        string Name,
        string Image,
        string Address,
        string UserType
        );
}
