using CanaApp.Domain.Entities.Comunity;

namespace CanaApp.Domain.Specification.Reactions
{
    public class ReactionSpecification : Specification<Reaction, int>
    {
        public ReactionSpecification() :
            base()
        {
            AddOrderByDesc(R => R.Time);
        }

    }
}
