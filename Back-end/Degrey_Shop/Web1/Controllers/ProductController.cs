using Microsoft.AspNetCore.Mvc;
using Web1.Models;
using X.PagedList;

namespace Web1.Controllers
{
    public class ProductController : Controller
    {
        public MyDB db = new MyDB();
        public IActionResult Index(int? page, int? fk_Id_Category, int? filter)
        {
            int current_page = page ?? 1;
            int record_per_page = 9;

            IQueryable<ItemSanPham> query = db.Products;

            if (fk_Id_Category != null && fk_Id_Category != 0)
            {
                query = query.Where(item => item.fk_Id_Category == fk_Id_Category);
                var loai = db.Categories.FirstOrDefault(m => m.Id_Category == fk_Id_Category);
                ViewBag.Categories = loai.Name;
            }

            if (filter == 1)
            {
                query = query.OrderByDescending(item => item.Price);
            }
            else
            {
                query = query.OrderBy(item => item.Price);
            }

            var list_record = query.ToList();

            ViewBag.fk_Id_Category = fk_Id_Category;
            ViewBag.filter = filter;

            return View("Index", list_record.ToPagedList(current_page, record_per_page));
        }




        public IActionResult Detail(int? id)
        {
            int _id = id ?? 0;
            ItemSanPham record = db.Products.Where(item => item.Id_Product == _id).FirstOrDefault();
            return View("Detail", record);
        }


        [HttpPost]
        public JsonResult SubmitReview(int id_sanPham, int id_NguoiDung, int rating, string review)
        {
            try
            {
                // Lấy danh sách các đơn hàng của người dùng
                var userOrders = db.Orders
                    .Where(order => order.fk_Id_User == id_NguoiDung)
                    .Select(order => order.Id_Order)
                    .ToList();

                // Kiểm tra xem người dùng có mua sản phẩm này hay không
                bool hasPurchased = db.OrdersDetail
                    .Any(od => userOrders.Contains(od.fk_Id_Order) && od.fk_Id_Product == id_sanPham);

                if (hasPurchased)
                {
                    var ratings = new ItemRating
                    {
                        fk_Id_Product = id_sanPham,
                        fk_Id_User = id_NguoiDung,
                        Rating = rating,
                        Comment = review,
                        Rate_Date = DateTime.Now.Date
                    };

                    db.Rating.Add(ratings);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Bạn cần mua sản phẩm trước khi đánh giá." });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                return Json(new { success = false, error = ex.Message });
            }
        }



        [HttpGet]
        public JsonResult GetColor(int idMau)
        {
            var color = db.Colors.FirstOrDefault(c => c.Id_Color == idMau);
            if (color != null)
            {
               
                return Json(new { success = true, color = color });
            }
            else
            {
                return Json(new { success = false, message = "Image not found." });
            }
        }

    }
}
