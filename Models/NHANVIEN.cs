//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DoAnPhanMem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class NHANVIEN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NHANVIEN()
        {
            this.DANHGIA_NV = new HashSet<DANHGIA_NV>();
            this.HOADONs = new HashSet<HOADON>();
            this.NHANVIEN_DV = new HashSet<NHANVIEN_DV>();
        }
    
        public string IDNHANVIEN { get; set; }
        public string TENNV { get; set; }
        public Nullable<bool> GIOITINH { get; set; }
        public string DIACHI { get; set; }
        public double TONGDANHGIA { get; set; }
        public Nullable<System.DateTime> NGAYSINH { get; set; }
        public string SODT { get; set; }
        public string CHUCVU { get; set; }
        public string TENTK { get; set; }
        public string NGHIEPVU { get; set; }
        public string IMG { get; set; }
        public string MOTA { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DANHGIA_NV> DANHGIA_NV { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HOADON> HOADONs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NHANVIEN_DV> NHANVIEN_DV { get; set; }
        public virtual TAIKHOAN TAIKHOAN { get; set; }
    }
}