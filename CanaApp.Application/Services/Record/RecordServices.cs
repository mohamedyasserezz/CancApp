﻿using CanaApp.Domain.Contract.Infrastructure;
using CanaApp.Domain.Contract.Service.File;
using CanaApp.Domain.Contract.Service.Record;
using CanaApp.Domain.Entities.Records;
using CanaApp.Domain.Specification.Records;
using CancApp.Shared.Common.Errors;
using CancApp.Shared.Abstractions;
using CancApp.Shared.Models.Record;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using CanaApp.Domain.Entities.Models;
using CancApp.Shared.Common.Consts;
using CancApp.Shared.Models.RecordAccess;

namespace CanaApp.Application.Services.Records
{
    class RecordServices(
        UserManager<ApplicationUser> userManager,
        ILogger<RecordServices> logger,
        IUnitOfWork unitOfWork,
        IFileService fileService
        ) : IRecordServices
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ILogger<RecordServices> _logger = logger;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IFileService _fileService = fileService;

        public async Task<Result<IEnumerable<RecordResponse>>> GetAllRecordsAsync(string userId)
        {
            _logger.LogInformation("Getting all records for user with id {userId}", userId);

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return Result.Failure<IEnumerable<RecordResponse>>(UserErrors.UserNotFound);

            if(user.UserType != UserType.Patient)
                return Result.Failure<IEnumerable<RecordResponse>>(UserErrors.PatientsOnly);

            var recordSpec = new RecordSpecification(r => r.UserId == userId);

            var records = await _unitOfWork.GetRepository<Record, int>().GetAllWithSpecAsync(recordSpec);

           

            var response = records.Select(r => new RecordResponse(
                r.Id,
                r.RecordType.ToString(),
                r.UserId,
                r.File is null ? null : _fileService.GetImageUrl(r.File, "records"),
                r.Note,
                r.Date
                ));

            return Result.Success(response);
        }
        public async Task<Result> CreateRecordAsync(RecordRequest request)
        {
            _logger.LogInformation("trying to create a new record");

            var user = await _userManager.FindByIdAsync(request.UserId);

            if (user is null)
                return Result.Failure<IEnumerable<RecordResponse>>(UserErrors.UserNotFound);

            if (user.UserType != UserType.Patient)
                return Result.Failure<IEnumerable<RecordResponse>>(UserErrors.PatientsOnly);

            var record = new Record
            {
                Date = request.Date,
                Note = request.Notes,
                RecordType = (RecordType)Enum.Parse(typeof(RecordType), request.RecordType),
                UserId = request.UserId,
                File = request.File is not null ? await _fileService.SaveFileAsync(request.File, "record") : 
                null,
            };

            await _unitOfWork.GetRepository<Record, int>().AddAsync(record);

            await _unitOfWork.CompleteAsync();

            return Result.Success();


        }

        public async Task<Result> DeleteRecordAsync(int Id)
        {


            var recordSpec = new RecordSpecification(r => r.Id == Id);

            var record = await _unitOfWork.GetRepository<Record,int>().GetWithSpecAsync(recordSpec);

            if(record is null)
                return Result.Failure(RecordErrors.RecordNotFound);

            _unitOfWork.GetRepository<Record, int>().Delete(record);

            await _unitOfWork.CompleteAsync();

            return Result.Success();
        }


        public async Task<Result> UpdateRecordAsync(UpdateRecordRequest request)
        {
            var recordSpec = new RecordSpecification(r => r.Id == request.Id);

            var record = await _unitOfWork.GetRepository<Record, int>().GetWithSpecAsync(recordSpec);

            if (record is null)
                return Result.Failure(RecordErrors.RecordNotFound);

            if (request.File is not null)
                record.File = await _fileService.SaveFileAsync(request.File, "record");

            if(request.Date is not null)
                record.Date = request.Date.Value;

            if(request.Note is not null)
                record.Note = request.Note;

            _unitOfWork.GetRepository<Record, int>().Update(record);

            await _unitOfWork.CompleteAsync();

            return Result.Success();
        }

        
    }
}
