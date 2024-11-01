using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAnPhanMem.Models;

namespace DoAnPhanMem.Controllers
{
    public class SANPHAMController : Controller
    {
        private KHOCTHATNHIEUDB1 db = new KHOCTHATNHIEUDB1();

        // GET: SANPHAM
        public ActionResult Index()
        {
            var sanphams = db.SANPHAMs.Include(s => s.DANHMUCSP);
            return View(sanphams.ToList());
        }

        // GET: SANPHAM/Create
        public ActionResult Create()
        {
            ViewBag.IDDM = new SelectList(db.DANHMUCSPs, "IDDM", "TENDM");

            // Truyền dữ liệu thô cho DONVITINH
            ViewBag.DONVITINH = new SelectList(new List<string> { "Bộ", "Chai" });

            return View();
        }

        // POST: SANPHAM/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDSP,TENSP,MOTA,IMG,IMG2,IMG3,GIA,TONGDANHGIA,THUE,GIAMGIA,IDDM,DONVITINH,SOLUONG")] SANPHAM sanpham, HttpPostedFileBase uploadIMG, HttpPostedFileBase uploadIMG2, HttpPostedFileBase uploadIMG3)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    sanpham.IDSP = GenerateNewIDSP(); // Generate new IDSP

                    if (uploadIMG != null && uploadIMG.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(uploadIMG.FileName);
                        var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                        uploadIMG.SaveAs(path);
                        sanpham.IMG = fileName;
                    }

                    if (uploadIMG2 != null && uploadIMG2.ContentLength > 0)
                    {
                        var fileName2 = Path.GetFileName(uploadIMG2.FileName);
                        var path2 = Path.Combine(Server.MapPath("~/Images/"), fileName2);
                        uploadIMG2.SaveAs(path2);
                        sanpham.IMG2 = fileName2;
                    }

                    if (uploadIMG3 != null && uploadIMG3.ContentLength > 0)
                    {
                        var fileName3 = Path.GetFileName(uploadIMG3.FileName);
                        var path3 = Path.Combine(Server.MapPath("~/Images/"), fileName3);
                        uploadIMG3.SaveAs(path3);
                        sanpham.IMG3 = fileName3;
                    }

                    db.SANPHAMs.Add(sanpham);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
            }

            ViewBag.IDDM = new SelectList(db.DANHMUCSPs, "IDDM", "TENDM", sanpham.IDDM);

            // Truyền lại dữ liệu thô cho DONVITINH trong trường hợp có lỗi
            ViewBag.DONVITINH = new SelectList(new List<string> { "Bộ", "Chai" }, sanpham.DONVITINH);

            return View(sanpham);
        }

        // GET: SANPHAM/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SANPHAM sanpham = db.SANPHAMs.Find(id);
            if (sanpham == null)
            {
                return HttpNotFound();
            }

            ViewBag.IDDM = new SelectList(db.DANHMUCSPs, "IDDM", "TENDM", sanpham.IDDM);
            ViewBag.DONVITINH = new SelectList(new List<string> { "Bộ", "Chai" }, sanpham.DONVITINH);

            return View(sanpham);
        }

        // POST: SANPHAM/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDSP,TENSP,MOTA,IMG,IMG2,IMG3,GIA,GIAMOI,TONGDANHGIA,THUE,GIAMGIA,IDDM,DONVITINH,SOLUONG")] SANPHAM sanpham, HttpPostedFileBase uploadIMG, HttpPostedFileBase uploadIMG2, HttpPostedFileBase uploadIMG3)
        {
            if (ModelState.IsValid)
            {
                var existingSanpham = db.SANPHAMs.Find(sanpham.IDSP);
                if (existingSanpham == null)
                {
                    return HttpNotFound();
                }

                try
                {
                    if (uploadIMG != null && uploadIMG.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(uploadIMG.FileName);
                        var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                        uploadIMG.SaveAs(path);
                        existingSanpham.IMG = fileName;
                    }

                    if (uploadIMG2 != null && uploadIMG2.ContentLength > 0)
                    {
                        var fileName2 = Path.GetFileName(uploadIMG2.FileName);
                        var path2 = Path.Combine(Server.MapPath("~/Images/"), fileName2);
                        uploadIMG2.SaveAs(path2);
                        existingSanpham.IMG2 = fileName2;
                    }

                    if (uploadIMG3 != null && uploadIMG3.ContentLength > 0)
                    {
                        var fileName3 = Path.GetFileName(uploadIMG3.FileName);
                        var path3 = Path.Combine(Server.MapPath("~/Images/"), fileName3);
                        uploadIMG3.SaveAs(path3);
                        existingSanpham.IMG3 = fileName3;
                    }
                    else
                    {
                        // Giữ lại giá trị hình ảnh hiện tại
                        sanpham.IMG = existingSanpham.IMG;
                        sanpham.IMG2 = existingSanpham.IMG2;
                        sanpham.IMG3 = existingSanpham.IMG3;
                    }
                    existingSanpham.TENSP = sanpham.TENSP;
                    existingSanpham.MOTA = sanpham.MOTA;
                    existingSanpham.GIA = sanpham.GIA;
                    existingSanpham.TONGDANHGIA = sanpham.TONGDANHGIA;
                    existingSanpham.THUE = sanpham.THUE;
                    existingSanpham.GIAMGIA = sanpham.GIAMGIA;
                    existingSanpham.IDDM = sanpham.IDDM;
                    existingSanpham.DONVITINH = sanpham.DONVITINH;
                    existingSanpham.SOLUONG = sanpham.SOLUONG;
                    db.Entry(existingSanpham).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
            }

            ViewBag.IDDM = new SelectList(db.DANHMUCSPs, "IDDM", "TENDM", sanpham.IDDM);

            // Truyền lại dữ liệu thô cho DONVITINH trong trường hợp có lỗi
            ViewBag.DONVITINH = new SelectList(new List<string> { "Bộ", "Chai" }, sanpham.DONVITINH);

            return View(sanpham);
        }

        // Phương thức tạo IDSP mới
        private string GenerateNewIDSP()
        {
            var maxIDSP = db.SANPHAMs.Max(sp => sp.IDSP);
            int newId = 1;
            if (!string.IsNullOrEmpty(maxIDSP) && maxIDSP.StartsWith("SP"))
            {
                int.TryParse(maxIDSP.Substring(2), out newId);
                newId++;
            }
            return "SP" + newId.ToString("D3"); // "D3" for padding with zeroes to ensure three digits
        }
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SANPHAM sanpham = db.SANPHAMs.Find(id);
            if (sanpham == null)
            {
                return HttpNotFound();
            }

            return View(sanpham);
        }

        // POST: SANPHAM/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            SANPHAM sanpham = db.SANPHAMs.Find(id);
            db.SANPHAMs.Remove(sanpham);
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

        public PartialViewResult GetProduct()
        {
            var product = db.SANPHAMs.ToList();
            return PartialView("GetProduct", product);
        }
    }
}