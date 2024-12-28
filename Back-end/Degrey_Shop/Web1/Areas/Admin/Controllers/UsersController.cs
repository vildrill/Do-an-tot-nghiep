using Microsoft.AspNetCore.Mvc;
using Web1.Areas.Admin.Attributes;
//doi tuong phan trang
using X.PagedList;
//su dung kieu List
using System.Collections.Generic;
//de nhin thay file CheckLogin.cs trong thu muc Attributes
using System.Security.Cryptography;
using BC = BCrypt.Net.BCrypt;
using Web1.Models;

namespace Web1.Areas.Admin.Controllers
{
	[Area("Admin")]
	//[CheckLogin]
	public class UsersController : Controller
	{
		public MyDB db = new MyDB();
		public IActionResult Index(int? page)
		{
			//lấy trang hiện tại
			int current_page = page ?? 1;
			//định nghĩa số bản ghi trên một trang
			int record_per_page = 4;
			//lấy tất cả các bản ghi trong table Users
			List<ItemTaiKhoan> list_record = db.Users.OrderByDescending(item => item.Id_User).ToList();
			//truyền giá trị ra view có phân trang
			return View("Index", list_record.ToPagedList(current_page, record_per_page));
		}
        public IActionResult Update(int? id)
        {
            int _id = id ?? 0;
            //lay mot ban ghi
            ItemTaiKhoan record = db.Users.Where(item => item.Id_User == _id).FirstOrDefault();
            //tạo biến action để đưa vào thuộc tính action của thẻ form
            ViewBag.action = "/Admin/Users/UpdatePost/" + _id;
            //gọi view, truyền dữ liệu ra view
            return View("CreateUpdate", record);
        }
        [HttpPost]
        public IActionResult UpdatePost(IFormCollection fc, int? id)
        {
            //lấy dữ liệu của thẻ form thông qua đối tượng fc
            string _name = fc["name"].ToString().Trim();
            //cũng có thể sử dụng đối tượng Request.Form["ten the form"]
            string _password = Request.Form["password"].ToString().Trim();
            //lấy một bản ghi
            ItemTaiKhoan record = db.Users.Where(item => item.Id_User == id).FirstOrDefault();
            if (record != null)
            {
                record.Name = _name;
                //nếu password không rỗng thì update password
                if (!String.IsNullOrEmpty(_password))
                {
                    //mã hóa password
                    _password = BC.HashPassword(_password);
                    record.Password = _password;
                }
            }
            //cập nhật lại table
            db.SaveChanges();
            //di chuyển đến action có tên là Index
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            //tạo biến action để đưa vào thuộc tính action của thẻ form
            ViewBag.action = "/Admin/Users/CreatePost";
            return View("CreateUpdate");
        }

        [HttpPost]
        public IActionResult CreatePost(IFormCollection fc)
        {
            //lấy dữ liệu của thẻ form thông qua đối tượng fc
            string _name = fc["name"].ToString().Trim();
            string _email = fc["email"].ToString().Trim();
            string _address = fc["address"].ToString().Trim();
            string _phone = fc["phone"].ToString().Trim();
            string _password = fc["password"].ToString().Trim();



            //lay mot ban ghi
            ItemTaiKhoan record = new ItemTaiKhoan();
            record.Name = _name;
            record.Email = _email;
            record.Phone = _phone;
            record.Password = _password;

            //them ban ghi vao table Products
            db.Users.Add(record);
            db.SaveChanges();

            //---
            //di chuyển đến action có tên là Index
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int? id)
        {
            int _id = id ?? 0;
            //lay mot ban ghi
            ItemTaiKhoan record = db.Users.Where(item => item.Id_User == _id).FirstOrDefault();
            //xoa ban ghi khoi csdl
            db.Users.Remove(record);
            //cap nhat lai table Products
            db.SaveChanges();
            //di chuyển đến action có tên là Index
            return RedirectToAction("Index");
        }
    }
}
