using DoAnPhanMem.Dtos;
using DoAnPhanMem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DoAnPhanMem.Controllers
{
    public class DICHVUController : Controller
    {
        private KHOCTHATNHIEUDB1 db = new KHOCTHATNHIEUDB1();

        
        public ActionResult Index()
        {
            var dichvus = db.DICHVUs.Include(d => d.DANHMUCDV);
            return View(dichvus.ToList());
        }

        
        public ActionResult Create()
        {
            ViewBag.IDDM = new SelectList(db.DANHMUCDVs, "IDDM", "TENDM");
            ViewBag.DONVITINH = new SelectList(new List<string> { "Bộ", "Lần" });
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MADV,TENDV,MOTA,IMG,IMG2,IMG3,GIA,TONGDANHGIA,IDDM,DONVITINH,GIAMGIA")] DICHVU dichvu, HttpPostedFileBase uploadIMG, HttpPostedFileBase uploadIMG2, HttpPostedFileBase uploadIMG3)
        {
            if (ModelState.IsValid)
            {
                dichvu.MADV = GenerateNewMADV();
                if (uploadIMG != null && uploadIMG.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(uploadIMG.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    uploadIMG.SaveAs(path);
                    dichvu.IMG = fileName;
                }

                if (uploadIMG2 != null && uploadIMG2.ContentLength > 0)
                {
                    var fileName2 = Path.GetFileName(uploadIMG2.FileName);
                    var path2 = Path.Combine(Server.MapPath("~/Images/"), fileName2);
                    uploadIMG2.SaveAs(path2);
                    dichvu.IMG2 = fileName2;
                }

                if (uploadIMG3 != null && uploadIMG3.ContentLength > 0)
                {
                    var fileName3 = Path.GetFileName(uploadIMG3.FileName);
                    var path3 = Path.Combine(Server.MapPath("~/Images/"), fileName3);
                    uploadIMG3.SaveAs(path3);
                    dichvu.IMG3 = fileName3;
                }

                db.DICHVUs.Add(dichvu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDDM = new SelectList(db.DANHMUCDVs, "IDDM", "TENDM", dichvu.IDDM);
            ViewBag.DONVITINH = new SelectList(new List<string> { "Bộ", "Lần" }, dichvu.DONVITINH);
            return View(dichvu);
        }

        
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DICHVU dichvu = db.DICHVUs.Find(id);
            if (dichvu == null)
            {
                return HttpNotFound();
            }

            ViewBag.IDDM = new SelectList(db.DANHMUCDVs, "IDDM", "TENDM", dichvu.IDDM);
            ViewBag.DONVITINH = new SelectList(new List<string> { "Bộ", "Lần" }, dichvu.DONVITINH);
            return View(dichvu);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MADV,TENDV,MOTA,GIA,TONGDANHGIA,IDDM,DONVITINH,GIAMGIA")] DICHVU dichvu, HttpPostedFileBase uploadIMG, HttpPostedFileBase uploadIMG2, HttpPostedFileBase uploadIMG3)
        {
            if (ModelState.IsValid)
            {
                
                var existingDichvu = db.DICHVUs.Find(dichvu.MADV);
                if (existingDichvu == null)
                {
                    return HttpNotFound();
                }

                
                existingDichvu.TENDV = dichvu.TENDV;
                existingDichvu.MOTA = dichvu.MOTA;
                existingDichvu.GIA = dichvu.GIA;
                
                existingDichvu.TONGDANHGIA = dichvu.TONGDANHGIA;
                existingDichvu.IDDM = dichvu.IDDM;
                existingDichvu.DONVITINH = dichvu.DONVITINH;
                existingDichvu.GIAMGIA = dichvu.GIAMGIA;

                try
                {
                    
                    if (uploadIMG != null && uploadIMG.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(uploadIMG.FileName);
                        var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                        uploadIMG.SaveAs(path);
                        existingDichvu.IMG = fileName;
                    }

                    if (uploadIMG2 != null && uploadIMG2.ContentLength > 0)
                    {
                        var fileName2 = Path.GetFileName(uploadIMG2.FileName);
                        var path2 = Path.Combine(Server.MapPath("~/Images/"), fileName2);
                        uploadIMG2.SaveAs(path2);
                        existingDichvu.IMG2 = fileName2;
                    }

                    if (uploadIMG3 != null && uploadIMG3.ContentLength > 0)
                    {
                        var fileName3 = Path.GetFileName(uploadIMG3.FileName);
                        var path3 = Path.Combine(Server.MapPath("~/Images/"), fileName3);
                        uploadIMG3.SaveAs(path3);
                        existingDichvu.IMG3 = fileName3;
                    }

                    db.Entry(existingDichvu).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    
                    ModelState.AddModelError("", "Lỗi khi cập nhật dịch vụ: " + ex.Message);
                }
            }

            
            ViewBag.IDDM = new SelectList(db.DANHMUCDVs, "IDDM", "TENDM", dichvu.IDDM);
            ViewBag.DONVITINH = new SelectList(new List<string> { "Bộ", "Lần" }, dichvu.DONVITINH);
            return View(dichvu);
        }
        private string GenerateNewMADV()
        {
            var maxMADV = db.DICHVUs.Max(sp => sp.MADV);
            int newId = 1;

            if (!string.IsNullOrEmpty(maxMADV) && maxMADV.StartsWith("DV"))
            {
                int.TryParse(maxMADV.Substring(2), out newId);
                newId++;
            }

            return "DV" + newId.ToString("D3"); // Ensuring 4 digits with leading zeros
        }





        [HttpGet]
        [AllowAnonymous]
        public ActionResult FindBy(string IDDM)
        {
            var sanPham = db.DICHVUs.Where(x => x.IDDM == IDDM).FirstOrDefault();
            return PartialView("FindById", sanPham);
        }



        public PartialViewResult SearchByName(string name)
        {
            var results = db.DICHVUs.Where(d => d.TENDV.Contains(name)).ToList();
            return PartialView("SearchByName", results);
        } 
        public JsonResult SearchByNameJson(string name)
        {
            var results = db.DICHVUs.Where(d => d.TENDV.Contains(name)).Select(x => new searchdto
            {
                TENDV = x.TENDV,
                IDDM = x.IDDM
            }).ToList();
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DICHVU dichvu = db.DICHVUs.Find(id);
            if (dichvu == null)
            {
                return HttpNotFound();
            }
            return View(dichvu);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DICHVU dichvu = db.DICHVUs.Find(id);
            db.DICHVUs.Remove(dichvu);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
