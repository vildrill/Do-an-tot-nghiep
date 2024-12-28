using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web2.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        // GET: Admin/User
        public ActionResult Login ()
        {
            return View();
        }
		[HttpPost]
		public ActionResult DangNhap(string email, string mk)
		{

			var user = db.NguoiDungs.FirstOrDefault(u => u.email == email);


			if (email != null && mk != null && email.ToLower() == "admin@gmail.com" && mk.ToLower() == "1")
			{
				Session["tk"] = "AdminTechShop";
				return RedirectToAction("ThongKe", "SanPham");
			}
			if (user != null && HashingHelper.VerifyPassword(mk, user.matKhau))
			{
				// Đăng nhập thành công
				Session["id_NguoiDung"] = user.id_NguoiDung;
				Session["TenNguoiDung"] = user.ten;
				Session["SoDienThoai"] = user.sdt;
				Session["DiaChi"] = user.diaChi;

				ViewBag.Success = "Đăng Nhập thành công";
				TempData["SuccessMessage"] = "Thêm sản phẩm thành công";

				// Gọi hàm JavaScript để hiển thị thông báo
				ViewBag.NotificationMessage = TempData["SuccessMessage"];
				return RedirectToAction("Home", "TrangChu", new { area = "" });

			}
			else
			{
				// Đăng nhập không thành công
				ViewBag.Error = "(*)Số điện thoại hoặc mật khẩu không đúng.";
				return View();
			}
		}

	}
}