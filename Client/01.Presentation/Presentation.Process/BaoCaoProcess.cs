using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Utilities.Common;
using System.Data;
using Presentation.Process.Common;
using Presentation.Process.TruyVanServiceRef;
using Presentation.Process.BaoCaoServiceRef;
using System.ServiceModel.Description;

namespace Presentation.Process
{
    public class BaoCaoProcess
    {
        /// <summary>
        /// Biến cục bộ chứa dịch vụ tiện ích của server
        /// </summary>
        private static BaoCaoServiceClient Client { get; set; }
        private static TruyVanServiceClient ClientTruyVan { get; set; }

        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        static BaoCaoProcess()
        {
            //Client = new BaoCaoServiceClient();
            EndpointAddress endpointAddress = Common.Utilities.getEndpointAddress(ApplicationConstant.SystemService.BaoCaoService.layGiaTri());
            BasicHttpBinding basicHttpBinding = Common.Utilities.getBasicHttpBinding(ApplicationConstant.SystemService.BaoCaoService.layGiaTri());
            Client = new BaoCaoServiceClient(basicHttpBinding, endpointAddress);

            foreach (var operationDescription in Client.Endpoint.Contract.Operations)
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

        public List<HT_CNANG> LayDanhSachBaoCaoTheoPhanHe(string maPhanHeBaoCao)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.BaoCaoService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            BaoCaoRequest request = Common.Utilities.PrepareRequest(new BaoCaoRequest());
            request.Module = DatabaseConstant.Module.KTDL;
            request.Action = DatabaseConstant.Action.TRUY_VAN;
            request.maPhanHeBaoCao = maPhanHeBaoCao;

            // Lấy kết quả trả về
            BaoCaoResponse response = Client.LayDanhSachBaoCaoTheoPhanHe(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            return response.lstBaoCao.ToList();
        }

        public bool LayThongTinBaoCao(int idBaoCao, string maBaoCao, ref HT_BAOCAO htBaoCao, ref List<HT_BAOCAO_TSO> lstHtBaoCaoTso)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.BaoCaoService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            BaoCaoRequest request = Common.Utilities.PrepareRequest(new BaoCaoRequest());
            request.Module = DatabaseConstant.Module.KTDL;
            request.Action = DatabaseConstant.Action.TRUY_VAN;

            request.idBaoCao = idBaoCao;
            request.maBaoCao = maBaoCao;

            // Lấy kết quả trả về
            BaoCaoResponse response = Client.LayThongTinBaoCao(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            bool ret = true;
            // Lấy kết quả trả về
            if (response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                ret = true;
                htBaoCao = response.htBaoCao;
                lstHtBaoCaoTso = response.lstHtBaoCaoTso.ToList();
            }
            else
            {
                ret = false;
            }
            return ret;
        }

        public FileBase LayDuLieuBaoCao()
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.BaoCaoService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            BaoCaoRequest request = Common.Utilities.PrepareRequest(new BaoCaoRequest());
            request.Module = DatabaseConstant.Module.KTDL;
            request.Action = DatabaseConstant.Action.TRUY_VAN;
            // Lấy kết quả trả về
            BaoCaoResponse response = Client.LayDuLieuBaoCao(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            return response.duLieuBaoCao.fileDuLieuBaoCao;
        }

        public DataSet LayDuLieuBaoCaoTKe(int idBaoCao, string maBaoCao, ref BC_BIEUMAU objBieuMau, HT_BAOCAO htBaoCao, List<HT_BAOCAO_TSO> lstHtBaoCaoTso)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.BaoCaoService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            BaoCaoRequest request = Common.Utilities.PrepareRequest(new BaoCaoRequest());
            request.Module = DatabaseConstant.Module.KTDL;
            request.Action = DatabaseConstant.Action.TRUY_VAN;
            request.htBaoCao = htBaoCao;
            request.lstHtBaoCaoTso = lstHtBaoCaoTso.ToArray();
            request.idBaoCao = idBaoCao;
            request.maBaoCao = maBaoCao;
            // Lấy kết quả trả về
            BaoCaoResponse response = Client.LayDuLieuBaoCaoTKe(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            objBieuMau = response.objBieuMau;
            return response.dsDuLieuBaoCao ;
        }

        public ApplicationConstant.ResponseStatus LayDuLieu(HT_BAOCAO htBaoCao, List<HT_BAOCAO_TSO> lstHtBaoCaoTSo, ref FileBase fileBase, ref string responseMessage, DataSet ds = null, DatabaseConstant.Action Action = DatabaseConstant.Action.IN)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.BaoCaoService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            BaoCaoRequest request = Common.Utilities.PrepareRequest(new BaoCaoRequest());
            request.Module = DatabaseConstant.Module.KTDL;
            request.Action = Action;

            request.htBaoCao = htBaoCao;
            request.lstHtBaoCaoTso = lstHtBaoCaoTSo.ToArray();
            request.dsDuLieuBaoCao = ds;
            // Lấy kết quả trả về
            BaoCaoResponse response = Client.LayDuLieu(request);

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
                fileBase = response.duLieuBaoCao.fileDuLieuBaoCao;
                return response.ResponseStatus;
            }
        }

