using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoAnPhanMem.Dtos
{
    public class HoaDonDto
    {
        public string IDHOADON { get; set; }
        public int TONGGIA { get; set; }
        public int SOLUONG { get; set; }
        public DateTime NGAY { get; set; } // Ngày tạo hóa đơn
        public TimeSpan GIO { get; set; } // Giờ tạo hóa đơn
        public string MAKM { get; set; }
        public string TENKH { get; set; }
        public string TENNV { get; set; }
        public string SODT { get; set; }
    }
}