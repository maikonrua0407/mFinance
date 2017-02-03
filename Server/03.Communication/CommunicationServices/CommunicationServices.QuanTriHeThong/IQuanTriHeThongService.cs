using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DataModel.EntityFramework;
using CommunicationServices.QuanTriHeThong.Messages;
using CommunicationContracts.Base.ContractBases;

namespace CommunicationServices.QuanTriHeThong
{
    [ServiceContract]
    public interface IQuanTriHeThongService : IServiceBase
    {
        [OperationContract]
        string returnHello();
        
        [OperationContract]
        HT_CNANG LayChucNangTheoMa(string maChucNang);

        [OperationContract]
        List<HT_CNANG> LayChucNangTheoMaCha(string maChucNangCha);

        [OperationContract]
        List<HT_CNANG> LayChucNangTheoQuyen(string maDangNhap, string passWord);

        /// <summary>
        /// Kiểm tra các ngày trong tháng có phải là ngày làm việc
        /// </summary>
        /// <param name="year">Năm cần lấy danh sách</param>
        /// <param name="month">Tháng cần lấy danh sách</param>
        /// <returns>Trả lại danh sách theo từng ngày trong tháng chứa giá trị true là ngày làm việc, false là ngày nghỉ</returns>
        [OperationContract]
        LayDSNgayLamViecResponse LayDSNgayLamViec(LayDSNgayLamViecRequest layDSNgayLamViecRequest);

        /// <summary>
        /// Cập nhật toàn bộ bảng HT_LICH
        /// </summary>
        /// <param name="value">Danh sách các object dữ liệu lịch</param>
        [OperationContract]
        LuuLichResponse LuuLich(LuuLichRequest luuLichRequest);

        [OperationContract]
        PhanQuyenResponse layCNangTNang(PhanQuyenRequest phanQuyenRequest);

        [OperationContract]
        PhanQuyenResponse layCNangTNangTheoListIdChucNang(PhanQuyenRequest phanQuyenRequest);

        [OperationContract]
        PhanQuyenResponse layCNangTNangTheoListIdMenu(PhanQuyenRequest phanQuyenRequest);

        [OperationContract]
        PhanQuyenResponse layTNangDuocPhanQuyen(PhanQuyenRequest phanQuyenRequest);

        [OperationContract]
        PhanQuyenResponse layCNang(PhanQuyenRequest phanQuyenRequest);

        [OperationContract]
        PhanQuyenResponse layCNangTheoPhanHe(PhanQuyenRequest phanQuyenRequest);

        [OperationContract]
        PhanQuyenResponse layCNangThietLapAPTheoPhanHe(PhanQuyenRequest phanQuyenRequest);

        [OperationContract]
        PhanQuyenResponse layCNangPQuyen(PhanQuyenRequest request);

        [OperationContract]
        PhanQuyenResponse layCNangPQuyenTheoDoiTuong(PhanQuyenRequest request);
        [OperationContract]

        PhanQuyenResponse layCNangPQuyenTheoDoiTuongChucNang(PhanQuyenRequest request);

        [OperationContract]
        PhanQuyenResponse layTNang(PhanQuyenRequest phanQuyenRequest);

        [OperationContract]
        PhanQuyenResponse layTNangTheoListIdTinhNang(PhanQuyenRequest phanQuyenRequest);

        [OperationContract]
        PhanQuyenResponse layTNguyen(PhanQuyenRequest phanQuyenRequest);

        [OperationContract]
        PhanQuyenResponse layDSTNguyenKThac(PhanQuyenRequest phanQuyenRequest);

        [OperationContract]
        PhanQuyenResponse luuPhanQuyen(PhanQuyenRequest phanQuyenRequest);

        [OperationContract]
        PhanQuyenResponse luuPhanQuyenChucNang(PhanQuyenRequest phanQuyenRequest);

        [OperationContract]
        NguoiDungResponse layDanhSachDonVi(NguoiDungRequest nguoiSuDungRequest);

        [OperationContract]
        NguoiDungResponse layThongTinCaNhan(NguoiDungRequest nguoiSuDungRequest);

        [OperationContract]
        NguoiDungResponse layNSD(NguoiDungRequest nguoiSuDungRequest);

        [OperationContract]
        NguoiDungResponse layNhomNSD(NguoiDungRequest nguoiSuDungRequest);

        [OperationContract]
        NguoiDungResponse layNSDTheoNhom(NguoiDungRequest nguoiSuDungRequest);

        [OperationContract]
        NguoiDungResponse layNhomTheoNSD(NguoiDungRequest nguoiSuDungRequest);

        [OperationContract]
        NguoiDungResponse layTruyCapTheoNSD(NguoiDungRequest nguoiSuDungRequest);

        [OperationContract]
        NguoiDungResponse layTruyCapTheoNhomNSD(NguoiDungRequest nguoiSuDungRequest);

        [OperationContract]
        NguoiDungResponse layPhamViPhongGDTheoNSDVaMaLoaiPhamVi(NguoiDungRequest nguoiSuDungRequest);

        [OperationContract]
        NguoiDungResponse ThemNSD(NguoiDungRequest request);

        [OperationContract]
        NguoiDungResponse SuaNSD(NguoiDungRequest request);

        [OperationContract]
        NguoiDungResponse XoaListNSDTheoID(NguoiDungRequest request);

        [OperationContract]
        NguoiDungResponse ThemNHNSD(NguoiDungRequest request);

        [OperationContract]
        NguoiDungResponse SuaNHNSD(NguoiDungRequest request);

        [OperationContract]
        NguoiDungResponse XoaListNHNSDTheoID(NguoiDungRequest request);

        [OperationContract]
        ThamSoResponse layThamSo(ThamSoRequest thamSoRequest);

        [OperationContract]
        ThamSoResponse layThamSoHeThong(ThamSoRequest thamSoRequest);

        [OperationContract]
        ThamSoResponse layLoaiThamSo(ThamSoRequest thamSoRequest);

        [OperationContract]
        ThamSoResponse capNhatThamSo(ThamSoRequest thamSoRequest);

        [OperationContract]
        ThamSoResponse capNhatGiaTriThamSo(ThamSoRequest thamSoRequest);

        [OperationContract]
        ThamSoResponse capNhatLoaiThamSo(ThamSoRequest thamSoRequest);

        [OperationContract]
        NguoiDungResponse DoiMatKhauNguoiDung(NguoiDungRequest thamSoRequest);

        [OperationContract]
        NguoiDungResponse DoiMatKhauNguoiDungLucDangNhap(NguoiDungRequest thamSoRequest);

        [OperationContract]
        NguoiDungResponse ThietLapMatKhauNguoiDung(NguoiDungRequest thamSoRequest);

        [OperationContract]
        PhienBanResponse CheckClientVersion(PhienBanRequest request);

        [OperationContract]
        PhienBanResponse DownloadClientVersion(PhienBanRequest request);

        [OperationContract]
        PhienBanResponse DownloadClientVersionItem(PhienBanRequest request);

        [OperationContract]
        NguoiDungResponse LayDanhSachPhongGD(NguoiDungRequest request);

        [OperationContract]
        PhamViResponse PhanQuyenPhamVi(PhamViRequest request);

        [OperationContract]
        ChucNangResponse ChucNang(ChucNangRequest request);
    }
}
