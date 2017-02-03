using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PresentationWPF.KhachHang
{
    public class LoaiHinhAnh
    {
        public string MaLoai { get; set; }
        public string TenLoai { get; set; }
        public string LangKey { get; set; }
    }

    public class DoiTuongHinhAnh
    {
        public string MaDoiTuong { get; set; }
        public string TenDoiTuong { get; set; }
        public string MaLoai { get; set; }
        public string LangKey { get; set; }
    }

    public class DuLieuHinhAnh
    {
        public int ID { get; set; }
        public int STT { get; set; }
        public string MaLoai { get; set; }
        public string TenLoai { get; set; }
        public string MaDoiTuong { get; set; }
        public string TenDoiTuong { get; set; }
        public string MaHinhAnh { get; set; }
        public string TenHinhAnh { get; set; }
        public byte[] Data { get; set; }
        public byte[] Avatar { get; set; }
        public bool HieuLuc { get; set; }
        public bool HienThiHS { get; set; }
        public bool CHON { get; set; }
        public string ImageName { get; set; }
        public string ImageFormat { get; set; }
        public string LangKey { get; set; }
        public string TrangThai { get; set; }
        public string TenTrangThai { get; set; }
        public string NgayHieuLuc { get; set; }
        public string NgayDuLieu { get; set; }
    }
}
