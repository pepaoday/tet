using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnPhanMem.Dtos
{
    public class Result
    {
        public dynamic data { get;set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int totalPages { get; set; }

    }
}