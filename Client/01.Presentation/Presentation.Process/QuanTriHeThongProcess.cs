using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Utilities;
using Utilities.Common;
using Presentation.Process.Common;
using Presentation.Process.QuanTriHeThongServiceRef;
using System.ServiceModel.Description;
using System.Collections;

namespace Presentation.Process
{
    public class QuanTriHeThongProcess
    {
        private static QuanTriHeThongServiceClient Client { get; set; }

        static QuanTriHeThongProcess()
        {
            //Client = new QuanTriHeThongServiceClient();
            EndpointAddress endpointAddress = Common.Utilities.getEndpointAddress(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());
            BasicHttpBinding basicHttpBinding = Common.Utilities.getBasicHttpBinding(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());
            Client = new QuanTriHeThongServiceClient(basicHttpBinding, endpointAddress);

            foreach (var operationDescription in Client.Endpoint.Contract.Operations)
            {
                DataContractSerializerOperationBehavior dcsob =
                    operationDescription.Behaviors.Find<DataContractSerializerOperationBehavior>();
                if (dcsob != null)
                {
                    dcsob.MaxItemsInObjectGraph = 2147483646;
                }
            }
        }

        /// <summary>
        /// Kiểm tra các ngày trong tháng có phải là ngày làm việc
        /// </summary>
        /// <param name="year">Năm cần kiểm tra</param>
        /// <param name="month">Tháng cần kiểm tra</param>
        /// <returns>Trả lại danh sách theo từng ngày trong tháng chứa giá trị true là ngày làm việc, false là ngày nghỉ</returns>
        public static List<bool> IsWorkingDays(int year, int month)
        {
            var request = Presentation.Process.Common.Utilities.PrepareRequest(new LayDSNgayLamViecRequest());
            request.Year = year;
            request.Month = month;
            var response = Client.LayDSNgayLamViec(request);
            if (response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                throw new CustomException(response.ResponseMessage, response.ExceptionObject.Deserialize());
            return response.ListNgayLamViec.ToList();
        }

        public List<HT_CNANG_TNANG> layCNangTNang()
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            PhanQuyenRequest request = Common.Utilities.PrepareRequest(new PhanQuyenRequest());

            // Lấy kết quả trả về
            PhanQuyenResponse response = Client.layCNangTNang(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            return response.ListChucNangTinhNang.ToList();
        }

        public List<HT_CNANG_TNANG> layCNangTNangTheoListIdChucNang(List<int> lstIdChucNang)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            PhanQuyenRequest request = Common.Utilities.PrepareRequest(new PhanQuyenRequest());
            request.Module = DatabaseConstant.Module.QTHT;
            request.Function = DatabaseConstant.Function.HT_PQ_CHUC_NANG;
            request.Action = DatabaseConstant.Action.TRUY_VAN;

            request.lstIdChucNang = lstIdChucNang.ToArray();

            // Lấy kết quả trả về
            PhanQuyenResponse response = Client.layCNangTNangTheoListIdChucNang(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            return response.ListChucNangTinhNang.ToList();
        }

        public List<HT_CNANG_TNANG> layCNangTNangTheoListIdMenu(List<int> lstIdMenu)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            PhanQuyenRequest request = Common.Utilities.PrepareRequest(new PhanQuyenRequest());
            request.Module = DatabaseConstant.Module.QTHT;
            request.Function = DatabaseConstant.Function.HT_PQ_CHUC_NANG;
            request.Action = DatabaseConstant.Action.TRUY_VAN;

            request.lstIdMenu = lstIdMenu.ToArray();

            // Lấy kết quả trả về
            PhanQuyenResponse response = Client.layCNangTNangTheoListIdMenu(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            return response.ListChucNangTinhNang.ToList();
        }

        public List<HT_CNANG_PQUYEN> layCNangPQuyen()
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            PhanQuyenRequest request = Common.Utilities.PrepareRequest(new PhanQuyenRequest());

            // Lấy kết quả trả về
            PhanQuyenResponse response = Client.layCNangPQuyen(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            return response.ListChucNangPhanQuyen.ToList();
        }

        public List<HT_CNANG_PQUYEN> layCNangPQuyenTheoDoiTuong(string maDoiTuong, string loaiDoiTuong)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            PhanQuyenRequest request = Common.Utilities.PrepareRequest(new PhanQuyenRequest());
            request.Module = DatabaseConstant.Module.QTHT;
            request.Function = DatabaseConstant.Function.HT_PQ_CHUC_NANG;
            request.Action = DatabaseConstant.Action.TRUY_VAN;

            request.maDoiTuong = maDoiTuong;
            request.loaiDoiTuong = loaiDoiTuong;

            // Lấy kết quả trả về
            PhanQuyenResponse response = Client.layCNangPQuyenTheoDoiTuong(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            return response.ListChucNangPhanQuyen.ToList();
        }

        public List<HT_CNANG_PQUYEN> layCNangPQuyenTheoDoiTuongChucNang(string maDoiTuong, string loaiDoiTuong, List<int> lstIdChucNang)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            PhanQuyenRequest request = Common.Utilities.PrepareRequest(new PhanQuyenRequest());
            request.Module = DatabaseConstant.Module.QTHT;
            request.Function = DatabaseConstant.Function.HT_PQ_CHUC_NANG;
            request.Action = DatabaseConstant.Action.TRUY_VAN;

            request.maDoiTuong = maDoiTuong;
            request.loaiDoiTuong = loaiDoiTuong;
            request.lstIdChucNang = lstIdChucNang.ToArray();

            // Lấy kết quả trả về
            PhanQuyenResponse response = Client.layCNangPQuyenTheoDoiTuongChucNang(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            return response.ListChucNangPhanQuyen.ToList();
        }

        public List<HT_CNANG> layCNang()
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            PhanQuyenRequest request = Common.Utilities.PrepareRequest(new PhanQuyenRequest());

            // Lấy kết quả trả về
            PhanQuyenResponse response = Client.layCNang(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            return response.ListChucNang.ToList();
        }

        public List<HT_CNANG> layCNangTheoPhanHe(string maPhanHe)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            PhanQuyenRequest request = Common.Utilities.PrepareRequest(new PhanQuyenRequest());
            request.Module = DatabaseConstant.Module.QTHT;
            request.Function = DatabaseConstant.Function.HT_PQ_CHUC_NANG;
            request.Action = DatabaseConstant.Action.TRUY_VAN;

            request.maPhanHe = maPhanHe;

            // Lấy kết quả trả về
            PhanQuyenResponse response = Client.layCNangTheoPhanHe(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            return response.ListChucNang.ToList();
        }

        public List<HT_CNANG> layCNangThietLapAPTheoPhanHe(string maPhanHe)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            PhanQuyenRequest request = Common.Utilities.PrepareRequest(new PhanQuyenRequest());
            request.Module = DatabaseConstant.Module.QTHT;
            request.Function = DatabaseConstant.Function.HT_PQ_CHUC_NANG;
            request.Action = DatabaseConstant.Action.TRUY_VAN;

            request.maPhanHe = maPhanHe;

            // Lấy kết quả trả về
            PhanQuyenResponse response = Client.layCNangThietLapAPTheoPhanHe(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            return response.ListChucNang.ToList();
        }

        public List<HT_TNANG> layTNang()
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            PhanQuyenRequest request = Common.Utilities.PrepareRequest(new PhanQuyenRequest());

            // Lấy kết quả trả về
            PhanQuyenResponse response = Client.layTNang(request);

            // Lấy kết quả trả về
            return response.ListTinhNang.ToList();
        }

