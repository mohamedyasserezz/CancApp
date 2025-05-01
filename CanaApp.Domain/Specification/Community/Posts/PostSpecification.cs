using CanaApp.Domain.Entities.Comunity;
using System.Linq.Expressions;

namespace CanaApp.Domain.Specification.Community.Posts
{
    public class PostSpecification : Specification<Post, int>
    {
        public PostSpecification(int pageNumber, int pageSize) : base()
        {
            AddOrderByDesc(p => p.Time);
            AddIncludes();
            ApplyPagination(pageSize * (pageNumber - 1), pageSize);
        }
        public PostSpecification(Expression<Func<Post, bool>>? expression) : base(expression)
        {
            AddOrderByDesc(p => p.Time);
            AddIncludes();
        }

        private protected override void AddIncludes()
        {
            Includes.Add(p => p.Reactions!);
            Includes.Add(p => p.Comments!);
            Includes.Add(p => p.User);

            base.AddIncludes();
        }
    }
}
