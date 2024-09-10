using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebDatVeMayBay.Areas.Admin.Controllers
{
    public class ChuyenBayController : Controller
    {
        QuanLyMayBayDataContext da = new QuanLyMayBayDataContext();
        // GET: Admin/ChuyenBay
        public ActionResult Index()
        {

            List<ChuyenBay> ds = da.ChuyenBays.OrderByDescending(s => s.MaCB ).ToList();
            return View(ds);


        }


        // GET: Admin/ChuyenBay/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/ChuyenBay/Create
        [HttpPost]
        public ActionResult Create(ChuyenBay m, FormCollection collection)
        {
            try
            {
                var maCBLonNhat = da.ChuyenBays.OrderByDescending(s => s.MaCB).Select(s => s.MaCB).FirstOrDefault();
                string soHienTai = maCBLonNhat.Substring(2);
                int so = int.Parse(soHienTai);
                so++;
                string maLBMoi = "LB" + so.ToString("000");
                m.MaCB = maLBMoi;
                ChuyenBay p = new ChuyenBay();
                p = m;
                da.ChuyenBays.InsertOnSubmit(p);
                da.SubmitChanges();
                return RedirectToAction("Index");

            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/ChuyenBay/Edit/5
        public ActionResult Edit(string id)
        {
            ChuyenBay p = da.ChuyenBays.FirstOrDefault(s => s.MaCB == id);
            return View(p);
        }

        // POST: Admin/ChuyenBay/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, ChuyenBay o, FormCollection collection)
        {
            try
            {
                ChuyenBay p = da.ChuyenBays.FirstOrDefault(s => s.MaCB  == id);
                p.tenTuyenBay = o.tenTuyenBay;
                p.id_TuyenBay = o.id_TuyenBay;
                da.SubmitChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/ChuyenBay/Delete/5
        public ActionResult Delete(string id)
        {
            ChuyenBay p = da.ChuyenBays.FirstOrDefault(s => s.MaCB  == id);
            return View(p);
        }

        // POST: Admin/ChuyenBay/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                ChuyenBay p = da.ChuyenBays.FirstOrDefault(s => s.MaCB  == id);
                da.ChuyenBays.DeleteOnSubmit(p);
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