using CanaApp.Domain.Entities.Models;
using System.Linq.Expressions;

namespace CanaApp.Domain.Specification.Models
{
    public class ApplicationUserSpecification : Specification<ApplicationUser, string>
    {

        public ApplicationUserSpecification(Expression<Func<ApplicationUser, bool>>? expression) : base(expression)
        {
            AddIncludes();
        }
        private protected override void AddIncludes()
        {
            Includes.Add(u => u.FcmTokens);
            base.AddIncludes();
        }
    }
}
