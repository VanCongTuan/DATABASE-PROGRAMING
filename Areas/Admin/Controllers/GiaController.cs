using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebDatVeMayBay.Areas.Admin.Controllers
{
    public class GiaController : Controller
    {
        QuanLyMayBayDataContext da = new QuanLyMayBayDataContext();
        // GET: Admin/Gia
        public ActionResult Index()
        {

            List<GiaVe> ds = da.GiaVes.OrderByDescending(s => s.id).ToList();
            return View(ds);

            
        }

       
        // GET: Admin/Gia/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Gia/Create
        [HttpPost]
        public ActionResult Create(GiaVe m,FormCollection collection)
        {
            try
            {
                GiaVe p = new GiaVe();
                p = m;             
                da.GiaVes.InsertOnSubmit(p);
                da.SubmitChanges();
                return RedirectToAction("Index");

            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Gia/Edit/5
        public ActionResult Edit(string id)
        {
           GiaVe p = da.GiaVes.FirstOrDefault(s => s.id == id);
            return View(p);
        }

        // POST: Admin/Gia/Edit/5
        [HttpPost]
        public ActionResult Edit(string id,GiaVe o, FormCollection collection)
        {
            try
            {
                GiaVe p = da.GiaVes.FirstOrDefault(s => s.id == id);
                p.Gia = o.Gia;            
                da.SubmitChanges();
                return RedirectToAction("Index");              
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Gia/Delete/5
        public ActionResult Delete(string id)
        {
            GiaVe p = da.GiaVes.FirstOrDefault(s => s.id == id);
            return View(p);
        }

        // POST: Admin/Gia/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                GiaVe p = da.GiaVes.FirstOrDefault(s => s.id == id);
                da.GiaVes.DeleteOnSubmit(p);
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
