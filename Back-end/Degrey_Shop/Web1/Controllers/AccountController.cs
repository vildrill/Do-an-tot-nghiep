using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web1.Models;
using BC = BCrypt.Net.BCrypt;

namespace Web1.Controllers
{
    public class AccountController : Controller
    {
        public MyDB db = new MyDB();
        public IActionResult Register()
        {
			var cities = db.City.ToList();

			return View(cities);
        }
        [HttpPost]
        public IActionResult RegisterPost(IFormCollection fc)
        {
            string _name = fc["name"];
            string _email = fc["email"];
            string _phone = fc["phone"];
            string _password = fc["password"];
            string _id_tinh = fc["idTinh"];
            string _id_huyen = fc["district"];
            string _id_xa = fc["wardSelect"];
            int checkMail = db.Users.Where(item => item.Email == _email).Count();
            if (checkMail == 0)
            {
                ItemTaiKhoan record = new ItemTaiKhoan();
                record.Name = _name;
                record.Email = _email;
                record.Phone = _phone;
                record.Password = _password;
                record.id_tinh = _id_tinh;
                record.id_huyen = _id_huyen;
                record.id_xa = _id_xa;

				db.Users.Add(record);
                db.SaveChanges();
            }
            else
            {
                return Redirect("/Account/Register?notify=exists");
            }
            return Redirect("/Account/Login?notify=register-success");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LoginPost(IFormCollection fc)
        {

            string _email = fc["email"];
            string _password = fc["password"];
            ItemTaiKhoan record = db.Users.Where(item => item.Email == _email).FirstOrDefault();
            if (record != null)
            {
                HttpContext.Session.SetString("customer_email", record.Email.ToString());
                HttpContext.Session.SetString("customer_id", record.Id_User.ToString());
				HttpContext.Session.SetString("customer_Phone", record.Phone.ToString());
				HttpContext.Session.SetString("customer_Name", record.Name.ToString());

			}
			else
                return Redirect("/Account/Login?notify=login-invalid");
            return Redirect("/Home");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("customer_email");
            HttpContext.Session.Remove("customer_id");
            HttpContext.Session.Remove("customer_Name");
            HttpContext.Session.Remove("customer_Phone");


            return Redirect("/Home");
        }
    }
}
