using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Web1.Models
{
    [Table("Size")]
    public class ItemKichCo
    {
        [Key]
        public int Id_Size { get; set; }
        public string Name { get; set; }
        public int fk_Id_Product { get; set; }

    }
}
