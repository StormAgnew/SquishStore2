using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;


namespace Squish.DATA.EF.Models//.Metadata
{
	#region Species
	[ModelMetadataType(typeof(SquishSpeciesMetadata))]
	public partial class SquishSpecies { }
	#endregion

	#region SquishInfo
	[ModelMetadataType(typeof(SquishInformationMetadata))]
	public partial class SquishInformation 
	{
		[NotMapped]
		public IFormFile Image { get; set; }
	}

	#endregion

	#region Status
	[ModelMetadataType(typeof(StatusMetadata))]
	public partial class Status { }
	#endregion



	#region Orders
	[ModelMetadataType(typeof(OrderMetadata))]
	public partial class Order { }
	#endregion

	#region Shipping
	[ModelMetadataType(typeof(ShippingMetadata))]
	public partial class Shipping { }
	#endregion

	#region UserAccount
	[ModelMetadataType(typeof(UserAccountMetadata))]
	public partial class UserAccount 
	{ 
		//public string FullName { get { return $"{FirstName} {LastName}"; } } NOT FUNCTIONAL
	}
	#endregion
}
