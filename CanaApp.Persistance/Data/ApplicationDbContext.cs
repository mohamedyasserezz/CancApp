using CanaApp.Domain.Entities.Chats;
using CanaApp.Domain.Entities.Comunity;
using CanaApp.Domain.Entities.Models;
using CanaApp.Domain.Entities.Records;
using CanaApp.Domain.Entities.Reminders;
using CanaApp.Domain.Entities.Roles;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
namespace CanaApp.Persistance.Data
{
    public class ApplicationDbContext(DbContextOptions options) :
        IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
    {
        // User-related DbSets
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Pharmacist> Pharmacists { get; set; }
        public DbSet<Psychiatrist> Psychiatrists { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }

        // Community-related DbSets
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reaction> Reactions { get; set; }

        // Chat-related DbSets
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }

        // Record-related DbSets
        public DbSet<Record> Records { get; set; }
        public DbSet<RecordAccess> RecordAccessRequests { get; set; }

        // Reminder-related DbSets
        public DbSet<MedsReminder> MedsReminders { get; set; }
        public DbSet<VisitReminder> VisitReminders { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
