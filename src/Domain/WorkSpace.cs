using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exadel.OfficeBooking.Domain
{
    public class WorkSpace : BaseModel
    {
        public int OfficeId { get; set; }

        public int Floor { get; set; }

        public string Name { get; set; }

    }
}
