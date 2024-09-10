using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebDatVeMayBay.Areas.Admin.Controllers
{
    public class TinTucController : Controller
    {
        QuanLyMayBayDataContext da = new QuanLyMayBayDataContext();
        // GET: Admin/TinTuc
        public ActionResult Index()
        {
            List<TinTuc> ds = da.TinTucs.OrderByDescending(s => s.id).ToList();
            return View(ds);
            
        }

        
        // GET: Admin/TinTuc/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/TinTuc/Create
        [HttpPost]
        public ActionResult Create(TinTuc m, FormCollection collection)
        {
            try
            {
                TinTuc p = new TinTuc();
                p = m;
                p.ModifiedrDate = DateTime.Now;
                p.CreateDate = DateTime.Now;
                da.TinTucs.InsertOnSubmit(p);
                da.SubmitChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/TinTuc/Edit/5
        public ActionResult Edit(int id)
        {
            TinTuc p = da.TinTucs.FirstOrDefault(s => s.id == id);
            return View(p);
        }

        // POST: Admin/TinTuc/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, TinTuc o, FormCollection collection)
        {
            try
            {

                TinTuc p = da.TinTucs.FirstOrDefault(s => s.id == id);

                p.id = o.id;
                p.Title = o.Title;               
                p.Description = o.Description;
                p.CreateBy = o.CreateBy;
                p.Image = o.Image;
                p.IsActive = o.IsActive;
                p.ModifiedrDate = DateTime.Now;              
                p.ModifiedBy = o.ModifiedBy;
                

                da.SubmitChanges();
                return RedirectToAction("Index");


            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/TinTuc/Delete/5
        public ActionResult Delete(int id)
        {
            TinTuc p = da.TinTucs.FirstOrDefault(s => s.id == id);
            return View(p);
           
        }

        // POST: Admin/TinTuc/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                TinTuc p = da.TinTucs.FirstOrDefault(s => s.id == id);
                da.TinTucs.DeleteOnSubmit(p);
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
