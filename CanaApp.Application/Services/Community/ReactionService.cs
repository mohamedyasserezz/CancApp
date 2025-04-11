using AutoMapper;
using CanaApp.Domain.Contract.Infrastructure;
using CanaApp.Domain.Contract.Service.Community;
using CancApp.Shared.Abstractions;
using CancApp.Shared.Models.Community.Reactions;
using Microsoft.Extensions.Logging;

namespace CanaApp.Application.Services.Community
{
    class ReactionService(
        IUnitOfWork unitOfWork,
        IMapper mapper, 
        ILogger<ReactionService> logger
        ) : IReactionService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<ReactionService> _logger = logger;

        public Task<Result<IEnumerable<ReactionResponse>>> GetReactionsAsync(int postId, int? commentId)
        {
            throw new NotImplementedException();
        }
        public Task<Result> AddReactionAsync(ReactionRequest request)
        {
            throw new NotImplementedException();
        }


        public Task<Result> RemoveReactionAsync(ReactionRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
