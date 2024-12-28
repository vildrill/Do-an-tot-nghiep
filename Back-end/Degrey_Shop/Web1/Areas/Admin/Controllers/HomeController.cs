using Microsoft.AspNetCore.Mvc;
using Web1.Areas.Admin.Attributes;
using Web1.Models;

namespace Web1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [CheckLogin]
    public class HomeController : Controller
    {
		MyDB db = new MyDB();

		public IActionResult Index()
		{
			var totalOrders = db.Orders.Count();
			var totalUsers = db.Users.Count();
			var totalAmount = db.Orders.Sum(order => order.Total_Price);
            var totalRate = db.Rating.Count();

			ViewBag.TotalOrders = totalOrders;
			ViewBag.TotalUsers = totalUsers;
			ViewBag.TotalAmount = totalAmount;
            ViewBag.TotalRate = totalRate;

			return View();
		}





		public ActionResult GetChartData()
        {
            var chartData = db.Orders
                             .Where(h => h.Create_Date != null) 
                             .GroupBy(h => new {
                                 Year = h.Create_Date.Year,
                                 Month = h.Create_Date.Month
                             })
                             .Select(g => new {
                                 Year = g.Key.Year,
                                 Month = g.Key.Month,
                                 Total = g.Sum(h => h.Total_Price) 
                             })
                             .OrderBy(g => g.Year) 
                             .ThenBy(g => g.Month) 
                             .Select(g => new {
                                 Month = g.Month + "-" + g.Year,
                                 Total = g.Total
                             })
                             .ToList();

            return Json(chartData);
        }




    }
}
