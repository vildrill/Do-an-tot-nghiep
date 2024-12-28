using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web1.Models
{
	[Table("Users")]
	public class ItemTaiKhoan
	{
		[Key]
		public int Id_User { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Password { get; set; }
		public string id_tinh { get; set; }
		public string id_huyen { get; set; }	
		public string id_xa { get; set; }

	}
}
