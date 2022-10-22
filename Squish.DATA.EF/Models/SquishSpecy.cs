using System;
using System.Collections.Generic;

namespace Squish.DATA.EF.Models
{
    public partial class SquishSpecy
    {
        public SquishSpecy()
        {
            SquishInformations = new HashSet<SquishInformation>();
        }

        public int SpeciesId { get; set; }
        public string SpeciesName { get; set; } = null!;
        public string? SpeciesDescription { get; set; }

        public virtual ICollection<SquishInformation> SquishInformations { get; set; }
    }
}
