using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebDatVeMayBay.Areas.Admin.Controllers
{
    public class LichBayController : Controller
    {
        QuanLyMayBayDataContext da = new QuanLyMayBayDataContext();
        // GET: Admin/LichBay
        public ActionResult Index()
        {

            List<LichBay> ds = da.LichBays.OrderByDescending(s => s.MaLB).ToList();
            return View(ds);


        }


        // GET: Admin/LichBay/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/LichBay/Create
        [HttpPost]
        public ActionResult Create(LichBay m, FormCollection collection)
        {
            try
            {
                var maLBLonNhat = da.LichBays.OrderByDescending(s => s.MaLB).Select(s => s.MaLB).FirstOrDefault();                           
                string soHienTai = maLBLonNhat.Substring(2);
                int so = int.Parse(soHienTai);
                so++;
                string maLBMoi = "LB" + so.ToString("000");
                m.MaLB = maLBMoi;
                LichBay p = new LichBay();
                p = m;
                da.LichBays.InsertOnSubmit(p);
                da.SubmitChanges();
                return RedirectToAction("Index");

            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/LichBay/Edit/5
        public ActionResult Edit(string id)
        {
            LichBay p = da.LichBays.FirstOrDefault(s => s.MaLB == id);
            return View(p);
        }

        // POST: Admin/LichBay/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, LichBay o, FormCollection collection)
        {
            try
            {
                LichBay p = da.LichBays.FirstOrDefault(s => s.MaLB == id);
                p.NgayBay = o.NgayBay;
                p.thoiGianBay = o.thoiGianBay;
                p.trangThai = o.trangThai;
                p.id_ChuyenBay = o.id_ChuyenBay;
                p.id_MayBay = o.id_MayBay;
                p.id_NVLL = o.id_NVLL;
                da.SubmitChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/LichBay/Delete/5
        public ActionResult Delete(string id)
        {
            LichBay p = da.LichBays.FirstOrDefault(s => s.MaLB == id);
            return View(p);
        }

        // POST: Admin/LichBay/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                LichBay p = da.LichBays.FirstOrDefault(s => s.MaLB == id);
                da.LichBays.DeleteOnSubmit(p);
                da.SubmitChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

    }
}