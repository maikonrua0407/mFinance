using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Data;
using System.Drawing;
using System.IO;
using Presentation.Process.TruyVanServiceRef;
using Utilities.Common;
using Presentation.Process.TinDungServiceRef;
using Presentation.Process.Common;
using Presentation.Process.UtilitiesServiceRef;
using System.ServiceModel.Description;

namespace Presentation.Process
{
    public class TinDungProcess
    {
        /// <summary>
        /// Biến cục bộ chứa dịch vụ tiện ích của server
        /// </summary>
        private static TruyVanServiceClient ClientTruyVan { get; set; }
        private static TinDungServiceClient ClientTinDung { get; set; }

        /// <summary>
        /// Khởi tạo service Tín dụng
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        private TinDungServiceClient TinDungServiceClient(ApplicationConstant.SystemService service)
        {
            EndpointAddress endpointAddress = Common.Utilities.getEndpointAddress(service.layGiaTri());
            BasicHttpBinding basicHttpBinding = Common.Utilities.getBasicHttpBinding(service.layGiaTri());
            TinDungServiceClient Client = new TinDungServiceClient(basicHttpBinding, endpointAddress);

            foreach (var operationDescription in Client.Endpoint.Contract.Operations)
            {
                DataContractSerializerOperationBehavior dcsob =
                    operationDescription.Behaviors.Find<DataContractSerializerOperationBehavior>();
                if (dcsob != null)
                {
                    dcsob.MaxItemsInObjectGraph = 2147483646;
                }
            }

            return Client;
        }

        /// <summary>
        /// Khởi tạo service TruyVanService
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        private TruyVanServiceClient TruyVanServiceClient(ApplicationConstant.SystemService service)
        {
            EndpointAddress endpointAddress = Common.Utilities.getEndpointAddress(service.layGiaTri());
            BasicHttpBinding basicHttpBinding = Common.Utilities.getBasicHttpBinding(service.layGiaTri());
            TruyVanServiceClient Client = new TruyVanServiceClient(basicHttpBinding, endpointAddress);

            foreach (var operationDescription in Client.Endpoint.Contract.Operations)
            {
                DataContractSerializerOperationBehavior dcsob =
                    operationDescription.Behaviors.Find<DataContractSerializerOperationBehavior>();
                if (dcsob != null)
                {
                    dcsob.MaxItemsInObjectGraph = 2147483646;
                }
            }

            return Client;
        }        

        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        static TinDungProcess()
        {
            //Client = new TinDungServiceClient();
            EndpointAddress endpointAddress = Common.Utilities.getEndpointAddress(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            BasicHttpBinding basicHttpBinding = Common.Utilities.getBasicHttpBinding(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            ClientTinDung = new TinDungServiceClient(basicHttpBinding, endpointAddress);

            foreach (var operationDescription in ClientTinDung.Endpoint.Contract.Operations)
            {
                DataContractSerializerOperationBehavior dcsob =
                    operationDescription.Behaviors.Find<DataContractSerializerOperationBehavior>();
                if (dcsob != null)
                {
                    dcsob.MaxItemsInObjectGraph = 2147483646;
                }
            }

            EndpointAddress endpointAddressTruyVan = Common.Utilities.getEndpointAddress(ApplicationConstant.SystemService.TruyVanService.layGiaTri());
            BasicHttpBinding basicHttpBindingTruyVan = Common.Utilities.getBasicHttpBinding(ApplicationConstant.SystemService.TruyVanService.layGiaTri());
            ClientTruyVan = new TruyVanServiceClient(basicHttpBindingTruyVan, endpointAddressTruyVan);

            foreach (var operationDescription in ClientTruyVan.Endpoint.Contract.Operations)
            {
                DataContractSerializerOperationBehavior dcsob =
                    operationDescription.Behaviors.Find<DataContractSerializerOperationBehavior>();
                if (dcsob != null)
                {
                    dcsob.MaxItemsInObjectGraph = 2147483646;
                }
            }
        }
        #region Sản phẩm tín dụng
        /// <summary>
        /// Lưu sản phẩm tín dụng
        /// </summary>
        /// <param name="objSanPham"></param>
        /// <returns></returns>
        public int LuuSanPhamTinDung(TinDungServiceRef.TD_SAN_PHAM objSanPham, List<KT_PHAN_HE_PLOAI> listPhanLoai)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TDSanPhamRequest());

            request.obj = objSanPham;
            request.lstPhanHePLoai = listPhanLoai.ToArray();
            // Lấy kết quả trả về
            TDSanPhamResponse response = ClientTinDung.LuuSanPham(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response == null)
            {
                return 0;
            }
            else
            {
                return response.iKetQua;
            }
        }

        /// <summary>
        /// Sửa sản phẩm tín dụng
        /// </summary>
        /// <param name="objSanPham"></param>
        /// <returns></returns>
        public int SuaSanPhamTinDung(TinDungServiceRef.TD_SAN_PHAM objSanPham, List<KT_PHAN_HE_PLOAI> listPhanLoai)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TDSanPhamRequest());

