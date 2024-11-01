using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAnPhanMem.Models;

namespace DoAnPhanMem.Controllers
{
    public class NHANVIENController : Controller
    {
        private KHOCTHATNHIEUDB1 db = new KHOCTHATNHIEUDB1();

        
        public ActionResult Index()
        {
            var nhanviens = db.NHANVIENs.Include(n => n.TAIKHOAN);
            return View(nhanviens.ToList());
        }

        
        public ActionResult Create()
        {
            ViewBag.TENTK = new SelectList(db.TAIKHOANs, "TENTK", "TENTK");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TENNV,GIOITINH,DIACHI,TONGDANHGIA,NGAYSINH,SODT,CHUCVU,TENTK")] NHANVIEN nhanvien)
        {
            if (ModelState.IsValid)
            {
                // Generate new ID
                nhanvien.IDNHANVIEN = GenerateNewIDNHANVIEN();

                db.NHANVIENs.Add(nhanvien);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TENTK = new SelectList(db.TAIKHOANs, "TENTK", "TENTK", nhanvien.TENTK);
            return View(nhanvien);
        }


        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHANVIEN nhanvien = db.NHANVIENs.Find(id);
            if (nhanvien == null)
            {
                return HttpNotFound();
            }
            ViewBag.TENTK = new SelectList(db.TAIKHOANs, "TENTK", "TENTK", nhanvien.TENTK);
            return View(nhanvien);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDNHANVIEN,TENNV,GIOITINH,DIACHI,TONGDANHGIA,NGAYSINH,SODT,CHUCVU,TENTK")] NHANVIEN nhanvien)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(nhanvien.IDNHANVIEN)) // Kiểm tra IDNHANVIEN có phải null không
                {
                    ModelState.AddModelError("", "ID Nhân Viên không hợp lệ.");
                    return View(nhanvien);
                }

                db.Entry(nhanvien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TENTK = new SelectList(db.TAIKHOANs, "TENTK", "TENTK", nhanvien.TENTK);
            return View(nhanvien);
        }

        private string GenerateNewIDNHANVIEN()
        {
            // Get the last ID from the database
            var lastID = db.NHANVIENs
                .OrderByDescending(n => n.IDNHANVIEN)
                .Select(n => n.IDNHANVIEN)
                .FirstOrDefault();

            if (lastID == null)
            {
                return "NV001"; // Starting ID
            }

            // Extract the numeric part and increment it
            var numericPart = lastID.Substring(2);
            var newNumericPart = (int.Parse(numericPart) + 1).ToString("D3"); // Pad with zeros

            return "NV" + newNumericPart;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(string id)
        {
            var result = new { success = false, message = "Xóa không thành công." };
            if (id != null)
            {
                NHANVIEN nhanvien = db.NHANVIENs.Find(id);
                if (nhanvien != null)
                {
                    db.NHANVIENs.Remove(nhanvien);
                    db.SaveChanges();
                    result = new { success = true, message = "Xóa thành công." };
                }
            }
            return Json(result);
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
