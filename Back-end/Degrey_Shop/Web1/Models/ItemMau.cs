using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Web1.Models
{
    [Table("Color")]
    public class ItemMau
    {
        [Key]
        public int Id_Color { get; set; }
        public string Name { get; set; }
		public string Photo { get; set; }
        public int fk_Id_Product { get; set; }
        public string Color_Code { get; set; }

    }
}
