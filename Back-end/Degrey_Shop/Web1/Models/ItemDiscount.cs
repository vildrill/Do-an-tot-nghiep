using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Web1.Models
{
	[Table("Discount")]
	public class ItemDiscount
	{
		[Key]
		public int Id_Discount { get; set; }
		public string Name { get; set; }
        public double Percent { get; set; }
		public string Description { get; set; }
		public int Status { get; set; }
		public DateTime End_Date { get; set; }
		public string? Photo { get; set; }
	}
}
