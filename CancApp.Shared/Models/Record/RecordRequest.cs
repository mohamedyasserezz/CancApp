using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancApp.Shared.Models.Record
{
    public record RecordRequest(
        string Notes,
        IFormFile? File,
        DateTime Date,
        string RecordType,
        string UserId
        );
    
}
