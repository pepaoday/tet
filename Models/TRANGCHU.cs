using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnPhanMem.Models
{
    public class TRANGCHU
    {
        public List<NHANVIEN> NHANVIEN { get; set; }
        public List<DICHVU> DICHVU { get; set; }
        public List<DANHMUCDV> DANHMUCDV { get; set; }
    }
}