using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DoAnPhanMem.Dtos;
using DoAnPhanMem.Models;

namespace DoAnPhanMem.Controllers
{
    public class HOADONController : Controller
    {
        private KHOCTHATNHIEUDB1 db = new KHOCTHATNHIEUDB1();

        public ActionResult Index()
        {
            var hoadons = db.HOADONs.Include(h => h.KHACHHANG).Include(h => h.NHANVIEN).Include(h => h.KHUYENMAI);
            return View(hoadons.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.IDKHACHHANG = new SelectList(db.KHACHHANGs, "IDKHACHHANG", "TENKH");
            ViewBag.IDNHANVIEN = new SelectList(db.NHANVIENs, "IDNHANVIEN", "TENNV");

            var khuyenMaiList = db.KHUYENMAIs.Select(km => new {
                MAKM = km.MAKM,
                TENCTKM = km.TENCTKM
            }).ToList();

            // Thêm một mục rỗng vào đầu danh sách
            khuyenMaiList.Insert(0, new { MAKM = "", TENCTKM = "Không" });

            ViewBag.MAKM = new SelectList(khuyenMaiList, "MAKM", "TENCTKM");

            //ViewBag.MAKM = new SelectList(db.KHUYENMAIs, "MAKM", "TENCTKM");
            ViewBag.dropdownDichvu = new SelectList(db.DICHVUs, "MADV", "TENDV");
            ViewBag.dropdownSanPham = new SelectList(db.SANPHAMs, "IDSP", "TENSP");

            return View();
        }


        [HttpGet]
        public ActionResult GetByMa(string IDHD)
        {
            var data = db.HOADONs.FirstOrDefault(x => x.IDHOADON == IDHD);
            return View(data);
        }


        // GET: HoaDon/Edit/5
        public ActionResult Edit(string IDHOADON)
        {
            var hoadon = db.HOADONs.Find(IDHOADON);
            if (hoadon == null)
            {
                return HttpNotFound();
            }
            var listHoaDv = db.HOADON_DV.Where(x => x.IDHOADON == IDHOADON).Select(x => new { x.MADV,x.SOLUONG, TENDV = x.DICHVU.TENDV}).ToList();
            var listSp = db.HOADON_SP.Where(x => x.IDHOADON == IDHOADON).Select(x => new {x.IDSP, x.SOLUONG, TENSP = x.SANPHAM.TENSP }).ToList();
            var hoadonVM = new HoaDonVM
            {
                IDHOADON = hoadon.IDHOADON,
                TENKH = hoadon.TENKH,
                SOLUONG = hoadon.SOLUONG,
                TONGGIA = hoadon.TONGGIA,
                TENNV = hoadon.TENNV,
                MAKM = hoadon.MAKM,
                SODT = hoadon.SODT,
                IDKHACHHANG = hoadon.IDKHACHHANG,
                IDNHANVIEN = hoadon.IDNHANVIEN,
                MADV = listHoaDv.Select(x => x.MADV).ToList(),
                IDSP = listSp.Select(x => x.IDSP).ToList(),
                // Add any additional properties required for editing
            };

            // Populate dropdowns
            ViewBag.IDKHACHHANG = new SelectList(db.KHACHHANGs, "IDKHACHHANG", "TENKH", hoadon.IDKHACHHANG);
            ViewBag.IDNHANVIEN = new SelectList(db.NHANVIENs, "IDNHANVIEN", "TENNV", hoadon.IDNHANVIEN);

            var khuyenMaiList = db.KHUYENMAIs.Select(km => new {
                MAKM = km.MAKM,
                TENCTKM = km.TENCTKM
            }).ToList();
            khuyenMaiList.Insert(0, new { MAKM = "", TENCTKM = "Không" });
            ViewBag.MAKM = new SelectList(khuyenMaiList, "MAKM", "TENCTKM", hoadon.MAKM);


            ViewBag.dropdownDichvu = new SelectList(db.DICHVUs, "MADV", "TENDV");
            ViewBag.dropdownSanPham = new SelectList(db.SANPHAMs, "IDSP", "TENSP");


            ViewBag.QuantitiesDichVu = listHoaDv;
            ViewBag.QuantitiesSanPham = listSp;


            return View(hoadonVM);
        }

        // POST: HoaDon/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HoaDonVM hoadon, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                var hoadonUpdate = db.HOADONs.FirstOrDefault(x => x.IDHOADON == hoadon.IDHOADON);
                if (hoadonUpdate == null)
                {
                    return HttpNotFound();
                }

                hoadonUpdate.TENKH = hoadon.TENKH;
                hoadonUpdate.SOLUONG = hoadon.SOLUONG;
                hoadonUpdate.TONGGIA = hoadon.TONGGIA;
                hoadonUpdate.TENNV = hoadon.TENNV;
                hoadonUpdate.MAKM = hoadon.MAKM;
                hoadonUpdate.SODT = hoadon.SODT;
                hoadonUpdate.IDKHACHHANG = hoadon.IDKHACHHANG;
                hoadonUpdate.IDNHANVIEN = hoadon.IDNHANVIEN;
                hoadonUpdate.NGAY = DateTime.Today;
                hoadonUpdate.GIO = DateTime.Now.TimeOfDay;

                db.Entry(hoadonUpdate).State = EntityState.Modified;

                // Remove old details
                var existingDichVu = db.HOADON_DV.Where(d => d.IDHOADON == hoadon.IDHOADON).ToList();
                var existingSanPham = db.HOADON_SP.Where(p => p.IDHOADON == hoadon.IDHOADON).ToList();

                db.HOADON_DV.RemoveRange(existingDichVu);
                db.HOADON_SP.RemoveRange(existingSanPham);
                db.SaveChanges();

                // Re-add details
                var tongtien = 0;
                var soluong = 0;

                // Calculate total amount and quantity
                var selectedDichVu = form.GetValues("MADV") ?? new string[0];
                var selectedSanPham = form.GetValues("IDSP") ?? new string[0];

                soluong = selectedDichVu.Length + selectedSanPham.Length;

                var km = db.KHUYENMAIs.FirstOrDefault(x => x.MAKM == hoadon.MAKM);
                if (km != null && DateTime.Now >= km.THOIGIANBD && DateTime.Now <= km.THOIGIANKT)
                {
                    // Calculate discount
                    tongtien = (tongtien / 100) * km.GIAMGIA;
                }

                // Process services
                if (selectedDichVu.Length > 0)
                {
                    var hoaDonDvds = new List<HOADON_DV>();
                    foreach (var d in selectedDichVu)
                    {
                        var gia = db.DICHVUs.FirstOrDefault(x => x.MADV == d);
                        if (gia != null)
                        {
                            int sl = int.TryParse(form[$"Quantities[DichVu][{d}]"], out int sldv) ? sldv : 1;
                            var hddv = new HOADON_DV
                            {
                                SOLUONG = sl,
                                MADV = d,
                                THANHTIEN = gia.GIA * int.Parse(form[$"Quantities[DichVu][{d}]"]),
                                GIA = gia.GIA,
                                GIAMGIA = 0,
                                IDHOADON = hoadon.IDHOADON,
                                DICHVU = gia,
                                HOADON = hoadonUpdate
                            };
                            tongtien += hddv.THANHTIEN;
                            hoaDonDvds.Add(hddv);
                        }
                    }
                    db.HOADON_DV.AddRange(hoaDonDvds);
                }

                // Process products
                if (selectedSanPham.Length > 0)
                {
                    var hoaDonsp = new List<HOADON_SP>();
                    foreach (var m in selectedSanPham)
                    {
                        var gia = db.SANPHAMs.FirstOrDefault(x => x.IDSP == m);
                        if (gia != null)
                        {
                            int sl = int.TryParse(form[$"Quantities[SanPham][{m}]"], out int slsp) ? slsp : 1;
                            var hddv = new HOADON_SP
                            {
                                SOLUONG = sl,
                                IDSP = m,
                                TONGTIEN = gia.GIA * int.Parse(form[$"Quantities[SanPham][{m}]"]),
                                GIA = gia.GIA,
                                THUE = 0,
                                IDHOADON = hoadon.IDHOADON,
                                SANPHAM = gia,
                                HOADON = hoadonUpdate
                            };
                            tongtien += hddv.TONGTIEN;
                            hoaDonsp.Add(hddv);
                        }
                    }
                    db.HOADON_SP.AddRange(hoaDonsp);
                }

                // Update the total amount and quantity
                hoadonUpdate.TONGGIA = tongtien;
                hoadonUpdate.SOLUONG = soluong;
                hoadonUpdate.NGAY = DateTime.Now;
                hoadonUpdate.GIO = DateTime.Now.TimeOfDay;

                db.SaveChanges();

                return RedirectToAction("HoaDon", "Admin");
            }

