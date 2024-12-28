using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using Web1.Models;
using Microsoft.EntityFrameworkCore;
using Web1.Areas.Admin.Attributes;

namespace Web1.Areas.Admin.Controllers
{
	[Area("Admin")]
	[CheckLogin]
	public class ColorController : Controller
    {
        public MyDB db = new MyDB();
		
		public IActionResult Index(int? page, int fk_Id_Product)
        {
            int _RecordPerPage = 6;
            int _CurrentPage = page ?? 1;
            List<ItemMau> _ListRecord = db.Colors.Where(item => item.fk_Id_Product == fk_Id_Product).OrderByDescending(item => item.Id_Color).ToList();
            ViewBag.id_sanPham = fk_Id_Product;
            return View("Index", _ListRecord.ToPagedList(_CurrentPage, _RecordPerPage));
        }
       
        public IActionResult Update(int? id)
        {
                int _id = id ?? 0;
                ItemMau model = db.Colors.FirstOrDefault(item => item.Id_Color == _id);
                return View(model);
        }


        [HttpPost]
        public IActionResult UpdatePost(IFormCollection fc)
        {
            string _TenMau = fc["name"].ToString().Trim();
            string _MaMau = fc["color_code"].ToString().Trim();
            int _id = Convert.ToInt32(fc["Id_Color"].ToString().Trim());

            var record = db.Colors.FirstOrDefault(item => item.Id_Color == _id);
            int id_Product = record.fk_Id_Product;
            if (record != null)
            {
                record.Name = _TenMau;
                record.Color_Code = _MaMau;

                db.SaveChanges();
                return RedirectToAction("Index", "Color", new { area = "Admin", fk_Id_Product = id_Product });
            }

            return RedirectToAction("Index", "Color", new { area = "Admin", fk_Id_Product = id_Product });
        }

        public IActionResult Create(int fk_Id_Product)
        {

            var product = db.Products.FirstOrDefault(sp => sp.Id_Product == fk_Id_Product);
            return View(product);
        }
        [HttpPost]
        public IActionResult CreatePost(IFormCollection fc, IFormFile file)
        {
            if (string.IsNullOrEmpty(fc["name"]) || string.IsNullOrEmpty(fc["color_Code"]))
            {
                // Ví dụ: return BadRequest("Missing required fields");
            }

            string _TenMau = fc["name"].ToString().Trim();
            string _MaMau = fc["color_Code"].ToString().Trim();
            int _MaSP = Convert.ToInt32(fc["Id_Product"].ToString().Trim());



            ItemMau record = new ItemMau();
            record.Name = _TenMau;
            record.Color_Code = _MaMau;
            record.fk_Id_Product = _MaSP;

            if (file != null && file.Length > 0)
            {
                string _file_name = "";
                try
                {
                    _file_name = file.FileName;

                    var timestamp = DateTime.Now.ToFileTime();
                    _file_name = timestamp + "_" + _file_name;

                    string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Colors", _file_name);
                    using (var stream = new FileStream(_Path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    record.Photo = _file_name;
                }
                catch (Exception ex)
                {

                }
            }

            db.Colors.Add(record);
            db.SaveChanges();


            return Redirect("/Admin/Product");
        }

        public IActionResult Delete(int? id)
        {
            
            int _id = id ?? 0;
            var record = db.Colors.Where(item => item.Id_Color == _id).FirstOrDefault();

            db.Colors.Remove(record);
            db.SaveChanges();

            var id_product = record.fk_Id_Product;
            return RedirectToAction("Index", "Color", new { fk_Id_Product = id_product });
        }
    }
}
