namespace DoAnPhanMem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations; // Thêm namespace này nếu chưa có

    public partial class TAIKHOAN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TAIKHOAN()
        {
            this.KHACHHANGs = new HashSet<KHACHHANG>();
            this.NHANVIENs = new HashSet<NHANVIEN>();
        }

        [Required(ErrorMessage = "Tên tài khoản không được để trống")]
        public string TENTK { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [DataType(DataType.Password)]
        public string PASS { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string EMAIL { get; set; }

        public string LOAITK { get; set; }

        [Required(ErrorMessage = "Xác nhận mật khẩu không được để trống")]
        [DataType(DataType.Password)]
        [Compare("PASS", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không khớp")]
        public string ConfirmPassword { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KHACHHANG> KHACHHANGs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NHANVIEN> NHANVIENs { get; set; }
    }
}
