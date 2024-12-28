using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using Web1.Models;
using Microsoft.EntityFrameworkCore;
using Web1.Areas.Admin.Attributes;


namespace Web1.Areas.Admin.Controllers
{
	[Area("Admin")]
	//[CheckLogin]
	public class SizeController : Controller
    {
        public MyDB db = new MyDB();
        public IActionResult Index(int? page, int fk_Id_Product) 
        {
            int _RecordPerPage = 6;
            int _CurrentPage = page ?? 1;
            List<ItemKichCo> _ListRecord = db.Sizes.Where(item => item.fk_Id_Product == fk_Id_Product).OrderByDescending(item => item.Id_Size).ToList();
            ViewBag.id_sanPham = fk_Id_Product;
            return View("Index", _ListRecord.ToPagedList(_CurrentPage, _RecordPerPage));
        }
        public IActionResult Update(int? id)
        {
            int _id = id ?? 0;
            ItemKichCo model = db.Sizes.FirstOrDefault(item => item.Id_Size == _id);
            return View(model);
        }

        [HttpPost]
        public IActionResult UpdatePost(IFormCollection fc)
        {
            string _TenSize = fc["name"].ToString().Trim();
            int _id = Convert.ToInt32(fc["Id_Size"].ToString().Trim());

            var record = db.Sizes.FirstOrDefault(item => item.Id_Size == _id);
            if (record != null)
            {
                record.Name = _TenSize;

                db.SaveChanges();

                // Redirect to Index with the necessary parameters
                return RedirectToAction("Index", "Size", new { area = "Admin", page = 1, fk_Id_Product = record.fk_Id_Product });
            }

            return RedirectToAction("Index", "Size", new { area = "Admin", page = 1, fk_Id_Product = 0 }); // Trường hợp bản ghi không tồn tại
        }


        public IActionResult Create(int fk_Id_Product)
        {

            var product = db.Products.FirstOrDefault(sp => sp.Id_Product == fk_Id_Product);
            return View(product);
        }
        [HttpPost]
        public IActionResult CreatePost(IFormCollection fc)
        {           
            string id_sanPham= fc["Id_Product"].ToString().Trim();
            string _TenMau = fc["name"].ToString().Trim();
            int _MaSP = Convert.ToInt32(fc["Id_Product"].ToString().Trim());

            ItemKichCo record = new ItemKichCo();
            record.Name = _TenMau;
            record.fk_Id_Product = _MaSP;

            db.Sizes.Add(record);
            db.SaveChanges();


            return Redirect("/Admin/Product");
        }
        public IActionResult Delete(int? id)
        {
            int _id = id ?? 0;
            var record = db.Sizes.Where(item => item.Id_Size == _id).FirstOrDefault();

            db.Sizes.Remove(record);
            db.SaveChanges();
            return Redirect("/Admin/Size");
        }

    }
}
