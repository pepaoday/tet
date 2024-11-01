using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoAnPhanMem.Dtos
{
    public class LichHenDto
    {
        public string MADL { get; set; }
        public DateTime? NGAY { get; set; }
        public TimeSpan? THOIGIAN { get; set; }
        public bool TRANGTHAI { get; set; }
        public string GHICHU { get; set; }
        public string TENKH { get; set; }
        public string EMAIL { get; set; }
        public string SODTKH { get; set; }
        public string IDKHACHHANG { get; set; }
    }
}