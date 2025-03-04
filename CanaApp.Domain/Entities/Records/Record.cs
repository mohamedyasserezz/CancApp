using CanaApp.Domain.Entities.Models;

namespace CanaApp.Domain.Entities.Records
{
    public class Record
    {
        public int Id { get; set; }
        public RecordType RecordType { get; set; }
        public Patient Patient { get; set; } = default!;
        public Doctor Doctor { get; set; } = default!;

    }
}
