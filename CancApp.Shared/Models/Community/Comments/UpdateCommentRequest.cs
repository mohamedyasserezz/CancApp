using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancApp.Shared.Models.Community.Comments
{
    public record UpdateCommentRequest(
        int PostId,
        int CommentId,
        string Content
        );
}
