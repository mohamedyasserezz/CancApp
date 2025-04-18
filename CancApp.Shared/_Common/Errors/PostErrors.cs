using CancApp.Shared.Abstractions;
using Microsoft.AspNetCore.Http;

namespace CancApp.Shared.Common.Errors
{
    public static class PostErrors
    {
        
        public static readonly Error PostNotFound =
        new("Post.PostNotFound", "Post is not found", StatusCodes.Status404NotFound);
        
    }
}
