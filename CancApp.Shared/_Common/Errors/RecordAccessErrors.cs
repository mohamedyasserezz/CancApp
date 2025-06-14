using CancApp.Shared.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancApp.Shared._Common.Errors
{
    public static class RecordAccessErrors
    {
        public static readonly Error RequestExists =
        new Error("RequestExists", "There is already a pending request.",StatusCodes.Status400BadRequest);

        public static readonly Error RequestNotFound =
        new Error("RequestNotFound", "Request not found.", StatusCodes.Status400BadRequest);

        public static readonly Error Unauthorized =
            new Error("Unauthorized", "You are not authorized to respond to this request.", 403);
    }
}
