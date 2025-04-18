using CanaApp.Domain.Entities.Models;
using System.Linq.Expressions;

namespace CanaApp.Domain.Specification.Models
{
    public class PatientSpec : Specification<Patient, int>
    {
        public PatientSpec(Expression<Func<Patient, bool>>? expression) : base(expression)
        {

        }
        private protected override void AddIncludes()
        {
            Includes.Add(x => x.ApplicationUser);
        }
    }
}
