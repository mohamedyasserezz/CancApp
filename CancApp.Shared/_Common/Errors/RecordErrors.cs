using CancApp.Shared.Abstractions;
using Microsoft.AspNetCore.Http;

namespace CancApp.Shared.Common.Errors
{
    public static class RecordErrors
    {
        public static readonly Error RecordNotFound =
            new("Record.RecordNotFound", "Record is not found", StatusCodes.Status404NotFound);
    }
}
