using CancApp.Shared.Abstractions;
using CancApp.Shared.Models.Record;
using CancApp.Shared.Models.RecordAccess;
using CancApp.Shared.Models.User.EditProfile;

namespace CanaApp.Domain.Contract.Service.RecordAccess
{
    public interface IRecordAccessServices
    {
        Task<Result> RequestRecordAccessAsync(RecordAccessRequest request);
        Task<Result> ApproveRecordAccessAsync(ApproveRecordAccessRequest request, string patientId);
        Task<Result<RecordAccessResponse>> CanDoctorViewPatientRecordsAsync(RecordAccessRequest request, string doctorId);
        Task<Result<IEnumerable<RecordAccessResponse>>> GetPendingRequestsForPatientAsync(string patientId);

        Task<Result<IEnumerable<UserResponse>>> GetAllPatientsAccepted (string doctotId);

        Task<Result<IEnumerable<RecordResponse>>> GetAllRecordsForPatientAsync(RecordAccessRequest request);
    }
}
