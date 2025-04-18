using CanaApp.Domain.Entities.Models;
using System.Linq.Expressions;

namespace CanaApp.Domain.Specification.Models
{
    public class PatientSpecification : Specification<Patient, string>
    {
        public PatientSpecification(Expression<Func<Patient, bool>>? expression) : base(expression)
        {
            AddIncludes();
        }
        private protected override void AddIncludes()
        {
            Includes.Add(x => x.ApplicationUser);
        }
    }
}