        public ApplicationConstant.ResponseStatus LayDuLieu(HT_BAOCAO htBaoCao, List<HT_BAOCAO_TSO> lstHtBaoCaoTSo, ref List<FileBase> lstFileBase, ref string responseMessage, DataSet ds = null, DatabaseConstant.Action Action = DatabaseConstant.Action.IN)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.BaoCaoService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            BaoCaoRequest request = Common.Utilities.PrepareRequest(new BaoCaoRequest());
            request.Module = DatabaseConstant.Module.KTDL;
            request.Action = Action;

            request.htBaoCao = htBaoCao;
            request.lstHtBaoCaoTso = lstHtBaoCaoTSo.ToArray();
            request.dsDuLieuBaoCao = ds;
            // Lấy kết quả trả về
            BaoCaoResponse response = Client.LayDuLieu(request);

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
                lstFileBase = response.duLieuBaoCao.lstFileDuLieuBaoCao.ToList();
                return response.ResponseStatus;
            }
        }

        public ApplicationConstant.ResponseStatus LayDuLieuVanHanhGiaoDich(
            DoiTuongBaoCao doiTuongBaoCao,
            ref FileBase fileBase, 
            ref string responseMessage)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.BaoCaoService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            VanHanhGiaoDichRequest request = Common.Utilities.PrepareRequest(new VanHanhGiaoDichRequest());
            request.Module = DatabaseConstant.Module.KTDL;
            request.Action = DatabaseConstant.Action.TRUY_VAN;

            request.objDoiTuongBaoCao = doiTuongBaoCao;
            // Lấy kết quả trả về
            VanHanhGiaoDichResponse response = Client.LayDuLieuVanHanhGiaoDich(request);

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
                fileBase = response.duLieuBaoCao.fileDuLieuBaoCao;
                return response.ResponseStatus;
            }
        }

        public ApplicationConstant.ResponseStatus LuuDuLieuDauVaoBCTK(ref List<BC_BCTK_DU_LIEU> lstBCTKDuLieu, ref string responseMessage)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.BaoCaoService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            BaoCaoRequest request = Common.Utilities.PrepareRequest(new BaoCaoRequest());
            request.Module = DatabaseConstant.Module.KTDL;
            request.Action = DatabaseConstant.Action.TRUY_VAN;

            request.lstBCTKDuLieu = lstBCTKDuLieu.ToArray();
            // Lấy kết quả trả về
            BaoCaoResponse response = Client.NhapDuLieuDauVaoBCTK(request);

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
                return response.ResponseStatus;
            }
        }

        public ApplicationConstant.ResponseStatus XuatExcel(ref FileBase fileBase, ref string responseMessage, DataTable dtParameter, DatabaseConstant.DanhSachXuatExcel loaiXuatExcel)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.BaoCaoService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            BaoCaoRequest request = Common.Utilities.PrepareRequest(new BaoCaoRequest());
            request.Module = DatabaseConstant.Module.KTDL;
            request.Action = DatabaseConstant.Action.XUAT_DU_LIEU;

            request.LoaiXuatExcel = DatabaseConstant.DanhSachXuatExcel.KHTV_DANH_SACH_KH;
            request.dtParameter = dtParameter;
            // Lấy kết quả trả về
            BaoCaoResponse response = Client.XuatExcel(request);

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
                fileBase = response.duLieuBaoCao.fileDuLieuBaoCao;
                return response.ResponseStatus;
            }
        }

        public DataSet GetSoPhuTienGui(int idKhachHang, string loaiKyHan)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID_KHANG", "STRING", idKhachHang.ToString());
            LDatatable.AddParameter(ref dt, "@LOAI_KY_HAN", "STRING", loaiKyHan);
            //LDatatable.AddParameter(ref dt, "", "", "");      
            request.dtThamSo = dt;
            request.objectName = "INQ.BC.SO_PHU_TGUI";
            request.inquiryName = "%";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);


            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetPhanLoaiTheoDonVi(string maDonVi, string capTaiKhoan)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DVI", "STRING", maDonVi);
            LDatatable.AddParameter(ref dt, "@CAP_TKHOAN", "STRING", capTaiKhoan);
            //LDatatable.AddParameter(ref dt, "", "", "");      
            request.dtThamSo = dt;
            request.objectName = "INQ.BC.TKHOAN";
            request.inquiryName = "PLOAI";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);


            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetPLoaiTKTheoDonViTienTe(string maDonVi, string maLoaiTien)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DVI_QLY", "STRING", maDonVi);
            LDatatable.AddParameter(ref dt, "@MA_LOAI_TIEN", "STRING", maLoaiTien);
            //LDatatable.AddParameter(ref dt, "", "", "");      
            request.dtThamSo = dt;
            //request.objectName = "INQ.BC.TKHOAN_THEO_TTE";
            request.objectName = "INQ.BC.TKHOAN_THEO_TTE_ALL";
            request.inquiryName = "DSACH";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);


            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Lấy danh sách phân loại tài khoản không phải KHACH_HANG từ 3 số trở lên
        /// </summary>
        /// <param name="maDonVi"></param>
        /// <returns></returns>
        public DataSet GetPhanLoaiTKTheoDonVi(string maDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DVI", "STRING", maDonVi);
            //LDatatable.AddParameter(ref dt, "", "", "");      
            request.dtThamSo = dt;
            request.objectName = "INQ.BC.TKHOAN";
            request.inquiryName = "PLOAITK";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);


            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetTaiKhoanNoiBoTheoDonVi(string maDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DVI", "STRING", maDonVi);
            //LDatatable.AddParameter(ref dt, "", "", "");      
            request.dtThamSo = dt;
            request.objectName = "INQ.BC.TKHOAN";
            request.inquiryName = "TKHOAN_NBO";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);


            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetCumTheoDonVi(string maDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DVI", "STRING", maDonVi);    
            request.dtThamSo = dt;
            request.objectName = "GET_CUM_THEO_DONVI";
            request.inquiryName = "GET_CUM_THEO_DONVI";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);


            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetSPTinDungTheoDonVi(string maDonVi, string ngayDL)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DVI", "STRING", maDonVi);
            LDatatable.AddParameter(ref dt, "@NGAY_DL", "STRING", ngayDL);
            request.dtThamSo = dt;
            request.objectName = "GET_DS_SAN_PHAM_TD";
            request.inquiryName = "DANH_SACH";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);


            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetTaiKhoanTienGuiNganHang(string maDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DVI", "STRING", maDonVi);
            request.dtThamSo = dt;
            request.objectName = "GET_DS_TAI_KHOAN_TGUI_NHANG";
            request.inquiryName = "DANH_SACH";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);


            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetKheUocKhachHang(string maKhachHang, string maSanPham)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_KHANG", "STRING", maKhachHang);
            LDatatable.AddParameter(ref dt, "@MA_SAN_PHAM", "STRING", maSanPham);
            request.dtThamSo = dt;
            request.objectName = "GET_DS_KHE_UOC_KHANG";
            request.inquiryName = "DANH_SACH";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);


            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetKhachHangTheoCum(string maCum)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_CUM", "STRING", maCum);
            request.dtThamSo = dt;
            request.objectName = "GET_DS_KHACH_HANG_THEO_CUM";
            request.inquiryName = "DANH_SACH";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);


            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetThongTinTheoCum(string maCum)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_CUM", "STRING", maCum);
            request.dtThamSo = dt;
            request.objectName = "INQ.CT.THONG_TIN_THEO_CUM";
            request.inquiryName = "TTIN_CTIET";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);


            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetKhachHangTheoNhom(string maNhom)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_NHOM", "STRING", maNhom);
            request.dtThamSo = dt;
            request.objectName = "GET_DS_KHACH_HANG_THEO_NHOM";
            if (ClientInformation.Company.Equals("BINHKHANH") || ClientInformation.Company.Equals("M7MFI"))
                request.inquiryName = "DANH_SACH";
            else
                request.inquiryName = "DANH_SACH01";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);


            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetDSKheUocTheoNhom(string maNhom)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_NHOM", "STRING", maNhom);
            request.dtThamSo = dt;
            request.objectName = "GET_DS_KHE_UOC_THEO_NHOM";
            request.inquiryName = "DANH_SACH";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);


            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetDXVVMoiTheoNhom(string maNhom)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_NHOM", "STRING", maNhom);
            request.dtThamSo = dt;
            request.objectName = "GET_DS_DXVV_MOI_THEO_NHOM";
            request.inquiryName = "DANH_SACH";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);


            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetDXVVCuTheoNhom(string maNhom)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_NHOM", "STRING", maNhom);
            request.dtThamSo = dt;
            request.objectName = "GET_DS_DXVV_CU_THEO_NHOM";
            request.inquiryName = "DANH_SACH";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);


            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetTaiKhoanCongNoTheoDonVi(string maDonVi, string maTChatCNo, string maTChatGoc)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DVI", "STRING", maDonVi);
            LDatatable.AddParameter(ref dt, "@MA_TCHAT_CNO", "STRING", maTChatCNo);
            LDatatable.AddParameter(ref dt, "@MA_TCHAT_GOC", "STRING", maTChatGoc);
            request.dtThamSo = dt;
            request.objectName = "INQ.CT.KT_TKHOAN";
            request.inquiryName = "GET_BY_TCHAT_CNO";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);


            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetDanhSachDoiTuongTaiKhoan(string soTaiKhoan)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@SO_TAI_KHOAN", "STRING", soTaiKhoan);
            request.dtThamSo = dt;
            request.objectName = "GET_DS_TKHOAN_DTUONG_THEO_TK";
            request.inquiryName = "DANH_SACH";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);


            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetTaiKhoanTienMatTheoDonVi(string maDonViQLy, string maDonViTao)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DVI_QLY", "STRING", maDonViQLy);
            LDatatable.AddParameter(ref dt, "@MA_DVI_TAO", "STRING", maDonViTao);
            //LDatatable.AddParameter(ref dt, "", "", "");      
            request.dtThamSo = dt;
            request.objectName = "INQ.BC.TKHOAN_TIENMAT";
            request.inquiryName = "TKHOAN_TIENMAT";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);


            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetTaiKhoanTienMatTheoDonVi(string maDonViQLy, string maDonViTao, string nguonVon)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DVI_QLY", "STRING", maDonViQLy);
            LDatatable.AddParameter(ref dt, "@MA_DVI_TAO", "STRING", maDonViTao);
            LDatatable.AddParameter(ref dt, "@MA_NGUON_VON", "STRING", nguonVon);
            //LDatatable.AddParameter(ref dt, "", "", "");      
            request.dtThamSo = dt;
            request.objectName = "INQ.BC.TKHOAN_TIENMAT";
            request.inquiryName = "TKHOAN_TIENMAT_THEO_NGUON";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);


            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetSoCTGSTheoDonViNgayBC(string maDonViQLy, string maDonViTao, string tuNgay, string denNgay)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DVI_QLY", "STRING", maDonViQLy);
            LDatatable.AddParameter(ref dt, "@MA_DVI_TAO", "STRING", maDonViTao);
            LDatatable.AddParameter(ref dt, "@TU_NGAY", "STRING", tuNgay);
            LDatatable.AddParameter(ref dt, "@DEN_NGAY", "STRING", denNgay);
            //LDatatable.AddParameter(ref dt, "", "", "");      
            request.dtThamSo = dt;
            request.objectName = "INQ.BC.CTU_GHI_SO";
            request.inquiryName = "CTU_GHI_SO";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);


            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetDuLieuNhapBCTK(string maDonVi, string maPhongGD, string maMauBieu, string ngayDuLieu, string ngayDauThang, string ngayCuoiThang)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_CHI_NHANH", "STRING", maDonVi);
            LDatatable.AddParameter(ref dt, "@MA_PGD", "STRING", maPhongGD);
            LDatatable.AddParameter(ref dt, "@MA_MAU_BIEU", "STRING", maMauBieu);
            LDatatable.AddParameter(ref dt, "@NGAY_DU_LIEU", "STRING", ngayDuLieu);
            LDatatable.AddParameter(ref dt, "@NGAY_DAU_THANG", "STRING", ngayDauThang);
            LDatatable.AddParameter(ref dt, "@NGAY_CUOI_THANG", "STRING", ngayCuoiThang);
            //LDatatable.AddParameter(ref dt, "", "", "");      
            request.dtThamSo = dt;
            request.objectName = "INQ.BC.NHAP_DU_LIEU_DAU_VAO";
            request.inquiryName = "BCTK";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);


            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public ApplicationConstant.ResponseStatus NhapDuLieuDauVaoBCTK(ref List<BC_BCTK_DU_LIEU> lstBCTKDuLieu)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.BaoCaoService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            BaoCaoRequest request = Common.Utilities.PrepareRequest(new BaoCaoRequest());
            request.Module = DatabaseConstant.Module.KTDL;
            request.Action = DatabaseConstant.Action.TRUY_VAN;
            request.lstBCTKDuLieu = lstBCTKDuLieu.ToArray();
            // Lấy kết quả trả về
            BaoCaoResponse response = Client.NhapDuLieuDauVaoBCTK(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            if (response == null)
            {
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            else
            {
                lstBCTKDuLieu = response.lstBCTKDuLieu.ToList();
                return response.ResponseStatus;
            }
        }

        public DataSet LayDuLieuBCSaoKeTKCKH(string MachiNhanh, string MaPhongGD, string sLoai)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_CHI_NHANH", "STRING", MachiNhanh);
            LDatatable.AddParameter(ref dt, "@MA_PHONG_GD", "STRING", MaPhongGD);            
            request.dtThamSo = dt;
            request.objectName = "INQ.DS_SPHAM_KVUC";
            request.inquiryName = sLoai;

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);


            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet LayDanhSachTKhoanTGNH(string machinhanh, string maloaitien)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MaChiNhanh", "STRING", machinhanh);
            LDatatable.AddParameter(ref dt, "@MaLoaiTien", "STRING", maloaitien);
            request.dtThamSo = dt;
            request.objectName = "INQ.DS.TKHOAN_TGNH";
            request.inquiryName = "DANH_SACH";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);


            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet LayDanhSachNhanVienTinhLuong(string magiaodich)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MaGiaoDich", "STRING", magiaodich);            
            request.dtThamSo = dt;
            request.objectName = "INQ.DS.NSTL_DSNV";
            request.inquiryName = "DANH_SACH";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);


            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet LayDanhSachNhanVienInTongHopCPLuong(string machinhanh,string maphonggd)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MaChiNhanh", "STRING", machinhanh);
            LDatatable.AddParameter(ref dt, "@MaPhongGD", "STRING", maphonggd);
            request.dtThamSo = dt;
            request.objectName = "INQ.DS.NSTL_DSNV";
            request.inquiryName = "DS_TH_CPL";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);


            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet LayDsTaiKhoanCongNo(string machinhanh, string maphonggd)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MaChiNhanh", "STRING", machinhanh);
            LDatatable.AddParameter(ref dt, "@MaPhongGD", "STRING", maphonggd);
            request.dtThamSo = dt;
            request.objectName = "INQ.DS.GDKT_TK_CONG_NO";
            request.inquiryName = "DS_TKCN";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);


            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet LayDSNhom(string sIdCum)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@IdCum", "STRING", sIdCum);            
            request.dtThamSo = dt;
            request.objectName = "INQ.DS.DS_NHOM";
            request.inquiryName = "DS_NHOM";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);


            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet LayDSCum(string sIdKhuVuc,string machinhanh,string maphonggd)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DVI_QLY", "STRING", machinhanh);
            LDatatable.AddParameter(ref dt, "@MA_DVI_TAO", "STRING", maphonggd);
            LDatatable.AddParameter(ref dt, "@ID_KHU_VUC", "STRING", sIdKhuVuc);
            request.dtThamSo = dt;
            request.objectName = "INQ.DS.CUM";
            request.inquiryName = "DANH_SACH_02";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);


            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet LayDSCum(string maphonggd)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DVI", "STRING", maphonggd);
            request.dtThamSo = dt;
            request.objectName = "INQ.DS.XA.PHUTHO";
            request.inquiryName = "DANH_SACH";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);


            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet LayChiTieuThuongDiaBan(string machinhanh,string maPhongGD, string tuNgay,string denNgay)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MaChiNhanh", "STRING", machinhanh);
            LDatatable.AddParameter(ref dt, "@MaPhongGD", "STRING", maPhongGD);
            LDatatable.AddParameter(ref dt, "@TuNgay", "STRING", tuNgay);
            LDatatable.AddParameter(ref dt, "@DenNgay", "STRING", denNgay);
            request.dtThamSo = dt;
            request.objectName = "INQ.DS.NSTL_THUONG_DBAN";
            request.inquiryName = "DANH_SACH";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);


            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet LayChiTieuThuongCanBo(string machinhanh, string maPhongGD, string tuNgay, string denNgay)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MaChiNhanh", "STRING", machinhanh);
            LDatatable.AddParameter(ref dt, "@MaPhongGD", "STRING", maPhongGD);
            LDatatable.AddParameter(ref dt, "@TuNgay", "STRING", tuNgay);
            LDatatable.AddParameter(ref dt, "@DenNgay", "STRING", denNgay);
            request.dtThamSo = dt;
            request.objectName = "INQ.DS.NSTL_THUONG_CBO";
            request.inquiryName = "DANH_SACH";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);


            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet LayPhanLoaiTienGuiKhoBac(string machinhanh)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MaChiNhanh", "STRING", machinhanh);            
            request.dtThamSo = dt;
            request.objectName = "GET_DS_TGUI_KHO_BAC";
            request.inquiryName = "DANH_SACH";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);


            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet LayDSDXVVTieuDungBIDV(int idKhachHang)
        {
            //Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanResponse response = new TruyVanResponse();
            DataTable dt = null;
            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID_KHACH_HANG", "string", idKhachHang.ToString());
            request.dtThamSo = dt;
            request.objectName = "INQ.DS_DON_XIN_VAY_VON_TIEU_DUNG";
            request.inquiryName = "DSDXVVTD";

            response = ClientTruyVan.TruyVanMessage(request);

            Common.Utilities.ValidResponse(request, response);
            return response.dsResult;
        }

        public DataSet LayDSKUocTieuDungBIDV(int idKhachHang)
        {
            //Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanResponse response = new TruyVanResponse();
            DataTable dt = null;
            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID_KHACH_HANG", "string", idKhachHang.ToString());
            request.dtThamSo = dt;
            request.objectName = "INQ.DS_HOP_DONG_KHE_UOC_TD";
            request.inquiryName = "DSKUOCTD";

            response = ClientTruyVan.TruyVanMessage(request);

            Common.Utilities.ValidResponse(request, response);
            return response.dsResult;
        }

        public DataSet LayDSKUocTDVMTheoNhomBIDV(int idNhom)
        {
            //Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanResponse response = new TruyVanResponse();
            DataTable dt = null;
            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID_NHOM", "string", idNhom.ToString());
            request.dtThamSo = dt;
            request.objectName = "INQ.DS_HOP_DONG_KHE_UOC_NHOM";
            request.inquiryName = "DSKUOCNHOM";

            response = ClientTruyVan.TruyVanMessage(request);

            Common.Utilities.ValidResponse(request, response);
            return response.dsResult;
        }

        public DataSet LayDSKyHanHDV()
        {
            //Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanResponse response = new TruyVanResponse();
            DataTable dt = null;
            LDatatable.MakeParameterTable(ref dt);
            request.dtThamSo = dt;
            request.objectName = "INQ.DS.HDV_KY_HAN";
            request.inquiryName = "DANH_SACH";

            response = ClientTruyVan.TruyVanMessage(request);

            Common.Utilities.ValidResponse(request, response);
            return response.dsResult;
        }
    }
}
