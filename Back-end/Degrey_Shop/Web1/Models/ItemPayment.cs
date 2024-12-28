using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Web1.Models
{
	[Table("Payment")]
	public class ItemPayment
	{
		
			[Key]
			public int Id_Payment { get; set; }
			public string Name { get; set; }
			public int Status { get; set; }
			public DateTime Paid_Date { get; set; }
			public int fk_Id_Order { get; set; }
			

	}
}
