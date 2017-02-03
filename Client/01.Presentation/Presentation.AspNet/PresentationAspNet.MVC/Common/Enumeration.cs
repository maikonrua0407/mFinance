using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PresentationAspNet.MVC
{
    public static class Enumeration
    {
        public enum HinhThucNhapHang
        {
            NhapTrucTiep = 1,
            NhapTuDeXuat = 4
        }
        public enum LoaiKhuyenMai
        {
            TatCa = 0,
            KhuyenMaiCombo = 1,
            KhuyenMaiTongTien = 2,
            //KhuyenMaiNganhHang = 3,
            //KhuyenMaiThuongHieu = 4,
            KhuyenMaiSanPham = 5,
        }

        public enum DuyetKhuyenMai
        {
            TatCa = 0,
            ChuaDuyet = 1,
            DuyetLanCuoi = 2,
            DaHuy = 3,
            TamDung = 4,
            DuyetLan1 = 5,
            TuChoi = 6
        }

        public enum TrangThaiKhuyenMai
        {
            DangHoatDong = 0,
            DaKetThuc = 1,
        }

        public enum LoaiKhuyenMaiCombo
        {
            KhuyenMaiSanPham = 0,
            KhuyenMaiDichDanh = 1,
        }

        public enum LoaiThoiGian
        {
            Ngay = 0,
            Thang = 1,
            Nam = 2
        }

        public enum HinhThucXuat
        {
            TatCa = 0,
            BanHangOnline = 2,
            BanLe = 3,
            XuatBanBuon = 4,
            XuatChuyenKho = 5,
            XuatTraBaoHanh = 6,
            XuatTraNhaCungCap = 7
        }

        public enum HinhThucNhap
        {
            TatCa = 5,
            DeXuatNhap = 0,
            NhapTuDeXuat = 4,
            NhapMuaHang = 1,
            NhapTraLai = 2,
            NhapBaoHanh = 3,
        }
    }
}