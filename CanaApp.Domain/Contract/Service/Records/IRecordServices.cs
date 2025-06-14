using CanaApp.Domain.Entities.Records;
using CancApp.Shared.Abstractions;
using CancApp.Shared.Models.Record;
using CancApp.Shared.Models.RecordAccess;
using System.Threading.Tasks;

namespace CanaApp.Domain.Contract.Service.Record
{
    public interface IRecordServices
    {
        Task<Result<IEnumerable<RecordResponse>>> GetAllRecordsAsync(string userId);
        Task<Result> CreateRecordAsync(RecordRequest request);
        Task<Result> UpdateRecordAsync(UpdateRecordRequest request);
        Task<Result> DeleteRecordAsync(int Id);
    }
}
