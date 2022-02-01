using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exadel.OfficeBooking.Domain
{
    class Office : BaseModel
    {
        public int CountryId { get; set; }

        public int Floors { get; set; }

    }
}
