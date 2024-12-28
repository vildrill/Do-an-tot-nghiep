using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web1.Models;

namespace Web1.Controllers
{

    public class HomeController : Controller
    {
        public MyDB db = new MyDB();

        public IActionResult Index(int? value)
        {
            List<ItemSanPham> list_record = new List<ItemSanPham>();

            if (value == null || value == 1)
            {
                // Lấy danh sách 8 sản phẩm bán chạy nhất dựa trên bảng chi tiết đơn hàng
                list_record = db.OrdersDetail
                                 .GroupBy(od => od.fk_Id_Product)
                                 .Select(g => new {
                                     ProductId = g.Key,
                                     TotalQuantity = g.Sum(od => od.Quantity)
                                 })
                                 .OrderByDescending(g => g.TotalQuantity)
                                 .Take(6)
                                 .Join(db.Products,
                                       g => g.ProductId,
                                       p => p.Id_Product,
                                       (g, p) => new ItemSanPham
                                       {
                                           Id_Product = p.Id_Product,
                                           fk_Id_Category = p.fk_Id_Category,
                                           Name = p.Name,
                                           Description = p.Description,
                                           Discount = p.Discount,
                                           Price = p.Price,
                                           Photo = p.Photo,
                                           note = p.note
                                       })
                                 .ToList();
            }
            else if (value == 2)
            {
                // Lấy danh sách 8 sản phẩm mới nhất
                list_record = db.Products
                                 .OrderByDescending(p => p.Id_Product) // Sửa đổi thứ tự sắp xếp để lấy sản phẩm mới nhất
                                 .Take(6)
                                 .ToList();
            }
            else
            {
                // Lấy danh sách 8 sản phẩm giảm giá nhiều nhất
                list_record = db.Products
                                 .OrderByDescending(p => p.Discount)
                                 .Take(6)
                                 .ToList();
            }

            return View(list_record);
        }


        [HttpGet]
        public IActionResult SearchProducts(string key)   
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return Json(new List<ItemSanPham>());
            }

            var products = db.Products
                .Where(p => p.Name.Contains(key))
                .Select(p => new
                {
                    p.Id_Product,
                    p.Name,
                    p.Photo 
                })
                .ToList();

            return Json(products);
        }


    }
}
