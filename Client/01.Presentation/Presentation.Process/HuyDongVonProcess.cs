using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Data;
using Presentation.Process.TruyVanServiceRef;
using Utilities.Common;
using Presentation.Process.HuyDongVonServiceRef;
using System.Drawing;
using System.IO;
using Presentation.Process.Common;
using System.ServiceModel.Description;

namespace Presentation.Process
{
    public class HuyDongVonProcess
    {
        /// <summary>
        /// Biến cục bộ chứa dịch vụ tiện ích của server
        /// </summary>
        private static HuyDongVonServiceClient Client { get; set; }
        private static TruyVanServiceClient ClientTruyVan { get; set; }

        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        static HuyDongVonProcess()
        {
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

            EndpointAddress endpointAddress = Common.Utilities.getEndpointAddress(ApplicationConstant.SystemService.HuyDongVonService.layGiaTri());
            BasicHttpBinding basicHttpBinding = Common.Utilities.getBasicHttpBinding(ApplicationConstant.SystemService.HuyDongVonService.layGiaTri());
            Client = new HuyDongVonServiceClient(basicHttpBinding, endpointAddress);

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

        #region Sản phẩm 
        public DataSet GetSanPhamByMa(string ma)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_SP", "STRING", ma);
            //LDatatable.AddParameter(ref dt, "", "", "");      
            request.dtThamSo = dt;
            request.objectName = "INQ.CT.HDV_SANPHAM";
            request.inquiryName = "GET_BY_MA";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Get sản phẩm theo ID, sử dụng khi chỉ lấy 1 số thông tin cần thiết để làm tăng tốc độ xử lý
        /// </summary>
        /// <param name="id">id sản phẩm cần lấy</param>
        /// <returns></returns>
        public DataSet GetSanPhamByID(int id)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_ID", "STRING", id.ToString());
            //LDatatable.AddParameter(ref dt, "", "", "");      
            request.dtThamSo = dt;
            request.objectName = "INQ.CT.HDV_SANPHAM";
            request.inquiryName = "GET_BY_ID";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Get tất cả thông tin của sản phẩm, sử dụng khi đưa các thông tin lên Form
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataSet GetThongTinSanPham(int id)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_ID", "STRING", id.ToString());
            //LDatatable.AddParameter(ref dt, "", "", "");      
            request.dtThamSo = dt;
            request.objectName = "INQ.CT.HDV_SANPHAM";
            request.inquiryName = "TTIN_CTIET";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetDanhSachSanPham(string lstMaNhomSP, string trangThaiNghiepVu,string maSanPham,string tenSanPham, 
                                                string hinhThucTraLai, string loaiLaiSuat, string tenDangNhap, string maDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_NHOM_SP", "STRING", lstMaNhomSP);
            LDatatable.AddParameter(ref dt, "@INP_MA_TRANG_THAI_NGHIEP_VU", "STRING", trangThaiNghiepVu);
            LDatatable.AddParameter(ref dt, "@INP_MA_SAN_PHAM", "STRING", maSanPham);
            LDatatable.AddParameter(ref dt, "@INP_TEN_SAN_PHAM", "STRING", tenSanPham);
            LDatatable.AddParameter(ref dt, "@INP_HINH_THUC_TRA_LAI", "STRING", hinhThucTraLai);
            LDatatable.AddParameter(ref dt, "@INP_LOAI_LAI_SUAT", "STRING", loaiLaiSuat);
            LDatatable.AddParameter(ref dt, "@INP_TEN_DANG_NHAP", "STRING", tenDangNhap);
            LDatatable.AddParameter(ref dt, "@INP_MA_DON_VI", "STRING", maDonVi);

            request.dtThamSo = dt;
            request.objectName = "INQ.DS.HDV_SANPHAM";
            request.inquiryName = "DANH_SACH";
            
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);            
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetDanhSachNhomSanPham()
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "", "", "");           

            request.dtThamSo = dt;
            request.objectName = "INQ.DS.HDV_SANPHAM";
            request.inquiryName = "DS_NHOM_SP";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetTGuiMoiKyTheoID(int id)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_ID", "STRING", id.ToString());
            //LDatatable.AddParameter(ref dt, "", "", "");      
            request.dtThamSo = dt;
            request.objectName = "INQ.CT.HDV_SANPHAM";
            request.inquiryName = "TGUI_MOI_KY";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetBieuPhi(int id)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_ID", "STRING", id.ToString());
            //LDatatable.AddParameter(ref dt, "", "", "");      
            request.dtThamSo = dt;
            request.objectName = "INQ.CT.HDV_SANPHAM";
            request.inquiryName = "BIEU_PHI";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetTaiKhoanHachToan(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
                        
            request.dtThamSo = dt;
            request.objectName = "INQ.CT.HDV_SANPHAM";
            request.inquiryName = "TAI_KHOAN_HACH_TOAN";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public bool KiemTraTonTai(string maSanPham)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_SAN_PHAM", "STRING", maSanPham);

            request.dtThamSo = dt;
            request.objectName = "INQ.CT.HDV_SANPHAM";
            request.inquiryName = "GET_SP_THEO_MA";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response.dsResult != null && response.dsResult.Tables[0].Rows.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SanPham(DatabaseConstant.Action Action, ref BL_SAN_PHAM objBLSanPham, ref List<BL_SAN_PHAM_CT> lstBLSanPhamCT, ref List<KT_PHAN_HE_PLOAI> lstPHPL,ref string sMessage)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.HuyDongVonService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            HuyDongVonRequest request = Common.Utilities.PrepareRequest(new HuyDongVonRequest());
            request.Function = DatabaseConstant.Function.HDV_SAN_PHAM;            
            request.Action = Action;
            request.objBLSanPham = objBLSanPham;
            if (lstPHPL != null) request.lstKTPhanHePhanLoai = lstPHPL.ToArray();
            if (lstBLSanPhamCT != null) request.lstBLSanPhamCT = lstBLSanPhamCT.ToArray();            
            
            HuyDongVonResponse response = Client.HuyDongVon(request);

            //Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                objBLSanPham = response.objBLSanPham;
                lstBLSanPhamCT = response.lstBLSanPhamCT != null ? response.lstBLSanPhamCT.ToList() : null;
                lstPHPL = response.lstKTPhanHePhanLoai != null ? response.lstKTPhanHePhanLoai.ToList() : null;
                return true;
            }
            else
            {
                sMessage = response.ResponseMessage;
                return false;
            }
        }

