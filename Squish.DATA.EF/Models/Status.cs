using System;
using System.Collections.Generic;

namespace Squish.DATA.EF.Models
{
    public partial class Status
    {
        public Status()
        {
            SquishInformations = new HashSet<SquishInformation>();
        }

        public int? InStock { get; set; }
        public int? OutofStock { get; set; }
        public int StatusId { get; set; }

        public virtual ICollection<SquishInformation> SquishInformations { get; set; }
    }
}
