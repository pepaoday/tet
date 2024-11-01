using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DoAnPhanMem.Models;

namespace DoAnPhanMem.Controllers
{
    public class LICHHENController : Controller
    {
        private KHOCTHATNHIEUDB1 db = new KHOCTHATNHIEUDB1();
        public ActionResult Index()
        {
            var lichHen = db.LICHHENs.Include(l => l.KHACHHANG);
            return View(lichHen.ToList());
        }
        public ActionResult Create()
        {
            ViewBag.IDKHACHHANG = new SelectList(db.KHACHHANGs, "IDKHACHHANG", "TENKH");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NGAY,THOIGIAN,TRANGTHAI,GHICHU,EMAIL,SODTKH,IDKHACHHANG")] LICHHEN lICHHEN)
        {
            if (ModelState.IsValid)
            {
                var khachHang = db.KHACHHANGs.Find(lICHHEN.IDKHACHHANG);

                if (khachHang != null)
                {
                    lICHHEN.TENKH = khachHang.TENKH; // Lấy tên khách hàng từ IDKHACHHANG

                    lICHHEN.MADL = GenerateNewMADL();

                    db.LICHHENs.Add(lICHHEN);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Khách hàng không tồn tại.");
                }
            }

            ViewBag.IDKHACHHANG = new SelectList(db.KHACHHANGs, "IDKHACHHANG", "TENKH", lICHHEN.IDKHACHHANG);
            return View(lICHHEN);
        }
        private string GenerateNewMADL()
        {
            var maxMADL = db.LICHHENs.Max(lh => lh.MADL);
            int newId = 1;
            if (!string.IsNullOrEmpty(maxMADL))
            {
                newId = int.Parse(maxMADL.Substring(2)) + 1;
            }
            return "DL" + newId.ToString("D3");
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
