using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Utilities.Common;
using Presentation.Process.KhachHangServiceRef;
using System.Data;
using Presentation.Process.TruyVanServiceRef;
using Presentation.Process.Common;
using System.ServiceModel.Description;

namespace Presentation.Process
{
    public class KhachHangProcess
    {
        /// <summary>
        /// Biến cục bộ chứa dịch vụ tiện ích của server
        /// </summary>
        private static KhachHangServiceClient Client { get; set; }
        private static TruyVanServiceClient ClientTruyVan { get; set; }

        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        static KhachHangProcess()
        {
            //Client = new KhachHangServiceClient();
            EndpointAddress endpointAddress = Common.Utilities.getEndpointAddress(ApplicationConstant.SystemService.KhachHangService.layGiaTri());
            BasicHttpBinding basicHttpBinding = Common.Utilities.getBasicHttpBinding(ApplicationConstant.SystemService.KhachHangService.layGiaTri());
            Client = new KhachHangServiceClient(basicHttpBinding, endpointAddress);

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

        /// <summary>
        /// Lấy thông tin cơ bản hộ gia đình theo id khách hàng
        /// </summary>
        /// <param name="id">ID khách hàng</param>
        /// <returns></returns>
        public DataSet getThongTinCoBanHoGD(int id)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_ID", "INT", id.ToString());

            request.dtThamSo = dt;
            request.inquiryName = "INQ.CT.TD_VONG_VAY.TTIN_CTIET";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Lấy danh sách các khoản thu nhập
        /// </summary>
        /// <returns></returns>
        public DataSet getCacKhoanThuNhap(string ma_dmuc_loai)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DMUC_LOAI", "STRING", ma_dmuc_loai);

            request.dtThamSo = dt;
            request.inquiryName = "INQ.DS.DMUC_GTRI";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Tạo danh sách insert image
        /// </summary>
        /// <returns></returns>
        private Presentation.Process.KhachHangServiceRef.ImageBase[] getListImageInsert(DataSet ds)
        {
            Presentation.Process.KhachHangServiceRef.ImageBase[] baseImage = new Presentation.Process.KhachHangServiceRef.ImageBase[0];
            DataRow[] drHinhAnh = ds.Tables["VKH_CKY_HANH"].Select("CKHA_VI_TRI = 'CLIENT'");
            foreach (DataRow dr in drHinhAnh)
            {
                Presentation.Process.KhachHangServiceRef.ImageBase imgBase = new Presentation.Process.KhachHangServiceRef.ImageBase();
                string[] path = dr["CKHA_DUONG_DAN"].ToString().Split('\\');
                imgBase.ImageName = path[path.Length - 1];
                imgBase.ImageData = LImage.GetByteArrayFromImage(dr["CKHA_DUONG_DAN"].ToString());
                imgBase.ImageFormat = "JPG";

                Array.Resize(ref baseImage, baseImage.Length + 1);
                baseImage[baseImage.Length - 1] = imgBase;
                dr["CKHA_DUONG_DAN"] = imgBase.ImageName;
            }
            return baseImage;
        }
        
        /// <summary>
        /// Lấy toàn bộ thông tin trong kh_khang_gtri
        /// </summary>
        /// <returns></returns>
        public DataSet getThongTinKhac(string inquiryName)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "", "", "");

            request.dtThamSo = dt;
            request.inquiryName = inquiryName;
            request.objectName = "INQ.DS.VIEW_KHACH_HANG";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Lấy thông tin cụm theo id nhóm
        /// </summary>
        /// <param name="idNhom">id nhóm</param>
        /// <returns></returns>
        public DataSet getThongTinCumTheoIDNhom(string idNhom)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID", "INT", idNhom);

            request.dtThamSo = dt;
            request.inquiryName = "INQ.CT.TT_CUM_THEO_ID_NHOM";
            request.objectName = "INQ.CT.TT_CUM_THEO_ID_NHOM";

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

        public DataSet getTreeView(string maDonVi, string userName)
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
            LDatatable.AddParameter(ref dt, "@IdDonVi", "STRING", maDonVi);
            LDatatable.AddParameter(ref dt, "@UserName", "STRING", userName);

