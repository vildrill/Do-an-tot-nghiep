using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace Web1.Models
{
	[Table("City")]
	public class City
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }
		public string City_Code { get; set; }		
	}
}
