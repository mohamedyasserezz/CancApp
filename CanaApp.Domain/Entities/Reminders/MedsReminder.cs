using CanaApp.Domain.Entities.Models;

namespace CanaApp.Domain.Entities.Reminders
{
    class MedsReminder
    {
        public int Id { get; set; }
        public string Frequency { get; set; }
        public MedsReminderTypes MedsReminderTypes { get; set; }
        public TimeSpan Alarm { get; set; }

        // Foreign key and navigation property
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}
