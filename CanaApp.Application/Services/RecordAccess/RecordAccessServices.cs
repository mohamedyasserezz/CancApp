using CanaApp.Domain.Contract.Infrastructure;
using CanaApp.Domain.Contract.Service.File;
using CanaApp.Domain.Contract.Service.RecordAccess;
using CanaApp.Domain.Entities.Models;
using CanaApp.Domain.Entities.Records;
using CanaApp.Domain.Specification.Models;
using CancApp.Shared._Common.Errors;
using CancApp.Shared.Abstractions;
using CancApp.Shared.Common.Errors;
using CancApp.Shared.Models.RecordAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CanaApp.Application.Services.RecordsAccess
{
    public class RecordAccessServices(
        UserManager<ApplicationUser> userManager,
        IFileService fileService,
        IUnitOfWork unitOfWork,
        ILogger<RecordAccessServices> logger
        ) : IRecordAccessServices
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IFileService _fileService = fileService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<RecordAccessServices> _logger = logger;
        public async Task<Result> RequestRecordAccessAsync(RecordAccessRequest accessRequest)
        {
            _logger.LogInformation("Doctor {DoctorId} is requesting access to patient {PatientId}'s records.", accessRequest.DoctorId, accessRequest.PatientId);

            var patient = await _userManager.FindByIdAsync(accessRequest.PatientId);
            var doctor = await _userManager.FindByIdAsync(accessRequest.DoctorId);

            if (patient is null || patient.UserType != UserType.Patient)
            {
                _logger.LogWarning("Target user {PatientId} is not a patient.", accessRequest.PatientId);
                return Result.Failure(UserErrors.InvalidRoles);
            }
            if (doctor == null || doctor.UserType != UserType.Doctor)
            {
                _logger.LogWarning("Requester {DoctorId} is not a doctor.", accessRequest.DoctorId);
                return Result.Failure(UserErrors.InvalidRoles);
            }


            var recordAccessSpecification = new RecordAccessSpecification(r => r.DoctorId == accessRequest.DoctorId && r.PatientId == accessRequest.PatientId && r.IsApproved == null);

            var existing = await _unitOfWork.GetRepository<RecordAccess, int>().GetCountWithSpecAsync(recordAccessSpecification);

            if (existing > 0)
            {
                _logger.LogWarning("There is already a pending request from doctor {DoctorId} to patient {PatientId}.", accessRequest.DoctorId, accessRequest.PatientId);
                return Result.Failure(RecordAccessErrors.RequestExists);
            }

            var request = new RecordAccess
            {
                DoctorId = accessRequest.DoctorId,
                PatientId = accessRequest.PatientId,
                RequestedAt = DateTime.UtcNow,
                IsApproved = null
            };
            await _unitOfWork.GetRepository<RecordAccess, int>().AddAsync(request);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Record access request created: Doctor {DoctorId} -> Patient {PatientId}", request.DoctorId, request.PatientId);
            return Result.Success();
        }

        public async Task<Result> ApproveRecordAccessAsync(ApproveRecordAccessRequest dto, string patientId)
        {
            _logger.LogInformation("Patient {PatientId} is responding to record access request {RequestId} with approve={Approve}.", patientId, dto.RequestId, dto.Approve);

            var requestAccess = await _unitOfWork.GetRepository<RecordAccess, int>().GetByIdAsync(dto.RequestId);
            if (requestAccess is null)
            {
                _logger.LogWarning("Record access request {RequestId} not found.", dto.RequestId);
                return Result.Failure(RecordAccessErrors.RequestNotFound);
            }

            if (requestAccess.PatientId != patientId)
            {
                _logger.LogWarning("User {PatientId} is not authorized to respond to request {RequestId}.", patientId, dto.RequestId);
                return Result.Failure(RecordAccessErrors.Unauthorized);
            }

            if (requestAccess.IsApproved != null)
            {
                _logger.LogWarning("Request {RequestId} has already been responded to.", dto.RequestId);
                return Result.Failure(RecordAccessErrors.RequestExists);
            }

            requestAccess.IsApproved = dto.Approve;
            _unitOfWork.GetRepository<RecordAccess, int>().Update(requestAccess);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Request {RequestId} responded: approve={Approve}", dto.RequestId, dto.Approve);
            return Result.Success();
        }

        public async Task<Result<RecordAccessResponse>> CanDoctorViewPatientRecordsAsync(RecordAccessRequest request, string doctorId)
        {

            var user = await _userManager.FindByIdAsync(doctorId);

            if (user is null || user.UserType != UserType.Doctor)
            {
                _logger.LogWarning("User {DoctorId} is not a doctor.", doctorId);
                return Result.Failure<RecordAccessResponse>(UserErrors.InvalidRoles);
            }


            var requestSpecification = new RecordAccessSpecification(r => r.DoctorId == request.DoctorId && r.PatientId == request.PatientId && r.IsApproved == true);

            var recordAccess = await _unitOfWork.GetRepository<RecordAccess, int>().GetWithSpecAsync(requestSpecification);
            
            if(recordAccess is null)
            {
                _logger.LogInformation("Doctor {DoctorId} does not have access to patient {PatientId}'s records.", request.DoctorId, request.PatientId);
                return Result.Failure<RecordAccessResponse>(RecordAccessErrors.RequestNotFound);
            }


            var response = new RecordAccessResponse(
                recordAccess.Id,
                recordAccess.Patient.FullName,
                _fileService.GetProfileUrl(recordAccess.Patient),
                recordAccess.RequestedAt,
                recordAccess.IsApproved
            );
            if(recordAccess.IsApproved == null)
            {
                _logger.LogInformation("Doctor {DoctorId} has a pending request for patient {PatientId}'s records.", request.DoctorId, request.PatientId);
            }
            else if (recordAccess.IsApproved == false)
            {
                _logger.LogInformation("Doctor {DoctorId} has been denied access to patient {PatientId}'s records.", request.DoctorId, request.PatientId);
            }
            else
            {
                _logger.LogInformation("Doctor {DoctorId} has been granted access to patient {PatientId}'s records.", request.DoctorId, request.PatientId);
            }

            return Result.Success(response);
        }

        public async Task<Result<IEnumerable<RecordAccessResponse>>> GetPendingRequestsForPatientAsync(string patientId)
        {
            var requestSpecification = new RecordAccessSpecification(r => r.PatientId == patientId && r.IsApproved == null);

            var recordAccesses = await _unitOfWork.GetRepository<RecordAccess, int>().GetAllWithSpecAsync(requestSpecification);

            var response = recordAccesses.Select(r => new RecordAccessResponse(
                r.Id,
                r.Doctor.FullName,
                _fileService.GetProfileUrl(r.Doctor),
                r.RequestedAt
                ));

            return Result.Success(response);
        }
    }
}
