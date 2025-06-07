namespace CancApp.Shared.Models.Dashboard
{
    public record NumberOfUsersResponse(
        int NumberOfUsers,
        int NumberOfDoctors,
        int NumberOfPatients,
        int NumberOfPharmacist,
        int NumberOfVolunteers,
        int NumberOfPsychiatrist
        );
}
