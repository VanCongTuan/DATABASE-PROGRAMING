
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebDatVeMayBay.Areas.Admin.Controllers
{
    public class HanhKhachController : Controller
    {
        QuanLyMayBayDataContext da = new QuanLyMayBayDataContext();
        // GET: Admin/HanhKhach
        public ActionResult Index()
        {
            List<HanhKhach> ds = da.HanhKhaches.OrderByDescending(s => s.id).ToList();
            return View(ds);
        }


        // GET: Admin/HanhKhach/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/HanhKhach/Create
        [HttpPost]
        public ActionResult Create(HanhKhach m,FormCollection collection)
        {
            try
            {

                HanhKhach p = new HanhKhach();
                p = m;
                da.HanhKhaches.InsertOnSubmit(p);
                da.SubmitChanges();
                return RedirectToAction("Index");
             
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/HanhKhach/Edit/5
        public ActionResult Edit(string id)
        {

            HanhKhach p = da.HanhKhaches.FirstOrDefault(s => s.id == id);
            return View(p);
        }

        // POST: Admin/HanhKhach/Edit/5
        [HttpPost]
        public ActionResult Edit(string id,HanhKhach o, FormCollection collection)
        {
            try
            {
                HanhKhach p = da.HanhKhaches.FirstOrDefault(s => s.id == id);
                p.hoTen = o.hoTen;
                p.gioiTinh = o.gioiTinh;
                p.ngSinh = o.ngSinh;
                p.CCCD = o.CCCD;
                p.email = o.email;
                p.sdt = o.sdt;
                da.SubmitChanges();
                return RedirectToAction("Index");

               
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/HanhKhach/Delete/5
        public ActionResult Delete(string id)
        {
            HanhKhach p = da.HanhKhaches.FirstOrDefault(s => s.id == id);
            return View(p);
        }

        // POST: Admin/HanhKhach/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                HanhKhach p = da.HanhKhaches.FirstOrDefault(s => s.id == id);
                da.HanhKhaches.DeleteOnSubmit(p);
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