            // Reload dropdown lists and return to the view if validation fails
            ViewBag.IDKHACHHANG = new SelectList(db.KHACHHANGs, "IDKHACHHANG", "TENKH", hoadon.IDKHACHHANG);
            ViewBag.IDNHANVIEN = new SelectList(db.NHANVIENs, "IDNHANVIEN", "TENNV", hoadon.IDNHANVIEN);
            ViewBag.MAKM = new SelectList(db.KHUYENMAIs.Select(km => new
            {
                MAKM = km.MAKM,
                TENCTKM = km.TENCTKM
            }).ToList(), "MAKM", "TENCTKM", hoadon.MAKM);
            ViewBag.dropdownDichvu = new SelectList(db.DICHVUs, "MADV", "TENDV");
            ViewBag.dropdownSanPham = new SelectList(db.SANPHAMs, "IDSP", "TENSP");

            return View(hoadon);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MAKM,TENKH,TENNV,SODT,IDKHACHHANG,IDNHANVIEN,MADV,IDSP,Quantities")] HoaDonVM hoadon, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                // Generate new IDHOADON
                hoadon.IDHOADON = GenerateNewIDHOADON();

                // Fetch TENKH and TENNV
                hoadon.TENKH = db.KHACHHANGs.Where(kh => kh.IDKHACHHANG == hoadon.IDKHACHHANG).Select(kh => kh.TENKH).FirstOrDefault();
                hoadon.TENNV = db.NHANVIENs.Where(nv => nv.IDNHANVIEN == hoadon.IDNHANVIEN).Select(nv => nv.TENNV).FirstOrDefault();

