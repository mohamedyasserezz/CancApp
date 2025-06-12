using CanaApp.Domain.Entities.Records;
using System.Linq.Expressions;

namespace CanaApp.Domain.Specification.Records
{
    public class RecordSpecification : Specification<Record, int>
    {
        public RecordSpecification(Expression<Func<Record, bool>>? expression) : base(expression)
        {
            AddIncludes();
        }
        private protected override void AddIncludes()
        {
            Includes.Add(r => r.User);
            base.AddIncludes();
        }
    }
}
