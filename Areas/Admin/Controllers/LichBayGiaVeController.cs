using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebDatVeMayBay.Areas.Admin.Controllers
{
    public class LichBayGiaVeController : Controller
    {
       
        QuanLyMayBayDataContext da = new QuanLyMayBayDataContext();
        // GET: Admin/LichBay_GiaVe
        public ActionResult Index()
        {

            List<LichBay_GiaVe> ds = da.LichBay_GiaVes.OrderByDescending(s => s.id).ToList();
            return View(ds);


        }


        // GET: Admin/LichBay_GiaVe/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/LichBay_GiaVe/Create
        [HttpPost]
        public ActionResult Create(LichBay_GiaVe m, FormCollection collection)
        {
            try
            {
                var idLonNhat = da.LichBay_GiaVes.OrderByDescending(s => s.id).Select(s => s.id).FirstOrDefault();
                string soHienTai = idLonNhat.Substring(4);
                int so = int.Parse(soHienTai);
                so++;
                string idMoi = "LBGV" + so.ToString("000");
                m.id = idMoi;
                LichBay_GiaVe p = new LichBay_GiaVe();
                p = m;
                da.LichBay_GiaVes.InsertOnSubmit(p);
                da.SubmitChanges();
                return RedirectToAction("Index");

            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/LichBay_GiaVe/Edit/5
        public ActionResult Edit(string id)
        {
            LichBay_GiaVe p = da.LichBay_GiaVes.FirstOrDefault(s => s.id == id);
            return View(p);
        }

        // POST: Admin/LichBay_GiaVe/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, LichBay_GiaVe o, FormCollection collection)
        {
            try
            {
                LichBay_GiaVe p = da.LichBay_GiaVes.FirstOrDefault(s => s.id == id);
                p.ngayApDung = o.ngayApDung;
                p.ngayKetThuc= o.ngayKetThuc;
                p.SoLuongGhe = o.SoLuongGhe;
                p.id_HangVe= o.id_HangVe;
                p.id_LichBay= o.id_LichBay;
                p.id_GiaVe = o.id_GiaVe;
                da.SubmitChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/LichBay_GiaVe/Delete/5
        public ActionResult Delete(string id)
        {
            LichBay_GiaVe p = da.LichBay_GiaVes.FirstOrDefault(s => s.id == id);
            return View(p);
        }

        // POST: Admin/LichBay_GiaVe/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                LichBay_GiaVe p = da.LichBay_GiaVes.FirstOrDefault(s => s.id == id);
                da.LichBay_GiaVes.DeleteOnSubmit(p);
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
