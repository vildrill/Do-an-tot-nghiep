using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace Web1.Models
{
	[Table("District")]
	public class District
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }
		public string District_Code { get; set; }
		public int City_Id { get; set; }
	}
}
