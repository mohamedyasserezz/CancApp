using Microsoft.AspNetCore.Http;

namespace CancApp.Shared.Models.Record
{
    public record UpdateRecordRequest(
        int Id,
        string? Note,
        IFormFile? File,
        DateTime? Date
        );
}
