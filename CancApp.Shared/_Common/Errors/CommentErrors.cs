using CancApp.Shared.Abstractions;
using Microsoft.AspNetCore.Http;

namespace CancApp.Shared.Common.Errors
{
    public static class CommentErrors
    {

        public static readonly Error CommentNotFound =
        new("Post.CommentNotFound", "Comment is not found", StatusCodes.Status404NotFound);

    }
}
