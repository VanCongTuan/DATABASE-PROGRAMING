using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebDatVeMayBay.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {       
                Session["flight"] = new List<object>(); // Khởi tạo một danh sách rỗng
                return View("index");                    
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}