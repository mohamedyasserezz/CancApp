using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancApp.Shared.Models.Dashboard
{
    public class DashboardRequestValidator : AbstractValidator<DashboardRequest>
    {
        public DashboardRequestValidator()
        {
            RuleFor(x => x.id)
                .NotEmpty()
                .WithMessage("id can't be empty");
        }
    }
}
