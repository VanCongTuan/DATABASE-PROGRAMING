using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebDatVeMayBay.Areas.Admin.Controllers
{
    public class HangVeController : Controller
    {
        QuanLyMayBayDataContext da = new QuanLyMayBayDataContext();
        // GET: Admin/HangVe
        public ActionResult Index()
        {

            List<HangVe> ds = da.HangVes.OrderByDescending(s => s.id).ToList();
            return View(ds);


        }


        // GET: Admin/HangVe/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/HangVe/Create
        [HttpPost]
        public ActionResult Create(HangVe m, FormCollection collection)
        {
            try
            {
                HangVe p = new HangVe();
                p = m;
                da.HangVes.InsertOnSubmit(p);
                da.SubmitChanges();
                return RedirectToAction("Index");

            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/HangVe/Edit/5
        public ActionResult Edit(string id)
        {
            HangVe p = da.HangVes.FirstOrDefault(s => s.id == id);
            return View(p);
        }

        // POST: Admin/HangVe/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, HangVe o, FormCollection collection)
        {
            try
            {
                HangVe p = da.HangVes.FirstOrDefault(s => s.id == id);
                p.loaiHang = o.loaiHang;
                da.SubmitChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/HangVe/Delete/5
        public ActionResult Delete(string id)
        {
            HangVe p = da.HangVes.FirstOrDefault(s => s.id == id);
            return View(p);
        }

        // POST: Admin/HangVe/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                HangVe p = da.HangVes.FirstOrDefault(s => s.id == id);
                da.HangVes.DeleteOnSubmit(p);
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