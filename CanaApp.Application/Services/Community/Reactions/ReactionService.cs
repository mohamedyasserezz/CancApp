﻿
using AutoMapper;
using CanaApp.Domain.Contract.Infrastructure;
using CanaApp.Domain.Contract.Service.Community.Reaction;
using CanaApp.Domain.Entities.Comunity;
using CanaApp.Domain.Entities.Models;
using CanaApp.Domain.Specification;
using CanaApp.Domain.Specification.Community.Reactions;
using CancApp.Shared.Abstractions;
using CancApp.Shared.Common.Errors;
using CancApp.Shared.Models.Community.Reactions;
using Microsoft.Extensions.Logging;

namespace CanaApp.Application.Services.Community.Reactions
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

        public async Task<Result<IEnumerable<ReactionResponse>>> GetReactionsAsync(int postId, int? commentId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting reactions for post {PostId} and comment {CommentId}", postId, commentId);
            if (await _unitOfWork.GetRepository<Post, int>().GetByIdAsync(postId, cancellationToken) is null)
            {
                return Result.Failure<IEnumerable<ReactionResponse>>(PostErrors.PostNotFound);
            }
            var spec1 = new ReactionSpecification(
                r => r.PostId == postId && (commentId == null || r.CommentId == commentId));

            var spec2 = new ReactionSpecification(r => r.PostId == postId);

            var reactions = commentId.HasValue ?
                await _unitOfWork.GetRepository<Reaction, int>().GetAllWithSpecAsync(spec1)
                : await _unitOfWork.GetRepository<Reaction, int>().GetAllWithSpecAsync(spec2);

            
            var response = _mapper.Map<IEnumerable<ReactionResponse>>(reactions);

            return Result.Success(response);
        }
        public async Task<Result> AddReactionAsync(ReactionRequest request, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Adding reaction for post {PostId} and comment {CommentId}", request.PostId, request.CommentId);
            if (await _unitOfWork.GetRepository<Post, int>().GetByIdAsync(request.PostId, cancellationToken) is null)
            {
                return Result.Failure(PostErrors.PostNotFound);
            }

            var userSpec = new Specification<ApplicationUser, string>(u => u.Id == request.UserId);

            if (await _unitOfWork.GetRepository<ApplicationUser, string>().GetWithSpecAsync(userSpec) is null)
            {
                return Result.Failure(ReactionErrors.ReactionAlreadyExists);
            }

            if (request.IsComment && request.CommentId.HasValue)
            {
                if (await _unitOfWork.GetRepository<Comment, int>().GetByIdAsync(request.CommentId.Value, cancellationToken) is null)
                {
                    return Result.Failure(CommentErrors.CommentNotFound);
                }
                
            }

            var reaction = new Reaction
            {
                PostId = request.PostId,
                CommentId = request.CommentId,
                UserId = request.UserId,
                ReactionType = Enum.Parse<ReactionType>(request.ReactionType)
            };

            await _unitOfWork.GetRepository<Reaction, int>().AddAsync(reaction, cancellationToken);

            await _unitOfWork.CompleteAsync();

            return Result.Success();
        }
        public async Task<Result> RemoveReactionAsync(ReactionRequest request, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Removing reaction for post {PostId} and comment {CommentId}", request.PostId, request.CommentId);
            if (await _unitOfWork.GetRepository<Post, int>().GetByIdAsync(request.PostId, cancellationToken) is null)
            {
                return Result.Failure(PostErrors.PostNotFound);
            }

            var userSpec = new Specification<ApplicationUser, string>(u => u.Id == request.UserId);

            if (await _unitOfWork.GetRepository<ApplicationUser, string>().GetWithSpecAsync(userSpec) is null)
            {
                return Result.Failure(UserErrors.UserNotFound);
            }

            if (request.IsComment && request.CommentId.HasValue)
            {
                if (await _unitOfWork.GetRepository<Comment, int>().GetByIdAsync(request.CommentId.Value, cancellationToken) is null)
                {
                    return Result.Failure(CommentErrors.CommentNotFound);
                }

            }

            var reactionSpec = new ReactionSpecification(
                r => r.PostId == request.PostId && r.UserId == request.UserId && (request.IsComment ? r.CommentId == request.CommentId : !r.CommentId.HasValue));

            var reaction = await _unitOfWork.GetRepository<Reaction, int>().GetWithSpecAsync(reactionSpec);

            if (reaction is null)
                return Result.Failure(ReactionErrors.ReactionNotFound);

            _unitOfWork.GetRepository<Reaction, int>().Delete(reaction);

            await _unitOfWork.CompleteAsync();

            return Result.Success();
        }
    }
}
