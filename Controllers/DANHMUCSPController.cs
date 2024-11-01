using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnPhanMem.Models;
using System.IO;
using System.Data.Entity;
using System.Net;

namespace DoAnPhanMem.Controllers
{
    public class DANHMUCSPController : Controller
    {
        private KHOCTHATNHIEUDB1 db = new KHOCTHATNHIEUDB1();

       
        public ActionResult Index()
        {
            return View(db.DANHMUCSPs.ToList());
        }

        
        public ActionResult Create()
        {
            var newIDDM = GenerateNewIDDM();
            ViewBag.NewIDDM = newIDDM;
            ViewBag.IDDM = new SelectList(db.DANHMUCSPs, "IDDM", "TENDM");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DANHMUCSP danhmucsp, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                
                danhmucsp.IDDM = GenerateNewIDDM();

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    file.SaveAs(path);
                    danhmucsp.IMG = fileName;
                }
                db.DANHMUCSPs.Add(danhmucsp);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(danhmucsp);
        }

        
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DANHMUCSP danhmucsp = db.DANHMUCSPs.Find(id);
            if (danhmucsp == null)
            {
                return HttpNotFound();
            }
            return View(danhmucsp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DANHMUCSP danhmucsp, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                var existingDanhmucsp = db.DANHMUCSPs.Find(danhmucsp.IDDM);
                if (existingDanhmucsp == null)
                {
                    return HttpNotFound();
                }

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    file.SaveAs(path);
                    existingDanhmucsp.IMG = fileName;
                }

                existingDanhmucsp.TENDM = danhmucsp.TENDM;
                

                db.Entry(existingDanhmucsp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(danhmucsp);
        }

        
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DANHMUCSP danhmucsp = db.DANHMUCSPs.Find(id);
            if (danhmucsp == null)
            {
                return HttpNotFound();
            }
            return View(danhmucsp);
        }

        
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DANHMUCSP danhmucsp = db.DANHMUCSPs.Find(id);
            if (danhmucsp == null)
            {
                return HttpNotFound();
            }
            return View(danhmucsp);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DANHMUCSP danhmucsp = db.DANHMUCSPs.Find(id);
            db.DANHMUCSPs.Remove(danhmucsp);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        private string GenerateNewIDDM()
        {
            var allIDDMS = db.DANHMUCSPs.Select(dm => dm.IDDM).ToList();
            int maxId = 0;

            foreach (var iddm in allIDDMS)
            {
                if (!string.IsNullOrEmpty(iddm) && iddm.StartsWith("DMSP"))
                {
                    string numericPart = iddm.Substring(4); 
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
            return "DMSP" + newId.ToString("D3"); 
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
