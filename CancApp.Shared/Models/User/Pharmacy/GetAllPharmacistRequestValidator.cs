using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancApp.Shared.Models.User.Pharmacy
{
    public class GetAllPharmacistRequestValidator : AbstractValidator<GetAllPharmacistRequest>
    {
        public GetAllPharmacistRequestValidator()
        {
            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90)
                .When(x => x.Latitude != null)
                .WithMessage("Latitude must be between -90 and 90");
            
            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180)
                .When(x => x.Longitude != null)
                .WithMessage("Longitude must be between -180 and 180");
        }
    }
}
