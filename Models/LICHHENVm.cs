using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoAnPhanMem.Models
{
    public class LICHHENVm
    {
        public string MADL { get; set; }
        public Nullable<System.DateTime> NGAY { get; set; }
        public Nullable<System.TimeSpan> THOIGIAN { get; set; }
        public bool TRANGTHAI { get; set; }
        public string GHICHU { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên của bạn")]
        public string TENKH { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string EMAIL { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string SODTKH { get; set; }
        public string IDKHACHHANG { get; set; }
        public virtual KHACHHANG KHACHHANG { get; set; }
    }
}