            request.obj = objSanPham;
            request.lstPhanHePLoai = listPhanLoai.ToArray();
            request.Action = DatabaseConstant.Action.SUA;
            // Lấy kết quả trả về
            TDSanPhamResponse response = ClientTinDung.CapNhatSanPham(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response == null)
            {
                return 0;
            }
            else
            {
                return response.iKetQua;
            }
        }

        /// <summary>
        /// Xóa sản phẩm tín dụng
        /// </summary>
        /// <param name="objSanPham"></param>
        /// <returns></returns>
        public bool XoaSanPhamTinDung(List<int> lstid, ref List<ClientResponseDetail> responseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TDSanPhamRequest());

            request.listID = lstid.ToArray();

            // Lấy kết quả trả về
            TDSanPhamResponse response = ClientTinDung.XoaSanPham(request);
            
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response == null)
            {
                return false;
            }
            else
            {
                responseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        /// <summary>
        /// Tính toán dữ liệu bảng kê gốc lãi theo nhóm vong vay và lãi suất
        /// </summary>
        /// <param name="dsVongVonVay"></param>
        /// <param name="dsLaiSuat"></param>
        /// <returns></returns>
        public int TinhToanBangKeGoclai(ref SAN_PHAM_TIN_DUNG obj)
        {
            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());

                // Khởi tạo và gán giá trị cho request
                var request = Common.Utilities.PrepareRequest(new TDSanPhamRequest());

                request.objSPham = obj;

                // Lấy kết quả trả về
                TDSanPhamResponse response = ClientTinDung.TinhToanGocVay(request);

                // Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response == null)
                {
                    return 0;
                }
                else
                {
                    obj = response.objSPham;
                    return 1;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                return 0;
            }
        }

        /// <summary>
        /// Duyệt sản phẩm tín dụng
        /// </summary>
        /// <param name="objSanPham"></param>
        /// <returns></returns>
        public bool DuyetSanPhamTinDung(List<int> lstid, ref List<ClientResponseDetail> responseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TDSanPhamRequest());

            request.listID = lstid.ToArray();
            request.Action = DatabaseConstant.Action.DUYET;
            // Lấy kết quả trả về
            TDSanPhamResponse response = ClientTinDung.CapNhatSanPham(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response == null)
            {
                return false;
            }
            else
            {
                responseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        /// <summary>
        /// Từ chối sản phẩm tín dụng
        /// </summary>
        /// <param name="objSanPham"></param>
        /// <returns></returns>
        public bool TuChoiSanPhamTinDung(List<int> lstid, ref List<ClientResponseDetail> responseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TDSanPhamRequest());

            request.listID = lstid.ToArray();
            request.Action = DatabaseConstant.Action.TU_CHOI_DUYET;
            // Lấy kết quả trả về
            TDSanPhamResponse response = ClientTinDung.CapNhatSanPham(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response == null)
            {
                return false;
            }
            else
            {
                responseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        /// <summary>
        /// Hủy duyệt sản phẩm tín dụng
        /// </summary>
        /// <param name="objSanPham"></param>
        /// <returns></returns>
        public bool HuyDuyetSanPhamTinDung(List<int> lstid, ref List<ClientResponseDetail> responseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TDSanPhamRequest());

            request.listID = lstid.ToArray();
            request.Action = DatabaseConstant.Action.THOAI_DUYET;
            // Lấy kết quả trả về
            TDSanPhamResponse response = ClientTinDung.CapNhatSanPham(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response == null)
            {
                return false;
            }
            else
            {
                responseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        /// <summary>
        /// Lấy thông tin chi tiết vòng vay tín dụng
        /// </summary>
        /// <param name="idVongVay"></param>
        /// <returns></returns>
        public DataSet getSanPhamTDByID(string idSanPham)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_ID", "Int", idSanPham);
            request.dtThamSo = dt;
            request.inquiryName = "TTIN_CTIET";
            request.objectName = DatabaseConstant.DanhSachTruyVanTinDung.INQ_CT_TD_SAN_PHAM.getValue();
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Lấy danh sách sản phẩm tín dụng
        /// </summary>
        /// <param name="MaTTNVu"></param>
        /// <param name="MaNhomVV"></param>
        /// <param name="NgayLapTu"></param>
        /// <param name="NgayLapDen"></param>
        /// <param name="HMGocVay"></param>
        /// <param name="HMKyHan"></param>
        /// <param name="UserName"></param>
        /// <param name="MaDonVi"></param>
        /// <returns></returns>
        public DataSet getDanhSachSanPhamTinDung(string MaTTNVu, string MaLoaiSP, string MaTinhTrangHLuc, string MaMucDichVay, string MaNguonVon, string MaPhuongThucTinhLai, string MaLoaiLSuat, string MaSanPham, string TenSanPham, string NgayADungTu, string NgayADungDen, string NgayHetHanTu, string NgayHetHanDen,string PThucVay, string UserName, string MaDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_TRANG_THAI_NGHIEP_VU", "String", MaTTNVu);
            LDatatable.AddParameter(ref dt, "@INP_MA_LOAI_SAN_PHAM", "String", MaLoaiSP);
            LDatatable.AddParameter(ref dt, "@INP_MA_TINH_TRANG_HIEU_LUC", "String", MaTinhTrangHLuc);
            LDatatable.AddParameter(ref dt, "@INP_MA_MUC_DICH_VAY", "String", MaMucDichVay);
            LDatatable.AddParameter(ref dt, "@INP_MA_NGUON_VON", "String", MaNguonVon);
            LDatatable.AddParameter(ref dt, "@INP_MA_PHUONG_THUC_TINH_LAI", "String", MaPhuongThucTinhLai);
            LDatatable.AddParameter(ref dt, "@INP_MA_LOAI_LAI_SUAT", "String", MaLoaiLSuat);
            LDatatable.AddParameter(ref dt, "@INP_MA_SAN_PHAM", "String", MaSanPham);
            LDatatable.AddParameter(ref dt, "@INP_TEN_SAN_PHAM", "String", TenSanPham);
            LDatatable.AddParameter(ref dt, "@INP_NGAY_AP_DUNG_TU", "String", NgayADungTu);
            LDatatable.AddParameter(ref dt, "@INP_NGAY_AP_DUNG_DEN", "String", NgayADungDen);
            LDatatable.AddParameter(ref dt, "@INP_NGAY_HET_HAN_TU", "String", NgayHetHanTu);
            LDatatable.AddParameter(ref dt, "@INP_NGAY_HET_HAN_DEN", "String", NgayHetHanDen);
            LDatatable.AddParameter(ref dt, "@INP_PHUONG_THUC_VAY", "String", PThucVay);
            LDatatable.AddParameter(ref dt, "@INP_TEN_DANG_NHAP", "String", UserName);
            LDatatable.AddParameter(ref dt, "@INP_MA_DON_VI", "demical", MaDonVi);

            request.dtThamSo = dt;
            request.inquiryName = "DANH_SACH";
            request.objectName = DatabaseConstant.DanhSachTruyVanTinDung.INQ_DS_TD_SAN_PHAM.getValue();
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetTaiKhoanHachToan(string maDoiTuong, string maDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_PHAN_HE", "STRING", DatabaseConstant.Module.TDVM.getValue());
            LDatatable.AddParameter(ref dt, "@MA_DTUONG", "STRING", maDoiTuong);
            LDatatable.AddParameter(ref dt, "@MA_DON_VI", "STRING", maDonVi);

            request.dtThamSo = dt;
            request.objectName = DatabaseConstant.DanhSachTruyVanTinDung.INQ_CT_TD_SAN_PHAM.getValue();
            request.inquiryName = "TAI_KHOAN_HACH_TOAN";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public int BaremTinhLaiTienVay(decimal TongTienGop, decimal SoKy, out decimal SoTienGoc, out decimal SoTienLai, string maDonVi)
        {
            decimal dSoLamTron = 0;
            decimal dSoTienBarem = 1000000;
            string sGiaTriThamSo = new UtilitiesProcess().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_MUC_LAM_TRON_TD_TINH_LAI, maDonVi);
            if (sGiaTriThamSo.IsNumeric())
                dSoLamTron = sGiaTriThamSo.StringToDecimal();
            sGiaTriThamSo = new UtilitiesProcess().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_SO_TIEN_ADUNG_TINH_BAREM, maDonVi);
            if (sGiaTriThamSo.IsNumeric())
                dSoTienBarem = sGiaTriThamSo.StringToDecimal();

            SoTienGoc = 0;
            SoTienLai = 0;
            if (SoKy == 0 || dSoLamTron==0)
                return 0;
            decimal GocMoiKy = (dSoTienBarem / SoKy / dSoLamTron).Rounding(0, LNumber.RoundingType.Down) * dSoLamTron;
            if (GocMoiKy > TongTienGop)
                return 0;
            SoTienGoc = GocMoiKy;
            SoTienLai = TongTienGop - GocMoiKy;
            return 1;
        }
        #endregion

        #region Vòng vay vốn tín dụng
        /// <summary>
        /// Lưu vòng vay tín dụng
        /// </summary>
        /// <param name="objSanPham"></param>
        /// <returns></returns>
        public int LuuVongVayTinDung(TD_VONG_VAY objVongVay, List<TD_VONG_VAY_CTIET> lstobjVongVayCT)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TDVongVayRequest());

            request.obj = objVongVay;
            request.listobjCT = lstobjVongVayCT.ToArray();
            // Lấy kết quả trả về
            TDVongVayResponse response = ClientTinDung.LuuVongVay(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response == null)
            {
                return 0;
            }
            else
            {
                return response.iKetQua;
            }
        }

        /// <summary>
        /// Sửa vòng vay tín dụng
        /// </summary>
        /// <param name="objSanPham"></param>
        /// <returns></returns>
        public int SuaVongVayTinDung(TD_VONG_VAY objVongVay, List<TD_VONG_VAY_CTIET> lstobjVongVayCT)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TDVongVayRequest());

            request.obj = objVongVay;
            request.listobjCT = lstobjVongVayCT.ToArray();
            request.Action = DatabaseConstant.Action.SUA;
            // Lấy kết quả trả về
            TDVongVayResponse response = ClientTinDung.CapNhatVongVay(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response == null)
            {
                return 0;
            }
            else
            {
                return response.iKetQua;
            }
        }

        /// <summary>
        /// Xóa vòng vay tín dụng
        /// </summary>
        /// <param name="objSanPham"></param>
        /// <returns></returns>
        public bool XoaVongVayTinDung(int[] lstid,ref List<ClientResponseDetail> responseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TDVongVayRequest());

            request.listID = lstid;

            // Lấy kết quả trả về
            TDVongVayResponse response = ClientTinDung.XoaVongVay(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            responseDetail = Common.Utilities.convertToClientResponseDetail(response);
            if (response == null)
            {
                return false;
            }
            else
            {
                return response.bKetQua;
            }
        }

        /// <summary>
        /// Duyệt vòng vay tín dụng
        /// </summary>
        /// <param name="objSanPham"></param>
        /// <returns></returns>
        public bool DuyetVongVayTinDung(int[] lstid, ref List<ClientResponseDetail> responseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TDVongVayRequest());

            request.listID = lstid;
            request.Action = DatabaseConstant.Action.DUYET;
            // Lấy kết quả trả về
            TDVongVayResponse response = ClientTinDung.CapNhatVongVay(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            responseDetail = Common.Utilities.convertToClientResponseDetail(response);
            if (response == null)
            {
                return false;
            }
            else
            {
                return response.bKetQua;
            }
        }

        /// <summary>
        /// Từ chối vòng vay tín dụng
        /// </summary>
        /// <param name="objSanPham"></param>
        /// <returns></returns>
        public bool TuChoiVongVayTinDung(int[] lstid, ref List<ClientResponseDetail> responseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TDVongVayRequest());

            request.listID = lstid;
            request.Action = DatabaseConstant.Action.TU_CHOI_DUYET;
            // Lấy kết quả trả về
            TDVongVayResponse response = ClientTinDung.CapNhatVongVay(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            responseDetail = Common.Utilities.convertToClientResponseDetail(response);
            if (response == null)
            {
                return false;
            }
            else
            {
                return response.bKetQua;
            }
        }

        /// <summary>
        /// Hủy duyệt vòng vay tín dụng
        /// </summary>
        /// <param name="objSanPham"></param>
        /// <returns></returns>
        public bool HuyDuyetVongVayTinDung(int[] lstid, ref List<ClientResponseDetail> responseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TDVongVayRequest());

            request.listID = lstid;
            request.Action = DatabaseConstant.Action.THOAI_DUYET;
            // Lấy kết quả trả về
            TDVongVayResponse response = ClientTinDung.CapNhatVongVay(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            responseDetail = Common.Utilities.convertToClientResponseDetail(response);
            if (response == null)
            {
                return false;
            }
            else
            {
                return response.bKetQua;
            }
        }

        /// <summary>
        /// Lấy danh sách vòng vay tín dụng
        /// </summary>
        /// <param name="MaTTNVu"></param>
        /// <param name="MaNhomVV"></param>
        /// <param name="NgayLapTu"></param>
        /// <param name="NgayLapDen"></param>
        /// <param name="HMGocVay"></param>
        /// <param name="HMKyHan"></param>
        /// <param name="UserName"></param>
        /// <param name="MaDonVi"></param>
        /// <returns></returns>
        public DataSet getDanhSachVongVonVay(string MaTTNVu, string MaNhomVV, string NgayLapTu, string NgayLapDen, string HMGocVay, string HMKyHan, string UserName, string MaDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_TT_NGVU", "String", MaTTNVu);
            LDatatable.AddParameter(ref dt, "@MA_NHOM_VONGVAY", "String", MaNhomVV);
            LDatatable.AddParameter(ref dt, "@NGAY_LAP_TU", "String", NgayLapTu);
            LDatatable.AddParameter(ref dt, "@NGAY_LAP_DEN", "String", NgayLapDen);
            LDatatable.AddParameter(ref dt, "@HM_GOC_VAY", "demical", HMGocVay);
            LDatatable.AddParameter(ref dt, "@HM_KY_HAN", "demical", HMKyHan);
            LDatatable.AddParameter(ref dt, "@USER_NAME", "String", UserName);
            LDatatable.AddParameter(ref dt, "@MA_DON_VI", "demical", MaDonVi);

            request.dtThamSo = dt;
            request.inquiryName = "DANH_SACH";
            request.objectName = DatabaseConstant.DanhSachTruyVanTinDung.INQ_DS_TD_VONG_VAY.getValue();
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Lấy thông tin chi tiết vòng vay tín dụng
        /// </summary>
        /// <param name="idVongVay"></param>
        /// <returns></returns>
        public DataSet getVongVonVayByID(string idVongVay)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_ID", "Int", idVongVay);
            request.dtThamSo = dt;
            request.inquiryName = "TTIN_CTIET";
            request.objectName = DatabaseConstant.DanhSachTruyVanTinDung.INQ_CT_TD_VONG_VAY.getValue();
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        #endregion

        #region Hợp đồng tín dụng vi mô

        /// <summary>
        /// Thêm mới hợp đồng tín dụng
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="lstResponseDetail"></param>
        /// <returns></returns>
        public int ThemMoiHopDongTinDungViMo(ref TDVM_HDTD obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objHDTDVM = obj;
            request.Action = DatabaseConstant.Action.THEM;
            request.Function = DatabaseConstant.Function.TDVM_CHI_TIET_HOP_DONG;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objHDTDVM;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        /// <summary>
        /// Sửa hợp đồng tín dụng
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="lstResponseDetail"></param>
        /// <returns></returns>
        public int SuaHopDongTinDungViMo(ref TDVM_HDTD obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objHDTDVM = obj;
            request.Action = DatabaseConstant.Action.SUA;
            request.Function = DatabaseConstant.Function.TDVM_CHI_TIET_HOP_DONG;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objHDTDVM;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        /// <summary>
        /// Xoa hợp đồng tín dụng
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="lstResponseDetail"></param>
        /// <returns></returns>
        public int XoaHopDongTinDungViMo(TDVM_HDTD obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objHDTDVM = obj;
            request.Action = DatabaseConstant.Action.XOA;
            request.Function = DatabaseConstant.Function.TDVM_CHI_TIET_HOP_DONG;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objHDTDVM;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        /// <summary>
        /// Duyet hợp đồng tín dụng
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="lstResponseDetail"></param>
        /// <returns></returns>
        public int DuyetHopDongTinDungViMo(TDVM_HDTD obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objHDTDVM = obj;
            request.Action = DatabaseConstant.Action.DUYET;
            request.Function = DatabaseConstant.Function.TDVM_CHI_TIET_HOP_DONG;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objHDTDVM;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        /// <summary>
        /// Thoai Duyet hợp đồng tín dụng
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="lstResponseDetail"></param>
        /// <returns></returns>
        public int ThoaiDuyetHopDongTinDungViMo(TDVM_HDTD obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objHDTDVM = obj;
            request.Action = DatabaseConstant.Action.THOAI_DUYET;
            request.Function = DatabaseConstant.Function.TDVM_CHI_TIET_HOP_DONG;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objHDTDVM;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        /// <summary>
        /// Thoai Duyet hợp đồng tín dụng
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="lstResponseDetail"></param>
        /// <returns></returns>
        public int TuChoiDuyetHopDongTinDungViMo(TDVM_HDTD obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objHDTDVM = obj;
            request.Action = DatabaseConstant.Action.TU_CHOI_DUYET;
            request.Function = DatabaseConstant.Function.TDVM_CHI_TIET_HOP_DONG;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objHDTDVM;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public DataSet getThongTinChiTietHDTDVMBySoGiaoDich(string soGiaoDich)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@SoGiaoDich", "string", soGiaoDich);
            request.dtThamSo = dt;
            request.inquiryName = "TTIN_CTIET";
            request.objectName = DatabaseConstant.DanhSachTruyVanTinDung.INQ_CT_TDVM_HDTD.getValue();
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet LayDSDonVayVon(int idKhachHang)
        {
            //Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanResponse response = new TruyVanResponse();
            DataTable dt = null;
            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID_KHACH_HANG", "string", idKhachHang.ToString());
            request.dtThamSo = dt;
            request.objectName = "INQ.DS_DON_XIN_VAY_VON";
            request.inquiryName = "DSKH";

            response = ClientTruyVan.TruyVanMessage(request);

            Common.Utilities.ValidResponse(request, response);
            return response.dsResult;
        }

        public DataSet LayDSDonVayVonBIDV(int idKhachHang)
        {
            //Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanResponse response = new TruyVanResponse();
            DataTable dt = null;
            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID_KHACH_HANG", "string", idKhachHang.ToString());
            request.dtThamSo = dt;
            request.objectName = "INQ.DS_DON_XIN_VAY_VON";
            request.inquiryName = "DSDXVV";

            response = ClientTruyVan.TruyVanMessage(request);

            Common.Utilities.ValidResponse(request, response);
            return response.dsResult;
        }

        public DataSet getDanhSachHDTDVM(string INP_MA_TRANG_THAI_NGHIEP_VU, string INP_SO_HDTD, string INP_NGAY_HD_TU, string INP_NGAY_HD_DEN, string INP_LS_QHAN_TU, string INP_LS_QHAN_DEN, string INP_LS_CCAU_TU, string INP_LS_CCAU_DEN, string INP_MA_KHANG, string INP_TEN_KHANG, string INP_MA_GTO, string INP_SO_GTO, string INP_DIEN_THOAI, string INP_EMAIL, string INP_SANPHAM, string INP_KHUVUC,string INP_USERNAME, string MA_DVI_QLY, string sStartRow, string sEndRow)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_TRANG_THAI_NGHIEP_VU", "string", INP_MA_TRANG_THAI_NGHIEP_VU);
            LDatatable.AddParameter(ref dt, "@INP_SO_HDTD", "string", INP_SO_HDTD);
            LDatatable.AddParameter(ref dt, "@INP_NGAY_HD_TU", "string", INP_NGAY_HD_TU);
            LDatatable.AddParameter(ref dt, "@INP_NGAY_HD_DEN", "string", INP_NGAY_HD_DEN);
            LDatatable.AddParameter(ref dt, "@INP_LS_QHAN_TU", "string", INP_LS_QHAN_TU);
            LDatatable.AddParameter(ref dt, "@INP_LS_QHAN_DEN", "string", INP_LS_QHAN_DEN);
            LDatatable.AddParameter(ref dt, "@INP_LS_CCAU_TU", "string", INP_LS_CCAU_TU);
            LDatatable.AddParameter(ref dt, "@INP_LS_CCAU_DEN", "string", INP_LS_CCAU_DEN);
            LDatatable.AddParameter(ref dt, "@INP_MA_KHANG", "string", INP_MA_KHANG);
            LDatatable.AddParameter(ref dt, "@INP_TEN_KHANG", "string", INP_TEN_KHANG);
            LDatatable.AddParameter(ref dt, "@INP_MA_GTO", "string", INP_MA_GTO);
            LDatatable.AddParameter(ref dt, "@INP_SO_GTO", "string", INP_SO_GTO);
            LDatatable.AddParameter(ref dt, "@INP_DIEN_THOAI", "string", INP_DIEN_THOAI);
            LDatatable.AddParameter(ref dt, "@INP_EMAIL", "string", INP_EMAIL);
            LDatatable.AddParameter(ref dt, "@INP_SANPHAM", "string", INP_SANPHAM);
            LDatatable.AddParameter(ref dt, "@INP_KHUVUC", "string", INP_KHUVUC);
            LDatatable.AddParameter(ref dt, "@INP_USERNAME", "string", INP_USERNAME);
            LDatatable.AddParameter(ref dt, "@MA_DVI_QLY", "string", MA_DVI_QLY);
            LDatatable.AddParameter(ref dt, "@StartRow", "INT", sStartRow);
            LDatatable.AddParameter(ref dt, "@EndRow", "INT", sEndRow);
            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = DatabaseConstant.DanhSachTruyVanTinDung.INQ_DS_TDVM_HDTD.getValue();
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet getDanhSachDonVayVon(string INP_MA_TRANG_THAI_NGHIEP_VU, string INP_SO_HDTD, string INP_NGAY_HD_TU, string INP_NGAY_HD_DEN, string INP_LOAI_SPHAM, string INP_SPHAM, string INP_MUC_DICH_VAY, string INP_TGIAN_VAY_TU, string INP_TGIAN_VAY_DEN, string INP_TGIAN_DVTINH_TU, string INP_TGIAN_DVTINH_DEN, string INP_MUC_XIN_VAY_TU, string INP_MUC_XIN_VAY_DEN, string INP_MUC_DUYET_VAY_TU, string INP_MUC_DUYET_VAY_DEN, string INP_MA_KHANG, string INP_TEN_KHANG, string INP_MA_GTO, string INP_SO_GTO, string INP_DIEN_THOAI, string INP_EMAIL, string INP_SANPHAM, string INP_KHUVUC, string INP_USERNAME, string MA_DVI_QLY, string sStartRow, string sEndRow)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_TRANG_THAI_NGHIEP_VU", "string", INP_MA_TRANG_THAI_NGHIEP_VU);
            LDatatable.AddParameter(ref dt, "@INP_SO_HDTD", "string", INP_SO_HDTD);
            LDatatable.AddParameter(ref dt, "@INP_NGAY_HD_TU", "string", INP_NGAY_HD_TU);
            LDatatable.AddParameter(ref dt, "@INP_NGAY_HD_DEN", "string", INP_NGAY_HD_DEN);
            LDatatable.AddParameter(ref dt, "@INP_LOAI_SPHAM", "string", INP_LOAI_SPHAM);
            LDatatable.AddParameter(ref dt, "@INP_SPHAM", "string", INP_SPHAM);
            LDatatable.AddParameter(ref dt, "@INP_MUC_DICH_VAY", "string", INP_MUC_DICH_VAY);
            LDatatable.AddParameter(ref dt, "@INP_TGIAN_VAY_TU", "string", INP_TGIAN_VAY_TU);
            LDatatable.AddParameter(ref dt, "@INP_TGIAN_VAY_DEN", "string", INP_TGIAN_VAY_DEN);
            LDatatable.AddParameter(ref dt, "@INP_TGIAN_DVTINH_TU", "string", INP_TGIAN_DVTINH_TU);
            LDatatable.AddParameter(ref dt, "@INP_TGIAN_DVTINH_DEN", "string", INP_TGIAN_DVTINH_DEN);
            LDatatable.AddParameter(ref dt, "@INP_MUC_XIN_VAY_TU", "string", INP_MUC_XIN_VAY_TU);
            LDatatable.AddParameter(ref dt, "@INP_MUC_XIN_VAY_DEN", "string", INP_MUC_XIN_VAY_DEN);
            LDatatable.AddParameter(ref dt, "@INP_MUC_DUYET_VAY_TU", "string", INP_MUC_DUYET_VAY_TU);
            LDatatable.AddParameter(ref dt, "@INP_MUC_DUYET_VAY_DEN", "string", INP_MUC_DUYET_VAY_DEN);
            LDatatable.AddParameter(ref dt, "@INP_MA_KHANG", "string", INP_MA_KHANG);
            LDatatable.AddParameter(ref dt, "@INP_TEN_KHANG", "string", INP_TEN_KHANG);
            LDatatable.AddParameter(ref dt, "@INP_MA_GTO", "string", INP_MA_GTO);
            LDatatable.AddParameter(ref dt, "@INP_SO_GTO", "string", INP_SO_GTO);
            LDatatable.AddParameter(ref dt, "@INP_DIEN_THOAI", "string", INP_DIEN_THOAI);
            LDatatable.AddParameter(ref dt, "@INP_EMAIL", "string", INP_EMAIL);
            LDatatable.AddParameter(ref dt, "@INP_SANPHAM", "string", INP_SANPHAM);
            LDatatable.AddParameter(ref dt, "@INP_KHUVUC", "string", INP_KHUVUC);
            LDatatable.AddParameter(ref dt, "@INP_USERNAME", "string", INP_USERNAME);
            LDatatable.AddParameter(ref dt, "@MA_DVI_QLY", "string", MA_DVI_QLY);
            LDatatable.AddParameter(ref dt, "@StartRow", "INT", sStartRow);
            LDatatable.AddParameter(ref dt, "@EndRow", "INT", sEndRow);
            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = DatabaseConstant.DanhSachTruyVanTinDung.INQ_DS_TDVM_DONXINVAY.getValue();
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        #endregion

        #region Lấy thông tin lãi suất
        public DataSet getLaiSuatByID(string idLSuat)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID", "Int", idLSuat);
            request.dtThamSo = dt;
            request.inquiryName = "INQ.CT.DC_LSUAT";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        #endregion

        #region Khế ước tín dụng vi mô
        public int TinhToanLaiSuat(ref TDVM_KHE_UOC obj)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objKheUocVM = obj;
            request.Action = DatabaseConstant.Action.TINH_TOAN_LAI_SUAT;
            request.Function = DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objKheUocVM;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }
        public int TinhToanLichTraNo(ref TDVM_KHE_UOC obj)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objKheUocVM = obj;
            request.Action = DatabaseConstant.Action.TINH_TOAN_LICH_TRA_NO;
            request.Function = DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objKheUocVM;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }
        public int TinhToanSoTienGiaiNgan(ref TDVM_KHE_UOC obj)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objKheUocVM = obj;
            request.Action = DatabaseConstant.Action.TINH_TOAN_SO_TIEN_VAY;
            request.Function = DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objKheUocVM;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }
        /// <summary>
        /// Thêm mới khế ước tín dụng
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="lstResponseDetail"></param>
        /// <returns></returns>
        public int ThemMoiKheUocTinDungViMo(ref TDVM_KHE_UOC obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objKheUocVM = obj;
            request.Action = DatabaseConstant.Action.THEM;
            request.Function = DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objKheUocVM;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int SuaKheUocTinDungViMo(ref TDVM_KHE_UOC obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objKheUocVM = obj;
            request.Action = DatabaseConstant.Action.SUA;
            request.Function = DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objKheUocVM;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int XoaKheUocTinDungViMo(ref TDVM_KHE_UOC obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objKheUocVM = obj;
            request.Action = DatabaseConstant.Action.XOA;
            request.Function = DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objKheUocVM;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int DuyetKheUocTinDungViMo(ref TDVM_KHE_UOC obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objKheUocVM = obj;
            request.Action = DatabaseConstant.Action.DUYET;
            request.Function = DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objKheUocVM;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int ThoaiDuyetKheUocTinDungViMo(ref TDVM_KHE_UOC obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objKheUocVM = obj;
            request.Action = DatabaseConstant.Action.THOAI_DUYET;
            request.Function = DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objKheUocVM;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int TuChoiDuyetKheUocTinDungViMo(ref TDVM_KHE_UOC obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objKheUocVM = obj;
            request.Action = DatabaseConstant.Action.TU_CHOI_DUYET;
            request.Function = DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objKheUocVM;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }
        public DataSet getThongTinChiTietKUOCVMByID(string ID_KUOC)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID_KHEUOC", "string", ID_KUOC);
            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = DatabaseConstant.DanhSachTruyVanTinDung.INQ_CT_TDVM_KUOC.getValue();
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet getDanhSachKUOCVM(string INP_MA_TRANG_THAI_NGHIEP_VU, string INP_SO_HDTD, string INP_SO_KUOC, string INP_NGAY_NNO_TU, string INP_NGAY_NNO_DEN, string INP_NGAY_DHAN_TU, string INP_NGAY_DHAN_DEN, string INP_SO_TIEN_GN_TU, string INP_SO_TIEN_GN_DEN, string INP_SO_DU_TU, string INP_SO_DU_DEN, string INP_THOI_HAN_TU, string INP_THOI_HAN_DEN, string INP_THOI_HAN_DVI_TU, string INP_THOI_HAN_DVI_DEN, string INP_LSUAT_TU, string INP_LSUAT_DEN, string INP_MA_KHANG, string INP_TEN_KHANG, string INP_MA_GTO, string INP_SO_GTO, string INP_DIEN_THOAI, string INP_EMAIL, string INP_SANPHAM, string INP_KHUVUC, string INP_USERNAME, string MA_DVI_QLY, string sStartRow, string sEndRow)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_TRANG_THAI_NGHIEP_VU", "string", INP_MA_TRANG_THAI_NGHIEP_VU);
            LDatatable.AddParameter(ref dt, "@INP_SO_HDTD", "string", INP_SO_HDTD);
            LDatatable.AddParameter(ref dt, "@INP_SO_KUOC", "string", INP_SO_KUOC);
            LDatatable.AddParameter(ref dt, "@INP_NGAY_NNO_TU", "string", INP_NGAY_NNO_TU);
            LDatatable.AddParameter(ref dt, "@INP_NGAY_NNO_DEN", "string", INP_NGAY_NNO_DEN);
            LDatatable.AddParameter(ref dt, "@INP_NGAY_DHAN_TU", "string", INP_NGAY_DHAN_TU);
            LDatatable.AddParameter(ref dt, "@INP_NGAY_DHAN_DEN", "string", INP_NGAY_DHAN_DEN);
            LDatatable.AddParameter(ref dt, "@INP_SO_TIEN_GN_TU", "decimal", INP_SO_TIEN_GN_TU);
            LDatatable.AddParameter(ref dt, "@INP_SO_TIEN_GN_DEN", "decimal", INP_SO_TIEN_GN_DEN);
            LDatatable.AddParameter(ref dt, "@INP_SO_DU_TU", "decimal", INP_SO_DU_TU);
            LDatatable.AddParameter(ref dt, "@INP_SO_DU_DEN", "decimal", INP_SO_DU_DEN);
            LDatatable.AddParameter(ref dt, "@INP_THOI_HAN_TU", "int", INP_THOI_HAN_TU);
            LDatatable.AddParameter(ref dt, "@INP_THOI_HAN_DEN", "int", INP_THOI_HAN_DEN);
            LDatatable.AddParameter(ref dt, "@INP_THOI_HAN_DVI_TU", "string", INP_THOI_HAN_DVI_TU);
            LDatatable.AddParameter(ref dt, "@INP_THOI_HAN_DVI_DEN", "string", INP_THOI_HAN_DVI_DEN);
            LDatatable.AddParameter(ref dt, "@INP_LSUAT_TU", "decimal", INP_LSUAT_TU);
            LDatatable.AddParameter(ref dt, "@INP_LSUAT_DEN", "decimal", INP_LSUAT_DEN);
            LDatatable.AddParameter(ref dt, "@INP_MA_KHANG", "string", INP_MA_KHANG);
            LDatatable.AddParameter(ref dt, "@INP_TEN_KHANG", "string", INP_TEN_KHANG);
            LDatatable.AddParameter(ref dt, "@INP_MA_GTO", "string", INP_MA_GTO);
            LDatatable.AddParameter(ref dt, "@INP_SO_GTO", "string", INP_SO_GTO);
            LDatatable.AddParameter(ref dt, "@INP_DIEN_THOAI", "string", INP_DIEN_THOAI);
            LDatatable.AddParameter(ref dt, "@INP_EMAIL", "string", INP_EMAIL);
            LDatatable.AddParameter(ref dt, "@INP_SANPHAM", "string", INP_SANPHAM);
            LDatatable.AddParameter(ref dt, "@INP_KHUVUC", "string", INP_KHUVUC);
            LDatatable.AddParameter(ref dt, "@INP_USERNAME", "string", INP_USERNAME);
            LDatatable.AddParameter(ref dt, "@MA_DVI_QLY", "string", MA_DVI_QLY);
            LDatatable.AddParameter(ref dt, "@StartRow", "INT", sStartRow);
            LDatatable.AddParameter(ref dt, "@EndRow", "INT", sEndRow);
            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = DatabaseConstant.DanhSachTruyVanTinDung.INQ_DS_TDVM_KUOC.getValue();
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetPOPUPKUOCVM(string ID_KUOC)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID_KUOC", "string", ID_KUOC);
            request.dtThamSo = dt;
            request.inquiryName = "KUOC_GIAINGAN";
            request.objectName = DatabaseConstant.DanhSachTruyVanTinDung.INQ_CT_TDVM_GIAI_NGAN.getValue();
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        
        public int GetDanhSachKUOCVMGiaiNgan(TDVM_GIAI_NGAN obj)
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
                // Khởi tạo và gán giá trị cho request
                var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
                // Gán giá trị cho request
                request.objGiaiNgan = obj;
                request.Action = DatabaseConstant.Action.TINH_TOAN_SO_TIEN_VAY;
                request.Function = DatabaseConstant.Function.TDVM_GIAI_NGAN;
                // Lấy kết quả trả về
                TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
                // Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);
                obj = response.objGiaiNgan;
                if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                    return 0;
                else
                    return 1;
            }

        public int DanhSachKheUocViMo(ref TDVM_KHE_UOC_DSACH obj,DatabaseConstant.Action action, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objKheUocVMDS = obj;
            request.Action = action;
            request.Function = DatabaseConstant.Function.TDVM_DANH_SACH_KHE_UOC;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objKheUocVMDS;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int GetKUocById(ref TDVM_KHE_UOC objKUocVM)
        {
            //Kiem tra ket noi, service truoc khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            //Khoi tao va gan gia tri cho reques
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            request.objKheUocVM = objKUocVM;
            request.Action = DatabaseConstant.Action.GET_BY_ID;
            request.Function = DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC;
            //lay ket qua tra ve
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            //kiem tra ket qua tra ve
            Common.Utilities.ValidResponse(request,response);
            if(response!=null && response.ResponseStatus==ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                objKUocVM = response.objKheUocVM;
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public int GetKUocByMaKuoc(ref TDVM_KHE_UOC objKUocVM)
        {
            //Kiem tra ket noi, service truoc khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            //Khoi tao va gan gia tri cho reques
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            request.objKheUocVM = objKUocVM;
            request.Action = DatabaseConstant.Action.GET_BY_MA;
            request.Function = DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC;
            //lay ket qua tra ve
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            //kiem tra ket qua tra ve
            Common.Utilities.ValidResponse(request, response);
            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                objKUocVM = response.objKheUocVM;
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public int GetKHangByKuoc(ref KH_KHANG_HSO objKHang, TDVM_KHE_UOC objKUoc)
        {
            //Kiem tra ket noi
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());

            //khai bao request
            TinDungViMoRequest request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            request.objKhachHang = objKHang;
            request.objKheUocVM = objKUoc;
            request.Function = DatabaseConstant.Function.TDVM_LAY_KH_THEO_KUOC;

            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                objKHang = response.objKhachHang;
            }
            else
            {
                return 0;
            }
            return 1;
        }

        public DataSet LayTaiSanTheoIDKuoc(string sID_KUOC)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID_KHEUOC", "string", sID_KUOC);
            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = DatabaseConstant.DanhSachTruyVanTinDung.INQ_CT_TDVM_KUOC.getValue();
            request.inquiryName = "TSDB";
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public int LapKheUocDanhSach_01(DatabaseConstant.Action action, ref TDVM_KHE_UOC_DSACH obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            TinDungServiceClient client = null;
            TinDungViMoRequest request = null;
            TinDungViMoResponse response = null;
            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());

                client = TinDungServiceClient(ApplicationConstant.SystemService.TinDungService);
                request = Common.Utilities.PrepareRequest(new TinDungServiceRef.TinDungViMoRequest());
                response = new TinDungServiceRef.TinDungViMoResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.TDVM_DANH_SACH_KHE_UOC_01;
                request.Action = action;
                request.objKheUocVMDS = obj;

                // make a call to service client here
                response = client.TinDungViMo(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    obj = response.objKheUocVMDS;
                    return 1;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    client.Close();
                }

                client = null;
                request = null;
                response = null;
            }
        }

        public DataSet getThongTinChiTietKUOCVMDSByGDich(string MA_GDICH)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_GDICH", "string", MA_GDICH);
            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = DatabaseConstant.DanhSachTruyVanTinDung.INQ_CT_TDVM_LAP_KUOC_DS.getValue();
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetDuNoTheoKH(string sIDKhang)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_ID_KHANG", "string", sIDKhang);
            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = "INQ_CT_TDVM_KUOC_DU_NO_KH";
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        #endregion

        #region Giải ngân khế ước vi mô
        public int ThemMoiGiaoDichGiaiNgan(ref TDVM_GIAI_NGAN obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objGiaiNgan = obj;
            request.Action = DatabaseConstant.Action.THEM;
            request.Function = DatabaseConstant.Function.TDVM_GIAI_NGAN;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objGiaiNgan;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int SuaGiaoDichGiaiNgan(ref TDVM_GIAI_NGAN obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objGiaiNgan = obj;
            request.Action = DatabaseConstant.Action.SUA;
            request.Function = DatabaseConstant.Function.TDVM_GIAI_NGAN;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objGiaiNgan;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int XoaGiaoDichGiaiNgan(ref TDVM_GIAI_NGAN obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objGiaiNgan = obj;
            request.Action = DatabaseConstant.Action.XOA;
            request.Function = DatabaseConstant.Function.TDVM_GIAI_NGAN;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objGiaiNgan;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int DuyetGiaoDichGiaiNgan(ref TDVM_GIAI_NGAN obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objGiaiNgan = obj;
            request.Action = DatabaseConstant.Action.DUYET;
            request.Function = DatabaseConstant.Function.TDVM_GIAI_NGAN;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objGiaiNgan;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int ThoaiDuyetGiaoDichGiaiNgan(ref TDVM_GIAI_NGAN obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objGiaiNgan = obj;
            request.Action = DatabaseConstant.Action.THOAI_DUYET;
            request.Function = DatabaseConstant.Function.TDVM_GIAI_NGAN;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objGiaiNgan;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int TuChoiDuyetGiaoDichGiaiNgan(ref TDVM_GIAI_NGAN obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objGiaiNgan = obj;
            request.Action = DatabaseConstant.Action.TU_CHOI_DUYET;
            request.Function = DatabaseConstant.Function.TDVM_GIAI_NGAN;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objGiaiNgan;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int GetTienPhi(TDVM_GIAI_NGAN obj, ref decimal TienPhi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objGiaiNgan = obj;
            request.Action = DatabaseConstant.Action.TINH_TOAN;
            request.Function = DatabaseConstant.Function.TDVM_GIAI_NGAN;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objGiaiNgan;
            if (!LObject.IsNullOrEmpty(obj.DSACH_KHE_UOC) && obj.DSACH_KHE_UOC.Length > 0)
                TienPhi = obj.DSACH_KHE_UOC[0].SO_TIEN_PHI;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public DataSet GetThongTinChiTietGDichGiaiNgan(string MA_GIAODICH)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_GDICH", "string", MA_GIAODICH);
            LDatatable.AddParameter(ref dt, "@ID_KUOC", "string", "(0)");
            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = DatabaseConstant.DanhSachTruyVanTinDung.INQ_CT_TDVM_GIAI_NGAN.getValue();
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public int TinhLaiPhaiThu(ref TDVM_GIAI_NGAN obj)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objGiaiNgan = obj;
            request.Action = DatabaseConstant.Action.TINH_TOAN_DU_THU_TRONG_HAN;
            request.Function = DatabaseConstant.Function.TDVM_GIAI_NGAN;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objGiaiNgan;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }
        #endregion

        #region Lấy thông tin đơn vị
        public DataSet getDanhSachDonVi(string MaDonVi, string UserName)
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

                // Khởi tạo và gán các giá trị cho request
                TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

                TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
                DataTable dt = null;

                LDatatable.MakeParameterTable(ref dt);
                LDatatable.AddParameter(ref dt, "@INP_ID_DON_VI", "string", MaDonVi);
                LDatatable.AddParameter(ref dt, "@INP_USER_NAME", "string", UserName);
                request.dtThamSo = dt;
                request.inquiryName = "TREE_DVI_PGD_CUM";
                request.objectName = DatabaseConstant.DanhSachTruyVanTinDung.INQ_DS_TDVM_KUOC_TREE.getValue();
                // Lấy kết quả trả về
                response = ClientTruyVan.TruyVanMessage(request);

                // Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                return response.dsResult;
            }
        #endregion

        #region Gia hạn nợ

        public ApplicationConstant.ResponseStatus GiaHanNo(DatabaseConstant.Function function, DatabaseConstant.Action action, ref TDVM_GIA_HAN_NO obj, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TinDungViMoRequest request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            request.objGiaHanNo = obj;

            request.Module = DatabaseConstant.Module.TDVM;
            request.Function = function;
            request.Action = action;

            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);


            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                obj = response.objGiaHanNo;
                return response.ResponseStatus;
            }
        }

        #endregion

        #region Chuyển nợ quá nợ

        public int DanhSachChuyenNoQuaHan(ref TDVM_CHUYEN_NO_QUA_HAN obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objChuyenNoQuaHan = obj;
            request.Action = DatabaseConstant.Action.TIM_KIEM;
            request.Function = DatabaseConstant.Function.TDVM_CHUYEN_NO_QUA_HAN;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objChuyenNoQuaHan;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int TinhToanTrichLapDuPhong(ref TDVM_CHUYEN_NO_QUA_HAN obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objChuyenNoQuaHan = obj;
            request.Action = DatabaseConstant.Action.TINH_TOAN;
            request.Function = DatabaseConstant.Function.TDVM_CHUYEN_NO_QUA_HAN;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objChuyenNoQuaHan;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int ThemMoiGiaoDichChuyenNo(ref TDVM_CHUYEN_NO_QUA_HAN obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objChuyenNoQuaHan = obj;
            request.Action = DatabaseConstant.Action.THEM;
            request.Function = DatabaseConstant.Function.TDVM_CHUYEN_NO_QUA_HAN;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objChuyenNoQuaHan;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int SuaGiaoDichChuyenNo(ref TDVM_CHUYEN_NO_QUA_HAN obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objChuyenNoQuaHan = obj;
            request.Action = DatabaseConstant.Action.SUA;
            request.Function = DatabaseConstant.Function.TDVM_CHUYEN_NO_QUA_HAN;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objChuyenNoQuaHan;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int XoaGiaoDichChuyenNo(ref TDVM_CHUYEN_NO_QUA_HAN obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objChuyenNoQuaHan = obj;
            request.Action = DatabaseConstant.Action.XOA;
            request.Function = DatabaseConstant.Function.TDVM_CHUYEN_NO_QUA_HAN;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objChuyenNoQuaHan;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int DuyetGiaoDichChuyenNo(ref TDVM_CHUYEN_NO_QUA_HAN obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objChuyenNoQuaHan = obj;
            request.Action = DatabaseConstant.Action.DUYET;
            request.Function = DatabaseConstant.Function.TDVM_CHUYEN_NO_QUA_HAN;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objChuyenNoQuaHan;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int ThoaiDuyetGiaoDichChuyenNo(ref TDVM_CHUYEN_NO_QUA_HAN obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objChuyenNoQuaHan = obj;
            request.Action = DatabaseConstant.Action.THOAI_DUYET;
            request.Function = DatabaseConstant.Function.TDVM_CHUYEN_NO_QUA_HAN;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objChuyenNoQuaHan;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int TuChoiDuyetGiaoDichChuyenNo(ref TDVM_CHUYEN_NO_QUA_HAN obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objChuyenNoQuaHan = obj;
            request.Action = DatabaseConstant.Action.TU_CHOI_DUYET;
            request.Function = DatabaseConstant.Function.TDVM_CHUYEN_NO_QUA_HAN;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objChuyenNoQuaHan;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public DataSet GetThongTinChuyenNoQuaHan(string MaGiaoDich)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_GDICH", "string", MaGiaoDich);
            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = DatabaseConstant.DanhSachTruyVanTinDung.INQ_CT_TDVM_QUA_HAN.getValue();
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        #endregion

        #region Dự thu
        public int TinhToanDuThuTinDungTrongHan(ref TDVM_DU_THU obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objDuThu = obj;
            request.Action = DatabaseConstant.Action.TINH_TOAN_DU_THU_TRONG_HAN;
            request.Function = DatabaseConstant.Function.TDVM_DU_THU;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objDuThu;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int TinhToanDuThuTinDungQuaHan(ref TDVM_DU_THU obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objDuThu = obj;
            request.Action = DatabaseConstant.Action.TINH_TOAN_DU_THU_QUA_HAN;
            request.Function = DatabaseConstant.Function.TDVM_DU_THU;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objDuThu;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int ThemMoiGiaoDichDuThu(ref TDVM_DU_THU obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objDuThu = obj;
            request.Action = DatabaseConstant.Action.THEM;
            request.Function = DatabaseConstant.Function.TDVM_DU_THU;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objDuThu;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int SuaGiaoDichDuThu(ref TDVM_DU_THU obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objDuThu = obj;
            request.Action = DatabaseConstant.Action.SUA;
            request.Function = DatabaseConstant.Function.TDVM_DU_THU;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objDuThu;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int XoaGiaoDichDuThu(ref TDVM_DU_THU obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objDuThu = obj;
            request.Action = DatabaseConstant.Action.XOA;
            request.Function = DatabaseConstant.Function.TDVM_DU_THU;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objDuThu;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int DuyetGiaoDichDuThu(ref TDVM_DU_THU obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objDuThu = obj;
            request.Action = DatabaseConstant.Action.DUYET;
            request.Function = DatabaseConstant.Function.TDVM_DU_THU;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objDuThu;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int TuChoiGiaoDichDuThu(ref TDVM_DU_THU obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objDuThu = obj;
            request.Action = DatabaseConstant.Action.TU_CHOI_DUYET;
            request.Function = DatabaseConstant.Function.TDVM_DU_THU;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objDuThu;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int ThoaiDuyetGiaoDichDuThu(ref TDVM_DU_THU obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objDuThu = obj;
            request.Action = DatabaseConstant.Action.THOAI_DUYET;
            request.Function = DatabaseConstant.Function.TDVM_DU_THU;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objDuThu;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }
        public DataSet GetThongTinChiTietGDichDuThu(string MA_GIAODICH)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_GDICH", "string", MA_GIAODICH);
            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = DatabaseConstant.DanhSachTruyVanTinDung.INQ_CT_TDVM_DU_THU.getValue();
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        #endregion

        #region Điều chỉnh lãi suất
        public int TinhToanDieuChinhLSuatTinDung(ref TDVM_DIEU_CHINH_LAI_SUAT obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objDieuChinhLaiSuat = obj;
            request.Action = DatabaseConstant.Action.TINH_TOAN;
            request.Function = DatabaseConstant.Function.TDVM_DIEU_CHINH_LAI_SUAT;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objDieuChinhLaiSuat;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int ThemDieuChinhLSuatTinDung(ref TDVM_DIEU_CHINH_LAI_SUAT obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objDieuChinhLaiSuat = obj;
            request.Action = DatabaseConstant.Action.THEM;
            request.Function = DatabaseConstant.Function.TDVM_DIEU_CHINH_LAI_SUAT;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objDieuChinhLaiSuat;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int SuaDieuChinhLSuatTinDung(ref TDVM_DIEU_CHINH_LAI_SUAT obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objDieuChinhLaiSuat = obj;
            request.Action = DatabaseConstant.Action.SUA;
            request.Function = DatabaseConstant.Function.TDVM_DIEU_CHINH_LAI_SUAT;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objDieuChinhLaiSuat;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int XoaDieuChinhLSuatTinDung(ref TDVM_DIEU_CHINH_LAI_SUAT obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objDieuChinhLaiSuat = obj;
            request.Action = DatabaseConstant.Action.XOA;
            request.Function = DatabaseConstant.Function.TDVM_DIEU_CHINH_LAI_SUAT;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objDieuChinhLaiSuat;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int DuyetDieuChinhLSuatTinDung(ref TDVM_DIEU_CHINH_LAI_SUAT obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objDieuChinhLaiSuat = obj;
            request.Action = DatabaseConstant.Action.DUYET;
            request.Function = DatabaseConstant.Function.TDVM_DIEU_CHINH_LAI_SUAT;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objDieuChinhLaiSuat;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int TuChoiDieuChinhLSuatTinDung(ref TDVM_DIEU_CHINH_LAI_SUAT obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objDieuChinhLaiSuat = obj;
            request.Action = DatabaseConstant.Action.TU_CHOI_DUYET;
            request.Function = DatabaseConstant.Function.TDVM_DIEU_CHINH_LAI_SUAT;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objDieuChinhLaiSuat;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int ThoaiDuyetDieuChinhLSuatTinDung(ref TDVM_DIEU_CHINH_LAI_SUAT obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objDieuChinhLaiSuat = obj;
            request.Action = DatabaseConstant.Action.THOAI_DUYET;
            request.Function = DatabaseConstant.Function.TDVM_DIEU_CHINH_LAI_SUAT;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objDieuChinhLaiSuat;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public DataSet getThongTinChiTietGiaoDichLSuatByID(string ID_GIAO_DICH)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID_LAI_SUAT", "string", ID_GIAO_DICH);
            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = DatabaseConstant.DanhSachTruyVanTinDung.INQ_CT_TDVM_DCHINH_LSUAT.getValue();
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetDanhSachDieuChinhLSuat(string INP_MA_TRANG_THAI_NGHIEP_VU, string INP_SO_GDICH, string INP_NGAY_DCHINH_TU, string INP_NGAY_DCHINH_DEN, string INP_MA_NGUON_VON, string INP_TEN_NGUON_VON, string INP_MA_HDTD, string INP_MA_KUOC, string INP_NGAY_NHAN_NO_TU, string INP_NGAY_NHAN_NO_DEN, string INP_NGAY_DAO_HAN_TU, string INP_NGAY_DAO_HAN_DEN, string INP_SO_TIEN_GNGAN_TU, string INP_SO_TIEN_GNGAN_DEN, string INP_SO_DU_TU, string INP_SO_DU_DEN, string INP_THOI_HAN_VAY_TU, string INP_THOI_HAN_VAY_DEN, string INP_THOI_HAN_VAY_DVI_TINH_TU, string INP_THOI_HAN_VAY_DVI_TINH_DEN, string INP_LSUAT_TU, string INP_LSUAT_DEN, string INP_MA_KHANG, string INP_TEN_KHANG, string INP_MA_GTO, string INP_SO_GTO, string INP_DIEN_THOAI, string INP_EMAIL, string INP_SANPHAM, string INP_KHUVUC)
        {
            // Ki?m tra k?t n?i, server, service trư?c khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Kh?i t?o và gán các giá tr? cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_TRANG_THAI_NGHIEP_VU", "string", INP_MA_TRANG_THAI_NGHIEP_VU);
            LDatatable.AddParameter(ref dt, "@INP_SO_GDICH", "string", INP_SO_GDICH);
            LDatatable.AddParameter(ref dt, "@INP_NGAY_DCHINH_TU", "string", INP_NGAY_DCHINH_TU);
            LDatatable.AddParameter(ref dt, "@INP_NGAY_DCHINH_DEN", "string", INP_NGAY_DCHINH_DEN);
            LDatatable.AddParameter(ref dt, "@INP_MA_NGUON_VON", "string", INP_MA_NGUON_VON);
            LDatatable.AddParameter(ref dt, "@INP_TEN_NGUON_VON", "string", INP_TEN_NGUON_VON);
            LDatatable.AddParameter(ref dt, "@INP_MA_HDTD", "string", INP_MA_HDTD);
            LDatatable.AddParameter(ref dt, "@INP_MA_KUOC", "string", INP_MA_KUOC);
            LDatatable.AddParameter(ref dt, "@INP_NGAY_NHAN_NO_TU", "string", INP_NGAY_NHAN_NO_TU);
            LDatatable.AddParameter(ref dt, "@INP_NGAY_NHAN_NO_DEN", "string", INP_NGAY_NHAN_NO_DEN);
            LDatatable.AddParameter(ref dt, "@INP_NGAY_DAO_HAN_TU", "string", INP_NGAY_DAO_HAN_TU);
            LDatatable.AddParameter(ref dt, "@INP_NGAY_DAO_HAN_DEN", "string", INP_NGAY_DAO_HAN_DEN);
            LDatatable.AddParameter(ref dt, "@INP_SO_TIEN_GNGAN_TU", "decimal", INP_SO_TIEN_GNGAN_TU);
            LDatatable.AddParameter(ref dt, "@INP_SO_TIEN_GNGAN_DEN", "decimal", INP_SO_TIEN_GNGAN_DEN);
            LDatatable.AddParameter(ref dt, "@INP_SO_DU_TU", "decmal", INP_SO_DU_TU);
            LDatatable.AddParameter(ref dt, "@INP_SO_DU_DEN", "decimal", INP_SO_DU_DEN);
            LDatatable.AddParameter(ref dt, "@INP_THOI_HAN_VAY_TU", "int", INP_THOI_HAN_VAY_TU);
            LDatatable.AddParameter(ref dt, "@INP_THOI_HAN_VAY_DEN", "int", INP_THOI_HAN_VAY_DEN);
            LDatatable.AddParameter(ref dt, "@INP_THOI_HAN_VAY_DVI_TINH_TU", "string", INP_THOI_HAN_VAY_DVI_TINH_TU);
            LDatatable.AddParameter(ref dt, "@INP_THOI_HAN_VAY_DVI_TINH_DEN", "string", INP_THOI_HAN_VAY_DVI_TINH_DEN);
            LDatatable.AddParameter(ref dt, "@INP_LSUAT_TU", "decimal", INP_LSUAT_TU);
            LDatatable.AddParameter(ref dt, "@INP_LSUAT_DEN", "decimal", INP_LSUAT_DEN);
            LDatatable.AddParameter(ref dt, "@INP_MA_KHANG", "string", INP_MA_KHANG);
            LDatatable.AddParameter(ref dt, "@INP_TEN_KHANG", "string", INP_TEN_KHANG);
            LDatatable.AddParameter(ref dt, "@INP_MA_GTO", "string", INP_MA_GTO);
            LDatatable.AddParameter(ref dt, "@INP_SO_GTO", "string", INP_SO_GTO);
            LDatatable.AddParameter(ref dt, "@INP_DIEN_THOAI", "string", INP_DIEN_THOAI);
            LDatatable.AddParameter(ref dt, "@INP_EMAIL", "string", INP_EMAIL);
            LDatatable.AddParameter(ref dt, "@INP_SANPHAM", "string", INP_SANPHAM);
            LDatatable.AddParameter(ref dt, "@INP_KHUVUC", "string", INP_KHUVUC);
            request.dtThamSo = dt;
            request.inquiryName = "DANH_SACH";
            request.objectName = DatabaseConstant.DanhSachTruyVanTinDung.INQ_DS_TDVM_DCHINH_LSUAT.getValue();
            // L?y k?t qu? tr? v?
            response = ClientTruyVan.TruyVanMessage(request);

            // Ki?m tra k?t qu? tr? v?
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet getTreeViewDieuChinhLSuat(string MaDonVi,string NguoiGiaoDich)
        {
            // Ki?m tra k?t n?i, server, service trư?c khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Kh?i t?o và gán các giá tr? cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DVI_QLY", "string", MaDonVi);
            LDatatable.AddParameter(ref dt, "@USERNAME", "string", NguoiGiaoDich);
            request.dtThamSo = dt;
            request.inquiryName = "TREE_VIEW";
            request.objectName = DatabaseConstant.DanhSachTruyVanTinDung.INQ_DS_TDVM_DCHINH_LSUAT.getValue();
            // L?y k?t qu? tr? v?
            response = ClientTruyVan.TruyVanMessage(request);

            // Ki?m tra k?t qu? tr? v?
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        #endregion

        #region Hóa đơn thu tiền kỳ

        public DataSet getChiTietHoaDonThuTienTheoKhang(string listKhachHang, string ngayGD, string maSanPham="%", string ngayKyTruoc="false")
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ListKH", "STRING", listKhachHang);
            LDatatable.AddParameter(ref dt, "@NgayGD", "STRING", ngayGD);
            LDatatable.AddParameter(ref dt, "@MaSanPham", "STRING", maSanPham);
            LDatatable.AddParameter(ref dt, "@LayKyTruoc", "STRING", ngayKyTruoc);

            request.dtThamSo = dt;
            request.inquiryName = "KHACH_HANG";
            if (ClientInformation.Company.Equals("BINHKHANH")) //@CongLC sua
                request.objectName = "INQ.CT.THU_TIEN_KY_THEO_KHANG_00";
            else
                request.objectName = "INQ.CT.THU_TIEN_KY_THEO_KHANG";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response.dsResult != null && response.dsResult.Tables.Count > 0)
            {
                return response.dsResult;
            }
            return null;
        }

        public DataSet getDanhSachSoTKKhongKH(string maKhang)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_KHACH_HANG", "STRING", maKhang);

            request.dtThamSo = dt;
            request.inquiryName = "DANH_SACH";
            request.objectName = "INQ.DS.HDV.DS_SO_KHONG_KY_HAN";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response.dsResult != null && response.dsResult.Tables.Count > 0)
            {
                return response.dsResult;
            }
            return null;
        }

        public DataSet getThongTinHDTKTheoIDGiaoDich(int idGdich)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID_GDICH", "INT", idGdich.ToString());

            request.dtThamSo = dt;
            request.inquiryName = "%";
            if (ClientInformation.Company.Equals("BINHKHANH")) //@CongLC sua
                request.objectName = "INQ.CT.CT_HOA_DON_TIEN_KY_00";
            else if (ClientInformation.Company.Equals("PHUTHO")) 
                request.objectName = "INQ.CT.CT_HOA_DON_TIEN_KY_PHUTHO";
            else
                request.objectName = "INQ.CT.CT_HOA_DON_TIEN_KY";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response.dsResult != null && response.dsResult.Tables.Count > 0)
            {
                return response.dsResult;
            }
            return null;
        }

        public DataSet getThongTinKeHoach(string maKheUoc, string ngayGD, string maDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MaKUOC", "STRING", maKheUoc);
            LDatatable.AddParameter(ref dt, "@NgayGD", "STRING", ngayGD);
            LDatatable.AddParameter(ref dt, "@MaDonVi", "STRING", maDonVi);

            request.dtThamSo = dt;
            request.inquiryName = "INQ.CT.KE_HOACH_TRA_TRUOC";
            request.objectName = "INQ.CT.KE_HOACH_TRA_TRUOC";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response.dsResult != null && response.dsResult.Tables.Count > 0)
            {
                return response.dsResult;
            }
            return null;
        }

        public DataSet getThongTinKeHoachPopup(string maKheUoc, string ngayGD)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_KUOCVM", "STRING", maKheUoc);
            LDatatable.AddParameter(ref dt, "@INP_NGAY_THU", "STRING", ngayGD);

            request.dtThamSo = dt;
            request.inquiryName = "INQ.CT.KE_HOACH_POPUP";
            request.objectName = "INQ.CT.KE_HOACH_TRA_TRUOC";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response.dsResult != null && response.dsResult.Tables.Count > 0)
            {
                return response.dsResult;
            }
            return null;
        }

        public ApplicationConstant.ResponseStatus HoaDonThuTienKy(DatabaseConstant.Function function, DatabaseConstant.Action action, ref TDVM_LAP_HOA_DON_TIEN_KY obj, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KeToanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TinDungViMoRequest request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            request.objLapHoaDonTienKy = obj;
            request.Module = DatabaseConstant.Module.GDKT;
            request.Function = function;
            request.Action = action;

            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);


            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                obj = response.objLapHoaDonTienKy;
                return response.ResponseStatus;
            }
        }

        #endregion

        #region Trích lập dự phòng
        public int TinhToanTrichlapDuPhong(DatabaseConstant.Action Action, string denNgay, ref TDVM_TRICH_LAP_DU_PHONG obj)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objTrichLapDuPhong = obj;
            request.Action = Action;
            request.denNgay = denNgay;
            request.Function = DatabaseConstant.Function.TDVM_TRICH_LAP_DU_PHONG;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objTrichLapDuPhong;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        /// <summary>
        /// Thêm mới khế ước tín dụng
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="lstResponseDetail"></param>
        /// <returns></returns>
        public int ThemMoiTrichLapTinDungViMo(ref TDVM_TRICH_LAP_DU_PHONG obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objTrichLapDuPhong = obj;
            request.Action = DatabaseConstant.Action.THEM;
            request.Function = DatabaseConstant.Function.TDVM_TRICH_LAP_DU_PHONG;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objTrichLapDuPhong;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int SuaTrichLapTinDungViMo(ref TDVM_TRICH_LAP_DU_PHONG obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objTrichLapDuPhong = obj;
            request.Action = DatabaseConstant.Action.SUA;
            request.Function = DatabaseConstant.Function.TDVM_TRICH_LAP_DU_PHONG;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objTrichLapDuPhong;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int XoaTrichLapTinDungViMo(ref TDVM_TRICH_LAP_DU_PHONG obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objTrichLapDuPhong = obj;
            request.Action = DatabaseConstant.Action.XOA;
            request.Function = DatabaseConstant.Function.TDVM_TRICH_LAP_DU_PHONG;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objTrichLapDuPhong;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int DuyetTrichLapTinDungViMo(ref TDVM_TRICH_LAP_DU_PHONG obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objTrichLapDuPhong = obj;
            request.Action = DatabaseConstant.Action.DUYET;
            request.Function = DatabaseConstant.Function.TDVM_TRICH_LAP_DU_PHONG;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objTrichLapDuPhong;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int ThoaiDuyetTrichLapTinDungViMo(ref TDVM_TRICH_LAP_DU_PHONG obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objTrichLapDuPhong = obj;
            request.Action = DatabaseConstant.Action.THOAI_DUYET;
            request.Function = DatabaseConstant.Function.TDVM_TRICH_LAP_DU_PHONG;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objTrichLapDuPhong;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int TuChoiDuyetTrichLapTinDungViMo(ref TDVM_TRICH_LAP_DU_PHONG obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objTrichLapDuPhong = obj;
            request.Action = DatabaseConstant.Action.TU_CHOI_DUYET;
            request.Function = DatabaseConstant.Function.TDVM_TRICH_LAP_DU_PHONG;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objTrichLapDuPhong;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public DataSet GetThongTinTrichLapDuPhong(string MaGiaoDich)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_GDICH", "string", MaGiaoDich);
            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = DatabaseConstant.DanhSachTruyVanTinDung.INQ_CT_TDVM_TRICH_LAP.getValue();
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        #endregion

        #region Chuyen hoan nhom no

        public int DanhSachChuyenHoanNhomNo(ref TDVM_CHUYEN_HOAN_NHOM_NO obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objChuyenHoanNhomNo = obj;
            request.Action = DatabaseConstant.Action.TIM_KIEM;
            request.Function = DatabaseConstant.Function.TDVM_CHUYEN_HOAN_NHOM_NO;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objChuyenHoanNhomNo;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int TinhToanTrichLapDuPhong(ref TDVM_CHUYEN_HOAN_NHOM_NO obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objChuyenHoanNhomNo = obj;
            request.Action = DatabaseConstant.Action.TINH_TOAN;
            request.Function = DatabaseConstant.Function.TDVM_CHUYEN_HOAN_NHOM_NO;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objChuyenHoanNhomNo;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int ThemMoiGiaoDichChuyenHoanNo(ref TDVM_CHUYEN_HOAN_NHOM_NO obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objChuyenHoanNhomNo = obj;
            request.Action = DatabaseConstant.Action.THEM;
            request.Function = DatabaseConstant.Function.TDVM_CHUYEN_HOAN_NHOM_NO;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objChuyenHoanNhomNo;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int SuaGiaoDichChuyenHoanNo(ref TDVM_CHUYEN_HOAN_NHOM_NO obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objChuyenHoanNhomNo = obj;
            request.Action = DatabaseConstant.Action.SUA;
            request.Function = DatabaseConstant.Function.TDVM_CHUYEN_HOAN_NHOM_NO;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objChuyenHoanNhomNo;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int XoaGiaoDichChuyenHoanNo(ref TDVM_CHUYEN_HOAN_NHOM_NO obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objChuyenHoanNhomNo = obj;
            request.Action = DatabaseConstant.Action.XOA;
            request.Function = DatabaseConstant.Function.TDVM_CHUYEN_HOAN_NHOM_NO;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objChuyenHoanNhomNo;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int DuyetGiaoDichChuyenHoanNo(ref TDVM_CHUYEN_HOAN_NHOM_NO obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objChuyenHoanNhomNo = obj;
            request.Action = DatabaseConstant.Action.DUYET;
            request.Function = DatabaseConstant.Function.TDVM_CHUYEN_HOAN_NHOM_NO;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objChuyenHoanNhomNo;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int ThoaiDuyetGiaoDichChuyenHoanNo(ref TDVM_CHUYEN_HOAN_NHOM_NO obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objChuyenHoanNhomNo = obj;
            request.Action = DatabaseConstant.Action.THOAI_DUYET;
            request.Function = DatabaseConstant.Function.TDVM_CHUYEN_HOAN_NHOM_NO;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objChuyenHoanNhomNo;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int TuChoiDuyetGiaoDichChuyenHoanNo(ref TDVM_CHUYEN_HOAN_NHOM_NO obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objChuyenHoanNhomNo = obj;
            request.Action = DatabaseConstant.Action.TU_CHOI_DUYET;
            request.Function = DatabaseConstant.Function.TDVM_CHUYEN_HOAN_NHOM_NO;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objChuyenHoanNhomNo;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public DataSet GetThongTinChuyenHoanNhomNo(string MaGiaoDich)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_GDICH", "string", MaGiaoDich);
            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = DatabaseConstant.DanhSachTruyVanTinDung.INQ_CT_TDVM_CHUYEN_HOAN.getValue();
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        #endregion

        #region Phan loai no

        public int DanhSachPhanLoaiNo(ref TDVM_PHAN_LOAI_NO obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objPhanLoaiNo = obj;
            request.Action = DatabaseConstant.Action.TIM_KIEM;
            request.Function = DatabaseConstant.Function.TDVM_PHAN_LOAI_NO;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objPhanLoaiNo;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public int PhanLoaiNo(DatabaseConstant.Action action, ref TDVM_PHAN_LOAI_NO obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            // Gán giá trị cho request
            request.objPhanLoaiNo = obj;
            request.Action = action;
            request.Function = DatabaseConstant.Function.TDVM_PHAN_LOAI_NO;
            // Lấy kết quả trả về
            TinDungViMoResponse response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            obj = response.objPhanLoaiNo;
            if (response == null || response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                return 0;
            else
                return 1;
        }

        public DataSet GetThongTinPhanLoaiNo(string MaGiaoDich)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_GDICH", "string", MaGiaoDich);
            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = "INQ.CT.TDVM_PHAN_LOAI_NO";
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        #endregion

        #region Đặt lịch thu phát vốn

        public DataSet GetThongTinDatLichThuPhatVon(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = "INQ.CT.TDVM_LICH_THU_PHAT_VON";
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        public int LichThuPhatVon(DatabaseConstant.Action action, ref LICH_THU_PHAT_VON obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            TinDungServiceClient client = null;
            TinDungViMoRequest request = null;
            TinDungViMoResponse response = null;
            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());

                client = TinDungServiceClient(ApplicationConstant.SystemService.TinDungService);
                request = Common.Utilities.PrepareRequest(new TinDungServiceRef.TinDungViMoRequest());
                response = new TinDungServiceRef.TinDungViMoResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.TDVM_DAT_LICH_THU_PHAT_VON;
                request.Action = action;
                request.objLichThuPhatVon = obj;

                // make a call to service client here
                response = client.TinDungViMo(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    obj = response.objLichThuPhatVon;
                    return 1;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    client.Close();
                }

                client = null;
                request = null;
                response = null;
            }
        }
        #endregion

        public int LichThuPhatVonNhom(DatabaseConstant.Action action, ref LICH_THU_PHAT_VON obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            TinDungServiceClient client = null;
            TinDungViMoRequest request = null;
            TinDungViMoResponse response = null;
            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());

                client = TinDungServiceClient(ApplicationConstant.SystemService.TinDungService);
                request = Common.Utilities.PrepareRequest(new TinDungServiceRef.TinDungViMoRequest());
                response = new TinDungServiceRef.TinDungViMoResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.DC_DM_NHOM;
                request.Action = action;
                request.objLichThuPhatVon = obj;

                // make a call to service client here
                response = client.TinDungViMo(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    obj = response.objLichThuPhatVon;
                    return 1;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    client.Close();
                }

                client = null;
                request = null;
                response = null;
            }
        }

        #region Sản phẩm tín dụng

        public DataSet GetThongTinSanPhamTinDung(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = "INQ.CT.TDVM_LICH_THU_PHAT_VON";
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        public int SanPhamTinDung(DatabaseConstant.Action action, ref SAN_PHAM_TIN_DUNG obj, ref List<KT_PHAN_HE_PLOAI> lstPhanLoai, ref List<ClientResponseDetail> lstResponseDetail)
        {
            TinDungServiceClient client = null;
            TinDungViMoRequest request = null;
            TinDungViMoResponse response = null;
            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());

                client = TinDungServiceClient(ApplicationConstant.SystemService.TinDungService);
                request = Common.Utilities.PrepareRequest(new TinDungServiceRef.TinDungViMoRequest());
                response = new TinDungServiceRef.TinDungViMoResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM;
                request.Action = action;
                request.objSanPhamTDung = obj;
                request.lstPhanHePLoai = lstPhanLoai.ToArray();

                // make a call to service client here
                response = client.TinDungViMo(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    obj = response.objSanPhamTDung;
                    return 1;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    client.Close();
                }

                client = null;
                request = null;
                response = null;
            }
        }
        #endregion

        #region Đơn xin vay vốn tín dụng

        public DataSet GetThongTinDonXinVayVonTinDung(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = "INQ.CT.TDVM_DON_XIN_VAY_VON";
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetDSDonXinVayVonTinDung(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = "INQ.DS.TDVM_DON_XIN_VAY_VON";
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public int DonXinVayVon(DatabaseConstant.Action action, ref TDVM_HDTD obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            TinDungServiceClient client = null;
            TinDungViMoRequest request = null;
            TinDungViMoResponse response = null;
            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());

                client = TinDungServiceClient(ApplicationConstant.SystemService.TinDungService);
                request = Common.Utilities.PrepareRequest(new TinDungServiceRef.TinDungViMoRequest());
                response = new TinDungServiceRef.TinDungViMoResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON;
                request.Action = action;
                request.objHDTDVM = obj;

                // make a call to service client here
                response = client.TinDungViMo(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    obj = response.objHDTDVM;
                    return 1;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    client.Close();
                }

                client = null;
                request = null;
                response = null;
            }
        }
        #endregion
        #region Treeview Tín Dụng
        public DataSet GetTreeViewKhuVuc(string userName, string donViQLy)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_USER_NAME", "string", userName);
            LDatatable.AddParameter(ref dt, "@INP_MA_DVI", "string", donViQLy);
            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = "INQ.TREE.KHU_VUC_TDVM";
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetTreeViewCum(string userName, string donViQLy)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_USER_NAME", "string", userName);
            LDatatable.AddParameter(ref dt, "@INP_MA_DVI", "string", donViQLy);
            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = "INQ.TREE.CUM_TDVM";
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        #endregion

        #region Xu ly no 
        public int XuLyNo(ref TDVM_XY_LY_NO objXuLyNo, ref List<ClientResponseDetail> lstResponseDetail, DatabaseConstant.Action action)
        {
            //Kiem tra ket noi, service
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            TinDungViMoRequest request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            TinDungViMoResponse response = new TinDungViMoResponse();
            request.Action = action;
            request.Function = DatabaseConstant.Function.TDVM_XU_LY_NO;
            request.objXyLyNo = objXuLyNo;
            response = ClientTinDung.TinDungViMo(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                objXuLyNo = response.objXyLyNo;
            }
            else return 0;
            return 1;
        }

        public DataSet LayTTinGDichXLN(string sMaGiaoDich)
        {            
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MaGD", "string", sMaGiaoDich);
            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = DatabaseConstant.DanhSachTruyVanTinDung.INQ_TDVM_LAY_GD_XLNO.getValue();
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;            
        }

        public DataSet LayTTinTSanXuLyNo(string maGiaoDich, string maKheUoc)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MaGD", "string", maGiaoDich);
            LDatatable.AddParameter(ref dt, "@MaKUoc", "string", maKheUoc);
            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = DatabaseConstant.DanhSachTruyVanTinDung.INQ_TDVM_LAY_GD_XLNO.getValue();
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;            
        }
        #endregion

        #region Địa bàn sản phẩm

        public DataSet GetThongTinDiaBanSanPham(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = "INQ.CT.TDVM_DIA_BAN_SAN_PHAM";
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        public int DiaBanSanPham(DatabaseConstant.Action action, ref SAN_PHAM_DBAN obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            TinDungServiceClient client = null;
            TinDungViMoRequest request = null;
            TinDungViMoResponse response = null;
            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());

                client = TinDungServiceClient(ApplicationConstant.SystemService.TinDungService);
                request = Common.Utilities.PrepareRequest(new TinDungServiceRef.TinDungViMoRequest());
                response = new TinDungServiceRef.TinDungViMoResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.TDVM_DIA_BAN_SAN_PHAM;
                request.Action = action;
                request.objSanPhamDiaBan = obj;

                // make a call to service client here
                response = client.TinDungViMo(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    return 1;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    client.Close();
                }

                client = null;
                request = null;
                response = null;
            }
        }
        #endregion

        #region Phan bo doanh thu

        public int PhanBoDoanhThu(ref TDVM_PHAN_BO_LAI_VAY objPBoDThu, ref List<ClientResponseDetail> lstClientResponseDetail, DatabaseConstant.Action action)
        {
            int iRet = 1;
            //Kiem tra ket noi client service
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            TinDungViMoResponse response = new TinDungViMoResponse();
            TinDungViMoRequest request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            request.Action = action;
            request.objPhanBoLaiVay = objPBoDThu;
            request.Function = DatabaseConstant.Function.TDVM_PHAN_BO_LAI_VAY;
            response = ClientTinDung.TinDungViMo(request);
            lstClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                objPBoDThu = response.objPhanBoLaiVay;                
            }
            else
            {
                iRet = 0;
            }

            return iRet;    
        }

        public DataSet LayThongTinGiaoDichPhanBoDoanhThu(string sMaGiaoDich)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MaGD", "string", sMaGiaoDich);
            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = DatabaseConstant.DanhSachTruyVanTinDung.INQ_TDVM_LAY_GD_PBDT.getValue();
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        #endregion

        #region Tạm ứng giải ngân

        public int TamUngGiaiNgan( DatabaseConstant.Action action, ref TDVM_TAM_UNG objTamUng, ref List<ClientResponseDetail> lstClientResponseDetail)
        {
            int iRet = 1;
            //Kiem tra ket noi client service
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            TinDungViMoResponse response = new TinDungViMoResponse();
            TinDungViMoRequest request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            request.Action = action;
            request.objTamUngGiaiNgan = objTamUng;
            request.Function = DatabaseConstant.Function.TDVM_TAM_UNG;
            response = ClientTinDung.TinDungViMo(request);
            lstClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                objTamUng = response.objTamUngGiaiNgan;
            }
            else
            {
                iRet = 0;
            }

            return iRet;
        }

        public DataSet LayThongTinGiaoDichTamUngGiaiNgan(string sMaGiaoDich)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_IDGDICH", "string", sMaGiaoDich);
            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = DatabaseConstant.DanhSachTruyVanTinDung.INQ_CT_TDVM_GD_TAM_UNG.getValue();
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        #endregion

        #region Tạm ứng giải ngân

        public int HoanUngGiaiNgan(DatabaseConstant.Action action, ref TDVM_HOAN_UNG objHoanUng, ref List<ClientResponseDetail> lstClientResponseDetail)
        {
            int iRet = 1;
            //Kiem tra ket noi client service
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            TinDungViMoResponse response = new TinDungViMoResponse();
            TinDungViMoRequest request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            request.Action = action;
            request.objHoanUngGiaiNgan = objHoanUng;
            request.Function = DatabaseConstant.Function.TDVM_HOAN_UNG;
            response = ClientTinDung.TinDungViMo(request);
            lstClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                objHoanUng = response.objHoanUngGiaiNgan;
            }
            else
            {
                iRet = 0;
            }

            return iRet;
        }

        public DataSet LayThongTinGiaoDichHoanUngGiaiNgan(string sMaGiaoDich)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_IDGDICH", "string", sMaGiaoDich);
            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = DatabaseConstant.DanhSachTruyVanTinDung.INQ_CT_TDVM_GD_HOAN_UNG.getValue();
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        #endregion

        #region Tạm ứng giải ngân

        public int LapLichThuGocLai(DatabaseConstant.Action action, ref TDVM_LICH_THU_GOC_LAI obj, ref List<ClientResponseDetail> lstClientResponseDetail)
        {
            int iRet = 1;
            //Kiem tra ket noi client service
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            TinDungViMoResponse response = new TinDungViMoResponse();
            TinDungViMoRequest request = Common.Utilities.PrepareRequest(new TinDungViMoRequest());
            request.Action = action;
            request.objLapLichThuVon = obj;
            request.Function = DatabaseConstant.Function.TDVM_LAP_LICH_THU_GOC_LAI;
            response = ClientTinDung.TinDungViMo(request);
            lstClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                obj = response.objLapLichThuVon;
            }
            else
            {
                iRet = 0;
            }

            return iRet;
        }

        #endregion

        #region Đơn xin vay vốn tín dụng vi mo

        public DataSet GetThongTinDonXinVayVonTinDungViMo(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = "INQ.CT.TDVM_DON_XIN_VAY_VON_VI_MO";
            request.typePara = "UDTT";
            request.type = "Multi";
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetDSDonXinVayVonTinDungViMo(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = "INQ.DS.TDVM_DON_XIN_VAY_VON_VI_MO";
            request.typePara = "UDTT";
            request.type = "Multi";
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public int DonXinVayVonTinDungViMo(DatabaseConstant.Action action, ref DON_XIN_VAY_VON_VI_MO obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            TinDungServiceClient client = null;
            TinDungViMoRequest request = null;
            TinDungViMoResponse response = null;
            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());

                client = TinDungServiceClient(ApplicationConstant.SystemService.TinDungService);
                request = Common.Utilities.PrepareRequest(new TinDungServiceRef.TinDungViMoRequest());
                response = new TinDungServiceRef.TinDungViMoResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON;
                request.Action = action;
                request.objDonXinVayVon = obj;

                // make a call to service client here
                response = client.TinDungViMo(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    obj = response.objDonXinVayVon;
                    return 1;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    client.Close();
                }

                client = null;
                request = null;
                response = null;
            }
        }
        #endregion

        #region Hop dong theo nhom

        public DataSet GetThongTinHopDongTinDungViMoTheoNhom(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = "INQ.CT.TDVM_HOP_DONG_TIN_DUNG_VI_MO_THEO_NHOM";
            request.typePara = "UDTT";
            request.type = "Multi";
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetDSHopDongTinDungViMoTheoNhom(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = "INQ.DS.TDVM_HOP_DONG_TIN_DUNG_VI_MO_THEO_NHOM";
            request.typePara = "UDTT";
            request.type = "Multi";
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public int HopDongTinDungViMoTheoNhom(DatabaseConstant.Action action, ref HOP_DONG_TIN_DUNG_VI_MO_NHOM obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            TinDungServiceClient client = null;
            TinDungViMoRequest request = null;
            TinDungViMoResponse response = null;
            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());

                client = TinDungServiceClient(ApplicationConstant.SystemService.TinDungService);
                request = Common.Utilities.PrepareRequest(new TinDungServiceRef.TinDungViMoRequest());
                response = new TinDungServiceRef.TinDungViMoResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.TDVM_HOP_DONG_NHOM;
                request.Action = action;
                request.objHDTDVMNhom = obj;

                // make a call to service client here
                response = client.TinDungViMo(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    obj = response.objHDTDVMNhom;
                    return 1;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    client.Close();
                }

                client = null;
                request = null;
                response = null;
            }
        }
        #endregion

        #region Dang Ky Rut Goc
        public DataSet GetDSDangKyRutGocCT(string id)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID_RGOC", "string", id);
            request.dtThamSo = dt;
            request.inquiryName = "DANH_SACH";
            request.objectName = "INQ.DS.DKY_RGOC_CT";
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        #endregion

        #region Kiểm soát rủi ro
        public int KiemSoatRuiRo(DatabaseConstant.Action action, ref List<TD_KIEM_SOAT_RR> lst, ref List<ClientResponseDetail> lstResponseDetail)
        {
            TinDungServiceClient client = null;
            TinDungViMoRequest request = null;
            TinDungViMoResponse response = null;
            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());

                client = TinDungServiceClient(ApplicationConstant.SystemService.TinDungService);
                request = Common.Utilities.PrepareRequest(new TinDungServiceRef.TinDungViMoRequest());
                response = new TinDungServiceRef.TinDungViMoResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.TDVM_KIEM_SOAT_RR;
                request.Action = action;
                request.lstKiemSoatRR = lst.ToArray();

                // make a call to service client here
                response = client.TinDungViMo(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    lst = response.lstKiemSoatRR.ToList();
                    return 1;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    client.Close();
                }

                client = null;
                request = null;
                response = null;
            }
        }

        public DataSet GetDanhSachKiemSoatRuiRo(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            TruyVanServiceClient client = null;
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);

            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            request.dtThamSo = dt;
            request.inquiryName = "DANH_SACH";
            request.objectName = "INQ.DS.TDVM_KIEM_SOAT_RR";
            request.typePara = "UDTT";
            request.type = "Multi";
            // Lấy kết quả trả về
            response = client.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        #endregion

        #region Thu gốc lãi vay
        public DataSet GetThongTinKeHoachThuGocLai(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            TruyVanServiceClient client = null;
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);

            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            request.dtThamSo = dt;
            request.inquiryName = "KE_HOACH";
            request.objectName = "INQ.CT.TDVM_KE_HOACH_THU_GOC_LAI";
            request.typePara = "UDTT";
            request.type = "Multi";
            // Lấy kết quả trả về
            response = client.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public int ThuGocLaiVayTruocHan(DatabaseConstant.Action action, ref TDVM_THU_GOC_LAI_TRUOC_HAN obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            TinDungServiceClient client = null;
            TinDungViMoRequest request = null;
            TinDungViMoResponse response = null;
            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());

                client = TinDungServiceClient(ApplicationConstant.SystemService.TinDungService);
                request = Common.Utilities.PrepareRequest(new TinDungServiceRef.TinDungViMoRequest());
                response = new TinDungServiceRef.TinDungViMoResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.TDVM_THU_GOC_LAI_TRUOC_HAN;
                request.Action = action;
                request.objThuGocLaiTruocHan = obj;

                // make a call to service client here
                response = client.TinDungViMo(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    obj = response.objThuGocLaiTruocHan;
                    return 1;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    client.Close();
                }

                client = null;
                request = null;
                response = null;
            }
        }
        #endregion

    }
}
