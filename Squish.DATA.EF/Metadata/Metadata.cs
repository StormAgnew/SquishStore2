using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Squish.DATA.EF//.Metadata
{
    #region SquishSpecies
    public class SquishSpeciesMetadata
    {
        public int SpeciesID { get; set; }

        [Required (ErrorMessage = "You need a name for the Species!")]
        [StringLength(120)]
        [Display(Name = "Species of Squish")]
        public string SpeciesName { get; set; }

        [StringLength(50)]
        [Display(Name = "Description of Squish")]
        public string? SpeciesDescription { get; set; }

    }
    #endregion

    #region SquishInformation
    public class SquishInformationMetadata
    {
        public int SquishID { get; set; }

        [Required (ErrorMessage = "Your squish must have a name!")]
        [StringLength(150)]
        public string SquishName { get; set; }

        public int SpeciesID { get; set; } //FK

        public string? SpeciesDescription { get; set; } //FK Unknown if needed, will ask about


        [StringLength(200)]
        public string? Descritption { get; set; }

        
        public int Seasonalid { get; set; }

        [StringLength (50)]
        public string? SquishSize { get; set; }

        [StringLength(50)]
        public string? SquishColor { get; set; }


        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString ="{0:c}")]
        [Display(Name = "Price")]
        [Range(0, (double)decimal.MaxValue)]
        [Required]
        public decimal Price { get; set; }

        public int StatusID { get; set; }


        public string? SquishPic { get; set; }

    }
    #endregion

    #region Status
    public class StatusMetadata
    {
        public int? InStock { get; set; }

        public int? OutofStock { get; set;  }

        public int StatusID { get; set;  }
    }
    #endregion

    #region Orders
    public class OrderMetadata
    {
        public int OrderID { get; set; }  

        public int Quantity { get; set; }

        public int SquishID { get; set; }
    }
    #endregion

    #region Shipping
    public class ShippingInformationMetadata
    {
        public int ShippingID { get; set; }

        
        [StringLength(50)]
        [Required]
        public string Firstname { get; set; }

        [StringLength(50)]
        [Required]
        public string Lastname { get; set; }

        [StringLength(250)]
        [Required]
        public string Address { get; set; }

        [StringLength(50)]
        [Required]
        public string City { get; set; }


        [StringLength(2)]
        public string? State { get; set; }

        [StringLength(5)]
        public string? ZipCode { get; set; }

        public int OrderID { get; set; }

        public int UserID { get; set; }
    }
    #endregion

    #region UserAccount
    public class UserAccountMetadata
    {
        public string UserID { get; set; } = null!;

        [StringLength(50)]
        [Required]
        public string FirstName { get; set; } = null!;

        [StringLength(50)]
        [Required]
        public string LastName { get; set; } = null!;

        [StringLength(250)]
        [Required]
        public string Address { get; set; } = null!;

        [StringLength(50)]
        [Required]
        public string City { get; set; } = null!;
        [StringLength(2)]
        public string? State { get; set; }

        [StringLength(5)]
        public string? ZipCode { get; set; }
    }
    #endregion


}
