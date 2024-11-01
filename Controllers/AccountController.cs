using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using DoAnPhanMem.Models;

namespace DoAnPhanMem.Controllers
{
    public class AccountController : Controller
    {
        private KHOCTHATNHIEUDB1 db = new KHOCTHATNHIEUDB1();
        // GET: Account
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(TAIKHOAN account)
        {
            var taikhoan = db.TAIKHOANs.FirstOrDefault(k => k.TENTK == account.TENTK && k.PASS == account.PASS);
            if (taikhoan != null)
            {
                Session["TenTK"] = taikhoan.TENTK;
                Session["User"] = taikhoan;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["dangnhap"] = "Tài khoản hoặc mật khẩu không chính xác";
                return View();
            }
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(TAIKHOAN account)
        {
            if (ModelState.IsValid)
            {
                var register = db.TAIKHOANs.Where(s => s.TENTK == account.TENTK).FirstOrDefault();
                if (register == null)
                {
                    account.LOAITK = "KH";
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.TAIKHOANs.Add(account);
                    db.SaveChanges();
                    //thêm mới 1 khách hàng
                    var newKhachHang = new KHACHHANG()
                    {
                        TENTK = account.TENTK,
                        TENKH = account.TENTK,
                        TAIKHOAN = account,
                        HANG = "Đồng",
                        IDKHACHHANG = "KH" + account.TENTK,
                        CHITIEU = 0,
                        SODT = "",
                        NGAYSINH = DateTime.Now,
                        GIOITINH = null
                     
                    };
                    db.KHACHHANGs.Add(newKhachHang);
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Đăng ký thành công!";
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("TENTK", "Tài khoản đã tồn tại trong hệ thống");
                    return View();
                }
            }
            return View();
        }


        public ActionResult Resetpass()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }



    }
}