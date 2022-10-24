using System;
using System.Collections.Generic;

namespace Squish.DATA.EF.Models
{
    public partial class SquishInformation
    {
        public SquishInformation()
        {
            Orders = new HashSet<Order>();
        }

        public int SquishId { get; set; }
        public string Squishname { get; set; } = null!;
        public int SpeciesId { get; set; }
        public string? Description { get; set; }
        public int Seasonalid { get; set; }
        public string? SquishSize { get; set; }
        public string? SquishColor { get; set; }
        public decimal Price { get; set; }
        public int? StatusId { get; set; }

        public virtual SquishSpecy? Species { get; set; } = null!;
        public virtual Status? Status { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
    }
}
