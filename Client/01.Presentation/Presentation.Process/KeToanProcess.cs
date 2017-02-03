using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Data;
using Utilities.Common;
using Presentation.Process.KeToanServiceRef;
using Presentation.Process.TruyVanServiceRef;
using Presentation.Process.Common;
using System.ServiceModel.Description;

namespace Presentation.Process
{
    public class KeToanProcess
    {
        /// <summary>
        /// Biến cục bộ chứa dịch vụ tiện ích của server
        /// </summary>
        private static KeToanServiceClient Client { get; set; }
        private static TruyVanServiceClient ClientTruyVan { get; set; }

        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        static KeToanProcess()
        {
            //Client = new KeToanServiceClient();
            EndpointAddress endpointAddress = Common.Utilities.getEndpointAddress(ApplicationConstant.SystemService.KeToanService.layGiaTri());
            BasicHttpBinding basicHttpBinding = Common.Utilities.getBasicHttpBinding(ApplicationConstant.SystemService.KeToanService.layGiaTri());
            Client = new KeToanServiceClient(basicHttpBinding, endpointAddress);

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

        public DataTable getThongTinPhanLoaiTaiKhoan(int id)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID", "INT", id.ToString());

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

        public string getMaPhanLoaiGoiY(string maPhanLoaiCha)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MaPhanLoaiCha", "STRING", maPhanLoaiCha);

            request.dtThamSo = dt;
            request.inquiryName = "INQ.KT.LAY_MA_PLOAI_TIEP_THEO";
            request.objectName = "INQ.KT.LAY_MA_PLOAI_TIEP_THEO";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response.dsResult != null && response.dsResult.Tables.Count > 0)
            {
                if (!LString.IsNullOrEmptyOrSpace(response.dsResult.Tables[0].Rows[0][0].ToString()))
                {
                    return response.dsResult.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    return maPhanLoaiCha + "1";
                }
            }
            return maPhanLoaiCha + "1";
        }

        public DataTable getThongTinMaPhanLoaiTheoID(int id)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID", "INT", id.ToString());

            request.dtThamSo = dt;
            request.inquiryName = "INQ.KT.LAY_MA_PLOAI_THEO_ID";
            request.objectName = "INQ.KT.LAY_MA_PLOAI_THEO_ID";

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

        public DataTable getThongTinMaPhanLoaiTheoMa(string ma_ploai)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MaPhanLoai", "STRING", ma_ploai);

            request.dtThamSo = dt;
            request.inquiryName = "INQ.KT.LAY_MA_PLOAI_THEO_MA";
            request.objectName = "INQ.KT.LAY_MA_PLOAI_THEO_MA";

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

        public DataSet getDanhSachMaPhanLoai(string maDonVi, string tthai_nvu, string tthai_sdu, string maPhanLoai = "%")
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@TrangThaiNVU", "STRING", tthai_nvu);
            LDatatable.AddParameter(ref dt, "@TrangThaiSDU", "STRING", tthai_sdu);
            LDatatable.AddParameter(ref dt, "@MaDonVi", "STRING", maDonVi);
            LDatatable.AddParameter(ref dt, "@MaPhanLoai", "STRING", maPhanLoai);

            request.dtThamSo = dt;
            request.inquiryName = "INQ.DS.KT_MA_PLOAI";
            request.objectName = "INQ.DS.KT_MA_PLOAI";

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

