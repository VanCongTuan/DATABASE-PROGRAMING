using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebDatVeMayBay.Areas.Admin.Controllers
{
    public class MayBayController : Controller
    {
        public void ten()
        {
            ViewBag.Ma = "Mã Máy Bay";
            ViewBag.TenMoi = "Tên Máy Bay";
            ViewBag.HangMoi = "Hãng Máy Bay";
            ViewBag.SoHieu = "Số hiêu Máy Bay";
        }
        QuanLyMayBayDataContext da = new QuanLyMayBayDataContext();
        // GET: Admin/MayBay
        public ActionResult Index()
        {
            List<MayBay> ds = da.MayBays.OrderByDescending(s => s.maMb).ToList();
            return View(ds);
            
        }
        public ActionResult Add()
        {
            ten();
            
            return View();
        }
        [HttpPost]
        public ActionResult Add(MayBay m, FormCollection collection)
        {
            ten();
            try
            {
                MayBay p = new MayBay();
                p = m;
                da.MayBays.InsertOnSubmit(p);               
                da.SubmitChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Edit(string id)
        {
            ten();
            MayBay p = da.MayBays.FirstOrDefault(s => s.maMb == id);


            return View(p);
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(string id,MayBay o, FormCollection collection)
        {
            ten();
            try
            {

                MayBay p = da.MayBays.FirstOrDefault(s => s.maMb == id);

                p.maMb = o.maMb;
                p.tenMB = o.tenMB;
                p.hangMB = o.hangMB;
                p.soHieu = o.soHieu;

                da.SubmitChanges();
                return RedirectToAction("Index");


            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Delete/5
        public ActionResult Delete(string id)
        {
            ten();
            MayBay p = da.MayBays.FirstOrDefault(s => s.maMb == id);
            return View(p);
        }

        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            ten();
            try
            {
                MayBay p = da.MayBays.FirstOrDefault(s => s.maMb == id);
                da.MayBays.DeleteOnSubmit(p);
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