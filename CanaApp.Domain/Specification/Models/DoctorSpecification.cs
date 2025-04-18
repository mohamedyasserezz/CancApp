using CanaApp.Domain.Entities.Models;
using System.Linq.Expressions;

namespace CanaApp.Domain.Specification.Models
{
    public class DoctorSpecification : Specification<Doctor, string>
    {
        public DoctorSpecification(Expression<Func<Doctor, bool>>? expression) : base(expression)
        {
            AddIncludes();
        }
        private protected override void AddIncludes()
        {
            Includes.Add(x => x.ApplicationUser);
        }
    }

}
