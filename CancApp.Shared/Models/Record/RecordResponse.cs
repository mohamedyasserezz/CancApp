namespace CancApp.Shared.Models.Record
{
    public record RecordResponse(
        int Id,
        string RecordType,
        string UserId,
        string? File,
        DateTime Date
        );
}
