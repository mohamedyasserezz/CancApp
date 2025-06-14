using CancApp.Shared.Abstractions;
using CancApp.Shared.Models.RecordAccess;

namespace CanaApp.Domain.Contract.Service.RecordAccess
{
    public interface IRecordAccessServices
    {
        Task<Result> RequestRecordAccessAsync(RecordAccessRequest request);
        Task<Result> ApproveRecordAccessAsync(ApproveRecordAccessRequest request, string patientId);
        Task<Result<RecordAccessResponse>> CanDoctorViewPatientRecordsAsync(RecordAccessRequest request, string doctorId);
        Task<Result<IEnumerable<RecordAccessResponse>>> GetPendingRequestsForPatientAsync(string patientId);
    }
}
