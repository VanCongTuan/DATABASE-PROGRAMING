using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebDatVeMayBay.Areas.Admin.Controllers
{
    public class BaiVietController : Controller
    {
        QuanLyMayBayDataContext da = new QuanLyMayBayDataContext();
        // GET: Admin/BaiViet
        public ActionResult Index()
        {
            List<BaiViet> ds = da.BaiViets.OrderByDescending(s => s.id).ToList();
            return View(ds);
            
        }

       
        // GET: Admin/BaiViet/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/BaiViet/Create
        [HttpPost]
        public ActionResult Create(BaiViet m,FormCollection collection)
        {
            try
            {
                BaiViet p = new BaiViet();
                p = m;
                p.CreateDate = DateTime.Now;
                da.BaiViets.InsertOnSubmit(p);
                da.SubmitChanges();
                return RedirectToAction("Index");            
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/BaiViet/Edit/5
        public ActionResult Edit(int id)
        {
            BaiViet p = da.BaiViets.FirstOrDefault(s => s.id == id);


            return View(p);
        }

        // POST: Admin/BaiViet/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, BaiViet o, FormCollection collection)
        {
            try
            {
                BaiViet p = da.BaiViets.FirstOrDefault(s => s.id == id);


                p.id = o.id;
                p.Title = o.Title;
                p.Description = o.Description;
                p.CreateBy = o.CreateBy;
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

        // GET: Admin/BaiViet/Delete/5
        public ActionResult Delete(int id)
        {
            BaiViet p = da.BaiViets.FirstOrDefault(s => s.id == id);
            return View(p);
           
        }

        // POST: Admin/BaiViet/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here


                BaiViet p = da.BaiViets.FirstOrDefault(s => s.id == id);
                da.BaiViets.DeleteOnSubmit(p);
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
