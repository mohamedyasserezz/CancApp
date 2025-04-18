using CanaApp.Domain.Entities.Comunity;
using System.Linq.Expressions;

namespace CanaApp.Domain.Specification.Community.Comments
{
    public class CommentSpecification : Specification<Comment, int>
    {
        public CommentSpecification() : base()
        {
            AddOrderByDesc(c => c.Time);
        }
        public CommentSpecification(Expression<Func<Comment, bool>>? expression) : base(expression)
        {
            AddOrderByDesc(c => c.Time);
        }
    
    }
}
