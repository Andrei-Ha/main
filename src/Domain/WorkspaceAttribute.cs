using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exadel.OfficeBooking.Domain
{
    public class WorkspaceAttribute : BaseModel
    {
        public int WorkSpaceId { get; set; }

        public int AttributeId { get; set; }
    }
}
