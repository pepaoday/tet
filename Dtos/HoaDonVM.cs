using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoAnPhanMem.Dtos
{
    public class HoaDonVM
    {
        public string IDHOADON { get; set; }
        public int TONGGIA { get; set; }
        public int SOLUONG { get; set; }
        public string MAKM { get; set; }
        public string TENKH { get; set; }
        public string TENNV { get; set; }
        public string SODT { get; set; }
        public string IDNHANVIEN { get; set; }
        public string IDKHACHHANG { get; set; }
        [Required(ErrorMessage = "Yêu cầu chọn dịch vụ")]
        public List<string> MADV { get; set; }
        [Required(ErrorMessage = "Yêu cầu chọn sản phẩm")]
        public List<string> IDSP { get; set; }



    }
}