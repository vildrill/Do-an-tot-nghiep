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
using Web1.Areas.Admin.Attributes;
using Humanizer;

namespace Web1.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[CheckLogin]
    public class DiscountController : Controller
    {
        public MyDB db = new MyDB();
        public IActionResult Index(int? page)
        {
            int current_page = page ?? 1;
            int record_per_page = 10;

            List<ItemDiscount> list_record = db.Discount.OrderByDescending(item => item.Id_Discount).ToList();
            return View("Index", list_record.ToPagedList(current_page, record_per_page));
        }
        public IActionResult Update(int? id)
        {
            int _id = id ?? 0;
            ItemDiscount record = db.Discount.Where(item => item.Id_Discount == _id).FirstOrDefault();
            ViewBag.action = "/Admin/Discount/UpdatePost/" + _id;
            return View("CreateUpdate", record);
        }
        [HttpPost]
        public IActionResult UpdatePost(IFormCollection fc, int? id)
        {
            int _id = id ?? 0;
            string _name = fc["name"].ToString().Trim();
            double _percent = Convert.ToInt32(fc["percent"].ToString().Trim());
            DateTime _end_Date = Convert.ToDateTime(fc["End_Date"]);
            int _status = Convert.ToInt32(fc["Status"].ToString().Trim());
            string _description = fc["description"].ToString().Trim();

            ItemDiscount record = db.Discount.Where(item => item.Id_Discount == _id).FirstOrDefault();
            if (record != null)
            {
                record.Name = _name;
                record.Percent = _percent;
                record.End_Date = _end_Date;
                record.Status = _status;
                record.Description = _description;
               

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

                    string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Discounts", _file_name);
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
            ViewBag.action = "/Admin/Discount/CreatePost";
            return View("CreateUpdate");
        }

        [HttpPost]
        public IActionResult CreatePost(IFormCollection fc)
        {

            string _name = fc["name"].ToString().Trim();
            double _percent = Convert.ToInt32(fc["percent"].ToString().Trim());
            DateTime _end_Date = Convert.ToDateTime(fc["End_Date"]);
            int _status = Convert.ToInt32(fc["Status"].ToString().Trim());
            string _description = fc["description"].ToString().Trim();

            ItemDiscount record = new ItemDiscount();

            record.Name = _name;
            record.Percent = _percent;
            record.End_Date = _end_Date;
            record.Status = _status;
            record.Description = _description;    

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
                string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Discounts", _file_name);
                using (var stream = new FileStream(_Path, FileMode.Create))
                {
                    Request.Form.Files[0].CopyTo(stream);
                }
                record.Photo = _file_name;
                db.SaveChanges();
            }
            db.Discount.Add(record);
            db.SaveChanges();


            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? id)
        {
            int _id = id ?? 0;

            ItemDiscount record = db.Discount.Where(item => item.Id_Discount == _id).FirstOrDefault();

            db.Discount.Remove(record);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
