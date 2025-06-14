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
        public PostSpecification(bool IsTopPostsEnabled , int NumberOfPosts = 0)
        {
            AddIncludes();
            if (IsTopPostsEnabled)
            {
                IsGettingTopQuery = true;
                AddOrderBy(p => p.Comments.Count + p.Reactions.Count);
            }
            else
                AddOrderByDesc(p => p.Time);
            if (NumberOfPosts > 0)
                Take = NumberOfPosts;
        }
        public PostSpecification()
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
