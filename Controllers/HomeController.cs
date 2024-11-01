using DoAnPhanMem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnPhanMem.Controllers
{
    public class HomeController : Controller
    {
        private KHOCTHATNHIEUDB1 db = new KHOCTHATNHIEUDB1();
        public ActionResult Index()
        {
            var trangchu = new TRANGCHU()
            {
                NHANVIEN = db.NHANVIENs.ToList(),
                DICHVU = db.DICHVUs.ToList(),
                DANHMUCDV = db.DANHMUCDVs.ToList(),
            };
            return View(trangchu);
        }

        public ActionResult Gioithieu()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Booking()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login","Account");
            }

            var data = Session["TenTk"];
            var khachHang = db.KHACHHANGs.Where(x => x.TENTK == data).FirstOrDefault();
            if (khachHang == null)
            {
                return RedirectToAction("Login", "Account");

            }
            // lấy thông tin khách hàng
            ViewBag.IDKHACHHANG = khachHang.IDKHACHHANG;
            ViewBag.khachHang = khachHang;
            return View();
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

        [HttpPost]
        public ActionResult Booking(LICHHEN datlich)
        {

            //Lấy thông tin tài khoản dựa vào session
            //HttpContext.Session.
            var tentk = Session["TenTk"];
            var khachhang = db.KHACHHANGs.Where(x => x.TENTK == tentk).FirstOrDefault();
            var lichhen = new LICHHEN();
            lichhen.KHACHHANG = khachhang;
            lichhen.IDKHACHHANG = khachhang.IDKHACHHANG;
            lichhen.EMAIL = datlich.EMAIL;
            lichhen.TENKH = datlich.TENKH;
            lichhen.GHICHU = datlich.GHICHU;
            lichhen.NGAY = datlich.NGAY;
            lichhen.TRANGTHAI = datlich.TRANGTHAI;
            lichhen.THOIGIAN = datlich.THOIGIAN;
            lichhen.MADL = GenerateNewMADL();
            lichhen.SODTKH = datlich.SODTKH;
            db.LICHHENs.Add(lichhen);
            db.SaveChanges();
            ViewData["Message"] = "Bạn đã đặt lịch thành công";
      
            return RedirectToAction("Index");
        }
        public ActionResult Dichvu()
        {
            var dichvu = new TRANGCHU()
            {
                DICHVU = db.DICHVUs.ToList(),
                DANHMUCDV = db.DANHMUCDVs.ToList(),
                NHANVIEN = new List<NHANVIEN>() 
            };
            return View(dichvu);
        }
        public ActionResult NhanVien()
        {
            var nhanvien = db.NHANVIENs.ToList();
            return View(nhanvien);
        }
        public ActionResult DanhMuc()
        {
            var danhmucdv = db.DANHMUCDVs.ToList();
            return View(danhmucdv);
        }
        public ActionResult LienHe()
        {
            return View();
        }
        public ActionResult TimKiem()
        {
            return View();
        }
        public ActionResult KhuyenMai()
        {
            var khuyenMai = db.KHUYENMAIs.Where(x => x.THOIGIANBD <= DateTime.Now && x.THOIGIANKT >= DateTime.Now).OrderBy(x => x.THOIGIANBD).ToList();
            return View(khuyenMai);
        }


        public PartialViewResult renderSeach()
        {
            return PartialView("renderSeach");
        }
    }
}