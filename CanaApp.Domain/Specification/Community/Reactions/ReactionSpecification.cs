using CanaApp.Domain.Entities.Comunity;
using System.Linq.Expressions;
namespace CanaApp.Domain.Specification.Community.Reactions
{
    public class ReactionSpecification : Specification<Reaction, int>
    {
        public ReactionSpecification() :
            base()
        {
            AddOrderByDesc(R => R.Time);
        }

        public ReactionSpecification(Expression<Func<Reaction, bool>>? expression) : base(expression)
        {
            AddOrderByDesc(R => R.Time);
        }
        
    }
}
