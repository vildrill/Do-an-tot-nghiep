using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Web1.Models
{
    [Table("Categories")]
    public class ItemLoaiSP
    {
        [Key]
        public int Id_Category { get; set; }
        public string Name { get; set; }

    }
}
