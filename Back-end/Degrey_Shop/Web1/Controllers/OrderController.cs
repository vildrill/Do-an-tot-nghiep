using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using System.Net;
using System.Net.Mail;
using Web1.Models; //nhin thay cac file .cs trong folder Models
using Web1.Others;
using X.PagedList;

namespace Web1.Controllers
{


    public class OrderController : Controller
    {

        public MyDB db = new MyDB();
        private readonly EmailService _emailService;

        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OrderController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, EmailService emailService)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
        }

        [HttpPost]
        public IActionResult CreateOrder(float Total, string Tinh, string Huyen, string Xa, string Street, string note, string paymentMethod)
        {
            string customerIDString = HttpContext.Session.GetString("customer_id");
            int TinhInt = int.Parse(Tinh);
            var tenTinh = db.City.FirstOrDefault(m => m.Id == TinhInt);
            int HuyenInt = int.Parse(Huyen);
            var tenHuyen = db.District.FirstOrDefault(m => m.Id == HuyenInt);
            int XaInt = int.Parse(Xa);
            var tenXa = db.Ward.FirstOrDefault(m => m.Id == XaInt);

            if (string.IsNullOrEmpty(customerIDString))
            {
                return Json(new { success = false, message = "Invalid customer ID" });
            }
            int customerID = Convert.ToInt32(customerIDString);
            try
            {
                var NewItemOrder = new ItemOrder
                {
                    fk_Id_User = customerID,
                    Total_Price = Total,
                    Create_Date = DateTime.Now,
                    fk_Id_Discount = 3,
                    Discount = 1,
                    Status = 1,
                    Payment = (paymentMethod == "CheckPayment") ? 2 : 1,
                    Address = $" {Street}, {tenTinh.Name},{tenHuyen.Name},{tenXa.Name}",
                    Note = note
                };

                db.Orders.Add(NewItemOrder);
                db.SaveChanges();

                if (paymentMethod == "VNPay")
                {
                    string url = _configuration["VNPAY:Url"];
                    string returnUrl = _configuration["VNPAY:ReturnUrl"];
                    string tmnCode = _configuration["VNPAY:TmnCode"];
                    string hashSecret = _configuration["VNPAY:HashSecret"];
                    var tongTiens = (Total * 100).ToString();
                    var id_hoaDon = NewItemOrder.Id_Order.ToString();

                    PayLib pay = new PayLib(_httpContextAccessor);
                    pay.AddRequestData("vnp_Version", "2.1.0");
                    pay.AddRequestData("vnp_Command", "pay");
                    pay.AddRequestData("vnp_TmnCode", tmnCode);
                    pay.AddRequestData("vnp_Amount", tongTiens);
                    pay.AddRequestData("vnp_BankCode", "");
                    pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    pay.AddRequestData("vnp_CurrCode", "VND");
                    pay.AddRequestData("vnp_IpAddr", _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString());
                    pay.AddRequestData("vnp_Locale", "vn");
                    pay.AddRequestData("vnp_OrderInfo", "Complete payment!");
                    pay.AddRequestData("vnp_OrderType", "other");
                    pay.AddRequestData("vnp_ReturnUrl", returnUrl);
                    pay.AddRequestData("vnp_TxnRef", id_hoaDon);

                    string paymentUrl = pay.CreateRequestUrl(url, hashSecret);
                    return Json(new { success = true, id_hoaDon = NewItemOrder.Id_Order, paymentUrl });
                }
                else
                {
                    string paymentUrl = "paymentMethod";
                    return Json(new { success = true, id_hoaDon = NewItemOrder.Id_Order, paymentUrl });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error creating order: " + ex.Message });
            }
        }

        [HttpPost]
        public ActionResult OrderDetail(int hoaDonId, List<int> chiTietSPIds, List<int> soLuongs, List<string> color, List<string> size )
        {
            try
            {
                if (chiTietSPIds == null || soLuongs == null || chiTietSPIds.Count != soLuongs.Count)
                {
                    return Json(new { success = false, message = "Data invalid." });
                }

                for (int i = 0; i < chiTietSPIds.Count; i++)
                {
                    var chiTietSPId = chiTietSPIds[i];
                    var soLuong = soLuongs[i];
                    var colors = color[i];
                    var sizes = size[i];

                    var product = db.Products.FirstOrDefault(p => p.Id_Product == chiTietSPId);
                    if (product == null)
                    {
                        return Json(new { success = false, message = $"Can not found the product with the ID: {chiTietSPId}." });
                    }

                    var itemOrderDetail = new ItemOrderDetail
                    {
                        fk_Id_Order = hoaDonId,
                        fk_Id_Product = chiTietSPId,
                        Quantity = soLuong,
                        Price = product.Price * soLuong,
                        color = colors,
                        size = sizes
                    };

                    db.OrdersDetail.Add(itemOrderDetail);
                }

                db.SaveChanges();
                return Json(new { success = true, message = "Order created successfully" });
            }
            catch (Exception ex)
            {
                // Ghi nhật ký lỗi
                // Log.Error(ex, "Error processing order details.");
                return Json(new { success = false, message = "An error occurred while processing order details." });
            }
        }



        public IActionResult PaymentConfirm()
        {
            if (Request.Query.Count > 0)
            {
                string hashSecret = _configuration["VNPAY:HashSecret"];
                var vnpayData = Request.Query;
                PayLib pay = new PayLib(_httpContextAccessor);
                foreach (var key in vnpayData.Keys)
                {
                    string value = vnpayData[key];
                    if (!string.IsNullOrEmpty(value) && key.StartsWith("vnp_"))
                    {
                        pay.AddResponseData(key, value);
                    }
                }
                long orderId = Convert.ToInt64(pay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode");
                string vnp_SecureHash = Request.Query["vnp_SecureHash"];
                bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret);
                if (checkSignature == false)
                {
                    if (vnp_ResponseCode == "00")
                    {
                        var order = db.Orders.FirstOrDefault(o => o.Id_Order == orderId);
                        var user = db.Users.FirstOrDefault(u => u.Id_User == order.fk_Id_User);
                        var toAddress = user.Email;
                        var subject = "Notification from Degrey Shop ";
                        var body = "You have successfully paid the order: " + orderId + " | Paid code: " + vnpayTranId + " | View detail:" + toAddress;
                        ViewBag.Message = "You have successfully paid the order: " + orderId + " | Paid code: " + vnpayTranId + " | View detail:" + toAddress;
                        _emailService.SendEmail(toAddress, subject, body);
                    }
                    else
                    {
                        ViewBag.Message = "An error occurred while processing the order " + orderId + " | Paid code: " + vnpayTranId + " | Error code: " + vnp_ResponseCode;
                    }
                }
                else
                {
                    ViewBag.Message = "An error occurred during processing";
                }
            }
            return View();
        }


        public IActionResult YourOrder(int id_User, int? page)
        {
            int current_page = page ?? 1;
            int record_per_page = 6;
            // Lấy danh sách đơn hàng từ cơ sở dữ liệu theo id_User
            List<ItemOrder> list_record = db.Orders
                .Where(order => order.fk_Id_User == id_User)
                .Select(order => new ItemOrder
                {
                    Id_Order = order.Id_Order,
                    fk_Id_User = order.fk_Id_User,
                    Create_Date = order.Create_Date,
                    Address = order.Address,
                    Total_Price = order.Total_Price,
                    Status  = order.Status,
                    Note = order.Note

                    // Các thuộc tính khác
                })
                .ToList();
            ViewBag.id_user = id_User;
            var pagedList = list_record.ToPagedList(current_page, record_per_page);
            // Trả về View với danh sách đơn hàng đã phân trang
            return View("YourOrder", pagedList);
        }

        public IActionResult YourOrderDetail(int id_Order, int? page)
        {
            int current_page = page ?? 1;
            int record_per_page = 10;

            // Lấy danh sách đơn hàng từ cơ sở dữ liệu theo id_User
            List<ItemOrderDetail> list_record = db.OrdersDetail
                .Where(order => order.fk_Id_Order == id_Order)
                .Select(order => new ItemOrderDetail
                {
                    fk_Id_Order = order.fk_Id_Order,
                    fk_Id_Product = order.fk_Id_Product,
                    Quantity = order.Quantity,
                    Price = order.Price,
                    color = order.color,
                    size  = order.size,


                })
                .ToList();
            var Order = db.Orders.FirstOrDefault(item => item.Id_Order == id_Order);
            var id_user = Order.fk_Id_User;

            var pagedList = list_record.ToPagedList(current_page, record_per_page);
            ViewBag.id_user = id_user;
            // Trả về View với danh sách đơn hàng đã phân trang
            return View("YourOrderDetail", pagedList);
        }

        public IActionResult doiTrangThai(int value, int id_Order)
        {


            var hoaDon = db.Orders.FirstOrDefault(d => d.Id_Order == id_Order);

            if (hoaDon != null)
            {
                var id_user = hoaDon.fk_Id_User;
                hoaDon.Status = value;

                db.SaveChanges();


                return RedirectToAction("YourOrder", "Order", new { id_User = id_user });

            }
            else
            {

                ViewBag.ErrorMessage = "Can not found the order with ID: " + id_Order;
                return View("Error");
            }
        }




    }
}