            request.dtThamSo = dt;
            request.objectName = "INQ.DS.KHACH_HANG";
            request.inquiryName = "TREE_KHACH_HANG_DS";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        /// <summary>
        /// Tìm kiếm nâng cao
        /// </summary>
        /// <param name="dt">Bảng tham số</param>
        /// <returns></returns>
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
            request.objectName = "INQ.DS.KHACH_HANG";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Lấy thông tin khách hàng theo id
        /// </summary>
        /// <param name="idKhachHang"></param>
        /// <returns></returns>
        public DataSet getThongTinKHTheoID(int idKhachHang)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_ID", "INT", idKhachHang.ToString());

            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = "INQ.CT.KHACH_HANG";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Lấy thông tin cơ bản của khách hàng theo id
        /// </summary>
        /// <param name="idKhachHang"></param>
        /// <returns></returns>
        public DataSet getThongTinCoBanKHTheoID(int idKhachHang)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_ID", "INT", idKhachHang.ToString());

            request.dtThamSo = dt;
            request.inquiryName = "KH_KHANG_HSO";
            request.objectName = "INQ.CT.KHACH_HANG";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Lấy thông tin cơ bản của khách hàng theo id
        /// </summary>
        /// <param name="idKhachHang"></param>
        /// <returns></returns>
        public DataSet getThongTinCoBanKHTheoMaKH(string maKhachHang)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA", "STRING", maKhachHang);

            request.dtThamSo = dt;
            request.inquiryName = "CHI_TIET_THEO_MA";
            request.objectName = "INQ.CT.KHACH_HANG_THEO_MA_KHANG";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Lấy thông tin cơ bản của khách hàng theo mã khách hàng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="maKhachHang"></param>
        /// <param name="idDonVi"></param>
        /// <returns></returns>
        public DataSet getThongTinCoBanKHTheoMa(int id, string maKhachHang, int idDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID_KHANG", "INT", id.ToString());
            LDatatable.AddParameter(ref dt, "@MA_KHANG", "STRING", maKhachHang);
            LDatatable.AddParameter(ref dt, "@ID_DVI", "INT", idDonVi.ToString());

            request.dtThamSo = dt;
            request.inquiryName = "INQ.CT.KHACH_HANG_THEO_MA";
            request.objectName = "INQ.CT.KHACH_HANG_THEO_MA";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Lấy thông tin chuyển địa bàn theo id giao dịch
        /// </summary>
        /// <param name="id">id giao dịch</param>
        /// <returns></returns>
        public DataTable getThongTinChuyenDiaBan(int id)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID_GD", "INT", id.ToString());

            request.dtThamSo = dt;
            request.inquiryName = "INQ.CT.CHUYEN_DIABAN";
            request.objectName = "INQ.CT.CHUYEN_DIABAN";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response.dsResult != null && response.dsResult.Tables.Count > 0)
            {
                return response.dsResult.Tables[0];
            }
            return null;
        }

