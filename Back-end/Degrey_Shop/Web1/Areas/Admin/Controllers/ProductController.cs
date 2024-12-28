using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BC = BCrypt.Net.BCrypt;
using X.PagedList;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Data;
using Microsoft.Data.SqlClient;

using Web1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Web1.Areas.Admin.Attributes;

namespace Web1.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[CheckLogin]
    public class ProductController : Controller
    {
        public MyDB db = new MyDB();
        public IActionResult Index(int? page)
        {
            int current_page = page ?? 1;
            int record_per_page = 6;

            List<ItemSanPham> list_record = db.Products.OrderByDescending(item => item.Id_Product).ToList();
            return View("Index", list_record.ToPagedList(current_page, record_per_page));
        }


        public IActionResult Update(int? id)
        {
            int _id = id ?? 0;
            ItemSanPham record = db.Products.Where(item => item.Id_Product == _id).FirstOrDefault();
            ViewBag.action = "/Admin/Product/UpdatePost/" + _id;
            return View("CreateUpdate", record);
        }
        [HttpPost]
        public IActionResult UpdatePost(IFormCollection fc, int? id)
        {
            int _id = id ?? 0;
            string _name = fc["name"].ToString().Trim();
            double _price = Convert.ToDouble(fc["price"].ToString().Trim());
            string _discount = fc["discount"].ToString().Trim();
            string _description = fc["description"].ToString().Trim();
            string _note = fc["note"].ToString().Trim();


            ItemSanPham record = db.Products.Where(item => item.Id_Product == _id).FirstOrDefault();
            if (record != null)
            {
                record.Name = _name;
                record.Price = _price;
                record.Discount = _discount;
                record.Description = _description;
                record.note = _note;

                List<string> list_kind = Request.Form["Category"].ToList();
                foreach (var brand_id in list_kind)
                {
                    record.fk_Id_Category = Convert.ToInt32(brand_id);
                }

                string _file_name = "";
                try
                {
                    _file_name = Request.Form.Files[0].FileName;
                }
                catch {; }
                if (!string.IsNullOrEmpty(_file_name))
                {
                    var timestamp = DateTime.Now.ToFileTime();
                    _file_name = timestamp + "_" + _file_name;
                    string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Products", _file_name);
                    using (var stream = new FileStream(_Path, FileMode.Create))
                    {
                        Request.Form.Files[0].CopyTo(stream);
                    }
                    record.Photo = _file_name;

                    db.SaveChanges();
                }
                
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public IActionResult Create()
        {
            ViewBag.action = "/Admin/Product/CreatePost";
            return View("CreateUpdate");
        }

        [HttpPost]
        public IActionResult CreatePost(IFormCollection fc)
        {

            string _name = fc["name"].ToString().Trim();
            double _price = Convert.ToDouble(fc["price"].ToString().Trim());
            string _discount = fc["discount"].ToString().Trim();
            string _description = fc["description"].ToString().Trim();
            string _note = fc["note"].ToString().Trim();

            ItemSanPham record = new ItemSanPham();

            record.Name = _name;
            record.Price = _price;
            record.Discount = _discount;
            record.Description = _description;
            record.note = _note;

            List<string> list_brands = Request.Form["Category"].ToList();
            foreach (var brand_id in list_brands)
            {
                record.fk_Id_Category = Convert.ToInt32(brand_id);
            }


            string _file_name = "";
            try
            {
                _file_name = Request.Form.Files[0].FileName;
            }
            catch {; }
            if (!string.IsNullOrEmpty(_file_name))
            {

                var timestamp = DateTime.Now.ToFileTime();
                _file_name = timestamp + "_" + _file_name;
                string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Products", _file_name);
                using (var stream = new FileStream(_Path, FileMode.Create))
                {
                    Request.Form.Files[0].CopyTo(stream);
                }
                record.Photo = _file_name;
                db.SaveChanges();
            }
            db.Products.Add(record);
            db.SaveChanges();


            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? id)
        {
            int _id = id ?? 0;

            ItemSanPham record = db.Products.Where(item => item.Id_Product == _id).FirstOrDefault();

            db.Products.Remove(record);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
