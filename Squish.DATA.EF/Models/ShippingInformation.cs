using System;
using System.Collections.Generic;

namespace Squish.DATA.EF.Models
{
    public partial class ShippingInformation
    {
        public int ShippingId { get; set; }
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public int OrderId { get; set; }
        public string UserId { get; set; } = null!;

        public virtual Order Order { get; set; } = null!;
        public virtual UserAccountInfo? User { get; set; } = null!;
    }
}
