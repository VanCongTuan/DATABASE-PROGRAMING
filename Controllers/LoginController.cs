using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using System.Threading.Tasks;

namespace WebDatVeMayBay.Controllers
{
    public class Login1Controller : Controller
    {
        // GET: Login
        string idnvlogin = "";
        public ActionResult Index()
        {
            return View();
        }

        QuanLyMayBayDataContext da = new QuanLyMayBayDataContext();



        public NhanVien GetHotenByAccount(string username, string password)
        {
            var query = from nv in da.NhanViens
                        join tk in da.TaiKhoans on nv.id_TK equals tk.id
                        where tk.username == username && tk.password == password
                        select new { nv.hovaTen, nv.id };

            var result = query.FirstOrDefault();

            if (result != null)
            {
                // Tạo một đối tượng NhanVien từ kết quả truy vấn
                return new NhanVien { hovaTen = result.hovaTen, id = result.id };
            }

            return null;
        }

        [HttpGet]
        public ActionResult NhanVienLogin()
        {
            

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }
        
        [HttpPost]
        public ActionResult NhanVienLogin(string username, string password)
        {
            

            var user = da.TaiKhoans.SingleOrDefault(u => u.username == username && u.password == password); 
          
            
            if (user != null)
            {
                var hoten = GetHotenByAccount(username,password);
                var userRole = user.userrole.ToString();
                FormsAuthentication.SetAuthCookie(username, false);
                Session["hotennv"] = new
                {
                    hovaTen = hoten.hovaTen,
                    role = userRole
                };
                

                Session["id"] = hoten.id;
                idnvlogin = hoten.id;
                if (user.userrole == "NVLL")
                {
                    return RedirectToAction("LapLich");
                   
                }
                if (user.userrole == "NVBV")
                {
                    return RedirectToAction("BanVe");
                   
                }
                if (user.userrole == "AD")
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });

                }
            }
            else
            {
                ViewBag.ErrorMessage = "Tên đăng nhập hoặc mật khẩu không chính xác, vui lòng thử lại.";
                return RedirectToAction("Index");

            }
            
               
                return View();
            
        }

        public ActionResult LapLich()
        {
            var hotenSession = Session["hotennv"] as dynamic;
            if (hotenSession != null)
            {
                string hoTen = hotenSession.hovaTen;
                string role = hotenSession.role;

                // Lưu tên vào ViewBag để sử dụng trong View
                ViewBag.ten = hoTen;
            }
            return View();
        }
        public ActionResult BanVe()
        {
            var hotenSession = Session["hotennv"] as dynamic;
            if (hotenSession != null)
            {
                string hoTen = hotenSession.hovaTen;
                string role = hotenSession.role;

                // Lưu tên vào ViewBag để sử dụng trong View
                ViewBag.ten = hoTen;
            }
            
            var HangList = da.HangVes.ToList();
            var sanBayList = da.SanBays.ToList();

            // Gán kết quả của truy vấn vào ViewBag
            ViewBag.HangVe= HangList;
            ViewBag.SanBay = sanBayList;
            return View();
        }
        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // Xóa phiên
            Response.Cookies[FormsAuthentication.FormsCookieName].Expires = DateTime.Now.AddDays(-1);                 
            Session.Clear();           
            return RedirectToAction("Index");
        }
    }
}