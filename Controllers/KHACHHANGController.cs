using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DoAnPhanMem.Models;

namespace DoAnPhanMem.Controllers
{
    public class KHACHHANGController : Controller
    {
        private KHOCTHATNHIEUDB1 db = new KHOCTHATNHIEUDB1();

        
        public ActionResult Index()
        {
            var khachhangs = db.KHACHHANGs.Include(k => k.TAIKHOAN);
            return View(khachhangs.ToList());
        }

        
        public ActionResult Create()
        {
            ViewBag.TENTK = new SelectList(db.TAIKHOANs, "TENTK", "TENTK");
            return View();
        }




        [HttpGet]
        public ActionResult getProfile(string TenTK)
        {
            var khachHang = db.KHACHHANGs.Where(x => x.TENTK == TenTK).FirstOrDefault();
            if (khachHang == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(khachHang);
        }

        public PartialViewResult dsLichSu(string idKh)
        {
            var data = db.LICHHENs.AsQueryable().OrderBy(x => x.NGAY).Where(x => x.TRANGTHAI == true && x.IDKHACHHANG == idKh).Take(5).ToList();
            return PartialView("dsLichSu", data);
        }


        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDKHACHHANG,TENKH,GIOITINH,DIACHI,NGAYSINH,SODT,TENTK,CHITIEU,HANG")] KHACHHANG khachhang)
        {
            if (ModelState.IsValid)
            {
                khachhang.IDKHACHHANG = GenerateNewIDKHACHHANG();

                db.KHACHHANGs.Add(khachhang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TENTK = new SelectList(db.TAIKHOANs, "TENTK", "TENTK", khachhang.TENTK);
            return View(khachhang);
        }

        
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHACHHANG khachhang = db.KHACHHANGs.Find(id);
            if (khachhang == null)
            {
                return HttpNotFound();
            }
            ViewBag.TENTK = new SelectList(db.TAIKHOANs, "TENTK", "TENTK", khachhang.TENTK);
            return View(khachhang);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDKHACHHANG,TENKH,GIOITINH,DIACHI,NGAYSINH,SODT,TENTK,CHITIEU,HANG")] KHACHHANG khachhang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(khachhang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TENTK = new SelectList(db.TAIKHOANs, "TENTK", "TENTK", khachhang.TENTK);
            return View(khachhang);
        }

        
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHACHHANG khachhang = db.KHACHHANGs.Find(id);
            if (khachhang == null)
            {
                return HttpNotFound();
            }
            return View(khachhang);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(string id)
        {
            var result = new { success = false, message = "Xóa không thành công." };

            if (!string.IsNullOrEmpty(id))
            {
                var khachhang = db.KHACHHANGs.Find(id);

                if (khachhang != null)
                {
                    try
                    {
                        db.KHACHHANGs.Remove(khachhang);
                        db.SaveChanges();
                        result = new { success = true, message = "Xóa thành công." };
                    }
                    catch (Exception ex)
                    {
                        // Log the exception message if needed (ex.Message)
                        result = new { success = false, message = "Xóa không thành công do lỗi hệ thống." };
                    }
                }
                else
                {
                    result = new { success = false, message = "Khách hàng không tồn tại." };
                }
            }
            else
            {
                result = new { success = false, message = "ID không hợp lệ." };
            }

            return Json(result);
        }

        private string GenerateNewIDKHACHHANG()
        {
            var allIDs = db.KHACHHANGs.Select(kh => kh.IDKHACHHANG).ToList();
            int maxId = 0;

            foreach (var id in allIDs)
            {
                if (!string.IsNullOrEmpty(id) && id.StartsWith("KH"))
                {
                    string numericPart = id.Substring(2); // Skip "KH"
                    if (int.TryParse(numericPart, out int parsedId))
                    {
                        if (parsedId > maxId)
                        {
                            maxId = parsedId;
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Invalid ID format: {id}");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Invalid ID format: {id}");
                }
            }

            int newId = maxId + 1;
            return "KH" + newId.ToString("D3"); // "D3" for padding with zeroes to ensure three digits
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
