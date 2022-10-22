using System;
using System.Collections.Generic;

namespace Squish.DATA.EF.Models
{
    public partial class UserAccountInfo
    {
        public UserAccountInfo()
        {
            ShippingInformations = new HashSet<ShippingInformation>();
        }

        public string UserId { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string? State { get; set; }
        public string? ZipCode { get; set; }

        public virtual ICollection<ShippingInformation> ShippingInformations { get; set; }
    }
}
