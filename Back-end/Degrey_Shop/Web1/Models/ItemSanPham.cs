using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web1.Models
{
	[Table("Products")]
	public class ItemSanPham
	{
		[Key]
		public int Id_Product { get; set; }
		public int fk_Id_Category { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Discount { get; set; }
        public double Price { get; set; }
        public string Photo { get; set; }
		public string note {  get; set; }
    }
}
