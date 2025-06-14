using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancApp.Shared.Models.RecordAccess
{
    public record RecordAccessResponse(
        int Id,
        string Name,
        string? Image,
        DateTime RequestedAt,
        bool? IsApproved = null
        );
}