        public DataTable getDanhSachBToanTheoLoaiGD(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            
            request.dtThamSo = dt;
            request.inquiryName = "INQ.CT.BTOAN";
            request.objectName = "INQ.CT.BTOAN";

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

        public DataTable getBToanChuyenQuyTheoLoaiGD(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            

            request.dtThamSo = dt;
            request.inquiryName = "INQ.CT.CHUYEN_QUY";
            request.objectName = "INQ.CT.CHUYEN_QUY";

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

        #region  PhanLoaiTKCT

        public ApplicationConstant.ResponseStatus PhanLoaiChiTiet(DatabaseConstant.Action action, ref KT_PLOAI objKtPloai, ref KT_KY_HIEU_PLOAI objKtKyHieuPhanLoai, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KeToanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TaiKhoanRequest request = Common.Utilities.PrepareRequest(new TaiKhoanRequest());
            request.objKtPloai = objKtPloai;
            request.objKtKyHieuPhanLoai = objKtKyHieuPhanLoai;

            request.Module = DatabaseConstant.Module.GDKT;
            request.Function = DatabaseConstant.Function.KT_PHAN_LOAI_CT;
            request.Action = action;

            TaiKhoanResponse response = Client.TaiKhoan(request);


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
                objKtPloai = response.objKtPloai;
                objKtKyHieuPhanLoai = response.objKtKyHieuPhanLoai;
                return response.ResponseStatus;
            }
        }

        #endregion

        #region PhanLoaiTaiKhoanDS

        public bool XuLyPhanLoaiTaiKhoanDS(KT_PLOAI[] lstKtPloai, ref List<ClientResponseDetail> listClientResponseDetail, DatabaseConstant.Action action)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KeToanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TaiKhoanRequest request = Common.Utilities.PrepareRequest(new TaiKhoanRequest());
            request.lstKtPloai = lstKtPloai;

            request.Module = DatabaseConstant.Module.GDKT;
            request.Function = DatabaseConstant.Function.KT_PHAN_LOAI_DS;
            request.Action = action;

            TaiKhoanResponse response = Client.TaiKhoan(request);


            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return true;
            }
        }

        #endregion

        #region  HeThongTaiKhoanTHCT

        public ApplicationConstant.ResponseStatus HeThongTaiKhoanChiTiet(DatabaseConstant.Action action, ref HE_THONG_TKTH objHTTKTH, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KeToanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TaiKhoanRequest request = Common.Utilities.PrepareRequest(new TaiKhoanRequest());
            request.objHeThongTKTH = objHTTKTH;

            request.Module = DatabaseConstant.Module.GDKT;
            request.Function = DatabaseConstant.Function.KT_HE_THONG_TKTH;
            request.Action = action;

            TaiKhoanResponse response = Client.TaiKhoan(request);


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
                objHTTKTH = response.objHeThongTKTH;
                return response.ResponseStatus;
            }
        }

        #endregion

        #region  TaikhoanTongHop

        public ApplicationConstant.ResponseStatus TaiKhoanTongHop(DatabaseConstant.Action action, ref TKTONGHOP objHTTKTH, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KeToanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TaiKhoanRequest request = Common.Utilities.PrepareRequest(new TaiKhoanRequest());
            request.objTKTH = objHTTKTH;

            request.Module = DatabaseConstant.Module.GDKT;
            request.Function = DatabaseConstant.Function.KT_TAI_KHOAN_TH;
            request.Action = action;

            TaiKhoanResponse response = Client.TaiKhoan(request);


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
                objHTTKTH = response.objTKTH;
                return response.ResponseStatus;
            }
        }

        #endregion

        #region HeThongTaiKhoanTHDS

        public DataSet HeThongTaiKhoanDanhSach(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            request.dtThamSo = dt;
            request.inquiryName = "DANH_SACH";
            request.objectName = "INQ_DS_KT_HT_TKHOAN_TH";
            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        #endregion

        #region KiemSoatGiaoDich

        public DataSet getTreePhanHeGD(string userName, string maDonViQLy)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@UserName", "STRING", userName);
            LDatatable.AddParameter(ref dt, "@MaDonVi", "STRING", maDonViQLy);

            request.dtThamSo = dt;
            request.inquiryName = "INQ.TREE.PHAN_HE_GD";
            request.objectName = "INQ.TREE.PHAN_HE_GD";

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

        public DataSet getDanhSachGiaoDich(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            
            request.dtThamSo = dt;
            request.inquiryName = "%";
            if (ClientInformation.Company.Equals("BINHKHANH")) //@CongLC sua
                request.objectName = "INQ.DS.GIAO_DICH.DANH_SACH_00";
            else
                request.objectName = "INQ.DS.GIAO_DICH.DANH_SACH";

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

        public DataSet getDanhSachGiaoDichCT(string maDonVi, string idGiaoDich)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MaDonVi", "STRING", maDonVi);
            LDatatable.AddParameter(ref dt, "@IdGiaoDich", "STRING", idGiaoDich);

            request.dtThamSo = dt;
            request.inquiryName = "INQ.DS.GIAO_DICH_CT.DANH_SACH";
            request.objectName = "INQ.DS.GIAO_DICH_CT.DANH_SACH";

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

        public ApplicationConstant.ResponseStatus KiemSoatGiaoDich(List<GDICH_KSOAT> lstGiaoDich, DatabaseConstant.Action action, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KeToanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            GiaoDichKeToanRequest request = Common.Utilities.PrepareRequest(new GiaoDichKeToanRequest());
            request.objKiemSoatGiaoDich = new Presentation.Process.KeToanServiceRef.KIEM_SOAT_GIAO_DICH();
            request.objKiemSoatGiaoDich.DSACH_GDICH_KSOAT = lstGiaoDich.ToArray();

            request.Module = DatabaseConstant.Module.GDKT;
            request.Function = DatabaseConstant.Function.KT_GIAO_DICH;
            request.Action = action;

            GiaoDichKeToanResponse response = Client.GiaoDichKeToan(request);


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
                return response.ResponseStatus;
            }
        }

        public ApplicationConstant.ResponseStatus ThucHienCuoiNgay(BusinessConstant.NGHIEP_VU_CUOI_NGAY maNghiepVu, DatabaseConstant.Action action, ref List<ClientResponseDetail> listClientResponseDetail, string MaDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KeToanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            GiaoDichKeToanRequest request = Common.Utilities.PrepareRequest(new GiaoDichKeToanRequest());
            request.objKiemSoatGiaoDich = new Presentation.Process.KeToanServiceRef.KIEM_SOAT_GIAO_DICH();

            request.Module = DatabaseConstant.Module.GDKT;
            request.Function = DatabaseConstant.Function.KT_CUOI_NGAY;
            request.Action = action;
            request.objCuoiNgay = new CUOI_NGAY();
            request.objCuoiNgay.MA_DON_VI = MaDonVi;
            request.objCuoiNgay.NGHIEP_VU = maNghiepVu;

            GiaoDichKeToanResponse response = Client.GiaoDichKeToan(request);


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
                return response.ResponseStatus;
            }
        }

        #endregion

        #region Giao dịch - Phát sinh
        public DataSet getGiaoDich(string maDonVi, string maGiaoDich)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_GDICH", "STRING", maGiaoDich);
            LDatatable.AddParameter(ref dt, "@INP_MA_DON_VI", "STRING", maDonVi);            

            request.dtThamSo = dt;
            request.objectName = "sp_INQ.CT.GIAO_DICH";
            request.inquiryName = "GIAO_DICH";            

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

        public DataSet getPhatSinh(string maDonVi, string maGiaoDich)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_GDICH", "STRING", maGiaoDich);
            LDatatable.AddParameter(ref dt, "@INP_MA_DON_VI", "STRING", maDonVi);

            request.dtThamSo = dt;
            request.objectName = "sp_INQ.CT.GIAO_DICH";
            request.inquiryName = "PHAT_SINH";

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

        public DataSet getPhatSinhChiTiet(string maDonVi, string maGiaoDich)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_GDICH", "STRING", maGiaoDich);
            LDatatable.AddParameter(ref dt, "@INP_MA_DON_VI", "STRING", maDonVi);

            request.dtThamSo = dt;
            request.objectName = "sp_INQ.CT.GIAO_DICH";
            request.inquiryName = "PHAT_SINH_CT";

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
        #endregion

        #region TaiKhoan

        public DataSet getDanhSachCauTrucTaiKhoan(string maCtruc)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MaCtruc", "STRING", maCtruc);

            request.dtThamSo = dt;
            request.inquiryName = "INQ.DS.TPHAN_CTRUC_TK";
            request.objectName = "INQ.DS.TPHAN_CTRUC_TK";

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

        public DataSet getDanhSachTaiKhoanChiTiet(DataTable dtThamSo, string inquiryName)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            request.dtThamSo = dtThamSo;
            request.inquiryName = inquiryName;
            request.objectName = "INQ.DS.TKHOAN_CTIET";

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

        public DataSet GetTaiKhoanByMaPLoai(string maPhanLoai, string maDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_PHAN_LOAI", "STRING", maPhanLoai);
            LDatatable.AddParameter(ref dt, "@MA_DVI", "STRING", maDonVi);

            request.dtThamSo = dt;
            request.objectName = "INQ.CT.KT_TKHOAN";
            request.inquiryName = "GET_BY_MA_PLOAI";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public ApplicationConstant.ResponseStatus TaiKhoanChiTiet(DatabaseConstant.Action action,ref KT_TKHOAN objTkhoan, ref List<ClientResponseDetail> listClientResponseDetail, List<DC_CTRUC_CTIET> lstCtruc = null)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KeToanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TaiKhoanRequest request = Common.Utilities.PrepareRequest(new TaiKhoanRequest());
            request.objKtTkhoan = objTkhoan;
            if (lstCtruc != null)
            {
                request.lstDcCtrucCtiet = lstCtruc.ToArray();
            }

            request.Module = DatabaseConstant.Module.GDKT;
            request.Function = DatabaseConstant.Function.KT_TAI_KHOAN_CT;
            request.Action = action;

            TaiKhoanResponse response = Client.TaiKhoan(request);


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
                objTkhoan = response.objKtTkhoan;
                return response.ResponseStatus;
            }
        }

        public ApplicationConstant.ResponseStatus TaiKhoanChiTietDS(DatabaseConstant.Action action, List<KT_TKHOAN> listTkhoan, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KeToanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TaiKhoanRequest request = Common.Utilities.PrepareRequest(new TaiKhoanRequest());
            request.lstKtTkhoan = listTkhoan.ToArray();

            request.Module = DatabaseConstant.Module.GDKT;
            request.Function = DatabaseConstant.Function.KT_TAI_KHOAN_DS;
            request.Action = action;

            TaiKhoanResponse response = Client.TaiKhoan(request);


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
                return response.ResponseStatus;
            }
        }

        #endregion

        #region PhieuKeToan

        public DataSet getThongTinMaPhieuKeToanTheoMa(string maGD, string maDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MaGD", "STRING", maGD);
            LDatatable.AddParameter(ref dt, "@MaDVi", "STRING", maDonVi);

            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = "INQ.CT.PHIEU_KTOAN";

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

        public ApplicationConstant.ResponseStatus PhieuKeToan(DatabaseConstant.Function function, DatabaseConstant.Action action, ref PHIEU_KE_TOAN obj, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KeToanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            GiaoDichKeToanRequest request = Common.Utilities.PrepareRequest(new GiaoDichKeToanRequest());
            request.objPhieuKeToan = obj;

            request.Module = DatabaseConstant.Module.GDKT;
            request.Function = function;
            request.Action = action;

            GiaoDichKeToanResponse response = Client.GiaoDichKeToan(request);


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
                obj = response.objPhieuKeToan;
                return response.ResponseStatus;
            }
        }

        #endregion

        #region PhieuNgoaiBang

        public ApplicationConstant.ResponseStatus PhieuNgoaiBang(DatabaseConstant.Function function, DatabaseConstant.Action action, ref NHAP_XUAT_NGOAI_BANG obj, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KeToanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            GiaoDichKeToanRequest request = Common.Utilities.PrepareRequest(new GiaoDichKeToanRequest());
            request.objNhapXuatNgoaiBang = obj;

            request.Module = DatabaseConstant.Module.GDKT;
            request.Function = function;
            request.Action = action;            

            GiaoDichKeToanResponse response = Client.GiaoDichKeToan(request);


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
                obj = response.objNhapXuatNgoaiBang;
                return response.ResponseStatus;
            }
        }

        #endregion

        #region DieuChinhGiaoDich

        public ApplicationConstant.ResponseStatus DieuChinh(DatabaseConstant.Function function, DatabaseConstant.Action action, ref DIEU_CHINH_BUT_TOAN obj, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KeToanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            GiaoDichKeToanRequest request = Common.Utilities.PrepareRequest(new GiaoDichKeToanRequest());
            request.objDieuChinhButToan = obj;

            request.Module = DatabaseConstant.Module.GDKT;
            request.Function = function;
            request.Action = action;

            GiaoDichKeToanResponse response = Client.GiaoDichKeToan(request);


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
                obj = response.objDieuChinhButToan;
                return response.ResponseStatus;
            }
        }

        #endregion

        /// <summary>
        /// Lay doi tuong giao dich
        /// Tra ve 0 neu khong thanh cong
        /// Tra ve 1 neu thanh cong
        /// </summary>
        /// <param name="objDTuong"></param>
        /// <returns></returns>
        public int LayDoiTuongGiaoDichTheoMa(ref KT_GIAO_DICH_DTUONG objDTuong)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KeToanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            GiaoDichKeToanRequest request = Common.Utilities.PrepareRequest(new GiaoDichKeToanRequest());            

            request.Module = DatabaseConstant.Module.GDKT;
            request.Function = DatabaseConstant.Function.KT_GIAO_DICH_DOI_TUONG;
            request.objDTuong = objDTuong;                     
            GiaoDichKeToanResponse response = Client.GiaoDichKeToan(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return 0;
            }
            else
            {
                objDTuong = response.objDTuong;
                return 1;                
            }
        }

        /// <summary>
        /// Lay doi tuong giao dich theo ma goi nho
        /// Tra ve 1 neu thanh cong
        /// Tra ve 0 neu khong thanh cong
        /// </summary>
        /// <param name="objDTuong"></param>
        /// <returns></returns>
        public int LayDTuongGDichTheoMaGoiNho(ref KT_GIAO_DICH_DTUONG objDTuong)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KeToanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            GiaoDichKeToanRequest request = Common.Utilities.PrepareRequest(new GiaoDichKeToanRequest());

            request.Module = DatabaseConstant.Module.GDKT;
            request.Function = DatabaseConstant.Function.KT_GIAO_DICH_DOI_TUONG;
            request.objDTuong = objDTuong;
            GiaoDichKeToanResponse response = Client.GiaoDichKeToan(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return 0;
            }
            else
            {
                objDTuong = response.objDTuong;
                return 1;
            }
        }

        #region PhieuKetChuyen

        public DataSet getThongTinGDKetChuyenTheoMa(string maGD, string maDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MaGD", "STRING", maGD);
            //LDatatable.AddParameter(ref dt, "@MaDVi", "STRING", maDonVi);

            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = "INQ.CT.GDICH_KCHUYEN";

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

        public ApplicationConstant.ResponseStatus GiaoDichKetChuyen(DatabaseConstant.Function function, DatabaseConstant.Action action, ref KET_CHUYEN obj, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KeToanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            GiaoDichKeToanRequest request = Common.Utilities.PrepareRequest(new GiaoDichKeToanRequest());
            request.objKetChuyen = obj;

            request.Module = DatabaseConstant.Module.GDKT;
            request.Function = function;
            request.Action = action;

            GiaoDichKeToanResponse response = Client.GiaoDichKeToan(request);


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
                obj = response.objKetChuyen;
                return response.ResponseStatus;
            }
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

        #region Chứng từ ghi sổ

        public DataSet getThongTinChungTuGhiSoKBao(string maDonVi, string maDonViQLy)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MaDViTao", "STRING", maDonVi);
            LDatatable.AddParameter(ref dt, "@MaDViQLy", "STRING", maDonViQLy);

            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = "INQ.CT.CHUNG_TU_KBAO";

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

        public DataSet GetDSGhiSoKBao(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            
            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = "INQ.DS.CHUNG_TU_KBAO";

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

        public ApplicationConstant.ResponseStatus ChungTuGhiSoKBao(DatabaseConstant.Function function, DatabaseConstant.Action action, ref GHI_SO obj, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KeToanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TaiKhoanRequest request = Common.Utilities.PrepareRequest(new TaiKhoanRequest());
            request.objGHI_SO = obj;

            request.Module = DatabaseConstant.Module.GDKT;
            request.Function = function;
            request.Action = action;

            TaiKhoanResponse response = Client.TaiKhoan(request);


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
                return response.ResponseStatus;
            }
        }

        #endregion

        public void LaySoDuTheoKyHieuPLoai(ref decimal decTongSoDu,KT_KY_HIEU_PLOAI objKHPL,KT_TKHOAN_SDU objTKhoanSDU)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KeToanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TaiKhoanRequest request = Common.Utilities.PrepareRequest(new TaiKhoanRequest());
            request.decTongSoDu = decTongSoDu;
            request.objKtKyHieuPhanLoai = objKHPL;
            request.objKtTkhoanSdu = objTKhoanSDU;
            request.Function = DatabaseConstant.Function.KT_LAY_TONG_SO_DU;
            TaiKhoanResponse response = Client.TaiKhoan(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                decTongSoDu = response.decTongSoDu;
            }
            else
            {
                decTongSoDu = 0;
            }
        }

        #region DoiTuongSoDuCT

        public DataSet getDanhSachDoiTuongSoDu(string maGD, string maDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MaGD", "STRING", maGD);
            //LDatatable.AddParameter(ref dt, "@MaDVi", "STRING", maDonVi);

            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = "INQ.CT.GDICH_KCHUYEN";

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

        public ApplicationConstant.ResponseStatus DoiTuongSoDuCT(DatabaseConstant.Function function, DatabaseConstant.Action action, ref DOI_TUONG_SDU_TKHOAN obj, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KeToanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TaiKhoanRequest request = Common.Utilities.PrepareRequest(new TaiKhoanRequest());
            request.objDoiTuongSDUTKhoan = obj;

            request.Module = DatabaseConstant.Module.GDKT;
            request.Function = function;
            request.Action = action;

            TaiKhoanResponse response = Client.TaiKhoan(request);


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
                obj = response.objDoiTuongSDUTKhoan;
                return response.ResponseStatus;
            }
        }

        #endregion

        #region TongHop

        public DataSet getDanhSachTongHop(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = "INQ.DS.TONG_HOP_DULIEU";

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

        public ApplicationConstant.ResponseStatus TongHopDuLieu(DatabaseConstant.Function function, DatabaseConstant.Action action, ref TONG_HOP_DU_LIEU obj, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KeToanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TaiKhoanRequest request = Common.Utilities.PrepareRequest(new TaiKhoanRequest());
            request.objTongHopDuLieu = obj;

            request.Module = DatabaseConstant.Module.GDKT;
            request.Function = function;
            request.Action = action;

            TaiKhoanResponse response = Client.TaiKhoan(request);


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
                return response.ResponseStatus;
            }
        }

        #endregion

        #region Đánh giá ngoại tệ

        public ApplicationConstant.ResponseStatus DanhGiaNgoaiTe(DatabaseConstant.Function function, DatabaseConstant.Action action, ref DANH_GIA_NGOAI_TE obj, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            GiaoDichKeToanRequest request = null;
            GiaoDichKeToanResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KeToanService.layGiaTri());

                
                request = Common.Utilities.PrepareRequest(new KeToanServiceRef.GiaoDichKeToanRequest());
                response = new KeToanServiceRef.GiaoDichKeToanResponse();

                //Khởi tạo request
                request.Function = function;
                request.Action = action;
                request.objDanhGiaNgoaiTe = obj;

                // make a call to service client here
                response = Client.GiaoDichKeToan(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response == null)
                {
                    return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                }
                else
                {
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    obj = response.objDanhGiaNgoaiTe;
                    return response.ResponseStatus;
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
    }
}
