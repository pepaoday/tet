using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DoAnPhanMem.Models;

namespace DoAnPhanMem.Controllers
{
    public class NHANVIEN_DVController : Controller
    {
        private KHOCTHATNHIEUDB1 db = new KHOCTHATNHIEUDB1();

        
        public ActionResult Index()
        {
            var nhanvien_dv = db.NHANVIEN_DV
                                .Include(n => n.DICHVU)
                                .Include(n => n.NHANVIEN)
                                .OrderBy(n => n.IDNHANVIEN);
            return View(nhanvien_dv.ToList());
        }

        
        public ActionResult Create()
        {
            ViewBag.MADV = new SelectList(db.DICHVUs, "MADV", "TENDV");
            ViewBag.IDNHANVIEN = new SelectList(db.NHANVIENs, "IDNHANVIEN", "TENNV");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MADV,IDNHANVIEN")] NHANVIEN_DV nhanvien_dv)
        {
            if (ModelState.IsValid)
            {
                var dichVu = db.DICHVUs.Find(nhanvien_dv.MADV);
                var nhanVien = db.NHANVIENs.Find(nhanvien_dv.IDNHANVIEN);

                if (dichVu != null && nhanVien != null)
                {
                    nhanvien_dv.TENDV = dichVu.TENDV;
                    nhanvien_dv.TENNV = nhanVien.TENNV;

                    db.NHANVIEN_DV.Add(nhanvien_dv);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Dịch vụ hoặc nhân viên không tồn tại.");
                }
            }

            ViewBag.MADV = new SelectList(db.DICHVUs, "MADV", "TENDV", nhanvien_dv.MADV);
            ViewBag.IDNHANVIEN = new SelectList(db.NHANVIENs, "IDNHANVIEN", "TENNV", nhanvien_dv.IDNHANVIEN);
            return View(nhanvien_dv);
        }

        
        public ActionResult Delete(string madv, string idnhanvien)
        {
            if (madv == null || idnhanvien == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHANVIEN_DV nhanvien_dv = db.NHANVIEN_DV.Find(madv, idnhanvien);
            if (nhanvien_dv == null)
            {
                return HttpNotFound();
            }
            return View(nhanvien_dv);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string madv, string idnhanvien)
        {
            NHANVIEN_DV nhanvien_dv = db.NHANVIEN_DV.Find(madv, idnhanvien);
            db.NHANVIEN_DV.Remove(nhanvien_dv);
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
