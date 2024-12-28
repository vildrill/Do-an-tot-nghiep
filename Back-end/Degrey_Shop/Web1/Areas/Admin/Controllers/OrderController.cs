using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Web1.Models; 
using System.Data;
using System.Data.SqlClient; 
using X.PagedList; 
using Microsoft.AspNetCore.Http;
using Web1.Areas.Admin.Attributes; 
using System.IO;
using BC = BCrypt.Net.BCrypt;
using Microsoft.EntityFrameworkCore;

namespace Web1.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[CheckLogin]
    public class OrderController : Controller
    {
        public MyDB db = new MyDB();
        public IActionResult Index(int? page)
        {
            int current_page = page ?? 1;
            int record_per_page = 5;

            List<ItemOrder> list_record = db.Orders
                .Select(order => new ItemOrder
                {
                    Id_Order = order.Id_Order,
                    fk_Id_User = order.fk_Id_User,
                    Create_Date = order.Create_Date,
                    Address = order.Address,
                    Total_Price = order.Total_Price,
                    Status = order.Status,
                    Note = order.Note
                })
                .ToList();

            var pagedList = list_record.ToPagedList(current_page, record_per_page);

            return View("Index", pagedList);
        }

        public IActionResult YourOrderDetail(int id_Order, int? page)
        {
            int current_page = page ?? 1;
            int record_per_page = 1;

            List<ItemOrderDetail> list_record = db.OrdersDetail
                .Where(order => order.fk_Id_Order == id_Order)
                .Select(order => new ItemOrderDetail
                {
                    fk_Id_Order = order.fk_Id_Order,
                    fk_Id_Product = order.fk_Id_Product,
                    Quantity = order.Quantity,
                    Price = order.Price,
                    color = order.color,
                    size = order.size,


                })
                .ToList();
            var Order = db.Orders.FirstOrDefault(item => item.Id_Order == id_Order);
            var id_user = Order.fk_Id_User;

            var pagedList = list_record.ToPagedList(current_page, record_per_page);
            ViewBag.id_user = id_user;
            ViewBag.id_order = id_Order;
            return View("YourOrderDetail", pagedList);
        }

        public IActionResult Detail(int? id)
        {
            int _OrderId = id ?? 0;
            ViewBag.ID_ChiTietDD = _OrderId;
            List<ItemOrderDetail> _ListRecord = db.OrdersDetail.Where(tbl => tbl.Id_OrderDetail == _OrderId).ToList();
            return View("Detail", _ListRecord);
        }

        public IActionResult Delivery(int? id)
        {
            int _OrderId = id ?? 0;
            ItemOrder record = db.Orders.Where(tbl => tbl.Id_Order == _OrderId).FirstOrDefault();
            if (record != null)
            {
                record.Status = 1;
                db.SaveChanges();
            }
            return Redirect("/Admin/Order");
        }

        public IActionResult doiTrangThai(int value, int id_Order)
        {
                var hoaDon = db.Orders.FirstOrDefault(d => d.Id_Order == id_Order);

                if (hoaDon != null)
                {

                    hoaDon.Status = value;

                    db.SaveChanges();


                    return RedirectToAction("Index", "Order", new { area = "Admin" });

                }
                else
                {

                    ViewBag.ErrorMessage = "Không tìm thấy hóa đơn có ID " + id_Order;
                    return View("Error");
                }
        }

        [HttpPost]
        public JsonResult CancelOrder(int id_Order, string note)
        {
            try
            {
                var order = db.Orders.FirstOrDefault(o => o.Id_Order == id_Order);
                if (order != null)
                {
                    order.Status = 4; // Cancel by shop
                    order.Note = note;
                    db.SaveChanges();

                    return Json(new { success = true });
                }
                return Json(new { success = false });
            }
            catch (Exception ex)
            {
                // Log exception
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
