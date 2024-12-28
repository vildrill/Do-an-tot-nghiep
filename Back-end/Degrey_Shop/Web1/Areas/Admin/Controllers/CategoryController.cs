using Microsoft.AspNetCore.Mvc;
using Web1.Areas.Admin.Attributes;
using Web1.Models;
using X.PagedList;

namespace Web1.Areas.Admin.Controllers
{
    [Area("Admin")]
	//[CheckLogin]
	public class CategoryController : Controller
    {
        public MyDB db = new MyDB();
		public IActionResult Index(int? page)
        {
            int _RecordPerPage = 6;
            int _CurrentPage = page ?? 1;
            List<ItemLoaiSP> _ListRecord = db.Categories.OrderByDescending(item => item.Id_Category).ToList();
            return View("Index", _ListRecord.ToPagedList(_CurrentPage, _RecordPerPage));
        }
        public IActionResult Update(int? id)
        {
            int _id = id ?? 0;
            ItemLoaiSP record = db.Categories.Where(item => item.Id_Category == _id).FirstOrDefault();
            ViewBag.action = "/Admin/Category/UpdatePost/" + _id;
            return View("CreateUpdate", record);
        }

        [HttpPost]
        public IActionResult UpdatePost(IFormCollection fc, int? id)
        {

            string _Category_Name = fc["Category"].ToString().Trim();

            int _id = id ?? 0;
            var record = db.Categories.Where(item => item.Id_Category == _id).FirstOrDefault();

            record.Name = _Category_Name;

            db.SaveChanges();
            return Redirect("/Admin/Category");
        }

        public IActionResult Create()
        {
            ViewBag.action = "/Admin/Category/CreatePost";
            return View("CreateUpdate");
        }
        [HttpPost]
        public IActionResult CreatePost(IFormCollection fc)
        {
            string _Category_Name = fc["Category"].ToString().Trim();

            ItemLoaiSP record = new ItemLoaiSP();

            record.Name = _Category_Name;

            db.Categories.Add(record);
            db.SaveChanges();
            //---
            return Redirect("/Admin/Category");
        }
        public IActionResult Delete(int? id)
        {
            int _id = id ?? 0;
            var record = db.Categories.Where(item => item.Id_Category == _id).FirstOrDefault();

            db.Categories.Remove(record);
            db.SaveChanges();
            return Redirect("/Admin/Category");
        }
    }
}
