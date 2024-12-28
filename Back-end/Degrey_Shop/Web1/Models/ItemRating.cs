using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Web1.Models
{
	[Table("Rating")]
	public class ItemRating
	{
		[Key]
		public int Id_Rate { get; set; }
		public int Rating { get; set; }
		public string Comment { get; set; }
		public DateTime Rate_Date { get; set; }
        public int fk_Id_Product { get; set; }
		public int fk_Id_User { get; set; }
	}
}
