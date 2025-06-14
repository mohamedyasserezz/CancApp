using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancApp.Shared.Models.User.Pharmacy
{
    public record PharmacistResponse(
        string Name,
        string Image,
        string Address,
        bool IsDeliveryEnabled,
        float Latitude,
        float Longitude,
        bool IsOpeningNow
        );
}
