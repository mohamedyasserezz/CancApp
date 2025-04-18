using CanaApp.Domain.Entities.Models;
using System.Linq.Expressions;

namespace CanaApp.Domain.Specification.Models
{
    public class VolunteerSpecification : Specification<Volunteer, string>
    {
        public VolunteerSpecification(Expression<Func<Volunteer, bool>>? expression) : base(expression)
        {
            AddIncludes();
        }
        private protected override void AddIncludes()
        {
            Includes.Add(x => x.ApplicationUser);
        }
    }

}
