using CanaApp.Domain.Entities.Models;
using System.Linq.Expressions;

namespace CanaApp.Domain.Specification.Models
{
    public class PsychiatristSpecification : Specification<Psychiatrist, string>
    {
        public PsychiatristSpecification(Expression<Func<Psychiatrist, bool>>? expression) : base(expression)
        {
            AddIncludes();
        }
        private protected override void AddIncludes()
        {
            Includes.Add(x => x.ApplicationUser);
        }
    }
 
}
