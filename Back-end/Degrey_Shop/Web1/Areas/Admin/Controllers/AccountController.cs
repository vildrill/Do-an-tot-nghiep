using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BC = BCrypt.Net.BCrypt;
using Web1.Models;


namespace Web1.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class AccountController : Controller
	{
		public MyDB db = new MyDB();
		public IActionResult Login()
		{
			return View("Login");
		}
		public IActionResult LoginPost(string email, string password)
		{

			
			if (email != null && password != null)
			{
				if(email =="admin@gmail.com" && password == "123")
					HttpContext.Session.SetString("admin_email", email);
					return Redirect("/Admin/Home");
				
			}
			return Redirect("/Admin/Account/Login?notify=invalid");
		}
		public IActionResult Loggout()
		{
			HttpContext.Session.Remove("admin_email");
			return Redirect("/Admin/Account/Login");
		}
	}
}
