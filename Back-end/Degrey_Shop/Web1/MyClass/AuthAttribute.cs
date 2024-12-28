using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Web1.MyClass
{
    public class AuthAttribute : ActionFilterAttribute
    {
        public class AdminAuthAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                var userAdmin = filterContext.HttpContext.Session.GetString("customer_id");
                if (userAdmin == null)
                {
                    filterContext.Result = new RedirectToActionResult("LoginPost", "Account", new { area = "Admin" });
                }
                base.OnActionExecuting(filterContext);
            }
        }

        public class UserAuthAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                var user = filterContext.HttpContext.Session.GetString("id_NguoiDung");
                if (user == null)
                {
                    filterContext.Result = new RedirectToActionResult("LoginPost", "Account", new { area = "Admin" });
                }
                base.OnActionExecuting(filterContext);
            }
        }
    }
}
