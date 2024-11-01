using DoAnPhanMem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DoAnPhanMem.Controllers
{
    public class TAIKHOANController : Controller
    {
        private KHOCTHATNHIEUDB1 db = new KHOCTHATNHIEUDB1();

        // GET: TAIKHOAN
        public ActionResult Index()
        {
            var taikhoans = db.TAIKHOANs.ToList();
            return View(taikhoans);
        }

        // GET: TAIKHOAN/Create
        public ActionResult Create()
        {
            ViewBag.LOAITK = new SelectList(new List<string> { "QL", "NV", "KH" });
            return View();
        }

        // POST: TAIKHOAN/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TENTK,PASS,EMAIL,LOAITK")] TAIKHOAN taikhoan)
        {
            if (ModelState.IsValid)
            {
                db.TAIKHOANs.Add(taikhoan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LOAITK = new SelectList(new List<string> { "QL", "NV", "KH" }, taikhoan.LOAITK);

            return View(taikhoan);
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TAIKHOAN taikhoan = db.TAIKHOANs.Find(id);
            if (taikhoan == null)
            {
                return HttpNotFound();
            }



            ViewBag.LOAITK = new SelectList(new List<string> { "QL", "NV", "KH" }, taikhoan.LOAITK);

            return View(taikhoan);
        }




        // POST: TAIKHOAN/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TENTK,PASS,EMAIL,LOAITK")] TAIKHOAN taikhoan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(taikhoan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Reload data for LOAITK dropdown in case of an error
            ViewBag.LOAITK = new SelectList(new List<string> { "QL", "NV", "KH" }, taikhoan.LOAITK);

            return View(taikhoan);
        }

        // GET: TAIKHOAN/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAIKHOAN taikhoan = db.TAIKHOANs.Find(id);
            if (taikhoan == null)
            {
                return HttpNotFound();
            }
            return View(taikhoan);
        }

        // POST: TAIKHOAN/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TAIKHOAN taikhoan = db.TAIKHOANs.Find(id);
            db.TAIKHOANs.Remove(taikhoan);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
