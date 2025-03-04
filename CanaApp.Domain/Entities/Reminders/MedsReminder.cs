using CanaApp.Domain.Entities.Models;

namespace CanaApp.Domain.Entities.Reminders
{
    public class MedsReminder
    {
        public int Id { get; set; }
        public string Frequency { get; set; } = string.Empty;
        public MedsReminderTypes MedsReminderTypes { get; set; }
        public TimeSpan Alarm { get; set; }

        // Foreign key and navigation property
        public int PatientId { get; set; }
        public Patient Patient { get; set; } = default!;
    }
}
