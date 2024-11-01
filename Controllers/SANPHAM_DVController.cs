using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DoAnPhanMem.Models;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace DoAnPhanMem.Controllers
{
    public class SANPHAM_DVController : Controller
    {
        private KHOCTHATNHIEUDB1 db = new KHOCTHATNHIEUDB1();
        
        // GET: SANPHAM_DV
        public ActionResult Index()
        {
            var sanpham_dv = db.SANPHAM_DV.Include(s => s.DICHVU).Include(s => s.SANPHAM).OrderBy(n => n.MADV);
            return View(sanpham_dv.ToList());
        }

        // GET: SANPHAM_DV/Create
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.MADV = new SelectList(db.DICHVUs, "MADV", "TENDV");
            ViewBag.IDSP = new SelectList(db.SANPHAMs, "IDSP", "TENSP");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MADV,IDSP")] SANPHAM_DV sanpham_dv)
        {
            if (ModelState.IsValid)
            {
                var dichVu = db.DICHVUs.Find(sanpham_dv.MADV);
                var sanPham = db.SANPHAMs.Find(sanpham_dv.IDSP);

                if (dichVu == null || sanPham == null)
                {
                    ModelState.AddModelError("", "Dịch vụ hoặc sản phẩm không tồn tại.");
                }
                else
                {
                    sanpham_dv.TENDV = dichVu.TENDV;
                    sanpham_dv.TENSP = sanPham.TENSP;

                    try
                    {
                        db.SANPHAM_DV.Add(sanpham_dv);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (var validationErrors in ex.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                Debug.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                            }
                        }
                    }
                }
            }

            ViewBag.MADV = new SelectList(db.DICHVUs, "MADV", "TENDV", sanpham_dv.MADV);
            ViewBag.IDSP = new SelectList(db.SANPHAMs, "IDSP", "TENSP", sanpham_dv.IDSP);
            return View(sanpham_dv);
        }
        // GET: SANPHAM_DV/Edit
        public ActionResult Edit(string madv, string idsp)
        {
            if (madv == null || idsp == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM_DV sanpham_dv = db.SANPHAM_DV.Find(madv, idsp);
            if (sanpham_dv == null)
            {
                return HttpNotFound();
            }

            ViewBag.MADV = new SelectList(db.DICHVUs, "MADV", "TENDV", sanpham_dv.MADV);
            ViewBag.IDSP = new SelectList(db.SANPHAMs, "IDSP", "TENSP", sanpham_dv.IDSP);
            return View(sanpham_dv);
        }

        // POST: SANPHAM_DV/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MADV,IDSP")] SANPHAM_DV sanpham_dv)
        {
            Debug.WriteLine("ModelState.IsValid: " + ModelState.IsValid);
            if (ModelState.IsValid)
            {
                var existingSanphamDv = db.SANPHAM_DV.Find(sanpham_dv.MADV, sanpham_dv.IDSP);
                if (existingSanphamDv != null)
                {
                    Debug.WriteLine("Found existing SANPHAM_DV");
                    var dichVu = db.DICHVUs.Find(sanpham_dv.MADV);
                    var sanPham = db.SANPHAMs.Find(sanpham_dv.IDSP);

                    if (dichVu != null && sanPham != null)
                    {
                        existingSanphamDv.TENDV = dichVu.TENDV;
                        existingSanphamDv.TENSP = sanPham.TENSP;

                        db.Entry(existingSanphamDv).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Dịch vụ hoặc sản phẩm không tồn tại.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Mã dịch vụ hoặc mã sản phẩm không tồn tại.");
                }
            }

            ViewBag.MADV = new SelectList(db.DICHVUs, "MADV", "TENDV", sanpham_dv.MADV);
            ViewBag.IDSP = new SelectList(db.SANPHAMs, "IDSP", "TENSP", sanpham_dv.IDSP);
            return View(sanpham_dv);
        }

        // GET: SANPHAM_DV/Delete/5
        public ActionResult Delete(string madv, string idsp)
        {
            if (madv == null || idsp == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM_DV sanpham_dv = db.SANPHAM_DV.Find(madv, idsp);
            if (sanpham_dv == null)
            {
                return HttpNotFound();
            }
            return View(sanpham_dv);
        }

        // POST: SANPHAM_DV/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string madv, string idsp)
        {
            SANPHAM_DV sanpham_dv = db.SANPHAM_DV.Find(madv, idsp);
            db.SANPHAM_DV.Remove(sanpham_dv);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
