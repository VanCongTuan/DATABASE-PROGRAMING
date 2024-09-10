using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;
namespace WebDatVeMayBay.Areas.Admin.Controllers
{
    public class TaiKhoanController : Controller
    {
        QuanLyMayBayDataContext da = new QuanLyMayBayDataContext();
        // GET: Admin/TaiKhoan
        public ActionResult Index()
        {

            List<TaiKhoan> ds = da.TaiKhoans.OrderByDescending(s => s.id).ToList();
            return View(ds);


        }


        // GET: Admin/TaiKhoan/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/TaiKhoan/Create
        [HttpPost]
        public ActionResult Create(TaiKhoan m, FormCollection collection)
        {
            try
            {
                var idLonNhat = da.TaiKhoans.OrderByDescending(s => s.id).Select(s => s.id).FirstOrDefault();
                string soHienTai = idLonNhat.Substring(2);
                int so = int.Parse(soHienTai);
                so++;
                string idMoi = "TK" + so.ToString("0");
                m.id = idMoi;
                m.password = ComputeSHA256Hash(m.password);
                TaiKhoan p = new TaiKhoan();
                p = m;
                da.TaiKhoans.InsertOnSubmit(p);
                da.SubmitChanges();
                return RedirectToAction("Index");

            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/TaiKhoan/Edit/5
        public ActionResult Edit(string id)
        {
            TaiKhoan p = da.TaiKhoans.FirstOrDefault(s => s.id == id);
            return View(p);
        }

        // POST: Admin/TaiKhoan/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, TaiKhoan o, FormCollection collection)
        {
            try
            {
                TaiKhoan p = da.TaiKhoans.FirstOrDefault(s => s.id == id);
                p.username = o.username;
                p.password = o.password;
                p.userrole = o.userrole;
                da.SubmitChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/TaiKhoan/Delete/5
        public ActionResult Delete(string id)
        {
            TaiKhoan p = da.TaiKhoans.FirstOrDefault(s => s.id == id);
            return View(p);
        }

        // POST: Admin/TaiKhoan/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                TaiKhoan p = da.TaiKhoans.FirstOrDefault(s => s.id == id);
                da.TaiKhoans.DeleteOnSubmit(p);
                da.SubmitChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public static string ComputeSHA256Hash(string rawData)
        {
            // Tạo một đối tượng SHA256
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Tạo một mảng bytes từ chuỗi đầu vào
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Chuyển đổi mảng bytes thành chuỗi hex
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
