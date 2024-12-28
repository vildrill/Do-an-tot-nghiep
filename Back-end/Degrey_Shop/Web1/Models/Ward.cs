using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace Web1.Models
{
	[Table("Ward")]
	public class Ward
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }
		public string Ward_Code { get; set; }
		public int District_Id { get; set; }
		public int City_Id { get; set; }
	}
}
