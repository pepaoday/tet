using DoAnPhanMem.Dtos;
using DoAnPhanMem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace DoAnPhanMem.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin

        private KHOCTHATNHIEUDB1 db = new KHOCTHATNHIEUDB1();
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public PartialViewResult SastiffyAll()
        {
            var totalOrder = db.LICHHENs.Count(x => x.TRANGTHAI == true);
            var totalOrderFailed = db.LICHHENs.Count(x => x.TRANGTHAI == false);
            var totalProduct = db.SANPHAMs.Count();
            var totalNhanVien = db.NHANVIENs.Count();
            var totalDichVu = db.DICHVUs.Count();


            ViewBag.totalOrder = totalOrder;
            ViewBag.totalOrderFailed = totalOrderFailed;
            ViewBag.totalProduct = totalProduct;
            ViewBag.totalNhanVien = totalNhanVien;
            ViewBag.totalDichVu = totalDichVu;
            return PartialView("SastiffyAll");
        }


        [HttpGet]
        public PartialViewResult DoanhThuDichVu()
        {
            var doanhThuDichVu = db.HOADON_DV
                .GroupBy(hd => hd.DICHVU.TENDV)
                .Select(g => new
                {
                    DICHVU = g.Key,
                    TONGDOANHTHU = g.Sum(x => x.THANHTIEN)
                })
                .ToList();
            ViewBag.doanhThuDichVu = doanhThuDichVu;
            return PartialView("DoanhThuDichVu");
        }
        [HttpGet]

        // GET: ThongKe/SanPham
        public PartialViewResult DoanhThuSanPham()
        {
            var doanhThuSanPham = db.HOADON_SP
                .GroupBy(hd => hd.SANPHAM.TENSP)
                .Select(g => new
                {
                    SANPHAM = g.Key,
                    TONGDOANHTHU = g.Sum(x => x.TONGTIEN)
                })
                .ToList();
            ViewBag.doanhThuSanPham = doanhThuSanPham;
            return PartialView("DoanhThuSanPham");
        }


        [HttpGet]
        public PartialViewResult SastiffyDoanhThu()
        {
            int year = DateTime.Now.Year;
            var data = db.HOADONs.Where(x => x.NGAY.Year == year).GroupBy(x => x.NGAY.Month).Select(g => new
            {
                Month = g.Key,
                Count = g.Count(),
                Total = g.Sum(t => t.TONGGIA)
            }).ToList();

            ViewBag.totalMonth = data;
            return PartialView("SastiffyDoanhThu");
        }
        [HttpGet]
        public ActionResult LichHen(int pageIndex = 1,int pageSize = 10)
        {
            var data = db.LICHHENs.AsQueryable().Where(x => x.TRANGTHAI == false).OrderByDescending(x => x.THOIGIAN).Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(x => new LichHenDto
            {
                MADL = x.MADL,
                EMAIL = x.EMAIL,
                TENKH = x.TENKH,
                GHICHU = x.GHICHU,
                IDKHACHHANG = x.IDKHACHHANG,
                NGAY = x.NGAY,
                SODTKH = x.SODTKH,
                THOIGIAN = x.THOIGIAN,
                TRANGTHAI = x.TRANGTHAI,
            }).ToList();
            var totalPage = (int)Math.Ceiling((decimal)db.LICHHENs.AsQueryable().Where(x => x.TRANGTHAI == false).Count() / pageSize);
            var result = new Result()
            {
                data = data,
                totalPages = totalPage,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            return View(result);
        }


        [HttpGet]
        public ActionResult LichSu(int pageIndex = 1, int pageSize = 10, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var data = db.LICHHENs.AsQueryable().Where(x => x.TRANGTHAI == true);


            if (fromDate.HasValue && toDate.HasValue)
            {
                data = data.Where(x => x.NGAY >= fromDate.Value && x.NGAY <= toDate.Value);
            }

           
            var pagedData = data.OrderByDescending(x => x.NGAY)
                                .ThenByDescending(x => x.THOIGIAN)
                                .Skip((pageIndex - 1) * pageSize)
                                .Take(pageSize)
                                .Select(x => new LichHenDto
                                {
                                    MADL = x.MADL,
                                    EMAIL = x.EMAIL,
                                    TENKH = x.TENKH,
                                    GHICHU = x.GHICHU,
                                    IDKHACHHANG = x.IDKHACHHANG,
                                    NGAY = x.NGAY,
                                    SODTKH = x.SODTKH,
                                    THOIGIAN = x.THOIGIAN,
                                    TRANGTHAI = x.TRANGTHAI,
                                }).ToList();

       
            var totalPage = (int)Math.Ceiling((decimal)data.Count() / pageSize);
            var result = new Result()
            {
                data = pagedData,
                totalPages = totalPage,
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            return View(result);
        }


        [HttpGet]
        public JsonResult getLichHen(int pageIndex = 1, int pageSize = 10)
        {
            var data = db.LICHHENs.AsQueryable().OrderByDescending(x => x.NGAY).ThenByDescending(x => x.THOIGIAN).Where(x => x.TRANGTHAI == false).Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(x => new LichHenDto
            {
                MADL = x.MADL,
                EMAIL = x.EMAIL,
                TENKH = x.TENKH,
                GHICHU = x.GHICHU,
                IDKHACHHANG = x.IDKHACHHANG,
                NGAY = x.NGAY,
                SODTKH = x.SODTKH,
                THOIGIAN = x.THOIGIAN,
                TRANGTHAI = x.TRANGTHAI,
            }).ToList();
            var totalPage = (int)Math.Ceiling((decimal)db.LICHHENs.AsQueryable().Where(x => x.TRANGTHAI == false).Count() / pageSize);
            var result = new Result()
            {
                data = data,
                totalPages = totalPage,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ActiveLich(string MaDl)
        {
            var result = false;
            var dl = db.LICHHENs.FirstOrDefault(x => x.MADL == MaDl);
            try
            {
                if (dl != null)
                {
                    dl.TRANGTHAI = true;
                    db.SaveChanges();
                    result = true;
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult Delete(string MaDL)
        {
            var result = false;
            var dl = db.LICHHENs.FirstOrDefault(x => x.MADL == MaDL);
            try
            {
                if (dl != null)
                {
                    db.LICHHENs.Remove(dl);
                    db.SaveChanges();
                    result = true;
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult HoaDon(int pageIndex = 1, int pageSize = 10, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var data = db.HOADONs.AsQueryable();

            if (fromDate.HasValue && toDate.HasValue)
            {
                data = data.Where(x => x.NGAY >= fromDate.Value && x.NGAY <= toDate.Value);
            }

            var pagedData = data.OrderByDescending(x => x.NGAY).ThenByDescending(x => x.GIO)
                                .Skip((pageIndex - 1) * pageSize)
                                .Take(pageSize)
                                .Select(x => new HoaDonDto
                                {
                                    IDHOADON = x.IDHOADON,
                                    SODT = x.SODT,
                                    TENKH = x.TENKH,
                                    TENNV = x.TENNV,
                                    MAKM = x.MAKM,
                                    SOLUONG = x.SOLUONG,
                                    TONGGIA = x.TONGGIA,
                                    NGAY = x.NGAY,
                                    GIO = x.GIO
                                }).ToList();

            var totalPage = (int)Math.Ceiling((decimal)data.Count() / pageSize);
            var result = new Result()
            {
                data = pagedData,
                totalPages = totalPage,
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            return View(result);
        }

    }
}