                var tongtien = 0;
                var soluong = 0;

                // Calculate total amount and quantity
                var selectedDichVu = form.GetValues("MADV") ?? new string[0];
                var selectedSanPham = form.GetValues("IDSP") ?? new string[0];

                soluong = selectedDichVu.Length + selectedSanPham.Length;

                var km = db.KHUYENMAIs.FirstOrDefault(x => x.MAKM == hoadon.MAKM);
                if (km != null && DateTime.Now >= km.THOIGIANBD && DateTime.Now <= km.THOIGIANKT)
                {
                    // Calculate discount
                    tongtien = (tongtien / 100) * km.GIAMGIA;
                }

                var hoadonNew = new HOADON
                {
                    IDHOADON = hoadon.IDHOADON,
                    TENKH = hoadon.TENKH,
                    SOLUONG = soluong,
                    TONGGIA = tongtien,
                    TENNV = hoadon.TENNV,
                    MAKM = hoadon.MAKM,
                    NGAY = DateTime.Today,
                    GIO = DateTime.Now.TimeOfDay,
                    SODT = hoadon.SODT,
                    IDKHACHHANG = hoadon.IDKHACHHANG,
                    IDNHANVIEN = hoadon.IDNHANVIEN
                };

                db.HOADONs.Add(hoadonNew);
                db.SaveChanges();

