using CanaApp.Domain.Entities.Comunity;
using System.Linq.Expressions;

namespace CanaApp.Domain.Specification.Community.Comments
{
    public class CommentSpecification : Specification<Comment, int>
    {
        public CommentSpecification() : base()
        {
            AddOrderByDesc(c => c.Time);
            AddIncludes();
        }
        public CommentSpecification(Expression<Func<Comment, bool>>? expression) : base(expression)
        {
            AddOrderByDesc(c => c.Time);
            AddIncludes();
        }

        private protected override void AddIncludes()
        {
            Includes.Add(c => c.User);
            Includes.Add(c => c.Reactions!);
        }

    }
}
