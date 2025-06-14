using CanaApp.Domain.Entities.Models;
using CanaApp.Domain.Entities.Records;
using System.Linq.Expressions;

namespace CanaApp.Domain.Specification.Models
{
    public class RecordAccessSpecification : Specification<RecordAccess , int>
    {

        public RecordAccessSpecification(Expression<Func<RecordAccess, bool>>? expression)
        {
            AddIncludes();
        }

        private protected override void AddIncludes()
        {
            Includes.Add(x => x.Doctor);
            Includes.Add(x => x.Patient);

            base.AddIncludes();
        }
    }
}