        /// <summary>
        /// Danh sách các giao dịch chuyển địa bàn
        /// </summary>
        /// <param name="dt">Bảng tham số</param>
        /// <returns></returns>
        public DataSet getChuyenDiaBanDS(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            request.dtThamSo = dt;
            request.inquiryName = "INQ.DS.CHUYEN_DIABAN";
            request.objectName = "INQ.DS.CHUYEN_DIABAN";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        #region KH_KHANG_HSO
        /// <summary>
        /// Thêm mới dữ liệu
        /// </summary>
        /// <param name="obj">object khách hàng</param>
        /// <param name="ds">Dataset thuộc tính</param>
        /// <param name="idKH">id khách hàng trả về</param>
        /// <returns></returns>
        public int Them(KH_KHANG_HSO obj, DataSet ds, ref int idKH, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            KhachHangServiceRef.KhachHangRequest request = Common.Utilities.PrepareRequest(new KhachHangServiceRef.KhachHangRequest());
            KhachHangServiceRef.KhachHangResponse response = new KhachHangServiceRef.KhachHangResponse();

            request.obj = obj;
            request.dsTTinh = ds;

            request.lstHinhAnh = getListImageInsert(ds);

            // Lấy kết quả trả về
            response = Client.ThemKhachHang(request);
            idKH = response.idTraVe;
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.ketqua;
        }

        /// <summary>
        /// Sửa thông tin khách hàng
        /// </summary>
        /// <param name="obj">object khách hàng</param>
        /// <param name="ds">Dataset thuộc tính</param>
        /// <returns></returns>
        public int Sua(KH_KHANG_HSO obj, DataSet ds, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            KhachHangServiceRef.KhachHangRequest request = Common.Utilities.PrepareRequest(new KhachHangServiceRef.KhachHangRequest());
            KhachHangServiceRef.KhachHangResponse response = new KhachHangServiceRef.KhachHangResponse();
            request.obj = obj;
            request.dsTTinh = ds;
            request.lstHinhAnh = getListImageInsert(ds);
            if (request.lstHinhAnh != null && request.lstHinhAnh.Length > 0)
            {
                for (int i = 0; i < request.lstHinhAnh.Length; i++)
                {
                    request.lstHinhAnh[i].ImageName = obj.MA_KHANG + request.lstHinhAnh[i].ImageName;
                }
            }

            // Lấy kết quả trả về
            response = Client.SuaKhachHang(request);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.ketqua;
        }

        /// <summary>
        /// Xóa khách hàng theo list id 
        /// </summary>
        /// <param name="lstid"></param>
        /// <returns></returns>
        public int Xoa(int[] lstid, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            KhachHangServiceRef.KhachHangRequest request = Common.Utilities.PrepareRequest(new KhachHangServiceRef.KhachHangRequest());
            KhachHangServiceRef.KhachHangResponse response = new KhachHangServiceRef.KhachHangResponse();
            request.lstIdKhachHang = lstid;

            // Lấy kết quả trả về
            response = Client.XoaKhachHang(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            return response.ketqua;
        }

        /// <summary>
        /// Duyệt khách hàng theo list id
        /// </summary>
        /// <param name="lstid"></param>
        /// <returns></returns>
        public int Duyet(int[] lstid, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            KhachHangServiceRef.KhachHangRequest request = Common.Utilities.PrepareRequest(new KhachHangServiceRef.KhachHangRequest());
            KhachHangServiceRef.KhachHangResponse response = new KhachHangServiceRef.KhachHangResponse();
            request.lstIdKhachHang = lstid;
            request.Function = DatabaseConstant.Function.KH_THANH_VIEN;
            request.Action = DatabaseConstant.Action.DUYET;

            // Lấy kết quả trả về
            response = Client.DuyetKhachHang(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            return response.ketqua;
        }

        /// <summary>
        /// Từ chối duyệt khách hàng theo list id
        /// </summary>
        /// <param name="lstid"></param>
        /// <returns></returns>
        public int TuChoi(int[] lstid, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            KhachHangServiceRef.KhachHangRequest request = Common.Utilities.PrepareRequest(new KhachHangServiceRef.KhachHangRequest());
            KhachHangServiceRef.KhachHangResponse response = new KhachHangServiceRef.KhachHangResponse();
            request.lstIdKhachHang = lstid;
            request.Function = DatabaseConstant.Function.KH_THANH_VIEN;
            request.Action = DatabaseConstant.Action.TU_CHOI_DUYET;

            // Lấy kết quả trả về
            response = Client.TuChoiKhachHang(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            return response.ketqua;
        }

        /// <summary>
        /// Thoái duyệt khách hàng theo list id
        /// </summary>
        /// <param name="lstid"></param>
        /// <returns></returns>
        public int ThoaiDuyet(int[] lstid, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            KhachHangServiceRef.KhachHangRequest request = Common.Utilities.PrepareRequest(new KhachHangServiceRef.KhachHangRequest());
            KhachHangServiceRef.KhachHangResponse response = new KhachHangServiceRef.KhachHangResponse();
            request.lstIdKhachHang = lstid;
            request.Function = DatabaseConstant.Function.KH_THANH_VIEN;
            request.Action = DatabaseConstant.Action.THOAI_DUYET;

            // Lấy kết quả trả về
            response = Client.ThoaiDuyetKhachHang(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            return response.ketqua;
        }

        public int CheckSoCMND(string soCMND, string maKH)
        {
            int kq = 0;
            try
            {
                kq = Client.CheckSoCMND(soCMND, maKH);
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                kq = 0;
            }
            return kq;
        }

        /// <summary>
        /// Lấy thông tin từ các view khách hàng áp dụng cho Form ucKhachHangCT01.xaml
        /// </summary>
        /// <returns></returns>
        public DataSet getViewKhachHang(int idKhachHang)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID_KHANG", "STRING", idKhachHang.ToString());

            request.dtThamSo = dt;
            request.objectName = "INQ.DS.VIEW_KHACH_HANG_01";
            request.inquiryName = "%";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Lấy thông tin từ các view khách hàng áp dụng cho Form ucKhachHangCT04.xaml
        /// </summary>
        /// <returns></returns>
        public DataSet getViewKhachHang04(int idKhachHang)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID_KHANG", "STRING", idKhachHang.ToString());

            request.dtThamSo = dt;
            request.objectName = "INQ.DS.VIEW_KHACH_HANG_04";
            request.inquiryName = "%";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        
        public bool KhachHang01(DatabaseConstant.Action action, ref KH_KHANG_HSO objKhachHang, ref DataSet dsTTinh, ref List<BS_FileObject> lstDuLieuHinhAnh, ref List<ClientResponseDetail> listClientResponseDetail)
        {            
            KhachHangRequest request = null;
            KhachHangResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KhachHangService.layGiaTri());
                
                request = Common.Utilities.PrepareRequest(new KhachHangServiceRef.KhachHangRequest());
                response = new KhachHangServiceRef.KhachHangResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.KH_THANH_VIEN;
                request.Action = action;
                request.obj = objKhachHang;
                request.dsTTinh = dsTTinh;
                request.lstFileObject = lstDuLieuHinhAnh.ToArray();

                // make a call to service client here
                response = Client.KhachHang01(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objKhachHang  = response.obj;
                    dsTTinh = response.dsTTinh;
                    lstDuLieuHinhAnh = response.lstFileObject != null ? response.lstFileObject.ToList() : null;
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return true;
                }
                else
                {
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                request = null;
                response = null;
            }
        }

        public bool DanhSachKhachHang01(DatabaseConstant.Action action, ref List<KH_KHANG_HSO> lstKhachHang, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            KhachHangRequest request = null;
            KhachHangResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KhachHangService.layGiaTri());

                request = Common.Utilities.PrepareRequest(new KhachHangServiceRef.KhachHangRequest());
                response = new KhachHangServiceRef.KhachHangResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.KH_DANH_SACH;
                request.Action = action;
                if(lstKhachHang.Count>0)
                    request.lstKhachHang = lstKhachHang.ToArray();

                // make a call to service client here
                response = Client.KhachHang01(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    lstKhachHang = response.lstKhachHang.ToList();
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return true;
                }
                else
                {
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                request = null;
                response = null;
            }
        }


        /// <summary>
        /// Lấy thông tin từ các view khách hàng áp dụng cho Form ucKhachHangCT02.xaml
        /// </summary>
        /// <returns></returns>
        public DataSet getViewKhachHang02(int idKhachHang)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID_KHANG", "STRING", idKhachHang.ToString());

            request.dtThamSo = dt;
            request.objectName = "INQ.DS.VIEW_KHACH_HANG_02";
            request.inquiryName = "%";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public bool KhachHang02(DatabaseConstant.Action action, ref KH_KHANG_HSO objKhachHang, ref List<VKH_TTINH_GTRI> lstTTinhGTri, ref DataSet dsTTinh, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            KhachHangRequest request = null;
            KhachHangResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KhachHangService.layGiaTri());

                request = Common.Utilities.PrepareRequest(new KhachHangServiceRef.KhachHangRequest());
                response = new KhachHangServiceRef.KhachHangResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.KH_THANH_VIEN;
                request.Action = action;
                request.obj = objKhachHang;
                if(lstTTinhGTri != null && lstTTinhGTri.Count>0)
                    request.lstTTinhGTri = lstTTinhGTri.ToArray();
                request.dsTTinh = dsTTinh;

                // make a call to service client here
                response = Client.KhachHang02(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objKhachHang = response.obj;
                    lstTTinhGTri = response.lstTTinhGTri.ToList();
                    dsTTinh = response.dsTTinh;
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return true;
                }
                else
                {
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return false;
                }                
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                request = null;
                response = null;
            }
        }

        public bool DanhSachKhachHang02(DatabaseConstant.Action action, ref List<KH_KHANG_HSO> lstKhachHang, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            KhachHangRequest request = null;
            KhachHangResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KhachHangService.layGiaTri());

                request = Common.Utilities.PrepareRequest(new KhachHangServiceRef.KhachHangRequest());
                response = new KhachHangServiceRef.KhachHangResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.KH_DANH_SACH;
                request.Action = action;
                if (lstKhachHang.Count > 0)
                    request.lstKhachHang = lstKhachHang.ToArray();

                // make a call to service client here
                response = Client.KhachHang02(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    lstKhachHang = response.lstKhachHang.ToList();
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return true;
                }
                else
                {
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                request = null;
                response = null;
            }
        }


