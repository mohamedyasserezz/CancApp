using CanaApp.Domain.Entities.Models;

namespace CanaApp.Domain.Entities.Records
{
    public class Record
    {
        public int Id { get; set; }
        public string? File { get; set; }
        public string UserId { get; set; } = default!;
        public ApplicationUser User { get; set; } = null!;
        public string Note { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public RecordType RecordType { get; set; }

    }
}
