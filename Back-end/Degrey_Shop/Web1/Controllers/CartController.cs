using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Security;
using Web1.Models;

namespace Web1.Controllers
{
    public class CartController : Controller
    {

        public MyDB db = new MyDB();
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetProducts([FromBody] List<int> productIds)
        {
            var products = db.Products.Where(product => productIds.Contains(product.Id_Product)).ToList();

            return Json(products);
        }



        //[Authorize]
        public IActionResult Checkout(string totalPrice)
        {
            var cities = db.City.ToList();
            var customerName = HttpContext.Session.GetString("customer_Name");
            var customerID = HttpContext.Session.GetString("customer_id");
            var customerPhone = HttpContext.Session.GetString("customer_Phone");
            var customerEmail = HttpContext.Session.GetString("customer_email");
			int id_user = Convert.ToInt32(customerID);
			var User = db.Users.FirstOrDefault(p => p.Id_User == id_user);

			if (customerID == null)
            {
                ViewBag.totalPrice = totalPrice;
                return Redirect("/Account/Login");

            }
            else {

                ViewBag.name = customerName;
                ViewBag.Id = customerID;
                ViewBag.Phone = customerPhone;
                ViewBag.Email = customerEmail;
                ViewBag.totalPrice = totalPrice;
                ViewBag.id_xa = User.id_xa;
                ViewBag.id_huyen = User.id_huyen;
                ViewBag.id_tinh = User.id_tinh;
                return View(cities);
            }


        }

        [HttpGet]
        public IActionResult danhSachQuanHuyen(int? id_city)
        {
            if (id_city == null)
            {
                var districts = db.District.ToList();
                return Json(districts);
            }

            var filteredDistricts = db.District.Where(d => d.City_Id == id_city).ToList();
            return Json(filteredDistricts);
        }

        [HttpGet]
        public IActionResult danhSachXaPhuong(int? id_district)
        {
            if (id_district == null)
            {
                var wards = db.Ward.ToList();
                return Json(wards);
            }

            var data = db.Ward.Where(m => m.District_Id == id_district).ToList();   
            return Json(data);
        }

        [HttpGet]
        public IActionResult SearchGiamGia(string? key)
        {
            // Lấy ngày hiện tại
            var currentDate = DateTime.Now;

            // Lấy tất cả các khuyến mãi chưa hết hạn
            var discountsQuery = db.Discount.Where(d => d.End_Date > currentDate);

            // Nếu từ khóa tìm kiếm không rỗng, áp dụng bộ lọc
            if (!string.IsNullOrEmpty(key))
            {
                var lowerKey = key.ToLower();
                discountsQuery = discountsQuery.Where(m => m.Name.ToLower().Contains(lowerKey));
            }

            // Lấy danh sách khuyến mãi sau khi áp dụng bộ lọc
            var discounts = discountsQuery.ToList();

            return Json(discounts);
        }


        [HttpPost]
        public IActionResult KiemTraMaGiamGia(string maGiamGia)
        {
            if (int.TryParse(maGiamGia, out int idDiscount))
            {
                var discount = db.Discount.FirstOrDefault(m => m.Id_Discount == idDiscount);
                if (discount != null)
                {
                    if (discount.End_Date > DateTime.Now)
                    {
                        return Json(new { success = true, percent = discount.Percent });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Discount code has expired." });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Discount code does not exist." });
                }
            }
            else
            {
                return Json(new { success = false, message = "Invalid discount code." });
            }
        }






    }
}
