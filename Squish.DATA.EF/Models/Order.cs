using System;
using System.Collections.Generic;

namespace Squish.DATA.EF.Models
{
    public partial class Order
    {
        public Order()
        {
            ShippingInformations = new HashSet<ShippingInformation>();
        }

        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public int SquishId { get; set; }

        public virtual SquishInformation Squish { get; set; } = null!;
        public virtual ICollection<ShippingInformation> ShippingInformations { get; set; }
    }
}
