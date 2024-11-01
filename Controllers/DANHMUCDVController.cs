using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using DoAnPhanMem.Models;

namespace DoAnPhanMem.Controllers
{
    public class DANHMUCDVController : Controller
    {
        private KHOCTHATNHIEUDB1 db = new KHOCTHATNHIEUDB1();

        
        public ActionResult Index()
        {
            return View(db.DANHMUCDVs.ToList());
        }

        
        public ActionResult Create()
        {
            var newIDDM = GenerateNewIDDM();
            ViewBag.NewIDDM = newIDDM;
            ViewBag.IDDM = new SelectList(db.DANHMUCDVs, "IDDM", "TENDM");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DANHMUCDV dANHMUCDV, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                dANHMUCDV.IDDM = GenerateNewIDDM();
                if (upload != null && upload.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(upload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    upload.SaveAs(path);
                    dANHMUCDV.IMG = fileName;
                }
                db.DANHMUCDVs.Add(dANHMUCDV);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dANHMUCDV);
        }

        
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DANHMUCDV danhMuc = db.DANHMUCDVs.Find(id);
            if (danhMuc == null)
            {
                return HttpNotFound();
            }
            return View(danhMuc);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DANHMUCDV danhMuc, HttpPostedFileBase imgFile)
        {
            if (ModelState.IsValid)
            {
                var existingDanhMuc = db.DANHMUCDVs.Find(danhMuc.IDDM);
                if (existingDanhMuc == null)
                {
                    return HttpNotFound();
                }

                
                if (imgFile != null && imgFile.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(imgFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    imgFile.SaveAs(path);
                    existingDanhMuc.IMG = fileName;
                }

                existingDanhMuc.TENDM = danhMuc.TENDM;

                db.Entry(existingDanhMuc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(danhMuc);
        }


        
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DANHMUCDV danhMuc = db.DANHMUCDVs.Find(id);
            if (danhMuc == null)
            {
                return HttpNotFound();
            }
            return View(danhMuc);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DANHMUCDV danhMuc = db.DANHMUCDVs.Find(id);
            db.DANHMUCDVs.Remove(danhMuc);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        private string GenerateNewIDDM()
        {
            var allIDDMS = db.DANHMUCDVs.Select(dm => dm.IDDM).ToList();
            int maxId = 0;

            foreach (var iddm in allIDDMS)
            {
                if (!string.IsNullOrEmpty(iddm) && iddm.StartsWith("DMDV"))
                {
                    string numericPart = iddm.Substring(4); // Bỏ qua "DMSP"
                    if (int.TryParse(numericPart, out int parsedId))
                    {
                        if (parsedId > maxId)
                        {
                            maxId = parsedId;
                        }
                    }
                    else
                    {
                        
                        System.Diagnostics.Debug.WriteLine($"Invalid IDDM format: {iddm}");
                    }
                }
                else
                {
                    
                    System.Diagnostics.Debug.WriteLine($"Invalid IDDM format: {iddm}");
                }
            }

            int newId = maxId + 1;
            return "DMDV" + newId.ToString("D3"); // "D3" for padding with zeroes to ensure three digits
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
