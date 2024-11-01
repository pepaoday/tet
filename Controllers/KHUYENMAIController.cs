using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAnPhanMem.Models;

namespace DoAnPhanMem.Controllers
{
    public class KHUYENMAIController : Controller
    {
        private KHOCTHATNHIEUDB1 db = new KHOCTHATNHIEUDB1();

       
        public ActionResult Index()
        {
            return View(db.KHUYENMAIs.ToList());
        }

         
        public ActionResult Create()
        {
            
            var newMAKM = GenerateNewMAKM();
            ViewBag.NewMAKM = newMAKM;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TENCTKM,GIAMGIA,THOIGIANBD,THOIGIANKT")] KHUYENMAI khuyenMai)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    khuyenMai.MAKM = GenerateNewMAKM();

                    db.KHUYENMAIs.Add(khuyenMai);
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

                    
                    System.Diagnostics.Debug.WriteLine(ex.EntityValidationErrors.SelectMany(e => e.ValidationErrors).Select(e => e.ErrorMessage).ToString());
                }
            }

            return View(khuyenMai);
        }

        
        private string GenerateNewMAKM()
        {
            var maxMAKM = db.KHUYENMAIs.Max(km => km.MAKM);
            int newId = 1;
            if (!string.IsNullOrEmpty(maxMAKM))
            {
                newId = int.Parse(maxMAKM.Substring(2)) + 1; 
            }
            return "KM" + newId.ToString("D3");
        }
        
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHUYENMAI khuyenMai = db.KHUYENMAIs.Find(id);
            if (khuyenMai == null)
            {
                return HttpNotFound();
            }
            return View(khuyenMai);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MAKM,TENCTKM,GIAMGIA,THOIGIANBD,THOIGIANKT")] KHUYENMAI khuyenMai)
        {
            if (ModelState.IsValid)
            {
                db.Entry(khuyenMai).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(khuyenMai);
        }
        
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHUYENMAI khuyenMai = db.KHUYENMAIs.Find(id);
            if (khuyenMai == null)
            {
                return HttpNotFound();
            }
            return View(khuyenMai);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(string id)
        {
            var result = new { success = false, message = "Xóa không thành công." };

            if (string.IsNullOrEmpty(id))
            {
                return Json(result);
            }

            try
            {
                KHUYENMAI khuyenMai = db.KHUYENMAIs.Find(id);
                if (khuyenMai == null)
                {
                    result = new { success = false, message = "Khuyến mãi không tìm thấy." };
                    return Json(result);
                }

                db.KHUYENMAIs.Remove(khuyenMai);
                db.SaveChanges();

                result = new { success = true, message = "Khuyến mãi đã được xóa thành công." };
            }
            catch (Exception ex)
            {
                result = new { success = false, message = "Đã xảy ra lỗi: " + ex.Message };
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