        public bool KhachHang04(DatabaseConstant.Action action, ref KH_KHANG_HSO objKhachHang, ref DataSet dsTTinh, ref List<BS_FileObject> lstDuLieuHinhAnh, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            KhachHangRequest request = null;
            KhachHangResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KhachHangService.layGiaTri());

                request = Common.Utilities.PrepareRequest(new KhachHangServiceRef.KhachHangRequest());
                response = new KhachHangServiceRef.KhachHangResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.KH_THANH_VIEN;
                request.Action = action;
                request.obj = objKhachHang;
                request.dsTTinh = dsTTinh;
                request.lstFileObject = lstDuLieuHinhAnh.ToArray();

                // make a call to service client here
                response = Client.KhachHang04(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objKhachHang = response.obj;
                    dsTTinh = response.dsTTinh;
                    lstDuLieuHinhAnh = response.lstFileObject != null ? response.lstFileObject.ToList() : null;
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return true;
                }
                else
                {
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                request = null;
                response = null;
            }
        }

        public bool KhachHang07(DatabaseConstant.Action action, ref KH_KHANG_HSO objKhachHang, ref DataSet dsTTinh, ref List<BS_FileObject> lstDuLieuHinhAnh, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            KhachHangRequest request = null;
            KhachHangResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KhachHangService.layGiaTri());

                request = Common.Utilities.PrepareRequest(new KhachHangServiceRef.KhachHangRequest());
                response = new KhachHangServiceRef.KhachHangResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.KH_THANH_VIEN;
                request.Action = action;
                request.obj = objKhachHang;
                request.dsTTinh = dsTTinh;
                request.lstFileObject = lstDuLieuHinhAnh.ToArray();

                // make a call to service client here
                response = Client.KhachHang07(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objKhachHang = response.obj;
                    dsTTinh = response.dsTTinh;
                    lstDuLieuHinhAnh = response.lstFileObject != null ? response.lstFileObject.ToList() : null;
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return true;
                }
                else
                {
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                request = null;
                response = null;
            }
        }

        public bool DanhSachKhachHang04(DatabaseConstant.Action action, ref List<KH_KHANG_HSO> lstKhachHang, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            KhachHangRequest request = null;
            KhachHangResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KhachHangService.layGiaTri());

                request = Common.Utilities.PrepareRequest(new KhachHangServiceRef.KhachHangRequest());
                response = new KhachHangServiceRef.KhachHangResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.KH_DANH_SACH;
                request.Action = action;
                if (lstKhachHang.Count > 0)
                    request.lstKhachHang = lstKhachHang.ToArray();

                // make a call to service client here
                response = Client.KhachHang04(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    lstKhachHang = response.lstKhachHang.ToList();
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return true;
                }
                else
                {
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                request = null;
                response = null;
            }
        }


        public bool DanhSachKhachHang07(DatabaseConstant.Action action, ref List<KH_KHANG_HSO> lstKhachHang, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            KhachHangRequest request = null;
            KhachHangResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KhachHangService.layGiaTri());

                request = Common.Utilities.PrepareRequest(new KhachHangServiceRef.KhachHangRequest());
                response = new KhachHangServiceRef.KhachHangResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.KH_DANH_SACH;
                request.Action = action;
                if (lstKhachHang.Count > 0)
                    request.lstKhachHang = lstKhachHang.ToArray();

                // make a call to service client here
                response = Client.KhachHang07(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    lstKhachHang = response.lstKhachHang.ToList();
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return true;
                }
                else
                {
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                request = null;
                response = null;
            }
        }

        /// <summary>
        /// Lấy thông tin từ các view khách hàng áp dụng cho Form ucKhachHangCT05.xaml
        /// </summary>
        /// <returns></returns>
        public DataSet getViewKhachHang05(int idKhachHang)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID_KHANG", "STRING", idKhachHang.ToString());

            request.dtThamSo = dt;
            request.objectName = "INQ.DS.VIEW_KHACH_HANG_04";
            request.inquiryName = "%";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public bool KhachHang05(DatabaseConstant.Action action, ref KH_KHANG_HSO objKhachHang, ref DataSet dsTTinh, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            KhachHangRequest request = null;
            KhachHangResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KhachHangService.layGiaTri());

                request = Common.Utilities.PrepareRequest(new KhachHangServiceRef.KhachHangRequest());
                response = new KhachHangServiceRef.KhachHangResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.KH_THANH_VIEN;
                request.Action = action;
                request.obj = objKhachHang;
                request.dsTTinh = dsTTinh;

                // make a call to service client here
                response = Client.KhachHang05(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objKhachHang = response.obj;
                    dsTTinh = response.dsTTinh;
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return true;
                }
                else
                {
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                request = null;
                response = null;
            }
        }

        public bool DanhSachKhachHang05(DatabaseConstant.Action action, ref List<KH_KHANG_HSO> lstKhachHang, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            KhachHangRequest request = null;
            KhachHangResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KhachHangService.layGiaTri());

                request = Common.Utilities.PrepareRequest(new KhachHangServiceRef.KhachHangRequest());
                response = new KhachHangServiceRef.KhachHangResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.KH_DANH_SACH;
                request.Action = action;
                if (lstKhachHang.Count > 0)
                    request.lstKhachHang = lstKhachHang.ToArray();

                // make a call to service client here
                response = Client.KhachHang05(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    lstKhachHang = response.lstKhachHang.ToList();
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return true;
                }
                else
                {
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                request = null;
                response = null;
            }
        }
        #endregion

        #region KH_CHUYEN_DBAN
        /// <summary>
        /// Thêm giao dịch chuyển địa bàn
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="lstGiaoDichCT"></param>
        /// <param name="idGiaoDich"></param>
        /// <returns></returns>
        public KH_CHUYEN_DBAN ThemGDChuyenDiaBan(KH_CHUYEN_DBAN obj, List<KH_CHUYEN_DBAN_CTIET> lstGiaoDichCT, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            KhachHangServiceRef.ChuyenDiaBanRequest request = Common.Utilities.PrepareRequest(new KhachHangServiceRef.ChuyenDiaBanRequest());
            KhachHangServiceRef.ChuyenDiaBanResponse response = new KhachHangServiceRef.ChuyenDiaBanResponse();
            obj.MA_GDICH = "1";
            request.obj = obj;
            request.lstDiaBanCT = lstGiaoDichCT.ToArray();
            request.Action1 = DatabaseConstant.Action.THEM;
            request.Function = DatabaseConstant.Function.KH_CHUYEN_DIA_BAN;

            // Lấy kết quả trả về
            response = Client.ChuyenDiaBan(request);


            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            if (response == null)
            {
                return null;
            }
            else
            {
                return response.objGiaoDich;
            }
        }

        /// <summary>
        /// Sửa giao dịch chuyển địa bàn
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="lstGiaoDichCT"></param>
        /// <returns></returns>
        public KH_CHUYEN_DBAN SuaGDChuyenDiaBan(KH_CHUYEN_DBAN obj, List<KH_CHUYEN_DBAN_CTIET> lstGiaoDichCT, DatabaseConstant.Action action, ref List<ClientResponseDetail> lstResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            KhachHangServiceRef.ChuyenDiaBanRequest request = Common.Utilities.PrepareRequest(new KhachHangServiceRef.ChuyenDiaBanRequest());
            KhachHangServiceRef.ChuyenDiaBanResponse response = new KhachHangServiceRef.ChuyenDiaBanResponse();
            obj.MA_GDICH = "1";
            request.obj = obj;
            request.lstDiaBanCT = lstGiaoDichCT.ToArray();
            request.Action1 = action;
            request.Function = DatabaseConstant.Function.KH_CHUYEN_DIA_BAN;

            // Lấy kết quả trả về
            response = Client.ChuyenDiaBan(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            if (response == null)
            {
                return null;
            }
            else
            {
                return response.objGiaoDich;
            }
        }

        /// <summary>
        /// Xóa giao dịch chuyển địa bàn
        /// </summary>
        /// <param name="lstid"></param>
        /// <returns></returns>
        public ApplicationConstant.ResponseStatus XoaGDChuyenDiaBan(int[] lstid, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            KhachHangServiceRef.ChuyenDiaBanRequest request = Common.Utilities.PrepareRequest(new KhachHangServiceRef.ChuyenDiaBanRequest());
            KhachHangServiceRef.ChuyenDiaBanResponse response = new KhachHangServiceRef.ChuyenDiaBanResponse();
            request.Action1 = DatabaseConstant.Action.XOA;
            request.Function = DatabaseConstant.Function.KH_CHUYEN_DIA_BAN;
            request.lstIDGiaoDich = lstid;

            // Lấy kết quả trả về
            response = Client.ChuyenDiaBan(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            if (response != null)
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            }

            return response.ResponseStatus;
        }

        /// <summary>
        /// Duyệt giao dịch chuyển địa bàn
        /// </summary>
        /// <param name="lstid"></param>
        /// <returns></returns>
        public ApplicationConstant.ResponseStatus DuyetGDChuyenDiaBan(int[] lstid, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            KhachHangServiceRef.ChuyenDiaBanRequest request = Common.Utilities.PrepareRequest(new KhachHangServiceRef.ChuyenDiaBanRequest());
            KhachHangServiceRef.ChuyenDiaBanResponse response = new KhachHangServiceRef.ChuyenDiaBanResponse();
            request.Action1 = DatabaseConstant.Action.DUYET;
            request.Function = DatabaseConstant.Function.KH_CHUYEN_DIA_BAN;
            request.lstIDGiaoDich = lstid;

            // Lấy kết quả trả về
            response = Client.ChuyenDiaBan(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            if (response != null)
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            }

            return response.ResponseStatus;
        }

        /// <summary>
        /// Từ chối duyệt giao dịch chuyển địa bàn
        /// </summary>
        /// <param name="lstid"></param>
        /// <returns></returns>
        public ApplicationConstant.ResponseStatus TuChoiGDChuyenDiaBan(int[] lstid, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            KhachHangServiceRef.ChuyenDiaBanRequest request = Common.Utilities.PrepareRequest(new KhachHangServiceRef.ChuyenDiaBanRequest());
            KhachHangServiceRef.ChuyenDiaBanResponse response = new KhachHangServiceRef.ChuyenDiaBanResponse();
            request.Action1 = DatabaseConstant.Action.TU_CHOI_DUYET;
            request.Function = DatabaseConstant.Function.KH_CHUYEN_DIA_BAN;
            request.lstIDGiaoDich = lstid;

            // Lấy kết quả trả về
            response = Client.ChuyenDiaBan(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            if (response != null)
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            }

            return response.ResponseStatus;
        }

        /// <summary>
        /// Thoai duyet giao dịch chuyển địa bàn
        /// </summary>
        /// <param name="lstid"></param>
        /// <returns></returns>
        public ApplicationConstant.ResponseStatus ThoaiDuyetGDChuyenDiaBan(int[] lstid, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            KhachHangServiceRef.ChuyenDiaBanRequest request = Common.Utilities.PrepareRequest(new KhachHangServiceRef.ChuyenDiaBanRequest());
            KhachHangServiceRef.ChuyenDiaBanResponse response = new KhachHangServiceRef.ChuyenDiaBanResponse();
            request.Action1 = DatabaseConstant.Action.THOAI_DUYET;
            request.Function = DatabaseConstant.Function.KH_CHUYEN_DIA_BAN;
            request.lstIDGiaoDich = lstid;

            // Lấy kết quả trả về
            response = Client.ChuyenDiaBan(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            if (response != null)
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            }

            return response.ResponseStatus;
        }
        #endregion

        #region Quan ly hinh anh
        public bool QuanLyHinhAnh(DatabaseConstant.Action action, ref KH_KHANG_HSO objKH, ref List<VKH_CKY_HANH> lstChuKyHinhAnh, ref List<BS_FileObject> lstHinhAnh, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            KhachHangRequest request = null;
            KhachHangResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KhachHangService.layGiaTri());

                request = Common.Utilities.PrepareRequest(new KhachHangServiceRef.KhachHangRequest());
                response = new KhachHangServiceRef.KhachHangResponse();

                //Khởi tạo request
                request.Action = action;
                request.obj = objKH;
                request.lstChuKyHinhAnh = lstChuKyHinhAnh.ToArray();
                request.lstFileObject = lstHinhAnh.ToArray();
                
                // make a call to service client here
                response = Client.QuanLyHinhAnh(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    lstHinhAnh = response.lstFileObject.ToList();
                    lstChuKyHinhAnh = response.lstChuKyHinhAnh.ToList();
                    return true;
                }
                else
                {
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                request = null;
                response = null;
            }
        }
        #endregion

        #region Thong tin khao sat
        public bool ThongTinKhaoSat(DatabaseConstant.Action action, ref KH_THONG_TIN_KHAO_SAT objThongTinKhaoSat, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            KhachHangRequest request = null;
            KhachHangResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KhachHangService.layGiaTri());

                request = Common.Utilities.PrepareRequest(new KhachHangServiceRef.KhachHangRequest());
                response = new KhachHangServiceRef.KhachHangResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.KH_THONG_TIN_KHAO_SAT_CT;
                request.Action = action;
                request.objThongTinKhaoSat = objThongTinKhaoSat;                          

                // make a call to service client here
                response = Client.ThongTinKhaoSat(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objThongTinKhaoSat = response.objThongTinKhaoSat;                    
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return true;
                }
                else
                {
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                request = null;
                response = null;
            }
        }

        public bool DanhSachThongTinKhaoSat(DatabaseConstant.Action action, ref List<KH_THONG_TIN_KHAO_SAT> lstThongTinKhaoSat, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            KhachHangRequest request = null;
            KhachHangResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KhachHangService.layGiaTri());

                request = Common.Utilities.PrepareRequest(new KhachHangServiceRef.KhachHangRequest());
                response = new KhachHangServiceRef.KhachHangResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.KH_THONG_TIN_KHAO_SAT_DS;
                request.Action = action;
                request.lstThongTinKhaoSat = lstThongTinKhaoSat.ToArray();

                // make a call to service client here
                response = Client.ThongTinKhaoSat(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    lstThongTinKhaoSat = response.lstThongTinKhaoSat.ToList();
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return true;
                }
                else
                {
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                request = null;
                response = null;
            }
        }

        /// <summary>
        /// Lấy thông tin từ các view khách hàng áp dụng cho Form ucKhachHangCT01.xaml
        /// </summary>
        /// <returns></returns>
        public DataSet getViewThongTinKhaoSat(int idKhachHang)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID_KHANG", "STRING", idKhachHang.ToString());

            request.dtThamSo = dt;
            request.objectName = "INQ.CT.VIEW_THONG_TIN_KHAO_SAT";
            request.inquiryName = "%";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Lấy thông tin từ các view khách hàng áp dụng cho Form ucKhachHangCT01.xaml
        /// </summary>
        /// <returns></returns>
        public DataSet DanhSachThongTinKhaoSat(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = "INQ.DS.THONG_TIN_KHAO_SAT";
            request.typePara = "UDTT";
            request.type = "Multi";
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        #endregion

        #region Thong tin khao sat 05
        public bool ThongTinKhaoSat05(DatabaseConstant.Action action, ref KH_THONG_TIN_KHAO_SAT objThongTinKhaoSat, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            KhachHangRequest request = null;
            KhachHangResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KhachHangService.layGiaTri());

                request = Common.Utilities.PrepareRequest(new KhachHangServiceRef.KhachHangRequest());
                response = new KhachHangServiceRef.KhachHangResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.KH_THONG_TIN_KHAO_SAT_CT;
                request.Action = action;
                request.objThongTinKhaoSat = objThongTinKhaoSat;

                // make a call to service client here
                response = Client.ThongTinKhaoSat05(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objThongTinKhaoSat = response.objThongTinKhaoSat;
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return true;
                }
                else
                {
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                request = null;
                response = null;
            }
        }

        public bool DanhSachThongTinKhaoSat05(DatabaseConstant.Action action, ref List<KH_THONG_TIN_KHAO_SAT> lstThongTinKhaoSat, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            KhachHangRequest request = null;
            KhachHangResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KhachHangService.layGiaTri());

                request = Common.Utilities.PrepareRequest(new KhachHangServiceRef.KhachHangRequest());
                response = new KhachHangServiceRef.KhachHangResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.KH_THONG_TIN_KHAO_SAT_DS;
                request.Action = action;
                request.lstThongTinKhaoSat = lstThongTinKhaoSat.ToArray();

                // make a call to service client here
                response = Client.ThongTinKhaoSat05(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    lstThongTinKhaoSat = response.lstThongTinKhaoSat.ToList();
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return true;
                }
                else
                {
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                request = null;
                response = null;
            }
        }

        /// <summary>
        /// Lấy thông tin từ các view khách hàng áp dụng cho Form ucKhachHangCT01.xaml
        /// </summary>
        /// <returns></returns>
        public DataSet getViewThongTinKhaoSat05(int idKhachHang)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID_KHANG", "STRING", idKhachHang.ToString());

            request.dtThamSo = dt;
            request.objectName = "INQ.CT.VIEW_THONG_TIN_KHAO_SAT";
            request.inquiryName = "%";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Lấy thông tin từ các view khách hàng áp dụng cho Form ucKhachHangCT01.xaml
        /// </summary>
        /// <returns></returns>
        public DataSet DanhSachThongTinKhaoSat05(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = "INQ.DS.THONG_TIN_KHAO_SAT";
            request.typePara = "UDTT";
            request.type = "Multi";
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        #endregion
    }
}
