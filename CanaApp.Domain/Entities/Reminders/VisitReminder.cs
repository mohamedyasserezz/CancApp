using CanaApp.Domain.Entities.Models;

namespace CanaApp.Domain.Entities.Reminders
{
    public class VisitReminder
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } = default!;
        public TimeOnly Alarm { get; set; }

        // Foreign key and navigation property
        public Patient Patient { get; set; } = default!;
    }
}
