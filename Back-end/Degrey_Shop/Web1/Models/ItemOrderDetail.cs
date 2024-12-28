using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web1.Models
{
    [Table("Order_Detail")] 
    public class ItemOrderDetail
    {
        [Key]
        public int Id_OrderDetail { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
		public int fk_Id_Order { get; set; }
		public int fk_Id_Product { get; set; }
        public string color { get; set; }
        public string size { get; set; }
	}
}