        public bool DanhSachSanPham(DatabaseConstant.Action Action, List<int> lstIDSanPham, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.HuyDongVonService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            HuyDongVonRequest request = Common.Utilities.PrepareRequest(new HuyDongVonRequest());
            request.Function = DatabaseConstant.Function.HDV_DANH_SACH_SP;
            request.Action = Action;
            request.lstIDSanPham = lstIDSanPham.ToArray();

            HuyDongVonResponse response = Client.HuyDongVon(request);

            //Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                
                return true;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return false;
            }
        }        

        
        #endregion

        #region Sổ tiền gửi
        public DataSet GetDanhSachSoTGui(string sMaDonVi, string sSanPham,string sIDCum, string sCBQL, string sTrangThaiNVu, string sSoTGui,
                                         string sTenSoTGui, string sTuNgayMoSo, string sDenNgayMoSo, string sTuNgayDHan, string sDenNgayDHan,
                                         string soDuTu, string soDuDen, string iKyHanTu, string iKyHanDen, string sKyHanDonVi, 
                                         string sMaKH, string sTenKH, string sSoCMND, string sSDT, string sEmail, string sNgayDuChi = "%",
                                         string sStartRow = "0", string sEndRow = "0")
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DON_VI", "STRING", sMaDonVi);
            LDatatable.AddParameter(ref dt, "@ID_CUM", "STRING", sIDCum);
            LDatatable.AddParameter(ref dt, "@MA_SAN_PHAM", "STRING", sSanPham);
            LDatatable.AddParameter(ref dt, "@CBQL", "STRING", sCBQL);
            LDatatable.AddParameter(ref dt, "@MA_TRANG_THAI_NGHIEP_VU", "STRING", sTrangThaiNVu);
            LDatatable.AddParameter(ref dt, "@SO_TGUI", "STRING", sSoTGui);
            LDatatable.AddParameter(ref dt, "@TEN_SO_TGUI", "STRING", sTenSoTGui);
            LDatatable.AddParameter(ref dt, "@TU_NGAY_MO_SO", "STRING", sTuNgayMoSo);
            LDatatable.AddParameter(ref dt, "@DEN_NGAY_MO_SO", "STRING", sDenNgayMoSo);
            LDatatable.AddParameter(ref dt, "@TU_NGAY_DHAN", "STRING", sTuNgayDHan);
            LDatatable.AddParameter(ref dt, "@DEN_NGAY_DHAN", "STRING", sDenNgayDHan);
            LDatatable.AddParameter(ref dt, "@SO_DU_TU", "STRING", soDuTu);
            LDatatable.AddParameter(ref dt, "@SO_DU_DEN", "STRING", soDuDen);
            LDatatable.AddParameter(ref dt, "@KY_HAN_TU", "STRING", iKyHanTu);
            LDatatable.AddParameter(ref dt, "@KY_HAN_DEN", "STRING", iKyHanDen);
            LDatatable.AddParameter(ref dt, "@KY_HAN_DON_VI", "STRING", sKyHanDonVi);
            LDatatable.AddParameter(ref dt, "@MA_KH", "STRING", sMaKH);
            LDatatable.AddParameter(ref dt, "@TEN_KH", "STRING", sTenKH);
            LDatatable.AddParameter(ref dt, "@SO_CMND", "STRING", sSoCMND);
            LDatatable.AddParameter(ref dt, "@SDT", "STRING", sSDT);
            LDatatable.AddParameter(ref dt, "@EMAIL", "STRING", sEmail);
            LDatatable.AddParameter(ref dt, "@NGAY_DCHI", "STRING", sEmail);
            LDatatable.AddParameter(ref dt, "@StartRow", "INT", sStartRow);
            LDatatable.AddParameter(ref dt, "@EndRow", "INT", sEndRow);

            request.dtThamSo = dt;
            request.objectName = "INQ.DS.HDV_MOSO.DANH_SACH";
            request.inquiryName = "%";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetDanhSachSoTGuiNhom(string sMaDonVi, string sSanPham, string sIDNhom, string sCBQL, string sTrangThaiNVu, string sSoTGui,
                                         string sTenSoTGui, string sTuNgayMoSo, string sDenNgayMoSo, string sTuNgayDHan, string sDenNgayDHan,
                                         string soDuTu, string soDuDen, string iKyHanTu, string iKyHanDen, string sKyHanDonVi,
                                         string sMaKH, string sTenKH, string sSoCMND, string sSDT, string sEmail, string sNgayDuChi = "%",
                                         string sStartRow = "0", string sEndRow = "0")
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DON_VI", "STRING", sMaDonVi);
            LDatatable.AddParameter(ref dt, "@ID_NHOM", "STRING", sIDNhom);
            LDatatable.AddParameter(ref dt, "@MA_SAN_PHAM", "STRING", sSanPham);
            LDatatable.AddParameter(ref dt, "@CBQL", "STRING", sCBQL);
            LDatatable.AddParameter(ref dt, "@MA_TRANG_THAI_NGHIEP_VU", "STRING", sTrangThaiNVu);
            LDatatable.AddParameter(ref dt, "@SO_TGUI", "STRING", sSoTGui);
            LDatatable.AddParameter(ref dt, "@TEN_SO_TGUI", "STRING", sTenSoTGui);
            LDatatable.AddParameter(ref dt, "@TU_NGAY_MO_SO", "STRING", sTuNgayMoSo);
            LDatatable.AddParameter(ref dt, "@DEN_NGAY_MO_SO", "STRING", sDenNgayMoSo);
            LDatatable.AddParameter(ref dt, "@TU_NGAY_DHAN", "STRING", sTuNgayDHan);
            LDatatable.AddParameter(ref dt, "@DEN_NGAY_DHAN", "STRING", sDenNgayDHan);
            LDatatable.AddParameter(ref dt, "@SO_DU_TU", "STRING", soDuTu);
            LDatatable.AddParameter(ref dt, "@SO_DU_DEN", "STRING", soDuDen);
            LDatatable.AddParameter(ref dt, "@KY_HAN_TU", "STRING", iKyHanTu);
            LDatatable.AddParameter(ref dt, "@KY_HAN_DEN", "STRING", iKyHanDen);
            LDatatable.AddParameter(ref dt, "@KY_HAN_DON_VI", "STRING", sKyHanDonVi);
            LDatatable.AddParameter(ref dt, "@MA_KH", "STRING", sMaKH);
            LDatatable.AddParameter(ref dt, "@TEN_KH", "STRING", sTenKH);
            LDatatable.AddParameter(ref dt, "@SO_CMND", "STRING", sSoCMND);
            LDatatable.AddParameter(ref dt, "@SDT", "STRING", sSDT);
            LDatatable.AddParameter(ref dt, "@EMAIL", "STRING", sEmail);
            LDatatable.AddParameter(ref dt, "@NGAY_DCHI", "STRING", sEmail);
            LDatatable.AddParameter(ref dt, "@StartRow", "INT", sStartRow);
            LDatatable.AddParameter(ref dt, "@EndRow", "INT", sEndRow);

            request.dtThamSo = dt;
            request.objectName = "INQ.DS.HDV_MOSO.DANH_SACH_NHOM";
            request.inquiryName = "%";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetDanhSachSoTGuiDuocGuiThem(string sMaDonVi, string sSanPham)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DVI", "STRING", sMaDonVi);
            LDatatable.AddParameter(ref dt, "@MA_SAN_PHAM", "STRING", sSanPham);

            request.dtThamSo = dt;
            request.objectName = "INQ.DS.HDV_SO_CPHEP_GTHEM";
            request.inquiryName = "DANH_SACH";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Get tất cả thông tin của sổ tiền gửi, sử dụng khi đưa các thông tin lên Form
        /// </summary>
        /// <param name="id">ID của sổ</param>
        /// <returns></returns>
        public DataSet GetThongTinSoTGui(int id)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_ID", "STRING", id.ToString());
            //LDatatable.AddParameter(ref dt, "", "", "");      
            request.dtThamSo = dt;
            request.objectName = "INQ.CT.HDV_MOSO";
            request.inquiryName = "%";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Lấy thông tin quan trọng của sổ tiền gửi, sử dụng ở các Form liên quan đến giao dịch huy động vốn(VD: Gửi thêm tiền, Tất toán, Đóng tài khoản...)
        /// </summary>
        /// <param name="id">ID của sổ</param>
        /// <returns></returns>
        public DataSet GetThongTinQTrongSoTGui(int id)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_ID", "STRING", id.ToString());
            
            request.dtThamSo = dt;
            request.objectName = "INQ.CT.HDV_MOSO";
            request.inquiryName = "TTIN_CTIET";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Lấy thông tin quan trọng của sổ tiền gửi theo số sổ truyền vào
        /// Sử dụng ở các Form liên quan đến giao dịch huy động vốn(VD: Gửi thêm tiền, Tất toán, Đóng tài khoản...) khi mà người dùng nhập số sổ vào LostFocus ra khỏi TextBox
        /// </summary>
        /// <param name="sSoTGui">số sổ tiền gửi</param>
        /// <returns></returns>
        public DataSet GetThongTinSoTGuiTheoMa(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            request.dtThamSo = dt;
            request.objectName = "INQ.CT.HDV_SO_TGUI";
            request.inquiryName = "GET_BY_MA";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Lấy thông tin quan trọng của sổ tiền gửi theo số sổ truyền vào
        /// Sử dụng ở các Form liên quan đến giao dịch huy động vốn(VD: Gửi thêm tiền, Tất toán, Đóng tài khoản...) khi mà người dùng nhập số sổ vào LostFocus ra khỏi TextBox
        /// </summary>
        /// <param name="sSoTGui">số sổ tiền gửi</param>
        /// <returns></returns>
        public DataSet GetThongTinSoTGuiTheoMaKH(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            request.dtThamSo = dt;
            request.objectName = "INQ.CT.HDV_SO_TGUI";
            request.inquiryName = "GET_BY_MA_KHANG";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }        

        public bool DanhSachSoTGui(DatabaseConstant.Action Action,List<int> lstID, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.HuyDongVonService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            HuyDongVonRequest request = Common.Utilities.PrepareRequest(new HuyDongVonRequest());
            request.Function = DatabaseConstant.Function.HDV_DANH_SACH_SO;
            List<BL_TIEN_GUI> lstBLTienGui = new List<BL_TIEN_GUI>();
            foreach (int id in lstID)
            {
                BL_TIEN_GUI objTienGui = new BL_TIEN_GUI();
                objTienGui.ID = id;
                lstBLTienGui.Add(objTienGui);
            }
            request.lstBLTienGui = lstBLTienGui.ToArray();
            request.Action = Action;          
            
            HuyDongVonResponse response = Client.HuyDongVon(request);            

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                //lstBLTienGui = response.lstBLTienGui.ToList();
                return true;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return false;
            }
        }

        public bool MoSoTietKiemQuyDinh(DatabaseConstant.Action Action, ref BL_TIEN_GUI objBLTienGui, ref List<BL_TIEN_GUI_DCSH> lstBLTienGuiDCSH,ref HDV_THONG_TIN_SO_TGUI objThongTinSoTG, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.HuyDongVonService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            HuyDongVonRequest request = Common.Utilities.PrepareRequest(new HuyDongVonRequest());

            request.Function = DatabaseConstant.Function.HDV_SO_TKQD;

            request.Action = Action;            

            request.objBLTienGui = objBLTienGui;

            request.objThongTinSoTG = objThongTinSoTG;

            if (lstBLTienGuiDCSH != null && lstBLTienGuiDCSH.Count>0)
                request.lstBLTienGuiDCSH = lstBLTienGuiDCSH.ToArray();
            
            HuyDongVonResponse response = Client.HuyDongVon(request);
            
             //Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                objBLTienGui = response.objBLTienGui;
                objThongTinSoTG = response.objThongTinSoTG;
                return true;                
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return false;
            }
        }

        public bool MoSoTietKiemKhongKyHan(DatabaseConstant.Action Action, ref BL_TIEN_GUI objBLTienGui, ref List<BL_TIEN_GUI_DCSH> lstBLTienGuiDCSH, ref HDV_THONG_TIN_SO_TGUI objThongTinSoTG, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.HuyDongVonService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            HuyDongVonRequest request = Common.Utilities.PrepareRequest(new HuyDongVonRequest());
            request.Function = DatabaseConstant.Function.HDV_SO_TKKKH;
            request.Action = Action;
            request.objBLTienGui = objBLTienGui;
            if (lstBLTienGuiDCSH != null && lstBLTienGuiDCSH.Count > 0)
                request.lstBLTienGuiDCSH = lstBLTienGuiDCSH.ToArray();

            HuyDongVonResponse response = Client.HuyDongVon(request);

            //Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                objBLTienGui = response.objBLTienGui;
                objThongTinSoTG = response.objThongTinSoTG;
                return true;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return false;
            }
        }

        public bool MoSoTietKiemCoKyHan(DatabaseConstant.Action Action, ref BL_TIEN_GUI objBLTienGui, ref List<BL_TIEN_GUI_DCSH> lstBLTienGuiDCSH, ref HDV_THONG_TIN_SO_TGUI objThongTinSoTG, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.HuyDongVonService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            HuyDongVonRequest request = Common.Utilities.PrepareRequest(new HuyDongVonRequest());
            request.Function = DatabaseConstant.Function.HDV_SO_TKCKH;
            request.Action = Action;
            request.objBLTienGui = objBLTienGui;
            if (lstBLTienGuiDCSH != null && lstBLTienGuiDCSH.Count > 0)
                request.lstBLTienGuiDCSH = lstBLTienGuiDCSH.ToArray();

            HuyDongVonResponse response = Client.HuyDongVon(request);

            //Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                objBLTienGui = response.objBLTienGui;
                objThongTinSoTG = response.objThongTinSoTG;
                return true;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return false;
            }
        }

        public bool MoTaiKhoanTienGuiThanhToan(DatabaseConstant.Action Action, ref BL_TIEN_GUI objBLTienGui, ref List<BL_TIEN_GUI_DCSH> lstBLTienGuiDCSH, ref HDV_THONG_TIN_SO_TGUI objThongTinSoTG, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.HuyDongVonService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            HuyDongVonRequest request = Common.Utilities.PrepareRequest(new HuyDongVonRequest());
            request.Function = DatabaseConstant.Function.HDV_SO_TK_TGTT;
            request.Action = Action;
            request.objBLTienGui = objBLTienGui;
            if (lstBLTienGuiDCSH != null && lstBLTienGuiDCSH.Count > 0)
                request.lstBLTienGuiDCSH = lstBLTienGuiDCSH.ToArray();

            HuyDongVonResponse response = Client.HuyDongVon(request);

            //Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                objBLTienGui = response.objBLTienGui;
                objThongTinSoTG = response.objThongTinSoTG;
                return true;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return false;
            }
        }

        public bool MoSoTienGuiCoKyHan(DatabaseConstant.Action Action, ref BL_TIEN_GUI objBLTienGui, ref List<BL_TIEN_GUI_DCSH> lstBLTienGuiDCSH, ref List<BL_TIEN_GUI_KHOACH> lstBLTienGuiKHoach, ref HDV_THONG_TIN_SO_TGUI objThongTinSoTG, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.HuyDongVonService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            HuyDongVonRequest request = Common.Utilities.PrepareRequest(new HuyDongVonRequest());
            request.Function = DatabaseConstant.Function.HDV_SO_TGCKH;
            request.Action = Action;
            request.objBLTienGui = objBLTienGui;
            if (lstBLTienGuiDCSH != null && lstBLTienGuiDCSH.Count > 0)
                request.lstBLTienGuiDCSH = lstBLTienGuiDCSH.ToArray();

            HuyDongVonResponse response = Client.HuyDongVon(request);

            //Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                objBLTienGui = response.objBLTienGui;
                objThongTinSoTG = response.objThongTinSoTG;
                return true;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return false;
            }
        }

        #endregion

        #region Gửi thêm tiền
        public bool GuiThemTheoTungSo(DatabaseConstant.Action Action,ref HDV_GUI_TIEN_THEO_SO objHDVGuiTienTheoSo,ref List<ClientResponseDetail> listClientResponseDetail)
        {            
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.HuyDongVonService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            HuyDongVonRequest request = Common.Utilities.PrepareRequest(new HuyDongVonRequest());
            request.Function = DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_SO;
            request.Action = Action;
            request.objHDVGuiTienTheoSo = objHDVGuiTienTheoSo;
            
            HuyDongVonResponse response = Client.HuyDongVon(request);

            //Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                objHDVGuiTienTheoSo = response.objHDVGuiTienTheoSo;
                return true;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return false;
            }
        }

        public bool GuiThemTheoDanhSach(DatabaseConstant.Action Action, ref HDV_GUI_TIEN_THEO_DANH_SACH objHDVGuiTienTheoDanhSach, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.HuyDongVonService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            HuyDongVonRequest request = Common.Utilities.PrepareRequest(new HuyDongVonRequest());
            request.Function = DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_DANH_SACH;
            request.Action = Action;
            request.objHDVGuiTienTheoDanhSach = objHDVGuiTienTheoDanhSach;
            
            HuyDongVonResponse response = Client.HuyDongVon(request);

            //Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                objHDVGuiTienTheoDanhSach = response.objHDVGuiTienTheoDanhSach;
                return true;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return false;
            }
        }

        public bool GuiThemTheoExcel(DatabaseConstant.Function function, DatabaseConstant.Action action, ref HDV_GUI_TIEN_THEO_EXCEL objHDVGuiTienTheoExcel, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.HuyDongVonService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            HuyDongVonRequest request = Common.Utilities.PrepareRequest(new HuyDongVonRequest());
            request.Function = function;
            request.Action = action;
            request.objHDVGuiTienTheoExcel = objHDVGuiTienTheoExcel;

            HuyDongVonResponse response = Client.HuyDongVon(request);

            //Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                objHDVGuiTienTheoExcel = response.objHDVGuiTienTheoExcel;
                return true;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return false;
            }
        }

        public DataSet GetThongTinGuiThemTheoDS(string maDonVi, string maGiaoDich)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_DVI", "STRING", maDonVi);
            LDatatable.AddParameter(ref dt, "@INP_MA_GDICH", "STRING", maGiaoDich);

            request.dtThamSo = dt;
            request.objectName = "INQ.CT.HDV_GUITHEM_THEO_DS";
            request.inquiryName = "CHUNG";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetThongTinGuiThemTheoDSExcel(string maDonVi, string maGiaoDich)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_DVI", "STRING", maDonVi);
            LDatatable.AddParameter(ref dt, "@INP_MA_GDICH", "STRING", maGiaoDich);

            request.dtThamSo = dt;
            request.objectName = "INQ.CT.HDV_GUITHEM_THEO_DS_EXCEL";
            request.inquiryName = "CHUNG";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetThongTinGuiThemTheoDSCTiet(string maDonVi, string maGiaoDich)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_DVI", "STRING", maDonVi);
            LDatatable.AddParameter(ref dt, "@INP_MA_GDICH", "STRING", maGiaoDich);

            request.dtThamSo = dt;
            request.objectName = "INQ.CT.HDV_GUITHEM_THEO_DS";
            request.inquiryName = "TTIN_CTIET";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetThongTinGuiThemTheoDSExcelCTiet(string maDonVi, string maGiaoDich)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_DVI", "STRING", maDonVi);
            LDatatable.AddParameter(ref dt, "@INP_MA_GDICH", "STRING", maGiaoDich);

            request.dtThamSo = dt;
            request.objectName = "INQ.CT.HDV_GUITHEM_THEO_DS_EXCEL";
            request.inquiryName = "TTIN_CTIET";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Get thông tin của giao dịch gửi thêm tiền lên Form
        /// </summary>
        /// <returns></returns>
        public DataSet GetThongTinGuiThemTienTheoTungSo(string maDonVi, string maGiaoDich)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_DVI", "STRING", maDonVi);
            LDatatable.AddParameter(ref dt, "@INP_MA_GDICH", "STRING", maGiaoDich);

            request.dtThamSo = dt;
            request.objectName = "INQ.CT.HDV_GTHEM_TSO";
            request.inquiryName = "TTIN_CTIET";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetTreeSanPhamGuiThem()
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "", "STRING", "");


            request.dtThamSo = dt;
            request.objectName = "INQ.DS.HDV_GTHEM.TREE";
            request.inquiryName = "TREE_SAN_PHAM";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        #endregion

        #region Rút bớt gốc
        public bool RutBotGoc(DatabaseConstant.Action Action, ref HDV_RUT_GOC_MOT_PHAN objHDVRutGocMotPhan, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.HuyDongVonService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            HuyDongVonRequest request = Common.Utilities.PrepareRequest(new HuyDongVonRequest());
            request.Function = DatabaseConstant.Function.HDV_RUT_BOT_GOC;
            request.Action = Action;
            request.objHDVRutGocMotPhan = objHDVRutGocMotPhan;
           
            HuyDongVonResponse response = Client.HuyDongVon(request);

            //Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                objHDVRutGocMotPhan = response.objHDVRutGocMotPhan;
                return true;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return false;
            }
        }

        public bool RutGocTheoDanhSach(DatabaseConstant.Action Action, ref HDV_RUT_GOC_THEO_DANH_SACH objHDVRutGocTheoDanhSach, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.HuyDongVonService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            HuyDongVonRequest request = Common.Utilities.PrepareRequest(new HuyDongVonRequest());
            request.Function = DatabaseConstant.Function.HDV_RUT_GOC_THEO_DANH_SACH;
            request.Action = Action;
            request.objHDVRutGocTheoDanhSach = objHDVRutGocTheoDanhSach;

            HuyDongVonResponse response = Client.HuyDongVon(request);

            //Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                objHDVRutGocTheoDanhSach = response.objHDVRutGocTheoDanhSach;
                return true;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return false;
            }
        }
        
        public DataSet GetThongTinRutGoc(string maDonVi, string maGiaoDich)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_DVI", "STRING", maDonVi);
            LDatatable.AddParameter(ref dt, "@INP_MA_GDICH", "STRING", maGiaoDich);
            
            request.dtThamSo = dt;
            request.objectName = "INQ.CT.HDV_RUTGOC";
            request.inquiryName = "TTIN_CTIET";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        
        public DataSet GetThongTinRutGocTheoDS(string maDonVi, string maGiaoDich)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_DVI", "STRING", maDonVi);
            LDatatable.AddParameter(ref dt, "@INP_MA_GDICH", "STRING", maGiaoDich);

            request.dtThamSo = dt;
            request.objectName = "INQ.CT.HDV_RUTGOC_THEO_DS";
            request.inquiryName = "CHUNG";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetThongTinRutGocTheoDSCTiet(string maDonVi, string maGiaoDich)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_DVI", "STRING", maDonVi);
            LDatatable.AddParameter(ref dt, "@INP_MA_GDICH", "STRING", maGiaoDich);

            request.dtThamSo = dt;
            request.objectName = "INQ.CT.HDV_RUTGOC_THEO_DS";
            request.inquiryName = "TTIN_CTIET";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        #endregion

        #region Tất toán sổ
        public bool TatToan(DatabaseConstant.Action Action, ref HDV_TAT_TOAN objHDVTatToan, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.HuyDongVonService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            HuyDongVonRequest request = Common.Utilities.PrepareRequest(new HuyDongVonRequest());
            request.Function = DatabaseConstant.Function.HDV_TAT_TOAN;
            request.Action = Action;
            request.objHDVTatToan = objHDVTatToan;
           
            HuyDongVonResponse response = Client.HuyDongVon(request);

            //Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                objHDVTatToan = response.objHDVTatToan;
                return true;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return false;
            }
        }

        public bool TatToanTheoDanhSach(DatabaseConstant.Action Action, ref HDV_TAT_TOAN_THEO_DANH_SACH objHDVTatToanTheoDanhSach, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.HuyDongVonService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            HuyDongVonRequest request = Common.Utilities.PrepareRequest(new HuyDongVonRequest());
            request.Function = DatabaseConstant.Function.HDV_TAT_TOAN_THEO_DANH_SACH;
            request.Action = Action;
            request.objHDVTatToanTheoDanhSach = objHDVTatToanTheoDanhSach;

            HuyDongVonResponse response = Client.HuyDongVon(request);

            //Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                objHDVTatToanTheoDanhSach = response.objHDVTatToanTheoDanhSach;
                return true;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return false;
            }
        }

        /// <summary>
        /// Get thông tin của giao dịch tất toán lên Form
        /// </summary>
        /// <param name="id">ID của sổ</param>
        /// <returns></returns>
        public DataSet GetThongTinTatToan(string maDonVi, string maGiaoDich)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_DVI", "STRING", maDonVi);
            LDatatable.AddParameter(ref dt, "@INP_MA_GDICH", "STRING", maGiaoDich);

            request.dtThamSo = dt;
            request.objectName = "INQ.CT.HDV_TAT_TOAN";
            request.inquiryName = "TTIN_CTIET";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetThongTinTatToanDS(string maDonVi, string maGiaoDich)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_DVI", "STRING", maDonVi);
            LDatatable.AddParameter(ref dt, "@INP_MA_GDICH", "STRING", maGiaoDich);

            request.dtThamSo = dt;
            request.objectName = "INQ.CT.HDV_TAT_TOAN_DS";
            request.inquiryName = "CHUNG";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetThongTinTatToanDSCTiet(string maDonVi, string maGiaoDich)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_DVI", "STRING", maDonVi);
            LDatatable.AddParameter(ref dt, "@INP_MA_GDICH", "STRING", maGiaoDich);

            request.dtThamSo = dt;
            request.objectName = "INQ.CT.HDV_TAT_TOAN_DS";
            request.inquiryName = "TTIN_CTIET";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        #endregion

        #region Phong tỏa tài khoản
        public bool PhongToa(DatabaseConstant.Action Action, ref HDV_PHONG_TOA_SO_DU objHDVPhongToaSoDu, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.HuyDongVonService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            HuyDongVonRequest request = Common.Utilities.PrepareRequest(new HuyDongVonRequest());
            request.Function = DatabaseConstant.Function.HDV_PHONG_TOA_SD;
            request.Action = Action;
            request.objHDVPhongToaSoDu = objHDVPhongToaSoDu;
            
            HuyDongVonResponse response = Client.HuyDongVon(request);

            //Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                objHDVPhongToaSoDu = response.objHDVPhongToaSoDu;
                return true;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return false;
            }
        }
        #endregion

        #region Giải toả tài khoản
        public bool GiaiToa(DatabaseConstant.Action Action, ref HDV_GIAI_TOA_SO_DU objHDVGiaiToaSoDu, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.HuyDongVonService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            HuyDongVonRequest request = Common.Utilities.PrepareRequest(new HuyDongVonRequest());
            request.Function = DatabaseConstant.Function.HDV_GIAI_TOA_SD;
            request.Action = Action;
            request.objHDVGiaiToaSoDu = objHDVGiaiToaSoDu;

            return true;
            HuyDongVonResponse response = Client.HuyDongVon(request);

            //Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                objHDVGiaiToaSoDu = response.objHDVGiaiToaSoDu;
                return true;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return false;
            }
        }
        #endregion

        #region Trả lãi
        public bool TraLai(DatabaseConstant.Action Action, ref HDV_TRA_LAI objHDVTraLai, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.HuyDongVonService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            HuyDongVonRequest request = Common.Utilities.PrepareRequest(new HuyDongVonRequest());
            request.Function = DatabaseConstant.Function.HDV_TRA_LAI;
            request.Action = Action;
            request.objHDVTraLai = objHDVTraLai;
            
            HuyDongVonResponse response = Client.HuyDongVon(request);

            //Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                objHDVTraLai = response.objHDVTraLai;
                return true;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return false;
            }
        }

        public bool TraLaiTheoDanhSach(DatabaseConstant.Action Action, ref HDV_TRA_LAI_THEO_DANH_SACH objHDVTraLaiTheoDanhSach, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.HuyDongVonService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            HuyDongVonRequest request = Common.Utilities.PrepareRequest(new HuyDongVonRequest());
            request.Function = DatabaseConstant.Function.HDV_TRA_LAI_THEO_DANH_SACH;
            request.Action = Action;
            request.objHDVTraLaiTheoDanhSach = objHDVTraLaiTheoDanhSach;

            HuyDongVonResponse response = Client.HuyDongVon(request);

            //Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                objHDVTraLaiTheoDanhSach = response.objHDVTraLaiTheoDanhSach;
                return true;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return false;
            }
        }
        
        public DataSet GetThongTinTraLai(string maDonVi, string maGiaoDich)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_DVI", "STRING", maDonVi);
            LDatatable.AddParameter(ref dt, "@INP_MA_GDICH", "STRING", maGiaoDich);

            request.dtThamSo = dt;
            request.objectName = "INQ.CT.HDV_TRA_LAI";
            request.inquiryName = "TTIN_CTIET";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetThongTinTraLaiDS(string maDonVi, string maGiaoDich)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_DVI", "STRING", maDonVi);
            LDatatable.AddParameter(ref dt, "@INP_MA_GDICH", "STRING", maGiaoDich);

            request.dtThamSo = dt;
            request.objectName = "INQ.CT.HDV_TRA_LAI_DS";
            request.inquiryName = "CHUNG";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetThongTinTraLaiDSCTiet(string maDonVi, string maGiaoDich)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_DVI", "STRING", maDonVi);
            LDatatable.AddParameter(ref dt, "@INP_MA_GDICH", "STRING", maGiaoDich);

            request.dtThamSo = dt;
            request.objectName = "INQ.CT.HDV_TRA_LAI_DS";
            request.inquiryName = "TTIN_CTIET";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        #endregion

        #region Lãi nhập gốc
        public bool LaiNhapGocTheoTungSo(DatabaseConstant.Action Action, ref HDV_LAI_NHAP_GOC_THEO_TUNG_SO objHDVLaiNhapGocTheoTungSo, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.HuyDongVonService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            HuyDongVonRequest request = Common.Utilities.PrepareRequest(new HuyDongVonRequest());
            request.Function = DatabaseConstant.Function.HDV_LAI_NHAP_GOC_THEO_SO;
            request.Action = Action;
            request.objHDVLaiNhapGocTheoTungSo = objHDVLaiNhapGocTheoTungSo;
            
            HuyDongVonResponse response = Client.HuyDongVon(request);

            //Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                objHDVLaiNhapGocTheoTungSo = response.objHDVLaiNhapGocTheoTungSo;
                return true;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return false;
            }
        }

        public bool LaiNhapGocTheoDanhSach(DatabaseConstant.Action Action, ref HDV_LAI_NHAP_GOC_THEO_DANH_SACH objHDVLaiNhapGocTheoDanhSach, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.HuyDongVonService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            HuyDongVonRequest request = Common.Utilities.PrepareRequest(new HuyDongVonRequest());
            request.Function = DatabaseConstant.Function.HDV_LAI_NHAP_GOC_THEO_DANH_SACH;
            request.Action = Action;
            request.objHDVLaiNhapGocTheoDanhSach = objHDVLaiNhapGocTheoDanhSach;
            
            HuyDongVonResponse response = Client.HuyDongVon(request);

            //Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                objHDVLaiNhapGocTheoDanhSach = response.objHDVLaiNhapGocTheoDanhSach;
                return true;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return false;
            }
        }

        public DataSet GetThongTinLNGDS(string maDonVi, string maGiaoDich)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_DVI", "STRING", maDonVi);
            LDatatable.AddParameter(ref dt, "@INP_MA_GDICH", "STRING", maGiaoDich);

            request.dtThamSo = dt;
            request.objectName = "INQ.CT.HDV_LNG_DS";
            request.inquiryName = "CHUNG";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetThongTinLNGDSCTiet(string maDonVi, string maGiaoDich)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_DVI", "STRING", maDonVi);
            LDatatable.AddParameter(ref dt, "@INP_MA_GDICH", "STRING", maGiaoDich);

            request.dtThamSo = dt;
            request.objectName = "INQ.CT.HDV_LNG_DS";
            request.inquiryName = "TTIN_CTIET";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }


        /// <summary>
        /// Get thông tin của giao dịch lãi nhập gốc theo từng sổ lên Form
        /// </summary>
        /// <returns></returns>
        public DataSet GetThongTinLaiNhapGocTheoTungSo(string maDonVi, string maGiaoDich)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_DVI", "STRING", maDonVi);
            LDatatable.AddParameter(ref dt, "@INP_MA_GDICH", "STRING", maGiaoDich);

            request.dtThamSo = dt;
            request.objectName = "INQ.CT.HDV_LAI_NHAP_GOC_TSO";
            request.inquiryName = "TTIN_CTIET";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
       
        #endregion

        #region Đóng tài khoản
        public bool DongTaiKhoan(DatabaseConstant.Action Action, ref HDV_DONG_TAI_KHOAN objHDVDongTaiKhoan, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.HuyDongVonService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            HuyDongVonRequest request = Common.Utilities.PrepareRequest(new HuyDongVonRequest());
            request.Function = DatabaseConstant.Function.HDV_DONG_TK;
            request.Action = Action;
            request.objHDVDongTaiKhoan = objHDVDongTaiKhoan;
            
            HuyDongVonResponse response = Client.HuyDongVon(request);

            //Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                objHDVDongTaiKhoan = response.objHDVDongTaiKhoan;
                return true;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return false;
            }
        }
        #endregion

        #region Mở tài khoản
        public bool MoTaiKhoan(DatabaseConstant.Action Action, ref HDV_MO_LAI_TAI_KHOAN objHDVMoLaiTaiKhoan, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.HuyDongVonService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            HuyDongVonRequest request = Common.Utilities.PrepareRequest(new HuyDongVonRequest());
            request.Function = DatabaseConstant.Function.HDV_MO_LAI_TK;
            request.Action = Action;
            request.objHDVMoLaiTaiKhoan = objHDVMoLaiTaiKhoan;
            
            HuyDongVonResponse response = Client.HuyDongVon(request);

            //Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                objHDVMoLaiTaiKhoan = response.objHDVMoLaiTaiKhoan; 
                return true;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return false;
            }
        }
        #endregion

        #region Dự chi
        public bool DuChi(DatabaseConstant.Action Action, ref HDV_DU_CHI objHDVDuChi, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.HuyDongVonService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            HuyDongVonRequest request = Common.Utilities.PrepareRequest(new HuyDongVonRequest());
            request.Function = DatabaseConstant.Function.HDV_DU_CHI;
            request.Action = Action;
            request.objHDVDuChi = objHDVDuChi;
            
            HuyDongVonResponse response = Client.HuyDongVon(request);

            //Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                objHDVDuChi = response.objHDVDuChi;
                return true;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return false;
            }
        }

        public DataSet GetThongTinDuChi(string maDonVi, string maGiaoDich)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_DVI", "STRING", maDonVi);
            LDatatable.AddParameter(ref dt, "@INP_MA_GDICH", "STRING", maGiaoDich);

            request.dtThamSo = dt;
            request.objectName = "INQ.CT.HDV_DU_CHI";
            request.inquiryName = "CHUNG";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetThongTinDuChiCTiet(string maDonVi, string maGiaoDich)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_DVI", "STRING", maDonVi);
            LDatatable.AddParameter(ref dt, "@INP_MA_GDICH", "STRING", maGiaoDich);

            request.dtThamSo = dt;
            request.objectName = "INQ.CT.HDV_DU_CHI";
            request.inquiryName = "TTIN_CTIET";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetTreeSanPhamDuChi()
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "", "STRING", "");


            request.dtThamSo = dt;
            request.objectName = "INQ.DS.HDV_DU_CHI.TREE";
            request.inquiryName = "TREE_SAN_PHAM";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        #endregion

        #region Phân bổ
        public bool PhanBo(DatabaseConstant.Action Action, ref HDV_PHAN_BO_CHI_PHI objHDVPhanBoChiPhi, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.HuyDongVonService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            HuyDongVonRequest request = Common.Utilities.PrepareRequest(new HuyDongVonRequest());
            request.Function = DatabaseConstant.Function.HDV_PHAN_BO;
            request.Action = Action;
            request.objHDVPhanBoChiPhi = objHDVPhanBoChiPhi;
            
            HuyDongVonResponse response = Client.HuyDongVon(request);

            //Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                objHDVPhanBoChiPhi = response.objHDVPhanBoChiPhi;
                return true;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return false;
            }
        }
        public DataSet GetDanhSachSoChoPhanBo(string sMaDonVi, string sSanPham, string sIDCum, string sCBQL, string sTrangThaiNVu, string sSoTGui,
                                         string sTenSoTGui, string sTuNgayMoSo, string sDenNgayMoSo, string sTuNgayDHan, string sDenNgayDHan,
                                         decimal soDuTu, decimal soDuDen, int iKyHanTu, int iKyHanDen, string sKyHanDonVi,
                                         string sMaKH, string sTenKH, string sSoCMND, string sSDT, string sEmail, string sNgayDuChi = "%")
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DON_VI", "STRING", sMaDonVi);
            LDatatable.AddParameter(ref dt, "@ID_CUM", "STRING", sIDCum);
            LDatatable.AddParameter(ref dt, "@MA_SAN_PHAM", "STRING", sSanPham);
            LDatatable.AddParameter(ref dt, "@CBQL", "STRING", sCBQL);
            LDatatable.AddParameter(ref dt, "@MA_TRANG_THAI_NGHIEP_VU", "STRING", sTrangThaiNVu);
            LDatatable.AddParameter(ref dt, "@SO_TGUI", "STRING", sSoTGui);
            LDatatable.AddParameter(ref dt, "@TEN_SO_TGUI", "STRING", sTenSoTGui);
            LDatatable.AddParameter(ref dt, "@TU_NGAY_MO_SO", "STRING", sTuNgayMoSo);
            LDatatable.AddParameter(ref dt, "@DEN_NGAY_MO_SO", "STRING", sDenNgayMoSo);
            LDatatable.AddParameter(ref dt, "@TU_NGAY_DHAN", "STRING", sTuNgayDHan);
            LDatatable.AddParameter(ref dt, "@DEN_NGAY_DHAN", "STRING", sDenNgayDHan);
            LDatatable.AddParameter(ref dt, "@SO_DU_TU", "STRING", soDuTu.ToString());
            LDatatable.AddParameter(ref dt, "@SO_DU_DEN", "STRING", soDuDen.ToString());
            LDatatable.AddParameter(ref dt, "@KY_HAN_TU", "STRING", iKyHanTu.ToString());
            LDatatable.AddParameter(ref dt, "@KY_HAN_DEN", "STRING", iKyHanDen.ToString());
            LDatatable.AddParameter(ref dt, "@KY_HAN_DON_VI", "STRING", sKyHanDonVi);
            LDatatable.AddParameter(ref dt, "@MA_KH", "STRING", sMaKH);
            LDatatable.AddParameter(ref dt, "@TEN_KH", "STRING", sTenKH);
            LDatatable.AddParameter(ref dt, "@SO_CMND", "STRING", sSoCMND);
            LDatatable.AddParameter(ref dt, "@SDT", "STRING", sSDT);
            LDatatable.AddParameter(ref dt, "@EMAIL", "STRING", sEmail);
            LDatatable.AddParameter(ref dt, "@NGAY_DCHI", "STRING", sEmail);

            request.dtThamSo = dt;
            request.objectName = "INQ.DS.HDV.SO_PHAN_BO";
            request.inquiryName = "DANH_SACH";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        public DataSet GetTTinPhanBo(string sSoTGui)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@SO_SO_TGUI", "STRING", sSoTGui);

            request.dtThamSo = dt;
            request.objectName = "INQ.CT.HDV.TTIN_PBO";
            request.inquiryName = "TTIN_CTIET";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        public DataSet GetTTinChiTietPhanBo(string maGiaoDich, string maDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_DVI", "STRING", maDonVi);
            LDatatable.AddParameter(ref dt, "@INP_MA_GDICH", "STRING", maGiaoDich);

            request.dtThamSo = dt;
            request.objectName = "INQ.DS.HDV.TTIN_PBO";
            request.inquiryName = "%";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        #endregion

        #region Thay đổi lãi suất
        public bool ThayDoiLaiSuat(DatabaseConstant.Function Function, DatabaseConstant.Action Action, ref List<HDV_THAY_DOI_LAI_SUAT> lstHDVThayDoiLaiSuat, ref HDV_THAY_DOI_LAI_SUAT objHDVThayDoiLaiSuat, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.HuyDongVonService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            HuyDongVonRequest request = Common.Utilities.PrepareRequest(new HuyDongVonRequest());
            request.Function = Function;
            request.Action = Action;
            request.objHDVThayDoiLaiSuat = objHDVThayDoiLaiSuat;
            request.lstHDVThayDoiLaiSuat = lstHDVThayDoiLaiSuat.ToArray();
            
            HuyDongVonResponse response = Client.HuyDongVon(request);

            //Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                objHDVThayDoiLaiSuat = response.objHDVThayDoiLaiSuat;
                return true;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return false;
            }
        }

        /// <summary>
        /// Lấy danh sách thay đổi lãi suất
        /// </summary>
        /// <param name="id">ID thay đổi lãi suất</param>
        /// <returns></returns>
        public DataSet GetDanhSachThayDoiLaiSuat()
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "", "", "");
            //LDatatable.AddParameter(ref dt, "", "", "");      
            request.dtThamSo = dt;
            request.objectName = "INQ.DS.HDV_THAY_DOI_LAI_SUAT";
            request.inquiryName = "DANH_SACH";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetDanhSachThayDoiLaiSuat(string maDonVi, string tuNgay, string denNgay)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DVI", "STRING", maDonVi);
            LDatatable.AddParameter(ref dt, "@TU_NGAY", "STRING", tuNgay);
            LDatatable.AddParameter(ref dt, "@DEN_NGAY", "STRING", denNgay);

            request.dtThamSo = dt;
            request.objectName = "INQ.DS.HDV_THAY_DOI_LAI_SUAT";
            request.inquiryName = "DANH_SACH";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Get tất cả thông tin của thay đổi lãi suất, sử dụng khi đưa các thông tin lên Form
        /// </summary>
        /// <param name="id">ID thay đổi lãi suất</param>
        /// <returns></returns>
        public DataSet GetThongTinThayDoiLaiSuat(int id)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID", "STRING", id.ToString());
            //LDatatable.AddParameter(ref dt, "", "", "");      
            request.dtThamSo = dt;
            request.objectName = "INQ.CT.HDV_THAY_DOI_LAI_SUAT";
            request.inquiryName = "%";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        #endregion

        #region Popup
        public DataSet GetDanhSachSoTGui(string sMaDonVi, string sSanPham, string sIDCum)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DON_VI", "STRING", sMaDonVi);
            LDatatable.AddParameter(ref dt, "@ID_CUM", "STRING", sIDCum);
            LDatatable.AddParameter(ref dt, "@MA_SAN_PHAM", "STRING", sSanPham);
           
            request.dtThamSo = dt;
            request.objectName = "INQ.DS.HDV_MOSO.POPUP";
            request.inquiryName = "POPUP";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetDanhSachSoTGuiNhom(string sMaDonVi, string sSanPham, string sIDNhom)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DON_VI", "STRING", sMaDonVi);
            LDatatable.AddParameter(ref dt, "@ID_NHOM", "STRING", sIDNhom);
            LDatatable.AddParameter(ref dt, "@MA_SAN_PHAM", "STRING", sSanPham);

            request.dtThamSo = dt;
            request.objectName = "INQ.DS.HDV_MOSO.POPUP";
            request.inquiryName = "POPUP_NHOM";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetDanhSachSoLNG(string sMaDonVi, string sSanPham, string sIDCum, string ngayGD)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DON_VI", "STRING", sMaDonVi);
            LDatatable.AddParameter(ref dt, "@ID_CUM", "STRING", sIDCum);
            LDatatable.AddParameter(ref dt, "@MA_SAN_PHAM", "STRING", sSanPham);
            LDatatable.AddParameter(ref dt, "@NGAY_GIAO_DICH", "STRING", ngayGD);

            request.dtThamSo = dt;
            request.objectName = "INQ.DS.HDV_SO_TGUI.POPUP";
            request.inquiryName = "LAI_NHAP_GOC";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetDanhSachSoDuChi(string sMaDonVi, string sSanPham, string sIDCum, string ngayGD)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DON_VI", "STRING", sMaDonVi);
            LDatatable.AddParameter(ref dt, "@ID_CUM", "STRING", sIDCum);
            LDatatable.AddParameter(ref dt, "@MA_SAN_PHAM", "STRING", sSanPham);
            LDatatable.AddParameter(ref dt, "@NGAY_GIAO_DICH", "STRING", ngayGD);

            request.dtThamSo = dt;
            request.objectName = "INQ.DS.HDV_SO_TGUI.POPUP";
            request.inquiryName = "DU_CHI";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetDanhSachSoTraLai(string sMaDonVi, string sSanPham, string sIDCum, string ngayGD)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DON_VI", "STRING", sMaDonVi);
            LDatatable.AddParameter(ref dt, "@ID_CUM", "STRING", sIDCum);
            LDatatable.AddParameter(ref dt, "@MA_SAN_PHAM", "STRING", sSanPham);
            LDatatable.AddParameter(ref dt, "@NGAY_GIAO_DICH", "STRING", ngayGD);

            request.dtThamSo = dt;
            request.objectName = "INQ.DS.HDV_SO_TGUI.POPUP";
            request.inquiryName = "TRA_LAI";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetDanhSachSoTraLaiNhom(string sMaDonVi, string sSanPham, string sIDNhom, string ngayGD)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DON_VI", "STRING", sMaDonVi);
            LDatatable.AddParameter(ref dt, "@ID_NHOM", "STRING", sIDNhom);
            LDatatable.AddParameter(ref dt, "@MA_SAN_PHAM", "STRING", sSanPham);
            LDatatable.AddParameter(ref dt, "@NGAY_GIAO_DICH", "STRING", ngayGD);

            request.dtThamSo = dt;
            request.objectName = "INQ.DS.HDV_SO_TGUI.POPUP";
            request.inquiryName = "TRA_LAI_NHOM";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetDanhSachSoTatToan(string sMaDonVi, string sSanPham, string sIDCum, string ngayGD)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DON_VI", "STRING", sMaDonVi);
            LDatatable.AddParameter(ref dt, "@ID_CUM", "STRING", sIDCum);
            LDatatable.AddParameter(ref dt, "@MA_SAN_PHAM", "STRING", sSanPham);
            LDatatable.AddParameter(ref dt, "@NGAY_GIAO_DICH", "STRING", ngayGD);

            request.dtThamSo = dt;
            request.objectName = "INQ.DS.HDV_SO_TGUI.POPUP";
            request.inquiryName = "TAT_TOAN";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetDanhSachSoTatToanNhom(string sMaDonVi, string sSanPham, string sIDNhom, string ngayGD)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DON_VI", "STRING", sMaDonVi);
            LDatatable.AddParameter(ref dt, "@ID_NHOM", "STRING", sIDNhom);
            LDatatable.AddParameter(ref dt, "@MA_SAN_PHAM", "STRING", sSanPham);
            LDatatable.AddParameter(ref dt, "@NGAY_GIAO_DICH", "STRING", ngayGD);

            request.dtThamSo = dt;
            request.objectName = "INQ.DS.HDV_SO_TGUI.POPUP";
            request.inquiryName = "TAT_TOAN_NHOM";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetDanhSachSoThayDoiLS(string sMaDonVi, string sSanPham, string sIDCum, string ngayGD)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DON_VI", "STRING", sMaDonVi);
            LDatatable.AddParameter(ref dt, "@ID_CUM", "STRING", sIDCum);
            LDatatable.AddParameter(ref dt, "@MA_SAN_PHAM", "STRING", sSanPham);
            LDatatable.AddParameter(ref dt, "@NGAY_GIAO_DICH", "STRING", ngayGD);

            request.dtThamSo = dt;
            request.objectName = "INQ.DS.HDV_SO_TGUI.POPUP";
            request.inquiryName = "THAY_DOI_LS";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetDanhSachSoRutGoc(string sMaDonVi, string sSanPham, string sIDCum, string ngayGD)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DON_VI", "STRING", sMaDonVi);
            LDatatable.AddParameter(ref dt, "@ID_CUM", "STRING", sIDCum);
            LDatatable.AddParameter(ref dt, "@MA_SAN_PHAM", "STRING", sSanPham);
            LDatatable.AddParameter(ref dt, "@NGAY_GIAO_DICH", "STRING", ngayGD);

            request.dtThamSo = dt;
            request.objectName = "INQ.DS.HDV_SO_TGUI.POPUP";
            request.inquiryName = "RUT_GOC";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetDanhSachSoRutGocNhom(string sMaDonVi, string sSanPham, string sIDNhom, string ngayGD)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DON_VI", "STRING", sMaDonVi);
            LDatatable.AddParameter(ref dt, "@ID_NHOM", "STRING", sIDNhom);
            LDatatable.AddParameter(ref dt, "@MA_SAN_PHAM", "STRING", sSanPham);
            LDatatable.AddParameter(ref dt, "@NGAY_GIAO_DICH", "STRING", ngayGD);

            request.dtThamSo = dt;
            request.objectName = "INQ.DS.HDV_SO_TGUI.POPUP";
            request.inquiryName = "RUT_GOC_NHOM";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }


        public DataSet GetTreeDonVi(string maDangNhap, string maDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            //Khởi tạo request
            DataTable dt = null;
            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DANG_NHAP", "STRING", maDangNhap);
            LDatatable.AddParameter(ref dt, "@MA_DVI_QLY", "STRING", maDonVi);

            request.dtThamSo = dt;
            request.objectName = "INQ.DS.TREE_PVI";
            request.inquiryName = "DANH_SACH";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetTreeDonVi(string sIDDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID_DON_VI", "STRING", sIDDonVi);


            request.dtThamSo = dt;
            request.objectName = "INQ.DS.HDV_SO_TGUI.TREE";
            request.inquiryName = "TREE_DVI_PGD_CUM";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        // tree don vi nhom phan theo pham vi du lieu
        public DataSet GetTreeDonViNhom(string MaDonVi, string UserName, string MaDonViCha)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            //LDatatable.AddParameter(ref dt, "@ID_DON_VI", "STRING", sIDDonVi);
            LDatatable.AddParameter(ref dt, "@INP_ID_DON_VI", "string", MaDonVi);
            LDatatable.AddParameter(ref dt, "@INP_USER_NAME", "string", UserName);
            LDatatable.AddParameter(ref dt, "@INP_MA_DVI_CHA", "string", MaDonViCha);


            request.dtThamSo = dt;
            request.objectName = "INQ.DS.HDV_SO_TGUI.TREE";
            request.inquiryName = "TREE_DVI_PGD_NHOM_NSD";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        public DataSet GetTreeDonViNhom(string sIDDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID_DON_VI", "STRING", sIDDonVi);


            request.dtThamSo = dt;
            request.objectName = "INQ.DS.HDV_SO_TGUI.TREE";
            request.inquiryName = "TREE_DVI_PGD_NHOM";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetTreeCBQL(string sMaDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DON_VI", "STRING", sMaDonVi);


            request.dtThamSo = dt;
            request.objectName = "INQ.DS.HDV_SO_TGUI.TREE";
            request.inquiryName = "TREE_CBQL";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetTreeSanPham(string maDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DON_VI", "STRING", maDonVi);


            request.dtThamSo = dt;
            request.objectName = "INQ.DS.HDV_SO_TGUI.TREE";
            request.inquiryName = "TREE_SAN_PHAM";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetTreeSanPhamRutGoc(string maDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DON_VI", "STRING", maDonVi);


            request.dtThamSo = dt;
            request.objectName = "INQ.DS.HDV_SO_TGUI.TREE";
            request.inquiryName = "TREE_SAN_PHAM_RUT_GOC";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetTreeSanPhamGuiThem(string maDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DON_VI", "STRING", maDonVi);


            request.dtThamSo = dt;
            request.objectName = "INQ.DS.HDV_SO_TGUI.TREE";
            request.inquiryName = "TREE_SAN_PHAM_GUI_THEM";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetTreeSanPhamTraLai(string maDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DON_VI", "STRING", maDonVi);


            request.dtThamSo = dt;
            request.objectName = "INQ.DS.HDV_SO_TGUI.TREE";
            request.inquiryName = "TREE_SAN_PHAM_TRA_LAI";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetTreeSanPhamDuChi(string maDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DON_VI", "STRING", maDonVi);


            request.dtThamSo = dt;
            request.objectName = "INQ.DS.HDV_SO_TGUI.TREE";
            request.inquiryName = "TREE_SAN_PHAM_DU_CHI";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetTreeSanPhamPhanBo(string maDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DON_VI", "STRING", maDonVi);


            request.dtThamSo = dt;
            request.objectName = "INQ.DS.HDV_SO_TGUI.TREE";
            request.inquiryName = "TREE_SAN_PHAM_PHAN_BO";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetTreeSanPhamThayDoiLS(string maDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DON_VI", "STRING", maDonVi);


            request.dtThamSo = dt;
            request.objectName = "INQ.DS.HDV_SO_TGUI.TREE";
            request.inquiryName = "TREE_SAN_PHAM_THAY_DOI_LS";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet getTreeView(int idDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@TrangThaiNVU", "STRING", BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri());
            LDatatable.AddParameter(ref dt, "@TrangThaiSDU", "STRING", BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri());
            LDatatable.AddParameter(ref dt, "@IdDonVi", "INT", idDonVi.ToString());

            request.dtThamSo = dt;
            request.inquiryName = "INQ.DS.TREE_KHACH_HANG";
            request.objectName = "INQ.DS.TREE_KHACH_HANG";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet getKetQuaTimKiemNangCao(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            request.dtThamSo = dt;
            //request.inquiryName = "INQ.DS.KHACH_HANG";
            request.inquiryName = "%";
            request.objectName = "INQ.DS.KHACH_HANG_TKIEM";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        #endregion

        #region Đăng ký rút gốc Phu Tho
        public bool DangKyRutGoc(DatabaseConstant.Action Action, ref HDV_THONG_TIN_DK_RUT_GOC objHDVDangKyRutGoc, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.HuyDongVonService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            HuyDongVonRequest request = Common.Utilities.PrepareRequest(new HuyDongVonRequest());
            request.Function = DatabaseConstant.Function.HDV_DANG_KY_RUT_GOC;
            request.Action = Action;
            request.objHDVDangKyRutGoc = objHDVDangKyRutGoc;

            HuyDongVonResponse response = Client.HuyDongVon(request);

            //Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                objHDVDangKyRutGoc = response.objHDVDangKyRutGoc;
                return true;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return false;
            }
        }
        #endregion

        #region Đăng ký rút gốc Quang Binh
        public bool DangKyRutGocQB(DatabaseConstant.Action Action, ref HDV_THONG_TIN_DKY_RUT_GOC objThongTinDKyRutGoc, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.HuyDongVonService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            HuyDongVonRequest request = Common.Utilities.PrepareRequest(new HuyDongVonRequest());
            request.Function = DatabaseConstant.Function.HDV_DANG_KY_RUT_GOC_CT;
            request.Action = Action;
            request.objThongTinDKyRutGoc = objThongTinDKyRutGoc;

            HuyDongVonResponse response = Client.HuyDongVon(request);

            //Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                objThongTinDKyRutGoc = response.objThongTinDKyRutGoc;
                return true;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return false;
            }
        }

        public bool DanhSachDangKyRutGocQB(DatabaseConstant.Action Action, ref List<HDV_THONG_TIN_DKY_RUT_GOC> lstThongTinDKyRutGoc, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.HuyDongVonService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            HuyDongVonRequest request = Common.Utilities.PrepareRequest(new HuyDongVonRequest());
            request.Function = DatabaseConstant.Function.HDV_DANG_KY_RUT_GOC_DS;
            request.Action = Action;
            request.lstThongTinDKyRutGoc = lstThongTinDKyRutGoc.ToArray();

            HuyDongVonResponse response = Client.HuyDongVon(request);

            //Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {

                return true;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return false;
            }
        }

        public DataSet GetDanhSachDangKyRutGocQB(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();


            request.dtThamSo = dt;
            request.objectName = "INQ.DS.DKY_RGOC";
            request.inquiryName = "DANH_SACH";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        #endregion

        public DataSet GetThongTinDonVi(string maKhachHang)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_KHANG", "STRING", maKhachHang);
            //LDatatable.AddParameter(ref dt, "", "", "");      
            request.dtThamSo = dt;
            request.objectName = "INQ.CT.TTIN_DVI_KHANG";
            request.inquiryName = "TTIN_CTIET";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
    }
}
