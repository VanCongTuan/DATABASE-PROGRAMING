using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebDatVeMayBay.Areas.Admin.Controllers
{
    public class TuyenBayController : Controller
    {
        // GET: Admin/TuyenBay
        
        QuanLyMayBayDataContext da = new QuanLyMayBayDataContext();
       
        public ActionResult Index()
        {

            List<TuyenBay> ds = da.TuyenBays.OrderByDescending(s => s.maTuyenBay).ToList();
            return View(ds);


        }


        // GET: Admin/TuyenBay/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/TuyenBay/Create
        [HttpPost]
        public ActionResult Create(TuyenBay m, FormCollection collection)
        {
            try
            {
                TuyenBay p = new TuyenBay();
                p = m;
                da.TuyenBays.InsertOnSubmit(p);
                da.SubmitChanges();
                return RedirectToAction("Index");

            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/TuyenBay/Edit/5
        public ActionResult Edit(string id)
        {
            TuyenBay p = da.TuyenBays.FirstOrDefault(s => s.maTuyenBay == id);
            return View(p);
        }

        // POST: Admin/TuyenBay/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, TuyenBay o, FormCollection collection)
        {
            try
            {
                TuyenBay p = da.TuyenBays.FirstOrDefault(s => s.maTuyenBay == id);
                p.tenTuyenBay = o.tenTuyenBay;
                p.id_SbDi = o.id_SbDi;
                p.id_SbDen = o.id_SbDen;
                da.SubmitChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/TuyenBay/Delete/5
        public ActionResult Delete(string id)
        {
            TuyenBay p = da.TuyenBays.FirstOrDefault(s => s.maTuyenBay == id);
            return View(p);
        }

        // POST: Admin/TuyenBay/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                TuyenBay p = da.TuyenBays.FirstOrDefault(s => s.maTuyenBay == id);
                da.TuyenBays.DeleteOnSubmit(p);
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