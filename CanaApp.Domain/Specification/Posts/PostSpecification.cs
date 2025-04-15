using CanaApp.Domain.Entities.Comunity;
using System.Linq.Expressions;

namespace CanaApp.Domain.Specification.Posts
{
    public class PostSpecification : Specification<Post, int>
    {
        public PostSpecification() : base()
        {
            AddOrderByDesc(p => p.Time);
        }
        public PostSpecification(Expression<Func<Post, bool>>? expression) : base(expression)
        {
            AddOrderByDesc(p => p.Time);
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
