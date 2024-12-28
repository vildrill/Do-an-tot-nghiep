using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web1.Models
{
    [Table("Orders")]
    public class ItemOrder
    {
        [Key]
        public int Id_Order { get; set; }
        public int Status { get; set; }
        public DateTime Create_Date { get; set; }
        public double Total_Price { get; set; }
		public int fk_Id_User { get; set; }
        public double Discount { get; set; }
		public int fk_Id_Discount { get; set; }
		public int Payment { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }

    }
}