                // Process services
                if (selectedDichVu.Length > 0)
                {
                    var hoaDonDvds = new List<HOADON_DV>();
                    foreach (var d in selectedDichVu)
                    {
                        var gia = db.DICHVUs.FirstOrDefault(x => x.MADV == d);
                        if (gia != null)
                        {

                            int sl = int.TryParse(form[$"Quantities[DichVu][{d}]"], out int sldv) ? sldv : 0;
                            var hddv = new HOADON_DV
                            {
                                SOLUONG = sl,
                                MADV = d,
                                THANHTIEN = gia.GIA * int.Parse(form[$"Quantities[DichVu][{d}]"]),
                                GIA = gia.GIA,
                                GIAMGIA = 0,
                                IDHOADON = hoadon.IDHOADON,
                                DICHVU = gia,
                                HOADON = hoadonNew
                            };
                            tongtien += hddv.THANHTIEN;
                            hoaDonDvds.Add(hddv);
                        }
                    }
                    db.HOADON_DV.AddRange(hoaDonDvds);
                    db.SaveChanges();
                }

                // Process products
                if (selectedSanPham.Length > 0)
                {
                    var hoaDonsp = new List<HOADON_SP>();
                    foreach (var m in selectedSanPham)
                    {
                        var gia = db.SANPHAMs.FirstOrDefault(x => x.IDSP == m);
                        if (gia != null)
                        {
                            int sl = int.TryParse(form[$"Quantities[SanPham][{m}]"], out int slsp) ? slsp : 0;

                            var hddv = new HOADON_SP
                            {
                                SOLUONG = sl,
                                IDSP = m,
                                TONGTIEN = gia.GIA * int.Parse(form[$"Quantities[SanPham][{m}]"]),
                                GIA = gia.GIA,
                                THUE = 0,
                                IDHOADON = hoadon.IDHOADON,
                                SANPHAM = gia,
                                HOADON = hoadonNew
                            };
                            tongtien += hddv.TONGTIEN;
                            hoaDonsp.Add(hddv);
                        }
                    }
                    db.HOADON_SP.AddRange(hoaDonsp);
                    db.SaveChanges();
                }

                // Update the total amount and quantity
                var hoadonUpdate = db.HOADONs.FirstOrDefault(x => x.IDHOADON == hoadon.IDHOADON);
                if (hoadonUpdate != null)
                {
                    hoadonUpdate.TONGGIA = tongtien;
                    hoadonUpdate.SOLUONG = soluong;
                    hoadonUpdate.NGAY = DateTime.Now;
                    hoadonUpdate.GIO = DateTime.Now.TimeOfDay;
                    db.SaveChanges();
                }

                return RedirectToAction("HoaDon", "Admin");
            }

            // Reload dropdown lists and return to the view if validation fails
            ViewBag.IDKHACHHANG = new SelectList(db.KHACHHANGs, "IDKHACHHANG", "TENKH");
            ViewBag.IDNHANVIEN = new SelectList(db.NHANVIENs, "IDNHANVIEN", "TENNV");
            ViewBag.MAKM = new SelectList(db.KHUYENMAIs.Select(km => new
            {
                MAKM = km.MAKM,
                TENCTKM = km.TENCTKM
            }).ToList(), "MAKM", "TENCTKM");
            ViewBag.dropdownDichvu = new SelectList(db.DICHVUs, "MADV", "TENDV");
            ViewBag.dropdownSanPham = new SelectList(db.SANPHAMs, "IDSP", "TENSP");

            return View(hoadon);
        }


        private string GenerateNewIDHOADON()
        {
            using (var db = new KHOCTHATNHIEUDB1())
            {
                var maxID = db.HOADONs.Max(hd => hd.IDHOADON);
                int newId = 1;
                if (!string.IsNullOrEmpty(maxID))
                {
                    newId = int.Parse(maxID.Substring(2)) + 1;
                }
                return "HD" + newId.ToString("D3");
            }
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