        public List<HT_TNANG> layTNangTheoListIdTinhNang(List<int> lstIdTinhNang)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            PhanQuyenRequest request = Common.Utilities.PrepareRequest(new PhanQuyenRequest());
            request.Module = DatabaseConstant.Module.QTHT;
            request.Function = DatabaseConstant.Function.HT_PQ_CHUC_NANG;
            request.Action = DatabaseConstant.Action.TRUY_VAN;

            request.lstIdTinhNang = lstIdTinhNang.ToArray();

            // Lấy kết quả trả về
            PhanQuyenResponse response = Client.layTNangTheoListIdTinhNang(request);

            // Lấy kết quả trả về
            return response.ListTinhNang.ToList();
        }

        public List<HT_TNANG> layTNangDuocPhanQuyen()
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            PhanQuyenRequest request = Common.Utilities.PrepareRequest(new PhanQuyenRequest());
            request.Module = DatabaseConstant.Module.QTHT;
            request.Function = DatabaseConstant.Function.HT_PQ_CHUC_NANG;
            request.Action = DatabaseConstant.Action.TRUY_VAN;

            // Lấy kết quả trả về
            PhanQuyenResponse response = Client.layTNangDuocPhanQuyen(request);

            // Lấy kết quả trả về
            return response.ListTinhNang.ToList();
        }

        public List<HT_TNGUYEN> layTNguyen(string maTNguyen = null, string loaiTNguyen = null)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            PhanQuyenRequest request = Common.Utilities.PrepareRequest(new PhanQuyenRequest());
            request.maTaiNguyen = maTNguyen;
            request.loaiTaiNguyen = loaiTNguyen;
            // Lấy kết quả trả về
            PhanQuyenResponse response = Client.layTNguyen(request);

            // Lấy kết quả trả về
            return response.ListTaiNguyen.ToList();
        }

        public List<HT_TNGUYEN_KTHAC> layDSTNguyenKThac(string maDoiTuong, string loaiDoiTuong, string loaiTNguyen=null)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            PhanQuyenRequest request = Common.Utilities.PrepareRequest(new PhanQuyenRequest());
            request.maDoiTuong = maDoiTuong;
            request.loaiDoiTuong = loaiDoiTuong;
            request.loaiTaiNguyen = loaiTNguyen;
            // Lấy kết quả trả về
            PhanQuyenResponse response = Client.layDSTNguyenKThac(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            return response.ListTaiNguyenKhaiThac.ToList();
        }

        public bool luuPhanQuyen(string maDoiTuong, string loaiDoiTuong, ArrayList lstPhanQuyen, string nguoiCapNhat)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            PhanQuyenRequest request = Common.Utilities.PrepareRequest(new PhanQuyenRequest());
            request.maDoiTuong = maDoiTuong;
            request.loaiDoiTuong = loaiDoiTuong;
            List<DSChucNangDto> lst = new List<DSChucNangDto>();
            foreach (List<string> item in lstPhanQuyen)
            {
                DSChucNangDto cnangDto = new DSChucNangDto();
                cnangDto.lstChucNang = item.ToArray();
                lst.Add(cnangDto);
            }
            request.lstPhanQuyen = lst.ToArray();
            request.nguoiCapNhat = nguoiCapNhat;
            // Lấy kết quả trả về
            PhanQuyenResponse response = Client.luuPhanQuyen(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            return response.bKetQua;
        }

        public bool luuPhanQuyenChucNang(
            string loaiDoiTuong,
            int idDoiTuong,
            string maDoiTuong,
            string maDonVi,
            List<HT_CNANG_PQUYEN> dsCNangPQuyenXoa,
            List<HT_CNANG_TNANG> dsCNangTNangThem)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            PhanQuyenRequest request = Common.Utilities.PrepareRequest(new PhanQuyenRequest());
            request.Module = DatabaseConstant.Module.QTHT;
            request.Function = DatabaseConstant.Function.HT_PQ_CHUC_NANG;
            request.Action = DatabaseConstant.Action.LUU;

            request.loaiDoiTuong = loaiDoiTuong;
            request.idDoiTuong = idDoiTuong;
            request.maDoiTuong = maDoiTuong;
            request.maDonVi = maDonVi;
            request.dsCNangPQuyenXoa = dsCNangPQuyenXoa.ToArray();
            request.dsCNangTNangThem = dsCNangTNangThem.ToArray();

            // Lấy kết quả trả về
            PhanQuyenResponse response = Client.luuPhanQuyenChucNang(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            return response.bKetQua;
        }

        public List<DM_DON_VI> layDanhSachDonVi()
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            NguoiDungRequest request = Common.Utilities.PrepareRequest(new NguoiDungRequest());
            request.Module = DatabaseConstant.Module.QTHT;
            request.Function = DatabaseConstant.Function.HT_NHNSD;
            request.Action = DatabaseConstant.Action.TRUY_VAN;

            // Lấy kết quả trả về
            NguoiDungResponse response = Client.layDanhSachDonVi(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            return response.ListDonVi.ToList();
        }

        public HT_NSD layThongTinCaNhan()
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            NguoiDungRequest request = Common.Utilities.PrepareRequest(new NguoiDungRequest());
            request.Module = DatabaseConstant.Module.QTHT;
            request.Function = DatabaseConstant.Function.HT_NSD;
            request.Action = DatabaseConstant.Action.TRUY_VAN;

            // Lấy kết quả trả về
            NguoiDungResponse response = Client.layThongTinCaNhan(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            return response.NSD;
        }

        public List<HT_NSD> layNSD(string loaiNguoiSuDung)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            NguoiDungRequest request = Common.Utilities.PrepareRequest(new NguoiDungRequest());
            request.Module = DatabaseConstant.Module.QTHT;
            request.Function = DatabaseConstant.Function.HT_NSD;
            request.Action = DatabaseConstant.Action.TRUY_VAN;

            request.UserType = BusinessConstant.layLoaiNguoiSuDung(loaiNguoiSuDung);

            // Lấy kết quả trả về
            NguoiDungResponse response = Client.layNSD(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            return response.ListNSD.ToList();
        }

        public List<HT_NSD> layAllNSD()
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            NguoiDungRequest request = Common.Utilities.PrepareRequest(new NguoiDungRequest());
            request.Module = DatabaseConstant.Module.QTHT;
            request.Function = DatabaseConstant.Function.HT_NSD;
            request.Action = DatabaseConstant.Action.TRUY_VAN;

            request.UserType = BusinessConstant.layLoaiNguoiSuDung(BusinessConstant.LoaiNguoiSuDung.CAP_SA.layGiaTri());

            // Lấy kết quả trả về
            NguoiDungResponse response = Client.layNSD(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            return response.ListNSD.ToList();
        }

        public List<HT_NHNSD> layNhomNSD(string loaiNguoiSuDung)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            NguoiDungRequest request = Common.Utilities.PrepareRequest(new NguoiDungRequest());
            request.Module = DatabaseConstant.Module.QTHT;
            request.Function = DatabaseConstant.Function.HT_NHNSD;
            request.Action = DatabaseConstant.Action.TRUY_VAN;

            request.UserType = BusinessConstant.layLoaiNguoiSuDung(loaiNguoiSuDung);

            // Lấy kết quả trả về
            NguoiDungResponse response = Client.layNhomNSD(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            return response.ListNHNSD.ToList();
        }

        public List<HT_NSD> layNSDTheoNhom(HT_NHNSD objNHNSD)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            NguoiDungRequest request = Common.Utilities.PrepareRequest(new NguoiDungRequest());
            request.objNHNSD = objNHNSD;
            // Lấy kết quả trả về
            NguoiDungResponse response = Client.layNSDTheoNhom(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            return response.ListNSD.ToList();
        }

        public List<HT_NHNSD> layNhomTheoNSD(HT_NSD objNSD)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            NguoiDungRequest request = Common.Utilities.PrepareRequest(new NguoiDungRequest());
            request.objNSD = objNSD;
            // Lấy kết quả trả về
            NguoiDungResponse response = Client.layNhomTheoNSD(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            return response.ListNHNSD.ToList();
        }

        public List<HT_TRUY_CAP> layTruyCapTheoNSD(HT_NSD objNSD)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            NguoiDungRequest request = Common.Utilities.PrepareRequest(new NguoiDungRequest());
            request.objNSD = objNSD;
            // Lấy kết quả trả về
            NguoiDungResponse response = Client.layTruyCapTheoNSD(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            return response.lstTruyCap.ToList();
        }

        public List<HT_TRUY_CAP> layTruyCapTheoNhomNSD(HT_NHNSD objNHNSD)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            NguoiDungRequest request = Common.Utilities.PrepareRequest(new NguoiDungRequest());
            request.objNHNSD = objNHNSD;
            // Lấy kết quả trả về
            NguoiDungResponse response = Client.layTruyCapTheoNhomNSD(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);            

            // Lấy kết quả trả về
            return response.lstTruyCap.ToList();
        }

        public List<PhamViDto> layPhamViPhongGDTheoNSD(HT_NSD objNSD, string maLoaiPhamVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            NguoiDungRequest request = Common.Utilities.PrepareRequest(new NguoiDungRequest());
            request.objNSD = objNSD;
            request.maLoaiPhamVi = maLoaiPhamVi;

            // Lấy kết quả trả về
            NguoiDungResponse response = Client.layPhamViPhongGDTheoNSDVaMaLoaiPhamVi(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            return response.listPhamVi.ToList();
        }

        public ApplicationConstant.ResponseStatus ThemNSD(ref HT_NSD obj, List<int> lstIdNHNSD, List<PhamViDto> lstPhamVi, List<HT_TRUY_CAP> lstTruyCap, ref string responseMessage)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            NguoiDungRequest request = Common.Utilities.PrepareRequest(new NguoiDungRequest());

            request.Module = DatabaseConstant.Module.QTHT;
            request.Function = DatabaseConstant.Function.HT_NSD;
            request.Action = DatabaseConstant.Action.THEM;

            if (obj != null)
                request.objNSD = obj;
            if (lstIdNHNSD != null)
                request.lstIdNHNSD = lstIdNHNSD.ToArray();
            if (lstPhamVi != null)
                request.listPhamVi = lstPhamVi.ToArray();
            if (lstTruyCap != null)
                request.lstTruyCap = lstTruyCap.ToArray();

            // Lấy kết quả trả về
            NguoiDungResponse response = Client.ThemNSD(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            if (response == null)
            {
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            else
            {
                responseMessage = response.ResponseMessage;
                obj = response.NSD;
                return response.ResponseStatus;
            }            
        }

        public ApplicationConstant.ResponseStatus SuaNSD(ref HT_NSD obj, List<int> lstIdNHNSD, List<PhamViDto> lstPhamVi, List<HT_TRUY_CAP> lstTruyCap, ref string responseMessage)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            NguoiDungRequest request = Common.Utilities.PrepareRequest(new NguoiDungRequest());

            request.Module = DatabaseConstant.Module.QTHT;
            request.Function = DatabaseConstant.Function.HT_NSD;
            request.Action = DatabaseConstant.Action.SUA;

            if (obj != null)
                request.objNSD = obj;
            if (lstIdNHNSD != null)
                request.lstIdNHNSD = lstIdNHNSD.ToArray();
            if (lstPhamVi != null)
                request.listPhamVi = lstPhamVi.ToArray();
            if (lstTruyCap != null)
                request.lstTruyCap = lstTruyCap.ToArray();

            // Lấy kết quả trả về
            NguoiDungResponse response = Client.SuaNSD(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            if (response == null)
            {
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            else
            {
                responseMessage = response.ResponseMessage;
                obj = response.NSD;
                return response.ResponseStatus;
            }
        }

        public bool XoaListNSD(int[] listID, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            NguoiDungRequest request = Common.Utilities.PrepareRequest(new NguoiDungRequest());

            request.Module = DatabaseConstant.Module.QTHT;
            request.Function = DatabaseConstant.Function.HT_NSD;
            request.Action = DatabaseConstant.Action.XOA;

            if (listID != null)
                request.listID = listID.ToArray();

            // Lấy kết quả trả về
            NguoiDungResponse response = Client.XoaListNSDTheoID(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        public ApplicationConstant.ResponseStatus ThemNHNSD(ref HT_NHNSD obj, List<int> lstIdNSD, List<HT_TRUY_CAP> lstTruyCap, ref string responseMessage)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            NguoiDungRequest request = Common.Utilities.PrepareRequest(new NguoiDungRequest());

            request.Module = DatabaseConstant.Module.QTHT;
            request.Function = DatabaseConstant.Function.HT_NSD;
            request.Action = DatabaseConstant.Action.THEM;

            if (obj != null)
                request.objNHNSD = obj;
            if (lstIdNSD != null)
                request.lstIdNSD = lstIdNSD.ToArray();
            if (lstTruyCap != null)
                request.lstTruyCap = lstTruyCap.ToArray();

            // Lấy kết quả trả về
            NguoiDungResponse response = Client.ThemNHNSD(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            if (response == null)
            {
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            else
            {
                responseMessage = response.ResponseMessage;
                obj = response.NHNSD;
                return response.ResponseStatus;
            }
        }

        public ApplicationConstant.ResponseStatus SuaNHNSD(ref HT_NHNSD obj, List<int> lstIdNSD, List<HT_TRUY_CAP> lstTruyCap, ref string responseMessage)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            NguoiDungRequest request = Common.Utilities.PrepareRequest(new NguoiDungRequest());

            request.Module = DatabaseConstant.Module.QTHT;
            request.Function = DatabaseConstant.Function.HT_NSD;
            request.Action = DatabaseConstant.Action.SUA;

            if (obj != null)
                request.objNHNSD = obj;
            if (lstIdNSD != null)
                request.lstIdNSD = lstIdNSD.ToArray();
            if (lstTruyCap != null)
                request.lstTruyCap = lstTruyCap.ToArray();

            // Lấy kết quả trả về
            NguoiDungResponse response = Client.SuaNHNSD(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            if (response == null)
            {
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            else
            {
                responseMessage = response.ResponseMessage;
                obj = response.NHNSD;
                return response.ResponseStatus;
            }
        }

        public bool XoaListNHNSD(int[] listID, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            NguoiDungRequest request = Common.Utilities.PrepareRequest(new NguoiDungRequest());

            request.Module = DatabaseConstant.Module.QTHT;
            request.Function = DatabaseConstant.Function.HT_NSD;
            request.Action = DatabaseConstant.Action.XOA;

            if (listID != null)
                request.listID = listID.ToArray();

            // Lấy kết quả trả về
            NguoiDungResponse response = Client.XoaListNHNSDTheoID(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        public List<HT_TSO> layThamSo()
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            ThamSoRequest request = Common.Utilities.PrepareRequest(new ThamSoRequest());
            // Lấy kết quả trả về
            ThamSoResponse response = Client.layThamSo(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            return response.ListThamSo.ToList();
        }

        public List<HT_TSO> layThamSoHeThong(string loaiThamSo, string maDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            ThamSoRequest request = Common.Utilities.PrepareRequest(new ThamSoRequest());
            request.Module = DatabaseConstant.Module.QTHT;
            request.Function = DatabaseConstant.Function.HT_THAM_SO;
            request.Action = DatabaseConstant.Action.TRUY_VAN;

            request.loaiThamSo = loaiThamSo;
            request.maDonVi = maDonVi;

            // Lấy kết quả trả về
            ThamSoResponse response = Client.layThamSoHeThong(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            return response.ListThamSo.ToList();
        }

        public List<HT_TSO_LOAI> layLoaiThamSo()
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            ThamSoRequest request = Common.Utilities.PrepareRequest(new ThamSoRequest());
            // Lấy kết quả trả về
            ThamSoResponse response = Client.layLoaiThamSo(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            return response.ListLoaiThamSo.ToList();
        }

        public ApplicationConstant.ResponseStatus capNhatThamSo(DatabaseConstant.Action action, HT_TSO obj, List<int> listID)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            ThamSoRequest request = Common.Utilities.PrepareRequest(new ThamSoRequest());
            request.Action = action;
            if (obj != null)
                request.objThamSo = obj;
            if (listID != null)
                request.listID = listID.ToArray();
            // Lấy kết quả trả về
            ThamSoResponse response = Client.capNhatThamSo(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            return response.ResponseStatus;
        }

        public HT_TSO capNhatGiaTriThamSo(HT_TSO obj)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            ThamSoRequest request = Common.Utilities.PrepareRequest(new ThamSoRequest());
            request.Module = DatabaseConstant.Module.QTHT;
            request.Function = DatabaseConstant.Function.HT_THAM_SO;
            request.Action = DatabaseConstant.Action.SUA;

            request.objThamSo = obj;
            // Lấy kết quả trả về
            ThamSoResponse response = Client.capNhatGiaTriThamSo(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            return response.objThamSo;
        }

        public ApplicationConstant.ResponseStatus capNhatLoaiThamSo(DatabaseConstant.Action action, HT_TSO_LOAI obj, List<int> listID)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            ThamSoRequest request = Common.Utilities.PrepareRequest(new ThamSoRequest());
            request.Action = action;
            if (obj != null)
                request.objLoaiThamSo = obj;
            if (listID != null)
                request.listID = listID.ToArray();
            // Lấy kết quả trả về
            ThamSoResponse response = Client.capNhatLoaiThamSo(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            return response.ResponseStatus;
        }

        public ApplicationConstant.ResponseStatus DoiMatKhauNguoiDungLucDangNhap(string maNguoiDung, string oldPassword, string newPassword,
            ref string responseMessage)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            NguoiDungRequest request = Common.Utilities.PrepareRequest(new NguoiDungRequest());
            request.maNguoiDung = maNguoiDung;
            request.oldPassword = oldPassword;
            request.newPassword = newPassword;

            // Lấy kết quả trả về
            NguoiDungResponse response = Client.DoiMatKhauNguoiDungLucDangNhap(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            responseMessage = response.ResponseMessage.ToString();
            return response.ResponseStatus;
        }

        public ApplicationConstant.ResponseStatus DoiMatKhauNguoiDung(string maNguoiDung, string oldPassword, string newPassword,
            ref string responseMessage)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            NguoiDungRequest request = Common.Utilities.PrepareRequest(new NguoiDungRequest());
            request.maNguoiDung = maNguoiDung;
            request.oldPassword = oldPassword;
            request.newPassword = newPassword;

            // Lấy kết quả trả về
            NguoiDungResponse response = Client.DoiMatKhauNguoiDung(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            responseMessage = response.ResponseMessage.ToString();
            return response.ResponseStatus;
        }

        public ApplicationConstant.ResponseStatus ThietLapMatKhauNguoiDung(string maNguoiDung, string password, ref string responseMessage)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            NguoiDungRequest request = Common.Utilities.PrepareRequest(new NguoiDungRequest());
            request.maNguoiDung = maNguoiDung;
            request.newPassword = password;

            // Lấy kết quả trả về
            NguoiDungResponse response = Client.ThietLapMatKhauNguoiDung(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            responseMessage = response.ResponseMessage.ToString();
            return response.ResponseStatus;
        }

        public ApplicationConstant.ResponseStatus CheckClientVersion(string clientVersion, ref string serverVersion, ref string lastestClientVersion, ref HT_PBAN htPban, ref List<HT_PBAN_CTIET> listHtPbanCtiet, ref List<HT_PBAN_FILE> listHtPbanFile, ref string responseMessage)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            PhienBanRequest request = Common.Utilities.PrepareRequest(new PhienBanRequest());
            request.ClientVersion = clientVersion;

            // Lấy kết quả trả về
            PhienBanResponse response = Client.CheckClientVersion(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            serverVersion = response.ServerVersion;
            lastestClientVersion = response.LastestClientVersion;
            htPban = response.HtPban;
            listHtPbanCtiet = response.ListHtPbanCtiet.ToList();
            listHtPbanFile = response.ListHtPbanFile.ToList();            
            responseMessage = response.ResponseMessage.ToString();
            return response.ResponseStatus;
        }

        public ApplicationConstant.ResponseStatus DownloadClientVersion(string clientVersion, string lastestClientVersion, HT_PBAN htPBan, ref PhienBanDTO phienBanDTO, ref string responseMessage)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            PhienBanRequest request = Common.Utilities.PrepareRequest(new PhienBanRequest());
            request.ClientVersion = clientVersion;
            request.LastestClientVersion = lastestClientVersion;
            request.HtPban = htPBan;

            // Lấy kết quả trả về
            PhienBanResponse response = Client.DownloadClientVersion(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            phienBanDTO = response.PhienBanDTO;
            responseMessage = response.ResponseMessage.ToString();
            return response.ResponseStatus;
        }

        public ApplicationConstant.ResponseStatus DownloadClientVersionItem(string clientVersion, string lastestClientVersion, HT_PBAN htPban, HT_PBAN_CTIET htPbanCtiet, ref PhienBanItemDTO phienBanItemDTO, ref string responseMessage)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            PhienBanRequest request = Common.Utilities.PrepareRequest(new PhienBanRequest());
            request.ClientVersion = clientVersion;
            request.LastestClientVersion = lastestClientVersion;
            request.HtPban = htPban;
            request.HtPbanCtiet = htPbanCtiet;

            // Lấy kết quả trả về
            PhienBanResponse response = Client.DownloadClientVersionItem(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            phienBanItemDTO = response.PhienBanItemDTO;
            responseMessage = response.ResponseMessage.ToString();
            return response.ResponseStatus;
        }

        public ApplicationConstant.ResponseStatus LayDanhSachPhongGD(ref List<DM_DON_VI> listPhongGD, ref string responseMessage)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            NguoiDungRequest request = Common.Utilities.PrepareRequest(new NguoiDungRequest());

            // Lấy kết quả trả về
            NguoiDungResponse response = Client.LayDanhSachPhongGD(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            listPhongGD = response.ListPhongGD.ToList();
            responseMessage = response.ResponseMessage.ToString();
            return response.ResponseStatus;
        }

        public ApplicationConstant.ResponseStatus LuuPhanQuyenPhamVi(ref PHAM_VI objPhamVi, ref string responseMessage)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            PhamViRequest request = Common.Utilities.PrepareRequest(new PhamViRequest());
            request.Action = DatabaseConstant.Action.LUU;
            request.objPhamVi = objPhamVi;

            // Lấy kết quả trả về
            PhamViResponse response = Client.PhanQuyenPhamVi(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            objPhamVi = response.objPhamVi;
            responseMessage = response.ResponseMessage.ToString();
            return response.ResponseStatus;
        }

        public ApplicationConstant.ResponseStatus XoaPhanQuyenPhamVi(ref PHAM_VI objPhamVi, ref string responseMessage)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            PhamViRequest request = Common.Utilities.PrepareRequest(new PhamViRequest());
            request.Action = DatabaseConstant.Action.XOA;
            request.objPhamVi = objPhamVi;

            // Lấy kết quả trả về
            PhamViResponse response = Client.PhanQuyenPhamVi(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            objPhamVi = response.objPhamVi;
            responseMessage = response.ResponseMessage.ToString();
            return response.ResponseStatus;
        }

        public ApplicationConstant.ResponseStatus LayPhanQuyenPhamVi(ref PHAM_VI objPhamVi, ref string responseMessage)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            PhamViRequest request = Common.Utilities.PrepareRequest(new PhamViRequest());
            request.Action = DatabaseConstant.Action.LAY_LAI;
            request.objPhamVi = objPhamVi;

            // Lấy kết quả trả về
            PhamViResponse response = Client.PhanQuyenPhamVi(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            objPhamVi = response.objPhamVi;
            responseMessage = response.ResponseMessage.ToString();
            return response.ResponseStatus;
        }

        public ApplicationConstant.ResponseStatus MaTranPheDuyet(ref HT_CNANG obj, ref string responseMessage)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.QuanTriHeThongService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            ChucNangRequest request = Common.Utilities.PrepareRequest(new ChucNangRequest());
            request.Action = DatabaseConstant.Action.LUU;
            request.htCNang = obj;

            // Lấy kết quả trả về
            ChucNangResponse response = Client.ChucNang(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            obj = response.htCNang;
            responseMessage = response.ResponseMessage.ToString();
            return response.ResponseStatus;
        }
    }
}
